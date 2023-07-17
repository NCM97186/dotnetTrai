<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="myqry.aspx.cs" Inherits="myqry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <center>
            
            Qry : <asp:TextBox ID="TextBox1" runat="server" Width="1100" Height="100" TextMode="MultiLine"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" />
            <br /><br />

            <div id="divresults" runat="server"></div>

        </center>

    </div>
    </form>
</body>
</html>
 