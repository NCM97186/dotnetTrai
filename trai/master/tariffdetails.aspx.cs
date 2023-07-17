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


public partial class tariffdetails : System.Web.UI.Page
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

        tablename = "TRAI_tariffs";

        try
        {
            /*
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("logout.aspx");
            }
            */

            string uid = Request["uid"].ToString().Trim();

            int exists = 0;
            com = new MySqlCommand("select count(*) from " + tablename + " where(uniqueid='" + uid + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            if(Convert.ToInt32(dr[0].ToString().Trim())>0)
            {
                exists = 1;
            }
            con.Close();

            if(exists==0)
            {
                divresults.InnerHtml = "<b>This tariff is no longer active.";
            }
            else
            {
                com = new MySqlCommand("select * from " + tablename + " where(uniqueid='" + uid + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();

                string strheader = "";
                strheader = strheader + "<table cellspacing=1 cellpadding=4 width=100%><tr>";
                strheader = strheader + "<td align=center valign=bottom class=tablehead width=14%><b>Product</b></td>";
                if (dr["ISP_rental"].ToString().Trim()!="-1")
                {
                    strheader = strheader + "<td align=center valign=bottom class=tablehead width=19%><b>Monthly Rental</b></td>";
                }
                else
                {
                    strheader = strheader + "<td align=center valign=bottom class=tablehead width=9%><b>Price</b></td>";
                }
                strheader = strheader + "<td align=center valign=bottom class=tablehead width=14%><b>Talktime</b></td>";
                strheader = strheader + "<td align=center valign=bottom class=tablehead width=14%><b>Validity</b></td>";
                strheader = strheader + "<td align=center valign=bottom class=tablehead width=9%><b>TSP</b></td>";
                strheader = strheader + "<td align=center valign=bottom class=tablehead><b>Tariff Summary</b></td>";
                strheader = strheader + "</tr>";
              
                string oper = dr["oper"].ToString().Trim();
                string operlogo = "logo" + oper.Replace(" ", "") + ".jpg";
                string ttype = dr["ttype"].ToString().Replace("Prepaid_Plan Voucher", "Plan").Replace("Prepaid_STV", "STV").Replace("Prepaid_Combo", "Combo").Replace("Prepaid_Top Up", "Top Up").Replace("Prepaid_VAS", "VAS").Replace("Promo Offer", "Promo").Replace("Postpaid- Plan", "Plans").Replace("Postpaid- Add On Pack", "Add On Pack").Replace("Postpaid_VAS", "VAS").Replace("Fixed Line -Tariff", "Plan").Replace("Fixed Line Add-On Pack", "Add On Pack").Replace("Postpaid_VAS", "VAS");
            
                
                strheader = strheader + "<tr>";
                strheader = strheader + "<td class=tablecell>" + ttype + "</td>";
                if (dr["ISP_rental"].ToString().Trim() != "-1")
                {
                    strheader = strheader + "<td class=tablecell align=center>" + "&#8377; " + dr["ISP_rental"].ToString().Trim() + "</td>";
                }
                else
                {
                    strheader = strheader + "<td class=tablecell align=center>" + "&#8377; " + dr["mrp"].ToString().Trim() + "</td>";
                }
                if (dr["monval"].ToString().Trim() == "-1")
                {
                    strheader = strheader + "<td class=tablecell align=center>-</td>";
                }
                else
                {
                    strheader = strheader + "<td class=tablecell align=center>&#8377; " + dr["monval"].ToString().Trim() + "</td>";
                }
                if (dr["validity"].ToString().Trim() == "0" || dr["validity"].ToString().Trim() == "-1")
                {
                    strheader = strheader + "<td class=tablecell align=center>-</td>";
                }
                else
                {
                    if (dr["validity"].ToString().Trim() == "-2")
                    {
                        strheader = strheader + "<td class=tablecell align=center>Unlimited</td>";
                    }
                    else
                    {
                        strheader = strheader + "<td class=tablecell align=center>" + dr["validity"].ToString().Trim() + " days</td>";
                    }
                }
                strheader = strheader + "<td class=tablecell align=center><img src=../consumerview/logos/" + operlogo + " border=0 />";
                strheader = strheader + "<td class=tablecell>" + dr["tariffdet"].ToString().Trim() + "</td>";
                strheader = strheader + "</tr>";

                strheader = strheader + "</table>";

                divresults.InnerHtml = strheader;

                con.Close();
            }



        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
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







}
