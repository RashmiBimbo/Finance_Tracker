using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Finance_Tracker.DBOperations;
using System.Threading.Tasks;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System.Web.Services.Description;

namespace Finance_Tracker.Masters
{
    public partial class Users : Page
    {
        private string RoleId, LocId, UsrId, LoggdUsrName, LoggdUsrMail;
        private readonly DBOperations DBOprn;
        private const string Emp = "", All = "All", Select = "Select";
        private bool IsSuperAdmin, IsAdmin;
        private string Host, HostPswd, From, HostName;
        private int Port;

        private DataTable GVUsersDS
        {
            get
            {
                DataTable dt = (DataTable)Session["Users_GVUsersDS"];
                if (!(dt?.Rows.Count > 0))
                {
                    OleDbParameter[] paramCln = new OleDbParameter[]
                        {
                            new OleDbParameter("@Role_Id", DdlRoleV.SelectedValue)
                           ,new OleDbParameter("@Locn_Id", DdlLocnV.SelectedValue)
                           ,new OleDbParameter("@User_Id", DdlUser.SelectedValue)
                        };
                    dt = DBOprn.GetDataProc("SP_Get_Users", DBOprn.ConnPrimary, paramCln);
                    if (dt.Rows.Count == 0)
                        dt = null;
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    value = null;
                Session["Reports_GVUsersDS"] = value;
            }
        }

        public Users()
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
            LocId = Session["Location_Id"]?.ToString();
            LoggdUsrName = Session["User_Name"]?.ToString();
            LoggdUsrMail = Session["Email"]?.ToString();

            IsAdmin = LoginTypes[RoleId] == Admin;
            IsSuperAdmin = LoginTypes[RoleId] == SuperAdmin;

            if (!IsPostBack)
            {
                BtnDlt.Attributes.Add("onclick", "return BtnDltOnClientClick();");

                foreach (DropDownList ddl in new[] { DdlRoleV, DdlLocnV, DdlLocnA, DdlRoleA })
                    ddl.DataBind();

                if (IsAdmin)
                {
                    DdlRoleA.Items.Cast<ListItem>().ToList().ForEach(i => i.Enabled = i.Text.ToUpper() != "ADMIN");
                    if (!string.IsNullOrWhiteSpace(LocId))
                        if (LocId == "CORP")
                            DdlRoleV.SelectedIndex = DdlRoleV.Items.IndexOf(DdlRoleV.Items.FindByText("Corporate"));
                        else
                            DdlRoleV.SelectedIndex = DdlRoleV.Items.IndexOf(DdlRoleV.Items.FindByText("Plant"));
                    DdlRoleV.Enabled = false;
                    DdlLocnV.SelectedIndex = DdlLocnV.Items.IndexOf(DdlLocnV.Items.FindByValue(LocId));
                    DdlLocnV.Enabled = false;
                }
                DdlRoleV.Items.FindByText("Super Admin").Enabled = false;
                DdlRoleA.Items.FindByText("Super Admin").Enabled = false;
                DdlUser.DataBind();
                Menu_MenuItemClick(Menu, new MenuEventArgs(Menu.Items[0]));

                if (!IsSmtpConfigValid())
                {
                    PopUp("Email Settings could not be verified. No emails will be sent for registration!");
                    //return;
                }
                Host = Network?.Host;
                HostPswd = Network?.Password;
                From = Settings?.From;
                HostName = Network?.UserName;
                Port = Network is null ? 0 : Network.Port;
            }
        }

