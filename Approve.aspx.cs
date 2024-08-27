using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Configuration;
using System.Threading.Tasks;
using static System.DateTime;
using static Finance_Tracker.DBOperations;

namespace Finance_Tracker
{
    public partial class Approve : System.Web.UI.Page
    {
        const string MonthFormat = "MMM-yyyy";
        const string SqlDateFormat = "yyyy-MM-dd";
        const string Emp = "";
        private readonly DateTime crntMnthDay1 = new DateTime(Today.Year, Today.Month, 1);
        private readonly DateTime crntMnthLastDay = new DateTime(Today.Year, Today.Month, DaysInMonth(Today.Year, Today.Month));
        private static int chkCountA = 0;
        private static int chKCountR = 0;
        private static SmtpSection settings;
        private static SmtpNetworkElement Network;
        private readonly DBOperations DBOprn;

        public Approve()
        {
            DBOprn = new DBOperations();
            if (!DBOprn.AuthenticatConns())
            {
                PopUp("Database connection could not be established. No operations will be performed");
                return;
            }
        }

        private DataTable GVPendingDS
        {
            get
            {
                DataTable dt = (DataTable)Session["Approve_GVPendingDS"];

                if (!(dt?.Rows.Count > 0))
                {
                    try
                    {
                        dt = GetData(false);
                        Session["Approve_GVPendingDS"] = dt;
                    }
                    catch (Exception ex)
                    {
                        PopUp(ex.Message);
                    }
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    Session["Approve_GVPendingDS"] = null;
                else
                    Session["Approve_GVPendingDS"] = value;
            }
        }

        private DataTable GVApprovedDS
        {
            get
            {
                DataTable dt = (DataTable)Session["Approve_GVApprovedDS"];

                if (!(dt?.Rows.Count > 0))
                {
                    try
                    {
                        dt = GetData(true);
                        Session["Approve_GVApprovedDS"] = dt;
                    }
                    catch (Exception ex)
                    {
                        PopUp(ex.Message);
                    }
                }
                return dt;
            }
            set
            {
                if (!(value?.Rows.Count > 0))
                    Session["Approve_GVApprovedDS"] = null;
                else
                    Session["Approve_GVApprovedDS"] = value;
            }
        }

        private DataTable GetData(bool IsApproved)
        {
            try
            {
                string fromDt = TxtMnth.ToolTip.Split(',')[0];
                string toDt = TxtMnth.ToolTip.Split(',')[1];
                string typ = string.IsNullOrWhiteSpace(DdlType.SelectedValue) ? "0" : DdlType.SelectedValue;
                OleDbParameter[] paramCln = new OleDbParameter[]
                {
                     new OleDbParameter("@From_Date", fromDt)
                    ,new OleDbParameter("@To_Date",  toDt)
                    ,new OleDbParameter("@User_Id", DdlUsr.SelectedValue)
                    ,new OleDbParameter("@IsApproved", IsApproved)
                    ,new OleDbParameter("@ReportTypeId", typ)
                    ,new OleDbParameter("@Approver_Id", Session["User_Id"]?.ToString().Trim().ToUpper())
                };
                //paramCln = null;
                DataTable dt = DBOprn.GetDataProc("SP_Get_Submit_Tasks_SubOrdinates", DBOprn.ConnPrimary, paramCln);
                if (!(dt?.Rows.Count > 0))
                    dt = null;
                return dt;
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
                return null;
            }
        }

        #region Page Code

        protected void Page_Load(object sender, EventArgs e)
        {
            //string usrId = Session["User_Id"]?.ToString();
            //if (usrId == null || usrId == "")
            //{
            //    Response.Redirect("~/Account/Login");
            //    return;
            //}
            if (!Page.IsPostBack)
            {
                if (Session["Is_Approver"].ToString() == "0")
                {
                    Response.Redirect("~/Default");
                    return;
                }
                DdlType.DataBind();
                Menu1_MenuItemClick(Menu1, new MenuEventArgs(Menu1.Items[0]));
                TxtMnth.Attributes.Add("autocomplete", "off");

                if (!IsSmtpConfigValid())
                    PopUp("Email settings could not be verified. No emails will be sent for approval/rejection!");
                //else
                //    PopUp("Email settings verified.!");

                chkCountA = 0;
                chKCountR = 0;
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

            DdlUsr_DataBinding(DdlUsr, new EventArgs());
            DdlType.SelectedIndex = 0;
            SetTooltip(DdlType);

            if (Menu1.SelectedValue == "0")
                ResetGVPending();
            else if (Menu1.SelectedValue == "1")
                ResetGVApprovd();
        }

        private void ResetGVPending()
        {
            GVPendingDiv.Visible = false;
            GVPending.DataSource = null;
            GVPendingDS = null;
        }

        private void ResetGVApprovd()
        {
            DivApproved.Visible = false;
            GVApproved.DataSource = null;
            GVApprovedDS = null;
        }

        protected void DdlUsr_DataBinding(object sender, EventArgs e)
        {
            FillDdl(DdlUsr, "SP_Get_SubOrdinates", "", "All", null,
                new OleDbParameter[]
                {
                    new OleDbParameter("@Approver_Id", Session["User_Id"]?.ToString().Trim().ToUpper())
                }
            );
            DdlUsr.SelectedIndex = 0;
            SetTooltip(DdlUsr);
        }

        protected void DdlUsr_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlUsr.ToolTip = DdlUsr.SelectedItem.Text;
        }

        private void FillDdl(DropDownList ddl, string proc, string selectVal, string selectTxt = "", DropDownList prntDdl = null, OleDbParameter[] paramCln = null, string TxtField = "", string ValField = "")
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(selectTxt, selectVal));
            ddl.SelectedValue = selectVal;
            ddl.ToolTip = selectTxt;
            if (prntDdl != null && prntDdl.SelectedIndex == 0)
                return;

