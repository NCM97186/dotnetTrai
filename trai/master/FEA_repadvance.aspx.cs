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
using System.Net;

public partial class FEA_repadvance : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    Table tbresults;
    CheckBox[] arrResult;
    CheckBox chkResult;
    int zno, arrSize, cntrConditions;
    string[] arrAttr, arrFlds, arrType;
    TextBox[] arrConditionsNumeric, arrConditionsText, arrConditionsDate;
    TextBox[] arrValNumeric, arrValText, arrValDate;
    TextBox TextN1, TextN2, TextT1, TextT2, TextD1, TextD2;
    TextBox[] arrNumFig1, arrNumFig2, arrTextFig1, arrDateFig1, arrDateFig2;
    TextBox F1, F2, F3, F4, F5;

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

        int resultsize = 20000;
        arrResult = new CheckBox[resultsize];
        for (int ii = 0; ii < resultsize; ii++)
        {
            chkResult = new CheckBox();
            arrResult[ii] = chkResult;
            divChkResults.Controls.Add(arrResult[ii]);
        }

        // Controls for storing conditions //

        cntrConditions = 50;
        arrConditionsNumeric = new TextBox[cntrConditions];
        arrConditionsText = new TextBox[cntrConditions];
        arrConditionsDate = new TextBox[cntrConditions];
        arrValNumeric = new TextBox[cntrConditions];
        arrValText = new TextBox[cntrConditions];
        arrValDate = new TextBox[cntrConditions];
        arrNumFig1 = new TextBox[cntrConditions];
        arrNumFig2 = new TextBox[cntrConditions];
        arrTextFig1 = new TextBox[cntrConditions];
        arrDateFig1 = new TextBox[cntrConditions];
        arrDateFig2 = new TextBox[cntrConditions];
        for (int i = 0; i < cntrConditions; i++)
        {
            TextN1 = new TextBox();
            TextN1.ID = "TextNV" + i.ToString();
            arrConditionsNumeric[i] = TextN1;
            divConditions.Controls.Add(arrConditionsNumeric[i]);
            TextN2 = new TextBox();
            TextN2.ID = "TextNP" + i.ToString();
            arrValNumeric[i] = TextN2;
            divConditions.Controls.Add(arrValNumeric[i]);
            TextT1 = new TextBox();
            TextT1.ID = "TextTV" + i.ToString();
            arrConditionsText[i] = TextT1;
            divConditions.Controls.Add(arrConditionsText[i]);
            TextT2 = new TextBox();
            TextT2.ID = "TextTP" + i.ToString();
            arrValText[i] = TextT2;
            divConditions.Controls.Add(arrValText[i]);
            TextD1 = new TextBox();
            TextD1.ID = "TextDV" + i.ToString();
            arrConditionsDate[i] = TextD1;
            divConditions.Controls.Add(arrConditionsDate[i]);
            TextD2 = new TextBox();
            TextD2.ID = "TextDP" + i.ToString();
            arrValDate[i] = TextD2;
            divConditions.Controls.Add(arrValDate[i]);
            F1 = new TextBox();
            F1.ID = "FA" + i.ToString();
            arrNumFig1[i] = F1;
            divConditions.Controls.Add(arrNumFig1[i]);
            F2 = new TextBox();
            F2.ID = "FB" + i.ToString();
            arrNumFig2[i] = F2;
            divConditions.Controls.Add(arrNumFig2[i]);
            F3 = new TextBox();
            F3.ID = "FC" + i.ToString();
            arrTextFig1[i] = F3;
            divConditions.Controls.Add(arrTextFig1[i]);
            F4 = new TextBox();
            F4.ID = "FD" + i.ToString();
            arrDateFig1[i] = F4;
            divConditions.Controls.Add(arrDateFig1[i]);
            F5 = new TextBox();
            F5.ID = "FE" + i.ToString();
            arrDateFig2[i] = F5;
            divConditions.Controls.Add(arrDateFig2[i]);
        }

        // Controls for storing conditions - CODE ENDS HERE //


        // set the array for 'Attribute Key' column values //
        arrSize = 278;
        arrAttr = new string[arrSize];
        arrFlds = new string[arrSize];
        arrType = new string[arrSize];
        int arrPos = 0;

        arrAttr[arrPos] = "Tariff Product Type";
        arrPos++;

        arrAttr[arrPos] = "Tariff Summary";
        arrPos++;

        arrAttr[arrPos] = "Unique Record ID";
        arrPos++;

        arrAttr[arrPos] = "TSP";
        arrPos++;

        arrAttr[arrPos] = "LSA";
        arrPos++;

        arrAttr[arrPos] = "SSA / Cities (In which Services available)";
        arrPos++;

        arrAttr[arrPos] = "Type of Service";
        arrPos++;

        arrAttr[arrPos] = "Prepaid / Postpaid ";
        arrPos++;

        arrAttr[arrPos] = "Launch / Revision / Correction / Withdrawal";
        arrPos++;

        arrAttr[arrPos] = "Revision Unique Record ID No., if applicable";
        arrPos++;

        arrAttr[arrPos] = "Name of the  Product";
        arrPos++;

        arrAttr[arrPos] = "Unique Plan ID of the Voucher / of the plans for which exclusively applicable";
        arrPos++;

        arrAttr[arrPos] = "Date of Reporting";
        arrPos++;

        arrAttr[arrPos] = "Date of Launch / Revision / Correction / Withdrawal";
        arrPos++;

        arrAttr[arrPos] = "Regular / Promotional";
        arrPos++;

        arrAttr[arrPos] = "Start date of Promotional offer";
        arrPos++;

        arrAttr[arrPos] = "End date of Promotional offer";
        arrPos++;

        arrAttr[arrPos] = "Special Eligibility Conditions, if any";
        arrPos++;

        arrAttr[arrPos] = "Price (Including Processing Fee & GST)";
        arrPos++;

        arrAttr[arrPos] = "Monetary Value (in Rs.)";
        arrPos++;

        arrAttr[arrPos] = "Available in (Rural / Urban/ Both / Any other)";
        arrPos++;

        arrAttr[arrPos] = "Registration charges";
        arrPos++;

        arrAttr[arrPos] = "Installation / activation charges";
        arrPos++;

        arrAttr[arrPos] = "One time Security Deposit";
        arrPos++;

        arrAttr[arrPos] = "Normal Modem Charges";
        arrPos++;

        arrAttr[arrPos] = "WiFi Modem Charges";
        arrPos++;

        arrAttr[arrPos] = "Plan enrolment fee, if any";
        arrPos++;

        arrAttr[arrPos] = "Other one time charges";
        arrPos++;

        arrAttr[arrPos] = "Rental / Minimum billing amount, if any - Fixed Charges";
        arrPos++;

        arrAttr[arrPos] = "Advance rental option for longer periods";
        arrPos++;

        arrAttr[arrPos] = "Security Deposit - Local";
        arrPos++;

        arrAttr[arrPos] = "Security Deposit – Local + STD";
        arrPos++;

        arrAttr[arrPos] = "Security Deposit – Local + STD + ISD";
        arrPos++;

        arrAttr[arrPos] = "Security Deposit – National Roaming";
        arrPos++;

        arrAttr[arrPos] = "Security Deposit – International Roaming";
        arrPos++;

        arrAttr[arrPos] = "Security Deposit – Any Other";
        arrPos++;

        arrAttr[arrPos] = "Fixed Charges –Exchange Capacity<=999";
        arrPos++;

        arrAttr[arrPos] = "Fixed Charges –Exchange Capacity > 999 and <=29999";
        arrPos++;

        arrAttr[arrPos] = "Fixed Charges –Exchange Capacity >= 30000 and <=99999";
        arrPos++;

        arrAttr[arrPos] = "Fixed Charges –Exchange Capacity >= 100000";
        arrPos++;

        arrAttr[arrPos] = "Optional Fixed Monthly Charges - CLIP";
        arrPos++;

        arrAttr[arrPos] = "Free Calls – MCUS per month";
        arrPos++;

        arrAttr[arrPos] = "Free Calls – MCUS per month – Exchange capacity<=999";
        arrPos++;

        arrAttr[arrPos] = "Free Calls – MCUS per month – Exchange capacity> 999 and <=29999";
        arrPos++;

        arrAttr[arrPos] = "Free Calls – MCUS per month – Exchange capacity>=30000 and <=99999";
        arrPos++;

        arrAttr[arrPos] = "Free Calls – MCUS per month – Exchange capacity >= 100000";
        arrPos++;

        arrAttr[arrPos] = "Free Calls – MCUS per month – Free Talktime";
        arrPos++;

        arrAttr[arrPos] = "Validity ";
        arrPos++;

        arrAttr[arrPos] = "Pay Per Use charges";
        arrPos++;

        arrAttr[arrPos] = "Other Model – Details";
        arrPos++;

        arrAttr[arrPos] = "Benefits";
        arrPos++;

        arrAttr[arrPos] = "Call Rates – Peak Time From, if any";
        arrPos++;

        arrAttr[arrPos] = "Call Rates – Peak Time Till, if any";
        arrPos++;

        arrAttr[arrPos] = "Call Rates – Off Peak Time From, if any";
        arrPos++;

        arrAttr[arrPos] = "Call Rates – Off Peak Time Till, if any";
        arrPos++;

        arrAttr[arrPos] = "All Local Call (Onnet + Offnet + Peak + Offpeak) – Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "All Local Call (Onnet + Offnet + Peak + Offpeak) – Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "All Local Call Rate (Onnet + Offnet + Peak + Offpeak) - Voice Call Rate (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OnNet - Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OnNet - Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OnNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OnNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OnNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OnNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OnNet - Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OffNet - Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OffNet - Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OffNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OffNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OffNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OffNet - Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "Local Call Fixed OnNet - Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Fixed OnNet - Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Fixed OnNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Fixed OnNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Fixed OnNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Fixed OnNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Fixed OnNet -Video Call Rate (off Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Fixed OnNet - Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OffNet - Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OffNet - Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OffNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Rate Mobile OffNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Local Call Mobile OffNet - Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "CUP (Commercial Usage Policy), if any";
        arrPos++;

        arrAttr[arrPos] = "Description of local call rate / pulse rate in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "All STD Call (Onnet + Offnet + Peak + Offpeak) – Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "All STD Call (Onnet + Offnet + Peak + Offpeak) – Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "All STD Call Rate (Onnet + Offnet + Peak + Offpeak) - Voice Call Rate (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "All STD Call Rate (Onnet + Offnet + Peak + Offpeak) - Video Call Rate (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Mobile OnNet - Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Mobile OnNet - Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Mobile OnNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Mobile OnNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Mobile OnNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Mobile OnNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Mobile OnNet - Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "STD Call Mobile OffNet - Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Mobile OffNet - Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Mobile OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Mobile OffNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Mobile OffNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Mobile OffNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Mobile OffNet - Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "STD Call Fixed OnNet - Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Fixed OnNet - Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Fixed OnNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Fixed OnNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Fixed OnNet -Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Fixed OnNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Fixed OnNet - Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "STD Call Fixed OffNet - Voice Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Fixed OffNet - Video Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Fixed OffNet - Voice Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Fixed OffNet - Video Call Rate (Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Fixed OffNet - Voice Call Rate (off peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Rate Fixed OffNet - Video Call Rate (off Peak Hrs) (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "STD Call Fixed OffNet - Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "CUP (Commercial Usage Policy), if any";
        arrPos++;

        arrAttr[arrPos] = "Description of STD Call Rate / pulse rate in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - All Local (onnet +Offnet)";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - All National (onnet +Offnet)";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - Local Onnet";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - Local Offnet";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - National Onnet";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - National Offnet";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - International";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - Terms";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate (in INR/SMS) - Validity";
        arrPos++;

        arrAttr[arrPos] = "CUP (Commercial Usage Policy), if any";
        arrPos++;

        arrAttr[arrPos] = "Description of SMS Rate, in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "ISD call – ISD Countries";
        arrPos++;

        arrAttr[arrPos] = "ISD call - Pulse Rate (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "ISD Call Rate - ISD Calls to Mobile (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "ISD Call Rate - ISD calls to Landline (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "ISD Call Rate - ISD Video calls (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "ISD Call Rate - Weblink for ISD Call Rate";
        arrPos++;

        arrAttr[arrPos] = "ISD Call Rate - Conditions, if any";
        arrPos++;

        arrAttr[arrPos] = "ISD call - Validity (In Days) ";
        arrPos++;

        arrAttr[arrPos] = "CUP (Commercial Usage Policy), if any";
        arrPos++;

        arrAttr[arrPos] = "Description of ISD Call Rate / pulse rate in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Call while Roaming - Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Call Rate while Roaming – Incoming voice (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Call Rate while Roaming - Incoming video (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Call Rate while Roaming - Local Outgoing voice (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Call Rate while Roaming – Local Outgoing video (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Call Rate while Roaming – STD Outgoing voice (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Call Rate while Roaming – STD Outgoing video (in INR/pulse)";
        arrPos++;

        arrAttr[arrPos] = "Call while Roaming – Validity in Days";
        arrPos++;

        arrAttr[arrPos] = "Call Rate while Roaming - Web link for National outgoing ISD Call Rate";
        arrPos++;

        arrAttr[arrPos] = "CUP (Commercial Usage Policy), if any";
        arrPos++;

        arrAttr[arrPos] = "Description of Call Rate while roaming in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate while Roaming - Local (in INR/SMS)";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate while Roaming - National (in INR/SMS)";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate while Roaming - International (in INR/SMS)";
        arrPos++;

        arrAttr[arrPos] = "SMS Rate while Roaming - Any other";
        arrPos++;

        arrAttr[arrPos] = "SMS while Roaming - Validity in days";
        arrPos++;

        arrAttr[arrPos] = "CUP (Commercial Usage Policy), if any";
        arrPos++;

        arrAttr[arrPos] = "Description of SMS Rate / pulse rate in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Countries";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Incoming Pulse (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Rate while International Roaming - ISD Incoming Calls";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Pulse Outgoing Local Calls (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Rate while International Roaming - ISD Outgoing Local Calls";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Pulse Outgoing Calls to India (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Rate while International Roaming - ISD Outgoing Calls to India";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Pulse Outgoing Calls to Other Countries (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Rate while International Roaming - ISD Outgoing Calls to Other Countries";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Video call (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "Rate while International Roaming - ISD Outgoing SMS";
        arrPos++;

        arrAttr[arrPos] = "Rate while International Roaming - ISD Incoming SMS";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Pulse Incoming Free Usage (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Incoming Free Usage (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Pulse Outgoing Free Usage (in seconds)";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Outgoing Free Usage (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Unit Free Data Usage";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Free Data Usage";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - ISD Free SMS";
        arrPos++;

        arrAttr[arrPos] = "While International Roaming - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "CUP (Commercial Usage Policy), if any";
        arrPos++;

        arrAttr[arrPos] = "Description of Rate while International Roaming in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Data Rate – Home";
        arrPos++;

        arrAttr[arrPos] = "Data Rate - Roaming";
        arrPos++;

        arrAttr[arrPos] = "Data Rate - Conditions";
        arrPos++;

        arrAttr[arrPos] = "Monthly Rental for International roaming (in Rs.)";
        arrPos++;

        arrAttr[arrPos] = "Weblink for Rates while international roaming";
        arrPos++;

        arrAttr[arrPos] = "Duration for Additional Benefits -Time From (hh:mm:ss)";
        arrPos++;

        arrAttr[arrPos] = "Duration for Additional Benefits -Time Till (hh:mm:ss)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local (in minutes) - Local (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local (in minutes) - Local Onnet (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local (in minutes) - Local Offnet (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local (in minutes) - Local Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local (in minutes) - Local Onnet Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local (in minutes) - Local Offnet Mobile (in minutes";
        arrPos++;

        arrAttr[arrPos] = "Additional Local (in minutes) - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional Local in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Additional STD (in minutes) - Local (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional STD (in minutes) - Local Onnet (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional STD (in minutes) - Local Offnet (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional STD (in minutes) - Local Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional STD (in minutes) - Local Onnet Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional STD (in minutes) - Local Offnet Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional STD (in minutes) - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional STD in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Additional Local & STD (in minutes) - Local & STD (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local & STD (in minutes) - Local & STD Onnet (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local & STD (in minutes) - Local & STD Offnet (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local & STD (in minutes) - Local & STD Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local & STD (in minutes) - Local & STD Onnet Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local & STD (in minutes) - Local & STD Offnet Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local & STD (in minutes) - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional Local & STD in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Additional Local, STD & Roaming (in minutes) - Local, STD & Roaming (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local, STD & Roaming (in minutes) - Local, STD & Roaming Mobile (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Local, STD & Roaming (in minutes) - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional Local, STD & Roaming in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Additional Roaming (in minutes) - Incoming & Outgoing (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Roaming (in minutes) - Incoming Free (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Roaming (in minutes) - Outgoing Free (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Roaming (in minutes) - Outgoing Local & STD Mobile Free (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Roaming (in minutes) - Outgoing Local Free (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Roaming (in minutes) - Outgoing STD Free (in minutes)";
        arrPos++;

        arrAttr[arrPos] = "Additional Roaming (in minutes) - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional Roaming in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - Local & National";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - Local & National Onnet";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - Local & National Offnet";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - Local";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - Local Onnet";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - Local Offnet";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - National";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - National Onnet";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - National Offnet";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - International";
        arrPos++;

        arrAttr[arrPos] = "Additional SMS - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional SMS in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Additional ISD - Summary of ISD freebies";
        arrPos++;

        arrAttr[arrPos] = "Additional ISD - Weblink for ISD freebies";
        arrPos++;

        arrAttr[arrPos] = "Additional ISD - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional ISD in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Additional Video Call - Local & STD Video";
        arrPos++;

        arrAttr[arrPos] = "Additional Video Call - Local Video";
        arrPos++;

        arrAttr[arrPos] = "Additional Video Call - Local Video Onnet";
        arrPos++;

        arrAttr[arrPos] = "Additional Video Call - Local Video Offnet";
        arrPos++;

        arrAttr[arrPos] = "Additional Video Call - STD Video";
        arrPos++;

        arrAttr[arrPos] = "Additional Video Call - STD Video Onnet";
        arrPos++;

        arrAttr[arrPos] = "Additional Video Call - STD Video Offnet";
        arrPos++;

        arrAttr[arrPos] = "Additional Video Call - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional Video Call in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Unit (MB / GB)";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Total 2G Data";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Total 3G Data";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Total 4G Data";
        arrPos++;

        arrAttr[arrPos] = "Total Data (For ISP / Fixed Line Tariff / Fixed Line Add On Pack)";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Day/Night Data Capping";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Weekly Data Capping";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Monthly Data Capping";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Carry Forward (Yes / No)";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Validity (In Days)";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - FUP, if any (Yes/No)";
        arrPos++;

        arrAttr[arrPos] = "Additional Data - Condition, if any";
        arrPos++;

        arrAttr[arrPos] = "Description of Additional Data in case any other pattern is followed";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous -Special benefits , if any";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Other charges, if any";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Remarks, if any";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Terms and Conditions, if any";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Mode of Activation / Recharge";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Whether details of this service have been uploaded on the website";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - TSP website link of the Plan";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Any other declarations";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Condition for termination of Product if any";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Activation Code If Any";
        arrPos++;

        arrAttr[arrPos] = "Miscellaneous - Deactivation Code If Any";
        arrPos++;

        arrAttr[arrPos] = "Delayed Submission";
        arrPos++;

        arrAttr[arrPos] = "Reason for delay";
        arrPos++;

        for (int i = 4; i < arrSize + 4; i++)
        {
            arrType[i - 4] = "Numeric";
            if (i <= 15 || i == 18 || i == 21 || i == 24 || i == 52 || i == 53 || i == 54 || i == 91 || i == 92 || i == 125 || i == 126 || i == 134 || i == 136 || i == 137 || i == 138 || i == 143 || i == 144 || i == 146 || i == 147 || i == 156 || i == 157 || i == 158 || i == 164 || i == 165 || i == 166 || i == 182 || i == 186 || i == 187 || i == 188 || i == 189 || i == 190 || i == 192 || i == 202 || i == 210 || i == 218 || i == 222 || i == 230 || i == 242 || i == 243 || i == 244 || i == 246 || i == 255 || i == 256 || i == 264 || i == 266 || i == 267 || i == 268 || i == 269 || i == 270 || i == 271 || i == 272 || i == 273 || i == 274 || i == 275 || i == 276 || i == 277 || i == 278 || i == 279 || i == 280 || i == 281)
            {
                arrType[i - 4] = "Text";
            }
            if (i == 16 || i == 17 || i == 19 || i == 20)
            {
                arrType[i - 4] = "Date";
            }
        }

        int colpos = 0;
        com = new MySqlCommand("SELECT column_name FROM information_schema.columns WHERE table_name='TRAI_tariffs'", con);
        con.Open();
        dr = com.ExecuteReader();
        while (dr.Read())
        {
            if (colpos >= 4 && colpos < 282)
            {
                arrFlds[colpos - 4] = dr[0].ToString().Trim();
            }
            colpos++;
        }
        con.Close();


        // set the array for 'Attribute Key' column values - CODE ENDS HERE //


        if (!IsPostBack)
        {
            DropNumeric.Items.Add("");
            DropText.Items.Add("");
            DropDate.Items.Add("");
            int cntrnumeric = 1, cntrtext = 1, cntrdate = 1;
            for (int i = 0; i < arrSize; i++)
            {
                if (arrType[i] == "Numeric")
                {
                    DropNumeric.Items.Add(arrAttr[i].ToString().Trim());
                    DropNumeric.Items[cntrnumeric].Value = (i).ToString();
                    cntrnumeric++;
                }
                if (arrType[i] == "Text")
                {
                    DropText.Items.Add(arrAttr[i].ToString().Trim());
                    DropText.Items[cntrtext].Value = (i).ToString();
                    cntrtext++;
                }
                if (arrType[i] == "Date")
                {
                    DropDate.Items.Add(arrAttr[i].ToString().Trim());
                    DropDate.Items[cntrdate].Value = (i).ToString();
                    cntrdate++;
                }
            }
            if (!string.IsNullOrEmpty(Request.QueryString["radOtype"]))
            {
                RadOType.SelectedValue = Request.QueryString["radOtype"];
            }
            LoadOperators(null, null);

            com = new MySqlCommand("select * from TRAI_circles  order by circ", con);
            con.Open();
            dr = com.ExecuteReader();

            int j = 0;
            while (dr.Read())
            {
                ChkCirc.Items.Add(dr["circ"].ToString().Trim());
                if (!string.IsNullOrEmpty(Request.QueryString["conditions"]) && Request.QueryString["conditions"].Contains(dr["circ"].ToString().Trim()))
                {
                    ChkCirc.Items[j].Selected = true;
                }
                j++;
            }
            con.Close();

            com = new MySqlCommand("select * from TRAI_ttypes order by rno", con);
            con.Open();
            dr = com.ExecuteReader();
            int k = 0;
            while (dr.Read())
            {
                ChkTtype.Items.Add(dr["ttype"].ToString().Trim());
                if (!string.IsNullOrEmpty(Request.QueryString["conditions"]) && Request.QueryString["conditions"].Contains(dr["ttype"].ToString().Trim()))
                {
                    ChkTtype.Items[k].Selected = true;
                }
                k++;
            }
            con.Close();

            if (!string.IsNullOrEmpty(Request.QueryString["pn"]) && !string.IsNullOrEmpty(Request.QueryString["conditions"]) && !string.IsNullOrEmpty(Request.QueryString["radActive"]))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["radActive"]))
                {
                    RadActive.SelectedValue  = Request.QueryString["radActive"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["radCondition"]))
                {
                    RadCondition.SelectedValue = Request.QueryString["radCondition"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["strNumerics"]))
                {
                    string[] arr = Request.QueryString["strNumerics"].Split('$');
                    TextPtrNumeric.Text = (arr.Length - 1).ToString();
                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        string[] arr1 = arr[i].Split('#');
                        arrConditionsNumeric[i].Text = arr1[0].Trim();     // Text of selected parameter
                        arrValNumeric[i].Text = arr1[1].Trim();            // value of selected parameter = column number in TRAI_tariffs table
                        arrNumFig1[i].Text = arr1[2].Trim();               // entered value 1 by user
                        arrNumFig2[i].Text = arr1[3].Trim();
                    }
                }

                if (!string.IsNullOrEmpty(Request.QueryString["strTexts"]))
                {
                    string[] arr = Request.QueryString["strTexts"].Split('$');
                    TextPtrText.Text = (arr.Length - 1).ToString();
                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        string[] arr1 = arr[i].Split('#');

                        arrConditionsText[i].Text = arr1[0].Trim();
                        arrValText[i].Text = arr1[1].Trim();
                        arrTextFig1[i].Text = arr1[2].Trim();
                    }
                }                

                if (!string.IsNullOrEmpty(Request.QueryString["strDates"]))
                {
                    string[] arr = Request.QueryString["strDates"].Split('$');
                    TextPtrDate.Text = (arr.Length - 1).ToString();
                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        string[] arr1 = arr[i].Split('#');
                        arrConditionsDate[i].Text = arr1[0].Trim();         // Text of selected parameter
                        arrValDate[i].Text = arr1[1].Trim();              // value of selected parameter = column number in TRAI_tariffs table
                        arrDateFig1[i].Text = arr1[2].Trim();             // entered value 1 by user
                        arrDateFig2[i].Text = arr1[3].Trim();                        
                    }
                }
                showConditions(null, null);
                showRecords(null, null);
            }
        }
    }

    protected void LoadOperators(object sender, EventArgs e)
    {
        try
        {
            ChkOper.Items.Clear();
            ChkAllOperators.Checked = false;
            if (RadOType.SelectedItem.Text.Trim() == "Both")
            {
                com = new MySqlCommand("select * from TRAI_operators order by oper", con);
            }
            else
            {
                if (RadOType.SelectedItem.Text.Trim() == "TSP")
                {
                    com = new MySqlCommand("select * from TRAI_operators where (upper(oper)='AIRCEL' or upper(oper)='AIRTEL' or upper(oper)='BSNL' or upper(oper)='IDEA' or upper(oper)='JIO' or upper(oper)='MTNL' or upper(oper)='QUADRANT (CONNECT)' or upper(oper)='TATA TELE' or upper(oper)='TELENOR' or upper(oper)='VODAFONE' or upper(oper)='VODAFONE IDEA' or upper(oper)='SURFTELECOM' or upper(oper)='AEROVOYCE') order by oper", con);
                }
                else
                {
                    com = new MySqlCommand("select * from TRAI_operators where (upper(oper)!='AIRCEL' and upper(oper)!='AIRTEL' and upper(oper)!='BSNL' and upper(oper)!='IDEA' and upper(oper)!='JIO' and upper(oper)!='MTNL' and upper(oper)!='QUADRANT (CONNECT)' and upper(oper)!='TATA TELE' and upper(oper)!='TELENOR' and upper(oper)!='VODAFONE' and upper(oper)!='VODAFONE IDEA' and upper(oper)!='SURFTELECOM' and upper(oper)!='AEROVOYCE') order by oper", con);
                }
            }
            con.Open();
            dr = com.ExecuteReader();
            int j = 0;
            while (dr.Read())
            {
                ChkOper.Items.Add(dr["oper"].ToString().Trim());
                if (!string.IsNullOrEmpty(Request.QueryString["conditions"]) && Request.QueryString["conditions"].Contains(dr["oper"].ToString().Trim()))
                {
                    ChkOper.Items[j].Selected = true;
                }
                j++;
            }
            con.Close();
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
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
            //double mrp1 = 1, mrp2 = 999999;
            //double mrp1 = 0.01, mrp2 = 999999;
            double mrp1 = -1, mrp2 = 999999;

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


            string conditionop = " OR ";
            if (RadCondition.SelectedItem.Text.Trim() == "Tariffs Matching All Conditions")
            {
                conditionop = " AND ";
            }


            // Numeric Parameters //
            conditions = conditions + " and (";
            int numconditions = 0;
            int ptr1 = 0;
            try
            {
                ptr1 = Convert.ToInt32(TextPtrNumeric.Text.Trim());
            }
            catch (Exception ex) { }
            for (int i = 0; i < ptr1; i++)
            {
                if (arrConditionsNumeric[i].Text.Trim() != "")
                {
                    try
                    {
                        int ind = Convert.ToInt32(arrValNumeric[i].Text.Trim());
                        conditions = conditions + "(" + arrFlds[ind].ToString().Trim() + " >= " + Convert.ToDouble(arrNumFig1[i].Text.Trim()) + " and " + arrFlds[ind].ToString().Trim() + " <= " + Convert.ToDouble(arrNumFig2[i].Text.Trim()) + ") " + conditionop;
                        numconditions++;
                    }
                    catch (Exception ex) { throw; }
                }
            }
            if (numconditions > 0)
            {
                conditions = conditions.Substring(0, conditions.Length - conditionop.Length);   // remove trailing  AND / OR
            }
            conditions = conditions + ")";
            conditions = conditions.Replace("and ()", "");
            conditions = conditions.Replace("()", "");


            // Text Parameters //
            conditions = conditions + " and (";
            int txtconditions = 0;
            int ptr2 = 0;
            try
            {
                ptr2 = Convert.ToInt32(TextPtrText.Text.Trim());
            }
            catch (Exception ex) { }
            for (int i = 0; i < ptr2; i++)
            {
                if (arrConditionsText[i].Text.Trim() != "")
                {
                    try
                    {
                        int ind = Convert.ToInt32(arrValText[i].Text.Trim());
                        conditions = conditions + "(" + arrFlds[ind].ToString().Trim() + " like '%" + arrTextFig1[i].Text.Trim() + "%') " + conditionop;
                        txtconditions++;
                    }
                    catch (Exception ex) {}
                }
            }
            if (txtconditions > 0)
            {
                conditions = conditions.Substring(0, conditions.Length - conditionop.Length);   // remove trailing  AND / OR
            }
            conditions = conditions + ")";
            conditions = conditions.Replace("and ()", "");
            conditions = conditions.Replace("()", "");


            // Date Parameters //
            conditions = conditions + " and (";
            int dtconditions = 0;
            int ptr3 = 0;
            try
            {
                ptr3 = Convert.ToInt32(TextPtrDate.Text.Trim());
            }
            catch (Exception ex) { }
            for (int i = 0; i < ptr3; i++)
            {
                if (arrConditionsDate[i].Text.Trim() != "")
                {
                    try
                    {
                        int ind = Convert.ToInt32(arrValDate[i].Text.Trim());
                        conditions = conditions + "(" + arrFlds[ind].ToString().Trim() + " >= '" + Convert.ToDateTime(arrDateFig1[i].Text.Trim()).ToString("yyyy-MM-dd") + "' and " + arrFlds[ind].ToString().Trim() + " < '" + Convert.ToDateTime(arrDateFig2[i].Text.Trim()).AddDays(1).ToString("yyyy-MM-dd") + "') " + conditionop;
                        dtconditions++;
                    }
                    catch (Exception ex) { }
                }
            }
            if (dtconditions > 0)
            {
                conditions = conditions.Substring(0, conditions.Length - conditionop.Length);   // remove trailing  AND / OR
            }
            conditions = conditions + ")";
            conditions = conditions.Replace("and ()", "");
            conditions = conditions.Replace("()", "");

            if (flag == 0)
            {
                TextConditions.Text = conditions;
                TextPage.Text = "1";
                showRecords(null, null);
            }

        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    protected void showRecords(object s, EventArgs e)
    {
        try
        {
            int pageNo = !string.IsNullOrEmpty(TextPage.Text.Trim()) ? Convert.ToInt32(TextPage.Text.Trim()) : Convert.ToInt32(Request.QueryString["pn"]);
            int PageSize = 50;
            Int64 TotalRecords = 0;

            string conditions = Request.QueryString["conditions"] == null ? TextConditions.Text.Trim() : WebUtility.HtmlDecode(Request.QueryString["conditions"]);
            string radActive = Request.QueryString["radActive"] == null ? RadActive.SelectedItem.Text.Trim() : Request.QueryString["radActive"];
            string user = Request["user"].ToString().Trim();

            string sortby = TextSortBy.Text.Trim();
            if (sortby == "")
            {
                sortby = " order by reportdate";
            }

            string strNumerics = string.Empty, strTexts = string.Empty, strDates = string.Empty;


            int ptr1 = 0;
            try
            {
                ptr1 = Convert.ToInt32(TextPtrNumeric.Text.Trim());
            }
            catch (Exception ex) { }
            for (int i = 0; i < ptr1; i++)
            {
                if (arrConditionsNumeric[i].Text.Trim() != "")
                {
                    try
                    {
                      strNumerics = strNumerics + arrConditionsNumeric[i].Text.Trim() + "#" + Convert.ToInt32(arrValNumeric[i].Text.Trim()) + "#" + Convert.ToDouble(arrNumFig1[i].Text.Trim()) + "#" + Convert.ToDouble(arrNumFig2[i].Text.Trim()) + "$";
                    }
                    catch (Exception ex) { }
                }
            }

            int ptr2 = 0;
            try
            {
                ptr2 = Convert.ToInt32(TextPtrText.Text.Trim());
            }
            catch (Exception ex) { }
            for (int i = 0; i < ptr2; i++)
            {
                if (arrConditionsText[i].Text.Trim() != "")
                {
                    try
                    {
                        strTexts = strTexts + arrConditionsText[i].Text.Trim() + "#" + Convert.ToInt32(arrValText[i].Text.Trim()) + "#" + arrTextFig1 + "$";
                    }
                    catch (Exception ex) { }
                }
            }

            int ptr3 = 0;
            try
            {
                ptr3 = Convert.ToInt32(TextPtrDate.Text.Trim());
            }
            catch (Exception ex) { }
            for (int i = 0; i < ptr3; i++)
            {
                if (arrConditionsDate[i].Text.Trim() != "")
                {
                    try
                    {
                        strDates = strDates + arrConditionsDate[i].Text.Trim() + "#" + Convert.ToInt32(arrValDate[i].Text.Trim()) + "#" + Convert.ToDateTime(arrDateFig1[i].Text.Trim()).ToString("yyyy-MM-dd") + "#" + Convert.ToDateTime(arrDateFig2[i].Text.Trim()).ToString("yyyy-MM-dd") + "$";
                    }
                    catch (Exception ex) { }
                }
            }

            string tablename = "";

            getMaxRno("repno", "TRAI_tempReporting");
            int repno = zno;
            int cntr = 1;


            string myqry = "";

            if (radActive == "Active")// || radActive == "Both"
            {

                tablename = "TRAI_tariffs";

                com = new MySqlCommand("select Count(1) As Total from " + tablename + " where(rno>0) and (actiontotake<>'WITHDRAWAL') and " + conditions, con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    Int64.TryParse(dr["Total"].ToString().Trim(), out TotalRecords);
                }
                catch (Exception ex) { }
                con.Close();

                myqry = "select *, GetDelay(actiondate, reportdate) as Delay, Case When(Select Count(1) from TRAI_tariffreviewlog) > 0 then 'Yes' else 'No' end as takenonrecord, " +
                        "Case When(UPPER(categ)='PREPAID') then mrp else ISP_rental end as mrp1, 'Active' as stat from TRAI_tariffs where (rno > 0) and (actiontotake<>'WITHDRAWAL') and  " + conditions + " " + sortby; ;
                if (pageNo > 1)
                {
                    myqry = myqry + " LIMIT " + (pageNo - 1) * PageSize + "," + PageSize;
                }
                else
                {
                    myqry = myqry + " LIMIT " + PageSize;
                }
            }

            if (radActive == "Withdrawn")
            {
                tablename = "TRAI_archive";

                com = new MySqlCommand("select Count(1) As Total from " + tablename + " where(rno>0) and (actiontotake='WITHDRAWAL') and " + conditions, con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    Int64.TryParse(dr["Total"].ToString().Trim(), out TotalRecords);
                }
                catch (Exception ex) { }
                con.Close();

                myqry = "select *, GetDelay(actiondate, reportdate) as Delay, Case When(Select Count(1) from TRAI_tariffreviewlog) > 0 then 'Yes' else 'No' end as takenonrecord, " +
                        "Case When(UPPER(categ)='PREPAID') then mrp else ISP_rental end as mrp1, 'Withdrawn' as stat from TRAI_archive where (rno > 0) and (actiontotake<>'WITHDRAWAL') and  " + conditions + " " + sortby; ;
                if (pageNo > 1)
                {
                    myqry = myqry + " LIMIT " + (pageNo - 1) * PageSize + "," + PageSize;
                }
                else
                {
                    myqry = myqry + " LIMIT " + PageSize;
                }
            }
            if (radActive == "Both")
            {
                myqry = "Select sum(Total) As 'Total' from (select count(*) As Total from TRAI_tariffs where(rno>0) and (actiontotake<>'WITHDRAWAL') and " + conditions + " UNION " +
                        "select count(*) AS Total from TRAI_archive where (rno > 0) and (actiontotake='WITHDRAWAL') and " + conditions + " ) as TotalRecordCount";
                com = new MySqlCommand(myqry, con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    Int64.TryParse(dr["Total"].ToString().Trim(), out TotalRecords);
                }
                catch (Exception ex) { }
                con.Close();

                myqry = "Select * from (select *,'' as Archiveno,'' as Archivedate, GetDelay(actiondate, reportdate) as Delay, Case When(Select Count(1) from TRAI_tariffreviewlog) > 0 then 'Yes' else 'No' end as takenonrecord, " +
                    "Case When(UPPER(categ)='PREPAID') then mrp else ISP_rental end as mrp1, 'Active' as stat from TRAI_tariffs where (rno > 0) and (actiontotake<>'WITHDRAWAL') and " + conditions;
                myqry = myqry + " UNION ";
                myqry = myqry + " select *, GetDelay(actiondate, reportdate) as Delay, Case When(Select Count(1) from TRAI_tariffreviewlog) > 0 then 'Yes' else 'No' end as takenonrecord, " +
                    "Case When(UPPER(categ)='PREPAID') then mrp else ISP_rental end as mrp1, 'Not-Active' as stat from TRAI_archive where (rno > 0) and (actiontotake='WITHDRAWAL') and " + conditions + " ) As A " + sortby;
                if (pageNo > 1)
                {
                    myqry = myqry + " LIMIT " + (pageNo - 1) * PageSize + "," + PageSize;
                }
                else
                {
                    myqry = myqry + " LIMIT " + PageSize;
                }
            }

            com = new MySqlCommand(myqry, con);
            MySqlDataAdapter ada = new MySqlDataAdapter(com);
            DataSet ds = new DataSet();
            ada.Fill(ds);

            con.Close();


            // Display the records //
            //drawTable(repno, sortby, user);
            drawTable(ds, user);

            string strPaging = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strPaging = Set_Paging(pageNo, PageSize, TotalRecords, "activeLink", "FEA_repadvance.aspx", "disableLink", WebUtility.HtmlEncode(conditions), RadOType.SelectedItem.Text.Trim(), radActive, RadCondition.SelectedItem.Text.Trim(), user, HttpUtility.UrlEncode(strNumerics), HttpUtility.UrlEncode(strTexts), HttpUtility.UrlEncode(strDates));
            }
                pagingDiv1.InnerHtml = strPaging;
            pagingDiv2.InnerHtml = strPaging;
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
            con.Close();
        }
    }
    protected void drawTable(DataSet ds, string user)
    {
        try
        {
            tbresults = new Table();
            tbresults.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            tbresults.CellPadding = 5;
            tbresults.CellSpacing = 1;
            tbresults.BorderWidth = 0;

            TableRow tra = new TableRow();
            TableCell tca1 = new TableCell();
            TableCell tca2 = new TableCell();
            tca1.ColumnSpan = 11;
            tca2.ColumnSpan = 2;
            tca1.CssClass = "tablecell";
            tca2.CssClass = "tablecell";
            tca1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tca2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            tca2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Bottom;
            //tca1.Text = "<a href=javascript:funCompareSend() ><img src=images/btncomp.jpg width=100 border=0 /></a>";
            tca1.Text = "";
            tca2.Text = "<a href=javascript:funExcel() ><img src=images/excel.jpg border=0 /></a>";
            tra.Controls.Add(tca1);
            tra.Controls.Add(tca2);
            tbresults.Controls.Add(tra);


            TableRow tr = new TableRow();
            TableCell tc0 = new TableCell();
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            TableCell tc5 = new TableCell();
            TableCell tc6 = new TableCell();
            TableCell tc7 = new TableCell();
            TableCell tc8 = new TableCell();
            TableCell tc9 = new TableCell();
            TableCell tc10 = new TableCell();
            TableCell tc11 = new TableCell();
            TableCell tc12 = new TableCell();
            tc0.Width = System.Web.UI.WebControls.Unit.Percentage(3);
            tc1.Width = System.Web.UI.WebControls.Unit.Percentage(10);
            tc2.Width = System.Web.UI.WebControls.Unit.Percentage(8);
            tc3.Width = System.Web.UI.WebControls.Unit.Percentage(12);
            tc4.Width = System.Web.UI.WebControls.Unit.Percentage(10);
            tc5.Width = System.Web.UI.WebControls.Unit.Percentage(7);
            tc6.Width = System.Web.UI.WebControls.Unit.Percentage(6);
            tc7.Width = System.Web.UI.WebControls.Unit.Percentage(7);
            tc8.Width = System.Web.UI.WebControls.Unit.Percentage(9);
            tc9.Width = System.Web.UI.WebControls.Unit.Percentage(10);
            tc10.Width = System.Web.UI.WebControls.Unit.Percentage(6);
            tc11.Width = System.Web.UI.WebControls.Unit.Percentage(6);
            tc12.Width = System.Web.UI.WebControls.Unit.Percentage(6);
            tc0.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc8.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc9.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc10.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc11.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc12.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tc0.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc1.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc3.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc4.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc5.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc6.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc7.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc8.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc9.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc10.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc11.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            tc12.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
            string css = "tablecell3";
            tc0.CssClass = css;
            tc1.CssClass = css;
            tc2.CssClass = css;
            tc3.CssClass = css;
            tc4.CssClass = css;
            tc5.CssClass = css;
            tc6.CssClass = css;
            tc7.CssClass = css;
            tc8.CssClass = css;
            tc9.CssClass = css;
            tc10.CssClass = css;
            tc11.CssClass = css;
            tc12.CssClass = css;
            tc0.Text = "";
            tc1.Text = "TSP<br /><br /><a href=javascript:funsort('oper','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('oper','desc');><img src=images/sortdown.png border=0></a>";
            tc2.Text = "LSA<br /><br /><a href=javascript:funsort('circ','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('circ','desc');><img src=images/sortdown.png border=0></a>";
            tc3.Text = "Tariff Type<br /><br /><a href=javascript:funsort('ttype','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('ttype','desc');><img src=images/sortdown.png border=0></a>";
            tc4.Text = "Unique Record ID<br /><a href=javascript:funsort('uniqueid','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('uniqueid','desc');><img src=images/sortdown.png border=0></a>";
            tc5.Text = "Service Type<br /><a href=javascript:funsort('service','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('service','desc');><img src=images/sortdown.png border=0></a>";
            tc6.Text = "Price &#8377;<br /><br /><a href=javascript:funsort('mrp','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=images/sortdown.png border=0></a>";
            tc7.Text = "Validity<br /><br /><a href=javascript:funsort('validity','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('validity','desc');><img src=images/sortdown.png border=0></a>";
            tc8.Text = "Date of Launch / Revision / Correction / Withdrawal<br /><a href=javascript:funsort('actiondate','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('actiondate','desc');><img src=images/sortdown.png border=0></a>";
            tc9.Text = "Date of Reporting<br /><a href=javascript:funsort('reportdate','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('reportdate','desc');><img src=images/sortdown.png border=0></a>";
            tc10.Text = "Delayed Reporting";
            tc11.Text = "Taken on Record";
            tc12.Text = "Status";
            //tr.Controls.Add(tc0);     // Check box column - Not required in this report
            tr.Controls.Add(tc1);
            tr.Controls.Add(tc2);
            tr.Controls.Add(tc3);
            tr.Controls.Add(tc4);
            tr.Controls.Add(tc5);
            tr.Controls.Add(tc6);
            tr.Controls.Add(tc7);
            tr.Controls.Add(tc8);
            tr.Controls.Add(tc9);
            tr.Controls.Add(tc10);
            tr.Controls.Add(tc11);
            tr.Controls.Add(tc12);
            tbresults.Controls.Add(tr);

            int chkpos = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TableRow trr = new TableRow();
                TableCell tcc0 = new TableCell();
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
                TableCell tcc11 = new TableCell();
                TableCell tcc12 = new TableCell();
                tcc0.Width = System.Web.UI.WebControls.Unit.Percentage(3);
                tcc1.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                tcc2.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tcc3.Width = System.Web.UI.WebControls.Unit.Percentage(12);
                tcc4.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                tcc5.Width = System.Web.UI.WebControls.Unit.Percentage(7);
                tcc6.Width = System.Web.UI.WebControls.Unit.Percentage(6);
                tcc7.Width = System.Web.UI.WebControls.Unit.Percentage(7);
                tcc8.Width = System.Web.UI.WebControls.Unit.Percentage(9);
                tcc9.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                tcc10.Width = System.Web.UI.WebControls.Unit.Percentage(6);
                tcc11.Width = System.Web.UI.WebControls.Unit.Percentage(6);
                tcc12.Width = System.Web.UI.WebControls.Unit.Percentage(6);
                tcc0.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tcc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc8.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc9.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc10.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc11.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc12.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                string css2 = "tablecell5b";
                tcc0.CssClass = css2;
                tcc1.CssClass = css2;
                tcc2.CssClass = css2;
                tcc3.CssClass = css2;
                tcc4.CssClass = css2;
                tcc5.CssClass = css2;
                tcc6.CssClass = css2;
                tcc7.CssClass = css2;
                tcc8.CssClass = css2;
                tcc9.CssClass = css2;
                tcc10.CssClass = css2;
                tcc11.CssClass = css2;
                tcc12.CssClass = css2;
                arrResult[chkpos].ID = dr["ttype"].ToString().Trim() + "~" + dr["uniqueid"].ToString().Trim();
                arrResult[chkpos].Attributes.Add("onchange", "funComparison('" + dr["ttype"].ToString().Trim() + "','" + arrResult[chkpos].ID.ToString() + "')");
                tcc0.Controls.Add(arrResult[chkpos]);
                tcc1.Text = dr["oper"].ToString().Trim();
                tcc2.Text = dr["circ"].ToString().Trim();
                tcc3.Text = dr["ttype"].ToString().Trim();
                tcc4.Text = dr["uniqueid"].ToString().Trim();
                tcc5.Text = dr["service"].ToString().Trim();
                tcc6.Text = Math.Round(Convert.ToDouble(dr["mrp"].ToString().Trim())).ToString();
                tcc7.Text = Math.Round(Convert.ToDouble(dr["validity"].ToString().Trim())).ToString().Replace("-1", "").Replace("-2", "Unlimited");
                tcc8.Text = Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy");
                tcc9.Text = Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy");
                tcc10.Text = dr["delay"].ToString().Trim();
                tcc11.Text = dr["takenonrecord"].ToString().Trim();
                tcc12.Text = dr["stat"].ToString().Trim();
                //trr.Controls.Add(tcc0);       // Check box column - Not required in this report
                trr.Controls.Add(tcc1);
                trr.Controls.Add(tcc2);
                trr.Controls.Add(tcc3);
                trr.Controls.Add(tcc4);
                trr.Controls.Add(tcc5);
                trr.Controls.Add(tcc6);
                trr.Controls.Add(tcc7);
                trr.Controls.Add(tcc8);
                trr.Controls.Add(tcc9);
                trr.Controls.Add(tcc10);
                trr.Controls.Add(tcc11);
                trr.Controls.Add(tcc12);
                tbresults.Controls.Add(trr);

                TableRow trr2 = new TableRow();
                TableCell tcd1 = new TableCell();
                TableCell tcd2 = new TableCell();
                //tcd1.ColumnSpan = 7;
                tcd1.ColumnSpan = 6;
                tcd2.ColumnSpan = 6;
                tcd1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tcd2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd1.CssClass = "tablecell5c";
                tcd2.CssClass = "tablecell5c";
                tcd1.Text = "<b>Tariff Summary : </b> " + dr["tariffdet"].ToString().Trim().Replace("~", "&quot;").Replace("&amp;", "&");
                string det = "";
                det = det + "<a href='FEA_recorddetails.aspx?user=" + user + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Tariff Details</u></b></a>";
                det = det + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                det = det + "<a href='FEA_reviewsummary.aspx?user=" + user + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Review Details</u></b></a>";
                tcd2.Text = det;
                trr2.Controls.Add(tcd1);
                trr2.Controls.Add(tcd2);
                tbresults.Controls.Add(trr2);

                TableRow trr3 = new TableRow();
                TableCell tce1 = new TableCell();
                tce1.CssClass = "tablecell3b";
                tce1.ColumnSpan = 13;
                tce1.Text = "<hr size=0>";
                trr3.Controls.Add(tce1);
                tbresults.Controls.Add(trr3);

                chkpos++;

            }
            con.Close();


            TableRow trb = new TableRow();
            TableCell tcb1 = new TableCell();
            TableCell tcb2 = new TableCell();
            //tcb1.ColumnSpan = 11;
            tcb1.ColumnSpan = 10;
            tcb2.ColumnSpan = 2;
            tcb1.CssClass = "tablecell";
            tcb2.CssClass = "tablecell";
            tcb1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tcb2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            tcb2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Bottom;
            //tcb1.Text = "<a href=javascript:funCompareSend() ><img src=images/btncomp.jpg width=100 border=0 /></a>";
            tcb1.Text = "";
            tcb2.Text = "<a href=javascript:funExcel() ><img src=images/excel.jpg border=0 /></a>";
            trb.Controls.Add(tcb1);
            trb.Controls.Add(tcb2);
            tbresults.Controls.Add(trb);


            /*
            strexcel = strexcel + "</table>";
            divExcel.InnerHtml = strexcel;
            */


            divresult.Controls.Add(tbresults);


            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }
    public string Set_Paging(Int32 PageNumber, int PageSize, Int64 TotalRecords, string ClassName, string PageUrl, string DisableClassName, string conditions, string radOtype, string radActive, string radCondition, string user, string strNumerics, string strTexts, string strDates)
    {
        string ReturnValue = "";
        try
        {
            Int64 TotalPages = Convert.ToInt64(Math.Ceiling((double)TotalRecords / PageSize));
            if (PageNumber > 1)
            {
                if (PageNumber == 2)
                    ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim() + "?pn=" + Convert.ToString(PageNumber - 1) + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&radCondition=" + radCondition + "&strNumerics=" + strNumerics + "&strTexts=" + strTexts + "&strDates=" + strDates + "' class='" + ClassName + "'>Previous</a>&nbsp;&nbsp;&nbsp;";
                else
                {
                    ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim();
                    if (PageUrl.Contains("?"))
                        ReturnValue = ReturnValue + "&";
                    else
                        ReturnValue = ReturnValue + "?";
                    ReturnValue = ReturnValue + "pn=" + Convert.ToString(PageNumber - 1) + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&radCondition=" + radCondition + "&strNumerics=" + strNumerics + "&strTexts=" + strTexts + "&strDates=" + strDates + "' class='" + ClassName + "'>Previous</a>&nbsp;&nbsp;&nbsp;";
                }
            }
            else
                ReturnValue = ReturnValue + "<span class='" + DisableClassName + "'>Previous</span>&nbsp;&nbsp;&nbsp;";
            if ((PageNumber - 3) > 1)
                ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim() + "?pn=" + Convert.ToString(PageNumber) + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&radCondition=" + radCondition + "&strNumerics=" + strNumerics + "&strTexts=" + strTexts + "&strDates=" + strDates + "' class='" + ClassName + "'>1</a>&nbsp;.....&nbsp;|&nbsp;";

            for (int i = PageNumber - 3; i <= PageNumber; i++)
                if (i >= 1)
                {
                    if (PageNumber != i)
                    {
                        ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim();
                        if (PageUrl.Contains("?"))
                            ReturnValue = ReturnValue + "&";
                        else
                            ReturnValue = ReturnValue + "?";
                        ReturnValue = ReturnValue + "pn=" + i.ToString() + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&radCondition=" + radCondition + "&strNumerics=" + strNumerics + "&strTexts=" + strTexts + "&strDates=" + strDates + "' class='" + ClassName + "'>" + i.ToString() + "</a>&nbsp;|&nbsp;";
                    }
                    else
                    {
                        ReturnValue = ReturnValue + "<span style='font-weight:bold;'>" + i + "</span>&nbsp;|&nbsp;";
                    }
                }
            for (int i = PageNumber + 1; i <= PageNumber + 3; i++)
                if (i <= TotalPages)
                {
                    if (PageNumber != i)
                    {
                        ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim();
                        if (PageUrl.Contains("?"))
                            ReturnValue = ReturnValue + "&";
                        else
                            ReturnValue = ReturnValue + "?";
                        ReturnValue = ReturnValue + "pn=" + i.ToString() + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&radCondition=" + radCondition + "&strNumerics=" + strNumerics + "&strTexts=" + strTexts + "&strDates=" + strDates + "' class='" + ClassName + "'>" + i.ToString() + "</a>&nbsp;|&nbsp;";
                    }
                    else
                    {
                        ReturnValue = ReturnValue + "<span style='font-weight:bold;'>" + i + "</span>&nbsp;|&nbsp;";
                    }
                }
            if ((PageNumber + 3) < TotalPages)
            {
                ReturnValue = ReturnValue + ".....&nbsp;<a href='" + PageUrl.Trim();
                if (PageUrl.Contains("?"))
                    ReturnValue = ReturnValue + "&";
                else
                    ReturnValue = ReturnValue + "?";
                ReturnValue = ReturnValue + "pn=" + TotalPages.ToString() + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&radCondition=" + radCondition + "&strNumerics=" + strNumerics + "&strTexts=" + strTexts + "&strDates=" + strDates + "' class='" + ClassName + "'>" + TotalPages.ToString() + "</a>";
            }
            if (PageNumber < TotalPages)
            {
                ReturnValue = ReturnValue + "&nbsp;&nbsp;&nbsp;<a href='" + PageUrl.Trim();
                if (PageUrl.Contains("?"))
                    ReturnValue = ReturnValue + "&";
                else
                    ReturnValue = ReturnValue + "?";
                ReturnValue = ReturnValue + "pn=" + Convert.ToString(PageNumber + 1) + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&radCondition=" + radCondition + "&strNumerics=" + strNumerics + "&strTexts=" + strTexts + "&strDates=" + strDates + "' class='" + ClassName + "'>Next</a>";
            }
            else
                ReturnValue = ReturnValue + "&nbsp;&nbsp;&nbsp;<span class='" + DisableClassName + "'>Next</span>";
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
        return (ReturnValue);
    }
    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        try
        {
            //double myrepno = Convert.ToDouble(TextExcel.Text.Trim());

            string mystr = "<table width=100% border=1 cellspacing=1 cellpadding=5 style=border-collapse:collapse; >";
            mystr = mystr + "<tr>";
            mystr = mystr + "<td width=150 class=tablehead align=center valign=top><b>Active / Withdrawn</b></td>";
            for (int i = 0; i < arrSize; i++)
            {
                mystr = mystr + "<td width=150 class=tablehead align=center valign=top><b>" + arrAttr[i].ToString().Trim() + "</b></td>";
            }
            mystr = mystr + "</tr>";

            string conditions = Request.QueryString["conditions"] == null ? TextConditions.Text.Trim() : WebUtility.HtmlDecode(Request.QueryString["conditions"]);
            string radActive = Request.QueryString["radActive"] == null ? RadActive.SelectedItem.Text.Trim() : Request.QueryString["radActive"];
            string user = Request["user"].ToString().Trim();

            string sortby = TextSortBy.Text.Trim();
            if (sortby == "")
            {
                sortby = " order by reportdate";
            }

            string myqry = "";

            if (radActive == "Active")// || radActive == "Both"
            {
                myqry = "select *, GetDelay(actiondate, reportdate) as Delay, Case When(Select Count(1) from TRAI_tariffreviewlog) > 0 then 'Yes' else 'No' end as takenonrecord, " +
                        "Case When(UPPER(categ)='PREPAID') then mrp else ISP_rental end as mrp1, 'Active' as stat from TRAI_tariffs where (rno > 0) and (actiontotake<>'WITHDRAWAL') and  " + conditions + " " + sortby; ;
                }

            if (radActive == "Withdrawn")
            {
                myqry = "select *, GetDelay(actiondate, reportdate) as Delay, Case When(Select Count(1) from TRAI_tariffreviewlog) > 0 then 'Yes' else 'No' end as takenonrecord, " +
                        "Case When(UPPER(categ)='PREPAID') then mrp else ISP_rental end as mrp1, 'Withdrawn' as stat from TRAI_archive where (rno > 0) and (actiontotake<>'WITHDRAWAL') and  " + conditions + " " + sortby; ;
            }
            if (radActive == "Both")
            {
                myqry = "Select * from (select *,'' as Archiveno,'' as Archivedate, GetDelay(actiondate, reportdate) as Delay, Case When(Select Count(1) from TRAI_tariffreviewlog) > 0 then 'Yes' else 'No' end as takenonrecord, " +
                    "Case When(UPPER(categ)='PREPAID') then mrp else ISP_rental end as mrp1, 'Active' as stat from TRAI_tariffs where (rno > 0) and (actiontotake<>'WITHDRAWAL') and " + conditions;
                myqry = myqry + " UNION ";
                myqry = myqry + " select *, GetDelay(actiondate, reportdate) as Delay, Case When(Select Count(1) from TRAI_tariffreviewlog) > 0 then 'Yes' else 'No' end as takenonrecord, " +
                    "Case When(UPPER(categ)='PREPAID') then mrp else ISP_rental end as mrp1, 'Not-Active' as stat from TRAI_archive where (rno > 0) and (actiontotake='WITHDRAWAL') and " + conditions + " ) As A " + sortby;
            }

            com = new MySqlCommand(myqry, con);
            MySqlDataAdapter ada = new MySqlDataAdapter(com);
            DataSet ds = new DataSet();
            ada.Fill(ds);

            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                int recflag = 0;
                string tempstr = "<tr>";
                tempstr = tempstr + "<td align=left valign=top>" + dr["stat"].ToString().Trim() + "</td>";
                
                try
                {
                    for (int j = 4; j < 282; j++)
                    {
                        if (j == 16 || j == 17 || j == 19 || j == 20)
                        {
                            tempstr = tempstr + "<td align=left valign=top>" + Convert.ToDateTime(dr[j].ToString()).ToString("dd-MMM-yyyy").Replace("01-Jan-2001", "") + "</td>";
                        }
                        else
                        {
                            tempstr = tempstr + "<td align=left valign=top>" + dr[j].ToString().Trim().Replace("-2", "UNLIMITED").Replace("-1", "").Replace("&amp;", "&").Replace("&amp;", "&").Replace("~", "&quot;") + "</td>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    recflag = 1;
                }
                con1.Close();
                tempstr = tempstr + "</tr>";

                if (recflag == 0)
                {
                    mystr = mystr + tempstr;
                }
            }
            con.Close();

            mystr = mystr + "</table>";


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

                sb.Append(mystr.ToString());

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

    protected void ButtonAddNumeric_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;
            double val1 = 0, val2 = 0;
            int ptr = 0;
            try
            {
                ptr = Convert.ToInt32(TextPtrNumeric.Text.Trim());
            }
            catch (Exception ex) { }

            if (DropNumeric.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a parameter');</script>");
            }
            try
            {
                val1 = Convert.ToDouble(TextNumeric1.Text.Trim().Replace("'", "").Replace(",", ""));
                val2 = Convert.ToDouble(TextNumeric2.Text.Trim().Replace("'", "").Replace(",", ""));
            }
            catch (Exception ex)
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a valid value range');</script>");
            }


            if (flag == 0)
            {
                arrConditionsNumeric[ptr].Text = DropNumeric.SelectedItem.Text.Trim();    // Text of selected parameter
                arrValNumeric[ptr].Text = DropNumeric.SelectedValue.Trim();               // value of selected parameter = column number in TRAI_tariffs table
                arrNumFig1[ptr].Text = val1.ToString().Trim();                            // entered value 1 by user
                arrNumFig2[ptr].Text = val2.ToString().Trim();                            // entered value 2 by user

                DropNumeric.SelectedIndex = 0;
                TextNumeric1.Text = "";
                TextNumeric2.Text = "";
            }

            ptr++;

            TextPtrNumeric.Text = ptr.ToString();

            showConditions(null, null);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    protected void ButtonAddText_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;
            string mytext = TextText1.Text.Trim().Replace("'", "`");
            int ptr = 0;
            try
            {
                ptr = Convert.ToInt32(TextPtrText.Text.Trim());
            }
            catch (Exception ex) { }

            if (DropText.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a parameter');</script>");
            }
            if (mytext == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter the value of the selected parameter');</script>");
            }


            if (flag == 0)
            {
                arrConditionsText[ptr].Text = DropText.SelectedItem.Text.Trim();
                arrValText[ptr].Text = DropText.SelectedValue.Trim();
                arrTextFig1[ptr].Text = mytext;

                DropText.SelectedIndex = 0;
                TextText1.Text = "";
            }

            ptr++;

            TextPtrText.Text = ptr.ToString();

            showConditions(null, null);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    protected void ButtonAddDate_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;
            DateTime dt1 = Convert.ToDateTime("2001-01-01");
            DateTime dt2 = Convert.ToDateTime("2001-01-01");
            int ptr = 0;
            try
            {
                ptr = Convert.ToInt32(TextPtrDate.Text.Trim());
            }
            catch (Exception ex) { }

            if (DropDate.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a parameter');</script>");
            }
            try
            {
                dt1 = Convert.ToDateTime(TextDate.Text.Trim());
                dt2 = Convert.ToDateTime(TextDate2.Text.Trim());
            }
            catch (Exception ex)
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a valid date range');</script>");
            }

            if (dt1 > dt2)
            {
                flag = 1;
                Response.Write("<script>alert('From date can not be greater than To date.');</script>");
            }

            if (flag == 0)
            {
                arrConditionsDate[ptr].Text = DropDate.SelectedItem.Text.Trim();         // Text of selected parameter
                arrValDate[ptr].Text = DropDate.SelectedValue.Trim();               // value of selected parameter = column number in TRAI_tariffs table
                arrDateFig1[ptr].Text = dt1.ToString();                            // entered value 1 by user
                arrDateFig2[ptr].Text = dt2.ToString();                            // entered value 2 by user

                DropDate.SelectedIndex = 0;
                TextDate.Text = "";
                TextDate2.Text = "";
            }

            ptr++;

            TextPtrDate.Text = ptr.ToString();

            showConditions(null, null);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    protected void showConditions(object sender, EventArgs e)
    {
        try
        {
            int ptr1 = 0, ptr2 = 0, ptr3 = 0;
            string mystr = "";
            divNumeric.InnerHtml = "";
            divText.InnerHtml = "";
            divDates.InnerHtml = "";

            // Numeric Conditions //
            mystr = "<table width=100% cellspacing=1 cellpadding=1><tr><td class=tablecell3 align=center valign=top>Parameter</td><td class=tablecell3 align=center valign=top>Value</td><td class=tablecell3 align=center valign=top></td></tr>";
            try
            {
                ptr1 = Convert.ToInt32(TextPtrNumeric.Text.Trim());
            }
            catch (Exception ex) { }

            int cntr1 = 0;
            for (int i = 0; i < ptr1; i++)
            {
                if (arrConditionsNumeric[i].Text.Trim() != "")
                {
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell2b align=left valign=top width=55%>" + arrConditionsNumeric[i].Text.Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell2b align=center valign=top width=35%>" + arrNumFig1[i].Text.Trim() + " To " + arrNumFig2[i].Text.Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell2b align=center valign=top width=10%><a href=javascript:funDel('Numeric','" + i.ToString().Trim() + "'); ><img src=images/icondelete.jpg border=0></a></td>";
                    mystr = mystr + "</tr>";
                    cntr1++;
                }
            }
            mystr = mystr + "</table>";

            if (cntr1 > 0)
            {
                divNumeric.InnerHtml = mystr;
            }


            // Text Conditions //
            mystr = "<table width=100% cellspacing=1 cellpadding=1><tr><td class=tablecell3 align=center valign=top>Parameter</td><td class=tablecell3 align=center valign=top>Value</td><td class=tablecell3 align=center valign=top></td></tr>";
            try
            {
                ptr2 = Convert.ToInt32(TextPtrText.Text.Trim());
            }
            catch (Exception ex) { }

            int cntr2 = 0;
            for (int i = 0; i < ptr2; i++)
            {
                if (arrConditionsText[i].Text.Trim() != "")
                {
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell2b align=left valign=top width=55%>" + arrConditionsText[i].Text.Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell2b align=center valign=top width=35%>" + arrTextFig1[i].Text.Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell2b align=center valign=top width=10%><a href=javascript:funDel('Text','" + i.ToString().Trim() + "'); ><img src=images/icondelete.jpg border=0></a></td>";
                    mystr = mystr + "</tr>";
                    cntr2++;
                }
            }
            mystr = mystr + "</table>";

            if (cntr2 > 0)
            {
                divText.InnerHtml = mystr;
            }


            // Date Conditions //
            mystr = "<table width=100% cellspacing=1 cellpadding=1><tr><td class=tablecell3 align=center valign=top>Parameter</td><td class=tablecell3 align=center valign=top>Value</td><td class=tablecell3 align=center valign=top></td></tr>";
            try
            {
                ptr3 = Convert.ToInt32(TextPtrDate.Text.Trim());
            }
            catch (Exception ex) { }

            int cntr3 = 0;
            for (int i = 0; i < ptr3; i++)
            {
                if (arrConditionsDate[i].Text.Trim() != "")
                {
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell2b align=left valign=top width=55%>" + arrConditionsDate[i].Text.Trim() + "</td>";
                    mystr = mystr + "<td class=tablecell2b align=center valign=top width=35%>" + Convert.ToDateTime(arrDateFig1[i].Text.Trim()).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(arrDateFig2[i].Text.Trim()).ToString("dd-MMM-yyyy") + "</td>";
                    mystr = mystr + "<td class=tablecell2b align=center valign=top width=10%><a href=javascript:funDel('Date','" + i.ToString().Trim() + "'); ><img src=images/icondelete.jpg border=0></a></td>";
                    mystr = mystr + "</tr>";
                    cntr3++;
                }
            }
            mystr = mystr + "</table>";

            if (cntr3 > 0)
            {
                divDates.InnerHtml = mystr;
            }
            ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    protected void ButtonDel_Click(object sender, EventArgs e)
    {
        try
        {
            string dtype = TextDel1.Text.Trim();
            int dptr = Convert.ToInt32(TextDel2.Text.Trim());

            if (dtype == "Numeric")
            {
                arrConditionsNumeric[dptr].Text = "";
                arrNumFig1[dptr].Text = "";
                arrNumFig2[dptr].Text = "";
            }

            if (dtype == "Text")
            {
                arrConditionsText[dptr].Text = "";
                arrTextFig1[dptr].Text = "";
            }

            if (dtype == "Date")
            {
                arrConditionsDate[dptr].Text = "";
                arrDateFig1[dptr].Text = "";
                arrDateFig2[dptr].Text = "";
            }
            showConditions(null, null);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
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