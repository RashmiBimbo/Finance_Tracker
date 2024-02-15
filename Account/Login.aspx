<%@ Page Title="Log in" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Finance_Tracker.Account.Login" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - Finance Tracker</title>
    <link rel="stylesheet" href="../Content/bootstrap-theme.css" />
</head>
<body>
    <form runat="server" class="form-horizontal">
        <asp:ScriptManager runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <h2 style="margin-left: 25px"><%:  Page.Title %></h2>
        <div class="row">
            <div class="col-md-8">
                <section id="loginForm">
                    <div class="form-horizontal" style="margin-left: 5px">
                        <hr />
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="DdlLocn" CssClass="col-md-2 control-label">Location<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList runat="server" ID="DdlLocn" CssClass="form-control" OnDataBinding="DdlLocn_DataBinding" Width="30%" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlLocn"
                                    CssClass="text-danger" ErrorMessage="Location is required." Enabled="false" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="TxtUserId" CssClass="col-md-2 control-label">User Id<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="TxtUserId" CssClass="form-control" Width="30%" AutoCompleteType="None"/>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtUserId" CssClass="text-danger" ErrorMessage="User Id is required." />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="TxtPassword" CssClass="col-md-2 control-label">Password<span style="color:red">&nbsp*</span></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="TxtPassword" TextMode="Password" CssClass="form-control" Width="30%" AutoCompleteType="None"/>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPassword" CssClass="text-danger" ErrorMessage="Password is required." />
                            </div>
                        </div>
                        <div class="form-group" runat="server" hidden="hidden" visible="false">
                            <div class="col-md-offset-2 col-md-10">
                                <div class="checkbox">
                                    <asp:CheckBox runat="server" ID="CBRemMe" Visible="False" />
                                    <asp:Label runat="server" AssociatedControlID="CBRemMe" Visible="False">Remember me?</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10">
                                <asp:Button runat="server" OnClick="BtnLogIn_Click" Text="Log in" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                    <p>
                        <%-- Enable this once you have account confirmation enabled for password reset functionality
                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
                        --%>
                    </p>
                </section>
            </div>
            <div class="col-md-4">
                <%-- <section id="socialLoginForm">
                    <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
                </section>--%>
            </div>
        </div>
    </form>

</body>
</html>
