﻿using System;
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

        private readonly DBOperations DBOprn = new DBOperations();

        public DataTable DdlReport1DS
        {
            get
            {
                DataTable dt = (DataTable)Session["Performance_DdlReport1DS"];
                if (!(dt?.Rows.Count > 0))
                {
                    dt = DBOprn.GetDataProc("SP_Get_Reports", DBOprn.ConnPrimary,
                        new OleDbParameter[]
                        {
                            new OleDbParameter("@Category_Id", DdlCat1.SelectedValue)
                        }
                    );
                    if (dt.Rows.Count == 0)
                        dt = null;
                }
                return dt;
            }
            set
            {
                if (value?.Rows.Count > 0)
                    value = null;
                Session["Performance_DdlReport1DS"] = value;
            }
        }

        #region Page Code

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string usrId = Session["User_Id"]?.ToString();
                if (usrId == null || usrId == "")
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }
                Menu1_MenuItemClick(Menu1, new MenuEventArgs(Menu1.Items[0]));
                DdlCatType_DataBinding(DdlCatType1, new EventArgs());
                DdlCatType_DataBinding(DdlCatType2, new EventArgs());
                DdlCatType_DataBinding(DdlCatType3, new EventArgs());
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
            if (sender != null)
            {
                BtnApprove.Visible = false;

                if (Menu1.SelectedValue == "0")
                    ResetSbmtTab();
                else if (Menu1.SelectedValue == "1")
                {
                    Menu1.Items[0].Text = "Submit Task |";
                    GVReports2.DataSource = null;
                    GVReports2.Visible = false;

                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);
                    TxtSD.Text = startDate.ToString(DateFormat);
                    TxtED.Text = endDate.ToString(DateFormat);
                }
                else if (Menu1.SelectedValue == "2")
                {
                    GVReports3.DataSource = null;
                    GVReports3.Visible = false;
                    Menu1.Items[0].Text = "Submit Task |";
                    TxtMnth3.Text = DateTime.Now.ToString(MonthFormat);
                }
            }
        }

        private void SetCatType(DropDownList ddl, DropDownList childDdl)
        {
            try
            {
                string roleId = Session["Role_Id"]?.ToString();
                if (!string.IsNullOrWhiteSpace(roleId) && roleId != "1")
                {
                    string catType = roleId == "2" ? "Corporate" : roleId == "3" ? "Plant" : "";
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
            if (ddl.Equals(DdlCatType1))
            {
                childDdl = DdlCat1;
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
            if (ddl.Equals(DdlCat1))
            {
                prntddl = DdlCatType1;
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

            if (ddl.Equals(DdlReport1))
            {
                prntddl = DdlCat1;
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
            if (sender.Equals(DdlCatType1))
            {
                child = DdlCat1;
                grndchild = DdlReport1;
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

            if (sender.Equals(DdlCat1))
                child = DdlReport1;
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

            if (ddl.Equals(DdlReport1))
            {
                string type = DdlReport1DS.Select($"Report_Id = {DdlReport1.SelectedValue}")[0][2].ToString().Trim();
                DdlType1.SelectedValue = type.Trim();
                DivWeek1.Visible = type.Equals("W");
                DdlWeek1.Enabled = type.Equals("W");
            }
        }

        private void FillDdl(DropDownList ddl, String proc, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Select", selectVal));
            ddl.SelectedIndex = 0;
            ddl.ToolTip = "Select";

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


        #region Submit Task

        protected void TxtMnth_TextChanged(object sender, EventArgs e)
        {
            DropDownList ddlWeek = DdlWeek1;

            ddlWeek.SelectedIndex = 0;
            ddlWeek.Items[5].Enabled = true;
            try
            {
                TextBox TB = (TextBox)sender;
                DateTime date = DateTime.ParseExact(TB.Text, MonthFormat, CultureInfo.InvariantCulture);

                if (date.Month == 2) //feb selected
                {
                    int year = date.Year;
                    if (!DateTime.IsLeapYear(year))  //feb contains 28 days i.e. 4 weeks only
                        ddlWeek.Items[5].Enabled = false; //disable week no. 5
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateReportDtls())
            {
                try
                {
                    HttpPostedFile file = FUReport.PostedFile;
                    string ext = Path.GetExtension(file.FileName);
                    if (!(ext.Equals(".xlsx") || ext.Equals(".xls")))
                    {
                        PopUp("Please upload .xls or .xlsx files only");
                        return;
                    }

                    if (file.ContentLength == 0)
                    {
                        PopUp("Please do not enter empty file!");
                        return;
                    }

                    string saveFolder = AppDomain.CurrentDomain.BaseDirectory + @"Upload";
                    if (!Directory.Exists(saveFolder))
                    {
                        Directory.CreateDirectory(saveFolder);
                    }
                    String fullPath = $@"{saveFolder}\{Session["User_Name"]}_{DdlReport1.SelectedItem.Text}_{DateTime.Now.Date.ToShortDateString()}{Path.GetExtension(file.FileName)}";
                    string Report_Id = DdlReport1.SelectedValue;
                    SaveReportToDB(fullPath, Report_Id);
                    file.SaveAs(fullPath);

                    if (Menu1.Items[0].Text == "Edit Task |")
                    {
                        Menu1.Items[1].Selected = true;
                        Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[1]));
                        GVReports2.DataBind();
                        PopUp("File updated successfully!");
                    }
                    else
                        PopUp("File saved successfully!");
                    ResetSbmtTab();
                }
                catch (Exception ex)
                {
                    PopUp(ex.Message);
                }
            }
        }

        private bool ValidateReportDtls()
        {
            if (DdlCatType1.SelectedIndex == 0)
            {
                PopUp("Please select a Category Type!");
                return false;
            }
            if (DdlCat1.SelectedIndex == 0)
            {
                PopUp("Please select a Category!");
                return false;
            }
            if (DdlReport1.SelectedIndex == 0)
            {
                PopUp("Please select a Report Name!");
                return false;
            }
            if (!FUReport.HasFile)
            {
                PopUp("Please upload a File!");
                return false;
            }
            return true;
        }

        private void SaveReportToDB(string fullPath, string Report_Id)
        {
            OleDbParameter[] paramCln =
            {
                new OleDbParameter("@User_Id", Session["User_Id"]),
                new OleDbParameter("@Report_Id", Report_Id),
                new OleDbParameter("@Submit_Date", DateTime.Now.Date.ToString(SqlDateFormat)),
                new OleDbParameter("@Location", fullPath),
                new OleDbParameter("@Created_By", Session["User_Name"])
            };

            var output = DBOprn.ExecScalarProc("SP_Add_Performance", DBOprn.ConnPrimary, paramCln);

            if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
            {
                PopUp(output.ToString());
                return;
            }
        }

        #endregion Submit Task


        #region Edit Task

        protected void BtnEdit_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GVReports2.Rows[rowIndex];

                string id = ((Label)row.Cells[10].Controls[1]).Text;

                string roleId = Session["Role_Id"]?.ToString();
                if (!string.IsNullOrWhiteSpace(roleId) && roleId == "1")
                {
                    DdlCatType1.SelectedValue = DdlCatType1.Items.FindByText(row.Cells[2].Text)?.Value;
                }
                DdlCat1.Items.Add(new ListItem(row.Cells[3].Text, row.Cells[3].Text));
                DdlCat1.SelectedValue = row.Cells[3].Text;

                DdlReport1.Items.Add(new ListItem(row.Cells[4].Text, id));
                DdlReport1.SelectedValue = id;

                String type = row.Cells[6].Text.Trim();
                DdlType1.SelectedValue = DdlType1.Items.FindByText(type)?.Value;

                DateTime sbmtDt = DateTime.Parse(row.Cells[5].Text);
                TxtMnth1.Text = sbmtDt.ToString(MonthFormat);

                if (DdlType1.SelectedValue == "W")
                {
                    int weekNumber = GetWeekNumberOfMonth(sbmtDt);
                    DdlWeek1.SelectedValue = weekNumber.ToString();
                    DivWeek1.Visible = true;
                    DdlWeek1.Enabled = false;
                }

                LnkReport.Text = ((HiddenField)row.Cells[7].Controls[1]).Value;

                DdlCatType1.Enabled = false;
                DdlCat1.Enabled = false;
                DdlReport1.Enabled = false;
                TxtMnth1.Enabled = false;

                BtnCncl.Visible = true;
                Menu1.Items[0].Text = "Edit Task |";
                Menu1.Items[0].Selected = true;
                Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[0]));
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
            Menu1.Items[0].Text = "Submit Task |";

            //enable cat type for admin user only
            string roleId = Session["Role_Id"]?.ToString();
            if (!string.IsNullOrWhiteSpace(roleId) && roleId == "1")
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

            DdlCat1.Enabled = true;
            DdlReport1.Enabled = true;
            TxtMnth1.Enabled = true;

            DivWeek1.Visible = false;

            LnkReport.Text = "";
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

        private void ResetCtrls()
        {
            DdlCatType1.SelectedIndex = 0;
            DdlCat1.SelectedIndex = 0;
            DdlReport1.SelectedIndex = 0;
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            if (sender.Equals(BtnView2))
                GVReports2.DataBind();

            else if (sender.Equals(BtnView3))
                GVReports3.DataBind();
        }

        #region View Sbmt Task

        protected void TxtSD_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime sDate = DateTime.Parse(TxtSD.Text);
                CalendarExtender3.StartDate = sDate;

                if (sDate > DateTime.Parse(TxtED.Text))
                    TxtED.Text = sDate.ToString(DateFormat);
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void GVReports_DataBinding(object sender, EventArgs e)
        {
            try
            {
                bool approv = MultiView1.ActiveViewIndex == 2;
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
                BtnApprove.Visible = GVReports2.Visible;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

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
                catch (Exception ex)
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
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnApprove_Click(object sender, EventArgs e)
        {
            string jsonParam = ConstructJSON();

            if (!string.IsNullOrWhiteSpace(jsonParam))
            {
                var output = DBOprn.ExecScalarProc("SP_Approve_Performance", DBOprn.ConnPrimary,
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
                GVReports2.DataBind();
            }
        }

        private string ConstructJSON()
        {
            string jsonArr = "";
            int i = 1;
            int maxI = GVReports2.Rows.Count - 1;

            try
            {
                foreach (GridViewRow gvRow in GVReports2.Rows)
                {
                    string id = ((Label)gvRow.Cells[9].Controls[1]).Text;
                    string approvDt = DateTime.Now.ToString(SqlDateFormat);
                    Dictionary<string, string> paramVals = new Dictionary<string, string>()
                    {
                        {
                            "REC_ID",
                            id
                        },
                        {
                            "APPROVE_DATE",
                            approvDt
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
                    string jsonString = JsonConvert.SerializeObject(paramVals, Formatting.Indented);
                    jsonArr += jsonString + (i <= maxI ? "," : "");
                    i += 1;
                }
                jsonArr = "[" + jsonArr + "]";
                return jsonArr;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return string.Empty;
            }
        }

        #endregion View Sbmt Task


        private void SetGV(OleDbParameter[] paramCln, GridView gv)
        {
            DataTable dt = DBOprn.GetDataProc("SP_Get_Performance", DBOprn.ConnPrimary, paramCln);
            if (dt != null && dt.Rows.Count > 0)
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
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

    }
}