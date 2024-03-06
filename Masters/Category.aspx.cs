using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_Tracker.ADMIN
{
    public partial class Category : System.Web.UI.Page
    {
        private DBOperations connect = new DBOperations();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //string RoleId = Session["roleId"]?.ToString();
                //if (RoleId != "1")
                //{
                //    Response.Redirect("~/Default.aspx");
                //    return;
                //}
                //else
                {
                    MultiView1.ActiveViewIndex = 0;
                    Menu1_MenuItemClick(null, null);
                }
            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            MultiView1.ActiveViewIndex = e is null ? 0 : int.Parse(e.Item.Value);
            if (Menu1.SelectedValue == "0")
            {
                //DdlType.DataBind();
            }
        }

        protected void DdlType_DataBinding(object sender, EventArgs e)
        {
            DdlType.Items.Clear();
            DataTable dt = connect.GetDataProc("SP_Get_CategoryTypes", connect.ConnPrimary);
            if (dt != null && dt.Rows.Count > 0)
            {
                DdlType.DataSource = dt;
                DdlType.DataTextField = "Type_Name";
                DdlType.DataValueField = "Type_Id";
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string Name = TxtCatName.Text.ToUpper();
                string typeId = DdlType.SelectedValue;
                try
                {
                    OleDbParameter[] paramCln =
                    {
                        new OleDbParameter("@Name", Name),
                        new OleDbParameter("@Type_Id", typeId),
                        new OleDbParameter("@Created_By", Session["User_Name"])
                    };

                    var output = connect.ExecScalarProc("SP_Create_Category", connect.ConnPrimary, paramCln);

                    if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                    {
                        PopUp(output.ToString());
                        return;
                    }
                    PopUp("Category added successfully!");
                    ResetCtrls();
                }
                catch (Exception ex)
                {
                    PopUp(ex.Message);
                }
            }
        }

        private void ResetCtrls()
        {
            TxtCatName.Text = "";
            DdlType.SelectedIndex = 0;
            SetTooltip(DdlType);
            GVCategory.DataBind();
        }

        protected void GVCategory_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = connect.GetDataProc("SP_Get_Categories", connect.ConnPrimary);
            GVCategory.DataSource = dt;
        }

        private void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        private void SetTooltip(DropDownList ddl)
        {
            ddl.ToolTip = ddl.SelectedItem.Text;
        }

    }
}