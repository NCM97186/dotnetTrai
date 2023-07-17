<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_parameters.aspx.cs" Inherits="FEA_parameters" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:100vh;">
    
    
<center>

    <br /><br /><br /><br />

    <table width="550px" cellspacing="1" cellpadding="15" border="1" style="border-collapse:collapse;">
        <tr>
            <td colspan="2" align="center" class="tableheadcenter"><b>Penalty Parameters</td>
        </tr>
        <tr>
            <td width="50%" align="left" valign ="middle" class="tablecell2b">Financial Disincentive per day (Rs.)</td>
            <td align="left" valign ="middle" class="tablecell2b"><asp:TextBox ID="TextBox1" runat="server" CssClass="input" Width="250"></asp:TextBox></td>
        </tr>
   
        
    </table>
    <br />
    <asp:ImageButton ID="ImageButton1" runat="server" OnClick="Button1_Click" ImageUrl="images/submit2.jpg" />

    </center>
    
    </div>
    </form>
    
</body>
</html>