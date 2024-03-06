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
                    <asp:DropDownList runat="server" ID="DdlType" CssClass="form-control" onchange="SetToolTip(this);">
                        <asp:ListItem Value="">All</asp:ListItem>
                        <asp:ListItem Value="M">Monthly</asp:ListItem>
                        <asp:ListItem Value="W">Weekly</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <label class="col-lg-2 control-label">User</label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlUsr" CssClass="form-control" onchange="SetToolTip(this);"
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
                    <div runat="server" id="GVReportsDiv" visible="false">
                        <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px" runat="server">
                            <asp:GridView ID="GVReports"
                                runat="server" Font-Bold="False" CssClass="table table-bordered table-responsive table-hover"
                                Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                                RowStyle-HorizontalAlign="LEFT" TabIndex="10"
                                OnDataBinding="GVReports_DataBinding" BorderStyle="Solid" AutoGenerateColumns="False">
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
                                            <asp:CheckBox ID="CBApprovH" runat="server" OnCheckedChanged="CBApprovH_CheckedChanged" AutoPostBack="true" Text=""></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBApprov" OnCheckedChanged="CBApprov_CheckedChanged" AutoPostBack="true" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <asp:CheckBox ID="CBApprov" runat="server" BackColor="#7ad0ed" OnCheckedChanged="CBApprov_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                        </AlternatingItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                    <asp:BoundField DataField="User_Name" HeaderText="User" ReadOnly="True" />
                                    <asp:BoundField DataField="Report_Name" HeaderText="Task" ReadOnly="True" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" />
                                    <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                    <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                    <asp:TemplateField HeaderText="Comments" Visible="true">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="TxtCmnts" TextMode="MultiLine" AutoCompleteType="None" onchange="SetToolTip(this);" Height="40px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File" Visible="true" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBLocn" runat="server" ForeColor="#3366FF" OnClientClick=""
                                                Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'
                                                ToolTip='<%# Bind("Location")%>' OnClick="LBLocn_Click">
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
                            <asp:Button runat="server" ID="BtnReject1" OnClick="BtnReject1_Click" Text="Reject" CssClass="col-2 btn btn-default" ForeColor="White" Enabled="False" />
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
                                            <asp:CheckBox ID="CBRejectH" runat="server" OnCheckedChanged="CBRejectH_CheckedChanged" AutoPostBack="true" ToolTip="Approve" Text=""></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBReject" OnCheckedChanged="CBReject_CheckedChanged" AutoPostBack="true" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <asp:CheckBox ID="CBReject" OnCheckedChanged="CBReject_CheckedChanged" AutoPostBack="true" runat="server" BackColor="#7ad0ed"></asp:CheckBox>
                                        </AlternatingItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                    <asp:BoundField DataField="User_Name" HeaderText="User" ReadOnly="True" />
                                    <asp:BoundField DataField="Report_Name" HeaderText="Task" ReadOnly="True" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" />
                                    <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                    <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                    <asp:BoundField DataField="Approve_Date" HeaderText="Approve Date" />
                                    <asp:TemplateField HeaderText="Comments" Visible="true" SortExpression="Comments">
                                        <ItemTemplate >
                                            <asp:TextBox runat="server" ID="TxtCmnts" TextMode="MultiLine" AutoCompleteType="None" onchange="SetToolTip(this);" Height="40px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="File" Visible="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LBLocn" runat="server" ForeColor="#3366FF" ToolTip='<%# Bind("Location")%>'
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
                            <asp:Button runat="server" ID="BtnReject" OnClick="BtnReject_Click" Text="Reject" CssClass="btn btn-primary" ForeColor="White" Enabled="False" />
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
    <script type="text/javascript">
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
        function ConsoleLog (this)
        {
            console.log("hi");
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
