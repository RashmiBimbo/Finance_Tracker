﻿using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using static Finance_Tracker.DBOperations;

namespace Finance_Tracker
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            object usrId = Session["User_Id"];
            if (usrId == null || usrId.ToString() == "")
                Response.Redirect("~/Account/Login");
            if (!IsPostBack)
            {
                SetAccess();

                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        private void SetAccess()
        {
            string roleId = Session["Role_Id"]?.ToString() ?? string.Empty;
            bool isApprover = Session["Is_Approver"] != null && Convert.ToBoolean(Session["Is_Approver"]);

            bool isAdmin = LoginTypes[roleId] == Admin;
            bool isSuprAdmin = LoginTypes[roleId] == SuperAdmin;

            LnkReview.Visible = isAdmin || isSuprAdmin;
            //LnkRegister.Visible = isAdmin || isSuprAdmin;
            LnkApprove.Visible = isApprover;
            LnkMasters.Visible = isAdmin || isApprover || isSuprAdmin;
            LnkReports.Visible = isAdmin || isApprover || isSuprAdmin;
            LnkUTA.Visible = isAdmin || isApprover || isSuprAdmin;
            LnkUsers.Visible = isAdmin || isSuprAdmin;
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected void BtnLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("~/Account/Login");
        }
    }
}