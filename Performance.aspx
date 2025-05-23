﻿<%@ Page Title="Performance" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Performance.aspx.cs" Inherits="Finance_Tracker.Performance" EnableEventValidation="true" MaintainScrollPositionOnPostback="True" Async="True" %>

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
                            <div id="DvMnth" runat="server">
                                <asp:Label runat="server" AssociatedControlID="TxtMnthM" CssClass="col-md-2 control-label">Month<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtMnthM" runat="server" Width="160px" CssClass="form-control" BackColor="White" autocomplete="off" OnTextChanged="TxtMnth_TextChanged" Text='<%# DateTime.Now.ToString("MMM-yyyy") %>' AutoPostBack="True" required="required"></asp:TextBox>
                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="Select Month" TargetControlID="TxtMnthM" />
                                    <ajaxToolkit:CalendarExtender ID="CETxtMnthM" runat="server" TargetControlID="TxtMnthM" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                                </div>
                            </div>
                            <asp:Label runat="server" ID="LblTypM" AssociatedControlID="DdlTypeM" CssClass="col-md-2 control-label">Type<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-sm-2 col-md-2">
                                <asp:DropDownList runat="server" ID="DdlTypeM" CssClass="form-control" Enabled="true" AutoPostBack="true" OnSelectedIndexChanged="DdlType_SelectedIndexChanged" OnDataBinding="DdlType_DataBinding" required="required">
                                    <asp:ListItem Text="Select" Value="" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="DivWeekM" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="DdlWeekM" CssClass="col-md-2 control-label" ID="LblWeek">Week no.<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlWeekM" CssClass="form-control" onchange="HideDivGVBtnM()" required="required">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="DvQrtrM" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="DdlQrtrM" CssClass="col-md-2 control-label" ID="LblQrtrM">Quarter  no.<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlQrtrM" CssClass="form-control" required="required" onchange="HideDivGVBtnM()">
                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                        <asp:ListItem Text="1" Value="1" />
                                        <asp:ListItem Text="2" Value="2" />
                                        <asp:ListItem Text="3" Value="3" />
                                        <asp:ListItem Text="4" Value="4" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="DvHY" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="DdlHY" CssClass="col-md-2 control-label" ID="LblHy">Half no.<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlHY" CssClass="form-control" onchange="HideDivGVBtnM()" required="required">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                        </div>
                        <div class="row">
                            <div class="col-sm-2 col-md-2">
                                <asp:Button runat="server" ID="BtnViewAssTask" OnClick="BtnViewAssTask_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" Enabled="true" Visible="true" />
                            </div>
                        </div>
                        <br />
                        <div runat="server" id="DivGVBtnM" visible="true">
                            <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto;" runat="server" visible="true" id="DivGVAdd">
                                <asp:GridView ID="GVAdd"
                                    runat="server" Font-Bold="False" CssClass="table table-bordered  table-responsive table-hover border" Font-Size="Medium" ForeColor="#333333" GridLines="Both" RowStyle-HorizontalAlign="LEFT" TabIndex="10" OnDataBinding="GVAdd_DataBinding" BorderStyle="Solid" AutoGenerateColumns="False" AllowSorting="True">
                                    <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" Width="0em" />
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="False" />
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <%--<AlternatingRowStyle BackColor="#7ad0ed" />--%>
                                    <Columns>
                                        <%--0 --%>
                                        <asp:TemplateField Visible="true">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="CBAddH" runat="server" onclick="CBAddHOnClick(this);" TextAlign="Right" ToolTip="Add" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CBAdd" runat="server" ToolTip="Add"
                                                    onclick="CBAddOnClick(this);" />
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
                                        <asp:TemplateField HeaderText="Upload File" Visible="true" ControlStyle-Width="230px">
                                            <ItemTemplate>
                                                <asp:FileUpload runat="server" ID="FUAdd" CssClass="form-control" AllowMultiple="false" Height="40px" />
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
                                <asp:DropDownList runat="server" ID="DdlCatTypeS" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True" onchange="UpdateToolTip(this);">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCatType1"
                                            CssClass="text-danger" ErrorMessage="Category Type is required." />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlCatS" CssClass="col-md-2 control-label">Category</asp:Label>
                            <div class="col-sm-2">
                                <%-- <div class="form-group">--%>
                                <asp:DropDownList runat="server" ID="DdlCatS" CssClass="form-control text-capitalize" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True" onchange="UpdateToolTip(this);">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCat1"
                                            CssClass="text-danger" ErrorMessage="Category is required." />--%>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="DdlReportS" CssClass="col-md-2 control-label">Report</asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlReportS" CssClass="form-control" OnDataBinding="DdlReport_DataBinding"
                                    OnSelectedIndexChanged="DdlReport_SelectedIndexChanged" AutoPostBack="True" onchange="UpdateToolTip(this);">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                </asp:DropDownList>
                                <%--   <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlReport1"
                                            CssClass="text-danger" ErrorMessage="Report is required." ID="RequiredFieldValidator1" />--%>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <asp:Label runat="server" ID="LblTypS" AssociatedControlID="DdlTypeS" CssClass="col-md-2 control-label">Type</asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="DdlTypeS" CssClass="form-control" Enabled="False" OnDataBinding="DdlType_DataBinding" onchange="UpdateToolTip(this);">
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
                                    <asp:DropDownList runat="server" ID="DdlWeekS" CssClass="form-control" onchange="UpdateToolTip(this);">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2" id="DvQrtrS" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="DdlQrtr" CssClass="col-md-2 control-label" ID="LblQrtrS" />H
                                    <asp:DropDownList runat="server" ID="DdlQrtrS" CssClass="form-control" required="required">
                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                        <asp:ListItem Text="1" Value="1" />
                                        <asp:ListItem Text="2" Value="2" />
                                        <asp:ListItem Text="3" Value="3" />
                                        <asp:ListItem Text="4" Value="4" />
                                    </asp:DropDownList>
                            </div>
                            <div id="DvHyS" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="DdlHyS" CssClass="col-md-2 control-label">Half no.</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlHyS" CssClass="form-control" onchange="HideDivGVBtnM()" required="required">
                                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="DivCmnts" runat="server" visible="false">
                                <asp:Label runat="server" AssociatedControlID="TxtCmnts" ID="LblCmnts" CssClass="col-md-2 control-label">Comments</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="TxtCmnts" runat="server" Width="160px" CssClass="form-control" BackColor="White" TextMode="MultiLine" Enabled="false" Height="40px" onchange="UpdateToolTip(this);"></asp:TextBox>
                                </div>
                            </div>
                            <asp:Label runat="server" AssociatedControlID="FUReport" CssClass="col-md-2 control-label" ID="LblFU">Upload<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-sm-2" id="DvUpload" runat="server">
                                <asp:FileUpload ID="FUReport" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row" runat="server" visible="false" id="DivLnk">
                            <br />
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="col-12">
                                        <asp:LinkButton class="control-label" ID="LnkReport" runat="server" OnClick="LnkReport_Click" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="LnkReport" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <br />
                        </div>
                        <div class="row">
                            <%--</div>
                            <div class="row">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>--%>
                            <asp:Button runat="server" ID="BtnAdd" OnClick="BtnAdd_Click" Text="Add" CssClass="btn btn-primary" ForeColor="White" />
                            <%-- <div>--%>
                            <asp:Button runat="server" ID="BtnCncl" OnClick="BtnCncl_Click" Text="Cancel" CssClass="btn btn-default" ForeColor="White" Visible="false" />
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
                    <div class="row">
                        <asp:Label runat="server" AssociatedControlID="DdlCatType2" CssClass="col-md-2 control-label">Category Type</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlCatType2" CssClass="form-control text-capitalize" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True" onchange="UpdateToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlCat2" CssClass="col-md-2 control-label">Category</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlCat2" CssClass="form-control text-capitalize" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True" onchange="UpdateToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlReport2" CssClass="col-md-2 control-label">Report</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlReport2" CssClass="form-control" OnDataBinding="DdlReport_DataBinding">
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
                            <ajaxToolkit:CalendarExtender ID="CETxtMnth2" runat="server" TargetControlID="TxtMnth2" CssClass="modal-content" DaysModeTitleFormat="dd-MMM-yyyy" TodaysDateFormat="MMM-yyyy" Format="MMM-yyyy" DefaultView="Months" />
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlType2" CssClass="col-md-2 control-label">Type</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlType2" CssClass="form-control" OnDataBinding="DdlType_DataBinding">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" style="margin-left: 2px;">
                        <asp:Button runat="server" ID="BtnView2" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" UseSubmitBehavior="true" />
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
                            <%--<AlternatingRowStyle BackColor="#7ad0ed" />--%>
                            <Columns>
                                <%--0 --%>
                                <asp:TemplateField Visible="true">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="CBSubmitH" runat="server" BorderStyle="None" onclick="handleCheckBoxChangeH(this);" TextAlign="Right" ToolTip="Submit" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CBSubmit" runat="server" ToolTip="Submit" onclick="handleCheckBoxChange(this);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Action" HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton CssClass="control-label" ID="BtnAction" runat="server" OnClick="BtnAction_Click" Text='<%# Bind("BtnText")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--1 --%>
                                <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="10px" />
                                <%--2 --%>
                                <%--  <asp:BoundField DataField="Category_Type" HeaderText="Category Type" ReadOnly="True" />--%>
                                <asp:TemplateField HeaderText="" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCatType" runat="server" Text='<%# Bind("Category_Type")%>'></asp:Label>
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
                                <%--10--%>
                                <asp:TemplateField HeaderText="File" Visible="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LBGVReprts" runat="server" class="control-label"
                                            Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>' OnClick="LnkReport_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--11--%>
                                <%--12--%>
                                <asp:TemplateField HeaderText="Report_Id" Visible="False" InsertVisible="False" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="LblReport_Id" runat="server" Text='<%# Bind("Report_Id") %>'></asp:Label>
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
                    <div class="row">
                        <asp:Label runat="server" AssociatedControlID="DdlCatType3" CssClass="col-md-2 control-label">Category Type</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlCatType3" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True" onchange="UpdateToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlCat3" CssClass="col-md-2 control-label">Category</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlCat3" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True" onchange="UpdateToolTip(this);">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:Label runat="server" AssociatedControlID="DdlReport3" CssClass="col-md-2 control-label">Report</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlReport3" CssClass="form-control" OnDataBinding="DdlReport_DataBinding">
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
                        </div>
                        <label class="col-lg-2 control-label">Type</label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlType3" CssClass="form-control" OnDataBinding="DdlType_DataBinding">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" style="margin-left: 2px;">
                        <asp:Button runat="server" ID="BtnView3" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" />
                    </div>
                    <br />
                    <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto; margin-bottom: 10px" runat="server" id="GVReportsDiv3">
                        <asp:GridView ID="GVReports3"
                            runat="server" CssClass="table table-bordered table-striped table-hover" CellPadding="20" CellSpacing="15" Font-Bold="False"
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
                                <asp:BoundField DataField="Category_Name" HeaderText="Category Name" ReadOnly="True" />
                                <asp:BoundField DataField="Report_Name" HeaderText="Report name" ReadOnly="True" />
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                <asp:BoundField DataField="Add_Date" HeaderText="Add Date" />
                                <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" />
                                <asp:BoundField DataField="Approve_Date" HeaderText="Approve Date" />
                                <asp:TemplateField HeaderText="File" Visible="true">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="HFLocn" Value='<%# Bind("Location") %>' />
                                        <asp:LinkButton CssClass="control-label" ID="LnkReportSbmt" runat="server" OnClick="LnkReport_Click" Text='<%# System.IO.Path.GetFileName(Eval("Location").ToString())%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#7C6F57" />
                            <%--<AlternatingRowStyle BackColor="#7ad0ed" />--%>
                        </asp:GridView>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script src="assets/libs/Common.js" type="text/javascript"></script>

    <script type="text/javascript">

        let chKCount = 0;
        let chKCntA = 0;

        $(document).ready(function () {
            //debugger;
            var GVAdd = document.getElementById('<%= GVAdd.ClientID %>');
            if (GVAdd !== null) {
                for (let i = 1; i < GVAdd.rows.length; i++) {
                    let cbChld = GVAdd.rows[i].cells[0].querySelector('input[type="checkbox"]');
                    if (cbChld !== null) {
                        let chkd = cbChld.checked;
                        chKCntA += chkd ? 1 : 0;
                    }
                }
                //debugger;
                var cbH = GVAdd.rows[0].cells[0].querySelector('input[type="checkbox"]');
                if (cbH !== null) cbH.checked = (chKCntA == GVAdd.rows.length - 1);
                EnableDisableButton(chKCntA, '<%= BtnAddM.ClientID%>');
            }
            var GVReports2 = document.getElementById('<%= GVReports2.ClientID %>');
            if (GVReports2 !== null) {
                for (let i = 1; i < GVReports2.rows.length; i++) {
                    let cbChld = GVReports2.rows[i].cells[0].querySelector('input[type="checkbox"]');
                    if (cbChld !== null) {
                        let chkd = cbChld.checked;
                        chKCount += chkd ? 1 : 0;
                    }
                }
                //debugger;
                var cbH = GVReports2.rows[0].cells[0].querySelector('input[type="checkbox"]');
                if (cbH !== null) cbH.checked = (chKCount == GVReports2.rows.length - 1);
                EnableDisableButton(chKCount, '<%= BtnSubmit.ClientID%>');
            }
        });

        //Handle checkbox change for checkbox in Header row of GVReports2
        function handleCheckBoxChangeH (cb) {
            //debugger;
            var GVReports2 = document.getElementById('<%= GVReports2.ClientID %>');
            if (GVReports2 == null) return;
            //Update each row's checkbox
            for (let i = 1; i < GVReports2.rows.length; i++) {
                let cbChld = GVReports2.rows[i].cells[0].querySelector('input[type="checkbox"]');
                let chkdH = cb.checked;
                if (chkdH !== cbChld.checked) {
                    cbChld.checked = chkdH;
                    chKCount += chkdH ? 1 : -1;
                }
            }
            if (GVReports2.rows.length < chKCount)
                chKCount = GVReports2.rows.length;
            else if (chKCount < 0)
                chKCount = 0;

            EnableDisableButton(chKCount, '<%= BtnSubmit.ClientID%>');
        }

        //Handle checkbox change for checkbox in each row of GVReports2
        function handleCheckBoxChange (cb) {
            //debugger;
            chKCount += cb.checked ? 1 : -1;
            var GVReports2 = document.getElementById('<%= GVReports2.ClientID%>');

            if (GVReports2 == null) return;

            if (GVReports2.rows.length < chKCount)
                chKCount = GVReports2.rows.length;
            else if
                (chKCount < 0) chKCount = 0;

            // Update header checkbox
            let cbH = GVReports2.rows[0].querySelector('th input[type="checkbox"]');
            cbH.checked = (GVReports2.rows.length - 1 === chKCount);
            EnableDisableButton(chKCount, '<%= BtnSubmit.ClientID%>');
        }

        //Handle checkbox change for checkbox in Header row of GVReports2
        function CBAddHOnClick (cb) {
            //debugger;
            var GVAdd = document.getElementById('<%= GVAdd.ClientID %>');

            if (GVAdd == null) return;

            //Update each row's checkbox
            for (let i = 1; i < GVAdd.rows.length; i++) {
                let cbChld = GVAdd.rows[i].cells[0].querySelector('input[type="checkbox"]');
                let chkdH = cb.checked;
                if (chkdH !== cbChld.checked) {
                    cbChld.checked = chkdH;
                    chKCntA += chkdH ? 1 : -1;
                }
            }
            if (GVAdd.rows.length < chKCntA)
                chKCntA = GVAdd.rows.length;
            else if (chKCntA < 0)
                chKCntA = 0;

            EnableDisableButton(chKCntA, '<%= BtnAddM.ClientID%>');
        }

        //Handle checkbox change for checkbox in each row of GVAdd
        function CBAddOnClick (cb) {
            //debugger;
            chKCntA += cb.checked ? 1 : -1;
            var GVAdd = document.getElementById('<%= GVAdd.ClientID%>');

            if (GVAdd == null) return;

            if (GVAdd.rows.length < chKCntA)
                chKCntA = GVAdd.rows.length;
            else if
                (chKCntA < 0) chKCntA = 0;

            // Update header checkbox
            let cbH = GVAdd.rows[0].querySelector('th input[type="checkbox"]');
            cbH.checked = (GVAdd.rows.length - 1 === chKCntA);
            EnableDisableButton(chKCntA, '<%= BtnAddM.ClientID%>');
        }

        function EnableDisableButton (count, ctrlId) {
            let btnAddM = document.getElementById(ctrlId);
            btnAddM.disabled = (count === 0);
        }

        function HideDivGVBtnM () {
            $('#<%= DivGVBtnM.ClientID%>').hide();
        }

        function DdlType_SelectedIndexChanged () {
           <%-- console.log("DdlType_SelectedIndexChanged s");
            let DivGVBtnMId = "#<%=DivGVBtnM.ClientID%>";
            let DivWeekMId = "#<%=DivWeekM.ClientID%>";
            let DvHYId = "#<%=DvHY.ClientID%>";
            let ddlTypeValue = $("#<%=DdlTypeM.ClientID%>").val().toUpperCase().trim();

            $(DivGVBtnMId).hide();
            $(DivWeekMId).hide();
            $(DvHYId).hide();

            switch (ddlTypeValue)
            {
                case "WEEKLY":
                    console.log(ddlTypeValue);
                    var DivWeekM = document.getElementById('<%=DivWeekM.ClientID%>');
                    DivWeekM.style.display = 'block'; // Show the div
                    var DdlWeekM = document.getElementById('<%=DdlWeekM.ClientID%>');
                    DdlWeekM.value = "0";
                    break;
                case "HALF YEARLY":
                    console.log(ddlTypeValue);
                    var DvHY = document.getElementById('<%=DvHY.ClientID%>');
                    DvHY.style.display = 'block'; // Show the div
                    var DdlHY = document.getElementById('<%=DdlHY.ClientID%>');
                    DdlHY.value = "0";
                    break;
            }
            console.log("DdlType_SelectedIndexChanged e");--%>
        }

    </script>
</asp:Content>
