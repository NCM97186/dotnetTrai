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

public partial class FEA_Common : System.Web.UI.Page
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
                Response.Redirect("FEA_logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("FEA_logout.aspx");
            }


            com = new MySqlCommand("select * from TRAI_FEA where(uname='" + user + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            
            if (dr["review"].ToString().Trim()!="None")
            {
                TrMasters.Visible = true;
                Tr1.Visible = true;
                divReview.Visible = true;
            }

            if (dr["parameters"].ToString().Trim() == "Yes")
            {
                TrMasters.Visible = true;
                Tr2.Visible = true;
                divReset.Visible = true;
            }

            if (dr["review"].ToString().Trim() == "Level 4" || dr["review"].ToString().Trim() == "Level 5")
            {
                //TrMasters.Visible = true;
                Tr91.Visible = true;
                divFeedbacks.Visible = true;
            }
            if (dr["review"].ToString().Trim() == "Level 4" || dr["review"].ToString().Trim() == "Level 5")
            {
                //TrMasters.Visible = true;
                Tr92.Visible = true;
                divDownloads.Visible = true;
            }
            
            if (dr["report"].ToString().Trim() == "Yes")
            {
                TrReports.Visible = true;
                Tr51.Visible = true;
                divTariffReport.Visible = true;
                Tr52.Visible = true;
                divExceptionReport.Visible = true;
                Tr53.Visible = true;
                divDelayedReport.Visible = true;
                Tr54.Visible = true;
                divComparisonReport.Visible = true;
                Tr55.Visible = true;
                divAdvanceReport.Visible = true;
                Tr56.Visible = true;
                divTariffSummaryReport.Visible = true;
                Tr57.Visible = true;
                divQuartPrepaid.Visible = true;
                Tr58.Visible = true;
                divQuartPostpaid.Visible = true;
                Tr59.Visible = true;
                divQuartBulk.Visible = true;
                Tr60.Visible = true;
                divQuartBlack.Visible = true;
                Tr61.Visible = true;
                divQuartNotOnOffer.Visible = true;
            }
            

            con.Close();



            /*
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
            */


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

    }
}
