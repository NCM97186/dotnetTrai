<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="indexm.aspx.cs" Inherits="indexm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="indexm.css" type="text/css" media="all"/>
    
    <!-- files for slider - taken from refreshless dot com/nouislider/examples/ -->
    <link href="nouislider/base.css?v=1110" rel="stylesheet">
	<link href="nouislider/prism.css" rel="stylesheet">
	<script src="nouislider/wNumb.js"></script>
	<link href="nouislider/nouislider.css?v=1110" rel="stylesheet">
	<script src="nouislider/nouislider.js?v=1110"></script>
    <!-- files for slider  - CODE ENDS HERE-->
    

    


    <!-- *******$$$$$$$********* The below line is to set page width to 100% mobile screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=0.65, maximum-scale=1" />
    
    <!-- blinking feedback image -->
    <style type='text/css'>
        #imgFeedback {
          position:absolute;
          top:140px; left:100px;
          visibility:hidden;
        }
    </style>
    <script type="text/javascript">
        var imgId = 'feedback';
        var imgOnTime = 850;
        var imgOffTime = 100;
        window.onload = function () {
            // check for existence of objects we will use
            if (document.getElementById) {
                var ele = document.getElementById(imgId);
                if (ele && ele.style) {
                    setTimeout('blinkImg()', imgOffTime);
                }
            }
        }
        function blinkImg() {
            var v, t, ele = document.getElementById(imgId);
            if (ele.style.visibility == 'visible') {
                // hide it, then wait for imgOffTime
                v = 'hidden';
                t = imgOffTime;
            }
            else {
                // show it, then wait for imgOnTime
                v = 'visible';
                t = imgOnTime;
            }
            ele.style.visibility = v;
            setTimeout('blinkImg()', t);
        }
    </script> 
    <!-- blinking feedback image - Code Ends Here -->



        
    
        <script type="text/javascript">

            function funMoreOptions() {
                var str = document.getElementById("divMoreOptions").innerHTML;
                if (str.includes("More Options")) {
                    document.getElementById("divMoreOptions").innerHTML = "<b><a href=javascript:funMoreOptions();><font style='color:#2f4da7;'>Less Options</font></a></b>";
                    if (document.getElementsByName('RadProvider')[1].checked == true) {
                        document.getElementById("divAddParamISP").style.display = "block";
                    }
                    else {
                        document.getElementById("dashed3b").style.display = "block";
                    }
                }
                else {
                    document.getElementById("divMoreOptions").innerHTML = "<b><a href=javascript:funMoreOptions();><font style='color:#2f4da7;'>More Options</font></a></b>";
                    if (document.getElementsByName('RadProvider')[1].checked == true) {
                        document.getElementById("divAddParamISP").style.display = "none";
                    }
                    else {
                        document.getElementById("dashed3b").style.display = "none";
                    }
                }
            }

            function saveSliderText() {
                if (isNaN(document.getElementById("amount2a").value) && document.getElementById("amount2a").value != "") {
                    alert('Please enter a number');
                    document.getElementById("amount2a").value = "0";
                }
                if (isNaN(document.getElementById("amount2b").value) && document.getElementById("amount2b").value != "") {
                    alert('Please enter a number');
                    document.getElementById("amount2b").value = "10000";
                }
                if (isNaN(document.getElementById("amount3a").value) && document.getElementById("amount3a").value != "") {
                    alert('Please enter a number');
                    document.getElementById("amount3a").value = "0";
                }
                if (isNaN(document.getElementById("amount3b").value) && document.getElementById("amount3b").value != "") {
                    alert('Please enter a number');
                    document.getElementById("amount3b").value = "500";
                }
                if (isNaN(document.getElementById("amount4a").value) && document.getElementById("amount4a").value != "") {
                    alert('Please enter a number');
                    document.getElementById("amount4a").value = "0";
                }
                if (isNaN(document.getElementById("amount4b").value) && document.getElementById("amount4b").value != "") {
                    alert('Please enter a number');
                    document.getElementById("amount4b").value = "365";
                }


                document.getElementById("Text2a").value = document.getElementById("amount2a").value;
                document.getElementById("Text2b").value = document.getElementById("amount2b").value;
                document.getElementById("Text3a").value = document.getElementById("amount3a").value;
                document.getElementById("Text3b").value = document.getElementById("amount3b").value;
                document.getElementById("Text4a").value = document.getElementById("amount4a").value;
                document.getElementById("Text4b").value = document.getElementById("amount4b").value;

                setDataFlag();
            }

            function transVal() {
                document.getElementById("TextValues1").value = document.getElementById("amount1a").value + " - " + document.getElementById("amount1b").value;
                document.getElementById("TextValues2").value = document.getElementById("amount2a").value + " - " + document.getElementById("amount2b").value;
                document.getElementById("TextValues3").value = document.getElementById("amount3a").value + " - " + document.getElementById("amount3b").value;
                document.getElementById("TextValues4").value = document.getElementById("amount4a").value + " - " + document.getElementById("amount4b").value;
                return false;
            }

            function showhideTSP() {
                if (document.getElementById("imgCompare").src.includes('images/iconplus.png')) {
                    document.getElementById("imgCompare").src = "../images/iconminus.png";
                    document.getElementById("dashedTSP").style.display = "block";
                    //document.getElementById("divPlans").style.minHeight="340px";
                }
                else {
                    document.getElementById("imgCompare").src = "../images/iconplus.png";
                    document.getElementById("dashedTSP").style.display = "none";
                    //document.getElementById("divPlans").style.minHeight="274px";
                }

            }


            function setChk(a) {
                var chkBox = document.getElementById('<%= ChkPlans.ClientID %>');
                var options = chkBox.getElementsByTagName('input');
                var listOfSpans = chkBox.getElementsByTagName('span');

                if (a != "All Tariffs") {
                    options[0].checked = false;
                }
                else {
                    if (options[0].checked == true) {
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

            }


            function funsort(fld, updown) {
                var sorting = ' order by ' + fld + ' ' + updown;
                document.getElementById("TextSortBy").value = sorting;
                document.getElementById("ButtonShow").click();
            }

            function funmore(divno) {
                if (document.getElementById("divmore" + divno).style.display == "none") {
                    document.getElementById("p" + divno).innerHTML = "<a href=javascript:funmore('" + divno + "') class=indexlinks1>Less...</a>";
                    document.getElementById("divmore" + divno).style.display = "block";

                }
                else {
                    document.getElementById("divmore" + divno).style.display = "none";
                    document.getElementById("p" + divno).innerHTML = "<a href=javascript:funmore('" + divno + "') class=indexlinks1>More...</a>";
                }
            }

            function funAdvance() {
                if (document.getElementById("ChkAdvance").checked == true) {
                    document.getElementById("dashed6").style.display = "block";
                    // if 'Landline' is selected, disable 'SMS' and 'Roaming Calls' in 'Advance Filter'
                    if (document.getElementsByName('RadMobile')[1].checked == true) {
                        var tbl = document.getElementById('<%= CheckAdvSMS.ClientID %>');
                        var checkboxCollection = tbl.getElementsByTagName('input');
                        for (var i = 0; i < checkboxCollection.length; i++) {
                            if (checkboxCollection[i].type.toString().toLowerCase() == "checkbox") {
                                tbl.style.color = "#afafaf";
                                checkboxCollection[i].disabled = true;
                            }
                        }
                        var tbl2 = document.getElementById('<%= CheckAdvRoaming.ClientID %>');
                        var checkboxCollection2 = tbl2.getElementsByTagName('input');
                        for (var i = 0; i < checkboxCollection2.length; i++) {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox") {
                                tbl2.style.color = "#afafaf";
                                checkboxCollection2[i].disabled = true;
                            }
                        }
                    }
                    // if 'Landline' is selected, disable 'SMS' and 'Roaming Calls' in 'Advance Filter' - CODE ENDS HERE
                }
                else {
                    document.getElementById("dashed6").style.display = "none";

                    var chkBoxList1 = document.getElementById("CheckAdvLocal");
                    var chkBoxCount = chkBoxList1.getElementsByTagName("input");
                    for (var i = 0; i < chkBoxCount.length; i++) {
                        chkBoxCount[i].checked = false;
                    }

                    var chkBoxList2 = document.getElementById("CheckAdvSTD");
                    var chkBoxCount2 = chkBoxList2.getElementsByTagName("input");
                    for (var i = 0; i < chkBoxCount2.length; i++) {
                        chkBoxCount2[i].checked = false;
                    }

                    var chkBoxList3 = document.getElementById("CheckAdvSMS");
                    var chkBoxCount3 = chkBoxList3.getElementsByTagName("input");
                    for (var i = 0; i < chkBoxCount3.length; i++) {
                        chkBoxCount3[i].checked = false;
                    }

                    var chkBoxList4 = document.getElementById("CheckAdvRoaming");
                    var chkBoxCount4 = chkBoxList4.getElementsByTagName("input");
                    for (var i = 0; i < chkBoxCount4.length; i++) {
                        chkBoxCount4[i].checked = false;
                    }
                }
            }


            function ChkDataCap() {
                var flag = 0;
                if (document.getElementById('ChkDailyDataCap1').checked == true || document.getElementById('ChkDailyDataCap2').checked == true || document.getElementById('ChkDailyDataCap3').checked == true) {
                    flag = 1;
                }

                var elementRef = document.getElementById('RadDailyDataCap');
                var inputElementArray = elementRef.getElementsByTagName('input');

                if (flag == 0) {
                    alert('Please select 2G / 3G / 4G');

                    for (var i = 0; i < inputElementArray.length; i++) {
                        var inputElement = inputElementArray[i];

                        inputElement.checked = false;
                    }

                }
            }


            function ChkDataRadio() {
                var isItemSelected = false;
                if (document.getElementById('ChkDailyDataCap1').checked == true || document.getElementById('ChkDailyDataCap2').checked == true || document.getElementById('ChkDailyDataCap3').checked == true) {
                    isItemSelected = true;
                }
                if (!isItemSelected) {

                    var elementRef = document.getElementById('RadDailyDataCap');
                    var inputElementArray = elementRef.getElementsByTagName('input');

                    for (var i = 0; i < inputElementArray.length; i++) {
                        var inputElement = inputElementArray[i];
                        inputElement.checked = false;
                    }

                }

            }



            function funOperChange() {
                var oper = document.getElementById('DropOperator').value;

                for (i = 1; i <= 3; i++) {
                    document.getElementById('ChkDataTech' + i).checked = false;
                    document.getElementById('ChkDataTech' + i).disabled = false;
                    document.getElementById('ChkDataTech' + i).parentNode.style.color = "#364daf";

                    document.getElementById('ChkDailyDataCap' + i).checked = false;
                    document.getElementById('ChkDailyDataCap' + i).disabled = false;
                    document.getElementById('ChkDailyDataCap' + i).parentNode.style.color = "#364daf";
                }

                // Disable '2G' checkboxes for operators for which service not available //

                if (oper == "Jio") {
                    document.getElementById('ChkDataTech1').parentNode.style.color = '#afafaf';
                    document.getElementById('ChkDataTech1').checked = false;
                    document.getElementById('ChkDataTech1').disabled = true;

                    document.getElementById('ChkDailyDataCap1').parentNode.style.color = '#afafaf';
                    document.getElementById('ChkDailyDataCap1').checked = false;
                    document.getElementById('ChkDailyDataCap1').disabled = true;
                }

                // Disable '2G' checkboxes for operators for which service not available - CODE ENDS HERE //

                // Disable '3G' checkboxes for operators for which service not available //

                if (oper == "Jio") {
                    document.getElementById('ChkDataTech2').parentNode.style.color = '#afafaf';
                    document.getElementById('ChkDataTech2').checked = false;
                    document.getElementById('ChkDataTech2').disabled = true;

                    document.getElementById('ChkDailyDataCap2').parentNode.style.color = '#afafaf';
                    document.getElementById('ChkDailyDataCap2').checked = false;
                    document.getElementById('ChkDailyDataCap2').disabled = true;
                }

                // Disable '3G' checkboxes for operators for which service not available - CODE ENDS HERE //

                // Disable '4G' checkboxes for operators for which service not available //

                if (oper == "Aircel" || oper == "BSNL" || oper == "Tata Tele" || oper == "MTNL") {
                    document.getElementById('ChkDataTech3').parentNode.style.color = '#afafaf';
                    document.getElementById('ChkDataTech3').checked = false;
                    document.getElementById('ChkDataTech3').disabled = true;

                    document.getElementById('ChkDailyDataCap3').parentNode.style.color = '#afafaf';
                    document.getElementById('ChkDailyDataCap3').checked = false;
                    document.getElementById('ChkDailyDataCap3').disabled = true;
                }
                // Disable '4G' checkboxes for operators for which service not available - CODE ENDS HERE //
            }

            function setManualValues() {
                if (document.getElementById('Text2a').value != '') {
                    document.getElementById('amount2a').value = document.getElementById('Text2a').value;
                    document.getElementById('amount2b').value = document.getElementById('Text2b').value;
                    document.getElementById('amount3a').value = document.getElementById('Text3a').value;
                    document.getElementById('amount3b').value = document.getElementById('Text3b').value;
                    document.getElementById('amount4a').value = document.getElementById('Text4a').value;
                    document.getElementById('amount4b').value = document.getElementById('Text4b').value;


                }
            }


            function funExcel() {
                document.getElementById('ButtonExcel').click();
            }

            function funXML() {
                document.getElementById('ButtonXML').click();
            }


            function setDataFlag() {
                document.getElementById('TextDataTechFlag').value = 0;
            }

            function funDisableFilters() {  // function to disable all filters if any of 'ISD Pack' / 'International Roaming' / 'National Roaming' is selected
                if (document.getElementById('ChkISDPack').checked == true || document.getElementById('ChkISDRoaming').checked == true || document.getElementById('ChkNatRoaming').checked == true) {
                    var div1 = document.getElementById('dashed3a');
                    var checkboxCollection1 = div1.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection1.length; i++) {
                        if (checkboxCollection1[i].id != "ChkISDPack" && checkboxCollection1[i].id != "ChkISDRoaming" && checkboxCollection1[i].id != "ChkNatRoaming") {
                            if (checkboxCollection1[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection1[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection1[i].disabled = true;
                                checkboxCollection1[i].checked = false;
                            }
                        }
                    }

                    var div2 = document.getElementById('dashed3b');
                    var checkboxCollection2 = div2.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = true;
                                checkboxCollection2[i].checked = false;
                            }
                        }
                    }

                }

                if (document.getElementById('ChkISDPack').checked == false && document.getElementById('ChkISDRoaming').checked == false && document.getElementById('ChkNatRoaming').checked == false) {
                    var div1 = document.getElementById('dashed3a');
                    var checkboxCollection1 = div1.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection1.length; i++) {
                        if (checkboxCollection1[i].id != "ChkISDPack" && checkboxCollection1[i].id != "ChkISDRoaming" && checkboxCollection1[i].id != "ChkNatRoaming") {
                            if (checkboxCollection1[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection1[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection1[i].disabled = false;
                            }
                        }
                    }

                    var div2 = document.getElementById('dashed3b');
                    var checkboxCollection2 = div2.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = false;
                            }
                        }
                    }

                }

            }


            function funComparison(ttype, id) {
                if (document.getElementById(id).checked == true) {
                    if (document.getElementById('TextCompareProduct').value == "") {
                        document.getElementById('TextCompareProduct').value = ttype;
                    }
                    else {
                        if (document.getElementById('TextCompareProduct').value != ttype) {
                            alert('Comparison is possible for same product only.')
                            document.getElementById(id).checked = false;
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

                if (chkflag > 5) {
                    alert('A maximum of 5 tariff products can be selected for comparison');
                    document.getElementById(id).checked = false;
                }
            }


            function funSetDataValues() {
                if (document.getElementById('ChkDataTech1').checked == false && document.getElementById('ChkDataTech2').checked == false && document.getElementById('ChkDataTech3').checked == false) {
                    document.getElementById('amount3a').value = 0;
                    document.getElementById('amount3b').value = 500;
                    document.getElementById('TextDataTechFlag').value = "0";
                }
            }



            function funCompareSend() {
                var chkflag = 0;
                //var myURL="../comparisonview.aspx?t=" + document.getElementById('TextCompareProduct').value;
                if (document.getElementsByName('RadProvider')[1].checked == true) {
                    var myURL = "../comparisonview.aspx?p=ISP&t=" + document.getElementById('TextCompareProduct').value;
                }
                else {
                    var myURL = "../comparisonview.aspx?p=TSP&t=" + document.getElementById('TextCompareProduct').value;
                }
                var z = 0;
                var div1 = document.getElementById('divresults');
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
    
        
    <script src="../calendar/javascript/datepicker.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="../calendar/stylesheets/datepicker.css" />
     
</head>
<body style="background-image:url('../images/bg4.jpg');">
    <form id="form1" runat="server">
    
        <center>


            
            
        <asp:TextBox ID="Text2a" runat="server" Text="0" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text2b" runat="server" Text="10000" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text3a" runat="server" Text="0" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text3b" runat="server" Text="500" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text4a" runat="server" Text="0" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text4b" runat="server" Text="365" style="display:none;"></asp:TextBox>








            <div style="width:98%; text-align:center; background-color:#ffffff;">

                <div style="width:100%; min-height:50px; margin:auto;">
                    <div style="float: left;margin:0px;margin-left:50px;"><img src="../images/TRAI_logo.png" alt="TRAI" title="TRAI" width="60px" border="0" /></div>
                    <div style="float: right;margin:0px; margin-right:50px; margin-top:20px;"><font style="font-family:'Trebuchet MS'; font-size:15px;color:#053e7e;"><b>TARIFF</b></font></div>
                </div>
                

                <div style="background-image:url('../images/bg5.jpg');min-height:30px; text-align:center; padding-top:1px; border-radius:0px; margin-top:1px;">
                    <div class="wrapper">
                        <div style="width:100%; margin: auto; margin-top:-20px;">
                            <div style="float: left;text-align:center; width:95%;">
                                <center>
                                <asp:RadioButtonList ID="RadProvider" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LoadData" RepeatDirection="Horizontal" CssClass="radios" Width="180px"><asp:ListItem class="Redbutton" Selected="True" style="margin-right:10px;">TSP</asp:ListItem><asp:ListItem class="Redbutton">ISP</asp:ListItem></asp:RadioButtonList>
                                </center>
                            </div>
                        </div>
                    </div>
                </div>

                <div style="background-image:url('../images/bg5.jpg');min-height:30px; text-align:center; padding-top:1px; border-radius:0px; margin-top:1px;">
        
                    <div class="wrapper">
                        <div style="width:100%; margin: auto; margin-top:-20px;">
                            <div style="float: left;text-align:center; width:2%;">
                                
                            </div>
                            <div style="float: left;text-align:center; width:36%;">
                                <center>
                                <asp:RadioButtonList ID="RadMobile" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LoadData" RepeatDirection="Horizontal" CssClass="radios" Width="180px"><asp:ListItem class="Redbutton" Selected="True" style="margin-right:10px;">Mobile</asp:ListItem><asp:ListItem class="Redbutton">Landline</asp:ListItem></asp:RadioButtonList>
                                </center>
                            </div>
                            <div id="divSep1" runat="server" style="float:left; text-align:center; width:1%;"><font color="#48baff" style="font-size:22px;font-family:Arial;">|</font></div>
                            <div style="float:left; text-align:right; width:36%;">
                                <center>
                                <asp:RadioButtonList ID="RadPrePost" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LoadData" RepeatDirection="Horizontal" CssClass="radios" Width="180px"><asp:ListItem class="Redbutton" Selected="True" style="margin-right:10px;">Prepaid</asp:ListItem><asp:ListItem class="Redbutton">Postpaid</asp:ListItem></asp:RadioButtonList>
                                </center>
                            </div>
                            <div style="float:right ;text-align:center; width:2%; padding:0px; margin-bottom:-25px;">
                                
                            </div>
                        </div>
                        <div style="width:100%; margin: auto; overflow:auto;">
                            <div style="float: left;text-align:center; width:2%; margin-top:-10px;"></div>
                            <div style="float: left;text-align:center; width:36%; margin-top:-10px;">
                                <center>
                                <asp:DropDownList ID="DropCircle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LoadData" style="min-width:150px; margin-left:-20px;" BackColor="#ffffff" ForeColor="#4a4a4a" Font-Names="Verdana" CssClass="ddl">
                                    <asp:ListItem Text="CIRCLE"></asp:ListItem>
                                </asp:DropDownList>
                                </center>
                            </div>
                            <div style="float:left; text-align:center; width:1%; margin-top:-10px;"><font color="#48baff" style="font-size:22px;font-family:Arial;">|</font></div>
                            <div style="float:left; text-align:center; width:36%; margin-top:-10px;">
                                <center>
                                <asp:DropDownList ID="DropOperator" runat="server" onchange="funOperChange()" style="min-width:150px; margin-left:0px;" BackColor="#ffffff" ForeColor="#4a4a4a" Font-Names="Verdana" CssClass="ddl">
                                    <asp:ListItem Text="OPERATOR"></asp:ListItem>
                                </asp:DropDownList>
                                </center>
                            </div>
                            <div style="float:left ;text-align:center; width:2%; margin-top:-10px;"><a href="javascript:showhideTSP();"><img src="../images/iconplus.png" id="imgCompare" alt="Compare Tariff Products" title="Compare Tariff Products" border="0" /></a></div>
                        </div>
                     </div>

                </div>



                <div style="overflow:hidden; margin-bottom:0px;">
                    
                    
                    
                    <div style="width:100%;">
                        <div id="dashedTSP" style="display:none; width:98%; min-height:50px;">
                            <table width="100%" cellspacing="0" border="0" cellpadding="0">
                            <tr>
                            <td align="center" class="tablecell" valign="top" id="oper1" runat="server" width="50%">
                             
                            </td>
                            <td align="center" class="tablecell" valign="top" id="oper2" runat="server">
                             
                            </td>
                            <td align="center" class="tablecell" valign="top" id="oper3" runat="server">
                             
                            </td>
                            <td align="center" class="tablecell" valign="top" id="oper4" runat="server">
                             
                            </td>
                            <td align="center" class="tablecell" valign="top" id="oper5" runat="server">
                             
                            </td>
                            <td align="center" class="tablecell" valign="top" id="oper6" runat="server">
                             
                            </td>
                            </tr>
                            </table>
                        </div>

                        <div id="dashed3a">
                            <div id="divPlans" runat="server" align="center">
                                <asp:CheckBoxList ID="ChkPlans" runat="server" CssClass="chks" RepeatColumns="3" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="true">All Tariffs</asp:ListItem>
                                    <asp:ListItem>Plan Voucher</asp:ListItem>
                                    <asp:ListItem>STV</asp:ListItem>
                                    <asp:ListItem>Combo</asp:ListItem>
                                    <asp:ListItem>Top Up</asp:ListItem>
                                    <asp:ListItem>SUK</asp:ListItem>
                                </asp:CheckBoxList>
                                
                            </div>

                            <div id="dashed1f" style="text-align:left;">
                            <div align="center" style="margin-top:12px; font-size:17px;">
                            <p style="text-align:left; margin-top:-10px;">&nbsp;<asp:Label id="LblPrice" runat="server" class="yellowUnderline" Text="Price &#8377; (Optional)"></asp:Label></p>
                                
                                <!-- Slider for Price Range -->
                                <div>
                                    <div class="example">
			                            <div id="html5a"></div>
                                        Min&nbsp;<asp:TextBox ID="amount2a" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:70px; height:30px; border-radius:0px; margin-top:10px; color:#364daf; font-weight:bold; text-align:center; font-size:16px;' onblur="saveSliderText()"></asp:TextBox>
                                        &nbsp;to&nbsp;
                                        <asp:TextBox ID="amount2b" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:70px; height:30px; border-radius:0px; margin-top:10px; color:#364daf; font-weight:bold; text-align:center; font-size:16px;' onblur="saveSliderText()"></asp:TextBox>&nbsp;Max

                                             <script>
                                                 var html5aSlider = document.getElementById('html5a');

                                                 var min1 = 0;
                                                 var min2 = 10000;
                                                 if (!isNaN(document.getElementById('Text2a').value) && document.getElementById('Text2a').value != "") {
                                                     var min1 = parseInt(document.getElementById('Text2a').value);
                                                 }
                                                 if (!isNaN(document.getElementById('Text2b').value) && document.getElementById('Text2b').value != "") {
                                                     var min2 = parseInt(document.getElementById('Text2b').value);
                                                 }

                                                 noUiSlider.create(html5aSlider, {

                                                     start: [min1, min2],
                                                     connect: true,
                                                     range: {
                                                         'min': 00,
                                                         'max': 10000
                                                     }
                                                 });
                                                </script>			
                                                <script>
                                                    var inputNumber = document.getElementById('amount2a');
                                                    var inputNumber2 = document.getElementById('amount2b');
                                                    html5aSlider.noUiSlider.on('update', function (values, handle) {

                                                        //var value = values[handle];
                                                        var value = parseInt(values[handle]);

                                                        if (handle) {
                                                            inputNumber2.value = value;
                                                        } else {
                                                            //select.value = Math.round(value);
                                                            inputNumber.value = value;
                                                        }
                                                    });


                                                    /*
                                                    select.addEventListener('change', function(){
                                                        //html5aSlider.noUiSlider.set([this.value, null]);   // remove comments to change slider value on text box change
                                                    });
                                                    */

                                                    inputNumber.addEventListener('change', function () {
                                                        //html5aSlider.noUiSlider.set([this.value, null]);    // remove comments to change slider value on text box change
                                                    });
                                                    inputNumber2.addEventListener('change', function () {
                                                        //html5aSlider.noUiSlider.set([null, this.value]);    // remove comments to change slider value on text box change
                                                    });

                                                </script>		
                                                
                                            </div>
	                                    </div>
                                <!-- Slider for Price Range - CODE ENDS HERE -->

                                </div>
                            </div>
                            <div id="dashed1g" style="text-align:left;">
                                
                                <div id="divDataTech" style="min-height:25px; margin-top:5px; text-align:center; font-size:17px;">
                                    <p style="text-align:left; margin-top:-5px;">&nbsp;<font class="yellowUnderline">Total Data Capping (Optional)</font></p>
                                    <p style="text-align:center; margin-top:-5px;">&nbsp;
                                    <asp:CheckBox ID="ChkCapUnlim" runat="server" Text="Unlimited" CssClass="input3" style="font-size:15px;" />
                                    </p>
                                    <asp:CheckBox ID="ChkDataTech1" runat="server" CssClass="input3" style="margin-top:3px; font-size:15px;" Text="2G Data" onclick="funSetDataValues()" />
                                    &nbsp;
                                    <asp:CheckBox ID="ChkDataTech2" runat="server" CssClass="input3" style="margin-top:3px; font-size:15px;" Text="3G Data" onclick="funSetDataValues()" />
                                    &nbsp;
                                    <asp:CheckBox ID="ChkDataTech3" runat="server" CssClass="input3" style="margin-top:3px; font-size:15px;" Text="4G Data" onclick="funSetDataValues()" />
                                </div>
                                <center>
                                <div align="center" style="margin-top:5px; font-size:17px;">
                                    
                                <!-- Slider for Total Data Capping -->
                                <div>
                                    <div class="example">
			                            <div id="html5b"></div>
                                        <asp:TextBox ID="amount3a" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:70px; height:30px; border-radius:0px; margin-top:10px; color:#364daf; font-weight:bold; text-align:center; font-size:16px;' onblur="saveSliderText()"></asp:TextBox>&nbsp;GB
                                        &nbsp;to&nbsp;
                                        <asp:TextBox ID="amount3b" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:70px; height:30px; border-radius:0px; margin-top:10px; color:#364daf; font-weight:bold; text-align:center; font-size:16px;' onblur="saveSliderText()"></asp:TextBox>&nbsp;GB
                                    
                                             <script>
                                                 var html5bSlider = document.getElementById('html5b');

                                                 var min3 = 0;
                                                 var min4 = 500;
                                                 if (!isNaN(document.getElementById('Text3a').value) && document.getElementById('Text3a').value != "") {
                                                     var min3 = parseInt(document.getElementById('Text3a').value);
                                                 }
                                                 if (!isNaN(document.getElementById('Text3b').value) && document.getElementById('Text3b').value != "") {
                                                     var min4 = parseInt(document.getElementById('Text3b').value);
                                                 }

                                                 noUiSlider.create(html5bSlider, {
                                                     start: [min3, min4],
                                                     connect: true,
                                                     range: {
                                                         'min': 0,
                                                         'max': 500
                                                     }
                                                 });
                                                </script>			
                                                <script>
                                                    var inputNumbe3 = document.getElementById('amount3a');
                                                    var inputNumber4 = document.getElementById('amount3b');
                                                    html5bSlider.noUiSlider.on('update', function (values, handle) {

                                                        //var value = values[handle];
                                                        var value3 = parseInt(values[handle]);

                                                        if (handle) {
                                                            inputNumber4.value = value3;
                                                        } else {
                                                            //select.value = Math.round(value);
                                                            inputNumbe3.value = value3;
                                                        }



                                                        if ((document.getElementById('amount3a').value != '' && document.getElementById('amount3a').value > 0) || (document.getElementById('amount3b').value != '' && document.getElementById('amount3b').value < 500)) {
                                                            if (document.getElementById('TextDataTechFlag').value == 0) { // so that alert is shown only once.
                                                                if (document.getElementById('ChkDataTech1').checked == false && document.getElementById('ChkDataTech2').checked == false && document.getElementById('ChkDataTech3').checked == false) {
                                                                    alert('Please select 2G / 3G / 4G Data first.');
                                                                    document.getElementById('TextDataTechFlag').value = '1';
                                                                    document.getElementById('amount3a').focus();
                                                                }
                                                            }
                                                        }

                                                    });

                                                    /*
                                                    select.addEventListener('change', function(){
                                                        //html5aSlider.noUiSlider.set([this.value, null]);   // remove comments to change slider value on text box change
                                                    });
                                                    */

                                                    inputNumber3.addEventListener('change', function () {
                                                        //html5aSlider.noUiSlider.set([this.value, null]);    // remove comments to change slider value on text box change
                                                    });
                                                    inputNumber4.addEventListener('change', function () {
                                                        //html5aSlider.noUiSlider.set([null, this.value]);    // remove comments to change slider value on text box change
                                                    });

                                                    setManualValues();  // so that slider and box values don't get set to default on page reload

                                                </script>		
                                            </div>
	                                    </div>
                                <!-- Slider for Total Data Capping - CODE ENDS HERE -->

                                    
                                    </div>
                                </center>
                            </div>
                            <div id="dashed2" runat="server" style="text-align:left;">
                                <div align="left" id="divValidity" runat="server" style="font-family:Verdana;color:#364daf;font-size:13px;">
                                    
                                    <div  style="margin-top:7px; margin-left:10px; text-align:center; font-size:17px;">
                                    <p style="text-align:left; margin-top:-5px;"><font class="yellowUnderline">Validity Days (Optional)</font></p>
                                        <asp:CheckBox ID="ChkValidityMore" runat="server" Text="More Than 365 Days" CssClass="input3" style="font-size:15px;" />
                                    </div>
                                    <div align="center" style="margin-top:5px; font-size:17px;">
                                                                                    
                                        <!-- Slider for Validity -->
                                        <div>
                                            <div class="example">
			                                    <div id="html5c"></div>
                                                Min&nbsp;<asp:TextBox ID="amount4a" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:70px; height:30px; border-radius:0px; margin-top:10px; color:#364daf; font-weight:bold; text-align:center; font-size:16px;' onblur="saveSliderText()"></asp:TextBox>
                                                &nbsp;to&nbsp;
                                                <asp:TextBox ID="amount4b" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:70px; height:30px; border-radius:0px; margin-top:10px; color:#364daf; font-weight:bold; text-align:center; font-size:16px;' onblur="saveSliderText()"></asp:TextBox>&nbsp;Max
                                        
                                                     <script>
                                                         var html5cSlider = document.getElementById('html5c');

                                                         var min5 = 0;
                                                         var min6 = 365;
                                                         if (!isNaN(document.getElementById('Text4a').value) && document.getElementById('Text4a').value != "") {
                                                             var min5 = parseInt(document.getElementById('Text4a').value);
                                                         }
                                                         if (!isNaN(document.getElementById('Text4b').value) && document.getElementById('Text4b').value != "") {
                                                             var min6 = parseInt(document.getElementById('Text4b').value);
                                                         }

                                                         noUiSlider.create(html5cSlider, {
                                                             start: [min5, min6],
                                                             connect: true,
                                                             range: {
                                                                 'min': 0,
                                                                 'max': 365
                                                             }
                                                         });
                                                        </script>			
                                                        <script>
                                                            var inputNumbe5 = document.getElementById('amount4a');
                                                            var inputNumber6 = document.getElementById('amount4b');
                                                            html5cSlider.noUiSlider.on('update', function (values, handle) {

                                                                //var value = values[handle];
                                                                var value5 = parseInt(values[handle]);

                                                                if (handle) {
                                                                    inputNumber6.value = value5;
                                                                } else {
                                                                    //select.value = Math.round(value);
                                                                    inputNumbe5.value = value5;
                                                                }
                                                            });

                                                            /*
                                                            select.addEventListener('change', function(){
                                                                //html5aSlider.noUiSlider.set([this.value, null]);   // remove comments to change slider value on text box change
                                                            });
                                                            */

                                                            inputNumber5.addEventListener('change', function () {
                                                                //html5aSlider.noUiSlider.set([this.value, null]);    // remove comments to change slider value on text box change
                                                            });
                                                            inputNumber6.addEventListener('change', function () {
                                                                //html5aSlider.noUiSlider.set([null, this.value]);    // remove comments to change slider value on text box change
                                                            });

                                                            setManualValues();  // so that slider and box values don't get set to default on page reload

                                                        </script>		
                                                    </div>
	                                            </div>
                                        <!-- Slider for Validity - CODE ENDS HERE -->


                                    </div>
                                </div>
                            </div>
                        </div>


                        <div id="divMoreOptions" runat="server" style="text-align:left;font-family:Verdana;color:#2f4da7;font-size:13px; margin-left:5px; margin-top:10px; margin-bottom:30px;"><b><a href="javascript:funMoreOptions();"><font style='color:#2f4da7;'>More Options</font></a></b></div>


                        <div id="dashed3b" style="display:none;">
                            <div id="dashed1b" style="text-align:left;">
                                <font class="yellowUnderline">Unlimited Calls : </font>
                                <p style="height:0px; margin-bottom:-5px;"></p>
                                <center>
                                <asp:CheckBox ID="ChkUnlim_Local" runat="server" Text="Local" CssClass="input3" />
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="ChkUnlim_STD" runat="server" Text="STD" CssClass="input3" />
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="ChkUnlim_Roaming" runat="server" Text="Roaming" CssClass="input3" />
                                </center>
                            </div>
                            <div id="dashed1c" runat="server" style="text-align:left;">
                                <div id="divDailyDataCapping" runat="server" style="font-family:Verdana;color:#364daf;font-size:13px;">
                                <font class="yellowUnderline">Daily Data Capping :</font>
                                <p style="height:0px; margin-bottom:-5px;"></p>
                                <center>
                                <asp:CheckBox ID="ChkDailyDataCap1" runat="server" onclick="ChkDataRadio()" CssClass="input3" style="margin-top:3px; margin-left:0px;" Text="2G Data" />
                                &nbsp;
                                <asp:CheckBox ID="ChkDailyDataCap2" runat="server" onclick="ChkDataRadio()" CssClass="input3" style="margin-top:3px; margin-left:0px;" Text="3G Data" />
                                &nbsp;
                                <asp:CheckBox ID="ChkDailyDataCap3" runat="server" onclick="ChkDataRadio()" CssClass="input3" style="margin-top:3px; margin-left:0px;" Text="4G Data" />
                                <p style="margin-top:0px; margin-bottom:-3px; font-size:10px; color:#7a7a7a; text-align:center">-----------------------------</p>
                                <asp:RadioButtonList ID="RadDailyDataCap" runat="server" CssClass="input3" style="margin-top:-5px; font-size:17px;" onchange="ChkDataCap()" RepeatDirection="Horizontal">
                                    <asp:ListItem style="margin-right:30px;">< 1 GB</asp:ListItem>
                                    <asp:ListItem style="margin-right:13px;">1 to 2 GB</asp:ListItem>
                                    <asp:ListItem style="margin-right:20px;">> 2 GB</asp:ListItem>
                                </asp:RadioButtonList>
                                </center>
                                </div>
                            </div>
                            <div id="dashed2b" runat="server" style="text-align:left;">
                                <div align="left" id="divTalktime" runat="server" style="font-family:Verdana;color:#364daf;font-size:13px;">
                                    <font class="yellowUnderline">Talktime :</font>
                                    <center>
                                    <asp:CheckBox ID="ChkFullTalktime" runat="server" Text="Full Talktime & More" CssClass="input3" />
                                    </center>
                                </div>
                            </div>

                            <div id="dashed2c" style="text-align:left;">
                                <div id="divRoaming" runat="server" style="width:100%;font-family:Verdana;color:#364daf;font-size:13px;">
                                <font class="yellowUnderline">Other Packs : </font>
                                <p style="height:0px; margin-bottom:-5px;"></p>
                                <center>
                                <asp:CheckBox ID="ChkISDPack" runat="server" Text="ISD" CssClass="input3" onClick="funDisableFilters()" />
                                <asp:CheckBox ID="ChkISDRoaming" runat="server" Text="Int. Roaming" CssClass="input3" onClick="funDisableFilters()" />
                                <asp:CheckBox ID="ChkNatRoaming" runat="server" Text="Nat. Roaming" CssClass="input3" onClick="funDisableFilters()" />
                                </center>
                                </div>
                            </div>

                    </div>



                        
                            <div style="width:100%; margin: auto;" id="divAdvance" runat="server">
                                <div style="float: left;text-align:left; width:200px;">
                                    <b><font style="font-size:12px; padding-right:7px;"><asp:CheckBox ID="ChkAdvance" runat="server" CssClass="input3" style="padding-top:2px; font-size:15px;" Text="Advance Filters" onclick="funAdvance()" /></font></b>
                                </div>
                                <div style="float: right;text-align:right; margin-right:20px;">
                                    <asp:ImageButton ID="ButtonClearFilters" runat="server" style="padding-top:2px;" OnClick="ButtonClearFilters_Click" ImageUrl="../images/imgclearfilters.jpg" />
                                </div>
                            </div>

                       
                             <div id="divAddParamISP" style="margin-top:10px;display:none;"> 
                                <div id="dashed1b" style="text-align:left;">
                                    <font class="yellowUnderline">Date of Launch</font>
                                    <p style="height:0px; margin-bottom:-5px;"></p>
                                    <center>
                                    <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" width="85"  CssClass="input"></asp:TextBox>
                                    &nbsp;&nbsp;To&nbsp;&nbsp;
                                    <asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender" width="85"  CssClass="input"></asp:TextBox>
                                    </center>
                                </div>
                                <div id="dashed1b" style="text-align:left;">
                                    <font class="yellowUnderline">Service Area / SSA</font>
                                    <p style="height:0px; margin-bottom:-5px;"></p>
                                    <center>
                                    <asp:TextBox ID="TextISPSSA" runat="server" width="200"  CssClass="input"></asp:TextBox>
                                    </center>
                                </div>
                                <div id="dashed1b" style="text-align:left;">
                                    <font class="yellowUnderline">FUP</font>
                                    <p style="height:0px; margin-bottom:-5px;"></p>
                                    <center>
                                    <asp:CheckBox ID="ChkISPFUP" runat="server" Text="Show FUP Tariffs" />
                                    </center>
                                </div>
                                <div id="dashed1b" style="text-align:left;">
                                    <font class="yellowUnderline">Data Usage Limit (GB)</font>
                                    <p style="height:0px; margin-bottom:-5px;"></p>
                                    <center>
                                    <asp:TextBox ID="TextISPDataUsage1" runat="server" width="60"  CssClass="input"></asp:TextBox>
                                    &nbsp;&nbsp;To&nbsp;&nbsp;
                                    <asp:TextBox ID="TextISPDataUsage2" runat="server" width="60"  CssClass="input"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="ChkISPDataUnlim" runat="server" Text="Unlimited" />
                                    </center>
                                </div>
                                <div id="dashed1b" style="text-align:left; padding-bottom:20px;">
                                    <font class="yellowUnderline">If Promotional Offer, valid till</font>
                                    <p style="height:0px; margin-bottom:-5px;"></p>
                                    <center>
                                    <asp:TextBox ID="TextDate3" MaxLength="30" runat="server" OnPreRender="TextDate3_PreRender" width="85"  CssClass="input"></asp:TextBox>
                                    </center>
                                </div>
                            </div>
                        </div>



                        
                    </div>


                </div>
                

                
                <div style="text-align:left;">
            
                <div id="dashed4" align="left" style="min-height:30px;">
                    <div id="divAdvance2" runat="server">
                        <div id="dashed6" style="display:none;">
                            <div class="div50per">
                            &nbsp;&nbsp;<font class="yellowUnderline"><b>Local Calls:</b></font><br />
                            <asp:CheckBoxList ID="CheckAdvLocal" runat="server" CssClass="input3">
                                <asp:ListItem Value="local_on_voice_peak">Mobile On Net - Peak</asp:ListItem>
                                <asp:ListItem Value="local_on_voice_offpeak">Mobile On Net - Off Peak</asp:ListItem>
                                <asp:ListItem Value="local_off_voice_peak">Mobile Off Net - Peak</asp:ListItem>
                                <asp:ListItem Value="local_off_voice_offpeak">Mobile Off Net - Off Peak</asp:ListItem>
                                <asp:ListItem Value="local_fix_on_voice_peak">Landline On Net - Peak</asp:ListItem>
                                <asp:ListItem Value="local_fix_on_voice_offpeak">Landline On Net - Off Peak</asp:ListItem>
                                <asp:ListItem Value="local_fix_off_voice_peak">Landline Off Net - Peak</asp:ListItem>
                                <asp:ListItem Value="local_fix_off_voice_offpeak">Landline Off Net - Off Peak</asp:ListItem>
                            </asp:CheckBoxList>
                            <br /><br />
                            &nbsp;&nbsp;<font class="yellowUnderline"><b>SMS :</b></font><br />
                            <asp:CheckBoxList ID="CheckAdvSMS" runat="server" CssClass="input3">
                                <asp:ListItem Value="sms_local_on">Local On Net</asp:ListItem>
                                <asp:ListItem Value="sms_local_off">Local Off Net</asp:ListItem>
                                <asp:ListItem Value="sms_nat_on">National On Net</asp:ListItem>
                                <asp:ListItem Value="sms_nat_off">National Off Net</asp:ListItem>
                                <asp:ListItem Value="sms_int">International</asp:ListItem>
                            </asp:CheckBoxList>
                            </div>
                            <div class="div50perB">
                                &nbsp;&nbsp;<font class="yellowUnderline"><b>STD Calls :</b></font><br />
                                <asp:CheckBoxList ID="CheckAdvSTD" runat="server" CssClass="input3">
                                    <asp:ListItem Value="std_on_voice_peak">Mobile On Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="std_on_voice_offpeak">Mobile On Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="std_off_voice_peak">Mobile Off Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="std_off_voice_offpeak">Mobile Off Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="std_fix_on_voice_peak">Landline On Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="std_fix_on_voice_offpeak">Landline On Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="std_fix_off_voice_peak">Landline Off Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="std_fix_off_voice_offpeak">Landline Off Net - Off Peak</asp:ListItem>
                                </asp:CheckBoxList>
                                <br /><br />
                                &nbsp;&nbsp;<font class="yellowUnderline"><b>Roaming Calls :</b></font><br />
                                <asp:CheckBoxList ID="CheckAdvRoaming" runat="server" CssClass="input3">
                                    <asp:ListItem Value="roam_call_voice_out">Local Outgoing</asp:ListItem>
                                    <asp:ListItem Value="roam_call_voice_std">STD Outgoing</asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                        </div>
                </div>


                
                <div style="width:100%; float:left;">
                    <p style="text-align:center; width:100%; margin-top:-20px;">
                        <asp:ImageButton ID="ImagePopUp" runat="server" OnClick="ButtonPopUp_Click" OnClientClick="javascript:saveSliderText();" ImageUrl="../images/submit4.jpg" />
                    </p>
                </div>

                    

                </div>



                <a name="Bookmark1"></a>
        
                <div id="divheaders" runat="server"></div>
                <div id="divresults" runat="server" style="max-height:500px; overflow-y:auto;"></div>
                





        <!-- below three fields are for storing the parameters for sliders 2, 3 and 4 -->
        <asp:TextBox ID="HiddenQry2" runat="server" Visible="false" Text="" />
        <asp:TextBox ID="HiddenQry3" runat="server" Visible="false" Text="" />
        <asp:TextBox ID="HiddenQry4" runat="server" Visible="false" Text="" />
        <asp:TextBox ID="HiddenQry5" runat="server" Visible="false" Text="" />

                <p align="right">
                    <asp:TextBox ID="TextPrice1" runat="server" CssClass="input1" Width="1" Height="1"></asp:TextBox>
                    <asp:TextBox ID="TextPrice2" runat="server" CssClass="input1" Width="1" Height="1"></asp:TextBox>

                    <asp:TextBox ID="TextISDHidden" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox>
                    <!-- When a tariff type generates a valid preview, its name is added to the text box below. Then, when the submit button is pressed, it checks whether all visible tariff types have been properly populated. -->
                    <asp:TextBox ID="TextTariffEntered" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox>
                    <asp:TextBox ID="TextTariffSelected" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox><!-- This text box stores id's of all the selected tariff type checkboxes -->
                    <asp:TextBox ID="TextTariffSelectedCount" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox><!-- This text box stores 1 if additional tariff type selected, else stores 0 -->
                    <asp:TextBox ID="TextTariffNoShow" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> <!-- This text box is used to store the ids of the unchecked tariff types from 'Add Pack' option, so that when a default tariff type of a selected plan is unchecked, it does not get auto selected again through 'showextratariffs'  -->
                    
                    <asp:TextBox ID="TextResultCntr" CssClass="input1" runat="server" Width="1" Height="1" value="0"></asp:TextBox> <!-- This text box is used to store the no. of results (for generating checkboxes for each result entry)  -->
                    <div id="divChkResults" runat="server" style="display:none;"></div>
                    <asp:TextBox ID="TextCompareProduct" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> <!-- This text box is used to check whether all selected comparison checkboxes belong to the same product or not  -->
                    <asp:TextBox ID="TextDataTechFlag" CssClass="input1" runat="server" Width="1" Height="1" Text="0"></asp:TextBox> <!-- This text box is used so that the 'Please select 2G / 3G / 4G Data first.' is shown only once  -->

                    <asp:TextBox ID="TextSortBy" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> 
                    <asp:TextBox ID="TextConditions" CssClass="input1" runat="server" Width="1" Height="1"></asp:TextBox> 
                    <asp:Button ID="ButtonShow" runat="server" style="display:none;" OnClick="showRecords" PostBackUrl="#Bookmark1" />
                    <asp:Button ID="ButtonExcel" runat="server" style="display:none;" OnClick="ButtonExcel_Click" />
                    <asp:Button ID="ButtonXML" runat="server" style="display:none;" OnClick="ButtonXML_Click" />
                    
                </p>





                <div id="divPopUp" runat="server" style="position:fixed; top:50px;left:40px;border:1px solid; z-index:555;" visible="false">
                    <div id="divPopShadow" runat="server" style="box-shadow: 7px 10px #888888; min-height:300px; z-index:556;" visible="false">
                        <div id="divPopUpBg" runat="server" style="z-index:557;">
                            <div id="divSelection" runat="server" style="padding:5px;"></div>
                            <div style="text-align:center;">
                                <asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" PostBackUrl="#Bookmark1" ImageUrl="../images/btnconfirm.jpg" Visible="false" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Click" ImageUrl="../images/btncancel.jpg" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
                


                
                <!--
                    <div style="overflow:auto; width:100%; background-color:#48baff;">
                        <div style="float:left; vertical-align:middle;"><a href="../consumerview_userguide.pdf" target="_blank"><img src="../images/pdficon.png" alt="User Guide" title="User Guide" border="0" style="margin-bottom:5px;" /><br /><img src="../images/userguide1.png" alt="User Guide" title="User Guide" border="0" style="margin-bottom:5px;" /></a></div>
                        <div style="float:right; vertical-align:middle;"><a href="../feedback.aspx" target="_blank"><img src="../images/btnfeedback1.gif" id="feedback" alt="Feedback" title="Feedback" border="0" /></a></div>
                    </div>            
                -->

                <!--
                <div style="overflow:auto; width:100%; background-color:#2f4da7; min-height:35px;">
                        <div style="float:left; vertical-align:middle; margin-top:7px; margin-left:10px;"><a href="../consumerview_userguide2.pdf" target="_blank"><font style="font-size:15px; color:#ffffff;"><b>User Guide</b></font></a></div>
                        <div style="float:right; vertical-align:middle; margin-top:7px; margin-right:10px;" id="feedback"><a href="../feedback.aspx" target="_blank"><font style="font-size:15px; color:#ffffff;"><b>Feedback</b></font></a></div>
                </div>   
                -->

                <div style="overflow:auto; width:100%; background-color:#2f4da7; min-height:35px;">
                    <div style="float:left; vertical-align:middle; margin-top:14px; margin-left:10px;"><a href="../consumerview_userguide2.pdf" target="_blank"><font style="font-size:18px; color:#ffffff;"><b>User Guide</b></font></a></div>
                    <div style="float:right; vertical-align:middle; margin-top:7px; margin-right:10px; margin-bottom:5px;" id="feedback"><a href="../feedbackm.aspx" target="_blank"><img src="../images/btnfeedback3.png" id="feedback" alt="Feedback" title="Feedback" border="0" /></a></div>
                </div>   


                <div id="divDisclaimer" runat="server"><p style="text-align:justify; margin-left:5px; margin-right:5px;"><font style="font-size:10px;color:#9a9a9a;"><b>Disclaimer</b> : The details of tariffs on this portal are as per the data submitted by TSPs to TRAI. However, consumers are requested to visit respective TSPs website / customer care for latest applicable tariffs.  <br /><br /></font></p></div>
                

            </div>
        </center>
    </form>
    
    
</body>
</html>

