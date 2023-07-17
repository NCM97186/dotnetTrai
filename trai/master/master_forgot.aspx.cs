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

public partial class master_forgot : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, str2, mystr;
    int cntr, rno;
    DateTime masterdate;
    protected void Page_Load(object sender, EventArgs e)
    {
        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;
            str1 = TextBox1.Text.Trim();
            str2 = TextBox2.Text.Trim();

            if (str1 == "" && str2 == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter either the username or the registered email ID');</script>");
            }

            if(flag==0)
            {
                if(str1!="")
                {
                    com = new MySqlCommand("select count(*) from TRAI_admin where(uname='" + str1 + "')", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    cntr = Convert.ToInt32(dr[0].ToString().Trim());
                    con.Close();
                    if(cntr==0)
                    {
                        flag = 1;
                        Response.Write("<script>alert('The entered username does not exist in the database');</script>");
                    }
                }
                else
                {
                    com = new MySqlCommand("select count(*) from TRAI_admin where(email='" + str2 + "')", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    cntr = Convert.ToInt32(dr[0].ToString().Trim());
                    con.Close();
                    if (cntr == 0)
                    {
                        flag = 1;
                        Response.Write("<script>alert('The entered email ID does not exist in the database');</script>");
                    }
                }
            }


            if (flag == 0)
            {
                string uname = "";
                string pass = "";
                string email = "";

                if (str1 != "")
                {
                    com = new MySqlCommand("select * from TRAI_admin where(uname='" + str1 + "')", con);
                }
                else
                {
                    com = new MySqlCommand("select * from TRAI_admin where(email='" + str2 + "')", con);
                }
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    uname = dr["uname"].ToString().Trim();
                    pass = dr["pass"].ToString().Trim();
                    email = dr["email"].ToString().Trim();
                }
                catch (Exception ex) { }
                con.Close();

                if(email!="")
                {
                    mystr = "Dear Sir / Madam,<br /><br />Your login details are as given below : <br /><br /><br />";
                    mystr = mystr + "Username : " + uname + "<br /><br />";
                    mystr = mystr + "Password : " + pass + "<br /><br />";
                    mystr = mystr + "<br /><br />warm regards,<br /><br /><br />TRAI";
                    
                    
                    MailMessage objEmail = new MailMessage();
                    SmtpMail.SmtpServer = "127.0.0.1";
                    //SmtpMail.SmtpServer = "";
                    
                    objEmail.To = email;
                    objEmail.From = "tariff.filing@trai.gov.in";
                    //objEmail.Headers.Add("reply-to", "tariff.filing@trai.gov.in");
                    objEmail.Subject = "Login Details";
                    objEmail.BodyFormat = MailFormat.Html;
                    objEmail.Body = mystr;
                    SmtpMail.Send(objEmail);

                    Response.Write("<script>alert('Your login details have been mailed at your registered email ID. Please check your inbox.');</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }




}