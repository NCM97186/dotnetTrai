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
using System.Xml.Schema;   // for generating XML
using System.Xml;          // for generating XML

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


public partial class feedback : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, str2, mystr, tablename, ttype, colwidth0, colwidth, pricelabel, talktimelabel;
    double zno;

    static String username = "traibcssprp";
    static String password = "trai_sprp@bcs90";
    static String senderid = "TRAIND";
    static String message = "";
    static String mobileNo = "";
    //static String mobileNos = "9856XXXXX, 9856XXXXX ";
    //static String scheduledTime = "20010819 13:26:00";
    static HttpWebRequest request;
    static Stream dataStream;

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

        tablename = "TRAI_feedbacks";

        try
        {
            
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }





    protected void ButtonOTP_Click(object sender, EventArgs e)
    {
        try
        {
            int flag=0;
            string securekey = "46768929-9465-4d4c-85ef-04aff971d602";

            string mobile = TextMobile.Text.Trim().Replace("'", "`");
            if (flag == 0 && mobile.Length !=10)
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a 10 digit mobile number.');</script>");
            }

            if(flag==0)
            {
                //TextHidden.Text = "1234";

                // Generate a 4 characters random number //

                var chars = "0123456789";
                var stringChars = new char[4];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var OTP = new String(stringChars);
            
                TextHidden.Text = OTP;

                // Generate a 10 characters random string - CODE ENDS HERE //



                // Send OTP via SMS //

                string message="Your OTP for TRAI Consumer View Feedback is " + OTP;
                mobileNo = mobile;
                request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
                request.ProtocolVersion = HttpVersion.Version10;
                //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                Console.WriteLine("Before Calling Method");
                sendSingleSMS(username, password, senderid, mobileNo, message, securekey);
                
                // Send OTP via SMS - CODE ENDS HERE //


                Response.Write("<script>alert('Please enter the OTP sent on your mobile number.');</script>");
            }
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
            string pname = TextName.Text.Trim().Replace("'", "`");
            string mobile = TextMobile.Text.Trim().Replace("'", "`");
            string otp = TextOTP.Text.Trim().Replace("'", "`");
            string feedback = TextFeedback.Text.Trim().Replace("'", "`").Replace(Convert.ToChar(34), Convert.ToChar(126));

            int flag = 0;

            if(flag==0 && pname=="")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter your name.');</script>");
            }
            if (flag == 0 && mobile == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter your mobile number.');</script>");
            }
            if (flag == 0 && mobile.Length !=10)
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a 10 digit mobile number.');</script>");
            }
            if (flag == 0 && otp == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter the OTP.');</script>");
            }
            if (flag == 0 && feedback == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter your feedback.');</script>");
            }


            if(flag==0)
            {
                if (TextOTP.Text.Trim() != TextHidden.Text.Trim())
                {
                    flag = 1;
                    Response.Write("<script>alert('You have entered an invalid OTP. Please check again.');</script>");
                }
                else
                {
                    getMaxRno("rno", tablename);
                    com = new MySqlCommand("insert into " + tablename + " values('" + zno + "','" + pname + "','" + mobile + "','" + feedback + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','Non-Mobile')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Clone();

                    Table1.Visible = false;
                    divresults.Visible = true;

                }

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

            com9 = new MySqlCommand("select count(*) from " + mytable, con9);
            con9.Open();
            dr9 = com9.ExecuteReader();
            dr9.Read();
            if (Convert.ToInt32(dr9[0].ToString()) > 0)
            {
                com10 = new MySqlCommand("select max(" + myfield + ") from " + mytable, con10);
                con10.Open();
                dr10 = com10.ExecuteReader();
                dr10.Read();
                zno = Convert.ToDouble(dr10[0].ToString());
                zno = zno + 1;
                con10.Close();
            }
            else
            {
                zno = 1;
            }
            con9.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }



    /*
        
    public static void sendSingleSMS(String username, String password, String senderid, String mobileNo, String message)
    {
        String smsservicetype = "singlemsg"; //For single message.
        String query = "username=" + HttpUtility.UrlEncode(username) + "&password=" + HttpUtility.UrlEncode(password) + "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) + "&content=" + HttpUtility.UrlEncode(message) + "&mobileno=" + HttpUtility.UrlEncode(mobileNo) + "&senderid=" + HttpUtility.UrlEncode(senderid); 
        byte[] byteArray = Encoding.ASCII.GetBytes(query);
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteArray.Length;
        dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        WebResponse response = request.GetResponse();
        String Status = ((HttpWebResponse)response).StatusDescription;
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();
        reader.Close();
        dataStream.Close();
        response.Close();
        
    }
    */


    public String sendSingleSMS(String username, String password, String senderid, String mobileNo, String message, String secureKey)
        {
            //Latest Generated Secure Key
            Stream dataStream;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest");
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;
            //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
            request.Method = "POST";
            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy2();
            String encryptedPassword = encryptedPasswod(password);
            String NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
            String smsservicetype = "singlemsg"; //For single message.
            String query = "username=" + HttpUtility.UrlEncode(username.Trim()) + "&password=" + HttpUtility.UrlEncode(encryptedPassword) + "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) + "&content=" + HttpUtility.UrlEncode(message.Trim()) + "&mobileno=" + HttpUtility.UrlEncode(mobileNo) + "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) + "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim());
            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }



    protected String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] pp = sha1.ComputeHash(encPwd);
            // static string result = System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    
    
    protected String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            //static byte[] pwd = new byte[encPwd.Length];
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }

}





    class MyPolicy2 : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
            {
                return true;
            }
        }


