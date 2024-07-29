using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Net.Configuration;
using System.Configuration;
using System.Net.NetworkInformation;
using static Finance_Tracker.DBOperations;

namespace Finance_Tracker.Account
{

    public partial class Register : Page
    {
        private DBOperations DBOprn = new DBOperations();
        int RoleId;
        private readonly string Emp = string.Empty;
        private static SmtpSection settings;
        private static SmtpNetworkElement Network;

        protected void Page_Load(object sender, EventArgs e)
        {
            RoleId = Convert.ToInt32(Session["Role_Id"]?.ToString());
            if (!DBOprn.AuthenticatConns())
            {
                PopUp("Database connection could not be established");
                return;
            }
            if (!Page.IsPostBack)
            {
                string usrId = Session["User_Id"]?.ToString();
                if (string.IsNullOrWhiteSpace(usrId))
                    Response.Redirect("~/Account/Login");

                //if (!(RoleId == 1 || RoleId == 4))
                //{
                //    Response.Redirect("~/Default");
                //    return;
                //}

                if (!IsSmtpConfigValid())
                    PopUp("Email settings could not be verified. No emails will be sent for registration!");
                DdlRole.DataBind();
                DdlLocn.DataBind();
                TxtUsrId.Attributes.Add("autocomplete", "off");
                TxtPassword.Attributes.Add("autocomplete", "off");
            }
        }

        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            if (ValidateDtls())
            {
                string usrId = TxtUsrId.Text;
                string usrName = TxtUsrName.Text;
                string pswd = TxtPassword.Text;
                string slctdRoleId = DdlRole.SelectedValue;
                string email = TBEmail.Text;
                string locId = DdlLocn.SelectedValue == Emp ? null : DdlLocn.SelectedValue.ToUpper();
                string adrs = TxtAddress.Text;
                char loginType = DBOperations.LoginTypes[DdlRole.SelectedValue].First();
                try
                {
                    string userIpAdrs = HttpContext.Current.Request.UserHostAddress.ToUpper();
                    int usrRoleId = Convert.ToInt32(Session["Role_Id"]?.ToString());
                    if (usrRoleId != 4 && slctdRoleId == "4")
                    {
                        PopUp("You are not authorized to create a super admin user!");
                        return;
                    }
                    OleDbParameter[] paramCln =
                    {
                        new OleDbParameter("@User_Id", usrId ),
                        new OleDbParameter("@Password", pswd ),
                        new OleDbParameter("@User_Name", usrName),
                        new OleDbParameter("@Company_Id", "BBI"),
                        new OleDbParameter("@Sub_Company_Id", "BBI"),
                        new OleDbParameter("@Role_Id", slctdRoleId),
                        new OleDbParameter("@EMail", email),
                        new OleDbParameter("@Login_Type", loginType),
                        new OleDbParameter("@Active", 1),
                        new OleDbParameter("@Flag", 1),
                        new OleDbParameter("@Change_Password_Date", null),
                        new OleDbParameter("@Address", adrs),
                        new OleDbParameter("@IP_Address", userIpAdrs),
                        new OleDbParameter("@Location_Id", locId),
                        new OleDbParameter("@Created_Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                        new OleDbParameter("@Created_By", Session["User_Name"])
                    };

                    var output = DBOprn.ExecScalarProc("SP_Register_User", DBOprn.ConnPrimary, paramCln);

                    if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                    {
                        PopUp(output.ToString());
                        return;
                    }
                    SendMail(email, usrName, usrId, pswd);
                    PopUp("User added successfully!");
                    ResetCtrls();
                }
                catch (Exception ex)
                {
                    PopUp(ex.Message);
                }
            }
        }

        private void SendMail(string email, string usrName, string usrId, string pswd)
        {
            string host = Network.Host, hostPswd = Network.Password, from = settings.From, hostName = Network.UserName;
            int port = Network.Port;
            string subject = "Welcome to Finance Tracker";
            try
            {
                string msg = $@"</br>
                                <p>Hi, {usrName}!</p>
                                <p>{Session["User_Name"]} has registered you as a user in Finance Tracker Application.</p>  
                                <p>Please find your credentials below:</p>                                                                  
                                <p><b>User Id</b>: {usrId}</p>
                                <p><b>Password</b>: {pswd}</p>                                  
                                <p>You can now <a href=""http://10.10.1.171:88/Account/Login"">login</a> to your account.</p>
                                <p>For more information, please contact <a href=""mailto:{Session["Email"]}"">{Session["User_Name"]}</a></p>
                                <p>This is an automatically generated mail. Please do not reply as there will be no responses.</p>
                                <p>Best Regards,</p> 
                                <p>Grupo Bimbo</p>";
                using (SmtpClient smtp = new SmtpClient(host, port)) // Your SMTP server
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(hostName, hostPswd); // Your email credentials
                    smtp.EnableSsl = false; // Enable SSL if required                   

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(from, hostName), // Your email address
                        Subject = subject,
                        Body = msg,
                        IsBodyHtml = true
                    };
                    mail.To.Add(email);
                    smtp.Send(mail);
                }
            }
            catch (Exception e)
            {
                PopUp(e.Message);
            }
        }

        private bool ValidateDtls()
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
            if (string.IsNullOrWhiteSpace(TxtPassword.Text))
            {
                PopUp("Password is required");
                TBEmail.Focus();
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
            if (string.IsNullOrWhiteSpace(TBEmail.Text))
            {
                PopUp("Email is required");
                TxtPassword.Focus();
                return false;
            }
            try
            {
                MailAddress mail = new MailAddress(TBEmail.Text);
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
            if (DdlRole.SelectedValue == "0")
            {
                PopUp("Role is required");
                DdlRole.Focus();
                return false;
            }
            if (DdlLocn.SelectedValue == "" && DdlRole.SelectedValue != "4")
            {
                PopUp("Location is required for user other than Super Admin!");
                DdlLocn.Focus();
                return false;
            }
            return true;
        }

        public void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        private void ResetCtrls()
        {
            TxtUsrId.Text = "";
            TxtUsrName.Text = "";
            TxtPassword.Text = "";
            TxtConfirmPassword.Text = "";
            TBEmail.Text = "";
            DdlRole.SelectedIndex = 0;
            DdlLocn.SelectedIndex = 0;
            TxtAddress.Text = "";
        }

        protected void DdlLocn_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlLocn, "SP_Get_Locations", Emp);
            string usrLocn = ((string)Session["Location_Id"]).ToUpper();
            if (string.IsNullOrWhiteSpace(usrLocn) && RoleId != 4) Response.Redirect("~/Account/Login");
            if (RoleId == 1)
            {
                DdlLocn.Enabled = false;
                DdlLocn.SelectedValue = usrLocn;
            }
            else
            {
                DdlLocn.Enabled = true;
                DdlLocn.SelectedIndex = 0;
            }
        }

        protected void DdlRole_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlRole, "SP_Get_Roles", "0");

            DdlRole.Items.FindByValue("4").Enabled = RoleId > 4;
            DdlRole.Items.FindByValue("1").Enabled = RoleId >= 4;
        }

        private void FillDdl(DropDownList ddl, String proc, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Select", selectVal));
            ddl.SelectedValue = selectVal;
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

    }
}