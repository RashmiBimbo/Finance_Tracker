<%@ Page Title="Log in" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Finance_Tracker.Account.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %></title>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <h2><%:  Page.Title %>.</h2>

        <div class="container body-content">
            <hr />
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-8">
                            <section id="loginForm">
                                <div class="form-horizontal">
                                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                        <p class="text-danger">
                                            <asp:Literal runat="server" ID="FailureText" />
                                        </p>
                                    </asp:PlaceHolder>
                                    <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="DdlLocn" CssClass="col-md-2 control-label">Location<span style="color:red">&nbsp*</span></asp:Label>
                                        <div class="col-md-10">
                                            <asp:DropDownList runat="server" ID="DdlLocn" CssClass="form-control" OnDataBinding="DdlLocn_DataBinding" />
                                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlLocn"
                                CssClass="text-danger" ErrorMessage="Location is required." />--%>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="TxtUserId" CssClass="col-md-2 control-label">UserId<span style="color:red">&nbsp*</span></asp:Label>
                                        <div class="col-md-10">
                                            <asp:TextBox runat="server" ID="TxtUserId" CssClass="form-control" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtUserId"
                                                CssClass="text-danger" ErrorMessage="User Id is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" AssociatedControlID="TxtPassword" CssClass="col-md-2 control-label">Password<span style="color:red">&nbsp*</span></asp:Label>
                                        <div class="col-md-10">
                                            <asp:TextBox runat="server" ID="TxtPassword" TextMode="Password" CssClass="form-control" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPassword" CssClass="text-danger" ErrorMessage="Password is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10">
                                            <div class="checkbox">
                                                <asp:CheckBox runat="server" ID="CBRemMe" />
                                                <asp:Label runat="server" AssociatedControlID="CBRemMe">Remember me?</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10">
                                            <asp:Button runat="server" OnClick="BtnLogIn_Click" Text="Log in" CssClass="btn btn-default" ID="BtnLogIn" />
                                        </div>
                                    </div>
                            </section>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <p>
                <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled" NavigateUrl="~/Account/Register.aspx">Register as a new user</asp:HyperLink>
            </p>
            <p>
                <%-- Enable this once you have account confirmation enabled for password reset functionality
                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
                --%>
            </p>
            <footer>
                <p>&copy; <%: DateTime.Now.Year %> - Finance Tracker</p>
            </footer>
        </div>
    </form>
</body>
</html>
