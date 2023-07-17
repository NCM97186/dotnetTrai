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

public partial class FEA_repQuartPostpaid : System.Web.UI.Page
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
            string prepost = "Postpaid";
            string tablename = "TRAI_QuartData";
            string oper = DropOperator.SelectedItem.Text.Trim();
            string circ = DropCircle.SelectedItem.Text.Trim();
            string qtr = DropQtr.SelectedItem.Text.Trim();
            string yr = DropYr.SelectedItem.Text.Trim();

            tdHr1.InnerHtml = "";
            tdHr2.InnerHtml = "";
            tdRepDate.InnerHtml = "";
            tdDelay.InnerHtml = "";
            tdNotice.InnerHtml = "";
            DateTime repdate = Convert.ToDateTime("1/1/2011");

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
                string conditions = " where(prepost='" + prepost + "') and (oper='" + oper + "') and (circ='" + circ + "') and (qtr='" + qtr + "') and (yr='" + Convert.ToInt32(yr) + "')";
                com = new MySqlCommand("select count(*) from " + tablename + conditions, con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    plancount = Convert.ToInt32(dr[0].ToString().Trim());
                }
                con.Close();

                int arrsize = 50;
                string[,] arrParameters = new string[plancount, arrsize];
                for (int i = 0; i < plancount; i++)
                {
                    for (int j = 0; j < arrsize; j++)
                    {
                        arrParameters[i, j] = "";
                    }
                }

                string mystr = "<font face=arial><b><u>Quarterly Postpaid Tariff Reporting (Annexure - II)</u></b></font><br /><br />";
                mystr = mystr + "<table width=30% style=border-collapse:collapse;min-width:450px; cellspacing=1 border=1 cellpadding=5>";
                mystr = mystr + "<tr><td class=tablecell align=left>Name of Service Provider</td><td class=tablecell align=left>" + oper + "</td></tr>";
                mystr = mystr + "<tr><td class=tablecell align=left>Circle / Service Area</td><td class=tablecell align=left>" + circ + "</td></tr>";
                mystr = mystr + "<tr><td class=tablecell align=left>Quarter Ending</td><td class=tablecell align=left>" + qtr + " " + yr.ToString() + "</td></tr>";
                mystr = mystr + "<tr><td class=tablecell align=left><b>A. Total No. of Existing Plans</b></td><td class=tablecell align=left><b>" + plancount.ToString() + "</b></td></tr>";
                mystr = mystr + "</table><br />";

                if (plancount > 0)
                {
                    mystr = mystr + "<table style=border-collapse:collapse;min-width:600px; cellspacing=1 border=1 cellpadding=5>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Sl No.</td>";
                    mystr = mystr + "<td class=tablecell align=center>Particulars / Services</td>";
                    mystr = mystr + "<td class=tablecell align=center>Details</td>";
                    for (int i = 1; i <= plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center><p style=width:100px;>" + i.ToString() + "</p></td>";
                    }
                    mystr = mystr + "</tr>";

                    int tmp1 = 0;
                    com = new MySqlCommand("select * from " + tablename + conditions + " order by rno", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        arrParameters[tmp1, 0] = dr["planname"].ToString().Trim();
                        arrParameters[tmp1, 1] = dr["uniqueid"].ToString().Trim();
                        arrParameters[tmp1, 2] = dr["subscribers"].ToString().Trim();
                        arrParameters[tmp1, 3] = dr["weblink"].ToString().Trim();
                        repdate = Convert.ToDateTime(dr["recdate"].ToString().Trim());
                        tmp1++;
                    }
                    con.Close();


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

                    DateTime QtrEndDate = Convert.ToDateTime(yr + "/" + mnth + "/" + lastdayofmonth);
                    TimeSpan ts = repdate - QtrEndDate;
                    int delay = ts.Days;
                    delay = delay - 21;   // 21 days is the limit for filing.

                    tdHr1.InnerHtml = "<hr size=0>";
                    tdHr2.InnerHtml = "<hr size=0>";
                    tdRepDate.InnerHtml = "<b>Reporting Date : " + repdate.ToString("dd-MMM-yyyy") + "</b>";
                    if (delay > 0)
                    {
                        tdDelay.InnerHtml = "<b>Delay in Reporting : " + delay.ToString() + " Day(s)</b>";
                        tdNotice.InnerHtml = "<a href=javascript:funNotice('" + prepost + "','" + delay + "','" + QtrEndDate.ToString("dd-MMM-yyyy") + "','" + repdate.ToString("dd-MMM-yyyy") + "');><img src=images/word.jpg alt='Show Cause Notice' title='Show Cause Notice' style=margin-bottom:5px; border=0></a>";
                    }
                    else
                    {
                        tdDelay.InnerHtml = "<b>Delay in Reporting : Nil</b>";
                    }

                    // Check for Delay in Reporting - Code Ends Here //


                    for (int i = 0; i < plancount; i++)
                    {
                        string myqry = "";
                        string uid = arrParameters[i, 1];
                        com = new MySqlCommand("select count(*) from TRAI_tariffs where(uniqueid='" + uid + "')", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
                        {
                            myqry = "select * from TRAI_tariffs where(uniqueid='" + uid + "') order by rno desc limit 1";
                        }
                        else
                        {
                            myqry = "select * from TRAI_archive where(uniqueid='" + uid + "') order by rno desc limit 1";
                        }
                        con.Close();
                        com2 = new MySqlCommand(myqry, con2);
                        con2.Open();
                        dr2 = com2.ExecuteReader();
                        while (dr2.Read())
                        {
                            if (Convert.ToInt32(dr2["regcharges"].ToString().Trim()) > 0)
                            {
                                arrParameters[i, 4] = dr2["regcharges"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            }
                            else
                            {
                                if (Convert.ToInt32(dr2["actcharges"].ToString().Trim()) > 0)
                                {
                                    arrParameters[i, 4] = dr2["actcharges"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                                }
                            }

                            arrParameters[i, 5] = dr2["planfee"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            if (Convert.ToInt32(dr2["isp_advance"].ToString().Trim()) > 0)
                            {
                                arrParameters[i, 6] = dr2["isp_advance"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            }
                            else
                            {
                                if (Convert.ToInt32(dr2["othercharges"].ToString().Trim()) > 0)
                                {
                                    arrParameters[i, 6] = dr2["othercharges"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                                }
                            }

                            arrParameters[i, 7] = dr2["security_localstd"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 8] = dr2["security_LSI"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 9] = dr2["security_int_roam"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 10] = dr2["security_other"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 11] = dr2["isp_rental"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 12] = "";
                            arrParameters[i, 13] = "";
                            arrParameters[i, 14] = "";
                            arrParameters[i, 15] = "";
                            arrParameters[i, 16] = "";
                            arrParameters[i, 17] = "";

                            arrParameters[i, 18] = dr2["local_all_voicepulse"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 19] = "";

                            arrParameters[i, 20] = dr2["local_all_voicecharges"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 21] = dr2["local_on_voice_peak"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 22] = dr2["local_off_voice_peak"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 23] = dr2["local_fix_on_voice_peak"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 24] = dr2["std_on_voice_peak"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 25] = dr2["std_off_voice_peak"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 26] = dr2["std_fix_on_voice_peak"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 27] = dr2["isd_mobile"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 28] = dr2["isd_landline"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 29] = dr2["sms_all_local"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 30] = dr2["sms_all_national"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 31] = dr2["sms_int"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 32] = dr2["datacharges_home"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 33] = dr2["datacharges_roam"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 34] = dr2["datacharges_rental"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 35] = dr2["roam_call_voice_out"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 36] = dr2["roam_call_voice_std"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 37] = dr2["roam_call_voice_in"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 38] = (dr2["roam_sms_local"].ToString().Trim() + " / " + dr2["roam_sms_nat"].ToString().Trim() + " / " + dr2["roam_sms_int"].ToString().Trim()).Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 39] = dr2["introam_calls_outlocal"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 40] = dr2["introam_calls_other"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 41] = dr2["introam_in_calls"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 42] = dr2["introam_out_sms"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");

                            arrParameters[i, 43] = dr2["roam_sms_local"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 44] = dr2["roam_sms_nat"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");
                            arrParameters[i, 45] = "NIL";
                            arrParameters[i, 46] = dr2["roam_sms_int"].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;");


                        }
                        con2.Close();
                    }


                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center></td>";
                    mystr = mystr + "<td class=tablecell align=center></td>";
                    mystr = mystr + "<td class=tablecell align=center>Plan Name</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 0].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center></td>";
                    mystr = mystr + "<td class=tablecell align=center></td>";
                    mystr = mystr + "<td class=tablecell align=center>Unique Reference No.</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 1].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>1</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>Registration /Activation/Installation fees/ charges, if any</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 4].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=2>2</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=2>One Time charge, if any (specify whether convertible to security deposit)</td>";
                    mystr = mystr + "<td class=tablecell align=center>Plan enrolment</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 5].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Advance rental / other charges</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 6].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=4>3</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=4>Security Deposit (Refundable), if any</td>";
                    mystr = mystr + "<td class=tablecell align=center>Local + STD</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 7].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>ISD</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 8].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>International Roaming</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 9].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Others</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 10].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>4</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>Monthly Rental/minimum billing amount if any</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 11].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    /*
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=5>5</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=5>Free/discounted calls/ SMS/data transfer, if allowed (specify the conditions, if any)</td>";
                    mystr = mystr + "<td class=tablecell align=center>Local</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 12].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>STD</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 13].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>ISD</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 14].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>SMS</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 15   ].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Data</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 16].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>6</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>Monthly usage discount/ discount against fixed charges, if any</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 17].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    */

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>5</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>Pulse Rate</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 18].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    /*
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>8</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>Bill Period/Cycle</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 19].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    */

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>6</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>Call Charges</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 20].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>7a</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>Local</td>";
                    mystr = mystr + "<td class=tablecell align=center>Mobile on net</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 21].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Mobile off net</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 22].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Fixed (onnet/offnet)</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 23].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>7b</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>STD</td>";
                    mystr = mystr + "<td class=tablecell align=center>Mobile on net</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 24].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Mobile off net</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 25].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Fixed (onnet/offnet)</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 26].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>7c</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>ISD (Mobile)</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 27].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>7d</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>ISD (Landline)</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 28].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>8</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>SMS</td>";
                    mystr = mystr + "<td class=tablecell align=center>Local</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 29].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>National</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 30].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>International</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 31].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>9</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>Data</td>";
                    mystr = mystr + "<td class=tablecell align=center>Home</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 32].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>National Roaming</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 33].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>International Roaming</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 34].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>10a</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=3>Call Charges while National Roaming</td>";
                    mystr = mystr + "<td class=tablecell align=center>Local Outgoing</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 35].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>STD outgoing</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 36].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Incoming</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 37].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    /*
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>SMS (L/N/International)</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 38].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    */

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=4>10b</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=4>Call Charges while International Roaming</td>";
                    mystr = mystr + "<td class=tablecell align=center>Local Outgoing</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 39].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>STD outgoing</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 40].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Incoming</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 41].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>SMS (L/N/International)</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 42].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=4>10c</td>";
                    mystr = mystr + "<td class=tablecell align=center rowspan=4>SMS Charges while National Roaming</td>";
                    mystr = mystr + "<td class=tablecell align=center>Local Outgoing</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 43].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>STD outgoing</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 44].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>Incoming</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 45].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>SMS (International)</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 46].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";


                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>11</td>";
                    mystr = mystr + "<td class=tablecell align=center>Blackout days</td>";
                    string blackoutdays = "";
                    DateTime dt1 = Convert.ToDateTime("1/1/2011");
                    DateTime dt2 = Convert.ToDateTime("1/1/2011");
                    if (qtr == "March")
                    {
                        dt1 = Convert.ToDateTime("01-Jan-" + yr.ToString());
                        dt2 = Convert.ToDateTime("31-Mar-" + yr.ToString());
                    }
                    if (qtr == "June")
                    {
                        dt1 = Convert.ToDateTime("01-Apr-" + yr.ToString());
                        dt2 = Convert.ToDateTime("30-Jun-" + yr.ToString());
                    }
                    if (qtr == "September")
                    {
                        dt1 = Convert.ToDateTime("01-Jul-" + yr.ToString());
                        dt2 = Convert.ToDateTime("30-Sep-" + yr.ToString());
                    }
                    if (qtr == "December")
                    {
                        dt1 = Convert.ToDateTime("01-Oct-" + yr.ToString());
                        dt2 = Convert.ToDateTime("31-Dec-" + yr.ToString());
                    }
                    com = new MySqlCommand("select * from TRAI_QuartBlackOut where(oper='" + oper + "') and (circ='" + circ + "') and (yr=" + Convert.ToInt32(yr) + ")", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            if (Convert.ToDateTime(dr["date" + i.ToString().Trim()].ToString().Trim()) >= dt1 && Convert.ToDateTime(dr["date" + i.ToString().Trim()].ToString().Trim()) <= dt2)
                            {
                                blackoutdays += Convert.ToDateTime(dr["date" + i.ToString().Trim()].ToString().Trim()).ToString("dd-MMM-yyyy") + "<br />";
                            }
                        }
                    }
                    con.Close();
                    mystr = mystr + "<td class=tablecell align=center>" + blackoutdays.ToString().Trim() + "</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center></td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>12</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>Number of subscribers as on last day of Quarter ending</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 2].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center>13</td>";
                    mystr = mystr + "<td class=tablecell align=center colspan=2>Website link (URL) to the details of the tariff plans</td>";
                    for (int i = 0; i < plancount; i++)
                    {
                        mystr = mystr + "<td class=tablecell align=center>" + arrParameters[i, 3].ToString().Trim() + "</td>";
                    }
                    mystr = mystr + "</tr>";

                    mystr = mystr + "</table><br />";
                }


                int plans = -1;
                int subs = -1;
                com = new MySqlCommand("select * from TRAI_QuartNotOnOffer " + conditions + "  order by rno desc limit 1", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    try
                    {
                        plans = Convert.ToInt32(dr["plans"].ToString().Trim());
                        subs = Convert.ToInt32(dr["subscribers"].ToString().Trim());
                    }
                    catch (Exception ex) { }
                }
                con.Close();

                mystr = mystr + "<table width=30% style=border-collapse:collapse;min-width:450px; cellspacing=1 border=1 cellpadding=5>";
                mystr = mystr + "<tr><td class=tablecell align=left colspan=2><b>B. Tariff plans existing in the billing system but not on offer</b></td></tr>";
                if (plans >= 0)
                {
                    mystr = mystr + "<tr><td class=tablecell align=left>i. Number of tariff plans</td><td class=tablecell align=left>" + plans.ToString() + "</td></tr>";
                    mystr = mystr + "<tr><td class=tablecell align=left>ii. Number of total subscribers as on last day of the quarter</td><td class=tablecell align=left>" + subs.ToString() + "</td></tr>";
                }
                else
                {
                    mystr = mystr + "<tr><td class=tablecell align=left colspan=2> - Data not submitted yet - </td></tr>";
                }
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
            string tablename = "TRAI_QuartData";
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

            string pp = TextPrePost.Text.Trim();
            string oper = DropOperator.SelectedItem.Text.Trim();
            string compname = "";
            string addr = "";
            string circ = DropCircle.SelectedItem.Text.Trim();
            string qtr = DropQtr.SelectedItem.Text.Trim();
            string yr = DropYr.SelectedItem.Text.Trim();
            string delay = TextDelay.Text.Trim();
            string qtrenddt = TextQtrEndDate.Text.Trim();
            string repdt = TextRepDate.Text.Trim();
            /*
            DateTime reportdate = Convert.ToDateTime("1/1/2001");
            DateTime actiondate = Convert.ToDateTime("1/1/2001");
            int violationlimit_b = 7;   // no. of days permitted between launch date and reporting date
            double delaydays = 0;
            double penaltydays = 0;
            DateTime Valid8thDay;
            string ttype = "";
            */

            try
            {
                /*
                com = new MySqlCommand("select * from " + tablename + " where(uniqueid='" + uid + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                oper = dr["oper"].ToString().Trim();
                circ = dr["circ"].ToString().Trim();
                reportdate = Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                actiondate = Convert.ToDateTime(dr["actiondate"].ToString().Trim());
                ttype = dr["ttype"].ToString().Trim();
                */


                string violationsdet = "";
                violations = "None";
                /*
                int violationlimit = 7;   // no. of days permitted between launch date and reporting date
                DateTime Valid7thDay = Convert.ToDateTime(Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy"));
                Valid7thDay = Valid7thDay.AddDays(1);   // date of action is not to be counted, so start from the next date
                Valid8thDay = Valid7thDay;

                int validdaycount = 0;
                DateTime tempdate1 = Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                DateTime tempdate2 = Convert.ToDateTime(dr["actiondate"].ToString().Trim());



                tempdate2 = tempdate2.AddDays(1);  // launch date is to be excluded
                tempdate1 = tempdate1.AddDays(-1); // report date is to be excluded
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

                    }

                    if (validdaycount <= 7)
                    {
                        Valid8thDay = Valid8thDay.AddDays(1);
                    }

                    tempdate2 = tempdate2.AddDays(1);
                }

                if (validdaycount > 7)  // if more than 7 valid days passed in between, calculate the delay
                {


                    if (Convert.ToDateTime(dr["reportdate"].ToString().Trim()) > Valid8thDay)
                    {
                        TimeSpan ts2 = Convert.ToDateTime(Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy")) - Valid8thDay;
                        penaltydays = ts2.TotalDays;
                    }
                    totalpenalty = Convert.ToInt32(dailypenalty * penaltydays);
                    if (totalpenalty > 200000)
                    {
                        totalpenalty = 200000;
                    }
                    penaltyinwords = ConvertNumbertoWords(totalpenalty);
                }

            }
            catch (Exception ex) { }
            con.Close();
            */


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