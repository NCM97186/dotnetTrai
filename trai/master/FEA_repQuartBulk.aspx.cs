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

public partial class FEA_repQuartBulk : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    Table tbresults;
    int zno;
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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

            if (!IsPostBack)
            {
                /*
                DropOperator.Items.Add("");
                com = new MySqlCommand("select * from TRAI_operators order by oper", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    DropOperator.Items.Add(dr["oper"].ToString().Trim());
                }
                con.Close();
                */

                RadOType_Change(null, null);

                DropCircle.Items.Add("");
                com = new MySqlCommand("select * from TRAI_circles order by circ", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    DropCircle.Items.Add(dr["circ"].ToString().Trim());
                }
                con.Close();

                DropQtr.Items.Add("");
                DropQtr.Items.Add("March");
                DropQtr.Items.Add("June");
                DropQtr.Items.Add("September");
                DropQtr.Items.Add("December");

                int curryr = Convert.ToInt32(DateTime.Now.Year);
                DropYr.Items.Add("");
                for (int i = 2019; i <= curryr; i++)
                {
                    DropYr.Items.Add(i.ToString().Trim());
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }





    protected void RadOType_Change(object sender, EventArgs e)
    {
        try
        {
            string operconditions = "";
            if (RadOType.SelectedItem.Text == "TSP")
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

            DropOperator.Items.Clear();
            DropOperator.Items.Add("");
            com = new MySqlCommand("select * from TRAI_operators where " + operconditions + " order by oper", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                DropOperator.Items.Add(dr["oper"].ToString().Trim());
            }
            con.Close();
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
            string tablename = "TRAI_QuartBulk";
            string oper = DropOperator.SelectedItem.Text.Trim();
            string circ = DropCircle.SelectedItem.Text.Trim();
            string qtr = DropQtr.SelectedItem.Text.Trim();
            string yr = DropYr.SelectedItem.Text.Trim();
            DateTime repdate = Convert.ToDateTime("1/1/2011");

            tdHr1.InnerHtml = "";
            tdHr2.InnerHtml = "";
            tdRepDate.InnerHtml = "";
            tdDelay.InnerHtml = "";
            tdNotice.InnerHtml = "";

            if (DropOperator.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a TSP / ISP');</script>");
            }
            if (DropCircle.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a LSA');</script>");
            }
            if (DropQtr.SelectedItem.Text.Trim() == "" || DropYr.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a Quarter and Year');</script>");
            }

            if (flag == 0)
            {
                int plancount = 0;
                string conditions = " where (oper='" + oper + "') and (circ='" + circ + "') and (qtr='" + qtr + "') and (yr='" + Convert.ToInt32(yr) + "')";
                
                string mystr = "";
                mystr = mystr + "<table width=100% style=border-collapse:collapse;min-width:450px; cellspacing=1 border=1 cellpadding=5>";
                mystr = mystr + "<tr><td class=tablecell align=right colspan=2><b>Name of Service Provider : </b></td><td class=tablecell align=left colspan=2><b>" + oper + "</b></td></tr>";
                mystr = mystr + "<tr><td class=tablecell align=right colspan=2><b>Name of Service Area : </b></td><td class=tablecell align=left colspan=2><b>" + circ + "</b></td></tr>";
                mystr = mystr + "<tr><td class=tablecell align=center colspan=4><b>Quarterly Report for Bulk Tariff plans of Corporate Plans for the Quarter ending " + qtr + " " + yr.ToString() + "</b></td></tr>";
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell width=10% align=center><b>S. No.</b></td>";
                mystr = mystr + "<td class=tablecell width=40% align=center><b>Names of Tariff Plans for Bulk/Corporate/SME's</b></td>";
                mystr = mystr + "<td class=tablecell width=15% align=center><b>No. of Subscribers</b></td>";
                mystr = mystr + "<td class=tablecell width=35% align=center><b>Brief details of Bulk Tariff Plan</b></td>";
                mystr = mystr + "</tr>";



                int sno = 1;
                double tot = 0;
                com = new MySqlCommand("select * from " + tablename + conditions + " order by rno", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    
                    if(sno==1)
                    {

                        // Check for Delay in Reporting //
                        int mnth = 0;
                        if (qtr == "March")
                        {
                            mnth = 3;
                        }
                        if (qtr == "June")
                        {
                            mnth = 6;
                        }
                        if (qtr == "September")
                        {
                            mnth = 9;
                        }
                        if (qtr == "December")
                        {
                            mnth = 12;
                        }
                        int lastdayofmonth = DateTime.DaysInMonth(Convert.ToInt32(yr), Convert.ToInt32(mnth));

                        repdate = Convert.ToDateTime(dr["recdate"].ToString().Trim());
                        DateTime QtrEndDate = Convert.ToDateTime(yr + "/" + mnth + "/" + lastdayofmonth);
                        TimeSpan ts = repdate - QtrEndDate;
                        int delay = ts.Days;
                        delay = delay - 7;   // 7 days is the limit for filing.

                        tdHr1.InnerHtml = "<hr size=0>";
                        tdHr2.InnerHtml = "<hr size=0>";
                        tdRepDate.InnerHtml = "<b>Reporting Date : " + repdate.ToString("dd-MMM-yyyy") + "</b>";
                        if (delay > 0)
                        {
                            tdDelay.InnerHtml = "<b>Delay in Reporting : " + delay.ToString() + " Day(s)</b>";
                            tdNotice.InnerHtml = "<a href=javascript:funNotice('" + delay + "','" + QtrEndDate.ToString("dd-MMM-yyyy") + "','" + repdate.ToString("dd-MMM-yyyy") + "');><img src=images/word.jpg alt='Show Cause Notice' title='Show Cause Notice' style=margin-bottom:5px; border=0></a>";
                        }
                        else
                        {
                            tdDelay.InnerHtml = "<b>Delay in Reporting : Nil</b>";
                        }

                        // Check for Delay in Reporting - Code Ends Here //

                    }

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center valign=top>" + sno + "</td>";
                    mystr = mystr + "<td class=tablecell align=center valign=top>" + dr["planname"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell align=right valign=top>" + dr["subscribers"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell align=center valign=top>" + dr["remarks"].ToString().Trim() + "</td>";
                    mystr = mystr + "</tr>";
                    sno++;
                    

                    try
                    {
                        tot += Convert.ToDouble(dr["subscribers"].ToString().Trim());
                    }
                    catch (Exception ex) { }
                }
                con.Close();


                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell align=center></td>";
                mystr = mystr + "<td class=tablecell align=right><b>Total</b></td>";
                mystr = mystr + "<td class=tablecell align=right><b>" + tot.ToString().Trim() + "</b></td>";
                mystr = mystr + "<td class=tablecell align=center></td>";
                mystr = mystr + "</tr>";

                mystr = mystr + "</table><br /><br />";

                TdData.InnerHtml = mystr;
                TdData.Visible = true;
                TdPrint.Visible = true;

                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);
            }
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
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Data.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Output.Write(TdData.InnerHtml);
            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {
            //Response.Write(ex.ToString());
        }
    }





    protected void ButtonNotice_Click(object sender, EventArgs e)
    {
        try
        {
            string tablename = "TRAI_QuartBulk";
            string strBody = "<html><head></head><body>";

            string violations = "";
            double dailypenalty = 0;
            int totalpenalty = 0;
            string penaltyinwords = "";
            com = new MySqlCommand("select * from TRAI_penaltyparameters", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            try
            {
                dailypenalty = Convert.ToDouble(dr["penaltyperday"].ToString().Trim());
            }
            catch (Exception ex) { }
            con.Close();

            string oper = DropOperator.SelectedItem.Text.Trim();
            string compname = "";
            string addr = "";
            string circ = DropCircle.SelectedItem.Text.Trim();
            string qtr = DropQtr.SelectedItem.Text.Trim();
            string yr = DropYr.SelectedItem.Text.Trim();
            string delay = TextDelay.Text.Trim();
            string qtrenddt = TextQtrEndDate.Text.Trim();
            string repdt = TextRepDate.Text.Trim();

            try
            {
                string violationsdet = "";
                violations = "None";


                try
                {
                    totalpenalty = Convert.ToInt32(dailypenalty * Convert.ToDouble(delay));
                    penaltyinwords = ConvertNumbertoWords(totalpenalty);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }

                try
                {
                    com = new MySqlCommand("select * from TRAI_operators where(oper='" + oper + "')", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    compname = dr["company"].ToString().Trim();
                    addr = dr["addr"].ToString().Trim();
                }
                catch (Exception ex) { }
                con.Close();

                strBody = strBody + "<b>Notice Matter to be provided by TRAI</b>";
                /*
                strBody = strBody + "<font style='font-family:Bookman Old Style';><center><b>NOTICE TO SHOW CAUSE</b></center><p style=text-align:right>Dated: " + DateTime.Now.ToString("dd-MMM-yyyy") + "</p>";
                strBody = strBody + "<p style=text-align:justify;>Subject: <b>Show Cause Notice to M/s " + compname + " for non- compliance of the provisions of the Telecommunication Tariff Order, 1999.</b></p>";
                strBody = strBody + "<p style=text-align:justify;>F.No._________________: Whereas the Telecom Regulatory Authority of India [hereinafter referred to as the Authority], established under sub-section (1) of section 3 of the Telecom Regulatory Authority of India Act, 1997 (24 of 1997) (hereinafter referred to as TRAI Act, 1997), has been entrusted with discharge of certain functions, <i>inter alia</i>, to ensure compliance of terms and conditions of licence; regulate the telecommunication services; protect interest of consumers of the telecom sector;</p>";
                strBody = strBody + "<p style=text-align:justify;>2. 	And whereas, in exercise of the powers conferred upon it under sub-section (2) of section 11 of the TRAI Act, 1997 (24 of 1997), the Authority made the Telecommunication Tariff Order, 1999 dated the 9th March 1999 (hereinafter referred to as TTO, 1999);</p>";
                strBody = strBody + "<p style=text-align:left;>3. 	And whereas sub-clause (db) and (l) of clause 2 of the TTO, 1999 provides as under:-<br /><br />&quot;2(db): `Date of Reporting` means the date on which the report from a service provider regarding the proposed plan or any change in the existing tariff plan, is received at the Authority`s office.<br /><br />2(l): `Reporting Requirement` means the obligation of a service provider to report to the Authority any new tariff for telecommunication services under this order and/or any changes therein within seven working days from the date of implementation of the said tariff for information and record of the Authority after conducting a self-check to ensure that the tariff plan(s) is/are consistent with the regulatory principles in all respects which inter-alia include IUC compliance, Non-discrimination & non-predation`.&quot;;</p>";
                strBody = strBody + "<p style=text-align:justify;>4.   And whereas M/s " + compname + ", vide their letter no. _________________ dated ___________________, reported tariffs in respect of all LSAs;</p>";
                strBody = strBody + "<p style=text-align:justify;>5. 	And whereas the said tariff report was required to be received within seven working days from the date of implementation as per reporting requirement provided under 2(l) of the TTO, 1999;</p>";
                strBody = strBody + "<p style=text-align:justify;>6. 	And whereas the _______ in respect of all circles was implemented w.e.f. " + qtrenddt.ToString() + " but information regarding the same was received in the office on " + repdt.ToString() + ";</p>";
                strBody = strBody + "<p style=text-align:justify;>7.	And whereas M/s " + compname + ";</p>";
                strBody = strBody + "<p style=text-align:justify;>8. 	And whereas sub-clause (iii) of clause 7 of the TTO, 1999 provides as under:-<br /><br />&quot;Clause 7(iii): If any service provider fails to comply with the Reporting Requirement, it shall, without prejudice to the terms and conditions of its licence, or the provisions of the Act or rules or regulations or orders made, or directions issued, thereunder, be liable to pay five thousand rupees, by way of financial disincentive, for every day of delay subject to maximum of two lakh rupees as the Authority may by order direct:<br /><br />Provided that no order for payment of any amount by way of financial disincentive shall be made by the Authority unless the service provider has been given a reasonable opportunity of representing against the contravention of the tariff order observed by the Authority.&quot;;</p>";
                strBody = strBody + "<p style=text-align:justify;>9.    And whereas it is observed that there was a delay of " + delay.ToString().Trim() + " days in reporting the tariff plans by M/s " + compname + " in contravention of the provisions of the TTO, 1999;</p>";
                strBody = strBody + "<p style=text-align:justify;>10.    Now, therefore, M/s " + compname + " is called upon to show cause within two weeks from the date of issue of this notice as to why financial disincentive should not be levied against them for contravention of the provisions of Telecommunication Tariff Order, 1999 and in case no written explanation is received within the stipulated time it will be presumed that M/s " + compname + " has nothing to offer in their defence.</p>";
                strBody = strBody + "<p style=text-align:right;>Yours sincerely,<br /><br />(S.K. Mishra)<br />Principal Advisor (F&EA)<br />Tel: 011-23221856<p>";
                strBody = strBody + "<p style=text-align:left;>To,<br />" + compname + "<br />" + addr + "</p>";
                */




                strBody = strBody + "</font></body></html>";
                
                string fileName = "ShowCause.doc";

                // You can add whatever you want to add as the HTML and it will be generated as Ms Word docs

                Response.AppendHeader("Content-Type", "application/msword");
                //Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

                Response.AppendHeader("Content-disposition", "attachment; filename=" + fileName);

                Response.Write(strBody);

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString().Trim());
            }
        }

        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }
    }



    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "ZERO";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        /*
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
            number %= 1000000;
        }
         */
        if ((number / 10000000) > 0)
        {
            words += ConvertNumbertoWords(number / 10000000) + " CRORE ";
            number %= 10000000;
        }
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " LAC ";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "AND ";
            //var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            //var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

            string[] unitsMap = new string[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            string[] tensMap = new string[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
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