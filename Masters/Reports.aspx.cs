﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static Finance_Tracker.DBOperations;
using static System.Convert;

namespace Finance_Tracker.Masters
{
    public partial class Reports : Page
    {
        private string RoleId, LocId, UsrId, UsrName;
        private readonly DBOperations DBOprn;
        private const string emp = "";
        private static int chkDltCnt = 0;
        bool IsApprover, IsAdmin;
        public string DuType = "Date";

        private DataTable GVReportsDS
        {
            get
            {
                DataTable dt = (DataTable)Session["Reports_GVReportsDS"];
                if (!(dt?.Rows.Count > 0))
                {
                    OleDbParameter[] paramCln = new OleDbParameter[]
                        {
                           new OleDbParameter("@Category_Id", DdlCatV.SelectedValue)
                           ,new OleDbParameter("@Category_Type_Id", DdlCatTypeV.SelectedValue)
                           ,new OleDbParameter("@Type", DdlTypeV.SelectedValue)
                        };
                    dt = DBOprn.GetDataProc("SP_Report_Get", DBOprn.ConnPrimary, paramCln);
                    if (dt.Rows.Count == 0)
                        dt = null;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["Reports_GVReportsDS"] = value;
            }
        }

        public Reports()
        {
            DBOprn = new DBOperations();
            if (!DBOprn.AuthenticatConns())
            {
                PopUp("Database connection could not be established!");
                Response.Redirect("~/Default");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UsrId = Session["User_Id"]?.ToString();

            if (string.IsNullOrWhiteSpace(UsrId))
            {
                Response.Redirect("~/Account/Login");
                return;
            }
            RoleId = Session["Role_Id"]?.ToString();
            IsApprover = ToBoolean(Session["Is_Approver"]);
            LocId = Session["Location_Id"]?.ToString();
            UsrName = Session["User_Name"]?.ToString();

            IsAdmin = LoginTypes[RoleId] == Admin;
            if (!(IsAdmin || IsApprover))
            {
                Response.Redirect("~/Default");
                return;
            }
            if (!IsPostBack)
            {
                Session["DuType"] = "Date";
                chkDltCnt = 0;
                //DdlTypeA.Attributes.Add("onchange", "ChngDueDtType(this.value)");
                DdlCatTypeA.DataBind();
                DdlCatA.DataBind();
                DdlCatTypeV.DataBind();
                DdlCatV.DataBind();
                Menu_MenuItemClick(Menu, new MenuEventArgs(Menu.Items[0]));
            }
        }

        protected void DdlCatType_DataBinding(object sender, EventArgs e)
        {
            if (sender.Equals(DdlCatTypeA))
            {
                FillDdl(DdlCatTypeA, "SP_Get_CategoryTypes", "Select", "0");
            }
            else if (sender.Equals(DdlCatTypeV))
            {
                FillDdl(DdlCatTypeV, "SP_Get_CategoryTypes", "All", "0");
            }
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            if (sender.Equals(DdlCatA))
            {
                FillDdl(DdlCatA, "SP_Get_Categories", "Select", "0", DdlCatTypeA,
                    new OleDbParameter[]
                    {
                        new OleDbParameter("@Type_Id", DdlCatTypeA.SelectedValue)
                    }
                );
            }
            else if (sender.Equals(DdlCatV))
            {
                FillDdl(DdlCatV, "SP_Get_Categories", "All", "0", null,
                    new OleDbParameter[]
                    {
                        new OleDbParameter("@Type_Id", DdlCatTypeV.SelectedValue)
                    }
                );
            }
        }

        protected void DdlCatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = null, prntDdl = null;
            if (sender.Equals(DdlCatTypeA))
            {
                ddl = DdlCatA;
                prntDdl = DdlCatTypeA;
            }
            else if (sender.Equals(DdlCatTypeV))
            {
                ddl = DdlCatV;
                prntDdl = DdlCatTypeV;
            }
            SetTooltip(null, prntDdl);
            ddl.DataBind();
        }

        protected void Menu_MenuItemClick(object sender, MenuEventArgs e)
        {
            int slctItem = int.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = slctItem;
            Menu.Items[0].Text= "Add Report |";
            switch (slctItem)
            {
                case 0:
                {
                    ResetTabAdd();
                    break;
                }
                case 1:
                {
                    ResetTabVu();
                    break;
                }
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                bool add = BtnAdd.Text == "Add";

                if (!ValidateRptDtls()) return;

                if (!PerformDbOprn(add)) return;

                try
                {
                    if (add)
                    {
                        ResetTabAdd();
                        PopUp("Report added successfully!");
                    }
                    else
                    {
                        ResetGVReports();
                        BtnCncl_Click(BtnCncl, null);
                        PopUp("Report updated successfully!");
                    }
                }
                catch (Exception ex)
                {
                    PopUp(ex.Message);
                }
            }
        }

        private bool PerformDbOprn(bool add)
        {
            string name = TxtReportName.Text.Trim();
            string cat = DdlCatA.SelectedValue;
            string priority = TxtPriority.Text;
            string weight = TxtWeight.Text;
            string typeId = DdlTypeA.SelectedValue;
            string proc, duDt = emp;
            switch (typeId)
            {
                case "M":
                    duDt = TxtDuDt.Text;
                    break;
                case "W":
                    duDt = DdlWeekDay.SelectedValue;
                    break;
                case "HY":
                    duDt = DdlHY.SelectedValue;
                    break;
                default:
                    break;
            }

            OleDbParameter[] paramCln =
            {
                new OleDbParameter("@Name", name),
                new OleDbParameter("@Category_Id", cat),
                new OleDbParameter("@Priority", priority),
                new OleDbParameter("@Weight", weight),
                new OleDbParameter("@Type_Id", typeId),
                new OleDbParameter("@Due_Date", duDt),
                new OleDbParameter("@Created_By", Session["User_Name"])
            };

            if (add)
                proc = "SP_Report_Add";
            else
            {
                proc = "SP_Report_Update";
                paramCln = paramCln.Append(new OleDbParameter("@RecId", LblRprtId.Text.Trim())).ToArray();
            }

            var output = DBOprn.ExecScalarProc(proc, DBOprn.ConnPrimary, paramCln);

            if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
            {
                PopUp(output.ToString());
                return false;
            }
            return true;
        }

        private bool ValidateRptDtls()
        {
            try
            {
                int.TryParse(TxtDuDt.Text, out int duDtTxt);
                int.TryParse(TxtPriority.Text, out int priority);
                int.TryParse(TxtWeight.Text, out int weight);
                string name = TxtReportName.Text;
                char[] invalidFileNameChars = Path.GetInvalidFileNameChars().OrderByDescending(chr => chr).ToArray(); ;

                if (DdlCatTypeA.SelectedValue == "0")
                {
                    PopUp("Category Type is required!");
                    DdlCatTypeA.Focus();
                    return false;
                }
                if (DdlCatA.SelectedValue == "0")
                {
                    PopUp("Category is required!");
                    DdlCatA.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    PopUp("Report Name is required!");
                    TxtReportName.Focus();
                    return false;
                }
                if (name.Any(chr => invalidFileNameChars.Contains(chr)))
                {
                    string invalidCharsString = string.Join(", ", invalidFileNameChars.Select(c => char.IsControl(c) ? $"\\x{(int)c:X2}" : c.ToString()));
                    invalidCharsString = invalidCharsString.Substring(0, 24);
                    string msg = $"Report Name cannot contain any of the following characters: {invalidCharsString}";
                    PopUp(msg);
                    return false;
                }
                if (priority < 1 || priority > 99)
                {
                    PopUp("Priority must be within the range of 1 to 99!");
                    TxtPriority.Focus();
                    return false;
                }
                if (weight < 1 || weight > 99)
                {
                    PopUp("Weight must be within the range of 1 to 99!");
                    TxtWeight.Focus();
                    return false;
                }
                switch (DdlTypeA.SelectedValue)
                {
                    case "M":
                        if ((duDtTxt < 1 || duDtTxt > 31))
                        {
                            PopUp("Due Date must be within the range of 1 to 31!");
                            TxtDuDt.Focus();
                            return false;
                        }
                        break;
                    case "W":
                        if (string.IsNullOrWhiteSpace(DdlWeekDay.SelectedValue))
                        {
                            PopUp("Due Day is required!");
                            DdlWeekDay.Focus();
                            return false;
                        }
                        break;
                    case "HY":
                        if (DdlHY.SelectedValue == "0")
                        {
                            PopUp("Due Half is required!");
                            DdlHY.Focus();
                            return false;
                        }
                        break;
                    case emp:
                    {
                        PopUp("Type is required!");
                        DdlTypeA.Focus();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return false;
            }
        }

        private void ResetTabAdd()
        {
            Menu.Items[0].Text = "Add Report |";
            DdlCatTypeA.Enabled = true;
            DdlCatTypeA.SelectedIndex = 0;
            DdlCatA.Enabled = true;
            DdlCatType_SelectedIndexChanged(DdlCatTypeA, null);
            TxtReportName.Text = emp;
            TxtPriority.Text = emp;
            TxtWeight.Text = emp;
            TxtDuDt.Text = emp;
            DdlTypeA.SelectedIndex = 0;
            DvDuDt.Visible = false;
            DdlWeekDay.SelectedIndex = 0;
            DdlHY.SelectedIndex = 0;
            SetTooltip(new DropDownList[] { DdlCatTypeA, DdlCatA, DdlTypeA, DdlWeekDay });
            BtnCncl.Visible = false;
            BtnAdd.Text = "Add";
        }

        protected void CDeleteH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVReports.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chkDltCnt += chked ? 1 : -1;
                }
            }
            if (GVReports.Rows.Count < chkDltCnt)
                chkDltCnt = GVReports.Rows.Count;
            else if (chkDltCnt < 0)
                chkDltCnt = 0;
            BtnDlt.Enabled = chkDltCnt > 0;
        }

