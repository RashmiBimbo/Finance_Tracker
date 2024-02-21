<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Finance_Tracker.Account.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <br />
        <br />
        <br />
        <h3>Change Password</h3>
        <div class="col-10">
            <section id="ChangePswdForm">
                <div class="form-horizontal">
                    <hr />
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
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtConfirmPswd" ID="RequiredFieldValidator1" CssClass="text-danger" Display="Dynamic" ErrorMessage="Confirm password is required." />
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
</asp:Content>
