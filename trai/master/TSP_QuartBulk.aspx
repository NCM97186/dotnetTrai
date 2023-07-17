<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="TSP_QuartBulk.aspx.cs" Inherits="TSP_QuartBulk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
     <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>

    
    <script type="text/javascript">
        function hideButton2() {
            document.getElementById('Button2').style.display = 'none';
        }
    </script>

</head>
<body>
<center >
    <form id="form1" runat="server">
    <div>
        <br />
        <table width="95%" runat="server" cellspacing="1" cellpadding="8">
            <tr>
                <td colspan="2" align="center" class="tablehead">Quarterly Report - Bulk Tariff Plans</td>
            </tr>
            <tr>
                <td width="50%" align="left" valign="top" class="tablecell3">LSA <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropCircle" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">Quarter / Year <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropQtr" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="DropYr" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2" align="center" class="tablehead">
                    <asp:Button ID="Button1" runat="server" Width="100px" CssClass="input" OnClick="Button1_Click" Text="Next" />
                    <asp:TextBox ID="TextHidden" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <table width="95%" runat="server" cellspacing="1" cellpadding="8">
            <tr>
                <td colspan="2" align="center" class="tablehead" id="TdData" runat="server" visible="false"></td>
            </tr>
            <tr id="trCheck" runat="server" visible="false">
                <td class="tablecell3b" colspan="2">
                    <asp:CheckBox ID="Check1" runat="server" Text=" Certified that all tariff plans for bulk customers implemented during the preceding quarter are consistent with the regulatory principles in all respects." />
                    <br /><br />
                    <asp:CheckBox ID="Check2" runat="server" Text=" Certified that the above tariff plans are offered to bulk customers, such as corporates, small and medium enterprises, institutions etc. either in response to a tender process or as a result of negotiations between the access provider and such bulk customer." />
                    <br /><br />
                    <asp:CheckBox ID="Check3" runat="server" Text=" Certified that the tariff plan(s)/are consistent with the regulatory principles in all respects which inter–alia include IUC compliance, non-discrimination and non-predation." />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" class="tablehead" id="TdSubmit" runat="server" visible="false">
                    <asp:Button ID="Button2" runat="server" Width="67px" CssClass="input" OnClientClick="hideButton2()" OnClick="Button2_Click" Text="Submit" />
                </td>
            </tr>
            <tr id="trNote" runat="server" visible="false">
                <td colspan="2" align="center"><font color="red"><b>* Please check the data before submitting. Data once submitted cannot be edited or deleted.</b></font></td>
            </tr>
        </table>
    
    
        <div id="divHidden" runat="server" visible="false"></div>
        
    </div>
    </form>
    </center>
</body>
</html>
