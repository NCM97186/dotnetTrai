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


public partial class FEA_recorddetails : System.Web.UI.Page
{
    MySqlCommand com, com1, com2;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno;
    DateTime dt1, dt2;
    double modrno;
    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

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



            string uid = Request["uid"].ToString().Trim();
            string oper = Request["o"].ToString().Trim().Replace("%20", " ");
            string circ = Request["c"].ToString().Trim().Replace("%20", " ");



            // set the array for 'Attribute Key' column values //
            int arrsize=278;
            string[] arrAttr = new string[arrsize];
            int arrPos = 0;

            arrAttr[arrPos]="Tariff Product Type";
            arrPos++;

            arrAttr[arrPos]="Tariff Summary";
            arrPos++;

            arrAttr[arrPos]="Unique Record ID";
            arrPos++;

            arrAttr[arrPos]="TSP";
            arrPos++;

            arrAttr[arrPos]="LSA";
            arrPos++;

            arrAttr[arrPos]="SSA / Cities (In which Services available)";
            arrPos++;

            arrAttr[arrPos]="Type of Service";
            arrPos++;

            arrAttr[arrPos]="Prepaid / Postpaid ";
            arrPos++;

            arrAttr[arrPos]="Launch / Revision / Correction / Withdrawal";
            arrPos++;

            arrAttr[arrPos]="Revision Unique Record ID No., if applicable";
            arrPos++;

            arrAttr[arrPos]="Name of the  Product";
            arrPos++;

            arrAttr[arrPos]="Unique Plan ID of the Voucher / of the plans for which exclusively applicable";
            arrPos++;

            arrAttr[arrPos]="Date of Reporting";
            arrPos++;

            arrAttr[arrPos]="Date of Launch / Revision / Correction";
            arrPos++;

            arrAttr[arrPos]="Regular / Promotional";
            arrPos++;

            arrAttr[arrPos]="Start date of Promotional offer";
            arrPos++;

            arrAttr[arrPos]="End date of Promotional offer";
            arrPos++;

            arrAttr[arrPos]="Special Eligibility Conditions, if any";
            arrPos++;

            arrAttr[arrPos]="Price (Including Processing Fee & GST)";
            arrPos++;

            arrAttr[arrPos]="Monetary Value (in Rs.)";
            arrPos++;

            arrAttr[arrPos]="Available in (Rural / Urban/ Both / Any other)";
            arrPos++;

            arrAttr[arrPos]="Registration charges";
            arrPos++;

            arrAttr[arrPos]="Installation / activation charges";
            arrPos++;

            arrAttr[arrPos]="One time Security Deposit";
            arrPos++;

            arrAttr[arrPos]="Normal Modem Charges";
            arrPos++;

            arrAttr[arrPos]="WiFi Modem Charges";
            arrPos++;

            arrAttr[arrPos]="Plan enrolment fee, if any";
            arrPos++;

            arrAttr[arrPos]="Other one time charges";
            arrPos++;

            arrAttr[arrPos]="Rental / Minimum billing amount, if any - Fixed Charges";
            arrPos++;

            arrAttr[arrPos]="Advance rental option for longer periods";
            arrPos++;

            arrAttr[arrPos]="Security Deposit - Local";
            arrPos++;

            arrAttr[arrPos]="Security Deposit – Local + STD";
            arrPos++;

            arrAttr[arrPos]="Security Deposit – Local + STD + ISD";
            arrPos++;

            arrAttr[arrPos]="Security Deposit – National Roaming";
            arrPos++;

            arrAttr[arrPos]="Security Deposit – International Roaming";
            arrPos++;

            arrAttr[arrPos]="Security Deposit – Any Other";
            arrPos++;

            arrAttr[arrPos]="Fixed Charges –Exchange Capacity<=999";
            arrPos++;

            arrAttr[arrPos]="Fixed Charges –Exchange Capacity > 999 and <=29999";
            arrPos++;

            arrAttr[arrPos]="Fixed Charges –Exchange Capacity >= 30000 and <=99999";
            arrPos++;

            arrAttr[arrPos]="Fixed Charges –Exchange Capacity >= 100000";
            arrPos++;

