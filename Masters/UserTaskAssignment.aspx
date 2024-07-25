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
                <div id="DvApprs" runat="server">
                    <div runat="server" id="DvApprA" visible="false">
                        <asp:Label runat="server" AssociatedControlID="DdlApproversA" CssClass="col-md-2 control-label">Approver&nbsp&nbsp<span style="color:red">*</span></asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlApproversA" CssClass="form-control" OnDataBinding="DdlApprovers_DataBinding" required>
                                <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="DvApprU" runat="server" visible="false">
                        <asp:Label runat="server" AssociatedControlID="DdlApproversU" CssClass="col-md-2 control-label">Approver</asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="DdlApproversU" CssClass="form-control" OnDataBinding="DdlApprovers_DataBinding">
                                <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <asp:Label runat="server" AssociatedControlID="DdlUsrType" CssClass="col-md-2 control-label">User Type</asp:Label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlUsrType" CssClass="form-control" AutoPostBack="True" OnDataBinding="DdlUsrType_DataBinding" OnSelectedIndexChanged="DdlUsrType_SelectedIndexChanged">
                        <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Label runat="server" AssociatedControlID="DdlUsers" CssClass="col-md-2 control-label">User</asp:Label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlUsers" CssClass="form-control" OnDataBinding="DdlUsers_DataBinding">
                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>
            <br />
            <div class="row">
                <div id="DvCatTyp" runat="server" visible="false">
                    <asp:Label runat="server" AssociatedControlID="DdlCatType" CssClass="col-md-2 control-label">Category Type</asp:Label>
                    <div class="col-sm-2">
                        <asp:DropDownList runat="server" ID="DdlCatType" CssClass="form-control" OnDataBinding="DdlCatType_DataBinding" OnSelectedIndexChanged="DdlCatType_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:Label runat="server" AssociatedControlID="DdlCat" CssClass="col-md-2 control-label">Category</asp:Label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlCat" CssClass="form-control" OnDataBinding="DdlCat_DataBinding" OnSelectedIndexChanged="DdlCat_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Label runat="server" AssociatedControlID="DdlTasks" CssClass="col-md-2 control-label">Report</asp:Label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="DdlTasks" CssClass="form-control" OnDataBinding="DdlTasks_DataBinding">
                        <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
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
                        <div style="width: 100%; max-width: 1500px; height: auto; max-height: 350px; overflow: auto;" runat="server" id="DvAssign">
                            <asp:GridView ID="GVAssign"
                                runat="server" Font-Bold="False" CssClass="table table-bordered table-responsive table-hover"
                                Font-Size="Medium" ForeColor="#333333" GridLines="Both" RowStyle-HorizontalAlign="LEFT" TabIndex="10" BorderStyle="Solid" AutoGenerateColumns="False" AllowSorting="True" OnDataBinding="GVAssign_DataBinding">
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
                                            <asp:CheckBox ID="CBAddH" runat="server" onclick="CBAddHOnClick(this)" TextAlign="Right" ToolTip="Assign" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBAdd" runat="server" ToolTip="Assign" onclick="CBAddOnClick(this)" />
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
                                    <%--<asp:BoundField DataField="Approver" HeaderText="Approver" Visible="true" />--%>
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
                                runat="server" Font-Bold="False" CssClass="table table-bordered table-responsive table-hover"
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
                                            <asp:CheckBox ID="CBEditH" runat="server" TextAlign="Right" ToolTip="Edit" onclick="CBEditHOnClick(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CBEdit" runat="server" ToolTip="Edit" onclick="CBEditOnClick(this)" />
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
                                    <asp:TemplateField HeaderText="Approver">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="LblApprover" Text='<%# Bind("Approver")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <div class="row" style="margin-left: 2px;">
                            <asp:Button runat="server" ID="BtnUnAssign" OnClick="BtnUnAssign_Click" Text="Unassign" CssClass="btn btn-primary" ForeColor="White" Enabled="false" />
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <script src="../assets/libs/Common.js" type="text/javascript"></script>
    <script>

        let chkCntA = 0;
        let chkCntU = 0;

        $(document).ready(function () {
            try {
                //debugger;
                let gv = document.getElementById('<%= GVAssign.ClientID %>');
                if (gv !== null) {
                    let cnt = chkCntA
                    for (let i = 1; i < gv.rows.length; i++) {
                        let cbChld = gv.rows[i].cells[0].querySelector('input[type="checkbox"]');
                        let chkd = cbChld.checked;
                        chkCntA += chkd ? 1 : 0;
                    }
                    //debugger;
                    var cbH = gv.rows[0].cells[0].querySelector('input[type="checkbox"]');
                    if (cbH !== null) cbH.checked = (chkCntA == gv.rows.length - 1);

                    EnableDisableBtnA(chkCntA);
                }
                gv = document.getElementById('<%= GVView.ClientID %>');
                if (gv !== null) {
                    for (let i = 1; i < gv.rows.length; i++) {
                        let cbChld = gv.rows[i].cells[0].querySelector('input[type="checkbox"]');
                        let chkd = cbChld.checked;
                        chkCntU += chkd ? 1 : 0;
                    }
                    //debugger;
                    var cbH = gv.rows[0].cells[0].querySelector('input[type="checkbox"]');
                    if (cbH !== null) cbH.checked = (chkCntU == gv.rows.length - 1);

                    EnableDisableBtnU(chkCntU);
                }
            }
            catch (e) {
                console.log(e);
            }
        });

        //Handle checkbox change for checkbox in Header row of GVAssign
        function CBAddHOnClick (cb) {
            try {
                //console.log("hi CBAddHOnClick");
                //debugger;
                var GVAssign = document.getElementById('<%= GVAssign.ClientID %>');
                if (GVAssign !== null) {
                    //Update each row's checkbox
                    for (let i = 1; i < GVAssign.rows.length; i++) {
                        let cbChld = GVAssign.rows[i].cells[0].querySelector('input[type="checkbox"]');
                        let chkdH = cb.checked;
                        if (chkdH !== cbChld.checked) {
                            cbChld.checked = chkdH;
                            chkCntA += chkdH ? 1 : -1;
                        }
                    }
                    if (GVAssign.rows.length < chkCntA)
                        chkCntA = GVAssign.rows.length;
                    else if (chkCntA < 0)
                        chkCntA = 0;

                    EnableDisableBtnA(chkCntA);
                }
                //console.log("bye CBAddHOnClick");

            } catch (e) {

                console.log(e);
            }
        }

        //Handle checkbox change for checkbox in each row of GVAssign
        function CBAddOnClick (cb) {
            try {
                //console.log("hi CBAddOnClick");
                //debugger;
                chkCntA += cb.checked ? 1 : -1;
                var GVAssign = document.getElementById('<%= GVAssign.ClientID%>');
                if (GVAssign !== null) {
                    if (GVAssign.rows.length < chkCntA)
                        chkCntA = GVAssign.rows.length;
                    else if
                        (chkCntA < 0) chkCntA = 0;

                    // Update header checkbox
                    let cbH = GVAssign.rows[0].querySelector('th input[type="checkbox"]');
                    cbH.checked = (GVAssign.rows.length - 1 === chkCntA);
                    EnableDisableBtnA(chkCntA);
                }
                //console.log("bye CBAddOnClick");

            } catch (e) {
                console.log(e);
            }
        }

        function EnableDisableBtnA (count) {
            let btnAssign = document.getElementById('<%= BtnAssign.ClientID %>');
            btnAssign.disabled = (count === 0);
        }

        //Handle checkbox change for checkbox in Header row of GVView
        function CBEditHOnClick (cb) {
            try {
                //console.log("hi CBEditHOnClick");
                //debugger;
                var GVView = document.getElementById('<%= GVView.ClientID %>');

                //Update each row's checkbox
                if (GVView !== null) {
                    for (let i = 1; i < GVView.rows.length; i++) {
                        let cbChld = GVView.rows[i].cells[0].querySelector('input[type="checkbox"]');
                        let chkdH = cb.checked;
                        if (chkdH !== cbChld.checked) {
                            cbChld.checked = chkdH;
                            chkCntU += chkdH ? 1 : -1;
                        }
                    }
                    if (GVView.rows.length < chkCntU)
                        chkCntU = GVView.rows.length;
                    else if (chkCntU < 0)
                        chkCntU = 0;

                    EnableDisableBtnU(chkCntU);
                }

            } catch (e) {
                console.log(e);
            }
            //console.log("by CBEditHOnClick");
        }

        //Handle checkbox change for checkbox in each row of GVView
        function CBEditOnClick (cb) {
            try {
                //console.log("hi CBEditOnClick");
                //debugger;
                chkCntU += cb.checked ? 1 : -1;
                var GVView = document.getElementById('<%= GVView.ClientID%>');
                if (GVView !== null) {
                    if (GVView.rows.length < chkCntU)
                        chkCntU = GVView.rows.length;
                    else if
                        (chkCntU < 0) chkCntU = 0;

                    // Update header checkbox
                    let cbH = GVView.rows[0].querySelector('th input[type="checkbox"]');
                    cbH.checked = (GVView.rows.length - 1 === chkCntU);
                    EnableDisableBtnU(chkCntU);
                }
            }
            //console.log("by CBEditOnClick");
            catch (e) {
                console.log(e);
            }
        }

        function EnableDisableBtnU (count) {
            let btnUnAssign = document.getElementById('<%= BtnUnAssign.ClientID %>');
            btnUnAssign.disabled = (count === 0);
        }

        function BtnOnClientClick (HFClientID, chr) {
            try {
                //console.log("BtnOnClientClick called");
                //console.log("HFClientID: " + HFClientID);
                //debugger;
                //let cnt = chr === 'U' ? chkCntU : chr === 'A' ? chkCntA : 0;
                //var hf = $('#' + HFClientID);
                //hf.val(cnt);
                //console.log("chKCount: " + cnt);
                //console.log("HFCnt Value: " + hf.val());
                return true;
            }
            catch (e) {
                console.log(e);
                return false;
            }
        }
        // Function to store scroll position
        function storeScrollPosition () {
            var gridview = document.getElementById('<%= GVAssign.ClientID %>');
            if (gridview != null) {
                var scrollPosition = gridview.scrollTop;
                sessionStorage.setItem('GridViewScrollPosition', scrollPosition);
            }
        }

        // Function to restore scroll position
        function restoreScrollPosition () {
            console.log("hi restoreScrollPosition");
            var scrollPosition = sessionStorage.getItem('GridViewScrollPosition');
            var gridview = document.getElementById('<%= GVAssign.ClientID %>');
            if (gridview != null && scrollPosition != null) {
                gridview.scrollTop = scrollPosition;
            }
            console.log("by restoreScrollPosition");
        }

        function setscroll () {
            debugger;
            var y = $(scroll.Y);
            console.log(y);
            y.val($('#<%= DvAssign.ClientID%>').scrollTop);
        }
    </script>
</asp:Content>
