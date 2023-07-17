<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="masterFEAUsers.aspx.cs" Inherits="masterFEAUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    
    <script type="text/javascript">
        function funDelRecord(myval) {
            document.getElementById('TextDelNo').value = myval;
            document.getElementById('Button3').click();
        }

        function showPopUp() {
            document.getElementById('ButtonDiv').click();
        }

    </script>

    
    <!-- Code to prevent pressing Backspace to navigate to previous page -->
    <script src="js/jquerynew.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).on("keydown", function (e) {
            if (e.which === 8 && !$(e.target).is("input:not([readonly]):not([type=radio]):not([type=button]):not([type=submit]):not([type=checkbox]), textarea, [contentEditable], [contentEditable=true]")) {
                e.preventDefault();
            }
        });
    </script>
    <!-- Code to prevent pressing Backspace to navigate to previous page ends here -->


</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center><p align="right" style="margin-top:-20px;"><asp:TextBox ID="TextDelNo" runat="server" width="1" Height="1" CssClass="input1"></asp:TextBox></p>
            <table width="90%" cellspacing="1" cellpadding="8">
                <tr>
                    <td align="center" colspan="2" class="tablehead" valign="top">F&EA User Management</td>
                </tr>
                <tr>
                    <td width="25%" align="left" valign="top" class="tablecell3">User ID <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextUserID" runat="server" Width="250" CssClass="input"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">Password <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextPassword" runat="server" Width="250" CssClass="input"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">Email ID <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextEmail" runat="server" Width="250" CssClass="input"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">Designation <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextDesignation" runat="server" Width="250" CssClass="input"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">Review Operators</td>
                    <td align="left" valign="top" class="tablecell3">
                        <asp:CheckBoxList ID="CheckOper" runat="server" RepeatColumns="5" Width="100%" CssClass="chks2" RepeatDirection="Horizontal"></asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">Tariff Review Access Level <font color="red"><b>*</b><br /><br />(Level 5 is the hightest level)</font></td>
                    <td align="left" valign="top" class="tablecell3">
                        <asp:RadioButtonList ID="RadReview" runat="server">
                            <asp:ListItem style="margin-right:20px;" Selected="True">None</asp:ListItem>
                            <asp:ListItem style="margin-right:20px;">Level 0</asp:ListItem>
                            <asp:ListItem style="margin-right:20px;">Level 1</asp:ListItem>
                            <asp:ListItem style="margin-right:20px;">Level 2</asp:ListItem>
                            <asp:ListItem style="margin-right:20px;">Level 3</asp:ListItem>
                            <asp:ListItem style="margin-right:20px;">Level 4</asp:ListItem>
                            <asp:ListItem>Level 5</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">Access to Reporting Tools <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3">
                        <asp:RadioButtonList ID="RadReport" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem style="margin-right:20px;" Selected="True">Yes</asp:ListItem>
                            <asp:ListItem style="margin-right:20px;">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="FEAParameters" runat="server" visible="false">
                    <td align="left" valign="top" class="tablecell3">Access to F&EA Parameters management <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3">
                        <asp:RadioButtonList ID="RadParameters" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem style="margin-right:20px;" Selected="True">Yes</asp:ListItem>
                            <asp:ListItem style="margin-right:20px;">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">Allowed to cancel 'Taken On Record' status <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3">
                        <asp:RadioButtonList ID="RadFD" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem style="margin-right:20px;">Yes</asp:ListItem>
                            <asp:ListItem style="margin-right:20px;" Selected="True">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>

               <tr>
                    <td colspan="2" align="center" class="tablehead">
                        <asp:Button ID="Button1" runat="server" Text="Add" OnClick="Button1_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button2" runat="server" Text="Modify" OnClick="Button2_Click" Visible="false" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="ButtonClear" runat="server" Text="Cancel" OnClick="ButtonClear_Click" />
                    </td>
                </tr>
            </table>

        <asp:TextBox ID="TextHidden" runat="server" Visible="false"></asp:TextBox>


        
        <br /><hr size="0" width="90%" /><br />


        <div id="divresults" runat="server"></div>
        <p align="right"><asp:Button ID="Button3" runat="server" Text="" Width="1" Height="1" CssClass="input1" OnClick="Button3_Click" OnClientClick="return confirm('Are you sure you wish to delete this record ?');" /></p>
        <br /><br />


    </div>
    </form>
</body>
</html>
