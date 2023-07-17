<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zzSampleComparison.aspx.cs" Inherits="zzSampleComparison" %>

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


            <b><u>COMPARISON REPORT</u></b>
            <br /><br />
        
            <table width="95%" cellspacing="1" cellpadding="7">
            <tr>
                <td colspan="3" class="tablehead" align="center">TSP</td>
            </tr>
            <tr>
                <td colspan="3" class="tablecell3" align="center">
                    <asp:CheckBoxList ID="CheckTSP" runat="server" CssClass="chks" RepeatDirection="Horizontal">
                        <asp:ListItem style="margin-right:5px;">Aircel</asp:ListItem>
                        <asp:ListItem style="margin-right:5px;">Airtel</asp:ListItem>
                        <asp:ListItem style="margin-right:5px;">BSNL</asp:ListItem>
                        <asp:ListItem style="margin-right:5px;">Idea</asp:ListItem>
                        <asp:ListItem style="margin-right:5px;">Jio</asp:ListItem>
                        <asp:ListItem style="margin-right:5px;">MTNL</asp:ListItem>
                        <asp:ListItem style="margin-right:5px;">Quadrant (Connect)</asp:ListItem>
                        <asp:ListItem style="margin-right:5px;">Tata Tele</asp:ListItem>
                        <asp:ListItem style="margin-right:5px;">Telenor</asp:ListItem>
                        <asp:ListItem>Vodafone</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td class="tablehead" align="center" width="33%">LSA</td>
                <td class="tablehead" align="center" width="33%">Product Type</td>
                <td class="tablehead" align="center" width="33%">Validity Range (days)</td>
            </tr>
            <tr>
                <td class="tablecell3" align="center"><asp:DropDownList ID="DropLSA" runat="server"><asp:ListItem>Andhra Pradesh</asp:ListItem></asp:DropDownList></td>
                <td class="tablecell3" align="center"><asp:DropDownList ID="DropProductType" runat="server"><asp:ListItem>Prepaid STV</asp:ListItem></asp:DropDownList></td>
                <td class="tablecell3" align="center"><asp:TextBox ID="TextValidity1" runat="server" CssClass="input" Width="100"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;To&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="TextValidity2" runat="server" CssClass="input" Width="100"></asp:TextBox></td>
            </tr>

            <tr>
                <td class="tablehead" align="center">Price Range (&#8377;)</td>
                <td class="tablehead" align="center">Report Date Range</td>
                <td class="tablehead" align="center">Launch Date Range</td>
            </tr>
            <tr>
                <td class="tablecell3" align="center">
                    <asp:TextBox ID="TextPrice1" runat="server" CssClass="input" Width="100"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;To&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="TextPrice2" runat="server" CssClass="input" Width="100"></asp:TextBox>
                </td>
                <td class="tablecell3" align="center">
                    <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" width="150" ></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;To&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender" width="150" ></asp:TextBox>
                </td>
                <td class="tablecell3" align="center">
                    <asp:TextBox ID="TextDate3" MaxLength="30" runat="server" OnPreRender="TextDate3_PreRender" width="150" ></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;To&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="TextDate4" MaxLength="30" runat="server" OnPreRender="TextDate4_PreRender" width="150" ></asp:TextBox>
                </td>




            <tr>
                <td align="center" class="tablehead" colspan="3">
                    <asp:CheckBox ID="ChkArchives" runat="server" Text="Include Archives" CssClass="chks" />
                    <br /><br />
                    <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" />
                </td>
            </tr>
        </table>

        <p align="left" style="margin-left:40px;">
        <asp:ImageButton ID="ButtonCompare" runat="server" ImageUrl="../consumerview/images/btncompare.jpg" OnClick="ButtonCompare_Click" />
        </p>

        <div id="divTariffs" runat="server"></div>

        </div>


        


    </center>
    </div>
    </form>
</body>
</html>
