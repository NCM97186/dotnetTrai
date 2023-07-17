using System;
using System.Data;
using System.Configuration;
using System.Web.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;     // #####  FOR MYSQL
using System.IO;
using System.Text;
using System.Net;
using System.Xml.Schema;   // for generating XML
using System.Xml;          // for generating XML

public partial class webrequest_test : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    
    string links, pageurl;
    double sno, cntr;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

            string errstr = "";
            int errflag = 0;

            Response.ContentType = "text/xml";
            // Read XML posted via HTTP
            StreamReader reader = new StreamReader(Request.InputStream);
            String xmlData = reader.ReadToEnd();

            string headers = Request.Headers.ToString();
            string uname = Request.Headers["userid"].ToString();
            string pass = Request.Headers["password"].ToString();
            string authkey = Request.Headers["authkey"].ToString();

            if (uname != "TRAI")
            {
                errflag = 1;
                errstr=errstr + "Invalid User ID<br /><br />";
            }
            if (pass != "TESTING")
            {
                errflag = 1;
                errstr = errstr + "Invalid Password<br /><br />";
            }
            if (authkey != "ABCD1234")
            {
                errflag = 1;
                errstr = errstr + "Invalid Authorization Key<br /><br />";
            }

            if (xmlData =="")
            {
                errflag = 1;
                errstr = errstr + "XML File Stream Missing<br /><br />";
            }


            if(errflag==0)
            {
                //xmlData = xmlData.Replace("\r\n", "").Replace("'","&apos;").Replace("&","&amp;").Replace("<","&lt;").Replace(">","&gt;");
                xmlData = xmlData.Replace("\r\n", "").Replace("'", "&apos;").Replace("&", "&amp;");

                // Convert received data into xml file and store the file on server //

                string filename = "TestReceived.xml";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData);
                doc.Save(Server.MapPath(filename));

                // Convert received data into xml file and store the file on server - CODE ENDS HERE //


                /*
                // Now, Read data from the stored file and take necessary action (e.g. store in database) //

                XmlDocument doc2 = new XmlDocument();
                doc2.Load(Server.MapPath(filename));
                //Display all the 'Tariff' in the XML  
                XmlNodeList elemList = doc.GetElementsByTagName("Tariff");
                for (int i = 0; i < elemList.Count; i++)
                {
                    string myqry = "insert into trai_testlinking values(";

                    for (int j = 0; j < elemList[i].ChildNodes.Count; j++)
                    {
                        string nodetext = elemList[i].ChildNodes[j].InnerText;
                        myqry = myqry + "'" + nodetext + "',";
                    }
                    myqry = myqry.Substring(0, myqry.Length - 1);   //remove trailing comma
                    myqry = myqry + ")";

                                        
                    com = new MySqlCommand(myqry, con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                    
                }


                // Now, Read data from the stored file and take necessary action (e.g. store in database) - CODE ENDS HERE //
                */

            }


            if (errflag == 0)
            {
                Response.Write("Hello ! Your test linking is successful.");
            }
            else
            {
                Response.Write("The following errors were encountered during the test linking : <br /><br /><br />" + errstr);
            }


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }










}
