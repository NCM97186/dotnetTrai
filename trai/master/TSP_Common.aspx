<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="TSP_Common.aspx.cs" Inherits="TSP_Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
</head>
<body style="background-color:#f5f5f5;">
    <form id="form1" runat="server">
   <div style="min-height:520px; height:auto; height:520px;">
   <table width="100%" cellspacing="0" cellpadding="0">
   <tr>
   <td id="masterdiv" class="gentxt" style="padding:3px; line-height:5px;" align="justify">
   
   <table width="99%" cellspacing="1" cellpadding="5">
   <tr style="height:60px;">
   <td class="tablecellmenu" align="center" style="border:1px solid;"><b>Welcome <%=Request["user"].ToString().Trim() %></b></td>
   </tr>
   </table>
   
   <table cellspacing="0" cellpadding="0">
   <tr height="2"><td></td></tr>
   </table>
   
   <table width="99%" border="1" style="border-collapse:collapse;" cellspacing="1" cellpadding="5">
    
   <tr height="31" id="TrMasters" runat="server">
    <td class="tablehead" align="center" colspan="2">Tariff Section</td>
   </tr>
   <tr height="33" id="Tr1" runat="server">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divSubmission" runat="server"><a href ="master_redirect.aspx?t1=TSP_SubmissionReport.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Submission Report</a></div></td>
   </tr>
   <tr height="33" id="Tr2" runat="server">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divActiveRecords" runat="server"><a href ="master_redirect.aspx?t1=TSP_activerecords.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Active Records</a></div></td>
   </tr>
   
   <tr height="31" id="TrQuart" runat="server" visible="false">
    <td class="tablehead" align="center" colspan="2">Quarterly Data Section</td>
   </tr>
   <tr height="33" id="Tr4" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartData" runat="server"><a href ="master_redirect.aspx?t1=TSP_QuartData.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Quarterly Data Figures</a></div></td>
   </tr>
   <tr height="33" id="Tr5" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartBlack" runat="server"><a href ="master_redirect.aspx?t1=TSP_QuartBlack.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Black Out Days</a></div></td>
   </tr>
   <tr height="33" id="Tr3" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartPlans" runat="server"><a href ="master_redirect.aspx?t1=TSP_QuartPlans.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Tariff Plans (Not On Offer)</a></div></td>
   </tr>
   <tr height="33" id="Tr6" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartBulk" runat="server"><a href ="master_redirect.aspx?t1=TSP_QuartBulk.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Bulk Tariff Plans</a></div></td>
   </tr>
   


    <tr height="31">
    <td class="tablehead" align="center">General</td>
    </tr>
    <tr height="33">
        <td align="left" valign="middle" class="tablecellmenu"><a href="TSP_changepass.aspx?user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Change Password</a></td>
    </tr>
      
    <tr height="42">
    <td class="tablecell3" align="center"><a href="TSP_logout.aspx?user=<%=Request["user"].ToString().Trim() %>&rand=<%= DateTime.Now.Minute.ToString().Trim() + DateTime.Now.Second.ToString().Trim() + DateTime.Now.Millisecond.ToString().Trim() %>" class ="indexlinks2" target ="_parent">Logout</a></td>
    </tr>


   </table>







    
    
    
    

    
    
    
    
    
    </td>
   </tr>
   </table>
   
    
    </div>
    </form>
</body>
</html>
