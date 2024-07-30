using AjaxControlToolkit.Bundling;
using Newtonsoft.Json;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
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
        private const string Emp = "";
        private static int chKCntGVView = 0, chKCntGVAssign = 0;
        bool IsApprover, IsAdmin, IsSuperAdmin;

        #endregion Fields

        #region Properties

        private DataTable GVAssignDS
        {
            get
            {
                DataTable dt = (DataTable)Session["UserTaskAssignment_GVAssignDS"];
                if (!(dt?.Rows.Count > 0))
                {
                    string approverId = IsAdmin && IsApprover ? UsrId : IsAdmin ? null : IsSuperAdmin ? null : UsrId;
                    string locnId = IsAdmin ? LocId : null;
                    //dt = GetSubmitted("SP_Get_Unassigned_Tasks", approverId, locnId);
                    dt = GetData("SP_Get_Unassigned_Tasks", approverId, locnId);
                    if (!(dt?.Rows.Count > 0))
                        dt = null;
                    Session["UserTaskAssignment_GVAssignDS"] = dt;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["UserTaskAssignment_GVAssignDS"] = value;
            }
        }

        private DataTable GVViewDS
        {
            get
            {
                DataTable dt = (DataTable)Session["UserTaskAssignment_GVViewDS"];
                if (!(dt?.Rows.Count > 0))
                {
                    string approverId = IsSuperAdmin ? DdlApproversU.SelectedValue : UsrId;
                    dt = GetData("SP_Get_Assigned_Tasks", approverId, null);
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
            try
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
                IsSuperAdmin = LoginTypes[roleId] == SuperAdmin;
                //if (!(IsAdmin || IsApprover || IsSuperAdmin))
                //{
                //    Response.Redirect("~/Default");
                //    return;
                //}

                if (!IsPostBack)
                {
                    GVAssignDS = null;
                    GVViewDS = null;

                    foreach (DropDownList ddl in new DropDownList[] { DdlCatType, DdlCat, DdlTasks, DdlUsrType, DdlUsers, DdlApproversA, DdlApproversU })
                        ddl.DataBind();

                    chKCntGVView = 0;
                    chKCntGVAssign = 0;

                    if (!IsSmtpConfigValid())
                        PopUp("Email settings could not be verified. No emails will be sent for task assignment!");

                    Menu_MenuItemClick(Menu, new MenuEventArgs(Menu.Items[0]));
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void DdlCatType_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlCatType, "SP_Get_CategoryTypes", "0");
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlCat, "SP_Get_Categories", "0", "All", null,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Type_Id", DdlCatType.SelectedValue)
                }
            );
        }

        protected void DdlTasks_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlTasks, "SP_Report_Get", "0", "All", null,
                new OleDbParameter[]
                {
                     new OleDbParameter("@Category_Id", DdlCat.SelectedValue)
                    ,new OleDbParameter("@Category_Type_Id", DdlCatType.SelectedValue)
                }, "Report_Name", "Report_Id"
            );
        }

        protected void DdlUsers_DataBinding(object sender, EventArgs e)
        {
            if (IsAdmin && IsApprover)
            {
                FillDdl(DdlUsers, "SP_Get_Users_ApprAdmin", Emp, "All", null,
                    new OleDbParameter[]
                    {
                         new OleDbParameter("@Approver_Id", UsrId)
                         ,new OleDbParameter("@Location_Id", LocId)
                        ,new OleDbParameter("@User_Id", DdlUsers.SelectedValue)
                    }
                );
            }
            else

            if (IsAdmin)
            {
                FillDdl(DdlUsers, "SP_Get_Users", Emp, "All", null,
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
                FillDdl(DdlUsers, "SP_Get_SubOrdinates", Emp, "All", null,
                    new OleDbParameter[]
                    {
                         new OleDbParameter("@Approver_Id", UsrId)
                        ,new OleDbParameter("@Role_Id", DdlUsrType.SelectedValue)
                    }
                );
            }
            else if (IsSuperAdmin)
            {
                FillDdl(DdlUsers, "SP_Get_Users", Emp, "All", null,
                    new OleDbParameter[]
                    {
                         new OleDbParameter("@Role_Id", DdlUsrType.SelectedValue)
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
            DvApprs.Visible = IsSuperAdmin;
            DvApprU.Visible = slctItem != 0;
            DvApprA.Visible = slctItem == 0;
            //else
            //if ()
            //    DdlApproversU.Visible = true;
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
                        ResetGVAssign();
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

        private void ResetGVAssign()
        {
            GVAssign.DataSource = null;
            GVAssignDS = null;
            GVAssign.DataBind();
        }

        protected void GVAssign_DataBinding(object sender, EventArgs e)
        {
            if (GVAssign.DataSource == null)
            {
                DataTable dt = GVAssignDS;
                GVAssign.DataSource = dt;
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
            chKCntGVAssign += cb.Checked ? 1 : -1;
            if (GVAssign.Rows.Count < chKCntGVAssign)
                chKCntGVAssign = GVAssign.Rows.Count;
            else if (chKCntGVAssign < 0)
                chKCntGVAssign = 0;
            BtnAssign.Enabled = chKCntGVAssign > 0;
            GridViewRow row = GVAssign.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVAssign.Rows.Count == chKCntGVAssign);
        }

        protected void CBAddH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVAssign.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chKCntGVAssign += chked ? 1 : -1;
                }
            }
            if (GVAssign.Rows.Count < chKCntGVAssign)
                chKCntGVAssign = GVAssign.Rows.Count;
            else if (chKCntGVAssign < 0)
                chKCntGVAssign = 0;
            BtnAssign.Enabled = chKCntGVAssign > 0;
        }

        protected void BtnAssign_Click(object sender, EventArgs e)
        {
            //int.TryParse(HFCntA.Value, out chKCntGVAssign);
            string jsonParam = ConstructJSON("1", GVAssign, GVAssignDS, out Dictionary<string, string> mailSet);
            if (SubMission("SP_Add_Update_TaskAssignment", jsonParam))
            {
                PopUp("Tasks assigned successfully!");
                if (mailSet != null && mailSet.Count > 0)
                    SendEmails(mailSet, "Tasks Assignment Alert - Finance Tracker");
                ResetGVAssign();
                chKCntGVAssign = 0;
                BtnAssign.Enabled = false;
            }
        }

        private void SendEmails(Dictionary<string, string> mailSet, string subject)
        {
            string host = Network.Host, pswd = Network.Password, from = settings.From, usrName = Network.UserName;
            int port = Network.Port;
            try
            {
                using (SmtpClient smtp = new SmtpClient(host, port)) // Your SMTP server
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(usrName, pswd); // Your email credentials
                    smtp.EnableSsl = false; // Enable SSL if required

                    foreach (KeyValuePair<string, string> recipient in mailSet)
                    {
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress(from, usrName), // Your email address
                            Subject = subject,
                            Body = recipient.Value,
                            IsBodyHtml = true
                        };
                        mail.To.Add(recipient.Key);
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception e)
            {
                PopUp(e.Message);
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
                    chKCntGVView += chked ? 1 : -1;
                }
            }
            if (GVView.Rows.Count < chKCntGVView)
                chKCntGVView = GVView.Rows.Count;
            else if (chKCntGVView < 0)
                chKCntGVView = 0;
            BtnUnAssign.Enabled = chKCntGVView > 0;
        }

        protected void CBEdit_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox cb = (CheckBox)sender;
            chKCntGVView += cb.Checked ? 1 : -1;
            if (GVView.Rows.Count < chKCntGVView)
                chKCntGVView = GVView.Rows.Count;
            else if (chKCntGVView < 0)
                chKCntGVView = 0;
            BtnUnAssign.Enabled = chKCntGVView > 0;
            GridViewRow row = GVView.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVView.Rows.Count == chKCntGVView);
        }

        protected void BtnUnAssign_Click(object sender, EventArgs e)
        {
            //int.TryParse(HFCntU.Value, out chKCntGVAssign);
            string jsonParam = ConstructJSON("0", GVView, GVViewDS, out _);
            if (SubMission("SP_Add_Update_TaskAssignment", jsonParam))
            {
                PopUp("Tasks unassigned successfully!");
                ResetGVView();
                chKCntGVView = 0;
                BtnUnAssign.Enabled = false;
            };
        }

        protected void DdlApprovers_DataBinding(object sender, EventArgs e)
        {
            string selectTxt = sender.Equals(DdlApproversA) ? "Select" : "All";
            FillDdl(sender as DropDownList, "SP_Get_Users", Emp, selectTxt, null);
        }

        protected void DdlType_DataBinding(object sender, EventArgs e)
        {
            FillDdl((DropDownList)sender, "SP_ReportTypes_Get", Emp, "All", null, null, "TypeName", "TypeId"); ;
        }

        #endregion TabUnAssign

        #region CommonCode

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

        private DataTable GetData(string proc, string approverId, string locnId)
        {
            DataTable dt = null;
            try
            {
                List<OleDbParameter> parms = new List<OleDbParameter>
                {
                     new OleDbParameter("@Approver_Id", approverId)
                    ,new OleDbParameter("@User_Id", DdlUsers.SelectedValue)
                    ,new OleDbParameter("@Report_Id", DdlTasks.SelectedValue)
                    ,new OleDbParameter("@Category_Id", DdlCat.SelectedValue)
                    ,new OleDbParameter("@Category_Type_Id", DdlCatType.SelectedValue)
                    ,new OleDbParameter("@LocationId", locnId)
                    ,new OleDbParameter("@RoleId", DdlUsrType.SelectedValue)
                    ,new OleDbParameter("@Assigner", IsSuperAdmin ? UsrId : Emp)
                };
                //if (proc == "SP_Get_Unassigned_Tasks")
                //{
                //    parms.Add(new OleDbParameter("@Assigner", IsSuperAdmin ? UsrId : Emp));
                //}
                //else
                //    parms.Add(new OleDbParameter("@Assigner", IsSuperAdmin ? UsrId : Emp));
                dt = DBOprn.GetDataProc
                    (proc, DBOprn.ConnPrimary, parms.ToArray());
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

        private string ConstructJSON(string activ, GridView gv, DataTable gvDS, out Dictionary<string, string> mailSet)
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();
            mailSet = new Dictionary<string, string>();

            int chkCnt = 0;
            gv.Rows.Cast<GridViewRow>().ToList().ForEach(gvRow => chkCnt += ((CheckBox)gvRow.Cells[0].Controls[1]).Checked ? 1 : 0);

            string tableBody = Emp, strt = Emp, footer = Emp;
            if (activ == "1")
            {
                strt = $@"
                       <p>{Session["User_Name"]} has assigned you some tasks.</p>
                       <p>Please find the details below:</p>";

                tableBody = @"<table border='2'>
                        <tr>
                        <th>Task</th>
                        <th>Type</th>
                        <th>Due Date</th>
                        <th>Weight</th>
                        <th>Approver</th>
                        </tr>";
                footer = $@"
                 <p>You can check your assigned tasks at Performance page at
                     <a href= ""http://10.10.1.171:88"">Finance Tracker</a>.</p>
                 <p>For more information contact <a href=""mailto:{Session["Email"]}"">{Session["User_Name"]}</a>.</p>
                 <p>This is an automatically generated email. Please do not reply as there will be no responses.</p>
                 <p>Best Regards,</p>
                 <p>Grupo Bimbo</p>";
            }
            foreach (GridViewRow gvRow in gv.Rows)
            {
                if (chkCnt < 1) break;

                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (!cb.Checked) continue;
                string tblStr = Emp;
                string sno = gvRow.Cells[1].Text;
                DataRow dRo = gvDS.Select("Sno = " + sno)[0];

                string subordinateID = dRo["UserId"].ToString();
                string usrNm = dRo["User_Name"].ToString();
                string reportId = dRo["ReportId"].ToString();
                string reportName = dRo["Task_Name"].ToString();
                string rptType = dRo["Report_Type"].ToString();
                string approver = IsSuperAdmin ? DdlApproversA.SelectedValue : UsrId;
                string usrMail = dRo["Email"].ToString();
                string dueDate = dRo["Due_Date"].ToString();
                string weight = dRo["Weight"].ToString();

                Dictionary<string, string> paramVals = new Dictionary<string, string>()
                {
                    { "USER_ID" , subordinateID.ToUpper() },
                    { "REPORT_ID", reportId },
                    { "REPORT_NAME", reportName },
                    { "CREATED_BY", UsrName },
                    { "APPROVER", approver.ToUpper() },
                    { "ACTIVE", activ }
                };
                dtls.Add(paramVals);
                cb.Checked = false;
                chkCnt--;
                if (activ == "1")
                {
                    tblStr = $@"<tr><td>{reportName}</td><td>{rptType}</td><td>{dueDate}</td><td>{weight}</td><td>{approver}</td></tr>";

                    if (!string.IsNullOrWhiteSpace(usrMail))
                        if (!mailSet.ContainsKey(usrMail))
                            mailSet.Add(usrMail, $@"</br></br></br><p>Dear {usrNm},</p> {strt} {tableBody} {tblStr}");
                        else
                            mailSet[usrMail] += tblStr;
                }
            }

            if (activ == "1")
                mailSet = mailSet.ToDictionary(kvp => kvp.Key, kvp => kvp.Value + "</table>" + footer);
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