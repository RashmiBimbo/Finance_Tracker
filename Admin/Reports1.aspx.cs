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
    public partial class Reports1 : Page
    {
        private DBOperations connect = new DBOperations();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string RoleId = Session["roleId"]?.ToString();
                if (RoleId != "1")
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }
                else
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
                DdlCat.DataBind();
            }
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            DdlCat.Items.Clear();
            DataTable dt = connect.GetDataProc("SP_Get_Categories", connect.ConnPrimary);
            if (dt != null && dt.Rows.Count > 0)
            {
                DdlCat.DataSource = dt;
                DdlCat.DataTextField = "Type_Name";
                DdlCat.DataValueField = "Type_Id";
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string Name = TxtReportName.Text.ToUpper();
                string cat = DdlCat.SelectedValue;
                string priority = TxtPriority.Text;
                string weight = TxtWeight.Text;
                string typeId = DdlType.SelectedValue;
                try
                {
                    OleDbParameter[] paramCln =
                    {
                        new OleDbParameter("@Name", Name),
                        new OleDbParameter("@Category_Id", cat),
                        new OleDbParameter("@Priority", priority),
                        new OleDbParameter("@Weight", weight),
                        new OleDbParameter("@Type_Id", typeId),
                        new OleDbParameter("@Due_Date", typeId),
                        new OleDbParameter("@Created_By", Session["User_Name"])
                    };

                    var output = connect.ExecScalarProc("SP_Create_Report", connect.ConnPrimary, paramCln);

                    if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                    {
                        PopUp(output.ToString());
                        return;
                    }
                    PopUp("Report added successfully!");
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
            TxtReportName.Text = "";
            DdlCat.SelectedIndex = 0;
            GVReport.DataBind();
        }

        protected void GVReport_DataBinding(object sender, EventArgs e)
        {
            DataTable dt = connect.GetDataProc("SP_Get_Categories", connect.ConnPrimary);
            GVReport.DataSource = dt;
        }

        private void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }
    }
}