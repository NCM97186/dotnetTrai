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

public partial class TSP_Common : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    int rno;
    string user, rights;

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
            user = Request["user"].ToString().Trim();


            if (Request.UrlReferrer == null)
            {
                Response.Redirect("TSP_logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("TSP_logout.aspx");
            }


            /*
            if (rights.Contains("masterusers_") || rights == "All")
            {
                TrMasters.Visible = true;
                Tr1.Visible = true;
                divUsers.Visible = true;
            }
            */

            string useroperator = "";
            com = new MySqlCommand("select * from TRAI_TSPUsers where (uname='" + user + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            useroperator = dr["oper"].ToString().Trim();
            con.Close();



            if (useroperator.ToUpper() == "AIRCEL" || useroperator.ToUpper() == "AIRTEL" || useroperator.ToUpper() == "BSNL" || useroperator.ToUpper() == "IDEA" || useroperator.ToUpper() == "JIO" || useroperator.ToUpper() == "MTNL" || useroperator.ToUpper() == "QUADRANT (CONNECT)" || useroperator.ToUpper() == "TATA TELE" || useroperator.ToUpper() == "TELENOR" || useroperator.ToUpper() == "VODAFONE" || useroperator.ToUpper() == "VODAFONE IDEA" || useroperator.ToUpper() == "SURFTELECOM" || useroperator.ToUpper() == "AEROVOYCE")
            {
                TrQuart.Visible = true;
                Tr3.Visible = true;
                Tr4.Visible = true;
                Tr5.Visible = true;
                Tr6.Visible = true;
            }


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }
}
