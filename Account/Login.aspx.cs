using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Finance_Tracker.Models;
using System.Data;
using System.Web.UI.WebControls;

namespace Finance_Tracker.Account
{
    public partial class Login : Page
    {
        private DBOperations connect = new DBOperations();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!connect.AuthenticatCon())
            {
                ShowPopUpMsg("Database connection could not be established");
                return;
            }
            if (!Page.IsPostBack)
            {
                Session.Abandon();
                Session.RemoveAll();
                DdlLocn.DataBind();
                var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    //RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
                }
                if (Request.Browser.Cookies)
                {
                    if (!string.IsNullOrEmpty(Response.Cookies["UserName"]?.Value))
                        TxtUserId.Text = Response.Cookies["UserName"].Value;
                    if (!string.IsNullOrEmpty(Response.Cookies["PassWord"]?.Value))
                        TxtUserId.Text = Response.Cookies["PassWord"].Value;
                }
            }
            //RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
        }

        //protected void LogIn(object sender, EventArgs e)
        //{
        //    if (IsValid)
        //    {
        //        // Validate the user password
        //        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

        //        // This doen't count login failures towards account lockout
        //        // To enable password failures to trigger lockout, change to shouldLockout: true
        //        var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

        //        switch (result)
        //        {
        //            case SignInStatus.Success:
        //                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
        //                break;
        //            case SignInStatus.LockedOut:
        //                Response.Redirect("/Account/Lockout");
        //                break;
        //            case SignInStatus.RequiresVerification:
        //                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
        //                                                Request.QueryString["ReturnUrl"],
        //                                                RememberMe.Checked),
        //                                  true);
        //                break;
        //            case SignInStatus.Failure:
        //            default:
        //                FailureText.Text = "Invalid login attempt";
        //                ErrorMessage.Visible = true;
        //                break;
        //        }
        //    }
        //}

        protected void BtnLogIn_Click(object sender, EventArgs e)
        {
            string locn = DdlLocn.SelectedValue;
            string UsrId = TxtUserId.Text;
            string pswrd = TxtPassword.Text;
            try
            {
                if (IsValid)
                {
                    //if (!ValidateUser(locn, UsrId, pswrd)) return;
                    //connect. GetDataProc("SP_Login_Validate", connect.ConnTestFinTrack, { locn});

                    string mSqlQuery = "EXEC [dbo].[SP_Login_Validate] '" + locn + "', '" + UsrId + "', '" + pswrd + "'";
                    DataTable dt = connect.SelQuery(mSqlQuery, connect.ConnTestFinTrack);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        bool active = (bool)dt.Rows[0]["Active"];
                        if (!active)
                        {
                            ShowPopUpMsg("User exists but it is not active!");
                            return;
                        }
                        if (Request.Browser.Cookies)
                        {
                            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(1);
                            Response.Cookies["UserName"].Value = UsrId;
                        }

                        if (CBRemMe.Checked)
                        {
                            Response.Cookies["PassWord"].Expires = DateTime.Now.AddDays(1);
                            Response.Cookies["PassWord"].Value = pswrd;
                        }
                        FillSession(dt);
                        //Response.Redirect("~/Home.aspx");
                    }
                    else
                        ShowPopUpMsg("Kindly check your Location or ID or Password");
                }
            }
            catch (Exception ex)
            {
                ShowPopUpMsg( ex.Message);
            }
        }

        private void FillSession(DataTable dt)
        {

            Session["User_Id"] = dt.Rows[0]["User_Id"];
            Session["Password"] = dt.Rows[0]["Password"];
            Session["User_Name"] = dt.Rows[0]["User_Name"];
            Session["Company_Id"] = dt.Rows[0]["Company_Id"];
            Session["Sub_Company_Id"] = dt.Rows[0]["Sub_Company_Id"];
            Session["Role_Id"] = dt.Rows[0]["Role_Id"];
            Session["Role_Name"] = dt.Rows[0]["Role_Name"];
            Session["Login_Type"] = dt.Rows[0]["Login_Type"];
            Session["Active"] = (bool)dt.Rows[0]["Active"];
            Session["Flag"] = (bool)dt.Rows[0]["Flag"];
            Session["Change_Password_Date"] = dt.Rows[0]["Change_Password_Date"];
            Session["Address"] = dt.Rows[0]["Address"];
            Session["IP_Address"] = dt.Rows[0]["IP_Address"];
            Session["Location_Id"] = dt.Rows[0]["Location_Id"];
            Session["Created_Date"] = dt.Rows[0]["Created_Date"];
            Session["Created_By"] = dt.Rows[0]["Created_By"];
            Session["Modified_Date"] = dt.Rows[0]["Modified_Date"];
            Session["Modified_By"] = dt.Rows[0]["Modified_By"];
        }

        private bool ValidateUser(params string[] args)
        {
            //Session["Master_File"] = "~/Normal/Normal_.master";
            if (string.IsNullOrEmpty(args[0]))
            {
                this.ShowPopUpMsg("Please select location ");
                DdlLocn.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(args[1]))
            {
                this.ShowPopUpMsg("Please enter User Id");
                TxtUserId.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(args[2]))
            {
                this.ShowPopUpMsg("Please enter Password");
                TxtPassword.Focus();
                return false;
            }
            return true;
        }

        private void ShowPopUpMsg(string strMessage)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + strMessage + "');", true);
        }

        protected void DdlLocn_DataBinding(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = connect.GetDataProc("SP_Get_Locations", connect.ConnTestFinTrack);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DdlLocn.DataSource = dt;
                    DdlLocn.DataTextField = "Loc_Name";
                    DdlLocn.DataValueField = "Loc_Id";
                }
            }
            catch (Exception ex)
            {
                ShowPopUpMsg(ex.Message);
            }
        }

        protected void BtnLogIn1_Click(object sender, EventArgs e)
        {

        }
    }
}