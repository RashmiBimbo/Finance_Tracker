<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Finance_Tracker.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <br />
        <h2>Welcome to Finance Tracker</h2>
<%--        <div class="modal fade modal-dialog modal-md" id="MdlChngPswd" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="MdlChngPswdLabel">Change Password (Mandatory)</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="TxtOldPswd" class="control-label">Old Passowrd<span style="color: red">&nbsp*</span></label>
                            <asp:TextBox class="form-control" ID="TxtOldPswd" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtOldPswd" ID="ROPswd"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="Old password is required." />
                        </div>
                        <div class="form-group">
                            <label for="TxtNewPswd" class="control-label">New Password<span style="color: red">&nbsp*</span></label>
                            <asp:TextBox class="form-control" ID="TxtNewPswd" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtNewPswd" ID="RPswd"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="New password is required." />
                        </div>
                        <div class="form-group">
                            <label for="TxtConfirmPswd" class="control-label">Confirm Password<span style="color: red">&nbsp*</span></label>
                            <asp:TextBox class="form-control" ID="TxtConfirmPswd" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtConfirmPswd" ID="RCPswd"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                            <asp:CompareValidator runat="server" ControlToCompare="TxtNewPswd" ControlToValidate="TxtConfirmPswd" ID="CVPswd"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="New password and confirm password do not match." />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" runat="server" id="BtnOk" onserverclick="BtnOk_ServerClick">Save</button>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>
    <script src="assets/libs/Common.js" type="text/javascript"></script>
    <script type="text/javascript">

        function ChngPswd ()
        {
            //jQuery.noConflict();
            console.log("ChngPswd called");
            $("#MdlChngPswd").modal("show");
            console.log("MdlChngPswd shown");
        }
    </script>
    <!-- jQuery -->
    <%--<script src="//code.jquery.com/jquery-3.7.0.min.js"></script>--%>
</asp:Content>
