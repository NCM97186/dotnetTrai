using System;
using System.Globalization;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Net;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;     // #####  FOR MYSQL
using System.Data.OleDb;
using System.Text;
using System.IO;

public partial class FEA_repexception : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader  dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
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
        TextPage.Text = "1";
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["radOtype"]))
            {
                RadOType.SelectedValue = Request.QueryString["radOtype"];
            }

            LoadOperators(null, null);

            if (!string.IsNullOrEmpty(Request.QueryString["pn"]))
            {                
                if (!string.IsNullOrEmpty(Request.QueryString["actionDate"]))
                {
                    string[] dates = Request.QueryString["actionDate"].Split('$');
                    TextDate.Text = dates[0];
                    TextDate2.Text = dates[1];
                }
                TextPage.Text = !string.IsNullOrEmpty(Request.QueryString["pn"]) ? Request.QueryString["pn"] : (TextPage.Text);

                Button1_Click(null, null);
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
            dr1 = com.ExecuteReader();
            int j = 0;
            while (dr1.Read())
            {
                ChkOper.Items.Add(dr1["oper"].ToString().Trim());
                if (!string.IsNullOrEmpty(Request.QueryString["conditions"]) && Request.QueryString["conditions"].Contains(dr1["oper"].ToString().Trim()))
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
            divresult.InnerHtml = "";

            int flag = 0;
            DateTime sdate1 = Convert.ToDateTime("2001-01-01");
            DateTime sdate2 = Convert.ToDateTime("2030-01-01");

            string conditions = "(";

            // Operators Selection //
            
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


            int pageNo = Convert.ToInt32(TextPage.Text);
            int PageSize = 50;
            Int64 TotalRecords = 0;
            string actiondate = TextDate.Text.Trim() + "$" + TextDate2.Text.Trim();


            string tspusers = ",";
            if (flag == 0)
            {
                //com = new MySqlCommand("select * from TRAI_TSPUsers where " + conditions, con);
                //con.Open();
                //dr = com.ExecuteReader();
                //while (dr.Read())
                //{
                //    tspusers = tspusers + dr["uname"].ToString().Trim() + ",";
                //}
                //con.Close();

                try
                {
                    sdate1 = Convert.ToDateTime(TextDate.Text.Trim());
                }
                catch (Exception ex) { }
                try
                {
                    sdate2 = Convert.ToDateTime(TextDate2.Text.Trim());
                }
                catch (Exception ex) { }
                if (sdate1 > sdate2)
                {
                    flag = 1;
                    Response.Write("<script>alert('ActionDate From Can not be greater than ActionDate To');</script>");
                    TextDate.Focus();
                    return;
                }


                //com = new MySqlCommand("select * from TRAI_tarifferrorlog where(recdate>='" + sdate1.ToString("yyyy-MM-dd") + "' and recdate<'" + sdate2.AddDays(1).ToString("yyyy-MM-dd") + "') and ('" + tspusers.ToUpper() + "' like CONCAT('%', upper(uname), '%')) order by rno", con);
                //con.Open();
                //dr = com.ExecuteReader();
                //while (dr.Read())
                //{
                //    string oper = "";
                //    try
                //    {
                //        com1 = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + dr["uname"].ToString().Trim() + "')", con1);
                //        con1.Open();
                //        dr1 = com1.ExecuteReader();
                //        dr1.Read();
                //        oper = dr1["oper"].ToString().Trim();
                //        con1.Close();
                //    }
                //    catch (Exception ex) { }
                //    strdet = strdet + "<tr><td class=tablehead align=center valign=top>TSP : <b>" + oper + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;TSP User : <b>" + dr["uname"].ToString().Trim() + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Date / Time : <b>" + Convert.ToDateTime(dr["recdate"].ToString().Trim()).ToString("dd-MMM-yyyy hh:mm:ss") + "</b></td></tr>";
                //    strdet = strdet + "<tr><td class=tablecell align=center valign=top>" + dr["errdet"].ToString().Trim() + "</td></tr>";
                //    strdet = strdet + "<tr><td class=tablecell align=center valign=top><br /><hr size=0 color=#0000ff width=95%><br /></td></tr>";
                //}
                string myqry1 = "select Count(1) as Total from TRAI_tarifferrorlog a Inner Join TRAI_TSPUsers b on a.uname = b.Uname " +
                               "where " + conditions + " and (recdate>='" + sdate1.ToString("yyyy-MM-dd") + "' and recdate<'" + sdate2.AddDays(1).ToString("yyyy-MM-dd") + "') and errdet not like '%Success%'  order by a.rno";

                com = new MySqlCommand(myqry1, con);
                con.Open();
                dr1 = com.ExecuteReader();
                dr1.Read();
                try
                {
                    Int64.TryParse(dr1["Total"].ToString().Trim(), out TotalRecords);
                }
                catch (Exception ex) { }
                con.Close();

                string myqry = "select oper,a.* from TRAI_tarifferrorlog a Inner Join TRAI_TSPUsers b on a.uname = b.Uname " +
                               "where " + conditions + " and (recdate>='" + sdate1.ToString("yyyy-MM-dd") + "' and recdate<'" + sdate2.AddDays(1).ToString("yyyy-MM-dd") + "') and errdet not like '%Success%' order by a.rno";
                if (pageNo > 1)
                {
                    myqry = myqry + " LIMIT " + (pageNo - 1) * PageSize + "," + PageSize;
                }
                else
                {
                    myqry = myqry + " LIMIT " + PageSize;
                }

                com = new MySqlCommand(myqry, con);
                MySqlDataAdapter ada = new MySqlDataAdapter(com);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string strdet = "<table width=100% border=1 style=border-collapse:collapse cellspacing=1 cellpadding=5>";

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strdet = strdet + "<tr><td class=tablehead align=center valign=top>TSP : <b>" + dr["oper"].ToString().Trim() + " </b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;TSP User : <b>" + dr["uname"].ToString().Trim() + "</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Date / Time : <b>" + Convert.ToDateTime(dr["recdate"].ToString().Trim()).ToString("dd-MMM-yyyy hh:mm:ss") + "</b></td></tr>";
                        strdet = strdet + "<tr><td class=tablecell align=center valign=top>" + dr["errdet"].ToString().Trim() + "</td></tr>";
                        strdet = strdet + "<tr><td class=tablecell align=center valign=top><br /><hr size=0 color=#0000ff width=95%><br /></td></tr>";
                    }

                    strdet = strdet + "</table>";

                    divresult.InnerHtml = strdet;
                    ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);
                    string strPaging = Set_Paging(pageNo, PageSize, TotalRecords, "activeLink", "FEA_repexception.aspx", "disableLink", RadOType.SelectedItem.Text.Trim(), WebUtility.HtmlEncode(conditions), actiondate);

                    pagingDiv1.InnerHtml = strPaging;
                    pagingDiv2.InnerHtml = strPaging;
                }
                else
                {
                    divresult.InnerHtml = "No Record found for Selected Criteria";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    public string Set_Paging(Int32 PageNumber, int PageSize, Int64 TotalRecords, string ClassName, string PageUrl, string DisableClassName, string radOtype, string conditions, string actiondate)
    {
        string ReturnValue = "";
        try
        {
            Int64 TotalPages = Convert.ToInt64(Math.Ceiling((double)TotalRecords / PageSize));
            if (PageNumber > 1)
            {
                if (PageNumber == 2)
                    ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim() + "?pn=" + Convert.ToString(PageNumber - 1) + "&conditions=" + conditions + "&radOtype=" + radOtype + "&actionDate=" + actiondate +"' class='" + ClassName + "'>Previous</a>&nbsp;&nbsp;&nbsp;";
                else
                {
                    ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim();
                    if (PageUrl.Contains("?"))
                        ReturnValue = ReturnValue + "&";
                    else
                        ReturnValue = ReturnValue + "?";
                    ReturnValue = ReturnValue + "pn=" + Convert.ToString(PageNumber - 1) + "&conditions=" + conditions + "&radOtype=" + radOtype +"&actionDate=" + actiondate + "' class='" + ClassName + "'>Previous</a>&nbsp;&nbsp;&nbsp;";
                }
            }
            else
                ReturnValue = ReturnValue + "<span class='" + DisableClassName + "'>Previous</span>&nbsp;&nbsp;&nbsp;";
            if ((PageNumber - 3) > 1)
                ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim() + "?pn=" + Convert.ToString(PageNumber) + "&conditions=" + conditions + "&radOtype=" + radOtype + "&actionDate=" + actiondate + "' class='" + ClassName + "'>1</a>&nbsp;.....&nbsp;|&nbsp;";

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
                        ReturnValue = ReturnValue + "pn=" + i.ToString() + "&conditions=" + conditions + "&radOtype=" + radOtype + "&actionDate=" + actiondate + "' class='" + ClassName + "'>" + i.ToString() + "</a>&nbsp;|&nbsp;";
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
                        ReturnValue = ReturnValue + "pn=" + i.ToString() + "&conditions=" + conditions + "&radOtype=" + radOtype + "&actionDate=" + actiondate + "' class='" + ClassName + "'>" + i.ToString() + "</a>&nbsp;|&nbsp;";
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
                ReturnValue = ReturnValue + "pn=" + TotalPages.ToString() + "&conditions=" + conditions + "&radOtype=" + radOtype + "&actionDate=" + actiondate + "' class='" + ClassName + "'>" + TotalPages.ToString() + "</a>";
            }
            if (PageNumber < TotalPages)
            {
                ReturnValue = ReturnValue + "&nbsp;&nbsp;&nbsp;<a href='" + PageUrl.Trim();
                if (PageUrl.Contains("?"))
                    ReturnValue = ReturnValue + "&";
                else
                    ReturnValue = ReturnValue + "?";
                ReturnValue = ReturnValue + "pn=" + Convert.ToString(PageNumber + 1) + "&conditions=" + conditions + "&radOtype=" + radOtype + "&actionDate=" + actiondate + "' class='" + ClassName + "'>Next</a>";
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


    protected void TextDate_PreRender(object s, EventArgs e)
    {
        TextDate.Attributes.Add("onfocus", "showCalender(calender,this)");
    }

    protected void TextDate2_PreRender(object s, EventArgs e)
    {
        TextDate2.Attributes.Add("onfocus", "showCalender(calender,this)");
    }
}