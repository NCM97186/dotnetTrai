<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en">
<head>
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport" content="width=device-width, initial-scale=1">
<!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
<title>Telecom Regulatory Authority Of India | Government Of India</title>
    

<!-- Bootstrap -->
<link href="bower_components/bootstrap/dist/css/bootstrap.css" rel="stylesheet">
<link href="css/style.css" rel="stylesheet">
<link rel="stylesheet" href="https://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.css">
<link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.css">
<link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css">  <!-- For Rupee Symbol using   <i class="fa fa-inr"></i>   -->
<!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
<!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->

<script type="text/javascript" language="javascript">
    function hoverColor(x) {
        document.getElementById(x).style.backgroundColor = "#1068d6";
    }
    function hoverOut(x) {
        document.getElementById(x).style.backgroundColor = "#15488a";
    }
</script>

    
<!-- files for slider - taken from refreshless dot com/nouislider/examples/ -->
<link href="nouislider/base.css?v=1110" rel="stylesheet">
<link href="nouislider/prism.css" rel="stylesheet">
<script src="nouislider/wNumb.js"></script>
<link href="nouislider/nouislider.css?v=1110" rel="stylesheet">
<script src="nouislider/nouislider.js?v=1110"></script>
<!-- files for slider  - CODE ENDS HERE-->


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
                    document.getElementById("dashed3b").style.display = "block";
                }
                else {
                    document.getElementById("divMoreOptions").innerHTML = "<b><a href=javascript:funMoreOptions();><font style='color:#2f4da7;'>More Options</font></a></b>";
                    document.getElementById("dashed3b").style.display = "none";
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
                if (isNaN(document.getElementById("amount5a").value) && document.getElementById("amount5a").value != "") {
                    alert('Please enter a number');
                    document.getElementById("amount5a").value = "0";
                }
                if (isNaN(document.getElementById("amount5b").value) && document.getElementById("amount5b").value != "") {
                    alert('Please enter a number');
                    document.getElementById("amount5b").value = "500";
                }

                document.getElementById("Text2a").value = document.getElementById("amount2a").value;
                document.getElementById("Text2b").value = document.getElementById("amount2b").value;
                document.getElementById("Text3a").value = document.getElementById("amount3a").value;
                document.getElementById("Text3b").value = document.getElementById("amount3b").value;
                document.getElementById("Text4a").value = document.getElementById("amount4a").value;
                document.getElementById("Text4b").value = document.getElementById("amount4b").value;
                document.getElementById("Text5a").value = document.getElementById("amount5a").value;
                document.getElementById("Text5b").value = document.getElementById("amount5b").value;

                setDataFlag();
            }

            function transVal() {
                document.getElementById("TextValues1").value = document.getElementById("amount1a").value + " - " + document.getElementById("amount1b").value;
                document.getElementById("TextValues2").value = document.getElementById("amount2a").value + " - " + document.getElementById("amount2b").value;
                document.getElementById("TextValues3").value = document.getElementById("amount3a").value + " - " + document.getElementById("amount3b").value;
                document.getElementById("TextValues4").value = document.getElementById("amount4a").value + " - " + document.getElementById("amount4b").value;
                document.getElementById("TextValues5").value = document.getElementById("amount5a").value + " - " + document.getElementById("amount5b").value;
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
                    document.getElementById('amount5a').value = document.getElementById('Text5a').value;
                    document.getElementById('amount5b').value = document.getElementById('Text5b').value;


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
                    var div1 = document.getElementById('filter-unlimited-calls');
                    var checkboxCollection1 = div1.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection1.length; i++) {
                        if (checkboxCollection1[i].id != "ChkISDPack" && checkboxCollection1[i].id != "ChkISDRoaming" && checkboxCollection1[i].id != "ChkNatRoaming") {
                            if (checkboxCollection1[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection1[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection1[i].disabled = true;
                                checkboxCollection1[i].checked = false;
                            }
                        }
                    }

                    var div2 = document.getElementById('filter-data-type');
                    var checkboxCollection2 = div2.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = true;
                                checkboxCollection2[i].checked = false;
                            }
                        }
                    }

                    var div3 = document.getElementById('divAdvLocal');
                    var checkboxCollection2 = div3.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = true;
                                checkboxCollection2[i].checked = false;
                            }
                        }
                    }

                    var div4 = document.getElementById('divAdvSTD');
                    var checkboxCollection2 = div4.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = true;
                                checkboxCollection2[i].checked = false;
                            }
                        }
                    }

                    var div5 = document.getElementById('divAdvSMS');
                    var checkboxCollection2 = div5.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = true;
                                checkboxCollection2[i].checked = false;
                            }
                        }
                    }

                    var div6 = document.getElementById('divAdvRoaming');
                    var checkboxCollection2 = div6.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = true;
                                checkboxCollection2[i].checked = false;
                            }
                        }
                    }

                    document.getElementById('ChkFullTalktime').disabled = true;
                    document.getElementById('ChkValidityMore').disabled = true;
                    document.getElementById('ChkFullTalktime').checked = false;
                    document.getElementById('ChkValidityMore').checked = false;
                }


                if (document.getElementById('ChkISDPack').checked == false && document.getElementById('ChkISDRoaming').checked == false && document.getElementById('ChkNatRoaming').checked == false) {
                    var div1 = document.getElementById('filter-unlimited-calls');
                    var checkboxCollection1 = div1.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection1.length; i++) {
                        if (checkboxCollection1[i].id != "ChkISDPack" && checkboxCollection1[i].id != "ChkISDRoaming" && checkboxCollection1[i].id != "ChkNatRoaming") {
                            if (checkboxCollection1[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection1[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection1[i].disabled = false;
                            }
                        }
                    }

                    var div2 = document.getElementById('filter-data-type');
                    var checkboxCollection2 = div2.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = false;
                            }
                        }
                    }

                    var div3 = document.getElementById('divAdvLocal');
                    var checkboxCollection2 = div3.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = false;
                            }
                        }
                    }

                    var div4 = document.getElementById('divAdvSTD');
                    var checkboxCollection2 = div4.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = false;
                            }
                        }
                    }

                    var div5 = document.getElementById('divAdvSMS');
                    var checkboxCollection2 = div5.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = false;
                            }
                        }
                    }

                    var div6 = document.getElementById('divAdvRoaming');
                    var checkboxCollection2 = div6.getElementsByTagName('input');
                    for (var i = 0; i < checkboxCollection2.length; i++) {
                        if (checkboxCollection2[i].id != "ChkISDPack" && checkboxCollection2[i].id != "ChkISDRoaming" && checkboxCollection2[i].id != "ChkNatRoaming") {
                            if (checkboxCollection2[i].type.toString().toLowerCase() == "checkbox" || checkboxCollection2[i].type.toString().toLowerCase() == "radio") {
                                checkboxCollection2[i].disabled = false;
                            }
                        }
                    }

                    document.getElementById('ChkFullTalktime').disabled = false;
                    document.getElementById('ChkValidityMore').disabled = false;

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
                //var myURL = "comparisonview.aspx?t=" + document.getElementById('TextCompareProduct').value;
                if (document.getElementsByName('RadProvider')[1].checked == true) {
                    var myURL = "comparisonview.aspx?p=ISP&t=" + document.getElementById('TextCompareProduct').value;
                }
                else {
                    var myURL = "comparisonview.aspx?p=TSP&t=" + document.getElementById('TextCompareProduct').value;
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



            function funArrow() {
                if (document.getElementById('arrow1').src.indexOf('arrowdown3.jpg') > 0) {
                    document.getElementById('arrow1').src = 'images/arrowup3.jpg';
                }
                else {
                    document.getElementById('arrow1').src = 'images/arrowdown3.jpg';
                }
            }

            function funArrow2() {
                if (document.getElementById('arrow2').src.indexOf('arrowdown3.jpg') > 0) {
                    document.getElementById('arrow2').src = 'images/arrowup3.jpg';
                }
                else {
                    document.getElementById('arrow2').src = 'images/arrowdown3.jpg';
                }
            }


            function loaddata() {
                document.getElementById('ButtonLoadData').click();
            }

            function chkAll() {
                var checkList1 = document.getElementById('<%= ChkOper.ClientID %>');
                var checkBoxList1 = checkList1.getElementsByTagName("input");
                if (checkBoxList1[0].checked == true) {
                    for (var i = 1; i < checkBoxList1.length; i++) {
                        checkBoxList1[i].checked = false;
                    }
                }
                else {
                    for (var i = 1; i < checkBoxList1.length; i++) {
                        checkBoxList1[i].checked = true;
                    }
                }
            }

            function unchk() {
                var checkList1 = document.getElementById('<%= ChkOper.ClientID %>');
                var checkBoxList1 = checkList1.getElementsByTagName("input");
                checkBoxList1[0].checked = false;
            }

        </script>
        

    <script src="calendar/javascript/datepicker.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="calendar/stylesheets/datepicker.css" />


</head>

<body>
    <form id="form1" runat="server">

        <asp:TextBox ID="Text2a" runat="server" Text="0" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text2b" runat="server" Text="10000" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text3a" runat="server" Text="0" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text3b" runat="server" Text="500" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text4a" runat="server" Text="0" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text4b" runat="server" Text="365" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text5a" runat="server" Text="0" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="Text5b" runat="server" Text="500" style="display:none;"></asp:TextBox>

<div class="">
  <nav class="navbar navbar-expand-sm bg-faded fixed-top" style="background: #144e8c">
  <a class="navbar-brand" href="index.aspx"><img src="img/TRAI_logo.png" width="30" class="d-inline-block align-top" height="30" alt="">Tariff</a>
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#nav-content" aria-controls="nav-content" aria-expanded="false" aria-label="Toggle navigation"> <span class="navbar-toggler-icon"></span> </button>
  
  <!-- Links -->
  <div class="collapse navbar-collapse justify-content-end" id="nav-content">
    <ul class="navbar-nav">
      <li class="nav-item" style="width:120px; padding-right:5px; margin-right:5px; padding-left:5px; margin-left:5px;"> <a class="nav-link" href="http://trai.gov.in" target="_blank">TRAI Home</a> </li>
      <li class="nav-item" style="width:120px; padding-right:5px; margin-right:5px; padding-left:5px; margin-left:5px;"> <a class="nav-link" href="consumerview_userguide.pdf" target="_blank">User Guide</a> </li>
      <li class="nav-item" style="width:120px; padding-right:5px; margin-right:5px; padding-left:5px; margin-left:5px;"> <a class="nav-link" href="feedback.aspx" target="_blank">Feedback</a> </li>
    </ul>
  </div>
  </nav>
</div>

  <div class="container main-container">
    <div class="banner-area" style="padding-top:50px; padding-bottom:-50px;"> <a href="index.aspx"><img src="img/banner-edit-4.png" alt="" class="banner-image"></a> </div>
    <div class="row">
      <div class="col-left col-md-3">
        
        <div id = "type-of-provider" class="row filter-box-left" style="box-shadow: 4px 4px 4px #bbbbbb; padding-bottom:7px;">
          <label for="filter" class="color" title="Telecom Service Provider / Internet Service Provider">TSP / ISP<sup>*</sup></label>
          <ul>
            <li>
              <input type="radio" runat="server" name="RadProvider" value="TSP" title="Telecom Service Provider" checked="true" onclick="loaddata()" >
              <label style="margin-top:3px;">TSP</label>
              <div class="bullet"></div>
            </li>
            <li>
              <input type="radio" runat="server" name="RadProvider" value="ISP" title="Internet Service Provider" onclick="loaddata()">
              <label style="margin-top:3px;">ISP</label>
              <div class="bullet"></div>
            </li>
            
            <!--  
              <li>
                <input type="radio" name="device-value" value="Broadband">
                <label>Broadband</label>
                <div class="bullet"></div>
              </li>
            -->

          </ul>
        </div>

        <div id="divDevice" runat="server">
        <div id = "type-of-device" class="row filter-box-left" style="box-shadow: 4px 4px 4px #bbbbbb; padding-bottom:7px;">
          <label for="filter" class="color">Device<sup>*</sup></label>
          <ul>
            <li>
              <input type="radio" runat="server" name="RadDevice" value="Mobile" checked="true" onclick="loaddata()" >
              <label>Mobile</label>
              <div class="bullet"></div>
            </li>
            <li>
              <input type="radio" runat="server" name="RadDevice" value="Landline" onclick="loaddata()">
              <label>LandLine</label>
              <div class="bullet"></div>
            </li>
            
            <!--  
              <li>
                <input type="radio" name="device-value" value="Broadband">
                <label>Broadband</label>
                <div class="bullet"></div>
              </li>
            -->

          </ul>
        </div>
        </div>

        <div id="divPrePost" runat="server">
        <div id="type-mobiles" class="row filter-box-left" style="box-shadow: 4px 4px 4px #bbbbbb; padding-bottom:7px;">
         <label for="filter" class="color">Billing<sup>*</sup></label>
          <ul>
               <li>
              <input  type="radio" runat="server" name="RadPrePost" value="Prepaid" checked="true" onclick="loaddata()">
              <label>Prepaid</label>
              <div class="bullet"></div>
            </li>
            <li>
              <input  type="radio" runat="server" name="RadPrePost" value="Postpaid" onclick="loaddata()">
              <label>Postpaid</label>
              <div class="bullet"></div>
            </li>
       
          </ul>
        </div>
        </div>

        <div class="row filter-box-left" id="filter-circle" style="box-shadow: 4px 4px 4px #bbbbbb;">
            <label for="filter" class="color">Circle<sup>*</sup></label>
          <div>
              <asp:DropDownList ID="DropCircle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LoadData" style="min-width:235px;" BackColor="#ffffff" ForeColor="#4a4a4a" Font-Names="Verdana" CssClass="ddl">
                                <asp:ListItem Text="Select"></asp:ListItem>
                            </asp:DropDownList>
            <!--
            <select class="form-control dropdown-select" selected="Select">
            <option selected disabled hidden value="">Select</option>
              <option value="0">Delhi</option>
              <option value="1">Rajasthan</option>
              <option value="2">Gujrat</option>
              <option value="3">Uttar Pradesh</option>
              <option value="4">Madhya Pradesh</option>
            </select>
              -->
          </div>
        </div>
        
        <div class="row filter-box-left" id="filter-operator" style="box-shadow: 4px 4px 4px #bbbbbb;">
          <label for="filter">Operator</label>
                 <div class="boxes">
                     <a name="Bookmark2"></a>
              
                     <asp:CheckBoxList ID="ChkOper" runat="server"></asp:CheckBoxList>

                     <!--
                        <input type="checkbox" id="box-1">
                     <label for="box-1">Airtel</label>
   					
                        <input type="checkbox" id="box-2" checked>
                      <label for="box-2">BSNL</label>
                        
                         <input type="checkbox" id="box-3">
                      <label for="box-3">Idea</label>
                      
                        <div class="slide2" style="display:none">
  <input type="checkbox" id="box-4">
                          <label for="box-4">JIO </label>
                          
                          <input type="checkbox" id="box-5">
                          <label for="box-5">Reliance</label>
                          
                          <input type="checkbox" id="box-6">
                          <label for="box-6">Vodafone</label>
                        </div>
                        <button class="op view-more" >View more...</button>
                     -->

                    </div>
        </div>
        

        <div id="divUnlimCalls" runat="server">
            <div class="row filter-box-left" id="filter-unlimited-calls" style="box-shadow: 4px 4px 4px #bbbbbb;">
              <label for="filter">Unlimited Calls</label>
              <div class="boxes">
                    <asp:CheckBox ID="ChkUnlim_Local" runat="server" Text="Unlimited Local Calls" />
                    <asp:CheckBox ID="ChkUnlim_STD" runat="server" Text="Unlimited STD Calls"  />
                    <asp:CheckBox ID="ChkUnlim_Roaming" runat="server" Text="Unlimited Roaming Calls"  />

                <!--
                <input type="checkbox" id="box-4">
                <label for="box-4">Unlimited Local Calls</label>
                <input type="checkbox" id="box-5" checked>
                <label for="box-5">Unlimited STD Calls</label>
                <input type="checkbox" id="box-6">
                <label for="box-6">Unlimited Roaming Calls</label>
                -->

              </div>
            </div>
        </div>
        
          <div id="divDataTypes" runat="server">
            <div class="row filter-box-left ui-content" id="filter-data-type" style="box-shadow: 4px 4px 4px #bbbbbb;">
              <label for="filter">Data Type</label>
              <div class="boxes">

                <asp:CheckBox ID="ChkDataTech1" runat="server" Text="2G Data" />
                <asp:CheckBox ID="ChkDataTech2" runat="server" Text="3G Data" />
                <asp:CheckBox ID="ChkDataTech3" runat="server" Text="4G Data" />

                <!--
                <input type="checkbox" id="box-7">
                <label for="box-7">2G Data</label>
                <input type="checkbox" id="box-8" checked>
                <label for="box-8">3G Data</label>
                <input type="checkbox" id="box-9">
                <label for="box-9">4G Data</label>
                -->

              </div>
            </div>
        </div>

        <div id="divDailyDataCapping" runat="server">
            <div class="row filter-box-left ui-content" id="filter-daily-data-capping" style="box-shadow: 4px 4px 4px #bbbbbb; padding-bottom:4px;">
            &nbsp;&nbsp;<label for="filter">Daily Data Capping</label>

          <!-- Slider for Daily Data Capping -->
            <div>
                <div class="example">
			        <div id="html5d"></div>
                    <asp:TextBox ID="amount5a" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:43px; height:20px; border-radius:0px; margin-top:10px; color:#7a7a7a; font-weight:normal; text-align:center; font-size:14px;' onblur="saveSliderText()"></asp:TextBox>&nbsp;<font style="font-size:14px;">GB</font>
                    <font style="font-size:14px;">&nbsp;to&nbsp;</font>
                    <asp:TextBox ID="amount5b" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:43px; height:20px; border-radius:0px; margin-top:10px; color:#7a7a7a; font-weight:normal; text-align:center; font-size:14px;' onblur="saveSliderText()"></asp:TextBox>&nbsp;<font style="font-size:14px;">GB</font>
                                    
                            <script>
                                var html5dSlider = document.getElementById('html5d');

                                var min7 = 0;
                                var min8 = 500;
                                if (!isNaN(document.getElementById('Text5a').value) && document.getElementById('Text5a').value != "") {
                                    var min7 = parseInt(document.getElementById('Text5a').value);
                                }
                                if (!isNaN(document.getElementById('Text5b').value) && document.getElementById('Text5b').value != "") {
                                    var min8 = parseInt(document.getElementById('Text5b').value);
                                }

                                noUiSlider.create(html5dSlider, {
                                    start: [min7, min8],
                                    connect: true,
                                    range: {
                                        'min': 0,
                                        'max': 500
                                    }
                                });
                            </script>			
                            <script>
                                var inputNumber7 = document.getElementById('amount5a');
                                var inputNumber8 = document.getElementById('amount5b');
                                html5dSlider.noUiSlider.on('update', function (values, handle) {

                                    //var value = values[handle];
                                    var value7 = parseInt(values[handle]);

                                    if (handle) {
                                        inputNumber8.value = value7;
                                    } else {
                                        //select.value = Math.round(value);
                                        inputNumber7.value = value7;
                                    }



                                    if ((document.getElementById('amount5a').value != '' && document.getElementById('amount5a').value > 0) || (document.getElementById('amount5b').value != '' && document.getElementById('amount5b').value < 500)) {
                                        if (document.getElementById('TextDataTechFlag').value == 0) { // so that alert is shown only once.
                                            if (document.getElementById('ChkDataTech1').checked == false && document.getElementById('ChkDataTech2').checked == false && document.getElementById('ChkDataTech3').checked == false) {
                                                alert('Please select 2G / 3G / 4G Data first.');
                                                document.getElementById('TextDataTechFlag').value = '1';
                                                document.getElementById('amount5a').focus();
                                            }
                                        }
                                    }

                                });

                                /*
                                select.addEventListener('change', function(){
                                    //html5aSlider.noUiSlider.set([this.value, null]);   // remove comments to change slider value on text box change
                                });
                                */

                                inputNumber7.addEventListener('change', function () {
                                    //html5aSlider.noUiSlider.set([this.value, null]);    // remove comments to change slider value on text box change
                                });
                                inputNumber8.addEventListener('change', function () {
                                    //html5aSlider.noUiSlider.set([null, this.value]);    // remove comments to change slider value on text box change
                                });

                                setManualValues();  // so that slider and box values don't get set to default on page reload

                            </script>		
                        </div>
	                </div>
    <!-- Slider for Daily Data Capping - CODE ENDS HERE -->
        
        <!--
          <div class="range-sliders-capping">
            <div class="range range-primary">
              <input type="range" name="range" min="1" max="10" value="2" onchange="rangePrimary.value=value  +'GB'">
              <output id="rangePrimary">2 GB</output>
            </div>
          </div>
        -->
          </div>
        </div>

        <div class="row filter-box-left ui-content" id="filter-total-data-capping" style="box-shadow: 4px 4px 4px #bbbbbb; padding-bottom:4px; padding-top:0px;">
          <label for="filter" style="margin-top:20px;">Total Data Capping</label>

              <div class="boxes" style="margin-left:6px;">
                <asp:CheckBox ID="ChkCapUnlim" runat="server" Text="Unlimited" style="font-size:15px;" />
              </div>

        <!--
          <div class="range range-primary">
            <input type="range" name="range1" min="0" max="500" value="50" onchange="rangePrimary5.value=value + 'GB'">
            <output id="rangePrimary5">100 GB</output>
          </div>
        </div>
        -->
        <!-- Slider for Total Data Capping -->
            <div>
                <div class="example">
			        <div id="html5b"></div>
                    <asp:TextBox ID="amount3a" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:43px; height:20px; border-radius:0px; margin-top:10px; color:#7a7a7a; font-weight:normal; text-align:center; font-size:14px;' onblur="saveSliderText()"></asp:TextBox>&nbsp;<font style="font-size:14px;">GB</font>
                    <font style="font-size:14px;">&nbsp;to&nbsp;</font>
                    <asp:TextBox ID="amount3b" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:43px; height:20px; border-radius:0px; margin-top:10px; color:#7a7a7a; font-weight:normal; text-align:center; font-size:14px;' onblur="saveSliderText()"></asp:TextBox>&nbsp;<font style="font-size:14px;">GB</font>
                                    
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
                                var inputNumber3 = document.getElementById('amount3a');
                                var inputNumber4 = document.getElementById('amount3b');
                                html5bSlider.noUiSlider.on('update', function (values, handle) {

                                    //var value = values[handle];
                                    var value3 = parseInt(values[handle]);

                                    if (handle) {
                                        inputNumber4.value = value3;
                                    } else {
                                        //select.value = Math.round(value);
                                        inputNumber3.value = value3;
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

        <div id="divTalktime" runat="server">
            <div class="row filter-box-left ui-content" id="filter-talktime" style="box-shadow: 4px 4px 4px #bbbbbb;">
              <label for="filter">Talktime&nbsp;&nbsp;</label>
              <div class="boxes">
                <!--
                <input type="checkbox" id="box-10">
                <label for="box-10">Full Talktime & More </label>
                -->
                <asp:CheckBox ID="ChkFullTalktime" runat="server"  Text="Full Talktime & More" />
              </div>
            </div>
        </div>

        <div class="row filter-box-left ui-content" id="filter-price" style="box-shadow: 4px 4px 4px #bbbbbb; padding-bottom:4px;">
        <!--<label for="filter">Price ₹ (Optional)</label> -->
        <b><asp:Label ID="LblPrice" Text="Price ₹ (Optional)" runat="server"></asp:Label></b>
       <!-- Slider for Price Range -->
        <div>
            <div class="example">
			    <div id="html5a"></div>
                <font style="font-size:14px;">Min&nbsp;</font><asp:TextBox ID="amount2a" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:43px; height:20px; border-radius:0px; margin-top:10px; color:#7a7a7a; font-weight:normal; text-align:center; font-size:14px;' onblur="saveSliderText()"></asp:TextBox>
                <font style="font-size:14px;">&nbsp;to&nbsp;</font>
                <asp:TextBox ID="amount2b" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:43px; height:20px; border-radius:0px; margin-top:10px; color:#7a7a7a; font-weight:normal; text-align:center; font-size:14px;' onblur="saveSliderText()"></asp:TextBox><font style="font-size:14px;">&nbsp;Max</font>

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
            
        <!--
          <div class="range range-primary">
            <input type="range" name="range1" min="1" max="10000" value="100" onchange="rangePrimary1.value= '₹'+value">
            <output id="rangePrimary1">₹100</output>
          </div>
        </div>
        -->
        
        </div>

        <div class="row filter-box-left ui-content" id="divValidity" runat="server" style="box-shadow: 4px 4px 4px #bbbbbb; padding-bottom:4px;">
          <label for="filter">Validity Days (Optional)</label>
          <div class="boxes" style="margin-left:6px;">
            <asp:CheckBox ID="ChkValidityMore" runat="server" Text="More Than 365 Days" style="font-size:15px;" />
          </div>
                                                      
        <!-- Slider for Validity -->
        <div>
            <div class="example">
			    <div id="html5c"></div>
                <font style="font-size:14px;">Min&nbsp;</font><asp:TextBox ID="amount4a" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:43px; height:20px; border-radius:0px; margin-top:10px; color:#7a7a7a; font-weight:normal; text-align:center; font-size:14px;' onblur="saveSliderText()"></asp:TextBox>
                <font style="font-size:14px;">&nbsp;to&nbsp;</font>
                <asp:TextBox ID="amount4b" runat="server" title="Please set the slider or enter a value here" CssClass="input" style='border:1; width:43px; height:20px; border-radius:0px; margin-top:10px; color:#7a7a7a; font-weight:normal; text-align:center; font-size:14px;' onblur="saveSliderText()"></asp:TextBox><font style="font-size:14px;">&nbsp;Max</font>
                                        
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

    <!--
    <div class="row filter-box-left ui-content" id="divFeedback" runat="server">
        <p style="width:100%; text-align:center; margin-top:10px;"><a href="consumerview_userguide.pdf" target="_blank"><img src="images/userguide2.jpg" alt="User Guide" title="User Guide" border="0" style="margin-bottom:5px;" /></a></p>
        <p style="width:100%; text-align:center;"><a href="feedback.aspx" target="_blank"><img src="images/btnfeedback4.gif" alt="Feedback" title="Feedback" border="0" /></a></p>
    </div>
    -->

    </div>

      <div class="col-md-9 col-right">
        <div class="filter-advance-plans filter-box-left" style="box-shadow: 4px 4px 4px #bbbbbb;">
      
          <div class="col-md-12"  style="padding: 0 5% 0 0;margin-bottom:8px;">
            <p style="margin-top:-10px;">Tariff Types</p>
                        <asp:CheckBoxList ID="ChkPlans" runat="server" CssClass="chks" RepeatDirection="Horizontal" style="margin-left:-10px;">
                            <asp:ListItem Selected="true">All Tariffs</asp:ListItem>
                            <asp:ListItem title="test">Plan Voucher</asp:ListItem>
                            <asp:ListItem>STV</asp:ListItem>
                            <asp:ListItem>Combo</asp:ListItem>
                            <asp:ListItem>Top Up</asp:ListItem>
                            <asp:ListItem>SUK</asp:ListItem>
                        </asp:CheckBoxList>

                        <asp:Label ID="LblISPTtypes" runat="server"></asp:Label>

                
                    <div id="divAddInfoISP" runat="server" style="width:100%; display:flex; margin-right:0px;">
                        <div style="width:75%; margin-left:0px; margin-right:0px;">
                        </div>
                        <div style="width:24%; margin-left:0px; margin-right:0px;">
                            <a id="advanced-filters" href="#demo" class="" data-toggle="collapse" style="display:block;margin-top:-5px;" >Additional Information </a>
                        </div>
                        <div style="float:left;width:5%; margin-left:0px; margin-right:0px; margin-top:2px;">
                            <a href="#demo"  class="" data-toggle="collapse"><img id="arrow2" onclick="funArrow2()" src="images/arrowdown3.jpg" border="0" /></a>
                        </div>
                    </div>
                

              <!--
            <label>
              <input type="checkbox" rel="accounting" />
              All Tariffs</label>
            <label>
              <input type="checkbox" rel="courier" />
              Plan Voucher</label>
            <label>
              <input type="checkbox" rel="project-management" />
              STV</label>
            <label>
              <input type="checkbox" rel="video-games" />
              Combo</label>
            <label>
              <input type="checkbox" rel="video-games" />
              Top Up</label>
            <label>
              <input type="checkbox" rel="video-games" />
              SUK</label>
            <label>
              <input type="checkbox" rel="video-games" />
              VAS</label>
            <label>
              <input type="checkbox" rel="video-games" />
              Promo</label>
             -->

          </div>

        <!-- 
          <div class="col-md-12"  style="padding: 0;margin-bottom: 15px">
           <label><input type="checkbox" rel="accounting" />ISD Pack</label>
          </div>
        -->

            <a name="Bookmark1"></a>

            

            <div class="col-md-12" id="divRoaming" runat="server" style="padding: 0 5% 0 0; margin-top:8px;">
            <p >Roaming Options</p>
            
            <div style="width:100%; display:flex; margin-left:0px; margin-right:0px;">
            
                <div style="float:left; width: 72%; margin-left:1px; margin-right:0px;">
                    <asp:CheckBox ID="ChkISDPack" runat="server" Text="ISD Pack" CssClass="input3" onClick="funDisableFilters()"  style="margin-left:-15px;" />
                    &nbsp;&nbsp;
                    <asp:CheckBox ID="ChkISDRoaming" runat="server" Text="International Roaming Pack" CssClass="input3" style="margin-left:-12px;" onClick="funDisableFilters()" />
                    &nbsp;&nbsp;
                    <asp:CheckBox ID="ChkNatRoaming" runat="server" Text="National Roaming Pack" CssClass="input3" style="margin-left:-16px;" onClick="funDisableFilters()" />
                </div>
                <div style="width: 24%; margin-left:0px; margin-right:0px;">
                    <a id="advanced-filters" href="#demo" class="" data-toggle="collapse" style="display:block;margin-top:-5px;" >Additional Information </a>
                </div>
               <div style="width: 4%; margin-left:0px; margin-right:0px; margin-top:2px;">
                    <a href="#demo"  class="" data-toggle="collapse"><img id="arrow1" onclick="funArrow()" src="images/arrowdown3.jpg" border="0" /></a>
                </div>

            </div>

                <!--
            <label>
                <input type="checkbox" rel="accounting" />
                ISD Pack</label>
            <label>
              <input type="checkbox" rel="courier" />
              International Roaming Pack</label>
            <label>
              <input type="checkbox" rel="project-management" />
              National Roaming Pack</label>
            <label>
            -->







            </div>
            

            <!-- Div for Additional Information Parameters -->
            
            <div class="col-md-12" style="padding: 0 5% 0 0; margin-top:8px;">
                <div id="demo" class="collapse" style="margin-top:-5px;">
                
                    <div id="divAddParamTSP" runat="server">
                          <div class="advanced-filter-std-call col" id="divAdvLocal" style="margin-top:2px;">
                            <h6>Local Calls</h6>
                            <!--
                              <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OnNet Peak</label>
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OnNet Off-Peak</label>
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OffNet Off-Peak</label>
                            <label>
                              <input type="checkbox" rel="courier" />
                              Mobile OffNet Peak</label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OnNet Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OnNet Off-Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OffNet Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OffNet Off-Peak </label>
                            -->

                              <asp:CheckBoxList ID="CheckAdvLocal" runat="server" CssClass="input3" RepeatColumns="3" RepeatDirection="Horizontal" style="margin-left:-10px;">
                                    <asp:ListItem Value="local_on_voice_peak">Mobile On Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="local_on_voice_offpeak">Mobile On Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="local_off_voice_peak">Mobile Off Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="local_off_voice_offpeak">Mobile Off Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="local_fix_on_voice_peak">Landline On Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="local_fix_on_voice_offpeak">Landline On Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="local_fix_off_voice_peak">Landline Off Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="local_fix_off_voice_offpeak">Landline Off Net - Off Peak</asp:ListItem>
                                </asp:CheckBoxList>

                          </div>
                          <div class="advanced-filter-std-call col" id="divAdvSTD" style="margin-top:-10px;">
                            <h6>STD Calls</h6>
                            <!--
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OnNet Peak</label>
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OnNet Off-Peak</label>
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OffNet Off-Peak</label>
                            <label>
                              <input type="checkbox" rel="courier" />
                              Mobile OffNet Peak</label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OnNet Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OnNet Off-Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OffNet Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OffNet Off-Peak </label>
                            -->

                              <asp:CheckBoxList ID="CheckAdvSTD" runat="server" CssClass="input3" RepeatColumns="3" RepeatDirection="Horizontal" style="margin-left:-10px;">
                                    <asp:ListItem Value="std_on_voice_peak">Mobile On Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="std_on_voice_offpeak">Mobile On Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="std_off_voice_peak">Mobile Off Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="std_off_voice_offpeak">Mobile Off Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="std_fix_on_voice_peak">Landline On Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="std_fix_on_voice_offpeak">Landline On Net - Off Peak</asp:ListItem>
                                    <asp:ListItem Value="std_fix_off_voice_peak">Landline Off Net - Peak</asp:ListItem>
                                    <asp:ListItem Value="std_fix_off_voice_offpeak">Landline Off Net - Off Peak</asp:ListItem>
                                </asp:CheckBoxList>

                          </div>
                          <div class="advanced-filter-std-call col" id="divAdvSMS" style="margin-top:-10px;">
                            <h6>SMS</h6>
                            <!--
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OnNet Peak</label>
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OnNet Off-Peak</label>
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OffNet Off-Peak</label>
                            <label>
                              <input type="checkbox" rel="courier" />
                              Mobile OffNet Peak</label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OnNet Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OnNet Off-Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OffNet Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OffNet Off-Peak </label>
                            -->

                              <asp:CheckBoxList ID="CheckAdvSMS" runat="server" CssClass="input3" RepeatColumns="3" RepeatDirection="Horizontal" style="margin-left:-10px;">
                                    <asp:ListItem Value="sms_local_on">Local On Net</asp:ListItem>
                                    <asp:ListItem Value="sms_local_off">Local Off Net</asp:ListItem>
                                    <asp:ListItem Value="sms_nat_on">National On Net</asp:ListItem>
                                    <asp:ListItem Value="sms_nat_off">National Off Net</asp:ListItem>
                                    <asp:ListItem Value="sms_int">International</asp:ListItem>
                                </asp:CheckBoxList>

                          </div>
                          <div class="advanced-filter-std-call col" id="divAdvRoaming" style="margin-top:-10px; margin-bottom:-8px;">
                            <h6>Roaming Calls</h6>
                            <!--
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OnNet Peak</label>
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OnNet Off-Peak</label>
                            <label>
                              <input type="checkbox" rel="accounting" />
                              Mobile OffNet Off-Peak</label>
                            <label>
                              <input type="checkbox" rel="courier" />
                              Mobile OffNet Peak</label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OnNet Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OnNet Off-Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OffNet Peak </label>
                            <label>
                              <input type="checkbox" rel="project-management" />
                              LandLine OffNet Off-Peak </label>
                            -->

                              <asp:CheckBoxList ID="CheckAdvRoaming" runat="server" CssClass="input3" RepeatColumns="3" RepeatDirection="Horizontal" style="margin-left:-10px;">
                                <asp:ListItem Value="roam_call_voice_out">Local Outgoing</asp:ListItem>
                                <asp:ListItem Value="roam_call_voice_std">STD Outgoing</asp:ListItem>
                              </asp:CheckBoxList>

                          </div>
                        </div>

                        <div id="divAddParamISP" runat="server">
                            <div class="advanced-filter-std-call col" id="divAdvISPLaunch" style="margin-top:-5px; margin-bottom:12px;">
                                <h6>Date of Launch</h6> 
                                <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" width="105" ></asp:TextBox>
                                &nbsp;&nbsp;To&nbsp;&nbsp;
                                <asp:TextBox ID="TextDate2" MaxLength="30" runat="server" OnPreRender="TextDate2_PreRender" width="105" ></asp:TextBox>
                            </div>
                            <div class="advanced-filter-std-call col" id="divAdvISPSSA" style="margin-top:15px; margin-bottom:12px;">
                                <h6>Service Area / SSA</h6> 
                                <asp:TextBox ID="TextISPSSA" runat="server" width="300" ></asp:TextBox>
                            </div>
                            <div class="advanced-filter-std-call col" id="divAdvISPFUP" style="margin-top:15px; margin-bottom:12px;">
                                <h6>FUP</h6> 
                                <asp:CheckBox ID="ChkISPFUP" runat="server" Text="Show FUP Tariffs" />
                            </div>
                            <div class="advanced-filter-std-call col" id="divAdvISPData" style="margin-top:10px; margin-bottom:12px;">
                                <h6>Data Usage Limit (GB)</h6> 
                                <asp:TextBox ID="TextISPDataUsage1" runat="server" width="100" ></asp:TextBox>
                                &nbsp;&nbsp;To&nbsp;&nbsp;
                                <asp:TextBox ID="TextISPDataUsage2" runat="server" width="100" ></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="ChkISPDataUnlim" runat="server" Text="Unlimited" />
                            </div>
                            <div class="advanced-filter-std-call col" id="divAdvISPPromo" style="margin-top:15px; margin-bottom:12px;">
                                <h6>If Promotional Offer, valid till</h6> 
                                <asp:TextBox ID="TextDate3" MaxLength="30" runat="server" OnPreRender="TextDate3_PreRender" width="105" ></asp:TextBox>
                            </div>
                        </div>

                </div>
            </div>
            <!-- Div for Additional Information Parameters - CODE ENDS HERE -->



            <div align="left" style="width:100%; margin-top:15px; margin-bottom:-10px; margin-left:1px; margin-top:5px;">
                <asp:Button ID="ImagePopUp" runat="server" cssClass="btn btn-primary"  OnClick="ButtonPopUp_Click" OnClientClick="javascript:saveSliderText();" Style="background-color: #15488a;border-color: #15488a; font-size:13px; width:100px;" onmouseOver="hoverColor(this.id);" onmouseOut="hoverOut(this.id);" Text="Submit" />
            </div>
          <!-- <a href="#demo" class="btn btn-info" data-toggle="collapse">Simple collapsible</a> --> 
          
        </div>

        
        <div class="blank-result-pane" id="divblank" runat="server">
            <div class="jumbotron" style="box-shadow: 4px 4px 4px #bbbbbb;">
            <!-- <h1 class="display-4">Welcome To Tariff Centre</h1>-->
            <h1 class="display-4">Search Tariff Plans of Telecom Operators</h1>
            <!-- <p class="lead">A site to search tariff plans as per your requirements.</p>-->
            <hr class="my-4">
            <!-- <p>It uses utility classes for typography and spacing to space content out within the larger container.</p>
            <p class="lead">
                <a class="btn btn-primary btn-lg" href="#" role="button">Learn more</a>
            </p> -->
            </div>
        </div>


        
        

        <div class="row filter-rest-plans">
          <div class="col-md-12">
            <div class="sort-filters"><div id="divsort" runat="server"></div>
           </div>
        </div>
          <div class="col-md-12" style="margin-top: 10px;"><span style="font-size: 0.8em; position: relative; bottom: -20px; left:13px;" id="spanMatching" runat="server"></span>
            <div class="filter-rest-plans-button" id="divFilterButton" runat="server" visible="false" style="margin-top:-8px;">
                <asp:Button ID="ButtonCompare" runat="server" cssClass="btn btn-primary" OnClick="showRecords" OnClientClick="javascript:funCompareSend();" Style="background-color: #15488a;border-color: #15488a; font-size:13px; width:100px;" onmouseOver="hoverColor(this.id);" onmouseOut="hoverOut(this.id);" Text="Compare" />
                <asp:Button ID="ButtonReset" runat="server" cssClass="btn btn-primary" OnClick="ButtonClearFilters_Click" Style="background-color: #15488a;border-color: #15488a; font-size:13px; width:100px;" onmouseOver="hoverColor(this.id);" onmouseOut="hoverOut(this.id);" Text="Reset Filters" />
            </div>
          </div>
          <div class="col-md-12" style="margin-top: 0px;text-align:right;">
              <span style="font-size: 0.8em; position: relative; bottom: -20px; left:13px; margin-right:14px; text-align:right;" id="spanDownload" runat="server" visible="false"><a href="javascript:funExcel()"><img src="images/excel.jpg" title="Download Excel" border="0" /></a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:funXML()"><img src="images/xml.png" title="Download XML" border="0" /></a></span>
          </div>
        </div>



        <div id="divresults" runat="server" style="max-height:1120px; overflow-y:auto; border:1px solid; border-color:#afafaf; margin-right:2px;"></div>
          
        <div style="text-align:right; width:100%; margin-right:0px; margin-top:0px; margin-bottom:0px; border:1px solid; border-color:#eff2f3;">
        <div style="width:100%; margin-top: 10px; margin-right:0px; margin-bottom:0px;">
            <div class="filter-rest-plans-button" id="divFilterButton2" runat="server" visible="false" style="width:100%; margin-right:0px;">
                <asp:Button ID="ButtonCompare2" runat="server" cssClass="btn btn-primary" OnClick="showRecords" OnClientClick="javascript:funCompareSend();" Style="background-color: #15488a;border-color: #15488a; font-size:13px;" onmouseOver="hoverColor(this.id);" onmouseOut="hoverOut(this.id);" Text="Compare" />
                <asp:Button ID="ButtonReset2" runat="server" cssClass="btn btn-primary" OnClick="ButtonClearFilters_Click" Style="background-color: #15488a;border-color: #15488a; font-size:13px;" onmouseOver="hoverColor(this.id);" onmouseOut="hoverOut(this.id);" Text="Reset Filters" />
            </div>
        </div>
        <div style="margin-top:50px; margin-bottom:0px; text-align:right; width:100%;" id="divDownload2" runat="server" visible="false">
            <span style="font-size: 0.8em; bottom: 0px; left:13px; margin-right:0px; text-align:right;" id="spanDownload2" runat="server" visible="false"><a href="javascript:funExcel()"><img src="images/excel.jpg" title="Download Excel" border="0" /></a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:funXML()"><img src="images/xml.png" title="Download XML" border="0" /></a></span>
        </div>
      </div>

      </div>

      
        
    </div>
      <div id="divDisclaimer" runat="server" style="margin-top:20px; margin-bottom:20px; background-color:#fcfcfc; padding:5px;border-radius:5px;"><p style="text-align:justify; margin-left:5px; margin-right:5px; margin-bottom:0px; bottom:0px;"><font style="font-size:13px;color:#525252;"><b>Disclaimer</b> : The details of tariffs on this portal are as per the data submitted by TSPs / ISPs to TRAI. However, consumers are requested to visit respective TSPs / ISPs website / customer care for latest applicable tariffs.</font></p></div>

  </div>


<!-- jQuery (necessary for Bootstrap's JavaScript plugins) --> 
<script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" ></script> 
<script src="bower_components/jquery/dist/jquery.min.js"></script> 
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script> 
<script src="bower_components/bootstrap/dist/js/bootstrap.min.js"></script> 
<script src="js/script.js"></script> 
<!-- Include all compiled plugins (below), or include individual files as needed --> 
<script src="https //ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
<script src="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.js"></script>



        <!-- below three fields are for storing the parameters for sliders 2, 3 and 4 -->
        <asp:TextBox ID="HiddenQry2" runat="server" Visible="false" Text="" />
        <asp:TextBox ID="HiddenQry3" runat="server" Visible="false" Text="" />
        <asp:TextBox ID="HiddenQry4" runat="server" Visible="false" Text="" />
        <asp:TextBox ID="HiddenQry5" runat="server" Visible="false" Text="" />

                <p align="right" style="margin-top:-75px;">
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
                    <asp:Button ID="ButtonLoadData" runat="server" style="display:none;" OnClick="LoadData" />
                    <asp:Button ID="ButtonShow" runat="server" style="display:none;" OnClick="showRecords" PostBackUrl="#Bookmark1" />
                    <asp:Button ID="ButtonExcel" runat="server" style="display:none;" OnClick="ButtonExcel_Click" />
                    <asp:Button ID="ButtonXML" runat="server" style="display:none;" OnClick="ButtonXML_Click" />
                    
                </p>





                <div id="divPopUp" runat="server" style="position:absolute; top:50%;left:50%;border:0px solid; z-index:555; background-color:#ffffff;transform: translate(-50%,-50%);" visible="false">
                    <div id="divPopShadow" runat="server" style="border-left:1px solid; border-bottom:1px solid;box-shadow: 7px 10px #bbbbbb; min-height:300px; z-index:556;" visible="false">
                        <div id="divPopUpBg" runat="server" style="z-index:557; text-align:center;">
                            <div id="divSelection" runat="server" style="padding:5px;"></div>
                            <p style="text-align:center;">
                            <asp:Button ID="Button1" runat="server" cssClass="btn btn-primary"  OnClick="Button1_Click" PostBackUrl="#Bookmark1" Style="background-color: #15488a;border-color: #15488a; font-size:13px;" onmouseOver="hoverColor(this.id);" onmouseOut="hoverOut(this.id);" Text="Confirm" Visible="false" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="ButtonCancel" runat="server" cssClass="btn btn-primary"  OnClick="ButtonCancel_Click" Style="background-color: #15488a;border-color: #15488a; font-size:13px;" onmouseOver="hoverColor(this.id);" onmouseOut="hoverOut(this.id);" Text="Cancel" Visible="false" />
                            </p>
                        </div>
                    </div>
                </div>
        
</form>
</body>
</html>