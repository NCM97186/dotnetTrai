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

public partial class FEA_login : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, str2;
    int cntr, rno;
    DateTime masterdate;
 Random random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

 if (!IsPostBack)
        {
           
            FillCapctha();
        }
    }
    
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;


            if (flag == 0)
            {
                str1 = TextBox1.Text.Trim().Replace("'","`");
                str2 = TextBox2.Text.Trim().Replace("'", "`");

                //if (str1.ToUpper() == "PRADV")
                if (str1.ToUpper() == "PR-ADVISOR")
                {
                    
                    // if there are some records with duplicate unique ID's, keep 1 and send the rest to TRAI_duptariffs table
                    // This code is being used on zzRemoveDuplicates.aspx and FEA_Login.aspx 

                    try
                    {
                        int loginflag = 0;
                        com = new MySqlCommand("select * from TRAI_lastloginprad", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        if (DateTime.Now < Convert.ToDateTime(dr["lastlogin"].ToString().Trim()).AddMinutes(15))
                        {
                            loginflag = 1;
                        }
                        con.Close();

                        if(loginflag==0)
                        {
                            com = new MySqlCommand("update TRAI_lastloginprad set lastlogin='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'", con);
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();

                            com = new MySqlCommand("select count(*) as num, uniqueid from TRAI_tariffs group by uniqueid order by num desc", con);
                            con.Open();
                            dr = com.ExecuteReader();
                            while (dr.Read())
                            {
                                int mycount = Convert.ToInt32(dr[0].ToString().Trim());
                                string myid = dr["uniqueid"].ToString().Trim();
                                if (mycount > 1)  // if more than one records by same unique ID
                                {
                                    com2 = new MySqlCommand("select count(*) from TRAI_tariffs where(uniqueid='" + myid + "')", con2);
                                    con2.Open();
                                    dr2 = com2.ExecuteReader();
                                    dr2.Read();
                                    if (Convert.ToInt32(dr2[0].ToString().Trim()) > 1)
                                    {
                                        // move all copies of this record to the TRAI_duplicates table
                                        com1 = new MySqlCommand("insert into TRAI_duplicates select * from TRAI_tariffs where (uniqueid='" + myid + "')", con1);
                                        con1.Open();
                                        com1.ExecuteNonQuery();
                                        con1.Close();

                                        // delete top 1 copy of this record
                                        com1 = new MySqlCommand("delete from TRAI_tariffs where uniqueid='" + myid + "' limit 1", con1);
                                        con1.Open();
                                        com1.ExecuteNonQuery();
                                        con1.Close();
                                    }
                                    con2.Close();
                                }
                            }
                            con.Close();
                        }
                    }
                    catch (Exception ex) { }

                    // if there are some records with duplicate unique ID's, keep 1 and send the rest to TRAI_duptariffs table - CODE ENDS HERE

                }


                // Check for Login ///
                //com = new MySqlCommand("select count(*) from TRAI_admin where(BINARY uname='" + str1 + "') and (BINARY pass='" + str2 + "')", con);  // 'BINARY' for matching case sensitive
                //com = new MySqlCommand("select count(*) from TRAI_FEA where(BINARY uname='" + str1 + "') and (BINARY pass='" + str2 + "')", con);  // 'BINARY' for matching case sensitive
                com = new MySqlCommand("select count(*) from TRAI_FEA where(uname='" + str1 + "') and (BINARY pass='" + str2 + "')", con);  // 'BINARY' for matching case sensitive
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                cntr = Convert.ToInt32(dr[0].ToString().Trim());
                con.Close();
                if (cntr > 0)
                {
                    Session["master"] = str1;
                    //Response.Redirect("FEA_frame.aspx?uname=" + str1);
                    Response.Redirect("FEA_frame.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Please enter valid login details');</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

 protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillCapctha();
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
protected void CaptchaControl1_Load(object sender, EventArgs e)
    {

        if (ViewState["captcha"] != null)
        {

            txtCaptcha.Text = "";
        }




    }


}