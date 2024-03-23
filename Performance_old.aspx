<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Performance_old.aspx.cs" Inherits="Finance_Tracker.Performance_Old" EnableEventValidation="false" Title="Performance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <br />
        <br />
        <br />
        <h3>Performance</h3>
        <hr style="padding: 1px; margin-top: 10px; margin-bottom: 10px; position: inherit; line-height: 2px" />
        <div class="form-group">
            <div style="">
                <asp:Menu ID="Menu1" runat="server" BackColor="#006666" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.84em" ForeColor="White" Orientation="Horizontal" StaticSubMenuIndent="10px" OnMenuItemClick="Menu1_MenuItemClick" CssClass="form-control">
                    <DynamicHoverStyle BackColor="#3399FF" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        <asp:MenuItem Selected="True" Text="Add Task |" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="View & Submit Tasks |" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="Submitted Tasks |" Value="2"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
            <br />
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="TabAdd" runat="server">
                    <div class="col-sm-12">
                        <%--   <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>--%>
                        <div class="row">
                            <%--<div class="form-group">--%>
                            <asp:Label runat="server" AssociatedControlID="DdlCatType1" CssClass="col-md-2 control-label">Category Type<span style="color:red"> *</span></asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlCatType1" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCatType1"
                                            CssClass="text-danger" ErrorMessage="Category Type is required." />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlCat1" CssClass="col-md-2 control-label">Category<span style="color:red"> *</span></asp:Label>
                            <div class="col-sm-2">
                                <%-- <div class="form-group">--%>
                                <asp:DropDownList runat="server" ID="DdlCat1" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCat1"
                                            CssClass="text-danger" ErrorMessage="Category is required." />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlReport1" CssClass="col-md-2 control-label">Report<span style="color:red"> *</span></asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlReport1" CssClass="form-control" OnDataBinding="DdlReport_DataBinding"
                                    OnSelectedIndexChanged="DdlReport_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%--   <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlReport1"
                                            CssClass="text-danger" ErrorMessage="Report is required." ID="RequiredFieldValidator1" />--%>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <asp:Label runat="server" AssociatedControlID="DdlType1" CssClass="col-md-2 control-label">Type</asp:Label>
                            <div class="col-sm-2">
                                <%--<div class="form-group">--%>
                                <asp:DropDownList runat="server" ID="DdlType1" CssClass="form-control" Enabled="False" OnDataBinding="DdlType_DataBinding">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                    <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="TxtDueDt" CssClass="col-md-2 control-label">Due Date</asp:Label>
                            <div class="col-sm-2">
                                <%--<div class="form-group">--%>
                                <asp:TextBox ID="TxtDueDt" runat="server" Width="160px" CssClass="form-control" BackColor="White" Enabled="false" />
                            </div>
                            <asp:Label runat="server" AssociatedControlID="TxtMnth1" CssClass="col-md-2 control-label">Month</asp:Label>
                            <div class="col-sm-2">
                                <%--<div class="form-group">--%>
                                <asp:TextBox ID="TxtMnth1" runat="server" Width="160px" CssClass="form-control" BackColor="White" OnTextChanged="TxtMnth_TextChanged" Text='<%# DateTime.Now.ToString("MMM-yyyy") %>' AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtMnth1" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                            </div>
                        </div>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                        <br />
                        <div class="row">
                            <div id="DivWeek1" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="DdlWeek1" CssClass="col-md-2 control-label" ID="LblWeek1">Week no.<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlWeek1" CssClass="form-control">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="FUReport" CssClass="col-md-2 control-label">Upload<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-sm-12 col-md-3">
                                <asp:FileUpload ID="FUReport" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row" runat="server" visible="false" id="DivLnk">
                            <br />
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="col-sm-12 col-md-12 col-lg-12">
                                        <asp:LinkButton CssClass="control-label" ID="LnkReport" ForeColor="#3366FF" runat="server" OnClick="LnkReport_Click" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="LnkReport" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <br />
                            <br />
                        </div>
                        <div class="row">
                            <%-- <div>--%>
                            <asp:Button runat="server" ID="BtnCncl" OnClick="BtnCncl_Click" Text="Cancel" CssClass="col-2 btn btn-default" ForeColor="White" Visible="false" />
                            <%--</div>
                            <div class="row">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>--%>
                            <asp:Button runat="server" ID="BtnAdd" OnClick="BtnAdd_Click" Text="Add" CssClass="btn btn-primary" ForeColor="White" />
                            <%-- </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="BtnCncl" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>--%>
                            <asp:Label ID="LblTaskID" Visible="false" runat="server" />
                        </div>
                        <br />
                        <div class="row">
                            <asp:Label runat="server" ID="LblError"></asp:Label>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="TabView" runat="server">
                   <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                        <ContentTemplate>--%>
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="DdlCatType2" CssClass="col-md-2 control-label">Category Type</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCatType2" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlCat2" CssClass="col-md-2 control-label">Category</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCat2" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlReport2" CssClass="col-md-2 control-label">Report</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlReport2" CssClass="form-control" OnDataBinding="DdlReport_DataBinding" OnSelectedIndexChanged="DdlReport_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="TxtSD" CssClass="col-md-2 control-label">Start Date<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtSD" runat="server" Width="160px" CssClass="form-control" BackColor="White" OnTextChanged="TxtSD_TextChanged" Text='<%# DateTime.Now.ToString("dd-MMM-yyyy") %>'></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtSD" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="dd-MMM-yyyy" Format="dd-MMM-yyyy" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtSD" CssClass="text-danger" ErrorMessage="Please select start date." ID="RequiredFieldValidator3" />
                                </div>
                                <asp:Label runat="server" AssociatedControlID="TxtED" CssClass="col-md-2 control-label">End Date<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtED" runat="server" Width="160px" CssClass="form-control" BackColor="White" CausesValidation="True"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TxtED" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="dd-MMM-yyyy" Format="dd-MMM-yyyy"
                                        SelectedDate='<%# Eval(DateTime.Now.ToString("dd-MMM-yyyy")) %>' />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtED" CssClass="text-danger" ErrorMessage="Please select end date." ID="RequiredFieldValidator4" />
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlType2" CssClass="col-md-2 control-label">Type</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlType2" CssClass="form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                        <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <div class="row" style="margin-left: 2px;">
                        <asp:Button runat="server" ID="BtnView2" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White"  UseSubmitBehavior="true" />
                    </div>
                    <br />
                    <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px;" runat="server" id="GVReportsDiv2">
                        <asp:GridView ID="GVReports2"
                            runat="server" Font-Bold="False" CssClass="table table-bordered table-condensed table-responsive table-hover"
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
                                <%--0 --%>
                                <asp:TemplateField Visible="true">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="CBSubmitH" runat="server" BorderStyle="None" OnCheckedChanged="CBSubmitH_CheckedChanged" TextAlign="Right" ToolTip="Submit" AutoPostBack="true" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CBSubmit" runat="server" ToolTip="Submit" AutoPostBack="true" OnCheckedChanged="CBSubmit_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--1 --%>
                                <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                <%--2 --%>
                                <asp:BoundField DataField="Category_Type" HeaderText="Category Type" ReadOnly="True" />
                                <%--3 --%>
                                <asp:BoundField DataField="Category_Name" HeaderText="Category Name" ReadOnly="True" />
                                <%--4 --%>
                                <asp:BoundField DataField="Report_Name" HeaderText="Report Name" ReadOnly="True" />
                                <%--5 --%>
                                <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                <%--6 --%>
                                <asp:BoundField DataField="Add_Date" HeaderText="Add Date" />
                                <%--7 --%>
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <%--8 --%>
                                <asp:TemplateField HeaderText="File" Visible="true">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="HFLocn" Value='<%# Bind("Location") %>' />
                                        <asp:Label ID="LblLocn" runat="server" Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--9 --%>
                                <asp:TemplateField AccessibleHeaderText="Edit" HeaderText="Edit" ControlStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="BtnEdit" OnCommand="BtnEdit_Command" CommandName="EditRow" CommandArgument="<%# Container.DataItemIndex %>" Text='<%# Bind("BtnText")%>' ForeColor="#3366FF"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--10 --%>
                                <asp:TemplateField HeaderText="Report_Id" Visible="False" InsertVisible="False" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblReport_Id" runat="server" Text='<%# Bind("Report_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--11--%>
                                <asp:TemplateField HeaderText="Id" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblTaskId" runat="server" Text='<%# Bind("Task_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--12--%>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblFromDate" runat="server" Text='<%# Bind("From_Date") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--13--%>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblToDate" runat="server" Text='<%# Bind("To_Date") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--14--%>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblWeekNo" runat="server" Text='<%# Bind("Week_No") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="row" style="margin-left: 2px;">
                        <asp:Button runat="server" ID="BtnSubmit" OnClick="BtnSubmit_Click" Text="Submit" CssClass="btn btn-primary" ForeColor="White" Visible="False" Enabled="False" />
                    </div>
                </asp:View>
                <asp:View ID="TabSubmit" runat="server">
                 <%--   <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                        <ContentTemplate>--%>
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
                                <label class="col-lg-2 control-label">Type</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlType3" CssClass="form-control">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                        <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                        <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="TxtMnth3" CssClass="col-md-2 control-label">Month</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtMnth3" runat="server" Width="160px" CssClass="form-control" BackColor="White"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtMnth3" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                                </div>
                                <%--<div id="DivWeekNo" runat="server">
                                    <asp:Label class="col-lg-2 control-label" ID="LblWkNo" runat="server" Visible="False" Font-Bold="True">Week No.</asp:Label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList runat="server" ID="DdlWeek3" CssClass="form-control" Visible="false">
                                            <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                            <asp:ListItem Value="1">1</asp:ListItem>
                                            <asp:ListItem Value="2">2</asp:ListItem>
                                            <asp:ListItem Value="3">3</asp:ListItem>
                                            <asp:ListItem Value="4">4</asp:ListItem>
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>--%>
                            </div>
                            <br />
                            <div class="row" style="margin-left: 2px;">
                                <asp:Button runat="server" ID="BtnView3" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White"  />
                            </div>
                            <br />
                            <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px" runat="server" id="GVReportsDiv3">
                                <asp:GridView ID="GVReports3"
                                    runat="server" CssClass="table table-bordered table-condensed table-striped table-hover" CellPadding="20" CellSpacing="15" Font-Bold="False"
                                    Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                                    RowStyle-HorizontalAlign="LEFT" RowStyle-Wrap="false"
                                    HeaderStyle-Wrap="false" TabIndex="10"
                                    OnDataBinding="GVReports3_DataBinding" Visible="False" BorderStyle="Inset" AllowSorting="True" AutoGenerateColumns="False">
                                    <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" />
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="false" />
                                    <Columns>
                                        <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                        <%--<asp:BoundField DataField="User_Name" HeaderText="User Name" ReadOnly="True" />--%>
                                        <asp:BoundField DataField="Category_Type" HeaderText="Category Type" ReadOnly="True" />
                                        <asp:BoundField DataField="Category_Name" HeaderText="Category Name" ReadOnly="True" />
                                        <asp:BoundField DataField="Report_Name" HeaderText="Report name" ReadOnly="True" />
                                        <asp:BoundField DataField="Type" HeaderText="Type" />
                                        <%--5 --%>
                                        <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                        <asp:BoundField DataField="Add_Date" HeaderText="Add Date" />
                                        <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                        <asp:TemplateField HeaderText="File" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="LblLocn" runat="server"
                                                    Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <AlternatingRowStyle BackColor="#7ad0ed" />
                                </asp:GridView>
                            </div>
                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $("#TxtDueDt").text = "";
        });
    </script>
</asp:Content>
