using System;
using System.Globalization;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;     // #####  FOR MYSQL
using System.Data.OleDb;
using System.Text;
using System.IO;


public partial class zzSampleException : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno, size;
    DateTime dt1, dt2;
    double modrno;


    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        //tablename = "TRAI_tarifferrorlog";

        try
        {
            /*
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("logout.aspx");
            }
            */




            mystr = "<table width=95% cellspacing=1 cellpadding=5>";
            mystr = mystr + "<tr><td colspan=7 align=right><img src=images/excel.jpg border=0></td></tr>";


            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Tariff Product</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Price (&#8377;)</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Validity (days)</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Unique Record ID</td>";
            mystr = mystr + "<td class=tablehead align=center width=15%>Flag Reason</td>";
            mystr = mystr + "<td class=tablehead align=center>Tariff Summary</td>";
            mystr = mystr + "<td class=tablehead align=center width=7%>Details</td>";
            mystr = mystr + "</tr>";

            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablecell3 align=center>Prepaid STV</td>";
            mystr = mystr + "<td class=tablecell3 align=center>110</td>";
            mystr = mystr + "<td class=tablecell3 align=center>25</td>";
            mystr = mystr + "<td class=tablecell3 align=center>BS06PMST0001</td>";
            mystr = mystr + "<td class=tablecell3 align=center>Reported 9 days after Launch</td>";
            mystr = mystr + "<td class=tablecell3 align=left>Daily first 120 secs @ 2p/sec then, all Local/STD calls @ 35p/min &amp; Daily first 2 SMS @ Re1/local or Rs1.5/national sms then, all Local/National SMS @ 30p/sms</td>";
            mystr = mystr + "<td class=tablecell3 align=center><img src=images/lensplus.png border=0></td>";
            mystr = mystr + "</tr>";

            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablecell3b align=center>Prepaid STV</td>";
            mystr = mystr + "<td class=tablecell3b align=center>120</td>";
            mystr = mystr + "<td class=tablecell3b align=center>28</td>";
            mystr = mystr + "<td class=tablecell3b align=center>BS06PMST0021</td>";
            mystr = mystr + "<td class=tablecell3b align=center>Reported 10 days after Launch</td>";
            mystr = mystr + "<td class=tablecell3b align=left>150 Local Idea Night Mins(11pm-8 am), Choice Recharge. Recharge Code *130*pin*1#</td>";
            mystr = mystr + "<td class=tablecell3b align=center><img src=images/lensplus.png border=0></td>";
            mystr = mystr + "</tr>";

            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablecell3 align=center>Prepaid STV</td>";
            mystr = mystr + "<td class=tablecell3 align=center>130</td>";
            mystr = mystr + "<td class=tablecell3 align=center>28</td>";
            mystr = mystr + "<td class=tablecell3 align=center>BS06PMST0013</td>";
            mystr = mystr + "<td class=tablecell3 align=center>Reported 11 days after Launch</td>";
            mystr = mystr + "<td class=tablecell3 align=left>All Local &amp; STD Mobile calls @ 39p/min till 300min, Post 300min 1.2p/sec, Validity 26 days</td>";
            mystr = mystr + "<td class=tablecell3 align=center><img src=images/lensplus.png border=0></td>";
            mystr = mystr + "</tr>";

            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablecell3b align=center>Prepaid STV</td>";
            mystr = mystr + "<td class=tablecell3b align=center>140</td>";
            mystr = mystr + "<td class=tablecell3b align=center>28</td>";
            mystr = mystr + "<td class=tablecell3b align=center>BS06PMST0023</td>";
            mystr = mystr + "<td class=tablecell3b align=center>Reported 10 days after Launch</td>";
            mystr = mystr + "<td class=tablecell3b align=left>	Roam Local / STD calls will be charged at Home Local / STD rates ( including benefits of Home STV Rate cutter / Unlimited packs/Bulk Minute /SMS pack)</td>";
            mystr = mystr + "<td class=tablecell3b align=center><img src=images/lensplus.png border=0></td>";
            mystr = mystr + "</tr>";



            mystr = mystr + "</table>";



            divTariffs.InnerHtml = mystr;

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }








    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }







    protected void TextDate_PreRender(object s, EventArgs e)
    {
        TextDate.Attributes.Add("onfocus", "showCalender(calender,this)");
    }


    protected void TextDate2_PreRender(object s, EventArgs e)
    {
        TextDate2.Attributes.Add("onfocus", "showCalender(calender,this)");
    }






}
