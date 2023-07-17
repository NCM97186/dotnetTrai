<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_repQuartPostpaid.aspx.cs" Inherits="FEA_repQuartPostpaid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
     <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>

    <script type="text/javascript" language="javascript">

        function funPrint() {
            document.getElementById('Table1').style.display = 'none';
            document.getElementById('TdPrint').style.display = 'none';
            window.print();
            document.getElementById('Table1').style.display = 'block';
            document.getElementById('TdPrint').style.display = 'block';

        }

    </script>

    
    <!-- Code for 'Please Wait' -->

        <script language ="javascript" type="text/javascript" >

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

            function funNotice(prepost, delay, qtrenddate, repdate) {
                document.getElementById('TextPrePost').value = prepost;
                document.getElementById('TextDelay').value = delay;
                document.getElementById('TextQtrEndDate').value = qtrenddate;
                document.getElementById('TextRepDate').value = repdate;
                document.getElementById('ButtonNotice').click();
            }
        </script>

        <script language ="javascript" type="text/javascript" >

            DisableAllControls = function (CtrlName) {
                var elm;
                /*Loop for all the controls of the page.*/
                for (i = 0; i <= document.forms[0].elements.length - 1 ; i++) {
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
<center >
    <form id="form1" runat="server">
    <div>
        
        <!-- this div is the holder for the 'Please Wait' message -->
        <div id="ProcessingWindow" visible="false" style="position:fixed; width:100%; left:0px; top:300px; background-color:#ffffff;z-index:1000;">
        </div>

        <br />
        <table width="95%" runat="server" cellspacing="1" cellpadding="8" id="Table1">
            <tr>
            <td colspan="2" align="center" class="tablehead">Quarterly Postpaid Tariff Report</td>
            </tr>
            <tr id="tr1" runat="server" visible="false">
                <td width="50%" class="tablecell3" align="left">Report For</td>
                <td class="tablecell3" align="left"><asp:RadioButtonList ID="RadOType" Width="250px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadOType_Change" RepeatDirection="Horizontal"><asp:ListItem style="margin-right:20px;" Selected="True">TSP</asp:ListItem><asp:ListItem style="margin-right:20px;">ISP</asp:ListItem><asp:ListItem>Both</asp:ListItem></asp:RadioButtonList></td>
            </tr>
            <tr>
                <td width="50%" align="left" valign="top" class="tablecell3">TSP <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropOperator" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">LSA <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropCircle" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td align="left" valign="top" class="tablecell3">Quarter / Year <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:DropDownList ID="DropQtr" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="DropYr" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2" align="center" class="tablehead">
                    <asp:Button ID="Button1" runat="server" Width="100px" CssClass="input" OnClientClick="ShowProcessMessage('ProcessingWindow')" OnClick="Button1_Click" Text="Show Report" />
                </td>
            </tr>
        </table>
        <br />
        <a name="Bookmark1"></a>
                
        <table width="100%" cellspacing="1" cellpadding="15">
            <tr>
                <td colspan="3" id="tdHr1" runat="server"></td>
            </tr>
            <tr>
                <td align="left" class="tablecell" id="tdRepDate" runat="server"></td>
                <td align="center" class="tablecell" id="tdNotice" runat="server"></td>
                <td align="right" class="tablecell" id="tdDelay" runat="server"></td>
            </tr>
            <tr>
                <td colspan="3" id="tdHr2" runat="server"></td>
            </tr>
        </table>
        
        <table runat="server" cellspacing="1" cellpadding="8">
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
        
        <asp:TextBox ID="TextPrePost" runat="server" CssClass="input1" Width="1" Height="1"></asp:TextBox>
        <asp:TextBox ID="TextDelay" runat="server" CssClass="input1" Width="1" Height="1"></asp:TextBox>
        <asp:TextBox ID="TextQtrEndDate" runat="server" CssClass="input1" Width="1" Height="1"></asp:TextBox>
        <asp:TextBox ID="TextRepDate" runat="server" CssClass="input1" Width="1" Height="1"></asp:TextBox>
        <asp:Button ID="ButtonNotice" runat="server" style="display:none;" OnClick="ButtonNotice_Click" />

    </div>
    </form>
    </center>
</body>
</html>