        protected void Menu_MenuItemClick(object sender, MenuEventArgs e)
        {
            int slctItem = int.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = slctItem;
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

        private void ResetTabAdd()
        {
            try
            {
                Menu.Items[0].Text = "Add User |";
                DivPswd.Visible = true;
                DivCPswd.Visible = true;
                BtnCancel.Visible = false;
                BtnRegister.Text = "Register";
                ResetCtrls();
                //SetTooltip(new DropDownList[] { DdlCatTypeA, DdlCatA, DdlTypeA, DdlWeekDay });
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void GVUsers_DataBinding(object sender, EventArgs e)
        {
            try
            {
                if (GVUsers.DataSource == null)
                {
                    DataTable dt = GVUsersDS;
                    GVUsers.DataSource = dt;
                    if (dt == null)
                    {
                        //DvGV.Visible = false;
                        GVUsers.Visible = false;
                        BtnDlt.Visible = false;
                        PopUp("No data found!");
                    }
                    else
                    {
                        GVUsers.Visible = true;
                        //DvGV.Visible = true;
                        BtnDlt.Visible = true;
                    }
                }
                BtnDlt.Visible = GVUsers.Visible;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        private void ResetTabVu()
        {
            //DdlRoleV.SelectedIndex = 0;
            //DdlLocnV.SelectedIndex = 0;
            //DdlUser.SelectedIndex = 0;
            GVUsers.Visible = false;
            BtnDlt.Visible = false;
        }

        protected void BtnDlt_Click(object sender, EventArgs e)
        {
            string jsonParam = ConstructJSON();
            if (string.IsNullOrWhiteSpace(jsonParam))
            {
                PopUp("Error: The selected users could not be deleted!");
                return;
            }
            if (SubMission("SP_Users_Delete", jsonParam))
            {
                PopUp("User(s) deleted successfully!");
                ResetGVUsers();
                DdlUser.DataBind();
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
            int sNoColIndx = -1, chkCnt = 0;

            GVUsers.Rows.Cast<GridViewRow>().ToList().ForEach(gvRow => chkCnt += ((CheckBox)gvRow.Cells[0].Controls[1]).Checked ? 1 : 0);

            try
            {
                sNoColIndx = GVUsers.Columns.IndexOf(GVUsers.Columns.Cast<DataControlField>().ToList().First(col => col.HeaderText.Trim().ToUpper() == "SNO"));
            }
            catch (Exception) { }

            if (sNoColIndx < 0)
            {
                PopUp("Column \"Sno\" was not found in GridView!");
                return Emp;
            }
            //chkDltCntReports = ToInt16(HFCnt.Value);

            foreach (GridViewRow gvRow in GVUsers.Rows)
            {
                if (chkCnt < 1) break;

                DataRow dRo = GVUsersDS.Select("Sno = " + gvRow.Cells[sNoColIndx].Text)?[0];
                if (dRo is null) continue;

                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (!cb.Checked) continue;

                string id = dRo["Rec_Id"].ToString();
                Dictionary<string, string> paramVals = new Dictionary<string, string>()
                {
                    { "REC_ID", id },
                    { "MODIFIED_BY", LoggdUsrName?.ToString().Trim() },
                    { "MODIFIED_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") }
                };
                dtls.Add(paramVals);
                cb.Checked = false;
                chkCnt--;
            }
            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            ResetGVUsers();
            BtnDlt.Enabled = false;
        }

        private void ResetGVUsers()
        {
            GVUsers.DataSource = null;
            GVUsersDS = null;
            GVUsers.DataBind();
        }

        protected void DdlUser_DataBinding(object sender, EventArgs e)
        {
            if (!(sender is null))
            {
                FillDdl(DdlUser, "SP_Get_Users", Emp, All, null,
                    new OleDbParameter[]
                    {
                        new OleDbParameter("@Role_Id", DdlRoleV.SelectedValue),
                        new OleDbParameter("@Location_Id", DdlLocnV.SelectedValue)
                    },
                    "User_Name", "User_Id"
                );
            }
        }

        private void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        protected void BtnAction_Click(object sender, EventArgs e)
        {
            if (!(sender is LinkButton editBtn))
            {
                PopUp("Error in editing!");
                return;
            }
            int rowIndex = ((GridViewRow)editBtn.NamingContainer).RowIndex;
            DataRow dRo = GVUsersDS?.Select($"Sno = {rowIndex + 1}")[0];

            if (dRo is null || dRo.ItemArray.Length == 0)
            {
                PopUp("Error in editing!");
                return;
            }
            string userId = dRo["User_Id"]?.ToString();
            string locId = dRo["Location_Id"]?.ToString().Trim();
            string roleId = dRo["Role_Id"]?.ToString();

            DdlRoleA.SelectedValue = roleId;
            DdlLocnA.SelectedValue = locId;

            TxtUsrId.Enabled = false;
            TxtUsrId.Text = userId;
            TxtUsrId.Enabled = false;
            TxtAddress.Text = dRo["Address"]?.ToString();
            TxtPassword.Text = dRo["Password"]?.ToString();
            TxtConfirmPassword.Text = dRo["Password"]?.ToString();
            DivCPswd.Visible = false;
            DivPswd.Visible = false;
            HFRecIdA.Value = dRo["Rec_Id"]?.ToString();
            TxtEmail.Text = dRo["Email"]?.ToString();

            BtnCancel.Visible = true;
            BtnRegister.Text = "Save";
            MultiView1.ActiveViewIndex = 0;
            Menu.Items[0].Selected = true;
            Menu.Items[0].Text = "Edit User |";
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Menu.Items[0].Text = "Add User |";
            Menu.Items[1].Selected = true;
            MultiView1.ActiveViewIndex = 1;
        }

        protected void DdlLocn_DataBinding(object sender, EventArgs e)
        {
            if (sender is DropDownList ddl)
            {
                string selectTxt = sender.Equals(DdlLocnV) ? All : Select;
                FillDdl(ddl, "SP_Get_Locations", Emp, selectTxt, null, null, "Loc_Name", "Loc_Id");
            }
        }

        protected void DdlLocn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is null) && sender.Equals(DdlLocnV))
            {
                DdlLocnV.ToolTip = DdlLocnV.SelectedItem.Text;
                DdlUser.DataBind();
            }
        }

        protected void DdlRole_DataBinding(object sender, EventArgs e)
        {
            if (sender is DropDownList ddl)
            {
                string selectTxt = sender.Equals(DdlRoleV) ? All : Select;
                FillDdl(ddl, "SP_Get_Roles", "0", selectTxt, null, null, "Role_Name", "Role_Id");
            }
        }

        protected void DdlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is null) && sender.Equals(DdlRoleV))
            {
                DdlRoleV.ToolTip = DdlRoleV.SelectedItem.Text;
                DdlUser.DataBind();
            }
        }

