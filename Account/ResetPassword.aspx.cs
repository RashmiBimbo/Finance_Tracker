using System;
using System.Web.UI;
using System.Data.OleDb;
using static Finance_Tracker.Common;

namespace Finance_Tracker.Account
{
    public partial class ResetPassword : Page
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
            }
        }

        protected void BtnOk_ServerClick(object sender, EventArgs e)
        {
            if (ValidateDtls())
            {
                DBOperations dbO = new DBOperations();
                var output = dbO.ExecScalarProc("SP_Change_Password", dbO.ConnPrimary,
                    new OleDbParameter[]
                    {
                        new OleDbParameter("@User_Id", Session["User_Id"]?.ToString().Trim().ToUpper()),
                        new OleDbParameter("@OldPswd", TxtOldPswd.Text),
                        new OleDbParameter("@Password", TxtNewPswd.Text),
                        new OleDbParameter("@ChangePswdDate", DateTime.Today),
                        new OleDbParameter("@Modified_By", Session["User_Name"].ToString()),
                    }
                );

                if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                {
                    PopUp(this, output.ToString());
                    return;
                }
                PopUp(this, "Password changed successfully!");
                Session["Changed_Password"] = true;
                Session.Abandon();
                Session.RemoveAll();
                Response.Redirect("~/Account/Login");
            }
        }

        private bool ValidateDtls()
        {
            if (string.IsNullOrWhiteSpace(TxtOldPswd.Text))
            {
                PopUp(this, "Old password is required");
                TxtOldPswd.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtNewPswd.Text))
            {
                PopUp(this, "New password is required");
                TxtNewPswd.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtConfirmPswd.Text))
            {
                PopUp(this, "Confirm password is required");
                TxtConfirmPswd.Focus();
                return false;
            }
            if (!TxtNewPswd.Text.Equals(TxtConfirmPswd.Text))
            {
                PopUp(this, "New password and confirm password do not match.");
                return false;
            }
            return true;
        }
    }
}