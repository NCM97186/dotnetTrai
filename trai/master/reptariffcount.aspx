<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="reptariffcount.aspx.cs" Inherits="reptariffcount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    <script src="calendar/javascript/datepicker.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="calendar/stylesheets/datepicker.css" />
    
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



    <script type="text/javascript">
            function funExcel(){
                document.getElementById('ButtonExcel').click();
            }
    </script>



</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center><br />
        <div id="divDate">


        <table width="90%" id="tbLedger" runat="server" cellspacing="1" cellpadding="7">
            <tr>
                <td class="tablehead" align="center" colspan="2">Count of Active Tariffs Launched During Selected Date Range</td>
            </tr>
            <tr>
                <td class="tablecell" width="50%" align="left" valign="top">Submission Date (Date Range)</td>
                <td class="tablecell" align="left" valign="top">
                    <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" width="150" ></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;To&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender" width="150" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tablecell" align="left" valign="top">Report Type</td>
                <td class="tablecell" align="left" valign="top"><asp:RadioButtonList ID="RadOType" Width="250px" runat="server" RepeatDirection="Horizontal"><asp:ListItem style="margin-right:20px;" Selected="True">TSP</asp:ListItem><asp:ListItem style="margin-right:20px;">ISP</asp:ListItem><asp:ListItem>Both</asp:ListItem></asp:RadioButtonList></td>
            </tr>
            <tr>
                <td class="tablecell" align="left" valign="top">
                    Launch Date
                </td>
                <td class="tablecell" align="left" valign="top">
                    <asp:RadioButtonList ID="RadLaunchDate" runat="server" RepeatDirection="Horizontal"><asp:ListItem Selected="True" style="margin-right:30px;">After 01-July-2018</asp:ListItem><asp:ListItem>Before 01-July-2018</asp:ListItem></asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" class="tablehead"><asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" /></td>
            </tr>
        </table>


        </div>


        <br /><br />
        <div id="divresults" runat="server"></div>
        
    </center>
    </div>
    </form>
</body>
</html>
