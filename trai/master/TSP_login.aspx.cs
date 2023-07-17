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

public partial class TSP_login : System.Web.UI.Page
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
            Session["salt"] = random.Next(59999, 199999).ToString();
            FillCapctha();
            ViewState["captcha"] = txtCaptcha.Text;
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;

            if (Check1.Checked == false || Check2.Checked == false)
            {
                flag = 1;
                Response.Write("<script>alert('Please select both the checkboxes before submitting the form.');</script>");
            }

            if (flag == 0)
            {
                str1 = TextBox1.Text.Trim().Replace("'", "`");
                str2 = TextBox2.Text.Trim().Replace("'", "`");

                // Check for Login ///
                if (Session["captcha"].ToString() == txtCaptcha.Text.Trim())
                {
                    com = new MySqlCommand("select count(*) from TRAI_TSPUsers where(BINARY uname='" + str1 + "') and (BINARY pass='" + str2 + "')", con);  // 'BINARY' for matching case sensitive
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    cntr = Convert.ToInt32(dr[0].ToString().Trim());
                    con.Close();
                    if (cntr > 0)
                    {
                        Session["TSP"] = str1;
                        //Response.Redirect("TSP_frame.aspx?uname=" + str1);
                        Response.Redirect("TSP_frame.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Please enter valid login details');</script>");
                    }

                }

                else
                {

                    Response.Write("<script>alert('Please enter correct captcha code.');</script>");
                    FillCapctha();
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