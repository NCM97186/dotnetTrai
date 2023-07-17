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


public partial class zzRemoveDuplicates : System.Web.UI.Page
{
    MySqlCommand com, com1, com2;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno;
    DateTime dt1, dt2;
    double modrno;
    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        Server.ScriptTimeout = 9999999;
        tablename = "TRAI_tariffs";

        /*
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


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }
        */

    }








    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            int duplicates = 0;
            string mystr = "<table width=100% cellspacing=1 cellpadding=5><tr><td class=tablehead align=center>Count</td><td class=tablehead align=center>Unique ID</td></tr>";
            com = new MySqlCommand("select count(*) as num, uniqueid from TRAI_tariffs group by uniqueid order by num desc", con);
            con.Open();
            dr = com.ExecuteReader();
            while(dr.Read())
            {
                if(Convert.ToInt32(dr[0].ToString().Trim())>1)
                {
                    mystr += "<tr><td class=tablecell align=center><font color=red>" + dr[0].ToString().Trim() + "</font></td><td class=tablecell align=center><font color=red>" + dr[1].ToString().Trim() + "</font></td></tr>";
                    duplicates++;
                }
                else
                {
                    mystr += "<tr><td class=tablecell align=center>" + dr[0].ToString().Trim() + "</td><td class=tablecell align=center>" + dr[1].ToString().Trim() + "</td></tr>";
                }
            }
            con.Close();
            mystr += "</table>";

            divIDs.InnerHtml = mystr;

            LblTotal.Text = "( Duplicates : " + duplicates.ToString() + " )";
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }






    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {

            // if there are some records with duplicate unique ID's, keep 1 and send the rest to TRAI_duptariffs table
            // This code is being used on zzRemoveDuplicates.aspx and FEA_Login.aspx 

            try
            {
                com = new MySqlCommand("select count(*) as num, uniqueid from TRAI_tariffs group by uniqueid order by num desc", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    int mycount = Convert.ToInt32(dr[0].ToString().Trim());
                    string myid = dr["uniqueid"].ToString().Trim();
                    if (mycount > 1)  // if more than one records by same unique ID
                    {
                        com2 = new MySqlCommand("select count(*) from TRAI_tariffs where(uniqueid='" + myid + "')", con2);
                        con2.Open();
                        dr2 = com2.ExecuteReader();
                        dr2.Read();
                        if (Convert.ToInt32(dr2[0].ToString().Trim()) > 1)
                        {
                            // move all copies of this record to the TRAI_duplicates table
                            com1 = new MySqlCommand("insert into TRAI_duplicates select * from TRAI_tariffs where (uniqueid='" + myid + "')", con1);
                            con1.Open();
                            com1.ExecuteNonQuery();
                            con1.Close();

                            // delete top 1 copy of this record
                            com1 = new MySqlCommand("delete from TRAI_tariffs where uniqueid='" + myid + "' limit 1", con1);
                            con1.Open();
                            com1.ExecuteNonQuery();
                            con1.Close();
                        }
                        con2.Close();
                    }
                }
                con.Close();

                divresults.InnerHtml = "<font color=red size=4><b>All Duplicate Entries Have Been Removed</b></font>";
            }
            catch (Exception ex) { }

            // if there are some records with duplicate unique ID's, keep 1 and send the rest to TRAI_duptariffs table - CODE ENDS HERE

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }







    protected void getMaxRno(string myfield, string mytable)
    {
        try
        {

            com = new MySqlCommand("select count(*) from " + mytable, con1);
            con1.Open();
            dr = com.ExecuteReader();
            dr.Read();
            if (Convert.ToInt32(dr[0].ToString()) > 0)
            {
                com = new MySqlCommand("select max(" + myfield + ") from " + mytable, con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                zno = Convert.ToInt32(dr[0].ToString());
                zno = zno + 1;
                con.Close();
            }
            else
            {
                zno = 1;
            }
            con.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }







}
