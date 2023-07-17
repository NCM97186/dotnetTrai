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


public partial class TSP_recorddetails : System.Web.UI.Page
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

        tablename = "TRAI_tariffs";

        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("TSP_logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("TSP_logout.aspx");
            }

            double rno = Convert.ToDouble(Request["r"].ToString().Trim());
            string oper = Request["o"].ToString().Trim();
            string circ = Request["c"].ToString().Trim();


            string mystr = "<table width=95% cellspacing=1 cellpadding=5>";
            mystr = mystr + "<tr><td class=tablehead>Attribute Key</td><td class=tablehead>Attribute Value</td></tr>";

            int colno = 1;

            com = new MySqlCommand("select * from TRAI_tariffs where (rno=" + rno + ") and (oper='" + oper + "') and (circ='" + circ + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            for (int i = 4; i <= 281;i++)
            {
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell3 align=center><b>A" + colno.ToString().Trim() + "</b></td>";
                if (i == 16 || i == 17 || i == 19 || i == 20)
                {
                    mystr = mystr + "<td class=tablecell3b align=left style=min-width:150px; >" + Convert.ToDateTime(dr[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "") + "</td>";
                }
                else
                {
                    mystr = mystr + "<td class=tablecell3b align=left style=min-width:150px; >" + dr[i].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;") + "</td>";
                }
                mystr = mystr + "</tr>";
                colno++;
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
