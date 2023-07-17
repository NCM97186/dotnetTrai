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

public partial class common : System.Web.UI.Page
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
                Response.Redirect("logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("logout.aspx");
            }


            com = new MySqlCommand("select * from TRAI_admin where(uname='" + user + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            rights = dr["rights"].ToString().Trim();
            con.Close();

            if (rights.Contains("masterusers_") || rights == "All")
            {
                TrMasters.Visible = true;
                Tr1.Visible = true;
                divUsers.Visible = true;
            }
            if (rights.Contains("masteroperators_") || rights == "All")
            {
                TrMasters.Visible = true;
                Tr2.Visible = true;
                divOperators.Visible = true;
            }
            if (rights.Contains("mastercircles_") || rights == "All")
            {
                TrMasters.Visible = true;
                Tr3.Visible = true;
                divCircles.Visible = true;
            }
            if (rights.Contains("masterTSPLogins_") || rights == "All")
            {
                TrMasters.Visible = true;
                Tr4.Visible = true;
                divTSPLogins.Visible = true;
            }
            if (rights.Contains("FEALogins_") || rights == "All")
            {
                TrMasters.Visible = true;
                Tr5.Visible = true;
                divFEALogins.Visible = true;
            }
            if (rights.Contains("TTypes_") || rights == "All")
            {
                TrMasters.Visible = true;
                Tr6.Visible = true;
                divTTypes.Visible = true;
            }
            if (rights.Contains("ReportFeedbacks") || rights == "All")
            {
                //TrMasters.Visible = true;
                Tr91.Visible = true;
                divFeedbacks.Visible = true;
            }
            if (rights.Contains("ReportDownloads") || rights == "All")
            {
                //TrMasters.Visible = true;
                Tr92.Visible = true;
                divDownloads.Visible = true;
            }
            if (rights.Contains("ReportHitCounter") || rights == "All")
            {
                //TrMasters.Visible = true;
                Tr93.Visible = true;
                divHitCounter.Visible = true;
            }
            if (rights.Contains("FDAmount") || rights == "All")
            {
                //TrMasters.Visible = true;
                Tr94.Visible = true;
                divFDAmount.Visible = true;
            }
            if (rights.Contains("ReportTariffCount") || rights == "All")
            {
                //TrMasters.Visible = true;
                Tr95.Visible = true;
                divTariffCount.Visible = true;
            }







            
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }
}
