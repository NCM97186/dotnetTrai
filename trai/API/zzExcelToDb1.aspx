<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="zzExcelToDb1.aspx.cs" Inherits="zzExcelToDb1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>TRAI</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
  

</head>
<body>    
    <form id="form1" runat="server">
    <div>
    
    <center>
    <font face="arial" size="2">


    <table width="100%" cellspacing="1" cellpadding="5">
    <tr>
    <td align="left" valign="top">

        <table width="98%" height="200" cellspacing="1" cellpadding="10">
        <tr>
        <td class="tablehead" align="center" valign="top"><b><U>Upload Tariff Records File</U></b>
        </td>
        </tr>
        <tr>
        <td class="tablecell" align="center" valign="top"><input id="uplTheFile" runat="server" style="width: 238px" type="file" /></td>
        </tr>
        <tr>
        <td class="tablehead" align="center" valign="top"><br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Upload Data" />
        &nbsp;&nbsp;&nbsp;&nbsp;<span id="txtOutput" style="font: 8pt verdana;" runat="server" ></span>
        </td>
        </tr>
        </table>

    </td>
    </tr>
    </table>

    </font>
    </center>
</div>

</form>

</body>
</html>
