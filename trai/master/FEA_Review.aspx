<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="FEA_Review.aspx.cs" Inherits="FEA_Review" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    <script src="calendar/javascript/datepicker.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="calendar/stylesheets/datepicker.css" />
    
    <!-- Code to prevent pressing Backspace to navigate to previous page -->
    <script src="js/jquery.min.js" type="text/javascript"></script>
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

        function funSummary(uid) {
            document.getElementById("divSummary" + uid).style.display = "block";
            document.getElementById("divViol" + uid).style.display = "none";
        }
        function funViolation(uid) {
            document.getElementById("divViol" + uid).style.display = "block";
            document.getElementById("divSummary" + uid).style.display = "none";
        }
        
        function funExcel() {
            document.getElementById('ButtonExcel').click();
        }

        function funWord2(uid) {
            document.getElementById('TextUID').value = uid;
            document.getElementById('ButtonWord2').click();
        }

        function funWord(uid) {
            document.getElementById('TextUID').value = uid;
            document.getElementById('ButtonWord').click();
        }

        function funPDF() {
            document.getElementById('ButtonPDF').click();
        }

        function funClearAction(a){
            var elementRef = document.getElementById('A' + a);
            var inputElementArray = elementRef.getElementsByTagName('input');
            for (var i = 0; i < inputElementArray.length; i++) {
                var inputElement = inputElementArray[i];
                inputElement.checked = false;
            }
            return false;
        }

        function funHide() {
            document.getElementById("Button1").style.display = "none";
        }


    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center><br />
        <div id="divDate">


            
        <table width="100%" id="tbLedger" runat="server" cellspacing="1" cellpadding="7">
            <tr>
                <td class="tablecell3b" align="left" width="50%">
                    <b><u>TARIFF REVIEW (<asp:Label ID="LblTotal" runat="server"></asp:Label>)</u></b>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="DropOperator" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropOperator_Change"></asp:DropDownList>
                </td>
                <td class="tablecell3b" align="right">
                    <asp:RadioButtonList ID="RadType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RadType_Change"></asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2"><div id="divTariffs" runat="server"></div></td>
            </tr>
            <tr>
                <td align="center" class="tablehead" colspan="2"><asp:Button ID="Button1" runat="server" Text="Submit" OnClientClick="funHide();" OnClick="Button1_Click" /></td>
            </tr>
        </table>

        <br /><br />

            <div id="pagenums" runat="server"></div>

        </div>


        <asp:Button ID="ButtonExcel" runat="server" style="display:none;" OnClick="ButtonExcel_Click" />
        <asp:Button ID="ButtonWord2" runat="server" style="display:none;" OnClick="ButtonWord2_Click" />
        <asp:Button ID="ButtonWord" runat="server" style="display:none;" OnClick="ButtonWord_Click" />
        <asp:Button ID="ButtonPDF" runat="server" style="display:none;" OnClick="ButtonPDF_Click" />
        <asp:TextBox ID="TextCntr" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="TextUID" runat="server" CssClass="input1" Width="1" Height="1"></asp:TextBox>

    </center>
    </div>
    </form>
</body>
</html>
