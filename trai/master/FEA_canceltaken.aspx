<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_canceltaken.aspx.cs" Inherits="FEA_canceltaken" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
     <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    <script src="calendar/javascript/datepicker.js" type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="calendar/stylesheets/datepicker.css" />
    
    <script type="text/jscript" language="javascript">
        function funDel(uid)
        {
            document.getElementById('TextHidden').value = uid;
            document.getElementById('Button2').click();
        }
    </script>


</head>
<body>
<center >
    <form id="form1" runat="server">
    <div>
         <br />
        <br /><br />
       
        <div id="divDetails" runat="server" visible="false">
    
            <div id="divresult" runat="server"></div>
       
             <br /><br /><br />

            <hr size="0" />

        </div>
        

        <table width="50%" cellspacing="1" cellpadding="8">
            <tr>
            <td align="center" colspan="2" class="tablehead">Cancel 'Taken On Record' Status</td>
            </tr>
            <tr>
                <td align="center" class="tablecell3b" valign="top" width="50%">
                <br />
                    Date - Taken On Record</td>
                <td  class="tablecell3b" align="center" valign="top">
                <br />
                    <asp:TextBox ID="TextDate" MaxLength="30" runat="server" OnPreRender="TextDate_PreRender" width="100" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="Button1" runat="server" CssClass="input" OnClick="Button1_Click" Text="Show Details" />
                </td>
            </tr>
        </table>
    
        <br /><br />
        
        <p align="right"><asp:TextBox ID="TextHidden" runat="server" Width="1" Height="1" CssClass="input1"></asp:TextBox><asp:Button ID="Button2" runat="server" style="display:none;" OnClick="Button2_Click" /></p>
        
      </div>
    </form>
    </center>
</body>
</html>
