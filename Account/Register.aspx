<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Finance_Tracker.Account.Register" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div class="form-horizontal">
        <br />
        <h4>Create a new account</h4>
        <hr />
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <div class="form-group" style="">
                    <asp:Label runat="server" AssociatedControlID="TxtUsrId" CssClass="col-md-2 control-label">User Id<span style="color:red">&nbsp*</span></asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TxtUsrId" CssClass="form-control" Width="40%" />
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtUsrId"
                            CssClass="text-danger" ErrorMessage="The User Id is required." />--%>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TxtUsrName" CssClass="col-md-2 control-label">User Name<span style="color:red">&nbsp*</span></asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TxtUsrName" CssClass="form-control" autocomplete="new-password" Width="40%" />
                        <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtUsrName"
                            CssClass="text-danger" ErrorMessage="The User Name is required." />--%>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TxtPassword" CssClass="col-md-2 control-label">Password<span style="color:red">&nbsp*</span></asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TxtPassword" TextMode="Password" CssClass="form-control" AutoCompleteType="Disabled" Width="40%" />
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPassword"
                            CssClass="text-danger" ErrorMessage="The password field is required." />--%>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TxtConfirmPassword" CssClass="col-md-2 control-label">Confirm password<span style="color:red">&nbsp*</span></asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TxtConfirmPassword" TextMode="Password" CssClass="form-control" Width="40%" />
                        <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtConfirmPassword" ID="RVPswd"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                        <asp:CompareValidator runat="server" ControlToCompare="TxtPassword" ControlToValidate="TxtConfirmPassword" ID="CVPswd"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />--%>
                    </div>
                </div>
                <%-- <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DdlCompany" CssClass="col-md-2 control-label">Company</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="DdlCompanyId" CssClass="form-control" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DdlSubCompany" CssClass="col-md-2 control-label">Sub Company</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="DdlSubCompany" CssClass="form-control" /></div>
        </div>--%>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TBEmail" CssClass="col-md-2 control-label">Email<span style="color:red">&nbsp*</span></asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TBEmail" TextMode="Email" CssClass="form-control" Width="40%" />
                        <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlRole"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Role Name is required." />--%>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="DdlRole" CssClass="col-md-2 control-label">Role<span style="color:red">&nbsp*</span></asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="DdlRole" CssClass="form-control" OnDataBinding="DdlRole_DataBinding" Width="40%" />
                        <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlRole"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Role Name is required." />--%>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="DdlLocn" CssClass="col-md-2 control-label">Location<span style="color:red">&nbsp*</span></asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="DdlLocn" CssClass="form-control" OnDataBinding="DdlLocn_DataBinding" Width="40%" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="TxtAddress" CssClass="col-md-2 control-label">Address</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="TxtAddress" CssClass="form-control" TextMode="MultiLine" Width="40%" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server" ID="BtnRegister" OnClick="BtnRegister_Click" Text="Register" CssClass="btn btn-primary" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
