using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.DateTime;
using static System.Convert;

namespace Finance_Tracker
{
    public partial class Performance : Page
    {
        private const string DateFormat = "dd-MMM-yyyy";
        private const string MonthFormat = "MMM-yyyy";
        private const string SqlDateFormat = "yyyy-MM-dd";
        private readonly string emp = string.Empty;
        private static int chKCount = 0;
        private static int chKCountGVAdd = 0;
        private readonly DateTime today = Today;
        private readonly int crntYr = Today.Year;
        private readonly int crntMnth = Today.Month;
        private readonly DateTime crntMnthDay1 = new DateTime(Today.Year, Today.Month, 1);
        private readonly DateTime crntMnthLastDay = new DateTime(Today.Year, Today.Month, DaysInMonth(Today.Year, Today.Month));
        private readonly DateTime lstMnth = new DateTime(Today.Year, Today.Month, 1).AddMonths(-1);

        private readonly DBOperations DBOprn = new DBOperations();

        private DataTable DdlReport1DS
        {
            get
            {
                DataTable dt = (DataTable)Session["Performance_DdlReport1DS"];
                if (!(dt?.Rows.Count > 0))
                {
                    dt = DBOprn.GetDataProc("SP_Get_Reports", DBOprn.ConnPrimary,
                        new OleDbParameter[]
                        {
                            new OleDbParameter("@Category_Id", DdlCatS.SelectedValue)
                        }
                    );
                    if (dt.Rows.Count == 0)
                        dt = null;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["Performance_DdlReport1DS"] = value;
            }
        }

        private DataTable GVReports3DS
        {
            get
            {
                DataTable dt = (DataTable)Session["Performance_GVReports3DS"];
                if (!(dt?.Rows.Count > 0))
                {
                    string strtDate = TxtMnth3.ToolTip.Split(',')[0];
                    string endDate = TxtMnth3.ToolTip.Split(',')[1];
                    string User_Id = Session["User_Id"]?.ToString();
                    string Role_Id = Session["Role_Id"]?.ToString();

                    OleDbParameter[] paramCln = new OleDbParameter[]
                    {
                         new OleDbParameter("@Start_Date", strtDate)
                        ,new OleDbParameter("@End_Date",  endDate)
                        ,new OleDbParameter("@User_Id", User_Id)
                        ,new OleDbParameter("@Role_Id", Role_Id)
                        ,new OleDbParameter("@Category_Type_Id", DdlCatType3.SelectedValue)
                        ,new OleDbParameter("@Category_Id", DdlCat3.SelectedValue)
                        ,new OleDbParameter("@Report_Id", DdlReport3.SelectedValue)
                        ,new OleDbParameter("@Type", DdlType3.SelectedValue)
                        ,new OleDbParameter() {
                            ParameterName = "@IsApproved",
                            Value = 1,
                            OleDbType = OleDbType.Boolean
                        }
                    };
                    dt = DBOprn.GetDataProc("SP_Get_Tasks", DBOprn.ConnPrimary, paramCln);
                    if (dt.Rows.Count == 0)
                        dt = null;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["Performance_GVReports3DS"] = value;
            }
        }

        private DataTable GVReports2DS
        {
            get
            {
                DataTable dt = (DataTable)Session["Performance_GVReports2DS"];
                if (!(dt?.Rows.Count > 0))
                {
                    string strtDate = TxtMnth2.ToolTip.Split(',')[0];
                    string endDate = TxtMnth2.ToolTip.Split(',')[1];
                    string User_Id = Session["User_Id"]?.ToString();
                    string Role_Id = Session["Role_Id"]?.ToString();

                    OleDbParameter[] paramCln = new OleDbParameter[]
                        {
                            new OleDbParameter("@Start_Date", strtDate)
                           ,new OleDbParameter("@End_Date", endDate)
                           ,new OleDbParameter("@User_Id", User_Id)
                           ,new OleDbParameter("@Role_Id", Role_Id)
                           ,new OleDbParameter("@Category_Type_Id", DdlCatType2.SelectedValue)
                           ,new OleDbParameter("@Category_Id", DdlCat2.SelectedValue)
                           ,new OleDbParameter("@Report_Id", DdlReport2.SelectedValue)
                           ,new OleDbParameter("@Type", DdlType2.SelectedValue)
                        };
                    dt = DBOprn.GetDataProc("SP_Get_Tasks", DBOprn.ConnPrimary, paramCln);
                    if (dt.Rows.Count == 0)
                        dt = null;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["Performance_GVReports2DS"] = value;
            }
        }

        private DataTable GVAddDS
        {
            get
            {
                DataTable dt = (DataTable)Session["Performance_GVAddDS"];
                if (!(dt?.Rows.Count > 0))
                {
                    string fromDt = TxtMnthM.ToolTip.Split(',')[0];
                    string toDt = TxtMnthM.ToolTip.Split(',')[1];
                    string weekNo = DdlWeekM.SelectedValue;

                    dt = DBOprn.GetDataProc("SP_Get_UserTasks", DBOprn.ConnPrimary
                    , new OleDbParameter[]
                        {
                            new OleDbParameter("@User_Id", Session["User_Id"].ToString())
                           ,new OleDbParameter("@From_Date", fromDt)
                           ,new OleDbParameter("@To_Date", toDt)
                           ,new OleDbParameter("@WeekNo", weekNo)
                           ,new OleDbParameter("@Report_Type", DdlTypeM.SelectedValue)
                        }
                    );
                    if (dt.Rows.Count == 0)
                        dt = null;
                    Session["Performance_GVAddDS"] = dt;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["Performance_GVAddDS"] = value;
            }
        }

        #region Page Code

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string usrId = Session["User_Id"]?.ToString();
                if (usrId == null || usrId == emp)
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }
                TxtMnthM.Attributes.Add("autocomplete", "off");
                TxtMnth2.Attributes.Add("autocomplete", "off");
                TxtMnth3.Attributes.Add("autocomplete", "off");

                DdlCatType_DataBinding(DdlCatTypeS, new EventArgs());
                DdlCat_DataBinding(DdlCatS, new EventArgs());
                DdlReport_DataBinding(DdlReportS, new EventArgs());
                Menu1_MenuItemClick(Menu1, new MenuEventArgs(Menu1.Items[0]));
                chKCount = 0;
                chKCountGVAdd = 0;
                CETxtMnthM.StartDate = lstMnth;
                CETxtMnthM.EndDate = crntMnthLastDay;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //Verifies that the control is rendered
        }

        #endregion Page Code

        #region MenuClick

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            int eval = e is null ? 0 : int.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = eval;
            MultiView1.Views[eval].Focus();

            LblError.Text = emp;
            //User Clicked the menu item
            if (sender != null)
            {
                BtnSubmit.Visible = false;
                ResetTab1();
                Menu1.Items[0].Text = "Add Tasks |";
                switch (Menu1.SelectedValue)
                {
                    case "0":
                    {
                        DivAddSingl.Visible = false;
                        DivAddMultiple.Visible = true;
                        SetDivAddMultiple();
                        break;
                    }
                    case "1":
                    {
                        ResetTab2();
                        break;
                    }
                    case "2":
                    {
                        ResetTab3();
                        break;
                    }
                }
            }
        }

        #region DivAddMultiple

        private void SetDivAddMultiple()
        {
            Menu1.Items[0].Text = "Add Tasks |";
            DdlTypeM.SelectedIndex = 0;
            //TxtMnthM.Text = Today.ToString(MonthFormat);

            //TxtMnthM.ToolTip = fromDt + "," + toDt;
            TxtMnthM.Text = emp;
            //TxtMnthM.Text = today.ToString(MonthFormat);
            DivWeekM.Visible = false;
            GVAdd.SelectedIndex = -1;
            DivGVBtn.Visible = false;
        }

        protected void DdlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DivWeekM.Visible = DdlTypeM.SelectedIndex == 2;
            DivGVBtn.Visible = false;
            //TxtDueDtM.Text = emp;
            DdlWeekM.SelectedIndex = 0;
        }

