using AjaxControlToolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.OleDb;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.DateTime;

namespace Finance_Tracker
{
    public partial class Review : System.Web.UI.Page
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
        private static int chKCount = 0;

        #region Page Code

        private DataTable GVReportsDS
        {
            get
            {
                DataTable dt = (DataTable)Session["Review_GVReportsDS"];

                if (!(dt?.Rows.Count > 0))
                {
                    dt = GetData(true);
                    Session["Review_GVReportsDS"] = dt;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    Session["Review_GVReportsDS"] = null;
                else
                    Session["Review_GVReportsDS"] = value;
            }
        }

        private DataTable GetData(bool IsApproved)
        {
            DataTable dt = null;
            try
            {
                string fromDt = TxtMnth.ToolTip.Split(',')[0];
                string toDt = TxtMnth.ToolTip.Split(',')[1];

                OleDbParameter[] paramCln = new OleDbParameter[]
                {
                     new OleDbParameter("@From_Date", fromDt)
                    ,new OleDbParameter("@To_Date",  toDt)
                    ,new OleDbParameter("@Type",  DdlType.SelectedValue)
                    ,new OleDbParameter("@Approver_Id", string.Empty)
                    ,new OleDbParameter("@IsApproved", IsApproved)
                    ,new OleDbParameter("@User_Id", DdlUsr.SelectedValue)
                    ,new OleDbParameter("@Report_Id",Convert.ToInt16(0))
                    ,new OleDbParameter("@Role_Id", DdlUsrType.SelectedValue)
                    ,new OleDbParameter("@Location_Id",  Session["Location_Id"].ToString())
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DBOprn.AuthenticatConns())
            {
                PopUp("Database connection could not be established");
                return;
            }
            if (!Page.IsPostBack)
            {
                string usrId = Session["User_Id"]?.ToString();
                if (usrId == null || usrId == "")
                    Response.Redirect("~/Account/Login.aspx");

                int RoleId = Convert.ToInt32(Session["Role_Id"]?.ToString());
                if (!(RoleId == 1 || RoleId == 4))
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                TxtMnth.Attributes.Add("autocomplete", "off");
                Menu1_MenuItemClick(Menu1, new MenuEventArgs(Menu1.Items[0]));
                DdlCatType_DataBinding(DdlCatType, new EventArgs());
                DdlUsrType_DataBinding(DdlUsrType, new EventArgs());
                chKCount = 0;
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

            //User Clicked the menu item
            if (Menu1.SelectedValue == "0")
            {
                GVReports.DataSource = null;
                GVReports.Visible = false;
                //TxtMnth.Text = DateTime.Now.ToString(MonthFormat);
                TxtMnth.Text = TextBoxWatermarkExtender1.WatermarkText;
            }
        }

        protected void DdlCatType_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            FillDdl(ddl, "SP_Get_CategoryTypes", "0");
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = null;
            if (ddl.Equals(DdlCat))
            {
                prntddl = DdlCatType;
            }
            FillDdl(ddl, "SP_Get_Categories", "0", prntddl,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Type_Id", prntddl.SelectedValue)
                }
            );
        }

        protected void DdlReport_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = null;
            if (ddl.Equals(DdlReport))
            {
                prntddl = DdlCat;
            }

            FillDdl(ddl, "SP_Get_Reports", "0", prntddl,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Category_Id", prntddl.SelectedValue)
                }
            );
        }

        protected void DdlUsrType_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlUsrType, "SP_Get_Roles", "0");
            int roleId = Convert.ToInt32(Session["Role_Id"]);

            DdlUsrType.Items.FindByValue("4").Enabled = roleId > 4;
            DdlUsrType.Items.FindByValue("1").Enabled = roleId == 4;
        }

        protected void DdlUsr_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlUsr, "SP_Get_Users", "", DdlUsrType,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Role_Id", DdlUsrType.SelectedValue)
                   ,new OleDbParameter("@Location_Id", Session["Location_Id"].ToString())                   
                }
            );
        }

        protected void DdlCatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList child = null;
            DropDownList grndchild = null;
            DropDownList ddl = (DropDownList)sender;
            ddl.ToolTip = ddl.SelectedItem.Text;
            if (sender.Equals(DdlCatType))
            {
                child = DdlCat;
                grndchild = DdlReport;
            }
            child.DataBind();
            grndchild.DataBind();
        }

        protected void DdlCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList child = null;
            ddl.ToolTip = ddl.SelectedItem.Text;

            if (sender.Equals(DdlCat))
                child = DdlReport;
            child.DataBind();
        }

        protected void DdlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.ToolTip = ddl.SelectedItem.Text;
        }

        protected void DdlUsrType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlUsrType.ToolTip = DdlUsrType.SelectedItem.Text;
            DdlUsr.DataBind();
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

            if (!TryParseExact(TxtMnth.Text, MonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                PopUp("Please enter month in correct format like 'Jan-2024'!");
                return;
            }
            GVReportsDS = null;
            GVReports.DataSource = null;
            GVReports.DataBind();
        }

        protected void GVReports_DataBinding(object sender, EventArgs e)
        {
            if (GVReports.DataSource == null)
            {
                DataTable dt = GVReportsDS;
                if (dt != null)
                {
                    GVReports.DataSource = dt;
                    GVReports.Visible = true;
                    DivExport.Visible = true;
                }
                else
                {
                    GVReports.Visible = false;
                    DivExport.Visible = false;
                    PopUp("No data found!");
                }
            }
        }

        public void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
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
            foreach (GridViewRow gvRow in GVReports.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chKCount += chked ? 1 : -1;
                }
            }
            if (GVReports.Rows.Count < chKCount)
                chKCount = GVReports.Rows.Count;
            else if (chKCount < 0)
                chKCount = 0;
            //BtnReject.Enabled = chKCount > 0;
        }

        protected void CBReject_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chKCount += cb.Checked ? 1 : -1;
            if (GVReports.Rows.Count < chKCount)
                chKCount = GVReports.Rows.Count;
            else if (chKCount < 0)
                chKCount = 0;
            //BtnReject.Enabled = chKCount > 0;
            GridViewRow row = GVReports.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVReports.Rows.Count == chKCount);
        }

        protected void BtnReject_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonParam = ConstructJSON();

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
                    string id = ((HiddenField)gvRow.Cells[gvRow.Cells.Count - 1].Controls[1]).Value;
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
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                            }
                        };
                    dtls.Add(paramVals);
                    cb.Checked = false;
                    chKCount--;
                }
                continue;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        protected void TxtMnth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox TB = (TextBox)sender;

                bool convert = TryParseExact(TB.Text, MonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);
                if (!convert)
                {
                    PopUp("Please enter month in correct format like 'Jan-2024'");
                    TB.Text = TextBoxWatermarkExtender1.WatermarkText;
                    return;
                }

                int yr = dt.Year, mnth = dt.Month;
                DateTime strtDt = new DateTime(yr, mnth, 01);
                string fromDt = strtDt.ToString(SqlDateFormat);
                string toDt = strtDt.AddDays(DaysInMonth(yr, mnth) - 1).ToString(SqlDateFormat);
                TB.ToolTip = fromDt + "," + toDt;

            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }
    }
}