            try
            {
                DataTable dt = DBOprn.GetDataProc(proc, DBOprn.ConnPrimary, paramCln);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if ((TxtField != Emp) && ValField != Emp)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl.Items.Add(new ListItem(dt.Rows[i][TxtField].ToString(), dt.Rows[i][ValField].ToString()));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                        }
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
            if (TxtMnth.Text == string.Empty)
            {
                PopUp("Please select a Month!");
                TxtMnth.Focus();
                return;
            }
            if (!TryParseExact(TxtMnth.Text, MonthFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                PopUp("Please enter month in correct format like 'Jan-2024'!");
                TxtMnth.Text = TextBoxWatermarkExtender3.WatermarkText;
                TxtMnth.ToolTip = crntMnthDay1.ToString(SqlDateFormat) + "," + crntMnthLastDay.ToString(SqlDateFormat);
                TxtMnth.Focus();
                return;
            }
            else
            {
                TxtMnth.ToolTip = dt.ToString(SqlDateFormat) + "," + new DateTime(dt.Year, dt.Month, DaysInMonth(dt.Year, dt.Month)).ToString(SqlDateFormat);
            }
            if (MultiView1.ActiveViewIndex == 0)
            {
                ResetGVPending();
                GVPending.DataBind();
            }
            else if (MultiView1.ActiveViewIndex == 1)
            {
                ResetGVApprovd();
                GVApproved.DataBind();
            }
        }

        protected void GVPending_DataBinding(object sender, EventArgs e)
        {
            if (GVPending.DataSource == null)
            {
                DataTable dt = GVPendingDS;
                if (dt != null)
                {
                    GVPending.DataSource = dt;
                    GVPendingDiv.Visible = true;
                    BtnApprove.Enabled = false;
                }
                else
                {
                    GVPendingDiv.Visible = false;
                    PopUp("No data found!");
                }
            }
        }

        public void PopUp(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('" + msg + "');", true);
        }

