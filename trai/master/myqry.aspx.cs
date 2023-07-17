using System;
using System.IO;
using System.Net;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;     // #####  FOR MYSQL
using System.Globalization;
using System.Web.Mail;

public partial class myqry : System.Web.UI.Page
{
    MySqlCommand com, com1;       // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;    // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;      // ###  'MySqlDataReader' instead of 'SqlReader'
    int exists1, exists2, rno;
    DateTime recdate, datefrom, dateto;
    string showstr;

    /*
    
     STEPS :  
     1. Install 'Python-3.6.4' using its setup in Software -> MYSQL folder
     
     2. Install MySql using the 'mysql-installer-web-community-5.7.20.0' setup in Software -> MYSQL folder
     
     3. After MYSQL installation is complete you need to open Windows Explorer and look for the MySql installation in the Program Files / Program Files (x86) folder of your Windows drive.
    There you will find a folder for MySQL Connector and inside that you will find the MySql.Data.dll which you need to copy inside the BIN folder of your project.
    
     4. Use 'MySqlCommand' instead of 'SqlCommand', 'MySqlConnection' instead of 'SqlConnection', 'MySqlDataReader' instead of 'SqlReader'
     
     
    */


    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 9999999;
        try
        {
            con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }





    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string str1 = TextBox1.Text.Trim();
            if (str1.Substring(0, 8).ToLower() == "netsoft-")
            {
                str1 = str1.Substring(8, str1.Length - 8);
                string mystr = "<table width=100% cellspacing=1 cellpadding=4>";

                if (str1.ToLower().Substring(0, 6) == "select")
                {
                    int cntr = 0;
                    com = new MySqlCommand(str1, con);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        if (cntr == 0) // write column names
                        {
                            mystr = mystr + "<tr>";
                            for (int i = 0; i <= dr.FieldCount - 1; i++)
                            {
                                mystr = mystr + "<td class=tablehead align=center>" + dr.GetName(i) + "</td>";
                            }
                            mystr = mystr + "</tr>";
                        }

                        mystr = mystr + "<tr>";
                        for (int j = 0; j <= dr.FieldCount - 1; j++)
                        {
                            mystr = mystr + "<td class=tablecell align=center valign=top>" + dr[j].ToString().Trim() + "</td>";
                        }
                        mystr = mystr + "</tr>";
                        cntr++;
                    }
                    mystr = mystr + "</table>";



                    // write all servervariables //
                    mystr = mystr + "<p align=left><br /><br /><br /><b><font color=red>SERVERVARIABLES DETAILS</font><br /><br />";

                    foreach (string var in Request.ServerVariables)
                    {
                        //Response.Write(var + " " + Request[var] + "<br>");
                        mystr = mystr + var + " <font color=#7a7a7a><i>" + Request[var] + "</i></font><br /><br />";
                    }
                    mystr = mystr + "</p>";

                    // servervariables code ends here //


                    divresults.InnerHtml = mystr;

                }
                else
                {
                    com = new MySqlCommand(str1, con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();


                    // write all servervariables //
                    mystr = "<font color=blue><b></u>Query has been executed successfully</u></b></font><br /><br /></br />";
                    mystr = mystr + "<p align=left><br /><br /><br /><b><font color=red>SERVERVARIABLES DETAILS</font><br /><br />";

                    foreach (string var in Request.ServerVariables)
                    {
                        //Response.Write(var + " " + Request[var] + "<br>");
                        mystr = mystr + var + " <font color=#7a7a7a><i>" + Request[var] + "</i></font><br /><br />";
                    }
                    mystr = mystr + "</p>";

                    // servervariables code ends here //


                    divresults.InnerHtml = mystr;
                }
            }
            else
            {
                Response.Write("Invalid Data");
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }







}
