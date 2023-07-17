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

public partial class FEA_parameters : System.Web.UI.Page
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


        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("FEA_logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("FEA_logout.aspx");
            }

            if(!IsPostBack)
            {
                com = new MySqlCommand("select * from TRAI_penaltyparameters",con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                TextBox1.Text = Math.Round(Convert.ToDouble(dr["penaltyperday"].ToString().Trim())).ToString();
                con.Close();
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
            int flag = 0;
            double penalty = 0;

            try
            {
                penalty = Convert.ToDouble(TextBox1.Text.Trim());
            }
            catch (Exception ex) {
                flag=1;
                Response.Write("<script>alert('Please enter an amount.');</script>");
            }
            
            if(flag==0)
            {
                com = new MySqlCommand("update TRAI_penaltyparameters set penaltyperday=" + penalty, con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                Response.Write("<script>alert('Financial Disincentive amount has bee updated.');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }




}