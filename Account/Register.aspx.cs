using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Finance_Tracker.Models;
using System.Data;
using System.Data.OleDb;

namespace Finance_Tracker.Account
{

    public partial class Register : Page
    {
        private DBOperations connect = new DBOperations();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!connect.AuthenticatConns())
            {
                PopUp("Database connection could not be established");
                return;
            }
            if (!Page.IsPostBack)
            {
                string usrId = Session["User_Id"]?.ToString();
                if (usrId == null || usrId == "")
                    Response.Redirect("~/Account/Login.aspx");

                string RoleId = Session["Role_Id"]?.ToString();
                if (RoleId != "1")
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                DdlRole.DataBind();
                DdlLocn.DataBind();
            }
        }

        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            if (ValidateDtls())
            {
                string usrId = TxtUsrId.Text.ToUpper();
                string usrName = TxtUsrName.Text;
                string pswd = TxtPassword.Text;
                string roleId = DdlRole.SelectedValue;
                string locId = DdlLocn.SelectedValue.ToUpper();
                string adrs = TxtAddress.Text;
                try
                {
                    string userIpAdrs = HttpContext.Current.Request.UserHostAddress.ToUpper();

                    OleDbParameter[] paramCln =
                    {
                        new OleDbParameter("@User_Id", usrId ),
                        new OleDbParameter("@Password", pswd ),
                        new OleDbParameter("@User_Name", usrName),
                        new OleDbParameter("@Company_Id", "BBI"),
                        new OleDbParameter("@Sub_Company_Id", "BBI"),
                        new OleDbParameter("@Role_Id", roleId),
                        new OleDbParameter("@Login_Type", null),
                        new OleDbParameter("@Active", 1),
                        new OleDbParameter("@Flag", 1),
                        new OleDbParameter("@Change_Password_Date", null),
                        new OleDbParameter("@Address", adrs),
                        new OleDbParameter("@IP_Address", userIpAdrs),
                        new OleDbParameter("@Location_Id", locId),
                        new OleDbParameter("@Created_Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                        new OleDbParameter("@Created_By", Session["User_Name"])
                    };

                    var output = connect.ExecScalarProc("SP_Register_User", connect.ConnPrimary, paramCln);

                    if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                    {
                        PopUp(output.ToString());
                        return;
                    }
                    PopUp("User added successfully!");
                    ResetCtrls();
                }
                catch (Exception ex)
                {
                    PopUp(ex.Message);
                }
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
                TxtPassword.Focus();
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
            if (DdlRole.SelectedValue == "0")
            {
                PopUp("Role is required");
                DdlRole.Focus();
                return false;
            }
            if (DdlRole.SelectedValue != "1" && DdlLocn.SelectedValue == "0")
            {
                PopUp("Location is required for user other than Admin!");
                DdlLocn.Focus();
                return false;
            }
            return true;
        }

        public void PopUp(string msg)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        private void ResetCtrls()
        {
            TxtUsrId.Text = "";
            TxtUsrName.Text = "";
            TxtPassword.Text = "";
            DdlRole.SelectedIndex = 0;
            DdlLocn.SelectedIndex = 0;
            TxtAddress.Text = "";
        }

        protected void DdlLocn_DataBinding(object sender, EventArgs e)
        {
            DdlLocn.Items.Clear();
            DataTable dt = connect.GetDataProc("SP_Get_Locations", connect.ConnPrimary);
            if (dt != null && dt.Rows.Count > 0)
            {
                DdlLocn.DataSource = dt;
                DdlLocn.DataTextField = "Loc_Name";
                DdlLocn.DataValueField = "Loc_Id";
            }
        }

        protected void DdlRole_DataBinding(object sender, EventArgs e)
        {
            DdlRole.Items.Clear();
            DataTable dt = connect.GetDataProc("SP_Get_Roles", connect.ConnPrimary);
            if (dt != null && dt.Rows.Count > 0)
            {
                DdlRole.DataSource = dt;
                DdlRole.DataTextField = "Role_Name";
                DdlRole.DataValueField = "Role_Id";
            }
        }
    }
}