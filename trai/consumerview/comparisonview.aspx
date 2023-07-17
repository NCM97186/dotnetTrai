<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="comparisonview.aspx.cs" Inherits="comparisonview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai7.css" type="text/css" media="all"/>
    <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css">  <!-- For Rupee Symbol using   <i class="fa fa-inr"></i>   -->
     
    <script type="text/javascript">

        function funmore(divno, stat) {
            for (i = 0; i < 5; i++) {
                if (document.getElementById("divmore" + i)) {
                    document.getElementById("divmore" + i).style.display = "none";
                    document.getElementById("p" + i).innerHTML = "<a href=javascript:funmore('" + i + "','More') class=indexlinks1>More...</a>";
                }
            }

            //alert(divno + ":" + document.getElementById("divmore" + divno).style.display);
            if (stat == "More") {
                document.getElementById("p" + divno).innerHTML = "<a href=javascript:funmore('" + divno + "','Less') class=indexlinks1><font style=color:#ff0000>Less...</font></a>";
                document.getElementById("divmore" + divno).style.display = "block";
            }
            else {
                document.getElementById("p" + divno).innerHTML = "<a href=javascript:funmore('" + divno + "','More') class=indexlinks1>More...</a>";
                document.getElementById("divmore" + divno).style.display = "none";
            }
        }

    </script>

</head>
<body style="background-image:url('images/bg4.jpg');">
    <form id="form1" runat="server">
    
        <center>
            <div style="width:1030px; text-align:center; background-color:#ffffff;">
                

                <!-- floating pop up feedback code starts here -->
                  <!--
                    <div style="margin:auto; width:100%">
                    <div style="position:fixed; right:3%;top:5%;"><a href="feedback.aspx" target="_blank"><img src="images/feedback.png" alt="Feedback" title="Feedback" border="0" /></a></div>
                    </div>
                  -->
                <!-- floating pop up feedback code ends here -->

                <!--
                <div style="background-image: url('images/bg1.jpg'); margin:0px; height:100px;">
                    <div class="wrapper">
                        <div id="logo">
                            <img src="images/logo-trai.png" alt="TRAI" width="60" border="0" />
                        </div>
                        <div id="threecenter" align="center">
                            <p style="padding-top:5px; padding-right:180px;"><font style="font-family:'Arial'; font-size:22px; color:#00429b;"><u><b>TARIFF</b></u></font></p>
                        </div>
                    </div>
                </div>
                -->

                <!--<div style="margin:0px;"><img src="images/topbnr.jpg" alt="TRAI" title="TRAI" border="0" /></div>-->
                <div style="margin:0px;"><a href="index.aspx"><img src="img/banner-edit-4.png" alt="TRAI" title="TRAI" border="0" /></a></div>
                

                <br />
                <asp:Label ID="Lblttype" runat="server"></asp:Label>
                <br /><br />
                <table width="100%" cellspacing="1" cellpadding="5">
                    <tr>
                        <td width="20%" class="tablehead" align="center" valign="top"><asp:CheckBox ID="CheckPrice" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="showTable" Text="Price" /></td>
                        <td width="20%" class="tablehead" align="center" valign="top" id="tdTalktime" runat="server"><asp:CheckBox ID="CheckMonVal" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="showTable" Text="Talktime" /></td>
                        <td width="20%" class="tablehead" align="center" valign="top"><asp:CheckBox ID="CheckValidity" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="showTable" Text="Validity" /></td>
                        <td width="20%" class="tablehead" align="center" valign="top"><asp:CheckBox ID="CheckDailyCap" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="showTable" Text="Daily Data Capping" /></td>
                        <td width="20%" class="tablehead" align="center" valign="top"><asp:CheckBox ID="CheckTotalCap" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="showTable" Text="Total Data Capping" /></td>
                    </tr>
                </table>
                
                <div id="divresults" runat="server" style="margin-top:0px; margin-bottom:0px;"></div>
                <div id="divshowdetails" runat="server" style="margin-top:0px; margin-bottom:0px;"></div>
                <div id="divmores" runat="server" style="margin-top:0px; margin-bottom:0px;"></div>
                <div id="divDisclaimer" runat="server" style="width:95%; margin-top:20px; margin-bottom:20px; margin-left:20px; border:1px solid; border-color:#ffffff;"><p style="text-align:justify; margin-left:5px; margin-right:5px; margin-top:0px;"><font style="font-size:13px;color:#525252;"><b>Disclaimer</b> : The details of tariffs on this portal are as per the data submitted by TSPs / ISPs to TRAI. However, consumers are requested to visit respective TSPs / ISPs website / customer care for latest applicable tariffs.</font></p></div>


                

            </div>
        </center>
    </form>
    
    
</body>
</html>