        protected void CBApprovH_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow gvRow in GVPending.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chkCountA += chked ? 1 : -1;
                }
            }
            if (GVPending.Rows.Count < chkCountA)
                chkCountA = GVPending.Rows.Count;
            else if (chkCountA < 0)
                chkCountA = 0;

            BtnApprove.Enabled = chkCountA > 0;
            BtnReject.Enabled = chkCountA > 0;
        }

        protected void CBApprov_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chkCountA += cb.Checked ? 1 : -1;
            if (GVPending.Rows.Count < chkCountA)
                chkCountA = GVPending.Rows.Count;
            else if (chkCountA < 0)
                chkCountA = 0;
            BtnApprove.Enabled = chkCountA > 0;
            BtnReject.Enabled = chkCountA > 0;
            GridViewRow row = GVPending.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVPending.Rows.Count == chkCountA);
        }

        protected void BtnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonParam = ConstructJSONA(out Dictionary<string, string> mailSet);

                if (!string.IsNullOrWhiteSpace(jsonParam))
                {
                    var output = DBOprn.ExecScalarProc("SP_Approve_Tasks", DBOprn.ConnPrimary,
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
                    PopUp("Tasks approved successfully!");
                    if (mailSet != null && mailSet.Count > 0)
                        SendEmails(mailSet, "Tasks Approval Alert - Finance Tracker");

                    GVPending.DataSource = null;
                    GVPendingDS = null;
                    GVPending.DataBind();
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        private string ConstructJSONA(out Dictionary<string, string> mailSet)
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();
            mailSet = new Dictionary<string, string>();
            int chkCnt = 0;
            GVPending.Rows.Cast<GridViewRow>().ToList().ForEach(gvRow => chkCnt += ((CheckBox)gvRow.Cells[0].Controls[1]).Checked ? 1 : 0);

            string strt = $@"
                            <p>{Session["User_Name"]} has approved your submitted tasks.</p>
                            <p>Please find the details below:</p>";

            string tableBody = @"<table border='2'>";
            tableBody += @"<tr>
                        <th>Task</th>
                        <th>Comments</th>
                        <th>Submit Date</th>
                        <th>Due Date</th>
                        </tr>";

            string footer = $@"
                 <p> You can check your approved tasks at Performance page at
                     <a href = ""http://10.10.1.171:88"">Finance Tracker</a>.</p>
                 <p>For more information contact <a href=""mailto:{Session["Email"]}"">{Session["User_Name"]}</a>.</ p >
                 <p>This is an automatically generated mail. Please do not reply as there will be no responses.</p>
                 <p> Best Regards, </p>
                 <p> Grupo Bimbo </p> ";
            foreach (GridViewRow gvRow in GVPending.Rows)
            {
                if (chkCnt < 1) break;

                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (!cb.Checked) continue;

                string tblStr = Emp;
                int sno = Convert.ToInt32(gvRow.Cells[1].Text);
                DataRow dRo = GVPendingDS.Select("Sno = " + sno)?[0];

                string tskId = dRo["Task_Id"].ToString();
                string usrMail = dRo["Email"].ToString();
                string usrNm = dRo["User_Name"].ToString();
                string usrId = dRo["User_Id"].ToString();
                string sbmtDt = dRo["Submit_Date"].ToString();
                string duDt = dRo["Due_Date"].ToString();
                string task = dRo["Report_Name"].ToString();
                string cmnts = ((TextBox)gvRow.Cells[7].Controls[1]).Text.Trim();

                Dictionary<string, string> paramVals = new Dictionary<string, string>()
                {
                    { "REC_ID", tskId },
                    { "APPROVE_DATE", Now.ToString("yyyy-MM-dd HH:mm:ss.fff") },
                    { "COMMENTS", cmnts },
                    { "MODIFIED_BY", Session["User_Name"].ToString() },
                    { "MODIFIED_DATE", Now.ToString("yyyy-MM-dd HH:mm:ss.fff") }
                };
                dtls.Add(paramVals);
                cb.Checked = false;
                chkCnt--;

                //tblStr = @"<tr style='border-bottom: 1px solid #000000; padding:3px'>" +
                //        $@"<td style='border-right: 1px solid #000000; padding:3px'>{task}</td>"+
                //        $@"<td style='border-right: 1px solid #000000; padding:3px'>{cmnts}</td>"+
                //        $@"<td style='border-right: 1px solid #000000; padding:3px'>{sbmtDt}</td>"+
                //        $@"<td style='border-right: 1px solid #000000; padding:3px'>{duDt}</td>"+
                //         @"</tr>";

                tblStr = $@"<tr><td>{task}</td><td>{cmnts}</td><td>{sbmtDt}</td><td>{duDt}</td></tr>";

                if (!string.IsNullOrWhiteSpace(usrMail))
                    if (!mailSet.ContainsKey(usrMail))
                        mailSet.Add(usrMail, $@"</br></br><p>Dear {usrNm},</p> {strt} {tableBody} {tblStr}");
                    else
                        mailSet[usrMail] += tblStr;
            }
            mailSet = mailSet.ToDictionary(kvp => kvp.Key, kvp => kvp.Value + "</table>" + footer);

            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        protected void TxtMnth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = ParseExact(TxtMnth.Text, MonthFormat, CultureInfo.InvariantCulture);
                int mnthNo = dt.Month;
                int year = dt.Year;
                TxtMnth.ToolTip = new DateTime(year, mnthNo, 01).ToString(SqlDateFormat) + "," + new DateTime(year, mnthNo, DaysInMonth(year, mnthNo)).ToString(SqlDateFormat);
            }
            catch (Exception ex)
            {
                PopUp("Please enter the month in correct format!");
                TxtMnth.Text = TextBoxWatermarkExtender3.WatermarkText;
                TxtMnth.ToolTip = crntMnthDay1.ToString(SqlDateFormat) + "," + crntMnthLastDay.ToString(SqlDateFormat);
            }
        }

        protected void GVApproved_DataBinding(object sender, EventArgs e)
        {
            if (GVApproved.DataSource == null)
            {
                DataTable dt = GVApprovedDS;
                if (dt != null)
                {
                    GVApproved.DataSource = dt;
                    DivApproved.Visible = true;
                }
                else
                {
                    DivApproved.Visible = false;
                    PopUp("No data found!");
                }
            }
        }

        protected void LBLocn_Click(object sender, EventArgs e)
        {
            LinkButton LBLocn = (LinkButton)sender;
            string fileName = LBLocn.Text;
            dynamic contnr = LBLocn.NamingContainer;
            string gvID = contnr.NamingContainer.ID;
            DataTable dt = gvID == "GVPending" ? GVPendingDS : gvID == "GVApproved" ? GVApprovedDS : null;
            string fullPath = dt?.Rows[contnr.RowIndex]?["Location"]?.ToString();
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
            foreach (GridViewRow gvRow in GVApproved.Rows)
            {
                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                bool chked = ((CheckBox)sender).Checked;
                if (cb.Checked != chked)
                {
                    cb.Checked = chked;
                    chKCountR += chked ? 1 : -1;
                }
            }
            if (GVApproved.Rows.Count < chKCountR)
                chKCountR = GVApproved.Rows.Count;
            else if (chKCountR < 0)
                chKCountR = 0;
            BtnRejectA.Enabled = chKCountR > 0;
        }

        protected void CBReject_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            chKCountR += cb.Checked ? 1 : -1;
            if (GVApproved.Rows.Count < chKCountR)
                chKCountR = GVApproved.Rows.Count;
            else if (chkCountA < 0)
                chKCountR = 0;
            BtnRejectA.Enabled = chKCountR > 0;
            GridViewRow row = GVApproved.HeaderRow;
            CheckBox cbH = (CheckBox)row.Cells[0].Controls[1];
            cbH.Checked = (GVApproved.Rows.Count == chKCountR);
        }

        protected void BtnRejectA_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonParam = ConstructJSONReject(GVApproved, GVApprovedDS, 8, out Dictionary<string, string> mailSet);
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

                    ////await SendMultipleEmailsAsync(mailSet, "Tasks rejection");

                    if (mailSet != null && mailSet.Count > 0)
                        SendEmails(mailSet, "Tasks Rejection Alert - Finance Tracker");

                    GVApprovedDS = null;
                    GVApproved.DataSource = null;
                    GVApproved.DataBind();
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        protected void BtnReject_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonParam = ConstructJSONReject(GVPending, GVPendingDS, 7, out Dictionary<string, string> mailSet);

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
                    if (mailSet != null && mailSet.Count > 0)
                        SendEmails(mailSet, "Tasks Rejection Alert - Finance Tracker");

                    ResetGVPending();
                    GVPending.DataBind();
                }
            }
            catch (Exception ex)
            {
                PopUp(ex.Message);
            }
        }

        private string ConstructJSONReject(GridView gv, DataTable gvDS, int cmntIndex, out Dictionary<string, string> mailSet)
        {
            List<Dictionary<string, string>> dtls = new List<Dictionary<string, string>>();
            mailSet = new Dictionary<string, string>();
            int chkCnt = 0;
            gv.Rows.Cast<GridViewRow>().ToList().ForEach(gvRow => chkCnt += ((CheckBox)gvRow.Cells[0].Controls[1]).Checked ? 1 : 0);

            string strt = $@"
                            <p>{Session["User_Name"]} has rejected your submitted tasks.</p>
                            <p>Please find the details below:</p>";

            string tableBody = @"<table border='2'>";
            tableBody += @"<tr>
                        <th>Task</th>
                        <th>Comments</th>
                        <th>Submit Date</th>
                        <th>Due Date</th>
                        </tr>";

            string footer = $@"
                 <p> You can check your submitted tasks at Performance page at
                     <a href = ""http://10.10.1.171:88"">Finance Tracker</a>.
                 </p>
                 <p> For more information contact <a href=""mailto:{Session["Email"]}"">{Session["User_Name"]}</a>.</ p >
                 <p>This is an automatically generated email. Please do not reply as there will be no responses.</p>
                 <p> Best Regards, </p>
                 <p> Grupo Bimbo </p> ";
            foreach (GridViewRow gvRow in gv.Rows)
            {
                if (chkCnt < 1) break;

                CheckBox cb = (CheckBox)gvRow.Cells[0].Controls[1];
                if (!cb.Checked) continue;

                string tblStr = Emp;
                int sno = Convert.ToInt32(gvRow.Cells[1].Text);
                DataRow dRo = gvDS.Select("Sno = " + sno)?[0];

                string tskId = dRo["Task_Id"].ToString();
                string usrMail = dRo["Email"].ToString();
                string usrNm = dRo["User_Name"].ToString();
                string usrId = dRo["User_Id"].ToString();
                string sbmtDt = dRo["Submit_Date"].ToString();
                string duDt = dRo["Due_Date"].ToString();
                string task = dRo["Report_Name"].ToString();
                string cmnts = ((TextBox)gvRow.Cells[cmntIndex].Controls[1]).Text.Trim();

                Dictionary<string, string> paramVals = new Dictionary<string, string>()
                {
                    { "REC_ID", tskId },
                    { "COMMENTS", cmnts },
                    { "MODIFIED_BY", Session["User_Name"].ToString() },
                    { "MODIFIED_DATE", Now.ToString("yyyy-MM-dd HH:mm:ss.fff") }
                };
                dtls.Add(paramVals);
                cb.Checked = false;
                chkCnt--;

                //tblStr = @"<tr style='border-bottom: 1px solid #000000; padding:3px'>" +
                //        $@"<td style='border-right: 1px solid #000000; padding:3px'>{task}</td>"+
                //        $@"<td style='border-right: 1px solid #000000; padding:3px'>{cmnts}</td>"+
                //        $@"<td style='border-right: 1px solid #000000; padding:3px'>{sbmtDt}</td>"+
                //        $@"<td style='border-right: 1px solid #000000; padding:3px'>{duDt}</td>"+
                //         @"</tr>";

                tblStr = $@"<tr><td>{task}</td><td>{cmnts}</td><td>{sbmtDt}</td><td>{duDt}</td></tr>";

                if (!string.IsNullOrWhiteSpace(usrMail))
                    if (!mailSet.ContainsKey(usrMail))
                        mailSet.Add(usrMail, $@"</br></br><p>Dear {usrNm},</p> {strt} {tableBody} {tblStr}");
                    else
                        mailSet[usrMail] += tblStr;
            }
            mailSet = mailSet.ToDictionary(kvp => kvp.Key, kvp => kvp.Value + "</table>" + footer);

            string jsonString = JsonConvert.SerializeObject(dtls, Formatting.Indented);
            return jsonString;
        }

        public async Task SendMultipleEmailsAsync(Dictionary<string, string> mailSet, string subject)
        {
            string host = Network.Host, usrNm = Network.UserName, pswd = Network.Password;
            int port = Network.Port;

            using (SmtpClient smtp = new SmtpClient(host, port)) // Your SMTP server
            {
                smtp.Port = 587; // Port number
                smtp.Credentials = new NetworkCredential(usrNm, pswd); // Your email credentials
                smtp.EnableSsl = true; // Enable SSL if required

                var tasks = mailSet.Select(recipient => SendEmailAsync(recipient.Key, subject, recipient.Value, smtp));
                await Task.WhenAll(tasks);
            }
        }

        public async Task SendEmailAsync(string recipient, string subject, string body, SmtpClient smtp)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(settings.From), // Your email address
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(recipient);
            await smtp.SendMailAsync(mail);
            PopUp("Mails Sent successfully");
        }

        private void SendEmails(Dictionary<string, string> mailSet, string subject)
        {
            string host = Network.Host, pswd = Network.Password, from = settings.From, usrName = Network.UserName;
            int port = Network.Port;
            try
            {
                using (SmtpClient smtp = new SmtpClient(host, port)) // Your SMTP server
                {
                    smtp.UseDefaultCredentials = false;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.DeliveryFormat = SmtpDeliveryFormat.International;
                    smtp.Credentials = new NetworkCredential(usrName, pswd); // Your email credentials
                    smtp.EnableSsl = false; // Enable SSL if required

                    foreach (KeyValuePair<string, string> recipient in mailSet)
                    {
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress(from, usrName), // Your email address
                            Subject = subject,
                            Body = recipient.Value,
                            IsBodyHtml = true
                        };
                        mail.To.Add(recipient.Key);
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception e)
            {
                PopUp(e.Message);
            }
        }

        protected void DdlType_DataBinding(object sender, EventArgs e)
        {
            FillDdl((DropDownList)sender, "SP_ReportTypes_Get", Emp, "All", null, null, "TypeName", "TypeId");
        }

        private void SetTooltip(DropDownList ddl)
        {
            ddl.ToolTip = ddl.SelectedItem.Text;
        }
    }
}