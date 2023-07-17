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

public partial class FEA_reptariffsummary : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    Table tbresults;
    CheckBox[] arrResult;
    CheckBox chkResult;
    int zno;
    double[] arrColCount;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["master"] == null)
        {
            //Response.Redirect("sessout.aspx");
        }

        Server.ScriptTimeout = 999999;

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        if (Request.UrlReferrer == null)
        {
            Response.Redirect("FEA_logout.aspx");
        }
        if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
        {
            Response.Redirect("FEA_logout.aspx");
        }

        /*
        int resultsize = 20000;
        arrResult = new CheckBox[resultsize];
        for (int ii = 0; ii < resultsize; ii++)
        {
            chkResult = new CheckBox();
            arrResult[ii] = chkResult;
            divChkResults.Controls.Add(arrResult[ii]);
        }
        */

        if (!IsPostBack)
        {
            /*
            TextDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TextDate2.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TextDate3.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            TextDate4.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            */

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {

            int flag = 0;
            DateTime rdate1 = Convert.ToDateTime("2001-01-01");
            DateTime rdate2 = Convert.ToDateTime("2030-01-01");

            // Report Date Range //
            try
            {
                rdate1 = Convert.ToDateTime(TextDate3.Text.Trim());
            }
            catch (Exception ex) { }
            try
            {
                rdate2 = Convert.ToDateTime(TextDate4.Text.Trim());
            }
            catch (Exception ex) { }
            if (rdate1 > rdate2)
            {
                flag = 1;
                Response.Write("<script>alert('From Date Can not be greater than To Date.');</script>");
                TextDate3.Focus();
                return;
            }


            string operconditions = "";
            if(RadOType.SelectedItem.Text=="TSP")
            {
                operconditions += " (upper(oper)='AIRCEL' or upper(oper)='AIRTEL' or upper(oper)='BSNL' or upper(oper)='IDEA' or upper(oper)='JIO' or upper(oper)='MTNL' or upper(oper)='QUADRANT (CONNECT)' or upper(oper)='TATA TELE' or upper(oper)='TELENOR' or upper(oper)='VODAFONE' or upper(oper)='VODAFONE IDEA' or upper(oper)='SURFTELECOM' or upper(oper)='AEROVOYCE')";
            }
            if (RadOType.SelectedItem.Text == "ISP")
            {
                operconditions += " (upper(oper)!='AIRCEL' and upper(oper)!='AIRTEL' and upper(oper)!='BSNL' and upper(oper)!='IDEA' and upper(oper)!='JIO' and upper(oper)!='MTNL' and upper(oper)!='QUADRANT (CONNECT)' and upper(oper)!='TATA TELE' and upper(oper)!='TELENOR' and upper(oper)!='VODAFONE' and upper(oper)!='VODAFONE IDEA' and upper(oper)!='SURFTELECOM' and upper(oper)!='AEROVOYCE')";
            }
            if (RadOType.SelectedItem.Text == "Both")
            {
                // no condition here, as all TSP/ISP are to be included
                operconditions += " (rno>0)";
            }

            int cols = 0;
            //com = new MySqlCommand("select count(distinct(oper)) from TRAI_operators order by oper", con);
            com = new MySqlCommand("select count(distinct(oper)) from TRAI_operators where(rno>0) and " + operconditions + " order by oper", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                cols = Convert.ToInt32(dr[0].ToString().Trim());
            }
            con.Close();

            string excellink = "<p align=right style=margin-right:20px;><a href=javascript:funExcel() ><img src=images/excel.jpg border=0 /></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=javascript:funPrint() ><img src=images/iconprint.png width=30 border=0 /></a></p>";



            string mystr = "<font face=arial><b><u>Tariff Summary : " + rdate1.ToString("dd-MMM-yyyy") + " to " + rdate2.ToString("dd-MMM-yyyy") + "</u></b></font><br /><br /><table width=98% style=border-collapse:collapse; cellspacing=1 border=1 cellpadding=5>";

            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablehead align=center><b>Tariff Type / Name of TSP</b></td>";
            //com = new MySqlCommand("select distinct(oper) from TRAI_operators order by oper", con);
            com = new MySqlCommand("select distinct(oper) from TRAI_operators where(rno>0) and " + operconditions + " order by oper", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                mystr = mystr + "<td class=tablehead align=center width=7%><b>" + dr[0].ToString().Trim() + "</b></td>";
            }
            con.Close();
            mystr = mystr + "<td class=tablehead align=center width=7%><b>Total</b></td>";
            mystr = mystr + "</tr>";

            arrColCount = new double[cols];

            int rowno = 1;
            com1 = new MySqlCommand("select distinct(ttype) from TRAI_ttypes order by ttype", con1);
            con1.Open();
            dr1 = com1.ExecuteReader();
            while (dr1.Read())
            {
                string css = "tablecell3";
                if (rowno % 2 == 0)
                {
                    css = "tablecell3b";
                }
                mystr = mystr + "<tr>";
                double opertotal = 0;
                mystr = mystr + "<td class=" + css + " align=left><b>" + dr1[0].ToString().Trim() + "</b></td>";
                int colno = 0;
                //com = new MySqlCommand("select distinct(oper) from TRAI_operators order by oper", con);
                com = new MySqlCommand("select distinct(oper) from TRAI_operators where(rno>0) and " + operconditions + " order by oper", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    com2 = new MySqlCommand("select count(*) from TRAI_tariffs where (ttype='" + dr1[0].ToString().Trim() + "') and (oper='" + dr[0].ToString().Trim() + "') and (reportdate>='" + rdate1.ToString("yyyy-MM-dd") + "' and reportdate<'" + rdate2.AddDays(1).ToString("yyyy-MM-dd") + "')", con2);
                    con2.Open();
                    dr2 = com2.ExecuteReader();
                    while (dr2.Read())
                    {
                        mystr = mystr + "<td class=" + css + " align=right>" + dr2[0].ToString().Trim() + "</td>";
                        arrColCount[colno] += Convert.ToDouble(dr2[0].ToString().Trim());
                        opertotal += Convert.ToDouble(dr2[0].ToString().Trim());
                        colno++;
                    }
                    con2.Close();
                }
                con.Close();
                mystr = mystr + "<td class=" + css + " align=right><b>" + opertotal.ToString() + "</b></td>";
                mystr = mystr + "</tr>";
                rowno++;
            }
            con1.Close();

            double gtotal = 0;
            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablehead align=left><b>Total</b></td>";
            for (int i = 0; i < cols; i++)
            {
                mystr = mystr + "<td class=tablecell3 align=right><b>" + arrColCount[i].ToString() + "</b></td>";
                gtotal += Convert.ToDouble(arrColCount[i].ToString().Trim());
            }
            mystr = mystr + "<td class=tablecell3 align=right><b>" + gtotal.ToString() + "</b></td>";
            mystr = mystr + "</tr>";

            mystr = mystr + "</table><br /><br />";

            divLink.InnerHtml = excellink;
            divExcel.InnerHtml = "<center>" + mystr + "</center>";

            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);

        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
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

                sb.Append(divExcel.InnerHtml.ToString());

                Response.Write(sb.ToString());

                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                // Response.Close();

            }
            catch (Exception ex2)
            {
                Response.Write("<script>alert('" + ex2.ToString() + "');</script>");
            }

            Response.End();
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    protected void TextDate3_PreRender(object s, EventArgs e)
    {
        TextDate3.Attributes.Add("onfocus", "showCalender(calender,this)");
    }

    protected void TextDate4_PreRender(object s, EventArgs e)
    {
        TextDate4.Attributes.Add("onfocus", "showCalender(calender,this)");
    }

    protected void getMaxRno(string myfield, string mytable)
    {
        try
        {
            com10 = new MySqlCommand("select count(*) from " + mytable, con10);
            con10.Open();
            dr10 = com10.ExecuteReader();
            dr10.Read();
            if (Convert.ToInt32(dr10[0].ToString()) > 0)
            {
                com9 = new MySqlCommand("select max(" + myfield + ") from " + mytable, con9);
                con9.Open();
                dr9 = com9.ExecuteReader();
                dr9.Read();
                zno = Convert.ToInt32(dr9[0].ToString());
                zno = zno + 1;
                con9.Close();
            }
            else
            {
                zno = 1;
            }
            con10.Close();

        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }

    }
}