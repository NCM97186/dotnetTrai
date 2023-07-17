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

public partial class zzExcelToDb2 : System.Web.UI.Page
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
            /*
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("logout.aspx");
            }
            */

            
            string uname="";
            
            tablename="TRAI_tariffrecords";

            filename = Request["t1"].ToString().Trim();


            com = new MySqlCommand("delete from TRAI_temperrors where(recdate<'" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "');", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();


            
            //OleDbConnection oledbcn=new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("data.xls").ToString() + "; Extended Properties=Excel 8.0;");    
            //OleDbConnection oledbcn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("test.xlsx").ToString() + "; Extended Properties=Excel 12.0 Xml;");    
            OleDbConnection oledbcn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("Excel/" + filename).ToString() + ";Mode=ReadWrite;Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"");
            //OleDbConnection oledbcn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("test.xlsx").ToString() + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");    
            //OleDbConnection oledbcn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("subsexcel/" + filename).ToString() + "; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1;\"");

            int numofsheets=15;    // Total Number of worksheets in the excel format

            for (int k = 1; k <= numofsheets; k++)
            {
                flag = 0;
                string SheetName = "";
                int headerrows = 0;    // no. of rows for headings, to be ignored while reading data

                if (k == 1)
                {
                    SheetName = "Prepaid_Plan Voucher";
                    headerrows = 2;
                    colsInSheet = 184;   // no. of columns in this worksheet.
                }
                if (k == 2)
                {
                    SheetName = "Prepaid_STV";
                    headerrows = 2;
                    colsInSheet = 242;   // no. of columns in this worksheet.
                }
                if (k == 3)
                {
                    SheetName = "Prepaid_Combo";
                    headerrows = 2;
                    colsInSheet = 243;   // no. of columns in this worksheet.
                }
                if (k == 4)
                {
                    SheetName = "Prepaid_Top Up";
                    headerrows = 2;
                    colsInSheet = 24;   // no. of columns in this worksheet.
                }
                if (k == 5)
                {
                    SheetName = "Prepaid_VAS";
                    headerrows = 1;
                    colsInSheet = 32;   // no. of columns in this worksheet.
                }
                if (k == 6)
                {
                    SheetName = "Promo Offer";
                    headerrows = 1;
                    colsInSheet = 29;   // no. of columns in this worksheet.
                }
                if (k == 7)
                {
                    SheetName = "SUK";
                    headerrows = 2;
                    colsInSheet = 185;   // no. of columns in this worksheet.
                }
                if (k == 8)
                {
                    SheetName = "Postpaid- Plan";
                    headerrows = 2;
                    colsInSheet = 196;   // no. of columns in this worksheet.
                }
                if (k == 9)
                {
                    SheetName = "Postpaid- Add On Pack";
                    headerrows = 2;
                    colsInSheet = 222;   // no. of columns in this worksheet.
                }
                if (k == 10)
                {
                    SheetName = "Postpaid_VAS";
                    headerrows = 1;
                    colsInSheet = 32;   // no. of columns in this worksheet.
                }
                if (k == 11)
                {
                    SheetName = "Fixed Line -Tariff";
                    headerrows = 2;
                    colsInSheet = 175;   // no. of columns in this worksheet.
                }
                if (k == 12)
                {
                    SheetName = "Fixed Line Add-On Pack";
                    headerrows = 2;
                    colsInSheet = 184;   // no. of columns in this worksheet.
                }
                if (k == 13)
                {
                    SheetName = "ISP";
                    headerrows = 2;
                    colsInSheet = 52;   // no. of columns in this worksheet.
                }
                if (k == 14)
                {
                    SheetName = "Gen_ISD_Tariff";
                    headerrows = 2;
                    colsInSheet = 21;   // no. of columns in this worksheet.
                }
                if (k == 15)
                {
                    SheetName = "International_Roaming";
                    headerrows = 1;
                    colsInSheet = 39;   // no. of columns in this worksheet.
                }


                
                
                    arrColsInSheet = new string[colsInSheet];

                    try
                    {
                        DataTable dt = new DataTable();
                        oledbcn.Open();
                        try
                        {
                            OleDbCommand cmd = new OleDbCommand(@"SELECT * FROM [" + SheetName + "$]", oledbcn);
                            OleDbDataAdapter oledbda = new OleDbDataAdapter();
                            oledbda.SelectCommand = cmd;
                            oledbda.Fill(dt);
                        }
                        catch (Exception ex) { }
                        oledbcn.Close();

                        //if we have a large amount of data we have to use for loop to insert the data     
                        StringBuilder Str = new StringBuilder();
                        flag = 0;

                        TextInfo oInfo = CultureInfo.CurrentCulture.TextInfo;

                        rowcntr = 0;



                        foreach (DataRow drw in dt.Rows)  // row-wise entries start here
                        {
                            currflag = 0;
                            strerror = "";
                            tariffsumm = "";
                            uniqueid = "";
                            oper = "";
                            circ = "";
                            service = "";
                            actiontotake = "";
                            regprom = "";
                            tmpcntr = 0;



                            if (rowcntr >= headerrows)  //  ignore header rows
                            {
                                if (drw[1].ToString().Trim() != "" && drw[2].ToString().Trim() != "" && drw[3].ToString().Trim() != "")   // ignore blank rows
                                {

                                    getMaxRno("rno", tablename);
                                    string rowqry = "insert into " + tablename + " values('" + zno + "','" + filename + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + uname + "','" + SheetName + "',";





                                    // #####  'Prepaid_Plan Voucher' Sheet #######///
                                    if (SheetName == "Prepaid_Plan Voucher")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                        */
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) {
                                            strerror = strerror + "Invalid Report Date | ";
                                            flag++;
                                            currflag++;
                                        }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) {
                                            strerror = strerror + "Invalid Launch / Revision / Correction / Withdrawal Date | ";
                                            flag++;
                                            currflag++;
                                        }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'ISP_plancost' till 'fixed_plan' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";    // fixed_other
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'payperuse' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }

                                        // 'time for call rates' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_on_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_off_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_on_validity
                                        //cellno++;

                                        // 'Local Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_otherdet
                                        cellno++;


                                        // 'STD Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_off_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_otherdet
                                        cellno++;


                                        // 'SMS Charges (in Paisa/SMS)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_terms
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'',";    // isd_countries
                                        //cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // isd_weblink
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_conditions
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";  // isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_otherdet
                                        //cellno++;

                                        // 'Call charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_otherdet
                                        cellno++;

                                        // 'SMS charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'',";  // introam_countries
                                        //cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";  // introam_unit_free
                                        //cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // introam_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // introam_otherdet
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_home
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_roam
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_conditions
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";  // datacharges_rental
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_weblink
                                        //cellno++;

                                        // 'Duration for Additional Benefits' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Additional Local - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_local_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_local_otherdet
                                        cellno++;

                                        // 'Additional STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_std_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_std_otherdet
                                        cellno++;

                                        // 'Additional Local & STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_localstd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_localstd_otherdet
                                        cellno++;

                                        // 'Additional Local, STD & Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_LSR_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_LSR_otherdet
                                        cellno++;

                                        // 'Additional Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_roam_otherdet
                                        cellno++;

                                        // 'Additional SMS' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 10; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_summ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_link
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // add_isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_otherdet
                                        cellno++;

                                        // 'Additional Video Call' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_video_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_video_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;

                                        // 'Additional Data 2G/3G/4G' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'-1',";    // adddata_ISP
                                        //cellno++;

                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }
                                        
                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'',";    // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // misc_deactcode
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Prepaid_Plan Voucher' Sheet - CODE ENDS HERE #######///






                                    // #####  'Prepaid_STV' Sheet #######///
                                    if (SheetName == "Prepaid_STV")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'ISP_plancost' till 'fixed_plan' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";    // fixed_other
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'payperuse' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }

                                        // 'time for call rates' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }


                                        // 'Local Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - On-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - Off-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - Fixed On-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges- Mobile - Fixed Off-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_otherdet
                                        cellno++;


                                        // 'STD Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - On-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - Off-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - Fixed On-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - Fixed Off-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_otherdet
                                        cellno++;

                                        // 'SMS Charges (in Paisa/SMS)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_terms
                                        cellno++;
                                        double smsvalidity = -1;
                                        try
                                        {
                                            smsvalidity = Convert.ToDouble(arrColsInSheet[cellno]);   // sms_validity
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + smsvalidity + "',";
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_countries
                                        cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_conditions
                                        cellno++;
                                        double isdvalidity = -1;
                                        try
                                        {
                                            isdvalidity = Convert.ToDouble(arrColsInSheet[cellno]);   // isd_validity
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + isdvalidity + "',";
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_otherdet
                                        cellno++;

                                        // 'Call charges while roaming' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 8; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_otherdet
                                        cellno++;

                                        // 'SMS charges while roaming' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 5; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_countries
                                        cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_unit_free
                                        cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_home
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_roam
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_conditions
                                        cellno++;
                                        double datarental = -1;
                                        try
                                        {
                                            datarental = Convert.ToDouble(arrColsInSheet[cellno]);   // datacharges_rental
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + datarental + "',";
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_weblink
                                        cellno++;

                                        // 'Duration for Additional Benefits' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Additional Local - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_local_otherdet
                                        cellno++;

                                        // 'Additional STD - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_std_otherdet
                                        cellno++;

                                        // 'Additional Local & STD - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_localstd_otherdet
                                        cellno++;

                                        // 'Additional Local, STD & Roaming - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_LSR_otherdet
                                        cellno++;

                                        // 'Additional Roaming - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_roam_otherdet
                                        cellno++;

                                        // 'Additional SMS' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 11; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_summ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_link
                                        cellno++;
                                        double addisdvalidity = -1;
                                        try
                                        {
                                            addisdvalidity = Convert.ToDouble(arrColsInSheet[cellno]);   // add_isd_validity
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + addisdvalidity + "',";
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_otherdet
                                        cellno++;

                                        // 'Additional Video Call' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 8; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_video_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;

                                        // 'Additional Data 2G/3G/4G' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'-1',";    // adddata_ISP
                                        //cellno++;

                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // misc_actcode
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";  // misc_deactcode   
                                        cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;


                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Prepaid_STV' Sheet - CODE ENDS HERE #######///






                                    // #####  'Prepaid_Combo' Sheet #######///
                                    if (SheetName == "Prepaid_Combo")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        double monval = 0;
                                        try
                                        {
                                            monval = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + monval + "',";   // monval
                                        cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'ISP_plancost' till 'fixed_plan' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";    // fixed_other
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'payperuse' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }

                                        // 'time for call rates' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - On-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - Off-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - Fixed On-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges- Mobile - Fixed Off-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_otherdet
                                        cellno++;

                                        // 'STD Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }


                                        // 'STD Call Charges- Mobile - On-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - Off-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - Fixed On-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - Fixed Off-Net' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_otherdet
                                        cellno++;

                                        // 'SMS Charges (in Paisa/SMS)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        double smsvalidity = -1;
                                        try
                                        {
                                            smsvalidity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_terms
                                        cellno++;
                                        rowqry = rowqry + "'" + smsvalidity + "',";   // sms_validity
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_countries
                                        cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        double isdvalidity = -1;
                                        try
                                        {
                                            isdvalidity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_conditions
                                        cellno++;
                                        rowqry = rowqry + "'" + isdvalidity + "',";  // isd_validity
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_otherdet
                                        cellno++;

                                        // 'Call charges while roaming' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 8; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_otherdet
                                        cellno++;

                                        // 'SMS charges while roaming' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 5; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_countries
                                        cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_unit_free
                                        cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_home
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_roam
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_conditions
                                        cellno++;
                                        double datarental = -1;
                                        try
                                        {
                                            datarental = Convert.ToDouble(arrColsInSheet[cellno]);   // datacharges_rental
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + datarental + "',";
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_weblink
                                        cellno++;

                                        // 'Duration for Additional Benefits' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Additional Local - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_local_otherdet
                                        cellno++;

                                        // 'Additional STD - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_std_otherdet
                                        cellno++;

                                        // 'Additional Local & STD - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_localstd_otherdet
                                        cellno++;

                                        // 'Additional Local, STD & Roaming - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_LSR_otherdet
                                        cellno++;

                                        // 'Additional Roaming - (in minutes)' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_roam_otherdet
                                        cellno++;

                                        // 'Additional SMS' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 11; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_summ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_link
                                        cellno++;
                                        double addisdvalidity = -1;
                                        try
                                        {
                                            addisdvalidity = Convert.ToDouble(arrColsInSheet[cellno]);   // add_isd_validity
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + addisdvalidity + "',";
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_otherdet
                                        cellno++;

                                        // 'Additional Video Call' including validity fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 8; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_video_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;

                                        // 'Additional Data 2G/3G/4G' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'-1',";    // adddata_ISP
                                        //cellno++;

                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // misc_actcode
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";  // misc_deactcode   
                                        cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;


                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Prepaid_Combo' Sheet - CODE ENDS HERE #######///






                                    // #####  'Prepaid_Top Up' Sheet #######///
                                    if (SheetName == "Prepaid_Top Up")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        double monval = 0;
                                        try
                                        {
                                            monval = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + monval + "',";   // monval
                                        cellno++;

                                        for (int i = 1; i <= 257; i++)
                                        {
                                            rowqry = rowqry + "'-1',";       // These fields do not apply in this sheet.
                                        }


                                        // 'misc_terms' till 'misc_dec' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'',";   // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";  // misc_deactcode   
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;


                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Prepaid_Top Up' Sheet - CODE ENDS HERE #######///







                                    // #####  'Prepaid_VAS' Sheet #######///
                                    if (SheetName == "Prepaid_VAS")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'ISP_plancost' till 'fixed_plan' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";    // fixed_other
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'payperuse' till 'modelbenefits'
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }


                                        for (int i = 1; i <= 214; i++)
                                        {
                                            rowqry = rowqry + "'-1',";       // These fields do not apply to this sheet.
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // misc_actcode
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";  // misc_deactcode   
                                        cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;


                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Prepaid_VAS' Sheet - CODE ENDS HERE #######///








                                    // #####  'Promo Offer' Sheet #######///
                                    if (SheetName == "Promo Offer")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = "PROMOTIONAL";

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + regprom + "',";   // regprom
                                        //cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'ISP_plancost' till 'fixed_plan' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";    // fixed_other
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'payperuse' and 'othermodel' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // modelbenefits
                                        cellno++;

                                        for (int i = 1; i <= 214; i++)
                                        {
                                            rowqry = rowqry + "'-1',";       // These fields do not apply to this sheet.
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // misc_actcode
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";  // misc_deactcode   
                                        cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;


                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Promo Offer' Sheet - CODE ENDS HERE #######///








                                    // #####  'SUK' Sheet #######///
                                    if (SheetName == "SUK")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        double monval = 0;
                                        try
                                        {
                                            monval = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + monval + "',";   // monval
                                        cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'ISP_plancost' till 'fixed_plan' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";    // fixed_other
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'payperuse' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }

                                        // 'time for call rates' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_on_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_off_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_on_validity
                                        //cellno++;

                                        // 'Local Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_otherdet
                                        cellno++;

                                        // 'STD Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_off_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_otherdet
                                        cellno++;

                                        // 'SMS Charges (in Paisa/SMS)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_terms
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'',";    // isd_countries
                                        //cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // isd_weblink
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_conditions
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";  // isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_otherdet
                                        //cellno++;

                                        // 'Call charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_otherdet
                                        cellno++;

                                        // 'SMS charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'',";  // introam_countries
                                        //cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";  // introam_unit_free
                                        //cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // introam_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // introam_otherdet
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_home
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_roam
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_conditions
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";  // datacharges_rental
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_weblink
                                        //cellno++;

                                        // 'Duration for Additional Benefits' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Additional Local - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_local_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_local_otherdet
                                        cellno++;

                                        // 'Additional STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_std_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_std_otherdet
                                        cellno++;

                                        // 'Additional Local & STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_localstd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_localstd_otherdet
                                        cellno++;

                                        // 'Additional Local, STD & Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_LSR_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_LSR_otherdet
                                        cellno++;

                                        // 'Additional Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_roam_otherdet
                                        cellno++;

                                        // 'Additional SMS' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 10; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_summ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_link
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // add_isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_otherdet
                                        cellno++;

                                        // 'Additional Video Call' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_video_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_video_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;

                                        // 'Additional Data 2G/3G/4G' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'-1',";    // adddata_ISP
                                        //cellno++;

                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'',";    // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // misc_deactcode
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'SUK' Sheet - CODE ENDS HERE #######///








                                    // #####  'Postpaid- Plan' Sheet #######///
                                    if (SheetName == "Postpaid- Plan")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'-1',";   // mrp
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'ISP_plancost' till 'ISP_advance' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        // 'security_local' till 'security_other' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'fixed_monthly' till 'fixed_other' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 5; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'payperuse' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }

                                        // 'time for call rates' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_on_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_off_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_on_validity
                                        //cellno++;

                                        // 'Local Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_otherdet
                                        cellno++;

                                        // 'STD Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_off_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_otherdet
                                        cellno++;

                                        // 'SMS Charges (in Paisa/SMS)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_terms
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'',";    // isd_countries
                                        //cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // isd_weblink
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_conditions
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";  // isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // isd_otherdet
                                        //cellno++;

                                        // 'Call charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_otherdet
                                        cellno++;

                                        // 'SMS charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'',";  // introam_countries
                                        //cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";  // introam_unit_free
                                        //cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // introam_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // introam_otherdet
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_home
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_roam
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_conditions
                                        //cellno++;
                                        double datacharges_rental = 0;
                                        try
                                        {
                                            datacharges_rental = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + datacharges_rental + "',";   // datacharges_rental
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_weblink
                                        cellno++;

                                        // 'Duration for Additional Benefits' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Additional Local - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_local_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_local_otherdet
                                        cellno++;

                                        // 'Additional STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_std_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_std_otherdet
                                        cellno++;

                                        // 'Additional Local & STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_localstd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_localstd_otherdet
                                        cellno++;

                                        // 'Additional Local, STD & Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_LSR_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_LSR_otherdet
                                        cellno++;

                                        // 'Additional Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_roam_otherdet
                                        cellno++;

                                        // 'Additional SMS' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 10; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_summ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_link
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // add_isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_otherdet
                                        cellno++;

                                        // 'Additional Video Call' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_video_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_video_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;

                                        // 'Additional Data 2G/3G/4G' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'-1',";    // adddata_ISP
                                        //cellno++;

                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'',";    // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // misc_deactcode
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Postpaid- Plan' Sheet - CODE ENDS HERE #######///








                                    // #####  'Postpaid- Add On Pack' Sheet #######///
                                    if (SheetName == "Postpaid- Add On Pack")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'ISP_plancost' till 'fixed_plan' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";    // fixed_other
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'payperuse' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }

                                        // 'time for call rates' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_on_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_off_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_on_validity
                                        //cellno++;

                                        // 'Local Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_otherdet
                                        cellno++;

                                        // 'STD Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_off_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_otherdet
                                        cellno++;

                                        // 'SMS Charges (in Paisa/SMS)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_terms
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_countries
                                        cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_conditions
                                        cellno++;
                                        rowqry = rowqry + "'-1',";    // isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_otherdet
                                        cellno++;


                                        // 'Call charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_otherdet
                                        cellno++;

                                        // 'SMS charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // roam_sms_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_countries
                                        cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_unit_free
                                        cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_home
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_roam
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_conditions
                                        cellno++;
                                        double datarental = -1;
                                        try
                                        {
                                            datarental = Convert.ToDouble(arrColsInSheet[cellno]);   // datacharges_rental
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + datarental + "',";
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_weblink
                                        cellno++;

                                        // 'Duration for Additional Benefits' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Additional Local - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_local_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_local_otherdet
                                        cellno++;

                                        // 'Additional STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_std_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_std_otherdet
                                        cellno++;

                                        // 'Additional Local & STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_localstd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_localstd_otherdet
                                        cellno++;

                                        // 'Additional Local, STD & Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_LSR_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_LSR_otherdet
                                        cellno++;

                                        // 'Additional Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_roam_otherdet
                                        cellno++;

                                        // 'Additional SMS' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 10; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_summ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_link
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // add_isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_otherdet
                                        cellno++;

                                        // 'Additional Video Call' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_video_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_video_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;

                                        // 'Additional Data 2G/3G/4G' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'-1',";    // adddata_ISP
                                        //cellno++;

                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // misc_actcode
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";  // misc_deactcode   
                                        cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Postpaid- Add On Pack' Sheet - CODE ENDS HERE #######///









                                    // #####  'Postpaid_VAS' Sheet #######///
                                    if (SheetName == "Postpaid_VAS")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        regprom = arrColsInSheet[10].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'SSA' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'categ' field not present for this sheet, so blank entry
                                        //cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // 'segment' field not present for this sheet, so blank entry
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // ruralurban
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // otherchargesdet
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'ISP_plancost' till 'fixed_plan' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";    // fixed_other
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 13; i++)  // fields from 'fixed_999' till 'fixed_freetalk' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'payperuse' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        for (int i = 1; i <= 214; i++)
                                        {
                                            rowqry = rowqry + "'-1',";       // These fields do not apply to this sheet.
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // misc_actcode
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";  // misc_deactcode  
                                        cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;


                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Postpaid_VAS' Sheet - CODE ENDS HERE #######///









                                    // #####  'Fixed Line -Tariff' Sheet #######///
                                    if (SheetName == "Fixed Line -Tariff")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[7].ToUpper();
                                        regprom = arrColsInSheet[12].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // SSA
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // categ
                                        cellno++;
                                        /*
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // segment
                                         */ 
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // mrp
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // ruralurban
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'regcharges' till 'actcharges'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'ISP_deposit' till 'ISP_wifi'
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'planfee' till 'othercharges'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // otherchargesdet
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'ISP_plancost' till 'ISP_advance' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'security_local' till 'security_LSI'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";     // security_nat_roam
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";     // security_int_roam
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'security_other' till 'fixed_monthly'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'fixed_adv' till 'fixed_plan'
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double fixed_opt_other = 0;
                                        try
                                        {
                                            fixed_opt_other = Convert.ToDouble(arrColsInSheet[cellno + 7]);   // $$$$ fixed_opt_other COMES AFTER 8 COLUMNS, BUT IN DATABASE IS TO BE INSERTED FIRST
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + fixed_opt_other + "',";   // fixed_opt_other
                                        //cellno++;

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'fixed_999' till 'fixed_clip' 
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        cellno++;
                                        string fixed_opt_specify = arrColsInSheet[cellno];   // fixed_opt_specify - TO BE INSERTED IN 'payperuse'   $$$$
                                        cellno++;

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)  // fields from 'fixed_MCU' till 'fixed_freetalk' 
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;

                                        rowqry = rowqry + "'" + fixed_opt_specify + "',";

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'othermodel' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }

                                        // 'time for call rates' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_on_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_off_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_on_validity
                                        //cellno++;

                                        // 'Local Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_otherdet
                                        cellno++;

                                        // 'STD Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_off_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_otherdet
                                        cellno++;

                                        // 'SMS Charges (in Paisa/SMS)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_terms
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'',";   // isd_countries
                                        //cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";   // isd_weblink
                                        //cellno++;
                                        rowqry = rowqry + "'',";   // isd_conditions
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";    // isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";   // isd_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";   // isd_otherdet
                                        //cellno++;


                                        // 'Call charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_weblink
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_otherdet
                                        //cellno++;

                                        // 'SMS charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_sms_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_sms_otherdet
                                        //cellno++;

                                        rowqry = rowqry + "'',";    // introam_countries
                                        //cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // introam_unit_free
                                        //cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // introam_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // introam_otherdet
                                        //cellno++;

                                        rowqry = rowqry + "'',";    // datacharges_home
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_roam
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_conditions
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";  // datacharges_rental
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_weblink
                                        //cellno++;

                                        // 'Duration for Additional Benefits' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Additional Local - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_local_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_local_otherdet
                                        cellno++;

                                        // 'Additional STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_std_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_std_otherdet
                                        cellno++;

                                        // 'Additional Local & STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_localstd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_localstd_otherdet
                                        cellno++;

                                        // 'Additional Local, STD & Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_LSR_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // add_LSR_otherdet
                                        //cellno++;

                                        // 'Additional Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            rowqry = rowqry + "'-1',";  // add_localstd_validity
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // add_roam_otherdet
                                        //cellno++;

                                        // 'Additional SMS' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 10; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_summ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_link
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // add_isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_otherdet
                                        cellno++;

                                        // 'Additional Video Call' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_video_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // add_video_otherdet
                                        //cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;
                                        
                                        // 'Additional Data 2G/3G/4G' fields //
                                        for (int i = 1; i <= 3; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        double adddata_ISP = -1;
                                        try
                                        {
                                            adddata_ISP = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_ISP + "',";    // adddata_ISP
                                        cellno++;

                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'',";   // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";  // misc_deactcode   ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Fixed Line -Tariff' Sheet - CODE ENDS HERE #######///








                                    // #####  'Fixed Line Add-On Pack' Sheet #######///
                                    if (SheetName == "Fixed Line Add-On Pack")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[7].ToUpper();
                                        regprom = arrColsInSheet[12].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // SSA
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // categ
                                        cellno++;
                                        /*
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // segment
                                         */ 
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // mrp
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // ruralurban
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'regcharges' till 'actcharges'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'ISP_deposit' till 'ISP_wifi'
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'planfee' till 'othercharges'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // otherchargesdet
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'ISP_plancost' till 'ISP_advance' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'security_local' till 'security_LSI'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";     // security_nat_roam
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";     // security_int_roam
                                        //cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'security_other' till 'fixed_monthly'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'fixed_adv' till 'fixed_plan'
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        double fixed_opt_other = 0;
                                        try
                                        {
                                            fixed_opt_other = Convert.ToDouble(arrColsInSheet[cellno + 7]);   // $$$$ fixed_opt_other COMES AFTER 8 COLUMNS, BUT IN DATABASE IS TO BE INSERTED FIRST
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + fixed_opt_other + "',";   // fixed_opt_other
                                        //cellno++;

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'fixed_999' till 'fixed_clip' 
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        cellno++;
                                        string fixed_opt_specify = arrColsInSheet[cellno];   // fixed_opt_specify - TO BE INSERTED IN 'payperuse'   $$$$
                                        cellno++;

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)  // fields from 'fixed_MCU' till 'fixed_freetalk' 
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;

                                        rowqry = rowqry + "'" + fixed_opt_specify + "',";

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)  // fields from 'othermodel' till 'modelbenefits' do not apply here
                                        {
                                            rowqry = rowqry + "'',";
                                            //cellno++;
                                        }

                                        // 'time for call rates' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Local Call Charges - Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_on_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_off_validity
                                        //cellno++;

                                        // 'Local Call Charges - Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_on_validity
                                        //cellno++;

                                        // 'Local Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // local_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // local_otherdet
                                        cellno++;

                                        // 'STD Call Charges - All' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'STD Call Charges- Mobile - On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_off_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed On-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_on_validity
                                        //cellno++;

                                        // 'STD Call Charges- Mobile - Fixed Off-Net' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // std_fix_off_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // std_otherdet
                                        cellno++;

                                        // 'SMS Charges (in Paisa/SMS)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_terms
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // sms_otherdet
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_countries
                                        cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_conditions
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_otherdet
                                        cellno++;


                                        // 'Call charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_weblink
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_otherdet
                                        //cellno++;

                                        // 'SMS charges while roaming' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // roam_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_sms_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // roam_sms_otherdet
                                        //cellno++;

                                        rowqry = rowqry + "'',";    // introam_countries
                                        //cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // introam_unit_free
                                        //cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'',";    // introam_cup
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // introam_otherdet
                                        //cellno++;

                                        rowqry = rowqry + "'',";    // datacharges_home
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_roam
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_conditions
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";  // datacharges_rental
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // datacharges_weblink
                                        //cellno++;

                                        // 'Duration for Additional Benefits' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        // 'Additional Local - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_local_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_local_otherdet
                                        cellno++;

                                        // 'Additional STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_std_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_std_otherdet
                                        cellno++;

                                        // 'Additional Local & STD - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_localstd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_localstd_otherdet
                                        cellno++;

                                        // 'Additional Local, STD & Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 2; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_LSR_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // add_LSR_otherdet
                                        //cellno++;

                                        // 'Additional Roaming - (in minutes)' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 6; i++)
                                        {
                                            rowqry = rowqry + "'-1',";  // add_localstd_validity
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_roam_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // add_roam_otherdet
                                        //cellno++;

                                        // 'Additional SMS' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 10; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_sms_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_sms_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_summ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_link
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // add_isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // add_isd_otherdet
                                        cellno++;

                                        // 'Additional Video Call' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }
                                        rowqry = rowqry + "'-1',";  // add_video_validity
                                        //cellno++;
                                        rowqry = rowqry + "'',";    // add_video_otherdet
                                        //cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;


                                        // 'Additional Data 2G/3G/4G' fields //
                                        for (int i = 1; i <= 3; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        double adddata_ISP = -1;
                                        try
                                        {
                                            adddata_ISP = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_ISP + "',";    // adddata_ISP
                                        cellno++;

                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'',";   // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";  // misc_deactcode   ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Fixed Line Add-On Pack' Sheet - CODE ENDS HERE #######///











                                    // #####  'ISP' Sheet #######///
                                    if (SheetName == "ISP")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        //service = arrColsInSheet[4];
                                        service = "";
                                        actiontotake = arrColsInSheet[7].ToUpper();
                                        regprom = arrColsInSheet[12].ToUpper();

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // SSA
                                        cellno++;
                                        rowqry = rowqry + "'',";   // service
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // categ
                                        cellno++;
                                        /*
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // segment
                                         */ 
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planname
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // planid
                                        cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // regprom
                                        cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offerfrom = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            offertill = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        rowqry = rowqry + "'-1',";   // mrp
                                        //cellno++;
                                        rowqry = rowqry + "'-1',";   // monval
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // ruralurban
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 7; i++)  // fields from 'regcharges' till 'othercharges'
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        tmpcntr = cellno;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // otherchargesdet
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)  // fields from 'ISP_plancost' till 'ISP_advance' do not apply here
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 24; i++)  // fields from 'security_local' till 'fixed_freetalk'
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        double validity = 0;
                                        try
                                        {
                                            validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + validity + "',";   // validity
                                        cellno++;

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 202; i++)  // fields from 'payperuse' till 'add_video_std_off' do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        double opt_fix_monthly = 0;     //  $$$$$$$  'add_video_validity' field is being used to store 'optional fixed monthly charges - other'
                                        try
                                        {
                                            opt_fix_monthly = Convert.ToDouble(arrColsInSheet[cellno].Replace(":", "."));
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + opt_fix_monthly + "',";
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   //  $$$$$$$  'add_video_otherdet' field is being used to store 'optional fixed monthly charges - other specify'
                                        cellno++;

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_unit
                                        cellno++;

                                        // 'Additional Data 2G/3G/4G' fields //
                                        for (int i = 1; i <= 3; i++)
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        double adddata_ISP = -1;
                                        try
                                        {
                                            adddata_ISP = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_ISP + "',";    // adddata_ISP
                                        cellno++;
                                        
                                        // 'adddata_daycap' till 'adddata_monthcap' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // adddata_carry
                                        cellno++;
                                        double adddata_validity = -1;
                                        try
                                        {
                                            adddata_validity = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + adddata_validity + "',";
                                        cellno++;
                                        // 'adddata_fup' till 'adddata_otherdet' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        // 'Miscellaneous' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)
                                        {
                                            rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";
                                            cellno++;
                                        }

                                        rowqry = rowqry + "'',";   // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";  // misc_deactcode   
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'ISP' Sheet - CODE ENDS HERE #######///











                                    // #####  'Gen_ISD_Tariff' Sheet #######///
                                    if (SheetName == "Gen_ISD_Tariff")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        //regprom = arrColsInSheet[12].ToUpper();
                                        regprom = "REGULAR";

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // SSA
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // categ
                                        cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // segment
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'',";   // planname
                                        //cellno++;
                                        rowqry = rowqry + "'',";   // planid
                                        //cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + regprom + "',";   // regprom
                                        //cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        //cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 125; i++)  // fields from 'mrp' till 'sms_otherdet' - do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_countries
                                        cellno++;
                                        // 'isd_pulserate' to 'isd_video' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 4; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_weblink
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_conditions
                                        cellno++;
                                        rowqry = rowqry + "'-1',";  // isd_validity
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // isd_otherdet
                                        cellno++;

                                        for (int i = tmpcntr; i < tmpcntr + 130; i++)  // fields from 'roam_call_pulse' till 'misc_dec' - do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        rowqry = rowqry + "'',";   // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";  // misc_deactcode
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'Gen_ISD_Tariff' Sheet - CODE ENDS HERE #######///









                                    // #####  'International_Roaming' Sheet #######///
                                    if (SheetName == "International_Roaming")
                                    {

                                        for (int i = 0; i < colsInSheet; i++)
                                        {
                                            arrColsInSheet[i] = drw[i].ToString().Trim().Replace("'", "`").Replace("’", "`").Replace("&", "&amp;").Replace(Convert.ToChar(34), Convert.ToChar(126));
                                        }

                                        tariffsumm = arrColsInSheet[0];
                                        uniqueid = arrColsInSheet[1];
                                        oper = arrColsInSheet[2];
                                        circ = arrColsInSheet[3];
                                        service = arrColsInSheet[4];
                                        actiontotake = arrColsInSheet[5].ToUpper();
                                        //regprom = arrColsInSheet[12].ToUpper();
                                        regprom = "REGULAR";

                                        // Compile insert query from colsInSheet //

                                        int cellno = 0;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // tariffdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // uniqueid
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // oper
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // circ
                                        cellno++;
                                        rowqry = rowqry + "'',";   // SSA
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // service
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // categ
                                        cellno++;
                                        /*
                                        rowqry = rowqry + "'',";   // segment
                                         */ 
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // actiontotake
                                        cellno++;
                                        rowqry = rowqry + "'',";   // 'revisionid' field not present for this sheet, so blank entry
                                        //cellno++;
                                        rowqry = rowqry + "'',";   // planname
                                        //cellno++;
                                        rowqry = rowqry + "'',";   // planid
                                        //cellno++;
                                        DateTime reportdate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            reportdate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + reportdate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // reportdate
                                        cellno++;
                                        DateTime actiondate = Convert.ToDateTime("1/1/2011");
                                        try
                                        {
                                            actiondate = Convert.ToDateTime(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + actiondate.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // actiondate
                                        cellno++;
                                        rowqry = rowqry + "'" + regprom + "',";   // regprom
                                        //cellno++;
                                        DateTime offerfrom = Convert.ToDateTime("1/1/2011");
                                        rowqry = rowqry + "'" + offerfrom.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offerfrom
                                        //cellno++;
                                        DateTime offertill = Convert.ToDateTime("1/1/2011");
                                        rowqry = rowqry + "'" + offertill.ToString("yyyy-MM-dd HH:mm:ss") + "',";   // offertill
                                        //cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // offerconditions
                                        cellno++;
                                        double mrp = 0;
                                        try
                                        {
                                            mrp = Convert.ToDouble(arrColsInSheet[cellno]);
                                        }
                                        catch (Exception ex) { }
                                        rowqry = rowqry + "'" + mrp + "',";   // mrp
                                        cellno++;
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 142; i++)  // fields from 'monval' till 'roam_validity' - do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";   // 'roam_weblink'
                                        cellno++;

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 9; i++)  // fields from 'roam_cup' till 'roam_sms_otherdet' - do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_countries
                                        cellno++;
                                        // 'introam_in_pulse' till 'introam_min_out_free' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 15; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_unit_free
                                        cellno++;
                                        // 'introam_data_free' till 'introam_validity' fields //
                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 3; i++)
                                        {
                                            double myval = -1;
                                            try
                                            {
                                                myval = Convert.ToDouble(arrColsInSheet[cellno]);
                                            }
                                            catch (Exception ex) { }
                                            rowqry = rowqry + "'" + myval + "',";
                                            cellno++;
                                        }
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_cup
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // introam_otherdet
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_home
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_roam
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "',";    // datacharges_conditions
                                        cellno++;

                                        tmpcntr = cellno;
                                        for (int i = tmpcntr; i < tmpcntr + 87; i++)  // fields from 'datacharges_rental' till 'misc_dec' - do not apply here
                                        {
                                            rowqry = rowqry + "'-1',";
                                            //cellno++;
                                        }

                                        rowqry = rowqry + "'',";   // misc_actcode
                                        //cellno++;
                                        rowqry = rowqry + "'',";  // misc_deactcode
                                        //cellno++;

                                        // 'Delay in Submission' fields //
                                        if (arrColsInSheet[cellno].ToUpper() == "YES")
                                        {
                                            rowqry = rowqry + "'Yes',";
                                        }
                                        else
                                        {
                                            rowqry = rowqry + "'No',";
                                        }
                                        cellno++;
                                        rowqry = rowqry + "'" + arrColsInSheet[cellno] + "'";   // ### LAST COLUMN VALUE SHOULD BE WITHOUT COMMA
                                        cellno++;



                                        rowqry = rowqry + ")";


                                        // Compile insert query from colsInSheet - CODE ENDS HERE //


                                    }
                                    // #####  'International_Roaming' Sheet - CODE ENDS HERE #######///










                                    // validate referential integrity parameters //

                                    /*
                                    if(tariffsumm=="")
                                    {
                                        strerror = strerror + "Invalid Tariff Summary | ";
                                        flag++;
                                        currflag++;
                                    }
                                    */ 
                                    if (uniqueid == "")
                                    {
                                        strerror = strerror + "Invalid Record ID No. | ";
                                        flag++;
                                        currflag++;
                                    }

                                    if (SheetName != "ISP")
                                    {
                                        com1 = new MySqlCommand("select count(*) from TRAI_operators where(oper='" + oper + "')", con1);
                                        con1.Open();
                                        dr1 = com1.ExecuteReader();
                                        dr1.Read();
                                        try
                                        {
                                            if (Convert.ToInt32(dr1[0].ToString().Trim()) == 0)
                                            {
                                                strerror = strerror + "Invalid Operator Name | ";
                                                flag++;
                                                currflag++;
                                            }
                                        }
                                        catch (Exception ex) { }
                                        con1.Close();
                                        com1 = new MySqlCommand("select count(*) from TRAI_circles where(circ='" + circ + "')", con1);
                                        con1.Open();
                                        dr1 = com1.ExecuteReader();
                                        dr1.Read();
                                        try
                                        {
                                            if (Convert.ToInt32(dr1[0].ToString().Trim()) == 0)
                                            {
                                                strerror = strerror + "Invalid Circle Name | ";
                                                flag++;
                                                currflag++;
                                            }
                                        }
                                        catch (Exception ex) { }
                                        con1.Close();
                                    }
                                    com1 = new MySqlCommand("select count(*) from TRAI_tariffrecords where(oper='" + oper + "') and (uniqueid='" + uniqueid + "') and (actiontotake='Launch')", con1);
                                    con1.Open();
                                    dr1 = com1.ExecuteReader();
                                    dr1.Read();
                                    try
                                    {
                                        if (Convert.ToInt32(dr1[0].ToString().Trim()) > 0)
                                        {
                                            strerror = strerror + "Record ID already exists | ";
                                            flag++;
                                            currflag++;
                                        }
                                    }
                                    catch (Exception ex) { }
                                    con1.Close();
                                    if (actiontotake.ToUpper() != "LAUNCH" && actiontotake.ToUpper() != "REVISION" && actiontotake.ToUpper() != "CORRECTION" && actiontotake.ToUpper() != "WITHDRAWAL")
                                    {
                                        strerror = strerror + "Record type should be LAUNCH / REVISION / CORRECTION / WITHDRAWAL | ";
                                        flag++;
                                        currflag++;
                                    }
                                    if (regprom.ToUpper() != "REGULAR" && regprom.ToUpper() != "PROMOTIONAL")
                                    {
                                        strerror = strerror + "Record type should be REGULAR or PROMOTIONAL | ";
                                        flag++;
                                        currflag++;
                                    }



                                    // validate referential integrity parameters - CODE ENDS HERE //

                                    if (uniqueid == null)
                                    {
                                        uniqueid = "";
                                    }

                                    if (uniqueid != "" && oper != "" && circ != "")
                                    {

                                        if (currflag > 0 && strerror != "")
                                        {
                                            getMaxRno("rno", "TRAI_temperrors");
                                            com1 = new MySqlCommand("insert into TRAI_temperrors values('" + zno + "','" + filename + "','" + SheetName + "','" + strerror + "','" + uniqueid + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')", con1);
                                            con1.Open();
                                            com1.ExecuteNonQuery();
                                            con1.Close();
                                        }
                                        else
                                        {
                                            if (rowcntr == 0)
                                            {
                                                //Response.Redirect("invalid.aspx?t1=Data. There are no records to upload.");
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    com = new MySqlCommand(rowqry, con);
                                                    con.Open();
                                                    com.ExecuteNonQuery();
                                                }
                                                catch (Exception ex)
                                                {
                                                    getMaxRno("rno", "TRAI_temperrors");
                                                    com1 = new MySqlCommand("insert into TRAI_temperrors values('" + zno + "','" + filename + "','" + SheetName + "','" + ex.ToString().Replace("'", "`") + "','" + uniqueid + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')", con1);
                                                    con1.Open();
                                                    com1.ExecuteNonQuery();
                                                    con1.Close();
                                                }
                                                con.Close();
                                            }
                                        }

                                    }
                                }
                            }

                            rowcntr++;

                        }       // row-wise entries end here


                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.ToString());
                    }

                
               
            }



            // display errors for this filename //

            string str1 = "select * from TRAI_temperrors where(fname='" + filename + "') order by rno";
            string mystr = "<table width=100% cellspacing=1 cellpadding=4>";

            int cntr = 0;
            com3 = new MySqlCommand(str1, con3);
            con3.Open();
            dr3 = com3.ExecuteReader();
            while (dr3.Read())
            {
                if (cntr == 0) // write column names
                {
                    mystr = mystr + "<tr>";
                    for (int i = 0; i <= dr3.FieldCount - 1; i++)
                    {
                        mystr = mystr + "<td class=tablehead align=center>" + dr3.GetName(i) + "</td>";
                    }
                    mystr = mystr + "</tr>";
                }

                mystr = mystr + "<tr>";
                for (int j = 0; j <= dr3.FieldCount - 1; j++)
                {
                    mystr = mystr + "<td class=tablecell align=center>" + dr3[j].ToString().Trim() + "</td>";
                }
                mystr = mystr + "</tr>";
                cntr++;
            }
            con3.Close();
            mystr = mystr + "</table>";

            Response.Write(mystr);

            // display errors for this filename - CODE ENDS HERE //




        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("System.Data.DataRow.GetDataColumn(String columnName)"))
            {
                Response.Write("<font face=arial color=red><b>The File Contains An Invalid Column Name.</b></font>");
            }
            if (ex.ToString().Contains("String or binary data would be truncated"))
            {
                Response.Write("<font face=arial color=red><b>The length of one or more data fields is greater than the permitted length in the database table.</b></font>");
            }
            if (ex.ToString().Contains("Data too long for column"))
            {
                Response.Write("<font face=arial color=red><b>The length of one or more data fields is greater than the permitted length in the database table.</b></font>");
            }

            Response.Write("<br /><br />" + ex.ToString());
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






}
