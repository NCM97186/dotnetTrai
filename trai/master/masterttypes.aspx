<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="masterttypes.aspx.cs" Inherits="masterttypes" %>

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
    <center><p align="right"><asp:TextBox ID="TextDelNo" runat="server" width="1" Height="1" CssClass="input1"></asp:TextBox></p>
            <table width="70%" cellspacing="1" cellpadding="14">
                <tr>
                    <td align="center" colspan="2" class="tablehead" valign="top">Tariff Product Management</td>
                </tr>
                <tr>
                    <td width="50%" align="left" valign="top" class="tablecell3">Tariff Product Type <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextBox1" runat="server" Width="200" CssClass="input"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="left" valign="top" class="tablecell3">Tariff Product Code <font color="red"><b>*</b></font></td>
                    <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextBox2" runat="server" Width="45" MaxLength="4" CssClass="input"></asp:TextBox></td>
                </tr>
          
               <tr>
                    <td colspan="2" align="center" class="tablehead">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button1" runat="server" Text="Add" OnClick="Button1_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button2" runat="server" Text="Modify" OnClick="Button2_Click" Visible="false" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="ButtonClear" runat="server" Text="Cancel" OnClick="ButtonClear_Click" />
                    </td>
                </tr>
            </table>

             <br /><br />
            <hr size="0" color="#a7a7a7" width="80%"/>
            <br />

            <div id="divresults" runat="server"></div>

        <p align="right"><asp:Button ID="Button3" runat="server" Text="" Width="1" Height="1" CssClass="input1" OnClick="Button3_Click" OnClientClick="return confirm('Are you sure you wish to delete this record ?');" /></p>

        

    </div>
    </form>
</body>
</html>
