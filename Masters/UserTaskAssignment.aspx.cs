using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Finance_Tracker.DBOperations;

namespace Finance_Tracker.Masters
{
    public partial class UserTaskAssignment : Page
    {
        #region Fields

        private string roleId, LocId, UsrId, UsrName;
        private readonly DBOperations DBOprn;
        private const string emp = "";
        private static int chKGVViewCount = 0, chKCountGVAdd = 0;
        bool IsApprover, IsAdmin;

        #endregion Fields

        #region Properties

        private DataTable GVAddDS
        {
            get
            {
                DataTable dt = (DataTable)Session["UserTaskAssignment_GVAddDS"];
                if (!(dt?.Rows.Count > 0))
                {
                    string approverId = IsAdmin ? null : UsrId;
                    string locnId = IsAdmin ? LocId : null;
                    //dt = GetData("SP_Get_Unassigned_Tasks", approverId, locnId);
                    dt = GetData("SP_Get_Unassigned_Tasks", UsrId, LocId);
                    if (!(dt?.Rows.Count > 0))
                        dt = null;
                    Session["UserTaskAssignment_GVAddDS"] = dt;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["UserTaskAssignment_GVAddDS"] = value;
            }
        }

        private DataTable GVViewDS
        {
            get
            {
                DataTable dt = (DataTable)Session["UserTaskAssignment_GVViewDS"];
                if (!(dt?.Rows.Count > 0))
                {
                    //string approverId = IsAdmin ? null : UsrId;
                    dt = GetData("SP_Get_Assigned_Tasks", UsrId, null);
                    if (!(dt?.Rows.Count > 0))
                        dt = null;
                    Session["UserTaskAssignment_GVViewDS"] = dt;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["UserTaskAssignment_GVViewDS"] = value;
            }
        }

        #endregion Properties

        public UserTaskAssignment()
        {
            DBOprn = new DBOperations();
            if (!DBOprn.AuthenticatConns())
            {
                PopUp("Database connection could not be established!");
                Response.Redirect("~/Default");
            }
        }

        #region PageCode

        protected void Page_Load(object sender, EventArgs e)
        {
            UsrId = Session["User_Id"]?.ToString();

            if (string.IsNullOrWhiteSpace(UsrId))
            {
                Response.Redirect("~/Account/Login");
                return;
            }
            roleId = Session["Role_Id"]?.ToString();
            IsApprover = Convert.ToBoolean(Session["Is_Approver"]);
            LocId = Session["Location_Id"]?.ToString();
            UsrName = Session["User_Name"]?.ToString();

            IsAdmin = LoginTypes[roleId] == Admin;
            if (!(IsAdmin || IsApprover))
            {
                Response.Redirect("~/Default");
                return;
            }

            if (!IsPostBack)
            {
                GVAddDS = null;
                GVViewDS = null;

                DdlCatType.DataBind();
                DdlCat.DataBind();
                DdlTasks.DataBind();
                DdlUsrType.DataBind();
                DdlUsers.DataBind();

                chKGVViewCount = 0;
                chKCountGVAdd = 0;

                Menu_MenuItemClick(Menu, new MenuEventArgs(Menu.Items[0]));
            }
        }

        protected void DdlCatType_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlCatType, "SP_Get_CategoryTypes", "0");
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlCat, "SP_Get_Categories", "0", DdlCatType,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Type_Id", DdlCatType.SelectedValue)
                }
            );
        }

        protected void DdlTasks_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlTasks, "SP_Report_Get", "0", DdlCat,
                new OleDbParameter[]
                {
                     new OleDbParameter("@Category_Id", DdlCat.SelectedValue)
                    ,new OleDbParameter("@Category_Type_Id", DdlCatType.SelectedValue)
                }, "Report_Name", "Report_Id"
            );
        }

        protected void DdlUsers_DataBinding(object sender, EventArgs e)
        {
            if (IsAdmin)
            {
                FillDdl(DdlUsers, "SP_Get_Users", emp, null,
                    new OleDbParameter[]
                    {
                         new OleDbParameter("@Role_Id", DdlUsrType.SelectedValue)
                         //new OleDbParameter("@Role_Id", "0")
                        ,new OleDbParameter("@Location_Id", LocId)
                    }
                );
            }
            else if (IsApprover)
            {
                FillDdl(DdlUsers, "SP_Get_SubOrdinates", emp, null,
                    new OleDbParameter[]
                    {
                         new OleDbParameter("@Approver_Id", UsrId)
                        ,new OleDbParameter("@Role_Id", DdlUsrType.SelectedValue)
                    }
                );
            }
        }

        protected void DdlUsrType_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlUsrType, "SP_Get_Roles", "0");
            int roleId = Convert.ToInt32(Session["Role_Id"]);

            DdlUsrType.Items.FindByValue("4").Enabled = roleId > 4;
            DdlUsrType.Items.FindByValue("1").Enabled = roleId >= 4;
        }

        protected void Menu_MenuItemClick(object sender, MenuEventArgs e)
        {
            int slctItem = int.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = slctItem;
            DivAdd.Visible = false;
            DivView.Visible = false;
            DdlCatType.SelectedIndex = 0;
            DdlCat.SelectedIndex = 0;
            DdlTasks.SelectedIndex = 0;
            DdlUsrType.SelectedIndex = 0;
            DdlUsers.SelectedIndex = 0;
        }

        protected void DdlCatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlCatType.ToolTip = DdlCatType.SelectedItem.Text;
            DdlCat.DataBind();
            DdlTasks.DataBind();
        }

        protected void DdlCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlCat.ToolTip = DdlCat.SelectedItem.Text;
            DdlTasks.DataBind();
        }

        protected void DdlUsrType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlUsrType.ToolTip = DdlUsrType.SelectedItem.Text;
            DdlUsers.DataBind();
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            switch (Menu.SelectedValue)
            {
                case "0":
                {
                    ResetGVAdd();
                    break;
                }
                case "1":
                {
                    ResetGVView();
                    break;
                }
            }
        }

        #endregion PageCode

        #region TabAssign

        private void ResetGVAdd()
        {
            GVAdd.DataSource = null;
            GVAddDS = null;
            GVAdd.DataBind();
        }

        protected void GVAdd_DataBinding(object sender, EventArgs e)
        {
            if (GVAdd.DataSource == null)
            {
                DataTable dt = GVAddDS;
                GVAdd.DataSource = dt;
                if (dt == null)
                {
                    DivAdd.Visible = false;
                    PopUp("No data found!");
                }
                else
                    DivAdd.Visible = true;
            }
        }

        protected void CBAdd_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chKCountGVAdd += cb.Checked ? 1 : -1;
            if (GVAdd.Rows.Count < chKCountGVAdd)
                chKCountGVAdd = GVAdd.Rows.Count;
            else if (chKCountGVAdd < 0)
                chKCountGVAdd = 0;
            BtnAssign.Enabled = chKCountGVAdd > 0;
            GridViewRow row = GVAdd.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVAdd.Rows.Count == chKCountGVAdd);
        }

        protected void CBAddH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVAdd.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
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
            BtnAssign.Enabled = chKCountGVAdd > 0;
        }

        protected void BtnAssign_Click(object sender, EventArgs e)
        {
            string jsonParam = ConstructJSON("1", GVAdd, GVAddDS, chKCountGVAdd);
            if (SubMission("SP_Add_Update_TaskAssignment", jsonParam))
            {
                PopUp("Tasks assigned successfully!");
                ResetGVAdd();
                chKCountGVAdd = 0;
                BtnAssign.Enabled = false;
            }
        }

        #endregion TabAssign

        #region TabUnAssign

        private void ResetGVView()
        {
            GVView.DataSource = null;
            GVViewDS = null;
            GVView.DataBind();
        }

        protected void GVView_DataBinding(object sender, EventArgs e)
        {
            if (GVView.DataSource == null)
            {
                DataTable dt = GVViewDS;
                GVView.DataSource = dt;
                if (dt == null)
                {
                    DivView.Visible = false;
                    PopUp("No data found!");
                }
                else
                    DivView.Visible = true;
            }
        }

        protected void CBEditH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVView.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chKGVViewCount += chked ? 1 : -1;
                }
            }
            if (GVView.Rows.Count < chKGVViewCount)
                chKGVViewCount = GVView.Rows.Count;
            else if (chKGVViewCount < 0)
                chKGVViewCount = 0;
            BtnUnAssign.Enabled = chKGVViewCount > 0;
        }

        protected void CBEdit_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox cb = (CheckBox)sender;
            chKGVViewCount += cb.Checked ? 1 : -1;
            if (GVView.Rows.Count < chKGVViewCount)
                chKGVViewCount = GVView.Rows.Count;
            else if (chKGVViewCount < 0)
                chKGVViewCount = 0;
            BtnUnAssign.Enabled = chKGVViewCount > 0;
            GridViewRow row = GVView.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVView.Rows.Count == chKGVViewCount);
        }

        protected void BtnUnAssign_Click(object sender, EventArgs e)
        {
            string jsonParam = ConstructJSON("0", GVView, GVViewDS, chKGVViewCount);
            if (SubMission("SP_Add_Update_TaskAssignment", jsonParam))
            {
                PopUp("Tasks unassigned successfully!");
                ResetGVView();
                chKGVViewCount = 0;
                BtnUnAssign.Enabled = false;
            };
        }

        #endregion TabUnAssign

        #region CommonCode

        private void FillDdl(DropDownList ddl, string proc, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null, string TxtField = emp, string ValField = emp)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("All", selectVal));
            ddl.SelectedIndex = 0;
            ddl.ToolTip = "All";

            //if (prntDdl != null && prntDdl.SelectedIndex == 0)
            //    return;

            try
            {
                DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if ((TxtField != emp) && ValField != emp)
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

        private DataTable GetData(string proc, string approverId, string locnId)
        {
            DataTable dt = null;
            try
            {
                dt = DBOprn.GetDataProc
                    (proc, DBOprn.ConnPrimary
                        , new OleDbParameter[]
                         {
                              new OleDbParameter("@Approver_Id", approverId)
                             ,new OleDbParameter("@User_Id", DdlUsers.SelectedValue)
                             ,new OleDbParameter("@Report_Id", DdlTasks.SelectedValue)
                             ,new OleDbParameter("@Category_Id", DdlCat.SelectedValue)
                             ,new OleDbParameter("@Category_Type_Id", DdlCatType.SelectedValue)
                             ,new OleDbParameter("@LocationId", locnId)
                             ,new OleDbParameter("@RoleId", DdlUsrType.SelectedValue)
                         }
                    );
            }
            catch (Exception e)
            {
                PopUp(e.Message);
            }
            return dt;
        }

        public void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        private string ConstructJSON(string activ, GridView gv, DataTable gvDS, int chk)
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();

            foreach (GridViewRow gvRow in gv.Rows)
            {
                if (chk < 1) break;

                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (cb.Checked)
                {
                    DataRow dRow = gvDS.Select("Sno = " + gvRow.Cells[1].Text)[0];
                    string subordinateID = dRow["UserId"].ToString();
                    string reportId = dRow["ReportId"].ToString();
                    string reportName = dRow["Task_Name"].ToString();
                    Dictionary<string, string> paramVals = new Dictionary<string, string>()
                    {
                        {
                            "USER_ID" ,
                             subordinateID.ToUpper()
                        },
                        {
                            "REPORT_ID",
                            reportId
                        },
                        {
                            "REPORT_NAME",
                             reportName
                        },
                        {
                            "CREATED_BY",
                            UsrName
                        },
                        {
                            "APPROVER",
                            UsrId.ToUpper()
                        },
                        {
                            "ACTIVE",
                             activ
                        }
                    };
                    dtls.Add(paramVals);
                    cb.Checked = false;
                    chk--;
                }
                continue;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
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

        #endregion CommonCode

    }
}