<%@ Page Title="Change Password" Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Finance_Tracker.Account.ResetPassword" Async="true" %>

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
        <h2 style="margin-left: 25px">Change Password (Mandatory)</h2>
        <div class="row">
            <div class="col-md-10">
                <section id="ChangePswdForm">
                    <div class="form-horizontal" style="margin-left: 5px">
                        <hr />
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <%--<asp:Literal runat="server" ID="FailureText" />--%>
                            </p>
                        </asp:PlaceHolder>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtOldPswd" CssClass="col-md-2 control-label">Old Password<span style="color:red">&nbsp*</span></asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="TxtOldPswd" TextMode="Password" CssClass="form-control" Width="40%" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtOldPswd" ID="RequiredFieldValidator2"
                                            CssClass="text-danger" Display="Dynamic" ErrorMessage="Old password is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtNewPswd" CssClass="col-md-2 control-label">New Password<span style="color:red">&nbsp*</span></asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="TxtNewPswd" TextMode="Password" CssClass="form-control" Width="40%" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtNewPswd" ID="RequiredFieldValidator3"
                                            CssClass="text-danger" Display="Dynamic" ErrorMessage="New password is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtConfirmPswd" CssClass="col-md-2 control-label">Confirm Password<span style="color:red">&nbsp*</span></asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="TxtConfirmPswd" TextMode="Password" CssClass="form-control" Width="40%" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtConfirmPswd" ID="RequiredFieldValidator1" CssClass="text-danger" Display="Dynamic" ErrorMessage="Confirm password is required."></asp:RequiredFieldValidator>
                                        <asp:CompareValidator runat="server" ControlToCompare="TxtNewPswd" ControlToValidate="TxtConfirmPswd" ID="CompareValidator1" CssClass="text-danger" Display="Dynamic" ErrorMessage="New password and confirm password do not match." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-offset-2 col-md-10">
                                        <button type="button" class="btn btn-primary" runat="server" id="BtnOk" onserverclick="BtnOk_ServerClick">Save</button>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </section>
            </div>
        </div>
    </form>

</body>
</html>
