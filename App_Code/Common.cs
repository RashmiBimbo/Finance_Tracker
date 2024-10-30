using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Web.UI;
using static System.DateTime;

namespace Finance_Tracker
{
    public static class Common
    {
        public static readonly string Emp = string.Empty;
        public static string LogFile = string.Empty;
        public static SmtpSection Settings;
        public static SmtpNetworkElement Network;
        private static List<string> NameSpaces = new List<string> { "Finance_Tracker" };
        public static readonly string Entr = Environment.NewLine;
        public static readonly StringComparison StrComp = StringComparison.InvariantCultureIgnoreCase;

        static Common()
        {
            try
            {
                string projectFolder = AppDomain.CurrentDomain.BaseDirectory;
                LogFile = projectFolder + "Finance_Tracker_Log.txt";
                if (IsEmpty(projectFolder)) throw new Exception("Project folder path was not found!");
            }
            catch (Exception)
            { }
        }

        public static void PopUp(Page page, string msg)
        {
            try
            {
                msg.Replace("'", Emp);
                //ScriptManager.RegisterStartupScript(page, page.GetType(), "showalert", "alert('" + msg + "');", true);
                ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "showalert", "alert('" + msg + "');", true);
            }
            catch (Exception)
            {}
        }

        /// <summary>
        /// checks if string is null, empty or just white space
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmpty(string input)
        {
            try
            {
                return string.IsNullOrWhiteSpace(input);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Verify the SMTP Configuations
        /// </summary>
        /// <returns></returns>
        public static bool IsSmtpConfigValid()
        {
            Settings = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            Network = Settings?.Network;
            bool @return =
                (Settings != null
                && Network != null
                && !string.IsNullOrWhiteSpace(Settings.From)
                && !string.IsNullOrWhiteSpace(Network.Host)
                && !string.IsNullOrWhiteSpace(Network.UserName)
                && !string.IsNullOrWhiteSpace(Network.Password));
            if (!@return) return false;
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(Network.Host, 1000); // Timeout set to 1 second
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return false;
            }
        }

        /// <summary>
        /// Get only project relevant stacktrace from exception 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="nameSpaces"></param>
        /// <returns></returns>
        public static string GetRelErrorMsg(Exception ex, List<string> nameSpaces = null)
        {
            try
            {
                if (!(nameSpaces?.Count > 0))
                    nameSpaces = NameSpaces;

                string relevantStackTrace = GetRelevantStackTrace(ex, nameSpaces);
                relevantStackTrace = ex?.Message + (IsEmpty(relevantStackTrace) ? Emp : $"{Entr}StackTrace: {relevantStackTrace}");

                if (IsEmpty(relevantStackTrace)) return Emp;

                if (ex.InnerException != null)
                {
                    string innerStackTrace = GetRelevantStackTrace(ex.InnerException, nameSpaces);
                    relevantStackTrace += $"{Entr}Inner Exception Details:" + ex.InnerException.Message;
                    relevantStackTrace += $"{Entr}StackTrace:" + (IsEmpty(innerStackTrace) ? Emp : $"{Entr}StackTrace: {innerStackTrace}");
                }
                return relevantStackTrace;
            }
            catch (Exception)
            {
                return Emp;
            }
        }

        private static string GetRelevantStackTrace(Exception ex, List<string> _projectNamespaces)
        {
            if (ex is null) return Emp;
            try
            {
                var stackTraceLines = ex.StackTrace?.Split(new string[] { Entr }, StringSplitOptions.None);
                if (stackTraceLines == null) return Emp;

                string[] relevantLines = stackTraceLines?
                    .Where(line => _projectNamespaces.Any(ns => line.Contains(ns)))?
                    .ToArray();
                if (!(relevantLines?.Length > 0)) return Emp;
                return string.Join(Entr, relevantLines).Trim();
            }
            catch (Exception)
            {
                return Emp;
            }
        }

        /// <summary>
        /// write message in log file 
        /// </summary>
        /// <param name="msg"></param>
        public static void LogMsg(string msg)
        {
            try
            {
                if (!IsEmpty(msg))
                    File.AppendAllText(LogFile, $"{Entr}{msg}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Entr}{Now}: {GetRelErrorMsg(ex, NameSpaces)}");
            }
        }

        /// <summary>
        /// Write exception details in log file
        /// </summary>
        /// <param name="ex"></param>
        public static void LogError(Exception ex)
        {
            try
            {
                string msg = GetRelErrorMsg(ex, NameSpaces);
                LogMsg($"{Entr}{Now}: {msg}");
            }
            catch (Exception methodEx)
            {
                Console.WriteLine($"{Entr}{Now}: {GetRelErrorMsg(methodEx, NameSpaces)}");
            }
        }

    }
}