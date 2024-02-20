using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.DateTime;

namespace Finance_Tracker
{
    public partial class Approve : System.Web.UI.Page
    {
        const string DateFormat = "dd-MMM-yyyy";
        const string MonthFormat = "MMM-yyyy";
        const string SqlDateFormat = "yyyy-MM-dd";
        private readonly DateTime today = Today;
        private readonly int crntYr = Today.Year;
        private readonly int crntMnth = Today.Month;
        private readonly DateTime crntMnthDay1 = new DateTime(Today.Year, Today.Month, 1);
        private readonly DateTime crntMnthLastDay = new DateTime(Today.Year, Today.Month, DaysInMonth(Today.Year, Today.Month));
        private readonly DateTime lstMnth = new DateTime(Today.Year, Today.Month, 1).AddMonths(-1);

        private readonly DBOperations DBOprn = new DBOperations();
        private static int chKCountA = 0;
        private static int chKCountR = 0;

        private DataTable GVReportsDS
        {
            get
            {
                DataTable dt = (DataTable)Session["Approve_GVReportsDS"];

                if (!(dt?.Rows.Count > 0))
                {
                    dt = GetData(false);
                    Session["Approve_GVReportsDS"] = dt;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    Session["Approve_GVReportsDS"] = null;
                else
                    Session["Approve_GVReportsDS"] = value;
            }
        }

        private DataTable GVApprovedDS
        {
            get
            {
                DataTable dt = (DataTable)Session["Approve_GVApprovedDS"];

                if (!(dt?.Rows.Count > 0))
                {
                    dt = GetData(true);
                    Session["Approve_GVApprovedDS"] = dt;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    Session["Approve_GVApprovedDS"] = null;
                else
                    Session["Approve_GVApprovedDS"] = value;
            }
        }

        private DataTable GetData(bool IsApproved)
        {
            DataTable dt = null;
            try
            {
                //string fromDt = new DateTime(year, mnthNo, 01).ToString(SqlDateFormat);
                //string toDt = new DateTime(year, mnthNo, DaysInMonth(year, mnthNo)).ToString(SqlDateFormat);
                string fromDt = TxtMnth.ToolTip.Split(',')[0];
                string toDt = TxtMnth.ToolTip.Split(',')[1];

                OleDbParameter[] paramCln = new OleDbParameter[]
                {
                     new OleDbParameter("@From_Date", fromDt)
                    ,new OleDbParameter("@To_Date",  toDt)
                    ,new OleDbParameter("@Type",  DdlType.SelectedValue)
                    ,new OleDbParameter("@Approver_Id", Session["User_Id"].ToString())
                    ,new OleDbParameter("@IsApproved", IsApproved)
                    ,new OleDbParameter("@User_Id", DdlUsr.SelectedValue)
                };
                dt = DBOprn.GetDataProc("SP_Get_Submit_Tasks_SubOrdinates", DBOprn.ConnPrimary, paramCln);
                if (!(dt?.Rows.Count > 0))
                    dt = null;
                return dt;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return null;
            }
        }

        #region Page Code

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!DBOprn.AuthenticatConns())
                {
                    PopUp("Database connection could not be established");
                    return;
                }
                string usrId = Session["User_Id"]?.ToString();
                if (usrId == null || usrId == "")
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }
                if (Session["Is_Approver"].ToString() == "0")
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                Menu1_MenuItemClick(Menu1, new MenuEventArgs(Menu1.Items[0]));
                TxtMnth.Attributes.Add("autocomplete", "off");

                chKCountA = 0;
                chKCountR = 0;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //Verifies that the control is rendered 
        }

        #endregion Page Code

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            int eval = e is null ? 0 : int.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = eval;
            MultiView1.Views[eval].Focus();

            //TxtMnth.Text = Now.ToString(MonthFormat);
            DdlUsr_DataBinding(DdlUsr, new EventArgs());
            DdlType.SelectedIndex = 0;

