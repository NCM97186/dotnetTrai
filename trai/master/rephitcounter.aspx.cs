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


public partial class rephitcounter : System.Web.UI.Page
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

        tablename = "TRAI_hitcounter";

        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("logout.aspx");
            }

            if (!IsPostBack)
            {
                TextDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                TextDate2.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }








    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;
            divresults.InnerHtml = "";

            try
            {
                dt1 = Convert.ToDateTime(TextDate.Text.Trim());
                dt2 = Convert.ToDateTime(TextDate2.Text.Trim());
                dt2 = dt2.AddDays(1);
            }
            catch (Exception ex)
            {
                flag = 1;
                Response.Write("<script>alert('Please select a valid date range');</script>");
            }


            if (flag == 0)
            {
                divdownload.InnerHtml = "<p align=right><a href=javascript:funExcel() ><img src=images/excel.jpg border=0 title='Download Excel' /></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>";

                mystr = "<table width=90% cellspacing=1 border=1 style=border-collapse:collapse cellpadding=7>";
                //mystr = mystr + "<tr><td class=tablehead align=center><b>Date</b></td><td class=tablehead align=center><b>Hits from Mobiles</b></td><td class=tablehead align=center><b>Hits from Non-Mobile Devices</b></td></tr>";
                mystr = mystr + "<tr><td class=tablehead align=center><b>Date</b></td><td class=tablehead align=center><b>Website Hits</b></td></tr>";
                string css = "tablecell3";

                int sno = 1;

                DateTime tempdate = dt1;
                while (tempdate < dt2)
                {
                    double mobilecount = 0;
                    double nonmobilecount = 0;
                    com = new MySqlCommand("select count(*) from " + tablename + " where(recdate>='" + tempdate.ToString("yyyy-MM-dd") + "' and recdate<'" + tempdate.AddDays(1).ToString("yyyy-MM-dd") + "') and (device='Mobile')", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    try
                    {
                        mobilecount = Convert.ToDouble(dr[0].ToString().Trim());
                    }
                    catch (Exception ex) { }
                    con.Close();
                    com = new MySqlCommand("select count(*) from " + tablename + " where(recdate>='" + tempdate.ToString("yyyy-MM-dd") + "' and recdate<'" + tempdate.AddDays(1).ToString("yyyy-MM-dd") + "') and (device='Non-Mobile')", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    try
                    {
                        nonmobilecount = Convert.ToDouble(dr[0].ToString().Trim());
                    }
                    catch (Exception ex) { }
                    con.Close();

                    if (sno % 2 == 0)
                    {
                        css = "tablecell3";
                    }
                    else
                    {
                        css = "tablecell3b";
                    }
                    
                    /*
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + tempdate.ToString("dd-MMM-yyyy") + "</td>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + mobilecount.ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + nonmobilecount.ToString().Trim() + "</td>";
                    mystr = mystr + "</tr>";
                    */

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + tempdate.ToString("dd-MMM-yyyy") + "</td>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + (mobilecount + nonmobilecount).ToString().Trim() + "</td>";
                    mystr = mystr + "</tr>";

                    sno++;
                    tempdate = tempdate.AddDays(1);

                }

                mystr = mystr + "</table>";

                divresults.InnerHtml = mystr;

            }


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }







    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        try
        {

            try
            {
                //string attachment = "attachment; filename=TariffProducts.xls";
                string attachment = "attachment; filename=Report.xls";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                StringBuilder sb = new StringBuilder();

                sb.Append(divresults.InnerHtml.ToString());

                Response.Write(sb.ToString());

                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                // Response.Close();

            }
            catch (Exception ex2)
            {
                Response.Write(ex2.ToString());
            }

            Response.End();
        }
        catch (Exception ex)
        {

        }
    }









    protected void getMaxRno(string myfield, string mytable)
    {
        try
        {

            com = new MySqlCommand("select count(*) from " + mytable, con1);
            con1.Open();
            dr = com.ExecuteReader();
            dr.Read();
            if (Convert.ToInt32(dr[0].ToString()) > 0)
            {
                com = new MySqlCommand("select max(" + myfield + ") from " + mytable, con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                zno = Convert.ToInt32(dr[0].ToString());
                zno = zno + 1;
                con.Close();
            }
            else
            {
                zno = 1;
            }
            con.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }






    protected void TextDate_PreRender(object s, EventArgs e)
    {
        TextDate.Attributes.Add("onfocus", "showCalender(calender,this)");
    }


    protected void TextDate2_PreRender(object s, EventArgs e)
    {
        TextDate2.Attributes.Add("onfocus", "showCalender(calender,this)");
    }




}
