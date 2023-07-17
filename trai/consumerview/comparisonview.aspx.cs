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
using System.Xml.Schema;   // for generating XML
using System.Xml;          // for generating XML

public partial class comparisonview : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, str2, mystr, tablename, ttype, colwidth0, colwidth, pricelabel, talktimelabel;
    int cntr, rno, colcount, circount, size, reccount;
    double[] arrRec, arrPrice, arrTalktime, arrValidity, arrDailyData, arrTotalData;
    string[] arrLogo, arrSummary, arrDataUnit;
    string otype;
    
    /*
    
     STEPS :  
     1. Install 'Python-3.6.4' using its setup in Software -> MYSQL folder
     
     2. Install MySql using the 'mysql-installer-web-community-5.7.20.0' setup in Software -> MYSQL folder
     
     3. After MYSQL installation is complete you need to open Windows Explorer and look for the MySql installation in the Program Files / Program Files (x86) folder of your Windows drive.
    There you will find a folder for MySQL Connector and inside that you will find the MySql.Data.dll which you need to copy inside the BIN folder of your project.
    
     4. Use 'MySqlCommand' instead of 'SqlCommand', 'MySqlConnection' instead of 'SqlConnection', 'MySqlDataReader' instead of 'SqlReader'
     
     
    */



    protected void Page_Load(object sender, EventArgs e)
    {
        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        Server.ScriptTimeout = 999999;

        tablename = "TRAI_tariffs";

        try
        {
            size = 5;    // no. of records to compare (a maximum of this many record numbers will be received in querystring)

            arrRec = new double[size];
            arrPrice = new double[size];
            arrTalktime = new double[size];
            arrValidity = new double[size];
            arrDailyData = new double[size];
            arrTotalData = new double[size];
            arrLogo = new string[size];
            arrSummary = new string[size];
            arrDataUnit = new string[size];

            otype = "TSP";
            try
            {
                otype = Request["p"].ToString().Trim();
            }
            catch (Exception ex) { }

            if(otype=="ISP")
            {
                CheckMonVal.Visible = false;
                tdTalktime.Visible = false;
            }


            ttype=Request["t"].ToString().Trim();
            
            //Lblttype.Text = "<b><u>" + ttype + "</u></b>";
            Lblttype.Text = "<b><u>Comparison</u></b>";

            //pricelabel = "Price &#8377;";
            //talktimelabel = "Talktime &#8377;";
            pricelabel = "Price <i class=\"fa fa-inr\"></i>";
            talktimelabel = "Talktime <i class=\"fa fa-inr\"></i>";
            
            if(ttype=="Plans" || ttype=="Add On Pack" || ttype=="VAS" || ttype=="Promo")
            {
                //pricelabel = "Monthly Rental &#8377;";
                pricelabel = "Monthly Rental <i class=\"fa fa-inr\"></i>";
            }
            CheckPrice.Text = pricelabel;

            if(ttype=="Combo")
            {
                //talktimelabel = "Monetary Value &#8377;";
                talktimelabel = "Monetary Value <i class=\"fa fa-inr\"></i>";
            }
            CheckMonVal.Text = talktimelabel;

            reccount = 0;
            for (int i = 0; i < size; i++)
            {
                try
                {
                    string chk = Request["R" + i].ToString().Trim();
                    arrRec[i] = Convert.ToDouble(Request["R" + i.ToString().Trim()]);
                    reccount++;
                }
                catch (Exception ex)
                {
                    arrRec[i] = 0;
                }
            }


            colwidth0 = "20%";    // width of the first column (field names) is to be kept at 20%, the rest is to be divided betweeen record columns 
            if(reccount==2)
            {
                colwidth = "40%";
            }
            if (reccount == 3)
            {
                colwidth = "26%";
            }
            if (reccount == 4)
            {
                colwidth = "20%";
            }
            if (reccount == 5)
            {
                colwidth = "16%";
            }

            
            // store record parameters in respective arrays
            for (int i = 0; i < reccount; i++)
            {
                string operlogo = "";
                com = new MySqlCommand("select * from " + tablename + " where(rno=" + Convert.ToDouble(arrRec[i].ToString().Trim()) + ")", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    arrSummary[i] = dr["tariffdet"].ToString().Trim().Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34));
                    arrLogo[i] = "logo" + dr["oper"].ToString().Trim().Replace(" ", "") + ".jpg";
                    arrPrice[i] = Convert.ToDouble(dr["mrp"].ToString().Trim());
                    arrTalktime[i] = Convert.ToDouble(dr["monval"].ToString().Trim());
                    arrValidity[i] = Convert.ToDouble(dr["validity"].ToString().Trim());
                    arrDailyData[i] = Convert.ToDouble(dr["adddata_daycap"].ToString().Trim());
                    if (dr["adddata_total4g"].ToString().Trim()!="-1")
                    {
                        arrTotalData[i] = Convert.ToDouble(dr["adddata_total4g"].ToString().Trim());
                    }
                    else
                    {
                        if (dr["adddata_total3g"].ToString().Trim()!="-1")
                        {
                            arrTotalData[i] = Convert.ToDouble(dr["adddata_total3g"].ToString().Trim());
                        }
                        else
                        {
                            arrTotalData[i] = Convert.ToDouble(dr["adddata_total2g"].ToString().Trim());
                        }
                    }
                    arrDataUnit[i] = dr["adddata_unit"].ToString().Trim();
                }
                catch (Exception ex) { }
                con.Close();
            }


            showTable(null, null);

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }


    }






    protected void showTable(object sender, EventArgs e)
    {
        try
        {

            // 'Logo' Row
            mystr = "<center><br /><br />";
            mystr = mystr + "<table width=95% cellspacing=1 border=1 style=border-collapse:collapse; cellpadding=5>";
            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablecell align=center valign=top width=" + colwidth0 + "></td>";
            for (int i = 0; i < reccount; i++)
            {
                mystr = mystr + "<td class=tablecell width=" + colwidth + " align=center valign=top><img src=logos/" + arrLogo[i].ToString().Trim() + " border=0 /></td>";
            }
            mystr = mystr + "</tr>";


            // 'Price / Monthly Rental' row
            if (CheckPrice.Checked == true)
            {
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell align=center valign=top><b>" + pricelabel + "</b></td>";
                for (int i = 0; i < reccount; i++)
                {
                    mystr = mystr + "<td class=tablecell width=" + colwidth + " align=center valign=top>" + arrPrice[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "-").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)) + "</td>";
                }
                mystr = mystr + "</tr>";
            }

            if (otype == "TSP")
            {
                // 'Talktime / Monetary Value' row
                if (CheckMonVal.Checked == true)
                {
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=center valign=top><b>" + talktimelabel + "</b></td>";
                    for (int i = 0; i < reccount; i++)
                    {
                        mystr = mystr + "<td class=tablecell width=" + colwidth + " align=center valign=top>" + arrTalktime[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "-").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)) + "</td>";
                    }
                    mystr = mystr + "</tr>";
                }
            }

            // 'Validity' row
            if (CheckValidity.Checked == true)
            {
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell align=center valign=top><b>Validity (Days)</b></td>";
                for (int i = 0; i < reccount; i++)
                {
                    mystr = mystr + "<td class=tablecell width=" + colwidth + " align=center valign=top>" + arrValidity[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "-").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)) + "</td>";
                }
                mystr = mystr + "</tr>";
            }
            
            
            // 'Daily Data Capping' row
            if (CheckDailyCap.Checked == true)
            {
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell align=center valign=top><b>Daily Data Capping</b></td>";
                for (int i = 0; i < reccount; i++)
                {
                    string mydata = arrDailyData[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "-").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34));
                    if(mydata!="-" && mydata!="")
                    {
                        mydata = mydata + " " + arrDataUnit[i].ToString().Trim().Replace("-1", "-");
                    }
                    mystr = mystr + "<td class=tablecell width=" + colwidth + " align=center valign=top>" + mydata + "</td>";
                }
                mystr = mystr + "</tr>";
            }


            // 'Total Data Capping' row
            if (CheckTotalCap.Checked == true)
            {
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell align=center valign=top><b>Total Data Capping</b></td>";
                for (int i = 0; i < reccount; i++)
                {
                    string mydata = arrTotalData[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "-").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34));
                    if (mydata != "-" && mydata != "")
                    {
                        mydata = mydata + " " + arrDataUnit[i].ToString().Trim().Replace("-1", "-");
                    }
                    mystr = mystr + "<td class=tablecell width=" + colwidth + " align=center valign=top>" + mydata + "</td>";
                }
                mystr = mystr + "</tr>";
            }


            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablecell align=center valign=middle><b>More Details</b></td>";
            int moredivcntr = 0;
            for (int i = 0; i < reccount; i++)
            {
                string moredet = "<div id='divmore" + moredivcntr.ToString().Trim() + "' style=display:none;>";

                com = new MySqlCommand("select * from " + tablename + " where(rno=" + Convert.ToDouble(arrRec[i].ToString().Trim()) + ")", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    moredet = moredet + "<center><br /><table width=95% cellspacing=1 border=1 style=border-collapse:collapse;border-color:#cfcfcf; cellpadding=3>";
                    if (otype == "TSP")
                    {
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>Tariff Summary</b></td></tr>";
                        moredet = moredet + "<tr><td class=tablecellsmall colspan=8 align=center>" + arrSummary[i].ToString().Trim() + "</td></tr>";
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>Local Call Details (INR / Pulse)</b></td></tr>";
                        moredet = moredet + "<tr><td class=tableheadsmall align=center>Mobile On Net<br />(Peak)</td><td class=tableheadsmall align=center>Mobile On Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Mobile Off Net<br />(Peak)</td><td class=tableheadsmall align=center>Mobile Off Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Fixed On Net<br />(Peak)</td><td class=tableheadsmall align=center>Fixed On Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Fixed Off Net<br />(Peak)</td><td class=tableheadsmall align=center>Fixed Off Net<br />(Off Peak)</td></tr>";
                        moredet = moredet + "<tr>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["local_on_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["local_on_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["local_off_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["local_off_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["local_fix_on_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["local_fix_on_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["local_fix_off_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["local_fix_off_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "</tr>";
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>STD Call Details (INR / Pulse)</b></td></tr>";
                        moredet = moredet + "<tr><td class=tableheadsmall align=center>Mobile On Net<br />(Peak)</td><td class=tableheadsmall align=center>Mobile On Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Mobile Off Net<br />(Peak)</td><td class=tableheadsmall align=center>Mobile Off Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Fixed On Net<br />(Peak)</td><td class=tableheadsmall align=center>Fixed On Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Fixed Off Net<br />(Peak)</td><td class=tableheadsmall align=center>Fixed Off Net<br />(Off Peak)</td></tr>";
                        moredet = moredet + "<tr>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["std_on_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["std_on_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["std_off_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["std_off_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["std_fix_on_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["std_fix_on_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["std_fix_off_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["std_fix_off_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "</tr>";
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>SMS Details (INR / SMS)</b></td></tr>";
                        moredet = moredet + "<tr><td class=tableheadsmall align=center>Local On Net</td><td class=tableheadsmall align=center>Local Off Net</td><td class=tableheadsmall align=center>National On Net</td><td class=tableheadsmall align=center>National Off Net</td><td class=tableheadsmall align=center>International</td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td></tr>";
                        moredet = moredet + "<tr>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["sms_local_on"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["sms_local_off"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["sms_nat_on"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["sms_nat_off"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["sms_int"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center></td>";
                        moredet = moredet + "<td class=tablecellsmall align=center></td>";
                        moredet = moredet + "<td class=tablecellsmall align=center></td>";
                        moredet = moredet + "</tr>";
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>National Roaming (INR / Pulse)</b></td></tr>";
                        moredet = moredet + "<tr><td class=tableheadsmall align=center>Pulse (seconds)</td><td class=tableheadsmall align=center>Incoming</td><td class=tableheadsmall align=center>Local Outgoing</td><td class=tableheadsmall align=center>STD Outgoing</td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td></tr>";
                        moredet = moredet + "<tr>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["roam_call_pulse"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["roam_call_voice_in"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["roam_call_voice_out"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center>" + dr["roam_call_voice_std"].ToString().Trim().Replace("-1", "-") + "</td>";
                        moredet = moredet + "<td class=tablecellsmall align=center></td>";
                        moredet = moredet + "<td class=tablecellsmall align=center></td>";
                        moredet = moredet + "<td class=tablecellsmall align=center></td>";
                        moredet = moredet + "<td class=tablecellsmall align=center></td>";
                        moredet = moredet + "</tr>";
                    }

                    //if (dr["adddata_unit"].ToString().Trim().Replace("-1", "").ToUpper() == "MBPS" || dr["adddata_unit"].ToString().Trim().Replace("-1", "").ToUpper() == "KBPS")
                    if (otype=="ISP")
                    {
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>Data Usage</b></td></tr>";
                        moredet = moredet + "<tr>";
                        string totdata = "";
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total2g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "<b>Data usage limit with higher speed (GB)  : </b>" + dr["adddata_total2g"].ToString().Trim().Replace("-1", "0").Replace("-2", "Unlimited");
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total3g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "<br /><b>Speed upto data usage limit : </b>" + dr["adddata_total3g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "");
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total4g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "<br /><b>Speed beyond data usage limit : </b>" + dr["adddata_total4g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "");
                        }
                        if (totdata == "")
                        {
                            totdata = "-";
                        }
                        moredet = moredet + "<td class=tablecellsmall align=left colspan=8>" + totdata + "</td>";
                        moredet = moredet + "</tr>";
                    }
                    else
                    {
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>Total Data</b></td></tr>";
                        moredet = moredet + "<tr>";
                        string totdata = "";
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total2g"].ToString().Trim() != "-1")
                        {
                             totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2G : " + dr["adddata_total2g"].ToString().Trim().Replace("-1", "0").Replace("-2", "Unlimited") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total3g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3G : " + dr["adddata_total3g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total4g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;4G : " + dr["adddata_total4g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (totdata == "")
                        {
                            totdata = "-";
                        }
                        moredet = moredet + "<td class=tablecellsmall align=center colspan=8>" + totdata + "</td>";
                        moredet = moredet + "</tr>";
                    }

                    
                    if (dr["offerconditions"].ToString().Replace("-1", "") != "")
                    {
                        moredet = moredet + "<tr><td class=tablecellsmall align=left colspan=8><b>Eligibility Conditions : </b>" + dr["offerconditions"].ToString().Replace("-1", "") + "</td></tr>";
                    }
                    if (dr["misc_remarks"].ToString().Replace("-1", "") != "")
                    {
                        //moredet = moredet + "<tr><td class=tablecellsmall align=left colspan=8><b>Remarks : </b>" + dr["misc_remarks"].ToString().Replace("-1", "") + "</td></tr>";
                    }
                    if (dr["misc_terms"].ToString().Replace("-1", "") != "")
                    {
                        moredet = moredet + "<tr><td class=tablecellsmall align=left colspan=8><b>Terms & Conditions : </b>" + dr["misc_remarks"].ToString().Replace("-1", "") + "</td></tr>";
                    }
                }
                catch (Exception ex) { }
                con.Close();

                moredet = moredet + "</table><br /><br /></center>";

                moredet = moredet + "</div>";

                divmores.InnerHtml = divmores.InnerHtml + moredet;

                mystr = mystr + "<td class=tablecell width=" + colwidth + " align=center valign=top><p id='p" + moredivcntr.ToString().Trim() + "' align=center><a href=javascript:funmore('" + moredivcntr.ToString().Trim() + "','More') class=indexlinks1>More...</a></p></td>";

                moredivcntr++;

            }
            mystr = mystr + "</tr>";

            mystr = mystr + "</table>";

            divresults.InnerHtml = mystr;


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    
}