using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;     // #####  FOR MYSQL
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Web.UI;

public partial class FEA_reptariff : System.Web.UI.Page
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
            int flag = 0;
            DateTime ldate1 = Convert.ToDateTime("2001-01-01");
            DateTime ldate2 = Convert.ToDateTime("2030-01-01");
            DateTime rdate1 = Convert.ToDateTime("2001-01-01");
            DateTime rdate2 = Convert.ToDateTime("2030-01-01");

            double mrp1 = -1, mrp2 = 999999;

            string conditions = "(rno>0) ";

            // Operators Selection //
            conditions = conditions + " and (";
            string opers = "";
            string newOperator = "";
            for (int i = 0; i < ChkOper.Items.Count; i++)
            {
                if (ChkOper.Items[i].Selected == true)
                {
                    opers = opers + ChkOper.Items[i].Text.Trim();
                    newOperator = newOperator + ChkOper.Items[i].Text.Trim() + ",";

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

            conditions = conditions + " and ( ";
            string circs = "";
            string newCircs = "";
            for (int i = 0; i < ChkCirc.Items.Count; i++)
            {
                if (ChkCirc.Items[i].Selected == true)
                {
                    circs = circs + ChkCirc.Items[i].Text.Trim();

                    newCircs = newCircs + "'" + ChkCirc.Items[i].Text.Trim() + "',";
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
            string newTtypes = "";
            for (int i = 0; i < ChkTtype.Items.Count; i++)
            {
                if (ChkTtype.Items[i].Selected == true)
                {
                    ttypes = ttypes + ChkTtype.Items[i].Text.Trim();

                    newTtypes = newTtypes + "'" + ChkTtype.Items[i].Text.Trim() + "',";
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
            if (mrp1 > mrp2)
            {
                flag = 1;
                Response.Write("<script>alert('Price / Rental Range From Can not be greater than Price / Rental Range To');</script>");
                TextPrice1.Focus();
                return;
            }
            conditions = conditions + " and ((mrp>=" + mrp1 + " and mrp<=" + mrp2 + ") or (ISP_rental>=" + mrp1 + " and ISP_rental<=" + mrp2 + "))";


            if (flag == 0)
            {
                TextConditions.Text = conditions;
                TextPage.Text = "1";
                showRecords(null, null);
                //showRecordsNew(newOperator, newCircs, newTtypes);
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
            StringBuilder strb = new StringBuilder();


            int pageNo = !string.IsNullOrEmpty(TextPage.Text.Trim()) ? Convert.ToInt32(TextPage.Text.Trim()) : Convert.ToInt32(Request.QueryString["pn"]);
            int PageSize = 50;
            Int64 TotalRecords = 0;

            string conditions = !string.IsNullOrEmpty(TextConditions.Text.Trim()) ? TextConditions.Text.Trim() : WebUtility.HtmlDecode(Request.QueryString["conditions"]);
            int showType = ChkArchive.Checked == true ? 2 : 1;// :Convert.ToInt32(Request.QueryString["radActive"]); // '1' is for showing records from TRAI_tariffs only, '2' is for showing records from TRAI_tariffs as well as TRAI_archive
            string user = Request["user"].ToString().Trim();

            string actiondate = TextDate.Text.Trim() + "$" + TextDate2.Text.Trim();
            string reportdate = TextDate3.Text.Trim() + "$" + TextDate4.Text.Trim();
            string mrp = TextPrice1.Text.Trim() + "$" + TextPrice2.Text.Trim();

            string sortby = TextSortBy.Text.Trim();
            if (string.IsNullOrEmpty(sortby))
            {
                sortby = " order by reportdate";
            }

            string myqry = "";

            string cols = "TRAI_tariffs.rno, TRAI_tariffs.fname, TRAI_tariffs.recdate,TRAI_tariffs.uname, TRAI_tariffs.ttype, TRAI_tariffs.tariffdet, TRAI_tariffs.uniqueid, TRAI_tariffs.oper, TRAI_tariffs.circ,TRAI_tariffs.service, TRAI_tariffs.categ, TRAI_tariffs.actiontotake,TRAI_tariffs.planname,TRAI_tariffs.planid, TRAI_tariffs.reportdate, TRAI_tariffs.actiondate, TRAI_tariffs.regprom,TRAI_tariffs.offerconditions, TRAI_tariffs.mrp, TRAI_tariffs.validity,TRAI_tariffs.ISP_rental";

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

                myqry = "select *, 'Active' as stat from TRAI_tariffs where (rno > 0) and (actiontotake<>'WITHDRAWAL') and  " + conditions + " " + sortby;
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
                myqry = "Select sum(Total) As 'Total' from (select Count(1) As Total from TRAI_tariffs where(rno>0) and (actiontotake<>'WITHDRAWAL') and " + conditions + " UNION " +
                    "select Count(1) AS Total from TRAI_archive where (rno > 0) and (actiontotake='WITHDRAWAL') and " + conditions + " ) as TotalRecordCount";
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

                myqry = "Select * from (select *,'' as Archiveno,'' as Archivedate, 'Active' as stat from TRAI_tariffs where (rno > 0) and (actiontotake<>'WITHDRAWAL') and " + conditions;
                myqry = myqry + " UNION ";
                myqry = myqry + " select *, 'Not-Active' as stat from TRAI_archive where (rno > 0) and (actiontotake='WITHDRAWAL') and " + conditions + " ) As AllRecords " + sortby;
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
                divresult.InnerHtml = DisplayRecords(ds);
                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);
                string strPaging = Set_Paging(pageNo, PageSize, TotalRecords, "activeLink", "FEA_reptariff.aspx", "disableLink", WebUtility.HtmlEncode(conditions), RadOType.SelectedItem.Text.Trim(), showType.ToString(), user, actiondate, reportdate, mrp);

                pagingDiv1.InnerHtml = strPaging;
                pagingDiv2.InnerHtml = strPaging;
            }
            else
            {
                divresult.InnerHtml = "No Record found for Selected Criteria";
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    private string DisplayRecords(DataSet ds)
    {        
        StringBuilder strb2 = new StringBuilder();

        divicon.InnerHtml = "<a href=javascript:funExcel() ><img src=images/excel.jpg border=0 /></a>";

        string css = "tablecell3";        

        strb2.Append("<table width=100% border=1 style=border-collapse:collapse cellspacing=1 cellpadding=5>");
        strb2.Append("<tr>");
        strb2.Append("<td class=" + css + " width=10% align=center valign=top>TSP<br /><br /><a href=javascript:funsort('oper','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('oper','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=8% align=center valign=top>LSA<br /><br /><a href=javascript:funsort('circ','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('circ','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=12% align=center valign=top>Tariff Type<br /><br /><a href=javascript:funsort('ttype','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('ttype','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=10% align=center valign=top>Unique Record ID<br /><a href=javascript:funsort('uniqueid','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('uniqueid','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=7% align=center valign=top>Service Type<br /><a href=javascript:funsort('service','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('service','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=6% align=center valign=top>Price &#8377;<br /><br /><a href=javascript:funsort('mrp','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('mrp','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=7% align=center valign=top>Validity<br /><br /><a href=javascript:funsort('validity','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('validity','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=9% align=center valign=top>Date of Launch / Revision / Correction / Withdrawal<br /><a href=javascript:funsort('actiondate','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('actiondate','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=10% align=center valign=top>Date of Reporting<br /><a href=javascript:funsort('reportdate','asc');><img src=images/sortup.png border=0></a>&nbsp;&nbsp;&nbsp;<a href=javascript:funsort('reportdate','desc');><img src=images/sortdown.png border=0></a></td>");
        strb2.Append("<td class=" + css + " width=7% align=center valign=top>Delayed Reporting</td>");
        strb2.Append("<td class=" + css + " width=7% align=center valign=top>Taken On Record</td>");
        strb2.Append("<td class=" + css + " width=7% align=center valign=top>Status</td>");
        strb2.Append("</tr>");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string css2 = "tablecell5b";

            strb2.Append("<tr>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["oper"].ToString().Trim() + "</td>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["circ"].ToString().Trim() + "</td>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["ttype"].ToString().Trim() + "</td>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["uniqueid"].ToString().Trim() + "</td>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["service"].ToString().Trim() + "</td>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["mrp"].ToString().Trim() + "</td>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + dr["validity"].ToString().Trim().Replace("-1", "").Replace("-2", "Unlimited") + "</td>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "") + "</td>");
            strb2.Append("<td class=" + css2 + " align=center valign=top>" + Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "") + "</td>");
            strb2.Append("</tr>");
            string det = "";
            det = det + "<a href='FEA_recorddetails.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Tariff Details</u></b></a>";
            det = det + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            det = det + "<a href='FEA_reviewsummary.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "&o=" + dr["oper"].ToString().Trim() + "&c=" + dr["circ"].ToString().Trim() + "' target=_blank><b><u>View Review Details</u></b></a>";
            strb2.Append("<tr>");
            strb2.Append("<td class=tablecell5c colspan=6 align=center valign=top><b>Tariff Summary : </b> " + dr["tariffdet"].ToString().Trim().Replace("~", "&quot;").Replace("&amp;", "&") + "</td>");
            strb2.Append("<td class=tablecell5c colspan=6 align=center valign=top>" + det + "</td>");
            strb2.Append("</tr>");
            strb2.Append("<tr>");
            strb2.Append("<td class=tablecell3b colspan=12 align=center valign=top><hr size=0></td>");
            strb2.Append("</tr>");
        }

        strb2.Append("</table>");

        return strb2.ToString();        
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
                //string attachment = "attachment; filename=TariffProducts.xls";
                string attachment = "attachment; filename=Report.xls";
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                StringBuilder sb = new StringBuilder();

                string conditions = !string.IsNullOrEmpty(TextConditions.Text.Trim()) ? TextConditions.Text.Trim() : WebUtility.HtmlDecode(Request.QueryString["conditions"]);
                int showType = ChkArchive.Checked == true ? 2 : 1;// :Convert.ToInt32(Request.QueryString["radActive"]); // '1' is for showing records from TRAI_tariffs only, '2' is for showing records from TRAI_tariffs as well as TRAI_archive
                string user = Request["user"].ToString().Trim();

                string sortby = TextSortBy.Text.Trim();

                if (string.IsNullOrEmpty(sortby))
                {
                    sortby = " order by reportdate";
                }

                string myqry = "";

                string cols = "TRAI_tariffs.rno, TRAI_tariffs.fname, TRAI_tariffs.recdate,TRAI_tariffs.uname, TRAI_tariffs.ttype, TRAI_tariffs.tariffdet, TRAI_tariffs.uniqueid, TRAI_tariffs.oper, TRAI_tariffs.circ,TRAI_tariffs.service, TRAI_tariffs.categ, TRAI_tariffs.actiontotake,TRAI_tariffs.planname,TRAI_tariffs.planid, TRAI_tariffs.reportdate, TRAI_tariffs.actiondate, TRAI_tariffs.regprom,TRAI_tariffs.offerconditions, TRAI_tariffs.mrp, TRAI_tariffs.validity,TRAI_tariffs.ISP_rental";

                if (showType == 1)
                {
                    myqry = "select oper AS TSP, circ AS LSA ,ttype AS `Tariff Type` ,uniqueid AS `Unique Record ID` , service AS `Service Type`, mrp AS `Price` , validity AS `Validity`," +
                        " actiondate AS `Date of Launch / Revision / Correction / Withdrawal`,  'Active' as stat from TRAI_tariffs where (rno > 0) and (actiontotake<>'WITHDRAWAL') and  " + conditions + " " + sortby;
                }

                if (showType == 2) 
                {
                    myqry = "Select oper AS TSP, circ AS LSA ,ttype AS `Tariff Type` ,uniqueid AS `Unique Record ID` , service AS `Service Type`, mrp AS `Price` , validity AS `Validity`,"+
                        "actiondate AS `Date of Launch / Revision / Correction / Withdrawal`,  stat AS `Status` ,Archiveno ,Archivedate from (select *,'' as Archiveno,'' as Archivedate, 'Active' as stat from TRAI_tariffs where (rno > 0) and (actiontotake<>'WITHDRAWAL') and " + conditions;
                    myqry = myqry + " UNION ";
                    myqry = myqry + " select *, 'Not-Active' as stat from TRAI_archive where (rno > 0) and (actiontotake='WITHDRAWAL') and " + conditions + " ) As AllRecords " + sortby;
                }

                com = new MySqlCommand(myqry, con);
                MySqlDataAdapter ada = new MySqlDataAdapter(com);
                DataSet ds = new DataSet();
                ada.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    using (StringWriter sw = new StringWriter())
                    {
                        HtmlTextWriter hw = new HtmlTextWriter(sw);
                        GridView grid = new GridView();
                        grid.AllowPaging = false;
                        grid.DataSource = ds.Tables[0];
                        grid.DataBind();

                        foreach (GridViewRow row in grid.Rows)
                        {
                            row.BackColor = System.Drawing.Color.White;
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = grid.AlternatingRowStyle.BackColor;
                                }
                                else
                                {
                                    cell.BackColor = grid.RowStyle.BackColor;
                                }
                                cell.CssClass = "textmode";


                            }
                        }

                        grid.RenderControl(hw);
                        string style = @"<style> .textmode { } </style>";
                        Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex2)
            {
                Response.Write("<script>alert('" + ex2.ToString() + "');</script>");
            }

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.Message.ToString() + "');</script>");
        }
    }
    private string DisplayExcel(DataSet ds)
    {
        StringBuilder strb = new StringBuilder();

        strb.Append("<table width=100% border=1 style=border-collapse:collapse cellspacing=1 cellpadding=5>");
        strb.Append("<tr>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>TSP</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>LSA</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Tariff Type</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Unique Record ID</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Service Type</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Price</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Validity</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Date of Launch / Revision / Correction / Withdrawal</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Date of Reporting</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Tariff Details</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Delayed Reporting</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Taken On Record</b></td>");
        strb.Append("<td align=center valign=top style=background-color:#efefef;><b>Status</b></td>");
        strb.Append("</tr>");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strb.Append("<tr>");
            strb.Append("<td align=center valign=top>" + dr["oper"].ToString().Trim() + "</td>");
            strb.Append("<td align=center valign=top>" + dr["circ"].ToString().Trim() + "</td>");
            strb.Append("<td align=center valign=top>" + dr["ttype"].ToString().Trim() + "</td>");
            strb.Append("<td align=center valign=top>" + dr["uniqueid"].ToString().Trim() + "</td>");
            strb.Append("<td align=center valign=top>" + dr["service"].ToString().Trim() + "</td>");
            strb.Append("<td align=center valign=top>" + dr["mrp"].ToString().Trim() + "</td>");
            strb.Append("<td align=center valign=top>" + dr["validity"].ToString().Trim().Replace("-1", "").Replace("-2", "Unlimited") + "</td>");
            strb.Append("<td align=center valign=top>" + Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "") + "</td>");
            strb.Append("<td align=center valign=top>" + Convert.ToDateTime(dr["reportdate"].ToString().Trim()).ToString("dd-MMM-yyyy").Replace("01-Jan-2011", "").Replace("01-Jan-2001", "") + "</td>");
            strb.Append("<td align=center valign=top>" + dr["tariffdet"].ToString().Trim().Replace("~", "&quot;").Replace("&amp;", "&") + "</td>");
            strb.Append("</tr>");
        }

        strb.Append("</table>");
        return strb.ToString().Trim();
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
}