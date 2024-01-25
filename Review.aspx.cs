using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Finance_Tracker
{
    public partial class Review : System.Web.UI.Page
    {
        const string DateFormat = "dd-MMM-yyyy";
        const string MonthFormat = "MMM-yyyy";
        const string SqlDateFormat = "yyyy-MM-dd";

        private readonly DBOperations DBOprn = new DBOperations();

        #region Page Code

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DBOprn.AuthenticatConns())
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
                Menu1_MenuItemClick(Menu1, new MenuEventArgs(Menu1.Items[0]));
                DdlCatType_DataBinding(DdlCatType3, new EventArgs());
                DdlUsrType_DataBinding(DdlUsrType, new EventArgs());
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //Verifies that the control is rendered 
        }

        #endregion Page Code


        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            int eval = e is null ? 0 : int.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = eval;
            MultiView1.Views[eval].Focus();

            //User Clicked the menu item
            if (Menu1.SelectedValue == "0")
            {
                GVReports3.DataSource = null;
                GVReports3.Visible = false;
                TxtMnth3.Text = DateTime.Now.ToString(MonthFormat);
            }
        }

        protected void DdlCatType_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, childDdl = null;
            if (ddl.Equals(DdlCatType3))
            {
                childDdl = DdlCat3;
            }
            FillDdl(ddl, "SP_Get_CategoryTypes", "0");
        }

        protected void DdlCat_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = null;
            if (ddl.Equals(DdlCat3))
            {
                prntddl = DdlCatType3;
            }
            FillDdl(ddl, "SP_Get_Categories", "0", prntddl,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Type_Id", prntddl.SelectedValue)
                }
            );
        }

        protected void DdlReport_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender, prntddl = null;
            if (ddl.Equals(DdlReport3))
            {
                prntddl = DdlCat3;
            }

            FillDdl(ddl, "SP_Get_Reports", "0", prntddl,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Category_Id", prntddl.SelectedValue)
                }
            );
        }

        protected void DdlUsrType_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlUsrType, "SP_Get_Roles", "0");
        }

        protected void DdlUsr_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlUsr, "SP_Get_Users", "", DdlUsrType,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Role_Id", DdlUsrType.SelectedValue)
                }
            );
        }

        protected void DdlCatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList child = null;
            DropDownList grndchild = null;
            DropDownList ddl = (DropDownList)sender;
            ddl.ToolTip = ddl.SelectedItem.Text;
            if (sender.Equals(DdlCatType3))
            {
                child = DdlCat3;
                grndchild = DdlReport3;
            }
            child.DataBind();
            grndchild.DataBind();
        }

        protected void DdlCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList child = null;
            ddl.ToolTip = ddl.SelectedItem.Text;

            if (sender.Equals(DdlCat3))
                child = DdlReport3;
            child.DataBind();
        }

        protected void DdlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.ToolTip = ddl.SelectedItem.Text;
        }

        protected void DdlUsrType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlUsrType.ToolTip = DdlUsrType.SelectedItem.Text;
            DdlUsr.DataBind();
        }

        protected void DdlUsr_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlUsr.ToolTip = DdlUsr.SelectedItem.Text;
        }

        private void FillDdl(DropDownList ddl, String proc, string selectVal, DropDownList prntDdl = null, OleDbParameter[] paramCln = null)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Select", selectVal));
            ddl.SelectedIndex = 0;
            ddl.ToolTip = "Select";

            if (prntDdl != null && prntDdl.SelectedIndex == 0)
                return;
            try
            {
                DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            if (sender.Equals(BtnView3))
                GVReports3.DataBind();
        }

        protected void GVReports3_DataBinding(object sender, EventArgs e)
        {
            try
            {
                string User_Id = DdlUsr.SelectedValue;
                string Role_Id = DdlUsrType.SelectedValue;
                int mnthNo = 0, year = 0;
                try
                {
                    DateTime dt = DateTime.ParseExact(TxtMnth3.Text, MonthFormat, CultureInfo.InvariantCulture);
                    mnthNo = dt.Month;
                    year = dt.Year;
                }
                catch (Exception ex)
                { }

                OleDbParameter[] paramCln = new OleDbParameter[]
                {
                    new OleDbParameter("@Start_Date", null)
                   ,new OleDbParameter("@End_Date",  null)
                   ,new OleDbParameter("@User_Id", User_Id)
                   ,new OleDbParameter("@Role_Id", Role_Id)
                   ,new OleDbParameter("@Category_Type_Id", DdlCatType3.SelectedValue)
                   ,new OleDbParameter("@Category_Id", DdlCat3.SelectedValue)
                   ,new OleDbParameter("@Report_Id", DdlReport3.SelectedValue)
                   ,new OleDbParameter("@Type", DdlType3.SelectedValue)
                   ,new OleDbParameter() {
                       ParameterName = "@IsApproved",
                       Value = 1,
                       OleDbType = OleDbType.Boolean
                   }
                   ,new OleDbParameter("@ApprYearNo", year)
                   ,new OleDbParameter("@ApprMonthNo", mnthNo)
                };
                SetGV(paramCln, GVReports3);

            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        private void SetGV(OleDbParameter[] paramCln, GridView gv)
        {
            DataTable dt = DBOprn.GetDataProc("SP_Get_Tasks", DBOprn.ConnPrimary, paramCln);
            if (dt != null && dt.Rows.Count > 0)
            {
                gv.DataSource = dt;
                gv.Visible = true;
                DivExport.Visible = true;
                BtnReject.Visible = true;
            }
            else
            {
                gv.Visible = false;
                BtnReject.Visible = false;
                DivExport.Visible = false;
                PopUp("No data found!");
            }
        }

        public void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        protected void LBLocn_Click(object sender, EventArgs e)
        {
            LinkButton LBLocn = (LinkButton)sender;
            string fullPath = LBLocn.ToolTip;
            string fileName = LBLocn.Text;
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", $"attachment;filename={fileName}");
                Response.TransmitFile(fullPath);
                Response.End();
            }
            catch (Exception ex)
            {
                PopUp($"Error occurred: \n {ex.Message}");
            }
        }

        protected void CBRejectH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVReports3.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                cb.Checked = ((CheckBox)sender).Checked;
            }
        }

        protected void BtnReject_Click(object sender, EventArgs e)
        {
            string jsonParam = ConstructJSON();

            if (!string.IsNullOrWhiteSpace(jsonParam))
            {
                var output = DBOprn.ExecScalarProc("SP_Reject_Tasks", DBOprn.ConnPrimary,
                    new OleDbParameter[]
                    {
                        new OleDbParameter("@Collection", jsonParam)
                    }
                );

                if (!string.IsNullOrWhiteSpace((string)output)) //Error occurred
                {
                    PopUp(output.ToString());
                    return;
                }
                PopUp("Tasks rejected successfully!");
                GVReports3.DataBind();
            }
        }

        private string ConstructJSON()
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();
            try
            {
                foreach (GridViewRow gvRow in GVReports3.Rows)
                {
                    CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                    if (cb.Checked)
                    {
                        string id = ((Label)gvRow.Cells[gvRow.Cells.Count - 1].Controls[1]).Text;
                        Dictionary<string, string> paramVals = new Dictionary<string, string>()
                        {
                            {
                                "REC_ID",
                                id
                            },
                            {
                                "MODIFIED_BY",
                                Session["User_Name"].ToString()
                            },
                            {
                                "MODIFIED_DATE",
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                            }
                        };
                        dtls.Add(paramVals);
                    }
                    continue;
                }
                string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
                return jsonString;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return string.Empty;
            }
        }

    }
}