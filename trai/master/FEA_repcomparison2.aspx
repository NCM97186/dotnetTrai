<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_repcomparison2.aspx.cs" Inherits="FEA_repcomparison2" %>

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
                            myURL = myURL + "&R" + z.toString() + "=" + recno;
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
    
        <div id="divresults" runat="server"></div>
        <div id="divExcel" runat="server"></div>

        <asp:TextBox ID="TextSortBy" runat="server" Width="1" Height="1" CssClass="input1"></asp:TextBox>
        <asp:Button ID="ButtonExcel" runat="server" style="display:none;" OnClick="ButtonExcel_Click" />

    </div>
    </form>
    </center>
</body>
</html>
