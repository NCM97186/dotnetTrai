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
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Net;

public partial class zzTestPostXMLAPI2 : System.Web.UI.Page
{
    SqlCommand com;
    SqlConnection con;
    SqlDataReader dr;
    string links, pageurl;
    double sno, cntr;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SendDataUsingHttps();
            //Response.Write("<br /><br />Done");
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }


    private void SendDataUsingHttps()
    {

        WebRequest req = null;
        WebResponse rsp = null;
        try
        {
            //string fileName = Server.MapPath("TestStructureXML.xml");
            //string fileName = Server.MapPath("SampleXMLAll.xml");
            //string fileName = Server.MapPath("SampleXML.xml");
            //string fileName = Server.MapPath("SampleXMLblank.xml");
            string fileName = Server.MapPath("test2.xml");

            //string uri = "http://mysql1.telecomwatch.co.in/API/webrequest_test.aspx";
            //string uri = "http://13.127.75.202/API/webrequest_test.aspx";
            string uri = "http://localhost:30022/API/xmlapi2.aspx";
            //string uri = "http://tariff.trai.gov.in/API/xmlapi2.aspx";

            req = WebRequest.Create(uri);

            req.Headers["userid"] = "BSNL1";
            req.Headers["password"] = "trg409";
            req.Headers["authkey"] = "xiKS0aix74GynbgyJe4s";   // offline authkey
            //req.Headers["authkey"] = "Yd2ClO2NpuuKxO8aaLST";   // live authkey


            //req.Proxy = WebProxy.GetDefaultProxy(); // Enable if using proxy
            req.Method = "POST";        // Post method
            req.ContentType = "text/xml";     // content type
            // Wrap the request stream with a text-based writer
            StreamWriter writer = new StreamWriter(req.GetRequestStream());
            // Write the XML text into the stream
            writer.WriteLine(this.GetTextFromXMLFile(fileName));
            writer.Close();
            // Send the data to the webserver
            rsp = req.GetResponse();



            //Response.Write(((HttpWebResponse)rsp).StatusDescription + "<br /><br />");
            StreamReader reader = new StreamReader(rsp.GetResponseStream());
            string responseFromServer = reader.ReadToEnd();
            Response.Write(responseFromServer);





        }
        catch (WebException webEx)
        {

        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (req != null) req.GetRequestStream().Close();
            if (rsp != null) rsp.GetResponseStream().Close();
        }
        //Function to read xml data from local system
        /// <span class="code-SummaryComment"><summary></span>
        /// Read XML data from file
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="file"></param></span>
        /// <span class="code-SummaryComment"><returns>returns file content in XML string format</returns></span>




    }



    private string GetTextFromXMLFile(string file)
    {
        StreamReader reader = new StreamReader(file);
        string ret = reader.ReadToEnd();
        //Response.Write(ret);
        reader.Close();
        return ret;
    }


}