            arrAttr[arrPos]="Optional Fixed Monthly Charges - CLIP";
            arrPos++;

            arrAttr[arrPos]="Free Calls – MCUS per month";
            arrPos++;

            arrAttr[arrPos]="Free Calls – MCUS per month – Exchange capacity<=999";
            arrPos++;

            arrAttr[arrPos]="Free Calls – MCUS per month – Exchange capacity> 999 and <=29999";
            arrPos++;

            arrAttr[arrPos]="Free Calls – MCUS per month – Exchange capacity>=30000 and <=99999";
            arrPos++;

            arrAttr[arrPos]="Free Calls – MCUS per month – Exchange capacity >= 100000";
            arrPos++;

            arrAttr[arrPos]="Free Calls – MCUS per month – Free Talktime";
            arrPos++;

            arrAttr[arrPos]="Validity ";
            arrPos++;

            arrAttr[arrPos]="Pay Per Use charges";
            arrPos++;

            arrAttr[arrPos]="Other Model – Details";
            arrPos++;

            arrAttr[arrPos]="Benefits";
            arrPos++;

            arrAttr[arrPos]="Call Rates – Peak Time From, if any";
            arrPos++;

            arrAttr[arrPos]="Call Rates – Peak Time Till, if any";
            arrPos++;

            arrAttr[arrPos]="Call Rates – Off Peak Time From, if any";
            arrPos++;

            arrAttr[arrPos]="Call Rates – Off Peak Time Till, if any";
            arrPos++;

            arrAttr[arrPos]="All Local Call (Onnet + Offnet + Peak + Offpeak) – Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="All Local Call (Onnet + Offnet + Peak + Offpeak) – Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="All Local Call Rate (Onnet + Offnet + Peak + Offpeak) - Voice Call Rate (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos] = "All Local Call Rate (Onnet + Offnet + Peak + Offpeak) - Video Call Rate (in INR/pulse)";
            arrPos++;
                        
            arrAttr[arrPos]="Local Call Mobile OnNet - Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Local Call Mobile OnNet - Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OnNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OnNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OnNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OnNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Mobile OnNet - Validity in Days";
            arrPos++;

            arrAttr[arrPos]="Local Call Mobile OffNet - Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Local Call Mobile OffNet - Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OffNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OffNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OffNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Mobile OffNet - Validity in Days";
            arrPos++;

            arrAttr[arrPos]="Local Call Fixed OnNet - Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Local Call Fixed OnNet - Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Fixed OnNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Fixed OnNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Fixed OnNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Fixed OnNet -Video Call Rate (off Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Fixed OnNet - Validity in Days";
            arrPos++;

            arrAttr[arrPos]="Local Call Mobile OffNet - Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Local Call Mobile OffNet - Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OffNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Rate Mobile OffNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Local Call Mobile OffNet - Validity in Days";
            arrPos++;

            arrAttr[arrPos]="CUP (Commercial Usage Policy), if any";
            arrPos++;

