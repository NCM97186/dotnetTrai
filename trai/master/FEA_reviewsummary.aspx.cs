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


public partial class FEA_reviewsummary : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno;
    DateTime dt1, dt2;
    double modrno;
    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        tablename = "TRAI_tariffreviewlog";

        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("FEA_logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("FEA_logout.aspx");
            }

            
            string uid = Request["uid"].ToString().Trim();
            string oper = Request["o"].ToString().Trim().Replace("%20", " ");
            string circ = Request["c"].ToString().Trim().Replace("%20", " ");


            string mystr = "<table width=95% cellspacing=1 cellpadding=5>";
            mystr = mystr + "<tr><td class=tablehead colspan=5 align=center>Unique Record ID : " + uid + "</td></tr>";
            mystr = mystr + "<tr><td class=tablehead>Date / Time</td><td class=tablehead>Action Taken</td><td class=tablehead>Action By</td><td class=tablehead>Forwarded To</td><td class=tablehead>Remarks</td></tr>";

            int cntr = 0;
            com = new MySqlCommand("select * from " + tablename + " where(uniqueid='" + uid + "') order by rno",con);
            con.Open();
            dr = com.ExecuteReader();
            while(dr.Read())
            {
                string css = "tablecell2";
                if(cntr%2==0)
                {
                    css = "tablecell2b";
                }
                string act = dr["actiontaken"].ToString().Trim().Replace("Forward To", "Fwd To");
                if (dr["actiontaken"].ToString().Trim() != "Taken on Record")
                {
                    act = act + " - " + dr["forwardedto"].ToString().Trim();
                }
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=" + css + " align=left valign=top>" + Convert.ToDateTime(dr["recdate"].ToString().Trim()).ToString("dd-MMM-yyyy hh:mm:ss") + "</td>";
                mystr = mystr + "<td class=" + css + " align=left valign=top>" + act + "</td>";
                mystr = mystr + "<td class=" + css + " align=left valign=top>" + dr["forwardedby"].ToString().Trim() + "</td>";
                mystr = mystr + "<td class=" + css + " align=left valign=top>" + dr["forwardedto"].ToString().Trim() + "</td>";
                mystr = mystr + "<td class=" + css + " align=left valign=top>" + dr["remarks"].ToString().Trim() + "</td>";
                mystr = mystr + "</tr>";

                cntr++;
            }
            con.Close();

            mystr = mystr + "</table>";





            divresults.InnerHtml = mystr;



        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }







}
