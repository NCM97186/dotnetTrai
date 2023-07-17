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

public partial class zzFEA_reptariff : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    Table tbresults;
    CheckBox[] arrResult;
    CheckBox chkResult;
    int zno;

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

        /*
        if (Request.UrlReferrer == null)
        {
            Response.Redirect("FEA_logout.aspx");
        }
        if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
        {
            Response.Redirect("FEA_logout.aspx");
        }
        */

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

            com = new MySqlCommand("select * from TRAI_operators order by oper", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                ChkOper.Items.Add(dr["oper"].ToString().Trim());
            }
            con.Close();

            com = new MySqlCommand("select * from TRAI_circles where(circ!='All India') order by circ", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                ChkCirc.Items.Add(dr["circ"].ToString().Trim());
            }
            con.Close();

            com = new MySqlCommand("select * from TRAI_ttypes order by rno", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                ChkTtype.Items.Add(dr["ttype"].ToString().Trim());
            }
            con.Close();

        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {

            com = new MySqlCommand("delete from TRAI_tempReporting where(recdate<'" + DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss") + "')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            int flag = 0;
            DateTime ldate1 = Convert.ToDateTime("2001-01-01");
            DateTime ldate2 = Convert.ToDateTime("2030-01-01");
            DateTime rdate1 = Convert.ToDateTime("2001-01-01");
            DateTime rdate2 = Convert.ToDateTime("2030-01-01");
            double mrp1 = 1, mrp2 = 999999;

            string conditions = "(rno>0) ";

            // Operators Selection //
            conditions = conditions + " and (";
            string opers = "";
            for (int i = 0; i < ChkOper.Items.Count; i++)
            {
                if (ChkOper.Items[i].Selected == true)
                {
                    opers = opers + ChkOper.Items[i].Text.Trim();
                    conditions = conditions + "oper='" + ChkOper.Items[i].Text.Trim() + "' or ";
                }
            }
            if (opers != "")
            {
                conditions = conditions.Substring(0, conditions.Length - 4);
            }
            else
            {
                flag = 1;
                Response.Write("<script>alert('Please select at least one TSP');</script>");
            }
            conditions = conditions + ")";
            conditions = conditions.Replace("()", "");


            // Circles Selection //
            conditions = conditions + " and (circ='All India' or ";
            string circs = "";
            for (int i = 0; i < ChkCirc.Items.Count; i++)
            {
                if (ChkCirc.Items[i].Selected == true)
                {
                    circs = circs + ChkCirc.Items[i].Text.Trim();
                    conditions = conditions + "circ='" + ChkCirc.Items[i].Text.Trim() + "' or ";
                }
            }
            if (circs != "")
            {
                conditions = conditions.Substring(0, conditions.Length - 4);
            }
            else
            {
                flag = 1;
                Response.Write("<script>alert('Please select at least one LSA');</script>");
            }
            conditions = conditions + ")";
            conditions = conditions.Replace("()", "");


            // Tariff Type Selection //
            conditions = conditions + " and (";
            string ttypes = "";
            for (int i = 0; i < ChkTtype.Items.Count; i++)
            {
                if (ChkTtype.Items[i].Selected == true)
                {
                    ttypes = ttypes + ChkTtype.Items[i].Text.Trim();
                    conditions = conditions + "ttype='" + ChkTtype.Items[i].Text.Trim() + "' or ";
                }
            }
            if (ttypes != "")
            {
                conditions = conditions.Substring(0, conditions.Length - 4);
            }
            else
            {
                flag = 1;
                Response.Write("<script>alert('Please select at least one Tariff Type');</script>");
            }
            conditions = conditions + ")";
            conditions = conditions.Replace("()", "");

            // Launch Date Range //
            try
            {
                ldate1 = Convert.ToDateTime(TextDate.Text.Trim());
            }
            catch (Exception ex) { }
            try
            {
                ldate2 = Convert.ToDateTime(TextDate2.Text.Trim());
            }
            catch (Exception ex) { }
            conditions = conditions + " and (actiondate>='" + ldate1.ToString("yyyy-MM-dd") + "' and actiondate<'" + ldate2.AddDays(1).ToString("yyyy-MM-dd") + "')";

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
            conditions = conditions + " and (reportdate>='" + rdate1.ToString("yyyy-MM-dd") + "' and reportdate<'" + rdate2.AddDays(1).ToString("yyyy-MM-dd") + "')";


            // Price Range Selection //
            try
            {
                mrp1 = Convert.ToDouble(TextPrice1.Text.Trim());
            }
            catch (Exception ex) { }
            try
            {
                mrp2 = Convert.ToDouble(TextPrice2.Text.Trim());
            }
            catch (Exception ex) { }
            conditions = conditions + " and ((mrp>=" + mrp1 + " and mrp<=" + mrp2 + ") or (ISP_rental>=" + mrp1 + " and ISP_rental<=" + mrp2 + "))";


            if (flag == 0)
            {
                TextConditions.Text = conditions;

                showRecords(null, null);
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }







    protected void showRecords(object s, EventArgs e)
    {
        try
        {
            StringWriter strw = new StringWriter();
            StringBuilder strb = new StringBuilder();
            StringBuilder strb2 = new StringBuilder();

            int showType = 1;      // '1' is for showing records from TRAI_tariffs only, '2' is for showing records from TRAI_tariffs as well as TRAI_archive
            if (ChkArchive.Checked == true)
            {
                showType = 2;
            }

            strb.Append("<table width=100% border=1 style=border-collapse:collapse cellspacing=1 cellpadding=5>");
            strb.Append("<tr>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>TSP</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>LSA</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Tariff Type</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Unique Record ID</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Service Type</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Price</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Validity</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Date of Launch</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Date of Reporting</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Tariff Details</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Delayed Reporting</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Taken On Record</b></td>");
            strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Status</b></td>");
            strb.Append("</tr>");

            string conditions = TextConditions.Text.Trim();
            string sortby = TextSortBy.Text.Trim();
            if (sortby == "")
            {
                sortby = " order by reportdate";
            }

            string tablename = "";
            int totcount = 0;


            getMaxRno("repno", "TRAI_tempReporting");
            int repno = zno;
            int cntr = 1;


            string myqry = "";
            string stat = "";
            for (int a = 1; a <= showType; a++)
            {
                if (a == 1)
                {
                    tablename = "TRAI_tariffs";
                    myqry = "select * from " + tablename + " where(rno>0) and " + conditions;
                    stat = "Active";
                }
                if (a == 2)
                {
                    tablename = "TRAI_archive";
                    myqry = "select * from " + tablename + " where(rno>0) and (actiontotake='WITHDRAWAL') and " + conditions;
                    stat = "Not-Active";
                }


                com = new MySqlCommand(myqry, con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    // calculate delay days //

                    int violationlimit = 7;   // no. of days permitted between launch date and reporting date
                    DateTime Valid7thDay = Convert.ToDateTime(Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy"));
                    Valid7thDay = Valid7thDay.AddDays(1);   // date of action is not to be counted, so start from the next date
                    DateTime Valid8thDay = Valid7thDay;

                    int validdaycount = 0;
                    string delay = "No";
                    string takenonrecord = "No";
                    DateTime tempdate1 = Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                    DateTime tempdate2 = Convert.ToDateTime(dr["actiondate"].ToString().Trim());
                    while (tempdate2 <= tempdate1)
                    {
                        int violationflag = 0;
                        string weekday = tempdate2.ToString("dddd");
                        if (weekday == "Saturday" || weekday == "Sunday")
                        {
                            violationlimit++;    // if its a saturday or sunday, increase the days limit
                            violationflag = 1;
                        }
                        else    // if its a saturday or sunday, no need to check if its a gazetted holiday. hence gazetted holiday check put in else.
                        {
                            com1 = new MySqlCommand("select count(*) from TRAI_holidays where(hdate='" + tempdate2.ToString("yyyy-MM-dd") + "')", con1);
                            con1.Open();
                            dr1 = com1.ExecuteReader();
                            dr1.Read();
                            if (Convert.ToInt32(dr1[0].ToString().Trim()) > 0)
                            {
                                violationlimit++;    // if any holidays falls betweeen actiondate and report date, increase the days limit
                                violationflag = 1;
                            }
                            con1.Close();
                        }

                        if (violationflag == 0)
                        {
                            validdaycount++;
                            if (validdaycount == 7)
                            {
                                Valid7thDay = Valid7thDay.AddDays(7);
                            }
                            if (validdaycount == 8)
                            {
                                Valid8thDay = Valid8thDay.AddDays(8);
                            }
                        }

                        tempdate2 = tempdate2.AddDays(1);
                    }

                    if (Convert.ToDateTime(dr["reportdate"].ToString().Trim()) > Valid8thDay.AddDays(1))
                    {
                        TimeSpan ts2 = Convert.ToDateTime(Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy")) - Valid8thDay;
                        double penaltydays = ts2.TotalDays;
                        penaltydays = penaltydays - 1;    // reporting date is not to be included, so subtract one day from the calculation
                        //violations = "<font color=#ffffff style=font-size:12px;>" + penaltydays.ToString() + " day(s)</font>";
                        if (penaltydays > 0)
                        {
                            delay = "Yes";
                        }
                    }

                    // calculate delay days - CODE ENDS HERE //


                    com1 = new MySqlCommand("select count(*) from TRAI_tariffreviewlog where(uniqueid='" + dr["uniqueid"].ToString().Trim() + "') and (actiontaken='Taken on Record')", con1);
                    con1.Open();
                    dr1 = com1.ExecuteReader();
                    dr1.Read();
                    if (Convert.ToInt32(dr1[0].ToString().Trim()) > 0)
                    {
                        takenonrecord = "Yes";
                    }
                    con1.Close();

                    double mrp = 0;
                    if (dr["categ"].ToString().Trim().ToUpper() == "PREPAID")
                    {
                        mrp = Math.Round(Convert.ToDouble(dr["mrp"].ToString().Trim().Replace("-1", "0")), 2);
                    }
                    else
                    {
                        mrp = Math.Round(Convert.ToDouble(dr["ISP_rental"].ToString().Trim().Replace("-1", "0")), 2);
                    }
                    com1 = new MySqlCommand("insert into TRAI_tempReporting values('" + cntr + "','" + repno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + Request["user"].ToString().Trim() + "','" + dr["oper"].ToString().Trim() + "','" + dr["circ"].ToString().Trim() + "','" + dr["ttype"].ToString().Trim() + "','" + dr["uniqueid"].ToString().Trim() + "','" + dr["categ"].ToString().Trim() + "','" + dr["service"].ToString().Trim() + "','" + mrp + "','" + dr["validity"].ToString().Trim() + "','" + Convert.ToDateTime(dr["actiondate"].ToString()).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dr["reportdate"].ToString()).ToString("yyyy-MM-dd") + "','" + delay + "','" + takenonrecord + "','" + stat + "','" + dr["tariffdet"].ToString().Trim() + "','','','','','','0','0','0','0','0')", con1);
                    con1.Open();
                    com1.ExecuteNonQuery();
                    con1.Close();
                    cntr++;
                }
                con.Close();


            }




            // Display the records //


            divicon.InnerHtml = "<a href=javascript:funExcel() ><img src=images/excel.jpg border=0 /></a>";

            string css = "tablecell3";
            
            strb2.Append("<table width=100% border=1 style=border-collapse:collapse cellspacing=1 cellpadding=5>");
            strb2.Append("<tr>");
            strb2.Append("<td class=" + css + " width=10% align=center valign=top>TSP<br /><br /><a href=javascript:funsort('oper','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('oper','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=8% align=center valign=top>LSA<br /><br /><a href=javascript:funsort('circ','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('circ','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=12% align=center valign=top>Tariff Type<br /><br /><a href=javascript:funsort('ttype','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('ttype','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=10% align=center valign=top>Unique Record ID<br /><a href=javascript:funsort('uniqueid','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('uniqueid','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=7% align=center valign=top>Service Type<br /><a href=javascript:funsort('service','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('service','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=6% align=center valign=top>Price &#8377;<br /><br /><a href=javascript:funsort('mrp','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=7% align=center valign=top>Validity<br /><br /><a href=javascript:funsort('validity','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('validity','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=9% align=center valign=top>Date of Launch<br /><a href=javascript:funsort('actiondate','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('actiondate','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=10% align=center valign=top>Date of Reporting<br /><a href=javascript:funsort('reportdate','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('reportdate','desc');><img src=images/sortdown.png border=0></a></td>");
            strb2.Append("<td class=" + css + " width=7% align=center valign=top>Delayed Reporting</td>");
            strb2.Append("<td class=" + css + " width=7% align=center valign=top>Taken On Record</td>");
            strb2.Append("<td class=" + css + " width=7% align=center valign=top>Status</td>");
            strb2.Append("</tr>");

            com = new MySqlCommand("select * from TRAI_tempReporting where(repno=" + repno + ") " + sortby, con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                strb.Append("<tr>");
                strb.Append("<td align=center valign=top>" + dr["oper"].ToString().Trim() + "</td>");
                strb.Append("<td align=center valign=top>" + dr["circ"].ToString().Trim() + "</td>");
                strb.Append("<td align=center valign=top>" + dr["ttype"].ToString().Trim() + "</td>");
                strb.Append("<td align=center valign=top>" + dr["uniqueid"].ToString().Trim() + "</td>");
                strb.Append("<td align=center valign=top>" + dr["service"].ToString().Trim() + "</td>");
                strb.Append("<td align=center valign=top>" + dr["mrp"].ToString().Trim() + "</td>");
                strb.Append("<td align=center valign=top>" + dr["validity"].ToString().Trim().Replace("-1", "").Replace("-2", "Unlimited") + "</td>");
                strb.Append("<td align=center valign=top>" + Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "") + "</td>");
                strb.Append("<td align=center valign=top>" + Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "") + "</td>");
                strb.Append("<td align=center valign=top>" + dr["tariffdet"].ToString().Trim().Replace("~", "&quot;").Replace("&amp;", "&") + "</td>");
                strb.Append("<td align=center valign=top>" + dr["delay"].ToString().Trim() + "</td>");
                strb.Append("<td align=center valign=top>" + dr["takenonrecord"].ToString().Trim() + "</td>");
                strb.Append("<td align=center valign=top>" + dr["stat"].ToString().Trim() + "</td>");
                strb.Append("</tr>");

                string css2 = "tablecell5b";
                strb2.Append("<tr>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["oper"].ToString().Trim() + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["circ"].ToString().Trim() + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["ttype"].ToString().Trim() + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["uniqueid"].ToString().Trim() + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["service"].ToString().Trim() + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["mrp"].ToString().Trim() + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["validity"].ToString().Trim().Replace("-1", "").Replace("-2", "Unlimited") + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "") + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "") + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["delay"].ToString().Trim() + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["takenonrecord"].ToString().Trim() + "</td>");
                strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["stat"].ToString().Trim() + "</td>");
                strb2.Append("</tr>");
                string det = "";
                det = det + "<a href='FEA_recorddetails.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Tariff Details</u></b></a>";
                det = det + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                det = det + "<a href='FEA_reviewsummary.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Review Details</u></b></a>";
                strb2.Append("<tr>");
                strb2.Append("<td class=tablecell5c colspan=6 align=center valign=top><b>Tariff Summary : </b> " + dr["tariffdet"].ToString().Trim().Replace("~", "&quot;").Replace("&amp;", "&") + "</td>");
                strb2.Append("<td class=tablecell5c colspan=6 align=center valign=top>" + det + "</td>");
                strb2.Append("</tr>");
                strb2.Append("<tr>");
                strb2.Append("<td class=tablecell3b colspan=12 align=center valign=top><hr size=0></td>");
                strb2.Append("</tr>");
                
            }
            con.Close();

            strb.Append("</table>");
            divExcel.InnerHtml = strb.ToString().Trim();

            strb2.Append("</table>");
            divresult.InnerHtml = strb2.ToString().Trim();


            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
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
                Response.Write(ex2.ToString());
            }

            Response.End();
        }
        catch (Exception ex)
        {

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
            Response.Write(ex.ToString().Trim());
        }

    }




}