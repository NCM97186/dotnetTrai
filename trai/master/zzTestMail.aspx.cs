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
using System.Web.Mail;


public partial class zzTestMail : System.Web.UI.Page
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

        

    }





    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {

            MailMessage objEmail = new MailMessage();
            SmtpMail.SmtpServer = "127.0.0.1";
            //SmtpMail.SmtpServer = "";

            objEmail.To = TextBox1.Text.Trim();

            objEmail.From = "tariff.filing@trai.gov.in";
            objEmail.Subject = "Mail for forgot password.";
            objEmail.BodyFormat = MailFormat.Html;
            objEmail.Body = "The forgot password details will be sent through this email ID.";
            SmtpMail.Send(objEmail);

            Response.Write("<script>alert('The test mail has been sent successfully');</script>");

        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }




}
