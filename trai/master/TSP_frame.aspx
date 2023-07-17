<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false"  CodeFile="TSP_frame.aspx.cs" Inherits="TSP_frame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>

    <!-- Code to disable coming back on logout -->
    <script type="text/javascript">
        function noBack()
         {
             window.history.forward()
         }
        noBack();
        window.onload = noBack;
        window.onpageshow = function(evt) { if (evt.persisted) noBack() }
        window.onunload = function() { void (0) }
    </script>
    <!-- Code to disable coming back on logout - CODE ENDS HERE -->

</head>
        
<%
try
{ 
    string uname=Session["TSP"].ToString().Trim();   
%>
<frameset cols="20,80" border="0">
<frame name ="Leftframe" id="Leftframe" src="TSP_Common.aspx?user=<%=Session["TSP"].ToString().Trim() %>"></frame>
<frame name ="Rightframe" id="Rightframe" src ="blank.aspx?user=<%=Session["TSP"].ToString().Trim() %>"></frame>
</frameset>
<%
}
catch(Exception ex)
{
    Response.Write("Your session has expired. Please login again.");
} 
%>
</html>