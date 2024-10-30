using AjaxControlToolkit.Bundling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using static Finance_Tracker.Common;

namespace Finance_Tracker
{
    public class DBOperations
    {
        private readonly ConnectionStringSettingsCollection ConnStrs;
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
                cmd?.Dispose();
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
                cmd?.Dispose();
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
                cmd?.Dispose();
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
                adapter?.Dispose();
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

    }
}