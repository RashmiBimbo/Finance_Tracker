using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace Finance_Tracker
{
    public partial class Performance : System.Web.UI.Page
    {
        const string DateFormat = "dd-MMM-yyyy";
        const string MonthFormat = "MMM-yyyy";
        const string SqlDateFormat = "yyyy-MM-dd";
        private static int chKCount = 0;

        private readonly DBOperations DBOprn = new DBOperations();

        public DataTable DdlReport1DS
        {
            get
            {
                DataTable dt = (DataTable)Session["Performance_DdlReport1DS"];
                if ( !(dt?.Rows.Count > 0) )
                {
                    dt = DBOprn.GetDataProc("SP_Get_Reports", DBOprn.ConnPrimary,
                        new OleDbParameter[]
                        {
                            new OleDbParameter("@Category_Id", DdlCat1.SelectedValue)
                        }
                    );
                    if ( dt.Rows.Count == 0 )
                        dt = null;
                }
                return dt;
            }
            set
            {
                if ( value?.Rows.Count > 0 )
                    value = null;
                Session["Performance_DdlReport1DS"] = value;
            }
        }

        #region Page Code

        protected void Page_Load(object sender, EventArgs e)
        {
            if ( !Page.IsPostBack )
            {
                string usrId = Session["User_Id"]?.ToString();
                if ( usrId == null || usrId == "" )
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }
                Menu1_MenuItemClick(Menu1, new MenuEventArgs(Menu1.Items[0]));
                DdlCatType_DataBinding(DdlCatType1, new EventArgs());
                DdlCatType_DataBinding(DdlCatType2, new EventArgs());
                DdlCatType_DataBinding(DdlCatType3, new EventArgs());
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

            LblError.Text = "";
            //User Clicked the menu item
            if ( sender != null )
            {
                BtnSubmit.Visible = false;
                ResetSbmtTab();
                if ( Menu1.SelectedValue == "1" )
                {
                    Menu1.Items[0].Text = "Add Task |";
                    GVReports2.DataSource = null;
                    GVReports2.Visible = false;

                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);
                    TxtSD.Text = startDate.ToString(DateFormat);
                    TxtED.Text = endDate.ToString(DateFormat);
                    DdlCatType_DataBinding(DdlCatType2, new EventArgs());
                    DdlCat_DataBinding(DdlCat2, new EventArgs());
                    DdlReport_DataBinding(DdlReport2, new EventArgs());
                }
                else if ( Menu1.SelectedValue == "2" )
                {
                    GVReports3.DataSource = null;
                    GVReports3.Visible = false;
                    Menu1.Items[0].Text = "Add Task |";
                    TxtMnth3.Text = DateTime.Now.ToString(MonthFormat);
                    DdlCatType_DataBinding(DdlCatType3, new EventArgs());
                    DdlCat_DataBinding(DdlCat3, new EventArgs());
                    DdlReport_DataBinding(DdlReport3, new EventArgs());
                }
            }
        }

        private void SetCatType(DropDownList ddl, DropDownList childDdl)
        {
            try
            {
                string roleId = Session["Role_Id"]?.ToString();
                if ( !string.IsNullOrWhiteSpace(roleId) && roleId != "1" )
                {
                    string catType = roleId == "2" ? "Corporate" : roleId == "3" ? "Plant" : "";
                    catType = ddl.Items.FindByText(catType)?.Value;

                    ddl.SelectedValue = catType;
                    ddl.Enabled = false;
                    childDdl.DataBind();
                }
            }
            catch ( Exception ex )
            {
                PopUp(ex.Message);
            }
        }

        protected void DdlCatType_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, childDdl = null;
            if ( ddl.Equals(DdlCatType1) )
            {
                childDdl = DdlCat1;
            }
            else if ( ddl.Equals(DdlCatType2) )
            {
                childDdl = DdlCat2;
            }
            else if ( ddl.Equals(DdlCatType3) )
            {
                childDdl = DdlCat3;
            }
            FillDdl(ddl, "SP_Get_CategoryTypes", "0");
            SetCatType(ddl, childDdl);
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = null;
            if ( ddl.Equals(DdlCat1) )
            {
                prntddl = DdlCatType1;
            }
            else if ( ddl.Equals(DdlCat2) )
            {
                prntddl = DdlCatType2;
            }
            else if ( ddl.Equals(DdlCat3) )
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

            if ( ddl.Equals(DdlReport1) )
            {
                prntddl = DdlCat1;
            }
            else if ( ddl.Equals(DdlReport2) )
            {
                prntddl = DdlCat2;
            }
            else if ( ddl.Equals(DdlReport3) )
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
            if ( sender.Equals(DdlCatType1) )
            {
                child = DdlCat1;
                grndchild = DdlReport1;
            }
            else if ( sender.Equals(DdlCatType2) )
            {
                child = DdlCat2;
                grndchild = DdlReport2;
            }
            else if ( sender.Equals(DdlCatType3) )
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

            if ( sender.Equals(DdlCat1) )
                child = DdlReport1;
            else if ( sender.Equals(DdlCat2) )
                child = DdlReport2;
            else if ( sender.Equals(DdlCat3) )
                child = DdlReport3;
            child.DataBind();
        }

        protected void DdlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.ToolTip = ddl.SelectedItem.Text;

            if ( ddl.Equals(DdlReport1) )
            {
                DataRow row = DdlReport1DS.Select($"Report_Id = {DdlReport1.SelectedValue}")[0];
                string type = row[2].ToString().Trim();
                DdlType1.SelectedValue = type;

                bool isWeekly = type.Equals("W");
                DivWeek1.Visible = isWeekly;
                DdlWeek1.Enabled = isWeekly;
                DdlWeek1.Visible = isWeekly;
                LblWeek1.Visible = isWeekly;

                string DueDate = row[3].ToString().Trim();
                DateTime.TryParse(TxtMnth1.Text, out DateTime dt);

                if ( dt != null && int.TryParse(DueDate, out int day) )
                    DueDate = new DateTime(dt.Year, dt.Month, day).ToString("dd-MMM-yyyy");
                else
                    DueDate = "Every " + char.ToUpper(DueDate[0]) + DueDate.Substring(1).ToLower();

                TxtDueDt.Text = DueDate;
            }
        }

        private void FillDdl(DropDownList ddl, String proc, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Select", selectVal));
            ddl.SelectedIndex = 0;
            ddl.ToolTip = "Select";

            if ( prntDdl != null && prntDdl.SelectedIndex == 0 )
                return;

            try
            {
                DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

                if ( dt != null && dt.Rows.Count > 0 )
                {
                    for ( int i = 0; i < dt.Rows.Count; i++ )
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                    }
                }
            }
            catch ( Exception ex )
            {
                PopUp(ex.Message);
            }
        }

        #region Add Task

        protected void TxtMnth_TextChanged(object sender, EventArgs e)
        {
            DropDownList ddlWeek = DdlWeek1;

            ddlWeek.SelectedIndex = 0;
            ddlWeek.Items[5].Enabled = true;
            try
            {
                TextBox TB = (TextBox)sender;
                DateTime dt = DateTime.ParseExact(TB.Text, MonthFormat, CultureInfo.InvariantCulture);

                if ( dt.Month == 2 ) //feb selected
                {
                    if ( !DateTime.IsLeapYear(dt.Year) )  //if year is not a leap year, feb contains 28 days i.e. 4 weeks only
                        ddlWeek.Items[5].Enabled = false; //disable week no. 5
                }

                if ( DdlType1.SelectedIndex == 1 )
                {
                    string dueDt = new DateTime(dt.Year, dt.Month, DateTime.Parse(TxtDueDt.Text).Day).ToString("dd-MMM-yyyy");
                    TxtDueDt.Text = dueDt;
                }
            }
            catch ( Exception ex )
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            LblError.Text = "";
            if ( BtnAdd.Text == "OK" )
            {
                BtnCncl_Click(null, null);
                return;
            }
            if ( ValidateReportDtls() )
            {
                try
                {
                    HttpPostedFile file = FUReport.PostedFile;
                    string ext = Path.GetExtension(file.FileName);
                    if ( !(ext.Equals(".xlsx") || ext.Equals(".xls")) )
                    {
                        PopUp("Please upload .xls or .xlsx files only");
                        return;
                    }

                    if ( file.ContentLength == 0 )
                    {
                        PopUp("Please do not enter empty file!");
                        return;
                    }

                    string saveFolder = AppDomain.CurrentDomain.BaseDirectory + @"Upload";
                    if ( !Directory.Exists(saveFolder) )
                    {
                        Directory.CreateDirectory(saveFolder);
                    }
                    String fullPath = $@"{saveFolder}\{Session["User_Name"]}_{DdlReport1.SelectedItem.Text}_{DateTime.Now.Date.ToString(DateFormat)}{Path.GetExtension(file.FileName)}";

                    string Report_Id = DdlReport1.SelectedValue;
                    file.SaveAs(fullPath);

                    if ( Menu1.Items[0].Text == "Edit Task |" )
                    {
                        if ( !UpdateTask("SP_Update_Task", fullPath, LblTaskID.Text) ) return;
                        Menu1.Items[1].Selected = true;
                        Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[1]));
                        GVReports2.DataBind();
                        PopUp("Task updated successfully!");
                        LblError.Text = "Task updated successfully!";
                        //delete old file
                    }
                    else if ( Menu1.Items[0].Text == "Add Task |" )
                    {
                        if ( !AddTaskToDB("SP_Add_Task", fullPath, Report_Id) ) return;
                        PopUp("Task added successfully!");
                        LblError.Text = "Task added successfully!";
                    }
                    LblError.CssClass = "col-12 control-label text-success ";
                    ResetSbmtTab();
                }
                catch ( Exception ex )
                {
                    PopUp(ex.Message);
                    LblError.Text = ex.Message;
                    LblError.CssClass = "col-12 control-label text-danger ";
                }
                LblError.Visible = true;
            }
        }

        private bool ValidateReportDtls()
        {
            if ( DdlCatType1.SelectedIndex == 0 )
            {
                PopUp("Please select a Category Type!");
                return false;
            }
            if ( DdlCat1.SelectedIndex == 0 )
            {
                PopUp("Please select a Category!");
                return false;
            }
            if ( DdlReport1.SelectedIndex == 0 )
            {
                PopUp("Please select a Report Name!");
                return false;
            }
            if ( string.IsNullOrEmpty(TxtMnth1.Text) )
            {
                PopUp("Please select a month!");
                return false;
            }
            if ( DdlType1.SelectedValue == "W" && DdlWeek1.SelectedValue == "0" )
            {
                PopUp("Please select a week!");
                return false;
            }
            if ( !FUReport.HasFile )
            {
                PopUp("Please upload a File!");
                return false;
            }
            return true;
        }

        private bool AddTaskToDB(string proc, string fullPath, string rec_Id = "")
        {
            DateTime dt = DateTime.Parse(TxtMnth1.Text);
            OleDbParameter[] paramCln = new OleDbParameter[]
            {
                new OleDbParameter("@User_Id", Session["User_Id"]),
                new OleDbParameter("@Report_Id", DdlReport1.SelectedValue),
                new OleDbParameter("@Submit_Date", DateTime.Now.Date.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_From_Date", dt.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_To_Date", new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month)).ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_Week_No", DdlWeek1.SelectedValue),
                new OleDbParameter("@Location", fullPath),
                new OleDbParameter("@Created_By", Session["User_Name"])
            };

            var output = DBOprn.ExecScalarProc(proc, DBOprn.ConnPrimary, paramCln);

            if ( !string.IsNullOrWhiteSpace((string)output) ) //Error occurred
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
                new OleDbParameter("@Report_Id", DdlReport1.SelectedValue),
                new OleDbParameter("@Submit_Date", DateTime.Now.Date.ToString(SqlDateFormat)),
                new OleDbParameter("@Submit_From_Date", DateTime.Parse(TxtMnth1.Text)),
                new OleDbParameter("@Submit_To_Date", DateTime.Parse(TxtMnth1.Text)),
                new OleDbParameter("@Submit_Week_No", DdlWeek1.SelectedValue),
                new OleDbParameter("@Location", fullPath),
                new OleDbParameter("@Created_By", Session["User_Name"]),
                new OleDbParameter("@Rec_Id", rec_Id)
            };
            var output = DBOprn.ExecScalarProc(proc, DBOprn.ConnPrimary, paramCln);

            if ( !string.IsNullOrWhiteSpace((string)output) ) //Error occurred
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
            if ( sender.Equals(BtnView2) )
                GVReports2.DataBind();

            else if ( sender.Equals(BtnView3) )
                GVReports3.DataBind();
        }

        #region View Sbmt Task

        protected void TxtSD_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime sDate = DateTime.Parse(TxtSD.Text);
                CalendarExtender3.StartDate = sDate;

                if ( sDate > DateTime.Parse(TxtED.Text) )
                    TxtED.Text = sDate.ToString(DateFormat);
            }
            catch ( Exception ex )
            {
                PopUp(ex.Message);
            }
        }

        protected void GVReports_DataBinding(object sender, EventArgs e)
        {
            try
            {
                bool submit = MultiView1.ActiveViewIndex == 2;
                string strtDate = DateTime.Parse(TxtSD.Text).ToString(SqlDateFormat);
                string endDate = DateTime.Parse(TxtED.Text).ToString(SqlDateFormat);
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
                SetGV(paramCln, GVReports2);
                BtnSubmit.Visible = GVReports2.Visible;
            }
            catch ( Exception ex )
            {
                PopUp(ex.Message);
            }
        }

        protected void CBSubmitH_CheckedChanged(object sender, EventArgs e)
        {
            foreach ( GridViewRow gvRow in GVReports2.Rows )
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if ( cb.Checked != chked )
                {
                    cb.Checked = chked;
                    chKCount += chked ? 1 : -1;
                }
            }
            if ( GVReports2.Rows.Count < chKCount )
                chKCount = GVReports3.Rows.Count;
            else if ( chKCount < 0 )
                chKCount = 0;
            BtnSubmit.Enabled = chKCount > 0;
        }

        protected void CBSubmit_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chKCount += cb.Checked ? 1 : -1;
            if ( GVReports2.Rows.Count < chKCount )
                chKCount = GVReports3.Rows.Count;
            else if ( chKCount < 0 )
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

                if ( !string.IsNullOrWhiteSpace(jsonParam) )
                {
                    var output = DBOprn.ExecScalarProc("SP_Submit_Tasks", DBOprn.ConnPrimary,
                        new OleDbParameter[]
                        {
                            new OleDbParameter("@Collection", jsonParam)
                        }
                    );

                    if ( !string.IsNullOrWhiteSpace((string)output) ) //Error occurred
                    {
                        PopUp(output.ToString());
                        return;
                    }
                    PopUp("Tasks submitted successfully!");
                    GVReports2.DataBind();
                }
            }
            catch ( Exception ex )
            {
                PopUp(ex.Message);
            }
            chKCount = 0;
        }

        private string ConstructJSON()
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();

            foreach ( GridViewRow gvRow in GVReports2.Rows )
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if ( cb.Checked )
                {
                    string id = ((Label)gvRow.Cells[11].Controls[1]).Text;
                    string submitDt = DateTime.Now.ToString(SqlDateFormat);
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

        #endregion View Sbmt Task

        #region Edit Task

        protected void BtnEdit_Command(object sender, CommandEventArgs e)
        {
            if ( e.CommandName == "EditRow" )
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GVReports2.Rows[rowIndex];

                string roleId = Session["Role_Id"]?.ToString();
                if ( !string.IsNullOrWhiteSpace(roleId) && roleId == "1" )
                {
                    DdlCatType1.SelectedValue = DdlCatType1.Items.FindByText(row.Cells[2].Text)?.Value;
                    DdlCatType1.ToolTip = DdlCatType1.SelectedItem.Text;
                }
                DdlCat1.Items.Add(new ListItem(row.Cells[3].Text, row.Cells[3].Text));
                DdlCat1.SelectedValue = row.Cells[3].Text;
                DdlCat1.ToolTip = DdlCat1.SelectedItem.Text;

                string reportId = ((Label)row.Cells[10].Controls[1]).Text;
                DdlReport1.Items.Add(new ListItem(row.Cells[4].Text, reportId));
                DdlReport1.SelectedValue = reportId;
                DdlReport1.ToolTip = DdlReport1.SelectedItem.Text;

                String type = row.Cells[7].Text.Trim();
                DdlType1.SelectedValue = DdlType1.Items.FindByText(type)?.Value;

                TxtDueDt.Text = row.Cells[5].Text;

                DateTime toDate = DateTime.Parse(((Label)row.Cells[13].Controls[1]).Text);
                TxtMnth1.Text = toDate.ToString(MonthFormat);

                if ( row.Cells[7].Text.ToUpper() == "WEEKLY" )
                {
                    //int weekNumber = GetWeekNumberOfMonth(toDate);
                    DdlWeek1.SelectedValue = ((Label)row.Cells[14].Controls[1]).Text;
                    DivWeek1.Visible = true;
                    DdlWeek1.Enabled = false;
                }
                LnkReport.Text = ((HiddenField)row.Cells[8].Controls[1]).Value;
                DivLnk.Visible = true;

                BtnCncl.Visible = true;
                Menu1.Items[0].Selected = true;
                Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[0]));

                LblTaskID.Text = ((Label)row.Cells[11].Controls[1]).Text;

                DdlCatType1.Enabled = false;
                DdlCat1.Enabled = false;
                DdlReport1.Enabled = false;
                TxtMnth1.Enabled = false;

                LinkButton btn = (LinkButton)sender;
                if ( btn.Text == "View" )
                {
                    BtnCncl.Visible = false;
                    Menu1.Items[0].Text = "View Added Task |";
                    FUReport.Enabled = false;
                    BtnAdd.Text = "OK";
                }
                else if ( btn.Text == "Edit" )
                {
                    BtnCncl.Visible = true;
                    Menu1.Items[0].Text = "Edit Task |";
                    FUReport.Enabled = true;
                    BtnAdd.Text = "Edit";
                }
            }
        }

        private int GetWeekNumberOfMonth(DateTime date)
        {
            // Calculate the week number of the month
            int daysInWeek = 7;
            int dayOfWeek = (int)date.DayOfWeek + 1;
            int daysToFirstDayOfMonth = date.Day - 1;

            int weekNumber = (daysToFirstDayOfMonth + dayOfWeek - 1) / daysInWeek + 1;

            return weekNumber;
        }

        protected void BtnCncl_Click(object sender, EventArgs e)
        {
            ResetSbmtTab();
            Menu1.Items[1].Selected = true;
            Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[1]));
        }

        private void ResetSbmtTab()
        {
            Menu1.Items[0].Text = "Add Task |";

            //enable cat type for admin user only
            string roleId = Session["Role_Id"]?.ToString();
            if ( !string.IsNullOrWhiteSpace(roleId) && roleId == "1" )
            {
                DdlCatType1.SelectedIndex = 0;
                DdlCatType_SelectedIndexChanged(DdlCatType1, new EventArgs());
                DdlCatType1.Enabled = true;
            }
            DdlCat1.SelectedIndex = 0;

            DdlReport1.DataBind();
            DdlReport1.SelectedIndex = 0;
            DdlType1.SelectedIndex = 0;
            TxtMnth1.Text = DateTime.Now.ToString(MonthFormat);
            TxtDueDt.Text = "";
            CalendarExtender1.StartDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(-1);
            CalendarExtender1.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            DdlCat1.Enabled = true;
            DdlReport1.Enabled = true;
            TxtMnth1.Enabled = true;
            DdlWeek1.SelectedIndex = 0;
            DivWeek1.Visible = false;
            BtnAdd.Text = "Add";
            LnkReport.Text = "";
            DivLnk.Visible = false;
            BtnCncl.Visible = false;
        }

        protected void LnkReport_Click(object sender, EventArgs e)
        {
            string fullPath = LnkReport.Text;
            string fileName = Path.GetFileName(fullPath);

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

        #endregion Edit Task

        protected void GVReports3_DataBinding(object sender, EventArgs e)
        {
            try
            {
                string User_Id = Session["User_Id"]?.ToString();
                string Role_Id = Session["Role_Id"]?.ToString();
                int mnthNo = 0, year = 0;
                try
                {
                    DateTime dt = DateTime.ParseExact(TxtMnth3.Text, MonthFormat, CultureInfo.InvariantCulture);
                    mnthNo = dt.Month;
                    year = dt.Year;
                }
                catch ( Exception ex )
                { }

                OleDbParameter[] paramCln = new OleDbParameter[]
                {
                    new OleDbParameter("@Start_Date", null)
                   ,new OleDbParameter("@End_Date",  null)
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
                   ,new OleDbParameter("@ApprYearNo", year)
                   ,new OleDbParameter("@ApprMonthNo", mnthNo)
                };
                SetGV(paramCln, GVReports3);
            }
            catch ( Exception ex )
            {
                PopUp(ex.Message);
            }
        }

        private void SetGV(OleDbParameter[] paramCln, GridView gv)
        {
            DataTable dt = DBOprn.GetDataProc("SP_Get_Tasks", DBOprn.ConnPrimary, paramCln);
            if ( dt != null && dt.Rows.Count > 0 )
            {
                gv.DataSource = dt;
                gv.Visible = true;
            }
            else
            {
                gv.Visible = false;
                PopUp("No data found!");
            }
        }

        public void PopUp(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

    }
}