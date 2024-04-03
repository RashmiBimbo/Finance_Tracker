using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Finance_Tracker.DBOperations;
using static System.Convert;

namespace Finance_Tracker.Masters
{
    public partial class Reports : Page
    {
        private string RoleId, LocId, UsrId, UsrName;
        private readonly DBOperations DBOprn;
        private const string Emp = "";
        private static int chkDltCntReports = 0;
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
                BtnDlt.Attributes.Add("onclick", "return BtnDltOnClientClick();");
                Session["DuType"] = "Date";
                chkDltCntReports = 0;
                //DdlTypeA.Attributes.Add("onchange", "ChngDueDtType(this.value)");
                DdlCatTypeA.DataBind();
                DdlCatA.DataBind();
                //DdlTypeA.DataBind();
                DdlCatTypeV.DataBind();
                DdlCatV.DataBind();
                DdlTypeV.DataBind();
                Menu_MenuItemClick(Menu, new MenuEventArgs(Menu.Items[0]));
            }
        }

        protected void DdlCatType_DataBinding(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(DdlCatTypeA))
                {
                    FillDdl(DdlCatTypeA, "SP_Get_CategoryTypes", "0", "Select");
                }
                else if (sender.Equals(DdlCatTypeV))
                {
                    FillDdl(DdlCatTypeV, "SP_Get_CategoryTypes", "0", "All");
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(DdlCatA))
                {
                    FillDdl(DdlCatA, "SP_Get_Categories", "0", "Select", DdlCatTypeA,
                        new OleDbParameter[]
                        {
                        new OleDbParameter("@Type_Id", DdlCatTypeA.SelectedValue)
                        }
                    );
                }
                else if (sender.Equals(DdlCatV))
                {
                    FillDdl(DdlCatV, "SP_Get_Categories", "0", "All", null,
                        new OleDbParameter[]
                        {
                        new OleDbParameter("@Type_Id", DdlCatTypeV.SelectedValue)
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
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
            Menu.Items[0].Text = "Add Report |";
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
            try
            {
                string name = TxtReportName.Text.Trim();
                string cat = DdlCatA.SelectedValue;
                string priority = TxtPriority.Text;
                string weight = TxtWeight.Text;
                string typeId = DdlTypeA.SelectedValue.Trim();
                string proc, duDt = Emp;

                switch (typeId.ToUpper())
                {
                    case "MONTHLY":
                        duDt = TxtDuDt.Text;
                        break;
                    case "WEEKLY":
                        duDt = DdlWeekDay.SelectedValue;
                        break;
                    case "HALF YEARLY":
                        duDt = "4" + DdlHY.SelectedValue;
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
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return false;
            }
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
                switch (DdlTypeA.SelectedValue.ToUpper().Trim())
                {
                    case "MONTHLY":
                        if ((duDtTxt < 1 || duDtTxt > 31))
                        {
                            PopUp("Due Date must be within the range of 1 to 31!");
                            TxtDuDt.Focus();
                            return false;
                        }
                        break;
                    case "WEEKLY":
                        if (string.IsNullOrWhiteSpace(DdlWeekDay.SelectedValue))
                        {
                            PopUp("Due Day is required!");
                            DdlWeekDay.Focus();
                            return false;
                        }
                        break;
                    case "HALF YEARLY":
                        if (DdlHY.SelectedValue == "0")
                        {
                            PopUp("Due Half is required!");
                            DdlHY.Focus();
                            return false;
                        }
                        break;
                    case Emp:
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
            try
            {
                Menu.Items[0].Text = "Add Report |";
                DdlCatTypeA.Enabled = true;
                DdlCatTypeA.SelectedIndex = 0;
                DdlCatA.Enabled = true;
                DdlCatType_SelectedIndexChanged(DdlCatTypeA, null);
                TxtReportName.Text = Emp;
                TxtPriority.Text = Emp;
                TxtWeight.Text = Emp;
                TxtDuDt.Text = Emp;
                DdlTypeA.SelectedIndex = 0;
                DvDuDt.Visible = false;
                DdlWeekDay.SelectedIndex = 0;
                DdlHY.SelectedIndex = 0;
                BtnCncl.Visible = false;
                BtnAdd.Text = "Add";
                SetTooltip(new DropDownList[] { DdlCatTypeA, DdlCatA, DdlTypeA, DdlWeekDay });
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
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
                    chkDltCntReports += chked ? 1 : -1;
                }
            }
            if (GVReports.Rows.Count < chkDltCntReports)
                chkDltCntReports = GVReports.Rows.Count;
            else if (chkDltCntReports < 0)
                chkDltCntReports = 0;
            BtnDlt.Enabled = chkDltCntReports > 0;
        }

        protected void CDelete_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chkDltCntReports += cb.Checked ? 1 : -1;
            if (GVReports.Rows.Count < chkDltCntReports)
                chkDltCntReports = GVReports.Rows.Count;
            else if (chkDltCntReports < 0)
                chkDltCntReports = 0;
            BtnDlt.Enabled = chkDltCntReports > 0;
            GridViewRow row = GVReports.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVReports.Rows.Count == chkDltCntReports);
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
                chkDltCntReports = 0;
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
            int chkCnt = 0;
            GVReports.Rows.Cast<GridViewRow>().ToList().ForEach(gvRow => chkCnt += ((CheckBox)gvRow.Cells[0].Controls[1]).Checked ? 1 : 0);

            //chkDltCntReports = ToInt16(HFCnt.Value);

            foreach (GridViewRow gvRow in GVReports.Rows)
            {
                if (chkCnt < 1) break;

                DataRow dRo = GVReportsDS.Select("Sno = " + gvRow.Cells[1].Text)?[0];
                if (dRo is null) continue;

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
                chkCnt--;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        protected void BtnAction_Click(object sender, EventArgs e)
        {
            try
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

                string type = dRo["Type_orgnl"].ToString().Trim();
                DdlTypeA.SelectedValue = type;
                DdlType_SelectedIndexChanged(null, null);
                DvDuDt.Visible = true;
                String duDt = dRo["Due_Date_Orgnl"].ToString();

                switch (type.ToUpper())
                {
                    case "MONTHLY":
                        TxtDuDt.Text = duDt;
                        break;
                    case "WEEKLY":
                        DdlWeekDay.SelectedValue = duDt;
                        break;
                    case "HALF YEARLY":
                        DdlHY.SelectedValue = duDt.Replace("4", Emp);
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
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void DdlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTooltip(null, DdlTypeA);
            DvDuDt.Visible = true;
            DvTxtDuDt.Visible = false;
            DvHY.Visible = false;
            DvWkDay.Visible = false;
            DdlWeekDay.SelectedValue = Emp;
            DdlHY.SelectedValue = "0";
            TxtDuDt.Text = Emp;

            switch (DdlTypeA.SelectedValue.Trim().ToUpper())
            {
                case "MONTHLY":
                {
                    DvTxtDuDt.Visible = true;
                    Session["DuType"] = "Date";
                    break;
                }
                case "WEEKLY":
                {
                    DvWkDay.Visible = true;
                    Session["DuType"] = "Day";
                    break;
                }
                case "HALF YEARLY":
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

        protected void DdlType_DataBinding(object sender, EventArgs e)
        {
            FillDdl((DropDownList)sender, "SP_Report_Type_Get", Emp, "All", null, null, "ReportType", "ReportType");
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

    }
}