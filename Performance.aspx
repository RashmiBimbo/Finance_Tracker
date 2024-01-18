<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Performance.aspx.cs" Inherits="Finance_Tracker.Performance" EnableEventValidation="false" Title="Performance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <br />
    <br />
    <div class="form-horizontal">
        <h4>Performance</h4>
        <hr style="padding: 1px; margin-top: 10px; margin-bottom: 10px; position: inherit;" />
        <div class="form-group">
            <%--<div class="row">--%>
            <%--<div style="padding-top: 20px; padding-bottom: 10px; margin-bottom: 10px;">--%>
            <div style="">
                <asp:Menu ID="Menu1" runat="server" BackColor="#006666" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.83em" ForeColor="White" Orientation="Horizontal" StaticSubMenuIndent="10px" OnMenuItemClick="Menu1_MenuItemClick" CssClass="form-control">
                    <DynamicHoverStyle BackColor="#3399FF" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        <asp:MenuItem Selected="True" Text="Submit Task |" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="View Submitted Tasks |" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="View Approved Tasks |" Value="2"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
            <br />
            <%--</div>--%>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="tabAdd" runat="server">
                    <div class="col-sm-12">
                        <%-- <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>--%>
                        <div class="row" style="margin-bottom: 0;">
                            <asp:Label runat="server" AssociatedControlID="DdlType1" CssClass="col-md-2 control-label">Category Type<span style="color:red"> *</span></asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlType1" CssClass="form-control" OnDataBinding="DdlType_DataBinding" OnSelectedIndexChanged="DdlType_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlType1"
                                            CssClass="text-danger" ErrorMessage="Category Type is required." />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlCat1" CssClass="col-md-2 control-label">Category<span style="color:red"> *</span></asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlCat1" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCat1"
                                            CssClass="text-danger" ErrorMessage="Category is required." />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlReport1" CssClass="col-md-2 control-label">Report<span style="color:red"> *</span></asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlReport1" CssClass="form-control" OnDataBinding="DdlReport_DataBinding">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%--   <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlReport1"
                                            CssClass="text-danger" ErrorMessage="Report is required." ID="RequiredFieldValidator1" />--%>
                            </div>
                        </div>
                        <br />
                        <div class="row" style="margin-top: 0;">
                            <%--<%-- <asp:Label runat="server" AssociatedControlID="DdlRType1" CssClass="col-md-2 control-label">Type</asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlRType1" CssClass="form-control">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                    <asp:ListItem Value="M">Monthly</asp:ListItem>
                                    <asp:ListItem Value="W">Weekly</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                            <asp:Label runat="server" AssociatedControlID="FUReport" CssClass="col-md-2 control-label">Upload<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-sm-3">
                                <asp:FileUpload ID="FUReport" runat="server" CssClass="form-control" />
                            </div>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <%--<asp:Label runat="server" AssociatedControlID="LnkReport" CssClass="col-md-2 control-label">Uploaded</asp:Label>--%>
                                    <asp:LinkButton runat="server" CssClass="col-sm-12 col-md-6 control-label" ID="LnkReport" OnClick="LnkReport_Click" ForeColor="#3366FF"></asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <%--  <div class="col-sm-2">
                                        <asp:DropDownList runat="server" ID="DdlWeek1" CssClass="form-control" Visible="false">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">1</asp:ListItem>
                                            <asp:ListItem Value="2">2</asp:ListItem>
                                            <asp:ListItem Value="3">3</asp:ListItem>
                                            <asp:ListItem Value="4">4</asp:ListItem>
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlWeek1"
                                            CssClass="text-danger" ErrorMessage="Week no. is required." />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:DropDownList runat="server" ID="DdlMonth1" CssClass="form-control" Visible="false">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">January</asp:ListItem>
                                            <asp:ListItem Value="2">February</asp:ListItem>
                                            <asp:ListItem Value="3">March</asp:ListItem>
                                            <asp:ListItem Value="4">April</asp:ListItem>
                                            <asp:ListItem Value="5">May</asp:ListItem>
                                            <asp:ListItem Value="6">June</asp:ListItem>
                                            <asp:ListItem Value="7">July</asp:ListItem>
                                            <asp:ListItem Value="8">August</asp:ListItem>
                                            <asp:ListItem Value="9">September</asp:ListItem>
                                            <asp:ListItem Value="10">October</asp:ListItem>
                                            <asp:ListItem Value="11">November</asp:ListItem>
                                            <asp:ListItem Value="12">December</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMonth1"
                                            CssClass="text-danger" ErrorMessage="Month is required." />
                                    </div>--%>
                        <br />
                        <%--  <div class="row">
                        <asp:Label runat="server" AssociatedControlID="FUReport" CssClass="col-md-2 control-label">Upload<span style="color:red"> *</span></asp:Label>
                        <div class="col-sm-3">
                            <asp:FileUpload ID="FUReport" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="FUReport" CssClass="text-danger" ErrorMessage="Please upload a file." ID="RequiredFieldValidator2" SetFocusOnError="True" />
                        </div>
                    </div>--%>
                        <%-- <asp:UpdatePanel runat="server">
                            <ContentTemplate>--%>
                        <div class="row">
                            <%--<asp:Label runat="server" CssClass="col-md-4 control-label" ID="LblReport"></asp:Label>--%>
                            <%--<div class="col-sm-2">--%>
                        </div>
                        <div class="row" style="padding: 10px">
                            <asp:Button runat="server" ID="BtnCncl" OnClick="BtnCncl_Click" Text="Cancel" CssClass="col-2 btn btn-default" ForeColor="White" Width="8%" Visible="false" />
                            <asp:Button runat="server" ID="BtnAdd" OnClick="BtnAdd_Click" Text="Add" CssClass="btn btn-primary" ForeColor="White" Width="8%" />
                        </div>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                    <%-- </div>--%>
                </asp:View>
                <asp:View ID="TabView" runat="server">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="DdlType2" CssClass="col-md-2 control-label">Category Type</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlType2" CssClass="form-control" OnDataBinding="DdlType2_DataBinding" OnSelectedIndexChanged="DdlType_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlType2"
                                            CssClass="text-danger" ErrorMessage="Category Type is required." />--%>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlCat2" CssClass="col-md-2 control-label">Category</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCat2" CssClass="form-control" OnDataBinding="DdlCat2_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCat2"
                                            CssClass="text-danger" ErrorMessage="Category is required." />--%>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlReport2" CssClass="col-md-2 control-label">Report</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlReport2" CssClass="form-control" OnDataBinding="DdlReport_DataBinding">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlReport2"
                                            CssClass="text-danger" ErrorMessage="Report is required." ID="RequiredFieldValidator5" />--%>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <%--  <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlWeek2" CssClass="form-control" Visible="false">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlWeek2"
                                            CssClass="text-danger" ErrorMessage="Week no. is required." ID="RFVDdlWeek2" Enabled="false" />--%>
                                <%-- </div--%>
                                <%--<div class="conatiner">
                                    <div class="row">
                                        <label class="col-3 control-label"">Type</label>
                                        <div class="col-3 radio">
                                            <div class="radio" style="vertical-align: baseline; display: inline">
                                            <asp:RadioButton AutoPostBack="true" ID="RBAll2" Text="All" runat="server" OnCheckedChanged="RB_CheckedChanged" />
                                        </div>
                                        <div class="col-3 radio" >
                                            <asp:RadioButton AutoPostBack="true" ID="RBMonth2" Text="Monthly" runat="server" OnCheckedChanged="RB_CheckedChanged" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </div>
                                        <div class="col-3 radio">
                                            <asp:RadioButton AutoPostBack="true" ID="RBWeek2" Text="Weekly" runat="server" OnCheckedChanged="RB_CheckedChanged" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </div>
                                    </div>
                                </div>
                               </div>--%>
                                <%--  <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlMonth2" CssClass="form-control" Visible="false">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="1">January</asp:ListItem>
                                        <asp:ListItem Value="2">February</asp:ListItem>
                                        <asp:ListItem Value="3">March</asp:ListItem>
                                        <asp:ListItem Value="4">April</asp:ListItem>
                                        <asp:ListItem Value="5">May</asp:ListItem>
                                        <asp:ListItem Value="6">June</asp:ListItem>
                                        <asp:ListItem Value="7">July</asp:ListItem>
                                        <asp:ListItem Value="8">August</asp:ListItem>
                                        <asp:ListItem Value="9">September</asp:ListItem>
                                        <asp:ListItem Value="10">October</asp:ListItem>
                                        <asp:ListItem Value="11">November</asp:ListItem>
                                        <asp:ListItem Value="12">December</asp:ListItem>
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMonth2"
                                            CssClass="text-danger" ErrorMessage="Month is required." ID="RFVDdlMonth2" Enabled="false" />--%>
                                <%-- </div>--%>
                                <%--<div class="col-md-12">
                                   <div class="card">
                                        <div class="card-body">--%>
                                <%-- <div class="form-group row">--%>
                                <asp:Label runat="server" AssociatedControlID="TxtSD" CssClass="col-md-2 control-label">Start Date<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtSD" runat="server" Width="160px" CssClass="form-control" BackColor="White" OnTextChanged="TxtSD_TextChanged"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtSD" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="dd-MMM-yyyy" Format="dd-MMM-yyyy" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtSD" CssClass="text-danger" ErrorMessage="Please select start date." ID="RequiredFieldValidator3" />
                                </div>
                                <asp:Label runat="server" AssociatedControlID="TxtED" CssClass="col-md-2 control-label">End Date<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtED" runat="server" Width="160px" CssClass="form-control" BackColor="White" CausesValidation="True"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtED" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="dd-MMM-yyyy" Format="dd-MMM-yyyy" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtED" CssClass="text-danger" ErrorMessage="Please select end date." ID="RequiredFieldValidator4" />
                                    <%-- </div>--%>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlRType2" CssClass="col-md-2 control-label">Type</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlRType2" CssClass="form-control" AutoPostBack="True">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                        <asp:ListItem Value="M">Monthly</asp:ListItem>
                                        <asp:ListItem Value="W">Weekly</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <%--</div>
                                    </div
                                </div>>--%>
                            </div>
                            <br />
                            <%--<div class="row">
                            </div>
                            <br />--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row">
                        <div class="col-lg-10" style="padding-right: 2px; margin-left: 5px; margin-right: 5px; width: 100px; height: 50px;">
                            <asp:Button runat="server" ID="BtnView" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" Width="83px" UseSubmitBehavior="true" />
                        </div>
                    </div>
                    <div style="width: auto; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; padding: 25px; margin = 50px" runat="server" id="Div1">
                        <asp:GridView ID="GVReports2"
                            runat="server" CellPadding="20" CellSpacing="15" Font-Bold="False"
                            Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                            RowStyle-HorizontalAlign="LEFT" RowStyle-Wrap="true"
                            HeaderStyle-Wrap="false" TabIndex="10"
                            OnDataBinding="GVReports_DataBinding" Visible="False" BorderStyle="Inset" AllowSorting="True" AutoGenerateColumns="False">
                            <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="true" Height="5%" width="5%"/>
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="True" />
                            <Columns>
                                <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                <asp:BoundField DataField="User_Name" HeaderText="User Name" ReadOnly="True" />
                                <asp:BoundField DataField="Category_Type" HeaderText="Category Type" ReadOnly="True" />
                                <asp:BoundField DataField="Category_Name" HeaderText="Category Name" ReadOnly="True" />
                                <asp:BoundField DataField="Report_Name" HeaderText="Report Name" ReadOnly="True" />
                                <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <%--<asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="True" />--%>
                                <asp:TemplateField HeaderText="Location" Visible="true">
                                    <ItemTemplate>
                                        <asp:HiddenField runat ="server" ID="HFLocn" Value='<%# Bind("Location") %>' />
                                        <asp:Label ID="LblLocn" runat="server" Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Edit" HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="BtnEdit" OnCommand="BtnEdit_Command" CommandName="GetRow" CommandArgument="<%# Container.DataItemIndex %>" Text="Edit"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Id" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblId" runat="server" Text='<%# Bind("Rec_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Report_Id" Visible="False" InsertVisible="False" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblReport_Id" runat="server" Text='<%# Bind("Report_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#7C6F57" />
                            <AlternatingRowStyle BackColor="#7ad0ed" />
                        </asp:GridView>
                    </div>
                    <div class="row">
                        <div class="col-lg-10" style="padding-right: 2px; margin-left: 10px; margin-right: 10px; width: 100px; height: 50px;">
                            <asp:Button runat="server" ID="BtnApprove" OnClick="BtnApprove_Click" Text="Approve" CssClass="btn btn-primary" ForeColor="White" Width="83px" Visible="False" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="TabApproved" runat="server">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                        <ContentTemplate>
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="DdlType3" CssClass="col-md-2 control-label">Category Type</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlType3" CssClass="form-control" OnDataBinding="DdlType3_DataBinding" OnSelectedIndexChanged="DdlType_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlType3"
                                        CssClass="text-danger" ErrorMessage="Category Type is required." />
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlCat3" CssClass="col-md-2 control-label">Category</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCat3" CssClass="form-control" OnDataBinding="DdlCat3_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCat3"
                                        CssClass="text-danger" ErrorMessage="Category is required." />
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlReport3" CssClass="col-md-2 control-label">Report</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlReport3" CssClass="form-control" OnDataBinding="DdlReport_DataBinding">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlReport3"
                                        CssClass="text-danger" ErrorMessage="Report is required." ID="RFVDR3" />--%>
                                </div>
                            </div>
                            <div class="row">
                                <%-- <div class="form-group">--%>
                                <label class="col-lg-2 control-label">Month</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlMonth3" CssClass="form-control">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="1">January</asp:ListItem>
                                        <asp:ListItem Value="2">February</asp:ListItem>
                                        <asp:ListItem Value="3">March</asp:ListItem>
                                        <asp:ListItem Value="4">April</asp:ListItem>
                                        <asp:ListItem Value="5">May</asp:ListItem>
                                        <asp:ListItem Value="6">June</asp:ListItem>
                                        <asp:ListItem Value="7">July</asp:ListItem>
                                        <asp:ListItem Value="8">August</asp:ListItem>
                                        <asp:ListItem Value="9">September</asp:ListItem>
                                        <asp:ListItem Value="10">October</asp:ListItem>
                                        <asp:ListItem Value="11">November</asp:ListItem>
                                        <asp:ListItem Value="12">December</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMonth3"
                                            CssClass="text-danger" ErrorMessage="Month is required." ID="RFVDdlMonth3" Enabled="false" />--%>
                                </div>
                                <%--</div>--%>
                                <%--<div class="form-group">--%>
                                <label class="col-lg-2 control-label">Type</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlRType3" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DdlRType3_SelectedIndexChanged">
                                        <asp:ListItem Value="">All</asp:ListItem>
                                        <asp:ListItem Value="M">Monthly</asp:ListItem>
                                        <asp:ListItem Value="W">Weekly</asp:ListItem>
                                    </asp:DropDownList>
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
                                        </asp:DropDownList>--%>
                                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlWeek3"
                                        CssClass="text-danger" ErrorMessage="Week no. is required." ID="RFVDdlWeek3" Enabled="false" />--%>
                                <%--</div>
                                </div>--%>
                                <%-- </div>--%>
                            </div>
                            <br />
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="form-group row">
                                            <div class="col-lg-10" style="padding-right: 2px; margin-left: 5px; margin-right: 5px; width: 100px; height: 50px;">
                                                <asp:Button runat="server" ID="BtnView3" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" Width="83px" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; padding: 25px; margin = 50px" runat="server" id="GVReportsDiv3">
                                <asp:GridView ID="GVReports3"
                                    runat="server" CellPadding="20" CellSpacing="15" Font-Bold="False"
                                    Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                                    RowStyle-HorizontalAlign="LEFT" RowStyle-Wrap="true"
                                    HeaderStyle-Wrap="false" TabIndex="10"
                                    OnDataBinding="GVReports3_DataBinding" Visible="False" BorderStyle="Outset" AllowSorting="True" AutoGenerateColumns="False">
                                    <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="true" Height="5%" width="5%"/>
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="True" />
                                    <Columns>
                                        <asp:BoundField DataField="User_Name" HeaderText="User Name" ReadOnly="True" />
                                        <asp:BoundField DataField="Category_Type" HeaderText="Category Type" ReadOnly="True" />
                                        <asp:BoundField DataField="Category_Name" HeaderText="Category Name" ReadOnly="True" />
                                        <asp:BoundField DataField="Report_Name" HeaderText="Task Name" ReadOnly="True" />
                                        <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                        <asp:BoundField DataField="Type" HeaderText="Type" />
                                        <asp:BoundField DataField="Approve_Date" HeaderText="Approve Date" />
                                        <%-- <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="True" ItemStyle-Width="10%" ItemStyle-Height="5%" />--%>
                                        <asp:TemplateField HeaderText="Location" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="LblLocn" runat="server" Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <AlternatingRowStyle BackColor="#7ad0ed" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <script type="text/javascript">
        function ValidateFileExt() {
            var uploadr = document.getElementById('<%= FUReport.ClientID%>')
            var filePath = uploadr.value;
            var allowedExt = /(\.xlsx|\.xls)$i/;

            if (!allowedExt.exec(filePath)) {
                alert('Please upload only Excel files.');
                uploadr.value = "";
                return false;
            }
            return true;
        }

        function dateSelected(sender, args) {
            debugger;
            var selectedDate = args._selectedDate;
            var textBox = $get('<%= TxtSD.ClientID %>');
            textBox.value = selectedDate.format('dd-MMM-yyyy');
        }

    </script>
</asp:Content>
