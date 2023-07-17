<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="TSP_changepass.aspx.cs" Inherits="TSP_changepass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
     <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
</head>
<body>
<center >
    <form id="form1" runat="server">
    <div>
         <br />
        <br /><br />
       
        <br />
        <br />
        <table width="400" id="tablepass" runat="server" cellspacing="1" cellpadding="8">
            <tr>
            <td colspan="2" align="center" class="tablehead">Change Password</td>
            </tr>
            <tr>
                <td align="left" class="tablecell3b" valign="top">
                <br />
                    Enter New Password:</td>
                <td  class="tablecell3b" align="center" valign="top">
                <br />
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="input" TextMode="Password" Width="172px"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
                        ErrorMessage="*Enter Password" Font-Bold="False"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td  class="tablecell3b" align="left" valign="top">
                <br />
                    Confirm New Password:</td>
                <td  class="tablecell3b" align="center" valign="top">
                <br />
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="input" TextMode="Password" Width="170px"></asp:TextBox><br />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="TextBox1"
                        ControlToValidate="TextBox2" ErrorMessage="*Re-Enter Password"></asp:CompareValidator></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="Button1" runat="server" Width="67px" CssClass="input" OnClick="Button1_Click" Text="Submit" />
                </td>
            </tr>
        </table>
    
    
        <div id="divmsg" visible="false" runat="server"><font face="arial" size="2" color="#ff0000"><b>Your password has been changed successfully.</b></font></div>
    </div>
    </form>
    </center>
</body>
</html>
