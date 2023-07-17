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


public partial class TSP_activerecords : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno, size;
    DateTime dt1, dt2;
    double modrno;
    DateTime repdate1, repdate2, launchdate1, launchdate2;


    protected void Page_Load(object sender, EventArgs e)
    {

        Server.ScriptTimeout = 999999;

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        tablename = "TRAI_tariffs";

        try
        {
            
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("TSP_logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("TSP_logout.aspx");
            }
            


            if(!IsPostBack)
            {
                DropLSA.Items.Add("");
                com = new MySqlCommand("select * from TRAI_circles order by circ", con);
                con.Open();
                dr = com.ExecuteReader();
                while(dr.Read())
                {
                    DropLSA.Items.Add(dr["circ"].ToString().Trim());
                }
                con.Close();

                DropProductType.Items.Add("");
                com = new MySqlCommand("select * from TRAI_ttypes order by rno", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    DropProductType.Items.Add(dr["ttype"].ToString().Trim());
                }
                con.Close();

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
            string oper="";
            double price1 = 0, price2 = 0, validity1 = 0, validity2 = 0;
            
            if(string.IsNullOrEmpty(DropLSA.SelectedItem.Text.Trim()))
            {
                Response.Write("<script>alert('Please select LSA.');</script>");
                DropLSA.Focus();
                return;
            }

            if (string.IsNullOrEmpty(DropProductType.SelectedItem.Text.Trim()))
            {
                Response.Write("<script>alert('Please select ProductType.');</script>");
                DropProductType.Focus();
                return;
            }

            try
            {
                price1 = Convert.ToDouble(TextPrice1.Text.Trim().ToUpper());
            }
            catch (Exception ex)
            {
                price1 = -2;   //  -2 is used for unlimited.
            }
            try
            {
                price2 = Convert.ToDouble(TextPrice2.Text.Trim().ToUpper());
            }
            catch (Exception ex)
            {
                price2 = 9999999;
            }
            if (price1 > price2)
            {
                Response.Write("<script>alert('Price From Can not be greater than Price To');</script>");
                TextPrice1.Focus();
                return;
            }

            try
            {
                validity1 = Convert.ToDouble(TextValidity1.Text.Trim().ToUpper().Replace("UNLIMITED", "-2"));
            }
            catch (Exception ex)
            {
                validity1 = -2;   //  -2 is used for unlimited.
            }
            try
            {
                validity2 = Convert.ToDouble(TextValidity2.Text.Trim().ToUpper().Replace("UNLIMITED", "-2"));
            }
            catch (Exception ex)
            {
                validity2 = 999999;   //  -2 is used for unlimited.
            }
            if (validity1 > validity2)
            {
                Response.Write("<script>alert('Validity From Can not be greater than Validity To');</script>");
                TextValidity1.Focus();
                return;
            }


            try
            {
                repdate1 = Convert.ToDateTime(TextDate.Text.Trim());
            }
            catch (Exception ex)
            {
                repdate1 = Convert.ToDateTime("1/1/2001");
            }
            try
            {
                repdate2 = Convert.ToDateTime(TextDate2.Text.Trim());
            }
            catch (Exception ex)
            {
                repdate2 = Convert.ToDateTime("1/1/2040");
            }
            if (repdate1 > repdate2)
            {
                Response.Write("<script>alert('ReportDate From Can not be greater than ReportDate To');</script>");
                TextDate.Focus();
                return;
            }

            try
            {
                launchdate1 = Convert.ToDateTime(TextDate3.Text.Trim());
            }
            catch (Exception ex)
            {
                launchdate1 = Convert.ToDateTime("1/1/2001");
            }
            try
            {
                launchdate2 = Convert.ToDateTime(TextDate4.Text.Trim());
            }
            catch (Exception ex)
            {
                launchdate2 = Convert.ToDateTime("1/1/2040");
            }
            if (launchdate1 > launchdate2)
            {
                Response.Write("<script>alert('LaunchDate From Can not be greater than LaunchDate To');</script>");
                TextDate3.Focus();
                return;
            }

            com =new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + Request["user"].ToString().Trim() + "')",con);
            con.Open();
            dr=com.ExecuteReader();
            dr.Read();
            oper=dr["oper"].ToString().Trim();
            con.Close();            


            //string myqry = "select * from " + tablename + " where(oper='" + oper + "')";
            string myqry = "select distinct(uniqueid) from " + tablename + " where(oper='" + oper + "')";
            if(DropLSA.SelectedItem.Text!="")
            {
                myqry = myqry + " and (circ='" + DropLSA.SelectedItem.Text.Trim() + "' or circ='All India')";
            }
            if (DropProductType.SelectedItem.Text != "")
            {
                myqry = myqry + " and (ttype='" + DropProductType.SelectedItem.Text.Trim() + "')";
            }

            myqry = myqry + " and ((mrp>=" + price1 + " and mrp<=" + price2 + ") or (ISP_rental>=" + price1 + " and ISP_rental<=" + price2 + "))";

            myqry = myqry + " and (validity>=" + validity1 + " and validity<=" + validity2 + ")";

            myqry = myqry + " and (reportdate>='" + repdate1.ToString("yyyy-MM-dd") + "' and reportdate<='" + repdate2.ToString("yyyy-MM-dd") + "')";

            myqry = myqry + " and (actiondate>='" + launchdate1.ToString("yyyy-MM-dd") + "' and actiondate<='" + launchdate2.ToString("yyyy-MM-dd") + "')";

            myqry = myqry + " order by uniqueid";


            mystr = "<table width=95% cellspacing=1 cellpadding=5>";
            //mystr = mystr + "<tr><td colspan=8 align=right><img src=images/excel.jpg border=0></td></tr>";


            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablehead align=center width=4%>S. No.</td>"; 
            mystr = mystr + "<td class=tablehead align=center width=8%>LSA</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Tariff Type</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Price / Monthly Rental (&#8377;)</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Validity (days)</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Unique Record ID</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Reporting Date</td>";
            mystr = mystr + "<td class=tablehead align=center width=8%>Launch Date</td>";
            mystr = mystr + "<td class=tablehead align=center>Tariff Summary</td>";
            mystr = mystr + "<td class=tablehead align=center width=7%>Details</td>";
            mystr = mystr + "</tr>";

            int sno = 1;

            com1 = new MySqlCommand(myqry, con1);
            con1.Open();
            dr1 = com1.ExecuteReader();
            while (dr1.Read())
            {
                com = new MySqlCommand("select * from " + tablename + " where(uniqueid='" + dr1[0].ToString().Trim() + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell3 align=left>" + sno.ToString() + "</td>";
                    mystr = mystr + "<td class=tablecell3 align=center>" + dr["circ"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell3 align=center>" + dr["ttype"].ToString().Trim() + "</td>";
                    double price = 0;
                    if (dr["categ"].ToString().Trim() == "Prepaid")
                    {
                        price = Convert.ToDouble(dr["mrp"].ToString().Trim());
                    }
                    else
                    {
                        price = Convert.ToDouble(dr["ISP_rental"].ToString().Trim());
                    }
                    mystr = mystr + "<td class=tablecell3 align=center>" + price.ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "") + "</td>";
                    if (dr["validity"].ToString().Trim() == "0")
                    {
                        mystr = mystr + "<td class=tablecell3 align=center>-</td>";
                    }
                    else
                    {
                        mystr = mystr + "<td class=tablecell3 align=center>" + dr["validity"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "-") + "</td>";
                    }
                    mystr = mystr + "<td class=tablecell3 align=center>" + dr["uniqueid"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell3 align=center>" + Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>";
                    mystr = mystr + "<td class=tablecell3 align=center>" + Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>";
                    mystr = mystr + "<td class=tablecell3 align=left>" + dr["tariffdet"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell3 align=center><a href='TSP_recorddetails.aspx?r=" + dr["rno"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><img src=images/lensplus.png border=0></a></td>";
                    mystr = mystr + "</tr>";

                    sno++;
                }
                catch (Exception ex) { }
                con.Close();
            }
            con1.Close();


            divTariffs.InnerHtml = mystr;

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

    protected void TextDate3_PreRender(object s, EventArgs e)
    {
        TextDate3.Attributes.Add("onfocus", "showCalender(calender,this)");
    }

    protected void TextDate4_PreRender(object s, EventArgs e)
    {
        TextDate4.Attributes.Add("onfocus", "showCalender(calender,this)");
    }
}
