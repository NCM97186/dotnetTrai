<%@ Page Language="C#" AutoEventWireup="true" EnableViewStateMac="false" ValidateRequest="false" CodeFile="masterusers.aspx.cs" Inherits="masterusers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Telecom Regulatory Authority Of India | Government Of India</title>
    <link rel="stylesheet" href="trai.css" type="text/css" media="all"/>
    
    <script type="text/javascript">
        function CheckAll(fmobj) {
            for (var i = 0; i < document.forms['form1'].elements.length; i++) {
                var e = document.forms['form1'].elements[i];
                if ((e.name != 'CheckBox1') && (e.name != 'ChkPastDate') && (e.name != 'ChkFutureDate') && (e.name != 'CheckOrderSel') && (!e.name.includes('TreeView1')) && (!e.name.includes('ChkSource')) && (!e.name.includes('ChkDestination')) && (e.type == 'checkbox') && (!e.disabled)) {
                    e.checked = document.forms['form1'].CheckBox1.checked;
                }
            }
        }
    </script>
      
    <script type="text/javascript">
        function unchk() {
            document.forms['form1'].CheckBox1.checked = false;
        }
    </script>
    
    <script type="text/javascript">
        function funDelRecord(myval) {
            document.getElementById('TextHidden').value = myval;
            document.getElementById('Button3').click();
        }
    </script>



    <!-- Code for checking sub-checkboxes on clicing parent checkbox in the TreeView -->
    <script type="text/javascript" src="js/jquerynew.min.js"></script>
   <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquerynew.min.js"></script>--%>
<script type="text/javascript">
    $(function () {
        $("[id*=TreeView1] input[type=checkbox]").bind("click", function () {
            var table = $(this).closest("table");
            if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                //Is Parent CheckBox
                var childDiv = table.next();
                var isChecked = $(this).is(":checked");
                $("input[type=checkbox]", childDiv).each(function () {
                    if (isChecked) {
                        $(this).attr("checked", "checked");
                    } else {
                        $(this).removeAttr("checked");
                    }
                });
            } else {
                //Is Child CheckBox
                var parentDIV = $(this).closest("DIV");
                if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                    $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                } else {
                    $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                }
            }
        });
    })

    </script>

    <!-- Code for checking sub-checkboxes on clicing parent checkbox in the TreeView - CODE ENDS HERE -->


