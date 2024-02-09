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
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Finance_Tracker.Account
{

    public partial class Register : Page
    {
        private DBOperations DBOprn = new DBOperations();

        private static readonly Dictionary<string, string> LoginTypes = new Dictionary<string, string>
        {
            { "1", "A" },
            { "2", "C" },
            { "3", "P" },
            { "4", "S" }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if ( !DBOprn.AuthenticatConns() )
            {
                PopUp("Database connection could not be established");
                return;
            }
            if ( !Page.IsPostBack )
            {
                string usrId = Session["User_Id"]?.ToString();
                if ( string.IsNullOrWhiteSpace(usrId) )
                    Response.Redirect("~/Account/Login.aspx");

                int RoleId = Convert.ToInt32(Session["Role_Id"]?.ToString());
                if ( !(RoleId == 1 || RoleId == 4 ))
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
            if ( ValidateDtls() )
            {
                string usrId = TxtUsrId.Text;
                string usrName = TxtUsrName.Text;
                string pswd = TxtPassword.Text;
                string slctdRoleId = DdlRole.SelectedValue;
                string email = TBEmail.Text;
                string locId = DdlLocn.SelectedIndex == 0 ? null : DdlLocn.SelectedValue.ToUpper();
                string adrs = TxtAddress.Text;
                string loginType = LoginTypes[DdlRole.SelectedValue].ToUpper();
                try
                {
                    string userIpAdrs = HttpContext.Current.Request.UserHostAddress.ToUpper();
                    int usrRoleId = Convert.ToInt32(Session["Role_Id"]?.ToString());
                    if ( usrRoleId != 4 && slctdRoleId == "4" )
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

                    if ( !string.IsNullOrWhiteSpace((string)output) ) //Error occurred
                    {
                        PopUp(output.ToString());
                        return;
                    }
                    PopUp("User added successfully!");
                    ResetCtrls();
                }
                catch ( Exception ex )
                {
                    PopUp(ex.Message);
                }
            }
        }

        private bool ValidateDtls()
        {
            if ( string.IsNullOrWhiteSpace(TxtUsrId.Text) )
            {
                PopUp("User Id is required");
                TxtUsrId.Focus();
                return false;
            }
            if ( string.IsNullOrWhiteSpace(TxtUsrName.Text) )
            {
                PopUp("User Name is required");
                TxtUsrName.Focus();
                return false;
            }
            if ( string.IsNullOrWhiteSpace(TxtPassword.Text) )
            {
                PopUp("Password is required");
                TBEmail.Focus();
                return false;
            }
            if ( string.IsNullOrWhiteSpace(TxtConfirmPassword.Text) )
            {
                PopUp("Confirm Password is required");
                TxtConfirmPassword.Focus();
                return false;
            }
            if ( !TxtConfirmPassword.Text.Equals(TxtPassword.Text) )
            {
                PopUp("The password and confirmation password do not match.");
                TxtConfirmPassword.Focus();
                return false;
            }
            if ( string.IsNullOrWhiteSpace(TBEmail.Text) )
            {
                PopUp("Email is required");
                TxtPassword.Focus();
                return false;
            }
            if ( DdlRole.SelectedValue == "0" )
            {
                PopUp("Role is required");
                DdlRole.Focus();
                return false;
            }
            if ( DdlLocn.SelectedValue == "" && DdlRole.SelectedValue != "4" )
            {
                PopUp("Location is required for user other than Super Admin!");
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
            TxtConfirmPassword.Text = "";
            TBEmail.Text = "";
            DdlRole.SelectedIndex = 0;
            DdlLocn.SelectedIndex = 0;
            TxtAddress.Text = "";
        }

        protected void DdlLocn_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlLocn, "SP_Get_Locations", "");
        }

        protected void DdlRole_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlRole, "SP_Get_Roles", "0");

            int roleId = Convert.ToInt32(Session["Role_Id"]);

            DdlRole.Items.FindByValue("4").Enabled = roleId > 4;
            DdlRole.Items.FindByValue("1").Enabled = roleId == 4;
        }

        private void FillDdl(DropDownList ddl, String proc, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Select", selectVal));
            ddl.SelectedIndex = 0;
            ddl.ToolTip = "Select";

            if ( prntDdl != null && prntDdl.SelectedIndex == 0 )
                return;
            try
            {
                DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

                if ( dt != null && dt.Rows.Count > 0 )
                {
                    for ( int i = 0; i < dt.Rows.Count; i++ )
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                    }
                }
            }
            catch ( Exception ex )
            {
                PopUp(ex.Message);
            }
        }

    }
}