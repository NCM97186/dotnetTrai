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


public partial class zzChangeFEAUnames : System.Web.UI.Page
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
            string[] olduname = new string[15];
            string[] newuname = new string[15];

            int cntr=0;
            
            olduname[cntr] = "ADV";
            newuname[cntr] = "Advisor-I";
            cntr++;

            olduname[cntr] = "Asstt-AB";
            newuname[cntr] = "Asstt-II";
            cntr++;

            olduname[cntr] = "Asstt-CPK";
            newuname[cntr] = "Asstt-III";
            cntr++;

            olduname[cntr] = "JA-AKD";
            newuname[cntr] = "JA-II";
            cntr++;

            olduname[cntr] = "JA-KVS";
            newuname[cntr] = "JA-I";
            cntr++;

            olduname[cntr] = "PRADV";
            newuname[cntr] = "Pr-Advisor";
            cntr++;

            olduname[cntr] = "RA-K";
            newuname[cntr] = "RA-III";
            cntr++;

            olduname[cntr] = "RA-S";
            newuname[cntr] = "RA-II";
            cntr++;

            olduname[cntr] = "RA-T";
            newuname[cntr] = "RA-I";
            cntr++;

            olduname[cntr] = "SO-B";
            newuname[cntr] = "SO-I";
            cntr++;

            olduname[cntr] = "SO-M";
            newuname[cntr] = "SO-II";
            cntr++;

            olduname[cntr] = "SRO-AJ";
            newuname[cntr] = "SRO-I";
            cntr++;

            /*
            olduname[cntr] = "SRO-KKP";
            newuname[cntr] = "SRO-IB";
            cntr++;
            */

            olduname[cntr] = "SRO-MG";
            newuname[cntr] = "SRO-III";
            cntr++;

            olduname[cntr] = "SRO-VKM";
            newuname[cntr] = "SRO-II";
            cntr++;



            string[] tbname = new string[20];
            string[] fldname = new string[20];
            int tbcntr = 0;

            tbname[tbcntr] = "TRAI_archive";
            fldname[tbcntr] = "currstaff";
            tbcntr++;

            tbname[tbcntr] = "TRAI_duplicates";
            fldname[tbcntr] = "currstaff";
            tbcntr++;

            tbname[tbcntr] = "TRAI_FEA";
            fldname[tbcntr] = "uname";
            tbcntr++;

            tbname[tbcntr] = "TRAI_tariffreviewlog";
            fldname[tbcntr] = "forwardedby";
            tbcntr++;

            tbname[tbcntr] = "TRAI_tariffreviewlog";
            fldname[tbcntr] = "forwardedto";
            tbcntr++;

            tbname[tbcntr] = "TRAI_tariffs";
            fldname[tbcntr] = "currstaff";
            tbcntr++;

            tbname[tbcntr] = "TRAI_tempReporting";
            fldname[tbcntr] = "uname";
            tbcntr++;



            for(int i=0;i<tbcntr;i++)
            {
                for(int j=0;j<cntr;j++)
                {
                    com = new MySqlCommand("update " + tbname[i] + " set " + fldname[i] + "='" + newuname[j] + "' where(" + fldname[i] + "='" + olduname[j] + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
            }


            string mystr = "<font color=red><b>Usernames have been updated</b></font><br /><br />List of currently existing usernames in various tables in which changes are done is as given below :<br /><br />";
            mystr = mystr + "<table width=100% cellspacing=1 border=1 style=border-collapse:collapse; cellpadding=5>";
            mystr=mystr + "<tr>";
            for (int i = 0; i < tbcntr; i++)
            {
                mystr = mystr + "<td><b>" + tbname[i] + " (" + fldname[i] + ")</b></td>";
            }
            mystr = mystr + "</tr>";
            mystr=mystr + "<tr>";
            for (int i = 0; i < tbcntr; i++)
            {
                string unames = "";
                com = new MySqlCommand("select distinct(" + fldname[i] + ") from " + tbname[i] + " order by " + fldname[i], con);
                con.Open();
                dr = com.ExecuteReader();
                while(dr.Read())
                {
                    unames = unames + dr[0].ToString().Trim() + "<br />";
                }
                con.Close();
                mystr = mystr + "<td valign=top align=left>" + unames + "</td>";
            }
            mystr = mystr + "</tr>";
            mystr = mystr + "</table>";

            divmsg.InnerHtml = mystr;

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }











}
