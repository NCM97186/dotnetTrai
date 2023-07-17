<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TSP_SubmissionReport.aspx.cs" Inherits="TSP_SubmissionReport" %>

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
        function funExcel() {
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
                <td class="tablehead" align="center" colspan="2">Tariff Submission Report</td>
            </tr>
            <tr>
                <td class="tablecell" width="50%" align="left" valign="top">Date Range</td>
                <td class="tablecell" align="left" valign="top">
                    <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" width="150" ></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;To&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender" width="150" ></asp:TextBox>
                </td>
            </tr>         
            <tr>
                <td colspan="2" align="center" class="tablehead"><asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" /></td>
            </tr>
        </table>
        </div>
        <br /><br />
        <div id="divdownload" runat="server"></div>
        <div id="pagingDiv1" runat="server"></div>
        <div id="divresults" runat="server"></div>
        <div id="pagingDiv2" runat="server"></div>

        <asp:Button ID="ButtonExcel" runat="server" style="display:none;" OnClick="ButtonExcel_Click" />
        <asp:TextBox ID="TextPage" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox>
    </center>
    </div>
    </form>
</body>
</html>
