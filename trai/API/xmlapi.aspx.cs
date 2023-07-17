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
using System.Security.Cryptography;   // #######  For MD5 encryption
using System.Globalization;  // for CultureInfo

public partial class xmlapi : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'

    string links, pageurl, tablename, validTSPnames, validTSPcodes, validLSAnames, validLSAcodes, validTTypenames, validTTypecodes, validservicetypes, validcategories, validactions, myqry, errstr1, filename, errcode;
    double sno, cntr;
    int zno, errflag, submitcount;
    XmlNodeList elemList;
    DateTime recdate;


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

            Server.ScriptTimeout = 999999;

            //tablename="TRAI_TestXMLInput";
            //tablename = "TRAI_Tariffrecords";
            tablename = "TRAI_tariffs";

            submitcount = 0;
            validservicetypes = ",LANDLINE,BROADBAND,BOTH,GSM,CDMA,LTE,";
            validcategories = ",PREPAID,POSTPAID,ADVANCE RENTAL,";
            validactions = ",LAUNCH,REVISION,CORRECTION,WITHDRAWAL,";
            validTTypenames = ",";
            validTTypecodes = ",";
            validLSAnames = ",";
            validLSAcodes = ",";
            validTSPnames = ",";
            validTSPcodes = ",";

            com = new MySqlCommand("select * from TRAI_ttypes order by rno", con);
            con.Open();
            dr = com.ExecuteReader();
            while(dr.Read())
            {
                validTTypenames = validTTypenames + dr["ttype"].ToString().Trim() + ",";
                validTTypecodes = validTTypecodes + dr["tcode"].ToString().Trim() + ",";
            }
            con.Close();

            com = new MySqlCommand("select * from TRAI_operators order by rno", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                validTSPnames = validTSPnames + dr["oper"].ToString().Trim() + ",";
                validTSPcodes = validTSPcodes + dr["opcode"].ToString().Trim() + ",";
            }
            con.Close();

            com = new MySqlCommand("select * from TRAI_circles order by rno", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                validLSAnames = validLSAnames + dr["circ"].ToString().Trim() + ",";
                validLSAcodes = validLSAcodes + dr["ccode"].ToString().Trim() + ",";
            }
            con.Close();





            //string errstr1 = "";
            errstr1 = "<table width=100% cellspacing=1 border=1 style=border-collapse:collapse; cellpadding=5><tr><td align=center valign=top width=15%><b>Unique Record ID</b></td><td align=center valign=top width=15%><b>Submission Status</b></td><td align=center valign=top width=15%><b>Success / Error Code</b></td><td align=center valign=top width=15%><b>Error Attribute Key</b></td><td align=center valign=top><b>Error Description</b></td></tr>";
            errflag = 0;
            recdate = DateTime.Now;

            Response.ContentType = "text/xml";
            // Read XML posted via HTTP
            StreamReader reader = new StreamReader(Request.InputStream);
            String xmlData = reader.ReadToEnd();

            string headers = Request.Headers.ToString();
            string uname = Request.Headers["userid"].ToString();
            string pass = Request.Headers["password"].ToString();
            string authkey = Request.Headers["authkey"].ToString();
            string useroperator = "";
            string authkeyMD5 = "";
            

            com = new MySqlCommand("select count(*) from TRAI_TSPUsers where(uname='" + uname + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            if(Convert.ToInt32(dr[0].ToString().Trim())==0)
            {
                errflag = 1;
                errstr1 = errstr1 + "<tr><td align=left valign=top></td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>000001</td><td align=left valign=top></td><td align=left valign=top>Invalid User ID</td></tr>";
            }
            con.Close();

            if(errflag==0)
            {
                com = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + uname + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    useroperator = dr["oper"].ToString().Trim();
                    if(pass!=dr["pass"].ToString().Trim())
                    {
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top></td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>000002</td><td align=left valign=top></td><td align=left valign=top>Invalid Password</td></tr>";
                    }

                    authkeyMD5 = encryption(authkey);
                    if(authkeyMD5!=dr["authkeyMD5"].ToString().Trim())
                    {
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top></td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>000003</td><td align=left valign=top></td><td align=left valign=top>Invalid Authorization Key</td></tr>";
                    }
                }
                catch(Exception ex)
                {
                    errflag = 1;
                    errstr1 = errflag + "<tr><td align=left valign=top></td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>000002</td><td align=left valign=top></td><td align=left valign=top>" + ex.ToString() + "</td></tr>";
                }
                con.Close();
            }


            if (xmlData == "")
            {
                errflag = 1;
                errstr1 = errstr1 + "<tr><td align=left valign=top></td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>000004</td><td align=left valign=top></td><td align=left valign=top>XML File Stream Missing</td></tr>";
            }


            if (errflag == 0)
            {
                xmlData = xmlData.Replace("\r\n", "").Replace("'", "&apos;").Replace("&", "&amp;");

                // Convert received data into xml file and store the file on server //

                filename = uname + "_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".xml";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData);
                doc.Save(Server.MapPath("XML/" + filename));

                // Convert received data into xml file and store the file on server - CODE ENDS HERE //


                
                // Now, Read data from the stored file and take necessary action (e.g. store in database) //

                XmlDocument doc2 = new XmlDocument();
                doc2.Load(Server.MapPath("XML/" + filename));
                //Display all the 'Tariff' in the XML  
                elemList = doc.GetElementsByTagName("Tariff");
                for (int i = 0; i < elemList.Count; i++)
                {
                    errflag = 0;
                    errcode = "";

                    getMaxRno("rno",tablename);
                    myqry = "insert into " + tablename + " values('" + zno + "','" + filename + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + uname + "',";


                    // check unique ID validations //
                    string uniqueid = elemList[i].ChildNodes[2].InnerText.ToString().Trim().ToUpper();
                    string revisionID = elemList[i].ChildNodes[9].InnerText.ToString().Trim().ToUpper();
                    string actiontotake = elemList[i].ChildNodes[8].InnerText.ToString().Trim().ToUpper();

                    if (uniqueid == "")
                    {
                        errcode = "003005";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top></td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>No Unique Record ID provided</td></tr>";
                    }
                    else
                    {
                        if (uniqueid.Length != 12)
                        {
                            errcode = "003006";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>Unique ID needs to be a 12 character string as per predefined format</td></tr>";
                        }
                        else
                        {
                            string recTSPcode = uniqueid.Substring(0, 2);
                            if (!validTSPcodes.Contains("," + recTSPcode + ","))
                            {
                                errcode = "003007";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The first two characters of the Unique Record ID need to be your TSP code.</td></tr>";
                            }
                            string recLSAcode = uniqueid.Substring(2, 2);
                            if (!validLSAcodes.Contains("," + recLSAcode + ","))
                            {
                                errcode = "003008";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The third and fourth characters of the Unique Record ID need to be the LSA code.</td></tr>";
                            }
                            string recProductCode = uniqueid.Substring(4, 4);
                            if (!validTTypecodes.Contains("," + recProductCode + ","))
                            {
                                errcode = "003009";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The 5-8 characters of the Unique Record ID need to be the Tariff Product code.</td></tr>";
                            }
                        }
                    }

                    if (uniqueid != "" && actiontotake.ToUpper() == "LAUNCH")
                    {
                        com = new MySqlCommand("select count(*) from " + tablename + " where(uniqueid='" + uniqueid + "') and (oper='" + useroperator + "')", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        try
                        {
                            if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
                            {
                                errcode = "003010";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The Unique Record ID already Exists</td></tr>";
                            }
                        }
                        catch (Exception ex) { }
                        con.Close();
                        if (errflag == 0)
                        {
                            com = new MySqlCommand("select count(*) from TRAI_archive where(uniqueid='" + uniqueid + "') and (oper='" + useroperator + "')", con);
                            con.Open();
                            dr = com.ExecuteReader();
                            dr.Read();
                            try
                            {
                                if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
                                {
                                    errcode = "003010";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The Unique Record ID already Exists</td></tr>";
                                }
                            }
                            catch (Exception ex) { }
                            con.Close();
                        }
                    }
                    if (uniqueid != "" && actiontotake.ToUpper() == "REVISION")
                    {
                        com = new MySqlCommand("select count(*) from " + tablename + " where(uniqueid='" + revisionID + "') and (oper='" + useroperator + "')", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        try
                        {
                            if(Convert.ToInt32(dr[0].ToString().Trim())>0)
                            {
                                errcode = "010011";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A10</td><td align=left valign=top>The Revision ID already Exists</td></tr>";
                            }
                        }
                        catch (Exception ex) { }
                        con.Close();
                        if (errflag == 0)
                        {
                            com = new MySqlCommand("select count(*) from TRAI_archive where(uniqueid='" + revisionID + "') and (oper='" + useroperator + "')", con);
                            con.Open();
                            dr = com.ExecuteReader();
                            dr.Read();
                            try
                            {
                                if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
                                {
                                    errcode = "010011";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A10</td><td align=left valign=top>The Revision ID already Exists</td></tr>";
                                }
                            }
                            catch (Exception ex) { }
                            con.Close();
                        }
                    }
                    if (uniqueid != "" && (actiontotake.ToUpper() == "WITHDRAWAL" || actiontotake.ToUpper() == "CORRECTION"))
                    {
                        com = new MySqlCommand("select count(*) from " + tablename + " where(uniqueid='" + uniqueid + "') and (oper='" + useroperator + "')", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        try
                        {
                            if (Convert.ToInt32(dr[0].ToString().Trim()) == 0)
                            {
                                errcode = "003013";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The Unique Record ID for " + actiontotake + " does not exist</td></tr>";
                            }
                        }
                        catch (Exception ex) { }
                        con.Close();
                    }

                    // check ttype //
                    //string ttype = elemList[i].ChildNodes[0].InnerText.ToString().Trim();
                    string ttype = elemList[i].ChildNodes[0].InnerText.ToString().Trim().Replace("PrepaidInternational", "Prepaid International").Replace("PostpaidInternational", "Postpaid International");
                    if (ttype == "")
                    {
                        errcode = "001014";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A1</td><td align=left valign=top>No Tariff Product Type provided</td></tr>";
                    }
                    else
                    {
                        if (!validTTypenames.ToUpper().Contains("," + ttype.ToUpper() + ","))
                        {
                            errcode = "001015";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A1</td><td align=left valign=top>Invalid Tariff Product Type</td></tr>";
                        }
                    }

                    // check tariff summary //
                    string tariffdet = elemList[i].ChildNodes[1].InnerText.ToString().Trim().Replace("'","`");
                    if (tariffdet=="")
                    {
                        errcode = "002016";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A2</td><td align=left valign=top>Tariff Summary cannot be left blank</td></tr>";
                    }
                    

                    // check operator validation //
                    if (useroperator == "")
                    {
                        errcode = "004017";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A4</td><td align=left valign=top>No TSP Name provided</td></tr>";
                    }
                    else
                    {
                        if (useroperator.ToUpper() != elemList[i].ChildNodes[3].InnerText.ToString().Trim().ToUpper())
                        {
                            errcode = "004018";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A4</td><td align=left valign=top>Invalid TSP Name</td></tr>";
                        }
                    }

                    // check circle validation //
                    string circ=elemList[i].ChildNodes[4].InnerText.ToString().Trim().ToUpper();
                    if (circ == "")
                    {
                        errcode = "005019";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A5</td><td align=left valign=top>LSA is compulsory</td></tr>";
                    }
                    else
                    {
                        ArrayList list = new ArrayList();
                        list.AddRange(circ.Split(new char[] { ',' }));
                        for (int j = 0; j < list.Count; j++)
                        {
                            com = new MySqlCommand("select count(*) from TRAI_circles where(upper(circ)='" + list[j].ToString().Trim() + "')", con);
                            con.Open();
                            dr = com.ExecuteReader();
                            dr.Read();
                            try
                            {
                                if (Convert.ToInt32(dr[0].ToString().Trim()) == 0)
                                {
                                    errcode = "005020";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A5</td><td align=left valign=top>Invalid LSA Name</td></tr>";
                                }
                            }
                            catch (Exception ex) { }
                            con.Close();
                        }
                    }
                    
                    // check service type //
                    string servicetype = elemList[i].ChildNodes[6].InnerText.ToString().Trim().ToUpper().Replace("/",",");
                    if (servicetype == "")
                    {
                        errcode = "007021";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A7</td><td align=left valign=top>Type of Service  is compulsory</td></tr>";
                    }
                    else
                    {
                        if (servicetype != "ISP")
                        {
                            ArrayList list = new ArrayList();
                            list.AddRange(servicetype.Split(new char[] { ',' }));
                            for (int j = 0; j < list.Count; j++)
                            {
                                if (!validservicetypes.ToUpper().Contains("," + list[j].ToString().ToUpper() + ","))
                                {
                                    errcode = "007022";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A7</td><td align=left valign=top>Invalid Type of Service</td></tr>";
                                }
                            }
                        }
                    }
                    

                    // check category //
                    string categ = elemList[i].ChildNodes[7].InnerText.ToString().Trim();
                    if (categ == "")
                    {
                        errcode = "008023";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A8</td><td align=left valign=top>Prepaid / Postpaid  is compulsory</td></tr>";
                    }
                    else
                    {
                        if (!validcategories.ToUpper().Contains("," + categ.ToUpper() + ","))
                        {
                            errcode = "008024";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A8</td><td align=left valign=top>Prepaid / Postpaid not specified</td></tr>";
                        }
                    }

                    // check actiontotake //
                    if (actiontotake == "")
                    {
                        errcode = "009025";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A9</td><td align=left valign=top>Launch / Revision / Correction / Withdrawal is compulsory</td></tr>";
                    }
                    else
                    {
                        if (!validactions.ToUpper().Contains("," + actiontotake.ToUpper() + ","))
                        {
                            errcode = "009026";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A9</td><td align=left valign=top>Please specify whether Launch / Revision / Correction / Withdrawal</td></tr>";
                        }
                    }
                    

                    // check Revision ID, if its a 'Revision' record //
                    if (actiontotake.ToUpper().Trim()=="REVISION" && revisionID=="")
                    {
                        errcode = "010027";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A10</td><td align=left valign=top>Please enter a Revision Record ID No.</td></tr>";
                    }

                    if (revisionID != "")
                    {
                        if (revisionID.Length != 12)
                        {
                            errcode = "003006";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>Revision ID needs to be a 12 character string as per predefined format</td></tr>";
                        }
                        else
                        {
                            string recTSPcode = revisionID.Substring(0, 2);
                            if (!validTSPcodes.Contains("," + recTSPcode + ","))
                            {
                                errcode = "003007";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The first two characters of the Revision ID need to be your TSP code.</td></tr>";
                            }
                            string recLSAcode = revisionID.Substring(2, 2);
                            if (!validLSAcodes.Contains("," + recLSAcode + ","))
                            {
                                errcode = "003008";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The third and fourth characters of the Revision ID need to be the LSA code.</td></tr>";
                            }
                            string recProductCode = revisionID.Substring(4, 4);
                            if (!validTTypecodes.Contains("," + recProductCode + ","))
                            {
                                errcode = "003009";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The 5-8 characters of the Revision ID need to be the Tariff Product code.</td></tr>";
                            }
                        }
                        
                        com = new MySqlCommand("select count(*) from " + tablename + " where(uniqueid='" + uniqueid + "') and (oper='" + useroperator + "')", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        try
                        {
                            if (Convert.ToInt32(dr[0].ToString().Trim()) == 0)
                            {
                                errcode = "003013";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A3</td><td align=left valign=top>The Unique Record ID to revise does not exist</td></tr>";
                            }
                        }
                        catch (Exception ex) { }
                        con.Close();
                    }



                    // check Name of Plan //
                    string productname = elemList[i].ChildNodes[10].InnerText.ToString().Trim();
                    if (productname == "")
                    {
                        if (!ttype.ToUpper().Contains("GEN ISD TARIFF") && !ttype.ToUpper().Contains("INTERNATIONAL ROAMING"))
                        {
                            errcode = "011028";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A11</td><td align=left valign=top>Name of the product is compulsory</td></tr>";
                        }
                    }
                    

                    // check Date of Reporting //
                    DateTime reportdate = Convert.ToDateTime("2001-01-01");
                    try
                    {
                        if (elemList[i].ChildNodes[12].InnerText.ToString().Trim() == "")
                        {
                            errcode = "013029";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A13</td><td align=left valign=top>Date of Reporting is compulsory</td></tr>";
                        }
                        else
                        {
                            reportdate = Convert.ToDateTime(elemList[i].ChildNodes[12].InnerText.ToString().Trim());
                            if (Convert.ToDateTime(reportdate.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")))
                            {
                                errcode = "013030";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A13</td><td align=left valign=top>Date of Reporting cannot be greater / less than current date</td></tr>";
                            }
                            if (Convert.ToDateTime(reportdate.ToString("dd-MMM-yyyy")) < Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")))
                            {
                                errcode = "013030";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A13</td><td align=left valign=top>Date of Reporting cannot be greater / less than current date</td></tr>";
                            }
                            // check if date passed in "dd-MMM-yyyy" format
                            DateTime dt;
                            string[] formats = { "dd-MMM-yyyy", "dd-MMM-yyyy" };
                            if (!DateTime.TryParseExact(elemList[i].ChildNodes[12].InnerText.ToString().Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                            {
                                errcode = "013031";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A13</td><td align=left valign=top>Date of Reporting should be in dd-MMM-yyyy format</td></tr>";
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        errcode = "013032";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A13</td><td align=left valign=top>Invalid Date of Reporting</td></tr>";
                    }


                    // check Date of action //
                    DateTime actiondate = Convert.ToDateTime("2001-01-01");
                    try
                    {
                        if (elemList[i].ChildNodes[13].InnerText.ToString().Trim() == "")
                        {
                            errcode = "014033";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A14</td><td align=left valign=top>Date of Launch / Revision / Correction / Withdrawal is compulsory</td></tr>";
                        }
                        else
                        {
                            actiondate = Convert.ToDateTime(elemList[i].ChildNodes[13].InnerText.ToString().Trim());
                            if (Convert.ToDateTime(actiondate.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"))) 
                            {
                                errcode = "014034";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A14</td><td align=left valign=top>Date of Launch / Revision / Correction / Withdrawal cannot be greater than current date</td></tr>";
                            }
                            // check if date passed in "dd-MMM-yyyy" format
                            DateTime dt;
                            string[] formats = { "dd-MMM-yyyy", "dd-MMM-yyyy" };
                            if (!DateTime.TryParseExact(elemList[i].ChildNodes[13].InnerText.ToString().Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                            {
                                errcode = "014035";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A14</td><td align=left valign=top>Date of Launch / Revision / Correction / Withdrawal should be in dd-MMM-yyyy format</td></tr>";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errcode = "014036";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A14</td><td align=left valign=top>Invalid Date of Launch / Revision / Correction / Withdrawal</td></tr>";
                    }

                    // Date of action cannot be greater than Date of reporting
                    if (Convert.ToDateTime(actiondate.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(reportdate.ToString("dd-MMM-yyyy")))
                    {
                        errcode = "014037";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A14</td><td align=left valign=top>Date of Launch / Revision / Correction / Withdrawal cannot be greater than date of reporting</td></tr>";
                    }

                    // Regular / Promotional //
                    string regprom = elemList[i].ChildNodes[14].InnerText.ToString().Trim();
                    if(regprom=="")
                    {
                        if (ttype.ToUpper().Contains("PROMO OFFER"))
                        {
                            regprom = "PROMOTIONAL";
                        }
                        if (ttype.ToUpper().Contains("GEN ISD TARIFF") || ttype.ToUpper().Contains("INTERNATIONAL ROAMING"))
                        {
                            regprom = "REGULAR";
                        }
                    }
                    if (regprom == "")
                    {
                        errcode = "015038";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A15</td><td align=left valign=top>Regular / Promotional is compulsory</td></tr>";
                    }
                    else
                    {
                        if (regprom.ToUpper().Trim() != "REGULAR" && regprom.ToUpper().Trim() != "PROMOTIONAL")
                        {
                            errcode = "015039";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A15</td><td align=left valign=top>Please specify whether regular or promotional.</td></tr>";
                        }
                    }

                    string dataonly = "";
                    for (int ii = 253; ii <= 255; ii++)
                    {
                        string tempval = elemList[i].ChildNodes[ii].InnerText.Trim().Replace("-", "");
                        if (tempval != "")
                        {
                            dataonly = "Yes";
                        }
                    }
                    for (int ii = 55; ii <= 183; ii++)
                    {
                        string tempval = elemList[i].ChildNodes[ii].InnerText.Trim().Replace("-", "").Replace("0", "");
                        if (tempval != "")
                        {
                            dataonly = "No";
                        }
                    }
                    for (int ii = 187; ii <= 251; ii++)
                    {
                        string tempval = elemList[i].ChildNodes[ii].InnerText.Trim().Replace("-", "").Replace("0", "");
                        if (tempval != "")
                        {
                            dataonly = "No";
                        }
                    }


                    // For 'Promotional', promotion start date and end date are compulsary //
                    if(regprom.ToUpper().Trim()=="PROMOTIONAL")
                    {
                        DateTime promstart = Convert.ToDateTime("1/1/2001");
                        DateTime promend = Convert.ToDateTime("1/1/2001");
                        try
                        {
                            if (elemList[i].ChildNodes[15].InnerText.ToString().Trim() == "")
                            {
                                errcode = "016040";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A16</td><td align=left valign=top>Start date of promotional offer is compulsory</td></tr>";
                            }
                            else
                            {
                                promstart = Convert.ToDateTime(elemList[i].ChildNodes[15].InnerText.ToString().Trim());
                                // check if date passed in "dd-MMM-yyyy" format
                                DateTime dt;
                                string[] formats = { "dd-MMM-yyyy", "dd-MMM-yyyy" };
                                if (!DateTime.TryParseExact(elemList[i].ChildNodes[15].InnerText.ToString().Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                                {
                                    errcode = "016041";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A16</td><td align=left valign=top>Start date of promotional offer should be in dd-MMM-yyyy format</td></tr>";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errcode = "016042";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A16</td><td align=left valign=top>Please specify the start date of promotional offer</td></tr>";
                        }
                        try
                        {
                            if (elemList[i].ChildNodes[16].InnerText.ToString().Trim() == "")
                            {
                                errcode = "017043";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A17</td><td align=left valign=top>End date of promotional offer is compulsory</td></tr>";
                            }
                            else
                            {
                                promend = Convert.ToDateTime(elemList[i].ChildNodes[16].InnerText.ToString().Trim());
                                // check if date passed in "dd-MMM-yyyy" format
                                DateTime dt;
                                string[] formats = { "dd-MMM-yyyy", "dd-MMM-yyyy" };
                                if (!DateTime.TryParseExact(elemList[i].ChildNodes[16].InnerText.ToString().Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                                {
                                    errcode = "017044";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A17</td><td align=left valign=top>End date of promotional offer should be in dd-MMM-yyyy format</td></tr>";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errcode = "017045";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A17</td><td align=left valign=top>Please specify the end date of promotional offer</td></tr>";
                        }
                        if(promstart>promend)
                        {
                            errcode = "016046";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A16</td><td align=left valign=top>Start date of promotional offer cannot be greater than end date</td></tr>";
                        }
                        else
                        {
                            if(ttype.ToUpper()=="PREPAID STV" || ttype.ToUpper()=="PREPAID COMBO")
                            {
                                int maxvalidity = 90;
                                if(dataonly=="Yes")
                                {
                                    maxvalidity = 365;
                                }
                                TimeSpan tsprom=(promend - promstart);
                                if (Convert.ToInt32(tsprom.Days) > maxvalidity)
                                {
                                    errcode = "017047";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A17</td><td align=left valign=top>Maximum validity for promotional Prepaid STV / Prepaid Combo can be maximum 90 days (365 days for data only offers)</td></tr>";
                                }
                            }
                        }
                    }


                    // Special Eligibility Conditions //
                    string elig = elemList[i].ChildNodes[17].InnerText.ToString().Trim();
                    if (elig == "")
                    {
                        errcode = "018048";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A18</td><td align=left valign=top>Special Eligibility Conditions cannot be left blank. If no conditions, please enter `NIL`</td></tr>";
                    }



                    // Rental / Minimum billing amount //
                    //if(ttype.ToUpper()=="PREPAID ISP" || ttype.ToUpper()=="POSTPAID ISP" || ttype.ToUpper()=="POSTPAID PLAN" || ttype.ToUpper()=="POSTPAID ADD ON PACK" || ttype.ToUpper().Contains("FIXED LINE"))
                    if (ttype.ToUpper() == "PREPAID ISP" || ttype.ToUpper() == "POSTPAID ISP" || ttype.ToUpper() == "POSTPAID PLAN" || ttype.ToUpper() == "POSTPAID ADD ON PACK" || ttype.ToUpper().Contains("POSTPAID FIXED LINE"))
                    {
                        double rental = -1;
                        try
                        {
                            if (elemList[i].ChildNodes[28].InnerText.ToString().Trim()=="")
                            {
                                errcode = "029049";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A29</td><td align=left valign=top>Rental / Minimum billing amount is compulsory</td></tr>";
                            }
                            else
                            {
                                rental = Convert.ToDouble(elemList[i].ChildNodes[28].InnerText.ToString().Trim());
                            }
                        }
                        catch (Exception ex) { }
                        if(rental==-1)
                        {
                            errcode = "029050";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A29</td><td align=left valign=top>Please enter the Rental / Minimum Billing Amount</td></tr>";
                        }
                    }
                    

                    // ISD Weblink Conditions //
                    if (ttype.ToUpper() == "PREPAID STV" || ttype.ToUpper() == "PREPAID COMBO" || ttype.ToUpper() == "POSTPAID ADD ON PACK" || ttype.ToUpper() == "PREPAID FIXED LINE ADD ON PACK" || ttype.ToUpper() == "POSTPAID FIXED LINE ADD ON PACK" || ttype.ToUpper() == "PREPAID GEN ISD TARIFF" || ttype.ToUpper() == "POSTPAID GEN ISD TARIFF")
                    {
                        int ISDexists = 0;
                        for(int k=134;k<=138;k++)   // if data exists in any of fields A135 till A139, weblink is mandatory
                        {
                            if(elemList[i].ChildNodes[k].InnerText.ToString().Trim()!="")
                            {
                                ISDexists = 1;
                            }
                        }
                        if (ISDexists == 1)
                        {
                            string isdweblink = elemList[i].ChildNodes[139].InnerText.ToString().Trim();
                            if (isdweblink == "")
                            {
                                errcode = "140051";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A140</td><td align=left valign=top>Please enter the Weblink for ISD call rate</td></tr>";
                            }
                        }
                    }

                    // International Roaming Weblink Conditions //
                    if (ttype.ToUpper() == "PREPAID STV" || ttype.ToUpper() == "PREPAID COMBO" || ttype.ToUpper() == "POSTPAID ADD ON PACK" || ttype.ToUpper() == "PREPAID INTERNATIONAL ROAMING" || ttype.ToUpper() == "POSTPAID INTERNATIONAL ROAMING")
                    {
                        int INTexists = 0;
                        for(int k=162;k<=182;k++)   // if data exists in any of fields A1163 till A183, weblink is mandatory
                        {
                            if(elemList[i].ChildNodes[k].InnerText.ToString().Trim()!="")
                            {
                                INTexists = 1;
                            }
                        }
                        if (INTexists == 1)
                        {
                            string intweblink = elemList[i].ChildNodes[188].InnerText.ToString().Trim();
                            if (intweblink == "")
                            {
                                errcode = "189052";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A189</td><td align=left valign=top>Please enter the Weblink for rates while international roaming</td></tr>";
                            }
                        }
                    }


                    
                    // check 'Delayed Submission' //
                    if (elemList[i].ChildNodes[276].InnerText.ToString().Trim().ToUpper() != "YES" && elemList[i].ChildNodes[276].InnerText.ToString().Trim().ToUpper() != "NO")
                    {
                        errcode = "277053";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A277</td><td align=left valign=top>Please specify `Yes` or `No` in Delayed Submission</td></tr>";
                    }
                    if (elemList[i].ChildNodes[277].InnerText.ToString().Trim().ToUpper()=="")
                    {
                        errcode = "278054";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A278</td><td align=left valign=top>For delayed submission, please specify the reason for Delayed Submission. Otherwise enter `NIL`.</td></tr>";
                    }


                    if (actiontotake == "REVISION")   // In case of revision, the submitted revision id is the new unique id, and the old unique id becomes the revision id (id against which revision is being made)
                    {
                        myqry = myqry + "'" + ttype + "','" + tariffdet + "','" + revisionID + "','" + useroperator + "','" + circ + "','" + elemList[i].ChildNodes[5].InnerText + "','" + servicetype + "','" + categ + "','" + actiontotake + "','" + uniqueid + "',";
                    }
                    else
                    {
                        myqry = myqry + "'" + ttype + "','" + tariffdet + "','" + uniqueid + "','" + useroperator + "','" + circ + "','" + elemList[i].ChildNodes[5].InnerText + "','" + servicetype + "','" + categ + "','" + actiontotake + "','" + revisionID + "',";
                    }




                    // $$$$$$ Regulatory Validations $$$$$$$$ // 

                    DateTime repdate = Convert.ToDateTime("1/1/2001");
                    try
                    {
                        repdate = Convert.ToDateTime(elemList[i].ChildNodes[12].InnerText);
                    }
                    catch (Exception ex) { }

                    /*
                    if (actiontotake.ToUpper() == "LAUNCH" || actiontotake == "WITHDRAWAL")
                    {
                        if (Convert.ToDateTime(repdate.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")))
                        {
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>A13</td><td align=left valign=top>Date of Reporting cannot be greater than today's date (" + DateTime.Now.ToString("dd-MMM-yyyy") + ")</td></tr>";
                        }
                    }
                    */

                    double mrp = -1;
                    try
                    {
                        mrp = Convert.ToDouble(elemList[i].ChildNodes[18].InnerText);
                    }
                    catch (Exception ex) { }
                    //if (ttype.ToUpper() == "PREPAID PLAN VOUCHER" || ttype.ToUpper() == "PREPAID STV" || ttype.ToUpper() == "PREPAID COMBO" || ttype.ToUpper() == "PREPAID TOP UP" || ttype.ToUpper() == "PREPAID VAS" || ttype.ToUpper() == "PREPAID PROMO OFFER" || ttype.ToUpper() == "SUK" || ttype.ToUpper() == "PREPAID ISP" || ttype.ToUpper() == "POSTPAID ISP")
                    if (ttype.ToUpper() == "PREPAID PLAN VOUCHER" || ttype.ToUpper() == "PREPAID STV" || ttype.ToUpper() == "PREPAID COMBO" || ttype.ToUpper() == "PREPAID TOP UP" || ttype.ToUpper() == "PREPAID VAS" || ttype.ToUpper() == "PREPAID PROMO OFFER" || ttype.ToUpper() == "SUK" || ttype.ToUpper() == "PREPAID ISP" || ttype.ToUpper() == "POSTPAID ISP" || ttype.ToUpper().Contains("PREPAID FIXED LINE"))
                    {
                        if (mrp == -1)
                        {
                            errcode = "019055";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A19</td><td align=left valign=top>Price is compulsory</td></tr>";
                        }
                    }

                    double monval = -1;
                    try
                    {
                        monval = Convert.ToDouble(elemList[i].ChildNodes[19].InnerText);
                    }
                    catch (Exception ex) { }
                    if (ttype.ToUpper() == "PREPAID COMBO" || ttype.ToUpper() == "PREPAID TOP UP")
                    {
                        if (monval == -1)
                        {
                            errcode = "020056";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A20</td><td align=left valign=top>Monetary value is compulsory</td></tr>";
                        }
                    }

                    double validity = 0;
                    try
                    {
                        if (elemList[i].ChildNodes[47].InnerText.ToUpper() == "UNLIMITED")
                        {
                            validity = -2;
                        }
                        else
                        {
                            validity = Convert.ToDouble(elemList[i].ChildNodes[47].InnerText);
                        }
                    }
                    catch (Exception ex) { }

                    // For 'Prepaid Top Up', validity should be blank.
                    if (ttype.ToUpper() == "PREPAID TOP UP")
                    {
                        if (elemList[i].ChildNodes[47].InnerText.ToUpper() != "UNLIMITED" && elemList[i].ChildNodes[47].InnerText.ToUpper() != "")
                        {
                            errcode = "048057";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A48</td><td align=left valign=top>Prepaid Top Up cannot have a validity</td></tr>";
                        }
                    }

                    if (ttype.ToUpper() == "PREPAID PLAN VOUCHER" || ttype.ToUpper() == "PREPAID STV" || ttype.ToUpper() == "PREPAID COMBO" || ttype.ToUpper() == "PREPAID TOP UP" || ttype.ToUpper() == "PREPAID VAS" || ttype.ToUpper() == "SUK" || ttype.ToUpper() == "PREPAID ISP" || ttype.ToUpper() == "POSTPAID ISP")
                    {
                        if(mrp<0)
                        {
                            errcode = "019058";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A19</td><td align=left valign=top>Price cannot be negative</td></tr>";
                        }
                    }
                    if (ttype.ToUpper() == "PREPAID COMBO" || ttype.ToUpper() == "PREPAID TOP UP")
                    {
                        if (monval < 0)
                        {
                            errcode = "020059";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A20</td><td align=left valign=top>Please specify the monetary value</td></tr>";
                        }
                    }

                    if (ttype.ToUpper() == "PREPAID TOP UP")
                    {
                        if(mrp%10!=0)
                        {
                            errcode = "019060";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A19</td><td align=left valign=top>Prepaid Top Up should be in multiples of Rs. 10</td></tr>";
                        }
                        if (monval == 0)
                        {
                            errcode = "020061";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A20</td><td align=left valign=top>Prepaid Top Up should contain a monetary value</td></tr>";
                        }
                    }

                    if (ttype.ToUpper() == "PREPAID STV")
                    {
                        if (mrp > 0)
                        {
                            if (mrp % 10 == 0)
                            {
                                errcode = "019062";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A19</td><td align=left valign=top>Prepaid STV should not be in multiples of Rs. 10</td></tr>";
                            }
                        }
                        if(monval>0)
                        {
                            errcode = "020063";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A20</td><td align=left valign=top>Prepaid STV should not contain any monetary value</td></tr>";
                        }

                        if (actiontotake.ToUpper() != "WITHDRAWAL")
                        {
                            if (dataonly == "Yes")
                            {
                                if (validity > 365 || validity == -2)
                                {
                                    errcode = "048064";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A48</td><td align=left valign=top>Validity of Prepaid STV with data only benefit cannot be more than 365 days</td></tr>";
                                }
                            }
                            else
                            {
                                if (validity > 90 || validity == -2)
                                {
                                    errcode = "048065";
                                    errflag = 1;
                                    errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A48</td><td align=left valign=top>Validity of Prepaid STV cannot be more than 90 days</td></tr>";
                                }
                            }
                        }

                    }

                    if (ttype.ToUpper() == "PREPAID COMBO")
                    {
                        if (mrp % 10 == 0)
                        {
                            errcode = "019066";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A19</td><td align=left valign=top>Prepaid Combo should not be in multiples of Rs. 10</td></tr>";
                        }
                        if (monval <= 0)
                        {
                            errcode = "020067";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A20</td><td align=left valign=top>Prepaid Combo should contain a monetary value</td></tr>";
                        }
                        if (validity > 90 || validity == -2)
                        {
                            errcode = "048068";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A48</td><td align=left valign=top>Validity of Prepaid Combo cannot be more than 90 days</td></tr>";
                        }
                    }

                    /*
                    // Maximum of 25 plans is allowed in respective categories for plan products //
                    if (actiontotake.ToUpper().Trim() == "LAUNCH")
                    {
                        int planlimit = 25;
                        if (ttype.ToUpper() == "PREPAID PLAN VOUCHER" || ttype.ToUpper() == "POSTPAID PLAN" || ttype.ToUpper() == "PREPAID FIXED LINE PLAN" || ttype.ToUpper() == "POSTPAID FIXED LINE PLAN" || ttype.ToUpper() == "PREPAID FIXED LINE WLL PLAN" || ttype.ToUpper() == "POSTPAID FIXED LINE WLL PLAN")
                        {
                            if (servicetype.ToUpper().Contains("GSM") || servicetype.ToUpper().Contains("LTE"))
                            {
                                com = new MySqlCommand("select count(distinct(uniqueid)) from " + tablename + " where(upper(ttype)='" + ttype.ToUpper() + "') and (upper(oper)='" + useroperator.ToUpper() + "') and (upper(circ)='" + circ.ToUpper() + "' || upper(circ)='ALL INDIA') and (upper(service) like '%GSM%' or upper(service) like '%LTE%')", con);
                                con.Open();
                                dr = com.ExecuteReader();
                                dr.Read();
                                try
                                {
                                    if (Convert.ToInt32(dr[0].ToString().Trim()) >= planlimit)
                                    {
                                        errcode = "001069";
                                        errflag = 1;
                                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A1</td><td align=left valign=top>You have already reached the maximum limit of 25 plans allowed.</td></tr>";
                                    }
                                }
                                catch (Exception ex) { }
                                con.Close();
                            }

                            if (servicetype.ToUpper().Contains("CDMA"))
                            {
                                com = new MySqlCommand("select count(distinct(uniqueid)) from " + tablename + " where(upper(ttype)='" + ttype.ToUpper() + "') and (upper(oper)='" + useroperator.ToUpper() + "') and (upper(circ)='" + circ.ToUpper() + "' || upper(circ)='ALL INDIA') and (upper(service) like '%CDMA%')", con);
                                con.Open();
                                dr = com.ExecuteReader();
                                dr.Read();
                                try
                                {
                                    if (Convert.ToInt32(dr[0].ToString().Trim()) >= planlimit)
                                    {
                                        errcode = "001069";
                                        errflag = 1;
                                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A1</td><td align=left valign=top>You have already reached the maximum limit of 25 plans allowed.</td></tr>";
                                    }
                                }
                                catch (Exception ex) { }
                                con.Close();
                            }
                        }
                    }
                    // Maximum of 25 plans is allowed in respective categories for plan products //
                    */


                    // Maximum of 25 plans is allowed in respective categories for plan products - limit is only for TSP's//
                    if (useroperator.ToUpper() == "AIRCEL" || useroperator.ToUpper() == "AIRTEL" || useroperator.ToUpper() == "BSNL" || useroperator.ToUpper() == "IDEA" || useroperator.ToUpper() == "JIO" || useroperator.ToUpper() == "MTNL" || useroperator.ToUpper() == "QUADRANT (CONNECT)" || useroperator.ToUpper() == "TATA TELE" || useroperator.ToUpper() == "TELENOR" || useroperator.ToUpper() == "VODAFONE" || useroperator.ToUpper() == "VODAFONE IDEA" || useroperator.ToUpper() == "SURFTELECOM" || useroperator.ToUpper() == "AEROVOYCE")
                    {
                        if (actiontotake.ToUpper().Trim() == "LAUNCH")
                        {
                            int planlimit = 25;
                            if (ttype.ToUpper() == "PREPAID PLAN VOUCHER" || ttype.ToUpper() == "POSTPAID PLAN" || ttype.ToUpper() == "PREPAID FIXED LINE PLAN" || ttype.ToUpper() == "POSTPAID FIXED LINE PLAN" || ttype.ToUpper() == "PREPAID FIXED LINE WLL PLAN" || ttype.ToUpper() == "POSTPAID FIXED LINE WLL PLAN")
                            {
                                if (servicetype.ToUpper().Contains("GSM") || servicetype.ToUpper().Contains("LTE"))
                                {
                                    com = new MySqlCommand("select count(distinct(uniqueid)) from " + tablename + " where (upper(ttype)='PREPAID PLAN VOUCHER' or upper(ttype)='POSTPAID PLAN') and (upper(oper)='" + useroperator.ToUpper() + "') and (upper(circ)='" + circ.ToUpper() + "' || upper(circ)='ALL INDIA') and (upper(service) like '%GSM%' or upper(service) like '%LTE%')", con);
                                    con.Open();
                                    dr = com.ExecuteReader();
                                    dr.Read();
                                    try
                                    {
                                        if (Convert.ToInt32(dr[0].ToString().Trim()) >= planlimit)
                                        {
                                            errcode = "001069";
                                            errflag = 1;
                                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A1</td><td align=left valign=top>You have already reached the maximum limit of 25 plans allowed.</td></tr>";
                                        }
                                    }
                                    catch (Exception ex) { }
                                    con.Close();
                                }

                                if (servicetype.ToUpper().Contains("CDMA"))
                                {
                                    com = new MySqlCommand("select count(distinct(uniqueid)) from " + tablename + " where (upper(ttype)='PREPAID PLAN VOUCHER' or upper(ttype)='POSTPAID PLAN') and (upper(oper)='" + useroperator.ToUpper() + "') and (upper(circ)='" + circ.ToUpper() + "' || upper(circ)='ALL INDIA') and (upper(service) like '%CDMA%')", con);
                                    con.Open();
                                    dr = com.ExecuteReader();
                                    dr.Read();
                                    try
                                    {
                                        if (Convert.ToInt32(dr[0].ToString().Trim()) >= planlimit)
                                        {
                                            errcode = "001069";
                                            errflag = 1;
                                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A1</td><td align=left valign=top>You have already reached the maximum limit of 25 plans allowed.</td></tr>";
                                        }
                                    }
                                    catch (Exception ex) { }
                                    con.Close();
                                }

                                if (ttype.ToUpper().Contains("WLL"))
                                {
                                    com = new MySqlCommand("select count(distinct(uniqueid)) from " + tablename + " where (upper(oper)='" + useroperator.ToUpper() + "') and (upper(circ)='" + circ.ToUpper() + "' || upper(circ)='ALL INDIA') and (upper(ttype) like '%WLL%')", con);
                                    con.Open();
                                    dr = com.ExecuteReader();
                                    dr.Read();
                                    try
                                    {
                                        if (Convert.ToInt32(dr[0].ToString().Trim()) >= planlimit)
                                        {
                                            errcode = "001069";
                                            errflag = 1;
                                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A1</td><td align=left valign=top>You have already reached the maximum limit of 25 plans allowed.</td></tr>";
                                        }
                                    }
                                    catch (Exception ex) { }
                                    con.Close();
                                }

                                if (ttype.ToUpper().Contains("FIXED") && !ttype.ToUpper().Contains("WLL") && !servicetype.ToUpper().Contains("BROADBAND"))
                                {
                                    com = new MySqlCommand("select count(distinct(uniqueid)) from " + tablename + " where (upper(oper)='" + useroperator.ToUpper() + "') and (upper(circ)='" + circ.ToUpper() + "' || upper(circ)='ALL INDIA') and (upper(ttype) like '%FIXED%' and upper(ttype) not like '%WLL%' and upper(ttype) not like '%ADD ON PACK%' and upper(service) not like '%BROADBAND%')", con);
                                    con.Open();
                                    dr = com.ExecuteReader();
                                    dr.Read();
                                    try
                                    {
                                        if (Convert.ToInt32(dr[0].ToString().Trim()) >= planlimit)
                                        {
                                            errcode = "001069";
                                            errflag = 1;
                                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A1</td><td align=left valign=top>You have already reached the maximum limit of 25 plans allowed.</td></tr>";
                                        }
                                    }
                                    catch (Exception ex) { }
                                    con.Close();
                                }
                            }
                        }
                    }
                    // Maximum of 25 plans is allowed in respective categories for plan products - limit is only for TSP's //


                    if (actiontotake.ToUpper().Trim() == "CORRECTION")
                    {
                        if(elemList[i].ChildNodes[267].InnerText.Trim()=="")
                        {
                            errcode = "268070";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A268</td><td align=left valign=top>Reason of correction should be mentioned in Miscellaneous - Remarks</td></tr>";
                        }
                        TimeSpan ts=(DateTime.Now - repdate);
                        if(Convert.ToInt32(ts.Days)>2)
                        {
                            errcode = "009071";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A9</td><td align=left valign=top>Correction is allowed only within 2 days of date of reporting</td></tr>";
                        }
                    }

                    // Call Rate while Roaming - Incoming voice (in INR/pulse) can be Rs. 0.45 max
                    double A146val = -1;
                    try
                    {
                        A146val = Convert.ToDouble(elemList[i].ChildNodes[145].InnerText.Trim());
                    }
                    catch (Exception ex) { }
                    if(A146val>0.45)
                    {
                        errcode = "146072";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A146</td><td align=left valign=top>Call Rate while Roaming - Incoming voice cannot exceed Rs. 0.45 </td></tr>";
                    }

                    // Call Rate while Roaming - Local Outgoing can be Rs. 0.80 max
                    double A148val = -1;
                    try
                    {
                        A148val = Convert.ToDouble(elemList[i].ChildNodes[147].InnerText.Trim());
                    }
                    catch (Exception ex) { }
                    if (A148val > 0.80)
                    {
                        errcode = "148073";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A148</td><td align=left valign=top>Call Rate while Roaming - Local Outgoing cannot exceed Rs. 0.80 </td></tr>";
                    }

                    // Call Rate while Roaming - STD Outgoing voice can be Rs. 1.15 max
                    double A150val = -1;
                    try
                    {
                        A150val = Convert.ToDouble(elemList[i].ChildNodes[149].InnerText.Trim());
                    }
                    catch (Exception ex) { }
                    if (A150val > 1.15)
                    {
                        errcode = "150074";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A150</td><td align=left valign=top>Call Rate while Roaming - STD Outgoing voice cannot exceed Rs. 1.15 </td></tr>";
                    }
                    
                    // SMS Rate while Roaming - Local can be Rs. 0.25 max
                    double A156val = -1;
                    try
                    {
                        A156val = Convert.ToDouble(elemList[i].ChildNodes[155].InnerText.Trim());
                    }
                    catch (Exception ex) { }
                    if (A156val > 0.25)
                    {
                        errcode = "156075";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A156</td><td align=left valign=top>SMS Rate while Roaming - Local cannot exceed Rs. 0.25 </td></tr>";
                    }

                    // SMS Rate while Roaming - National can be Rs. 0.38 max
                    double A157val = -1;
                    try
                    {
                        A157val = Convert.ToDouble(elemList[i].ChildNodes[156].InnerText.Trim());
                    }
                    catch (Exception ex) { }
                    if (A157val > 0.38)
                    {
                        errcode = "158076";
                        errflag = 1;
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A158</td><td align=left valign=top>SMS Rate while Roaming - National cannot exceed Rs. 0.38 </td></tr>";
                    }


                    // for ISPs, fields 'adddata_unit' through 'adddata_total4g' are compulsary, as these are being used to store speed values for ISPs //

                    //if (useroperator != "" && useroperator.ToUpper() != "AIRCEL" && useroperator.ToUpper() != "AIRTEL" && useroperator.ToUpper() != "BSNL" && useroperator.ToUpper() != "IDEA" && useroperator.ToUpper() != "JIO" && useroperator.ToUpper() != "MTNL" && useroperator.ToUpper() != "QUADRANT (CONNECT)" && useroperator.ToUpper() != "TATA TELE" && useroperator.ToUpper() != "TELENOR" && useroperator.ToUpper() != "VODAFONE" && useroperator.ToUpper() != "SURFTELECOM" && useroperator.ToUpper() != "AEROVOYCE")
                    if (useroperator != "" && useroperator.ToUpper() != "AIRCEL" && useroperator.ToUpper() != "IDEA" && useroperator.ToUpper() != "JIO" && useroperator.ToUpper() != "TATA TELE" && useroperator.ToUpper() != "TELENOR" && useroperator.ToUpper() != "VODAFONE" && useroperator.ToUpper() != "VODAFONE IDEA" && useroperator.ToUpper() != "SURFTELECOM" && useroperator.ToUpper() != "AEROVOYCE")
                    {
                        int speedflag = 0;

                        if ((useroperator.ToUpper() == "AIRTEL" || useroperator.ToUpper() == "BSNL" || useroperator.ToUpper() == "MTNL" || useroperator.ToUpper() == "QUADRANT (CONNECT)") && (elemList[i].ChildNodes[252].InnerText.Trim().ToLower() != "kbps" && elemList[i].ChildNodes[252].InnerText.Trim().ToLower() != "mbps"))
                        {
                            speedflag = 1;
                        }

                        if (speedflag == 0)
                        {

                            if (elemList[i].ChildNodes[252].InnerText.Trim() == "")
                            {
                                errcode = "253080";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A253</td><td align=left valign=top>Speed Unit is a mandatory field for ISPs</td></tr>";
                            }

                            double A254val = -1;
                            if (elemList[i].ChildNodes[253].InnerText.Trim().ToUpper() == "UNLIMITED")
                            {
                                A254val = -2;
                            }
                            else
                            {
                                try
                                {
                                    A254val = Convert.ToDouble(elemList[i].ChildNodes[253].InnerText.Trim());
                                }
                                catch (Exception ex) { }
                            }
                            if (A254val == -1)
                            {
                                errcode = "254081";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A254</td><td align=left valign=top>Please enter a numeric value for Data usage limit with higher speed</td></tr>";
                            }

                            double A255val = -1;
                            try
                            {
                                A255val = Convert.ToDouble(elemList[i].ChildNodes[254].InnerText.Trim());
                            }
                            catch (Exception ex) { }
                            if (A255val == -1)
                            {
                                errcode = "255081";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A255</td><td align=left valign=top>Please enter a numeric value for Speed of connection upto data usage limit</td></tr>";
                            }

                            double A256val = -1;
                            try
                            {
                                A256val = Convert.ToDouble(elemList[i].ChildNodes[255].InnerText.Trim());
                            }
                            catch (Exception ex) { }
                            if (A256val == -1)
                            {
                                errcode = "256081";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A256</td><td align=left valign=top>Please enter a numeric value for Speed of connection beyond data usage limit</td></tr>";
                            }

                        }
                    }

                     // for ISPs, fields 'adddata_unit' through 'adddata_total4g' are compulsary, as these are being used to store speed values for ISPs - CODE ENDS HERE //
                   


                    // $$$$$$ Regulatory Validations - CODE ENDS HERE $$$$$$$$ //





                    // 'A11' till 'A12'
                    getAlphanumeric(i, 10, 11);


                    // 'A13' till 'A14'
                    for (int j = 12; j <= 13; j++)
                    {
                        DateTime nodedate = Convert.ToDateTime("2001-01-01");
                        try
                        {
                            nodedate = Convert.ToDateTime(elemList[i].ChildNodes[j].InnerText);
                        }
                        catch(Exception ex)
                        { }
                        myqry = myqry + "'" + nodedate.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                    }


                    //Regular / Promotiona
                    myqry = myqry + "'" + regprom + "',";



                    // 'A16' till 'A17'
                    for (int j = 15; j <= 16; j++)
                    {
                        DateTime nodedate = Convert.ToDateTime("2001-01-01");
                        try
                        {
                            nodedate = Convert.ToDateTime(elemList[i].ChildNodes[j].InnerText);
                        }
                        catch (Exception ex)
                        { }
                        myqry = myqry + "'" + nodedate.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                    }


                    // 'A18' till 'A18'
                    getAlphanumeric(i, 17, 17);
                    
                    // 'A19' till 'A20'
                    getNumeric(i, 18, 19, uniqueid);

                    // Rural / Urban //
                    myqry = myqry + "'" + elemList[i].ChildNodes[20].InnerText.Replace(Convert.ToChar(34), Convert.ToChar(126)).Replace("'", "`") + "',";
                                        
                    // 'A22' till 'A48'
                    getNumeric(i, 21, 47, uniqueid);

                    // 'A49' till 'A51'
                    getAlphanumeric(i, 48, 50);

                    // 'A52' till 'A55'
                    getTimeNumeric(i, 51, 54, uniqueid);

                    // 'A56' till 'A87'
                    getNumeric(i, 55, 86, uniqueid);

                    // 'A88' till 'A89'
                    getAlphanumeric(i, 87, 88);
                    
                    // 'A90' till 'A121'
                    getNumeric(i, 89, 120, uniqueid);

                    // 'A122' till 'A123'
                    getAlphanumeric(i, 121, 122);

                    // 'A124' till 'A130'
                    getNumeric(i, 123, 129, uniqueid);

                    // 'A131'
                    getAlphanumeric(i, 130, 130);

                    // 'A132'
                    getNumeric(i, 131, 131, uniqueid);

                    // 'A133' till 'A135'
                    getAlphanumeric(i, 132, 134);

                    // 'A136' till 'A139'
                    getNumeric(i, 135, 138, uniqueid);

                    // 'A140' till 'A141'
                    getAlphanumeric(i, 139, 140);

                    // 'A142'
                    getNumeric(i, 141, 141, uniqueid);

                    // 'A143' till 'A144'
                    getAlphanumeric(i, 142, 143);

                    // 'A145' till 'A152'
                    getNumeric(i, 144, 151, uniqueid);

                    // 'A153' till 'A155'
                    getAlphanumeric(i, 152, 154);

                    // 'A156' till 'A160'
                    getNumeric(i, 155, 159, uniqueid);

                    // 'A161' till 'A163'
                    getAlphanumeric(i, 160, 162);

                    // 'A164' till 'A178'
                    getNumeric(i, 163, 177, uniqueid);

                    // 'A179'
                    getAlphanumeric(i, 178, 178);

                    // 'A180' till 'A182'
                    getNumeric(i, 179, 181, uniqueid);

                    // 'A183' till 'A187'
                    getAlphanumeric(i, 182, 186);

                    // 'A188'
                    getNumeric(i, 187, 187, uniqueid);

                    // 'A189'
                    getAlphanumeric(i, 188, 188);

                    // 'A190' till 'A191'
                    getTimeNumeric(i, 189, 190, uniqueid);

                    // 'A192' till 'A198'
                    getNumeric(i, 191, 197, uniqueid);

                    // 'A199'
                    getAlphanumeric(i, 198, 198);

                    // 'A200' till 'A206'
                    getNumeric(i, 199, 205, uniqueid);

                    // 'A207'
                    getAlphanumeric(i, 206, 206);

                    // 'A208' till 'A214'
                    getNumeric(i, 207, 213, uniqueid);

                    // 'A215'
                    getAlphanumeric(i, 214, 214);

                    // 'A216' till 'A218'
                    getNumeric(i, 215, 217, uniqueid);

                    // 'A219'
                    getAlphanumeric(i, 218, 218);

                    // 'A220' till 'A226'
                    getNumeric(i, 219, 225, uniqueid);

                    // 'A227'
                    getAlphanumeric(i, 226, 226);

                    // 'A228' till 'A238'
                    getNumeric(i, 227, 237, uniqueid);

                    // 'A239' till 'A241'
                    getAlphanumeric(i, 238, 240);

                    // 'A242'
                    getNumeric(i, 241, 241, uniqueid);

                    // 'A243'
                    getAlphanumeric(i, 242, 242);

                    // 'A244' till 'A251'
                    getNumeric(i, 243, 250, uniqueid);

                    // 'A252' till 'A253'
                    getAlphanumeric(i, 251, 252);

                    // 'A254' till 'A260'
                    getNumeric(i, 253, 259, uniqueid);

                    // 'A261'
                    getAlphanumeric(i, 260, 260);

                    // 'A262'
                    getNumeric(i, 261, 261, uniqueid);

                    // 'A263' till 'A278'
                    getAlphanumeric(i, 262, 277);



                    ////myqry = myqry.Substring(0, myqry.Length - 1);   //remove trailing comma
                    

                    // Add three blank values for fields 'checked', 'checkedby' and 'checkedon'
                    //myqry = myqry + "'No','','" + Convert.ToDateTime("1/1/2001").ToString("yyyy-MM-dd HH:mm:ss") + "','','0','No'";
                    if (actiondate < Convert.ToDateTime("6/15/2018"))
                    {
                        myqry = myqry + "'Yes','admin','" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "','','0','No'";
                    }
                    else
                    {
                        myqry = myqry + "'No','','" + Convert.ToDateTime("1/1/2001").ToString("yyyy-MM-dd HH:mm:ss") + "','','0','No'";
                    }

                    string withdrawalqry = myqry.Replace("insert into " + tablename + " values", "insert into TRAI_archive values");  // withdrawal query is same as myqry from fields 'rno' till 'checkedon'

                    myqry = myqry + ")";


                    if (errflag == 0)
                    {
                        if(actiontotake!="LAUNCH")   // add existing record to archive table if its not a LAUNCH
                        {
                            com1 = new MySqlCommand("select * from " + tablename + " where(uniqueid='" + uniqueid + "') order by rno desc limit 1", con1);   // 'limit 1' is the MySql equivalient of 'top 1 *' in Sql Server
                            con1.Open();
                            dr1 = com1.ExecuteReader();
                            while (dr1.Read())
                            {
                                string archiveqry = "insert into TRAI_archive values(";
                                try
                                {
                                    for (int k = 0; k <= 286; k++)
                                    {
                                        if (k == 2 || k == 16 || k == 17 || k == 19 || k == 20 || k==284)
                                        {
                                            archiveqry = archiveqry + "'" + Convert.ToDateTime(dr1[k].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "',";
                                        }
                                        else
                                        {
                                            archiveqry = archiveqry + "'" + dr1[k].ToString().Trim() + "',";
                                        }
                                    }
                                    getMaxRno("archiveno", "TRAI_archive");
                                    archiveqry = archiveqry + "'No','" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                    try
                                    {
                                        com = new MySqlCommand(archiveqry, con);
                                        con.Open();
                                        com.ExecuteNonQuery();
                                    }
                                    catch (Exception ex) { }
                                    con.Close();

                                    com = new MySqlCommand("delete from " + tablename + " where(uniqueid='" + uniqueid + "')", con);
                                    con.Open();
                                    com.ExecuteNonQuery();
                                    con.Close();

                                }
                                catch (Exception ex) { }
                            }
                            con1.Close();
                        }

                        if (actiontotake != "WITHDRAWAL")   // add record to table if its not a WITHDRAWAL
                        {
                            com = new MySqlCommand(myqry, con);
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
                        }
                        else
                        {
                            getMaxRno("archiveno", "TRAI_archive");
                            withdrawalqry=withdrawalqry + ",'" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            com = new MySqlCommand(withdrawalqry, con);   // insert 'withdrawal' record in archive table.
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
                        }

                        //errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:blue;>Success</font></td><td align=left valign=top>000000</td><td align=left valign=top></td><td align=left valign=top></td></tr>";
                        string successdet = "<b>SUCCESS : </b>";
                        if (elemList[i].ChildNodes[18].InnerText.ToString().Trim() != "-1" && elemList[i].ChildNodes[18].InnerText.ToString().Trim() != "")
                        {
                            successdet = successdet + "Price : Rs. " + elemList[i].ChildNodes[18].InnerText.ToString().Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (elemList[i].ChildNodes[28].InnerText.ToString().Trim() != "-1" && elemList[i].ChildNodes[28].InnerText.ToString().Trim() != "")
                        {
                            successdet = successdet + "Rental : Rs. " + elemList[i].ChildNodes[28].InnerText.ToString().Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        successdet = successdet + "Tariff Type : " + elemList[i].ChildNodes[0].InnerText.ToString().Trim() + "<br />";
                        successdet = successdet + "<a href=https://tariff.trai.gov.in/master/tariffdetails.aspx?uid=" + uniqueid + " class=indexlinks2 target=_blank>View Details</a>";
                        
                        
                        errstr1 = errstr1 + "<tr><td align=left valign=top>" + uniqueid + "</td><td align=left valign=top><font style=color:blue;>Success</font></td><td align=left valign=top>000000</td><td align=left valign=top></td><td align=left valign=top>" + successdet + "</td></tr>";

                        submitcount++;
                    }
                }


                // Now, Read data from the stored file and take necessary action (e.g. store in database) - CODE ENDS HERE //
                

            }

            errstr1 = errstr1 + "</table>";


            // insert the success / error report for the file in the error log //
            try
            {
                getMaxRno("rno","TRAI_tarifferrorlog");
                //com3 = new MySqlCommand("insert into TRAI_tarifferrorlog values('" + zno + "','" + filename + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + uname + "','" + errstr1 + "')", con3);
                com3 = new MySqlCommand("insert into TRAI_tarifferrorlog values('" + zno + "','" + filename + "','" + recdate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + uname + "','" + errstr1 + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')", con3);
                con3.Open();
                com3.ExecuteNonQuery();
                con3.Close();
            }
            catch (Exception ex) { }

            
            

            Response.Write(errstr1);


            // $$$$$$  If error is to be sent in XML format instead of the present string format, comment the live above ("Response.Write(errstr1);"), and prepare the XML report as per the commented section below
            /*
            string myerr = "";
            myerr=myerr + "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
            myerr = myerr + "<SubmissionReport>";
            myerr = myerr + "<RecordStatus>";
            myerr = myerr + "<UniqueID/>";
            myerr = myerr + "<Status/>";
            myerr = myerr + "<ErrorKey/>";
            myerr = myerr + "<ErrorDetail>Invalid User Id</ErrorDetail>";
            myerr = myerr + "</RecordStatus>";
            myerr = myerr + "<RecordStatus>";
            myerr = myerr + "<UniqueID>BS06PMPV0001</UniqueID>";
            myerr = myerr + "<Status>Success</Status>";
            myerr = myerr + "<ErrorKey/>";
            myerr = myerr + "<ErrorDetail/>";
            myerr = myerr + "</RecordStatus>";
            myerr = myerr + "<RecordStatus>";
            myerr = myerr + "<UniqueID>BS06PMPV0002</UniqueID>";
            myerr = myerr + "<Status>Failure</Status>";
            myerr = myerr + "<ErrorKey>A5</ErrorKey>";
            myerr = myerr + "<ErrorDetail>LSA is compulsory</ErrorDetail>";
            myerr = myerr + "</RecordStatus>";
            myerr = myerr + "</SubmissionReport>";
            Response.Write(myerr);
            */
            
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }




    protected void getAlphanumeric(Int32 recno, Int32 fromcol, Int32 tillcol)
    {
        try
        {
            for (int j = fromcol; j <= tillcol; j++)
            {
                string nodetext = elemList[recno].ChildNodes[j].InnerText.Replace(Convert.ToChar(34), Convert.ToChar(126)).Replace("'","`");
                myqry = myqry + "'" + nodetext + "',";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }







    protected void getTimeNumeric(Int32 recno, Int32 fromcol, Int32 tillcol, string uid)
    {
        try
        {
            for (int j = fromcol; j <= tillcol; j++)
            {
                double nodeval = -1;
                string nodetext = elemList[recno].ChildNodes[j].InnerText.Replace(Convert.ToChar(34), Convert.ToChar(126)).Replace("'", "`").Replace(":", ".");
                if(nodetext.Contains("."))
                {
                    nodetext = nodetext.Replace(".00", "");  // remove 'seconds' value, if 00 
                    if(!nodetext.Contains("."))
                    {
                        nodetext = nodetext + ".00"; // if minutes and seconds are both zero, it will give only hours output (e.g. 14). So add '.00' to make it in 14:00 format
                    }
                }
                if (nodetext == "-1" || nodetext == "-2")   // if user entered -1 or -2, make it blank, so that it does not clash with standard meaning of -1 (blank) or -2(unlimited)
                {
                    nodetext = "";
                }
                if (nodetext.ToUpper() == "UNLIMITED")
                {
                    nodeval = -2;
                }
                else
                {
                    if (nodetext != "")
                    {
                        try
                        {
                            nodeval = Convert.ToDouble(nodetext);
                            if (nodeval < 0)
                            {
                                int attno = j + 1;
                                string attkey = attno.ToString();
                                if(attno<100)
                                {
                                    if(attno<10)
                                    {
                                        attkey = "00" + attkey;
                                    }
                                    else
                                    {
                                        attkey = "0" + attkey;
                                    }
                                }
                                errcode = attkey + "077";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A" + (j + 1).ToString() + "</td><td align=left valign=top>Data should be entered in HH:mm:ss format in A" + (j + 1).ToString().Trim() + "</td></tr>";
                            }
                        }
                        catch (Exception ex)
                        {
                            int attno2 = j + 1;
                            string attkey2 = attno2.ToString();
                            if (attno2 < 100)
                            {
                                if (attno2 < 10)
                                {
                                    attkey2 = "00" + attkey2;
                                }
                                else
                                {
                                    attkey2 = "0" + attkey2;
                                }
                            }
                            errcode = attkey2 + "077";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A" + (j + 1).ToString() + "</td><td align=left valign=top>Data should be entered in HH:mm:ss format in A" + (j + 1).ToString().Trim() + "</td></tr>";
                        }
                    }
                    else
                    {
                        nodeval = -1;
                    }
                }
                myqry = myqry + "'" + Math.Round(nodeval).ToString().Trim() + "',";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }




    protected void getNumeric(Int32 recno, Int32 fromcol, Int32 tillcol, string uid)
    {
        try
        {
            for (int j = fromcol; j <= tillcol; j++)
            {
                double nodeval = -1;
                string nodetext = elemList[recno].ChildNodes[j].InnerText.Replace(Convert.ToChar(34), Convert.ToChar(126)).Replace("'", "`").Replace(":",".");
                if(nodetext=="-1" || nodetext=="-2")   // if user entered -1 or -2, make it blank, so that it does not clash with standard meaning of -1 (blank) or -2(unlimited)
                {
                    nodetext = "";
                }
                if (nodetext.ToUpper() == "UNLIMITED")
                {
                    nodeval = -2;
                }
                else
                {
                    if (nodetext != "")
                    {
                        try
                        {
                            nodeval = Convert.ToDouble(nodetext);
                            if(nodeval<0)
                            {
                                int attno = j + 1;
                                string attkey = attno.ToString();
                                if (attno < 100)
                                {
                                    if (attno < 10)
                                    {
                                        attkey = "00" + attkey;
                                    }
                                    else
                                    {
                                        attkey = "0" + attkey;
                                    }
                                }
                                errcode = attkey + "078";
                                errflag = 1;
                                errstr1 = errstr1 + "<tr><td align=left valign=top>" + uid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A" + (j + 1).ToString() + "</td><td align=left valign=top>Negative value entered in A" + (j + 1).ToString().Trim() + "</td></tr>";
                            }
                        }
                        catch (Exception ex)
                        {
                            int attno2 = j + 1;
                            string attkey2 = attno2.ToString();
                            if (attno2 < 100)
                            {
                                if (attno2 < 10)
                                {
                                    attkey2 = "00" + attkey2;
                                }
                                else
                                {
                                    attkey2 = "0" + attkey2;
                                }
                            }
                            errcode = attkey2 + "078";
                            errflag = 1;
                            errstr1 = errstr1 + "<tr><td align=left valign=top>" + uid + "</td><td align=left valign=top><font style=color:red;>Failure</font></td><td align=left valign=top>" + errcode + "</td><td align=left valign=top>A" + (j + 1).ToString() + "</td><td align=left valign=top>Non-numeric value entered in A" + (j + 1).ToString().Trim() + "</td></tr>";
                        }
                    }
                    else
                    {
                        nodeval = -1;
                    }
                }
                myqry = myqry + "'" + Math.Round(nodeval,2).ToString().Trim() + "',";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }






    protected void getMaxRno(string myfield, string mytable)
    {
        try
        {
            com10 = new MySqlCommand("select count(*) from " + mytable, con10);
            con10.Open();
            dr10 = com10.ExecuteReader();
            dr10.Read();
            if (Convert.ToInt32(dr10[0].ToString()) > 0)
            {
                com9 = new MySqlCommand("select max(" + myfield + ") from " + mytable, con9);
                con9.Open();
                dr9 = com9.ExecuteReader();
                dr9.Read();
                zno = Convert.ToInt32(dr9[0].ToString());
                zno = zno + 1;
                con9.Close();
            }
            else
            {
                zno = 1;
            }
            con10.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }









    public string encryption(String strauthkey)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] encrypt;
        UTF8Encoding encode = new UTF8Encoding();
        //encrypt the given password string into Encrypted data  
        encrypt = md5.ComputeHash(encode.GetBytes(strauthkey));
        StringBuilder encryptdata = new StringBuilder();
        //Create a new string by using the encrypted data  
        for (int i = 0; i < encrypt.Length; i++)
        {
            encryptdata.Append(encrypt[i].ToString());
        }
        return encryptdata.ToString();
    }  





}
