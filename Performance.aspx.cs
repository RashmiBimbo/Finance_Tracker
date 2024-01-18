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

        private readonly DBOperations DBOprn = new DBOperations();

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
                SetDates();
                DdlType_DataBinding(DdlType1, new EventArgs());
                DdlType2_DataBinding(DdlType2, new EventArgs());
                DdlType3_DataBinding(DdlType3, new EventArgs());
            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            int eval = e is null ? 0 : int.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = eval;
            MultiView1.Views[eval].Focus();
            BtnApprove.Visible = false;

            //User Clicked the menu item
            if (Menu1.SelectedValue == "0")
            {
                if (sender != null)
                {
                    ResetSbmtTab();
                    Menu1.Items[0].Text = "Submit Task |";
                }
                //    DdlType1.DataBind();
            }
            else
            if (Menu1.SelectedValue == "1")
            {
                if (sender != null)
                {
                    //DdlType2.DataBind();
                    Menu1.Items[0].Text = "Submit Task |";
                    GVReports2.DataSource = null;
                    GVReports2.Visible = false;
                    SetDates();
                }
            }
            else if (Menu1.SelectedValue == "2")
                if (sender != null)
                {
                    //DdlType3.DataBind();
                    GVReports3.DataSource = null;
                    GVReports3.Visible = false;
                    Menu1.Items[0].Text = "Submit Task |";
                    //SetDates();
                }
            //function called externally
            //else
            //    GVReports2.DataBind();
        }

        private void SetDates()
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            TxtSD.Text = startDate.ToString("dd-MMM-yyyy");
            TxtED.Text = endDate.ToString("dd-MMM-yyyy");
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

        protected void DdlType_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList childDdl = DdlCat1;

            //if (MultiView1.ActiveViewIndex == 0)
            //{
            //    ddl = DdlType1;
            //    childDdl = DdlCat1;
            //}
            //else if (MultiView1.ActiveViewIndex == 1)
            //{
            //    ddl = DdlType2;
            //    childDdl = DdlCat2;
            //}
            //else if (MultiView1.ActiveViewIndex == 2)
            //{
            //    ddl = DdlType3;
            //    childDdl = DdlCat3;
            //}
            FillDdl(ddl, "SP_Get_CategoryTypes", "Category_Type_Name", "Category_Type_Id");
            SetCatType(ddl, childDdl);
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = DdlType1;
            { 
            //if (MultiView1.ActiveViewIndex == 0)
            //{
            //    ddl = DdlCat1;
            //    prntddl = DdlType1;
            //}
            //else if (MultiView1.ActiveViewIndex == 1)
            //{
            //    ddl = DdlCat2;
            //    prntddl = DdlType2;
            //}
            //else if (MultiView1.ActiveViewIndex == 2)
            //{
            //    ddl = DdlCat3;
            //    prntddl = DdlType3;
            //}
            }
            FillDdl(ddl, "SP_Get_Categories", "Category_Name", "Category_Id", prntddl,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Type_Id", prntddl.SelectedValue)
                }
            );
        }

        protected void DdlReport_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = null;
            if (sender.Equals(DdlReport1))
            {
                ddl = DdlReport1;
                prntddl = DdlCat1;
            }
            else if (sender.Equals(DdlReport2))
            {
                ddl = DdlReport2;
                prntddl = DdlCat2;
            }
            else if (sender.Equals(DdlReport3))
            {
                ddl = DdlReport3;
                prntddl = DdlCat3;
            }
            FillDdl(ddl, "SP_Get_Reports", "Report_Name", "Report_Id", prntddl,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Category_Id", prntddl.SelectedValue)
                }
            );
        }

        protected void DdlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList child = null;
            DropDownList grndchild = null;
            if (sender.Equals(DdlType1))
            {
                child = DdlCat1;
                grndchild = DdlReport1;
            }
            else if (sender.Equals(DdlType2))
            {
                child = DdlCat2;
                grndchild = DdlReport2;
            }
            else if (sender.Equals(DdlType3))
            {
                child = DdlCat3;
                grndchild = DdlReport3;
            }
            child.DataBind();
            grndchild.DataBind();
        }

        protected void DdlCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList child = null;
            if (sender.Equals(DdlCat1))
                child = DdlReport1;
            else if (sender.Equals(DdlCat2))
                child = DdlReport2;
            else if (sender.Equals(DdlCat3))
                child = DdlReport3;
            child.DataBind();
        }

        private void FillDdl(DropDownList ddl, String proc, string DataTextField, string DataValueField, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        {
            ddl.Items.Clear();
            if (prntDdl != null && prntDdl.SelectedIndex == 0)
            {
                ddl.Items.Add(new ListItem("Select", "0"));
                ddl.SelectedIndex = 0;
                return;
            }
            try
            {
                DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //ddl.DataSource = dt;
                    //ddl.DataTextField = DataTextField;
                    //ddl.DataValueField = DataValueField;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                    }
                }
                else
                    ddl.Items.Add(new ListItem("Select", "0"));

                ddl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateDtls())
            {
                try
                {
                    //string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string savePath = @"\\10.10.1.135\E:\HODATA\ACCTS-DATA\FIN-TRACKER";
                    savePath = Server.MapPath(savePath);
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

                    String fullPath = $@"{savePath}\{Session["User_Name"]}_{DdlReport1.SelectedItem.Text}_{DateTime.Now.Date.ToShortDateString()}.{Path.GetExtension(file.FileName)}";
                    //if (File.Exists(fullPath))
                    //{
                    //    PopUp("File already exists.");
                    //    return;
                    //}
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
                        PopUp("File uploaded successfully!");
                    ResetSbmtTab();
                }
                catch (Exception ex)
                {
                    PopUp(ex.Message);
                }
            }
        }

        private bool ValidateDtls()
        {
            if (DdlType1.SelectedIndex == 0)
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
            //if (RBWeekly.Checked && DdlWeek.SelectedIndex == 0)
            //{
            //    PopUp("Please select a week no.!");
            //    return false;
            //}
            //if (RBMonth2.Checked && DdlMonth.SelectedIndex == 0)
            //{
            //    PopUp("Please select a month!");
            //    return false;
            //}
            return true;
        }

        private void SaveReportToDB(string fullPath, string Report_Id)
        {
            OleDbParameter[] paramCln =
            {
                new OleDbParameter("@User_Id", Session["User_Id"]),
                new OleDbParameter("@Report_Id", Report_Id),
                //new OleDbParameter("@Report_Type", Report_Id),
                new OleDbParameter("@Submit_Date", DateTime.Now.Date.ToString("yyyy-MM-dd")),
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

        private void ResetCtrls()
        {
            DdlType1.SelectedIndex = 0;
            DdlCat1.SelectedIndex = 0;
            DdlReport1.SelectedIndex = 0;
        }

        //protected void RB_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (sender.Equals(RBMonth2))
        //    {
        //        DdlMonth2.Visible = true;
        //        RFVDdlMonth2.Enabled = true;
        //        DdlWeek2.Visible = false;
        //        RFVDdlWeek2.Enabled = false;
        //    }
        //    else if (sender.Equals(RBWeek2))
        //    {
        //        DdlMonth2.Visible = false;
        //        RFVDdlMonth2.Enabled = false;
        //        DdlWeek2.Visible = true;
        //        RFVDdlWeek2.Enabled = true;
        //    }
        //    else if (sender.Equals(RBAll2))
        //    {
        //        DdlMonth2.Visible = false;
        //        RFVDdlMonth2.Enabled = false;
        //        DdlWeek2.Visible = false;
        //        RFVDdlWeek2.Enabled = false;
        //    }
        //}

        protected void BtnView_Click(object sender, EventArgs e)
        {
            if (MultiView1.ActiveViewIndex == 1)
                GVReports2.DataBind();

            else if (MultiView1.ActiveViewIndex == 2)
                GVReports3.DataBind();
        }

        protected void GVReports_DataBinding(object sender, EventArgs e)
        {
            //GridView gv = (GridView)sender;
            try
            {
                bool approv = MultiView1.ActiveViewIndex == 2;
                string strtDate = DateTime.Parse(TxtSD.Text).ToString("yyyy-MM-dd");
                string endDate = DateTime.Parse(TxtED.Text).ToString("yyyy-MM-dd");
                string User_Id = Session["User_Id"]?.ToString();
                string Role_Id = Session["Role_Id"]?.ToString();
                //DropDownList ddlType = DdlRType3 : DdlRType2;
                //DropDownList ddlMnth = DdlMonth3 : DdlMonth2;
                //string type = ddlType.SelectedValue ;
                //int mnth = Convert.ToInt32(ddlMnth.SelectedValue);

                OleDbParameter[] paramCln = new OleDbParameter[]
                    {
                        new OleDbParameter("@Start_Date", strtDate)
                       ,new OleDbParameter("@End_Date", endDate)
                       ,new OleDbParameter("@User_Id", User_Id)
                       ,new OleDbParameter("@Role_Id", Role_Id)
                       ,new OleDbParameter("@Category_Type_Id", DdlType2.SelectedValue)
                       ,new OleDbParameter("@Category_Id", DdlCat2.SelectedValue)
                       ,new OleDbParameter("@Report_Id", DdlReport2.SelectedValue)
                       ,new OleDbParameter("@Type", DdlRType2.SelectedValue)
                       //,new OleDbParameter() {
                       //    ParameterName = "@IsApproved",
                       //    Value = /*approv ? 1 :*/ 0,
                       //    OleDbType = OleDbType.Boolean
                       //}
                       //,new OleDbParameter("@ApprWeekNo", 0)
                       //,new OleDbParameter("@ApprMonthNo", 0)
                       //,new OleDbParameter("@ApprYearNo", 0)
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

                OleDbParameter[] paramCln = new OleDbParameter[]
                {
                    new OleDbParameter("@Start_Date", null)
                   ,new OleDbParameter("@End_Date",  null)
                   ,new OleDbParameter("@User_Id", User_Id)
                   ,new OleDbParameter("@Role_Id", Role_Id)
                   ,new OleDbParameter("@Category_Type_Id", DdlType3.SelectedValue)
                   ,new OleDbParameter("@Category_Id", DdlCat3.SelectedValue)
                   ,new OleDbParameter("@Report_Id", DdlReport3.SelectedValue)
                   ,new OleDbParameter("@Type", DdlRType3.SelectedValue)
                   ,new OleDbParameter() {
                       ParameterName = "@IsApproved",
                       Value = 1,
                       OleDbType = OleDbType.Boolean
                   }
                   ,new OleDbParameter("@ApprYearNo", DateTime.Now.Year)
                   ,new OleDbParameter("@ApprMonthNo", DdlMonth3.SelectedValue)
                   //,new OleDbParameter("@ApprWeekNo", DdlWeek3.SelectedValue)
                };
                SetGV(paramCln, GVReports3);
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

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

        protected void BtnEdit_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "GetRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GVReports2.Rows[rowIndex];

                string id = ((Label)row.Cells[10].Controls[1]).Text;

                string roleId = Session["Role_Id"]?.ToString();
                if (!string.IsNullOrWhiteSpace(roleId) && roleId == "1")
                {
                    DdlType1.SelectedValue = DdlType1.Items.FindByText(row.Cells[2].Text)?.Value;
                }
                DdlCat1.Items.Add(new ListItem(row.Cells[3].Text, row.Cells[3].Text));
                DdlCat1.SelectedValue = row.Cells[3].Text;

                DdlReport1.Items.Add(new ListItem(row.Cells[4].Text, id));
                DdlReport1.SelectedValue = id;

                DdlType1.Enabled = false;
                DdlCat1.Enabled = false;
                DdlReport1.Enabled = false;

                String type = row.Cells[6].Text;
                //DdlRType1.SelectedValue = DdlRType1.Items.FindByText(type)?.Value;
                //DdlRType1.Enabled = false;

                LnkReport.Text = ((HiddenField)row.Cells[7].Controls[1]).Value;

                //DateTime sbmtDt = DateTime.Parse(row.Cells[5].Text);
                //int weekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(sbmtDt, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

                BtnCncl.Visible = true;
                Menu1.Items[0].Text = "Edit Task |";
                Menu1.Items[0].Selected = true;
                Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[0]));
            }
        }

        protected void BtnCncl_Click(object sender, EventArgs e)
        {
            ResetSbmtTab();
            Menu1.Items[1].Selected = true;
            Menu1_MenuItemClick(null, new MenuEventArgs(Menu1.Items[1]));
        }

        private void ResetSbmtTab()
        {
            BtnCncl.Visible = false;
            Menu1.Items[0].Text = "Submit Task |";

            string roleId = Session["Role_Id"]?.ToString();
            if (!string.IsNullOrWhiteSpace(roleId) && roleId == "1")
            {
                DdlType1.SelectedIndex = 0;
                DdlType_SelectedIndexChanged(DdlType1, new EventArgs());
                DdlType1.Enabled = true;
            }
            DdlCat1.SelectedIndex = 0;
            DdlReport1.DataBind();
            DdlReport1.SelectedIndex = 0;
            DdlRType2.SelectedIndex = 0;
            DdlCat1.Enabled = true;
            DdlReport1.Enabled = true;
            //DdlRType1.Enabled = true;
            //DdlMonth.SelectedIndex = 0;
            //DdlWeek.SelectedIndex = 0;
            LnkReport.Text = "";
        }

        public void PopUp(string msg)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        protected void DdlRType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bool approv = MultiView1.ActiveViewIndex == 2;
            //DropDownList ddl = (DropDownList)sender;
            ////DropDownList ddlMnth = approv ? DdlMonth3 : DdlMonth2;
            ////DropDownList ddlWeek = approv ? DdlWeek3 : DdlWeek2;
            ////RequiredFieldValidator rfvW =  /*approv? RFVDdlWeek3 :*/ RFVDdlWeek2;
            ////RequiredFieldValidator rfvM =  /*approv? RFVDdlMonth3 :*/ RFVDdlMonth2;

            //switch (ddl.SelectedValue)
            //{
            //    case "":
            //        {
            //            ddlMnth.Visible = false;
            //            //rfvM.Enabled = false;
            //            ddlWeek.Visible = false;
            //            //rfvW.Enabled = false;
            //            break;
            //        }
            //    case "M":
            //        {
            //            ddlMnth.Visible = true;
            //            ddlMnth.SelectedIndex = 0;
            //            //rfvM.Enabled = true;
            //            ddlWeek.Visible = false;
            //            //rfvW.Enabled = false;
            //            break;
            //        }
            //    case "W":
            //        {
            //            ddlWeek.SelectedIndex = 0;
            //            ddlWeek.Visible = true;
            //            //rfvW.Enabled = true;
            //            ddlMnth.Visible = false;
            //            //rfvM.Enabled = true;
            //            break;
            //        }
            //}
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
                    PopUp("Error occurred \r\n" + output.ToString());
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
                    string approvDt = DateTime.Now.ToString("yyyy-MM-dd");
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

        protected void TxtSD_TextChanged(object sender, EventArgs e)
        {
            //CalendarExtender2.EndDate
        }

        protected void DdlRType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DdlRType3.SelectedIndex == 2)
            //{
            //    LblWkNo.Visible = true;
            //    DdlWeek3.Visible = true;
            //}
            //else
            //{
            //    LblWkNo.Visible = false;
            //    DdlWeek3.Visible = false;
            //}
            //DdlWeek3.SelectedIndex = 0;
        }

        protected void LnkReport_Click(object sender, EventArgs e)
        {
            string path = LnkReport.Text;
            bool canOpen;
            if (File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                try
                {
                    using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        canOpen = true;
                        fs.Close();
                        fs.Dispose();
                    }
                    if (canOpen)
                        System.Diagnostics.Process.Start(path);
                    else
                        PopUp("File can not be opened!");
                }
                catch (IOException ex)
                {
                    PopUp("Error occurred :\r\n" + ex.Message);
                }
                catch (Exception ex)
                {
                    PopUp("Error occurred :\r\n" + ex.Message);
                }
            }
            else
                PopUp("File does not exists!");
        }

        protected void DdlType2_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList childDdl = DdlCat2;
            FillDdl(ddl, "SP_Get_CategoryTypes", "Category_Type_Name", "Category_Type_Id");
            SetCatType(ddl, childDdl);
        }

        protected void DdlType3_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList childDdl = DdlCat3;
            FillDdl(ddl, "SP_Get_CategoryTypes", "Category_Type_Name", "Category_Type_Id");
            SetCatType(ddl, childDdl);
        }

        protected void DdlCat3_DataBinding(object sender, EventArgs e)
        {
            //DropDownList ddl = (DropDownList)sender, prntddl = DdlType3;
            FillDdl(DdlCat3, "SP_Get_Categories", "Category_Name", "Category_Id", DdlType3,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Type_Id", DdlType3.SelectedValue)
                }
            );
        }

        protected void DdlCat2_DataBinding(object sender, EventArgs e)
        {
            //DropDownList ddl = (DropDownList)sender, prntddl = DdlType2;
            FillDdl(DdlCat2, "SP_Get_Categories", "Category_Name", "Category_Id", DdlType2,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Type_Id", DdlType2.SelectedValue)
                }
            );
        }

    }
}