<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zzChangeFEAUnames.aspx.cs" Inherits="zzChangeFEAUnames" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <center>
            <br /><br />
            <b>Clicking on the below button will change the FEA usernames as per the new list provided by them.</b>
            <br /><br />
            <asp:Button ID="Button1" runat="server" Text="Change FEA Usernames" OnClick="Button1_Click" OnClientClick="return confirm('Are you sure you wish to change the FEA usernames ?');" />
            <br /><br /><br />
            <div id="divmsg" runat="server"></div>
        </center>
    
    </div>
    </form>
</body>
</html>
