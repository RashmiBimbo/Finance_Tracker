using System;
using System.Web.UI;
using System.Data.OleDb;

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
                    Response.Redirect("~/Account/Login.aspx");
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
                        new OleDbParameter("@User_Id", Session["User_Id"].ToString()),
                        new OleDbParameter("@OldPswd", TxtOldPswd.Text),
                        new OleDbParameter("@Password", TxtNewPswd.Text),
                        new OleDbParameter("@ChangePswdDate", DateTime.Today),
                        new OleDbParameter("@Modified_By", Session["User_Name"].ToString()),
                    }
                );

                if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                {
                    PopUp(output.ToString());
                    return;
                }
                PopUp("Password changed successfully!");
                Session["Changed_Password"] = true;
                Session.Abandon();
                Session.RemoveAll();
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        private bool ValidateDtls()
        {
            if (string.IsNullOrWhiteSpace(TxtOldPswd.Text))
            {
                PopUp("Old password is required");
                TxtOldPswd.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(TxtNewPswd.Text))
            {
                PopUp("Confirm Password is required");
                TxtNewPswd.Focus();
                return false;
            }
            if (!TxtConfirmPswd.Text.Equals(TxtConfirmPswd.Text))
            {
                PopUp("The password and confirmation password do not match.");
                TxtConfirmPswd.Focus();
                return false;
            }
            return true;
        }
        public void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }
    }
}