<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zzSampleReview.aspx.cs" Inherits="zzSampleReview" %>

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


            <b><u>TARIFF REVIEW</u></b>
        <table width="100%" id="tbLedger" runat="server" cellspacing="1" cellpadding="7">
            
            <tr>
                <td align="center"><div id="divTariffs" runat="server"></div></td>
            </tr>
            <tr>
                <td align="center" class="tablehead"><asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" /></td>
            </tr>
        </table>


        </div>


        


    </center>
    </div>
    </form>
</body>
</html>
