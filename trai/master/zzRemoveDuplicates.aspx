<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zzRemoveDuplicates.aspx.cs" Inherits="zzRemoveDuplicates" %>

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
    <form id="form1" runat="server">
    <div>

        
        <!-- this div is the holder for the 'Please Wait' message -->
        <div id="ProcessingWindow" visible="false" style="position:fixed; width:100%; left:0px; top:300px; background-color:#ffffff;z-index:1000;">
        </div>


    <center><br />
        <div id="divDate">


        <table width="90%" id="tbLedger" runat="server" cellspacing="1" cellpadding="7">
            <tr>
                <td class="tablehead" colspan="2" align="center"><b><font color="yellow">PLEASE TAKE A BACKUP OF THE DATABASE BEFORE PRESSING THE BUTTON</font></b></td>
            </tr>
            <tr>
                <td class="tablecell" align="center" valign="top"><asp:Button ID="Button1" runat="server" Text="Show Duplicates" OnClick="Button1_Click"  OnClientClick="ShowProcessMessage('ProcessingWindow')" /></td>
                <td class="tablecell" align="center" valign="top"><asp:Button ID="Button2" runat="server" Text="REMOVE DUPLICATES" OnClick="Button2_Click" OnClientClick="return confirm('Are you sure you wish to remove duplicate records ?');" /></td>
            </tr>
            <tr>
                <td width="50%" class="tablecell" align="center" valign="top">
                    <b>Display Duplicate Count</b>&nbsp;<asp:Label ID="LblTotal" runat="server" ForeColor="Red" Font-Bold="true" Text="(Duplicates : )"></asp:Label>
                    <br />(select count(*) as num, uniqueid from TRAI_tariffs group by uniqueid order by num desc)
                    <br /><div id="divIDs" runat="server"></div>
                </td>
                <td class="tablecell" align="center" valign="top">
                    <b>Remove Duplicates</b><br />(Press the button once <font color="red"><b>ONLY</b></font> and wait for the confirmation)
                    <br /><div id="divresults" runat="server"></div>
                </td>
            </tr>
            
            
         
        </table>


        </div>



    </center>
    </div>
    </form>
</body>
</html>
