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
using iTextSharp.text;    // uses the 'itextsharp.dll' file placed in the 'bin' folder
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data.OleDb;
using System.Text;
using System.IO;
using System.Web.Mail;




public partial class FEA_Review : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno, size;
    DateTime dt1, dt2;
    double modrno;
    RadioButtonList[] arrAction;
    RadioButtonList RadAction;
    DropDownList[] arrForward, arrReason;
    DropDownList DropForward, DropReason;
    TextBox[] arrRemarks, arrUniqueId;
    TextBox TextRemarks, TextUniqueId;
    CheckBox[] arrClear, arrPriority;
    CheckBox chkClear, chkPriority;

    string tsp, lsa, tariffproduct, servicetype, violations, reportingtype, recid, productname, tariffdet, prevremarks, trail;
    DateTime reportdate, launchdate;
    double price, validity;

    double tot, resultsonpage, z1, z2, bylevel;
    int pages, pageno, rno, flag = 0, cntr = 0;



    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        tablename = "TRAI_tariffs";

        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("FEA_logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("FEA_logout.aspx");
            }

            resultsonpage = 10;
            try
            {
                pageno = Convert.ToInt32(Request["pg"].ToString().Trim());
            }
            catch(Exception ex)
            {
                pageno = 1;
            }

            string byoperators = "";
            bylevel = 0;
            com = new MySqlCommand("select * from TRAI_FEA where(uname='" + Request["user"].ToString().Trim() + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            bylevel = Convert.ToDouble(dr["review"].ToString().Trim().Replace("Level", "").Trim());
            byoperators = dr["oper"].ToString().Trim();
            con.Close();

            if(!IsPostBack)
            {
                DropOperator.Items.Add("All Operators");
                DropOperator.Items.Add("All TSPs");
                DropOperator.Items.Add("All ISPs");
                com = new MySqlCommand("select * from TRAI_operators where('" + byoperators.ToUpper() + "' like CONCAT('%', upper(oper), '%')) order by oper", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    DropOperator.Items.Add(dr["oper"].ToString().Trim());
                }
                con.Close();

                try
                {
                    DropOperator.Text = Request["o"].ToString().Trim();
                }
                catch (Exception ex) { }

                RadType.Items.Add("Action Required");
                RadType.Items[0].Attributes.Add("style", "margin-right:30px;");
                RadType.Items.Add("Forwarded");
                RadType.Items[1].Attributes.Add("style", "margin-right:30px;");
                RadType.Items.Add("Analyzed");
                RadType.Items[2].Attributes.Add("style", "margin-right:30px;");
                
                string seltype = "Action Required";
                try
                {
                    seltype = Request["sel"].ToString().Trim();
                }
                catch (Exception ex) { }

                RadType.Text = seltype;

            }


            size = 1000;
            arrAction = new RadioButtonList[size];
            arrRemarks = new TextBox[size];
            arrUniqueId = new TextBox[size];
            arrForward = new DropDownList[size];
            arrReason = new DropDownList[size];
            arrClear = new CheckBox[size];
            arrPriority = new CheckBox[size];
            

            int i = 0;

            //string opercondition = "";
            string opercondition = " and (upper(circ)!='TEST CIRCLE' and upper(circ)!='TESTING CIRCLE' and upper(circ)!='ISP TESTCIRCLE')";

            if (DropOperator.SelectedItem.Text.Trim() != "All Operators" && DropOperator.SelectedItem.Text.Trim() != "All TSPs" && DropOperator.SelectedItem.Text.Trim() != "All ISPs")
            {
                opercondition = opercondition + " and (oper='" + DropOperator.SelectedItem.Text.Trim() + "')";
            }
            if(DropOperator.SelectedItem.Text.Trim()=="All TSPs")
            {
                opercondition = opercondition + " and (upper(oper)='AIRCEL' or upper(oper)='AIRTEL' or upper(oper)='BSNL' or upper(oper)='IDEA' or upper(oper)='JIO' or upper(oper)='MTNL' or upper(oper)='QUADRANT (CONNECT)' or upper(oper)='TATA TELE' or upper(oper)='TELENOR' or upper(oper)='VODAFONE' or upper(oper)='VODAFONE IDEA' or upper(oper)='SURFTELECOM' or upper(oper)='AEROVOYCE')";
            }
            if (DropOperator.SelectedItem.Text.Trim() == "All ISPs")
            {
                opercondition = opercondition + " and (upper(oper)!='AIRCEL' and upper(oper)!='AIRTEL' and upper(oper)!='BSNL' and upper(oper)!='IDEA' and upper(oper)!='JIO' and upper(oper)!='MTNL' and upper(oper)!='QUADRANT (CONNECT)' and upper(oper)!='TATA TELE' and upper(oper)!='TELENOR' and upper(oper)!='VODAFONE' and upper(oper)!='VODAFONE IDEA' and upper(oper)!='SURFTELECOM' and upper(oper)!='AEROVOYCE')";
            }

            for (int loopno = 1; loopno <= 2; loopno++)    // loopno=1 is for Launch / Correction / Revision, while 2 is for Withdrawal
            {
                
                if (loopno == 1)
                {
                    //com = new MySqlCommand("select * from " + tablename + " where(checked!='Yes') order by rno limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                    if (RadType.SelectedItem.Text.Trim() == "Action Required")
                    {
                        if (bylevel == 5)
                        {
                            com = new MySqlCommand("select * from TRAI_tariffs where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (TRAI_tariffs.checked!='Yes') " + opercondition + " and (TRAI_tariffs.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog)) order by priority desc, rno desc limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                            com2 = new MySqlCommand("select count(*) from TRAI_tariffs where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (TRAI_tariffs.checked!='Yes') " + opercondition + " and (TRAI_tariffs.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog))", con2);
                        }
                        else
                        {
                            com = new MySqlCommand("select * from TRAI_tariffs where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (TRAI_tariffs.checked!='Yes') " + opercondition + " and ('" + byoperators.ToUpper() + "' like CONCAT('%', upper(oper), '%')) and (currstaff='" + Request["user"].ToString().Trim() + "') and (TRAI_tariffs.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To'))) order by priority desc, rno desc limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                            com2 = new MySqlCommand("select count(*) from TRAI_tariffs where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (TRAI_tariffs.checked!='Yes') " + opercondition + " and ('" + byoperators.ToUpper() + "' like CONCAT('%', upper(oper), '%')) and (currstaff='" + Request["user"].ToString().Trim() + "') and (TRAI_tariffs.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To')))", con2);
                        }
                    }
                    if (RadType.SelectedItem.Text.Trim() == "Forwarded")
                    {
                        //com = new MySqlCommand("select * from TRAI_tariffs where(TRAI_tariffs.checked!='Yes') " + opercondition + " and (TRAI_tariffs.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(forwardedby='" + Request["user"].ToString().Trim() + "' or forwardedto='" + Request["user"].ToString().Trim() + "'))) and (TRAI_tariffs.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To'))) order by priority desc, rno limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                        //com2 = new MySqlCommand("select count(*) from TRAI_tariffs where(TRAI_tariffs.checked!='Yes') " + opercondition + " and (TRAI_tariffs.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(forwardedby='" + Request["user"].ToString().Trim() + "' or forwardedto='" + Request["user"].ToString().Trim() + "'))) and (TRAI_tariffs.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To')))", con2);  
                        com = new MySqlCommand("select * from TRAI_tariffs where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (TRAI_tariffs.checked!='Yes') " + opercondition + " and (TRAI_tariffs.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(forwardedby='" + Request["user"].ToString().Trim() + "'))) and (TRAI_tariffs.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To'))) order by priority desc, rno desc limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                        com2 = new MySqlCommand("select count(*) from TRAI_tariffs where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (TRAI_tariffs.checked!='Yes') " + opercondition + " and (TRAI_tariffs.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(forwardedby='" + Request["user"].ToString().Trim() + "'))) and (TRAI_tariffs.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To')))", con2);  
                    }
                    if (RadType.SelectedItem.Text.Trim() == "Analyzed")
                    {
                        com = new MySqlCommand("select * from TRAI_tariffs where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (TRAI_tariffs.checked!='Yes') " + opercondition + " and(currstaff='" + Request["user"].ToString().Trim() + "') and (TRAI_tariffs.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To'))) order by priority desc, rno desc limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                        com2 = new MySqlCommand("select count(*) from TRAI_tariffs where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (TRAI_tariffs.checked!='Yes') " + opercondition + " and(currstaff='" + Request["user"].ToString().Trim() + "') and (TRAI_tariffs.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To')))", con2);  
                    }
                }
                else 
                {
                    //com = new MySqlCommand("select * from TRAI_archive where(upper(actiontotake)='WITHDRAWAL') and (checked!='Yes') order by rno limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                    if (RadType.SelectedItem.Text.Trim() == "Action Required")
                    {
                        if (bylevel == 5)
                        {
                            com = new MySqlCommand("select * from TRAI_archive where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and (TRAI_archive.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog)) order by priority desc, rno desc limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                            com2 = new MySqlCommand("select count(*) from TRAI_archive where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and (TRAI_archive.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog))", con2);
                        }
                        else
                        {
                            com = new MySqlCommand("select * from TRAI_archive where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and ('" + byoperators.ToUpper() + "' like CONCAT('%', upper(oper), '%')) and (currstaff='" + Request["user"].ToString().Trim() + "') and (TRAI_archive.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To'))) order by priority desc, rno desc limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                            com2 = new MySqlCommand("select count(*) from TRAI_archive where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and ('" + byoperators.ToUpper() + "' like CONCAT('%', upper(oper), '%')) and (currstaff='" + Request["user"].ToString().Trim() + "') and (TRAI_archive.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To')))", con2);
                        }
                    }
                    if (RadType.SelectedItem.Text.Trim() == "Forwarded")
                    {
                        //com = new MySqlCommand("select * from TRAI_archive where(upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and (TRAI_archive.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(forwardedby='" + Request["user"].ToString().Trim() + "' or forwardedto='" + Request["user"].ToString().Trim() + "'))) and (TRAI_archive.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To'))) order by priority desc, rno limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                        //com2 = new MySqlCommand("select count(*) from TRAI_archive where(upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and (TRAI_archive.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(forwardedby='" + Request["user"].ToString().Trim() + "' or forwardedto='" + Request["user"].ToString().Trim() + "'))) and (TRAI_archive.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To')))", con2);  
                        com = new MySqlCommand("select * from TRAI_archive where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and (TRAI_archive.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(forwardedby='" + Request["user"].ToString().Trim() + "'))) and (TRAI_archive.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To'))) order by priority desc, rno desc limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                        com2 = new MySqlCommand("select count(*) from TRAI_archive where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and (TRAI_archive.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(forwardedby='" + Request["user"].ToString().Trim() + "'))) and (TRAI_archive.uniqueid not in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To')))", con2);  
                    }
                    if (RadType.SelectedItem.Text.Trim() == "Analyzed")
                    {
                        com = new MySqlCommand("select * from TRAI_archive where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and(currstaff='" + Request["user"].ToString().Trim() + "') and (TRAI_archive.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To'))) order by priority desc, rno desc limit " + size, con);  // to select top 1000 records (as mentioned in 'size')
                        com2 = new MySqlCommand("select count(*) from TRAI_archive where (actiondate>='" + Convert.ToDateTime("2018-06-15").ToString("yyyy-MM-dd") + "') and (upper(TRAI_archive.actiontotake)='WITHDRAWAL') " + opercondition + " and (TRAI_archive.checked!='Yes') and(currstaff='" + Request["user"].ToString().Trim() + "') and (TRAI_archive.uniqueid in (select distinct(uniqueid) from TRAI_tariffreviewlog where(actiontaken='Analyzed & Fwd To')))", con2);
                    }
                }
                 

                // get the count 
                con2.Open();
                dr2 = com2.ExecuteReader();
                dr2.Read();
                try
                {
                    tot = tot + Convert.ToInt32(dr2[0].ToString().Trim());
                }
                catch (Exception ex) { }
                con2.Close();


                // run the query
                con.Open();
                dr = com.ExecuteReader();
                //while (dr.Read())
                while (dr.Read() && cntr < resultsonpage)
                {
                    if (flag < ((pageno - 1) * resultsonpage))
                    {
                        // keep incrementing the flag untill it exceeds the count of exhausted results
                        flag++;
                    }
                    else
                    {
                        tsp = dr["oper"].ToString().Trim();
                        lsa = dr["circ"].ToString().Trim();
                        tariffproduct = dr["ttype"].ToString().Trim();
                        servicetype = dr["service"].ToString().Trim();
                        reportdate = Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                        if (dr["categ"].ToString().Trim().ToUpper() == "PREPAID")
                        {
                            price = Convert.ToDouble(dr["mrp"].ToString().Trim());
                        }
                        else
                        {
                            price = Convert.ToDouble(dr["ISP_rental"].ToString().Trim());
                        }

                        string violationsdet = "";
                        violations = "None";
                        int violationlimit = 7;   // no. of days permitted between launch date and reporting date
                        DateTime Valid7thDay = Convert.ToDateTime(Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy"));
                        Valid7thDay = Valid7thDay.AddDays(1);   // date of action is not to be counted, so start from the next date
                        DateTime Valid8thDay = Valid7thDay;

                        int validdaycount = 0;
                        DateTime tempdate1 = Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                        DateTime tempdate2 = Convert.ToDateTime(dr["actiondate"].ToString().Trim());

                        /*
                        TimeSpan span1 = tempdate1 - tempdate2;
                        int datediff = Convert.ToInt32(span1.TotalDays);
                        */
                        
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
                                /*
                                if (validdaycount == 7)
                                {
                                    Valid7thDay = Valid7thDay.AddDays(7);
                                    Valid8thDay = Valid8thDay.AddDays(8);
                                }
                                if (validdaycount == 8)
                                {
                                    Valid8thDay = Valid8thDay.AddDays(8);
                                }
                                */ 
                            }

                            if(validdaycount<=7)
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
                                double penaltydays = ts2.TotalDays;
                                violations = "<font color=#ffffff style=font-size:12px;>" + penaltydays.ToString() + " day(s)</font>";
                            }
                            
                        }

                        reportingtype = dr["actiontotake"].ToString().Trim();
                        recid = dr["uniqueid"].ToString().Trim();
                        productname = dr["planname"].ToString().Trim();
                        validity = Convert.ToDouble(dr["validity"].ToString().Trim());
                        launchdate = Convert.ToDateTime(dr["actiondate"].ToString().Trim());
                        tariffdet = dr["tariffdet"].ToString().Trim();

                        DateTime lastopdate = Convert.ToDateTime("1/1/2001");
                        trail = "<table width=100% cellspacing=1 cellpadding=1><tr><td class=tablehead>By</td><td class=tablehead>Action</td><td class=tablehead>Date / Time</td></tr>";
                        int trailcount = 0;
                        com1 = new MySqlCommand("select * from TRAI_tariffreviewlog where (uniqueid='" + dr["uniqueid"].ToString().Trim() + "') order by recdate", con1);
                        con1.Open();
                        dr1 = com1.ExecuteReader();
                        while(dr1.Read())
                        {
                            trail = trail + "<tr>";
                            string act = dr1["actiontaken"].ToString().Trim().Replace("Forward To", "Fwd To");
                            if (dr1["actiontaken"].ToString().Trim()!="Taken on Record")
                            {
                                act = act + " - " + dr1["forwardedto"].ToString().Trim();
                            }
                            trail = trail + "<td class=tablecell4 align=left valign=top>" + dr1["forwardedby"].ToString().Trim() + "</td>";
                            trail = trail + "<td class=tablecell4 align=left valign=top>" + act + "</td>";
                            trail = trail + "<td class=tablecell4 align=left valign=top>" + Convert.ToDateTime(dr1["recdate"].ToString().Trim()).ToString("dd-MMM-yyyy HH:mm:ss") + "</td>";
                            trail = trail + "</tr>";
                            trailcount++;
                            lastopdate = Convert.ToDateTime(dr1["recdate"].ToString().Trim());
                        }
                        con1.Close();

                        /*
                        com1 = new MySqlCommand("select count(*) from TRAI_archive where(uniqueid='" + dr["uniqueid"].ToString().Trim() + "') and (upper(actiontotake)='WITHDRAWAL')", con1);
                        con1.Open();
                        dr1 = com1.ExecuteReader();
                        dr1.Read();
                        if(Convert.ToInt32(dr1[0].ToString().Trim())>0)
                        {
                            DateTime wdate = Convert.ToDateTime("1/1/2011");
                            com2 = new MySqlCommand("select * from TRAI_archive where(uniqueid='" + dr["uniqueid"].ToString().Trim() + "') and (upper(actiontotake)='WITHDRAWAL') order by rno desc", con2);
                            con2.Open();
                            dr2 = com2.ExecuteReader();
                            dr2.Read();
                            try
                            {
                                wdate = Convert.ToDateTime(dr2["actiondate"].ToString().Trim());
                            }
                            catch (Exception ex) { }
                            con2.Close();

                            trail = trail + "<tr>";
                            trail = trail + "<td class=tablecell4 align=left valign=top>" + tsp + "</td>";
                            trail = trail + "<td class=tablecell4 align=left valign=top><font color=red>WITHDRAWN</font></td>";
                            trail = trail + "<td class=tablecell4 align=left valign=top>" + wdate.ToString("dd-MMM-yyyy") + "</td>";
                            trail = trail + "</tr>";
                        }
                        con1.Close();
                        */

                        trail = trail + "</table><br />";

                        TimeSpan tts = Convert.ToDateTime(DateTime.Now.ToShortDateString()) - lastopdate;
                        int daygap = Convert.ToInt32(tts.TotalDays);
                        if(daygap>=7)
                        {
                            trail = trail + "<font style=background-color:#ffff00; >Last action performed " + daygap + " days ago</font><br />";
                        }

                        if(trailcount==0)
                        {
                            trail = "";
                        }


                        prevremarks = "<center><i>Previous Remarks</i></center>";
                        int remcount = 0;
                        com1 = new MySqlCommand("select * from TRAI_tariffreviewlog where(uniqueid='" + dr["uniqueid"].ToString().Trim() + "') and (remarks!='') order by rno", con1);
                        con1.Open();
                        dr1 = com1.ExecuteReader();
                        while (dr1.Read())
                        {
                            string reason = "";
                            if (dr1["reason"].ToString().Trim() != "")
                            {
                                reason = "<p align=left><font style=background-color:#b8008b;color:#ffffff;>(Direct Forwarding Reason : " + dr1["reason"].ToString().Trim() + ")</font></p>";
                            }
                            prevremarks = prevremarks + "<b>" + Convert.ToDateTime(dr1["recdate"].ToString().Trim()).ToString("dd-MMM-yyyy") + "</b><br />";
                            if (dr1["actiontaken"].ToString().Trim() == "Revert to TSP")
                            {
                                prevremarks = prevremarks + "Remarks to TSP : ";
                            }
                            prevremarks = prevremarks + dr1["remarks"].ToString().Trim() + reason;
                            prevremarks = prevremarks + "<p align=right>By : " + dr1["forwardedby"].ToString().Trim() + "</p>";
                            remcount++;
                        }
                        con1.Close();

                        if (remcount == 0)
                        {
                            prevremarks = prevremarks + "<center><br />---</center>";
                        }


                        string wstat = "No";
                        com1 = new MySqlCommand("select count(*) from TRAI_archive where(uniqueid='" + dr["uniqueid"].ToString().Trim() + "') and (upper(actiontotake)='WITHDRAWAL')", con1);
                        con1.Open();
                        dr1 = com1.ExecuteReader();
                        dr1.Read();
                        if (Convert.ToInt32(dr1[0].ToString().Trim()) > 0)
                        {
                            wstat = "Yes";
                        }
                        con1.Close();

                        
                        Table tb = new Table();
                        tb.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        tb.CellPadding = 5;
                        tb.CellSpacing = 1;

                        TableRow tr1 = new TableRow();
                        TableCell tcc1 = new TableCell();
                        TableCell tcc2 = new TableCell();
                        TableCell tcc3 = new TableCell();
                        TableCell tcc4 = new TableCell();
                        TableCell tcc5 = new TableCell();
                        TableCell tcc6 = new TableCell();
                        TableCell tcc7 = new TableCell();
                        TableCell tcc8 = new TableCell();
                        TableCell tcc9 = new TableCell();
                        TableCell tcc10 = new TableCell();
                        tcc1.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                        tcc2.Width = System.Web.UI.WebControls.Unit.Percentage(9);
                        tcc3.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                        tcc4.Width = System.Web.UI.WebControls.Unit.Percentage(5);
                        tcc5.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                        tcc6.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                        tcc7.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                        tcc8.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                        tcc9.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                        tcc10.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                        tcc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc8.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc9.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc10.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcc1.CssClass = "tablehead";
                        tcc2.CssClass = "tablehead";
                        tcc3.CssClass = "tablehead";
                        tcc4.CssClass = "tablehead";
                        tcc5.CssClass = "tablehead";
                        tcc6.CssClass = "tablehead";
                        tcc7.CssClass = "tablehead";
                        tcc8.CssClass = "tablehead";
                        tcc9.CssClass = "tablehead";
                        tcc10.CssClass = "tablehead";
                        tcc1.Text = "<a name='Book" + i.ToString().Trim() + "'></a>TSP";
                        tcc2.Text = "LSA";
                        tcc3.Text = "Tariff Type";
                        tcc4.Text = "Service Type";
                        tcc5.Text = "Date of Reporting";
                        tcc6.Text = "Price (&#8377;)";
                        tcc7.Text = "High Priority";
                        tcc8.Text = "Action<br />(Click to select, Dbl Click to Clear)";
                        tcc9.Text = "Forward To";
                        tcc10.Text = "If forwarding to a non-immediate senior level, please specify reason";
                        tr1.Controls.Add(tcc1);
                        tr1.Controls.Add(tcc2);
                        tr1.Controls.Add(tcc3);
                        tr1.Controls.Add(tcc4);
                        tr1.Controls.Add(tcc5);
                        tr1.Controls.Add(tcc6);
                        tr1.Controls.Add(tcc7);
                        tr1.Controls.Add(tcc8);
                        tr1.Controls.Add(tcc9);
                        tr1.Controls.Add(tcc10);
                        tb.Controls.Add(tr1);

                        TableRow tr2 = new TableRow();
                        TableCell tcd1 = new TableCell();
                        TableCell tcd2 = new TableCell();
                        TableCell tcd3 = new TableCell();
                        TableCell tcd4 = new TableCell();
                        TableCell tcd5 = new TableCell();
                        TableCell tcd6 = new TableCell();
                        TableCell tcd7 = new TableCell();
                        TableCell tcd8 = new TableCell();
                        TableCell tcd9 = new TableCell();
                        TableCell tcd10 = new TableCell();
                        tcd1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd8.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd9.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd10.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcd1.CssClass = "tablecell3b";
                        tcd2.CssClass = "tablecell3b";
                        tcd3.CssClass = "tablecell3b";
                        tcd4.CssClass = "tablecell3b";
                        tcd5.CssClass = "tablecell3b";
                        tcd6.CssClass = "tablecell3b";
                        tcd7.CssClass = "tablecell3b";
                        tcd8.CssClass = "tablecell3b";
                        tcd9.CssClass = "tablecell3b";
                        tcd10.CssClass = "tablecell3b";
                        tcd1.Text = tsp;
                        tcd2.Text = lsa;
                        tcd3.Text = tariffproduct;
                        tcd4.Text = servicetype;
                        tcd5.Text = reportdate.ToString("dd-MMM-yyyy");
                        tcd6.Text = price.ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "");
                        chkPriority = new CheckBox();
                        chkPriority.CssClass = "chks";
                        chkPriority.ID = "Priority" + i.ToString();
                        if(dr["priority"].ToString().Trim()=="Yes")
                        {
                            chkPriority.Checked = true;
                            chkPriority.Attributes.Add("style", "outline: 2px solid #c00;");
                            //tcd7.Attributes.Add("style", "background-color:#ff0000;");
                        }
                        if(bylevel<5)
                        {
                            chkPriority.Enabled = false;
                        }
                        arrPriority[i] = chkPriority;
                        tcd7.Controls.Add(arrPriority[i]);
                        RadAction = new RadioButtonList();
                        RadAction.ID = "A" + i.ToString().Trim();
                        if (RadType.SelectedItem.Text.Trim() != "Analyzed")
                        {
                            RadAction.Items.Add("Forward To");
                        }
                        if (bylevel == 5)
                        {
                            RadAction.Items.Add("Taken on Record");
                        }
                        else
                        {
                            RadAction.Items.Add("Analyzed & Fwd To");
                        }
                        if (RadType.SelectedItem.Text.Trim() == "Analyzed")
                        {
                            RadAction.Items.Add("Fwd to Re-Analyze");
                        }
                        //RadAction.Items.Add("Revert to TSP");   // Not required anymore
                        RadAction.Attributes.Add("ondblclick", "funClearAction('" + i.ToString() + "');");   // Clear 'Action' on double click
                        RadAction.AutoPostBack = true;
                        RadAction.SelectedIndexChanged += new EventHandler(DropForward_Change);

                        if (RadType.SelectedItem.Text.Trim() == "Forwarded")
                        {
                            RadAction.Visible = false;
                        }
                        else
                        {
                            RadAction.Visible = true;
                        }

                        string analyzed = "No";
                        string analyzedby = "";
                        com1 = new MySqlCommand("select count(*) from TRAI_tariffreviewlog where(uniqueid='" + recid + "') and (actiontaken='Analyzed & Fwd To')", con1);
                        con1.Open();
                        dr1 = com1.ExecuteReader();
                        dr1.Read();
                        try
                        {
                            if (Convert.ToInt32(dr1[0].ToString().Trim()) > 0)   // If already 'Analyzed', disable the action radiobuttonlist
                            {
                                /*
                                if (bylevel != 5)
                                {
                                    RadAction.Enabled = false;
                                }
                                */ 
                                analyzed = "Yes";
                                com2 = new MySqlCommand("select * from TRAI_tariffreviewlog where(uniqueid='" + recid + "') and (actiontaken='Analyzed & Fwd To') order by rno desc limit 1", con2);
                                con2.Open();
                                dr2 = com2.ExecuteReader();
                                dr2.Read();
                                try
                                {
                                    analyzedby = dr2["forwardedby"].ToString().Trim();
                                }
                                catch (Exception ex) { }
                                con2.Close();
                            }
                        }
                        catch (Exception ex) { }
                        con1.Close();
                        arrAction[i] = RadAction;
                        TextUniqueId = new TextBox();
                        TextUniqueId.ID = "U" + i.ToString();
                        TextUniqueId.Text = recid;
                        TextUniqueId.Visible = false;
                        arrUniqueId[i] = TextUniqueId;
                        tcd8.Controls.Add(arrAction[i]);
                        tcd8.Controls.Add(arrUniqueId[i]);
                        DropForward = new DropDownList();
                        DropForward.ID = "D" + i.ToString();
                        
                        arrForward[i] = DropForward;
                        tcd9.Controls.Add(arrForward[i]);
                        DropReason = new DropDownList();
                        DropReason.ID = "N" + i.ToString();
                        DropReason.Items.Add("");
                        DropReason.Items.Add("Senior not available");
                        DropReason.Items.Add("Senior on leave");
                        DropReason.AutoPostBack = true;
                        DropReason.SelectedIndexChanged += new EventHandler(DropReason_Change);
                        arrReason[i] = DropReason;
                        tcd10.Controls.Add(arrReason[i]);
                        tr2.Controls.Add(tcd1);
                        tr2.Controls.Add(tcd2);
                        tr2.Controls.Add(tcd3);
                        tr2.Controls.Add(tcd4);
                        tr2.Controls.Add(tcd5);
                        tr2.Controls.Add(tcd6);
                        tr2.Controls.Add(tcd7);
                        tr2.Controls.Add(tcd8);
                        tr2.Controls.Add(tcd9);
                        tr2.Controls.Add(tcd10);
                        tb.Controls.Add(tr2);

                        TableRow tr3 = new TableRow();
                        TableCell tce1 = new TableCell();
                        TableCell tce2 = new TableCell();
                        TableCell tce3 = new TableCell();
                        TableCell tce4 = new TableCell();
                        TableCell tce5 = new TableCell();
                        TableCell tce6 = new TableCell();
                        TableCell tce7 = new TableCell();
                        TableCell tce8 = new TableCell();
                        TableCell tce9 = new TableCell();
                        tce1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce8.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce8.ColumnSpan = 3;
                        //tce9.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tce1.CssClass = "tablehead";
                        tce2.CssClass = "tablehead";
                        tce3.CssClass = "tablehead";
                        tce4.CssClass = "tablehead";
                        tce5.CssClass = "tablehead";
                        tce6.CssClass = "tablehead";
                        tce7.CssClass = "tablehead";
                        tce8.CssClass = "tablehead";
                        //tce9.CssClass = "tablehead";
                        tce1.Text = "Reporting Type";
                        tce2.Text = "Unique Record ID";
                        tce3.Text = "Product Name";
                        tce4.Text = "Validity (Days)";
                        tce5.Text = "Date of Launch / Withdrawal";
                        tce6.Text = "Delay Reporting";
                        tce7.Text = "Violation Files";
                        tce8.Text = "Remarks";
                        //tce9.Text = "";
                        tr3.Controls.Add(tce1);
                        tr3.Controls.Add(tce2);
                        tr3.Controls.Add(tce3);
                        tr3.Controls.Add(tce4);
                        tr3.Controls.Add(tce5);
                        tr3.Controls.Add(tce6);
                        tr3.Controls.Add(tce7);
                        tr3.Controls.Add(tce8);
                        //tr3.Controls.Add(tce9);
                        tb.Controls.Add(tr3);

                        TableRow tr4 = new TableRow();
                        TableCell tcf1 = new TableCell();
                        TableCell tcf2 = new TableCell();
                        TableCell tcf3 = new TableCell();
                        TableCell tcf4 = new TableCell();
                        TableCell tcf5 = new TableCell();
                        TableCell tcf6 = new TableCell();
                        TableCell tcf7 = new TableCell();
                        TableCell tcf8 = new TableCell();
                        tcf8.ColumnSpan = 3;
                        //TableCell tcf9 = new TableCell();
                        tcf1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcf2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcf3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcf4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcf5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcf6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcf7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcf8.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        //tcf9.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcf1.CssClass = "tablecell3b";
                        tcf2.CssClass = "tablecell3b";
                        tcf3.CssClass = "tablecell3b";
                        tcf4.CssClass = "tablecell3b";
                        tcf5.CssClass = "tablecell3b";
                        tcf6.CssClass = "tablecell3b";
                        tcf7.CssClass = "tablecell3b";
                        tcf8.CssClass = "tablecell3b";
                        //tcf9.CssClass = "tablecell3b";
                        tcf1.Text = reportingtype;
                        tcf2.Text = recid;
                        tcf3.Text = productname;
                        tcf4.Text = validity.ToString().Replace("-1", "").Replace("-2", "Unlimited");
                        tcf5.Text = launchdate.ToString("dd-MMM-yyyy");
                        //tcf6.Text = "<a href=javascript:funSummary('" + dr["uniqueid"].ToString().Trim() + "');><b><u>View Summary</u></b></a><br /><br /><a href='FEA_recorddetails.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Details</u></b></a>";
                        //tcf6.Text = "<a href='FEA_recorddetails.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Details</u></b></a>";
                        if (violations == "None")
                        {
                            tcf6.Attributes.Add("style", "background-color:#00ff00;");
                        }
                        else
                        {
                            tcf6.Attributes.Add("style", "background-color:#ff0000;");
                        }
                        tcf6.Text = violations;
                        string mylinks = "";
                        if (violations != "None")
                        {
                            mylinks = mylinks + "<a href=javascript:funWord2('" + dr["uniqueid"].ToString().Trim() + "');><img src=images/word.jpg alt='Show Cause Notice' title='Show Cause Notice' style=margin-bottom:5px; border=0></a>";
                            mylinks = mylinks + "<br /><a href=javascript:funWord('" + dr["uniqueid"].ToString().Trim() + "');><img src=images/word.jpg alt='FD Order' title='FD Order' style=margin-bottom:5px; border=0></a>";
                            //mylinks = mylinks + "<br /><a href=javascript:funPDF('" + dr["uniqueid"].ToString().Trim() + "');><img src=images/pdf.jpg alt='Download PDF' title='Download PDF' style=margin-bottom:5px; border=0></a>";
                            //mylinks = mylinks + "<br /><a href=javascript:funExcel('" + dr["uniqueid"].ToString().Trim() + "');><img src=images/excel.jpg style=margin-bottom:5px; border=0></a>";
                        }
                        tcf7.Text = mylinks;
                        TextRemarks = new TextBox();
                        TextRemarks.ID = "R" + i.ToString();
                        TextRemarks.TextMode = TextBoxMode.MultiLine;
                        TextRemarks.Width = 490;
                        TextRemarks.Height = 45;
                        arrRemarks[i] = TextRemarks;
                        tcf8.Controls.Add(arrRemarks[i]);
                        //tcf9.Controls.Add(arrRemarks[i]);
                        tr4.Controls.Add(tcf1);
                        tr4.Controls.Add(tcf2);
                        tr4.Controls.Add(tcf3);
                        tr4.Controls.Add(tcf4);
                        tr4.Controls.Add(tcf5);
                        tr4.Controls.Add(tcf6);
                        tr4.Controls.Add(tcf7);
                        tr4.Controls.Add(tcf8);
                        //tr4.Controls.Add(tcf9);
                        tb.Controls.Add(tr4);

                        TableRow tr5 = new TableRow();
                        TableCell tcg1 = new TableCell();
                        tcg1.ColumnSpan = 5;
                        TableCell tcg1b = new TableCell();
                        TableCell tcg2 = new TableCell();
                        TableCell tcg3 = new TableCell();
                        tcg3.ColumnSpan = 3;
                        tcg1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                        tcg1b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcg2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tcg3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                        tcg1.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
                        tcg1b.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Middle;
                        tcg2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
                        tcg3.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
                        tcg1.CssClass = "tablecell3b";
                        tcg1b.CssClass = "tablecell3b";
                        tcg2.CssClass = "tablecell3b";
                        tcg3.CssClass = "tablecell3b";
                        string summ = "<div id='divSummary" + dr["uniqueid"].ToString().Trim() + "' style=display:block; ><b>Tariff Summary : </b> " + tariffdet.Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;") + "</div>";
                        string viol = "<div id='divViol" + dr["uniqueid"].ToString().Trim() + "' style=display:none;background-color:#ffcccc;><b>Violations : </b> " + violationsdet.Replace("&amp;", "&").Replace("&amp;", "&") + "</div>";
                        tcg1.Text = summ + viol;
                        string markedfor = "<font style=font-size:12px;color:#ff0000;>Not Yet Forwarded</font>";
                        if (dr["currstaff"].ToString().Trim() != "")
                        {
                            markedfor = "<font style=font-size:12px;color:#0000ff;>Action Required By : " + dr["currstaff"].ToString().Trim() + "</font>";
                        }
                        if (analyzed == "Yes")
                        {
                            markedfor = "<font style=font-size:12px;color:#ffffff;background-color:#fb0053;>Analyzed by " + analyzedby + "</font>";
                        }
                        tcg1b.Text = "<a href='FEA_recorddetails.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Details</u></b></a>";
                        tcg2.Text = markedfor;
                        tcg3.Text = trail + prevremarks;
                        tr5.Controls.Add(tcg1);
                        tr5.Controls.Add(tcg1b);
                        tr5.Controls.Add(tcg2);
                        tr5.Controls.Add(tcg3);
                        tb.Controls.Add(tr5);

                        TableRow tr9 = new TableRow();
                        tr9.Height = 70;
                        TableCell tcz1 = new TableCell();
                        tcz1.ColumnSpan = 10;
                        tcz1.Text = "<hr size=0>";
                        tr9.Controls.Add(tcz1);
                        tb.Controls.Add(tr9);

                        divTariffs.Controls.Add(tb);

                        i++;

                        cntr++;
                        
                    }
                }
                con.Close();

            }



            pagenums.InnerHtml = "";
            z2 = Convert.ToDouble(tot / resultsonpage);
            z1 = Convert.ToInt32(tot / resultsonpage);
            if (z2 - z1 == 0)
            {
                pages = Convert.ToInt32(tot / resultsonpage);
            }
            else
            {
                if (z2 > z1)
                {
                    pages = Convert.ToInt32((tot / resultsonpage) + 1);
                }
                else
                {
                    pages = Convert.ToInt32(tot / resultsonpage);
                }
            }


            // First Page Link //
            pagenums.InnerHtml = pagenums.InnerHtml + "<a style=background-color:#888;padding:5px; title='First Page' href='FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=1&o=" + DropOperator.SelectedItem.Text.Trim() + "' class=indexlinks1><font style=font-size:18px;font-family:arial color=white>&laquo;</font></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            // Previous Page Link //
            if(pageno>=2)
            {
                pagenums.InnerHtml = pagenums.InnerHtml + "<a style=background-color:#aaa;padding:5px; title='Previous Page' href='FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=" + (pageno - 1).ToString().Trim() + "&o=" + DropOperator.SelectedItem.Text.Trim() + "' class=indexlinks1><font style=font-size:18px;font-family:arial color=white>&#8249;</font></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }
            else
            {
                pagenums.InnerHtml = pagenums.InnerHtml + "<a style=background-color:#aaa;padding:5px; title='Previous Page' href='#' class=indexlinks1><font style=font-size:18px;font-family:arial color=white>&#8249;</font></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }

            for (int k = 1; k <= pages; k++)
            {
                if (k == pageno)
                {

                    pagenums.InnerHtml = pagenums.InnerHtml + "<a href='FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=" + k + "&o=" + DropOperator.SelectedItem.Text.Trim() + "' class=indexlinks1><font style=font-size:14px; color=red><b>" + k + "</b></font></a>&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                else
                {
                    pagenums.InnerHtml = pagenums.InnerHtml + "<a href='FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=" + k + "&o=" + DropOperator.SelectedItem.Text.Trim() + "' class=indexlinks1><font style=font-size:12px; color=grey><b>" + k + "</b></font></a>&nbsp;&nbsp;&nbsp;&nbsp;";
                }
            }

            // Next Page Link //
            if (pageno < pages)
            {
                pagenums.InnerHtml = pagenums.InnerHtml + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style=background-color:#aaa;padding:5px; title='Next Page' href='FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=" + (pageno + 1).ToString().Trim() + "&o=" + DropOperator.SelectedItem.Text.Trim() + "' class=indexlinks1><font style=font-size:18px;font-family:arial color=white>&#8250;</font></a>";
            }
            else
            {
                pagenums.InnerHtml = pagenums.InnerHtml + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style=background-color:#aaa;padding:5px; title='Next Page' href='#' class=indexlinks1><font style=font-size:18px;font-family:arial color=white>&#8250;</font></a>";
            }

            // Last Page Link //
            pagenums.InnerHtml = pagenums.InnerHtml + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style=background-color:#888;padding:5px; title='Last Page' href='FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=" + pages.ToString().Trim() + "&o=" + DropOperator.SelectedItem.Text.Trim() + "' class=indexlinks1><font style=font-size:18px;font-family:arial color=white>&raquo;</font></a>";



            TextCntr.Text = i.ToString();
            LblTotal.Text = tot.ToString() + " Tariffs";

            if(i==0)
            {
                Button1.Visible = false;
                divTariffs.InnerHtml = "<br /><br /><b><font style=color:#ff0000>No Pending Tariffs</font></b><br /><br />";
            }


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }





    protected void DropForward_Change(object sender, EventArgs e)
    {
        try
        {
            string tmptsp = "";
            string tmpuid = "";
            RadioButtonList ddl = sender as RadioButtonList;
            string dd_id = ddl.ID;
            string idno = dd_id.Replace("A", "");
            try
            {
                tmpuid = arrUniqueId[Convert.ToInt32(idno)].Text.Trim();
            }
            catch (Exception ex) { }
            com1 = new MySqlCommand("select * from TRAI_tariffs where(uniqueid='" + tmpuid + "')", con1);
            con1.Open();
            dr1 = com1.ExecuteReader();
            dr1.Read();
            try
            {
                tmptsp = dr1["oper"].ToString().Trim();
            }
            catch (Exception ex) { }
            con1.Close();
            if(tmptsp=="")
            {
                com1 = new MySqlCommand("select * from TRAI_archive where(uniqueid='" + tmpuid + "')", con1);
                con1.Open();
                dr1 = com1.ExecuteReader();
                dr1.Read();
                try
                {
                    tmptsp = dr1["oper"].ToString().Trim();
                }
                catch (Exception ex) { }
                con1.Close();
            }

            DropDownList ddx = (DropDownList)Page.FindControl("D" + idno);
            ddx.Items.Clear();
            ddx.Items.Add("");
            if (ddl.SelectedItem.Text.Trim() == "Forward To" || ddl.SelectedItem.Text.Trim() == "Fwd to Re-Analyze")
            {
                //if (bylevel == 1)
                if (bylevel == 0)
                {
                    com1 = new MySqlCommand("select * from TRAI_FEA where(uname!='" + Request["user"].ToString().Trim() + "') and (oper like '%," + tmptsp + ",%') and (review='Level " + (bylevel + 1).ToString().Trim() + "') order by review desc, uname", con1);   // since this is the lowest level, show one level up users
                }
                else
                {
                    com1 = new MySqlCommand("select * from TRAI_FEA where(uname!='" + Request["user"].ToString().Trim() + "') and (oper like '%," + tmptsp + ",%') and (review='Level " + (bylevel - 1).ToString().Trim() + "') order by review desc, uname", con1);     // show only lower 1 level users.
                }
                con1.Open();
                dr1 = com1.ExecuteReader();
                while (dr1.Read())
                {
                    ddx.Items.Add(dr1["uname"].ToString().Trim());
                }
                con1.Close();
            }
            if (ddl.SelectedItem.Text.Trim() == "Analyzed & Fwd To")
            {
                com1 = new MySqlCommand("select * from TRAI_FEA where(uname!='" + Request["user"].ToString().Trim() + "') and (oper like '%," + tmptsp + ",%') and (review='Level " + (bylevel + 1).ToString().Trim() + "') order by review desc, uname", con1);   // show only one level up users
                con1.Open();
                dr1 = com1.ExecuteReader();
                while (dr1.Read())
                {
                    ddx.Items.Add(dr1["uname"].ToString().Trim());
                }
                con1.Close();
            }

            //this.ClientScript.RegisterStartupScript(this.GetType(),"navigate","window.onload = function() {window.location.hash='#Book" + idno.ToString().Trim() + "';}",true);

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }
    





    protected void DropReason_Change(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = sender as DropDownList;
            string dd_id = ddl.ID;
            string idno = dd_id.Replace("N", "");
            TextBox tt1 = (TextBox)Page.FindControl("U" + idno);
            string mytsp = "";
            string uid = tt1.Text.Trim();
            com1 = new MySqlCommand("select * from " + tablename + " where(uniqueid='" + uid + "')", con1);
            con1.Open();
            dr1 = com1.ExecuteReader();
            dr1.Read();
            try
            {
                mytsp = dr1["oper"].ToString().Trim();
            }
            catch (Exception ex) { }
            con1.Close();
            if(mytsp=="")
            {
                com1 = new MySqlCommand("select * from TRAI_archive where(uniqueid='" + uid + "')", con1);
                con1.Open();
                dr1 = com1.ExecuteReader();
                dr1.Read();
                try
                {
                    mytsp = dr1["oper"].ToString().Trim();
                }
                catch (Exception ex) { }
                con1.Close();
            }
            DropDownList d1 = (DropDownList)Page.FindControl("D" + idno);
            d1.Items.Clear();
            d1.Items.Add("");
            com1 = new MySqlCommand("select * from TRAI_FEA where(uname!='" + Request["user"].ToString().Trim() + "') and (oper like '%," + mytsp + ",%') order by review desc, uname", con1);
            con1.Open();
            dr1 = com1.ExecuteReader();
            while (dr1.Read())
            {
                d1.Items.Add(dr1["uname"].ToString().Trim());
            }
            con1.Close();


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
            string forwardedby=Request["user"].ToString().Trim();
            double bylevel=0;
            com = new MySqlCommand("select * from TRAI_FEA where(uname='" + Request["user"].ToString().Trim() + "')",con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            bylevel = Convert.ToDouble(dr["review"].ToString().Trim().Replace("Level", "").Trim());
            con.Close();

            int cnt = Convert.ToInt32(TextCntr.Text.Trim());

            for(int i=0;i<cnt;i++)
            {
                string priority = "No";
                if(arrPriority[i].Checked==true)
                {
                    priority="Yes";
                }
                if(arrAction[i].SelectedIndex > -1)
                {
                    string action = arrAction[i].SelectedItem.Text.Trim();
                    string fwdto = "";
                    
                    double tolevel=0;
                    
                    string remarks = arrRemarks[i].Text.Trim().Replace("'", "`");
                    string reason = arrReason[i].Text.Trim().Replace("'", "`");
                    getMaxRno("rno", "TRAI_tariffreviewlog");

                    if (action == "Taken on Record")
                    {
                        com = new MySqlCommand("insert into TRAI_tariffreviewlog values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + arrUniqueId[i].Text.Trim() + "','" + forwardedby + "','" + bylevel + "','" + fwdto + "','" + tolevel + "','" + action + "','" + remarks + "','" + reason + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                        com = new MySqlCommand("update " + tablename + " set checked='Yes',checkedby='" + Request["user"].ToString().Trim() + "',checkedon='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where(uniqueid='" + arrUniqueId[i].Text.Trim() + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                        // also update this record with 'Withdrawal' status in archive table so that if its a withdrawal request, it should be updated
                        com = new MySqlCommand("update TRAI_archive set checked='Yes',checkedby='" + Request["user"].ToString().Trim() + "',checkedon='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where(uniqueid='" + arrUniqueId[i].Text.Trim() + "') and (upper(actiontotake)='WITHDRAWAL')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }

                    if (action == "Analyzed & Fwd To")
                    {
                        fwdto = arrForward[i].SelectedItem.Text.Trim();
                        if (fwdto == "")
                        {
                            fwdto = forwardedby;
                        }
                        if (fwdto != "")
                        {
                            com = new MySqlCommand("select * from TRAI_FEA where(uname='" + fwdto + "')", con);
                            con.Open();
                            dr = com.ExecuteReader();
                            dr.Read();
                            tolevel = Convert.ToDouble(dr["review"].ToString().Trim().Replace("Level", "").Trim());
                            con.Close();
                        }
                        com = new MySqlCommand("insert into TRAI_tariffreviewlog values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + arrUniqueId[i].Text.Trim() + "','" + forwardedby + "','" + bylevel + "','" + fwdto + "','" + tolevel + "','" + action + "','" + remarks + "','" + reason + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        com = new MySqlCommand("update " + tablename + " set currstaff='" + fwdto + "', currstafflevel=" + tolevel + ",priority='" + priority + "' where(uniqueid='" + arrUniqueId[i].Text.Trim() + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        // also update this record with 'Withdrawal' status in archive table so that if its a withdrawal request, it should be updated
                        com = new MySqlCommand("update TRAI_archive set currstaff='" + fwdto + "', currstafflevel=" + tolevel + " where(uniqueid='" + arrUniqueId[i].Text.Trim() + "') and (upper(actiontotake)='WITHDRAWAL')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }

                    if (action == "Forward To" && arrForward[i].SelectedItem.Text.Trim() != "")
                    {
                        fwdto = arrForward[i].SelectedItem.Text.Trim();
                        if (fwdto != "")
                        {
                            com = new MySqlCommand("select * from TRAI_FEA where(uname='" + fwdto + "')", con);
                            con.Open();
                            dr = com.ExecuteReader();
                            dr.Read();
                            tolevel = Convert.ToDouble(dr["review"].ToString().Trim().Replace("Level", "").Trim());
                            con.Close();
                        }

                        /*
                        com = new MySqlCommand("update TRAI_tariffreviewlog set actiontaken='Analyzed (Revoked)' where (uniqueid='" + arrUniqueId[i].Text.Trim() + "') and (actiontaken='Analyzed & Fwd To')", con);  // if being forwarded after being analyzed, delete the existing analyzed entry.
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        */

                        com = new MySqlCommand("insert into TRAI_tariffreviewlog values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + arrUniqueId[i].Text.Trim() + "','" + forwardedby + "','" + bylevel + "','" + fwdto + "','" + tolevel + "','" + action + "','" + remarks + "','" + reason + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        com = new MySqlCommand("update " + tablename + " set currstaff='" + fwdto + "', currstafflevel=" + tolevel + ",priority='" + priority + "' where(uniqueid='" + arrUniqueId[i].Text.Trim() + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        // also update this record with 'Withdrawal' status in archive table so that if its a withdrawal request, it should be updated
                        com = new MySqlCommand("update TRAI_archive set currstaff='" + fwdto + "', currstafflevel=" + tolevel + " where(uniqueid='" + arrUniqueId[i].Text.Trim() + "') and (upper(actiontotake)='WITHDRAWAL')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }

                    if (action == "Fwd to Re-Analyze")
                    {
                        com = new MySqlCommand("update TRAI_tariffreviewlog set actiontaken='Analyzed (Revoked)' where (uniqueid='" + arrUniqueId[i].Text.Trim() + "') and (actiontaken='Analyzed & Fwd To')", con);  // if being forwarded after being analyzed, delete the existing analyzed entry.
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        fwdto = arrForward[i].SelectedItem.Text.Trim();
                        if (fwdto == "")
                        {
                            fwdto = forwardedby;
                        }
                        if (fwdto != "")
                        {
                            com = new MySqlCommand("select * from TRAI_FEA where(uname='" + fwdto + "')", con);
                            con.Open();
                            dr = com.ExecuteReader();
                            dr.Read();
                            tolevel = Convert.ToDouble(dr["review"].ToString().Trim().Replace("Level", "").Trim());
                            con.Close();
                        }
                        com = new MySqlCommand("insert into TRAI_tariffreviewlog values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + arrUniqueId[i].Text.Trim() + "','" + forwardedby + "','" + bylevel + "','" + fwdto + "','" + tolevel + "','" + action + "','" + remarks + "','" + reason + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        com = new MySqlCommand("update " + tablename + " set currstaff='" + fwdto + "', currstafflevel=" + tolevel + ",priority='" + priority + "' where(uniqueid='" + arrUniqueId[i].Text.Trim() + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                        // also update this record with 'Withdrawal' status in archive table so that if its a withdrawal request, it should be updated
                        com = new MySqlCommand("update TRAI_archive set currstaff='" + fwdto + "', currstafflevel=" + tolevel + " where(uniqueid='" + arrUniqueId[i].Text.Trim() + "') and (upper(actiontotake)='WITHDRAWAL')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }

                    if(action=="Revert to TSP")
                    {
                        com = new MySqlCommand("insert into TRAI_tariffreviewlog values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + arrUniqueId[i].Text.Trim() + "','" + forwardedby + "','" + bylevel + "','" + fwdto + "','" + tolevel + "','" + action + "','" + remarks + "','" + reason + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                        string email = "";
                        com = new MySqlCommand("select * from " + tablename + " where(uniqueid='" + arrUniqueId[i].Text.Trim() + "')", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        try
                        {
                            string uname = dr["uname"].ToString().Trim();
                            com1 = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + uname + "')", con1);
                            con1.Open();
                            dr1 = com1.ExecuteReader();
                            dr1.Read();
                            try
                            {
                                email = dr1["email"].ToString().Trim();
                            }
                            catch (Exception ex) { }
                            con1.Close();
                        }
                        catch (Exception ex) { }
                        con.Close();

                        if (email != "")
                        {
                            try
                            {
                                string tariffstr = "Dear Sir / Madam,<br /><br /><b>Regarding Tariff Unique ID " + arrUniqueId[i].Text.Trim() + "</b><br /><br />" + remarks + "<br /><br /><br />warm regards,<br /><br />TRAI";
                                MailMessage objEmail = new MailMessage();
                                SmtpMail.SmtpServer = "127.0.0.1";
                                //SmtpMail.SmtpServer = "";
                                objEmail.To = email;
                                objEmail.From = "tariff.filing@trai.gov.in";
                                //objEmail.Headers.Add("reply-to", "tariff.filing@trai.gov.in");
                                objEmail.Subject = "Tariff Update : Tariff ID " + arrUniqueId[i].Text.Trim();
                                objEmail.BodyFormat = MailFormat.Html;
                                objEmail.Body = tariffstr;
                                //SmtpMail.Send(objEmail);   // $$$$$$$$    REMOVE THIS COMMENT WHEN MAKING LIVE     $$$$$$$$$$$$$$$$$
                            }
                            catch (Exception ex) { }
                        }
                    }

                }
                                

            }

            Response.Redirect("FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=1&o=" + DropOperator.SelectedItem.Text.Trim());

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }




    protected void RadType_Change(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=1&o=" + DropOperator.SelectedItem.Text.Trim());
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }







    protected void DropOperator_Change(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("FEA_Review.aspx?user=" + Request["user"].ToString().Trim() + "&sel=" + RadType.SelectedItem.Text.Trim() + "&pg=1&o=" + DropOperator.SelectedItem.Text.Trim());
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }






    protected void ButtonWord2_Click(object sender, EventArgs e)
    {
        try
        {

            string strBody = "<html><head></head><body>";
            
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

            string uid = TextUID.Text.Trim();
            string oper = "";
            string compname = "";
            string addr="";
            string circ = "";
            DateTime reportdate = Convert.ToDateTime("1/1/2001");
            DateTime actiondate = Convert.ToDateTime("1/1/2001");
            int violationlimit_b = 7;   // no. of days permitted between launch date and reporting date
            double delaydays = 0;
            double penaltydays = 0;
            DateTime Valid8thDay;
            string ttype = "";
                

            try
            {
                com = new MySqlCommand("select * from " + tablename + " where(uniqueid='" + uid + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                oper = dr["oper"].ToString().Trim();
                circ = dr["circ"].ToString().Trim();
                reportdate = Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                actiondate = Convert.ToDateTime(dr["actiondate"].ToString().Trim());
                ttype = dr["ttype"].ToString().Trim();

                

            string violationsdet = "";
            violations = "None";
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

                if(validdaycount<=7)
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
                if(totalpenalty>200000)
                {
                    totalpenalty = 200000;
                }
                penaltyinwords = ConvertNumbertoWords(totalpenalty);
            }

            }
            catch (Exception ex) { }
            con.Close();

            try
            {
                com = new MySqlCommand("select * from TRAI_operators where(oper='" + oper + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                compname = dr["company"].ToString().Trim();
                addr=dr["addr"].ToString().Trim();
            }
            catch (Exception ex) { }
            con.Close();

            strBody = strBody + "<font style='font-family:Bookman Old Style';><center><b>NOTICE TO SHOW CAUSE</b></center><p style=text-align:right>Dated: " + DateTime.Now.ToString("dd-MMM-yyyy") + "</p>";
            strBody = strBody + "<p style=text-align:justify;>Subject: <b>Show Cause Notice to M/s " + compname + " for non- compliance of the provisions of the Telecommunication Tariff Order, 1999.</b></p>"; 
            strBody = strBody + "<p style=text-align:justify;>F.No._________________: Whereas the Telecom Regulatory Authority of India [hereinafter referred to as the Authority], established under sub-section (1) of section 3 of the Telecom Regulatory Authority of India Act, 1997 (24 of 1997) (hereinafter referred to as TRAI Act, 1997), has been entrusted with discharge of certain functions, <i>inter alia</i>, to ensure compliance of terms and conditions of licence; regulate the telecommunication services; protect interest of consumers of the telecom sector;</p>";
            strBody = strBody + "<p style=text-align:justify;>2. 	And whereas, in exercise of the powers conferred upon it under sub-section (2) of section 11 of the TRAI Act, 1997 (24 of 1997), the Authority made the Telecommunication Tariff Order, 1999 dated the 9th March 1999 (hereinafter referred to as TTO, 1999);</p>";
            strBody = strBody + "<p style=text-align:left;>3. 	And whereas sub-clause (db) and (l) of clause 2 of the TTO, 1999 provides as under:-<br /><br />&quot;2(db): `Date of Reporting` means the date on which the report from a service provider regarding the proposed plan or any change in the existing tariff plan, is received at the Authority`s office.<br /><br />2(l): `Reporting Requirement` means the obligation of a service provider to report to the Authority any new tariff for telecommunication services under this order and/or any changes therein within seven working days from the date of implementation of the said tariff for information and record of the Authority after conducting a self-check to ensure that the tariff plan(s) is/are consistent with the regulatory principles in all respects which inter-alia include IUC compliance, Non-discrimination & non-predation`.&quot;;</p>";
            strBody = strBody + "<p style=text-align:justify;>4.   And whereas M/s " + compname + ", vide their letter no. _________________ dated ___________________, reported tariffs in respect of all LSAs;</p>";
            strBody = strBody + "<p style=text-align:justify;>5. 	And whereas the said tariff report was required to be received within seven working days from the date of implementation as per reporting requirement provided under 2(l) of the TTO, 1999;</p>";
            strBody = strBody + "<p style=text-align:justify;>6. 	And whereas the " + ttype + " in respect of all circles was implemented w.e.f. " + actiondate.ToString("dd-MMM-yyyy") + " but information regarding the same was received in the office on " + reportdate.ToString("dd-MMM-yyyy") + ";</p>";
            strBody = strBody + "<p style=text-align:justify;>7.	And whereas M/s " + compname + ";</p>";
            strBody = strBody + "<p style=text-align:justify;>8. 	And whereas sub-clause (iii) of clause 7 of the TTO, 1999 provides as under:-<br /><br />&quot;Clause 7(iii): If any service provider fails to comply with the Reporting Requirement, it shall, without prejudice to the terms and conditions of its licence, or the provisions of the Act or rules or regulations or orders made, or directions issued, thereunder, be liable to pay five thousand rupees, by way of financial disincentive, for every day of delay subject to maximum of two lakh rupees as the Authority may by order direct:<br /><br />Provided that no order for payment of any amount by way of financial disincentive shall be made by the Authority unless the service provider has been given a reasonable opportunity of representing against the contravention of the tariff order observed by the Authority.&quot;;</p>";
            strBody = strBody + "<p style=text-align:justify;>9.    And whereas it is observed that there was a delay of " + penaltydays.ToString().Trim() + " days in reporting the tariff plans by M/s " + compname + " in contravention of the provisions of the TTO, 1999;</p>";
            strBody = strBody + "<p style=text-align:justify;>10.    Now, therefore, M/s " + compname + " is called upon to show cause within two weeks from the date of issue of this notice as to why financial disincentive should not be levied against them for contravention of the provisions of Telecommunication Tariff Order, 1999 and in case no written explanation is received within the stipulated time it will be presumed that M/s " + compname + " has nothing to offer in their defence.</p>";
            strBody = strBody + "<p style=text-align:right;>Yours sincerely,<br /><br />(S.K. Mishra)<br />Principal Advisor (F&EA)<br />Tel: 011-23221856<p>";
            strBody = strBody + "<p style=text-align:left;>To,<br />" + compname + "<br />" + addr + "</p>";




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



    protected void ButtonWord_Click(object sender, EventArgs e)
    {
        try
        {
            /*
            string matter = "Hello.<br /><br />This is a test file.";
            string attachment = "attachment; filename=Test.doc";
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/msword";
            StringWriter sw = new StringWriter();
            StringBuilder sb = new StringBuilder();

            sb.Append(matter.ToString());

            Response.Write(sb.ToString());

            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            // Response.Close();
             */


            string strBody = "<html><head></head><body><font style='font-family:Bookman Old Style';>"; 
            /*
            strBody=strBody + "<div>Your name is: <b>Test</b></div>";
            strBody = strBody + "<table width=100% style=background-color:#cfcfcf;><tr><td>1st Cell body data</td><td>2nd cell body data</td></tr></table>";
            strBody = strBody + "Ms Word document generated successfully.";
            */

            /*
            strBody = strBody + "<center><img src=https://tariff.trai.gov.in/consumerview/images/trai_logo.png width=160 height=140 border=0><br /><br /><br />This is a sample document</center>";
            strBody = strBody + "</body></html>";
            */

            double dailypenalty=0;
            int totalpenalty=0;
            string penaltyinwords="";
            com=new MySqlCommand("select * from TRAI_penaltyparameters",con);
            con.Open();
            dr=com.ExecuteReader();
            dr.Read();
            try
            {
                dailypenalty=Convert.ToDouble(dr["penaltyperday"].ToString().Trim());
            }
            catch(Exception ex){ }
            con.Close();

            string uid=TextUID.Text.Trim();
            string oper="";
            string compname="";
            string addr = "";
            string circ="";
            DateTime reportdate=Convert.ToDateTime("1/1/2001");
            DateTime actiondate=Convert.ToDateTime("1/1/2001");
            int violationlimit_b = 7;   // no. of days permitted between launch date and reporting date
            double delaydays=0;
            double penaltydays = 0;
            DateTime Valid8thDay = Convert.ToDateTime("1/1/2001");

            try
            {
                com=new MySqlCommand("select * from " + tablename + " where(uniqueid='" + uid + "')",con);
                con.Open();
                dr=com.ExecuteReader();
                dr.Read();
                oper=dr["oper"].ToString().Trim();
                circ=dr["circ"].ToString().Trim();
                reportdate=Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                actiondate=Convert.ToDateTime(dr["actiondate"].ToString().Trim());

                /*
                // get no. of eligible days count (by counting saturday / sunday / holidays in between report date and action date)
                DateTime tempdate1_b = Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                DateTime tempdate2_b = Convert.ToDateTime(dr["actiondate"].ToString().Trim());
                TimeSpan ts_b = Convert.ToDateTime(Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy")) - Convert.ToDateTime(Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy"));
                while(tempdate2_b<=tempdate1_b)
                {
                    string weekday = tempdate2_b.ToString("dddd");
                    if (weekday == "Saturday" || weekday == "Sunday")
                    {
                        violationlimit_b++;    // if its a saturday or sunday, increase the days limit
                    }
                    else    // if its a saturday or sunday, no need to check if its a gazetted holiday. hence gazetted holiday check put in else.
                    {
                        com1 = new MySqlCommand("select count(*) from TRAI_holidays where(hdate='" + tempdate2_b.ToString("yyyy-MM-dd") + "')", con1);
                        con1.Open();
                        dr1 = com1.ExecuteReader();
                        dr1.Read();
                        if (Convert.ToInt32(dr1[0].ToString().Trim()) > 0)
                        {
                            violationlimit_b++;    // if any holidays falls betweeen actiondate and report date, increase the days limit
                        }
                        con1.Close();
                    }

                    tempdate2_b = tempdate2_b.AddDays(1);
                }
                double tsdays_b = ts_b.TotalDays;
                if(tsdays_b>violationlimit_b)
                {
                    //delaydays = Math.Round((tsdays_b - violationlimit_b));
                    delaydays = tsdays_b;
                }
                totalpenalty= Convert.ToInt32(dailypenalty * delaydays);
                penaltyinwords= ConvertNumbertoWords(totalpenalty);
                */

                /*
                int violationlimit = 7;   // no. of days permitted between launch date and reporting date
                DateTime Valid7thDay = Convert.ToDateTime(Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy"));
                Valid7thDay = Valid7thDay.AddDays(1);   // date of action is not to be counted, so start from the next date
                Valid8thDay = Valid7thDay;

                int validdaycount = 0;
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
                if (Convert.ToDateTime(dr["reportdate"].ToString().Trim()) > Valid8thDay)
                {
                    TimeSpan ts2 = Convert.ToDateTime(Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy")) - Valid8thDay;
                    penaltydays = ts2.TotalDays;
                    penaltydays = penaltydays - 1;    // reporting date is not to be included, so subtract one day from the calculation
                }
                totalpenalty = Convert.ToInt32(dailypenalty * penaltydays);
                penaltyinwords = ConvertNumbertoWords(totalpenalty);
                */


                string violationsdet = "";
                violations = "None";
                int violationlimit = 7;   // no. of days permitted between launch date and reporting date
                DateTime Valid7thDay = Convert.ToDateTime(Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy"));
                Valid7thDay = Valid7thDay.AddDays(1);   // date of action is not to be counted, so start from the next date
                Valid8thDay = Valid7thDay;

                int validdaycount = 0;
                DateTime tempdate1 = Convert.ToDateTime(dr["reportdate"].ToString().Trim());
                DateTime tempdate2 = Convert.ToDateTime(dr["actiondate"].ToString().Trim());

                /*
                TimeSpan span1 = tempdate1 - tempdate2;
                int datediff = Convert.ToInt32(span1.TotalDays);
                */

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
                        /*
                        if (validdaycount == 7)
                        {
                            Valid7thDay = Valid7thDay.AddDays(7);
                            Valid8thDay = Valid8thDay.AddDays(8);
                        }
                        if (validdaycount == 8)
                        {
                            Valid8thDay = Valid8thDay.AddDays(8);
                        }
                        */
                    }

                    if (validdaycount <= 7)
                    {
                        Valid8thDay = Valid8thDay.AddDays(1);
                    }

                    tempdate2 = tempdate2.AddDays(1);
                }

                if (validdaycount > 7)  // if more than 7 valid days passed in between, calculate the delay
                {
                    /*
                    if (Convert.ToDateTime(dr["reportdate"].ToString().Trim()) > Valid8thDay.AddDays(1))
                    //if (Convert.ToDateTime(dr["reportdate"].ToString().Trim()) > Valid8thDay)
                    {
                        TimeSpan ts2 = Convert.ToDateTime(Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy")) - Valid8thDay;
                        double penaltydays = ts2.TotalDays;
                        penaltydays = penaltydays - 1;    // reporting date is not to be included, so subtract one day from the calculation
                        violations = "<font color=#ffffff style=font-size:12px;>" + penaltydays.ToString() + " day(s)</font>";
                    }
                        */

                    /*
                    double penaltydays = validdaycount - 7;
                    violations = "<font color=#ffffff style=font-size:12px;>" + penaltydays.ToString() + " day(s)</font>";
                    */

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
            catch(Exception ex){}
            con.Close();
            
            
            try
            {
                com=new MySqlCommand("select * from TRAI_operators where(oper='" + oper + "')",con);
                con.Open();
                dr=com.ExecuteReader();
                dr.Read();
                compname=dr["company"].ToString().Trim();
                addr = dr["addr"].ToString().Trim();
            }
            catch(Exception ex){}
            con.Close();

            strBody = strBody + "<table width=100% border=0><tr><td align=left>F. No. _________________________</td><td align=right>Dated: " + DateTime.Now.ToString("dd-MMM-yyyy") + "</td></tr></table>";
            strBody = strBody + "<br /><center><b><u>DRAFT ORDER</u></b></center><br />";
            strBody = strBody + "<p style=text-align:justify;><b>Sub.: Payment of financial disincentive for non compliance of the provisions of sub-clause (l) of clause (2), read with clause 7 of the Telecommunication Tariff Order, 1999 by M/s " + compname + " in " + circ + " service area.</b></p>";
            strBody = strBody + "<p style=text-align:justify;>No. 310-7(62)/2014-F&EA: Whereas the Telecom Regulatory Authority of India, [hereinafter referred to as the Authority], has been established under sub-section (1) of section 3 of the Telecom Regulatory Authority of India Act, 1997 (24 of 1997) (hereinafter referred to as TRAI Act), has been  entrusted with discharge of certain functions, <i>inter-alia</i>, to regulate the telecommunication services; ensure compliance of terms and conditions of licence; notify rates for telecommunication services; protect the interests of consumers of telecom sector;</p>";
            strBody = strBody + "<p style=text-align:justify;>2.	And whereas, in exercise of the powers conferred upon it under sub-section (2) of section 11 of TRAI Act, the Authority made the Telecommunication Tariff Order, 1999 dated the 9th March, 1999 (hereinafter referred to as TTO,1999);</p>";
            strBody = strBody + "<p style=text-align:justify;>3.	 And whereas sub-clause (db) of clause 2 of TTO, 1999 provides as under:- <br /><br >&quot;db. `Date of Reporting` means the date on which the report from a service provider regarding the proposed tariff plan or any change in the existing tariff plan, is received at the Authority`s office.&quot; ;</p>";
            strBody = strBody + "<p style=text-align:justify;>4.	 And whereas sub-clause (l) of clause 2 of TTO, 1999 provides as under:-<br /><br /><i>&quot;l. `Reporting requirement` means the obligation of a service provider to report to the Authority any new tariff for telecommunication services under this order and/or any changes therein within seven working days from the date of implementation of the said tariff for information and record of the Authority after conducting a self-check to ensure that the tariff plan(s) is/are consistent with the regulatory principles in all respects which inter-alia include IUC compliance, non-discrimination and Non-predation.&quot;</i>";
            strBody = strBody + "<p style=text-align:justify;>5.	And whereas sub-clause (i) of clause 7 of the TTO, 1999, <i>inter-alia</i>, provides that all service providers shall comply with the reporting requirement;</p>";
            strBody = strBody + "<p style=text-align:justify;>6.	And whereas, M/s " + compname + ", vide its letter dated " + reportdate.ToString("dd-MMM-yyyy") + " reported the launch of postpaid tariff plan w.e.f. " + actiondate.ToString("dd-MMM-yyyy") + " in respect of " + circ + " service area, which was received in the office of the Authority on the " + reportdate.ToString("dd-MMM-yyyy") + ";</p>";
            strBody = strBody + "<p style=text-align:justify;>7.	And whereas, as per sub-clause (l) of clause 2 of TTO, 1999, M/s " + compname + " was required to report the tariff, mentioned in the preceding para, (implemented on " + actiondate.ToString("dd-MMM-yyyy") + ") latest by the " + Valid8thDay.AddDays(1).ToString("dd-MMM-yyyy") + ", thus, there has been a delay of " + penaltydays.ToString().Trim() + " days in reporting of the said tariff;";
            strBody = strBody + "<p style=text-align:justify;>8.	And whereas sub-clause (iii) of clause 7 of the TTO, 1999 provides as under:-<br /><br /><i>&quot;(iii): If any service provider fails to comply with the Reporting Requirement, it shall, without prejudice to the terms and conditions of its licence, or the provisions of the Act or rules or regulations or orders made, or directions issued, thereunder, be liable to pay five thousand rupees, by way of financial disincentive, for every day of delay subject to maximum of two lakh rupees as the Authority may by order direct: &nbsp;<br /><br />Provided that no order for payment of any amount by way of financial disincentive shall be made by the Authority unless the service provider has been given a reasonable opportunity of representing against the contravention of the tariff order observed by the Authority.&quot;</i>";
            strBody = strBody + "<p style=text-align:justify;>9.	And whereas the Authority, vide Show Cause Notice dated the ______________________, <i>inter-alia</i>, asked M/s " + compname + " to furnish reasons as to why financial disincentive should not be imposed upon it for contravention of the provisions of TTO,1999;";
            strBody = strBody + "<p style=text-align:justify;>10.	And whereas M/s " + compname + ", vide its letter dated the ______________________, in response to the Authority`s letter referred to in the preceding para, submitted that there was no intentional delay in reporting tariff. M/s " + compname + " mentioned that delay, if any, may be due to the non-clarity of what constitutes a &quot;holiday/ holiday(s)&quot;. M/s " + compname + " further pointed out that TRAI has included non-working days as well while calculating the delay.";
            strBody = strBody + "<p style=text-align:justify;>11.	And whereas the Authority considered the written explanations furnished by M/s " + compname + ", referred to in the preceding para, and found that the explanations furnished by M/s " + compname + " is not satisfactory as the stated reason that _______________________________________________________ ___________________________________________________________________________________. And further as per the calculation, ______________________ _________________________________________________________________________";
            strBody = strBody + "<p style=text-align:justify;>12.	Now, therefore, the Authority in exercise of the powers conferred upon it under sub-clause (iii) of clause 7 of the Telecommunication Tariff order, 1999, hereby directs M/s " + compname + " to pay, within twenty days from the date of issue of this Order, financial disincentive of rupees " + totalpenalty.ToString().Trim() + "/- (Rs. " + penaltyinwords + " ONLY) for contravention of the provisions of the Telecommunication Tariff Order,1999, through NEFT/RTGS to the Authority`s bank account as mentioned below and furnish a confirmation to this office:-";
            strBody = strBody + "<p style=text-align:left;>Account No.: 067900101800810<br />Bank and Branch: Corporation Bank, Asaf Ali Road<br />IFSC Code: CORP0000679</p>";
            strBody = strBody + "<p style=text-align:right;>(S.K. Mishra)<br />Pr. Advisor (F&EA)</p>";
            strBody = strBody + "<p style=text-align:left;>To : <br />" + compname + "<br />" + addr + "</p>";


            strBody = strBody + "</font></body></html>";

            string fileName = "FDOrder.doc";
            
            // You can add whatever you want to add as the HTML and it will be generated as Ms Word docs

            Response.AppendHeader("Content-Type", "application/msword");
            //Response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            Response.AppendHeader ("Content-disposition", "attachment; filename="+ fileName);

            Response.Write(strBody);
            
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }




    protected void ButtonPDF_Click(object sender, EventArgs e)
    {
        try
        {
            string strBody = "<html><body><head></head>";
            /*
            strBody = strBody + "<div>Your name is: <b>Test</b></div>";
            strBody = strBody + "<table width=100% style=background-color:#cfcfcf;><tr><td>1st Cell body data</td><td>2nd cell body data</td></tr></table>";
            strBody = strBody + "PDF document generated successfully.<br /><br />";
            strBody = strBody + "<img src=" + Server.MapPath("images/submit.jpg") + " width=70 border=0><br /><br />";
            strBody = strBody + "Image generated successfully.";
            */

            strBody = strBody + "<table width=100%><tr><td align=center style=text-align:center;><img src=" + Server.MapPath("images/logo-trai.png") + " width=70 border=0 style=text-align:center; ><br /><br /><p style=text-align:center;margin-top:100; >This is a sample document</p></td></tr></table>";
            strBody = strBody + "</body></html>";

            
            string fileName = "PDFFile.pdf";

            Response.ContentType = "application/pdf"; // Setting the application

            // Assigning the header
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Creating the object of the String Writer.
            StringWriter sw = new StringWriter();

            /*
            // Creating the object of HTML Writer and passing the object of String Writer to HTMl Text Writer
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            this.Page.RenderControl(hw);
            */



            // Now we what ever is rendered on the page we will give it to the object of the String reader so that we can 
            //StringReader srdr = new StringReader(sw.ToString());
            StringReader srdr = new StringReader(strBody);

            // Creating the PDF DOCUMENT using the Document class from Itextsharp.pdf namespace
            Document pdfDoc = new Document(PageSize.A4, 15F, 15F, 75F, 0.2F);

            // HTML Worker allows us to parse the HTML Content to the PDF Document.To do this we will pass the object of Document class as a Parameter.
            HTMLWorker hparse = new HTMLWorker(pdfDoc);
            // Finally we write data to PDF and open the Document
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            // Now we will pass the entire content that is stored in String reader to HTML Worker object to achieve the data from to String to HTML and then to PDF.
            hparse.Parse(srdr);

            pdfDoc.Close();
            // Now finally we write to the PDF Document using the Response.Write method.
            Response.Write(pdfDoc);
            Response.End();

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





}