        private void FillDdl(DropDownList ddl, string proc, string selectVal, string selectTxt = All, DropDownList prntDdl = null, OleDbParameter[] paramCln = null, string TxtField = Emp, string ValField = Emp)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(selectTxt, selectVal));
            ddl.SelectedValue = selectVal;
            ddl.ToolTip = selectTxt;

            DataTable dt;
            if (prntDdl != null && prntDdl.SelectedIndex == 0)
                return;
            try
            {
                dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

                if (dt != null && dt.Rows.Count > 0)
                    if ((TxtField != Emp) && ValField != Emp)
                        for (int i = 0; i < dt.Rows.Count; i++)
                            ddl.Items.Add(new ListItem(dt.Rows[i][TxtField].ToString(), dt.Rows[i][ValField].ToString()));
                    else
                        for (int i = 0; i < dt.Rows.Count; i++)
                            ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            bool Add = BtnRegister.Text == "Register";
            if (ValidateDtls(Add))
            {
                string usrId = TxtUsrId.Text;
                string usrName = TxtUsrName.Text;
                string pswd = TxtPassword.Text;
                int slctdRoleId = Convert.ToInt32(DdlRoleA.SelectedValue);
                string email = TxtEmail.Text;
                string locId = DdlLocnA.SelectedValue == Emp ? null : DdlLocnA.SelectedValue.ToUpper();
                string adrs = TxtAddress.Text;
                string locn = DdlLocnA.SelectedItem.Text;
                string slctdRole = DdlRoleA.SelectedItem.Text;
                long rec_Id = IsEmpty(HFRecIdA.Value) ? 0 : Convert.ToInt64(HFRecIdA.Value);
                char loginType = LoginTypes[DdlRoleA.SelectedValue].First();

                if (!Add)
                {
                    EditUser(usrId, usrName, slctdRoleId, email, locId, adrs, loginType, locn, slctdRole, rec_Id);
                    ResetGVUsers();
                    BtnCancel_Click(BtnCancel, null);
                    return;
                }
                else
                {
                    AddUser(usrId, usrName, slctdRoleId, email, locId, adrs, loginType, locn, slctdRole, pswd);
                    ResetCtrls();
                    return;
                }
            }
        }

        private bool AddUser(string usrId, string usrName, int slctdRoleId, string email, string locId, string adrs, char loginType, string slctdLocn, string slctdRole, string pswd)
        {
            bool success = true;
            try
            {
                string userIpAdrs = HttpContext.Current.Request.UserHostAddress.ToUpper();
                int usrRoleId = Convert.ToInt32(Session["Role_Id"]?.ToString());
                if (usrRoleId != 4 && slctdRoleId == 4)
                {
                    PopUp("You are not authorized to create a super admin user!");
                    return false;
                }
                OleDbParameter[] paramCln =
                {
                        new OleDbParameter("@User_Name", usrName),
                        new OleDbParameter("@Role_Id", slctdRoleId),
                        new OleDbParameter("@EMail", email),
                        new OleDbParameter("@Login_Type", loginType),
                        new OleDbParameter("@Location_Id", locId),
                        new OleDbParameter("@Address", adrs),
                        new OleDbParameter("@Created_By", LoggdUsrName),
                        new OleDbParameter() {
                            ParameterName = "@Rec_Id",
                            Value = 0,
                            OleDbType = OleDbType.BigInt
                        },
                        new OleDbParameter("@User_Id", usrId ),
                        new OleDbParameter("@Password", pswd ),
                        new OleDbParameter("@IP_Address", userIpAdrs)
                    };

                var output = DBOprn.ExecScalarProc("SP_Add_Edit_User", DBOprn.ConnPrimary, paramCln);

                if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                {
                    PopUp(output.ToString());
                    return false;
                }
                PopUp("User added successfully!");
                if (!(Network is null || Settings is null))
                    SendAddMail(email, usrName, usrId, pswd, slctdLocn, slctdRole);
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return false;
            }
            return true;
        }

        private bool EditUser(string usrId, string usrName, int slctdRoleId, string email, string locId, string adrs, char loginType, string slctdLocn, string slctdRole, long rec_Id)
        {
            bool success = true;
            try
            {
                OleDbParameter[] paramCln =
                {
                    new OleDbParameter("@User_Name", usrName),
                    new OleDbParameter("@Role_Id", slctdRoleId),
                    new OleDbParameter("@EMail", email),
                    new OleDbParameter("@Login_Type", loginType),
                    new OleDbParameter("@Location_Id", locId),
                    new OleDbParameter("@Address", adrs),
                    new OleDbParameter("@Created_By", LoggdUsrName),
                        new OleDbParameter() {
                            ParameterName = "@Rec_Id",
                            Value = rec_Id,
                            OleDbType = OleDbType.BigInt
                        },
                    new OleDbParameter("@User_Id", usrId),
                    new OleDbParameter("@Password", Emp),
                };
                var output = DBOprn.ExecScalarProc("SP_Add_Edit_User", DBOprn.ConnPrimary, paramCln);

                if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                {
                    PopUp(output.ToString());
                    return false;
                }
                PopUp("User details updated successfully!");

                if (!(Network is null || Settings is null))
                    SendEditMail(usrName, slctdRole, email, slctdLocn, adrs);
            }
            catch (Exception ex)
            {
                throw;
            }
            return success;
        }

        private void SendEditMail(string usrName, string slctdRole, string email, string locn, string adrs)
        {
            string subject = "Update in Profile";
            string usrNameDtl =  IsEmpty(usrName) ? Emp :$"<p><b>User Name</b>: {usrName}</p>";
            string slctdRoleDtl =  IsEmpty(slctdRole) ? Emp :$"<p><b>Role</b>: {slctdRole}</p>";
            string emailDtl = IsEmpty(email) ? Emp : $"<p><b>Email</b>: {email}</p>";
            string adrsDtl = IsEmpty(adrs) ? Emp : $"<p><b>Address</b>: {adrs}</p>";
            try
            {
                string msg = $@"</br></br></br>
                                <p>Dear,</p>
                                <p>{LoggdUsrName} has edited your details in Finance Tracker Application.</p>  
                                <p>Please find your details below:</p>                                                                  
                                {usrNameDtl}{slctdRoleDtl}{emailDtl}{adrsDtl}                                 
                                <p>You can now <a href=""http://10.10.1.171:88/Account/Login"">login</a> to your account.</p>
                                <p>For more information, please contact <a href=""mailto:{LoggdUsrMail}"">{LoggdUsrName}</a></p>
                                <p>This is an automatically generated emailId. Please do not reply as there will be no responses.</p>
                                <p>Best Regards,</p> 
                                <p>Grupo Bimbo</p>";
                SendMail(subject, msg, email);
            }
            catch (Exception e)
            {
                PopUp(e.Message);
            }
        }

        private void SendAddMail(string email, string usrName, string usrId, string pswd, string locn, string slctdRole)
        {
            string subject = "Welcome to Finance Tracker";
            try
            {
                string msg = $@"</br></br></br>
                                <p>Dear {usrName},</p>
                                <p>{LoggdUsrName} has registered you as a user in Finance Tracker Application.</p>  
                                <p>Please find your credentials below:</p>                                                                  
                                <p><b>User Id</b>: {usrId}</p>
                                <p><b>Password</b>: {pswd}</p>                                  
                                <p><b>Location</b>: {locn}</p>                                  
                                <p><b>Role</b>: {slctdRole}</p>                                  
                                <p>You can now <a href=""http://10.10.1.171:88/Account/Login"">login</a> to your account.</p>
                                <p>For more information, please contact <a href=""mailto:{LoggdUsrMail}"">{LoggdUsrName}</a></p>
                                <p>This is an automatically generated emailId. Please do not reply as there will be no responses.</p>
                                <p>Best Regards,</p> 
                                <p>Grupo Bimbo</p>";
                SendMail(subject, msg, email);
            }
            catch (Exception e)
            {
                PopUp(e.Message);
            }
        }

        private void SendMail(string subject, string msg, string emailId)
        {
            if (Network is null) return;
            if (Settings is null) return;

            Host = Host ?? Network.Host;
            Port = Port == 0 ? Network.Port : Port;
            HostName = IsEmpty(HostName) ? Network.UserName : HostName;
            From = IsEmpty(From) ? Settings.From : From;

            using (SmtpClient smtp = new SmtpClient(DBOperations.Network.Host, DBOperations.Network.Port)) // Your SMTP server
            {
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(DBOperations.Network.UserName, DBOperations.Network.Password); // Your emailId credentials
                smtp.EnableSsl = false; // Enable SSL if required                   

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(DBOperations.Settings.From, DBOperations.Network.UserName), // Your emailId address
                    Subject = subject,
                    Body = msg,
                    IsBodyHtml = true
                };
                mail.To.Add(emailId);
                smtp.Send(mail);
            }
        }

        private bool ValidateDtls(bool add)
        {
            if (string.IsNullOrWhiteSpace(TxtUsrId.Text))
            {
                PopUp("User Id is required");
                TxtUsrId.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtUsrName.Text))
            {
                PopUp("User Name is required");
                TxtUsrName.Focus();
                return false;
            }
            if (add)
            {
                if (string.IsNullOrWhiteSpace(TxtPassword.Text))
                {
                    PopUp("Password is required");
                    TxtEmail.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace(TxtConfirmPassword.Text))
                {
                    PopUp("Confirm Password is required");
                    TxtConfirmPassword.Focus();
                    return false;
                }
                if (!TxtConfirmPassword.Text.Equals(TxtPassword.Text))
                {
                    PopUp("The password and confirmation password do not match.");
                    TxtConfirmPassword.Focus();
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                PopUp("Email is required");
                TxtPassword.Focus();
                return false;
            }
            try
            {
                MailAddress mail = new MailAddress(TxtEmail.Text);
            }
            catch (FormatException e)
            {
                PopUp("Either email address is not in a recognized format or it contains non-ASCII characters.\n Please enter a valid email address!");
                return false;
            }
            catch (Exception e)
            {
                PopUp(e.Message);
                return false;
            }
            if (DdlRoleA.SelectedValue == "0")
            {
                PopUp("Role is required");
                DdlRoleA.Focus();
                return false;
            }
            if (DdlLocnA.SelectedValue == "" && DdlRoleA.SelectedValue != "4")
            {
                PopUp("Location is required for user other than Super Admin!");
                DdlLocnA.Focus();
                return false;
            }
            return true;
        }

        private void ResetCtrls()
        {
            TxtUsrId.Text = "";
            TxtUsrName.Text = "";
            TxtPassword.Text = "";
            TxtConfirmPassword.Text = "";
            TxtEmail.Text = "";
            DdlRoleA.SelectedIndex = 0;
            DdlLocnA.SelectedIndex = 0;
            TxtAddress.Text = "";
            TxtUsrId.Enabled = true;
        }
    }
}