</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center><p align="right"><asp:TextBox ID="TextHidden" runat="server" width="1" Height="1" CssClass="input1"></asp:TextBox><asp:TextBox ID="TextDelNo" runat="server" width="1" Height="1" CssClass="input1"></asp:TextBox></p>
        <table width="95%" cellspacing="1" cellpadding="5">
        <tr>
        <td colspan="2" class="tablehead" align="center">TRAI User Accounts</td>
        </tr>
        <tr>
        <td class="tablecell4" align="right" width="50%">Username <font color="red"><b>*</b></font></td>
        <td class="tablecell4" align="left"><asp:TextBox ID="TextBox3" runat="server" CssClass="input"></asp:TextBox></td>
        </tr>
        <tr>
        <td class="tablecell4" align="right">Password <font color="red"><b>*</b></font></td>
        <td class="tablecell4" align="left"><asp:TextBox ID="TextBox2" runat="server" CssClass="input"></asp:TextBox></td>
        </tr>
        <tr>
        <td class="tablecell4" align="right">Email ID <font color="red"><b>*</b></font></td>
        <td class="tablecell4" align="left"><asp:TextBox ID="TextBox4" runat="server" CssClass="input"></asp:TextBox></td>
        </tr>
        
        <tr id="trusername" runat="server" visible="false">
        <td class="tablecell4" width="50%" align="center">Username</td>
        <td class="tablecell4" align="center"><asp:TextBox ID="TextBox1" runat="server" CssClass="input"></asp:TextBox></td>
        </tr>
        <tr>
        <td colspan="2" class="tablehead" align="center">Menu Access Granted For</td>
        </tr>
        <tr>
        <td class="tablecell4" colspan="2" align="center">
            <br />
            <asp:CheckBox ID="CheckBox1" runat="server" Text="<b>All Options</b>" onClick="CheckAll(document.form1);" />
             <br /><br />
             <b>OR</b>
             <br /><br />
           

            <table width="100%" cellspacing="1" cellpadding="5">
                <tr>
                    <td class="tableheadcenter">Menu Options</td>
                    <td class="tableheadcenter">Addition</td>
                    <td class="tableheadcenter">Modification</td>
                    <td class="tableheadcenter">Deletion</td>
                </tr>
                <tr>
                    <td colspan="4" class="tablehead" align="center">Master Module</td>
                </tr>
                <tr>
                    <td class="tablecell4b">TRAI User Accounts</td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="masterusers_Add" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="masterusers_Mod" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="masterusers_Del" runat="server" Text="" onClick="unchk();" /></td>
                </tr>
                <tr>
                    <td class="tablecell4">TSP / ISP Management</td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="masteroperators_Add" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="masteroperators_Mod" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="masteroperators_Del" runat="server" Text="" onClick="unchk();" /></td>
                </tr>
                <tr>
                    <td class="tablecell4b">Circles Management</td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="mastercircles_Add" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="mastercircles_Mod" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="mastercircles_Del" runat="server" Text="" onClick="unchk();" /></td>
                </tr>
                <tr>
                    <td class="tablecell4">TSP / ISP User Management</td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="masterTSPLogins_Add" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="masterTSPLogins_Mod" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="masterTSPLogins_Del" runat="server" Text="" onClick="unchk();" /></td>
                </tr>
                <tr>
                    <td class="tablecell4b">F&EA User Management</td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="FEALogins_Add" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="FEALogins_Mod" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4b" align="center" valign="top"><asp:CheckBox ID="FEALogins_Del" runat="server" Text="" onClick="unchk();" /></td>
                </tr>
                <tr>
                    <td class="tablecell4">Tariff Product Management</td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="TTypes_Add" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="TTypes_Mod" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="TTypes_Del" runat="server" Text="" onClick="unchk();" /></td>
                </tr>
                <tr>
                    <td class="tablecell4">FD Amount Management</td>
                    <td class="tablecell4" align="center" valign="top"></td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="FDAmount" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                </tr>
                
                <tr>
                    <td colspan="4" class="tablehead" align="center">Reports - Vewing Rights</td>
                </tr>
                <tr>
                    <td class="tablecell4">Consumer View Feedbacks</td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="ReportFeedbacks" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                </tr>
                <tr>
                    <td class="tablecell4">Consumer View Downloads</td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="ReportDownloads" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                </tr>
                <tr>
                    <td class="tablecell4">Website Hit Counter</td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="ReportHitCounter" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                </tr>
                <tr>
                    <td class="tablecell4">Tariff Count Report</td>
                    <td class="tablecell4" align="center" valign="top"><asp:CheckBox ID="ReportTariffCount" runat="server" Text="" onClick="unchk();" /></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                    <td class="tablecell4" align="center" valign="top"></td>
                </tr>
                
           

           
           <tr>
           <td colspan="4" bgcolor="#ffffff" align="center"><br /><br />
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Add" Width="67px" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ImageButton1" runat="server" OnClick="Button2_Click" Text="Modify" Width="67px" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ButtonClear" runat="server" Text="Cancel" OnClick="ButtonClear_Click" />
           </td>
           </tr>
           
           </table>  
        
        </td>
        </tr>
        </table>
        
        <br /><hr size="0" /><br /><br />
        
        <div id="divresults" runat="server"></div>
      
      <p align="right"><asp:Button ID="Button3" runat="server" Text="" Width="1" Height="1" CssClass="input1" OnClick="Button3_Click" OnClientClick="return confirm('Are you sure you wish to delete this record ?');" /></p>  
        
        
        </center>
    </div>
    </form>
</body>
</html>
