using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using Finance_Tracker;

namespace DemandApp
{
    public partial class BBISiteNew : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        private DBOperations connect = new DBOperations();
        string Roleid = string.Empty;

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
            if (!IsPostBack)
            {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            connect.AuthenticatConns();
            if (!Page.IsPostBack)
            {
                Roleid = Convert.ToString(Session["RoleId"]);
                if (Roleid == null || Roleid == "")
                {
                    Roleid = "0";
                }

                //get_menuitem(Roleid);

            }
            //DataTable dt = this.BindMenuData(0);
            //DynamicMenuControlPopulation(dt, 0, null);
            //string msqlquery = "select LEAFID,PARENTID,LEAFNAME,LEAFPATH FROM USERLOGINURL ";
            // DataTable dt = GetDataSrc(msqlquery);

            // DataRow[] parentMenus = dt.Select("ParentId is null or parentid=0");

            // dynamic sb = new StringBuilder();
            // string unorderedList = GenerateUL(parentMenus, dt, sb);
            // Response.Write(unorderedList);
        }

        private string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb)
        {
            sb.AppendLine("<ul>");

            if (menu.Length > 0)
            {
                foreach (DataRow dr in menu)
                {
                    //Dim handler As String = dr("Handler").ToString()
                    string menuText = dr["LEAFNAME"].ToString();
                    string line = String.Format("<li><a href='#'>{0}</a>", menuText);
                    sb.Append(line);

                    string pid = dr["LEAFID"].ToString();

                    DataRow[] subMenu = table.Select(String.Format("ParentId = {0}", pid));
                    if (subMenu.Length > 0)
                    {
                        dynamic subMenuBuilder = new StringBuilder();
                        sb.Append(GenerateUL(subMenu, table, subMenuBuilder));
                    }
                    sb.Append("</li>");
                }
            }

            sb.Append("</ul>");
            return sb.ToString();
        }

        public void get_menuitem(string roleid)
        {
            string msqlquery = "";
            msqlquery = "select LEAFID,PARENTID,LEAFNAME,LEAFPATH FROM USERLOGINURL where PARENTID is null and  Roleid in (" + roleid + ")";
            DataTable dt = GetDataSrc(msqlquery);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataView viewitem = new DataView(dt);
                foreach (DataRowView childView in viewitem)
                {
                    MenuItem menuItem = new MenuItem(childView.Row["LEAFNAME"].ToString(), childView.Row["LEAFID"].ToString());
                    menuItem.NavigateUrl = childView.Row["LEAFPATH"].ToString();
                    Menu1.Items.Add(menuItem);
                    AddChildItems(dt, menuItem);
                }
            }
        }

        private void AddChildItems(DataTable dt, MenuItem menuitem)
        {
            string msqlquery = "select LEAFID,PARENTID,LEAFNAME,LEAFPATH FROM USERLOGINURL where   PARENTID in (" + Convert.ToString(menuitem.Value) + ")";
            DataTable dt1 = GetDataSrc(msqlquery);

            DataView viewitem = new DataView(dt1);
            foreach (DataRowView childView in viewitem)
            {
                MenuItem childItem = new MenuItem(childView.Row["LEAFNAME"].ToString(), childView.Row["LEAFID"].ToString());
                childItem.NavigateUrl = childView.Row["LEAFPATH"].ToString();
                menuitem.ChildItems.Add(childItem);
                AddChildItems(dt, childItem);
            }
        }

        private DataTable GetDataSrc(string query)
        {
            DataTable dt;
            try
            {
                dt = connect.SelQuery(query, connect.ConnPrimary);
                if (dt != null && dt.Rows.Count > 0)
                    return dt;
                return null;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", ex.Message, false);
                return null;
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }

}