<%@ Page Title="Task Assignment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserTaskAssignment.aspx.cs" Inherits="Finance_Tracker.Masters.UserTaskAssignment" EnableEventValidation="true" MaintainScrollPositionOnPostback="True" Async="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <br />
        <br />
        <br />
        <h3>Task Assignment</h3>
        <hr />
        <div class="form-group">
            <div>
                <asp:Menu ID="Menu" runat="server" BackColor="#006666" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.84em" ForeColor="White" Orientation="Horizontal" StaticSubMenuIndent="10px" OnMenuItemClick="Menu_MenuItemClick" CssClass="form-control">
                    <DynamicHoverStyle BackColor="#3399FF" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        <asp:MenuItem Selected="True" Text="Assign Tasks |" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="Unassign Tasks" Value="1"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
<br />
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="DdlCatType" CssClass="col-md-2 control-label">Category Type</asp:Label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlCatType" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True" onchange="updateTooltip(this);">
                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Label runat="server" AssociatedControlID="DdlCat" CssClass="col-md-2 control-label">Category</asp:Label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlCat" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True" onchange="updateTooltip(this);">
                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Label runat="server" AssociatedControlID="DdlTasks" CssClass="col-md-2 control-label">Report</asp:Label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlTasks" CssClass="form-control" OnDataBinding="DdlTasks_DataBinding"
                        onchange="updateTooltip(this);">
                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <div class="row">
                <asp:Label runat="server" AssociatedControlID="DdlUsers" CssClass="col-md-2 control-label">User</asp:Label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlUsers" CssClass="form-control" OnDataBinding="DdlUsers_DataBinding" onchange="updateTooltip(this);">
                        <asp:ListItem Value="0" Selected="True" onchange="updateTooltip(this);">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2">
                    <asp:Button runat="server" ID="BtnView" OnClick="BtnView_Click" Text="View" CssClass="btn btn-primary" ForeColor="White" />
                </div>
            </div>
            <br />
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View runat="server" ID="TabAdd">
                    <div id="DivAdd" runat="server" visible="false">
                        <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto;" runat="server">
                            <asp:GridView ID="GVAdd"
                                runat="server" Font-Bold="False" CssClass="table table-bordered table-condensed table-responsive table-hover"
                                Font-Size="Medium" ForeColor="#333333" GridLines="Both" RowStyle-HorizontalAlign="LEFT" TabIndex="10" BorderStyle="Solid" AutoGenerateColumns="False" AllowSorting="True" OnDataBinding="GVAdd_DataBinding">
                                <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="False" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="#7ad0ed" />
                                <Columns>
                                    <%--0 --%>
                                    <asp:TemplateField Visible="true">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="CBAddH" runat="server" AutoPostBack="true" OnCheckedChanged="CBAddH_CheckedChanged" TextAlign="Right" ToolTip="Add" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBAdd" runat="server" ToolTip="Add" AutoPostBack="true" OnCheckedChanged="CBAdd_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--1 --%>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="8px" />
                                    <%--2 --%>
                                    <asp:TemplateField HeaderText="User">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="LblUser" Text='<%# Bind("User_Name")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--3--%>
                                    <asp:TemplateField HeaderText="Task">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="LblTask" Text='<%# Bind("Task_Name")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <div class="row" style="margin-left: 2px;">
                            <asp:Button runat="server" ID="BtnAssign" OnClick="BtnAssign_Click" Text="Assign" CssClass="btn btn-primary" ForeColor="White" Enabled="false" />
                        </div>
                    </div>
                </asp:View>
                <asp:View runat="server" ID="TabViewEdit">
                    <div id="DivView" runat="server" visible="false">
                        <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto;" runat="server">
                            <asp:GridView ID="GVView"
                                runat="server" Font-Bold="False" CssClass="table table-bordered table-condensed table-responsive table-hover"
                                Font-Size="Medium" ForeColor="#333333" GridLines="Both" RowStyle-HorizontalAlign="LEFT" TabIndex="10" BorderStyle="Solid" AutoGenerateColumns="False" AllowSorting="True" OnDataBinding="GVView_DataBinding">
                                <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="false" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="075098" Font-Bold="True" ForeColor="white" Wrap="False" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="#7ad0ed" />
                                <Columns>
                                    <%--0 --%>
                                    <asp:TemplateField Visible="true">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="CBEditH" runat="server" AutoPostBack="true" OnCheckedChanged="CBEditH_CheckedChanged" TextAlign="Right" ToolTip="Edit" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBEdit" runat="server" ToolTip="Edit" AutoPostBack="true" OnCheckedChanged="CBEdit_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--1 --%>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" Visible="true" ControlStyle-Width="8px" />
                                    <%--2 --%>
                                    <asp:TemplateField HeaderText="User">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="LblUser" Text='<%# Bind("User_Name")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--3--%>
                                    <asp:TemplateField HeaderText="Task">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="LblTask" Text='<%# Bind("Task_Name")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <div class="row" style="margin-left: 2px;">
                            <asp:Button runat="server" ID="BtnDlt" OnClick="BtnDlt_Click" Text="Unassign" CssClass="btn btn-primary" ForeColor="White" Enabled="false" />
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <script>
        function ToggleClass (ctrl1, ctrl2)
        {
            console.log("ToggleClass");
            if (ctrl1.className === "")
            {
                ctrl1.className = "active";
            }
            if (ctrl2.className === "active")
            {
                ctrl2.className = "";
            }
        }
        function updateTooltip (ddl)
        {
            if (ddl.selectedIndex !== -1)
            {
                ddl.title = ddl.options[ddl.selectedIndex].text;
            }
        }
    </script>
</asp:Content>
