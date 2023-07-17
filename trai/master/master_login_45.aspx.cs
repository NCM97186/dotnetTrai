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
using System.Security.Cryptography;

public partial class master_login : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
   
    string str1, str2;
    int cntr, rno;
    DateTime masterdate;
    string randumid = "", logoutPath = "";
    Random random = new Random();
   

    protected void Page_Load(object sender, EventArgs e)
    {
        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        if(!IsPostBack)
        {
            FillCapctha();
            TextBox.Attributes.Add("style", "display:none;");
            TextBox.Width = 1;
            TextBox.Height = 1;
            Session["salt"] = random.Next(59999, 199999).ToString();
        }

        if(TextBox.Text.Trim()!="")
        {
            TextBox.Attributes.Add("style", "display:block;");
            TextBox.Width = 500;
            TextBox.Height = 40;
            ButtonQry.Attributes.Add("style", "display:block;");
            divres.Attributes.Add("style", "display:block;");
        }
    }
   
    #region[ Captch code genereted]

    public void FillCapctha()
    {
        try
        {

            Random random = new Random();
            string combination = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
            StringBuilder captcha = new StringBuilder();
            for (int i = 0; i < 6; i++)
                captcha.Append(combination[random.Next(combination.Length)]);
            Session["captcha"] = captcha.ToString();
            imgCaptcha.ImageUrl = "GenerateCaptcha.aspx?" + DateTime.Now.Ticks.ToString();
        }
        catch
        {
            throw;
        }
    }
    #endregion
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillCapctha();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;


            if (flag == 0)
            {
                Session["msg"] = "";
                Session["Temp"] = random.Next(59999, 199999).ToString();
               if( (Session["captcha"].ToString() == txtCaptcha.Text.Trim()))
                {
                    str1 = TextBox1.Text.Trim();
                    str2 = Convert.ToString(hfpwd.Value);
                
                    com = new MySqlCommand("select * from TRAI_admin where(BINARY uname='" + str1 + "')", con);   // 'BINARY' for matching case sensitive
                    con.Open();
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        string password = dr["pass"].ToString();

                        string Tpass = hashcodegenerate.GetSHA512(password + Session["salt"].ToString());
                        string Upass = hfpwd.Value;
                        if (Tpass == Upass)
                        {
                            Session["master"] = str1;
                            
                            Response.Redirect("master_frame.aspx");
                        }
                        con.Close();
                    }

                  

                    else
                    {
                        Response.Write("<script>alert('Please enter valid login details');</script>");
                    }
                }
                
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }


    public class hashcodegenerate
    {
        public static string GetSHA512(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA512Managed sha512hasher = new SHA512Managed();

            byte[] hashBytes = sha512hasher.ComputeHash(encoder.GetBytes(phrase));
            return BytesToHex(hashBytes);

            //UTF8Encoding encoder = new UTF8Encoding();
            //SHA256Managed sha256hasher = new SHA256Managed();

            //byte[] hashBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));
            //return BytesToHex(hashBytes);
        }

        public static String GenerateSHA256Hash(String input, String salt) // Function to add user input and randomly generated salt
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sha256hashstring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256hashstring.ComputeHash(bytes);
            return BytesToHex(hash);
        }

        public static string BytesToHex(byte[] bytes)
        {
            Project_Variables p_Val = new Project_Variables();
            StringBuilder hexString = new StringBuilder(bytes.Length);
            for (p_Val.i = 0; p_Val.i < bytes.Length; p_Val.i++)
            {
                hexString.Append(bytes[p_Val.i].ToString("X2"));
            }
            return hexString.ToString();
        }
    }
    public class Project_Variables : IDisposable
    {
        #region default constructor zone
        private bool disposed = false;
        public Project_Variables()
        {

        }

        #endregion

        #region variables declaration zone

        //Area for boolean type variables 

        public bool draft, review, publish, Edit, Delete, repeald, Hindi, English, flag;
        public bool uploadStatus = true;

        //End

        //Area for all ADO type variables 

    
        //End

        //Area for all integer type variables

        public Int32 Result, Result1, commands;

        public int dataKeyID, moduleid, pageIndex, Depttid, CurrentPage, position, menuid, LanguageID;
        public int i, j, k, linkid, count = 0;



        public int petition_id, rp_id, rtiid, pa_id, status_id;

        //End

        //Area for all datetime type variables

        public DateTime date;

        //End

        #endregion
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // TO DO: clean up managed objects
                }

                // TO DO: clean up unmanaged objects

                disposed = true;
            }
        }
    }
    protected void ButtonQry_Click(object sender, EventArgs e)
    {
        try
        {
            string str1 = TextBox.Text.Trim();
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
                    mystr = mystr + "</table><br /><br /><br />";




                    divres.InnerHtml = mystr;

                }
                else
                {
                    com = new MySqlCommand(str1, con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();



                    divres.InnerHtml = mystr;
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