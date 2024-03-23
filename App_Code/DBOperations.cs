using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
//using Finance_Tracker.App_Code;

namespace Finance_Tracker
{
    public class DBOperations
    {
        public readonly string strSQLCon;
        ConnectionStringSettingsCollection ConnStrs;
        public readonly string ConStrSecondary, ConStrPrimary;
        public OleDbConnection ConnSecondary, ConnPrimary;
        public const string Admin = "ADMIN", Corporate = "CORPORATE", Plant = "PLANT", SuperAdmin = "SUPERADMIN";

        public static readonly Dictionary<string, string> LoginTypes = new Dictionary<string, string>
        {
            { "1", Admin },
            { "2", Corporate },
            { "3", Plant },
            { "4", SuperAdmin }
        };


        public DBOperations()
        {
            ConnStrs = ConfigurationManager.ConnectionStrings;
            ConStrPrimary = ConnStrs["AppDBConnStrPrimary"].ConnectionString;
            ConStrSecondary = ConnStrs["AppDBConnStrSecondary"].ConnectionString;
            AuthenticatConns();
        }

        public OleDbConnection InitializeConnection(string constr)
        {
            OleDbConnection conn = null;
            try
            {
                conn = new OleDbConnection(constr);
            }
            catch (Exception ex)
            {
                throw;
            }
            return conn;
        }

        public bool AuthenticatConns()
        {
            bool AuthenticatCon = true;
            try
            {
                ConnSecondary = new OleDbConnection(ConStrSecondary);
            }
            catch (Exception ex)
            {
                AuthenticatCon = false;
            }
            try
            {
                ConnPrimary = new OleDbConnection(ConStrPrimary);
            }
            catch (Exception ex)
            {
                AuthenticatCon = false;
            }
            return AuthenticatCon;
        }

        public void PopUp(Page page, string msg)
        {
            //ScriptManager.RegisterStartupScript(page, page.GetType(), "showalert", "alert('" + msg + "');", true);
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        public bool InsertUpdateValues(string query, OleDbConnection conn)
        {
            conn = conn ?? ConnPrimary;
            return ExecNonQuery(query, conn) > 0;
        }

        private int ExecNonQuery(string query, OleDbConnection conn)
        {
            OleDbCommand cmd = null;
            try
            {
                conn.Open();
                cmd = new OleDbCommand(query, conn);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
                if (cmd != null) cmd.Dispose();
            }
        }

        public object ExecScalarProc(string proc, OleDbConnection conn, OleDbParameter[] paramCln = null)
        {
            OleDbCommand cmd = null;
            try
            {
                conn = conn ?? ConnPrimary;
                conn.Open();
                cmd = new OleDbCommand(proc, conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60
                };

                if (paramCln != null) cmd.Parameters.AddRange(paramCln);

                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
                if (cmd != null) cmd.Dispose();
            }
        }

        public object ExecScalar(string query, OleDbConnection conn)
        {
            OleDbCommand cmd = null;
            try
            {
                conn.Open();
                cmd = new OleDbCommand(query, conn);
                object result = cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
                if (cmd != null) cmd.Dispose();
            }
        }

        /// <summary>
        /// get data through procedure 
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public DataTable GetDataProc(string proc, OleDbConnection conn, OleDbParameter[] paramCln = null)
        {
            DataTable dt;
            OleDbCommand cmd = null;
            try
            {
                conn = conn ?? ConnPrimary;
                if (conn.State != ConnectionState.Open) conn.Open();
                cmd = new OleDbCommand()
                {
                    Connection = conn,
                    CommandText = proc,
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60
                };
                if (paramCln != null) cmd.Parameters.AddRange(paramCln);
                dt = GetDataCmd(cmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn?.Close();
                cmd?.Dispose();
            }
        }

        public DataTable SelQuery(string strQry, OleDbConnection conn)
        {
            DataTable results = GetData(strQry, conn ?? ConnPrimary);
            return results;
        }

        /// <summary>
        /// Get data through query other than procedure with or without parameter
        /// </summary>
        /// <param name="query"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private DataTable GetData(string query, OleDbConnection conn)
        {
            DataTable dt;
            OleDbDataAdapter adapter = null;
            try
            {
                dt = new DataTable();
                conn.Open();
                adapter = new OleDbDataAdapter(query, conn);
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
                if (adapter != null)
                    adapter.Dispose();
            }
        }

        private DataTable GetDataCmd(OleDbCommand cmd)
        {
            DataTable dt;
            OleDbDataAdapter adapter = null;
            try
            {
                dt = new DataTable();
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                adapter?.Dispose();
            }
        }

        public string GenrateNewID(string strTable, OleDbConnection conn, string strColmn = "pRowID")
        {
            string GenrateNewID;
            try
            {
                string StrQry = " Select 'VC'+Right('00000000'+convert(varchar,(Convert(int,Right(Isnull(Max(" + strColmn + "),0),8)) +1)),8) as pRowID from " + strTable + "";
                DataTable dtTemp = SelQuery(StrQry, conn ?? ConnPrimary);
                GenrateNewID = dtTemp.Rows[0]["pRowID"].ToString();
            }
            catch (Exception ex)
            {
                GenrateNewID = "VC00000001";
            }
            return GenrateNewID;
        }

        // #Region " Genrate New Invoice ID"
        // Public Function GenrateNewID(ByVal strTable As String, ByVal strDepot As String, ByVal monyr As String, ByVal tsaletype As String, Optional ByVal strColmn As String = "pRowID", Optional ByVal ConName As ConName = ConName.ConnectionString1) As String
        // Try
        // StrQry = " Select left('" & strDepot & "',2)+'/" & tsaletype & "/'+'" & monyr & "/'+Right('00000'+convert(varchar,(Convert(int,Right(Isnull(Max(" & strColmn & "),0),5)) +1)),5) as pRowID from " & strTable & " where Loading_Location='" & strDepot & "' and dbo.FN_GetMMYr(Route_managemnet_Date)='" & monyr & "' "
        // dtTemp = SelQuery(StrQry, ConName)
        // GenrateNewID = dtTemp.Rows(0).Item("pRowID")
        // Catch ex As Exception
        // GenrateNewID = "VC00000001"
        // End Try
        // Return GenrateNewID
        // End Function
        // #End Region

        public int CountSundays(DateTime strdate)
        {
            DateTime today = strdate; // Date.Today
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            // get only last day of month
            int day = endOfMonth.Day;

            DateTime now = strdate; // Date.Now
            int count;
            count = 0;
            for (int i = 0; i <= day - 1; i++)
            {
                DateTime d = new DateTime(now.Year, now.Month, i + 1);
                // Compare date with sunday
                if (d.DayOfWeek == DayOfWeek.Sunday)
                    count = count + 1;
            }
            return count;
        }

        public string GetDatesByMonth(string strDay, string strMonth, string strYear)
        {
            string dates = "";
            DateTime today = new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), 1);
            //"01-" & strMonth & "-" & strYear; // Date.Today
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            // get only last day of month
            int day = endOfMonth.Day;

            DateTime now = today; // Date.Now
            switch (strDay.Trim())
            {
                case "Sunday":
                    {
                        for (int i = 0; i <= day - 1; i++)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            if (d.DayOfWeek == DayOfWeek.Sunday)
                            {
                                if (dates.Trim() == "")
                                    dates = d.Date.ToString();
                                else
                                    dates = dates + "~" + System.Convert.ToString(d.Date);
                            }
                        }

                        break;
                    }

                case "Monday":
                    {
                        for (int i = 0; i <= day - 1; i++)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            if (d.DayOfWeek == DayOfWeek.Monday)
                            {
                                if (dates.Trim() == "")
                                    dates = d.Date.ToString();
                                else
                                    dates = dates + "~" + System.Convert.ToString(d.Date);
                            }
                        }
                        break;
                    }

