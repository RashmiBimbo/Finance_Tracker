<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs"
    Inherits="Finance_Tracker.Masters.Users" EnableEventValidation="false" MaintainScrollPositionOnPostback="True"
    Async="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <br />
        <br />
        <br />
        <h3>Users</h3>
        <hr />
        <div class="form-group">
            <updatepanel>
                <contenttemplate>
                    <div>
                        <asp:Menu ID="Menu" runat="server" BackColor="#006666" DynamicHorizontalOffset="2"
                            Font-Names="Verdana" Font-Size="0.84em" ForeColor="White" Orientation="Horizontal"
                            StaticSubMenuIndent="10px" OnMenuItemClick="Menu_MenuItemClick" CssClass="form-control">
                            <DynamicHoverStyle BackColor="#3399FF" ForeColor="White" />
                            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <DynamicMenuStyle BackColor="#F7F6F3" />
                            <DynamicSelectedStyle BackColor="#5D7B9D" />
                            <Items>
                                <asp:MenuItem Text="Add Users |" Value="0" />
                                <asp:MenuItem Text="Delete Users" Value="1" />
                            </Items>
                            <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <StaticSelectedStyle BackColor="#5D7B9D" />
                        </asp:Menu>
                    </div>
                    <br />
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="TabAdd" runat="server">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtUsrId"
                                    CssClass="col-md-2 control-label">User Id<span style="color: red">&nbsp*</span>
                                </asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="TxtUsrId" CssClass="form-control" Width="40%" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtUsrName" CssClass="col-md-2 control-label">User Name<span
                                            style="color: red">&nbsp*</span>
                                </asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="TxtUsrName" CssClass="form-control" autocomplete="new-password" Width="40%" />
                                </div>
                            </div>
                            <div class="form-group" id="DivPswd" runat="server">
                                <asp:Label runat="server" AssociatedControlID="TxtPassword" CssClass="col-md-2 control-label">Password<span style="color: red">&nbsp*</span>
                                </asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="TxtPassword" TextMode="Password"
                                        CssClass="form-control" AutoCompleteType="Disabled" Width="40%" aria-autocomplete="none" />
                                </div>
                            </div>
                            <div class="form-group" id="DivCPswd" runat="server">
                                <asp:Label runat="server" AssociatedControlID="TxtConfirmPassword" CssClass="col-md-2 control-label">Confirm password<span
                                            style="color: red">&nbsp*</span></asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="TxtConfirmPassword" TextMode="Password"
                                        CssClass="form-control" Width="40%" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtEmail"
                                    CssClass="col-md-2 control-label">Email<span style="color: red">&nbsp*</span>
                                </asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="TxtEmail" TextMode="Email"
                                        CssClass="form-control" Width="40%" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlRoleA"
                                    CssClass="col-md-2 control-label">Role<span style="color: red">&nbsp*</span>
                                </asp:Label>
                                <div class="col-md-10">
                                    <asp:DropDownList runat="server" ID="DdlRoleA" CssClass="form-control" OnDataBinding="DdlRole_DataBinding" Width="40%" onchange="UpdateToolTip(this);" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlLocnA" CssClass="col-md-2 control-label">Location<span style="color: red">&nbsp*</span>
                                </asp:Label>
                                <div class="col-md-10">
                                    <asp:DropDownList runat="server" ID="DdlLocnA" CssClass="form-control" OnDataBinding="DdlLocn_DataBinding" Width="40%" onchange="UpdateToolTip(this);" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAddress" CssClass="col-md-2 control-label">Address
                                </asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="TxtAddress" CssClass="form-control"
                                        TextMode="MultiLine" Width="40%" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class=" row col-md-offset-2 col-md-10" style="margin-left: 2px">
                                    <asp:Button runat="server" ID="BtnRegister" OnClick="BtnRegister_Click" Text="Add"
                                        CssClass="btn btn-primary" />
                                    <asp:Button runat="server" ID="BtnCancel" OnClick="BtnCancel_Click"
                                        Visible="false" Text="Cancel" CssClass="btn btn-secondary" />
                                </div>
                                <asp:HiddenField ID="HFRecIdA" runat="server" />
                        </asp:View>
                        <asp:View ID="TabEdit" runat="server">
                            <div class="row">
                                <asp:Label runat="server" AssociatedControlID="DdlRoleV"
                                    CssClass="col-md-2 control-label">User Type
                                </asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlRoleV" CssClass="form-control"
                                        AutoPostBack="True" OnDataBinding="DdlRole_DataBinding"
                                        OnSelectedIndexChanged="DdlRole_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="All" />
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlLocnV"
                                    CssClass="col-md-2 control-label">User Type
                                </asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlLocnV" CssClass="form-control"
                                        AutoPostBack="True" OnDataBinding="DdlLocn_DataBinding"
                                        OnSelectedIndexChanged="DdlLocn_SelectedIndexChanged">
                                        <asp:ListItem Value="" Text="All" />
                                    </asp:DropDownList>
                                </div>
                                <asp:Label runat="server" AssociatedControlID="DdlUser"
                                    CssClass="col-md-2 control-label">User
                                </asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="DdlUser" CssClass="form-control"
                                        OnDataBinding="DdlUser_DataBinding">
                                        <asp:ListItem Value="" Text="All" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <br />
                            <div class="row" style="margin-left: 2px">
                                <asp:Button runat="server" ID="BtnView" OnClick="BtnView_Click" Text="View"
                                    CssClass="btn btn-primary" />
                            </div>
                            <br />
                            <div style="width: auto; max-width: 1600px; height: auto; max-height: 350px; overflow: auto;">
                                <asp:GridView ID="GVUsers" runat="server" Font-Size="Medium" ForeColor="#333333"
                                    GridLines="Both"
                                    CssClass="table table-bordered table-striped table-responsive table-hover"
                                    TabIndex="10" OnDataBinding="GVUsers_DataBinding" BorderStyle="Solid"
                                    AutoGenerateColumns="False">
                                    <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false"
                                        VerticalAlign="Bottom" />
                                    <HeaderStyle Font-Bold="True" Wrap="False" />
                                    <%--<AlternatingRowStyle BackColor="#7ad0ed" />--%>
                                    <Columns>
                                        <%--0 --%>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="CDeleteH" runat="server" BorderStyle="None"
                                                    TextAlign="Right" ToolTip="Delete"
                                                    onclick="handleCheckBoxChangeH(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CDelete" runat="server" ToolTip="Delete"
                                                    onclick="handleCheckBoxChange(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--1--%>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="control-label" ID="BtnAction"
                                                    runat="server" OnClick="BtnAction_Click">Edit
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--2 --%>
                                        <asp:BoundField DataField="Sno" HeaderText="Sno"
                                            ControlStyle-Width="8px" />
                                        <%--3 --%>
                                        <asp:BoundField DataField="User_Name" HeaderText="Name" />
                                        <%--4--%>
                                        <asp:BoundField DataField="Role_Name"
                                            HeaderText="Role" />
                                        <%--5--%>
                                        <asp:BoundField DataField="Loc_Name"
                                            HeaderText="Location" />
                                        <%--6--%>
                                        <asp:BoundField DataField="User_Id"
                                            Visible="false" />
                                        <%--7--%>
                                        <asp:BoundField DataField="Location_Id"
                                            Visible="false" />
                                        <%--8--%>
                                        <asp:BoundField DataField="Role_Id"
                                            Visible="false" />
                                        <%--9--%>
                                        <asp:BoundField DataField="Rec_Id"
                                            Visible="false" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <div class="row" style="margin-left: 1px">
                                <asp:Button runat="server" ID="BtnDlt" OnClick="BtnDlt_Click" Text="Delete"
                                    CssClass="btn btn-primary font-weight-bold" Enabled="false" Visible="false" />
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </contenttemplate>
            </updatepanel>
        </div>
    </div>
    <script src="../assets/libs/Common.js" type="text/javascript"></script>
    <script>
        let chKCount = 0;

        $(document).ready(function () {
            //debugger;
            var GVUsers = document.getElementById("<%= GVUsers.ClientID %>");
            if (GVUsers !== null) {
                for (let i = 1; i < GVUsers.rows.length; i++) {
                    let cbChld = GVUsers.rows[i].cells[0].querySelector(
                        'input[type="checkbox"]'
                    );
                    let chkd = cbChld.checked;
                    chKCount += chkd ? 1 : 0;
                }
                EnableDisableButton(chKCount);
                //debugger;
                var cbH = GVUsers.rows[0].cells[0].querySelector(
                    'input[type="checkbox"]'
                );
                if (cbH !== null) cbH.checked = chKCount == GVUsers.rows.length - 1;
            }
            console.log("chKCount: " + chKCount);
        });

        //Handle checkbox change for checkbox in Header row of GVUsers
        function handleCheckBoxChangeH (cb) {
            //debugger;
            var GVUsers = document.getElementById("<%= GVUsers.ClientID %>");
            if (GVUsers !== null) {
                //Update each row's checkbox
                for (let i = 1; i < GVUsers.rows.length; i++) {
                    let cbChld = GVUsers.rows[i].cells[0].querySelector(
                        'input[type="checkbox"]'
                    );
                    let chkdH = cb.checked;
                    if (chkdH !== cbChld.checked) {
                        cbChld.checked = chkdH;
                        chKCount += chkdH ? 1 : -1;
                    }
                }
                if (GVUsers.rows.length < chKCount) chKCount = GVUsers.rows.length;
                else if (chKCount < 0) chKCount = 0;
                EnableDisableButton(chKCount);
            }
        }

        //Handle checkbox change for checkbox in each row of GVUsers
        function handleCheckBoxChange (cb) {
            //debugger;
            chKCount += cb.checked ? 1 : -1;
            var GVUsers = document.getElementById("<%= GVUsers.ClientID%>");
            if (GVUsers !== null) {
                if (GVUsers.rows.length < chKCount) chKCount = GVUsers.rows.length;
                else if (chKCount < 0) chKCount = 0;

                // Update header checkbox
                let cbH = GVUsers.rows[0].querySelector('th input[type="checkbox"]');
                cbH.checked = GVUsers.rows.length - 1 === chKCount;
                EnableDisableButton(chKCount);
            }
        }

        function EnableDisableButton (count) {
            let btnAddM = document.getElementById("<%= BtnDlt.ClientID %>");
            btnAddM.disabled = count === 0;
        }

        function BtnDltOnClientClick () {
            console.log("BtnDltOnClientClick called");
            var ans = confirm("Are you sure you want to Delete the selected users?");
            console.log(ans.valueOf());

            //debugger;
            console.log("chKCount: " + chKCount);
            return ans;
        }
    </script>
</asp:Content>
