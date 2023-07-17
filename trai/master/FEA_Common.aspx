<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="FEA_Common.aspx.cs" Inherits="FEA_Common" %>

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
    <td class="tablehead" align="center" colspan="2">Review Section</td>
   </tr>
   <tr height="23" id="Tr1" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divReview" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_Review.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Review Tariffs</a></div></td>
   </tr>
   <tr height="23" id="Tr2" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divReset" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_canceltaken.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Cancel 'Taken on Record'</a></div></td>
   </tr>

    <tr height="21" id="TrReports" runat="server" visible="false">
    <td class="tablehead" align="center" colspan="2">Reports Section</td>
   </tr>
   <tr height="23" id="Tr51" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divTariffReport" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_reptariff.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Tariff Product Report</a></div></td>
   </tr>
    <tr height="23" id="Tr52" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divExceptionReport" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repexception.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Exception Report</a></div></td>
   </tr>
    <tr height="23" id="Tr53" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divDelayedReport" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repdelayed.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Delayed Reporting</a></div></td>
   </tr>
   <tr height="23" id="Tr54" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divComparisonReport" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repcomparison.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Comparison Report</a></div></td>
   </tr>
   <tr height="23" id="Tr55" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divAdvanceReport" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repadvance.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Advance Report</a></div></td>
   </tr>
   <tr height="23" id="Tr56" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divTariffSummaryReport" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_reptariffsummary.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Tariff Summary Report</a></div></td>
   </tr>
   <tr height="23" id="Tr57" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartPrepaid" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repQuartPrepaid.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Quarterly Prepaid Tariff Report</a></div></td>
   </tr>
   <tr height="23" id="Tr58" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartPostpaid" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repQuartPostpaid.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Quarterly Postpaid Tariff Report</a></div></td>
   </tr>
   <tr height="23" id="Tr59" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartBulk" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repQuartBulk.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Quarterly Bulk Tariff Plans Report</a></div></td>
   </tr>
   <tr height="23" id="Tr60" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartBlack" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repQuartBlack.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Black Out Days Report</a></div></td>
   </tr>
   <tr height="23" id="Tr61" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divQuartNotOnOffer" runat="server" visible="false"><a href ="master_redirect.aspx?t1=FEA_repQuartNotOnOffer.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Tariff Plans (Not On Offer) Report</a></div></td>
   </tr>
   

    <tr height="21">
    <td class="tablehead" align="center">General</td>
    </tr>
    <tr height="23" id="Tr91" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divFeedbacks" runat="server"><a href ="master_redirect.aspx?t1=feedbacks.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Consumer View Feedbacks</a></div></td>
    </tr>
    <tr height="23" id="Tr92" runat="server" visible="false">
       <td align="left" valign="middle" class="tablecellmenu"><div id="divDownloads" runat="server"><a href ="master_redirect.aspx?t1=repdownloads.aspx&user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Consumer View Downloads</a></div></td>
    </tr>
    <tr height="23">
        <td align="left" valign="middle" class="tablecellmenu"><a href="changeFEApass.aspx?user=<%=Request["user"].ToString().Trim() %>" class ="indexlinks_common" target ="Rightframe">Change Password</a></td>
    </tr>
           
    <tr height="21">
    <td class="tablecell3" align="center"><a href="FEA_logout.aspx?user=<%=Request["user"].ToString().Trim() %>&rand=<%= DateTime.Now.Minute.ToString().Trim() + DateTime.Now.Second.ToString().Trim() + DateTime.Now.Millisecond.ToString().Trim() %>" class ="indexlinks2" target ="_parent">Logout</a></td>
    </tr>


   </table>







    
    
    
    

    
    
    
    
    
    </td>
   </tr>
   </table>
   
    
    </div>
    </form>
</body>
</html>
