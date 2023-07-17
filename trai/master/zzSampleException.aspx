<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zzSampleException.aspx.cs" Inherits="zzSampleException" %>

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


            <b><u>EXCEPTION REPORT</u></b>
            <br /><br />
        
            <table width="95%" cellspacing="1" cellpadding="7">
            <tr>
                <td class="tablehead" align="center" width="50%">TSP</td>
                <td class="tablehead" align="center" width="50%">LSA</td>
            </tr>
            <tr>
                <td class="tablecell3" align="center"><asp:DropDownList ID="DropTSP" runat="server"><asp:ListItem>BSNL</asp:ListItem></asp:DropDownList></td>
                <td class="tablecell3" align="center"><asp:DropDownList ID="DropLSA" runat="server"><asp:ListItem>Delhi</asp:ListItem></asp:DropDownList></td>
            </tr>

            <tr>
                <td class="tablehead" align="center">Date Range</td>
                <td class="tablehead" align="center">Report Type</td>
            </tr>
            <tr>
                <td class="tablecell3" align="center">
                    <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" width="150" ></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;To&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender" width="150" ></asp:TextBox>
                </td>
                <td class="tablecell3" align="center">
                    <asp:RadioButtonList ID="RadType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem style="margin-right:30px;">Error Logs</asp:ListItem>
                        <asp:ListItem>Flags in Tariff Review</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>



            <tr>
                <td align="center" class="tablehead" colspan="3"><asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" /></td>
            </tr>
        </table>

            <br /><br /><br />

        <div id="divTariffs" runat="server"></div>

        </div>


        


    </center>
    </div>
    </form>
</body>
</html>