        protected void DdlWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            DivGVBtn.Visible = false;
            //TxtDueDtM.Text = emp;
        }

        protected void BtnViewAssTask_Click(object sender, EventArgs e)
        {
            DivGVBtn.Visible = false;
            if (TxtMnthM.Text == emp)
            {
                PopUp("Please select a Month!");
                TxtMnthM.Focus();
                return;
            }
            if (!TryParseExact(TxtMnthM.Text, MonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                PopUp("Please enter month in correct format like 'Jan-2024'!");
                TxtMnthM.Focus();
                return;
            }
            else
            {
                TxtMnthM.ToolTip = dt.ToString(SqlDateFormat) + "," + new DateTime(dt.Year, dt.Month, DaysInMonth(dt.Year, dt.Month)).ToString(SqlDateFormat);
            }
            if (DdlTypeM.SelectedIndex == 0)
            {
                PopUp("Please select Task type!");
                DdlTypeM.Focus();
                return;
            }
            if (DdlTypeM.SelectedIndex == 2 && DdlWeekM.SelectedIndex == 0)
            {
                PopUp("Please select a Week!");
                DdlWeekM.Focus();
                return;
            }
            GVAddDS = null;
            GVAdd.DataSource = null;
            GVAdd.DataBind();
        }

        protected void GVAdd_DataBinding(object sender, EventArgs e)
        {
            GVAdd.SelectedIndex = -1;
            try
            {
                if (GVAdd.DataSource == null)
                {
                    DataTable dt = GVAddDS;
                    if (dt != null)
                    {
                        GVAdd.DataSource = dt;
                        DivGVBtn.Visible = true;
                    }
                    else
                    {
                        DivGVBtn.Visible = false;
                        PopUp("No data found!");
                    }
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void GVAdd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Attach client-side onclick event to the row
                //e.Row.Attributes["onclick"] = "SelectRow(this);";
                //((FileUpload)e.Row.Cells[6].FindControl("FUAdd")).Attributes["onchange"] = "saveFileName(this);";
            }
        }

        protected void GVAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GVAdd.SelectedIndex == -1) return;

            DataTable dSrc = GVAddDS;
            if (dSrc == null || dSrc.Rows.Count == 0) return;

            GridViewRow slctRo = GVAdd.Rows[GVAdd.SelectedIndex];

            int sno = ToInt32(slctRo.Cells[2].Text);
            DataRow dRo = dSrc.Select($"sno = {sno}")?[0];

            if (dRo == null) return;

            //string typ = dRo["Type_Orgnl"].ToString();
            //DdlTypeM.SelectedValue = typ;
            //DdlTypeM.Enabled = false;

            string dueDt = dRo["Due_Date"].ToString();
            DateTime dt;
            int mnthNo = crntMnth, year = crntYr;
            if (int.TryParse(dueDt, out int day))
            {
                try
                {
                    dt = ParseExact(TxtMnthM.Text, MonthFormat, CultureInfo.InvariantCulture);
                    mnthNo = dt.Month;
                    year = dt.Year;
                }
                catch (Exception ex)
                { }
                //TxtDueDtM.Text = new DateTime(year, mnthNo, day).ToString(DateFormat);
                DivWeekM.Visible = false;
            }
            else
            {
                //TxtDueDtM.Text = dueDt;
                DivWeekM.Visible = true;
            }
        }

