using System;
using System.Globalization;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;     // #####  FOR MYSQL
using System.Data.OleDb;
using System.Text;

public partial class reptariffcount : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    int rno, pos, pos2, rowcntr, zno, colsInSheet, tmpcntr;
    double flag, currflag; // flag is for total count, while currflag is to check whether current product in loop gave an error
    string tmp = "", blogo, filename, myopt, fileno, img2;
    string strerror, uniqueid, oper, circ, service, actiontotake, regprom, tablename, tariffsumm;
    string[] arrColsInSheet;


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

        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("logout.aspx");
            }

            if (!IsPostBack)
            {
                RadOType.Text = "Both";
                TextDate.Text = "01-Jul-2018";
                TextDate2.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
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
            DateTime dt1, dt2;
            try
            {
                dt1 = Convert.ToDateTime(TextDate.Text.Trim());
            }
            catch (Exception ex) {
                dt1 = Convert.ToDateTime("01-July-2018");
            }
            try
            {
                dt2 = Convert.ToDateTime(TextDate2.Text.Trim());
            }
            catch (Exception ex)
            {
                dt2 = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            }

            int a1 = 0;
            int activecount = 0;
            string datatype = "";
            string otype = RadOType.SelectedItem.Text.Trim();

            string operconditions = "";
            if (RadOType.SelectedItem.Text == "TSP")
            {
                operconditions += " (upper(oper)='AIRCEL' or upper(oper)='AIRTEL' or upper(oper)='BSNL' or upper(oper)='IDEA' or upper(oper)='JIO' or upper(oper)='MTNL' or upper(oper)='QUADRANT (CONNECT)' or upper(oper)='TATA TELE' or upper(oper)='TELENOR' or upper(oper)='VODAFONE' or upper(oper)='VODAFONE IDEA' or upper(oper)='SURFTELECOM' or upper(oper)='AEROVOYCE')";
            }
            if (RadOType.SelectedItem.Text == "ISP")
            {
                operconditions += " (upper(oper)!='AIRCEL' and upper(oper)!='AIRTEL' and upper(oper)!='BSNL' and upper(oper)!='IDEA' and upper(oper)!='JIO' and upper(oper)!='MTNL' and upper(oper)!='PLINTRON' and upper(oper)!='QUADRANT (CONNECT)' and upper(oper)!='TATA TELE' and upper(oper)!='TELENOR' and upper(oper)!='VODAFONE' and upper(oper)!='VODAFONE IDEA' and upper(oper)!='SURFTELECOM' and upper(oper)!='AEROVOYCE')";
            }
            if (RadOType.SelectedItem.Text == "Both")
            {
                // no condition here, as all TSP/ISP are to be included
                operconditions += " (rno>0)";
            }

            if(RadLaunchDate.SelectedItem.Text.Trim()=="After 01-July-2018")
            {
                datatype = "New Data";
            }
            else
            {
                datatype = "Old Data";
            }

            string mystr = "<center><font face=arial><br />" + datatype + " - Count of Active Tariffs With Launch Date Between " + dt1.ToString("dd-MMM-yyyy") + " and " + dt2.ToString("dd-MMM-yyyy") + "<br /><br />";
            mystr = mystr + "<table width=50% cellspacing=1 cellpadding=5><tr><td class=tablehead align=center>Operator</td><td class=tablehead align=center>Records</td></tr>";
            com1 = new MySqlCommand("select distinct(oper) from TRAI_operators where(oper!='Aircel' and oper!='Telenor' and oper!='Plintron') and " + operconditions + " order by oper", con1);
            con1.Open();
            dr1 = com1.ExecuteReader();
            while (dr1.Read())
            {
                int operdataexists = 0;
                string css = "tablecell3b";
                if (a1 % 2 == 0)
                {
                    css = "tablecell3c";
                }
                    
                if (RadLaunchDate.SelectedItem.Text.Trim() == "Before 01-July-2018")
                {
                    //com = new MySqlCommand("select count(*) from TRAI_tariffs where (upper(oper)='" + dr1[0].ToString().Trim().ToUpper() + "') and (recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "') and (actiondate<'2018-07-01') and (checkedby!='admin') and (upper(circ) in (select distinct(upper(circ)) from TRAI_circles))", con);
                    com = new MySqlCommand("select count(*) from TRAI_tariffs where (upper(oper)='" + dr1[0].ToString().Trim().ToUpper() + "') and (recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "') and (actiondate<'2018-07-01') and (checkedby!='admin') and (upper(circ) in (select distinct(upper(circ)) from TRAI_circles)) and (upper(circ)!='ISP TESTCIRCLE') and (upper(circ)!='TEST CIRCLE') and (upper(circ)!='TESTING CIRCLE')", con);
                }
                else
                {
                    //com = new MySqlCommand("select count(*) from TRAI_tariffs where (upper(oper)='" + dr1[0].ToString().Trim().ToUpper() + "') and (recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "') and (actiondate>='2018-07-01') and (checkedby!='admin') and (upper(circ) in (select distinct(upper(circ)) from TRAI_circles))", con);
                    com = new MySqlCommand("select count(*) from TRAI_tariffs where (upper(oper)='" + dr1[0].ToString().Trim().ToUpper() + "') and (recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "') and (actiondate>='2018-07-01') and (checkedby!='admin') and (upper(circ) in (select distinct(upper(circ)) from TRAI_circles)) and (upper(circ)!='ISP TESTCIRCLE') and (upper(circ)!='TEST CIRCLE') and (upper(circ)!='TESTING CIRCLE')", con);
                }
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    operdataexists++;
                    mystr = mystr + "<tr><td class=" + css + " align=center>" + dr1[0].ToString().Trim() + "</td><td class=" + css + " align=center>" + dr[0].ToString().Trim() + "</td></tr>";
                    activecount += Convert.ToInt32(dr[0].ToString().Trim());
                    a1++;
                }
                con.Close();
                if(operdataexists==0)
                {
                    mystr = mystr + "<tr><td class=" + css + " align=center>" + dr1[0].ToString().Trim() + "</td><td class=" + css + " align=center>0</td></tr>";
                }
            }
            con1.Close();
            mystr = mystr + "<tr><td class=tablehead></td><td class=tablehead align=center>Total : " + activecount.ToString() + "</td></tr>";
            mystr = mystr + "</table><hr size=0>";
            /*
            int a2 = 0;
            mystr = mystr + "<br />Circle-wise Count of Active Tariffs With Launch Date Between " + dt1.ToString("dd-MMM-yyyy") + " and " + dt2.ToString("dd-MMM-yyyy") + "<br /><br />";
            mystr = mystr + "<table width=50% cellspacing=1 cellpadding=5><tr><td class=tablehead align=center>TSP</td><td class=tablehead align=center>Circle</td><td class=tablehead align=center>Records</td></tr>";
            com2 = new MySqlCommand("select distinct(oper) from TRAI_tariffs order by oper", con2);
            con2.Open();
            dr2 = com2.ExecuteReader();
            while (dr2.Read())
            {
                com1 = new MySqlCommand("select distinct(circ) from TRAI_tariffs order by circ", con1);
                con1.Open();
                dr1 = com1.ExecuteReader();
                while (dr1.Read())
                {
                    if (RadLaunchDate.SelectedItem.Text.Trim() == "Before 01-July-2018")
                    {
                        com = new MySqlCommand("select count(*) from TRAI_tariffs where (recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "') and (actiondate<'2018-07-01') and (checkedby!='admin') and (oper='" + dr2[0].ToString().Trim() + "') and (circ='" + dr1[0].ToString().Trim() + "')", con);
                    }
                    else
                    {
                        com = new MySqlCommand("select count(*) from TRAI_tariffs where (recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "') and (actiondate>='2018-07-01') and (checkedby!='admin') and (oper='" + dr2[0].ToString().Trim() + "') and (circ='" + dr1[0].ToString().Trim() + "')", con);
                    }
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        string css = "tablecell3b";
                        if (a2 % 2 == 0)
                        {
                            css = "tablecell3c";
                        }
                        mystr = mystr + "<tr><td class=" + css + " align=center>" + dr2[0].ToString().Trim() + "</td><td class=" + css + " align=center>" + dr1[0].ToString().Trim() + "</td><td class=" + css + " align=center>" + dr[0].ToString().Trim() + "</td></tr>";
                        a2++;
                    }
                    con.Close();
                }
                con1.Close();
            }
            con2.Close();
            mystr = mystr + "</table>";
            */

            int a2 = 0;
            int circount = 23;
            int opercount=0;

            mystr = mystr + "<br />" + datatype + " - Circle-wise Count of Active Tariffs With Launch Date Between " + dt1.ToString("dd-MMM-yyyy") + " and " + dt2.ToString("dd-MMM-yyyy") + "<br /><br />";
            mystr = mystr + "<table width=80% cellspacing=1 cellpadding=5>";
            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablehead align=center>S.No.</td><td class=tablehead align=center>LSA</td><td class=tablehead align=center>Date for online Submission </td>";
            com2 = new MySqlCommand("select distinct(oper) from TRAI_tariffs where(oper!='Plintron') and " + operconditions + " order by oper", con2);
            con2.Open();
            dr2 = com2.ExecuteReader();
            while(dr2.Read())
            {
                mystr = mystr + "<td class=tablehead align=center>" + dr2[0].ToString().Trim() + "</td>";
                opercount++;
            }
            con2.Close();
            mystr = mystr + "<td class=tablehead align=center>Total</td>";
            mystr = mystr + "</tr>";

            double[] opertot = new double[opercount];
            for(int i=0; i<opercount; i++)
            {
                opertot[i] = 0;
            }

            for (int i = 1; i <= circount;i++)
            {
                int counttariffs = 0;
                string circ = "";
                DateTime subdate = Convert.ToDateTime("1/1/2001");

                if (i == 1)
                {
                    circ = "All India";
                    subdate = Convert.ToDateTime("7/6/2018");
                }
                if (i == 2)
                {
                    circ = "Delhi";
                    subdate = Convert.ToDateTime("7/6/2018");
                }
                if (i == 3)
                {
                    circ = "Gujarat";
                    subdate = Convert.ToDateTime("7/6/2018");
                }
                if (i == 4)
                {
                    circ = "Kolkata";
                    subdate = Convert.ToDateTime("07/10/2018");
                }
                if (i == 5)
                {
                    circ = "Odisha";
                    subdate = Convert.ToDateTime("07/10/2018");
                }
                if (i == 6)
                {
                    circ = "Mumbai";
                    subdate = Convert.ToDateTime("07/13/2018");
                }
                if (i == 7)
                {
                    circ = "North East";
                    subdate = Convert.ToDateTime("07/13/2018");
                }
                if (i == 8)
                {
                    circ = "Andhra Pradesh";
                    subdate = Convert.ToDateTime("07/17/2018");
                }
                if (i == 9)
                {
                    circ = "Jammu and Kashmir";
                    subdate = Convert.ToDateTime("07/17/2018");
                }
                if (i == 10)
                {
                    circ = "Karnataka";
                    subdate = Convert.ToDateTime("07/20/2018");
                }
                if (i == 11)
                {
                    circ = "Himachal Pradesh";
                    subdate = Convert.ToDateTime("07/20/2018");
                }
                if (i == 12)
                {
                    circ = "Maharashtra";
                    subdate = Convert.ToDateTime("07/24/2018");
                }
                if (i == 13)
                {
                    circ = "Bihar";
                    subdate = Convert.ToDateTime("07/24/2018");
                }
                if (i == 14)
                {
                    circ = "Chennai and Tamil Nadu";
                    subdate = Convert.ToDateTime("07/27/2018");
                }
                if (i == 15)
                {
                    circ = "Assam";
                    subdate = Convert.ToDateTime("07/27/2018");
                }
                if (i == 16)
                {
                    circ = "Haryana";
                    subdate = Convert.ToDateTime("07/30/2018");
                }
                if (i == 17)
                {
                    circ = "Kerala";
                    subdate = Convert.ToDateTime("07/30/2018");
                }
                if (i == 18)
                {
                    circ = "Madhya Pradesh";
                    subdate = Convert.ToDateTime("08/01/2018");
                }
                if (i == 19)
                {
                    circ = "Punjab";
                    subdate = Convert.ToDateTime("08/01/2018");
                }
                if (i == 20)
                {
                    circ = "UP East";
                    subdate = Convert.ToDateTime("08/03/2018");
                }
                if (i == 21)
                {
                    circ = "UP West";
                    subdate = Convert.ToDateTime("08/03/2018");
                }
                if (i == 22)
                {
                    circ = "Rajasthan";
                    subdate = Convert.ToDateTime("08/06/2018");
                }
                if (i == 23)
                {
                    circ = "West Bengal";
                    subdate = Convert.ToDateTime("08/06/2018");
                }

                string css = "tablecell3b";
                if (a2 % 2 == 0)
                {
                    css = "tablecell3c";
                }
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=" + css + " align=center>" + i.ToString().Trim() + "</td>";
                mystr = mystr + "<td class=" + css + " align=center>" + circ + "</td>";
                if (circ == "All India")
                {
                    mystr = mystr + "<td class=" + css + " align=center>-</td>";
                }
                else
                {
                    mystr = mystr + "<td class=" + css + " align=center>" + subdate.ToString("dd-MMM-yyyy") + "</td>";
                }

                int cntr = 0;
                com2 = new MySqlCommand("select distinct(oper) from TRAI_tariffs where(oper!='Plintron') and " + operconditions + " order by oper", con2);
                con2.Open();
                dr2 = com2.ExecuteReader();
                while (dr2.Read())
                {
                    string oper=dr2[0].ToString().Trim();

                    // check whether current TSP services available in current LSA
                    int NAflag = 0;   
                    if(oper.ToUpper()=="BSNL")
                    {
                        if (circ.ToUpper() == "DELHI" || circ.ToUpper() == "MUMBAI")
                        {
                            NAflag = 1;
                        }
                    }
                    if (oper.ToUpper() == "MTNL")
                    {
                        if (circ.ToUpper() != "DELHI" && circ.ToUpper() != "MUMBAI")
                        {
                            NAflag = 1;
                        }
                    }
                    if (oper.ToUpper() == "QUADRANT (CONNECT)")
                    {
                        if (circ.ToUpper() != "PUNJAB")
                        {
                            NAflag = 1;
                        }
                    }

                    // check whether current TSP services available in current LSA - CODE ENDS HERE


                    if(NAflag==1)
                    {
                        mystr = mystr + "<td class=" + css + " align=center>NA</td>";
                        cntr++;
                    }
                    else
                    {
                        if (RadLaunchDate.SelectedItem.Text.Trim() == "Before 01-July-2018")
                        {
                            com = new MySqlCommand("select count(*) from TRAI_tariffs where (recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "') and (actiondate<'2018-07-01') and (checkedby!='admin') and (upper(oper)='" + oper.ToUpper() + "') and (upper(circ)='" + circ.ToUpper() + "')", con);
                        }
                        else
                        {
                            com = new MySqlCommand("select count(*) from TRAI_tariffs where (recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.AddDays(1).ToString("yyyy-MM-dd") + "') and (actiondate>='2018-07-01') and (checkedby!='admin') and (upper(oper)='" + oper.ToUpper() + "') and (upper(circ)='" + circ.ToUpper() + "')", con);
                        }
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        int reccount = 0;
                        try
                        {
                            reccount = Convert.ToInt32(dr[0].ToString().Trim());
                        }
                        catch(Exception ex)
                        {
                            Response.Write(ex.ToString());
                        }
                        con.Close();
                        mystr = mystr + "<td class=" + css + " align=center>" + reccount.ToString() + "</td>";
                        opertot[cntr] += reccount;
                        cntr++;
                        counttariffs += reccount;
                    }
                }
                con2.Close();
                mystr = mystr + "<td class=" + css + " align=center><b>" + counttariffs.ToString() + "</b></td>";
                mystr = mystr + "</tr>";
                a2++;
                
            }

            double gtot = 0;
            mystr = mystr + "<tr>";
            mystr = mystr + "<td class=tablehead colspan=3 align=right>Total</td>";
            for (int i = 0; i < opercount;i++)
            {
                mystr = mystr + "<td class=tablehead align=center><b>" + opertot[i].ToString() + "</b></td>";
                gtot += opertot[i];
            }
            mystr = mystr + "<td class=tablehead align=center><b>" + gtot.ToString() + "</b></td>";
            mystr = mystr + "</tr>";
            mystr = mystr + "</table><br /><br />";

                


            divresults.InnerHtml = mystr;
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
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






}
