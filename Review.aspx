<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Review.aspx.cs" Inherits="Finance_Tracker.Review" EnableEventValidation="false" Title="Review" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <br />
        <br />
        <br />
        <h3>Review</h3>
        <hr style="padding: 1px; margin-top: 10px; margin-bottom: 10px; position: inherit;" />
        <div class="form-group">
            <div style="">
                <asp:Menu ID="Menu1" runat="server" BackColor="#006666" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.83em" ForeColor="White" Orientation="Horizontal" StaticSubMenuIndent="10px" OnMenuItemClick="Menu1_MenuItemClick" CssClass="form-control">
                    <DynamicHoverStyle BackColor="#3399FF" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        <asp:MenuItem Text="View Submitted Tasks |" Value="0" Selected="true"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
            <br />
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="TabApproved" runat="server">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                        <ContentTemplate>
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="DdlCatType3" CssClass="col-md-2 control-label">Category Type</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCatType3" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlCat3" CssClass="col-md-2 control-label">Category</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCat3" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlReport3" CssClass="col-md-2 control-label">Report</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlReport3" CssClass="form-control" OnDataBinding="DdlReport_DataBinding" OnSelectedIndexChanged="DdlReport_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-lg-2 control-label">User Type</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlUsrType" CssClass="form-control" AutoPostBack="True" OnDataBinding="DdlUsrType_DataBinding" OnSelectedIndexChanged="DdlUsrType_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label class="col-lg-2 control-label">User</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlUsr" CssClass="form-control" AutoPostBack="True" OnDataBinding="DdlUsr_DataBinding" OnSelectedIndexChanged="DdlUsr_SelectedIndexChanged">
                                        <asp:ListItem Value="" Selected="True">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label class="col-lg-2 control-label">Type</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlType3" CssClass="form-control" AutoPostBack="True">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                        <asp:ListItem Value="M">Monthly</asp:ListItem>
                                        <asp:ListItem Value="W">Weekly</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="TxtMnth3" CssClass="col-md-2 control-label">Month</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtMnth3" runat="server" Width="160px" CssClass="form-control" BackColor="White"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtMnth3" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <div class="row" style="margin-left: 2px; display: flex; align-items: baseline;">
                        <div style="margin-right: 1.5%;">
                            <asp:Button runat="server" ID="BtnView3" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" />
                        </div>
                        <div id="DivExport" runat="server" visible="false" class="flex" style="vertical-align: bottom;">
                            <asp:ImageButton ID="IB_Print" runat="server" Height="20px" Width="32px" ImageAlign="AbsBottom" ImageUrl="~/App_Themes/Images/printer.png" OnClientClick="javascript:CallPrint('GVReportsDiv3')" Style="align-self: baseline; vertical-align: bottom" ToolTip="Print" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="IB_ExportExcel" runat="server" Height="25px" Width="40px" OnClientClick="ExportToExcelXls()" ImageAlign="AbsBottom" ImageUrl="~/App_Themes/Images/export-excel.png" ToolTip="Export to Excel" />
                        </div>
                    </div>
                    <br />
                    <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px" runat="server" id="GVReportsDiv3">
                        <asp:GridView ID="GVReports3"
                            runat="server" Font-Bold="False" CssClass="table table-bordered table-condensed table-responsive table-hover"
                            Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                            RowStyle-HorizontalAlign="LEFT" TabIndex="10"
                            OnDataBinding="GVReports3_DataBinding" Visible="False" BorderStyle="Solid" AutoGenerateColumns="False">
                            <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" Width="0em" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="False" />
                            <EditRowStyle BackColor="#7C6F57" />
                            <AlternatingRowStyle BackColor="#7ad0ed" />
                            <Columns>
                                <asp:TemplateField HeaderText="Reject" Visible="true" ControlStyle-CssClass="form-check-input" ControlStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="CBRejectH" runat="server" OnCheckedChanged="CBRejectH_CheckedChanged" AutoPostBack="true" ToolTip="Reject" Text=""></asp:CheckBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CBReject" OnCheckedChanged="CBReject_CheckedChanged" AutoPostBack="true" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <asp:CheckBox ID="CBReject" runat="server" BackColor="#7ad0ed"></asp:CheckBox>
                                    </AlternatingItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                <asp:BoundField DataField="User_Name" HeaderText="User" ReadOnly="True" />
                                <asp:BoundField DataField="Category_Type" HeaderText="Category Type" ReadOnly="True" />
                                <asp:BoundField DataField="Category_Name" HeaderText="Category" ReadOnly="True" />
                                <asp:BoundField DataField="Report_Name" HeaderText="Report" ReadOnly="True" />
                                <%--<asp:BoundField DataField="Submit_Date" HeaderText="Add Date" />--%>
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                <asp:TemplateField HeaderText="File" Visible="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LBLocn" runat="server" OnClick="LBLocn_Click" ForeColor="#3366FF"
                                            Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'
                                            ToolTip='<%# Bind("Location")%>'>
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
                        <asp:Button runat="server" ID="BtnReject" OnClick="BtnReject_Click" Text="Reject" CssClass="btn btn-primary" ForeColor="White" Visible="False" Enabled="False" />
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.5/xlsx.full.min.js"></script>

    <script type="text/javascript">

        function CallPrint(strid) {
            var gridHtml = document.getElementById('<%= GVReports3.ClientID %>').outerHTML;

            // Open a new window
            var printWindow = window.open('', '_blank', 'width=600,height=600');

            // Write the GridView HTML content to the new window
            printWindow.document.write('<html><head><title>Review Print</title></head><body>');
            printWindow.document.write('<h2>Review Content</h2>');
            printWindow.document.write(gridHtml);
            printWindow.document.write('</body></html>');

            // Close the document and initiate the print
            printWindow.document.close();
            printWindow.print();
        }

        function ExportToExcelXls() {
            debugger;

            // Get the GridView HTML content
            var gridViewHtml = document.getElementById('<%= GVReports3.ClientID %>').outerHTML;

            // Create a Blob from the HTML content
            var blob = new Blob([gridViewHtml], { type: 'application/vnd.ms-excel' });

            // Create a download link
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);

            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
            var yyyy = today.getFullYear();

            today = dd + '-' + mm + '-' + yyyy;

            link.download = 'Review_' + today + '.xls';

            // Append the link to the body and trigger the download
            document.body.appendChild(link);
            link.click();

            // Remove the link from the body
            document.body.removeChild(link);
        }

        function ExportToExcel() {
            //debugger;
            var grid = document.getElementById('<%= GVReports3.ClientID %>');
            //debugger;

            // Convert GridView data to a worksheet
            var worksheet = XLSX.utils.table_to_sheet(grid);
            //debugger;

            // Create a workbook with a single worksheet
            var workbook = XLSX.utils.book_new();
            //debugger;
            XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');
            //debugger;

            // Save the workbook as a blob
            var blob = XLSX.write(workbook, { bookType: 'xlsx', type: 'blob' });
            //debugger;

            // Create a download link
            var link = document.createElement('a');
            //debugger;
            link.href = URL.createObjectURL(blob);
            //debugger;
            link.download = 'GridViewData.xlsx';
            //debugger;

            // Append the link to the body and trigger the download
            document.body.appendChild(link);
            //debugger;
            link.click();
            //debugger;

            // Remove the link from the body
            document.body.removeChild(link);
            //debugger;
        }
    </script>
</asp:Content>
