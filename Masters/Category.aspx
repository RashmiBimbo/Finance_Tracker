<%--<%@ Page Title="" Language="C#"  MasterPageFile="~/`"  AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="Finance_Tracker.ADMIN.Category" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="Finance_Tracker.ADMIN.Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <hr />
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <div class="form-group">
                    <asp:Menu ID="Menu1" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal" StaticSubMenuIndent="10px" OnMenuItemClick="Menu1_MenuItemClick">
                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicMenuStyle BackColor="#F7F6F3" />
                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                        <Items>
                            <asp:MenuItem Selected="True" Text="Add Category |" ToolTip="Add new category for reports" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Edit Category |" Value="1" ToolTip="Edit category for reports"></asp:MenuItem>
                            <asp:MenuItem Text="Get Categories" ToolTip="Get all active categories" Value="2"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <StaticSelectedStyle BackColor="#5D7B9D" />
                    </asp:Menu>
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="tabAdd" runat="server">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="form-group row">
                                            <asp:Label runat="server" AssociatedControlID="DdlType" CssClass="col-md-2 control-label">Category Type<span style="color:red">&nbsp*</span></asp:Label>
                                            <div class="col-sm-3">
                                                <asp:DropDownList runat="server" ID="DdlType" CssClass="form-control" OnDataBinding="DdlType_DataBinding" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlType"
                                                    CssClass="text-danger" ErrorMessage="Category Type is required." />
                                            </div>
                                            <asp:Label runat="server" AssociatedControlID="TxtCatName" CssClass="col-md-2 control-label">Category Name</asp:Label>
                                            <div class="col-sm-3">
                                                <asp:TextBox runat="server" ID="TxtCatName" CssClass="form-control" MaxLength="150" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtCatName"
                                                    CssClass="text-danger" ErrorMessage="Category Name is required." />
                                            </div>
                                            <asp:Label runat="server" AssociatedControlID="TxtCatName" CssClass="col-md-2 control-label">Priority</asp:Label>
                                            <div class="col-sm-3">
                                                <asp:TextBox runat="server" ID="TxtPriority" CssClass="form-control" MaxLength="2" TextMode="Number" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtCatName"
                                                    CssClass="text-danger" ErrorMessage="Priority is required." />
                                            </div>
                                            <asp:Label runat="server" AssociatedControlID="Txt" CssClass="col-md-2 control-label">Weight</asp:Label>
                                            <div class="col-sm-3">
                                                <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control" MaxLength="150" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtCatName"
                                                    CssClass="text-danger" ErrorMessage="Weight is required." />
                                            </div>
                                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                            <div class="form-group">
                                                <div class="col-md-offset-2 col-md-10">
                                                    <asp:Button runat="server" ID="BtnAdd" OnClick="BtnAdd_Click" Text="Add" CssClass="btn btn-default" BackColor="#003399" ForeColor="White" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 100%; height: 350px; overflow: auto; padding: 10px" runat="server" id="Grid1div">
                                <asp:GridView ID="GVCategory" Style="max-height: 30px; overflow: auto"
                                    runat="server" CellPadding="10" CellSpacing="5" Font-Bold="False"
                                    CssClass="grid-view" Width="760px" Font-Size="Small"
                                    ForeColor="#333333" GridLines="Vertical"
                                    RowStyle-HorizontalAlign="LEFT" RowStyle-Wrap="false"
                                    HeaderStyle-Wrap="false" AutoGenerateColumns="true" TabIndex="10"
                                    OnDataBinding="GVCategory_DataBinding">
                                    <RowStyle BackColor="white" HorizontalAlign="LEFT" Wrap="False" />
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings NextPageText="&gt;" PreviousPageText="&lt;" />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="075098" Font-Bold="True"
                                        ForeColor="white" Wrap="False" />
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <AlternatingRowStyle BackColor="#7ad0ed" />
                                </asp:GridView>
                            </div>
                        </asp:View>
                    </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script src="../assets/libs/Common.js" type="text/javascript"></script>
</asp:Content>