            arrAttr[arrPos]="Description of local call rate / pulse rate in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="All STD Call (Onnet + Offnet + Peak + Offpeak) – Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="All STD Call (Onnet + Offnet + Peak + Offpeak) – Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="All STD Call Rate (Onnet + Offnet + Peak + Offpeak) - Voice Call Rate (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="All STD Call Rate (Onnet + Offnet + Peak + Offpeak) - Video Call Rate (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Mobile OnNet - Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="STD Call Mobile OnNet - Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Mobile OnNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Mobile OnNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Mobile OnNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Mobile OnNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Mobile OnNet - Validity in Days";
            arrPos++;

            arrAttr[arrPos]="STD Call Mobile OffNet - Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="STD Call Mobile OffNet - Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Mobile OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Mobile OffNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Mobile OffNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Mobile OffNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Mobile OffNet - Validity in Days";
            arrPos++;

            arrAttr[arrPos]="STD Call Fixed OnNet - Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="STD Call Fixed OnNet - Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Fixed OnNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Fixed OnNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Fixed OnNet -Voice Call Rate (off peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Fixed OnNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Fixed OnNet - Validity in Days";
            arrPos++;

            arrAttr[arrPos]="STD Call Fixed OffNet - Voice Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="STD Call Fixed OffNet - Video Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Fixed OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Fixed OffNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Fixed OffNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Rate Fixed OffNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="STD Call Fixed OffNet - Validity in Days";
            arrPos++;

            arrAttr[arrPos]="CUP (Commercial Usage Policy), if any";
            arrPos++;

            arrAttr[arrPos]="Description of STD Call Rate / pulse rate in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - All Local (onnet +Offnet)";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - All National (onnet +Offnet)";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - Local Onnet";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - Local Offnet";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - National Onnet";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - National Offnet";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - International";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - Terms";
            arrPos++;

            arrAttr[arrPos]="SMS Rate (in INR/SMS) - Validity";
            arrPos++;

            arrAttr[arrPos]="CUP (Commercial Usage Policy), if any";
            arrPos++;

            arrAttr[arrPos]="Description of SMS Rate, in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="ISD call – ISD Countries";
            arrPos++;

            arrAttr[arrPos]="ISD call - Pulse Rate (in seconds)";
            arrPos++;

            arrAttr[arrPos]="ISD Call Rate - ISD Calls to Mobile (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="ISD Call Rate - ISD calls to Landline (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="ISD Call Rate - ISD Video calls (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="ISD Call Rate - Weblink for ISD Call Rate";
            arrPos++;

            arrAttr[arrPos]="ISD Call Rate - Conditions, if any";
            arrPos++;

            arrAttr[arrPos]="ISD call - Validity (In Days) ";
            arrPos++;

            arrAttr[arrPos]="CUP (Commercial Usage Policy), if any";
            arrPos++;

            arrAttr[arrPos]="Description of ISD Call Rate / pulse rate in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Call while Roaming - Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Call Rate while Roaming – Incoming voice (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Call Rate while Roaming - Incoming video (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Call Rate while Roaming - Local Outgoing voice (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Call Rate while Roaming – Local Outgoing video (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Call Rate while Roaming – STD Outgoing voice (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Call Rate while Roaming – STD Outgoing video (in INR/pulse)";
            arrPos++;

            arrAttr[arrPos]="Call while Roaming – Validity in Days";
            arrPos++;

            arrAttr[arrPos]="Call Rate while Roaming - Web link for National outgoing ISD Call Rate";
            arrPos++;

            arrAttr[arrPos]="CUP (Commercial Usage Policy), if any";
            arrPos++;

            arrAttr[arrPos]="Description of Call Rate while roaming in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="SMS Rate while Roaming - Local (in INR/SMS)";
            arrPos++;

            arrAttr[arrPos]="SMS Rate while Roaming - National (in INR/SMS)";
            arrPos++;

            arrAttr[arrPos]="SMS Rate while Roaming - International (in INR/SMS)";
            arrPos++;

            arrAttr[arrPos]="SMS Rate while Roaming - Any other";
            arrPos++;

            arrAttr[arrPos]="SMS while Roaming - Validity in days";
            arrPos++;

            arrAttr[arrPos]="CUP (Commercial Usage Policy), if any";
            arrPos++;

            arrAttr[arrPos]="Description of SMS Rate / pulse rate in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Countries";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Incoming Pulse (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Rate while International Roaming - ISD Incoming Calls";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Pulse Outgoing Local Calls (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Rate while International Roaming - ISD Outgoing Local Calls";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Pulse Outgoing Calls to India (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Rate while International Roaming - ISD Outgoing Calls to India";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Pulse Outgoing Calls to Other Countries (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Rate while International Roaming - ISD Outgoing Calls to Other Countries";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Video call (in seconds)";
            arrPos++;

            arrAttr[arrPos]="Rate while International Roaming - ISD Outgoing SMS";
            arrPos++;

            arrAttr[arrPos]="Rate while International Roaming - ISD Incoming SMS";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Pulse Incoming Free Usage (in seconds)";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Incoming Free Usage (in minutes)";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Pulse Outgoing Free Usage (in seconds)";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Outgoing Free Usage (in minutes)";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Unit Free Data Usage";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Free Data Usage";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - ISD Free SMS";
            arrPos++;

            arrAttr[arrPos]="While International Roaming - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="CUP (Commercial Usage Policy), if any";
            arrPos++;

            arrAttr[arrPos]="Description of Rate while International Roaming in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Data Rate – Home";
            arrPos++;

            arrAttr[arrPos]="Data Rate - Roaming";
            arrPos++;

            arrAttr[arrPos]="Data Rate - Conditions";
            arrPos++;

            arrAttr[arrPos]="Monthly Rental for International roaming (in Rs.)";
            arrPos++;

            arrAttr[arrPos]="Weblink for Rates while international roaming";
            arrPos++;

            arrAttr[arrPos]="Duration for Additional Benefits -Time From (hh:mm:ss)";
            arrPos++;

            arrAttr[arrPos]="Duration for Additional Benefits -Time Till (hh:mm:ss)";
            arrPos++;

            arrAttr[arrPos]="Additional Local (in minutes) - Local (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local (in minutes) - Local Onnet (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local (in minutes) - Local Offnet (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local (in minutes) - Local Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local (in minutes) - Local Onnet Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local (in minutes) - Local Offnet Mobile (in minutes";
            arrPos++;

            arrAttr[arrPos]="Additional Local (in minutes) - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Description of Additional Local in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Additional STD (in minutes) - Local (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional STD (in minutes) - Local Onnet (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional STD (in minutes) - Local Offnet (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional STD (in minutes) - Local Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional STD (in minutes) - Local Onnet Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional STD (in minutes) - Local Offnet Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional STD (in minutes) - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Description of Additional STD in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Additional Local & STD (in minutes) - Local & STD (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local & STD (in minutes) - Local & STD Onnet (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local & STD (in minutes) - Local & STD Offnet (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local & STD (in minutes) - Local & STD Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local & STD (in minutes) - Local & STD Onnet Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local & STD (in minutes) - Local & STD Offnet Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local & STD (in minutes) - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Description of Additional Local & STD in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Additional Local, STD & Roaming (in minutes) - Local, STD & Roaming (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local, STD & Roaming (in minutes) - Local, STD & Roaming Mobile (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Local, STD & Roaming (in minutes) - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Description of Additional Local, STD & Roaming in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Additional Roaming (in minutes) - Incoming & Outgoing (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Roaming (in minutes) - Incoming Free (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Roaming (in minutes) - Outgoing Free (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Roaming (in minutes) - Outgoing Local & STD Mobile Free (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Roaming (in minutes) - Outgoing Local Free (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Roaming (in minutes) - Outgoing STD Free (in minutes)";
            arrPos++;

            arrAttr[arrPos]="Additional Roaming (in minutes) - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Description of Additional Roaming in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - Local & National";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - Local & National Onnet";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - Local & National Offnet";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - Local";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - Local Onnet";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - Local Offnet";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - National";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - National Onnet";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - National Offnet";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - International";
            arrPos++;

            arrAttr[arrPos]="Additional SMS - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Description of Additional SMS in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Additional ISD - Summary of ISD freebies";
            arrPos++;

            arrAttr[arrPos]="Additional ISD - Weblink for ISD freebies";
            arrPos++;

            arrAttr[arrPos]="Additional ISD - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Description of Additional ISD in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Additional Video Call - Local & STD Video";
            arrPos++;

            arrAttr[arrPos]="Additional Video Call - Local Video";
            arrPos++;

            arrAttr[arrPos]="Additional Video Call - Local Video Onnet";
            arrPos++;

            arrAttr[arrPos]="Additional Video Call - Local Video Offnet";
            arrPos++;

            arrAttr[arrPos]="Additional Video Call - STD Video";
            arrPos++;

            arrAttr[arrPos]="Additional Video Call - STD Video Onnet";
            arrPos++;

            arrAttr[arrPos]="Additional Video Call - STD Video Offnet";
            arrPos++;

            arrAttr[arrPos]="Additional Video Call - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Description of Additional Video Call in case any other pattern is followed";
            arrPos++;

            if (oper.ToUpper() == "AIRCEL" || oper.ToUpper() == "AIRTEL" || oper.ToUpper() == "BSNL" || oper.ToUpper() == "IDEA" || oper.ToUpper() == "JIO" || oper.ToUpper() == "MTNL" || oper.ToUpper() == "QUADRANT (CONNECT)" || oper.ToUpper() == "TATA TELE" || oper.ToUpper() == "TELENOR" || oper.ToUpper() == "VODAFONE" || oper.ToUpper() == "VODAFONE IDEA" || oper.ToUpper() == "SURFTELECOM" || oper.ToUpper() == "AEROVOYCE")
            {
                arrAttr[arrPos] = "Additional Data - Unit (MB / GB)";
                arrPos++;

                arrAttr[arrPos] = "Additional Data - Total 2G Data";
                arrPos++;

                arrAttr[arrPos] = "Additional Data - Total 3G Data";
                arrPos++;

                arrAttr[arrPos] = "Additional Data - Total 4G Data";
                arrPos++;
            }
            else
            {
                arrAttr[arrPos] = "Data Speed - Unit (kbps / mbps)";
                arrPos++;

                arrAttr[arrPos] = "Data Usage Limit With Higher Speed (in GB)";
                arrPos++;

                arrAttr[arrPos] = "Speed of Connection Upto Data Usage Limit";
                arrPos++;

                arrAttr[arrPos] = "Speed of Connection Beyond Data Usage Limit";
                arrPos++;
            }
            
            arrAttr[arrPos]="Total Data (For ISP / Fixed Line Tariff / Fixed Line Add On Pack)";
            arrPos++;

            arrAttr[arrPos]="Additional Data - Day/Night Data Capping";
            arrPos++;

            arrAttr[arrPos]="Additional Data - Weekly Data Capping";
            arrPos++;

            arrAttr[arrPos]="Additional Data - Monthly Data Capping";
            arrPos++;

            arrAttr[arrPos]="Additional Data - Carry Forward (Yes / No)";
            arrPos++;

            arrAttr[arrPos]="Additional Data - Validity (In Days)";
            arrPos++;

            arrAttr[arrPos]="Additional Data - FUP, if any (Yes/No)";
            arrPos++;

            arrAttr[arrPos]="Additional Data - Condition, if any";
            arrPos++;

            arrAttr[arrPos]="Description of Additional Data in case any other pattern is followed";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous -Special benefits , if any";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Other charges, if any";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Remarks, if any";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Terms and Conditions, if any";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Mode of Activation / Recharge (Website / App only / paper / USSD / 3rd party Wallet etc)";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Whether details of this service have been uploaded on the website (Either &quot;Yes&quot; or &quot;No&quot; should be entered)";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - TSP website link of the Plan";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Any other declarations";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Condition for termination of Product if any";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Activation Code If Any";
            arrPos++;

            arrAttr[arrPos]="Miscellaneous - Deactivation Code If Any";
            arrPos++;

            arrAttr[arrPos]="Delayed Submission";
            arrPos++;

            arrAttr[arrPos]="Reason for delay";
            arrPos++;


            // set the array for 'Attribute Key' column values - CODE ENDS HERE //





            string mystr = "<table width=95% cellspacing=1 cellpadding=5>";
            
            int colno = 1;

            string activerecord = "";
            com = new MySqlCommand("select count(*) from TRAI_archive where (uniqueid='" + uid + "') and (oper='" + oper + "') and (circ='" + circ + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            try
            {
                if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
                {
                    activerecord = "No";
                }
            }
            catch (Exception ex) { }
            con.Close();
            com = new MySqlCommand("select count(*) from TRAI_tariffs where (uniqueid='" + uid + "') and (oper='" + oper + "') and (circ='" + circ + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            try
            {
                if(Convert.ToInt32(dr[0].ToString().Trim())>0)
                {
                    activerecord = "Yes";
                }
            }
            catch (Exception ex) { }
            con.Close();


            if (activerecord == "No")
            {
                com = new MySqlCommand("select * from TRAI_archive where (uniqueid='" + uid + "') and (oper='" + oper + "') and (circ='" + circ + "') order by rno desc", con);
            }
            else
            {
                com = new MySqlCommand("select * from TRAI_tariffs where (uniqueid='" + uid + "') and (oper='" + oper + "') and (circ='" + circ + "')", con);
            }
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();

            string actiontotake=dr["actiontotake"].ToString().Trim();



            if(actiontotake.ToUpper()=="LAUNCH" || actiontotake=="WITHDRAWAL")
            {
                mystr = mystr + "<tr><td class=tablehead>Parameter Name</td><td class=tablehead>Parameter Value</td></tr>";

                int rowno = 1;
                for (int i = 4; i <= 281; i++)
                {
                    int showflag = 0;
                    if (i == 16 || i == 17 || i == 19 || i == 20)
                    {
                        if (Convert.ToDateTime(dr[i].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-2001" || Convert.ToDateTime(dr[i].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-2011")
                        {
                            showflag = 1;
                        }
                    }
                    else
                    {
                        if (dr[i].ToString() == "" || dr[i].ToString() == "-1")
                        {
                            showflag = 1;
                        }
                    }
                    if (showflag == 0)
                    {
                        string css = "tablecell3";
                        if (rowno % 2 == 0)
                        {
                            css = "tablecell3b";
                        }

                        if (i == 17)
                        {
                            if (activerecord == "No")   // if withdrawn, show launch date as well as withdrawal date
                            {
                                com2 = new MySqlCommand("select * from TRAI_archive where (uniqueid='" + uid + "') and (oper='" + oper + "') and (circ='" + circ + "') order by rno", con2);
                                con2.Open();
                                dr2 = com2.ExecuteReader();
                                dr2.Read();
                                try
                                {
                                    mystr = mystr + "<tr><td class=" + css + " align=left width=40%>" + arrAttr[i - 4] + "</td>";
                                    mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + Convert.ToDateTime(dr2[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "").Replace("01-Jan-2011", "") + "</td>";
                                    mystr = mystr + "</tr>";
                                    mystr = mystr + "<tr><td class=" + css + " align=left width=40%>Date of Withdrawal</td>";
                                    mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + Convert.ToDateTime(dr[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "").Replace("01-Jan-2011", "") + "</td>";
                                    mystr = mystr + "</tr>";
                                }
                                catch (Exception ex) { }
                            }
                            else
                            {
                                mystr = mystr + "<tr><td class=" + css + " align=left width=40%>" + arrAttr[i - 4] + "</td>";
                                mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + Convert.ToDateTime(dr[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "").Replace("01-Jan-2011", "") + "</td>";
                                mystr = mystr + "</tr>";
                            }
                        }
                        else
                        {
                            mystr = mystr + "<tr>";
                            mystr = mystr + "<td class=" + css + " align=left width=40%>" + arrAttr[i - 4] + "</td>";
                            if (i == 16 || i == 19 || i == 20)
                            {
                                mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + Convert.ToDateTime(dr[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "").Replace("01-Jan-2011", "") + "</td>";
                            }
                            else
                            {
                                mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + dr[i].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;") + "</td>";
                            }
                            mystr = mystr + "</tr>";
                        }
                        colno++;
                        rowno++;
                    }
                }
            }



            if (actiontotake.ToUpper() == "REVISION")
            {
                mystr = mystr + "<tr><td class=tablehead>Parameter Name</td><td class=tablehead>Old Parameter Value</td><td class=tablehead>New Parameter Value</td></tr>";

                //com1 = new MySqlCommand("select * from TRAI_archive where(uniqueid='" + uid + "' or revisionid='" + uid + "') and (oper='" + oper + "') and (circ='" + circ + "') order by rno desc limit 1", con1);
                com1 = new MySqlCommand("select * from TRAI_archive where(uniqueid='" + dr["revisionid"].ToString().Trim() + "' or revisionid='" + dr["revisionid"].ToString().Trim() + "') and (oper='" + oper + "') and (circ='" + circ + "') order by rno desc limit 1", con1);
                con1.Open();
                dr1 = com1.ExecuteReader();
                dr1.Read();

                int rowno = 1;
                for (int i = 4; i <= 281; i++)
                {
                    int showflag = 0;
                    if (i == 16 || i == 17 || i == 19 || i == 20)
                    {
                        if (Convert.ToDateTime(dr[i].ToString()) == Convert.ToDateTime("1/1/2001") || Convert.ToDateTime(dr[i].ToString()) == Convert.ToDateTime("1/1/2011"))
                        {
                            showflag = 1;
                        }
                    }
                    else
                    {
                        if (dr[i].ToString().Trim() == "" || dr[i].ToString().Trim() == "-1") 
                        {
                            showflag = 1;
                        }
                    }

                    if (showflag == 0)
                    {
                        string css = "tablecell3";
                        if (rowno % 2 == 0)
                        {
                            css = "tablecell3b";
                        }
                        mystr = mystr + "<tr>";
                        string font = "<font style=color:#000000;>";
                        if (dr[i].ToString().Trim() != dr1[i].ToString().Trim())
                        {
                            font = "<font style=color:#ff0000;font-weight:bold;>";
                        }
                        mystr = mystr + "<td class=" + css + " align=left width=40%>" + arrAttr[i - 4] + "</td>";
                        if (i == 16 || i == 17 || i == 19 || i == 20)
                        {
                            mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + font + Convert.ToDateTime(dr1[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "").Replace("01-Jan-2011", "") + "</font></td>";
                            mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + font + Convert.ToDateTime(dr[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "").Replace("01-Jan-2011", "") + "</td>";
                        }
                        else
                        {
                            mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + font + dr1[i].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;") + "</font></td>";
                            mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + font + dr[i].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;") + "</font></td>";
                        }
                        mystr = mystr + "</tr>";
                        colno++;
                        rowno++;
                    }
                }

                con1.Clone();
            }



            if (actiontotake.ToUpper() == "CORRECTION")
            {
                mystr = mystr + "<tr><td class=tablehead>Parameter Name</td><td class=tablehead>Old Parameter Value</td><td class=tablehead>New Parameter Value</td></tr>";

                //com1 = new MySqlCommand("select * from TRAI_archive where(uniqueid='" + uid + "' or revisionid='" + uid + "') and (oper='" + oper + "') and (circ='" + circ + "') order by rno desc limit 1", con1);
                com1 = new MySqlCommand("select * from TRAI_archive where(uniqueid='" + dr["uniqueid"].ToString().Trim() + "') and (oper='" + oper + "') and (circ='" + circ + "') order by rno desc limit 1", con1);
                con1.Open();
                dr1 = com1.ExecuteReader();
                dr1.Read();

                int rowno = 1;
                for (int i = 4; i <= 281; i++)
                {
                    int showflag = 0;
                    if (i == 16 || i == 17 || i == 19 || i == 20)
                    {
                        if (Convert.ToDateTime(dr[i].ToString()) == Convert.ToDateTime("1/1/2001") || Convert.ToDateTime(dr[i].ToString()) == Convert.ToDateTime("1/1/2011"))
                        {
                            showflag = 1;
                        }
                    }
                    else
                    {
                        if (dr[i].ToString().Trim() == "" || dr[i].ToString().Trim() == "-1")
                        {
                            showflag = 1;
                        }
                    }

                    if (showflag == 0)
                    {
                        string css = "tablecell3";
                        if (rowno % 2 == 0)
                        {
                            css = "tablecell3b";
                        }
                        mystr = mystr + "<tr>";
                        string font = "<font style=color:#000000;>";
                        if (dr[i].ToString().Trim() != dr1[i].ToString().Trim())
                        {
                            font = "<font style=color:#ff0000;font-weight:bold;>";
                        }
                        mystr = mystr + "<td class=" + css + " align=left width=40%>" + arrAttr[i - 4] + "</td>";
                        if (i == 16 || i == 17 || i == 19 || i == 20)
                        {
                            mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + font + Convert.ToDateTime(dr1[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "").Replace("01-Jan-2011", "") + "</font></td>";
                            mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + font + Convert.ToDateTime(dr[i].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "").Replace("01-Jan-2011", "") + "</td>";
                        }
                        else
                        {
                            mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + font + dr1[i].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;") + "</font></td>";
                            mystr = mystr + "<td class=" + css + " align=left style=min-width:150px; >" + font + dr[i].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;") + "</font></td>";
                        }
                        mystr = mystr + "</tr>";
                        colno++;
                        rowno++;
                    }
                }

                con1.Clone();
            }
            
            
            con.Close();

            mystr = mystr + "</table>";








            divresults.InnerHtml = mystr;



        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }







}
