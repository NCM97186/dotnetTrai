<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_repadvance.aspx.cs" Inherits="FEA_repadvance" %>

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

        function funDel(a, b) {
            document.getElementById('TextDel1').value = a;
            document.getElementById('TextDel2').value = b;
            document.getElementById('ButtonDel').click();
        }

        function funExcel() {            
            document.getElementById('ButtonExcel').click();
        }

        function funsort(fld, updown) {
            var sorting = ' order by ' + fld + ' ' + updown;
            document.getElementById("TextSortBy").value = sorting;
            document.getElementById("ButtonShow").click();
        }

        function funComparison(ttype, id) {
            if (document.getElementById(id).checked == true) {
                if (document.getElementById('TextCompareProduct').value == "") {
                    document.getElementById('TextCompareProduct').value = ttype;
                }
                else {
                    if (document.getElementById('TextCompareProduct').value != ttype) {
                        //alert('Comparison is possible for same product only.')
                        //document.getElementById(id).checked = false;
                    }
                }
            }

            var chkflag = 0;
            var div1 = document.getElementById('divresults');
            var checkboxCollection1 = div1.getElementsByTagName('input');
            for (var i = 0; i < checkboxCollection1.length; i++) {
                if (checkboxCollection1[i].type.toString().toLowerCase() == "checkbox") {
                    if (checkboxCollection1[i].checked == true) {
                        chkflag++;
                    }
                }
            }
            if (chkflag == 0) {
                document.getElementById('TextCompareProduct').value = "";
            }

            if (chkflag > 10) {
                alert('A maximum of 5 tariff products can be selected for comparison');
                document.getElementById(id).checked = false;
            }
        }

        function funCompareSend() {
            var chkflag = 0;
            var myURL = "FEA_repcomparison2.aspx?t=" + document.getElementById('TextCompareProduct').value;
            var z = 0;
            var div1 = document.getElementById('divresult');
            var checkboxCollection1 = div1.getElementsByTagName('input');
            for (var i = 0; i < checkboxCollection1.length; i++) {
                if (checkboxCollection1[i].type.toString().toLowerCase() == "checkbox") {
                    if (checkboxCollection1[i].checked == true) {
                        chkflag++;
                        var recno = checkboxCollection1[i].id;
                        var pos = parseInt(recno.indexOf("~"));
                        if (pos > -1) {
                            recno = recno.substr(pos + 1, recno.length);
                            myURL = myURL + "&U" + z.toString() + "=" + recno;
                            z++;
                        }

                    }
                }
            }

            if (chkflag < 2) {
                alert('Please select at least 2 tariff products for comparison');
            }
            else {
                window.open(myURL);
            }
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

        <table width="100%" cellspacing="1" cellpadding="5">
            <tr>
            <td align="center" colspan="4" class="tableheadcenter"><table width="100%"><tr><td align="right" class="tablehead" width="50%"><b>Advance Report : </b></td><td align="left" class="tableheadcenter"><b><asp:RadioButtonList ID="RadOType" Width="250px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LoadOperators" RepeatDirection="Horizontal"><asp:ListItem style="margin-right:20px; color:#ffffff;" Selected="True">TSP</asp:ListItem><asp:ListItem style="margin-right:20px; color:#ffffff;">ISP</asp:ListItem><asp:ListItem style="color:#ffffff;">Both</asp:ListItem></asp:RadioButtonList></b></td></tr></table></td>
            </tr>
            <tr>
                <td colspan="2" class="tablehead" align="left">TSP(s) / ISP(s)</td>
                <td colspan="2" class="tablehead" align="right"><asp:CheckBox ID="ChkAllOperators" runat="server" onclick="funChkAll('ChkAllOperators','ChkOper')" CssClass="input3" Text="Select All" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4"  class="tablecell3b" align="center" valign="top">
                    <asp:CheckBoxList ID="ChkOper" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" Width="100%" CssClass="chks2"></asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="tablehead" align="left">LSA(s)</td>
                <td colspan="2" class="tablehead" align="right"><asp:CheckBox ID="ChkAllCircles" runat="server" onclick="funChkAll('ChkAllCircles','ChkCirc')" CssClass="input3" Text="Select All" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4" class="tablecell3b" align="center" valign="top">
                    <asp:CheckBoxList ID="ChkCirc" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" Width="100%" CssClass="chks2"></asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="tablehead" align="left">Tariff Product Type(s)</td>
                <td colspan="2" class="tablehead" align="right"><asp:CheckBox ID="CheckAllTtypes" runat="server" onclick="funChkAll('CheckAllTtypes','ChkTtype')" CssClass="input3" Text="Select All" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4" class="tablecell3b" align="center" valign="top">
                    <asp:CheckBoxList ID="ChkTtype" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" Width="100%" CssClass="chks2"></asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="tablehead" align="center" valign="top">Numeric Parameters <a name="Bookmark1"></a></td>
            </tr>
            <tr>
                <td  colspan="4" class="tablecell3b" align="center" valign="top">
                    <table width="100%" cellspacing="1" cellpadding="5">
                        <tr>
                            <td align="left" valign="top" width="55%"><asp:DropDownList ID="DropNumeric" runat="server"></asp:DropDownList></td>
                            <td align="center" valign="top" width="10%">Between</td>
                            <td align="left" valign="top" width="25%">
                                <asp:TextBox ID="TextNumeric1" runat="server" CssClass="input" Width="55"></asp:TextBox>
                                &nbsp;&nbsp;To&nbsp;&nbsp;
                                <asp:TextBox ID="TextNumeric2" runat="server" CssClass="input" Width="55"></asp:TextBox>
                            </td>
                            <td align="center" valign="top" width="10%"><asp:Button ID="ButtonAddNumeric" runat="server" Text="Add To List" OnClick="ButtonAddNumeric_Click" /></td>
                        </tr>
                    </table>
                    <div id="divNumeric" runat="server"></div>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="tablehead" align="center" valign="top" width="50%">Text Parameters</td>
            </tr>
            <tr>
                <td  colspan="4" class="tablecell3b" align="center" valign="top">
                    <table width="100%" cellspacing="1" cellpadding="5">
                        <tr>
                            <td align="left" valign="top" width="55%"><asp:DropDownList ID="DropText" runat="server"></asp:DropDownList></td>
                            <td align="center" valign="top" width="10%">Contains</td>
                            <td align="left" valign="top" width="25%">
                                <asp:TextBox ID="TextText1" runat="server" CssClass="input" Width="150"></asp:TextBox>
                            </td>
                            <td align="center" valign="top" width="10%"><asp:Button ID="ButtonAddText" runat="server" Text="Add To List" OnClick="ButtonAddText_Click" /></td>
                        </tr>
                    </table>
                    <div id="divText" runat="server"></div>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="tablehead" align="center" valign="top" width="50%">Date Parameters</td>
            </tr>
            <tr>
                <td  colspan="4" class="tablecell3b" align="center" valign="top">
                    <table width="100%" cellspacing="1" cellpadding="5">
                        <tr>
                            <td align="left" valign="top" width="55%"><asp:DropDownList ID="DropDate" runat="server"></asp:DropDownList></td>
                            <td align="center" valign="top" width="10%">Between</td>
                            <td align="left" valign="top" width="25%">
                                <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" width="85" ></asp:TextBox>
                                &nbsp;&nbsp;To&nbsp;&nbsp;
                                <asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender"  width="85" ></asp:TextBox>
                            </td>
                            <td align="center" valign="top" width="10%"><asp:Button ID="Button2" runat="server" Text="Add To List" OnClick="ButtonAddDate_Click" /></td>
                        </tr>
                    </table>
                    <div id="divDates" runat="server"></div>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="tablehead" align="center" valign="top" width="50%">Numeric / Text / Date Condition Combination Type</td>
                <td colspan="2" class="tablehead" align="center" valign="top" width="50%">Active / Withdrawn</td>
            </tr>
            <tr>  
                <td  colspan="2" class="tablecell3b" align="center" valign="top">
                    <asp:RadioButtonList ID="RadCondition" runat="server" CssClass="input3" RepeatDirection="Vertical"><asp:ListItem Selected="True">Tariffs Matching Any One Condition</asp:ListItem><asp:ListItem>Tariffs Matching All Conditions</asp:ListItem></asp:RadioButtonList>
                </td>
                <td  colspan="2" class="tablecell3b" align="center" valign="top">
                    <asp:RadioButtonList ID="RadActive" runat="server" CssClass="input3" RepeatDirection="Horizontal"><asp:ListItem Selected="True" style="margin-right:15px;">Active</asp:ListItem><asp:ListItem style="margin-right:15px;">Withdrawn</asp:ListItem><asp:ListItem>Both</asp:ListItem></asp:RadioButtonList>
                </td>
            </tr>

            

            <tr>
                <td colspan="4" align="center"><br />
                    <asp:Button ID="Button1" runat="server" CssClass="input" OnClientClick="ShowProcessMessage('ProcessingWindow')" OnClick="Button1_Click" Text="Show Tariffs" />
                </td>
            </tr>
        </table>
    
        <br /><br />
        <div id="pagingDiv1" runat="server"></div>
        <a name="Bookmark1"></a>
        <div id="div1" runat="server">
            <div id="divresult" runat="server"></div>
        </div>       
        
        <div id="pagingDiv2" runat="server"></div>
        
        <div id="divChkResults" runat="server" style="display:none;"></div>
        <asp:TextBox ID="TextConditions" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> 
        <asp:TextBox ID="TextSortBy" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> 
        <asp:TextBox ID="TextCompareProduct" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> 
        <asp:TextBox ID="TextExcel" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> 
        <asp:Button ID="ButtonExcel" runat="server" style="display:none;" OnClick="ButtonExcel_Click" />
        <asp:Button ID="ButtonShow" runat="server" style="display:none;" OnClick="showRecords" PostBackUrl="#Bookmark1" />
        <div id="divExcel" runat="server" visible="false"></div>
        
        <asp:TextBox ID="TextDel1" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox>
        <asp:TextBox ID="TextDel2" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox>
        <asp:Button ID="ButtonDel" runat="server" style="display:none;" OnClick="ButtonDel_Click" />
        
        <asp:TextBox ID="TextPtrNumeric" runat="server" Visible="false" Text="0"></asp:TextBox>
        <asp:TextBox ID="TextPtrText" runat="server" Visible="false" Text="0"></asp:TextBox>
        <asp:TextBox ID="TextPtrDate" runat="server" Visible="false" Text="0"></asp:TextBox>
        <asp:TextBox ID="TextPage" runat="server" Visible="false"></asp:TextBox>
        <div id="divConditions" runat="server" visible="false">
            

        </div>

      </div>
    </form>
    </center>
</body>
</html>