            if (Menu1.SelectedValue == "0")
                ResetGVReports();
            else if (Menu1.SelectedValue == "1")
                ResetGVApprovd();
        }

        private void ResetGVReports()
        {
            GVReportsDiv.Visible = false;
            GVReports.DataSource = null;
            GVReportsDS = null;
        }

        private void ResetGVApprovd()
        {
            DivApproved.Visible = false;
            GVApproved.DataSource = null;
            GVApprovedDS = null;
        }

        protected void DdlUsr_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlUsr, "SP_Get_SubOrdinates", "", null,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Approver_Id", Session["User_Id"].ToString())
                }
            );
            DdlUsr.SelectedIndex = 0;
        }

        protected void DdlUsr_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlUsr.ToolTip = DdlUsr.SelectedItem.Text;
        }

        private void FillDdl(DropDownList ddl, String proc, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("All", selectVal));
            ddl.SelectedIndex = 0;
            ddl.ToolTip = "All";

            if (prntDdl != null && prntDdl.SelectedIndex == 0)
                return;
            try
            {
                DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            if (TxtMnth.Text == string.Empty)
            {
                PopUp("Please select a Month!");
                TxtMnth.Focus();
                return;
            }
            if (!TryParseExact(TxtMnth.Text, MonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                PopUp("Please enter month in correct format like 'Jan-2024'!");
                TxtMnth.Text = TextBoxWatermarkExtender3.WatermarkText;
                TxtMnth.ToolTip = crntMnthDay1.ToString(SqlDateFormat) + "," + crntMnthLastDay.ToString(SqlDateFormat);
                TxtMnth.Focus();
                return;
            }
            else
            {
                TxtMnth.ToolTip = dt.ToString(SqlDateFormat) + "," + new DateTime(dt.Year, dt.Month, DaysInMonth(dt.Year, dt.Month)).ToString(SqlDateFormat);
            }
            if (MultiView1.ActiveViewIndex == 0)
            {
                ResetGVReports();
                GVReports.DataBind();
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                ResetGVApprovd();
                GVApproved.DataBind();
            }
        }

        protected void GVReports_DataBinding(object sender, EventArgs e)
        {
            if (GVReports.DataSource == null)
            {
                DataTable dt = GVReportsDS;
                if (dt != null)
                {
                    GVReports.DataSource = dt;
                    GVReportsDiv.Visible = true;
                    BtnApprove.Enabled = false;
                }
                else
                {
                    GVReportsDiv.Visible = false;
                    PopUp("No data available!");
                }
            }
        }

        public void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        protected void CBApprovH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVReports.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chKCountA += chked ? 1 : -1;
                }
            }
            if (GVReports.Rows.Count < chKCountA)
                chKCountA = GVReports.Rows.Count;
            else if (chKCountA < 0)
                chKCountA = 0;

            BtnApprove.Enabled = chKCountA > 0;
        }

        protected void CBApprov_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chKCountA += cb.Checked ? 1 : -1;
            if (GVReports.Rows.Count < chKCountA)
                chKCountA = GVReports.Rows.Count;
            else if (chKCountA < 0)
                chKCountA = 0;
            BtnApprove.Enabled = chKCountA > 0;
            GridViewRow row = GVReports.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVReports.Rows.Count == chKCountA);
        }

        protected void BtnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonParam = ConstructJSON();

                if (!string.IsNullOrWhiteSpace(jsonParam))
                {
                    var output = DBOprn.ExecScalarProc("SP_Approve_Tasks", DBOprn.ConnPrimary,
                        new OleDbParameter[]
                        {
                            new OleDbParameter("@Collection", jsonParam)
                        }
                    );

                    if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                    {
                        PopUp(output.ToString());
                        return;
                    }
                    PopUp("Tasks approved successfully!");


                    GVReports.DataSource = null;
                    GVReportsDS = null;
                    GVReports.DataBind();
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        private string ConstructJSON()
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();

            foreach (GridViewRow gvRow in GVReports.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (cb.Checked)
                {
                    string id = ((Label)gvRow.Cells[gvRow.Cells.Count - 1].Controls[1]).Text;
                    Dictionary<string, string> paramVals = new Dictionary<string, string>()
                        {
                            {
                                "REC_ID",
                                id
                            },
                            {
                                "APPROVE_DATE",
                                Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                            },
                            {
                                "MODIFIED_BY",
                                Session["User_Name"].ToString()
                            },
                            {
                                "MODIFIED_DATE",
                                Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                            }
                        };
                    dtls.Add(paramVals);
                    cb.Checked = false;
                    chKCountA--;
                }
                continue;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        protected void TxtMnth_TextChanged(object sender, EventArgs e)
        {
            int mnthNo = Today.Month, year = Today.Year;
            try
            {
                DateTime dt = ParseExact(TxtMnth.Text, MonthFormat, CultureInfo.InvariantCulture);
                mnthNo = dt.Month;
                year = dt.Year;
                TxtMnth.ToolTip = new DateTime(year, mnthNo, 01).ToString(SqlDateFormat) + "," + new DateTime(year, mnthNo, DaysInMonth(year, mnthNo)).ToString(SqlDateFormat);
            }
            catch (Exception ex)
            {
                PopUp("Please enter the month in correct format!");
                TxtMnth.Text = TextBoxWatermarkExtender3.WatermarkText;
                TxtMnth.ToolTip = crntMnthDay1.ToString(SqlDateFormat) + "," + crntMnthLastDay.ToString(SqlDateFormat);
            }
        }

        protected void GVApproved_DataBinding(object sender, EventArgs e)
        {
            if (GVApproved.DataSource == null)
            {
                DataTable dt = GVApprovedDS;
                if (dt != null)
                {
                    GVApproved.DataSource = dt;
                    DivApproved.Visible = true;
                }
                else
                {
                    DivApproved.Visible = false;
                    PopUp("No data available!");
                }
            }
        }

        protected void LBLocn_Click(object sender, EventArgs e)
        {
            LinkButton LBLocn = (LinkButton)sender;
            string fullPath = LBLocn.ToolTip;
            string fileName = LBLocn.Text;
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", $"attachment;filename={fileName}");
                Response.TransmitFile(fullPath);
                Response.End();
            }
            catch (Exception ex)
            {
                PopUp($"Error occurred: \n {ex.Message}");
            }
        }

        protected void CBRejectH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVApproved.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chKCountR += chked ? 1 : -1;
                }
            }
            if (GVApproved.Rows.Count < chKCountR)
                chKCountR = GVApproved.Rows.Count;
            else if (chKCountR < 0)
                chKCountR = 0;
            BtnReject.Enabled = chKCountR > 0;
        }

        protected void CBReject_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chKCountR += cb.Checked ? 1 : -1;
            if (GVApproved.Rows.Count < chKCountR)
                chKCountR = GVApproved.Rows.Count;
            else if (chKCountA < 0)
                chKCountR = 0;
            BtnReject.Enabled = chKCountR > 0;
            GridViewRow row = GVApproved.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVApproved.Rows.Count == chKCountR);
        }

        protected void BtnReject_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonParam = ConstructJSONReject();

                if (!string.IsNullOrWhiteSpace(jsonParam))
                {
                    var output = DBOprn.ExecScalarProc("SP_Reject_Tasks", DBOprn.ConnPrimary,
                        new OleDbParameter[]
                        {
                            new OleDbParameter("@Collection", jsonParam)
                        }
                    );

                    if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                    {
                        PopUp(output.ToString());
                        return;
                    }
                    PopUp("Tasks rejected successfully!");
                    GVApprovedDS = null;
                    GVApproved.DataSource = null;
                    GVApproved.DataBind();
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        private string ConstructJSONReject()
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();

            foreach (GridViewRow gvRow in GVApproved.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (cb.Checked)
                {
                    string id = ((Label)gvRow.Cells[gvRow.Cells.Count - 1].Controls[1]).Text;
                    Dictionary<string, string> paramVals = new Dictionary<string, string>()
                        {
                            {
                                "REC_ID",
                                id
                            },
                            {
                                "MODIFIED_BY",
                                Session["User_Name"].ToString()
                            },
                            {
                                "MODIFIED_DATE",
                                Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                            }
                        };
                    dtls.Add(paramVals);
                    cb.Checked = false;
                    chKCountR--;
                }
                continue;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        protected void TxtMnth_TextChanged1(object sender, EventArgs e)
        {

        }
    }
}