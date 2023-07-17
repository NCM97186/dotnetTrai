<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_login.aspx.cs" Inherits="FEA_login" %>
<%@ Register Assembly="ncmcaptcha" Namespace="ncmcaptcha" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    <script type="text/javascript" src="js/jquerynew.min.js"></script>
    <script type="text/javascript" src="js/sha512.js"></script>
<script type="text/javascript" language="javascript">
        function getPass() {

            var exp = /((?=.*\d)(?=.*[a-zA-Z])(?=.*[@#$&]).{8,15})/;
            var exp1 = /^[A-Za-z0-9._-]{0,25}$/;
            var exp3 = /(^[\s\S]{0,2}$)/;
            var exp4 = /(^[0-9a-zA-Z ]+$)/;


            var value = document.getElementById('TextBox2').value;
            var value1 = document.getElementById('<%=TextBox1.ClientID%>').value;
            var value2 = document.getElementById('<%=txtCaptcha.ClientID%>').value;

            if (value1 == '') {
                alert("Please enter username");
                return false;
            }
            if (value1.search(exp1) == -1) {
                alert("Please enter valid User name");
                document.getElementById('<%=TextBox1.ClientID%>').focus();
                return false;
            }
            if (value == '') {
                alert('Please enter password');


                return false;
            }
            if (value2 == '') {
                alert('Please Enter Captcha Code');
                document.getElementById('<%=txtCaptcha.ClientID%>').focus();
                return false;
            }
           // if (value.search(exp) == -1) {
               // alert("Incorrect username or password.");
                //return false;
            }
            if (value2.search(exp4) == -1) {
                alert("Don't use any special characters in captcha");
                return false;
            }
            var salt = '<%=Session["salt"]%>';
            var hashnew = (hex_sha512(value) + salt);
            alert(hashnew);
            var hash2 = hex_sha512(hashnew);
            
        }
        function tablemain_onclick() {

        }

    </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">

  <asp:ScriptManager ID="SM1" runat="server"> </asp:ScriptManager>
    <div style="height:100vh;">
    
    
<center>

    <img src="images/logo-trai2.png" alt="TRAI" title="TRAI" style="margin-top:5px;" border="0" />
    <br /><br />
    <img src="images/otfrs.png" alt="Online Tariff Filing and Retrieval System" title="Online Tariff Filing and Retrieval System" border="0" />
    
    
    <br /><br /><br />
    
    <table width="350px" cellspacing="1" cellpadding="13" border="1" style="border-collapse:collapse;">
        <tr>
            <td colspan="2" align="center" class="tableheadcenter"><b>F&EA Division Login</b></td>
        </tr>
        <tr>
            <td width="50%" align="left" valign ="middle" class="tablecell2b">Username</td>
            <td align="left" valign ="middle" class="tablecell2b"><asp:TextBox ID="TextBox1" runat="server" CssClass="input" Width="150"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="left" valign ="middle" class="tablecell2b">Password</td>
            <td align="left" valign ="middle" class="tablecell2b"><asp:TextBox ID="TextBox2" runat="server" CssClass="input" Width="150" TextMode="Password"></asp:TextBox></td>
        </tr>

<tr>
           <asp:UpdatePanel ID="UP1" runat="server">

                        <ContentTemplate>
                                               <td align="left" valign ="middle" width="50%" class="tablecell2b">
                                               
																
																<asp:Image ID="imgCaptcha" runat="server" /><span><asp:ImageButton ID="btnRefresh" ImageUrl="~/master/images/refresh.png" runat="server" OnClick="btnRefresh_Click" /></span>
                                                    <label for="txtCaptcha">
                                                        
                                                       <span style="color:red">Note: Please enter the code shown above.</span> </label>
                                               </td>
                                                  
                                                  <td align="left" valign ="middle" class="tablecell2b">
                                                      <asp:TextBox ID="txtCaptcha" runat="server" AutoCompleteType="Disabled"  CssClass="input" Width="150"
                                                        MaxLength="6"></asp:TextBox>
<asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtCaptcha" errormessage="Please Enter Captcha Code!" />
    <br /><br />
                                                  </td>
                                                    
                                              
                            </ContentTemplate>
                            </asp:UpdatePanel>
        </tr>
        
    </table>
    <br />
    <asp:ImageButton ID="ImageButton1" runat="server" OnClick="Button1_Click" ImageUrl="images/submit2.jpg" OnClientClick="getPass();" />
    
    <br /><br /><br />

    <a href="FEA_forgot.aspx" runat="server" class="indexlinks1"><u>Forgot Password? Click Here</u></a>

    </center>
    
    </div>
    </form>
    
</body>
</html>