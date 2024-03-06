<%@ Page Title="Performance" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Performance.aspx.cs" Inherits="Finance_Tracker.Performance" EnableEventValidation="true" MaintainScrollPositionOnPostback="True" Async="True" %>

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
                        <asp:MenuItem Selected="True" Text="Add Tasks |" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="Submit Tasks |" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="Approved Tasks" Value="2"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
            <br />
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="TabAdd" runat="server">
                    <div id="DivAddMultiple" runat="server">
                        <div class="row">
                            <asp:Label runat="server" AssociatedControlID="TxtMnthM" CssClass="col-md-2 control-label">Month<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="TxtMnthM" runat="server" Width="160px" CssClass="form-control" BackColor="White" autocomplete="off" OnTextChanged="TxtMnth_TextChanged" Text='<%# DateTime.Now.ToString("MMM-yyyy") %>' AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="Select Month" TargetControlID="TxtMnthM" />
                                <ajaxToolkit:CalendarExtender ID="CETxtMnthM" runat="server" TargetControlID="TxtMnthM" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                                <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtMnthM" CssClass="text-danger" ErrorMessage="Please select a month." ID="RequiredFieldValidatorM" />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlTypeM" CssClass="col-md-2 control-label">Type<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-sm-2 col-md-2">
                                <asp:DropDownList runat="server" ID="DdlTypeM" CssClass="form-control" Enabled="true" OnSelectedIndexChanged="DdlType_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                    <asp:ListItem Value="" Selected="True">Select</asp:ListItem>
                                    <asp:ListItem Value="M">Monthly</asp:ListItem>
                                    <asp:ListItem Value="W">Weekly</asp:ListItem>
                                </asp:DropDownList>
                                <%--  <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlTypeM" CssClass="text-danger" ErrorMessage="Please select a type." ID="RequiredFieldValidator2" />--%>
                            </div>
                            <div id="DivWeekM" runat="server" visible="true">
                                <asp:Label runat="server" AssociatedControlID="DdlWeekM" CssClass="col-md-2 control-label" ID="LblWeek">Week no.<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlWeekM" CssClass="form-control" OnSelectedIndexChanged="DdlWeek_SelectedIndexChanged" AutoPostBack="true" onchange="SetToolTip(this);">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <br />
                            </div>
                            <br />
                        </div>
                        <div class="row">
                            <div class="col-sm-2 col-md-2">
                                <asp:Button runat="server" ID="BtnViewAssTask" OnClick="BtnViewAssTask_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" Enabled="true" Visible="true" />
                            </div>
                        </div>
                        <br />
                        <%--<asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                        <div runat="server" id="DivGVBtn" visible="false">
                            <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto;" runat="server" visible="true" id="DivGVAdd">
                                <asp:GridView ID="GVAdd"
                                    runat="server" Font-Bold="False" CssClass="table table-bordered table-condensed table-responsive table-hover"
                                    Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                                    RowStyle-HorizontalAlign="LEFT" TabIndex="10"
                                    OnDataBinding="GVAdd_DataBinding" BorderStyle="Solid" AutoGenerateColumns="False" AllowSorting="True">
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
                                            <%-- <HeaderTemplate>
                                                <asp:CheckBox ID="CBSubmitH" runat="server" BorderStyle="None" OnCheckedChanged="CBSubmitH_CheckedChanged1" Text="" TextAlign="Right" ToolTip="Add" AutoPostBack="false" onchange="handleCheckBoxChange1()" />
                                            </HeaderTemplate>--%>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CBSubmit" runat="server" ToolTip="Add" AutoPostBack="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--1 --%>
                                        <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="8px" />
                                        <%--2 --%>
                                        <asp:BoundField DataField="Task_Name" HeaderText="Task" ReadOnly="True" />
                                        <%--3--%>
                                        <asp:BoundField DataField="Due_Date" HeaderText="Due Date" ReadOnly="True" />
                                        <%--4--%>
                                        <asp:BoundField DataField="Priority" HeaderText="Priority" ReadOnly="True" />
                                        <%--5--%>
                                        <asp:BoundField DataField="Weight" HeaderText="Weight" ReadOnly="True" />
                                        <%--6--%>
                                        <%--<asp:BoundField DataField="Type" HeaderText="Type" ReadOnly="True" />--%>
                                        <%--6--%>
                                        <asp:TemplateField HeaderText="Upload File" Visible="true" ControlStyle-Width="230px">
                                            <ItemTemplate>
                                                <asp:FileUpload runat="server" ID="FUAdd" CssClass="form-control" AllowMultiple="false" Height="40px" onchange="saveFileName(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--7--%>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label Text="" runat="server" ID="LblRoErr" CssClass="control-label text-danger" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <div class="row" style="margin-left: 2px;">
                                <asp:Button runat="server" ID="BtnAddM" OnClick="BtnAddM_Click" Text="Add" CssClass="btn btn-primary" ForeColor="White" Enabled="true" />
                            </div>
                        </div>
                        <%-- </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                    <div class="col-sm-12" runat="server" visible="false" id="DivAddSingl">
                        <%--   <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>--%>
                        <div class="row">
                            <asp:Label runat="server" AssociatedControlID="DdlCatTypeS" CssClass="col-md-2 control-label">Category Type</asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlCatTypeS" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCatType1"
                                            CssClass="text-danger" ErrorMessage="Category Type is required." />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlCatS" CssClass="col-md-2 control-label">Category</asp:Label>
                            <div class="col-sm-2">
                                <%-- <div class="form-group">--%>
                                <asp:DropDownList runat="server" ID="DdlCatS" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCat1"
                                            CssClass="text-danger" ErrorMessage="Category is required." />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlReportS" CssClass="col-md-2 control-label">Report</asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlReportS" CssClass="form-control" OnDataBinding="DdlReport_DataBinding"
                                    OnSelectedIndexChanged="DdlReport_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%--   <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlReport1"
                                            CssClass="text-danger" ErrorMessage="Report is required." ID="RequiredFieldValidator1" />--%>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <asp:Label runat="server" AssociatedControlID="DdlTypeS" CssClass="col-md-2 control-label">Type</asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlTypeS" CssClass="form-control" Enabled="False" onchange="SetToolTip(this);">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="M">Monthly</asp:ListItem>
                                    <asp:ListItem Value="W">Weekly</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="TxtDueDtS" CssClass="col-md-2 control-label">Due Date</asp:Label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="TxtDueDtS" runat="server" Width="160px" CssClass="form-control" BackColor="White" Enabled="false" />
                            </div>
                            <asp:Label runat="server" AssociatedControlID="TxtMnthS" CssClass="col-md-2 control-label">Month</asp:Label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="TxtMnthS" runat="server" Width="160px" CssClass="form-control" BackColor="White" OnTextChanged="TxtMnth_TextChanged" Text='<%# DateTime.Now.ToString("MMM-yyyy") %>' AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtMnthS" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                            </div>
                        </div>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                        <br />
                        <div class="row">
                            <div id="DivWeek1" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="DdlWeekS" CssClass="col-md-2 control-label" ID="LblWeekS">Week no.</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlWeekS" CssClass="form-control" onchange="SetToolTip(this);">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="DivCmnts" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="TxtCmnts" ID="LblCmnts" CssClass="col-md-2 control-label">Comments</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtCmnts" runat="server" Width="160px" CssClass="form-control" BackColor="White" TextMode="MultiLine" Enabled="false" Height="40px" onchange="SetToolTip(this);"></asp:TextBox>
                                </div>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="FUReport" CssClass="col-md-2 control-label">Upload<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-sm-2">
                                <asp:FileUpload ID="FUReport" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row" runat="server" visible="false" id="DivLnk">
                            <br />
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="col-12">
                                        <asp:LinkButton CssClass="control-label" ID="LnkReport" ForeColor="#3366FF" runat="server" OnClick="LnkReport_Click" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="LnkReport" />
                                </Triggers>
                            </asp:UpdatePanel>
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
                            <asp:DropDownList runat="server" ID="DdlCatType2" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlCat2" CssClass="col-md-2 control-label">Category</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlCat2" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlReport2" CssClass="col-md-2 control-label">Report</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlReport2" CssClass="form-control" OnDataBinding="DdlReport_DataBinding" OnSelectedIndexChanged="DdlReport_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <asp:Label runat="server" AssociatedControlID="TxtMnth2" CssClass="col-md-2 control-label" autocomplete="off">Month<span style="color:red">&nbsp*</span></asp:Label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="TxtMnth2" runat="server" Width="160px" CssClass="form-control" BackColor="White" OnTextChanged="TxtMnth_TextChanged" Text='<%# DateTime.Now.ToString("MMM-yyyy") %>' AutoPostBack="True"></asp:TextBox>
                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" WatermarkText="Select Month" TargetControlID="TxtMnth2" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtMnth2" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                            <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtMnth2" CssClass="text-danger" ErrorMessage="Please select a month." ID="RequiredFieldValidator3" />--%>
                        </div>
                        <%--<asp:Label runat="server" AssociatedControlID="TxtED" CssClass="col-md-2 control-label">End Date<span style="color:red">&nbsp*</span></asp:Label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="TxtED" runat="server" Width="160px" CssClass="form-control" BackColor="White" CausesValidation="True"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TxtED" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="dd-MMM-yyyy" Format="dd-MMM-yyyy"
                                SelectedDate='<%# Eval(DateTime.Now.ToString("dd-MMM-yyyy")) %>' />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtED" CssClass="text-danger" ErrorMessage="Please select end date." ID="RequiredFieldValidator4" />
                        </div>--%>
                        <asp:Label runat="server" AssociatedControlID="DdlType2" CssClass="col-md-2 control-label">Type</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlType2" CssClass="form-control" AutoPostBack="True" onchange="SetToolTip(this);">
                                <asp:ListItem Value="">All</asp:ListItem>
                                <asp:ListItem Value="M">Monthly</asp:ListItem>
                                <asp:ListItem Value="W">Weekly</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <div class="row" style="margin-left: 2px;">
                        <asp:Button runat="server" ID="BtnView2" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" Width="83px" UseSubmitBehavior="true" />
                    </div>
                    <br />
                    <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px;" runat="server" id="GVReportsDiv2">
                        <asp:GridView ID="GVReports2"
                            runat="server" Font-Bold="False" CssClass="table table-bordered table-responsive table-hover"
                            Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                            RowStyle-HorizontalAlign="LEFT" TabIndex="10"
                            OnDataBinding="GVReports2_DataBinding" Visible="False" BorderStyle="Solid" AutoGenerateColumns="False">
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
                                <%--  <asp:BoundField DataField="Category_Type" HeaderText="Category Type" ReadOnly="True" />--%>
                                <asp:TemplateField HeaderText="" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCatType" runat="server" Text='<%# Bind("Category_Type")%>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--3 --%>
                                <asp:BoundField DataField="Category_Name" HeaderText="Category Name" ReadOnly="True" />
                                <%--4 --%>
                                <asp:BoundField DataField="Report_Name" HeaderText="Report Name" ReadOnly="True" />
                                <%--5 --%>
                                <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                <%--6 --%>
                                <asp:BoundField DataField="Add_Date" HeaderText="Add Date" />
                                <%--7 --%>
                                <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                <%--8 --%>
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <%--9 --%>
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <%--9--%>
                                <%--<asp:BoundField DataField="Comments" HeaderText="Comments" />--%>
                                <%--10--%>
                                <asp:TemplateField HeaderText="File" Visible="true">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="HFLocn" Value='<%# Bind("Location") %>' />
                                        <asp:Label ID="LblLocn" runat="server" Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--11--%>
                                <asp:TemplateField AccessibleHeaderText="Action" HeaderText="Action" ControlStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="BtnEdit" OnCommand="BtnEdit_Command" CommandName="EditRow" CommandArgument="<%# Container.DataItemIndex %>" Text='<%# Bind("BtnText")%>' ForeColor="#3366FF"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--12--%>
                                <asp:TemplateField HeaderText="Report_Id" Visible="False" InsertVisible="False" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblReport_Id" runat="server" Text='<%# Bind("Report_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--13--%>
                                <asp:TemplateField HeaderText="Id" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblTaskId" runat="server" Text='<%# Bind("Task_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--14--%>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblFromDate" runat="server" Text='<%# Bind("From_Date") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--15--%>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblToDate" runat="server" Text='<%# Bind("To_Date") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--16--%>
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
                            <asp:DropDownList runat="server" ID="DdlCatType3" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlCat3" CssClass="col-md-2 control-label">Category</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlCat3" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlReport3" CssClass="col-md-2 control-label">Report</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlReport3" CssClass="form-control" OnDataBinding="DdlReport_DataBinding" OnSelectedIndexChanged="DdlReport_SelectedIndexChanged" AutoPostBack="True" onchange="SetToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <asp:Label runat="server" AssociatedControlID="TxtMnth3" CssClass="col-md-2 control-label">Month<span style="color :red">&nbsp*</span></asp:Label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="TxtMnth3" runat="server" Width="160px" CssClass="form-control" BackColor="White" OnTextChanged="TxtMnth_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" WatermarkText="Select Month" TargetControlID="TxtMnth3" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtMnth3" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                            <%--  <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtMnth3" CssClass="text-danger" ErrorMessage="Please select a month." ID="RequiredFieldValidator1" />--%>
                        </div>
                        <label class="col-lg-2 control-label">Type</label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlType3" CssClass="form-control" AutoPostBack="True" onchange="SetToolTip(this);">
                                <asp:ListItem Value="">All</asp:ListItem>
                                <asp:ListItem Value="M">Monthly</asp:ListItem>
                                <asp:ListItem Value="W">Weekly</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" style="margin-left: 2px;">
                        <asp:Button runat="server" ID="BtnView3" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" Width="83px" />
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
                                <%--<asp:BoundField DataField="Category_Type" HeaderText="Category Type" ReadOnly="True" />--%>
                                <asp:BoundField DataField="Category_Name" HeaderText="Category Name" ReadOnly="True" />
                                <asp:BoundField DataField="Report_Name" HeaderText="Report name" ReadOnly="True" />
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                <asp:BoundField DataField="Add_Date" HeaderText="Add Date" />
                                <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                <%--<asp:BoundField DataField="Is_Approved" HeaderText="Approved"/>--%>
                                <asp:BoundField DataField="Approve_Date" HeaderText="Approve Date" />
                                <asp:TemplateField HeaderText="File" Visible="true">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="HFLocn" Value='<%# Bind("Location") %>' />
                                        <asp:LinkButton CssClass="control-label" ID="LnkReportSbmt" ForeColor="#3366FF" runat="server" OnClick="LnkReport_Click" Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>' />
                                        <%--<asp:Label ID="LblLocn" runat="server" on
                                            Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'>
                                        </asp:Label>--%>
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
        //$(document).ready(function ()
        //{
        //    $("#TxtDueDt").text = "";
        //});
    </script>
    <script type="text/javascript">
        var previousRow = null;
        function SelectRow (row)
        {
            // Enable FileUpload control of the current row
            //var currentFileUpload = row.cells[1].getElementsByTagName("input")[0];
            //currentFileUpload.disabled = false;

            //// Disable FileUpload control of the previously selected row
            //if (previousRow !== null && previousRow !== row) {
            //    var previousFileUpload = previousRow.cells[1].getElementsByTagName("input")[0];
            //    previousFileUpload.disabled = true;
            //}

            //// Update the previousRow variable
            //previousRow = row;
        }
    </script>
    <script type="text/javascript">

        var countChecked = 0;

        function handleCheckBoxChange1 ()
        {
           <%-- debugger;
            var gv = document.getElementById('<%= GVAdd.ClientID %>');
            debugger;--%>
            //var headerCheckBox = gv.querySelectorAll('[id*="CBSubmitH].ClientID %>');
            //var rowCheckboxes = gv.querySelectorAll('[id*="CBSubmit"]');
            //for (var i = 0; i < rowCheckboxes.length; i++)
            //{
            //    if (rowCheckboxes[i].checked)
            //    {
            //        countChecked++;
            //    }
            //}
            //if (countChecked < 0)
            //    countChecked = 0;
            //else if (countChecked < rowCheckboxes.length)
            //    countChecked = rowCheckboxes.length;

            //headerCheckBox.checked = (countChecked === rowCheckboxes.length);
        }
        function handleCheckBoxChange (cb)
        {
        //debugger;
            //var gv = document.getElementById('<%= GVAdd.ClientID %>');
            //debugger;
            //var checkboxes = gv.getElementsByTagName('input');
            //debugger;
            //var headerCheckbox = gv.getElementById('<%--<%= CBSubmitH.ClientID %.--%>);
            //debugger;
            //for (var i = 0; i < checkboxes.length; i++)
            //{
            //    if (checkboxes[i].type === 'checkbox' && checkboxes[i].id.indexOf('CBSubmit') !== -1)
            //    {
            //        if (checkboxes[i].checked)
            //        {
            //            countChecked++;
            //        }
            //    }
            //}
            //headerCheckbox.checked = (countChecked === checkboxes.length - 1);
            //EnableDisableButton(countChecked); 
        }

        function EnableDisableButton (count)
        {
            <%--var btnAddM = document.getElementById('<%= BtnAddM.ClientID %>');
            btnAddM.disabled = (count === 0);--%>
        }

        function SetToolTip (ddl)
        {
            if (ddl.selectedIndex !== -1)
            {
                ddl.title = ddl.options[ddl.selectedIndex].text;
            }
        }
        //function saveFileName (fileUpload)
        //{
        //    debugger;

        //    var fileName = fileUpload.value.split('\\').pop(); // Get the file name
        //    var hiddenField = fileUpload.nextElementSibling; // Get the next hidden input element
        //    hiddenField.value = fileName; // Store the file name in the hidden field
        //}

        //window.onload = function ()
        //{
        //    debugger;
        //    var fileUploads = document.querySelectorAll('.fileNames');
        //    for (var i = 0; i < fileUploads.length; i++) {
        //        var fileName = fileUploads[i].value;
        //        var fileUpload = fileUploads[i].previousElementSibling;
        //        if (fileName && fileUpload) {
        //            fileUpload.value = fileName; // Restore file name in FileUpload control
        //        }
        //    }
        //};
        function SetToolTip (ddl)
        {
            if (ddl.selectedIndex !== -1)
            {
                ddl.title = ddl.options[ddl.selectedIndex].text;
            }
        }
    </script>
</asp:Content>
