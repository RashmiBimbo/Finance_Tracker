using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using Finance_Tracker.App_Code;

namespace Finance_Tracker
{
    public class DBOperations
    {
        public readonly string strSQLCon;
        ConnectionStringSettingsCollection ConnStrs;
        public readonly string ConStrFinTrack, ConStrTestFinTrack;
        public OleDbConnection ConnFinTrack, ConnTestFinTrack;

        public DBOperations()
        {
            //ConnStrs = ConnectionStrings;
            ConStrFinTrack = ConnStrs["AppDBConnStrOriginal"].ConnectionString;
            ConStrTestFinTrack = ConnStrs["AppDBConnStr"].ConnectionString;
        }

        public OleDbConnection InitializeConnection(string constr)
        {
            OleDbConnection conn = null;
            try
            {
                conn = new OleDbConnection(constr);
            }
            catch (Exception)
            {
                throw;
            }
            return conn;
        }

        public bool AuthenticatCon()
        {
            bool AuthenticatCon = true;
            try
            {
                ConnFinTrack = new OleDbConnection(ConStrFinTrack);
            }
            catch (Exception ex)
            {
                AuthenticatCon = false;
            }
            try
            {
                ConnTestFinTrack = new OleDbConnection(ConStrTestFinTrack);
            }
            catch (Exception ex)
            {
                AuthenticatCon = false;
            }
            return AuthenticatCon;
        }

        public bool InsertValues(string query, OleDbConnection conn)
        {
            conn = conn ??ConnTestFinTrack;
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
        public DataTable GetDataProc(string proc, OleDbConnection conn, Array paramCln = null)
        {
            DataTable dt;
            OleDbCommand cmd = null;
            try
            {
                conn = conn ?? ConnTestFinTrack;
                conn.Open();
                cmd = new OleDbCommand(proc, conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 60
                };
                if (paramCln != null) cmd.Parameters.AddRange(paramCln);
                dt = GetData(cmd);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                if (cmd != null) cmd.Dispose();
            }
        }

        public DataTable SelQuery(string strQry, OleDbConnection conn)
        {
            DataTable results = GetData(strQry, conn ?? ConnTestFinTrack);
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

        private DataTable GetData(OleDbCommand cmd)
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
                if (adapter != null)
                    adapter.Dispose();
            }
        }

        public string GenrateNewID(string strTable, OleDbConnection conn, string strColmn = "pRowID")
        {
            string GenrateNewID;
            try
            {
                string StrQry = " Select 'VC'+Right('00000000'+convert(varchar,(Convert(int,Right(Isnull(Max(" + strColmn + "),0),8)) +1)),8) as pRowID from " + strTable + "";
                DataTable dtTemp = SelQuery(StrQry, conn ?? ConnTestFinTrack);
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

        public DataSet ImportExceltoDataset(string Excel)
        {
            DataSet dsExcel = new DataSet();
            // Dim dtexcel As New DataTable()
            bool hasHeaders = false;
            int i;

            string HDR = hasHeaders ? "Yes" : "No";
            string strConn = null;

            if (Excel.Substring(Excel.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Excel + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Excel + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            // Looping Total Sheet of Xl File
            // foreach (DataRow schemaRow in schemaTable.Rows)
            // {
            // }

            // Looping a first Sheet of Xl File
            for (i = 0; i <= schemaTable.Rows.Count - 1; i++)
            {
                DataRow schemaRow = schemaTable.Rows[i];
                string sheet = schemaRow["TABLE_NAME"].ToString();
                var strtblName = sheet;
                strtblName = strtblName.Replace("$", "");
                DataTable dtexcel = new DataTable(strtblName);
                if (!sheet.EndsWith("_"))
                {
                    string query = "SELECT  * FROM [" + sheet + "]";
                    System.Data.OleDb.OleDbDataAdapter daexcel = new System.Data.OleDb.OleDbDataAdapter(query, conn);
                    // dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }
                dsExcel.Tables.Add(dtexcel);
            }
            conn.Close();
            return dsExcel;
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

        public bool ExecuteSP(DataTable dtInsert, string strTbl)
        {
            bool blnExecuteQry = true;
            //switch (ConName)
            //{
            //    case Connect.ConName.ConnectionString2:
            //        {
            //            try
            //            {
            //                var sqlCon = new System.Data.SqlClient.SqlConnection(strSQLCon);

            //                using ((sqlCon))
            //                {
            //                    System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();

            //                    sqlComm.Connection = sqlCon;

            //                    sqlComm.CommandText = "SP_CustomerItem";
            //                    sqlComm.CommandType = CommandType.StoredProcedure;

            //                    sqlComm.Parameters.AddWithValue("@tblSql", dtInsert);
            //                    // sqlComm.Parameters.AddWithValue("@tblName", strTbl)

            //                    sqlCon.Open();

            //                    blnExecuteQry = sqlComm.ExecuteNonQuery() >= 0;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                blnExecuteQry = false;
            //            }

            //            break;
            //        }
            //}

            return blnExecuteQry;
        }

        public bool ExecuteSP_SalePro(DataTable dtInsert, string ProcName)
        {
            bool blnExecuteQry = true;
            //switch (ConName)
            //{
            //    case Connect.ConName.ConnectionString2:
            //        {
            //            try
            //            {
            //                var sqlCon = new System.Data.SqlClient.SqlConnection(strSQLCon);

            //                using ((sqlCon))
            //                {
            //                    System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();

            //                    sqlComm.Connection = sqlCon;

            //                    sqlComm.CommandText = ProcName;  // "SP_Insert_Depot_Sale_Projection"
            //                    sqlComm.CommandType = CommandType.StoredProcedure;

            //                    sqlComm.Parameters.AddWithValue("@tbl_DepotSaleProjection", dtInsert);
            //                    // sqlComm.Parameters.AddWithValue("@tblName", strTbl)

            //                    sqlCon.Open();

            //                    blnExecuteQry = sqlComm.ExecuteNonQuery() >= 0;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                blnExecuteQry = false;
            //            }
            //            break;
            //        }
            //}

            return blnExecuteQry;
        }
    }
}