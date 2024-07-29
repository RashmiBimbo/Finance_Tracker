<%@ Page Title="Review" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Review.aspx.cs" Inherits="Finance_Tracker.Review" EnableEventValidation="true" MaintainScrollPositionOnPostback="True" Async="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <br />
        <br />
        <br />
        <h3>Review</h3>
        <hr style="padding: 1px; margin-top: 10px; margin-bottom: 10px; position: inherit;" />
        <div class="form-group">
            <div>
                <asp:Menu ID="Menu1" runat="server" BackColor="#006666" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.83em" ForeColor="White" Orientation="Horizontal" StaticSubMenuIndent="10px" OnMenuItemClick="Menu1_MenuItemClick" CssClass="form-control">
                    <DynamicHoverStyle BackColor="#3399FF" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        <asp:MenuItem Text="Approved Tasks" Value="0"></asp:MenuItem>
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
                            <div class="row" runat="server" id="TabApprovedRow1" visible="false">
                                <asp:Label runat="server" AssociatedControlID="DdlCatType" CssClass="col-md-2 control-label">Category Type</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCatType" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlCat" CssClass="col-md-2 control-label">Category</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCat" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True" onchange="UpdateToolTip(this);">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlReport" CssClass="col-md-2 control-label">Report</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlReport" CssClass="form-control" OnDataBinding="DdlReport_DataBinding" onchange="UpdateToolTip(this);">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <br />
                            </div>
                            <div class="row">
                                <label class="col-lg-2 control-label">User Type</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlUsrType" CssClass="form-control" AutoPostBack="True" OnDataBinding="DdlUsrType_DataBinding" OnSelectedIndexChanged="DdlUsrType_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label class="col-lg-2 control-label">User</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlUsr" CssClass="form-control" OnDataBinding="DdlUsr_DataBinding">
                                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <label class="col-lg-2 control-label">Report Type</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlReportType" CssClass="form-control" OnDataBinding="DdlReportType_DataBinding">
                                        <asp:ListItem Selected="True" Text="All" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <label class="col-lg-2 control-label">Type</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlType" CssClass="form-control" OnSelectedIndexChanged="DdlType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Master" Value="0" Selected="True" />
                                        <asp:ListItem Text="Approved" Value="1" />
                                        <asp:ListItem Text="Submitted" Value="2" />
                                    </asp:DropDownList>
                                </div>
                                <div runat="server" id="DvTxtMnth" visible="false">
                                    <asp:Label runat="server" AssociatedControlID="TxtMnth" CssClass="col-md-2 control-label">Month<span style="color :red">&nbsp*</span></asp:Label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="TxtMnth" runat="server" Width="160px" CssClass="form-control" BackColor="White" OnTextChanged="TxtMnth_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="Select Month" TargetControlID="TxtMnth" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtMnth" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <div class="row" style="margin-left: 2px; display: flex; align-items: baseline;">
                        <div style="margin-right: 1.5%;">
                            <asp:Button runat="server" ID="BtnView" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" />
                        </div>
                        <div id="DivExport" runat="server" visible="false" class="flex" style="vertical-align: bottom;">
                            <asp:ImageButton ID="IB_Print" runat="server" Height="20px" Width="32px" ImageAlign="AbsBottom" ImageUrl="~/App_Themes/Images/printer.png" OnClientClick="javascript:CallPrint('GVReportsDiv')" Style="align-self: baseline; vertical-align: bottom" ToolTip="Print" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="IB_ExportExcel" runat="server" Height="25px" Width="40px" OnClientClick="ExportToExcelXls()" ImageAlign="AbsBottom" ImageUrl="~/App_Themes/Images/export-excel.png" ToolTip="Export to Excel" />
                        </div>
                    </div>
                    <br />
                    <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px" runat="server" id="GVReportsDiv">
                        <asp:GridView ID="GVReports"
                            runat="server" Font-Bold="False" CssClass="table table-bordered table-responsive table-hover"
                            Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                            RowStyle-HorizontalAlign="LEFT" TabIndex="10"
                            OnDataBinding="GVReports_DataBinding" Visible="False" BorderStyle="Solid" AutoGenerateColumns="False">
                            <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" Width="0em" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="False" />
                            <EditRowStyle BackColor="#7C6F57" />
                            <AlternatingRowStyle BackColor="#7ad0ed" />
                            <Columns>
                                <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                <asp:BoundField DataField="User_Name" HeaderText="User" ReadOnly="True" />
                                <%--<asp:BoundField DataField="Category_Type_Name" HeaderText="Category Type" ReadOnly="True" />--%>
                                <%--<asp:BoundField DataField="Category_Name" HeaderText="Category" ReadOnly="True" />--%>
                                <asp:BoundField DataField="Report_Name" HeaderText="Report" ReadOnly="True" />
                                <asp:BoundField DataField="Report_Type" HeaderText="Report Type" />
                                <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                <asp:BoundField DataField="Priority" HeaderText="Priority" />
                                <asp:BoundField DataField="Weight" HeaderText="Weight" />
                                <asp:BoundField DataField="Approve_Date" HeaderText="Approve Date" />
                                <asp:BoundField DataField="Approver" HeaderText="Approver" />
                                <asp:TemplateField HeaderText="File" Visible="true">      
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LBLocn" runat="server" OnClick="LBLocn_Click" ForeColor="#3366FF"
                                            Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" Visible="false">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="HFTaskId" Value='<%# Bind("Task_Id")%>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" Visible="false">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="HFUserId" Value='<%# Bind("User_Id")%>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.5/xlsx.full.min.js"></script>
    <script src="assets/libs/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            // Get the dropdown and the div element by their client-side IDs
            var ddlType = document.getElementById('<%= DdlType.ClientID %>');
            var dvTxtMnth = document.getElementById('<%= DvTxtMnth.ClientID %>');

            // Add a change event listener to the dropdown
            ddlType.addEventListener("change", function () {
                // Get the selected value of the dropdown
                var selectedValue = ddlType.value;

                // Toggle visibility of the div based on the selected value
                if (selectedValue !== '0') {
                    dvTxtMnth.style.display = 'block'; // Show the div
                } else {
                    dvTxtMnth.style.display = 'none'; // Hide the div
                }
            });
        });
        function CallPrint (strid) {
            var gridHtml = document.getElementById('<%= GVReports.ClientID %>').outerHTML;

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

        function ExportToExcelXls () {
            debugger;

            // Get the GridView HTML content
            var gridViewHtml = document.getElementById('<%= GVReports.ClientID %>').outerHTML;

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

        function ExportToExcel () {
            //debugger;
            var grid = document.getElementById('<%= GVReports.ClientID %>');
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
