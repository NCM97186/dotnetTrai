<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="masterTSPLogins.aspx.cs" Inherits="masterTSPLogins" %>

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
    <script src="js/jquery.min.js" type="text/javascript"></script>
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
    <form id="form1" runat="server" autocomplete="off">
    <div>
    <center><p align="right" style="margin-top:-20px;"><asp:TextBox ID="TextDelNo" runat="server" width="1" Height="1" CssClass="input1"></asp:TextBox></p>
            <table width="60%" cellspacing="1" cellpadding="8">
                <tr>
                    <td align="center" colspan="2" class="tablehead" valign="top">TSP / ISP User Management</td>
                </tr>
                <tr>
                    <td width="50%" align="left" valign="top" class="tablecell3">TSP / ISP</td>
                    <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropOperator" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">User ID <font color="red"><b>*</b></font></td>
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
                    <td colspan="2" align="center" class="tablehead"><asp:Button ID="ButtonGenKey" runat="server" Text="Generate Key" OnClick="ButtonGenKey_Click" /></td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3b">Authorization Key for API <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3b">
                        <asp:TextBox ID="TextKey" runat="server" Width="250" CssClass="input" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="TextHiddenKey" Width="400" runat="server" Visible="false"></asp:TextBox>
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