                case "Tuesday":
                    {
                        for (int i = 0; i <= day - 1; i++)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            if (d.DayOfWeek == DayOfWeek.Tuesday)
                            {
                                if (dates.Trim() == "")
                                    dates = d.Date.ToString();
                                else
                                    dates = dates + "~" + System.Convert.ToString(d.Date);
                            }
                        }
                        break;
                    }

                case "Wednesday":
                    {
                        for (int i = 0; i <= day - 1; i++)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            if (d.DayOfWeek == DayOfWeek.Wednesday)
                            {
                                if (dates.Trim() == "")
                                    dates = d.Date.ToString();
                                else
                                    dates = dates + "~" + System.Convert.ToString(d.Date);
                            }
                        }
                        break;
                    }

                case "Thursday":
                    {
                        for (int i = 0; i <= day - 1; i++)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            if (d.DayOfWeek == DayOfWeek.Thursday)
                            {
                                if (dates.Trim() == "")
                                    dates = d.Date.ToString();
                                else
                                    dates = dates + "~" + System.Convert.ToString(d.Date);
                            }
                        }

                        break;
                    }

                case "Friday":
                    {
                        for (int i = 0; i <= day - 1; i++)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            if (d.DayOfWeek == DayOfWeek.Friday)
                            {
                                if (dates.Trim() == "")
                                    dates = d.Date.ToString();
                                else
                                    dates = dates + "~" + System.Convert.ToString(d.Date);
                            }
                        }

                        break;
                    }

                case "Saturday":
                    {
                        for (int i = 0; i <= day - 1; i++)
                        {
                            DateTime d = new DateTime(now.Year, now.Month, i + 1);
                            if (d.DayOfWeek == DayOfWeek.Saturday)
                            {
                                if (dates.Trim() == "")
                                    dates = d.Date.ToString();
                                else
                                    dates = dates + "~" + System.Convert.ToString(d.Date);
                            }
                        }

                        break;
                    }
            }

            return dates;
        }

    }
}