        protected void CBSubmitH_CheckedChanged1(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVAdd.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[1].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chKCountGVAdd += chked ? 1 : -1;
                }
            }
            if (GVAdd.Rows.Count < chKCountGVAdd)
                chKCountGVAdd = GVAdd.Rows.Count;
            else if (chKCountGVAdd < 0)
                chKCountGVAdd = 0;
            BtnAddM.Enabled = chKCountGVAdd > 0;
        }

        protected void CBSubmit_CheckedChanged1(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chKCountGVAdd += cb.Checked ? 1 : -1;
            if (GVAdd.Rows.Count < chKCountGVAdd)
                chKCountGVAdd = GVAdd.Rows.Count;
            else if (chKCountGVAdd < 0)
                chKCountGVAdd = 0;
            BtnAddM.Enabled = chKCountGVAdd > 0;
            GridViewRow row = GVAdd.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[1].Controls[1];
            cbH.Checked = (GVAdd.Rows.Count == chKCountGVAdd);
        }

        protected void BtnAddM_Click(object sender, EventArgs e)
        {
            if (DdlTypeM.SelectedIndex == 0)
            {
                PopUp("Please select a type!");
                DdlTypeM.Focus();
                return;
            }
            if (string.IsNullOrEmpty(TxtMnthM.Text))
            {
                PopUp("Please select a month!");
                TxtMnthM.Focus();
                return;
            }
            if (DdlTypeM.SelectedValue == "W" && DdlWeekM.SelectedValue == "0")
            {
                PopUp("Please select a week!");
                DdlWeekM.Focus();
                return;
            }
            string jsonParam = ConstructJSON_M(out int chkCnt);
            if (chkCnt == 0) PopUp("Please check any row to add!");

            if (SubMission("SP_Add_Multiple_Tasks", jsonParam))
            {
                PopUp("Tasks added successfully!");
                GVAddDS = null;
                GVAdd.DataSource = null;
                GVAdd.DataBind();
                chKCountGVAdd = 0;
            };
        }

        private string ConstructJSON_M(out int chkCnt)
        {
            chkCnt = 0;
            string jsonString = emp;
            DataTable dSrc = GVAddDS;
            if (dSrc == null || dSrc.Rows.Count == 0) return jsonString;

            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();
            foreach (GridViewRow gvRow in GVAdd.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (!cb.Checked)
                    continue;
                chkCnt++;
                int sno = ToInt32(gvRow.Cells[1].Text);
                DataRow dRo = dSrc.Select($"sno = {sno}")?[0];

                if (dRo == null) return jsonString;

                string fromDt = TxtMnthM.ToolTip.Split(',')[0];
                string toDt = TxtMnthM.ToolTip.Split(',')[1];
                string weekNo = DdlWeekM.SelectedValue;
                string reportName = dRo["Task_Name"].ToString();
                string rptType = dRo["Type_Orgnl"].ToString();
                string addDt = Now.ToString(SqlDateFormat);
                Label LblRoErr = (Label)gvRow.Cells[7].FindControl("LblRoErr");

                dynamic fuAdd = (FileUpload)gvRow.Cells[6].FindControl("FUAdd");
                if (!fuAdd.HasFile)
                {
                    LblRoErr.Text = "Please upload a File!";
                    LblRoErr.CssClass = "control-label text-danger ";
                    cb.Checked = false;
                    continue;
                }
                else
                    LblRoErr.Text = emp;

                if (!FileOprn(fuAdd.PostedFile, TxtMnthM.Text, weekNo, DdlTypeM.SelectedValue, reportName, out string fullPath, out string msg))
                {
                    LblRoErr.Text = msg;
                    LblRoErr.CssClass = "control-label text-danger ";
                    cb.Checked = false;
                    continue;
                }
                else
                    LblRoErr.Text = emp;

                Dictionary<string, string> paramVals = new Dictionary<string, string>()
                {
                    {"USER_ID", Session["User_Id"].ToString()},
                    {"REPORT_ID",  dRo["ReportId"].ToString()},
                    {"REPORT_TYPE", rptType},
                    {"ADD_DATE", addDt},
                    {"MONTH_FROM_DATE", fromDt},
                    {"MONTH_TO_DATE", toDt},
                    {"MONTH_WEEK_NO", weekNo},
                    {"LOCATION", fullPath},
                    {"CREATED_BY", Session["User_Name"].ToString()}
                };
                dtls.Add(paramVals);
                cb.Checked = false;
                chKCount--;
                LblRoErr.Text = "Success";
                LblRoErr.CssClass = "control-label text-success ";
            }
            jsonString = dtls.Count > 0 ? JsonConvert.SerializeObject(dtls, Formatting.Indented) : emp;
            return jsonString;
        }

        #endregion DivAddMultiple

        private void ResetTab1()
        {
            //Menu1.Items[0].Text = "Add Task |";

            //enable cat type for admin user only
            string roleId = Session["Role_Id"]?.ToString();
            if (!string.IsNullOrWhiteSpace(roleId) && roleId == "1")
            {
                DdlCatTypeS.SelectedIndex = 0;
                DdlCatType_SelectedIndexChanged(DdlCatTypeS, new EventArgs());
                DdlCatTypeS.Enabled = true;
            }
            DdlCatS.SelectedIndex = 0;
            DdlCatS.Enabled = true;

            DdlReportS.DataBind();
            DdlReportS.SelectedIndex = 0;
            DdlReportS.Enabled = true;

            TxtMnthS.Text = Now.ToString(MonthFormat);
            TxtMnthS.Enabled = true;

            CalendarExtender1.StartDate = lstMnth;
            CalendarExtender1.EndDate = crntMnthLastDay;

            DdlWeekS.SelectedIndex = 0;
            DdlWeekS.Enabled = true;
            DivWeek1.Visible = false;

            DdlTypeS.SelectedIndex = 0;
            TxtDueDtS.Text = emp;
            BtnAdd.Text = "Add";
            LnkReport.Text = emp;
            DivLnk.Visible = false;
            BtnCncl.Visible = false;
        }

        private void ResetTab2()
        {
            DdlCatType_DataBinding(DdlCatType2, new EventArgs());
            DdlCat_DataBinding(DdlCat2, new EventArgs());
            DdlReport_DataBinding(DdlReport2, new EventArgs());

            GVReports2.DataSource = null;
            GVReports2.Visible = false;
            BtnSubmit.Visible = false;

            DateTime now = Now;
            var startDate = new DateTime(crntYr, crntMnth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            //TxtSD.Text = startDate.ToString(DateFormat);
            //TxtED.Text = endDate.ToString(DateFormat);
            TxtMnth2.Text = TextBoxWatermarkExtender2.WatermarkText;
            DdlType2.SelectedIndex = 0;
            CalendarExtender2.StartDate = lstMnth;
            CalendarExtender2.EndDate = crntMnthLastDay;

        }

        private void ResetTab3()
        {
            DdlCatType_DataBinding(DdlCatType3, new EventArgs());
            DdlCat_DataBinding(DdlCat3, new EventArgs());
            DdlReport_DataBinding(DdlReport3, new EventArgs());

            GVReports3.DataSource = null;
            GVReports3.Visible = false;

            TxtMnth3.Text = TextBoxWatermarkExtender3.WatermarkText;
            DdlType3.SelectedIndex = 0;
            //CalendarExtender4.StartDate = lstMnth;
            //CalendarExtender4.EndDate = crntMnthLastDay;

        }

        private void SetCatType(DropDownList ddl, DropDownList childDdl)
        {
            try
            {
                string roleId = Session["Role_Id"]?.ToString();
                if (!string.IsNullOrWhiteSpace(roleId) && roleId != "1")
                {
                    string catType = roleId == "2" ? "Corporate" : roleId == "3" ? "Plant" : emp;
                    catType = ddl.Items.FindByText(catType)?.Value;

                    ddl.SelectedValue = catType;
                    ddl.Enabled = false;
                    childDdl.DataBind();
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void DdlCatType_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, childDdl = null;
            if (ddl.Equals(DdlCatTypeS))
            {
                childDdl = DdlCatS;
            }
            else if (ddl.Equals(DdlCatType2))
            {
                childDdl = DdlCat2;
            }
            else if (ddl.Equals(DdlCatType3))
            {
                childDdl = DdlCat3;
            }
            FillDdl(ddl, "SP_Get_CategoryTypes", "0");
            SetCatType(ddl, childDdl);
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = null;
            if (ddl.Equals(DdlCatS))
            {
                prntddl = DdlCatTypeS;
            }
            else if (ddl.Equals(DdlCat2))
            {
                prntddl = DdlCatType2;
            }
            else if (ddl.Equals(DdlCat3))
            {
                prntddl = DdlCatType3;
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

            if (ddl.Equals(DdlReportS))
            {
                prntddl = DdlCatS;
            }
            else if (ddl.Equals(DdlReport2))
            {
                prntddl = DdlCat2;
            }
            else if (ddl.Equals(DdlReport3))
            {
                prntddl = DdlCat3;
            }

            FillDdl(ddl, "SP_Get_Reports", "0", prntddl,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Category_Id", prntddl.SelectedValue)
                }
            );
        }

        protected void DdlCatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList child = null;
            DropDownList grndchild = null;
            DropDownList ddl = (DropDownList)sender;
            ddl.ToolTip = ddl.SelectedItem.Text;
            if (sender.Equals(DdlCatTypeS))
            {
                child = DdlCatS;
                grndchild = DdlReportS;
            }
            else if (sender.Equals(DdlCatType2))
            {
                child = DdlCat2;
                grndchild = DdlReport2;
            }
            else if (sender.Equals(DdlCatType3))
            {
                child = DdlCat3;
                grndchild = DdlReport3;
            }
            child.DataBind();
            grndchild.DataBind();
        }

        protected void DdlCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList child = null;
            ddl.ToolTip = ddl.SelectedItem.Text;

            if (sender.Equals(DdlCatS))
                child = DdlReportS;
            else if (sender.Equals(DdlCat2))
                child = DdlReport2;
            else if (sender.Equals(DdlCat3))
                child = DdlReport3;

            child.DataBind();
        }

        protected void DdlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.ToolTip = ddl.SelectedItem.Text;

            if (ddl.Equals(DdlReportS))
            {
                DataRow row = DdlReport1DS.Select($"Report_Id = {DdlReportS.SelectedValue}")[0];
                string type = row[2].ToString().Trim();
                DdlTypeS.SelectedValue = type;

                bool isWeekly = type.Equals("W");
                DivWeek1.Visible = isWeekly;
                DdlWeekS.Enabled = isWeekly;

                string DueDate = row[3].ToString().Trim();
                TryParse(TxtMnthS.Text, out DateTime dt);

                if (dt != null && int.TryParse(DueDate, out int day))
                    DueDate = new DateTime(dt.Year, dt.Month, day).ToString(DateFormat);
                else
                    DueDate = "Every " + char.ToUpper(DueDate[0]) + DueDate.Substring(1).ToLower();

                TxtDueDtS.Text = DueDate;
            }
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

        #endregion MenuClick

        #region Add Task

        protected void TxtMnth_TextChanged(object sender, EventArgs e)
        {
            bool addMultpl = Menu1.Items[0].Selected && DivAddMultiple.Visible;
            try
            {
                TextBox TB = (TextBox)sender;
                DropDownList ddlWeek = addMultpl ? DdlWeekM : DdlWeekS;
                DropDownList ddlType = addMultpl ? DdlTypeM : DdlTypeS;
                TextBox txtDueDt = TxtDueDtS;

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

                ddlWeek.SelectedIndex = 0;
                ddlWeek.Items[5].Enabled = true;

                if (dt.Month == 2) //feb selected
                {
                    if (!IsLeapYear(dt.Year))  //if year is not a leap year, feb contains 28 days i.e. 4 weeks only
                        ddlWeek.Items[5].Enabled = false; //disable week no. 5
                }
                if (addMultpl)
                {
                    GVAdd.SelectedIndex = -1;
                    DivGVBtn.Visible = false;
                    if (dt < lstMnth)
                    {
                        TxtMnthM.Text = lstMnth.ToString(MonthFormat);
                    }
                    else if (dt > crntMnthLastDay)
                    {
                        TxtMnthM.Text = crntMnthLastDay.ToString(MonthFormat);
                    }
                }
                else if (ddlType.SelectedIndex == 1)
                {
                    string dueDt = new DateTime(dt.Year, dt.Month, Parse(txtDueDt.Text).Day).ToString("dd-MMM-yyyy");
                    txtDueDt.Text = dueDt;
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            LblError.Text = emp;
            if (BtnAdd.Text == "OK")
            {
                BtnCncl_Click(null, null);
                return;
            }
            if (ValidateReportDtls())
            {
                try
                {
                    string Report_Id = DdlReportS.SelectedValue;
                    string weekNo = DdlWeekM.SelectedValue;
                    if (!FileOprn(FUReport.PostedFile, TxtMnthS.Text, weekNo, DdlTypeS.SelectedValue, DdlReportS.SelectedItem.Text, out string fullPath, out string msg))
                    {
                        PopUp(msg);
                        return;
                    }

                    if (Menu1.Items[0].Text == "Edit Task |")
                    {
                        if (!UpdateTask("SP_Update_Task", fullPath, LblTaskID.Text)) return;
                        Menu1.Items[1].Selected = true;
                        Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[1]));
                        GVReports2.DataBind();
                        PopUp("Task updated successfully!");
                        LblError.Text = "Task updated successfully!";
                        //delete old file
                    }
                    else if (Menu1.Items[0].Text == "Add Task |")
                    {
                        if (!AddTaskToDB("SP_Add_Task", fullPath, Report_Id)) return;
                        PopUp("Task added successfully!");
                        LblError.Text = "Task added successfully!";
                    }
                    LblError.CssClass = "col-12 control-label text-success ";
                    ResetTab1();
                    Menu1.Items[0].Text = "Add Tasks |";
                }
                catch (Exception ex)
                {
                    PopUp(ex.Message);
                    LblError.Text = ex.Message;
                    LblError.CssClass = "col-12 control-label text-danger ";
                }
                LblError.Visible = true;
            }
        }

        private bool FileOprn(HttpPostedFile file, string txtMnth, string weekNo, string fileType, string rprtName, out string fullPath, out string msg)
        {
            fullPath = emp;
            msg = emp;
            try
            {
                string ext = Path.GetExtension(file.FileName).ToLower();

                HashSet<string> allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    { ".xls", ".xlsx", ".xlsm", ".xlsb", ".doc", ".docx", ".ppt", ".pptx", ".pdf", ".zip" };

                if (!allowedExtensions.Contains(ext))
                {
                    msg = "Please upload .xls, .xlsx, .xlsm, .xlsb, .doc, .docx, .ppt, .pptx, .pdf, .zip files only";
                    return false;
                }

                if (file.ContentLength == 0)
                {
                    msg = "Please do not enter empty file!";
                    return false;
                }
                string month = txtMnth.Split('-')[0].Trim();
                string year = txtMnth.Split('-')[1].Trim();
                string appFoldr = AppDomain.CurrentDomain.BaseDirectory;

                string saveFolder = appFoldr + $@"Upload\{year}\{month}";

                if (!Directory.Exists(saveFolder))
                    Directory.CreateDirectory(saveFolder);

                //string dueDt = DdlTypeS.SelectedValue == "M" ? TxtDueDtS.Text : "Week_" + DdlWeekS.SelectedValue + "_" + TxtMnthS.Text;
                //fullpath = saveFolder\[User_Name]_[Report_Name]_Due Date[dueDt]_Add Date[today]
                //fullPath = $@"{saveFolder}\[{Session["User_Name"]}]_[{DdlReportS.SelectedItem.Text}]_Due Date[{dueDt}]_Add Date[{today.ToString(DateFormat)}]{Path.GetExtension(file.FileName)}";

                string dueDt = fileType == "W" ? "_Week_" + weekNo : emp;
                fullPath = $@"{saveFolder}\{Session["User_Name"]}_{rprtName}{dueDt}_{today.ToString(DateFormat)}{Path.GetExtension(file.FileName)}";


                if (string.IsNullOrWhiteSpace(fullPath))
                {
                    msg = "Error occurred at FileOprn(HttpPostedFile file, string fileName, string fileType, string weekNo, out string fullPath, out string msg)";
                    return false;
                }
                file.SaveAs(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return false;
            }
        }

        private bool ValidateReportDtls()
        {
            if (DdlCatTypeS.SelectedIndex == 0)
            {
                PopUp("Please select a Category Type!");
                DdlCatTypeS.Focus();
                return false;
            }
            if (DdlCatS.SelectedIndex == 0)
            {
                PopUp("Please select a Category!");
                DdlCatS.Focus();
                return false;
            }
            if (DdlReportS.SelectedIndex == 0)
            {
                PopUp("Please select a Report Name!");
                DdlReportS.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(TxtMnthS.Text))
            {
                PopUp("Please select a month!");
                TxtMnthS.Focus();
                return false;
            }
            if (DdlTypeS.SelectedValue == "W" && DdlWeekS.SelectedValue == "0")
            {
                PopUp("Please select a week!");
                DdlTypeS.Focus();
                return false;
            }
            if (!FUReport.HasFile)
            {
                PopUp("Please upload a File!");
                FUReport.Focus();
                return false;
            }
            return true;
        }

        private bool AddTaskToDB(string proc, string fullPath, string rec_Id = "")
        {
            DateTime dt = Parse(TxtMnthS.Text);
            OleDbParameter[] paramCln = new OleDbParameter[]
            {
                new OleDbParameter("@User_Id", Session["User_Id"]),
                new OleDbParameter("@Report_Id", DdlReportS.SelectedValue),
                new OleDbParameter("@Add_Date", Now.Date.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_From_Date", dt.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_To_Date", new DateTime(dt.Year, dt.Month, DaysInMonth(dt.Year, dt.Month)).ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_Week_No", DdlWeekS.SelectedValue),
                new OleDbParameter("@Location", fullPath),
                new OleDbParameter("@Created_By", Session["User_Name"])
            };

            var output = DBOprn.ExecScalarProc(proc, DBOprn.ConnPrimary, paramCln);

            if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
            {
                PopUp(output.ToString());
                LblError.Text = output.ToString();
                LblError.CssClass = "col-12 control-label text-danger ";
                return false;
            }
            return true;
        }

        private bool UpdateTask(string proc, string fullPath, string rec_Id)
        {
            OleDbParameter[] paramCln = new OleDbParameter[]
            {
                new OleDbParameter("@User_Id", Session["User_Id"]),
                new OleDbParameter("@Report_Id", DdlReportS.SelectedValue),
                new OleDbParameter("@Add_Date", Now.Date.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_From_Date", Parse(TxtMnthS.Text)),
                new OleDbParameter("@Submit_To_Date", Parse(TxtMnthS.Text)),
                new OleDbParameter("@Submit_Week_No", DdlWeekS.SelectedValue),
                new OleDbParameter("@Location", fullPath),
                new OleDbParameter("@Created_By", Session["User_Name"]),
                new OleDbParameter("@Rec_Id", rec_Id)
            };
            var output = DBOprn.ExecScalarProc(proc, DBOprn.ConnPrimary, paramCln);

            if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
            {
                PopUp(output.ToString());
                LblError.Text = output.ToString();
                LblError.CssClass = "col-12 control-label text-danger ";
                return false;
            }
            return true;
        }

        #endregion Add Task

        protected void BtnView_Click(object sender, EventArgs e)
        {

            if (sender.Equals(BtnView2))
            {
                if (string.IsNullOrEmpty(TxtMnth2.Text) || !TryParse(TxtMnth2.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    PopUp("Please select a month!");
                    return;
                }
                GVReports2.DataSource = null;
                GVReports2DS = null;
                GVReports2.DataBind();
            }
            else if (sender.Equals(BtnView3))
            {
                if (string.IsNullOrEmpty(TxtMnth3.Text) || !TryParse(TxtMnth3.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    PopUp("Please select a month!");
                    return;
                }
                GVReports3.DataSource = null;
                GVReports3DS = null;
                GVReports3.DataBind();
            }
        }

        #region View Sbmt Task

        protected void TxtSD_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    DateTime sDate = Parse(TxtSD.Text);
            //    CalendarExtender3.StartDate = sDate;

            //    if (sDate > Parse(TxtED.Text))
            //        TxtED.Text = sDate.ToString(DateFormat);
            //}
            //catch (Exception ex)
            //{
            //    PopUp(ex.Message);
            //}
        }

        protected void GVReports2_DataBinding(object sender, EventArgs e)
        {
            try
            {
                //string strtDate = TxtMnth2.ToolTip.Split(',')[0];
                //string endDate = TxtMnth2.ToolTip.Split(',')[1];
                //string User_Id = Session["User_Id"]?.ToString();
                //string Role_Id = Session["Role_Id"]?.ToString();

                //OleDbParameter[] paramCln = new OleDbParameter[]
                //    {
                //        new OleDbParameter("@Start_Date", strtDate)
                //       ,new OleDbParameter("@End_Date", endDate)
                //       ,new OleDbParameter("@User_Id", User_Id)
                //       ,new OleDbParameter("@Role_Id", Role_Id)
                //       ,new OleDbParameter("@Category_Type_Id", DdlCatType2.SelectedValue)
                //       ,new OleDbParameter("@Category_Id", DdlCat2.SelectedValue)
                //       ,new OleDbParameter("@Report_Id", DdlReport2.SelectedValue)
                //       ,new OleDbParameter("@Type", DdlType2.SelectedValue)
                //    };
                //SetGV(paramCln, GVReports2);
                if (GVReports2.DataSource == null)
                {
                    DataTable dt = GVReports2DS;
                    GVReports2.DataSource = dt;
                    if (dt == null)
                    {
                        GVReports2.Visible = false;
                        PopUp("No data found!");
                    }
                    else
                        GVReports2.Visible = true;
                }
                BtnSubmit.Visible = GVReports2.Visible;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void CBSubmitH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVReports2.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chKCount += chked ? 1 : -1;
                }
            }
            if (GVReports2.Rows.Count < chKCount)
                chKCount = GVReports2.Rows.Count;
            else if (chKCount < 0)
                chKCount = 0;
            BtnSubmit.Enabled = chKCount > 0;
        }

        protected void CBSubmit_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chKCount += cb.Checked ? 1 : -1;
            if (GVReports2.Rows.Count < chKCount)
                chKCount = GVReports2.Rows.Count;
            else if (chKCount < 0)
                chKCount = 0;
            BtnSubmit.Enabled = chKCount > 0;
            GridViewRow row = GVReports2.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVReports2.Rows.Count == chKCount);
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string jsonParam = ConstructJSON();
            if (SubMission("SP_Submit_Tasks", jsonParam))
            {
                PopUp("Tasks submitted successfully!");
                GVReports2.DataSource = null;
                GVReports2DS = null;
                GVReports2.DataBind();
                chKCount = 0;
            };
        }

        private bool SubMission(string proc, string jsonParam)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonParam))
                    return false;

                var output = DBOprn.ExecScalarProc(proc, DBOprn.ConnPrimary,
                    new OleDbParameter[]
                    {
                        new OleDbParameter("@Collection", jsonParam)
                    }
                );
                if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                {
                    PopUp(output.ToString());
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return false;
            }
        }

        private string ConstructJSON()
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();

            foreach (GridViewRow gvRow in GVReports2.Rows)
            {
                if(chKCount == 0)  break;
                DataRow dRo = GVReports2DS.Select("Sno = " + gvRow.Cells[1].Text)?[0]; 
                if (dRo == null) continue;
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (cb.Checked)
                {
                    string id = dRo["Task_Id"].ToString();
                    string submitDt = Now.ToString(SqlDateFormat);
                    Dictionary<string, string> paramVals = new Dictionary<string, string>()
                        {
                            {
                                "REC_ID",
                                id
                            },
                            {
                                "SUBMIT_DATE",
                                submitDt
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
                    chKCount--;
                }
                continue;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        #endregion View Sbmt Task

        #region Edit Task

        //protected void BtnEdit_Command(object sender, CommandEventArgs e)
        //{
        //    if (e.CommandName == "EditRow")
        //    {
        //        DataTable ds = GVReports2DS;
        //        int rowIndex = ToInt32(e.CommandArgument);
        //        DataRow dRo = ds.Select($"Sno = {rowIndex - 1}")[0];
        //        GridViewRow row = GVReports2.Rows[rowIndex];

        //        string roleId = Session["Role_Id"]?.ToString();
        //        if (!string.IsNullOrWhiteSpace(roleId) && roleId == "1")
        //        {
        //            string catType = dRo["Category_Type"].ToString();
        //            DdlCatTypeS.SelectedValue = DdlCatTypeS.Items.FindByText(((Label)row.Cells[2].Controls[1]).Text)?.Value;

        //            DdlCatTypeS.ToolTip = DdlCatTypeS.SelectedItem.Text;
        //        }
        //        DdlCatS.Items.Add(new ListItem(row.Cells[3].Text, row.Cells[3].Text));
        //        DdlCatS.SelectedValue = row.Cells[3].Text;
        //        DdlCatS.ToolTip = DdlCatS.SelectedItem.Text;

        //        string reportId = ((Label)row.Cells[11].Controls[1]).Text;
        //        DdlReportS.Items.Add(new ListItem(row.Cells[4].Text, reportId));
        //        DdlReportS.SelectedValue = reportId;
        //        DdlReportS.ToolTip = DdlReportS.SelectedItem.Text;

        //        string type = row.Cells[8].Text.Trim();
        //        DdlTypeS.SelectedValue = DdlTypeS.Items.FindByText(type)?.Value;

        //        TxtDueDtS.Text = row.Cells[5].Text;

        //        DateTime toDate = Parse(((Label)row.Cells[14].Controls[1]).Text);
        //        TxtMnthS.Text = toDate.ToString(MonthFormat);

        //        if (type.ToUpper() == "WEEKLY")
        //        {
        //            DdlWeekS.SelectedValue = ((Label)row.Cells[15].Controls[1]).Text;
        //            DivWeek1.Visible = true;
        //            DdlWeekS.Enabled = false;
        //        }
        //        LnkReport.Text = ((HiddenField)row.Cells[9].Controls[1]).Value;
        //        DivLnk.Visible = true;

        //        BtnCncl.Visible = true;
        //        Menu1.Items[0].Selected = true;
        //        Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[0]));

        //        LblTaskID.Text = ((Label)row.Cells[12].Controls[1]).Text;

        //        DdlCatTypeS.Enabled = false;
        //        DdlCatS.Enabled = false;
        //        DdlReportS.Enabled = false;
        //        TxtMnthS.Enabled = false;

        //        LinkButton btn = (LinkButton)sender;
        //        if (btn.Text == "View")
        //        {
        //            BtnCncl.Visible = false;
        //            Menu1.Items[0].Text = "View Added Task |";
        //            FUReport.Enabled = false;
        //            //FUReport.Visible = false;
        //            BtnAdd.Text = "OK";
        //        }
        //        else if (btn.Text == "Edit")
        //        {
        //            BtnCncl.Visible = true;
        //            Menu1.Items[0].Text = "Edit Task |";
        //            FUReport.Enabled = true;
        //            //FUReport.Visible = true;
        //            BtnAdd.Text = "Edit";
        //        }
        //        DivAddSingl.Visible = true;
        //        DivAddMultiple.Visible = false;
        //    }
        //}

        protected void BtnEdit_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = ToInt32(e.CommandArgument);
                DataRow dRo = GVReports2DS.Select($"Sno = {rowIndex + 1}")[0];
                GridViewRow row = GVReports2.Rows[rowIndex];

                string roleId = Session["Role_Id"]?.ToString();
                if (!string.IsNullOrWhiteSpace(roleId) && roleId == "1")
                {
                    string catType = dRo["Category_Type"].ToString();
                    DdlCatTypeS.SelectedValue = DdlCatTypeS.Items.FindByText(catType)?.Value;

                    DdlCatTypeS.ToolTip = DdlCatTypeS.SelectedItem.Text;
                }
                string catNm = dRo["Category_Name"].ToString();
                DdlCatS.Items.Add(new ListItem(catNm, catNm));
                DdlCatS.SelectedValue = catNm;
                DdlCatS.ToolTip = DdlCatS.SelectedItem.Text;

                string reportId = dRo["Report_Id"].ToString();
                DdlReportS.Items.Add(new ListItem(dRo["Report_Name"].ToString(), reportId));
                DdlReportS.SelectedValue = reportId;
                DdlReportS.ToolTip = DdlReportS.SelectedItem.Text;

                string type = dRo["Type"].ToString().Trim();
                DdlTypeS.SelectedValue = DdlTypeS.Items.FindByText(type)?.Value;

                TxtDueDtS.Text = dRo["Due_Date"].ToString();

                DateTime toDate = Parse(dRo["To_Date"].ToString());
                TxtMnthS.Text = toDate.ToString(MonthFormat);

                if (type.ToUpper() == "WEEKLY")
                {
                    DdlWeekS.SelectedValue = dRo["Week_No"].ToString();
                    DivWeek1.Visible = true;
                    DdlWeekS.Enabled = false;
                }
                LnkReport.Text = dRo["Location"].ToString();
                DivLnk.Visible = true;

                BtnCncl.Visible = true;
                Menu1.Items[0].Selected = true;
                Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[0]));

                LblTaskID.Text = dRo["Task_Id"].ToString();

                DdlCatTypeS.Enabled = false;
                DdlCatS.Enabled = false;
                DdlReportS.Enabled = false;
                TxtMnthS.Enabled = false;

                LinkButton btn = (LinkButton)sender;
                if (btn.Text == "View")
                {
                    BtnCncl.Visible = false;
                    Menu1.Items[0].Text = "View Added Task |";
                    FUReport.Enabled = false;
                    //FUReport.Visible = false;
                    BtnAdd.Text = "OK";
                }
                else if (btn.Text == "Edit")
                {
                    BtnCncl.Visible = true;
                    Menu1.Items[0].Text = "Edit Task |";
                    FUReport.Enabled = true;
                    //FUReport.Visible = true;
                    BtnAdd.Text = "Edit";
                }
                DivAddSingl.Visible = true;
                DivAddMultiple.Visible = false;
            }
        }
        protected void BtnCncl_Click(object sender, EventArgs e)
        {
            ResetTab1();
            Menu1.Items[0].Text = "Add Tasks |";
            Menu1.Items[1].Selected = true;
            Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[1]));
        }

        protected void LnkReport_Click(object sender, EventArgs e)
        {
            string fullPath = emp;
            try
            {
                LinkButton lnkBtn = (LinkButton)sender;
                switch (lnkBtn.ID)
                {
                    case "LnkReportSbmt":
                    {
                        GridViewRow row = (GridViewRow)lnkBtn.NamingContainer;
                        fullPath = ((HiddenField)row.Cells[9].Controls[1]).Value.ToString();
                        break;
                    }
                    case "LnkReport":
                    {
                        fullPath = lnkBtn.Text;
                        break;
                    }
                }
                string fileName = Path.GetFileName(fullPath);

                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = emp;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", $"attachment;filename={fileName}");
                Response.TransmitFile(fullPath);
                Response.End();
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        #endregion Edit Task

        protected void GVReports3_DataBinding(object sender, EventArgs e)
        {
            try
            {
                //string User_Id = Session["User_Id"]?.ToString();
                //string Role_Id = Session["Role_Id"]?.ToString();
                //int mnthNo = 0, year = 0;
                //try
                //{
                //    DateTime dt = ParseExact(TxtMnth3.Text, MonthFormat, CultureInfo.InvariantCulture);
                //    mnthNo = dt.Month;
                //    year = dt.Year;
                //}
                //catch (Exception ex)
                //{ }

                //OleDbParameter[] paramCln = new OleDbParameter[]
                //{
                //    new OleDbParameter("@Start_Date", null)
                //   ,new OleDbParameter("@End_Date",  null)
                //   ,new OleDbParameter("@User_Id", User_Id)
                //   ,new OleDbParameter("@Role_Id", Role_Id)
                //   ,new OleDbParameter("@Category_Type_Id", DdlCatType3.SelectedValue)
                //   ,new OleDbParameter("@Category_Id", DdlCat3.SelectedValue)
                //   ,new OleDbParameter("@Report_Id", DdlReport3.SelectedValue)
                //   ,new OleDbParameter("@Type", DdlType3.SelectedValue)
                //   ,new OleDbParameter() {
                //       ParameterName = "@IsApproved",
                //       Value = 1,
                //       OleDbType = OleDbType.Boolean
                //   }
                //   ,new OleDbParameter("@ApprYearNo", year)
                //   ,new OleDbParameter("@ApprMonthNo", mnthNo)
                //};
                //SetGV(paramCln, GVReports3);

                if (GVReports3.DataSource == null)
                {
                    DataTable dt = GVReports3DS;
                    GVReports3.DataSource = dt;
                    if (dt == null)
                    {
                        GVReports3.Visible = false;
                        PopUp("No data found!");
                    }
                    else
                        GVReports3.Visible = true;
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        public void PopUp(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }


        //private void SetGV(OleDbParameter[] paramCln, GridView gv)
        //{
        //    DataTable dt = DBOprn.GetDataProc("SP_Get_Tasks", DBOprn.ConnPrimary, paramCln);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        gv.DataSource = dt;
        //        gv.Visible = true;
        //    }
        //    else
        //    {
        //        gv.Visible = false;
        //        PopUp("No data found!");
        //    }
        //}

        //private bool ParseTextMnth(string exp)
        //{
        //    int mnthNo = 0, year = 0;
        //    try
        //    {
        //        DateTime dt = ParseExact(exp, MonthFormat, CultureInfo.InvariantCulture);
        //        mnthNo = dt.Month;
        //        year = dt.Year;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    { return false; }
        //}
        //GVAdd.Rows.Cast<GridViewRow>().ToList().ForEach(row => ((FileUpload) row.Cells[6].FindControl("FUAdd")).Enabled = false);

        //            FileUpload FUAdd = ((FileUpload)slctRo.Cells[6].FindControl("FUAdd"));
        //FUAdd.Enabled = true;


        //TableCell cell = gvRow.Cells[8];
        //((HiddenField) cell.FindControl("Type")).Value = DdlTypeM.SelectedValue;
        //            ((HiddenField) cell.FindControl("FromDate")).Value = fromDt;
        //            ((HiddenField) cell.FindControl("ToDate")).Value = toDt;
        //            ((HiddenField) cell.FindControl("WeekNo")).Value = weekNo;
        //            ((HiddenField) cell.FindControl("DueDate")).Value = fromDt;
    }
}