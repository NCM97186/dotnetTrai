<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_repQuartBlack.aspx.cs" Inherits="FEA_repQuartBlack" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
     <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>

    <script type="text/javascript" language="javascript">

        function funPrint() {
            document.getElementById('Table1').style.display = 'none';
            window.print();
            document.getElementById('Table1').style.display = 'block';
        }

    </script>


</head>
<body>
<center >
    <form id="form1" runat="server">
    <div>
        <br />
        <table width="95%" runat="server" cellspacing="1" cellpadding="8" id="Table1">
            <tr>
            <td colspan="2" align="center" class="tablehead">Report of Tariff for Black Out Days</td>
            </tr>
            <tr id="tr1" runat="server" visible="false">
                <td width="50%" class="tablecell3" align="left">TSP(s) / ISP(s)</td>
                <td class="tablecell3" align="left"><asp:RadioButtonList ID="RadOType" Width="250px" runat="server" RepeatDirection="Horizontal"><asp:ListItem style="margin-right:20px;" Selected="True">TSP</asp:ListItem><asp:ListItem style="margin-right:20px;">ISP</asp:ListItem><asp:ListItem>Both</asp:ListItem></asp:RadioButtonList></td>
            </tr>
            <tr>
                <td width="50%" align="left" valign="top" class="tablecell3">Calendar Year <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropYr" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2" align="center" class="tablehead">
                    <asp:Button ID="Button1" runat="server" Width="100px" CssClass="input" OnClick="Button1_Click" Text="Show Report" />
                </td>
            </tr>
        </table>
        <br />
        <a name="Bookmark1"></a>
        <table runat="server" cellspacing="1" cellpadding="8" width="100%">
            <tr>
                <td colspan="2" align="left" class="tablecell" id="TdPrint" runat="server" visible="false">
                    <asp:ImageButton ID="ButtonExcel" runat="server" ImageUrl="images/excel.jpg" AlternateText="Export to Excel" ToolTip="Export to Excel" OnClick="ButtonExcel_Click" />
                    <!--
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <a href="javascript:funPrint()"><img src="images/iconprint.png" alt="" width="30" border="0" /></a>
                    -->
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" class="tablecell" id="TdData" runat="server" visible="false"></td>
            </tr>
        </table>
        
    </div>
    </form>
    </center>
</body>
</html>
