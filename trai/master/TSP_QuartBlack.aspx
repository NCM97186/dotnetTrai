<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="TSP_QuartBlack.aspx.cs" Inherits="TSP_QuartBlack" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
     <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    <script src="calendar/javascript/datepicker.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="calendar/stylesheets/datepicker.css" />
    
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
            <td colspan="2" align="center" class="tablehead">Black Out Days</td>
            </tr>
            <tr>
                <td width="50%" align="left" valign="top" class="tablecell3">Calendar Year <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropYr" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">LSA <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropCircle" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2" align="center" class="tablehead">
                    <asp:Button ID="Button1" runat="server" Width="100px" CssClass="input" OnClick="Button1_Click" Text="Show Dates" />
                </td>
            </tr>
        </table>
        <br /><br />
        <table width="95%" id="tableData" runat="server" visible="false" cellspacing="1" cellpadding="8">
            <tr>
                <td width="33%" class="tablehead" align="left">Black Out Days</td>
                <td width="33%" class="tablehead" align="left">Date</td>
                <td width="33%" class="tablehead" align="left">Occasion</td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">Black Out Day 1</td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextDate1" MaxLength="30" runat="server" OnPreRender="TextDate1_PreRender" width="150" ></asp:TextBox></td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextOcc1" Width="250" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">Black Out Day 2</td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender" width="150" ></asp:TextBox></td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextOcc2" Width="250" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">Black Out Day 3</td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextDate3" MaxLength="30" runat="server" OnPreRender="TextDate3_PreRender" width="150" ></asp:TextBox></td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextOcc3" Width="250" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">Black Out Day 4</td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextDate4" MaxLength="30" runat="server" OnPreRender="TextDate4_PreRender" width="150" ></asp:TextBox></td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextOcc4" Width="250" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">Black Out Day 5</td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextDate5" MaxLength="30" runat="server" OnPreRender="TextDate5_PreRender" width="150" ></asp:TextBox></td>
                <td align="left" valign="top" class="tablecell3"><asp:TextBox ID="TextOcc5" Width="250" runat="server"></asp:TextBox></td>
            </tr>
            <tr style="height:2px;">
                <td class="tablehead" colspan="3"></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">Weblink <font color="red">*</font></td>
                <td align="left" valign="top" colspan="2" class="tablecell3"><asp:TextBox ID="TextWebLink" Width="450" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="3" align="center" class="tablehead" id="TdSubmit" runat="server" visible="false">
                    <asp:Button ID="Button2" runat="server" Width="67px" CssClass="input" OnClientClick="hideButton2()" OnClick="Button2_Click" Text="Submit" />
                </td>
            </tr>
            <tr id="trNote" runat="server" visible="false">
                <td colspan="3" align="center"><font color="red"><b>* Please check the data before submitting. Data once submitted cannot be edited or deleted.</b></font></td>
            </tr>
        </table>
    
    </div>
    </form>
    </center>
</body>
</html>
