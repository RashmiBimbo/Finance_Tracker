<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="Finance_Tracker.Masters.Reports" EnableEventValidation="true" MaintainScrollPositionOnPostback="True" Async="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <br />
        <br />
        <br />
        <h3>Reports</h3>
        <hr />
        <div class="form-group">
            <updatepanel>
                <contenttemplate>
                    <div>
                        <asp:Menu ID="Menu" runat="server" BackColor="#006666" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.84em" ForeColor="White" Orientation="Horizontal" StaticSubMenuIndent="10px" OnMenuItemClick="Menu_MenuItemClick" CssClass="form-control">
                            <DynamicHoverStyle BackColor="#3399FF" ForeColor="White" />
                            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <DynamicMenuStyle BackColor="#F7F6F3" />
                            <DynamicSelectedStyle BackColor="#5D7B9D" />
                            <Items>
                                <asp:MenuItem Selected="True" Text="Add Report |" Value="0"></asp:MenuItem>
                                <asp:MenuItem Text="View Reports" Value="1"></asp:MenuItem>
                            </Items>
                            <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <StaticSelectedStyle BackColor="#5D7B9D" />
                        </asp:Menu>
                    </div>
                    <br />
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="TabAdd" runat="server">
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="DdlCatTypeA" CssClass="col-md-2 control-label">Category Type<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2 col-md-2">
                                    <asp:DropDownList runat="server" ID="DdlCatTypeA" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True" required="required">
                                        <asp:ListItem Text="Select" Selected="True" Value="0" />
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCatTypeA" CssClass="text-danger" ErrorMessage="Category Type is required." />--%>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlCatA" CssClass="col-md-2 control-label">Category<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCatA" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" required="required">
                                        <asp:ListItem Text="Select" Selected="True" Value="0" />
                                    </asp:DropDownList>
                                    <%--  <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlCatA" CssClass="text-danger" ErrorMessage="Category is required." />--%>
                                </div>
                                <div runat="server" id="DvRprtNm">
                                    <asp:Label runat="server" AssociatedControlID="TxtReportName" CssClass="col-md-2 control-label">Report Name<span style="color:red">&nbsp*</span></asp:Label>
                                    <div class="col-sm-2">
                                        <asp:TextBox runat="server" ID="TxtReportName" TextMode="MultiLine" CssClass="form-control" MaxLength="150" Height="40px" required="required"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtReportName"
                                            CssClass="text-danger" ErrorMessage="Report Name is required." />--%>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="TxtPriority" CssClass="col-md-2 control-label">Priority<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="TxtPriority" CssClass="form-control" required="required" TextMode="Number" MaxLength="2" min="1" max="99" />
                                    <%--  <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPriority"
                                        CssClass="text-danger" ErrorMessage="Priority is required." />--%>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="TxtWeight" CssClass="col-md-2 control-label">Weight<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" ID="TxtWeight" CssClass="form-control" required="required" TextMode="Number" MaxLength="2" min="1" max="99" />
                                    <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtWeight"
                                        CssClass="text-danger" ErrorMessage="Weight is required." />--%>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlTypeA" CssClass="col-md-2 control-label">Type<span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlTypeA" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DdlType_SelectedIndexChanged" required="required">
                                        <asp:ListItem Text="Select" Value="" Selected="True" />
                                        <asp:ListItem Text="Monthly" Value="M" />
                                        <asp:ListItem Text="Weekly" Value="W" />
                                        <asp:ListItem Text="Half Yearly" Value="HY" />
                                    </asp:DropDownList>
                                    <%--  <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlTypeA"
                                        CssClass="text-danger" ErrorMessage="Please select a type." />--%>
                                </div>
                            </div>
                            <div class="row" runat="server" id="DvDuDt" visible="false">
                                <br />
                                <asp:Label runat="server" CssClass="col-md-2 control-label" AssociatedControlID="TxtDuDt">Due <%= Session["DuType"]%><span style="color:red">&nbsp*</span></asp:Label>
                                <div class="col-sm-2" id="DvTxtDuDt" runat="server" visible="false">
                                    <asp:TextBox runat="server" ID="TxtDuDt" TextMode="number" CssClass="form-control" MaxLength="2" min="1" max="31" required="required"></asp:TextBox>
                                </div>
                                <div class="col-sm-2" id="DvWkDay" runat="server" visible="false">
                                    <asp:DropDownList runat="server" ID="DdlWeekDay" CssClass="form-control" required="required">
                                        <asp:ListItem Text="Select" Value="" Selected="True" />
                                        <asp:ListItem Text="Sunday" Value="SUNDAY" />
                                        <asp:ListItem Text="Monday" Value="MONDAY" />
                                        <asp:ListItem Text="Tuesday" Value="TUESDAY" />
                                        <asp:ListItem Text="Wednesday" Value="WEDNESDAY" />
                                        <asp:ListItem Text="Thursday" Value="THURSDAY" />
                                        <asp:ListItem Text="Friday" Value="FRIDAY" />
                                        <asp:ListItem Text="Saturday" Value="SATURDAY" />
                                    </asp:DropDownList>
                                    <%--  <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlWeekDay"
                                        CssClass="text-danger" ErrorMessage="Due Date is required!" />--%>
                                </div>
                                <div class="col-sm-2" id="DvHY" runat="server" visible="false">
                                    <asp:DropDownList runat="server" ID="DdlHY" CssClass="form-control" required="required">
                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                        <asp:ListItem Text="1" Value="1" />
                                        <asp:ListItem Text="2" Value="2" />
                                    </asp:DropDownList>
                                </div>
                                <br />
                                <br />
                            </div>
                            <div class="row" style="margin-left: 2px;">
                                <asp:Button runat="server" ID="BtnCncl" OnClick="BtnCncl_Click" Text="Cancel" CssClass="btn btn-default" Visible="false" UseSubmitBehavior="False" />
                                <asp:Button runat="server" ID="BtnAdd" OnClick="BtnAdd_Click" Text="Add" CssClass="btn btn-primary" />
                            </div>
                            <asp:Label runat="server" ID="LblRprtId" Visible="false"></asp:Label>
                        </asp:View>
                        <asp:View ID="TabEdit" runat="server">
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="DdlCatTypeV" CssClass="col-md-2 control-label">Category Type</asp:Label>
                                <div class="col-sm-2 col-md-2">
                                    <asp:DropDownList runat="server" ID="DdlCatTypeV" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Text="All" Selected="True" Value="0" />
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlCatV" CssClass="col-md-2 control-label">Category</asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlCatV" CssClass="form-control" OnDataBinding="DdlCat_DataBinding">
                                        <asp:ListItem Text="All" Selected="True" Value="0" />
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlTypeV" CssClass="col-md-2 control-label">Type</asp:Label>
                                <div class="col-sm-2 col-md-2">
                                    <asp:DropDownList runat="server" ID="DdlTypeV" CssClass="form-control">
                                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                        <asp:ListItem Value="M">Monthly</asp:ListItem>
                                        <asp:ListItem Value="W">Weekly</asp:ListItem>
                                        <asp:ListItem Value="HY">Half Yearly</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row" style="margin-left: 2px;">
                                <asp:Button runat="server" ID="BtnView" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" />
                            </div>
                            <br />
                            <div style="width: auto; max-width: 1600px; height: auto; max-height: 350px; overflow: auto;">
                                <asp:GridView ID="GVReports"
                                    runat="server" Font-Size="Medium" ForeColor="#333333" GridLines="Both"
                                    CssClass="table table-bordered table-striped table-responsive table-hover" TabIndex="10"
                                    OnDataBinding="GVReports_DataBinding" BorderStyle="Solid" AutoGenerateColumns="False">
                                    <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" VerticalAlign="Bottom" />
                                    <HeaderStyle Font-Bold="True" ForeColor="white" Wrap="False" />
                                    <AlternatingRowStyle BackColor="#7ad0ed" />
                                    <Columns>
                                        <%--0 --%>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="CDeleteH" runat="server" BorderStyle="None" OnCheckedChanged="CDeleteH_CheckedChanged" TextAlign="Right" ToolTip="Delete" AutoPostBack="true" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CDelete" runat="server" ToolTip="Delete" AutoPostBack="true" OnCheckedChanged="CDelete_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--1 --%>
                                        <asp:BoundField DataField="Sno" HeaderText="Sno" ControlStyle-Width="8px" />
                                        <%--2 --%>
                                        <asp:BoundField DataField="Category_Type_Name" HeaderText="Category Type" />
                                        <%--2 --%>
                                        <asp:BoundField DataField="Category_Name" HeaderText="Category" />
                                        <%--2 --%>
                                        <asp:BoundField DataField="Report_Name" HeaderText="Report" />
                                        <%--3--%>
                                        <asp:BoundField DataField="Due_Date" HeaderText="Due Date" />
                                        <%--4--%>
                                        <asp:BoundField DataField="Priority" HeaderText="Priority" />
                                        <%--5--%>
                                        <asp:BoundField DataField="Weight" HeaderText="Weight" />
                                        <%--6--%>
                                        <asp:BoundField DataField="Type" HeaderText="Type" />
                                        <%--7--%>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="control-label" ForeColor="#3366FF" ID="BtnAction" runat="server" OnClick="BtnAction_Click" Text='<%# Bind("BtnTxt")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <div class="row" style="margin-left: 1px;">
                                <asp:Button runat="server" ID="BtnDlt" OnClick="BtnDlt_Click" Text="Delete" CssClass="btn btn-primary font-weight-bold" Enabled="false" Visible="false" />
                            </div>
                            <%--                            </div>--%>
                        </asp:View>
                    </asp:MultiView>
                </contenttemplate>
            </updatepanel>
        </div>
    </div>
    <script src="../assets/libs/Common.js" type="text/javascript"></script>
    <script>                    
        function ChngDueDtType (type, DvDuDt, TxtDuDt, DvWkDay)
        {
            //// Ensure all elements are properly defined
            //if (typeof DvDuDt === 'undefined' || typeof TxtDuDt === 'undefined' || typeof DvWkDay === 'undefined') {
            //    console.error('One or more elements are not defined.');
            //    return;
            //}
            //// Handle different cases based on the type
            //if (type === "") {
            //    console.log("Select");
            //    divDuDt.style.display = "block";
            //    divWkDay.style.display = "none";
            //    txtDuDt.style.display = "none";
            //} else if (type === "M") {
            //    console.log("Monthly");
            //    divDuDt.style.display = "none";
            //    divWkDay.style.display = "none";
            //    txtDuDt.style.display = "block";
            //} else if (type === "W") {
            //    console.log("Weekly");
            //    divDuDt.style.display = "none";
            //    divWkDay.style.display = "block";
            //    txtDuDt.style.display = "none";
            //} else {
            //    console.error("Invalid type");
            //}
        }

