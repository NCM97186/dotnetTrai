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
//using ClosedXML;              // for generating .XLSX file
using ClosedXML.Excel;       // for generating .XLSX file


// $$$$$$$$#$#$#$#$$#$#$#  ClosedXML.dll and DocumentFormat.OpenXml.dll files must be present in the 'Bin' directory for genration of .XLSX files


public partial class indexm : System.Web.UI.Page
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
    public decimal slide1Min = 0;
    public decimal slide1Max = 1000;
    public decimal slide2Min = 0;
    public decimal slide2Max = 1000;
    public decimal slide3Min = 0;
    public decimal slide3Max = 500;
    public decimal slide4Min = 0;
    public decimal slide4Max = 365;
    public decimal slide1MinGlobal = 0;
    public decimal slide1MaxGlobal = 1000;
    public decimal slide2MinGlobal = 0;
    public decimal slide2MaxGlobal = 1000;
    public decimal slide3MinGlobal = 0;
    public decimal slide3MaxGlobal = 500;
    public decimal slide4MinGlobal = 0;
    public decimal slide4MaxGlobal = 365;
    public decimal slide1Step = 10;
    public decimal slide2Step = 10;
    public decimal slide3Step = 10;
    public decimal slide4Step = 10;
    */


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

        // CODE TO CHECK IF THE PAGE IS BEING OPENED ON A MOBILE DEVICE. IF NOT, REDIRECT IT TO MAIN INDEX PAGE

        String labelText = "";
        System.Web.HttpBrowserCapabilities myBrowserCaps = Request.Browser;
        if (((System.Web.Configuration.HttpCapabilitiesBase)myBrowserCaps).IsMobileDevice)
        {
            //labelText = "Browser is a mobile device.";
        }
        else
        {
            ////labelText = "Browser is not a mobile device.";
            Response.Redirect("https://tariff.trai.gov.in/");
        }
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('" + labelText + "');", true);

        // CODE TO CHECK IF THE PAGE IS BEING OPENED ON A MOBILE DEVICE. IF NOT, REDIRECT IT TO MAIN INDEX PAGE - CODE ENDS HERE



        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con4 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        Server.ScriptTimeout = 999999;

        //tablename = "TRAI_tariffrecords";
        tablename = "TRAI_tariffs";

        //int resultsize=30000;
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
            com = new MySqlCommand("select count(distinct(oper)) from TRAI_operators where (oper!='Aircel' and oper!='Telenor' and oper!='Plintron' and oper!='Idea' and oper!='Vodafone')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            size = Convert.ToInt32(dr[0].ToString());
            con.Close();


        }
        catch (Exception ex)
        { }
        charr = new CheckBox[size];
        int i = 0;
        //com = new MySqlCommand("select distinct(oper) from TRAI_operators order by oper", con);
        com = new MySqlCommand("select distinct(oper) from TRAI_operators where(oper!='Aircel' and oper!='Telenor' and oper!='Plintron' and oper!='Idea' and oper!='Vodafone') order by oper", con);
        
        con.Open();
        dr = com.ExecuteReader();
        while (dr.Read())
        {
            TableRow tr1 = new TableRow();
            TableRow tr2 = new TableRow();
            TableRow tr3 = new TableRow();
            TableRow tr4 = new TableRow();
            TableRow tr5 = new TableRow();
            //TableRow tr6 = new TableRow();
            TableCell tcc1 = new TableCell();
            tcc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tcc1.ForeColor = System.Drawing.Color.Black;
            ch = new CheckBox();
            ch.CssClass = "chks2";
            ch.ID = "OP-" + dr["oper"].ToString().Trim();
            ch.Text = " " + dr["oper"].ToString().Trim();
            //ch.Checked = true;
            ch.Attributes.Add("onClick", "javascript:unchk();");
            charr[i] = ch;
            tcc1.Controls.Add(charr[i]);
            if (colcount == 0)
            {
                tr1.Controls.Add(tcc1);
                tb.Controls.Add(tr1);
            }
            if (colcount == 1)
            {
                tr2.Controls.Add(tcc1);
                tb2.Controls.Add(tr2);
                colcount = -1;
            }
            /*
            if (colcount == 2)
            {
                tr3.Controls.Add(tcc1);
                tb3.Controls.Add(tr3);
            }
            if (colcount == 3)
            {
                tr4.Controls.Add(tcc1);
                tb4.Controls.Add(tr4);
            }
            if (colcount == 4)
            {
                tr5.Controls.Add(tcc1);
                tb5.Controls.Add(tr5);
                colcount = -1;
            }
            if (colcount == 5)
            {
                tr6.Controls.Add(tcc1);
                tb6.Controls.Add(tr6);
                colcount = -1;
            }
            */
            colcount++;
            i++;
        }


        circount = i;
        oper1.Controls.Add(tb);
        oper2.Controls.Add(tb2);
        //oper3.Controls.Add(tb3);
        //oper4.Controls.Add(tb4);
        //oper5.Controls.Add(tb5);
        //oper6.Controls.Add(tb6);
        con.Close();


        if (!IsPostBack)
        {
            // insert MOBILE record in hit counter table //
            getMaxRno("rno", "TRAI_hitcounter");
            com = new MySqlCommand("insert into TRAI_hitcounter values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','Mobile')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            /*
            for (int d = 0; d < ChkPlans.Items.Count; d++)
            {
                ChkPlans.Items[d].Attributes.Add("title", "test");
            }
            */

            //com = new MySqlCommand("select distinct(oper) from TRAI_operators order by oper", con);
            com = new MySqlCommand("select distinct(oper) from TRAI_operators where(oper!='Aircel' and oper!='Telenor' and oper!='Plintron' and oper!='Idea' and oper!='Vodafone') order by oper", con);
            
            con.Open();
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                DropOperator.Items.Add(dr[0].ToString().Trim());
            }
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
            divheaders.InnerHtml = "";
            divresults.InnerHtml = "";

            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            string otype = RadProvider.SelectedItem.Text.Trim();
            string mob = RadMobile.SelectedItem.Text.Trim();
            string prepost = RadPrePost.SelectedItem.Text.Trim();
            string circ = DropCircle.SelectedItem.Text.Trim();
            string oper = DropOperator.SelectedItem.Text.Trim();
            TextCompareProduct.Text = "";



            RadPrePost.Visible = true;
            ChkDataTech1.Visible = true;
            ChkDataTech2.Visible = true;
            ChkDataTech3.Visible = true;


            string temp = DropCircle.SelectedItem.Text.Trim();

            /*
            DropCircle.Items.Remove("Gujarat");
            if (mob != "Landline")
            {
                DropCircle.Items.Add("Gujarat");
            }
            */

            DropCircle.Text = temp;

            if (otype == "ISP")
            {
                //divMoreOptions.Visible = false;
                RadMobile.Text = "Mobile";
                RadMobile.Visible = false;
                divPlans.Visible = false;
                ChkDataTech1.Visible = false;
                ChkDataTech2.Visible = false;
                ChkDataTech3.Visible = false;
                divAdvance2.Visible = false;
                ChkAdvance.Visible = false;
                ButtonClearFilters.Visible = false;
            }
            else
            {
                //divMoreOptions.Visible = true;
                RadMobile.Visible = true;
                divPlans.Visible = true;
                ChkDataTech1.Visible = true;
                ChkDataTech2.Visible = true;
                ChkDataTech3.Visible = true;
                divAdvance2.Visible = true;
                ChkAdvance.Visible = true;
                ButtonClearFilters.Visible = true;
            }

            if (otype == "TSP")
            {
                if (mob == "Landline" || prepost == "Postpaid")
                {
                    divValidity.Visible = false;
                    dashed2.Visible = false;
                    divTalktime.Visible = false;
                    dashed2b.Visible = false;
                    LblPrice.Text = "Monthly Rental &#8377; (Optional)";
                }
                else
                {
                    divValidity.Visible = true;
                    dashed2.Visible = true;
                    divTalktime.Visible = true;
                    dashed2b.Visible = true;
                    LblPrice.Text = "Price &#8377; (Optional)";
                }

                if (mob == "Landline")
                {
                    ChkISDRoaming.Visible = false;
                    ChkNatRoaming.Visible = false;
                    ChkUnlim_Roaming.Visible = false;
                    divDailyDataCapping.Visible = false;
                    dashed1c.Visible = false;
                    divSep1.Visible = false;
                }
                else
                {
                    ChkISDRoaming.Visible = true;
                    ChkNatRoaming.Visible = true;
                    ChkUnlim_Roaming.Visible = true;
                    divDailyDataCapping.Visible = true;
                    dashed1c.Visible = true;
                    divSep1.Visible = true;
                }
            }

            DropOperator.Items.Clear();
            DropOperator.Items.Add("OPERATOR");
            DropOperator.Items.Add("All Operators");

            int planid = 0;
            if (mob == "Mobile" && prepost == "Prepaid")
            {
                // Left Pane Items //

                ChkPlans.Items.Clear();
                ChkPlans.Items.Add("All Tariffs");
                ChkPlans.Items[planid].Attributes.Add("title", "All Tariffs");
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

                for (int i = 0; i < size; i++)
                {
                    charr[i].Enabled = false;
                    charr[i].Attributes.Add("style", "color:#afafaf;");
                }

                string ops = ",";

                /*
                if (circ == "Delhi")
                {
                    DropOperator.Items.Add("Aircel");
                    ops = ops + "Aircel" + ",";
                }
                */

                if (otype == "TSP")
                {
                    DropOperator.Items.Add("Aerovoyce");
                    ops = ops + "Aerovoyce" + ",";
                    DropOperator.Items.Add("Airtel");
                    ops = ops + "Airtel" + ",";
                    if (circ != "Delhi" && circ != "Mumbai")
                    {
                        DropOperator.Items.Add("BSNL");
                        ops = ops + "BSNL" + ",";
                    }
                    /*
                    DropOperator.Items.Add("Idea");
                    ops = ops + "Idea" + ",";
                     */ 
                    DropOperator.Items.Add("Jio");
                    ops = ops + "Jio" + ",";
                    if (circ == "Delhi" || circ == "Mumbai")
                    {
                        DropOperator.Items.Add("MTNL");
                        ops = ops + "MTNL" + ",";
                    }
                    if (circ != "Assam" && circ != "Bihar" && circ != "Jammu and Kashmir" && circ != "North East" && circ != "West Bengal")
                    {
                        DropOperator.Items.Add("Tata Tele");
                        ops = ops + "Tata Tele" + ",";
                    }
                    if (circ == "Andhra Pradesh" || circ == "Bihar" || circ == "Gujarat" || circ == "Maharashtra" || circ == "UP East" || circ == "UP West")
                    {
                        //DropOperator.Items.Add("Telenor");
                        //ops = ops + "Telenor" + ",";
                    }
                    /*
                    if (circ == "Delhi" || circ == "Gujarat")
                    {
                        DropOperator.Items.Add("Vodafone");
                        ops = ops + "Vodafone" + ",";
                    }
                    */

                    /*
                    DropOperator.Items.Add("Vodafone");
                    ops = ops + "Vodafone" + ",";
                    */
                    DropOperator.Items.Add("Vodafone Idea");
                    ops = ops + "Vodafone Idea" + ",";
                    
                    if (circ == "Chennai and Tamil Nadu")
                    {
                        DropOperator.Items.Add("Surftelecom");
                        ops = ops + "Surftelecom" + ",";
                    }
                }
                else
                {
                    LoadISP(null, null);
                }

                for (int i = 0; i < size; i++)
                {
                    if (ops.Contains("," + charr[i].Text.Trim() + ","))
                    {
                        charr[i].Enabled = true;
                        charr[i].Attributes.Add("style", "color:#364daf;");
                    }
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

                for (int i = 0; i < size; i++)
                {
                    charr[i].Enabled = false;
                    charr[i].Attributes.Add("style", "color:#afafaf;");
                }

                string ops = ",";
                /*
                DropOperator.Items.Add("Aircel");
                ops = ops + "Aircel" + ",";
                */

                if (otype == "TSP")
                {
                    DropOperator.Items.Add("Aerovoyce");
                    ops = ops + "Aerovoyce" + ",";
                    DropOperator.Items.Add("Airtel");
                    ops = ops + "Airtel" + ",";
                    if (circ != "Delhi" && circ != "Mumbai")
                    {
                        DropOperator.Items.Add("BSNL");
                        ops = ops + "BSNL" + ",";
                    }
                    /*
                    DropOperator.Items.Add("Idea");
                    ops = ops + "Idea" + ",";
                    */ 
                    DropOperator.Items.Add("Jio");
                    ops = ops + "Jio" + ",";
                    if (circ == "Delhi" || circ == "Mumbai")
                    {
                        DropOperator.Items.Add("MTNL");
                        ops = ops + "MTNL" + ",";
                    }
                    if (circ != "Assam" && circ != "Bihar" && circ != "Delhi" && circ != "Jammu and Kashmir" && circ != "North East" && circ != "West Bengal")
                    {
                        DropOperator.Items.Add("Tata Tele");
                        ops = ops + "Tata Tele" + ",";
                    }
                    /*
                    DropOperator.Items.Add("Vodafone");
                    ops = ops + "Vodafone" + ",";
                    */
                    DropOperator.Items.Add("Vodafone Idea");
                    ops = ops + "Vodafone Idea" + ",";
                    
                    if (circ == "Chennai and Tamil Nadu")
                    {
                        DropOperator.Items.Add("Surftelecom");
                        ops = ops + "Surftelecom" + ",";
                    }
                }
                else
                {
                    LoadISP(null, null);
                }

                for (int i = 0; i < size; i++)
                {
                    if (ops.Contains("," + charr[i].Text.Trim() + ","))
                    {
                        charr[i].Enabled = true;
                        charr[i].Attributes.Add("style", "color:#364daf;");
                    }
                }

                // Operator DropDown Population - CODE ENDS HERE //
            }

            if (mob == "Landline")
            {
                if (otype == "TSP")
                {
                    RadPrePost.Visible = false;
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


                for (int i = 0; i < size; i++)
                {
                    charr[i].Enabled = false;
                    charr[i].Attributes.Add("style", "color:#afafaf;");
                }

                string ops = ",";

                if (otype == "TSP")
                {
                    DropOperator.Items.Add("Aerovoyce");
                    ops = ops + "Aerovoyce" + ",";
                    DropOperator.Items.Add("Airtel");
                    ops = ops + "Airtel" + ",";
                    if (circ != "Delhi" && circ != "Mumbai")
                    {
                        DropOperator.Items.Add("BSNL");
                        ops = ops + "BSNL" + ",";
                    }
                    if (circ == "Delhi" || circ == "Mumbai")
                    {
                        DropOperator.Items.Add("MTNL");
                        ops = ops + "MTNL" + ",";
                    }
                    if (circ == "Punjab")
                    {
                        DropOperator.Items.Add("Quadrant (Connect)");
                        ops = ops + "Quadrant (Connect)" + ",";
                    }

                    if (circ != "Assam" && circ != "Bihar" && circ != "Chennai & Tamil Nadu" && circ != "Jammu and Kashmir" && circ != "North East" && circ != "West Bengal")
                    {
                        DropOperator.Items.Add("Tata Tele");
                        ops = ops + "Tata Tele" + ",";
                    }
                }
                else
                {
                    LoadISP(null, null);
                }

                for (int i = 0; i < size; i++)
                {
                    if (ops.Contains("," + charr[i].Text.Trim() + ","))
                    {
                        charr[i].Enabled = true;
                        charr[i].Attributes.Add("style", "color:#364daf;");
                    }
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
            DropOperator.Items.Clear();
            int operpos = 0;

            DropOperator.Items.Add("All Operators");
            ops = ops + "All Operators" + ",";

            com4 = new MySqlCommand("select distinct(oper) from TRAI_tariffs where(upper(oper)!='AIRCEL' and upper(oper)!='AIRTEL' and upper(oper)!='BSNL' and upper(oper)!='IDEA' and upper(oper)!='JIO' and upper(oper)!='MTNL' and upper(oper)!='PLINTRON' and upper(oper)!='QUADRANT (CONNECT)' and upper(oper)!='TATA TELE' and upper(oper)!='TELENOR' and upper(oper)!='VODAFONE' and upper(oper)!='VODAFONE IDEA' and upper(oper)!='SURFTELECOM' and upper(oper)!='AEROVOYCE') order by oper", con4);
            con4.Open();
            dr4 = com4.ExecuteReader();
            while (dr4.Read())
            {
                DropOperator.Items.Add(dr4[0].ToString().Trim());
                ops = ops + dr4[0].ToString().Trim() + ",";
            }
            con4.Close();

            for (int i = 0; i < size; i++)
            {
                if (ops.Contains("," + charr[i].Text.Trim() + ","))
                {
                    charr[i].Enabled = true;
                    charr[i].Attributes.Add("style", "color:#364daf;");
                }
            }
        }
        catch (Exception ex) { }
    }




    /*
    protected void DropOperator_Change(object sender, EventArgs e)
    {
        try
        {
            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            string oper=DropOperator.SelectedItem.Text.Trim();

            for (int i = 0; i < ChkDataTech.Items.Count;i++)
            {
                ChkDataTech.Items[i].Selected = false;
                ChkDataTech.Items[i].Enabled = true;
                ChkDataTech.Items[0].Attributes.Add("style", "color:#364daf;");

                ChkDailyDataCap.Items[i].Selected = false;
                ChkDailyDataCap.Items[i].Enabled = true;
                ChkDailyDataCap.Items[0].Attributes.Add("style", "color:#364daf;");
            }


            // Disable '2G' checkboxes for operators for which service not available //

            if (oper == "Jio")
            {
                ChkDataTech.Items[0].Attributes.Add("style", "color:#afafaf;");
                ChkDataTech.Items[0].Selected = false;
                ChkDataTech.Items[0].Enabled = false;

                ChkDailyDataCap.Items[0].Attributes.Add("style", "color:#afafaf;");
                ChkDailyDataCap.Items[0].Selected = false;
                ChkDailyDataCap.Items[0].Enabled = false;
            }

            // Disable '2G' checkboxes for operators for which service not available - CODE ENDS HERE //

            // Disable '3G' checkboxes for operators for which service not available //

            if (oper == "Jio")
            {
                ChkDataTech.Items[1].Attributes.Add("style", "color:#afafaf;");
                ChkDataTech.Items[1].Selected = false;
                ChkDataTech.Items[1].Enabled = false;

                ChkDailyDataCap.Items[1].Attributes.Add("style", "color:#afafaf;");
                ChkDailyDataCap.Items[1].Selected = false;
                ChkDailyDataCap.Items[1].Enabled = false;
            }

            // Disable '3G' checkboxes for operators for which service not available - CODE ENDS HERE //

            // Disable '4G' checkboxes for operators for which service not available //

            if (oper == "Aircel" || oper == "BSNL" || oper == "Tata Tele")
            {
                ChkDataTech.Items[2].Attributes.Add("style", "color:#afafaf;");
                ChkDataTech.Items[2].Selected = false;
                ChkDataTech.Items[2].Enabled = false;

                ChkDailyDataCap.Items[2].Attributes.Add("style", "color:#afafaf;");
                ChkDailyDataCap.Items[2].Selected = false;
                ChkDailyDataCap.Items[2].Enabled = false;
            }
            // Disable '4G' checkboxes for operators for which service not available - CODE ENDS HERE //


        }
        catch (Exception ex) { }
    }
    */




    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            int flag = 0;

            string otype = RadProvider.SelectedItem.Text.Trim();
            string mob = RadMobile.SelectedItem.Text.Trim();
            string prepost = RadPrePost.SelectedItem.Text.Trim();
            string circ = DropCircle.SelectedItem.Text.Trim();

            string oper = "";
            if (DropOperator.SelectedItem.Text.Trim() == "All Operators")
            {
                oper = ",All,";
            }
            else
            {
                oper = "," + DropOperator.SelectedItem.Text.Trim() + ",";
            }

            for (int i = 0; i < size; i++)
            {
                //if (charr[i].Checked == true || oper.Contains(",All,"))
                if (charr[i].Enabled==true && (charr[i].Checked == true || oper.Contains(",All,")))
                {
                    oper = oper + charr[i].Text.Trim() + ",";
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

            /*
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
            */

            if (otype == "ISP")     // 'ISP' is selected
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
            

            if (circ == "CIRCLE")
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

                /*
                if(mob=="Mobile")
                {
                    conditions = conditions + " and (ttype!='Fixed Line -Tariff' and ttype!='Fixed Line Add-On Pack' and ttype!='ISP' and ttype!='Gen_ISD_Tariff' and ttype!='International_Roaming')";
                }
                else
                {
                    conditions = conditions + " and (ttype='Fixed Line -Tariff' or ttype='Fixed Line Add-On Pack')";
                }

                if(RadPrePost.Visible==true && prepost=="Prepaid")
                {
                    conditions = conditions + " and (ttype='Prepaid_Plan Voucher' or ttype='Prepaid_STV' or ttype='Prepaid_Combo' or ttype='Prepaid_Top Up' or ttype='Prepaid_VAS' or ttype='Promo Offer' or ttype='SUK')";
                }
                if (RadPrePost.Visible == true && prepost == "Postpaid")
                {
                    conditions = conditions + " and (ttype='Postpaid- Plan' or ttype='Postpaid- Add On Pack' or ttype='Postpaid_VAS' or ttype='Promo Offer')";
                }
                */

                if (otype == "TSP")
                {
                    if (mob == "Mobile")
                    {
                        conditions = conditions + " and (ttype!='Postpaid Fixed Line Plan' and ttype!='Prepaid Fixed Line Plan' and ttype!='Postpaid Fixed Line Add On Pack' and ttype!='Prepaid Fixed Line Add On Pack' and ttype!='Prepaid ISP' and ttype!='Postpaid ISP' and ttype!='Prepaid Gen ISD Tariff' and ttype!='Postpaid Gen ISD Tariff' and ttype!='Prepaid International Roaming' and ttype!='Postpaid International Roaming')";
                    }
                    else
                    {
                        conditions = conditions + " and (ttype='Postpaid Fixed Line Plan' or ttype='Prepaid Fixed Line Plan' or ttype='Postpaid Fixed Line Add On Pack' or ttype='Prepaid Fixed Line Add On Pack')";
                    }


                    if (RadPrePost.Visible == true && prepost == "Prepaid")
                    {
                        conditions = conditions + " and (ttype='Prepaid Plan Voucher' or ttype='Prepaid STV' or ttype='Prepaid Combo' or ttype='Prepaid Top Up' or ttype='Prepaid VAS' or ttype='Prepaid Promo Offer' or ttype='SUK' or ttype='Prepaid ISP')";
                    }
                    if (RadPrePost.Visible == true && prepost == "Postpaid")
                    {
                        conditions = conditions + " and (ttype='Postpaid Plan' or ttype='Postpaid Add On Pack' or ttype='Postpaid VAS' or ttype='Postpaid Promo Offer' or ttype='Postpaid ISP')";
                    }
                }
                else
                {
                    if (RadPrePost.Visible == true && prepost == "Prepaid")
                    {
                        conditions = conditions + " and (categ='Prepaid')";
                    }
                    if (RadPrePost.Visible == true && prepost == "Postpaid")
                    {
                        conditions = conditions + " and (categ='Postpaid')";
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

                if (otype == "TSP")
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

                /*
                conditions=conditions + " and (";
                if(ChkDataTech1.Checked==true)
                {
                    conditions = conditions + "adddata_type like '%" + ChkDataTech1.Text.Replace("Data", "").Trim() + "%' or ";
                }
                if (ChkDataTech2.Checked == true)
                {
                    conditions = conditions + "adddata_type like '%" + ChkDataTech2.Text.Replace("Data", "").Trim() + "%' or ";
                }
                if (ChkDataTech3.Checked == true)
                {
                    conditions = conditions + "adddata_type like '%" + ChkDataTech3.Text.Replace("Data", "").Trim() + "%' or ";
                }
                if (conditions.Substring(conditions.Length - 4, 4) == " or ")
                {
                    conditions = conditions.Substring(0, conditions.Length - 4);
                } 
                conditions = conditions + ")";
                conditions = conditions.Replace(" and ()", "");
                                
                if (Convert.ToDouble(amount3a.Text.Trim()) == 0)
                {
                    conditions = conditions + " and (adddata_total >= -1 and adddata_total <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")";
                }
                else
                {
                    conditions = conditions + " and (adddata_total >= " + Convert.ToDouble(amount3a.Text.Trim()) + " and adddata_total <= " + Convert.ToDouble(amount3b.Text.Trim()) + ")";
                }
                */

                if (otype == "ISP" && ChkCapUnlim.Checked == true)
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


                /*
                if (ChkUnlim_Local.Checked == true)
                {
                    conditions = conditions + " and ((local_all_voicecharges=0) or (local_on_voice_peak=0 and local_on_voice_offpeak=0 and local_off_voice_peak=0 and local_off_voice_offpeak=0 and local_fix_on_voice_peak=0 and local_fix_on_voice_offpeak=0 and local_fix_off_voice_peak=0 and local_fix_off_voice_offpeak=0))";
                }
                if (ChkUnlim_STD.Checked == true)
                {
                    conditions = conditions + " and ((std_all_voicecharges=0) or (std_on_voice_peak=0 and std_on_voice_offpeak=0 and std_off_voice_peak=0 and std_off_voice_offpeak=0 and std_fix_on_voice_peak=0 and std_fix_on_voice_offpeak=0 and std_fix_off_voice_peak=0 and std_fix_off_voice_offpeak=0))";
                }
                if (ChkUnlim_Roaming.Checked == true)
                {
                    conditions = conditions + " and (roam_call_voice_out=0 and roam_call_voice_std=0)";
                }
                */
                
                if (otype == "TSP")
                {
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

                    /*
                    if (ChkISDPack.Checked == true)
                    {
                        conditions = conditions + " and ((isd_countries!='' and isd_countries!='-' and isd_countries!='-1') or (isd_weblink!='' and isd_weblink!='-' and isd_weblink!='-1') or (add_isd_summ!='' and add_isd_summ!='-' and add_isd_summ!='-1') or (add_isd_link!='' and add_isd_link!='-' and add_isd_link!='-1')) and (ttype='Prepaid_STV' or ttype='Prepaid_Combo')";
                    }
                    if (ChkISDRoaming.Checked == true)
                    {
                        conditions = conditions + " and ((introam_countries!='' and introam_countries!='-' and introam_countries!='-1') or (introam_otherdet!='' and introam_otherdet!='-' and introam_otherdet!='-1')) and (ttype='Prepaid_STV' or ttype='Prepaid_Combo')";
                    }
                    if (ChkNatRoaming.Checked == true)
                    {
                        conditions = conditions + " and (roam_call_voice_out>=0 or roam_call_voice_std>=0) and (ttype='Prepaid_STV' or ttype='Prepaid_Combo')";
                    }
                    */

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


                    /*
                    string dailycaptype = "";
                    conditions = conditions + " and (";
                    if (ChkDailyDataCap1.Checked == true)
                    {
                        conditions = conditions + "adddata_type like '%" + ChkDailyDataCap1.Text.Replace("Data", "").Trim() + "%' or ";
                        dailycaptype = dailycaptype + ChkDailyDataCap1.Text.Replace("Data", "").Trim() + ",";
                    }
                    if (ChkDailyDataCap2.Checked == true)
                    {
                        conditions = conditions + "adddata_type like '%" + ChkDailyDataCap2.Text.Replace("Data", "").Trim() + "%' or ";
                        dailycaptype = dailycaptype + ChkDailyDataCap2.Text.Replace("Data", "").Trim() + ",";
                    }
                    if (ChkDailyDataCap3.Checked == true)
                    {
                        conditions = conditions + "adddata_type like '%" + ChkDailyDataCap3.Text.Replace("Data", "").Trim() + "%' or ";
                        dailycaptype = dailycaptype + ChkDailyDataCap3.Text.Replace("Data", "").Trim() + ",";
                    }
                    if (conditions.Substring(conditions.Length - 4, 4) == " or ")
                    {
                        conditions = conditions.Substring(0, conditions.Length - 4);
                    } 
                    conditions = conditions + ")";
                    conditions = conditions.Replace(" and ()", "");
                    */

                    string dailycaptype = "";
                    if (ChkDailyDataCap1.Checked == true || ChkDailyDataCap2.Checked == true || ChkDailyDataCap3.Checked == true)
                    {
                        conditions = conditions + " and (adddata_total2g > 999999 or ";  // for implementing 'OR' conditions, an additional condition is required.
                        if (ChkDailyDataCap1.Checked == true)
                        {
                            conditions = conditions + "adddata_total2g>0 or ";
                            dailycaptype = dailycaptype + ChkDailyDataCap1.Text.Replace("Data", "").Trim() + ",";
                        }
                        if (ChkDailyDataCap2.Checked == true)
                        {
                            conditions = conditions + "adddata_total3g>0 or ";
                            dailycaptype = dailycaptype + ChkDailyDataCap2.Text.Replace("Data", "").Trim() + ",";
                        }
                        if (ChkDailyDataCap3.Checked == true)
                        {
                            conditions = conditions + "adddata_total4g>0 or ";
                            dailycaptype = dailycaptype + ChkDailyDataCap3.Text.Replace("Data", "").Trim() + ",";
                        }
                        if (conditions.Substring(conditions.Length - 4, 4) == " or ")
                        {
                            conditions = conditions.Substring(0, conditions.Length - 4);
                        }
                        conditions = conditions + ")";
                        conditions = conditions.Replace(" and ()", "");

                        if (dailycaptype != "")
                        {
                            if (RadDailyDataCap.SelectedIndex == 0)
                            {
                                conditions = conditions + " and (adddata_daycap >0  and adddata_daycap <= 1 and adddata_unit='GB')";
                            }
                            if (RadDailyDataCap.SelectedIndex == 1)
                            {
                                conditions = conditions + " and (adddata_daycap > 1 and adddata_daycap <= 2 and adddata_unit='GB')";
                            }
                            if (RadDailyDataCap.SelectedIndex == 2)
                            {
                                conditions = conditions + " and (adddata_daycap > 2 and adddata_unit='GB')";
                            }
                        }

                    }

                }



                // Advance Filter Conditions //

                if (otype == "TSP")
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
                if (otype == "ISP")
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

                    if (ISP_SSA != "")
                    {
                        conditions = conditions + " and (upper(SSA) like '%" + ISP_SSA.ToUpper() + "%' or upper(SSA)='ALL INDIA')";
                    }

                    if (ChkISPFUP.Checked == true)
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

                    if (TextDate3.Text.Trim() != "")
                    {
                        conditions = conditions + " and (upper(regprom)='REGULAR' or (upper(regprom)='PROMOTIONAL' and offertill<='" + ISP_Dt3.ToString("yyyy-MM-dd") + "'))";
                    }
                }

                // Advance Filter Conditions - CODE ENDS HERE //


                TextConditions.Text = conditions;


                showRecords(null, null);

                ButtonCancel_Click(null, null);   // To hide the summary pop up

                setManual(null, null);
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }



    protected void showRecords(object sender, EventArgs e)
    {
        try
        {
            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

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

            if (RadProvider.SelectedItem.Text.Trim() == "ISP")
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

            string strheader = "<table cellspacing=0 cellpadding=3 width=100%><tr><td align=left valign=bottom class=tablecell><b>Found " + matching.ToString() + " Matching Records in " + totcount.ToString() + " Records</b><br /><br /><a href=javascript:funCompareSend() ><img src=../images/btncompare.jpg width=100 border=0 /></a></td>";
            strheader = strheader + "<td align=right valign=bottom class=tablecell><a href=javascript:funExcel() ><img src=../images/excel.jpg border=0 /></a>&nbsp;&nbsp;&nbsp;&nbsp;<a href=javascript:funXML() ><img src=../images/xml.jpg border=0 /></a></td></tr>";
            strheader = strheader + "</table>";

            int colspansize = 7;
            if (RadProvider.SelectedItem.Text.Trim() == "ISP")
            {
                colspansize = 6;
            }

            /*
            strheader = strheader + "<table cellspacing=1 cellpadding=3 width=100%><tr>";
            strheader = strheader + "<td align=center valign=bottom class=tablehead width=2%>&nbsp;</td>";
            strheader = strheader + "<td align=center valign=bottom class=tablehead width=14%><b>Product</b><br /><a href=javascript:funsort('ttype','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('ttype','desc');><img src=../images/sortdown.png border=0></a></td>";
            if (RadMobile.Text == "Landline" || RadPrePost.Text == "Postpaid")
            {
                strheader = strheader + "<td align=center valign=bottom class=tablehead width=19%><b>Monthly Rental</b><br /><a href=javascript:funsort('mrp','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=../images/sortdown.png border=0></a></td>";
            }
            else
            {
                strheader = strheader + "<td align=center valign=bottom class=tablehead width=9%><b>Price</b><br /><a href=javascript:funsort('mrp','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=../images/sortdown.png border=0></a></td>";
            }
            strheader = strheader + "<td align=center valign=bottom class=tablehead width=14%><b>Talktime</b><br /><a href=javascript:funsort('monval','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('monval','desc');><img src=../images/sortdown.png border=0></a></td>";
            strheader = strheader + "<td align=center valign=bottom class=tablehead width=14%><b>Validity</b><br /><a href=javascript:funsort('validity','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('validity','desc');><img src=../images/sortdown.png border=0></a></td>";
            strheader = strheader + "<td align=center valign=bottom class=tablehead width=9%><b>TSP</b><br /><a href=javascript:funsort('oper','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('oper','desc');><img src=../images/sortdown.png border=0></a></td>";
            strheader = strheader + "<td align=center valign=bottom class=tablehead><b>Tariff Summary<br />&nbsp;</b></td>";
            strheader = strheader + "</tr>";
            strheader = strheader + "</table>";
            */

            divheaders.InnerHtml = strheader;


            tbresults = new Table();
            tbresults.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            tbresults.CellPadding = 3;
            tbresults.CellSpacing = 1;
            tbresults.BorderWidth = 0;

            /*
            TableRow tra = new TableRow();
            TableCell tca1 = new TableCell();
            TableCell tca2 = new TableCell();
            tca1.ColumnSpan = 5;
            tca2.ColumnSpan = 2;
            tca1.CssClass = "tablecell";
            tca2.CssClass = "tablecell";
            tca1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tca2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            tca2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Bottom;
            tca1.Text = "<b>Found " + matching.ToString() + " Matching Records in " + totcount.ToString() + " Records</b><br /><br /><a href=javascript:funCompareSend() ><img src=../images/btncompare.jpg width=100 border=0 /></a>";
            tca2.Text = "<a href=javascript:funExcel() ><img src=../images/excel.jpg border=0 /></a>&nbsp;&nbsp;&nbsp;&nbsp;<a href=javascript:funXML() ><img src=../images/xml.jpg border=0 /></a>";
            tra.Controls.Add(tca1);
            tra.Controls.Add(tca2);
            tbresults.Controls.Add(tra);
            

            TableRow trr = new TableRow();
            TableCell tcc0 = new TableCell();
            TableCell tcc1 = new TableCell();
            TableCell tcc2 = new TableCell();
            TableCell tcc3 = new TableCell();
            TableCell tcc4 = new TableCell();
            TableCell tcc5 = new TableCell();
            TableCell tcc6 = new TableCell();
            tcc0.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcc5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcc6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcc0.CssClass = "tablehead";
            tcc1.CssClass = "tablehead";
            tcc2.CssClass = "tablehead";
            tcc3.CssClass = "tablehead";
            tcc4.CssClass = "tablehead";
            tcc5.CssClass = "tablehead";
            tcc6.CssClass = "tablehead";
            tcc0.Width = System.Web.UI.WebControls.Unit.Percentage(2);
            tcc1.Width = System.Web.UI.WebControls.Unit.Percentage(14);
            tcc1.Text = "<b>Product</b>&nbsp;&nbsp;<a href=javascript:funsort('ttype','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('ttype','desc');><img src=../images/sortdown.png border=0></a>";
            if (RadMobile.Text == "Landline" || RadPrePost.Text == "Postpaid")
            {
                tcc2.Width = System.Web.UI.WebControls.Unit.Percentage(19);
                tcc2.Text = "<b>Monthly Rental</b>&nbsp;&nbsp;<a href=javascript:funsort('mrp','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=../images/sortdown.png border=0></a>";
            }
            else
            {
                tcc2.Width = System.Web.UI.WebControls.Unit.Percentage(9);
                tcc2.Text = "<b>Price</b>&nbsp;&nbsp;<a href=javascript:funsort('mrp','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=../images/sortdown.png border=0></a>";
            }
            tcc3.Width = System.Web.UI.WebControls.Unit.Percentage(14);
            tcc3.Text = "<b>Talktime</b>&nbsp;&nbsp;<a href=javascript:funsort('monval','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('monval','desc');><img src=../images/sortdown.png border=0></a>";
            tcc4.Width = System.Web.UI.WebControls.Unit.Percentage(14);
            tcc4.Text = "<b>Validity</b>&nbsp;&nbsp;<a href=javascript:funsort('validity','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('validity','desc');><img src=../images/sortdown.png border=0></a>";
            tcc5.Width = System.Web.UI.WebControls.Unit.Percentage(9);
            tcc5.Text = "<b>TSP</b>&nbsp;&nbsp;<a href=javascript:funsort('oper','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('oper','desc');><img src=../images/sortdown.png border=0></a>";
            tcc6.Text = "<b>Tariff Summary</b>";
            trr.Controls.Add(tcc0);
            trr.Controls.Add(tcc1);
            trr.Controls.Add(tcc2);
            trr.Controls.Add(tcc3);
            trr.Controls.Add(tcc4);
            trr.Controls.Add(tcc5);
            trr.Controls.Add(tcc6);
            tbresults.Controls.Add(trr);
            */


            Table tbHeading = new Table();
            tbHeading.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            TableRow trv = new TableRow();
            TableCell tcv0 = new TableCell();
            TableCell tcv1 = new TableCell();
            TableCell tcv2 = new TableCell();
            TableCell tcv3 = new TableCell();
            TableCell tcv4 = new TableCell();
            TableCell tcv5 = new TableCell();
            TableCell tcv6 = new TableCell();
            tcv0.Width = System.Web.UI.WebControls.Unit.Percentage(2);
            tcv1.Width = System.Web.UI.WebControls.Unit.Percentage(14);
            if (RadMobile.Text == "Landline" || RadPrePost.Text == "Postpaid")
            {
                tcv2.Width = System.Web.UI.WebControls.Unit.Percentage(19);
            }
            else
            {
                tcv2.Width = System.Web.UI.WebControls.Unit.Percentage(9);
            }
            tcv3.Width = System.Web.UI.WebControls.Unit.Percentage(14);
            tcv4.Width = System.Web.UI.WebControls.Unit.Percentage(14);
            tcv5.Width = System.Web.UI.WebControls.Unit.Percentage(11);
            //tcv6.Width = System.Web.UI.WebControls.Unit.Percentage(2);

            tcv0.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcv1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcv2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcv3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcv4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcv5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcv6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            string cssv = "tablehead";
            tcv0.CssClass = cssv;
            tcv1.CssClass = cssv;
            tcv2.CssClass = cssv;
            tcv3.CssClass = cssv;
            tcv4.CssClass = cssv;
            tcv5.CssClass = cssv;
            tcv6.CssClass = cssv;
            tcv1.Text = "<b>Product</b><br /><a href=javascript:funsort('ttype','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('ttype','desc');><img src=../images/sortdown.png border=0></a>";
            if (RadMobile.Text == "Landline" || RadPrePost.Text == "Postpaid")
            {
                tcv2.Text = "<b>Monthly Rental</b><br /><a href=javascript:funsort('mrp','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=../images/sortdown.png border=0></a>";
            }
            else
            {
                tcv2.Text = "<b>Price</b><br /><a href=javascript:funsort('mrp','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=../images/sortdown.png border=0></a>";
            }
            tcv3.Text = "<b>Talktime</b><br /><a href=javascript:funsort('monval','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('monval','desc');><img src=../images/sortdown.png border=0></a>";
            tcv4.Text = "<b>Validity</b><br /><a href=javascript:funsort('validity','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('validity','desc');><img src=../images/sortdown.png border=0></a>";
            if (RadProvider.SelectedItem.Text.Trim() == "TSP")
            {
                tcv5.Text = "<b>TSP</b><br /><a href=javascript:funsort('oper','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('oper','desc');><img src=../images/sortdown.png border=0></a>";
            }
            else
            {
                tcv5.Text = "<b>ISP</b><br /><a href=javascript:funsort('oper','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('oper','desc');><img src=../images/sortdown.png border=0></a>";
            }
            tcv6.Text = "<div style=font-size:12px;width:125px;><b>Tariff Summary<br />&nbsp;</b></div>";
            trv.Controls.Add(tcv0);
            trv.Controls.Add(tcv1);
            trv.Controls.Add(tcv2);
            if (RadProvider.SelectedItem.Text.Trim() == "TSP")
            {
                trv.Controls.Add(tcv3);
            }
            trv.Controls.Add(tcv4);
            trv.Controls.Add(tcv5);
            trv.Controls.Add(tcv6);
            tbHeading.Controls.Add(trv);

            divheaders.Controls.Add(tbHeading);

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
                /*
                if (moredivcntr == 0)
                {
                    TableRow trv = new TableRow();
                    TableCell tcv0 = new TableCell();
                    TableCell tcv1 = new TableCell();
                    TableCell tcv2 = new TableCell();
                    TableCell tcv3 = new TableCell();
                    TableCell tcv4 = new TableCell();
                    TableCell tcv5 = new TableCell();
                    TableCell tcv6 = new TableCell();
                    tcv0.Width = System.Web.UI.WebControls.Unit.Percentage(2);
                    tcv1.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                    if (RadMobile.Text == "Landline" || RadPrePost.Text == "Postpaid")
                    {
                        tcv2.Width = System.Web.UI.WebControls.Unit.Percentage(19);
                    }
                    else
                    {
                        tcv2.Width = System.Web.UI.WebControls.Unit.Percentage(9);
                    }
                    tcv3.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                    tcv4.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                    tcv5.Width = System.Web.UI.WebControls.Unit.Percentage(9);
                    //tcv6.Width = System.Web.UI.WebControls.Unit.Percentage(2);

                    tcv0.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tcv1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tcv2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tcv3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tcv4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tcv5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tcv6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    string cssv = "tablehead";
                    tcv0.CssClass = cssv;
                    tcv1.CssClass = cssv;
                    tcv2.CssClass = cssv;
                    tcv3.CssClass = cssv;
                    tcv4.CssClass = cssv;
                    tcv5.CssClass = cssv;
                    tcv6.CssClass = cssv;
                    tcv1.Text = "<b>Product</b><br /><a href=javascript:funsort('ttype','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('ttype','desc');><img src=../images/sortdown.png border=0></a>";
                    if (RadMobile.Text == "Landline" || RadPrePost.Text == "Postpaid")
                    {
                        tcv2.Text = "<b>Monthly Rental</b><br /><a href=javascript:funsort('mrp','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=../images/sortdown.png border=0></a>";
                    }
                    else
                    {
                        tcv2.Text = "<b>Price</b><br /><a href=javascript:funsort('mrp','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=../images/sortdown.png border=0></a>";
                    }
                    tcv3.Text = "<b>Talktime</b><br /><a href=javascript:funsort('monval','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('monval','desc');><img src=../images/sortdown.png border=0></a>";
                    tcv4.Text = "<b>Validity</b><br /><a href=javascript:funsort('validity','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('validity','desc');><img src=../images/sortdown.png border=0></a>";
                    tcv5.Text = "<b>TSP</b><br /><a href=javascript:funsort('oper','asc');><img src=../images/sortup.png border=0></a>&nbsp;&nbsp;<a href=javascript:funsort('oper','desc');><img src=../images/sortdown.png border=0></a>";
                    tcv6.Text = "<b>Tariff Summary<br />&nbsp;</b>";
                    trv.Controls.Add(tcv0);
                    trv.Controls.Add(tcv1);
                    trv.Controls.Add(tcv2);
                    trv.Controls.Add(tcv3);
                    trv.Controls.Add(tcv4);
                    trv.Controls.Add(tcv5);
                    trv.Controls.Add(tcv6);
                    tbresults.Controls.Add(trv);
                }
                */

                if (!idlist.Contains(dr["uniqueid"].ToString().Trim()))
                {
                    string oper = dr["oper"].ToString().Trim();
                    string operlogo = "logo" + oper.Replace(" ", "") + ".jpg";
                    string ttype = dr["ttype"].ToString().Replace("Prepaid_Plan Voucher", "Plan").Replace("Prepaid_STV", "STV").Replace("Prepaid_Combo", "Combo").Replace("Prepaid_Top Up", "Top Up").Replace("Prepaid_VAS", "VAS").Replace("Promo Offer", "Promo").Replace("Postpaid- Plan", "Plans").Replace("Postpaid- Add On Pack", "Add On Pack").Replace("Postpaid_VAS", "VAS").Replace("Fixed Line -Tariff", "Plan").Replace("Fixed Line Add-On Pack", "Add On Pack").Replace("Postpaid_VAS", "VAS");

                    TableRow tr = new TableRow();
                    TableCell tc0 = new TableCell();
                    TableCell tc1 = new TableCell();
                    TableCell tc2 = new TableCell();
                    TableCell tc3 = new TableCell();
                    TableCell tc4 = new TableCell();
                    TableCell tc5 = new TableCell();
                    TableCell tc6 = new TableCell();
                    tc0.Width = System.Web.UI.WebControls.Unit.Percentage(2);
                    tc1.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                    if (RadMobile.Text == "Landline" || RadPrePost.Text == "Postpaid")
                    {
                        tc2.Width = System.Web.UI.WebControls.Unit.Percentage(19);
                    }
                    else
                    {
                        tc2.Width = System.Web.UI.WebControls.Unit.Percentage(9);
                    }
                    tc3.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                    tc4.Width = System.Web.UI.WebControls.Unit.Percentage(14);
                    tc5.Width = System.Web.UI.WebControls.Unit.Percentage(11);
                    //tc6.Width = System.Web.UI.WebControls.Unit.Percentage(2);

                    tc0.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tc5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tc6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    string css = "tablecell";
                    tc0.CssClass = css;
                    tc1.CssClass = css;
                    tc2.CssClass = css;
                    tc3.CssClass = css;
                    tc4.CssClass = css;
                    tc5.CssClass = css;
                    tc6.CssClass = css;

                    arrResult[chkpos].ID = ttype + "~" + dr["rno"].ToString().Trim();
                    arrResult[chkpos].Attributes.Add("onchange", "funComparison('" + ttype + "','" + arrResult[chkpos].ID.ToString() + "')");
                    //arrResult[chkpos].Text = dr["rno"].ToString().Trim(); 
                    tc0.Controls.Add(arrResult[chkpos]);
                    tc1.Text = ttype;
                    if (RadMobile.Text == "Landline" || dr["ttype"].ToString().Trim() == "Postpaid Plan" || dr["ttype"].ToString().Trim() == "Postpaid Add On Pack")
                    {
                        //if (dr["fixed_monthly"].ToString().Trim() == "-1")
                        if (dr["ISP_rental"].ToString().Trim() == "-1")
                        {
                            tc2.Text = "-";
                        }
                        else
                        {
                            //tc2.Text = "&#8377; " + dr["fixed_monthly"].ToString().Trim();
                            tc2.Text = "&#8377; " + dr["ISP_rental"].ToString().Trim();
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(dr["mrp"].ToString().Trim()) <= 0)
                        {
                            tc2.Text = "-";
                        }
                        else
                        {
                            tc2.Text = "&#8377; " + dr["mrp"].ToString().Trim();
                        }
                    }
                    if (dr["monval"].ToString().Trim() == "-1")
                    {
                        tc3.Text = "-";
                    }
                    else
                    {
                        tc3.Text = "&#8377; " + dr["monval"].ToString().Trim();
                    }
                    if (dr["validity"].ToString().Trim() == "0" || dr["validity"].ToString().Trim() == "-1")
                    {
                        tc4.Text = "-";
                    }
                    else
                    {
                        if (dr["validity"].ToString().Trim() == "-2")
                        {
                            tc4.Text = "Unlimited";
                        }
                        else
                        {
                            tc4.Text = dr["validity"].ToString().Trim() + " days";
                        }
                    }
                    tc5.Text = "<img src=../logos/" + operlogo + " width=70% border=0 />";


                    string moredet = "<div id='divmore" + moredivcntr.ToString().Trim() + "' style=display:none;margin-top:-20px;>";
                    moredet = moredet + "<table width=100% cellspacing=1 border=1 style=border-collapse:collapse;border-color:#cfcfcf; cellpadding=3>";
                    if (RadProvider.SelectedItem.Text.Trim() == "TSP")
                    {
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

                    string totdata = "";
                        
                    if (RadProvider.SelectedItem.Text.Trim() == "TSP")
                    {
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>Total Data</b></td></tr>";
                        moredet = moredet + "<tr>";
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
                        moredet = moredet + "<tr><td class=tableheadsmall colspan=8 align=center><b>Data Speed</b></td></tr>";
                        moredet = moredet + "<tr>";
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total2g"].ToString().Trim() != "-1")
                        {
                            //totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Data Usage Limit With Higher Speed : " + dr["adddata_total2g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Data Usage Limit With Higher Speed : " + dr["adddata_total2g"].ToString().Trim().Replace("-1", "0") + " " + "GB" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total3g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Speed of Connection Upto Data Usage Limit : " + dr["adddata_total3g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        if (dr["adddata_unit"].ToString().Trim() != "" && dr["adddata_unit"].ToString().Trim() != "-1" && dr["adddata_total4g"].ToString().Trim() != "-1")
                        {
                            totdata = totdata + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Speed of Connection Beyond Data Usage Limit : " + dr["adddata_total4g"].ToString().Trim().Replace("-1", "0") + " " + dr["adddata_unit"].ToString().Trim().Replace("-1", "") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
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
                    moredet = moredet + "<td class=tablecellsmall align=center colspan=8>" + totdata + "</td>";
                    moredet = moredet + "</tr>";
                    if (dr["offerconditions"].ToString().Replace("-1", "") != "")
                    {
                        moredet = moredet + "<tr><td class=tablecellsmall align=left colspan=8><b>Eligibility Conditions : </b>" + dr["offerconditions"].ToString().Replace("-1", "") + "</td></tr>";
                    }
                    /*
                    if (dr["misc_remarks"].ToString().Replace("-1", "") != "")
                    {
                        moredet = moredet + "<tr><td class=tablecellsmall align=left colspan=8><b>Remarks : </b>" + dr["misc_remarks"].ToString().Replace("-1", "") + "</td></tr>";
                    }
                    */ 
                    if (dr["misc_terms"].ToString().Replace("-1", "") != "")
                    {
                        moredet = moredet + "<tr><td class=tablecellsmall align=left colspan=8><b>Terms & Conditions : </b>" + dr["misc_remarks"].ToString().Replace("-1", "") + "</td></tr>";
                    }

                    moredet = moredet + "</table>";

                    moredet = moredet + "</div>";

                    tc6.Text = dr["tariffdet"].ToString().Trim().Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)) + "<p id='p" + moredivcntr.ToString().Trim() + "' align=right><a href=javascript:funmore('" + moredivcntr.ToString().Trim() + "') class=indexlinks1>More...</a></p>";

                    tr.Controls.Add(tc0);
                    tr.Controls.Add(tc1);
                    tr.Controls.Add(tc2);
                    if (RadProvider.SelectedItem.Text.Trim() == "TSP")
                    {
                        tr.Controls.Add(tc3);
                    }
                    tr.Controls.Add(tc4);
                    tr.Controls.Add(tc5);
                    tr.Controls.Add(tc6);
                    tbresults.Controls.Add(tr);

                    TableRow trcc = new TableRow();
                    trcc.Height = System.Web.UI.WebControls.Unit.Pixel(1);
                    TableCell tccc1 = new TableCell();
                    tccc1.ColumnSpan = colspansize;
                    tccc1.CssClass = "tablecell";
                    tccc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tccc1.Text = moredet;
                    trcc.Controls.Add(tccc1);
                    tbresults.Controls.Add(trcc);

                    TableRow trd = new TableRow();
                    TableCell tcd1 = new TableCell();
                    tcd1.ColumnSpan = colspansize;
                    tcd1.CssClass = "tablecell";
                    tcd1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tcd1.Text = "<hr size=0 color=#eaeaea />";
                    trd.Controls.Add(tcd1);
                    tbresults.Controls.Add(trd);



                    chkpos++;
                    moredivcntr++;
                    TextResultCntr.Text = chkpos.ToString();

                    idlist += dr["uniqueid"].ToString().Trim() + ",";
                }
            }
            con.Close();


            TableRow trb = new TableRow();
            TableCell tcb1 = new TableCell();
            TableCell tcb2 = new TableCell();
            tcb1.ColumnSpan = colspansize - 2;
            tcb2.ColumnSpan = 2;
            tcb1.CssClass = "tablecell";
            tcb2.CssClass = "tablecell";
            tcb1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            tcb2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            tcb2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Bottom;
            tcb1.Text = "<a href=javascript:funCompareSend() ><img src=../images/btncompare.jpg width=100 border=0 /></a>";
            tcb2.Text = "";
            trb.Controls.Add(tcb1);
            trb.Controls.Add(tcb2);
            tbresults.Controls.Add(trb);


            divresults.Controls.Add(tbresults);



            try
            {
                for (int i = 0; i < ChkPlans.Items.Count; i++)
                {
                    if (ChkPlans.Items[i].Text == "All Tariffs")
                    {
                        ChkPlans.Items[i].Attributes.Add("title", "All Tariffs");
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


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }




    protected void ButtonPopUp_Click(object sender, EventArgs e)
    {
        try
        {
            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            int flag = 0;

            string mob = RadMobile.SelectedItem.Text.Trim();
            string prepost = RadPrePost.SelectedItem.Text.Trim();
            string circ = DropCircle.SelectedItem.Text.Trim();

            string oper = "," + DropOperator.SelectedItem.Text.Trim() + ",";
            for (int i = 0; i < size; i++)
            {
                if (charr[i].Checked == true)
                {
                    oper = oper + charr[i].Text.Trim() + ",";
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



            if (circ == "CIRCLE")
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
                string myselection = "<table width=500px border=1 style=border-collapse:collapse;border-radius:10px; cellspacing=1 cellpadding=8>";
                myselection = myselection + "<tr><td class=tablehead colspan=2 style='background-color:#48baff;color:#ffffff;'>You Have Selected The Following Options</td></tr>";
                if (RadProvider.SelectedItem.Text.Trim() == "TSP")
                {
                    myselection = myselection + "<tr><td class=tableheadsmall width=50%>Mobile / Landline</td><td class=tableheadsmall>" + RadMobile.SelectedItem.Text.Trim() + "</td></tr>";
                }
                myselection = myselection + "<tr><td class=tableheadsmall>Prepaid / Postpaid</td><td class=tableheadsmall>" + RadPrePost.SelectedItem.Text.Trim() + "</td></tr>";
                myselection = myselection + "<tr><td class=tableheadsmall width=50%>Circle</td><td class=tableheadsmall>" + DropCircle.SelectedItem.Text.Trim() + "</td></tr>";
                string seloper = ",";
                if (DropOperator.SelectedItem.Text.Trim() == "All Operators")
                {
                    seloper = DropOperator.SelectedItem.Text.Trim();
                }
                else
                {
                    seloper = seloper + DropOperator.SelectedItem.Text.Trim() + ",";
                    for (int i = 0; i < size; i++)
                    {
                        if (charr[i].Checked == true && !seloper.Contains(charr[i].Text.Trim()))
                        {
                            seloper = seloper + charr[i].Text.Trim() + ",";
                        }
                    }
                    if (seloper.Length > 3)
                    {
                        seloper = seloper.Substring(1, seloper.Length - 2);
                    }
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

                if (divDailyDataCapping.Visible == true)
                {
                    string dailycap = "";
                    if (ChkDailyDataCap1.Checked == true)
                    {
                        dailycap = dailycap + ChkDailyDataCap1.Text.Replace("Data", "").Trim() + " / ";
                    }
                    if (ChkDailyDataCap2.Checked == true)
                    {
                        dailycap = dailycap + ChkDailyDataCap2.Text.Replace("Data", "").Trim() + " / ";
                    }
                    if (ChkDailyDataCap3.Checked == true)
                    {
                        dailycap = dailycap + ChkDailyDataCap3.Text.Replace("Data", "").Trim() + " / ";
                    }
                    if (dailycap != "")
                    {
                        if (RadDailyDataCap.SelectedIndex > -1)
                        {
                            dailycap = dailycap.Trim();
                            dailycap = dailycap.Substring(0, dailycap.Length - 1);
                            dailycap = dailycap + " (" + RadDailyDataCap.SelectedItem.Text.Trim() + ")";
                            myselection = myselection + "<tr><td class=tableheadsmall>Daily Data Capping</td><td class=tableheadsmall>" + dailycap + "</td></tr>";
                        }
                    }
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
            Response.Write(ex.ToString());
        }
    }







    /*

    protected void ButtonExcelOLD_Click(object sender, EventArgs e)
    {
        try
        {

            getMaxRno("rno", "TRAI_downloadcounter");
            com = new MySqlCommand("insert into TRAI_downloadcounter values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','Excel')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            string excelstr = "<table width=100% border=1 style=border-collapse:collapse cellspacing=1 cellpadding=5>";
            string conditions = TextConditions.Text.Trim();
            string sortby = " order by oper";
            //int totcols=266;

            try
            {

                // First Header Row //
                excelstr = excelstr + "<tr>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=4><b>License Details</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b>One Time Charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Validity</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=4><b>Time For Call Rates</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=89><b>Call charges(Regular)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=11><b>Call charges while Roaming</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>SMS charges while Roaming</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=20><b>Charges while International Roaming</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=3><b>Data Charges (Please specify charges in paisa/ quantum of data transfer e.g 2 paisa / 10KB )</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b>Charges while Roaming  International</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b>Duration for Additional Benefits</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=61><b>Additional Benefits (Mins/Secs/SMS)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=13><b>Additional Data</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=11><b>Miscellaneous</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>One Time Charges, if any (specify whether convertible to security deposit)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=3><b>Details of the Service</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=6><b>Security Deposit (Refundable), if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=6><b>Fixed Charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=4><b>Optional fixed Monthly charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=6><b>Free calls in MCUS per month</b></td>";
                //excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=3><b></b></td>";
                excelstr = excelstr + "</tr>";

                // Second Header Row //
                excelstr = excelstr + "<tr>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=200><b>Tariff Product<b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=400><b>Tariff Summary<b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Operator Name</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Circle (Service Area)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Service (GSM / CDMA / LTE)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Category (Prepaid / Postpaid)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Name of the Product</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Unique ID's of the Plans for which exclusively applicable</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Date of Launch</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Regular / Promotional</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Time duration of Promotional / limited period offer starts from</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Time duration of Promotional / limited period offer valid till</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Eligibility Conditions, if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Price (Including Processing Fee & GST)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Monetory Value (in Rs.) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Product Validity ( in days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b>Peak Timing</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b>Off Peak Timing</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=4><b>All Local Call Charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>Local Call Charges Mobile OnNet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>Local Call Charges Mobile OffNet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>Local Call Charges Fixed OnNet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>Local Call Charges Fixed OffNet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of local call charges / pulse rate in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=4><b>All STD Call Charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>STD Call Charges Mobile OnNet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>STD Call Charges Mobile OffNet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>STD Call Charges Fixed OnNet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>STD Call Charges Fixed OffNet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of STD call charges / pulse rate in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=9><b>SMS Charges (in INR/SMS)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of SMS charges , in case SMS charges follow some other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=8><b>ISD call charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of ISD call charges / pulse rate in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Pulse (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b>Incoming</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b>Local Outgoing</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=2><b>STD Outgoing</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Web link for National outgoing ISD call charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Call Charges while Roaming in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Local</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>National</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>International</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Any other (please specify)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of SMS Charges while Roaming in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=20><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>CUP (Commercial Usage Policy)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Charges while International Roaming in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Home</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Roaming</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Conditions pertaining to data charges , if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Monthly Rental for International roaming (in Rs.)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Weblink for Charges while international roaming</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Time From</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Time To</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>Additional Local (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Local in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>Additional STD (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional STD in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>Additional Local & STD (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Local & STD in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=3><b>Additional Local, STD & Roaming  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Local, STD & Roaming in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=7><b>Additional Roaming (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Roaming in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=11><b>Additional SMS</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional SMS in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=3><b>Additional ISD</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional ISD in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=8><b>Additional Video Call</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Video Call in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; colspan=12><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Description of Additional Data in case they follow any other pattern</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Special benefits , if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Other charges, if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Remarks, if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Terms and Conditions,  if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Mode of Activation / Recharge (Website / App only / paper / USSD / 3rd party Wallet etc)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Whether details of this service have been uploaded on the website</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>TSP website link of the Plan</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Any other declarations</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Condition for termination of Product if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Activation Code If Any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Deactivation Code If Any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Available in (Rural / Urban/ Both / Any other)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Registration charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Installation / activation charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>One time Security Deposit</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Plan enrolment fee, if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Other one time charges (Please specify)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>If yes please specify</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Pay per use model (Usage charges)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Any other model (Details)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Benefits </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Local</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Local + STD </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Local + STD + ISD</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>National Roaming</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>International Roaming</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Any Other, Please Specify</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Compulsory Fixed monthly charges including Rental / Minimum billing amount, if any  </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>If Exchange capacity wise FMC (Charges for exchange capacity <=999)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>If Exchange capacity wise FMC(Charges for exchange capacity> 999 and <=29999)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>If Exchange capacity wise FMC(Charges for exchange capacity >= 30000 and <=99999)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>If Exchange capacity wise FMC(Charges for exchange capacity >= 100000)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>If Flat FMC(Flat monthly charges)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Advance rental option for longer periods </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>CLIP</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Any other</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>If yes please specify</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month for exchange capacity <=999</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month for exchange capacity> 999 and <=29999</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month for exchange capacity >= 30000 and <=99999</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Free calls in MCUS per month for exchange capacity >= 100000</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Free talk value in Rs. (per month)</b></td>";
                //excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Details of Service</b></td>";
                //excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Benefits</b></td>";
                //excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef;><b>Rental of Pack (Including Processing Fee & GST)</b></td>";
                excelstr = excelstr + "</tr>";

                // Third Header Row //
                excelstr = excelstr + "<tr>";
                for (int i = 1; i <= 16; i++)
                {
                    excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";   // blank cells
                }
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>From</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>To</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>From</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>To</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs) (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs) (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs) (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs) (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs) (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs) (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Pulse  (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice Call charges (off peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video Call charges (off Peak Hrs)  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>All Local</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>All National</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Onnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Offnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>National Onnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>National Offnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>International</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Terms and conditions including conditions pertaining to SMS charges, if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Countries List </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Pulse Rate (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Calls to Mobile  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD calls to Landline  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Video calls  (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Weblink for ISD call charges</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Conditions, if any</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Voice (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Video (in INR/pulse)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>in INR/SMS</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>in INR/SMS</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>in INR/SMS</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Countries</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Incoming Pulse (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Incoming Calls</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Outgoing Local Calls (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing  Local Calls</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Outgoing Calls to India (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing Calls to India</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Outgoing Calls to Other Countries (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing Calls to Other Countries</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Video call (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing SMS</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Incoming SMS</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Incoming Free Usage (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Incoming Free Usage (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Pulse  Outgoing Free Usage (in seconds)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Outgoing Free Usage (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Unit  Free Data Usage</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Free Data Usage</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>ISD Free SMS</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                for (int i = 1; i <= 9; i++)
                {
                    excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                }
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Onnet  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Offnet  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Mobile  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Onnet Mobile  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Offnet Mobile  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days) </b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD Onnet  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD Offnet  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD Mobile  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD Onnet Mobile  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD Offnet Mobile  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Onnet (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Offnet (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Mobile (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Onnet Mobile (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Offnet Mobile (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local, STD & Roaming (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local, STD & Roaming Mobile (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Incoming & Outgoing (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Incoming Free (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Outgoing Free (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Outgoing Local & STD Mobile Free  (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Outgoing Local Free (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Outgoing STD Free (in minutes)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & National</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & National Onnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & National Offnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Onnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Offnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>National</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>National Onnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>National Offnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>International</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Summary of ISD freebies</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Weblink for ISD freebies</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local & STD Video</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Video</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Video Onnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Local Video Offnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD Video</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD Video Onnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>STD Video Offnet</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Unit (MB / GB)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Total 2G Data</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Total 3G Data</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Total 4G Data</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Total Data</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Day/Night Data Capping</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Weekly Data Capping</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Monthly Data Capping</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Carry Forward (Yes / No)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Validity (In Days)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>FUP, if any (Yes/No)</b></td>";
                excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b>Condition, if any</b></td>";
                for (int i = 1; i <= 44; i++)
                {
                    excelstr = excelstr + "<td align=center valign=top style=background-color:#efefef; width=150><b></b></td>";
                }
                excelstr = excelstr + "</tr>";



                // Data Rows //
                com = new MySqlCommand("select * from " + tablename + " where(rno>0) " + conditions + sortby, con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    excelstr = excelstr + "<tr>";
                    for (int i = 4; i <= 5; i++)     // 'Tariff Product' till 'Tariff Summary' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)) + "</td>";
                    }
                    for (int i = 7; i <= 8; i++)     // 'Operator' till 'Circle' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34)) + "</td>";
                    }
                    excelstr = excelstr + "<td align=center valign=top>" + dr[10].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";    // 'Service'
                    excelstr = excelstr + "<td align=center valign=top>" + dr[11].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";    // 'Category'
                    excelstr = excelstr + "<td align=center valign=top>" + dr[14].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";    // 'Plan Name'
                    if (dr["ttype"].ToString().Trim() == "PREPAID PLAN" || dr["ttype"].ToString().Trim().ToUpper() == "POSTPAID PLAN")
                    {
                        //excelstr = excelstr + "<td align=center valign=top>" + dr[15].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";    // 'Unique ID of Plan'
                        excelstr = excelstr + "<td align=center valign=top></td>";
                    }
                    else
                    {
                        //excelstr = excelstr + "<td align=center valign=top></td>";
                        excelstr = excelstr + "<td align=center valign=top>" + dr[15].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";    // 'Unique ID of Plan for which exclusively available'
                    }

                    if (Convert.ToDateTime(dr[17].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + Convert.ToDateTime(dr[17].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>";      // 'Date of Launch / Activation etc'
                    }
                    else
                    {
                        excelstr = excelstr + "<td align=center valign=top></td>";
                    }

                    excelstr = excelstr + "<td align=center valign=top>" + dr[18].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";    // 'Regular/Promotional'

                    if (Convert.ToDateTime(dr[19].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + Convert.ToDateTime(dr[19].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>";   // 'Time duration of Promotional / limited period offer starts from'
                    }
                    else
                    {
                        excelstr = excelstr + "<td align=center valign=top></td>";
                    }
                    if (Convert.ToDateTime(dr[20].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + Convert.ToDateTime(dr[20].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>";      // 'Time duration of Promotional / limited period offer valid till'
                    }
                    else
                    {
                        excelstr = excelstr + "<td align=center valign=top></td>";
                    }

                    for (int i = 21; i <= 23; i++)     // 'Eligibility Conditions' till 'Monetory Value' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";
                    }
                    excelstr = excelstr + "<td align=center valign=top>" + dr[51].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";  // 'Validity'


                    for (int i = 55; i <= 58; i++)     // 'Time for call rates' columns
                    {
                        if (dr[i].ToString().Trim() != "")
                        {
                            try
                            {
                                excelstr = excelstr + "<td align=center valign=top>" + Convert.ToDouble(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "")).ToString("0.00").Replace(".", ":") + " " + "</td>";
                            }
                            catch (Exception ex)
                            {
                                excelstr = excelstr + "<td align=center valign=top></td>";
                            }
                        }
                        else
                        {
                            excelstr = excelstr + "<td align=center valign=top></td>";
                        }
                    }
                    for (int i = 59; i <= 279; i++)     // 'Local All' till 'Misc - Deactivation Code' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";
                    }
                    for (int i = 24; i <= 27; i++)     // 'Rural / Urban' till 'Installation / Activation Charges' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";
                    }
                    for (int i = 30; i <= 31; i++)     // 'Plan Enrollment Fee' till 'othercharges' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";
                    }
                    excelstr = excelstr + "<td align=center valign=top></td>";  // 'OtherChargesDet' - field has been removed now, so show a blank column

                    for (int i = 52; i <= 54; i++)     // 'Pay Per Use' till 'modelbenefits' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";
                    }
                    for (int i = 34; i <= 39; i++)     // 'Security Deposit' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";
                    }
                    //excelstr = excelstr + "<td align=center valign=top>" + dr[42].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";   // 'Fixed Monthly Charges'
                    excelstr = excelstr + "<td align=center valign=top></td>";   // 'Fixed Monthly Charges' - field has been removed, so show blank column
                    for (int i = 40; i <= 43; i++)     // 'Fixed <=999' till 'Fixed100K' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";
                    }
                    for (int i = 1; i <= 2; i++)     // 'FLAT FMC' and 'Advance Rental option' - fields have been removed, so show blank columns
                    {
                        excelstr = excelstr + "<td align=center valign=top></td>";
                    }
                    excelstr = excelstr + "<td align=center valign=top>" + dr[44].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";   // 'Fixed - CLIP'
                    excelstr = excelstr + "<td align=center valign=top></td>";   // 'Fixed - Other Charges' - field have been removed, so show blank columns
                    excelstr = excelstr + "<td align=center valign=top></td>";       // 'Optional Fixed Monthly Charges - If yes please specify'
                    for (int i = 45; i <= 50; i++)     // 'Fixed Calls MCU - Free Calls' till 'Free Talk Value in month' columns
                    {
                        excelstr = excelstr + "<td align=center valign=top>" + dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</td>";
                    }

                    excelstr = excelstr + "</tr>";
                }
                con.Close();

                excelstr = excelstr + "</table>";
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            try
            {
                string attachment = "attachment; filename=Report.xls";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                StringBuilder sb = new StringBuilder();

                sb.Append(excelstr.ToString());

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

    */




    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        try
        {

            getMaxRno("rno", "TRAI_downloadcounter");
            com = new MySqlCommand("insert into TRAI_downloadcounter values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','Excel')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            string conditions = TextConditions.Text.Trim();
            string sortby = " order by oper";
            //int totcols=266;

            string A253 = "";
            string A254 = "";
            string A255 = "";
            string A256 = "";
            
            if (RadProvider.SelectedItem.Text.Trim() == "TSP")
            {
                A253 = "Data Unit (MB / GB)";
                A254 = "Total 2G Data";
                A255 = "Total 3G Data";
                A256 = "Total 4G Data";
            }
            else
            {
                A253 = "Data Speed - Unit (kbps / mbps)";
                A254 = "Data Usage Limit With Higher Speed (GB)";
                A255 = "Speed of Connection Upto Data Usage Limit";
                A256 = "Speed of Connection Beyond Data Usage Limit";
            }

            try
            {

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[273] { new DataColumn("Tariff Product", typeof(string)),
            new DataColumn("Tariff Summary", typeof(string)),
            new DataColumn("Operator Name", typeof(string)),
            new DataColumn("Circle (Service Area)", typeof(string)),
            new DataColumn("Service (GSM / CDMA / LTE)", typeof(string)),
            new DataColumn("Category (Prepaid / Postpaid)", typeof(string)),
            new DataColumn("Name of the Product", typeof(string)),
            new DataColumn("Unique ID's of the Plans for which exclusively applicable", typeof(string)),
            new DataColumn("Date of Launch", typeof(string)),
            new DataColumn("Regular / Promotional", typeof(string)),
            new DataColumn("Time duration of Promotional / limited period offer starts from", typeof(string)),
            new DataColumn("Time duration of Promotional / limited period offer valid till", typeof(string)),
            new DataColumn("Eligibility Conditions, if any", typeof(string)),
            new DataColumn("Price (Including Processing Fee & GST)", typeof(string)),
            new DataColumn("Monetory Value (in Rs.) ", typeof(string)),
            new DataColumn("Product Validity ( in days)", typeof(string)),
            new DataColumn("Peak Timing	From", typeof(string)),
            new DataColumn("Peak Timing	To", typeof(string)),
            new DataColumn("Off Peak Timing	From", typeof(string)),
            new DataColumn("Off Peak Timing	To", typeof(string)),
            new DataColumn("All Local Call Charges - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("All Local Call Charges - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("All Local Call Charges - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("All Local Call Charges - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OnNet - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OnNet - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OnNet - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OnNet - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OnNet - Voice Call charges (off peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OnNet - Video Call charges (off Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OnNet - Validity (In Days)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OffNet - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OffNet - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OffNet - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OffNet - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OffNet - Voice Call charges (off peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OffNet - Video Call charges (off Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Mobile OffNet - Validity (In Days)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OnNet - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OnNet - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OnNet - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OnNet - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OnNet - Voice Call charges (off peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OnNet - Video Call charges (off Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OnNet - Validity (In Days)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OffNet - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OffNet - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OffNet - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OffNet - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OffNet - Voice Call charges (off peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OffNet - Video Call charges (off Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("Local Call Charges Fixed OffNet - Validity (In Days) ", typeof(string)),
            new DataColumn("Local - CUP (Commercial Usage Policy)", typeof(string)),
            new DataColumn("Description of local call charges / pulse rate in case they follow any other pattern", typeof(string)),
            new DataColumn("All STD Call Charges - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("All STD Call Charges - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("All STD Call Charges - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("All STD Call Charges - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OnNet - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OnNet - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OnNet - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OnNet - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OnNet - Voice Call charges (off peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OnNet - Video Call charges (off Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OnNet - Validity (In Days)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OffNet - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OffNet - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OffNet - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OffNet - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OffNet - Voice Call charges (off peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OffNet - Video Call charges (off Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Mobile OffNet - Validity (In Days)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OnNet - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OnNet - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OnNet - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OnNet - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OnNet - Voice Call charges (off peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OnNet - Video Call charges (off Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OnNet - Validity (In Days)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OffNet - Voice Pulse (in seconds)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OffNet - Video Pulse (in seconds)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OffNet - Voice Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OffNet - Video Call charges (Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OffNet - Voice Call charges (off peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OffNet - Video Call charges (off Peak Hrs) (in INR/pulse)", typeof(string)),
            new DataColumn("STD Call Charges Fixed OffNet - Validity (In Days) ", typeof(string)),
            new DataColumn("STD - CUP (Commercial Usage Policy)", typeof(string)),
            new DataColumn("Description of STD call charges / pulse rate in case they follow any other pattern", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - All Local", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - All National", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - Local Onnet", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - Local Offnet", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - National Onnet", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - National Offnet", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - International", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - Terms and conditions including conditions pertaining to SMS charges, if any", typeof(string)),
            new DataColumn("SMS Charges (in INR/SMS) - Validity (In Days)", typeof(string)),
            new DataColumn("SMS - CUP (Commercial Usage Policy)", typeof(string)),
            new DataColumn("Description of SMS charges , in case SMS charges follow some other pattern", typeof(string)),
            new DataColumn("ISD Countries List", typeof(string)),
            new DataColumn("Pulse Rate (in seconds)", typeof(string)),
            new DataColumn("ISD Calls to Mobile (in INR/pulse)", typeof(string)),
            new DataColumn("ISD calls to Landline (in INR/pulse)", typeof(string)),
            new DataColumn("ISD Video calls (in INR/pulse)", typeof(string)),
            new DataColumn("Weblink for ISD call charges", typeof(string)),
            new DataColumn("Conditions, if any", typeof(string)),
            new DataColumn("ISD Validity (In Days)", typeof(string)),
            new DataColumn("ISD - CUP (Commercial Usage Policy)", typeof(string)),
            new DataColumn("Description of ISD call charges / pulse rate in case they follow any other pattern", typeof(string)),
            new DataColumn("Call charges while Roaming - Pulse (in seconds)", typeof(string)),
            new DataColumn("Call charges while Roaming - Incoming Voice (in INR/pulse)", typeof(string)),
            new DataColumn("Call charges while Roaming - Incoming Video (in INR/pulse)", typeof(string)),
            new DataColumn("Call charges while Roaming - Local Outgoing	Voice (in INR/pulse)", typeof(string)),
            new DataColumn("Call charges while Roaming - Local Outgoing	Video (in INR/pulse)", typeof(string)),
            new DataColumn("Call charges while Roaming - STD Outgoing Voice (in INR/pulse)", typeof(string)),
            new DataColumn("Call charges while Roaming - STD Outgoing Video (in INR/pulse)", typeof(string)),
            new DataColumn("Call charges while Roaming - Validity (In Days)", typeof(string)),
            new DataColumn("Web link for National outgoing ISD call charges ", typeof(string)),
            new DataColumn("Roaming CUP (Commercial Usage Policy)", typeof(string)),
            new DataColumn("Description of Call Charges while Roaming in case they follow any other pattern", typeof(string)),
            new DataColumn("SMS charges while Roaming - Local", typeof(string)),
            new DataColumn("SMS charges while Roaming - National", typeof(string)),
            new DataColumn("SMS charges while Roaming - International", typeof(string)),
            new DataColumn("SMS charges while Roaming - Any other (please specify)", typeof(string)),
            new DataColumn("SMS charges while Roaming - Validity (In Days)", typeof(string)),
            new DataColumn("SMS charges while Roaming - CUP (Commercial Usage Policy)", typeof(string)),
            new DataColumn("SMS charges while Roaming - Description of SMS Charges while Roaming in case they follow any other pattern", typeof(string)),
            new DataColumn("International Roaming - ISD Countries", typeof(string)),
            new DataColumn("ISD Incoming Pulse (in seconds)", typeof(string)),
            new DataColumn("ISD Incoming Calls", typeof(string)),
            new DataColumn("ISD Pulse Outgoing Local Calls (in seconds)", typeof(string)),
            new DataColumn("ISD Outgoing Local Calls", typeof(string)),
            new DataColumn("ISD Pulse Outgoing Calls to India (in seconds)", typeof(string)),
            new DataColumn("ISD Outgoing Calls to India", typeof(string)),
            new DataColumn("ISD Pulse Outgoing Calls to Other Countries (in seconds)", typeof(string)),
            new DataColumn("ISD Outgoing Calls to Other Countries", typeof(string)),
            new DataColumn("ISD Video call (in seconds)", typeof(string)),
            new DataColumn("ISD Outgoing SMS", typeof(string)),
            new DataColumn("ISD Incoming SMS", typeof(string)),
            new DataColumn("ISD Pulse Incoming Free Usage (in seconds)", typeof(string)),
            new DataColumn("ISD Incoming Free Usage (in minutes)", typeof(string)),
            new DataColumn("ISD Pulse Outgoing Free Usage (in seconds)", typeof(string)),
            new DataColumn("ISD Outgoing Free Usage (in minutes)", typeof(string)),
            new DataColumn("ISD Unit Free Data Usage", typeof(string)),
            new DataColumn("ISD Free Data Usage", typeof(string)),
            new DataColumn("ISD Free SMS", typeof(string)),
            new DataColumn("Validity - (In Days)", typeof(string)),
            new DataColumn("CUP ( Commercial Usage Policy)", typeof(string)),
            new DataColumn("Description of Charges while International Roaming in case they follow any other pattern", typeof(string)),
            new DataColumn("Date Charges - Home", typeof(string)),
            new DataColumn("Data Charges - Roaming", typeof(string)),
            new DataColumn("Conditions pertaining to data charges , if any", typeof(string)),
            new DataColumn("Monthly Rental for International roaming (in Rs.)", typeof(string)),
            new DataColumn("Weblink for Charges while international roaming", typeof(string)),
            new DataColumn("Addn Benefits - Time From", typeof(string)),
            new DataColumn("Addn Benefits - Time To", typeof(string)),
            new DataColumn("Additional Local (in minutes) - Local (in minutes)", typeof(string)),
            new DataColumn("Additional Local (in minutes) - Local Onnet (in minutes)", typeof(string)),
            new DataColumn("Additional Local (in minutes) - Local Offnet (in minutes)", typeof(string)),
            new DataColumn("Additional Local (in minutes) - Local Mobile (in minutes)", typeof(string)),
            new DataColumn("Additional Local (in minutes) - Local Onnet Mobile (in minutes)", typeof(string)),
            new DataColumn("Additional Local (in minutes) - Local Offnet Mobile (in minutes)", typeof(string)),
            new DataColumn("Additional Local (in minutes) - Validity (In Days) ", typeof(string)),
            new DataColumn("Description of Additional Local in case they follow any other pattern", typeof(string)),
            new DataColumn("Additional STD (in minutes) - STD", typeof(string)),
            new DataColumn("Additional STD (in minutes) - STD Onnet", typeof(string)),
            new DataColumn("Additional STD (in minutes) - STD Offnet", typeof(string)),
            new DataColumn("Additional STD (in minutes) - STD Mobile", typeof(string)),
            new DataColumn("Additional STD (in minutes) - STD Onnet Mobile", typeof(string)),
            new DataColumn("Additional STD (in minutes) - STD Offnet Mobile", typeof(string)),
            new DataColumn("Additional STD (in minutes) - Validity", typeof(string)),
            new DataColumn("Description of Additional STD in case they follow any other pattern", typeof(string)),
            new DataColumn("Additional Local & STD (in minutes)	- Local & STD (in minutes)", typeof(string)),
            new DataColumn("Additional Local & STD (in minutes)	- Local & STD Onnet (in minutes)", typeof(string)),
            new DataColumn("Additional Local & STD (in minutes)	- Local & STD Offnet (in minutes)", typeof(string)),
            new DataColumn("Additional Local & STD (in minutes)	- Local & STD Mobile (in minutes)", typeof(string)),
            new DataColumn("Additional Local & STD (in minutes)	- Local & STD Onnet Mobile (in minutes)", typeof(string)),
            new DataColumn("Additional Local & STD (in minutes)	- Local & STD Offnet Mobile (in minutes)", typeof(string)),
            new DataColumn("Additional Local & STD (in minutes)	- Validity (In Days)", typeof(string)),
            new DataColumn("Description of Additional Local & STD in case they follow any other pattern", typeof(string)),
            new DataColumn("Additional Local, STD & Roaming (in minutes)", typeof(string)),
            new DataColumn("Additional Local, STD & Roaming Mobile (in minutes)", typeof(string)),
            new DataColumn("Addn. Validity (In Days)", typeof(string)),
            new DataColumn("Description of Additional Local, STD & Roaming in case they follow any other pattern", typeof(string)),
            new DataColumn("Addn Roaming - Incoming & Outgoing (in minutes)", typeof(string)),
            new DataColumn("Addn Roaming - Incoming Free (in minutes)", typeof(string)),
            new DataColumn("Addn Roaming - Outgoing Free (in minutes)", typeof(string)),
            new DataColumn("Addn Roaming - Outgoing Local & STD Mobile Free (in minutes)", typeof(string)),
            new DataColumn("Addn Roaming - Outgoing Local Free (in minutes)", typeof(string)),
            new DataColumn("Addn Roaming - Outgoing STD Free (in minutes)", typeof(string)),
            new DataColumn("Addn Roaming - Validity (In Days)", typeof(string)),
            new DataColumn("Description of Additional Roaming in case they follow any other pattern", typeof(string)),
            new DataColumn("Addn SMS - Local & National", typeof(string)),
            new DataColumn("Addn SMS - Local & National Onnet", typeof(string)),
            new DataColumn("Addn SMS - Local & National Offnet", typeof(string)),
            new DataColumn("Addn SMS - Local", typeof(string)),
            new DataColumn("Addn SMS - Local Onnet", typeof(string)),
            new DataColumn("Addn SMS - Local Offnet", typeof(string)),
            new DataColumn("Addn SMS - National", typeof(string)),
            new DataColumn("Addn SMS - National Onnet", typeof(string)),
            new DataColumn("Addn SMS - National Offnet", typeof(string)),
            new DataColumn("Addn SMS - International", typeof(string)),
            new DataColumn("Addn SMS - Validity (In Days)", typeof(string)),
            new DataColumn("Description of Additional SMS in case they follow any other pattern", typeof(string)),
            new DataColumn("Summary of ISD freebies", typeof(string)),
            new DataColumn("Weblink for ISD freebies", typeof(string)),
            new DataColumn("Addn SMS Validity (In Days)", typeof(string)),
            new DataColumn("Description of Additional ISD in case they follow any other pattern", typeof(string)),
            new DataColumn("Additional Video Call - Local & STD Video", typeof(string)),
            new DataColumn("Additional Video Call - Local Video", typeof(string)),
            new DataColumn("Additional Video Call - Local Video Onnet", typeof(string)),
            new DataColumn("Additional Video Call - Local Video Offnet", typeof(string)),
            new DataColumn("Additional Video Call - STD Video", typeof(string)),
            new DataColumn("Additional Video Call - STD Video Onnet", typeof(string)),
            new DataColumn("Additional Video Call - STD Video Offnet", typeof(string)),
            new DataColumn("Additional Video Call - Validity (In Days)", typeof(string)),
            new DataColumn("Description of Additional Video Call in case they follow any other pattern", typeof(string)),
            new DataColumn(A253, typeof(string)),
            new DataColumn(A254, typeof(string)),
            new DataColumn(A255, typeof(string)),
            new DataColumn(A256, typeof(string)),
            new DataColumn("Total Data", typeof(string)),
            new DataColumn("Day/Night Data Capping", typeof(string)),
            new DataColumn("Weekly Data Capping", typeof(string)),
            new DataColumn("Monthly Data Capping", typeof(string)),
            new DataColumn("Carry Forward (Yes / No)", typeof(string)),
            new DataColumn("Data Validity (In Days)", typeof(string)),
            new DataColumn("FUP, if any (Yes/No)", typeof(string)),
            new DataColumn("Condition, if any", typeof(string)),
            new DataColumn("Description of Additional Data in case they follow any other pattern", typeof(string)),
            new DataColumn("Special benefits , if any", typeof(string)),
            new DataColumn("Other charges, if any", typeof(string)),
            new DataColumn("Remarks, if any", typeof(string)),
            new DataColumn("Terms and Conditions, if any", typeof(string)),
            new DataColumn("Mode of Activation / Recharge (Website / App only / paper / USSD / 3rd party Wallet etc)", typeof(string)),
            new DataColumn("Whether details of this service have been uploaded on the website", typeof(string)),
            new DataColumn("TSP website link of the Plan", typeof(string)),
            new DataColumn("Any other declarations", typeof(string)),
            new DataColumn("Condition for termination of Product if any", typeof(string)),
            new DataColumn("Activation Code If Any", typeof(string)),
            new DataColumn("Deactivation Code If Any", typeof(string)),
            new DataColumn("Available in (Rural / Urban/ Both / Any other)", typeof(string)),
            new DataColumn("Registration charges", typeof(string)),
            new DataColumn("Installation / activation charges", typeof(string)),
            new DataColumn("One time Security Deposit", typeof(string)),
            new DataColumn("Plan enrolment fee, if any", typeof(string)),
            new DataColumn("Other one time charges (Please specify)", typeof(string)),
            new DataColumn("Other - If yes please specify", typeof(string)),
            new DataColumn("Pay per use model (Usage charges)", typeof(string)),
            new DataColumn("Any other model (Details)", typeof(string)),
            new DataColumn("Benefits ", typeof(string)),
            new DataColumn("Security Deposit - Local", typeof(string)),
            new DataColumn("Security Deposit - Local + STD ", typeof(string)),
            new DataColumn("Security Deposit - Local + STD + ISD", typeof(string)),
            new DataColumn("Security Deposit - National Roaming", typeof(string)),
            new DataColumn("Security Deposit - International Roaming", typeof(string)),
            new DataColumn("Security Deposit - Any Other, Please Specify", typeof(string)),
            new DataColumn("Compulsory Fixed monthly charges including Rental / Minimum billing amount, if any ", typeof(string)),
            new DataColumn("If Exchange capacity wise FMC (Charges for exchange capacity <=999)", typeof(string)),
            new DataColumn("If Exchange capacity wise FMC(Charges for exchange capacity> 999 and <=29999)", typeof(string)),
            new DataColumn("If Exchange capacity wise FMC(Charges for exchange capacity >= 30000 and <=99999)", typeof(string)),
            new DataColumn("If Exchange capacity wise FMC(Charges for exchange capacity >= 100000)", typeof(string)),
            new DataColumn("If Flat FMC(Flat monthly charges)", typeof(string)),
            new DataColumn("Advance rental option for longer periods ", typeof(string)),
            new DataColumn("CLIP", typeof(string)),
            new DataColumn("Any other", typeof(string)),
            new DataColumn("If yes please specify", typeof(string)),
            new DataColumn("Free calls in MCUS per month", typeof(string)),
            new DataColumn("Free calls in MCUS per month for exchange capacity <=999", typeof(string)),
            new DataColumn("Free calls in MCUS per month for exchange capacity> 999 and <=29999", typeof(string)),
            new DataColumn("Free calls in MCUS per month for exchange capacity >= 30000 and <=99999", typeof(string)),
            new DataColumn("Free calls in MCUS per month for exchange capacity >= 100000", typeof(string)),
            
            new DataColumn("Free talk value in Rs. (per month)",typeof(string)) });




                // Data Rows //

                string idlist = ",";
                com = new MySqlCommand("select * from " + tablename + " where(rno>0) " + conditions + sortby, con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (!idlist.Contains(dr["uniqueid"].ToString().Trim()))
                    {
                        int arrpos = 0;
                        string[] myarr = new string[273];
                        string myrow = "";

                        for (int i = 4; i <= 5; i++)     // 'Tariff Product' till 'Tariff Summary' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34));
                            arrpos++;
                        }
                        for (int i = 7; i <= 8; i++)     // 'Operator' till 'Circle' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "").Replace("&amp;", "&").Replace(Convert.ToChar(126), Convert.ToChar(34));
                            arrpos++;
                        }
                        myarr[arrpos] = dr[10].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                        arrpos++;
                        myarr[arrpos] = dr[11].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                        arrpos++;
                        myarr[arrpos] = dr[14].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                        arrpos++;

                        if (dr["ttype"].ToString().Trim() == "PREPAID PLAN" || dr["ttype"].ToString().Trim().ToUpper() == "POSTPAID PLAN")
                        {
                            myarr[arrpos] = "";
                            arrpos++;
                        }
                        else
                        {
                            myarr[arrpos] = dr[15].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");   // 'Unique ID of Plan for which exclusively available'
                            arrpos++;
                        }

                        if (Convert.ToDateTime(dr[17].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                        {
                            myarr[arrpos] = Convert.ToDateTime(dr[17].ToString().Trim()).ToString("dd-MMM-yyyy");     // 'Date of Launch / Activation etc'
                            arrpos++;
                        }
                        else
                        {
                            myarr[arrpos] = "";
                            arrpos++;
                        }

                        myarr[arrpos] = dr[18].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");   // 'Regular/Promotional'
                        arrpos++;

                        if (Convert.ToDateTime(dr[19].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                        {
                            myarr[arrpos] = Convert.ToDateTime(dr[19].ToString().Trim()).ToString("dd-MMM-yyyy");   // 'Time duration of Promotional / limited period offer starts from'
                            arrpos++;
                        }
                        else
                        {
                            myarr[arrpos] = "";
                            arrpos++;
                        }
                        if (Convert.ToDateTime(dr[20].ToString().Trim()) > Convert.ToDateTime("2/1/2001"))
                        {
                            myarr[arrpos] = Convert.ToDateTime(dr[20].ToString().Trim()).ToString("dd-MMM-yyyy");      // 'Time duration of Promotional / limited period offer valid till'
                            arrpos++;
                        }
                        else
                        {
                            myarr[arrpos] = "";
                            arrpos++;
                        }

                        for (int i = 21; i <= 23; i++)     // 'Eligibility Conditions' till 'Monetory Value' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                            arrpos++;
                        }
                        myarr[arrpos] = dr[51].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");  // 'Validity'
                        arrpos++;


                        for (int i = 55; i <= 58; i++)     // 'Time for call rates' columns
                        {
                            if (dr[i].ToString().Trim() != "")
                            {
                                try
                                {
                                    myarr[arrpos] = Convert.ToDouble(dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "")).ToString("0.00").Replace(".", ":");
                                    arrpos++;
                                }
                                catch (Exception ex)
                                {
                                    myarr[arrpos] = " ";
                                    arrpos++;
                                }
                            }
                            else
                            {
                                myarr[arrpos] = " ";
                                arrpos++;
                            }
                        }
                        for (int i = 59; i <= 279; i++)     // 'Local All' till 'Misc - Deactivation Code' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                            arrpos++;
                        }
                        for (int i = 24; i <= 27; i++)     // 'Rural / Urban' till 'Installation / Activation Charges' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                            arrpos++;
                        }
                        for (int i = 30; i <= 31; i++)     // 'Plan Enrollment Fee' till 'othercharges' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                            arrpos++;
                        }
                        myarr[arrpos] = " ";  // 'OtherChargesDet' - field has been removed now, so show a blank column

                        for (int i = 52; i <= 54; i++)     // 'Pay Per Use' till 'modelbenefits' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                            arrpos++;
                        }
                        for (int i = 34; i <= 39; i++)     // 'Security Deposit' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                            arrpos++;
                        }
                        myarr[arrpos] = " ";   // 'Fixed Monthly Charges' - field has been removed, so show blank column
                        arrpos++;
                        for (int i = 40; i <= 43; i++)     // 'Fixed <=999' till 'Fixed100K' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                            arrpos++;
                        }
                        for (int i = 1; i <= 2; i++)     // 'FLAT FMC' and 'Advance Rental option' - fields have been removed, so show blank columns
                        {
                            myarr[arrpos] = " ";
                            arrpos++;
                        }
                        myarr[arrpos] = dr[44].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");   // 'Fixed - CLIP'
                        arrpos++;
                        myarr[arrpos] = " ";    // 'Fixed - Other Charges' - field have been removed, so show blank columns
                        arrpos++;
                        myarr[arrpos] = " ";       // 'Optional Fixed Monthly Charges - If yes please specify'
                        arrpos++;
                        for (int i = 45; i <= 50; i++)     // 'Fixed Calls MCU - Free Calls' till 'Free Talk Value in month' columns
                        {
                            myarr[arrpos] = dr[i].ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "");
                            arrpos++;
                        }



                        dt.Rows.Add(myarr[0], myarr[1], myarr[2], myarr[3], myarr[4], myarr[5], myarr[6], myarr[7], myarr[8], myarr[9], myarr[10], myarr[11], myarr[12], myarr[13], myarr[14], myarr[15], myarr[16], myarr[17], myarr[18], myarr[19], myarr[20], myarr[21], myarr[22], myarr[23], myarr[24], myarr[25], myarr[26], myarr[27], myarr[28], myarr[29], myarr[30], myarr[31], myarr[32], myarr[33], myarr[34], myarr[35], myarr[36], myarr[37], myarr[38], myarr[39], myarr[40], myarr[41], myarr[42], myarr[43], myarr[44], myarr[45], myarr[46], myarr[47], myarr[48], myarr[49], myarr[50], myarr[51], myarr[52], myarr[53], myarr[54], myarr[55], myarr[56], myarr[57], myarr[58], myarr[59], myarr[60], myarr[61], myarr[62], myarr[63], myarr[64], myarr[65], myarr[66], myarr[67], myarr[68], myarr[69], myarr[70], myarr[71], myarr[72], myarr[73], myarr[74], myarr[75], myarr[76], myarr[77], myarr[78], myarr[79], myarr[80], myarr[81], myarr[82], myarr[83], myarr[84], myarr[85], myarr[86], myarr[87], myarr[88], myarr[89], myarr[90], myarr[91], myarr[92], myarr[93], myarr[94], myarr[95], myarr[96], myarr[97], myarr[98], myarr[99], myarr[100], myarr[101], myarr[102], myarr[103], myarr[104], myarr[105], myarr[106], myarr[107], myarr[108], myarr[109], myarr[110], myarr[111], myarr[112], myarr[113], myarr[114], myarr[115], myarr[116], myarr[117], myarr[118], myarr[119], myarr[120], myarr[121], myarr[122], myarr[123], myarr[124], myarr[125], myarr[126], myarr[127], myarr[128], myarr[129], myarr[130], myarr[131], myarr[132], myarr[133], myarr[134], myarr[135], myarr[136], myarr[137], myarr[138], myarr[139], myarr[140], myarr[141], myarr[142], myarr[143], myarr[144], myarr[145], myarr[146], myarr[147], myarr[148], myarr[149], myarr[150], myarr[151], myarr[152], myarr[153], myarr[154], myarr[155], myarr[156], myarr[157], myarr[158], myarr[159], myarr[160], myarr[161], myarr[162], myarr[163], myarr[164], myarr[165], myarr[166], myarr[167], myarr[168], myarr[169], myarr[170], myarr[171], myarr[172], myarr[173], myarr[174], myarr[175], myarr[176], myarr[177], myarr[178], myarr[179], myarr[180], myarr[181], myarr[182], myarr[183], myarr[184], myarr[185], myarr[186], myarr[187], myarr[188], myarr[189], myarr[190], myarr[191], myarr[192], myarr[193], myarr[194], myarr[195], myarr[196], myarr[197], myarr[198], myarr[199], myarr[200], myarr[201], myarr[202], myarr[203], myarr[204], myarr[205], myarr[206], myarr[207], myarr[208], myarr[209], myarr[210], myarr[211], myarr[212], myarr[213], myarr[214], myarr[215], myarr[216], myarr[217], myarr[218], myarr[219], myarr[220], myarr[221], myarr[222], myarr[223], myarr[224], myarr[225], myarr[226], myarr[227], myarr[228], myarr[229], myarr[230], myarr[231], myarr[232], myarr[233], myarr[234], myarr[235], myarr[236], myarr[237], myarr[238], myarr[239], myarr[240], myarr[241], myarr[242], myarr[243], myarr[244], myarr[245], myarr[246], myarr[247], myarr[248], myarr[249], myarr[250], myarr[251], myarr[252], myarr[253], myarr[254], myarr[255], myarr[256], myarr[257], myarr[258], myarr[259], myarr[260], myarr[261], myarr[262], myarr[263], myarr[264], myarr[265], myarr[266], myarr[267], myarr[268], myarr[269], myarr[270], myarr[271], myarr[272]);

                        //dt.Rows.Add(1, "C Sharp corner", "United States");

                        idlist += dr["uniqueid"].ToString().Trim() + ",";
                    }
                }
                con.Close();


                //Exporting to Excel
                string folderPath = Server.MapPath("MyPath");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                //Codes for the Closed XML
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Tariffs");
                    string fname = "Tariff" + "_" + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".xlsx";
                    wb.SaveAs(folderPath + "/" + fname);
                    //string myName = Server.UrlEncode("Tests" + "_" + DateTime.Now.ToShortDateString() +          ".xlsx");

                    // Open new window for results page //
                  //  ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('MyPath/" + fname + "');", true);

                }



            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
        catch (Exception ex)
        {

        }
    }






    protected void ButtonClearFilters_Click(object sender, EventArgs e)
    {
        try
        {
            ChkDataTech1.Checked = false;
            ChkDataTech2.Checked = false;
            ChkDataTech3.Checked = false;

            ChkValidityMore.Checked = false;

            ChkUnlim_Local.Checked = false;
            ChkUnlim_STD.Checked = false;
            ChkUnlim_Roaming.Checked = false;

            ChkDailyDataCap1.Checked = false;
            ChkDailyDataCap2.Checked = false;
            ChkDailyDataCap3.Checked = false;

            ChkFullTalktime.Checked = false;

            ChkISDPack.Checked = false;
            ChkISDRoaming.Checked = false;
            ChkNatRoaming.Checked = false;

            ChkAdvance.Checked = false;

            for (int i = 0; i < CheckAdvLocal.Items.Count; i++)
            {
                CheckAdvLocal.Items[i].Selected = false;
            }
            for (int i = 0; i < CheckAdvSMS.Items.Count; i++)
            {
                CheckAdvSMS.Items[i].Selected = false;
            }
            for (int i = 0; i < CheckAdvSTD.Items.Count; i++)
            {
                CheckAdvSTD.Items[i].Selected = false;
            }
            for (int i = 0; i < CheckAdvRoaming.Items.Count; i++)
            {
                CheckAdvRoaming.Items[i].Selected = false;
            }

            Text2a.Text = "0";
            Text2b.Text = "10000";
            Text3a.Text = "0";
            Text3b.Text = "500";
            Text4a.Text = "0";
            Text4b.Text = "365";




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

                string idlist = ",";
                com = new MySqlCommand("select * from " + tablename + " where(rno>0) " + conditions + sortby, con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (!idlist.Contains(dr["uniqueid"].ToString().Trim()))
                    {
                        sb.Append("<TariffProduct>");
                        sb.Append("<ProductType>" + dr["ttype"].ToString().Trim() + "</ProductType>");
                        sb.Append("<TariffSummary>" + dr["tariffdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;") + "</TariffSummary>");
                        sb.Append("<Operator>" + dr["oper"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;") + "</Operator>");
                        sb.Append("<Circle>" + dr["circ"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;") + "</Circle>");
                        sb.Append("<Service>" + dr["service"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</Service>");
                        sb.Append("<Category>" + dr["categ"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</Category>");
                        sb.Append("<ProductName>" + dr["planname"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</ProductName>");
                        sb.Append("<ApplicableToPlan>" + dr["planid"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;") + "</ApplicableToPlan>");
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
                        sb.Append("<RegularORPromo>" + dr["regprom"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</RegularORPromo>");
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
                        sb.Append("<Eligibility>" + dr["offerconditions"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;") + "</Eligibility>");
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

                        sb.Append("<LocalCUP>" + dr["local_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</LocalCUP>");
                        sb.Append("<LocalOtherDescription>" + dr["local_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</LocalOtherDescription>");


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

                        sb.Append("<STDCUP>" + dr["std_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</STDCUP>");
                        sb.Append("<STDOtherDescription>" + dr["std_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</STDOtherDescription>");


                        // SMS Charges //
                        sb.Append("<SMSCharges>");
                        sb.Append("<AllLocal>" + Math.Round(Convert.ToDouble(dr["sms_all_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AllLocal>");
                        sb.Append("<AllNational>" + Math.Round(Convert.ToDouble(dr["sms_all_national"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AllNational>");
                        sb.Append("<LocalOnNet>" + Math.Round(Convert.ToDouble(dr["sms_local_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOnNet>");
                        sb.Append("<LocalOffNet>" + Math.Round(Convert.ToDouble(dr["sms_local_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalOffNet>");
                        sb.Append("<NationalOnNet>" + Math.Round(Convert.ToDouble(dr["sms_nat_on"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</NationalOnNet>");
                        sb.Append("<NationalOffNet>" + Math.Round(Convert.ToDouble(dr["sms_nat_off"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</NationalOffNet>");
                        sb.Append("<International>" + Math.Round(Convert.ToDouble(dr["sms_int"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</International>");
                        sb.Append("<Terms>" + dr["sms_terms"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</Terms>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["sms_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</SMSCharges>");

                        sb.Append("<SMSCUP>" + dr["sms_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</SMSCUP>");
                        sb.Append("<SMSOtherDescription>" + dr["sms_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</SMSOtherDescription>");


                        // ISD Call Charges //
                        sb.Append("<ISDCallCharges>");
                        sb.Append("<Countries>" + dr["isd_countries"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</Countries>");
                        sb.Append("<PulseRateInSeconds>" + Math.Round(Convert.ToDouble(dr["isd_pulserate"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</PulseRateInSeconds>");
                        sb.Append("<ISDRateMobile>" + Math.Round(Convert.ToDouble(dr["isd_mobile"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDRateMobile>");
                        sb.Append("<ISDRateLandline>" + Math.Round(Convert.ToDouble(dr["isd_landline"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDRateLandline>");
                        sb.Append("<ISDRateVideoCalls>" + Math.Round(Convert.ToDouble(dr["isd_video"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ISDRateVideoCalls>");
                        sb.Append("<WebLink>" + dr["isd_weblink"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</WebLink>");
                        sb.Append("<Conditions>" + dr["isd_conditions"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Conditions>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["isd_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("</ISDCallCharges>");

                        sb.Append("<ISDCUP>" + dr["isd_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</ISDCUP>");
                        sb.Append("<ISDOtherDescription>" + dr["isd_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</ISDOtherDescription>");


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
                        sb.Append("<WebLink>" + dr["roam_weblink"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</WebLink>");
                        sb.Append("<RoamingCUP>" + dr["roam_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</RoamingCUP>");
                        sb.Append("<RoamingOtherDescription>" + dr["roam_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</RoamingOtherDescription>");
                        sb.Append("</RoamingCallCharges>");


                        // SMS Charges While Roaming //
                        sb.Append("<RoamingSMSCharges>");
                        sb.Append("<Local>" + Math.Round(Convert.ToDouble(dr["roam_sms_local"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Local>");
                        sb.Append("<National>" + Math.Round(Convert.ToDouble(dr["roam_sms_nat"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</National>");
                        sb.Append("<International>" + Math.Round(Convert.ToDouble(dr["roam_sms_int"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</International>");
                        sb.Append("<AnyOther>" + Math.Round(Convert.ToDouble(dr["roam_sms_other"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</AnyOther>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["roam_sms_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<CUP>" + dr["roam_sms_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</CUP>");
                        sb.Append("<OtherDescription>" + dr["roam_sms_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</RoamingSMSCharges>");


                        // Charges while International Roaming //
                        sb.Append("<InternationalRoaming>");
                        sb.Append("<Countries>" + dr["introam_countries"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Countries>");
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
                        sb.Append("<FreeDataUnit>" + dr["introam_unit_free"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", "") + "</FreeDataUnit>");
                        sb.Append("<FreeDataUsage>" + Math.Round(Convert.ToDouble(dr["introam_data_free"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeDataUsage>");
                        sb.Append("<FreeSMS>" + Math.Round(Convert.ToDouble(dr["introam_sms_free"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</FreeSMS>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["introam_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<CUP>" + dr["introam_cup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</CUP>");
                        sb.Append("<OtherDescription>" + dr["introam_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</InternationalRoaming>");


                        // Data Charges //
                        sb.Append("<DataCharges>");
                        sb.Append("<Home>" + dr["datacharges_home"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Home>");
                        sb.Append("<Roaming>" + dr["datacharges_roam"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Roaming>");
                        sb.Append("<Conditions>" + dr["datacharges_conditions"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Conditions>");
                        sb.Append("</DataCharges>");


                        // Monthly Rental - International Roaming //
                        sb.Append("<ChargesRoamingInternational>");
                        sb.Append("<MonthlyRental>" + Math.Round(Convert.ToDouble(dr["datacharges_rental"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</MonthlyRental>");
                        sb.Append("<WebLink>" + dr["datacharges_weblink"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</WebLink>");
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
                        sb.Append("<OtherDescription>" + dr["add_local_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
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
                        sb.Append("<OtherDescription>" + dr["add_std_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
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
                        sb.Append("<OtherDescription>" + dr["add_localstd_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddLocalSTDInMinutes>");


                        // Additional Local, STD & Roaming //
                        sb.Append("<AddLocalSTDRoamingInMinutes>");
                        sb.Append("<LocalSTDMobile>" + Math.Round(Convert.ToDouble(dr["add_LSR"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDMobile>");
                        sb.Append("<LocalSTDRoamingMobile>" + Math.Round(Convert.ToDouble(dr["add_LSR_mob"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</LocalSTDRoamingMobile>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_LSR_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_LSR_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
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
                        sb.Append("<OtherDescription>" + dr["add_roam_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
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
                        sb.Append("<OtherDescription>" + dr["add_sms_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddSMS>");


                        // Additional ISD //
                        sb.Append("<AddISD>");
                        sb.Append("<Summary>" + dr["add_isd_summ"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Summary>");
                        sb.Append("<Weblink>" + dr["add_isd_link"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Weblink>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["add_isd_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<OtherDescription>" + dr["add_isd_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
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
                        sb.Append("<OtherDescription>" + dr["add_video_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddVideoCall>");



                        // Additional Data //
                        sb.Append("<AddData>");
                        if (RadProvider.SelectedItem.Text.Trim() == "TSP")
                        {
                            sb.Append("<Unit>" + dr["adddata_unit"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Unit>");
                            sb.Append("<Total2GData>" + Math.Round(Convert.ToDouble(dr["adddata_total2g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Total2GData>");
                            sb.Append("<Total3GData>" + Math.Round(Convert.ToDouble(dr["adddata_total3g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Total3GData>");
                            sb.Append("<Total4GData>" + Math.Round(Convert.ToDouble(dr["adddata_total4g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Total4GData>");
                        }
                        else
                        {
                            sb.Append("<SpeedUnit>" + dr["adddata_unit"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</SpeedUnit>");
                            sb.Append("<HigherSpeedLimit>" + Math.Round(Convert.ToDouble(dr["adddata_total2g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</HigherSpeedLimit>");
                            sb.Append("<SpeedDataLimit>" + Math.Round(Convert.ToDouble(dr["adddata_total3g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</SpeedDataLimit>");
                            sb.Append("<SpeedBeyondLimit>" + Math.Round(Convert.ToDouble(dr["adddata_total4g"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</SpeedBeyondLimit>");
                        }
                        sb.Append("<TotalData>" + Math.Round(Convert.ToDouble(dr["adddata_ISP"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</TotalData>");
                        sb.Append("<DayNightDataCapping>" + Math.Round(Convert.ToDouble(dr["adddata_daycap"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</DayNightDataCapping>");
                        sb.Append("<WeeklyDataCapping>" + Math.Round(Convert.ToDouble(dr["adddata_weekcap"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</WeeklyDataCapping>");
                        sb.Append("<MonthlyDataCapping>" + Math.Round(Convert.ToDouble(dr["adddata_monthcap"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</MonthlyDataCapping>");
                        sb.Append("<CarryForward>" + dr["adddata_carry"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</CarryForward>");
                        sb.Append("<Validity>" + Math.Round(Convert.ToDouble(dr["adddata_validity"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</Validity>");
                        sb.Append("<FUP>" + dr["adddata_fup"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</FUP>");
                        sb.Append("<Conditions>" + dr["adddata_conditions"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Conditions>");
                        sb.Append("<OtherDescription>" + dr["adddata_otherdet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDescription>");
                        sb.Append("</AddData>");


                        // Miscellaneous //
                        sb.Append("<Miscellaneous>");
                        sb.Append("<SpecialBenefits>" + dr["misc_ben"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</SpecialBenefits>");
                        sb.Append("<OtherCharges>" + dr["misc_other"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherCharges>");
                        sb.Append("<Remarks>" + dr["misc_remarks"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Remarks>");
                        sb.Append("<Terms>" + dr["misc_terms"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Terms>");
                        sb.Append("<ActivationMode>" + dr["misc_mode"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</ActivationMode>");
                        sb.Append("<DetailsUploaded>" + dr["misc_uploaded"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</DetailsUploaded>");
                        sb.Append("<WebLink>" + dr["misc_link"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</WebLink>");
                        sb.Append("<OtherDeclarations>" + dr["misc_dec"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherDeclarations>");
                        sb.Append("<TerminationTerms>" + dr["misc_termination"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</TerminationTerms>");
                        sb.Append("<ActivationCode>" + dr["misc_actcode"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</ActivationCode>");
                        sb.Append("<DeActivationCode>" + dr["misc_deactcode"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</DeActivationCode>");
                        sb.Append("</Miscellaneous>");


                        // One Time Charges //
                        sb.Append("<OneTimeCharges>");
                        sb.Append("<RuralUrban>" + dr["ruralurban"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</RuralUrban>");
                        sb.Append("<RegistrationCharges>" + Math.Round(Convert.ToDouble(dr["regcharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</RegistrationCharges>");
                        sb.Append("<ActivationCharges>" + Math.Round(Convert.ToDouble(dr["actcharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</ActivationCharges>");
                        sb.Append("<SecurityDeposit>" + Math.Round(Convert.ToDouble(dr["ISP_deposit"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</SecurityDeposit>");
                        sb.Append("<EnrollmentFee>" + Math.Round(Convert.ToDouble(dr["planfee"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</EnrollmentFee>");
                        sb.Append("<OtherCharges>" + Math.Round(Convert.ToDouble(dr["othercharges"].ToString().Trim()), 2).ToString().Trim().Replace("-2", "Unlimited").Replace("-1", "") + "</OtherCharges>");
                        //sb.Append("<OtherChargesDetails>" + dr["otherchargedet"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</OtherChargesDetails>");
                        sb.Append("</OneTimeCharges>");


                        // Details of Service //
                        sb.Append("<DetailsOfService>");
                        sb.Append("<PayPerUseModel>" + dr["payperuse"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</PayPerUseModel>");
                        sb.Append("<AnyOtherModel>" + dr["othermodel"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</AnyOtherModel>");
                        sb.Append("<Benefits>" + dr["modelbenefits"].ToString().Trim().Replace(Convert.ToChar(126), Convert.ToChar(34)).Replace("<", "&lt;").Replace(">", "&gt;").Replace("-1", " ") + "</Benefits>");
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

                        idlist += dr["uniqueid"].ToString().Trim() + ",";

                    }

                }
                con.Close();

                sb.Append("</TariffProducts>");

                sb.Replace("&amp;", "&");     // there might be some '&amp;' and some '&' in the same tariff. This replacement and re-replacement will set it uniformly
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
            //Response.Write(ex.ToString());
        }
    }








    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SetCurrentSliderValues(null, null); // code to retain the slider current values on postback

            divPopUpBg.Attributes.Clear();
            divSelection.InnerHtml = "";
            Button1.Visible = false;
            ButtonCancel.Visible = false;
            divPopShadow.Visible = false;
            divPopUp.Visible = false;
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
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
            */

            Text2a.Text = amount2a.Text.Trim();
            Text2b.Text = amount2b.Text.Trim();
            Text3a.Text = amount3a.Text.Trim();
            Text3b.Text = amount3b.Text.Trim();
            Text4a.Text = amount4a.Text.Trim();
            Text4b.Text = amount4b.Text.Trim();


            Page.ClientScript.RegisterStartupScript(this.GetType(), "funAdvance", "funAdvance()", true);


            setManual(null, null);

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
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
            }

            // If the user has changed the values in the slider text boxes, below code will re-implement them in the boxes if they have been changed by slider automatically - CODE ENDS HERE //

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
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





    public MemoryStream GetStream(XLWorkbook excelWorkbook)
    {
        MemoryStream fs = new MemoryStream();
        excelWorkbook.SaveAs(fs);
        fs.Position = 0;
        return fs;
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