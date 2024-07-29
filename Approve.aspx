<%@ Page Title="Approve Submitted Tasks" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Approve.aspx.cs" Inherits="Finance_Tracker.Approve" EnableEventValidation="true" MaintainScrollPositionOnPostback="True" Async="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <br />
        <br />
        <br />
        <h3>Approve</h3>
        <hr style="padding: 1px; margin-top: 10px; margin-bottom: 10px; position: inherit;" />
        <div class="form-group">
            <div style="">
                <asp:Menu ID="Menu1" runat="server" BackColor="#006666" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.83em" ForeColor="White" Orientation="Horizontal" StaticSubMenuIndent="10px" OnMenuItemClick="Menu1_MenuItemClick" CssClass="form-control">
                    <DynamicHoverStyle BackColor="#3399FF" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        <asp:MenuItem Text="Pending Tasks |" Value="0" Selected="true"></asp:MenuItem>
                        <asp:MenuItem Text="Approved Tasks" Value="1"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
            <br />
            <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                <ContentTemplate>--%>
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="TxtMnth" CssClass="col-md-2 control-label">Month<span style=" color:red;">&nbsp*</span></asp:Label>
                <div class="col-sm-2">
                    <asp:TextBox ID="TxtMnth" runat="server" Width="160px" CssClass="form-control" BackColor="White" AutoPostBack="True" OnTextChanged="TxtMnth_TextChanged"></asp:TextBox>
                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkText="Select Month" TargetControlID="TxtMnth" />
                    <ajaxToolkit:CalendarExtender ID="CETxtMnth" runat="server" TargetControlID="TxtMnth" CssClass="modal-content"
                        DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                </div>
                <label class="col-lg-2 control-label">Type</label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlType" CssClass="form-control" OnDataBinding="DdlType_DataBinding" onchange="UpdateToolTip(this);">
                        <asp:ListItem Value="">All</asp:ListItem>
                        <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <label class="col-lg-2 control-label">User</label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlUsr" CssClass="form-control" onchange="UpdateToolTip(this);"
                        OnDataBinding="DdlUsr_DataBinding">
                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row" style="margin-left: 2px; display: flex; align-items: baseline;">
                <div style="margin-right: 1.5%;">
                    <asp:Button runat="server" ID="BtnView" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" />
                </div>
            </div>
            <br />
            <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="TabUnApproved" runat="server">
                    <div runat="server" id="GVPendingDiv" visible="false">
                        <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px" runat="server">
                            <asp:GridView ID="GVPending"
                                runat="server" Font-Bold="False" CssClass="table table-bordered table-responsive table-hover"
                                Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                                RowStyle-HorizontalAlign="LEFT" TabIndex="10"
                                OnDataBinding="GVPending_DataBinding" BorderStyle="Solid" AutoGenerateColumns="False">
                                <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" Width="0em" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="False" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="#7ad0ed" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Approve" Visible="true" ControlStyle-CssClass="form-check-input" ControlStyle-Width="20px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="CBApprovH" runat="server" onclick="CBHOnClick(this)" Text=""></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBApprov" runat="server" onclick="CBOnClick(this)"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                    <asp:BoundField DataField="User_Name" HeaderText="User" ReadOnly="True" />
                                    <asp:BoundField DataField="Report_Name" HeaderText="Task" ReadOnly="True" />
                                    <asp:BoundField DataField="Report_Type" HeaderText="Task Type" />
                                    <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                    <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                    <asp:TemplateField HeaderText="Comments" Visible="true">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="TxtCmnts" TextMode="MultiLine" AutoCompleteType="None" onchange="UpdateToolTip(this);" Height="40px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File" Visible="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBLocn" runat="server" ForeColor="#3366FF" OnClientClick=""
                                                Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'
                                                OnClick="LBLocn_Click">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="LblRecId" runat="server" Text='<%# Bind("Task_Id")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="row" style="margin-left: 2px;">
                            <asp:Button runat="server" ID="BtnReject" OnClick="BtnReject_Click" Text="Reject" CssClass="col-2 btn btn-default" ForeColor="White" Enabled="False" />
                            <asp:Button runat="server" ID="BtnApprove" OnClick="BtnApprove_Click" Text="Approve" CssClass="btn btn-primary" ForeColor="White" Enabled="False" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="TabApproved" runat="server">
                    <div runat="server" id="DivApproved" visible="false">
                        <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px" runat="server">
                            <asp:GridView ID="GVApproved"
                                runat="server" Font-Bold="False" CssClass="table table-bordered table-responsive table-hover"
                                Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                                RowStyle-HorizontalAlign="LEFT" TabIndex="10"
                                OnDataBinding="GVApproved_DataBinding" BorderStyle="Solid" AutoGenerateColumns="False">
                                <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" Width="0em" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="False" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="#7ad0ed" />
                                <Columns>
                                    <asp:TemplateField Visible="true" ControlStyle-CssClass="form-check-input" ControlStyle-Width="20px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="CBRejectH" runat="server" onclick="CBRejectHOnClick(this)" ToolTip="Reject" Text=""></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBReject" onclick="CBRejectOnClick(this)" runat="server" ToolTip="Reject"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                    <asp:BoundField DataField="User_Name" HeaderText="User" ReadOnly="True" />
                                    <asp:BoundField DataField="Report_Name" HeaderText="Task" ReadOnly="True" />
                                    <asp:BoundField DataField="Report_Type" HeaderText="Task Type" />
                                    <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                    <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                    <asp:BoundField DataField="Approve_Date" HeaderText="Approve Date" />
                                    <asp:TemplateField HeaderText="Comments" Visible="true" SortExpression="Comments">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="TxtCmnts" TextMode="MultiLine" AutoCompleteType="None" onchange="UpdateToolTip(this);" Height="40px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File" Visible="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBLocn" runat="server" ForeColor="#3366FF"
                                                Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'
                                                OnClick="LBLocn_Click">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="LblRecId" runat="server" Text='<%# Bind("Task_Id")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="row" style="margin-left: 2px;">
                            <asp:Button runat="server" ID="BtnRejectA" OnClick="BtnRejectA_Click" Text="Reject" CssClass="btn btn-primary" ForeColor="White" Enabled="False" />
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <script language="C#" runat="server">

        protected void LBLocn_Click1()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showalert", "alert('LBLocn_Click1 called');", true);
        }

    </script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.5/xlsx.full.min.js"></script>
    <script src="../assets/libs/Common.js" type="text/javascript"></script>

    <script type="text/javascript">

        let chkCntP = 0;
        let chkCntR = 0;

        $(document).ready(function ()
        {
            //debugger;
            var GVPending = document.getElementById('<%= GVPending.ClientID %>');
            if (GVPending !== null) 
            {
                for (let i = 1; i < GVPending.rows.length; i++)
                {
                    let cbChld = GVPending.rows[i].cells[0].querySelector('input[type="checkbox"]');
                    let chkd = cbChld.checked;
                    chkCntP += chkd ? 1 : 0;
                }
                //debugger;
                var cbH = GVPending.rows[0].cells[0].querySelector('input[type="checkbox"]');
                if (cbH !== null) cbH.checked = (chkCntP == GVPending.rows.length - 1);

                EnableDisableBtns(chkCntP);
            }
            var GVApproved = document.getElementById('<%= GVApproved.ClientID %>');
            if (GVApproved !== null) 
            {
                for (let i = 1; i < GVApproved.rows.length; i++)
                {
                    let cbChld = GVApproved.rows[i].cells[0].querySelector('input[type="checkbox"]');
                    let chkd = cbChld.checked;
                    chkCntR += chkd ? 1 : 0;
                }
                //debugger;
                var cbH = GVApproved.rows[0].cells[0].querySelector('input[type="checkbox"]');
                if (cbH !== null) cbH.checked = (chkCntR == GVApproved.rows.length - 1);

                EnableDisableBtnR(chkCntR);
            }
        });

        //Handle checkbox change for checkbox in Header row of GVPending
        function CBHOnClick (cb)
        {
            //console.log("hi CBHOnClick");
            //debugger;
            var GVPending = document.getElementById('<%= GVPending.ClientID %>');
            if (GVPending !== null) 
            {
                //Update each row's checkbox
                for (let i = 1; i < GVPending.rows.length; i++)
                {
                    let cbChld = GVPending.rows[i].cells[0].querySelector('input[type="checkbox"]');
                    let chkdH = cb.checked;
                    if (chkdH !== cbChld.checked)
                    {
                        cbChld.checked = chkdH;
                        chkCntP += chkdH ? 1 : -1;
                    }
                }
                if (GVPending.rows.length < chkCntP)
                    chkCntP = GVPending.rows.length;
                else if (chkCntP < 0)
                    chkCntP = 0;

                EnableDisableBtns(chkCntP);
            }
            //console.log("bye CBHOnClick");
        }

        //Handle checkbox change for checkbox in each row of GVPending
        function CBOnClick (cb)
        {
            //console.log("hi CBOnClick");
            //debugger;
            chkCntP += cb.checked ? 1 : -1;
            var GVPending = document.getElementById('<%= GVPending.ClientID%>');
            if (GVPending !== null)
            {
                if (GVPending.rows.length < chkCntP)
                    chkCntP = GVPending.rows.length;
                else if
                    (chkCntP < 0) chkCntP = 0;

                // Update header checkbox
                let cbH = GVPending.rows[0].querySelector('th input[type="checkbox"]');
                cbH.checked = (GVPending.rows.length - 1 === chkCntP);
                EnableDisableBtns(chkCntP);
            }
            //console.log("bye CBOnClick");
        }

        function EnableDisableBtns (count)
        {
            let btnApprv = document.getElementById('<%= BtnApprove.ClientID %>');
            btnApprv.disabled = (count === 0);

            let BtnReject = document.getElementById('<%= BtnReject.ClientID %>');
            BtnReject.disabled = (count === 0);
        }

        //Handle checkbox change for checkbox in Header row of GVApproved
        function CBRejectHOnClick (cb)
        {
            //console.log("hi CBRejectHOnClick");
            //debugger;
            var GVApproved = document.getElementById('<%= GVApproved.ClientID %>');

            //Update each row's checkbox
            if (GVApproved !== null)
            {
                for (let i = 1; i < GVApproved.rows.length; i++)
                {
                    let cbChld = GVApproved.rows[i].cells[0].querySelector('input[type="checkbox"]');
                    let chkdH = cb.checked;
                    if (chkdH !== cbChld.checked)
                    {
                        cbChld.checked = chkdH;
                        chkCntR += chkdH ? 1 : -1;
                    }
                }
                if (GVApproved.rows.length < chkCntR)
                    chkCntR = GVApproved.rows.length;
                else if (chkCntR < 0)
                    chkCntR = 0;

                EnableDisableBtnR(chkCntR);
            }
            //console.log("by CBRejectHOnClick");
        }

        //Handle checkbox change for checkbox in each row of GVApproved
        function CBRejectOnClick (cb)
        {
            //console.log("hi CBRejectOnClick");
            //debugger;
            chkCntR += cb.checked ? 1 : -1;
            var GVApproved = document.getElementById('<%= GVApproved.ClientID%>');
            if (GVApproved !== null)
            {
                if (GVApproved.rows.length < chkCntR)
                    chkCntR = GVApproved.rows.length;
                else if
                    (chkCntR < 0) chkCntR = 0;

                // Update header checkbox
                let cbH = GVApproved.rows[0].querySelector('th input[type="checkbox"]');
                cbH.checked = (GVApproved.rows.length - 1 === chkCntR);
                EnableDisableBtnR(chkCntR);
            }
            //console.log("by CBRejectOnClick");
        }

        function EnableDisableBtnR (count)
        {
            let BtnRejectA = document.getElementById('<%= BtnRejectA.ClientID %>');
            BtnRejectA.disabled = (count === 0);
        }

        function SetToolTip (ddl)
        {
            if (ddl.selectedIndex !== -1)
            {
                ddl.title = ddl.options[ddl.selectedIndex].text;
            }
        }
        function downloadFile (folder, fileName)
        {
            //debugger;
            //var xhr = new XMLHttpRequest();
            ////var lastIndex = fullPath.lastIndexOf('/');
            ////if (lastIndex === -1) {
            ////    lastIndex = fullPath.lastIndexOf('\\'); // Check for Windows-style paths
            ////}

            ////// Extract the substring representing the folder
            ////var folder = fullPath.substring(0, lastIndex);
            ////var file
            //xhr.open('GET', folder, true); // Specify the URL of your server-side endpoint
            //xhr.responseType = 'blob'; // Specify that the response will be a binary file
            //xhr.onload = function () {
            //    if (xhr.status === 200) {
            //        // Create a temporary anchor element to trigger the file download
            //        var blob = new Blob([xhr.response], { type: 'application/octet-stream' });
            //        var url = window.URL.createObjectURL(blob);
            //        var a = document.createElement('a');
            //        a.style.display = 'none';
            //        a.href = url;
            //        a.download = fileName; // Specify the file name
            //        document.body.appendChild(a);
            //        a.click();
            //        window.URL.revokeObjectURL(url);
            //    }
            //};
            //xhr.send();
        }
        function ConsoleLog ()
        {
            //console.log("hi");
        }
        function submitData ()
        {
            // Make an AJAX call to the server
            // Example using jQuery:
            $.ajax({
                url: 'Approve.aspx/HandleSubmit',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({}),
                success: function (response)
                {
                    // Handle success response
                },
                error: function (xhr, status, error)
                {
                    // Handle error
                }
            });
        }
    </script>
</asp:Content>