<%--        function ChngDueDtType (type) {
            console.log("ChngDueDtType called with " + type);

            // Select the DivWkDay element             
            console.log("<%= DvWkDay.ClientID %>");
            const divWkDay = document.getElementById("#<%= DvWkDay.ClientID %>");
            const dvWkDay = document.getElementById("DvWkDay");
            if (divWkDay === null) {
                console.log("DvWkDay is null");
            }
            if (dvWkDay === null) {
                console.log("divWkDay is null");
            }

            // Select the TxtDuDt element               
            console.log("<%= TxtDuDt.ClientID %>");
            var txtDuDt = document.getElementById("#<%= TxtDuDt.ClientID %>");
            if (txtDuDt === null) {
                console.log("txtDuDt is null");
            }

            // Check the type and perform actions accordingly
            if (type === "M") {
                console.log("M");
                divWkDay.style.display = "none";
                txtDuDt.style.display = "block";
            } else if (type === "W") {
                console.log("W");
                divWkDay.style.display = "block";
                txtDuDt.style.display = "none";
            }
        }--%>
        function validateDropDown ()
        {
            var dropdown = document.getElementById("<%= DdlCatTypeA.ClientID %>");
            if (dropdown.selectedIndex === 0)
            { // Assuming the first item is "Select One"
                alert("Category Type is required");
                return false; // Prevent form submission
            }
            return true; // Allow form submission
        }

    </script>
</asp:Content>
