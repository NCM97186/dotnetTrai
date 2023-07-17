<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="TSP_QuartData.aspx.cs" Inherits="TSP_QuartData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
     <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>

    <script type="text/javascript">
        function hideButton2() {
            document.getElementById('Button2').style.display = 'none';
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

        <br />
        <table width="95%" runat="server" cellspacing="1" cellpadding="8">
            <tr>
            <td colspan="2" align="center" class="tablehead">Quarterly Data Figures</td>
            </tr>
            <tr>
                <td width="50%" align="left" valign="top" class="tablecell3">Prepaid / Postpaid <font color="red"><b>*</b></font></td>
                <td align="left" valign="top" class="tablecell3"><asp:RadioButtonList ID="RadPrePost" runat="server" RepeatDirection="Horizontal"><asp:ListItem style="margin-right:20px;">Prepaid</asp:ListItem><asp:ListItem>Postpaid</asp:ListItem></asp:RadioButtonList></td>
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
                    <asp:Button ID="Button1" runat="server" Width="100px" CssClass="input" OnClientClick="ShowProcessMessage('ProcessingWindow')" OnClick="Button1_Click" Text="Show Plans" />
                    <asp:TextBox ID="TextHidden" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <table width="95%" runat="server" cellspacing="1" cellpadding="8">
            <tr>
                <td colspan="2" align="center" class="tablehead" id="TdData" runat="server" visible="false"></td>
            </tr>
            <tr>
                <td align="center" valign="top" class="tablecell3b" colspan="2" id="tdCert" runat="server" visible="false"><asp:CheckBox ID="CheckCert" runat="server" Text="Certified that at any given point of time, not more than twenty five tariff plans are on offer including postpaid and prepaid." /></td>
            </tr>
      
            <tr>
                <td colspan="2" align="center" class="tablehead" id="TdSubmit" runat="server" visible="false">
                    <asp:Button ID="Button2" runat="server" Width="67px" CssClass="input" OnClientClick="hideButton2()" OnClick="Button2_Click" Text="Submit" />
                </td>
            </tr>
            <tr id="trNote" runat="server" visible="false">
                <td colspan="2" align="center"><font color="red"><b>* Please check the data before submitting. Data once submitted cannot be edited or deleted.</b></font></td>
            </tr>
        </table>
    
    
        <div id="divHidden" runat="server" visible="false"></div>
        
    </div>
    </form>
    </center>
</body>
</html>
