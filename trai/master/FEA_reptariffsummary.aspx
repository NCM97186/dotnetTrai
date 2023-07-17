<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_reptariffsummary.aspx.cs" Inherits="FEA_reptariffsummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
     <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    <script src="calendar/javascript/datepicker.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="calendar/stylesheets/datepicker.css" />
    
    <script type="text/javascript" language="javascript">
        function funChkAll(a,b)
        {
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

        function funPrint() {
            document.getElementById('Table1').style.display = 'none';
            document.getElementById('divLink').style.display = 'none';
            document.getElementById('divPending').style.display = 'none';
            window.print();
            document.getElementById('Table1').style.display = 'block';
            document.getElementById('divLink').style.display = 'block';
            document.getElementById('divPending').style.display = 'block';


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

        <table width="600" cellspacing="1" cellpadding="5" id="Table1" runat="server">
            <tr>
            <td align="center" colspan="4" class="tableheadcenter"><b>Tariff Summary Report</b></td>
            </tr>
            <tr>
                <td colspan="2" class="tablecell3b" align="center">TSP(s) / ISP(s)</td>
                <td colspan="2" class="tablecell3b" align="center"><asp:RadioButtonList ID="RadOType" Width="250px" runat="server" RepeatDirection="Horizontal"><asp:ListItem style="margin-right:20px;" Selected="True">TSP</asp:ListItem><asp:ListItem style="margin-right:20px;">ISP</asp:ListItem><asp:ListItem>Both</asp:ListItem></asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align="center" class="tablecell3b" valign="top" colspan="2">
                    Reporting Date Range</td>
                <td  class="tablecell3b" align="center" valign="top" colspan="2">
                    <asp:TextBox ID="TextDate3" MaxLength="30" runat="server" OnPreRender="TextDate3_PreRender" width="85" ></asp:TextBox>
                    &nbsp;&nbsp;To&nbsp;&nbsp;
                    <asp:TextBox ID="TextDate4" MaxLength="30" runat="server" OnPreRender="TextDate4_PreRender" width="85" ></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td colspan="4" align="center"><br />
                    <asp:Button ID="Button1" runat="server" CssClass="input" OnClientClick="ShowProcessMessage('ProcessingWindow')" OnClick="Button1_Click" Text="Show Summary" />
                </td>
            </tr>
        </table>
    
        <br /><br />
    
        <a name="Bookmark1"></a>
        <div id="div1" runat="server">
            <div id="divresult" runat="server"></div>
        </div>
        
        
        <div id="divChkResults" runat="server" style="display:none;"></div>
        <asp:TextBox ID="TextConditions" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> 
        <asp:TextBox ID="TextSortBy" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> 
        <asp:Button ID="ButtonExcel" runat="server" style="display:none;" OnClick="ButtonExcel_Click" />
        
        <div id="divLink" runat="server"></div>
        <div id="divExcel" runat="server"></div>

        <div id="divPending" runat="server"></div>

      </div>
    </form>
    </center>
</body>
</html>
