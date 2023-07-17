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
using System.Net;
public partial class index : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com4, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con4, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr4, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, str2, tablename;
    int cntr, rno, colcount, circount, size, zno;
    CheckBox[] charr;
    CheckBox ch;
    Table tb = new Table();
    Table tb2 = new Table();
    Table tb3 = new Table();
    Table tb4 = new Table();
    Table tb5 = new Table();
    Table tb6 = new Table();

    Table tbresults;
    CheckBox[] arrResult;
    CheckBox chkResult;


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

        // CODE TO CHECK IF THE PAGE IS BEING OPENED ON A MOBILE DEVICE. IF YES, REDIRECT IT TO INDEX PAGE FOR MOBILES

        String labelText = "";
        System.Web.HttpBrowserCapabilities myBrowserCaps = Request.Browser;
        if (((System.Web.Configuration.HttpCapabilitiesBase)myBrowserCaps).IsMobileDevice)
        {
            //labelText = "Browser is a mobile device.";
            Response.Redirect("https://tariff.trai.gov.in/consumerview/mob/indexm.aspx");
        }
        else
        {
            //labelText = "Browser is not a mobile device.";
        }
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('" + labelText + "');", true);

        // CODE TO CHECK IF THE PAGE IS BEING OPENED ON A MOBILE DEVICE. IF YES, REDIRECT IT TO INDEX PAGE FOR MOBILES - CODE ENDS HERE



        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con4 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        Server.ScriptTimeout = 999999;

        tablename = "TRAI_tariffs";

        int resultsize = 3000;
        arrResult = new CheckBox[resultsize];
        for (int ii = 0; ii < resultsize; ii++)
        {
            chkResult = new CheckBox();
            arrResult[ii] = chkResult;
            divChkResults.Controls.Add(arrResult[ii]);
        }

        colcount = 0;
        try
        {
            //com = new MySqlCommand("select count(distinct(oper)) from TRAI_operators", con);
            //com = new MySqlCommand("select count(distinct(oper)) from TRAI_operators where(oper!='Aircel' and oper!='Telenor' and oper!='Plintron')", con);
            com = new MySqlCommand("select count(distinct(oper)) from TRAI_operators where(oper!='Aircel' and oper!='Telenor' and oper!='Plintron' and oper!='Idea' and oper!='Vodafone')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            size = Convert.ToInt32(dr[0].ToString());
            con.Close();


        }
        catch (Exception ex)
        { }

        if (ChkOper.Items.Count == 0)
        {
            ChkOper.Items.Add("All Operators");
            ChkOper.Items[0].Attributes.Add("onClick", "javascript:chkAll();");
            ChkOper.Items[0].Selected = true;
            int i = 1;
            com = new MySqlCommand("select distinct(oper) from TRAI_operators where(oper!='Aircel' and oper!='Telenor' and oper!='Plintron') order by oper", con);
          //  com = new MySqlCommand("select distinct(oper) from TRAI_operators where(oper!='Aircel' and oper!='Telenor' and oper!='Plintron' and oper!='Idea' and oper!='Vodafone') order by oper", con);
             
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                ChkOper.Items.Add(dr["oper"].ToString().Trim());
                ChkOper.Items[i].Attributes.Add("onClick", "javascript:unchk();");

                i++;
            }
            circount = i;
            con.Close();
        }

        if (!IsPostBack)
        {

            // insert NON-MOBILE record in hit counter table //
            getMaxRno("rno", "TRAI_hitcounter");
            com = new MySqlCommand("insert into TRAI_hitcounter values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','Non-Mobile')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            //com = new MySqlCommand("select distinct(circ) from TRAI_circles order by circ", con);
            //com = new MySqlCommand("select distinct(circ) from TRAI_circles where(upper(circ)='DELHI' or upper(circ)='GUJARAT' or upper(circ)='KOLKATA' or upper(circ)='ODISHA' or upper(circ)='MUMBAI' or upper(circ)='NORTH EAST' or upper(circ)='ANDHRA PRADESH' or upper(circ)='JAMMU AND KASHMIR' or upper(circ)='KARNATAKA' or upper(circ)='HIMACHAL PRADESH' or upper(circ)='MAHARASHTRA' or upper(circ)='BIHAR' or upper(circ)='CHENNAI AND TAMIL NADU' or upper(circ)='ASSAM' or upper(circ)='HARYANA' or upper(circ)='KERALA' or upper(circ)='MADHYA PRADESH' or upper(circ)='PUNJAB' or upper(circ)='UP EAST' or upper(circ)='UP WEST') order by circ", con);
            //com = new MySqlCommand("select distinct(circ) from TRAI_circles where(upper(circ)!='ALL INDIA') order by circ", con);
            com = new MySqlCommand("select distinct(circ) from TRAI_circles where(upper(circ)!='ALL INDIA' and upper(circ)!='TEST CIRCLE' and upper(circ)!='TESTING CIRCLE' and upper(circ)!='ISP TESTCIRCLE') order by circ", con);
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                DropCircle.Items.Add(dr[0].ToString().Trim().Replace("&amp;", "&"));
            }
            con.Close();


            LoadData(null, null);

        }

    }



    protected void LoadData(object sender, EventArgs e)
    {
        try
        {
            divFilterButton.Visible = false;
            divsort.Visible = false;
            spanDownload.Visible = false;
            divFilterButton2.Visible = false;
            spanDownload2.Visible = false;
            divDownload2.Visible = false;
            spanMatching.InnerHtml = "";
            divresults.Visible = false;
            TextCompareProduct.Text = "";

            //divheaders.InnerHtml = "";
            divresults.InnerHtml = "";

            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            string otype = Request.Form["RadProvider"];
            if (otype == null)
            {
                otype = "TSP";
            }
            if(otype=="ISP")
            {
                divRoaming.Visible = false;
                divUnlimCalls.Visible = false;
                divTalktime.Visible = false;
                divDataTypes.Visible = false;
                ChkPlans.Visible = false;
                LblISPTtypes.Visible = true;
                divDailyDataCapping.Visible = false;
                divAddInfoISP.Visible = true;
                divAddParamISP.Visible = true;
                divAddParamTSP.Visible = false;
            }
            else
            {
                divRoaming.Visible = true;
                divUnlimCalls.Visible = true;
                divTalktime.Visible = true;
                divDataTypes.Visible = true;
                ChkPlans.Visible = true;
                LblISPTtypes.Visible = false;
                divDailyDataCapping.Visible = true;
                divAddInfoISP.Visible = false;
                divAddParamISP.Visible = false;
                divAddParamTSP.Visible = true;
            }

            string mob = Request.Form["RadDevice"];
            if (mob == null)
            {
                mob = "Mobile";
            }
            string prepost = Request.Form["RadPrePost"];
            if (prepost == null)
            {
                prepost = "Prepaid";
            }

            if(prepost=="Postpaid")
            {
                LblISPTtypes.Text = "All Postpaid Tariffs";
            }
            else
            {
                LblISPTtypes.Text = "All Prepaid Tariffs";
            }


            string circ = DropCircle.SelectedItem.Text.Trim();
            //string oper = DropOperator.SelectedItem.Text.Trim();

            divPrePost.Visible = true;
            
            ChkDataTech1.Visible = true;
            ChkDataTech2.Visible = true;
            ChkDataTech3.Visible = true;


            string temp = DropCircle.SelectedItem.Text.Trim();

            DropCircle.Text = temp;

            if(otype=="TSP")
            {
                divDevice.Visible = true;
            }
            else
            {
                divDevice.Visible = false;
            }

            if (mob == "Landline" || prepost == "Postpaid")
            {
                divValidity.Visible = false;
                divTalktime.Visible = false;
                //LblPrice.Text = "Monthly Rental &#8377; (Optional)";
                //LblPrice.Text = "Monthly Rental &#x20B9; (Optional)";
                LblPrice.Text = "Monthly Rental <i class=\"fa fa-inr\"></i> (Optional)";
            }
            else
            {
                if (otype == "TSP")
                {
                    divValidity.Visible = true;
                    divTalktime.Visible = true;
                }
                //LblPrice.Text = "Price &#8377; (Optional)";
                //LblPrice.Text = "Price &#x20B9; (Optional)";
                LblPrice.Text = "Price <i class=\"fa fa-inr\"></i> (Optional)";
            }

            if (otype == "TSP")
            {
                if (mob == "Landline")
                {
                    ChkISDRoaming.Visible = false;
                    ChkNatRoaming.Visible = false;
                    ChkUnlim_Roaming.Visible = false;
                    divDailyDataCapping.Visible = false;
                }
                else
                {
                    ChkISDRoaming.Visible = true;
                    ChkISDRoaming.Attributes.Add("style", "margin-left:-12px;");
                    ChkNatRoaming.Visible = true;
                    ChkUnlim_Roaming.Visible = true;
                    divDailyDataCapping.Visible = true;
                }
            }

            /*
            DropOperator.Items.Clear();
            DropOperator.Items.Add("OPERATOR");
            DropOperator.Items.Add("All Operators");
            */

            int planid = 0;
            if (mob == "Mobile" && prepost == "Prepaid")
            {
                // Left Pane Items //

                ChkPlans.Items.Clear();
                ChkPlans.Items.Add("All Tariffs");
                ChkPlans.Items[planid].Attributes.Add("title", "All Tariffs");
                ChkPlans.Items[planid].Attributes.Add("style", "margin-right:10px");
                planid++;
                ChkPlans.Items.Add("Plan Voucher");
                ChkPlans.Items[planid].Attributes.Add("title", "Plan Voucher");
                planid++;
                ChkPlans.Items.Add("STV");
                ChkPlans.Items[planid].Attributes.Add("title", "Special Tariff Voucher");
                planid++;
                ChkPlans.Items.Add("Combo");
                ChkPlans.Items[planid].Attributes.Add("title", "Combo Pack");
                planid++;
                ChkPlans.Items.Add("Top Up");
                ChkPlans.Items[planid].Attributes.Add("title", "Top Up Voucher");
                planid++;
                ChkPlans.Items.Add("SUK");
                ChkPlans.Items[planid].Attributes.Add("title", "Start Up Kit");
                planid++;
                ChkPlans.Items.Add("VAS");
                ChkPlans.Items[planid].Attributes.Add("title", "Value Added Services");
                planid++;
                ChkPlans.Items.Add("Promo");
                ChkPlans.Items[planid].Attributes.Add("title", "Promotional Offer");
                planid++;


                ChkPlans.Items[0].Selected = true;

                // Left Pane Items - CODE ENDS HERE //


                // Operator DropDown and checkboxes Population //
                /*
                for (int i = 0; i < size; i++)
                {
                    ChkOper.Items[i].Enabled = false;
                    ChkOper.Items[i].Attributes.Add("style", "color:#afafaf;");
                }
                */

                string ops = ",";
                ChkOper.Items.Clear();
                int operpos = 0;

                if (otype == "TSP")
                {
                    ChkOper.Items.Add("All Operators");
                    ops = ops + "All Operators" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:chkAll();");
                    ChkOper.Items[operpos].Selected = true;
                    operpos++;

                    ChkOper.Items.Add("Aerovoyce");
                    ops = ops + "Aerovoyce" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;

                    ChkOper.Items.Add("Airtel");
                    ops = ops + "Airtel" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;

                    if (circ != "Delhi" && circ != "Mumbai")
                    {
                        ChkOper.Items.Add("BSNL");
                        ops = ops + "BSNL" + ",";
                        ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                        operpos++;
                    }
                    /*
                    ChkOper.Items.Add("Idea");
                    ops = ops + "Idea" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;
                    */
                    ChkOper.Items.Add("Jio");
                    ops = ops + "Jio" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;

                    if (circ == "Delhi" || circ == "Mumbai")
                    {
                        ChkOper.Items.Add("MTNL");
                        ops = ops + "MTNL" + ",";
                        ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                        operpos++;
                    }

                    if (circ != "Assam" && circ != "Bihar" && circ != "Jammu and Kashmir" && circ != "North East" && circ != "West Bengal")
                    {
                        ChkOper.Items.Add("Tata Tele");
                        ops = ops + "Tata Tele" + ",";
                        ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                        operpos++;
                    }
                    /*
                    ChkOper.Items.Add("Vodafone");
                    ops = ops + "Vodafone" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;
                    */

                    ChkOper.Items.Add("Vodafone Idea");
                    ops = ops + "Vodafone Idea" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;

                    if (circ == "Chennai and Tamil Nadu")
                    {
                        ChkOper.Items.Add("Surftelecom");
                        ops = ops + "Surftelecom" + ",";
                        ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                        operpos++;
                    }
                }
                else
                {
                    LoadISP(null, null);
                }

                // Operator DropDown and Checkboxes Population - CODE ENDS HERE //

            }

            if (mob == "Mobile" && prepost == "Postpaid")
            {
                // Left Pane Items //

                ChkPlans.Items.Clear();
                ChkPlans.Items.Add("All Tariffs");
                ChkPlans.Items[planid].Attributes.Add("title", "All Tariffs");
                planid++;
                ChkPlans.Items.Add("Plans");
                ChkPlans.Items[planid].Attributes.Add("title", "Postpaid Plans");
                planid++;
                ChkPlans.Items.Add("Add On Pack");
                ChkPlans.Items[planid].Attributes.Add("title", "Add On Pack");
                planid++;
                ChkPlans.Items.Add("VAS");
                ChkPlans.Items[planid].Attributes.Add("title", "Value Added Services");
                planid++;
                ChkPlans.Items.Add("Promo");
                ChkPlans.Items[planid].Attributes.Add("title", "Promotional Offers");
                planid++;

                ChkPlans.Items[0].Selected = true;

                // Left Pane Items - CODE ENDS HERE //


                // Operator DropDown Population //
                /*
                for (int i = 0; i < size; i++)
                {
                    ChkOper.Items[i].Enabled = false;
                    ChkOper.Items[i].Attributes.Add("style", "color:#afafaf;");
                }
                */
                string ops = ",";
                ChkOper.Items.Clear();
                int operpos = 0;

                if(otype=="TSP")
                { 
                    ChkOper.Items.Add("All Operators");
                    ops = ops + "All Operators" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:chkAll();");
                    ChkOper.Items[operpos].Selected = true;
                    operpos++;

                    ChkOper.Items.Add("Aerovoyce");
                    ops = ops + "Aerovoyce" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;

                    ChkOper.Items.Add("Airtel");
                    ops = ops + "Airtel" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;

                    if (circ != "Delhi" && circ != "Mumbai")
                    {
                        ChkOper.Items.Add("BSNL");
                        ops = ops + "BSNL" + ",";
                        ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                        operpos++;
                    }
                    /*
                    ChkOper.Items.Add("Idea");
                    ops = ops + "Idea" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;
                    */
                    ChkOper.Items.Add("Jio");
                    ops = ops + "Jio" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;

                    if (circ == "Delhi" || circ == "Mumbai")
                    {
                        ChkOper.Items.Add("MTNL");
                        ops = ops + "MTNL" + ",";
                        ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                        operpos++;
                    }

                    if (circ != "Assam" && circ != "Bihar" && circ != "Delhi" && circ != "Jammu and Kashmir" && circ != "North East" && circ != "West Bengal")
                    {
                        ChkOper.Items.Add("Tata Tele");
                        ops = ops + "Tata Tele" + ",";
                        ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                        operpos++;
                    }
                    /*
                    ChkOper.Items.Add("Vodafone");
                    ops = ops + "Vodafone" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;
                    */

                    ChkOper.Items.Add("Vodafone Idea");
                    ops = ops + "Vodafone Idea" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                    operpos++;

                    if (circ == "Chennai and Tamil Nadu")
                    {
                        ChkOper.Items.Add("Surftelecom");
                        ops = ops + "Surftelecom" + ",";
                        ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                        operpos++;
                    }
                }
                else
                {
                    LoadISP(null, null);
                }

                // Operator DropDown Population - CODE ENDS HERE //
            }

            if (mob == "Landline")
            {
                if (otype == "TSP")
                {
                    divPrePost.Visible = false;
                }
                ChkDataTech1.Visible = false;
                ChkDataTech2.Visible = false;
                ChkDataTech3.Visible = false;

                // Left Pane Items //

                ChkPlans.Items.Clear();
                ChkPlans.Items.Add("All Tariffs");
                ChkPlans.Items[planid].Attributes.Add("title", "All Tariffs");
                planid++;
                ChkPlans.Items.Add("Plans");
                ChkPlans.Items[planid].Attributes.Add("title", "Plans");
                planid++;
                ChkPlans.Items.Add("Add On Pack");
                ChkPlans.Items[planid].Attributes.Add("title", "Add On Pack");
                planid++;

                ChkPlans.Items[0].Selected = true;

                // Left Pane Items - CODE ENDS HERE //


                // Operator DropDown Population //

                string ops = ",";
                ChkOper.Items.Clear();
                int operpos = 0;

                if(otype=="TSP")
                { 
                    ChkOper.Items.Add("All Operators");
                    ops = ops + "All Operators" + ",";
                    ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:chkAll();");
                    ChkOper.Items[operpos].Selected = true;
                    operpos++;

                    ChkOper.Items.Add("Aerovoyce");
                    ops = ops + "Aerovoyce" + ",";
                    //ChkOper.Items[operpos].Selected = true;
                    operpos++;

                    ChkOper.Items.Add("Airtel");
                    ops = ops + "Airtel" + ",";
                    //ChkOper.Items[operpos].Selected = true;
                    operpos++;

                    if (circ != "Delhi" && circ != "Mumbai")
                    {
                        ChkOper.Items.Add("BSNL");
                        ops = ops + "BSNL" + ",";
                        //ChkOper.Items[operpos].Selected = true;
                        operpos++;
                    }

                    if (circ == "Delhi" || circ == "Mumbai")
                    {
                        ChkOper.Items.Add("MTNL");
                        ops = ops + "MTNL" + ",";
                        //ChkOper.Items[operpos].Selected = true;
                        operpos++;
                    }

                    if (circ == "Punjab")
                    {
                        ChkOper.Items.Add("Quadrant (Connect)");
                        ops = ops + "Quadrant (Connect)" + ",";
                        //ChkOper.Items[operpos].Selected = true;
                        operpos++;
                    }

                    if (circ != "Assam" && circ != "Bihar" && circ != "Chennai & Tamil Nadu" && circ != "Jammu and Kashmir" && circ != "North East" && circ != "West Bengal")
                    {
                        ChkOper.Items.Add("Tata Tele");
                        ops = ops + "Tata Tele" + ",";
                        //ChkOper.Items[operpos].Selected = true;
                        operpos++;
                    }
                }
                else
                {
                    LoadISP(null, null);
                }

                // Operator DropDown Population - CODE ENDS HERE //
            }


            for (int i = 0; i < ChkPlans.Items.Count; i++)
            {
                ChkPlans.Items[i].Attributes.Add("onclick", "setChk(this.value);");
            }
        }
        catch (Exception ex) { }
    }






    protected void LoadISP(object sender, EventArgs e)
    {
        try
        {
            string ops = ",";
            ChkOper.Items.Clear();
            int operpos = 0;

            ChkOper.Items.Add("All Operators");
            ops = ops + "All Operators" + ",";
            ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:chkAll();");
            ChkOper.Items[operpos].Selected = true;
            operpos++;

            com4 = new MySqlCommand("select distinct(oper) from TRAI_tariffs where(upper(oper)!='AIRCEL' and upper(oper)!='AIRTEL' and upper(oper)!='BSNL' and upper(oper)!='IDEA' and upper(oper)!='JIO' and upper(oper)!='MTNL' and upper(oper)!='PLINTRON' and upper(oper)!='QUADRANT (CONNECT)' and upper(oper)!='TATA TELE' and upper(oper)!='TELENOR' and upper(oper)!='VODAFONE' and upper(oper)!='VODAFONE IDEA' and upper(oper)!='SURFTELECOM' and upper(oper)!='AEROVOYCE') order by oper", con4);
            con4.Open();
            dr4 = com4.ExecuteReader();
            while (dr4.Read())
            {
                ChkOper.Items.Add(dr4[0].ToString().Trim());
                ops = ops + dr4[0].ToString().Trim() + ",";
                ChkOper.Items[operpos].Attributes.Add("onClick", "javascript:unchk();");
                operpos++;
            }
            con4.Close();
        }
        catch (Exception ex) { }
    }





    protected void Button1_Click(object sender, EventArgs e)
    {

        try
        {
            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            int flag = 0;

            string mob = Request.Form["RadDevice"];
            string prepost = Request.Form["RadPrePost"];
            string circ = DropCircle.SelectedItem.Text.Trim();

            string oper = ",";
            for (int i = 0; i < ChkOper.Items.Count; i++)
            {
                if (ChkOper.Items[i].Selected == true || ChkOper.Items[0].Selected == true)
                {
                    oper = oper + ChkOper.Items[i].Text.Trim() + ",";
                }
            }

            string plans = ",";
            for (int i = 0; i < ChkPlans.Items.Count; i++)
            {
                if (ChkPlans.Items[i].Selected == true)
                {
                    plans = plans + ChkPlans.Items[i].Text.Trim() + ",";
                }
            }

            if (mob == null)     // 'ISP' is selected
            {
                plans = plans.Replace(",Plan Voucher,", ",Prepaid Plan Voucher,");
                plans = plans.Replace(",STV,", ",Prepaid STV,");
                plans = plans.Replace(",Combo,", ",Prepaid Combo,");
                plans = plans.Replace(",Top Up,", ",Prepaid Top Up,");
                plans = plans.Replace(",VAS,", ",Prepaid VAS,Postpaid VAS,");
                plans = plans.Replace(",Promo,", ",Prepaid Promo Offer,Postpaid Promo Offer,");
                plans = plans.Replace(",Plans,", ",Postpaid Plan,Postpaid Fixed Line Plan,");
                plans = plans.Replace(",Add On Pack,", ",Postpaid Add On Pack,Postpaid Fixed Line Add On Pack,");
            }
            else
            {
                if (mob == "Mobile" && prepost == "Prepaid")    // rename selection as per stored sheet names
                {
                    plans = plans.Replace(",Plan Voucher,", ",Prepaid Plan Voucher,");
                    plans = plans.Replace(",STV,", ",Prepaid STV,");
                    plans = plans.Replace(",Combo,", ",Prepaid Combo,");
                    plans = plans.Replace(",Top Up,", ",Prepaid Top Up,");
                    plans = plans.Replace(",VAS,", ",Prepaid VAS,");
                    plans = plans.Replace(",Promo,", ",Prepaid Promo Offer,");
                }
                if (mob == "Mobile" && prepost == "Postpaid")    // rename selection as per stored sheet names
                {
                    plans = plans.Replace(",Plans,", ",Postpaid Plan,");
                    plans = plans.Replace(",Add On Pack,", ",Postpaid Add On Pack,");
                    plans = plans.Replace(",VAS,", ",Postpaid VAS,");
                    plans = plans.Replace(",Promo,", ",Postpaid Promo Offer,");
                }
                if (mob == "Landline")    // rename selection as per stored sheet names
                {
                    plans = plans.Replace(",Plans,", ",Postpaid Fixed Line Plan,");
                    plans = plans.Replace(",Add On Pack,", ",Postpaid Fixed Line Add On Pack,");
                }
            }

            if (plans.Contains("Prepaid Plan Voucher"))
            {
                plans += "Prepaid ISP,";
            }
            if (plans.Contains("Postpaid Plan"))
            {
                plans += "Postpaid ISP,";
            }
            

            //if (circ == "CIRCLE")
            if (circ == "Select")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a circle');</script>");
            }

            if (oper.Contains("OPERATOR"))
            {
                flag = 1;
                Response.Write("<script>alert('Please select an operator');</script>");
            }

            if (flag == 0)
            {
                string conditions = "";

                if (Request.Form["RadProvider"] == "ISP")
                {
                    if (divPrePost.Visible == true && prepost == "Prepaid")
                    {
                        conditions = conditions + " and (categ='Prepaid')";
                    }
                    if (divPrePost.Visible == true && prepost == "Postpaid")
                    {
                        conditions = conditions + " and (categ='Postpaid')";
                    }
                }
                else
                {
                    if (mob == "Mobile")
                    {
                        conditions = conditions + " and (ttype!='Postpaid Fixed Line Plan' and ttype!='Prepaid Fixed Line Plan' and ttype!='Postpaid Fixed Line Add On Pack' and ttype!='Prepaid Fixed Line Add On Pack' and ttype!='Prepaid ISP' and ttype!='Postpaid ISP' and ttype!='Prepaid Gen ISD Tariff' and ttype!='Postpaid Gen ISD Tariff' and ttype!='Prepaid International Roaming' and ttype!='Postpaid International Roaming')";
                    }
                    if (mob == "Landline")
                    {
                        conditions = conditions + " and (ttype='Postpaid Fixed Line Plan' or ttype='Prepaid Fixed Line Plan' or ttype='Postpaid Fixed Line Add On Pack' or ttype='Prepaid Fixed Line Add On Pack')";
                    }


                    if (divPrePost.Visible == true && prepost == "Prepaid")
                    {
                        conditions = conditions + " and (ttype='Prepaid Plan Voucher' or ttype='Prepaid STV' or ttype='Prepaid Combo' or ttype='Prepaid Top Up' or ttype='Prepaid VAS' or ttype='Prepaid Promo Offer' or ttype='SUK' or ttype='Prepaid ISP')";
                    }
                    if (divPrePost.Visible == true && prepost == "Postpaid")
                    {
                        conditions = conditions + " and (ttype='Postpaid Plan' or ttype='Postpaid Add On Pack' or ttype='Postpaid VAS' or ttype='Postpaid Promo Offer' or ttype='Postpaid ISP')";
                    }
                }


                if (circ != "All India")
                {
                    //conditions = conditions + " and ('" + circ.ToUpper() + "' like CONCAT('%', upper(circ), '%')) ";   // reverse like 
                    conditions = conditions + " and (('" + circ.ToUpper() + "' like CONCAT('%', upper(circ), '%') or upper(circ)='ALL INDIA')) ";   // reverse like 
                }

                if (oper != ",All,")
                {
                    //conditions = conditions + " and ('" + oper.ToUpper() + "' like CONCAT('%', upper(oper), '%')) ";   // reverse like 
                    conditions = conditions + " and ('" + oper.ToUpper() + "' like CONCAT('%,', upper(oper), ',%')) ";   // reverse like 
                }


                // fetch data as per selected sheet names //

                if (Request.Form["RadProvider"] == "TSP")
                {
                    if (plans != ",All Tariffs,")
                    {
                        plans = plans.Replace(",All Tariffs", "");
                        //conditions = conditions + " and ('" + plans + "' like '%,' + ttype + ',%') ";   // reverse like 
                        conditions = conditions + " and ('" + plans.ToUpper() + "' like CONCAT('%', upper(ttype), '%')) ";   // reverse like 
                    }
                }

                // fetch data as per selected sheet names - CODE ENDS HERE//

                /*
                if (Convert.ToDouble(amount2a.Text.Trim()) == 0)
                {
                    conditions = conditions + " and (mrp >= -1 and mrp <= " + Convert.ToDouble(amount2b.Text.Trim()) + ")";  // to include blank figure entries
                }
                else
                {
                    conditions = conditions + " and (mrp >= " + Convert.ToDouble(amount2a.Text.Trim()) + " and mrp <= " + Convert.ToDouble(amount2b.Text.Trim()) + ")";
                }
                */

                if (Convert.ToDouble(TextPrice1.Text.Trim()) == 0)
                {
                    conditions = conditions + " and (mrp >= -1 and mrp <= " + Convert.ToDouble(TextPrice2.Text.Trim()) + ")";  // to include blank figure entries
                }
                else
                {
                    conditions = conditions + " and (mrp >= " + Convert.ToDouble(TextPrice1.Text.Trim()) + " and mrp <= " + Convert.ToDouble(TextPrice2.Text.Trim()) + ")";
                }

                if (divValidity.Visible == true)
                {
                    if (ChkValidityMore.Checked == true)
                    {
                        conditions = conditions + " and (validity =-2 or validity=0 or validity > 365)";
                    }
                    else
                    {
                        if (Convert.ToDouble(amount4a.Text.Trim()) == 0 && Convert.ToDouble(amount4b.Text.Trim()) == 365)   // if validity section is not done, include 'Unlimited' records
                        {
                            conditions = conditions + " and (validity >= -1 and validity <= " + Convert.ToDouble(amount4b.Text.Trim()) + ")";
                        }
                        else
                        {
                            conditions = conditions + " and (validity>0) and (validity >= " + Convert.ToDouble(amount4a.Text.Trim()) + " and validity <= " + Convert.ToDouble(amount4b.Text.Trim()) + ")";
                        }
                    }
                }

                if (Request.Form["RadProvider"] == "ISP" && ChkCapUnlim.Checked == true)
                {
                    conditions = conditions + " and (adddata_ISP=-2)";
                }

                if (mob == "Landline")
                {
                    string unlimcondition = "";
                    if (amount3b.Text.Trim() == "500" || ChkCapUnlim.Checked==true)     // if upper limit is 500 GB, include unlimited data records also
                    {
                        unlimcondition = " or (adddata_ISP=-2)";
                    }
                    if (Convert.ToDouble(amount3a.Text.Trim()) == 0)
                    {
                        //conditions = conditions + " and ((adddata_ISP > -1 and adddata_ISP <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                        conditions = conditions + " and ((adddata_ISP >= -1 and adddata_ISP <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                    }
                    else
                    {
                        conditions = conditions + " and ((adddata_ISP >= " + Convert.ToDouble(amount3a.Text.Trim()) + " and adddata_ISP <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                    }
                }
                else
                {
                    if (ChkDataTech1.Checked == true || ChkDataTech2.Checked == true || ChkDataTech3.Checked == true)
                    {
                        if (Convert.ToDouble(amount3a.Text.Trim()) == 0)
                        {
                            conditions = conditions + " and (adddata_total2g > 999999 ";   // for implementing 'OR' conditions, an additional condition is required.
                            if (ChkDataTech1.Checked == true)
                            {
                                string unlimcondition = "";
                                if (amount3b.Text.Trim() == "500" || ChkCapUnlim.Checked == true)     // if upper limit is 500 GB, include unlimited data records also
                                {
                                    unlimcondition = " or (adddata_total2g=-2)";
                                }
                                //conditions = conditions + " or ((adddata_total2g > -1 and adddata_total2g <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                                conditions = conditions + " or (((adddata_total2g > -1 and adddata_total2g <= " + Convert.ToDouble(amount3b.Text.Trim()) + " and adddata_unit='GB') or (adddata_total2g > -1 and adddata_total2g <= " + Math.Round(Convert.ToDouble(amount3b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                            }
                            if (ChkDataTech2.Checked == true)
                            {
                                string unlimcondition = "";
                                if (amount3b.Text.Trim() == "500" || ChkCapUnlim.Checked == true)     // if upper limit is 500 GB, include unlimited data records also
                                {
                                    unlimcondition = " or (adddata_total3g=-2)";
                                }
                                //conditions = conditions + " or ((adddata_total3g > -1 and adddata_total3g <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                                //conditions = conditions + " or (((adddata_total3g > -1 and adddata_total3g <= " + Convert.ToDouble(amount3b.Text.Trim()) + " and adddata_unit='GB') or (adddata_total2g > -1 and adddata_total2g <= " + Math.Round(Convert.ToDouble(amount3b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                                conditions = conditions + " or (((adddata_total3g > -1 and adddata_total3g <= " + Convert.ToDouble(amount3b.Text.Trim()) + " and adddata_unit='GB') or (adddata_total3g > -1 and adddata_total3g <= " + Math.Round(Convert.ToDouble(amount3b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                            }
                            if (ChkDataTech3.Checked == true)
                            {
                                string unlimcondition = "";
                                if (amount3b.Text.Trim() == "500" || ChkCapUnlim.Checked == true)     // if upper limit is 500 GB, include unlimited data records also
                                {
                                    unlimcondition = " or (adddata_total4g=-2)";
                                }
                                //conditions = conditions + " or ((adddata_total4g > -1 and adddata_total4g <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                                //conditions = conditions + " or (((adddata_total4g > -1 and adddata_total4g <= " + Convert.ToDouble(amount3b.Text.Trim()) + " and adddata_unit='GB') or (adddata_total2g > -1 and adddata_total2g <= " + Math.Round(Convert.ToDouble(amount3b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                                conditions = conditions + " or (((adddata_total4g > -1 and adddata_total4g <= " + Convert.ToDouble(amount3b.Text.Trim()) + " and adddata_unit='GB') or (adddata_total4g > -1 and adddata_total4g <= " + Math.Round(Convert.ToDouble(amount3b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                            }
                            conditions = conditions + ")";
                        }
                        else
                        {
                            conditions = conditions + " and (adddata_total2g > 999999 ";   // for implementing 'OR' conditions, an additional condition is required.
                            if (ChkDataTech1.Checked == true)
                            {
                                string unlimcondition = "";
                                if (amount3b.Text.Trim() == "500" || ChkCapUnlim.Checked == true)     // if upper limit is 500 GB, include unlimited data records also
                                {
                                    unlimcondition = " or (adddata_total2g=-2)";
                                }
                                //conditions = conditions + " or ((adddata_total2g >= " + Convert.ToDouble(amount3a.Text.Trim()) + " and adddata_total2g <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                                conditions = conditions + " or (((adddata_total2g >= " + Convert.ToDouble(amount3a.Text.Trim()) + " and adddata_unit='GB') or (adddata_total2g >= " + Math.Round(Convert.ToDouble(amount3a.Text.Trim()) / 1000, 2) + " and adddata_unit='MB')) and ((adddata_total2g <= " + Convert.ToDouble(amount3b.Text.Trim()) + " and adddata_unit='GB') or (adddata_total2g <= " + Math.Round(Convert.ToDouble(amount3b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                            }
                            if (ChkDataTech2.Checked == true)
                            {
                                string unlimcondition = "";
                                if (amount3b.Text.Trim() == "500" || ChkCapUnlim.Checked == true)     // if upper limit is 500 GB, include unlimited data records also
                                {
                                    unlimcondition = " or (adddata_total3g=-2)";
                                }
                                //conditions = conditions + " or ((adddata_total3g >= " + Convert.ToDouble(amount3a.Text.Trim()) + " and adddata_total3g <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                                conditions = conditions + " or (((adddata_total3g >= " + Convert.ToDouble(amount3a.Text.Trim()) + " and adddata_unit='GB') or (adddata_total3g >= " + Math.Round(Convert.ToDouble(amount3a.Text.Trim()) / 1000, 2) + " and adddata_unit='MB')) and ((adddata_total3g <= " + Convert.ToDouble(amount3b.Text.Trim()) + " and adddata_unit='GB') or (adddata_total3g <= " + Math.Round(Convert.ToDouble(amount3b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                            }
                            if (ChkDataTech3.Checked == true)
                            {
                                string unlimcondition = "";
                                if (amount3b.Text.Trim() == "500" || ChkCapUnlim.Checked == true)     // if upper limit is 500 GB, include unlimited data records also
                                {
                                    unlimcondition = " or (adddata_total4g=-2)";
                                }
                                //conditions = conditions + " or ((adddata_total4g >= " + Convert.ToDouble(amount3a.Text.Trim()) + " and adddata_total4g <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")" + unlimcondition + ")";
                                conditions = conditions + " or (((adddata_total4g >= " + Convert.ToDouble(amount3a.Text.Trim()) + " and adddata_unit='GB') or (adddata_total4g >= " + Math.Round(Convert.ToDouble(amount3a.Text.Trim()) / 1000, 2) + " and adddata_unit='MB')) and ((adddata_total4g <= " + Convert.ToDouble(amount3b.Text.Trim()) + " and adddata_unit='GB') or (adddata_total4g <= " + Math.Round(Convert.ToDouble(amount3b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                            }
                            conditions = conditions + ")";
                        }
                    }
                }


                if (ChkUnlim_Local.Checked == true || ChkUnlim_STD.Checked == true || ChkUnlim_Roaming.Checked == true)
                {
                    conditions = conditions + " and (adddata_total2g > 999999 ";   // for implementing 'OR' conditions, an additional condition is required.
                    if (ChkUnlim_Local.Checked == true)
                    {
                        conditions = conditions + " or ((local_all_voicecharges=0) or (local_on_voice_peak=0 and local_on_voice_offpeak=0 and local_off_voice_peak=0 and local_off_voice_offpeak=0 and local_fix_on_voice_peak=0 and local_fix_on_voice_offpeak=0 and local_fix_off_voice_peak=0 and local_fix_off_voice_offpeak=0))";
                    }
                    if (ChkUnlim_STD.Checked == true)
                    {
                        conditions = conditions + " or ((std_all_voicecharges=0) or (std_on_voice_peak=0 and std_on_voice_offpeak=0 and std_off_voice_peak=0 and std_off_voice_offpeak=0 and std_fix_on_voice_peak=0 and std_fix_on_voice_offpeak=0 and std_fix_off_voice_peak=0 and std_fix_off_voice_offpeak=0))";
                    }
                    if (ChkUnlim_Roaming.Checked == true)
                    {
                        conditions = conditions + " or (roam_call_voice_out=0 and roam_call_voice_std=0)";
                    }
                    conditions = conditions + ")";
                }


                if (ChkFullTalktime.Checked == true)
                {
                    conditions = conditions + " and (mrp>0 and monval>=mrp)";
                }

                conditions = conditions + " and (";
                if (ChkISDPack.Checked == true)
                {
                    conditions = conditions + "(((isd_countries!='' and isd_countries!='-' and isd_countries!='-1') or (isd_weblink!='' and isd_weblink!='-' and isd_weblink!='-1') or (add_isd_summ!='' and add_isd_summ!='-' and add_isd_summ!='-1') or (add_isd_link!='' and add_isd_link!='-' and add_isd_link!='-1')) and (ttype='Prepaid STV' or ttype='Prepaid Combo')) or ";
                }
                if (ChkISDRoaming.Checked == true)
                {
                    conditions = conditions + "(((introam_countries!='' and introam_countries!='-' and introam_countries!='-1') or (introam_otherdet!='' and introam_otherdet!='-' and introam_otherdet!='-1')) and (ttype='Prepaid STV' or ttype='Prepaid Combo')) or ";
                }
                if (ChkNatRoaming.Checked == true)
                {
                    conditions = conditions + "((roam_call_voice_out>=0 or roam_call_voice_std>=0) and (ttype='Prepaid STV' or ttype='Prepaid Combo')) or ";
                }
                if (conditions.Substring(conditions.Length - 4, 4) == " or ")
                {
                    conditions = conditions.Substring(0, conditions.Length - 4);
                }
                conditions = conditions + ")";
                conditions = conditions.Replace(" and ()", "");


                string dailycaptype = "";
                /******/
                if (ChkDataTech1.Checked == true || ChkDataTech2.Checked == true || ChkDataTech3.Checked == true)
                {
                    conditions = conditions + " and (adddata_total2g > 999999 or ";  // for implementing 'OR' conditions, an additional condition is required.
                    if (ChkDataTech1.Checked == true)
                    {
                        conditions = conditions + "adddata_total2g>0 or ";
                        dailycaptype = dailycaptype + ChkDataTech1.Text.Replace("Data", "").Trim() + ",";
                    }
                    if (ChkDataTech2.Checked == true)
                    {
                        conditions = conditions + "adddata_total3g>0 or ";
                        dailycaptype = dailycaptype + ChkDataTech2.Text.Replace("Data", "").Trim() + ",";
                    }
                    if (ChkDataTech3.Checked == true)
                    {
                        conditions = conditions + "adddata_total4g>0 or ";
                        dailycaptype = dailycaptype + ChkDataTech3.Text.Replace("Data", "").Trim() + ",";
                    }
                    if (conditions.Substring(conditions.Length - 4, 4) == " or ")
                    {
                        conditions = conditions.Substring(0, conditions.Length - 4);
                    }
                    conditions = conditions + ")";
                    conditions = conditions.Replace(" and ()", "");

                    if (dailycaptype != "")
                    {
                        if (Convert.ToDouble(amount5a.Text.Trim()) == 0)
                        {
                            conditions = conditions + " and (adddata_daycap > 999999 ";   // for implementing 'OR' conditions, an additional condition is required.
                            string unlimcondition = "";
                            if (amount5b.Text.Trim() == "500")     // if upper limit is 500 GB, include unlimited data records also
                            {
                                unlimcondition = " or (adddata_daycap=-2)";
                            }
                            conditions = conditions + " or (((adddata_daycap > -1 and adddata_daycap <= " + Convert.ToDouble(amount5b.Text.Trim()) + " and adddata_unit='GB') or (adddata_daycap > -1 and adddata_daycap <= " + Math.Round(Convert.ToDouble(amount5b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                            conditions = conditions + ")";
                        }
                        else
                        {
                            conditions = conditions + " and (adddata_daycap > 999999 ";   // for implementing 'OR' conditions, an additional condition is required.
                            string unlimcondition = "";
                            if (amount5b.Text.Trim() == "500")     // if upper limit is 500 GB, include unlimited data records also
                            {
                                unlimcondition = " or (adddata_daycap=-2)";
                            }
                            conditions = conditions + " or (((adddata_daycap >= " + Convert.ToDouble(amount5a.Text.Trim()) + " and adddata_unit='GB') or (adddata_daycap >= " + Math.Round(Convert.ToDouble(amount5a.Text.Trim()) / 1000, 2) + " and adddata_unit='MB')) and ((adddata_daycap <= " + Convert.ToDouble(amount5b.Text.Trim()) + " and adddata_unit='GB') or (adddata_daycap <= " + Math.Round(Convert.ToDouble(amount5b.Text.Trim()) / 1000, 2) + " and adddata_unit='MB'))" + unlimcondition + ")";
                            conditions = conditions + ")";
                        }
                    }

                }


                // Advance Filter Conditions //

                if (Request.Form["RadProvider"] == "TSP")
                {
                    for (int i = 0; i < CheckAdvLocal.Items.Count; i++)
                    {
                        if (CheckAdvLocal.Items[i].Selected == true)
                        {
                            conditions = conditions + " and (" + CheckAdvLocal.Items[i].Value.ToString().Trim() + " != '-1')";
                        }
                    }
                    for (int i = 0; i < CheckAdvSTD.Items.Count; i++)
                    {
                        if (CheckAdvSTD.Items[i].Selected == true)
                        {
                            conditions = conditions + " and (" + CheckAdvSTD.Items[i].Value.ToString().Trim() + " != '-1')";
                        }
                    }
                    for (int i = 0; i < CheckAdvSMS.Items.Count; i++)
                    {
                        if (CheckAdvSMS.Items[i].Selected == true)
                        {
                            conditions = conditions + " and (" + CheckAdvSMS.Items[i].Value.ToString().Trim() + " != '-1')";
                        }
                    }
                    for (int i = 0; i < CheckAdvRoaming.Items.Count; i++)
                    {
                        if (CheckAdvRoaming.Items[i].Selected == true)
                        {
                            conditions = conditions + " and (" + CheckAdvRoaming.Items[i].Value.ToString().Trim() + " != '-1')";
                        }
                    }
                }

                if (Request.Form["RadProvider"] == "ISP")
                {
                    DateTime ISP_Dt1 = Convert.ToDateTime("1/1/2001");
                    DateTime ISP_Dt2 = DateTime.Now;
                    DateTime ISP_Dt3 = Convert.ToDateTime("1/1/2040");
                    double ISP_Data1 = -1;
                    double ISP_Data2 = 9999999;
                    try
                    {
                        ISP_Dt1 = Convert.ToDateTime(TextDate.Text.Trim());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        ISP_Dt2 = Convert.ToDateTime(TextDate2.Text.Trim());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        ISP_Dt3 = Convert.ToDateTime(TextDate3.Text.Trim());
                    }
                    catch (Exception ex) { }
                    string ISP_SSA = TextISPSSA.Text.Trim().Replace("'", "`");
                    try
                    {
                        ISP_Data1 = Convert.ToDouble(TextISPDataUsage1.Text.Trim());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        ISP_Data2 = Convert.ToDouble(TextISPDataUsage2.Text.Trim());
                    }
                    catch (Exception ex) { }

                    conditions = conditions + " and (actiondate>='" + ISP_Dt1.ToString("yyyy-MM-dd") + "' and actiondate<'" + ISP_Dt2.AddDays(1).ToString("yyyy-MM-dd") + "')";

                    if(ISP_SSA!="")
                    {
                        conditions = conditions + " and (upper(SSA) like '%" + ISP_SSA.ToUpper() + "%' or upper(SSA)='ALL INDIA')";
                    }

                    if(ChkISPFUP.Checked==true)
                    {
                        conditions = conditions + " and (upper(adddata_fup)='YES')";
                    }

                    if (ChkISPDataUnlim.Checked == true)
                    {
                        conditions = conditions + " and (adddata_total2g=-2)";
                    }
                    else
                    {
                        conditions = conditions + " and (adddata_total2g >=" + ISP_Data1 + " and adddata_total2g <=" + ISP_Data2 + ")";
                    }
                    
                    if(TextDate3.Text.Trim()!="")
                    {
                        conditions = conditions + " and (upper(regprom)='REGULAR' or (upper(regprom)='PROMOTIONAL' and offertill<='" + ISP_Dt3.ToString("yyyy-MM-dd") + "'))";
                    }

                }

                // Advance Filter Conditions - CODE ENDS HERE //


                TextConditions.Text = conditions;


                SetCurrentSliderValues(null, null); // code to retain the slider current values on postback
                divPopUpBg.Attributes.Clear();
                divSelection.InnerHtml = "";
                Button1.Visible = false;
                ButtonCancel.Visible = false;
                divPopShadow.Visible = false;
                divPopUp.Visible = false;


                showRecords(null, null);


                setManual(null, null);
            }

        }
        catch (Exception ex)
        {
            Response.Write("<br /><br /><br />" + ex.ToString());
        }
    }




    protected void showRecords(object sender, EventArgs e)
    {
        try
        {
            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            divresults.Controls.Clear();

            string conditions = TextConditions.Text.Trim();
            string sortby = TextSortBy.Text.Trim();
            if (sortby == "")
            {
                sortby = " order by ttype, mrp";
            }
            string myqry = "select * from " + tablename + " where(rno>0) " + conditions + sortby;
            //string countqry = "select count(*) from " + tablename + " where(rno>0) " + conditions;
            string countqry = "select count(distinct(uniqueid)) from " + tablename + " where(rno>0) " + conditions;

            double totcount = 0;
            double matching = 0;

            if (Request.Form["RadProvider"] == "ISP")
            {
                com = new MySqlCommand("select count(*) from " + tablename + " where(ttype!='Gen_ISD_Tariff' and ttype!='International_Roaming') and (upper(oper)!='AIRCEL' and upper(oper)!='AIRTEL' and upper(oper)!='BSNL' and upper(oper)!='IDEA' and upper(oper)!='JIO' and upper(oper)!='MTNL' and upper(oper)!='PLINTRON' and upper(oper)!='QUADRANT (CONNECT)' and upper(oper)!='TATA TELE' and upper(oper)!='TELENOR' and upper(oper)!='VODAFONE' and upper(oper)!='VODAFONE IDEA' and upper(oper)!='SURFTELECOM' and upper(oper)!='AEROVOYCE')", con);
            }
            else
            {
                //com = new MySqlCommand("select count(*) from " + tablename + " where(ttype!='Gen_ISD_Tariff' and ttype!='International_Roaming') and (upper(oper)='AIRCEL' or upper(oper)='AIRTEL' or upper(oper)='BSNL' or upper(oper)='IDEA' or upper(oper)='JIO' or upper(oper)='MTNL' or upper(oper)='QUADRANT (CONNECT)' or upper(oper)='TATA TELE' or upper(oper)='TELENOR' or upper(oper)='VODAFONE' or upper(oper)='VODAFONE IDEA' or upper(oper)='SURFTELECOM' or upper(oper)='AEROVOYCE')", con);
                com = new MySqlCommand("select count(*) from " + tablename + " where(ttype!='Gen_ISD_Tariff' and ttype!='International_Roaming') and (upper(oper)='AIRCEL' or upper(oper)='AIRTEL' or upper(oper)='BSNL' or upper(oper)='JIO' or upper(oper)='MTNL' or upper(oper)='QUADRANT (CONNECT)' or upper(oper)='TATA TELE' or upper(oper)='TELENOR' or upper(oper)='VODAFONE IDEA' or upper(oper)='SURFTELECOM' or upper(oper)='AEROVOYCE')", con);
            }
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            try
            {
                totcount = Convert.ToDouble(dr[0].ToString().Trim());
            }
            catch (Exception ex) { }
            con.Close();

            com = new MySqlCommand(countqry, con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            try
            {
                matching = Convert.ToDouble(dr[0].ToString().Trim());
            }
            catch (Exception ex) { }
            con.Close();

            divblank.Visible = false;

            string strsort = "<span>Sort By:</span>";
            
            if (Request.Form["RadProvider"] == "TSP")
            {
                strsort+=" <a class=\"sort-by-filter\" style=text-decoration:none; href=javascript:funsort('oper','asc'); >TSP</a> <a class=\"sort-by-filter\" style=text-decoration:none; href=javascript:funsort('ttype','asc'); >Product</a>";
            }
            else
            {
                strsort += " <a class=\"sort-by-filter\" style=text-decoration:none; href=javascript:funsort('oper','asc'); >ISP</a> <a class=\"sort-by-filter\" style=text-decoration:none; href=javascript:funsort('ttype','asc'); >Product</a>";
            }
            if (Request.Form["RadDevice"] == "Landline" || Request.Form["RadPrePost"] == "Postpaid")
            {
                strsort = strsort + " <a class=\"sort-by-filter\" style=text-decoration:none; href=javascript:funsort('isp_rental','asc'); >Monthly Rental</a> ";
            }
            else
            {
                strsort = strsort + " <a class=\"sort-by-filter\" style=text-decoration:none; href=javascript:funsort('mrp','asc'); >Price</a> ";
            }
            if (Request.Form["RadProvider"] == "TSP")
            {
                strsort = strsort + " <a class=\"sort-by-filter\" style=text-decoration:none; href=javascript:funsort('monval','asc'); >TalkTime</a>";
            }
            strsort = strsort + "  <a class=\"sort-by-filter\" style=text-decoration:none; href=javascript:funsort('validity','asc'); >Validity</a> ";

            divsort.InnerHtml = strsort;
            spanMatching.InnerHtml = "Found Matching " + matching.ToString() + " records from " + totcount.ToString() + " records";

            if (matching > 0)
            {
                divFilterButton.Visible = true;
                divsort.Visible = true;
                spanDownload.Visible = true;
                divresults.Visible = true;
            }
            else
            {
                divFilterButton.Visible = false;
                divsort.Visible = false;
                spanDownload.Visible = false;
                divresults.Visible = false;
            }

            if (matching > 5)
            {
                divFilterButton2.Visible = true;
                spanDownload2.Visible = true;
                divDownload2.Visible = true;
            }
            else
            {
                divFilterButton2.Visible = false;
                spanDownload2.Visible = false;
                divDownload2.Visible = false;
            }

            tbresults = new Table();
            tbresults.Width = System.Web.UI.WebControls.Unit.Percentage(99);
            tbresults.CellPadding = 5;
            tbresults.CellSpacing = 1;
            tbresults.BorderWidth = 0;

            int chkpos = 0;
            int moredivcntr = 0;


            // Below, connection is opened first, and then the 'net_write_timeout' and 'net_read_timeout' command is run before running the actual query.
            // This is done to avoid the buffer overload error ('MySql.Data.MySqlClient.MySqlException (0x80004005): Fatal error encountered during data read. ---> MySql.Data.MySqlClient.MySqlException (0x80004005): Reading from the stream has failed. ---> System.IO.IOException: Received an unexpected EOF or 0 bytes from the transport stream. at System.Net.FixedSizeReader.ReadPacket(Byte[] buffer, Int32 offset, Int32 count) .... ')

            con.Open();

            com = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", con);
            com.ExecuteNonQuery();

            string idlist = ",";
            com = new MySqlCommand(myqry, con);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                if (!idlist.Contains(dr["uniqueid"].ToString().Trim()))
                {
                    string oper = dr["oper"].ToString().Trim();
                    string operlogo = "logo" + oper.Replace(" ", "") + ".jpg";
                    string ttype = dr["ttype"].ToString().Replace("Prepaid_Plan Voucher", "Plan").Replace("Prepaid_STV", "STV").Replace("Prepaid_Combo", "Combo").Replace("Prepaid_Top Up", "Top Up").Replace("Prepaid_VAS", "VAS").Replace("Promo Offer", "Promo").Replace("Postpaid- Plan", "Plans").Replace("Postpaid- Add On Pack", "Add On Pack").Replace("Postpaid_VAS", "VAS").Replace("Fixed Line -Tariff", "Plan").Replace("Fixed Line Add-On Pack", "Add On Pack").Replace("Postpaid_VAS", "VAS");

                    TableRow tr = new TableRow();
                    tr.BackColor = System.Drawing.Color.FromName("#ffffff");
                    TableCell tc0 = new TableCell();
                    tc0.Attributes.Add("style", "border-top-left-radius: 10px; border-bottom-left-radius: 10px;box-shadow: 4px 4px 4px #bbbbbb;");
                    TableCell tc1 = new TableCell();
                    tc1.Attributes.Add("style", "box-shadow: 4px 4px 4px #bbbbbb;");
                    TableCell tc2 = new TableCell();
                    tc2.Attributes.Add("style", "border-top-right-radius: 10px; border-bottom-right-radius: 10px; padding-top:20px; box-shadow: 4px 4px 4px #bbbbbb;");

                    tc0.Width = System.Web.UI.WebControls.Unit.Percentage(5);
                    tc1.Width = System.Web.UI.WebControls.Unit.Percentage(15);
                    tc2.Width = System.Web.UI.WebControls.Unit.Percentage(80);
                    tc0.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    string css = "tablecell";
                    tc0.CssClass = css;
                    tc1.CssClass = css;
                    tc2.CssClass = css;

                    arrResult[chkpos].ID = ttype + "~" + dr["rno"].ToString().Trim();
                    arrResult[chkpos].Attributes.Add("onchange", "funComparison('" + ttype + "','" + arrResult[chkpos].ID.ToString() + "')");
                    tc0.Controls.Add(arrResult[chkpos]);
                    //tc1.Text = "<img src=logos2/" + operlogo + " border=0 />";
                    tc1.Text = "<img src=logos2/" + operlogo + " border=0 style=max-width:100%;max-height:78px;height:auto;text-align:center;margin:auto; />";

                    string price = "";
                    if (Request.Form["RadDevice"] == "Landline" || dr["ttype"].ToString().Trim() == "Postpaid Plan" || dr["ttype"].ToString().Trim() == "Postpaid Add On Pack")
                    {
                        if (dr["ISP_rental"].ToString().Trim() == "-1")
                        {
                            price = "-";
                        }
                        else
                        {
                            //price = "&#8377; " + dr["ISP_rental"].ToString().Trim();
                            //price = "&#x20B9; " + dr["ISP_rental"].ToString().Trim();
                            price = "<i class=\"fa fa-inr\"></i> " + dr["ISP_rental"].ToString().Trim();
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(dr["mrp"].ToString().Trim()) <= 0)
                        {
                            price = "-";
                        }
                        else
                        {
                            //price = "&#8377; " + dr["mrp"].ToString().Trim();
                            //price = "&#x20B9; " + dr["mrp"].ToString().Trim();
                            price = "<i class=\"fa fa-inr\"></i> " + dr["mrp"].ToString().Trim();
                        }
                    }

                    string talktime = "";
                    if (dr["monval"].ToString().Trim() == "-1")
                    {
                        talktime = "-";
                    }
                    else
                    {
                        //talktime = "&#8377; " + dr["monval"].ToString().Trim().Replace("-2","Unlimited");
                        //talktime = "&#x20B9; " + dr["monval"].ToString().Trim().Replace("-2", "Unlimited");
                        talktime = "<i class=\"fa fa-inr\"></i> " + dr["monval"].ToString().Trim().Replace("-2", "Unlimited");
                    }

                    string validity = "";
                    if (dr["validity"].ToString().Trim() == "0" || dr["validity"].ToString().Trim() == "-1")
                    {
                        validity = "-";
                    }
                    else
                    {
                        if (dr["validity"].ToString().Trim() == "-2")
                        {
                            validity = "Unlimited";
                        }
                        else
                        {
                            validity = dr["validity"].ToString().Trim() + " days";
                        }
                    }
                    string tdes = "";
                    tdes = tdes + "<div style=\"width:100%;text-align:left;\">";
                    tdes = tdes + "<p style=\"text-align:justify;font-size:13px; padding-right:20px;\">" + dr["tariffdet"].ToString().Trim().Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace(",", ", ").Replace("  ", " ") + "</p>";
                    tdes = tdes + "<table class=\"table borderless\" cellpadding=9 style=width:500px;>";
                    //tdes = tdes + "<thead><tr><th></th><th scope=\"col\">Product</th><th scope=\"col\">Price</th><th scope=\"col\">Talktime</th><th scope=\"col\">Validity</th></tr></thead>";
                    //tdes = tdes + "<tbody><tr><th></th><td><b> " + ttype + " </b></th><td>" + price + "</td><td>" + talktime + "</td><td>" + validity + "</td></tr></tbody>";
                    if (Request.Form["RadProvider"] == "TSP")
                    {
                        tdes = tdes + "<tr><td class=tableheadsmall2 align=left style=padding-right:30px;><b>Product</b></td><td class=tableheadsmall2 align=left style=padding-right:30px;><b>Price</b></td><td class=tableheadsmall2 align=left style=padding-right:30px;><b>Talktime</b></td><td class=tableheadsmall2 align=left style=padding-right:30px;><b>Validity</b></td></tr></thead>";
                        tdes = tdes + "<tr><td class=tableheadsmall2 align=left style=padding-right:30px;>" + ttype + "</td><td class=tableheadsmall2 align=left style=padding-right:30px;>" + price + "</td><td class=tableheadsmall2 align=left style=padding-right:30px;>" + talktime + "</td><td class=tableheadsmall2 align=left style=padding-right:30px;>" + validity + "</td></tr></tbody>";
                    }
                    else
                    {
                        tdes = tdes + "<tr><td class=tableheadsmall2 align=left style=padding-right:30px;><b>Product</b></td><td class=tableheadsmall2 align=left style=padding-right:30px;><b>Price</b></td><td class=tableheadsmall2 align=left style=padding-right:30px;><b>Validity</b></td></tr></thead>";
                        tdes = tdes + "<tr><td class=tableheadsmall2 align=left style=padding-right:30px;>" + ttype + "</td><td class=tableheadsmall2 align=left style=padding-right:30px;>" + price + "</td><td class=tableheadsmall2 align=left style=padding-right:30px;>" + validity + "</td></tr></tbody>";
                    }
                    tdes = tdes + "</table>";
                    tdes = tdes + "<a href=\"#demo" + moredivcntr.ToString().Trim() + "\" class=\"card-result-expand\" data-toggle=\"collapse\"> <span> <i class=\"fa fa-sort-down\"></i> </span> </a>";
                    tdes = tdes + "<div class=\"card-result-plus-sign\">";
                    tdes = tdes + "<div id=\"demo" + moredivcntr.ToString().Trim() + "\" class=\"collapse\">";
                    tdes = tdes + "<table width=\"100%\" cellspacing=\"1\" cellpadding=\"3\" border=\"1\" style=\"border-collapse:collapse;border-color:#cfcfcf;font-size: 13px;margin-top: 15px;\" bordercolor=\"#cfcfcf\" >";
                    tdes = tdes + "<tbody>";
                    if (Request.Form["RadProvider"] == "TSP")
                    {
                        tdes = tdes + "<tr><td class=tableheadsmall colspan=8 align=center><b>Local Call Details (INR / Pulse)</b></td></tr>";
                        tdes = tdes + "<tr><td class=tableheadsmall align=center>Mobile On Net<br />(Peak)</td><td class=tableheadsmall align=center>Mobile On Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Mobile Off Net<br />(Peak)</td><td class=tableheadsmall align=center>Mobile Off Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Fixed On Net<br />(Peak)</td><td class=tableheadsmall align=center>Fixed On Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Fixed Off Net<br />(Peak)</td><td class=tableheadsmall align=center>Fixed Off Net<br />(Off Peak)</td></tr>";
                        tdes = tdes + "<tr>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["local_on_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["local_on_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["local_off_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["local_off_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["local_fix_on_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["local_fix_on_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["local_fix_off_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["local_fix_off_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "</tr>";
                        tdes = tdes + "<tr><td class=tableheadsmall colspan=8 align=center><b>STD Call Details (INR / Pulse)</b></td></tr>";
                        tdes = tdes + "<tr><td class=tableheadsmall align=center>Mobile On Net<br />(Peak)</td><td class=tableheadsmall align=center>Mobile On Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Mobile Off Net<br />(Peak)</td><td class=tableheadsmall align=center>Mobile Off Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Fixed On Net<br />(Peak)</td><td class=tableheadsmall align=center>Fixed On Net<br />(Off Peak)</td><td class=tableheadsmall align=center>Fixed Off Net<br />(Peak)</td><td class=tableheadsmall align=center>Fixed Off Net<br />(Off Peak)</td></tr>";
                        tdes = tdes + "<tr>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["std_on_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["std_on_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["std_off_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["std_off_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["std_fix_on_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["std_fix_on_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["std_fix_off_voice_peak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["std_fix_off_voice_offpeak"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "</tr>";
                        tdes = tdes + "<tr><td class=tableheadsmall colspan=8 align=center><b>SMS Details (INR / SMS)</b></td></tr>";
                        tdes = tdes + "<tr><td class=tableheadsmall align=center>Local On Net</td><td class=tableheadsmall align=center>Local Off Net</td><td class=tableheadsmall align=center>National On Net</td><td class=tableheadsmall align=center>National Off Net</td><td class=tableheadsmall align=center>International</td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td></tr>";
                        tdes = tdes + "<tr>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["sms_local_on"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["sms_local_off"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["sms_nat_on"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["sms_nat_off"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["sms_int"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center></td>";
                        tdes = tdes + "<td class=tablecellsmall align=center></td>";
                        tdes = tdes + "<td class=tablecellsmall align=center></td>";
                        tdes = tdes + "</tr>";
                        tdes = tdes + "<tr><td class=tableheadsmall colspan=8 align=center><b>National Roaming (INR / Pulse)</b></td></tr>";
                        tdes = tdes + "<tr><td class=tableheadsmall align=center>Pulse (seconds)</td><td class=tableheadsmall align=center>Incoming</td><td class=tableheadsmall align=center>Local Outgoing</td><td class=tableheadsmall align=center>STD Outgoing</td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td><td class=tableheadsmall align=center></td></tr>";
                        tdes = tdes + "<tr>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["roam_call_pulse"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["roam_call_voice_in"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["roam_call_voice_out"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center>" + dr["roam_call_voice_std"].ToString().Trim().Replace("-1", "-") + "</td>";
                        tdes = tdes + "<td class=tablecellsmall align=center></td>";
                        tdes = tdes + "<td class=tablecellsmall align=center></td>";
                        tdes = tdes + "<td class=tablecellsmall align=center></td>";
                        tdes = tdes + "<td class=tablecellsmall align=center></td>";
                        tdes = tdes + "</tr>";
                    }

                    string totdata = "";
                        
                    if (Request.Form["RadProvider"] == "TSP")
                    {
                        tdes = tdes + "<tr><td class=tableheadsmall colspan=8 align=center><b>Total Data</b></td></tr>";
                        tdes = tdes + "<tr>";
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total2g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2G : " + dr["adddata_total2g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total3g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3G : " + dr["adddata_total3g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total4g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;4G : " + dr["adddata_total4g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_ISP"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dr["adddata_ISP"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                    }
                    else
                    {
                        tdes = tdes + "<tr><td class=tableheadsmall colspan=8 align=center><b>Data Speed</b></td></tr>";
                        tdes = tdes + "<tr>";
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total2g"].ToString().Trim() != "-1")
                        {
                            //totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Data Usage Limit With Higher Speed : " + dr["adddata_total2g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                            totdata = totdata + "<b>Data Usage Limit With Higher Speed :</b> " + dr["adddata_total2g"].ToString().Trim().Replace("-1", "0") + " " + "GB" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total3g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "<br /><br /><b>Speed of Connection Upto Data Usage Limit :</b> " + dr["adddata_total3g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total4g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "<br /><br /><b>Speed of Connection Beyond Data Usage Limit :</b> " + dr["adddata_total4g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        /*
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_ISP"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dr["adddata_ISP"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        */ 
                    }

                    if (totdata == "")
                    {
                        totdata = "-";
                    }
                    if (totdata.Contains("-2"))
                    {
                        totdata = "Unlimited";
                    }
                    tdes = tdes + "<td class=tablecellsmall align=left colspan=8>" + totdata + "</td>";
                    tdes = tdes + "</tr>";
                    if (dr["offerconditions"].ToString().Replace("-1", "") != "")
                    {
                        tdes = tdes + "<tr><td class=tablecellsmall align=left colspan=8><b>Eligibility Conditions : </b>" + dr["offerconditions"].ToString().Replace("-1", "") + "</td></tr>";
                    }
                    /*
                    if (dr["misc_remarks"].ToString().Replace("-1", "") != "")
                    {
                        tdes = tdes + "<tr><td class=tablecellsmall align=left colspan=8><b>Remarks : </b>" + dr["misc_remarks"].ToString().Replace("-1", "") + "</td></tr>";
                    }
                    */ 
                    if (dr["misc_terms"].ToString().Replace("-1", "") != "")
                    {
                        tdes = tdes + "<tr><td class=tablecellsmall align=left colspan=8><b>Terms & Conditions : </b>" + dr["misc_remarks"].ToString().Replace("-1", "") + "</td></tr>";
                    }
                    tdes = tdes + "</tbody>";
                    tdes = tdes + "</table>";
                    tdes = tdes + "</div>";

                    tc2.Text = tdes;


                    tr.Controls.Add(tc0);
                    tr.Controls.Add(tc1);
                    tr.Controls.Add(tc2);
                    tbresults.Controls.Add(tr);

                    // blank row
                    TableRow trcc = new TableRow();
                    trcc.Height = System.Web.UI.WebControls.Unit.Pixel(2);
                    TableCell tccc1 = new TableCell();
                    tccc1.ColumnSpan = 3;
                    //tccc1.CssClass = "tablecell";
                    tccc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tccc1.Text = "";
                    trcc.Controls.Add(tccc1);
                    tbresults.Controls.Add(trcc);

                    /*
                    TableRow trcc = new TableRow();
                    trcc.Height = System.Web.UI.WebControls.Unit.Pixel(1);
                    TableCell tccc1 = new TableCell();
                    tccc1.ColumnSpan = 7;
                    tccc1.CssClass = "tablecell";
                    tccc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tccc1.Text = moredet;
                    trcc.Controls.Add(tccc1);
                    tbresults.Controls.Add(trcc);
                    */

                    /*
                    TableRow trd = new TableRow();
                    TableCell tcd1 = new TableCell();
                    tcd1.ColumnSpan = 7;
                    tcd1.CssClass = "tablecell";
                    tcd1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tcd1.Text = "<hr size=0 color=#eaeaea />";
                    trd.Controls.Add(tcd1);
                    tbresults.Controls.Add(trd);
                    */


                    chkpos++;
                    moredivcntr++;
                    TextResultCntr.Text = chkpos.ToString();

                    idlist += dr["uniqueid"].ToString().Trim() + ",";
                }
            }
            con.Close();

            /*
            TableRow trb = new TableRow();
            TableCell tcb1 = new TableCell();
            TableCell tcb2 = new TableCell();
            tcb1.ColumnSpan = 5;
            tcb2.ColumnSpan = 2;
            tcb1.CssClass = "tablecell";
            tcb2.CssClass = "tablecell";
            tcb1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tcb2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            tcb2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Bottom;
            tcb1.Text = "<a href=javascript:funCompareSend() ><img src=images/btncompare.jpg width=100 border=0 /></a>";
            tcb2.Text = "";
            trb.Controls.Add(tcb1);
            trb.Controls.Add(tcb2);
            tbresults.Controls.Add(trb);
            */

            divresults.Controls.Add(tbresults);




            try
            {
                for (int i = 0; i < ChkPlans.Items.Count; i++)
                {
                    if (ChkPlans.Items[i].Text == "All Tariffs")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "All Tariffs");
                        ChkPlans.Items[i].Attributes.Add("style", "margin-right:10px;");
                    }
                    if (ChkPlans.Items[i].Text == "Plan Voucher")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Plan Voucher");
                    }
                    if (ChkPlans.Items[i].Text == "STV")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Special Tariff Voucher");
                    }
                    if (ChkPlans.Items[i].Text == "Combo")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Combo Pack");
                    }
                    if (ChkPlans.Items[i].Text == "Top Up")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Top Up Voucher");
                    }
                    if (ChkPlans.Items[i].Text == "")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "");
                    }
                    if (ChkPlans.Items[i].Text == "SUK")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Start Up Kit");
                    }
                    if (ChkPlans.Items[i].Text == "VAS")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Value Added Services");
                    }
                    if (ChkPlans.Items[i].Text == "Promo")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Promotional Offer");
                    }
                    if (ChkPlans.Items[i].Text == "Plans")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Postpaid Plans");
                    }
                    if (ChkPlans.Items[i].Text == "Add On Pack")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "Add On Pack");
                    }
                }
            }
            catch (Exception ex) { }

            ChkOper.Items[0].Attributes.Add("onClick", "javascript:chkAll();");
            for (int i = 1; i < ChkOper.Items.Count; i++)
            {
                ChkOper.Items[i].Attributes.Add("onClick", "javascript:unchk();");
            }
        }
        catch (Exception ex)
        {
            Response.Write("<br /><br /><br />" + ex.ToString());
        }
    }




    protected void ButtonPopUp_Click(object sender, EventArgs e)
    {
        try
        {
            divFilterButton.Visible = false;
            divsort.Visible = false;
            spanDownload.Visible = false;
            divFilterButton2.Visible = false;
            spanDownload2.Visible = false;
            divDownload2.Visible = false;
            divresults.Visible = false;

            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            int flag = 0;

            string mob = Request.Form["RadDevice"];
            string prepost = Request.Form["RadPrePost"];
            string circ = DropCircle.SelectedItem.Text.Trim();

            //string oper = "," + DropOperator.SelectedItem.Text.Trim() + ",";
            string oper = ",";
            for (int i = 0; i < ChkOper.Items.Count; i++)
            {
                if (ChkOper.Items[i].Selected == true)
                {
                    oper = oper + ChkOper.Items[i].Text.Trim() + ",";
                }
            }

            string plans = ",";
            string planssel = "";
            for (int i = 0; i < ChkPlans.Items.Count; i++)
            {
                if (ChkPlans.Items[i].Selected == true)
                {
                    plans = plans + ChkPlans.Items[i].Text.Trim() + ",";
                }
            }

            if (plans == ",")
            {
                flag = 1;
                Response.Write("<script>alert('Please select at least one tariff product type');</script>");
            }

            if (plans.Length > 3)
            {
                planssel = plans.Substring(1, plans.Length - 2);
            }

            if (mob == "Mobile" && prepost == "Prepaid")    // rename selection as per stored sheet names
            {
                plans = plans.Replace(",Plan Voucher,", ",Prepaid_Plan Voucher,");
                plans = plans.Replace(",STV,", ",Prepaid_STV,");
                plans = plans.Replace(",Combo,", ",Prepaid_Combo,");
                plans = plans.Replace(",Top Up,", ",Prepaid_Top Up,");
                plans = plans.Replace(",VAS,", ",Prepaid_VAS,");
                plans = plans.Replace(",Promo,", ",Promo Offer,");
            }
            if (mob == "Mobile" && prepost == "Postpaid")    // rename selection as per stored sheet names
            {
                plans = plans.Replace(",Plans,", ",Postpaid- Plan,");
                plans = plans.Replace(",Add On Pack,", ",Postpaid- Add On Pack,");
                plans = plans.Replace(",VAS,", ",Postpaid_VAS,");
                plans = plans.Replace(",Promo,", ",Promo Offer,");
            }
            if (mob == "Landline")    // rename selection as per stored sheet names
            {
                plans = plans.Replace(",Plans,", ",Fixed Line -Tariff,");
                plans = plans.Replace(",Add On Pack,", ",Fixed Line Add-On Pack,");
            }



            //if (circ == "CIRCLE")
            if (circ == "Select")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a circle');</script>");
            }

            if (oper.Contains("OPERATOR"))
            {
                flag = 1;
                Response.Write("<script>alert('Please select an operator');</script>");
            }

            if (flag == 0)
            {
                string myselection = "<table width=800px height=300px border=1 style=border-collapse:collapse;border-radius:10px; cellspacing=1 cellpadding=8>";
                myselection = myselection + "<tr><td class=tablehead colspan=2 style='background-color:#134882;color:#ffffff;text-align:center;'>You Have Selected The Following Options</td></tr>";
                if (Request.Form["RadProvider"] == "TSP")
                {
                    myselection = myselection + "<tr><td class=tableheadsmall width=50%>Mobile / Landline</td><td class=tableheadsmall>" + Request.Form["RadDevice"] + "</td></tr>";
                }
                myselection = myselection + "<tr><td class=tableheadsmall>Prepaid / Postpaid</td><td class=tableheadsmall>" + Request.Form["RadPrePost"] + "</td></tr>";
                myselection = myselection + "<tr><td class=tableheadsmall width=50%>Circle</td><td class=tableheadsmall>" + DropCircle.SelectedItem.Text.Trim() + "</td></tr>";
                string seloper = ",";
                for (int i = 0; i < ChkOper.Items.Count; i++)
                {
                    if (ChkOper.Items[i].Selected == true && !seloper.Contains(ChkOper.Items[i].Text.Trim()))
                    {
                        seloper = seloper + ChkOper.Items[i].Text.Trim() + ",";
                    }
                }
                if (seloper.Length > 3)
                {
                    seloper = seloper.Substring(1, seloper.Length - 2);
                }

                myselection = myselection + "<tr><td class=tableheadsmall width=50%>Operator(s)</td><td class=tableheadsmall>" + seloper + "</td></tr>";
                myselection = myselection + "<tr><td class=tableheadsmall>Tariff Types</td><td class=tableheadsmall>" + planssel + "</td></tr>";
                myselection = myselection + "<tr><td class=tableheadsmall>Price Range</td><td class=tableheadsmall>Rs. " + amount2a.Text.Trim() + " to Rs. " + amount2b.Text.Trim() + "</td></tr>";
                TextPrice1.Text = amount2a.Text.Trim();
                TextPrice2.Text = amount2b.Text.Trim();
                string datacaptypes = "";
                if (ChkDataTech1.Checked == true)
                {
                    datacaptypes = datacaptypes + ChkDataTech1.Text.Replace("Data", "").Trim() + "/";
                }
                if (ChkDataTech2.Checked == true)
                {
                    datacaptypes = datacaptypes + ChkDataTech2.Text.Replace("Data", "").Trim() + "/";
                } if (ChkDataTech3.Checked == true)
                {
                    datacaptypes = datacaptypes + ChkDataTech3.Text.Replace("Data", "").Trim() + "/";
                }
                if (datacaptypes != "")
                {
                    datacaptypes = datacaptypes.Substring(0, datacaptypes.Length - 1);
                    datacaptypes = " (" + datacaptypes + ")";
                }
                myselection = myselection + "<tr><td class=tableheadsmall>Total Data Capping</td><td class=tableheadsmall>" + amount3a.Text.Trim() + " GB to " + amount3b.Text.Trim() + " GB " + datacaptypes + "</td></tr>";

                if (divValidity.Visible == true)
                {
                    if (ChkValidityMore.Checked == true)
                    {
                        myselection = myselection + "<tr><td class=tableheadsmall>Validity</td><td class=tableheadsmall>More Than 365 Days</td></tr>";
                    }
                    else
                    {
                        myselection = myselection + "<tr><td class=tableheadsmall>Validity</td><td class=tableheadsmall>" + amount4a.Text.Trim() + " days to " + amount4b.Text.Trim() + " days</td></tr>";
                    }
                }
                string unlim = "";
                if (ChkUnlim_Local.Checked == true)
                {
                    unlim = unlim + ChkUnlim_Local.Text.Trim() + ", ";
                }
                if (ChkUnlim_STD.Checked == true)
                {
                    unlim = unlim + ChkUnlim_STD.Text.Trim() + ", ";
                }
                if (ChkUnlim_Roaming.Checked == true)
                {
                    unlim = unlim + ChkUnlim_Roaming.Text.Trim() + ", ";
                }
                if (unlim != "")
                {
                    unlim = unlim.Trim();
                    unlim = unlim.Substring(0, unlim.Length - 1);
                    myselection = myselection + "<tr><td class=tableheadsmall>Unlimited Calls</td><td class=tableheadsmall>" + unlim + "</td></tr>";
                }

                if (amount5a.Text.Trim() != "0" || amount5b.Text.Trim() != "500")
                {
                    myselection = myselection + "<tr><td class=tableheadsmall>Daily Data Capping</td><td class=tableheadsmall>" + amount5a.Text.Trim() + " GB to " + amount5b.Text.Trim() + " GB</td></tr>";
                }

                if (ChkFullTalktime.Checked == true)
                {
                    myselection = myselection + "<tr><td class=tableheadsmall>Talktime</td><td class=tableheadsmall>" + ChkFullTalktime.Text.Trim() + "</td></tr>";
                }

                string roam = "";
                if (ChkISDPack.Checked == true)
                {
                    roam = roam + ChkISDPack.Text.Trim() + " / ";
                }
                if (ChkISDRoaming.Checked == true)
                {
                    roam = roam + ChkISDRoaming.Text.Trim() + " / ";
                }
                if (ChkNatRoaming.Checked == true)
                {
                    roam = roam + ChkNatRoaming.Text.Trim() + " / ";
                }
                if (roam != "")
                {
                    roam = roam.Trim();
                    roam = roam.Substring(0, roam.Length - 1);
                    myselection = myselection + "<tr><td class=tableheadsmall>ISD / Roaming</td><td class=tableheadsmall>" + roam + "</td></tr>";
                }

                string adv = "";
                for (int i = 0; i < CheckAdvLocal.Items.Count; i++)
                {
                    if (CheckAdvLocal.Items[i].Selected == true)
                    {
                        adv = adv + "Local " + CheckAdvLocal.Items[i].Text.Trim() + ", ";
                    }
                }
                for (int i = 0; i < CheckAdvSTD.Items.Count; i++)
                {
                    if (CheckAdvSTD.Items[i].Selected == true)
                    {
                        adv = adv + "STD " + CheckAdvSTD.Items[i].Text.Trim() + ", ";
                    }
                }
                for (int i = 0; i < CheckAdvSMS.Items.Count; i++)
                {
                    if (CheckAdvSMS.Items[i].Selected == true)
                    {
                        adv = adv + "SMS - " + CheckAdvSMS.Items[i].Text.Trim() + ", ";
                    }
                }
                for (int i = 0; i < CheckAdvRoaming.Items.Count; i++)
                {
                    if (CheckAdvRoaming.Items[i].Selected == true)
                    {
                        adv = adv + "Roaming - " + CheckAdvRoaming.Items[i].Text.Trim() + ", ";
                    }
                }
                if (adv != "")
                {
                    adv = adv.Trim();
                    adv = adv.Substring(0, adv.Length - 1);
                    myselection = myselection + "<tr><td class=tableheadsmall>Advance Filters</td><td class=tableheadsmall>" + adv + "</td></tr>";
                }


                myselection = myselection + "</table><br /><br />";

                divPopUpBg.Attributes.Add("style", "background-color:#ffffff;");
                divSelection.InnerHtml = myselection;
                Button1.Visible = true;
                ButtonCancel.Visible = true;
                divPopShadow.Visible = true;
                divPopUp.Visible = true;



            }


        }
        catch (Exception ex)
        {
            Response.Write("<br /><br /><br />" + ex.ToString());
        }
    }




    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        try
        {
            getMaxRno("rno", "TRAI_downloadcounter");
            com = new MySqlCommand("insert into TRAI_downloadcounter values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','Excel')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            StringWriter strw = new StringWriter();
            StringBuilder strb = new StringBuilder();

            strb.Append("<table width=100% border=1 style=border-collapse:collapse cellspacing=1 cellpadding=5>");
            string conditions = TextConditions.Text.Trim();
            string sortby = " order by oper";
            //int totcols=266;

            try
            {

                // First Header Row //
                strb.Append("<tr>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=4><b>License Details</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b>One Time Charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Validity</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=4><b>Time For Call Rates</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=89><b>Call charges(Regular)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=11><b>Call charges while Roaming</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>SMS charges while Roaming</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=20><b>Charges while International Roaming</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=3><b>Data Charges (Please specify charges in paisa/ quantum of data transfer e.g 2 paisa / 10KB )</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b>Charges while Roaming  International</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b>Duration for Additional Benefits</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=61><b>Additional Benefits (Mins/Secs/SMS)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=13><b>Additional Data</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=11><b>Miscellaneous</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>One Time Charges, if any (specify whether convertible to security deposit)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=3><b>Details of the Service</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=6><b>Security Deposit (Refundable), if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=6><b>Fixed Charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=4><b>Optional fixed Monthly charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=6><b>Free calls in MCUS per month</b></td>");
                //strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=3><b></b></td>");
                strb.Append("</tr>");

                // Second Header Row //
                strb.Append("<tr>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=200><b>Tariff Product<b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=400><b>Tariff Summary<b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Operator Name</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Circle (Service Area)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Service (GSM / CDMA / LTE)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Category (Prepaid / Postpaid)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Name of the Product</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Unique ID's of the Plans for which exclusively applicable</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Date of Launch</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Regular / Promotional</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Time duration of Promotional / limited period offer starts from</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Time duration of Promotional / limited period offer valid till</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Eligibility Conditions, if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Price (Including Processing Fee & GST)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Monetory Value (in Rs.) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Product Validity ( in days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b>Peak Timing</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b>Off Peak Timing</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=4><b>All Local Call Charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>Local Call Charges Mobile OnNet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>Local Call Charges Mobile OffNet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>Local Call Charges Fixed OnNet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>Local Call Charges Fixed OffNet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of local call charges / pulse rate in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=4><b>All STD Call Charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>STD Call Charges Mobile OnNet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>STD Call Charges Mobile OffNet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>STD Call Charges Fixed OnNet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>STD Call Charges Fixed OffNet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of STD call charges / pulse rate in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=9><b>SMS Charges (in INR/SMS)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of SMS charges , in case SMS charges follow some other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=8><b>ISD call charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of ISD call charges / pulse rate in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Pulse (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b>Incoming</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b>Local Outgoing</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=2><b>STD Outgoing</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Web link for National outgoing ISD call charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Call Charges while Roaming in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Local</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>National</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>International</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Any other (please specify)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of SMS Charges while Roaming in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=20><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Charges while International Roaming in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Home</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Roaming</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Conditions pertaining to data charges , if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Monthly Rental for International roaming (in Rs.)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Weblink for Charges while international roaming</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Time From</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Time To</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>Additional Local (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Local in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>Additional STD (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional STD in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>Additional Local & STD (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Local & STD in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=3><b>Additional Local, STD & Roaming  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Local, STD & Roaming in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=7><b>Additional Roaming (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Roaming in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=11><b>Additional SMS</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional SMS in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=3><b>Additional ISD</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional ISD in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=8><b>Additional Video Call</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Video Call in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; colspan=12><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Data in case they follow any other pattern</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Special benefits , if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Other charges, if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Remarks, if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Terms and Conditions,  if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Mode of Activation / Recharge (Website / App only / paper / USSD / 3rd party Wallet etc)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Whether details of this service have been uploaded on the website</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>TSP website link of the Plan</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Any other declarations</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Condition for termination of Product if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Activation Code If Any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Deactivation Code If Any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Available in (Rural / Urban/ Both / Any other)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Registration charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Installation / activation charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>One time Security Deposit</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Plan enrolment fee, if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Other one time charges (Please specify)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>If yes please specify</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Pay per use model (Usage charges)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Any other model (Details)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Benefits </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Local</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Local + STD </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Local + STD + ISD</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>National Roaming</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>International Roaming</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Any Other, Please Specify</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Compulsory Fixed monthly charges including Rental / Minimum billing amount, if any  </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>If Exchange capacity wise FMC (Charges for exchange capacity <=999)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>If Exchange capacity wise FMC(Charges for exchange capacity> 999 and <=29999)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>If Exchange capacity wise FMC(Charges for exchange capacity >= 30000 and <=99999)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>If Exchange capacity wise FMC(Charges for exchange capacity >= 100000)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>If Flat FMC(Flat monthly charges)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Advance rental option for longer periods </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>CLIP</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Any other</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>If yes please specify</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month for exchange capacity <=999</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month for exchange capacity> 999 and <=29999</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month for exchange capacity >= 30000 and <=99999</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month for exchange capacity >= 100000</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Free talk value in Rs. (per month)</b></td>");
                //strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Details of Service</b></td>");
                //strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Benefits</b></td>");
                //strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Rental of Pack (Including Processing Fee & GST)</b></td>");
                strb.Append("</tr>");

                // Third Header Row //
                strb.Append("<tr>");
                for (int i = 1; i <= 16; i++)
                {
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");   // blank cells
                }
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>From</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>To</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>From</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>To</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs) (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs) (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs) (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs) (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs) (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs) (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>All Local</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>All National</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Onnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Offnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>National Onnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>National Offnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>International</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Terms and conditions including conditions pertaining to SMS charges, if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Countries List </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Pulse Rate (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Calls to Mobile  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD calls to Landline  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Video calls  (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Weblink for ISD call charges</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Conditions, if any</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Voice (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Video (in INR/pulse)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>in INR/SMS</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>in INR/SMS</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>in INR/SMS</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Countries</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Incoming Pulse (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Incoming Calls</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Outgoing Local Calls (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing  Local Calls</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Outgoing Calls to India (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing Calls to India</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Outgoing Calls to Other Countries (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing Calls to Other Countries</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Video call (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing SMS</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Incoming SMS</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Incoming Free Usage (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Incoming Free Usage (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Outgoing Free Usage (in seconds)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing Free Usage (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Unit  Free Data Usage</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Free Data Usage</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Free SMS</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                for (int i = 1; i <= 9; i++)
                {
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                }
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Onnet  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Offnet  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Mobile  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Onnet Mobile  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Offnet Mobile  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD Onnet  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD Offnet  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD Mobile  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD Onnet Mobile  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD Offnet Mobile  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Onnet (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Offnet (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Mobile (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Onnet Mobile (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Offnet Mobile (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local, STD & Roaming (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local, STD & Roaming Mobile (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Incoming & Outgoing (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Incoming Free (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Outgoing Free (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Outgoing Local & STD Mobile Free  (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Outgoing Local Free (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Outgoing STD Free (in minutes)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & National</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & National Onnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & National Offnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Onnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Offnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>National</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>National Onnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>National Offnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>International</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Summary of ISD freebies</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Weblink for ISD freebies</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Video</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Video</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Video Onnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Local Video Offnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD Video</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD Video Onnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>STD Video Offnet</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                if (Request.Form["RadProvider"] == "TSP")
                {
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Unit (MB / GB)</b></td>");
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Total 2G Data</b></td>");
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Total 3G Data</b></td>");
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Total 4G Data</b></td>");
                }
                else
                {
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Data Speed - Unit (kbps / mbps)</b></td>");
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Data Usage Limit With Higher Speed (GB)</b></td>");
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Speed of Connection Upto Data Usage Limit</b></td>");
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Speed of Connection Beyond Data Usage Limit</b></td>");
                }
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Total Data</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Day/Night Data Capping</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Weekly Data Capping</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Monthly Data Capping</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Carry Forward (Yes / No)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>FUP, if any (Yes/No)</b></td>");
                strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b>Condition, if any</b></td>");
                for (int i = 1; i <= 44; i++)
                {
                    strb.Append("<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>");
                }
                strb.Append("</tr>");



                // Data Rows //
                
                // Below, connection is opened first, and then the 'net_write_timeout' and 'net_read_timeout' command is run before running the actual query.
                // This is done to avoid the buffer overload error ('MySql.Data.MySqlClient.MySqlException (0x80004005): Fatal error encountered during data read. ---> MySql.Data.MySqlClient.MySqlException (0x80004005): Reading from the stream has failed. ---> System.IO.IOException: Received an unexpected EOF or 0 bytes from the transport stream. at System.Net.FixedSizeReader.ReadPacket(Byte[] buffer, Int32 offset, Int32 count) .... ')

                con.Open();

                com = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", con);
                com.ExecuteNonQuery();

                string idlist = ",";
                com = new MySqlCommand("select * from " + tablename + " where(rno>0) " + conditions + sortby, con);
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (!idlist.Contains(dr["uniqueid"].ToString().Trim()))
                    {
                        strb.Append("<tr>");
                        for (int i = 4; i <= 5; i++)     // 'Tariff Product' till 'Tariff Summary' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)) + "</td>");
                        }
                        for (int i = 7; i <= 8; i++)     // 'Operator' till 'Circle' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)) + "</td>");
                        }
                        strb.Append("<td align=center valign=top>" + dr[10].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");    // 'Service'
                        strb.Append("<td align=center valign=top>" + dr[11].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");    // 'Category'
                        strb.Append("<td align=center valign=top>" + dr[14].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");    // 'Plan Name'
                        if (dr["ttype"].ToString().Trim() == "PREPAID PLAN" || dr["ttype"].ToString().Trim().ToUpper() == "POSTPAID PLAN")
                        {
                            //strb.Append("<td align=center valign=top>" + dr[15].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>");    // 'Unique ID of Plan'
                            strb.Append("<td align=center valign=top></td>");
                        }
                        else
                        {
                            //strb.Append("<td align=center valign=top></td>");
                            strb.Append("<td align=center valign=top>" + dr[15].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");    // 'Unique ID of Plan for which exclusively available'
                        }

                        if (Convert.ToDateTime(dr[17].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                        {
                            strb.Append("<td align=center valign=top>" + Convert.ToDateTime(dr[17].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>");      // 'Date of Launch / Activation etc'
                        }
                        else
                        {
                            strb.Append("<td align=center valign=top></td>");
                        }

                        strb.Append("<td align=center valign=top>" + dr[18].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");    // 'Regular/Promotional'

                        if (Convert.ToDateTime(dr[19].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                        {
                            strb.Append("<td align=center valign=top>" + Convert.ToDateTime(dr[19].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>");   // 'Time duration of Promotional / limited period offer starts from'
                        }
                        else
                        {
                            strb.Append("<td align=center valign=top></td>");
                        }
                        if (Convert.ToDateTime(dr[20].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                        {
                            strb.Append("<td align=center valign=top>" + Convert.ToDateTime(dr[20].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>");      // 'Time duration of Promotional / limited period offer valid till'
                        }
                        else
                        {
                            strb.Append("<td align=center valign=top></td>");
                        }

                        for (int i = 21; i <= 23; i++)     // 'Eligibility Conditions' till 'Monetory Value' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");
                        }
                        strb.Append("<td align=center valign=top>" + dr[51].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");  // 'Validity'


                        for (int i = 55; i <= 58; i++)     // 'Time for call rates' columns
                        {
                            if (dr[i].ToString().Trim() != "")
                            {
                                try
                                {
                                    strb.Append("<td align=center valign=top>" + Convert.ToDouble(dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "")).ToString("0.00").Replace(".", ":") + " " + "</td>");
                                }
                                catch (Exception ex)
                                {
                                    strb.Append("<td align=center valign=top></td>");
                                }
                            }
                            else
                            {
                                strb.Append("<td align=center valign=top></td>");
                            }
                        }
                        for (int i = 59; i <= 279; i++)     // 'Local All' till 'Misc - Deactivation Code' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");
                        }
                        for (int i = 24; i <= 27; i++)     // 'Rural / Urban' till 'Installation / Activation Charges' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");
                        }
                        for (int i = 30; i <= 31; i++)     // 'Plan Enrollment Fee' till 'othercharges' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");
                        }
                        strb.Append("<td align=center valign=top></td>");  // 'OtherChargesDet' - field has been removed now, so show a blank column

                        for (int i = 52; i <= 54; i++)     // 'Pay Per Use' till 'modelbenefits' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");
                        }
                        for (int i = 34; i <= 39; i++)     // 'Security Deposit' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");
                        }
                        //strb.Append("<td align=center valign=top>" + dr[42].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>");   // 'Fixed Monthly Charges'
                        strb.Append("<td align=center valign=top></td>");   // 'Fixed Monthly Charges' - field has been removed, so show blank column
                        for (int i = 40; i <= 43; i++)     // 'Fixed <=999' till 'Fixed100K' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");
                        }
                        for (int i = 1; i <= 2; i++)     // 'FLAT FMC' and 'Advance Rental option' - fields have been removed, so show blank columns
                        {
                            strb.Append("<td align=center valign=top></td>");
                        }
                        strb.Append("<td align=center valign=top>" + dr[44].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");   // 'Fixed - CLIP'
                        strb.Append("<td align=center valign=top></td>");   // 'Fixed - Other Charges' - field have been removed, so show blank columns
                        strb.Append("<td align=center valign=top></td>");       // 'Optional Fixed Monthly Charges - If yes please specify'
                        for (int i = 45; i <= 50; i++)     // 'Fixed Calls MCU - Free Calls' till 'Free Talk Value in month' columns
                        {
                            strb.Append("<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "-2").Replace("-1", "") + "</td>");
                        }

                        strb.Append("</tr>");

                        idlist += dr["uniqueid"].ToString().Trim() + ",";
                    }
                }
                con.Close();

                strb.Append("</table>");
            }
            catch (Exception ex)
            {
                Response.Write("<br /><br /><br />" + ex.ToString());
            }

            try
            {
                //string attachment = "attachment; filename=TariffProducts.xls";
                string attachment = "attachment; filename=Report.xls";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";

                Response.Write(strb.ToString());

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







    protected void ButtonClearFilters_Click(object sender, EventArgs e)
    {
        try
        {
            /*
            ChkDataTech1.Checked = false;
            ChkDataTech2.Checked = false;
            ChkDataTech3.Checked = false;

            ChkValidityMore.Checked = false;

            ChkUnlim_Local.Checked = false;
            ChkUnlim_STD.Checked = false;
            ChkUnlim_Roaming.Checked = false;

            ChkFullTalktime.Checked = false;

            ChkISDPack.Checked = false;
            ChkISDRoaming.Checked = false;
            ChkNatRoaming.Checked = false;

            for(int g=0;g<CheckAdvLocal.Items.Count;g++)
            {
                CheckAdvLocal.Items[g].Selected = false;
            }

            for (int g = 0; g < CheckAdvSTD.Items.Count; g++)
            {
                CheckAdvSTD.Items[g].Selected = false;
            }

            for (int g = 0; g < CheckAdvSMS.Items.Count; g++)
            {
                CheckAdvSMS.Items[g].Selected = false;
            }

            for (int g = 0; g < CheckAdvRoaming.Items.Count; g++)
            {
                CheckAdvRoaming.Items[g].Selected = false;
            }

            
            for (int g = 0; g < ChkPlans.Items.Count; g++)
            {
                ChkPlans.Items[g].Selected = false;
            }
            ChkPlans.Items[0].Selected = true;

            amount2a.Text = "0";
            amount2b.Text = "1000";
            amount3a.Text = "0";
            amount3b.Text = "500";
            amount4a.Text = "0";
            amount4b.Text = "365";
            amount5a.Text = "0";
            amount5b.Text = "500";

            showRecords(null, null);
            */

            Response.Redirect("index.aspx");

        }
        catch (Exception ex)
        {

        }
    }








    protected void ButtonXML_Click(object sender, EventArgs e)
    {
        try
        {
            getMaxRno("rno", "TRAI_downloadcounter");
            com = new MySqlCommand("insert into TRAI_downloadcounter values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','XML')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();



            string conditions = TextConditions.Text.Trim();
            string sortby = " order by oper";




            try
            {

                StringBuilder sb = new StringBuilder();

                sb.Append("<TariffProducts>");

                
                // Below, connection is opened first, and then the 'net_write_timeout' and 'net_read_timeout' command is run before running the actual query.
                // This is done to avoid the buffer overload error ('MySql.Data.MySqlClient.MySqlException (0x80004005): Fatal error encountered during data read. ---> MySql.Data.MySqlClient.MySqlException (0x80004005): Reading from the stream has failed. ---> System.IO.IOException: Received an unexpected EOF or 0 bytes from the transport stream. at System.Net.FixedSizeReader.ReadPacket(Byte[] buffer, Int32 offset, Int32 count) .... ')

                con.Open();

                com = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", con);
                com.ExecuteNonQuery();

                string idlist = ",";
                com = new MySqlCommand("select * from " + tablename + " where(rno>0) " + conditions + sortby, con);
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if(!idlist.Contains(dr["uniqueid"].ToString().Trim()))
                    {
                        sb.Append("<TariffProduct>");
                        sb.Append("<ProductType>" + dr["ttype"].ToString().Trim() + "</ProductType>");
                        sb.Append("<TariffSummary>" + dr["tariffdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "</TariffSummary>");
                        sb.Append("<Operator>" + dr["oper"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "</Operator>");
                        sb.Append("<Circle>" + dr["circ"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "</Circle>");
                        sb.Append("<Service>" + dr["service"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</Service>");
                        sb.Append("<Category>" + dr["categ"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</Category>");
                        sb.Append("<ProductName>" + dr["planname"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</ProductName>");
                        sb.Append("<ApplicableToPlan>" + dr["planid"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "</ApplicableToPlan>");
                        DateTime actiondate = Convert.ToDateTime("1/1/2001");
                        try
                        {
                            actiondate = Convert.ToDateTime(dr["actiondate"].ToString().Trim());
                        }
                        catch (Exception ex) { }
                        if (actiondate >= Convert.ToDateTime("2/1/2001"))
                        {
                            sb.Append("<LaunchDate>" + actiondate.ToString("dd-MMM-yyyy") + "</LaunchDate>");
                        }
                        else
                        {
                            sb.Append("<LaunchDate></LaunchDate>");
                        }
                        sb.Append("<RegularORPromo>" + dr["regprom"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</RegularORPromo>");
                        DateTime fromdate = Convert.ToDateTime("1/1/2001");
                        try
                        {
                            fromdate = Convert.ToDateTime(dr["offerfrom"].ToString().Trim());
                        }
                        catch (Exception ex) { }
                        if (fromdate >= Convert.ToDateTime("2/1/2001"))
                        {
                            sb.Append("<PromoFrom>" + fromdate.ToString("dd-MMM-yyyy") + "</PromoFrom>");
                        }
                        else
                        {
                            sb.Append("<PromoFrom></PromoFrom>");
                        }
                        DateTime tilldate = Convert.ToDateTime("1/1/2001");
                        try
                        {
                            tilldate = Convert.ToDateTime(dr["offertill"].ToString().Trim());
                        }
                        catch (Exception ex) { }
                        if (tilldate >= Convert.ToDateTime("2/1/2001"))
                        {
                            sb.Append("<PromoTill>" + tilldate.ToString("dd-MMM-yyyy") + "</PromoTill>");
                        }
                        else
                        {
                            sb.Append("<PromoTill></PromoTill>");
                        }
                        sb.Append("<Eligibility>" + dr["offerconditions"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ") + "</Eligibility>");
                        sb.Append("<Price>" + Math.Round(Convert.ToDouble(dr["mrp"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("-1", "") + "</Price>");
                        sb.Append("<MonetaryValue>" + Math.Round(Convert.ToDouble(dr["mrp"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</MonetaryValue>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        if (Convert.ToDouble(dr["call_peaktimefrom"].ToString().Trim()) >= 0)
                        {
                            sb.Append("<PeakTimeFrom>" + Math.Round(Convert.ToDouble(dr["call_peaktimefrom"].ToString().Trim()), 2).ToString("0.00").Replace(".", ":").Replace("-2", "Unlimited").Replace("-1", "") + "</PeakTimeFrom>");
                        }
                        else
                        {
                            sb.Append("<PeakTimeFrom></PeakTimeFrom>");
                        }
                        if (Convert.ToDouble(dr["call_peaktimetill"].ToString().Trim()) >= 0)
                        {
                            sb.Append("<PeakTimeTo>" + Math.Round(Convert.ToDouble(dr["call_peaktimetill"].ToString().Trim()), 2).ToString("0.00").Replace(".", ":").Replace("-2", "Unlimited").Replace("-1", "") + "</PeakTimeTo>");
                        }
                        else
                        {
                            sb.Append("<PeakTimeTo></PeakTimeTo>");
                        }
                        if (Convert.ToDouble(dr["call_offpeaktimefrom"].ToString().Trim()) >= 0)
                        {
                            sb.Append("<OffPeakTimeFrom>" + Math.Round(Convert.ToDouble(dr["call_offpeaktimefrom"].ToString().Trim()), 2).ToString("0.00").Replace(".", ":").Replace("-2", "Unlimited").Replace("-1", "") + "</OffPeakTimeFrom>");
                        }
                        else
                        {
                            sb.Append("<OffPeakTimeFrom></OffPeakTimeFrom>");
                        }
                        if (Convert.ToDouble(dr["call_offpeaktimetill"].ToString().Trim()) >= 0)
                        {
                            sb.Append("<OffPeakTimeTo>" + Math.Round(Convert.ToDouble(dr["call_offpeaktimetill"].ToString().Trim()), 2).ToString("0.00").Replace(".", ":").Replace("-2", "Unlimited").Replace("-1", "") + "</OffPeakTimeTo>");
                        }
                        else
                        {
                            sb.Append("<OffPeakTimeTo></OffPeakTimeTo>");
                        }

                        sb.Append("<CallChargesRegular>");

                        // All Local Call Charges //
                        sb.Append("<AllLocal>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["local_all_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["local_all_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallCharges>" + Math.Round(Convert.ToDouble(dr["local_all_voicecharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallCharges>");
                        sb.Append("<VideoCallCharges>" + Math.Round(Convert.ToDouble(dr["local_all_videocharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallCharges>");
                        sb.Append("</AllLocal>");

                        // Local Call Charges Mobile OnNet //
                        sb.Append("<LocalMobileOnNet>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["local_on_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["local_on_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_on_voice_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallPeakCharges>");
                        sb.Append("<VideoCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_on_video_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallPeakCharges>");
                        sb.Append("<VoiceCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_on_voice_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallOffPeakCharges>");
                        sb.Append("<VideoCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_on_video_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallOffPeakCharges>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["local_on_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</LocalMobileOnNet>");

                        // Local Call Charges Mobile OffNet //
                        sb.Append("<LocalMobileOffNet>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["local_off_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["local_off_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_off_voice_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallPeakCharges>");
                        sb.Append("<VideoCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_off_video_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallPeakCharges>");
                        sb.Append("<VoiceCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_off_voice_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallOffPeakCharges>");
                        sb.Append("<VideoCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_off_video_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallOffPeakCharges>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["local_off_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</LocalMobileOffNet>");

                        // Local Call Charges Fixed OnNet //
                        sb.Append("<LocalFixedOnNet>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["local_fix_on_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["local_fix_on_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_fix_on_voice_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallPeakCharges>");
                        sb.Append("<VideoCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_fix_on_video_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallPeakCharges>");
                        sb.Append("<VoiceCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_fix_on_voice_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallOffPeakCharges>");
                        sb.Append("<VideoCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_fix_on_video_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallOffPeakCharges>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["local_fix_on_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</LocalFixedOnNet>");

                        // Local Call Charges Fixed OffNet //
                        sb.Append("<LocalFixedOffNet>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["local_fix_off_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["local_fix_off_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_fix_off_voice_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallPeakCharges>");
                        sb.Append("<VideoCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_fix_off_video_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallPeakCharges>");
                        sb.Append("<VoiceCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_fix_off_voice_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallOffPeakCharges>");
                        sb.Append("<VideoCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["local_fix_off_video_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallOffPeakCharges>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["local_fix_off_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</LocalFixedOffNet>");

                        sb.Append("<LocalCUP>" + dr["local_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</LocalCUP>");
                        sb.Append("<LocalOtherDescription>" + dr["local_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</LocalOtherDescription>");


                        // All STD Call Charges //
                        sb.Append("<AllSTD>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["std_all_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["std_all_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallCharges>" + Math.Round(Convert.ToDouble(dr["std_all_voicecharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallCharges>");
                        sb.Append("<VideoCallCharges>" + Math.Round(Convert.ToDouble(dr["std_all_videocharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallCharges>");
                        sb.Append("</AllSTD>");

                        // STD Call Charges Mobile OnNet //
                        sb.Append("<STDMobileOnNet>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["std_on_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["std_on_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_on_voice_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallPeakCharges>");
                        sb.Append("<VideoCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_on_video_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallPeakCharges>");
                        sb.Append("<VoiceCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_on_voice_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallOffPeakCharges>");
                        sb.Append("<VideoCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_on_video_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallOffPeakCharges>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["std_on_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</STDMobileOnNet>");

                        // STD Call Charges Mobile OffNet //
                        sb.Append("<STDMobileOffNet>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["std_off_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["std_off_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_off_voice_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallPeakCharges>");
                        sb.Append("<VideoCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_off_video_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallPeakCharges>");
                        sb.Append("<VoiceCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_off_voice_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallOffPeakCharges>");
                        sb.Append("<VideoCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_off_video_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallOffPeakCharges>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["std_off_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</STDMobileOffNet>");

                        // STD Call Charges Fixed OnNet //
                        sb.Append("<STDFixedOnNet>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["std_fix_on_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["std_fix_on_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_fix_on_voice_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallPeakCharges>");
                        sb.Append("<VideoCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_fix_on_video_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallPeakCharges>");
                        sb.Append("<VoiceCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_fix_on_voice_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallOffPeakCharges>");
                        sb.Append("<VideoCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_fix_on_video_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallOffPeakCharges>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["std_fix_on_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</STDFixedOnNet>");

                        // STD Call Charges Fixed OffNet //
                        sb.Append("<STDFixedOffNet>");
                        sb.Append("<VoicePulse>" + Math.Round(Convert.ToDouble(dr["std_fix_off_voicepulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoicePulse>");
                        sb.Append("<VideoPulse>" + Math.Round(Convert.ToDouble(dr["std_fix_off_videopulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoPulse>");
                        sb.Append("<VoiceCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_fix_off_voice_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallPeakCharges>");
                        sb.Append("<VideoCallPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_fix_off_video_peak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallPeakCharges>");
                        sb.Append("<VoiceCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_fix_off_voice_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VoiceCallOffPeakCharges>");
                        sb.Append("<VideoCallOffPeakCharges>" + Math.Round(Convert.ToDouble(dr["std_fix_off_video_offpeak"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</VideoCallOffPeakCharges>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["std_fix_off_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</STDFixedOffNet>");

                        sb.Append("<STDCUP>" + dr["std_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</STDCUP>");
                        sb.Append("<STDOtherDescription>" + dr["std_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</STDOtherDescription>");


                        // SMS Charges //
                        sb.Append("<SMSCharges>");
                        sb.Append("<AllLocal>" + Math.Round(Convert.ToDouble(dr["sms_all_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AllLocal>");
                        sb.Append("<AllNational>" + Math.Round(Convert.ToDouble(dr["sms_all_national"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AllNational>");
                        sb.Append("<LocalOnNet>" + Math.Round(Convert.ToDouble(dr["sms_local_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOnNet>");
                        sb.Append("<LocalOffNet>" + Math.Round(Convert.ToDouble(dr["sms_local_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOffNet>");
                        sb.Append("<NationalOnNet>" + Math.Round(Convert.ToDouble(dr["sms_nat_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</NationalOnNet>");
                        sb.Append("<NationalOffNet>" + Math.Round(Convert.ToDouble(dr["sms_nat_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</NationalOffNet>");
                        sb.Append("<International>" + Math.Round(Convert.ToDouble(dr["sms_int"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</International>");
                        sb.Append("<Terms>" + dr["sms_terms"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</Terms>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["sms_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</SMSCharges>");

                        sb.Append("<SMSCUP>" + dr["sms_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</SMSCUP>");
                        sb.Append("<SMSOtherDescription>" + dr["sms_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</SMSOtherDescription>");


                        // ISD Call Charges //
                        sb.Append("<ISDCallCharges>");
                        sb.Append("<Countries>" + dr["isd_countries"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</Countries>");
                        sb.Append("<PulseRateInSeconds>" + Math.Round(Convert.ToDouble(dr["isd_pulserate"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</PulseRateInSeconds>");
                        sb.Append("<ISDRateMobile>" + Math.Round(Convert.ToDouble(dr["isd_mobile"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDRateMobile>");
                        sb.Append("<ISDRateLandline>" + Math.Round(Convert.ToDouble(dr["isd_landline"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDRateLandline>");
                        sb.Append("<ISDRateVideoCalls>" + Math.Round(Convert.ToDouble(dr["isd_video"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDRateVideoCalls>");
                        sb.Append("<WebLink>" + dr["isd_weblink"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</WebLink>");
                        sb.Append("<Conditions>" + dr["isd_conditions"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Conditions>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["isd_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</ISDCallCharges>");

                        sb.Append("<ISDCUP>" + dr["isd_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</ISDCUP>");
                        sb.Append("<ISDOtherDescription>" + dr["isd_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</ISDOtherDescription>");


                        sb.Append("</CallChargesRegular>");


                        // Call Charges While Roaming //
                        sb.Append("<RoamingCallCharges>");
                        sb.Append("<PulseRateInSeconds>" + Math.Round(Convert.ToDouble(dr["roam_call_pulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</PulseRateInSeconds>");
                        sb.Append("<IncomingVoiceRate>" + Math.Round(Convert.ToDouble(dr["roam_call_voice_in"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</IncomingVoiceRate>");
                        sb.Append("<IncomingVideoRate>" + Math.Round(Convert.ToDouble(dr["roam_call_video_in"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</IncomingVideoRate>");
                        sb.Append("<LocalOutgoingVoiceRate>" + Math.Round(Convert.ToDouble(dr["roam_call_voice_out"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOutgoingVoiceRate>");
                        sb.Append("<LocalOutgoingVideoRate>" + Math.Round(Convert.ToDouble(dr["roam_call_video_out"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOutgoingVideoRate>");
                        sb.Append("<STDOutgoingVoiceRate>" + Math.Round(Convert.ToDouble(dr["roam_call_voice_std"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDOutgoingVoiceRate>");
                        sb.Append("<STDOutgoingVideoRate>" + Math.Round(Convert.ToDouble(dr["roam_call_video_std"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDOutgoingVideoRate>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["roam_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<WebLink>" + dr["roam_weblink"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</WebLink>");
                        sb.Append("<RoamingCUP>" + dr["roam_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</RoamingCUP>");
                        sb.Append("<RoamingOtherDescription>" + dr["roam_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</RoamingOtherDescription>");
                        sb.Append("</RoamingCallCharges>");


                        // SMS Charges While Roaming //
                        sb.Append("<RoamingSMSCharges>");
                        sb.Append("<Local>" + Math.Round(Convert.ToDouble(dr["roam_sms_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Local>");
                        sb.Append("<National>" + Math.Round(Convert.ToDouble(dr["roam_sms_nat"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</National>");
                        sb.Append("<International>" + Math.Round(Convert.ToDouble(dr["roam_sms_int"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</International>");
                        sb.Append("<AnyOther>" + Math.Round(Convert.ToDouble(dr["roam_sms_other"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AnyOther>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["roam_sms_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<CUP>" + dr["roam_sms_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</CUP>");
                        sb.Append("<OtherDescription>" + dr["roam_sms_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</RoamingSMSCharges>");


                        // Charges while International Roaming //
                        sb.Append("<InternationalRoaming>");
                        sb.Append("<Countries>" + dr["introam_countries"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Countries>");
                        sb.Append("<IncomingPulseInSeconds>" + Math.Round(Convert.ToDouble(dr["introam_in_pulse"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</IncomingPulseInSeconds>");
                        sb.Append("<ISDIncoming>" + Math.Round(Convert.ToDouble(dr["introam_in_calls"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDIncoming>");
                        sb.Append("<OutgoingPulseInSeconds>" + Math.Round(Convert.ToDouble(dr["introam_pulse_outlocal"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</OutgoingPulseInSeconds>");
                        sb.Append("<ISDOutgoing>" + Math.Round(Convert.ToDouble(dr["introam_calls_outlocal"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDOutgoing>");
                        sb.Append("<ISDPulseToIndia>" + Math.Round(Convert.ToDouble(dr["introam_pulse_outindia"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDPulseToIndia>");
                        sb.Append("<ISDOutgoingToIndia>" + Math.Round(Convert.ToDouble(dr["introam_calls_outindia"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDOutgoingToIndia>");
                        sb.Append("<ISDPulseToOther>" + Math.Round(Convert.ToDouble(dr["introam_pulse_other"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDPulseToOther>");
                        sb.Append("<ISDOutgoingToOther>" + Math.Round(Convert.ToDouble(dr["introam_calls_other"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDOutgoingToOther>");
                        sb.Append("<ISDVideoCall>" + Math.Round(Convert.ToDouble(dr["introam_video"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDVideoCall>");
                        sb.Append("<ISDOutgoingSMS>" + Math.Round(Convert.ToDouble(dr["introam_out_sms"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDOutgoingSMS>");
                        sb.Append("<ISDIncomingSMS>" + Math.Round(Convert.ToDouble(dr["introam_in_sms"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDIncomingSMS>");
                        sb.Append("<PulseIncomingFreeUsage>" + Math.Round(Convert.ToDouble(dr["introam_pulse_in_free"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</PulseIncomingFreeUsage>");
                        sb.Append("<IncomingFreeUsage>" + Math.Round(Convert.ToDouble(dr["introam_min_in_free"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</IncomingFreeUsage>");
                        sb.Append("<PulseOutgoingFreeUsage>" + Math.Round(Convert.ToDouble(dr["introam_pulse_out_free"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</PulseOutgoingFreeUsage>");
                        sb.Append("<OutgoingFreeUsage>" + Math.Round(Convert.ToDouble(dr["introam_min_out_free"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</OutgoingFreeUsage>");
                        sb.Append("<FreeDataUnit>" + dr["introam_unit_free"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", "") + "</FreeDataUnit>");
                        sb.Append("<FreeDataUsage>" + Math.Round(Convert.ToDouble(dr["introam_data_free"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeDataUsage>");
                        sb.Append("<FreeSMS>" + Math.Round(Convert.ToDouble(dr["introam_sms_free"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeSMS>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["introam_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<CUP>" + dr["introam_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</CUP>");
                        sb.Append("<OtherDescription>" + dr["introam_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</InternationalRoaming>");


                        // Data Charges //
                        sb.Append("<DataCharges>");
                        sb.Append("<Home>" + dr["datacharges_home"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Home>");
                        sb.Append("<Roaming>" + dr["datacharges_roam"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Roaming>");
                        sb.Append("<Conditions>" + dr["datacharges_conditions"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Conditions>");
                        sb.Append("</DataCharges>");


                        // Monthly Rental - International Roaming //
                        sb.Append("<ChargesRoamingInternational>");
                        sb.Append("<MonthlyRental>" + Math.Round(Convert.ToDouble(dr["datacharges_rental"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</MonthlyRental>");
                        sb.Append("<WebLink>" + dr["datacharges_weblink"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</WebLink>");
                        sb.Append("</ChargesRoamingInternational>");


                        // Addn Benefits Duration //
                        sb.Append("<AddBenefitsDuration>");
                        if (Convert.ToDouble(dr["addben_from"].ToString().Trim()) >= 0)
                        {
                            sb.Append("<TimeFrom>" + Math.Round(Convert.ToDouble(dr["addben_from"].ToString().Trim()), 2).ToString("0.00").Replace(".", ":").Replace("-2", "Unlimited").Replace("-1", "") + "</TimeFrom>");
                        }
                        else
                        {
                            sb.Append("<TimeFrom></TimeFrom>");
                        }
                        if (Convert.ToDouble(dr["addben_till"].ToString().Trim()) >= 0)
                        {
                            sb.Append("<TimeTill>" + Math.Round(Convert.ToDouble(dr["addben_till"].ToString().Trim()), 2).ToString("0.00").Replace(".", ":").Replace("-2", "Unlimited").Replace("-1", "") + "</TimeTill>");
                        }
                        else
                        {
                            sb.Append("<TimeTill></TimeTill>");
                        }
                        sb.Append("</AddBenefitsDuration>");


                        // Additional Local //
                        sb.Append("<AddLocalInMinutes>");
                        sb.Append("<Local>" + Math.Round(Convert.ToDouble(dr["add_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Local>");
                        sb.Append("<LocalOnnet>" + Math.Round(Convert.ToDouble(dr["add_local_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOnnet>");
                        sb.Append("<LocalOffnet>" + Math.Round(Convert.ToDouble(dr["add_local_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOffnet>");
                        sb.Append("<LocalMobile>" + Math.Round(Convert.ToDouble(dr["add_local_mob"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalMobile>");
                        sb.Append("<LocalOnnetMobile>" + Math.Round(Convert.ToDouble(dr["add_local_mob"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOnnetMobile>");
                        sb.Append("<LocalOffnetMobile>" + Math.Round(Convert.ToDouble(dr["add_local_mob_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOffnetMobile>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_local_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_local_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddLocalInMinutes>");


                        // Additional STD //
                        sb.Append("<AddSTDInMinutes>");
                        sb.Append("<STD>" + Math.Round(Convert.ToDouble(dr["add_std"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STD>");
                        sb.Append("<STDOnnet>" + Math.Round(Convert.ToDouble(dr["add_std_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDOnnet>");
                        sb.Append("<STDOffnet>" + Math.Round(Convert.ToDouble(dr["add_std_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDOffnet>");
                        sb.Append("<STDMobile>" + Math.Round(Convert.ToDouble(dr["add_std_mob"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDMobile>");
                        sb.Append("<STDOnnetMobile>" + Math.Round(Convert.ToDouble(dr["add_std_mob_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDOnnetMobile>");
                        sb.Append("<STDOffnetMobile>" + Math.Round(Convert.ToDouble(dr["add_std_mob_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDOffnetMobile>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_std_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_std_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddSTDInMinutes>");


                        // Additional Local & STD //
                        sb.Append("<AddLocalSTDInMinutes>");
                        sb.Append("<LocalSTD>" + Math.Round(Convert.ToDouble(dr["add_localstd"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTD>");
                        sb.Append("<LocalSTDOnnet>" + Math.Round(Convert.ToDouble(dr["add_localstd_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDOnnet>");
                        sb.Append("<LocalSTDOffnet>" + Math.Round(Convert.ToDouble(dr["add_localstd_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDOffnet>");
                        sb.Append("<LocalSTDMobile>" + Math.Round(Convert.ToDouble(dr["add_localstd_mob"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDMobile>");
                        sb.Append("<LocalSTDOnnetMobile>" + Math.Round(Convert.ToDouble(dr["add_localstd_mob_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDOnnetMobile>");
                        sb.Append("<LocalSTDOffnetMobile>" + Math.Round(Convert.ToDouble(dr["add_localstd_mob_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDOffnetMobile>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_localstd_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_localstd_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddLocalSTDInMinutes>");


                        // Additional Local, STD & Roaming //
                        sb.Append("<AddLocalSTDRoamingInMinutes>");
                        sb.Append("<LocalSTDMobile>" + Math.Round(Convert.ToDouble(dr["add_LSR"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDMobile>");
                        sb.Append("<LocalSTDRoamingMobile>" + Math.Round(Convert.ToDouble(dr["add_LSR_mob"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDRoamingMobile>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_LSR_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_LSR_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddLocalSTDRoamingInMinutes>");


                        // Additional Roaming //
                        sb.Append("<AddRoamingMinutes>");
                        sb.Append("<IncomingOutgoing>" + Math.Round(Convert.ToDouble(dr["add_roam_inout"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</IncomingOutgoing>");
                        sb.Append("<IncomingFree>" + Math.Round(Convert.ToDouble(dr["add_roam_in"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</IncomingFree>");
                        sb.Append("<OutgoingFree>" + Math.Round(Convert.ToDouble(dr["add_roam_out"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</OutgoingFree>");
                        sb.Append("<OutLocalSTDMobile>" + Math.Round(Convert.ToDouble(dr["add_roam_localstd"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</OutLocalSTDMobile>");
                        sb.Append("<OutLocalFree>" + Math.Round(Convert.ToDouble(dr["add_roam_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</OutLocalFree>");
                        sb.Append("<OutSTDFree>" + Math.Round(Convert.ToDouble(dr["add_roam_std"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</OutSTDFree>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_roam_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_roam_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddRoamingMinutes>");


                        // Additional SMS //
                        sb.Append("<AddSMS>");
                        sb.Append("<LocalNational>" + Math.Round(Convert.ToDouble(dr["add_sms_LN"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalNational>");
                        sb.Append("<LocalNationalOnnet>" + Math.Round(Convert.ToDouble(dr["add_sms_LN_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalNationalOnnet>");
                        sb.Append("<LocalNationalOffnet>" + Math.Round(Convert.ToDouble(dr["add_sms_LN_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalNationalOffnet>");
                        sb.Append("<Local>" + Math.Round(Convert.ToDouble(dr["add_sms_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Local>");
                        sb.Append("<LocalOnnet>" + Math.Round(Convert.ToDouble(dr["add_sms_local_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOnnet>");
                        sb.Append("<LocalOffnet>" + Math.Round(Convert.ToDouble(dr["add_sms_local_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOffnet>");
                        sb.Append("<National>" + Math.Round(Convert.ToDouble(dr["add_sms_nat"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</National>");
                        sb.Append("<NationalOnnet>" + Math.Round(Convert.ToDouble(dr["add_sms_nat_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</NationalOnnet>");
                        sb.Append("<NationalOffnet>" + Math.Round(Convert.ToDouble(dr["add_sms_nat_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</NationalOffnet>");
                        sb.Append("<International>" + Math.Round(Convert.ToDouble(dr["add_sms_int"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</International>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_sms_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_sms_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddSMS>");


                        // Additional ISD //
                        sb.Append("<AddISD>");
                        sb.Append("<Summary>" + dr["add_isd_summ"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Summary>");
                        sb.Append("<Weblink>" + dr["add_isd_link"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Weblink>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_isd_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_isd_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddISD>");


                        // Additional Video Call //
                        sb.Append("<AddVideoCall>");
                        sb.Append("<LocalSTD>" + Math.Round(Convert.ToDouble(dr["add_video_localstd"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTD>");
                        sb.Append("<Local>" + Math.Round(Convert.ToDouble(dr["add_video_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Local>");
                        sb.Append("<LocalOnnet>" + Math.Round(Convert.ToDouble(dr["add_video_local_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOnnet>");
                        sb.Append("<LocalOffnet>" + Math.Round(Convert.ToDouble(dr["add_video_local_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOffnet>");
                        sb.Append("<STD>" + Math.Round(Convert.ToDouble(dr["add_video_std"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STD>");
                        sb.Append("<STDOnnet>" + Math.Round(Convert.ToDouble(dr["add_video_std_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDOnnet>");
                        sb.Append("<STDOffnet>" + Math.Round(Convert.ToDouble(dr["add_video_std_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</STDOffnet>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_video_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_video_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddVideoCall>");



                        // Additional Data //
                        sb.Append("<AddData>");
                        if (Request.Form["RadProvider"] == "TSP")
                        {
                            sb.Append("<Unit>" + dr["adddata_unit"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Unit>");
                            sb.Append("<Total2GData>" + Math.Round(Convert.ToDouble(dr["adddata_total2g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Total2GData>");
                            sb.Append("<Total3GData>" + Math.Round(Convert.ToDouble(dr["adddata_total3g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Total3GData>");
                            sb.Append("<Total4GData>" + Math.Round(Convert.ToDouble(dr["adddata_total4g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Total4GData>");
                        }
                        else
                        {
                            sb.Append("<SpeedUnit>" + dr["adddata_unit"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</SpeedUnit>");
                            sb.Append("<HigherSpeedLimit>" + Math.Round(Convert.ToDouble(dr["adddata_total2g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</HigherSpeedLimit>");
                            sb.Append("<SpeedDataLimit>" + Math.Round(Convert.ToDouble(dr["adddata_total3g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</SpeedDataLimit>");
                            sb.Append("<SpeedBeyondLimit>" + Math.Round(Convert.ToDouble(dr["adddata_total4g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</SpeedBeyondLimit>");
                        }
                        sb.Append("<TotalData>" + Math.Round(Convert.ToDouble(dr["adddata_ISP"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</TotalData>");
                        sb.Append("<DayNightDataCapping>" + Math.Round(Convert.ToDouble(dr["adddata_daycap"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</DayNightDataCapping>");
                        sb.Append("<WeeklyDataCapping>" + Math.Round(Convert.ToDouble(dr["adddata_weekcap"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</WeeklyDataCapping>");
                        sb.Append("<MonthlyDataCapping>" + Math.Round(Convert.ToDouble(dr["adddata_monthcap"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</MonthlyDataCapping>");
                        sb.Append("<CarryForward>" + dr["adddata_carry"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</CarryForward>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["adddata_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<FUP>" + dr["adddata_fup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</FUP>");
                        sb.Append("<Conditions>" + dr["adddata_conditions"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Conditions>");
                        sb.Append("<OtherDescription>" + dr["adddata_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddData>");


                        // Miscellaneous //
                        sb.Append("<Miscellaneous>");
                        sb.Append("<SpecialBenefits>" + dr["misc_ben"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</SpecialBenefits>");
                        sb.Append("<OtherCharges>" + dr["misc_other"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherCharges>");
                        sb.Append("<Remarks>" + dr["misc_remarks"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Remarks>");
                        sb.Append("<Terms>" + dr["misc_terms"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Terms>");
                        sb.Append("<ActivationMode>" + dr["misc_mode"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</ActivationMode>");
                        sb.Append("<DetailsUploaded>" + dr["misc_uploaded"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</DetailsUploaded>");
                        sb.Append("<WebLink>" + dr["misc_link"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</WebLink>");
                        sb.Append("<OtherDeclarations>" + dr["misc_dec"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</OtherDeclarations>");
                        sb.Append("<TerminationTerms>" + dr["misc_termination"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</TerminationTerms>");
                        sb.Append("<ActivationCode>" + dr["misc_actcode"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</ActivationCode>");
                        sb.Append("<DeActivationCode>" + dr["misc_deactcode"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</DeActivationCode>");
                        sb.Append("</Miscellaneous>");


                        // One Time Charges //
                        sb.Append("<OneTimeCharges>");
                        sb.Append("<RuralUrban>" + dr["ruralurban"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</RuralUrban>");
                        sb.Append("<RegistrationCharges>" + Math.Round(Convert.ToDouble(dr["regcharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</RegistrationCharges>");
                        sb.Append("<ActivationCharges>" + Math.Round(Convert.ToDouble(dr["actcharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ActivationCharges>");
                        sb.Append("<SecurityDeposit>" + Math.Round(Convert.ToDouble(dr["ISP_deposit"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</SecurityDeposit>");
                        sb.Append("<EnrollmentFee>" + Math.Round(Convert.ToDouble(dr["planfee"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</EnrollmentFee>");
                        sb.Append("<OtherCharges>" + Math.Round(Convert.ToDouble(dr["othercharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</OtherCharges>");
                        //sb.Append("<OtherChargesDetails>" + dr["otherchargedet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & "," &amp; ").Replace("-1", " ") + "</OtherChargesDetails>");
                        sb.Append("</OneTimeCharges>");


                        // Details of Service //
                        sb.Append("<DetailsOfService>");
                        sb.Append("<PayPerUseModel>" + dr["payperuse"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</PayPerUseModel>");
                        sb.Append("<AnyOtherModel>" + dr["othermodel"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</AnyOtherModel>");
                        sb.Append("<Benefits>" + dr["modelbenefits"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace(" & ", " &amp; ").Replace("-1", " ") + "</Benefits>");
                        sb.Append("</DetailsOfService>");


                        // Security Deposit //
                        sb.Append("<SecurityDeposit>");
                        sb.Append("<Local>" + Math.Round(Convert.ToDouble(dr["security_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Local>");
                        sb.Append("<LocalSTD>" + Math.Round(Convert.ToDouble(dr["security_localstd"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTD>");
                        sb.Append("<LocalSTDISD>" + Math.Round(Convert.ToDouble(dr["security_LSI"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDISD>");
                        sb.Append("<NationalRoaming>" + Math.Round(Convert.ToDouble(dr["security_nat_roam"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</NationalRoaming>");
                        sb.Append("<InternationalRoaming>" + Math.Round(Convert.ToDouble(dr["security_int_roam"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</InternationalRoaming>");
                        sb.Append("<AnyOther>" + Math.Round(Convert.ToDouble(dr["security_other"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AnyOther>");
                        sb.Append("</SecurityDeposit>");


                        // Fixed Charges //
                        sb.Append("<FixedCharges>");
                        sb.Append("<MonthlyCharges>" + Math.Round(Convert.ToDouble(dr["security_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</MonthlyCharges>");
                        sb.Append("<ChargesEC999>" + Math.Round(Convert.ToDouble(dr["security_localstd"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ChargesEC999>");
                        sb.Append("<ChargesEC29999>" + Math.Round(Convert.ToDouble(dr["security_LSI"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ChargesEC29999>");
                        sb.Append("<ChargesEC99999>" + Math.Round(Convert.ToDouble(dr["security_nat_roam"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ChargesEC99999>");
                        sb.Append("<ChargesEC100000>" + Math.Round(Convert.ToDouble(dr["security_int_roam"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ChargesEC100000>");
                        sb.Append("<FlatFMC>" + Math.Round(Convert.ToDouble(dr["security_other"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FlatFMC>");
                        sb.Append("</FixedCharges>");


                        // Optional Fixed Monthly Charges //
                        sb.Append("<OptionalFixedCharges>");
                        //sb.Append("<AdvanceRental>" + Math.Round(Convert.ToDouble(dr["fixed_rental"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AdvanceRental>");
                        sb.Append("<CLIP>" + Math.Round(Convert.ToDouble(dr["fixed_CLIP"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</CLIP>");
                        //sb.Append("<AnyOther>" + Math.Round(Convert.ToDouble(dr["fixed_other"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AnyOther>");
                        sb.Append("<OtherDetails></OtherDetails>");
                        sb.Append("</OptionalFixedCharges>");


                        // Free Calls in MCUS per month //
                        sb.Append("<FreeCallsMCUS>");
                        sb.Append("<FreeCalls>" + Math.Round(Convert.ToDouble(dr["security_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeCalls>");
                        sb.Append("<FreeCallsEC999>" + Math.Round(Convert.ToDouble(dr["security_localstd"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeCallsEC999>");
                        sb.Append("<FreeCallsEC29999>" + Math.Round(Convert.ToDouble(dr["security_LSI"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeCallsEC29999>");
                        sb.Append("<FreeCallsEC99999>" + Math.Round(Convert.ToDouble(dr["security_nat_roam"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeCallsEC99999>");
                        sb.Append("<FreeCallsEC100000>" + Math.Round(Convert.ToDouble(dr["security_int_roam"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeCallsEC100000>");
                        sb.Append("<FreeTalkValue>" + Math.Round(Convert.ToDouble(dr["security_other"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeTalkValue>");
                        sb.Append("</FreeCallsMCUS>");







                        sb.Append("</TariffProduct>");

                        idlist+=dr["uniqueid"].ToString().Trim() + ",";

                    }
                }
                con.Close();

                sb.Append("</TariffProducts>");

                sb.Replace("&amp;", "&");    // there might be some '&amp;' and some '&' in the same tariff. This replacement and re-replacement will set it uniformly
                sb.Replace("&", "&amp;");

                //string attachment = "attachment; filename=TariffProducts.xml";
                string attachment = "attachment; filename=Report.xml";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/xml";
                StringWriter sw = new StringWriter();

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
            //Response.Write("<br /><br /><br />" + ex.ToString());
        }
    }







    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        try
        {

            /*
            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            divPopUpBg.Attributes.Clear();
            divSelection.InnerHtml = "";
            Button1.Visible = false;
            ButtonCancel.Visible = false;
            divPopShadow.Visible = false;
            divPopUp.Visible = false;

            showRecords(null, null);
            */

            Response.Redirect("index.aspx");
        }
        catch (Exception ex)
        {
            Response.Write("<br /><br /><br />" + ex.ToString());
        }
    }






    protected void SetCurrentSliderValues(object sender, EventArgs e)
    {
        try
        {
            /*
            if (hdnSilde1Min.Value != "")
            {
                slide1Min = Convert.ToDecimal(hdnSilde1Min.Value);
            }
            if (hdnSilde1Max.Value != "")
            {
                slide1Max = Convert.ToDecimal(hdnSilde1Max.Value);
            }
            if (hdnSilde2Min.Value != "")
            {
                slide2Min = Convert.ToDecimal(hdnSilde2Min.Value);
            }
            if (hdnSilde2Max.Value != "")
            {
                slide2Max = Convert.ToDecimal(hdnSilde2Max.Value);
            }
            if (hdnSilde3Min.Value != "")
            {
                slide3Min = Convert.ToDecimal(hdnSilde3Min.Value);
            }
            if (hdnSilde3Max.Value != "")
            {
                slide3Max = Convert.ToDecimal(hdnSilde3Max.Value);
            }
            if (hdnSilde4Min.Value != "")
            {
                slide4Min = Convert.ToDecimal(hdnSilde4Min.Value);
            }
            if (hdnSilde4Max.Value != "")
            {
                slide4Max = Convert.ToDecimal(hdnSilde4Max.Value);
            }


            Page.ClientScript.RegisterStartupScript(this.GetType(), "funAdvance", "funAdvance()", true);


            setManual(null, null);
            */
        }
        catch (Exception ex)
        {
            Response.Write("<br /><br /><br />" + ex.ToString());
        }
    }



    protected void setManual(object sender, EventArgs e)
    {
        try
        {
            
            // If the user has changed the values in the slider text boxes, below code will re-implement them in the boxes if they have been changed by slider automatically //

            if (Text2a.Text.Trim() != "")
            {
                amount2a.Text = Text2a.Text.Trim();
                amount2b.Text = Text2b.Text.Trim();
                amount3a.Text = Text3a.Text.Trim();
                amount3b.Text = Text3b.Text.Trim();
                amount4a.Text = Text4a.Text.Trim();
                amount4b.Text = Text4b.Text.Trim();
                amount5a.Text = Text5a.Text.Trim();
                amount5b.Text = Text5b.Text.Trim();
            }

            // If the user has changed the values in the slider text boxes, below code will re-implement them in the boxes if they have been changed by slider automatically - CODE ENDS HERE //

        }
        catch (Exception ex)
        {
            Response.Write("<br /><br /><br />" + ex.ToString());
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





}