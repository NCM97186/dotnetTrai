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

public partial class master_changeadminpass : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["master"] == null)
        {
            //Response.Redirect("sessout.aspx");
        }

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        if (Request.UrlReferrer == null)
        {
            Response.Redirect("logout.aspx");
        }
        if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
        {
            Response.Redirect("logout.aspx");
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {

            if (TextBox1.Text.Trim() != TextBox2.Text.Trim())
            {
                Response.Redirect("invalid.aspx?t1=password. The confirm password field does not match");
            }
            com = new MySqlCommand("update TRAI_admin set pass='" + TextBox1.Text.Trim() + "' where(uname='" + Request["user"].ToString().Trim() + "')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

           
            Session.Abandon();

            tablepass.Visible = false;
            divmsg.Visible = true;

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
}