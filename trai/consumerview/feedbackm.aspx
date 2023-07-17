<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="feedbackm.aspx.cs" Inherits="feedbackm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai7.css" type="text/css" media="all"/>
     
    <script language="Javascript" type="text/javascript">

        function onlyAlphabets(e, t) {
            try {
                flag = 0;
                str = document.getElementById('TextName').value;
                for (var i = 0; i < str.length; i++) {
                    charCode = str.charCodeAt(i);
                    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 32 || charCode == 46) {
                    }
                    else {
                        flag = 1;
                    }
                }
                if (flag == 1) {
                    alert('Please enter alphabets only.');
                    document.getElementById('TextName').value = '';
                    document.getElementById('TextName').focus();
                }


                /*
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 32 || charCode == 46)
                    return true;
                else
                    alert('Please enter alphabets only.');
                    return false;
                */

            }
            catch (err) {
                //alert(err.Description);
            }
        }


        function onlyNumbers(e, t) {
            flag = 0;
            str = t.value;
            if (str != '') {
                var isnum = /^\d+$/.test(str);
                if (isnum == false) {
                    alert('Please enter numbers only.');
                    t.value = '';
                    t.focus();
                }
            }
        }


        function alphanumeric(e,t)
        {
            flag = 0;
            var letterNumber = /^[0-9a-zA-Z .,/@#&]+$/;
            str = t.value;
            if(str.match(letterNumber)) 
            {
            }
            else
            { 
                flag = 1;
            }

            if (flag == 1 && str!='') {
                alert('Please enter alphanumeric value only.');
                t.value = '';
            }
        }

    </script>


</head>
<body style="background-image:url('images/bg4.jpg');">
    <form id="form1" runat="server" autocomplete="off">
    
        <center>
            <div style="width:98%; text-align:center; background-color:#ffffff;">
                
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

                <!--
                <div style="margin:0px;"><img src="images/topbnr.jpg" alt="TRAI" title="TRAI" border="0" /></div>
                -->


               
                <div style="width:100%; min-height:50px; margin:auto;">
                    <div style="float: left;margin:0px;margin-left:50px;"><img src="images/TRAI_logo.png" alt="TRAI" title="TRAI" width="60px" border="0" /></div>
                    <div style="float: right;margin:0px; margin-right:50px; margin-top:20px;"><font style="font-family:'Trebuchet MS'; font-size:15px;color:#053e7e;"><b>TARIFF</b></font></div>
                </div>

                <br /><br /><center>
                <table width="90%" cellspacing="1" cellpadding="9" id="Table1" runat="server">
                    <tr>
                        <td colspan="2" class="tablehead" align="center" valign="top"><font style="font-size:24px;">Please Enter Your Feedback</font></td>
                    </tr>
                    <tr>
                        <td class="tablehead" width="50%" align="left" valign="top"><font style="font-size:18px;">Name</font></td>
                        <td class="tablehead" align="left" valign="top"><asp:TextBox ID="TextName" runat="server" onblur="return onlyAlphabets(event,this);" CssClass="input" Width="300"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tablehead" align="left" valign="top"><font style="font-size:18px;">Mobile No.</font></td>
                        <td class="tablehead" align="left" valign="top"><table cellspacing="0" cellpadding="0"><tr><td align="left" valign="middle"><asp:TextBox ID="TextMobile" runat="server" onblur="return onlyNumbers(event,this);" CssClass="input" Width="150"></asp:TextBox></td><td align="left" valign="middle" width="20px"></td><td align="left" valign="middle"><asp:ImageButton ID="ImageOTP" runat="server" ImageUrl="images/otp.png" OnClick="ButtonOTP_Click" /></td></tr></table></td>
                    </tr>
                    <tr>
                        <td class="tablehead" align="left" valign="top"><font style="font-size:18px;">OTP</font><asp:TextBox ID="TextHidden" runat="server" Visible="false"></asp:TextBox></td>
                        <td class="tablehead" align="left" valign="top"><asp:TextBox ID="TextOTP" runat="server" onblur="return onlyNumbers(event,this);" CssClass="input" Width="150"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tablehead" align="left" valign="top"><font style="font-size:18px;">Feedback</font></td>
                        <td class="tablehead" align="left" valign="top"><asp:TextBox ID="TextFeedback" runat="server" onblur="return alphanumeric(event,this);" CssClass="input" Width="300" Height="70" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" class="tablecell">
                            <asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" ImageUrl="images/submit4.jpg" />
                        </td>
                    </tr>
                </table>
                
                <div id="divresults" runat="server" visible="false"><br /><br /><br /><br /><font style="font-size:18px; color:#5a5a5a;">Thank You. Your feedback has been received successfully.</font><br /><br /><br /><br /></div>
                

                </center>
                <br /><br /><br />

                <font style="font-size:18px;color:#5a5a5a;">Note : Feedback form is not for submitting complaints. It is for sending suggestions for improvements.<br /><br /></font>
            </div>
        </center>
    </form>
    
    
</body>
</html>