        protected void CDelete_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chkDltCnt += cb.Checked ? 1 : -1;
            if (GVReports.Rows.Count < chkDltCnt)
                chkDltCnt = GVReports.Rows.Count;
            else if (chkDltCnt < 0)
                chkDltCnt = 0;
            BtnDlt.Enabled = chkDltCnt > 0;
            GridViewRow row = GVReports.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVReports.Rows.Count == chkDltCnt);
        }

        protected void GVReports_DataBinding(object sender, EventArgs e)
        {
            try
            {
                if (GVReports.DataSource == null)
                {
                    DataTable dt = GVReportsDS;
                    GVReports.DataSource = dt;
                    if (dt == null)
                    {
                        //DvGV.Visible = false;
                        GVReports.Visible = false;
                        BtnDlt.Visible = false;
                        PopUp("No data found!");
                    }
                    else
                    {
                        GVReports.Visible = true;
                        //DvGV.Visible = true;
                        BtnDlt.Visible = true;
                    }
                }
                BtnDlt.Visible = GVReports.Visible;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        private void ResetTabVu()
        {
            DdlCatTypeV.SelectedIndex = 0;
            DdlCatType_SelectedIndexChanged(DdlCatTypeV, null);
            DdlCatV.SelectedIndex = 0;
            DdlTypeV.SelectedIndex = 0;
            GVReports.Visible = false;
            BtnDlt.Visible = false;
        }

        protected void BtnDlt_Click(object sender, EventArgs e)
        {
            string jsonParam = ConstructJSON();
            if (SubMission("SP_Report_Delete", jsonParam))
            {
                PopUp("Reports deleted successfully!");
                ResetGVReports();
                chkDltCnt = 0;
                BtnDlt.Enabled = false;
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

            foreach (GridViewRow gvRow in GVReports.Rows)
            {
                if (chkDltCnt == 0) break;

                DataRow dRo = GVReportsDS.Select("Sno = " + gvRow.Cells[1].Text)?[0];
                if (dRo == null) continue;

                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (!cb.Checked) continue;

                string id = dRo["Report_Id"].ToString();
                Dictionary<string, string> paramVals = new Dictionary<string, string>()
                {
                    {
                        "REC_ID",
                        id
                    },
                    {
                        "MODIFIED_BY",
                        Session["User_Name"]?.ToString().Trim()
                    },
                    {
                        "MODIFIED_DATE",
                        DateTime. Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    }
                };
                dtls.Add(paramVals);
                cb.Checked = false;
                chkDltCnt--;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        protected void BtnAction_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            Control ctrl = btn.NamingContainer;
            int rowIndex = ((GridViewRow)ctrl).RowIndex;
            DataRow dRo = GVReportsDS?.Select($"Sno = {rowIndex + 1}")[0];

            if (dRo is null || dRo.ItemArray.Length == 0) return;

            string catTypVal = dRo["Category_Type_Id"].ToString();
            DdlCatTypeA.SelectedValue = catTypVal;

            string catVal = dRo["Category_Id"].ToString();
            DdlCatA.Items.Add(new ListItem(dRo["Category_Name"].ToString(), catVal));
            DdlCatA.SelectedValue = catVal;

            string type = dRo["Type_orgnl"].ToString().Trim().ToUpper();
            DdlTypeA.SelectedValue = type;
            DvDuDt.Visible = true;

            DdlType_SelectedIndexChanged(null, null);
            String duDt = dRo["Due_Date_Orgnl"].ToString();
            switch (type.ToUpper())
            {
                case "M":
                {
                    TxtDuDt.Text = duDt;
                }
                break;
                case "W":
                {
                    DdlWeekDay.SelectedValue = duDt;
                    break;
                }
                case "HY":
                    DdlHY.SelectedValue = duDt;
                    break;
                default:
                    break;
            }
            TxtPriority.Text = dRo["Priority"].ToString();
            TxtReportName.Text = dRo["Report_Name"].ToString();
            TxtWeight.Text = dRo["Weight"].ToString();
            LblRprtId.Text = dRo["Report_Id"].ToString();
            BtnCncl.Visible = true;
            Menu.Items[0].Selected = true;
            MultiView1.ActiveViewIndex = 0;

            DdlCatTypeA.Enabled = false;
            DdlCatA.Enabled = false;

            BtnCncl.Visible = true;
            Menu.Items[0].Text = "Edit Report |";
            BtnAdd.Text = "Save";
            SetTooltip(new DropDownList[] { DdlCatTypeA, DdlCatA, DdlTypeA, DdlWeekDay });
        }

        protected void DdlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            SetTooltip(null, ddl);
            DvDuDt.Visible = true;
            DvTxtDuDt.Visible = false;
            DvHY.Visible = false;
            DvWkDay.Visible = false;
            DdlWeekDay.SelectedIndex = 0;
            DdlHY.SelectedIndex = 0;
            TxtDuDt.Text = emp;

            switch (DdlTypeA.SelectedValue)
            {
                case "M":
                {
                    DvTxtDuDt.Visible = true;
                    Session["DuType"] = "Date";
                    break;
                }
                case "W":
                {
                    DvWkDay.Visible = true;
                    Session["DuType"] = "Day";
                    break;
                }
                case "HY":
                {
                    DvHY.Visible = true;
                    Session["DuType"] = "Half";
                    break;
                }
                default:
                    DvDuDt.Visible = false;
                    break;
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            ResetGVReports();
            BtnDlt.Enabled = false;
        }

        private void ResetGVReports()
        {
            GVReports.DataSource = null;
            GVReportsDS = null;
            GVReports.DataBind();
        }


        protected void BtnCncl_Click(object sender, EventArgs e)
        {
            Menu.Items[0].Text = "Add Report |";
            Menu.Items[1].Selected = true;
            MultiView1.ActiveViewIndex = 1;
        }

        private void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        private void SetTooltip(DropDownList[] ddlLst = null, DropDownList ddl = null)
        {
            if (ddlLst != null)
                ddlLst.ToList().ForEach(itm => itm.ToolTip = itm.SelectedItem.Text);
            else if (ddl != null)
                ddl.ToolTip = ddl.SelectedItem.Text;
        }

        private void FillDdl(DropDownList ddl, string proc, string selectTxt, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(selectTxt, selectVal));
            ddl.SelectedIndex = 0;
            ddl.ToolTip = selectTxt;

            if (prntDdl != null && prntDdl.SelectedIndex == 0) return;
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

    }
}