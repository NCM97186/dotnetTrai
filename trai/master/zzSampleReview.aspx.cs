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


public partial class zzSampleReview : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno, size;
    DateTime dt1, dt2;
    double modrno;
    RadioButtonList[] arrAction;
    RadioButtonList RadAction;
    DropDownList[] arrForward;
    DropDownList DropForward;
    TextBox[] arrRemarks, arrUniqueId;
    TextBox TextRemarks, TextUniqueId;
    CheckBox[] arrClear;
    CheckBox chkClear;
    
    string tsp, lsa, tariffproduct, servicetype, violations, reportingtype, recid, productname, tariffdet, prevremarks;
    DateTime reportdate, launchdate;
    double price, validity;

    
    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        //tablename = "TRAI_tarifferrorlog";

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


            size = 3;
            arrAction = new RadioButtonList[size];
            arrRemarks = new TextBox[size];
            arrUniqueId = new TextBox[size];
            arrForward = new DropDownList[size];
            arrClear = new CheckBox[size];


            for (int i = 0; i < size; i++)
            {
                if(i==0)
                {
                    tsp = "Vodafone";
                    lsa = "Andhra Pradesh";
                    tariffproduct = "Prepaid STV";
                    servicetype = "GSM";
                    reportdate = Convert.ToDateTime("29-Apr-2018");
                    price = 210;
                    violations = "None";
                    reportingtype = "Launch";
                    recid = "VD02PMST0001";
                    productname = "Festive Bonanza";
                    validity = 28;
                    launchdate = Convert.ToDateTime("28-Apr-2018");
                    tariffdet = "";
                    prevremarks = "";
                }
                if (i == 1)
                {
                    tsp = "Airtel";
                    lsa = "Gujarat";
                    tariffproduct = "Prepaid Combo";
                    reportdate = Convert.ToDateTime("30-Apr-2018");
                    price = 115;
                    violations = "<u><font color=#ffffff>View Violations</font></u>";
                    reportingtype = "Launch";
                    recid = "AT09PMCM0005";
                    productname = "Dhamaka Combo";
                    validity = 20;
                    launchdate = Convert.ToDateTime("22-Apr-2018");
                    tariffdet = "1 GB 2G/3G/4G DATA/DAY, UNLIMITED LOCAL STD ALL CALLS FOR 14 DAYS,SPECIAL HANDSET FOR 1GB/PER DAY 3G/4G 28 DAYS.";
                    prevremarks = "<center><i>Previous Remarks</i></center><br /><b>30-Apr-2018</b><br />The tariff details provide ambiguous information about Local Call parameters. Please check. <p align=right>By : User 3</p>";
                }
                if (i == 2)
                {
                    tsp = "Jio";
                    lsa = "Delhi";
                    tariffproduct = "Prepaid STV";
                    reportdate = Convert.ToDateTime("30-Apr-2018");
                    price = 55;
                    violations = "None";
                    reportingtype = "Launch";
                    recid = "VD02PMST0002";
                    productname = "Double Deal";
                    validity = 10;
                    launchdate = Convert.ToDateTime("27-Apr-2018");
                    tariffdet = "";
                    prevremarks = "";
                }


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
                tcc1.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tcc2.Width = System.Web.UI.WebControls.Unit.Percentage(9);
                tcc3.Width = System.Web.UI.WebControls.Unit.Percentage(12);
                tcc4.Width = System.Web.UI.WebControls.Unit.Percentage(5);
                tcc5.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tcc6.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                tcc7.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tcc8.Width = System.Web.UI.WebControls.Unit.Percentage(20);
                tcc9.Width = System.Web.UI.WebControls.Unit.Percentage(20);
                tcc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc8.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc9.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc1.CssClass = "tablehead";
                tcc2.CssClass = "tablehead";
                tcc3.CssClass = "tablehead";
                tcc4.CssClass = "tablehead";
                tcc5.CssClass = "tablehead";
                tcc6.CssClass = "tablehead";
                tcc7.CssClass = "tablehead";
                tcc8.CssClass = "tablehead";
                tcc9.CssClass = "tablehead";
                tcc1.Text = "TSP";
                tcc2.Text = "LSA";
                tcc3.Text = "Tariff Product";
                tcc4.Text = "Service Type";
                tcc5.Text = "Date of Reporting";
                tcc6.Text = "Price (&#8377;)";
                tcc7.Text = "Regulatory violations";
                tcc8.Text = "Action";
                tcc9.Text = "Forward To";
                tr1.Controls.Add(tcc1);
                tr1.Controls.Add(tcc2);
                tr1.Controls.Add(tcc3);
                tr1.Controls.Add(tcc4);
                tr1.Controls.Add(tcc5);
                tr1.Controls.Add(tcc6);
                tr1.Controls.Add(tcc7);
                tr1.Controls.Add(tcc8);
                tr1.Controls.Add(tcc9);
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
                tcd1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd7.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd8.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd9.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcd1.CssClass = "tablecell3b";
                tcd2.CssClass = "tablecell3b";
                tcd3.CssClass = "tablecell3b";
                tcd4.CssClass = "tablecell3b";
                tcd5.CssClass = "tablecell3b";
                tcd6.CssClass = "tablecell3b";
                tcd7.CssClass = "tablecell3b";
                tcd8.CssClass = "tablecell3b";
                tcd9.CssClass = "tablecell3b";
                tcd1.Text = tsp;
                tcd2.Text = lsa;
                tcd3.Text = tariffproduct;
                tcd4.Text = servicetype;
                tcd5.Text = reportdate.ToString("dd-MMM-yyyy");
                tcd6.Text = price.ToString();
                if (violations == "None")
                {
                    tcd7.Attributes.Add("style", "background-color:#00ff00;");
                }
                else
                {
                    tcd7.Attributes.Add("style", "background-color:#ff0000;");
                }
                tcd7.Text = violations;
                RadAction = new RadioButtonList();
                RadAction.ID = "A" + i.ToString().Trim();
                RadAction.Items.Add("Taken on Record");
                RadAction.Items.Add("Revert to TSP");
                RadAction.Items.Add("Forward To");
                arrAction[i]=RadAction;
                TextUniqueId = new TextBox();
                TextUniqueId.ID = "U" + i.ToString();
                TextUniqueId.Text = recid;
                TextUniqueId.Visible = false;
                arrUniqueId[i] = TextUniqueId;
                tcd8.Controls.Add(arrAction[i]);
                tcd8.Controls.Add(arrUniqueId[i]);
                DropForward = new DropDownList();
                DropForward.ID = "D" + i.ToString();
                DropForward.Items.Add("");
                DropForward.Items.Add("User 1");
                DropForward.Items.Add("User 2");
                arrForward[i] = DropForward;
                tcd9.Controls.Add(arrForward[i]);
                tr2.Controls.Add(tcd1);
                tr2.Controls.Add(tcd2);
                tr2.Controls.Add(tcd3);
                tr2.Controls.Add(tcd4);
                tr2.Controls.Add(tcd5);
                tr2.Controls.Add(tcd6);
                tr2.Controls.Add(tcd7);
                tr2.Controls.Add(tcd8);
                tr2.Controls.Add(tcd9);
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
                tce8.ColumnSpan = 2;
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
                tce5.Text = "Date of Launch";
                tce6.Text = "Details";
                tce7.Text = "Generate File";
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
                tcf8.ColumnSpan = 2;
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
                tcf4.Text = validity.ToString();
                tcf5.Text = launchdate.ToString("dd-MMM-yyyy");
                tcf6.Text = "<b><u>View Summary</u></b><br /><br /><b><u>View Details</u></b>";
                tcf7.Text = "<img src=images/icongenerate.jpg border=0>";
                TextRemarks = new TextBox();
                TextRemarks.ID = "R" + i.ToString();
                TextRemarks.TextMode = TextBoxMode.MultiLine;
                TextRemarks.Width = 490;
                TextRemarks.Height = 75;
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
                tcg1.ColumnSpan = 6;
                TableCell tcg2 = new TableCell();
                TableCell tcg3 = new TableCell();
                tcg3.ColumnSpan = 2;
                tcg1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tcg2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcg3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tcg1.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
                tcg2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
                tcg3.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
                tcg1.CssClass = "tablecell3b";
                tcg2.CssClass = "tablecell3b";
                tcg3.CssClass = "tablecell3b";
                tcg1.Text = "<b>Tariff Summary : </b> " + tariffdet;
                tcg2.Text = "";
                tcg3.Text = prevremarks;
                tr5.Controls.Add(tcg1);
                tr5.Controls.Add(tcg2);
                tr5.Controls.Add(tcg3);
                tb.Controls.Add(tr5);

                TableRow tr6 = new TableRow();
                TableCell tch1 = new TableCell();
                tch1.ColumnSpan = 6;
                TableCell tch2 = new TableCell();
                TableCell tch3 = new TableCell();
                tch3.ColumnSpan = 2;
                tch1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tch2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tch3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tch1.CssClass = "tablecell3b";
                tch2.CssClass = "tablecell3b";
                tch3.CssClass = "tablecell3b";
                tch1.Text = "";
                tch2.Text = "";
                chkClear = new CheckBox();
                chkClear.ID = "C" + i.ToString();
                chkClear.Text = "Clear";
                chkClear.CssClass = "chks";
                arrClear[i] = chkClear;
                tch3.Attributes.Add("style", "background-color:#ff0000;");
                tch3.Controls.Add(arrClear[i]);
                tr6.Controls.Add(tch1);
                tr6.Controls.Add(tch2);
                tr6.Controls.Add(tch3);
                tb.Controls.Add(tr6);



                
                TableRow tr9 = new TableRow();
                tr9.Height = 70;
                TableCell tcz1 = new TableCell();
                tcz1.ColumnSpan = 9;
                tcz1.Text = "<hr size=0>";
                tr9.Controls.Add(tcz1);
                tb.Controls.Add(tr9);

                divTariffs.Controls.Add(tb);

            }


            

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
            

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }








}
