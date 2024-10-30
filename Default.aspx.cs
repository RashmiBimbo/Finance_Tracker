using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

namespace Finance_Tracker
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string usrId = Session["User_Id"]?.ToString();
                if (usrId == null || usrId == "")
                {
                    Response.Redirect("~/Account/Login");
                    return;
                }
                if (!(bool)Session["Changed_Password"])
                {
                    Response.Redirect("~/Account/ResetPassword");
                    return;
                }
            }
        }

    }
}