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
using System.Net;
using System.Text;
using System.IO;

public partial class FEA_repcomparison : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    Table tbresults;
    CheckBox[] arrResult;
    CheckBox chkResult;
    int zno;

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

        if (!IsPostBack)
        {
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
            int i = 0;
            while (dr.Read())
            {
                ChkTtype.Items.Add(dr["ttype"].ToString().Trim());
                if (!string.IsNullOrEmpty(Request.QueryString["conditions"]) && Request.QueryString["conditions"].Contains(dr["ttype"].ToString().Trim()))
                {
                    ChkTtype.Items[i].Selected = true;
                }
                i++;
            }
            con.Close();

            if (!string.IsNullOrEmpty(Request.QueryString["pn"]) && !string.IsNullOrEmpty(Request.QueryString["conditions"]) && !string.IsNullOrEmpty(Request.QueryString["radActive"]))
            {
                if (Request.QueryString["radActive"] == "2")
                {
                    ChkArchive.Checked = true;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["actionDate"]))
                {
                    string[] dates = Request.QueryString["actionDate"].Split('$');
                    TextDate.Text = dates[0];
                    TextDate2.Text = dates[1];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["reportDate"]))
                {
                    string[] dates = Request.QueryString["reportDate"].Split('$');
                    TextDate3.Text = dates[0];
                    TextDate4.Text = dates[1];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["mrp"]))
                {
                    string[] dates = Request.QueryString["mrp"].Split('$');
                    TextPrice1.Text = dates[0];
                    TextPrice2.Text = dates[1];
                }
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
                return;
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
                return;
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
                return;
            }
            conditions = conditions + ")";
            conditions = conditions.Replace("()", "");

            // Launch Date Range //
            try
            {
                ldate1 = Convert.ToDateTime(TextDate.Text.Trim());
            }
            catch (Exception ex) { }
            try
            {
                ldate2 = Convert.ToDateTime(TextDate2.Text.Trim());
            }
            catch (Exception ex) { }
            if (ldate1 > ldate2)
            {
                flag = 1;
                Response.Write("<script>alert('ActionDate From Can not be greater than ActionDate To');</script>");
                TextDate.Focus();
                return;
            }
            conditions = conditions + " and (actiondate>='" + ldate1.ToString("yyyy-MM-dd") + "' and actiondate<'" + ldate2.AddDays(1).ToString("yyyy-MM-dd") + "')";

            // Report Date Range //
            try
            {
                rdate1 = Convert.ToDateTime(TextDate3.Text.Trim());
            }
            catch (Exception ex) { }
            try
            {
                rdate2 = Convert.ToDateTime(TextDate4.Text.Trim());
            }
            catch (Exception ex) { }
            if (rdate1 > rdate2)
            {
                flag = 1;
                Response.Write("<script>alert('Reportdate From Can not be greater than ReportDate To');</script>");
                TextDate3.Focus();
                return;
            }
            conditions = conditions + " and (reportdate>='" + rdate1.ToString("yyyy-MM-dd") + "' and reportdate<'" + rdate2.AddDays(1).ToString("yyyy-MM-dd") + "')";


            // Price Range Selection //
            try
            {
                mrp1 = Convert.ToDouble(TextPrice1.Text.Trim());
            }
            catch (Exception ex) { }
            try
            {
                mrp2 = Convert.ToDouble(TextPrice2.Text.Trim());
            }
            catch (Exception ex) { }
            conditions = conditions + " and ((mrp>=" + mrp1 + " and mrp<=" + mrp2 + ") or (ISP_rental>=" + mrp1 + " and ISP_rental<=" + mrp2 + "))";


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
            string conditions = !string.IsNullOrEmpty(TextConditions.Text.Trim()) ? TextConditions.Text.Trim() : WebUtility.HtmlDecode(Request.QueryString["conditions"]);
            string sortby = !string.IsNullOrEmpty(TextSortBy.Text.Trim()) ? TextSortBy.Text.Trim() : Request.QueryString["sortby"];
            int showType = ChkArchive.Checked == true ? 2 : 1;// :Convert.ToInt32(Request.QueryString["radActive"]); // '1' is for showing records from TRAI_tariffs only, '2' is for showing records from TRAI_tariffs as well as TRAI_archive
            string user = Request["user"].ToString().Trim();

            string actiondate = TextDate.Text.Trim() + "$" + TextDate2.Text.Trim();
            string reportdate = TextDate3.Text.Trim() + "$" + TextDate4.Text.Trim();
            string mrp = TextPrice1.Text.Trim() + "$" + TextPrice2.Text.Trim();

            if (string.IsNullOrEmpty(sortby))
            {
                sortby = " order by reportdate";
            }

            string myqry = "";

            if (showType == 1)
            {
                com = new MySqlCommand("select Count(1) As Total from TRAI_tariffs where(rno>0) and (actiontotake<>'WITHDRAWAL') and " + conditions, con);
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
            if (showType == 2)
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

            // Display the records //

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                tca1.Text = "<a href=javascript:funCompareSend() ><img src=images/btncomp.jpg width=100 border=0 /></a>";
                tca2.Text = "";
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
                tr.Controls.Add(tc0);
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
                    tcc6.Text = Math.Round(Convert.ToDouble(dr["mrp1"].ToString().Trim())).ToString();
                    tcc7.Text = Math.Round(Convert.ToDouble(dr["validity"].ToString().Trim())).ToString().Replace("-1", "").Replace("-2", "Unlimited");
                    tcc8.Text = Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "");
                    tcc9.Text = Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "");
                    tcc10.Text = dr["delay"].ToString().Trim();
                    tcc11.Text = dr["takenonrecord"].ToString().Trim();
                    tcc12.Text = dr["stat"].ToString().Trim();
                    trr.Controls.Add(tcc0);
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
                    tcd1.ColumnSpan = 7;
                    tcd2.ColumnSpan = 6;
                    tcd1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tcd2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tcd1.CssClass = "tablecell5c";
                    tcd2.CssClass = "tablecell5c";
                    tcd1.Text = "<b>Tariff Summary : </b> " + dr["tariffdet"].ToString().Trim().Replace("~", "&quot;").Replace("&amp;", "&");
                    string det = "";
                    det = det + "<a href='FEA_recorddetails.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Tariff Details</u></b></a>";
                    det = det + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    det = det + "<a href='FEA_reviewsummary.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Review Details</u></b></a>";
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
                tcb1.ColumnSpan = 11;
                tcb2.ColumnSpan = 2;
                tcb1.CssClass = "tablecell";
                tcb2.CssClass = "tablecell";
                tcb1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tcb2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                tcb2.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Bottom;
                tcb1.Text = "<a href=javascript:funCompareSend() ><img src=images/btncomp.jpg width=100 border=0 /></a>";
                tcb2.Text = "";
                trb.Controls.Add(tcb1);
                trb.Controls.Add(tcb2);
                tbresults.Controls.Add(trb);

                divresult.Controls.Add(tbresults);
                string strPaging = Set_Paging(pageNo, PageSize, TotalRecords, "activeLink", "FEA_repcomparison.aspx", "disableLink", WebUtility.HtmlEncode(conditions), RadOType.SelectedItem.Text.Trim(), showType.ToString(), user, actiondate, reportdate, mrp);
                pagingDiv1.InnerHtml = strPaging;
                pagingDiv2.InnerHtml = strPaging;

                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    public string Set_Paging(Int32 PageNumber, int PageSize, Int64 TotalRecords, string ClassName, string PageUrl, string DisableClassName, string conditions, string radOtype, string radActive, string user, string actiondate, string reportdate, string mrp)
    {
        string ReturnValue = "";
        try
        {
            Int64 TotalPages = Convert.ToInt64(Math.Ceiling((double)TotalRecords / PageSize));
            if (PageNumber > 1)
            {
                if (PageNumber == 2)
                    ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim() + "?pn=" + Convert.ToString(PageNumber - 1) + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&actionDate=" + actiondate + "&reportDate=" + reportdate + "&mrp=" + mrp + "' class='" + ClassName + "'>Previous</a>&nbsp;&nbsp;&nbsp;";
                else
                {
                    ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim();
                    if (PageUrl.Contains("?"))
                        ReturnValue = ReturnValue + "&";
                    else
                        ReturnValue = ReturnValue + "?";
                    ReturnValue = ReturnValue + "pn=" + Convert.ToString(PageNumber - 1) + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&actionDate=" + actiondate + "&reportDate=" + reportdate + "&mrp=" + mrp + "' class='" + ClassName + "'>Previous</a>&nbsp;&nbsp;&nbsp;";
                }
            }
            else
                ReturnValue = ReturnValue + "<span class='" + DisableClassName + "'>Previous</span>&nbsp;&nbsp;&nbsp;";
            if ((PageNumber - 3) > 1)
                ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim() + "?pn=" + Convert.ToString(PageNumber) + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&actionDate=" + actiondate + "&reportDate=" + reportdate + "&mrp=" + mrp + "' class='" + ClassName + "'>1</a>&nbsp;.....&nbsp;|&nbsp;";

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
                        ReturnValue = ReturnValue + "pn=" + i.ToString() + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&actionDate=" + actiondate + "&reportDate=" + reportdate + "&mrp=" + mrp + "' class='" + ClassName + "'>" + i.ToString() + "</a>&nbsp;|&nbsp;";
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
                        ReturnValue = ReturnValue + "pn=" + i.ToString() + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&actionDate=" + actiondate + "&reportDate=" + reportdate + "&mrp=" + mrp + "' class='" + ClassName + "'>" + i.ToString() + "</a>&nbsp;|&nbsp;";
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
                ReturnValue = ReturnValue + "pn=" + TotalPages.ToString() + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&actionDate=" + actiondate + "&reportDate=" + reportdate + "&mrp=" + mrp + "' class='" + ClassName + "'>" + TotalPages.ToString() + "</a>";
            }
            if (PageNumber < TotalPages)
            {
                ReturnValue = ReturnValue + "&nbsp;&nbsp;&nbsp;<a href='" + PageUrl.Trim();
                if (PageUrl.Contains("?"))
                    ReturnValue = ReturnValue + "&";
                else
                    ReturnValue = ReturnValue + "?";
                ReturnValue = ReturnValue + "pn=" + Convert.ToString(PageNumber + 1) + "&user=" + user + "&conditions=" + conditions + "&radOtype=" + radOtype + "&radActive=" + radActive + "&actionDate=" + actiondate + "&reportDate=" + reportdate + "&mrp=" + mrp + "' class='" + ClassName + "'>Next</a>";
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

                sb.Append(divExcel.InnerHtml.ToString());

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

    protected void TextDate4_PreRender(object s, EventArgs e)
    {
        TextDate4.Attributes.Add("onfocus", "showCalender(calender,this)");
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