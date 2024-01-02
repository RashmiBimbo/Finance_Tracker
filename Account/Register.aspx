<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Finance_Tracker.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Create a new account</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="TxtUsrId" CssClass="col-md-2 control-label">UserId<span style="color:red">&nbsp*</span></asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="TxtUsrId" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtUsrId"
                    CssClass="text-danger" ErrorMessage="The User Id is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="TxtPassword" CssClass="col-md-2 control-label">Password<span style="color:red">&nbsp*</span></asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="TxtPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="The password field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="TxtConfirmPassword" CssClass="col-md-2 control-label">Confirm password<span style="color:red">&nbsp*</span></asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="TxtConfirmPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DdlCompany" CssClass="col-md-2 control-label">Company</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="DdlCompanyId" CssClass="form-control" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DdlSubCompany" CssClass="col-md-2 control-label">Sub Company</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="DdlSubCompany" CssClass="form-control" /></div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DdlRole" CssClass="col-md-2 control-label">Role</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="DdlRole" CssClass="form-control" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DdlLocation" CssClass="col-md-2 control-label">Location</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="DdlLocation" CssClass="form-control" />
            </div>
        </div>   
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="TxtAddress" CssClass="col-md-2 control-label">Address</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="TxtAddress" CssClass="form-control" TextMode="MultiLine"/>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
