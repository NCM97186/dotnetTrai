using System;
using System.Globalization;
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
using System.Data.OleDb;
using System.Text;
using System.Xml;
using System.Globalization;


public partial class zzCreateDummyXML : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    int rno, pos, pos2, rowcntr, zno, colsInSheet, tmpcntr;
    double flag, currflag; // flag is for total count, while currflag is to check whether current product in loop gave an error
    string tmp = "", blogo, filename, myopt, fileno, img2;
    string strerror, uniqueid, oper, circ, service, actiontotake, regprom, tablename, tariffsumm;
    string[] arrColsInSheet;
    XmlTextWriter writer;


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
        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        Server.ScriptTimeout = 999999;

        try
        {
           
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
            string feedname = "TestStructureXML.xml";
            writer = new XmlTextWriter(Server.MapPath(feedname), System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("TariffRecords");
            
            //com = new MySqlCommand("select * from TRAI_tariffrecords order by rno limit 2", con);   //  'limit 2' is mysql equivalent of 'top 2'
            //com = new MySqlCommand("select * from TRAI_tariffrecords where (uniqueid='IDGJPP01')", con);   
            //com = new MySqlCommand("select * from TRAI_tariffrecords_20MAR18 order by rno", con);   
            com = new MySqlCommand("select * from TRAI_tariffrecords order by rno", con);   
            con.Open();
            dr = com.ExecuteReader();
            while(dr.Read())
            {
                writer.WriteStartElement("Tariff");
                int pos = 1;
                for (int i = 4; i <= 11; i++)
                {
                    writer.WriteStartElement("A" + pos.ToString().Trim());
                    if (i == 4)
                    {
                        string ttype = dr[i].ToString().Trim();
                        ttype = ttype.Replace("Prepaid_Plan Voucher", "Prepaid Plan Voucher");
                        ttype = ttype.Replace("Prepaid_STV", "Prepaid STV");
                        ttype = ttype.Replace("Prepaid_Combo", "Prepaid Combo");
                        ttype = ttype.Replace("Prepaid_Top Up", "Prepaid Top Up");
                        ttype = ttype.Replace("Prepaid_VAS", "Prepaid VAS");
                        ttype = ttype.Replace("Promo Offer", "Prepaid Promo Offer");
                        ttype = ttype.Replace("Postpaid- Plan", "Postpaid Plan");
                        ttype = ttype.Replace("Postpaid- Add On Pack", "Postpaid Add On Pack");
                        ttype = ttype.Replace("Postpaid_VAS", "Postpaid VAS");
                        ttype = ttype.Replace("Fixed Line -Tariff", "Postpaid Fixed Line Plan");
                        ttype = ttype.Replace("Fixed Line Add-On Pack", "Postpaid Fixed Line Add On Pack");
                        ttype = ttype.Replace("Gen_ISD_Tariff", "Prepaid Gen ISD Tariff");
                        ttype = ttype.Replace("International_Roaming", "Prepaid International Roaming");

                        writer.WriteString(ttype);
                    }
                    else
                    {
                        writer.WriteString(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("01/01/2011 12:00:00 PM", "").Replace("12:00:00 PM", "").Replace("12:00:00", ""));
                    }
                    
                    writer.WriteEndElement();
                    pos++;
                }

                writer.WriteStartElement("A" + pos.ToString().Trim());
                writer.WriteString("LAUNCH");  // actiontotake
                writer.WriteEndElement();
                pos++;

                writer.WriteStartElement("A" + pos.ToString().Trim());
                writer.WriteString("");  // Revision ID
                writer.WriteEndElement();
                pos++;

                for (int i = 14; i <= 31; i++)
                {
                    writer.WriteStartElement("A" + pos.ToString().Trim());
                    if (i == 16 || i == 17 || i == 19 || i == 20)   // datetime fields
                    {
                        writer.WriteString(Convert.ToDateTime(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "")).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        writer.WriteString(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("01/01/2011 12:00:00 PM", "").Replace("12:00:00 PM", "").Replace("12:00:00", ""));
                    }
                    writer.WriteEndElement();
                    pos++;
                }

                for (int i = 34; i <= 41; i++)
                {
                    writer.WriteStartElement("A" + pos.ToString().Trim());
                    writer.WriteString(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("01/01/2011 12:00:00 PM", "").Replace("12:00:00 PM", "").Replace("12:00:00", ""));
                    writer.WriteEndElement();
                    pos++;
                }

                for (int i = 47; i <= 50; i++)
                {
                    writer.WriteStartElement("A" + pos.ToString().Trim());
                    writer.WriteString(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("01/01/2011 12:00:00 PM", "").Replace("12:00:00 PM", "").Replace("12:00:00", ""));
                    writer.WriteEndElement();
                    pos++;
                }

                for (int i = 53; i <= 290; i++)
                {
                    writer.WriteStartElement("A" + pos.ToString().Trim());
                    if (i == 16 || i == 17 || i == 19 || i == 20)   // datetime fields
                    {
                        writer.WriteString(Convert.ToDateTime(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "")).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                         writer.WriteString(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("01/01/2011 12:00:00 PM", "").Replace("12:00:00 PM", "").Replace("12:00:00", ""));
                    }
                    writer.WriteEndElement();
                    pos++;
                }
                

                writer.WriteEndElement();   // End of "Tariff" Tag
            }
            con.Close();

            writer.WriteEndElement();   // End of "TariffRecords" Tag

            writer.Close();


            Response.Write("<a href=TestStructureXML.xml>Download</a>");
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }








}
