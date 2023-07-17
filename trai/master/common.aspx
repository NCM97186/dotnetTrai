<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="common.aspx.cs" Inherits="common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
</head>
<body>
    <form id="form1" runat="server">
   <div style="min-height:520px; height:auto; height:520px;">
   <table width="100%" cellspacing="0" cellpadding="0">
   <tr>
   <td id="masterdiv" class="gentxt" style="padding:10px; line-height:5px;" align="justify">
   
   <table width="99%" cellspacing="1" cellpadding="5">
   <tr style="height:30px;">
   <td class="tablecellmenu" align="center"><b>Welcome <%=Request["user"].ToString().Trim() %></b></td>
   </tr>
   </table>
   
   <table cellspacing="0" cellpadding="0">
   <tr height="2"><td></td></tr>
   </table>
   
   <table width="99%" border="1" style="border-collapse:collapse;" cellspacing="1" cellpadding="2">
    
   <tr height="21" id="TrMasters" runat="server" visible="false">
    <td class="tablehead" align="center" colspan="2">Masters</td>
   </tr>
   <tr height="23" id="Tr1" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divUsers" runat="server" visible="false"><a href ="master_redirect.aspx?t1=masterusers.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">TRAI User Management</a></div></td>
   </tr>
   <tr height="23" id="Tr4" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divTSPLogins" runat="server" visible="false"><a href ="master_redirect.aspx?t1=masterTSPLogins.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">TSP / ISP User Management</a></div></td>
   </tr>
   <tr height="23" id="Tr5" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divFEALogins" runat="server" visible="false"><a href ="master_redirect.aspx?t1=masterFEAUsers.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">F&EA User Management</a></div></td>
   </tr>
   <tr height="23" id="Tr2" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divOperators" runat="server" visible="false"><a href ="master_redirect.aspx?t1=masteroperators.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">TSP / ISP Management</a></div></td>
   </tr>
   <tr height="23" id="Tr3" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divCircles" runat="server" visible="false"><a href ="master_redirect.aspx?t1=mastercircles.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">LSA Management</a></div></td>
   </tr>
   <tr height="23" id="Tr6" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divTTypes" runat="server" visible="false"><a href ="master_redirect.aspx?t1=masterttypes.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Tariff Product Management</a></div></td>
   </tr>
   

    <tr height="21">
    <td class="tablehead" align="center">General</td>
    </tr>
    <tr height="23" id="Tr91" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divFeedbacks" runat="server" visible="false"><a href ="master_redirect.aspx?t1=feedbacks.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Consumer View Feedbacks</a></div></td>
   </tr>
   <tr height="23" id="Tr92" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divDownloads" runat="server" visible="false"><a href ="master_redirect.aspx?t1=repdownloads.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Consumer View Downloads</a></div></td>
   </tr>
   <tr height="23" id="Tr93" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divHitCounter" runat="server" visible="false"><a href ="master_redirect.aspx?t1=rephitcounter.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Website Hit Counter</a></div></td>
   </tr>
   <tr height="23" id="Tr94" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divFDAmount" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_parameters.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">FD Amount</a></div></td>
   </tr>
   <tr height="23" id="Tr95" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divTariffCount" runat="server" visible="false"><a href ="master_redirect.aspx?t1=reptariffcount.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Tariff Count Report</a></div></td>
   </tr>

    <tr height="23">
        <td align="left" valign="middle" class="tablecellmenu"><a href="changeadminpass.aspx?user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Change Password</a></td>
    </tr>
    
    <tr height="21">
    <td class="tablecell3" align="center"><a href="logout.aspx?user=<%=Request["user"].ToString().Trim() %>&rand=<%= DateTime.Now.Minute.ToString().Trim() + DateTime.Now.Second.ToString().Trim() + DateTime.Now.Millisecond.ToString().Trim() %>" class ="indexlinks2" target ="_parent">Logout</a></td>
    </tr>

   </table>







    
    
    
    

    
    
    
    
    
    </td>
   </tr>
   </table>
   
    
    </div>
    </form>
</body>
</html>
