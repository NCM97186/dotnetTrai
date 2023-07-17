<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="FEA_repexception.aspx.cs" Inherits="FEA_repexception" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all" />
    <script src="calendar/javascript/datepicker.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="calendar/stylesheets/datepicker.css" />

    <script type="text/javascript" language="javascript">
        function funChkAll(a, b) {
            var chkBox = document.getElementById(b);
            var options = chkBox.getElementsByTagName('input');
            var listOfSpans = chkBox.getElementsByTagName('span');
            if (document.getElementById(a).checked == true) {
                for (var i = 0; i < options.length; i++) {
                    options[i].checked = true;
                }
            }
            else {
                for (var i = 0; i < options.length; i++) {
                    options[i].checked = false;
                }
            }
        }

        function funExcel() {
            document.getElementById('ButtonExcel').click();
        }

        function funsort(fld, updown) {
            var sorting = ' order by ' + fld + ' ' + updown;
            document.getElementById("TextSortBy").value = sorting;
            document.getElementById("ButtonShow").click();
        }

    </script>

    <!-- Code for 'Please Wait' -->

    <script language="javascript" type="text/javascript">

        ShowProcessMessage = function (PanelName) {
            //Sets the visibility of the Div to 'visible'
            document.getElementById(PanelName).style.visibility = "visible";

            /* Displays the  'In-Process' message through the innerHTML.
               You can write Image Tag to display Animated Gif */

            document.getElementById(PanelName).innerHTML = '<table style=padding:5px;border:4px;border-style:solid;border-color:#a20000;color:#a20000;background-color:#a20000;border-radius:15px;><tr><td align=center><font style=font-size:14px;><img src=images/processing.gif border=0></font></td></tr></table>';

            //Call Function to Disable all the other Controls
            //DisableAllControls('btnLoad');  // this function has been disabled as it causes the control to not retain the selected value

            return true; //Returns the control to the Server click event
        }

    </script>

    <script language="javascript" type="text/javascript">

        DisableAllControls = function (CtrlName) {
            var elm;
            /*Loop for all the controls of the page.*/
            for (i = 0; i <= document.forms[0].elements.length - 1; i++) {
                /* 1.Check for the Controls with type 'hidden' – 
                which are ASP.NET hidden controls for Viewstate and EventHandlers. 
                It is very important that these are always enabled, 
           for ASP.NET page to be working.
                   2.Also Check for the control which raised the event 
           (Button) - It should be active. */

                elm = document.forms[0].elements[i];

                if ((elm.name == CtrlName) || (elm.type == 'hidden')) {
                    elm.disabled = false;
                }
                else {
                    elm.disabled = true; //Disables all the other controls
                }
            }
        }

    </script>

    <!-- Code for 'Please Wait' - ENDS HERE -->

</head>
<body>
    <center>
        <form id="form1" runat="server">
            <div>
                <!-- this div is the holder for the 'Please Wait' message -->
                <div id="ProcessingWindow" visible="false" style="position: fixed; width: 100%; left: 0px; top: 300px; background-color: #ffffff; z-index: 1000;">
                </div>

                <table width="100%" cellspacing="1" cellpadding="5">
                    <tr>
                        <td align="center" colspan="4" class="tableheadcenter">
                            <table width="100%">
                                <tr>
                                    <td align="right" class="tablehead" width="50%"><b>Exception Report : </b></td>
                                    <td align="left" class="tableheadcenter"><b>
                                        <asp:RadioButtonList ID="RadOType" Width="250px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LoadOperators" RepeatDirection="Horizontal">
                                            <asp:ListItem style="margin-right: 20px; color: #ffffff;" Selected="True">TSP</asp:ListItem>
                                            <asp:ListItem style="margin-right: 20px; color: #ffffff;">ISP</asp:ListItem>
                                            <asp:ListItem style="color: #ffffff;">Both</asp:ListItem>
                                        </asp:RadioButtonList></b></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tablehead" align="left">TSP(s) / ISP(s)</td>
                        <td colspan="2" class="tablehead" align="right">
                            <asp:CheckBox ID="ChkAllOperators" runat="server" onclick="funChkAll('ChkAllOperators','ChkOper')" CssClass="input3" Text="Select All" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4" class="tablecell3b" align="center" valign="top">
                            <asp:CheckBoxList ID="ChkOper" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" Width="100%" CssClass="chks2"></asp:CheckBoxList>
                        </td>
                    </tr>

                    <tr>
                        <td align="center" class="tablehead" valign="top" colspan="4">Submission Date Range</td>
                    </tr>

                    <tr>
                        <td class="tablecell3b" align="center" valign="top" colspan="4">
                            <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" Width="85"></asp:TextBox>
                            &nbsp;&nbsp;To&nbsp;&nbsp;
                    <asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender" Width="85"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4" align="center">
                            <br />
                            <asp:Button ID="Button1" runat="server" CssClass="input" OnClientClick="ShowProcessMessage('ProcessingWindow')" OnClick="Button1_Click" Text="Show Report" />
                        </td>
                    </tr>
                </table>

                <br />
                <br />
                <a name="Bookmark1"></a>
                <div id="pagingDiv1" class="activeLink" runat="server"></div>                
                <div id="div1" runat="server">
                    <div id="divresult" runat="server"></div>
                </div>
                <div id="pagingDiv2" class="activeLink" runat="server"></div>
                <asp:TextBox ID="TextPage" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox>
            </div>
        </form>
    </center>
</body>
</html>
