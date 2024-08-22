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
using System.Linq;
using static System.DateTime;
using static System.Convert;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Finance_Tracker
{
    public partial class Performance : Page
    {
        private const string DateFormat = "dd-MMM-yyyy";
        private const string MonthFormat = "MMM-yyyy";
        private const string SqlDateFormat = "yyyy-MM-dd";
        private readonly string Emp = string.Empty;
        private static int chKCount = 0;
        private static int chKCountGVAdd = 0;
        private readonly DateTime today = Today;
        private readonly int crntYr = Today.Year;
        private readonly int crntMnth = Today.Month;
        private readonly DateTime crntMnthDay1 = new DateTime(Today.Year, Today.Month, 1);
        private readonly DateTime crntMnthLastDay = new DateTime(Today.Year, Today.Month, DaysInMonth(Today.Year, Today.Month));
        private readonly DateTime lstMnthDay1 = new DateTime(Today.Year, Today.Month, 1).AddMonths(-1);
        private string UsrId, UsrName, RoleId;

        private readonly DBOperations DBOprn = new DBOperations();

        private DataTable DdlReport1DS
        {
            get
            {
                DataTable dt = (DataTable)Session["Performance_DdlReport1DS"];
                if (!(dt?.Rows.Count > 0))
                {
                    try
                    {
                        dt = DBOprn.GetDataProc("SP_Report_Get", DBOprn.ConnPrimary,
                                new OleDbParameter[]
                                {
                                    new OleDbParameter("@Category_Id", DdlCatS.SelectedValue)
                                }
                            );
                        if (dt.Rows.Count == 0)
                            dt = null;
                    }
                    catch (Exception ex)
                    {
                        PopUp(ex.Message);
                    }
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
                    try
                    {
                        string strtDate = TxtMnth3.ToolTip.Split(',')[0];
                        string endDate = TxtMnth3.ToolTip.Split(',')[1];
                        string User_Id = UsrId;

                        OleDbParameter[] paramCln = new OleDbParameter[]
                        {
                             new OleDbParameter("@Start_Date", strtDate)
                            ,new OleDbParameter("@End_Date",  endDate)
                            ,new OleDbParameter("@User_Id", User_Id)
                            ,new OleDbParameter("@Category_Type_Id", DdlCatType3.SelectedValue)
                            ,new OleDbParameter("@Category_Id", DdlCat3.SelectedValue)
                            ,new OleDbParameter("@Report_Id", DdlReport3.SelectedValue)
                            ,new OleDbParameter("@TypeId", DdlType3.SelectedValue)
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
                    catch (Exception ex)
                    {
                        PopUp(ex.Message);
                    }
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
                    try
                    {
                        string strtDate = TxtMnth2.ToolTip.Split(',')[0];
                        string endDate = TxtMnth2.ToolTip.Split(',')[1];
                        string User_Id = UsrId;

                        OleDbParameter[] paramCln = new OleDbParameter[]
                        {
                            new OleDbParameter("@Start_Date", strtDate)
                           ,new OleDbParameter("@End_Date", endDate)
                           ,new OleDbParameter("@User_Id", User_Id)
                           ,new OleDbParameter("@Category_Type_Id", DdlCatType2.SelectedValue)
                           ,new OleDbParameter("@Category_Id", DdlCat2.SelectedValue)
                           ,new OleDbParameter("@Report_Id", DdlReport2.SelectedValue)
                           ,new OleDbParameter("@TypeId", DdlType2.SelectedValue)
                         };
                        dt = DBOprn.GetDataProc("SP_Get_Tasks", DBOprn.ConnPrimary, paramCln);
                        if (dt.Rows.Count == 0)
                            dt = null;
                    }
                    catch (Exception ex)
                    {
                        PopUp(ex.Message);
                    }
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
                    try
                    {
                        string fromDt = TxtMnthM.ToolTip.Split(',')[0];
                        string toDt = TxtMnthM.ToolTip.Split(',')[1];
                        string rptTypeCmp = DdlTypeM.SelectedItem.Text.ToUpper();
                        string weekNo = rptTypeCmp.Equals("WEEKLY") ? DdlWeekM.SelectedValue : "0";
                        string halfNo = rptTypeCmp.Equals("HALF YEARLY") ? DdlHY.SelectedValue : "0";

                        dt = DBOprn.GetDataProc("SP_Get_UserTasks", DBOprn.ConnPrimary
                                , new OleDbParameter[]
                                 {
                                      new OleDbParameter("@User_Id", UsrId)
                                     ,new OleDbParameter("@From_Date", fromDt)
                                     ,new OleDbParameter("@To_Date", toDt)
                                     ,new OleDbParameter("@ReportTypeId", DdlTypeM.SelectedValue)
                                     ,new OleDbParameter("@WeekNo", weekNo)
                                     ,new OleDbParameter("@Quarter_No", DdlQrtrM.SelectedValue)
                                     ,new OleDbParameter("@Half_No", halfNo)
                                 }
                             );
                        if (dt.Rows.Count == 0)
                            dt = null;
                        Session["Performance_GVAddDS"] = dt;
                    }
                    catch (Exception ex)
                    {
                        PopUp(ex.Message);
                    }
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
            try
            {
                UsrId = Session["User_Id"]?.ToString().Trim().ToUpper();
                if (UsrId == null || UsrId == Emp)
                {
                    Response.Redirect("~/Account/Login");
                    return;
                }
                UsrName = Session["User_Name"].ToString().Trim();
                RoleId = Session["Role_Id"]?.ToString().Trim();
                if (!Page.IsPostBack)
                {
                    foreach (TextBox tb in new TextBox[] { TxtMnthM, TxtMnth2, TxtMnth3, TxtCmnts })
                    {
                        tb.Attributes.Add("autocomplete", "off");
                    }
                    foreach (DropDownList ddl in new DropDownList[] { DdlType2, DdlType3, DdlTypeM, DdlTypeS, DdlCatType2, DdlCat2, DdlReport2, DdlCatType3, DdlCat3, DdlReport3 })
                    {
                        ddl.DataBind();
                    }
                    Menu1_MenuItemClick(Menu1, new MenuEventArgs(Menu1.Items[0]));
                    chKCount = 0;
                    chKCountGVAdd = 0;
                    CETxtMnthM.StartDate = lstMnthDay1;
                    CETxtMnthM.EndDate = crntMnthLastDay;
                    //DdlTypeM.Items.FindByValue("Half Yearly").Enabled = false;
                    //if (crntMnth == 1 || lstMnthDay1.Month == 1)
                    //{
                    //    DdlTypeM.Items.FindByValue("Half Yearly").Enabled = true;
                    //    DdlHY.Items.FindByValue("2").Enabled=false;
                    //}
                    //else
                    //if (crntMnth == 7 || lstMnthDay1.Month == 8)
                    //{
                    //    DdlTypeM.Items.FindByValue("Half Yearly").Enabled = true;
                    //    DdlHY.Items.FindByValue("1").Enabled = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
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
            try
            {
                int eval = e is null ? 0 : int.Parse(e.Item.Value);
                MultiView1.ActiveViewIndex = eval;
                MultiView1.Views[eval].Focus();

                LblError.Text = Emp;

                //User Clicked the menu item
                if (sender != null)
                {
                    BtnSubmit.Visible = false;
                    //ResetTab1();
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
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        #region DivAddMultiple

        private void SetDivAddMultiple()
        {
            Menu1.Items[0].Text = "Add Tasks |";
            DdlTypeM.SelectedIndex = 0;
            DdlWeekM.SelectedValue = "0";
            DdlHY.SelectedValue = "0";
            SetTooltip(DdlTypeM);
            TxtMnthM.Text = Emp;
            DivWeekM.Visible = false;
            DvHY.Visible = false;
            DivGVBtnM.Visible = false;
            DvQrtrM.Visible = false;
            DdlQrtrM.SelectedIndex = 0;
            GVAdd.SelectedIndex = -1;
        }

        protected void DdlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(DdlTypeM))
                {
                    DivGVBtnM.Visible = false;
                    DivWeekM.Visible = false;
                    DvHY.Visible = false;
                    DvQrtrM.Visible = false;

                    switch (DdlTypeM.SelectedItem.Text.ToUpper().Trim())
                    {
                        case "WEEKLY":
                            {
                                DivWeekM.Visible = true;
                                DdlWeekM.SelectedValue = "0";
                                break;
                            }
                            //case "QUARTERLY":
                            //    {
                            //        DvQrtrM.Visible = true;
                            //        DdlQrtrM.SelectedValue = "0";
                            //        break;
                            //    }
                            //case "HALF YEARLY":
                            //    {
                            //        DvHY.Visible = true;
                            //        DdlHY.SelectedValue = "0";
                            //        break;
                            //    }
                            //case "ANNUAL":
                            //    {
                            //        break;
                            //    }
                    }
                }
                SetTooltip((DropDownList)sender);
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnViewAssTask_Click(object sender, EventArgs e)
        {
            DivGVBtnM.Visible = false;
            string typ = DdlTypeM.SelectedItem.Text.Trim().ToUpper();

            if (TxtMnthM.Text == Emp)
            {
                PopUp("Please select a Month!");
                TxtMnthM.Focus();
                return;
            }
            if (!TryParseExact(TxtMnthM.Text, MonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                //PopUp("Please enter month in correct format like 'Jan-2024'!");
                TxtMnthM.Focus();
                return;
            }
            else
            {
                TxtMnthM.ToolTip = dt.ToString(SqlDateFormat) + "," + new DateTime(dt.Year, dt.Month, DaysInMonth(dt.Year, dt.Month)).ToString(SqlDateFormat);
            }
            switch (typ)
            {
                case "":
                    {
                        PopUp("Please select Task type!");
                        DdlTypeM.Focus();
                        return;
                    }
                case "WEEKLY":
                    {
                        if (DdlWeekM.SelectedValue == "0")
                        {
                            PopUp("Please select a Week!");
                            DdlWeekM.Focus();
                            return;
                        }
                        break;
                    }
                    //case "QUARTERLY":
                    //    {
                    //        if (DdlQrtrM.SelectedValue == "0")
                    //        {
                    //            PopUp("Please select a quarter no.!");
                    //            DdlQrtrM.Focus();
                    //            return;
                    //        }
                    //        break;
                    //    }
                    //case "HALF YEARLY":
                    //    {
                    //        if (DdlHY.SelectedValue == "0")
                    //        {
                    //            PopUp("Please select a half no.!");
                    //            DdlHY.Focus();
                    //            return;
                    //        }
                    //        break;
                    //    }
                    //case "ANNUAL":
                    //    {
                    //        break;
                    //    }
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
                        DivGVBtnM.Visible = true;
                    }
                    else
                    {
                        DivGVBtnM.Visible = false;
                        PopUp("No data found!");
                    }
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
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

            string dueDt = dRo["Due_Date"].ToString();
            DateTime dt;
            int year = crntYr;
            if (int.TryParse(dueDt, out int day))
            {
                try
                {
                    dt = ParseExact(TxtMnthM.Text, MonthFormat, CultureInfo.InvariantCulture);
                    int mnthNo = dt.Month;
                    year = dt.Year;
                }
                catch (Exception ex)
                { }
                DivWeekM.Visible = false;
            }
            else
            {
                DivWeekM.Visible = true;
            }
        }

        protected void BtnAddM_Click(object sender, EventArgs e)
        {
            if (DdlTypeM.SelectedValue == "0")
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
            if (DdlTypeM.SelectedValue == "Weekly" && DdlWeekM.SelectedValue == "0")
            {
                PopUp("Please select a week!");
                DdlWeekM.Focus();
                return;
            }
            string jsonParam = ConstructJSON_M();
            //if (chkCnt == 0) PopUp("Please check any row to add!");

            if (SubMission("SP_Add_Multiple_Tasks", jsonParam))
            {
                PopUp("Tasks added successfully!");
                GVAddDS = null;
                GVAdd.DataSource = null;
                GVAdd.DataBind();
                chKCountGVAdd = 0;
            };
        }

        private string ConstructJSON_M()
        {
            string jsonString = Emp;
            int chkCnt = 0;
            GVAdd.Rows.Cast<GridViewRow>().ToList().ForEach(gvRow => chkCnt += ((CheckBox)gvRow.Cells[0].Controls[1]).Checked ? 1 : 0);

            if (chkCnt == 0)
            {
                PopUp("Please check any row to add!");
                return jsonString;
            }
            DataTable dSrc = GVAddDS;
            if (dSrc == null || dSrc.Rows.Count == 0) return jsonString;

            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();
            foreach (GridViewRow gvRow in GVAdd.Rows)
            {
                if (chkCnt < 1) break;

                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (!cb.Checked) continue;

                int sno = ToInt32(gvRow.Cells[1].Text);
                DataRow dRo = dSrc.Select($"sno = {sno}")?[0];

                if (dRo == null) return jsonString;

                string fromDt = TxtMnthM.ToolTip.Split(',')[0];
                string toDt = TxtMnthM.ToolTip.Split(',')[1];
                string reportName = dRo["Task_Name"].ToString();
                string rptType = dRo["Type"].ToString().Trim();
                string rptTypeCmp = rptType.ToUpper();
                string fileType = DdlTypeM.SelectedItem.Text.Trim().ToUpper();
                string addDt = Now.ToString(SqlDateFormat);

                string weekNo = "0", halfNo = "0", qrtrNo = "0", dueDt = "";

                Label LblRoErr = (Label)gvRow.Cells[7].FindControl("LblRoErr");

                dynamic fuAdd = (FileUpload)gvRow.Cells[6].FindControl("FUAdd");
                if (!fuAdd.HasFile)
                {
                    LblRoErr.Text = "Please upload a File!";
                    LblRoErr.CssClass = "control-label text-danger ";
                    cb.Checked = false;
                    chkCnt--;
                    continue;
                }
                else
                    LblRoErr.Text = Emp;

                switch (fileType)
                {
                    case "WEEKLY":
                        {
                            weekNo = DdlWeekM.SelectedValue;
                            dueDt = "_Week_" + weekNo;
                            break;
                        }
                    case "QUARTERLY":
                        {
                            //qrtrNo = DdlQrtrM.SelectedValue;
                            //dueDt = "_Quarter_" + qrtrNo;
                            dueDt = "_Quarter";
                            break;
                        }
                    case "HALF YEARLY":
                        {
                            //halfNo = DdlHY.SelectedValue;
                            //dueDt = "_Half_" + halfNo;
                            dueDt = "_Half";
                            break;
                        }
                    case "ANNUAL":
                        {
                            dueDt = "_Annual";
                            break;
                        }
                }
                if (!FileOprn(fuAdd.PostedFile, TxtMnthM.Text, weekNo, DdlTypeM.SelectedValue, reportName, out string fullPath, out string msg, dueDt))
                {
                    LblRoErr.Text = msg;
                    LblRoErr.CssClass = "control-label text-danger ";
                    cb.Checked = false;
                    chkCnt--;
                    continue;
                }
                else
                    LblRoErr.Text = Emp;

                Dictionary<string, string> paramVals = new Dictionary<string, string>()
                {
                    {"USER_ID", UsrId},
                    {"REPORT_ID", dRo["ReportId"]?.ToString()},
                    {"REPORT_TYPE", rptType},
                    {"ADD_DATE", addDt},
                    {"MONTH_FROM_DATE", fromDt},
                    {"MONTH_TO_DATE", toDt},
                    {"MONTH_WEEK_NO", weekNo},
                    {"YEAR_QUARTER_NO", qrtrNo},
                    {"YEAR_HALF_NO", halfNo},
                    {"LOCATION", fullPath},
                    {"CREATED_BY", UsrName}
                };
                dtls.Add(paramVals);
                cb.Checked = false;
                chkCnt--;
                LblRoErr.Text = "Success";
                LblRoErr.CssClass = "control-label text-success ";
            }
            jsonString = dtls.Count > 0 ? JsonConvert.SerializeObject(dtls, Formatting.Indented) : Emp;
            return jsonString;
        }

        #endregion DivAddMultiple

        private void ResetTab1()
        {
            //enable cat type for admin user only
            if (!string.IsNullOrWhiteSpace(RoleId) && RoleId == "1")
            {
                DdlCatTypeS.SelectedIndex = 0;
                DdlCatType_SelectedIndexChanged(DdlCatTypeS, new EventArgs());
                DdlCatTypeS.Enabled = true;
            }
            DdlCatS.SelectedIndex = 0;
            DdlCatS.Enabled = true;

            //DdlReportS.DataBind();
            DdlReportS.SelectedIndex = 0;
            DdlReportS.Enabled = true;

            TxtMnthS.Text = Now.ToString(MonthFormat);
            TxtMnthS.Enabled = true;

            CalendarExtender1.StartDate = lstMnthDay1;
            CalendarExtender1.EndDate = crntMnthLastDay;

            DdlWeekS.SelectedIndex = 0;
            DdlWeekS.Enabled = true;
            DivWeek1.Visible = false;
            DvHyS.Visible = false;

            DdlQrtrS.SelectedIndex = 0;
            DdlQrtrS.Enabled = true;
            DvQrtrS.Visible = false;

            DdlTypeS.SelectedIndex = 0;
            TxtDueDtS.Text = Emp;
            BtnAdd.Text = "Add";
            LnkReport.Text = Emp;
            DivLnk.Visible = false;
            BtnCncl.Visible = false;
            SetTooltip(null, new DropDownList[] { DdlTypeS, DdlCatS });
        }

        private void SetTooltip(DropDownList ddl = null, DropDownList[] ddlLst = null)
        {
            try
            {
                if (!(ddl is null))
                    ddl.ToolTip = ddl.SelectedItem.Text;
                ddlLst?.ToList().ForEach(itm => itm.ToolTip = itm.SelectedItem.Text);
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        private void ResetTab2()
        {
            GVReports2.DataSource = null;
            GVReports2.Visible = false;
            GVReports2DS = null;

            BtnSubmit.Visible = false;
            TxtMnth2.Text = TextBoxWatermarkExtender2.WatermarkText;
            DdlType2.SelectedIndex = 0;
            SetTooltip(DdlType2);
            //CalendarExtender2.StartDate = lstMnthDay1;
            //CalendarExtender2.EndDate = crntMnthLastDay;

        }

        private void ResetTab3()
        {
            GVReports3.DataSource = null;
            GVReports3.Visible = false;
            GVReports3DS = null;

            TxtMnth3.Text = TextBoxWatermarkExtender3.WatermarkText;
            DdlType3.SelectedIndex = 0;
            SetTooltip(DdlType3);
        }

        private void SetCatType(DropDownList ddl, DropDownList childDdl)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(RoleId) && RoleId != "1")
                {
                    string catType = RoleId == "2" ? "Corporate" : RoleId == "3" ? "Plant" : Emp;
                    catType = ddl.Items.FindByText(catType)?.Value;

                    ddl.SelectedValue = catType;
                    SetTooltip(ddl);
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
            FillDdl(ddl, "SP_Get_CategoryTypes", "0", "All");
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
            FillDdl(ddl, "SP_Get_Categories", "0", "All", null,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Type_Id", prntddl.SelectedValue)
                }
            );
        }

        protected void DdlReport_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = null, grndprntDdl = null;

            if (ddl.Equals(DdlReportS))
            {
                prntddl = DdlCatS;
                grndprntDdl = DdlCatTypeS;
            }
            else if (ddl.Equals(DdlReport2))
            {
                prntddl = DdlCat2;
                grndprntDdl = DdlCatType2;
            }
            else if (ddl.Equals(DdlReport3))
            {
                prntddl = DdlCat3;
                grndprntDdl = DdlCatType3;
            }

            FillDdl(ddl, "SP_Report_Get", "0", "All", null,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Category_Id", prntddl.SelectedValue)
                   ,new OleDbParameter("@Category_Type_Id", grndprntDdl.SelectedValue)
                }, "Report_Name", "Report_Id"
            );
        }

        protected void DdlCatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList child = null;
            DropDownList grndchild = null;
            DropDownList ddl = (DropDownList)sender;
            ddl.ToolTip = ddl.SelectedItem.Text;
            if (ddl.Equals(DdlCatTypeS))
            {
                child = DdlCatS;
                grndchild = DdlReportS;
            }
            else if (ddl.Equals(DdlCatType2))
            {
                child = DdlCat2;
                grndchild = DdlReport2;
            }
            else if (ddl.Equals(DdlCatType3))
            {
                child = DdlCat3;
                grndchild = DdlReport3;
            }
            child.DataBind();
            grndchild.DataBind();
            ddl.ToolTip = ddl.SelectedItem.Text;
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
                SetTooltip(DdlTypeS);

                bool isWeekly = type.ToUpper().Equals("WEEKLY");
                DivWeek1.Visible = isWeekly;
                DdlWeekS.Enabled = isWeekly;

                bool isHfYrly = type.ToUpper().Equals("HALF YEARLY");
                DvHyS.Visible = isHfYrly;
                DdlHyS.Enabled = isHfYrly;

                string DueDate = row[3].ToString().Trim();
                TryParse(TxtMnthS.Text, out DateTime dt);

                if (dt != null && int.TryParse(DueDate, out int day))
                    DueDate = new DateTime(dt.Year, dt.Month, day).ToString(DateFormat);
                else
                    DueDate = "Every " + char.ToUpper(DueDate[0]) + DueDate.Substring(1).ToLower();

                TxtDueDtS.Text = DueDate;
            }
        }

        private void FillDdl(DropDownList ddl, string proc, string selectVal, string selectTxt = "All", DropDownList prntDdl = null, OleDbParameter[] paramCln = null, string TxtField = "", string ValField = "")
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(selectTxt, selectVal));
            ddl.SelectedValue = selectVal;
            ddl.ToolTip = selectTxt;

            if (prntDdl != null && prntDdl.SelectedIndex == 0)
                return;

            try
            {
                DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if ((TxtField != Emp) && ValField != Emp)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl.Items.Add(new ListItem(dt.Rows[i][TxtField].ToString(), dt.Rows[i][ValField].ToString()));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                        }
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
                    //PopUp("Please enter month in correct format like Jan-2024");
                    TB.Text = TextBoxWatermarkExtender1.WatermarkText;
                    return;
                }

                int yr = dt.Year, mnth = dt.Month;
                DateTime strtDt = new DateTime(yr, mnth, 01);
                string fromDt = strtDt.ToString(SqlDateFormat);
                string toDt = strtDt.AddDays(DaysInMonth(yr, mnth) - 1).ToString(SqlDateFormat);
                TB.ToolTip = fromDt + "," + toDt;

                ddlWeek.SelectedIndex = 0;
                SetTooltip(ddlWeek);
                ddlWeek.Items[5].Enabled = true;

                if (dt.Month == 2) //feb selected
                {
                    if (!IsLeapYear(dt.Year))  //if year is not a leap year, feb contains 28 days i.e. 4 weeks only
                        ddlWeek.Items[5].Enabled = false; //disable week no. 5
                }
                if (addMultpl)
                {
                    GVAdd.SelectedIndex = -1;
                    DivGVBtnM.Visible = false;

                    if (dt < lstMnthDay1)
                    {
                        TxtMnthM.Text = lstMnthDay1.ToString(MonthFormat);
                    }
                    else if (dt > crntMnthLastDay)
                    {
                        TxtMnthM.Text = crntMnthLastDay.ToString(MonthFormat);
                    }
                }
                else if (ddlType.SelectedIndex == 1)
                {
                    string dueDt = new DateTime(yr, mnth, Parse(txtDueDt.Text).Day).ToString("dd-MMM-yyyy");
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
            LblError.Text = Emp;
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
                    string weekNo = DdlWeekS.SelectedValue;
                    string fileType = DdlTypeS.SelectedValue;
                    string dueDt = Emp;
                    //fileType == "Weekly" ? "_Week_" + weekNo : Emp;
                    switch (DdlTypeS.SelectedItem.Text.Trim().ToUpper())
                    {
                        case "WEEKLY":
                            {
                                dueDt = "_Week_" + DdlWeekS.SelectedValue;
                                break;
                            }
                        case "QUARTERLY":
                            {
                                //dueDt = "_Quarter_" + DdlQrtrS.SelectedValue;
                                dueDt = "_Quarter";
                                break;
                            }
                        case "HALF YEARLY":
                            {
                                //dueDt = "_Half_" + DdlHyS.SelectedValue;
                                dueDt = "_Half";
                                break;
                            }
                        case "ANNUAL":
                            {
                                dueDt = "_Annual";
                                break;
                            }
                    }
                    if (!FileOprn(FUReport.PostedFile, TxtMnthS.Text, weekNo, fileType, DdlReportS.SelectedItem.Text, out string fullPath, out string msg, dueDt))
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
                    //ResetTab1();
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

        private bool FileOprn(HttpPostedFile file, string txtMnth, string weekNo, string fileType, string rprtName, out string fullPath, out string msg, string dueDt = "")
        {
            fullPath = Emp;
            msg = Emp;
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

                // Get invalid characters in paths
                char[] invalidPathChars = Path.GetInvalidPathChars();
                saveFolder = new string(saveFolder.Select(chr => invalidPathChars.Contains(chr) ? '_' : chr).ToArray());

                if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);

                // Get invalid characters in file names
                char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

                string fileName = $"{UsrName}_{rprtName}{dueDt}_{today.ToString(DateFormat)}{Path.GetExtension(file.FileName)}";

                fileName = new string(fileName.Select(chr => invalidFileNameChars.Contains(chr) ? '_' : chr).ToArray());

                // Combine base path and sanitized file name
                fullPath = $@"{saveFolder}\{fileName}";

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
            if (DdlCatTypeS.SelectedValue == "0")
            {
                PopUp("Please select a Category Type!");
                DdlCatTypeS.Focus();
                return false;
            }
            if (DdlCatS.SelectedValue == "0")
            {
                PopUp("Please select a Category!");
                DdlCatS.Focus();
                return false;
            }
            if (DdlReportS.SelectedValue == "0")
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
            if (DdlTypeS.SelectedValue == "Weekly" && DdlWeekS.SelectedValue == "0")
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
                new OleDbParameter("@User_Id", UsrId),
                new OleDbParameter("@Report_Id", DdlReportS.SelectedValue),
                new OleDbParameter("@Add_Date", Now.Date.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_From_Date", dt.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_To_Date",
                                    new DateTime(dt.Year, dt.Month, DaysInMonth(dt.Year, dt.Month)).ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_Week_No", DdlWeekS.SelectedValue),
                new OleDbParameter("@Submit_Half_No", DdlHyS.SelectedValue),
                new OleDbParameter("@Location", fullPath),
                new OleDbParameter("@Created_By", UsrName)
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
                new OleDbParameter("@User_Id", UsrId),
                new OleDbParameter("@Report_Id", DdlReportS.SelectedValue),
                new OleDbParameter("@Add_Date", Now.Date.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_From_Date", Parse(TxtMnthS.Text)),
                new OleDbParameter("@Submit_To_Date", Parse(TxtMnthS.Text)),
                new OleDbParameter("@Submit_Week_No", DdlWeekS.SelectedValue),
                new OleDbParameter("@Submit_Half_No", DdlHyS.SelectedValue),
                new OleDbParameter("@Location", fullPath),
                new OleDbParameter("@Created_By", UsrName),
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

        protected void GVReports2_DataBinding(object sender, EventArgs e)
        {
            try
            {
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
                    {
                        GVReports2.Visible = true;

                        int slctdMnth = ToInt16(TxtMnth2.ToolTip.Split(',')[0].Split('-')[1]);
                        bool showSbmt = slctdMnth == crntMnth || slctdMnth == lstMnthDay1.Month;
                        GVReports2.Columns[0].Visible = showSbmt;
                        BtnSubmit.Visible = showSbmt;
                    }
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
            try
            {
                string jsonParam = ConstructJSON();
                if (SubMission("SP_Submit_Tasks", jsonParam))
                {
                    PopUp("Tasks submitted successfully!");
                    GVReports2.DataSource = null;
                    GVReports2DS = null;
                    GVReports2.DataBind();
                    chKCount = 0;
                    BtnSubmit.Enabled = false;
                };
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
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

            int chkCnt = 0;
            GVReports2.Rows.Cast<GridViewRow>().ToList().ForEach(gvRow => chkCnt += ((CheckBox)gvRow.Cells[0].Controls[1]).Checked ? 1 : 0);

            foreach (GridViewRow gvRow in GVReports2.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (!cb.Checked) continue;

                if (chkCnt < 1) break;
                DataRow dRo = GVReports2DS.Select("Sno = " + gvRow.Cells[2].Text)?[0];
                if (dRo == null) continue;

                string id = dRo["Task_Id"].ToString();
                string submitDt = Now.ToString(SqlDateFormat);
                Dictionary<string, string> paramVals = new Dictionary<string, string>()
                {
                    { "REC_ID", id },
                    { "SUBMIT_DATE", submitDt },
                    { "MODIFIED_BY", UsrName },
                    { "MODIFIED_DATE", Now.ToString("yyyy-MM-dd HH:mm:ss.fff") }
                };
                dtls.Add(paramVals);
                cb.Checked = false;
                chkCnt--;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        #endregion View Sbmt Task

        #region Edit Task

        protected void BtnEdit_Command(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "EditRow") return;

                int rowIndex = ToInt32(e.CommandArgument);
                DataRow dRo = GVReports2DS.Select($"Sno = {rowIndex + 1}")[0];
                GridViewRow row = GVReports2.Rows[rowIndex];

                string catTypID = dRo["CatTypeId"].ToString();
                DdlCatTypeS.Items?.Clear();
                DdlCatTypeS.Items.Add(new ListItem(dRo["Category_Type"].ToString(), catTypID));
                DdlCatTypeS.SelectedValue = catTypID;

                string catId = dRo["CatId"].ToString();
                DdlCatS.Items?.Clear();
                DdlCatS.Items.Add(new ListItem(dRo["Category_Name"].ToString(), catId));
                DdlCatS.SelectedValue = catId;

                string reportId = dRo["Report_Id"].ToString();
                DdlReportS.Items?.Clear();
                DdlReportS.Items.Add(new ListItem(dRo["Report_Name"].ToString(), reportId));
                DdlReportS.SelectedValue = reportId;

                string type = dRo["Type"].ToString().Trim();
                DdlTypeS.SelectedValue = DdlTypeS.SelectedValue = DdlTypeS.Items.FindByText(type)?.Value;

                TxtDueDtS.Text = dRo["Due_Date"].ToString();

                DateTime toDate = Parse(dRo["To_Date"].ToString());
                TxtMnthS.Text = toDate.ToString(MonthFormat);

                DvUpload.Attributes["class"] = "col-sm-3";

                string cmnts = dRo["Comments"]?.ToString();
                bool dvVsbl = !string.IsNullOrWhiteSpace(cmnts);
                DivCmnts.Visible = dvVsbl;
                TxtCmnts.Text = cmnts;
                TxtCmnts.ToolTip = cmnts;

                switch (DdlTypeS.SelectedItem.Text.ToUpper())
                {
                    case "WEEKLY":
                        {
                            DdlWeekS.SelectedValue = dRo["Week_No"].ToString();
                            DivWeek1.Visible = true;
                            DdlWeekS.Enabled = false;
                            if (dvVsbl) DvUpload.Attributes["class"] = "col-sm-2";
                            break;
                        }
                        //case "QUARTERLY":
                        //    {
                        //        DdlQrtrS.SelectedValue = dRo["Quarter_No"].ToString();
                        //        DvQrtrS.Visible = true;
                        //        DdlQrtrS.Enabled = false;
                        //        if (dvVsbl) DvUpload.Attributes["class"] = "col-sm-2";
                        //        break;
                        //    }
                        //case "HALF YEARLY":
                        //    {
                        //        DdlHyS.SelectedValue = dRo["Half_No"].ToString();
                        //        DvHyS.Visible = true;
                        //        DdlHyS.Enabled = false;
                        //        if (dvVsbl) DvUpload.Attributes["class"] = "col-sm-2";
                        //        break;
                        //    }
                        //case "ANNUAL":
                        //    {
                        //        if (dvVsbl) DvUpload.Attributes["class"] = "col-sm-2";
                        //        break;
                        //    }
                }
                LnkReport.Text = Path.GetFileName(dRo["Location"]?.ToString());
                DivLnk.Visible = true;

                BtnCncl.Visible = true;
                Menu1.Items[0].Selected = true;
                Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[0]));

                LblTaskID.Text = dRo["Task_Id"]?.ToString();

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
                    FUReport.Visible = false;
                    BtnAdd.Text = "OK";
                    LblFU.Visible = false;
                }
                else if (btn.Text == "Edit")
                {
                    BtnCncl.Visible = true;
                    Menu1.Items[0].Text = "Edit Task |";
                    FUReport.Enabled = true;
                    FUReport.Visible = true;
                    BtnAdd.Text = "Save";
                    LblFU.Visible = true;
                }
                DivAddSingl.Visible = true;
                DivAddMultiple.Visible = false;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
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
            string fullPath = Emp;
            try
            {
                LinkButton lnkBtn = (LinkButton)sender;
                switch (lnkBtn.ID)
                {
                    case "LnkReportSbmt":
                        {
                            GridViewRow row = (GridViewRow)lnkBtn.NamingContainer;
                            fullPath = GVReports3DS.Select("Sno=" + row.RowIndex + 1)?[0]?["Location"].ToString();
                            break;
                        }
                    case "LnkReport":
                        {
                            fullPath = GVReports2DS.Select("Task_Id=" + LblTaskID.Text)?[0]?["Location"]?.ToString();
                            break;
                        }
                    case "LBGVReprts":
                        {
                            dynamic ctrl = lnkBtn.NamingContainer;
                            fullPath = GVReports2DS?.Rows[ctrl.RowIndex]?["Location"]?.ToString();
                            break;
                        }
                }
                string fileName = Path.GetFileName(fullPath);

                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = Emp;
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

        public void PopUp(string msg)
        {
            msg.Replace("'", Emp);
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + msg + "');", true);
        }

        protected void GVReports3_DataBinding(object sender, EventArgs e)
        {
            try
            {
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

        protected void DdlType_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string slctTxt = ddl.Equals(DdlTypeM) || ddl.Equals(DdlTypeS) ? "Select" : "All";

            FillDdl((DropDownList)sender, "SP_ReportTypes_Get", Emp, slctTxt, null, null, "TypeName", "TypeId");
        }

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


        //private void FillDdl(DropDownList ddl, String proc, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        //{
        //    ddl.Items.Clear();
        //    ddl.Items.Add(new ListItem("All", selectVal));
        //    ddl.SelectedIndex = 0;
        //    ddl.ToolTip = "All";

        //    if (prntDdl != null && prntDdl.SelectedIndex == 0)
        //        return;

        //    try
        //    {
        //        DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));                        
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PopUp(ex.Message);
        //    }
        //}
    }
}