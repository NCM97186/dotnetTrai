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


public partial class TSP_SubmissionReport : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno, page;
    DateTime dt1, dt2;
    double modrno;
    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        tablename = "TRAI_tarifferrorlog";

        try
        {
            if (Request.UrlReferrer == null)
            {
                Response.Redirect("TSP_logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("TSP_logout.aspx");
            }           

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["pn"]) && !string.IsNullOrEmpty(Request.QueryString["user"]) && !string.IsNullOrEmpty(Request.QueryString["dt1"]) && !string.IsNullOrEmpty(Request.QueryString["dt2"]))
                {
                    TextDate.Text = Convert.ToDateTime(Request.QueryString["dt1"]).ToString("dd-MMM-yyyy");
                    TextDate2.Text = Convert.ToDateTime(Request.QueryString["dt2"]).AddDays(-1).ToString("dd-MMM-yyyy");
                    BindData();

                }
                else
                {
                    TextDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                    TextDate2.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                }
            }

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
            try
            {
                dt1 = Convert.ToDateTime(TextDate.Text.Trim());
                dt2 = Convert.ToDateTime(TextDate2.Text.Trim());
                dt2 = dt2.AddDays(1);
            }
            catch (Exception ex)
            {
                flag = 1;
                Response.Write("<script>alert('Please select a valid date range');</script>");
            }
            if (dt1 > dt2)
            {
                flag = 1;
                Response.Write("<script>alert('From Date Can not be greater than ToDate.');</script>");
                TextDate.Focus();
                return;
            }

            if (flag == 0)
            {
                string oper = "";
                string user = Request["user"].ToString().Trim();
                com = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + user + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    oper = dr["oper"].ToString().Trim();
                }
                catch (Exception ex) { }
                con.Close();

                
                mystr = "";
                //mystr = mystr + "<tr><td class=tablehead align=center>S. No.</td><td class=tablehead align=center>Consumer Name</td><td class=tablehead align=center>Contact No.</td><td class=tablehead align=center>Feedback</td></tr>";
                string css = "tablecell3b";
                page = 1;
                BindData();
            }

            
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }

    }

    public void BindData()
    {
        divdownload.InnerHtml = "<p align=right><a href=javascript:funExcel() ><img src=images/excel.jpg border=0 title='Download Excel' /></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>";

        int pageNo = page==1 ? page : Convert.ToInt32(Request.QueryString["pn"]);

        dt1 = !string.IsNullOrEmpty(TextDate.Text) ? Convert.ToDateTime(TextDate.Text) : Convert.ToDateTime(Request.QueryString["dt1"]);
        dt2 = !string.IsNullOrEmpty(TextDate2.Text) ? Convert.ToDateTime(TextDate2.Text).AddDays(1) : Convert.ToDateTime(Request.QueryString["dt2"]);
        
        string user = Request["user"].ToString().Trim();
        int pageSize = 50;
        int reccount = 0;

        try
        {
            int sno = 1;
            Int64 TotalRecords = 0;
            com = new MySqlCommand("select Count(1) As Total from " + tablename + " where(recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.ToString("yyyy-MM-dd") + "') and (uname='" + user + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            try
            {
                Int64.TryParse(dr["Total"].ToString().Trim(), out TotalRecords);
            }
            catch (Exception ex) { }
            con.Close();

            string strQury = "select * from " + tablename + " where(recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.ToString("yyyy-MM-dd") + "') and (uname='" + user + "') order by recdate desc,rno desc ";
            if(pageNo > 1)
            {
                strQury = strQury + "LIMIT " + (pageNo-1) * pageSize + "," + pageSize;
            }
            else
            {
                strQury = strQury + "LIMIT " + pageSize;
            }
            
            com = new MySqlCommand(strQury, con);
            con.Open();
            MySqlDataAdapter ada = new MySqlDataAdapter(com);
            DataSet ds = new DataSet();
            ada.Fill(ds);

            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                mystr = mystr + "<table width=90% cellspacing=1 border=1 style=border-collapse:collapse cellpadding=7>";
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablehead align=left valign=top>Submitted On : " + Convert.ToDateTime(dr1["recdate"].ToString().Trim()).ToString("dd-MMM-yyyy hh:mm:ss") + "</td>";
                mystr = mystr + "<td class=tablehead align=right valign=top>Response Received On : " + Convert.ToDateTime(dr1["responsedate"].ToString().Trim()).ToString("dd-MMM-yyyy hh:mm:ss") + "</td>";
                mystr = mystr + "</tr>";
                mystr = mystr + "<tr>";
                mystr = mystr + "<td align=center valign=top colspan=2>" + dr1["errdet"].ToString().Trim() + "</td>";
                mystr = mystr + "</tr>";
                mystr = mystr + "</table><br />";
                reccount++;
                sno++;
            }
            con.Close();

            if (reccount == 0)
            {
                Response.Write("<script>alert('No Tariff Submitted in selected date range.');</script>");
                return;
            }
            divresults.InnerHtml = mystr; 
            string strPaging = Set_Paging(pageNo, pageSize, TotalRecords, "activeLink", "TSP_SubmissionReport.aspx", "disableLink", user, dt1, dt2);
            pagingDiv1.InnerHtml = strPaging;
            pagingDiv2.InnerHtml = strPaging;
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
        finally
        {
        }
    }

    public string Set_Paging(Int32 PageNumber, int PageSize, Int64 TotalRecords, string ClassName, string PageUrl, string DisableClassName, string user, DateTime dt1, DateTime dt2)
    {
        string ReturnValue = "";
        try
        {
            Int64 TotalPages = Convert.ToInt64(Math.Ceiling((double)TotalRecords / PageSize));
            if (PageNumber > 1)
            {
                if (PageNumber == 2)
                    ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim() + "?pn=" + Convert.ToString(PageNumber - 1) + "&user=" + user + "&dt1=" + dt1 + "&dt2=" + dt2 + "' class='" + ClassName + "'>Previous</a>&nbsp;&nbsp;&nbsp;";
                else
                {
                    ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim();
                    if (PageUrl.Contains("?"))
                        ReturnValue = ReturnValue + "&";
                    else
                        ReturnValue = ReturnValue + "?";
                    ReturnValue = ReturnValue + "pn=" + Convert.ToString(PageNumber - 1) + "&user=" + user + "&dt1=" + dt1 + "&dt2=" + dt2 + "' class='" + ClassName + "'>Previous</a>&nbsp;&nbsp;&nbsp;";
                }
            }
            else
                ReturnValue = ReturnValue + "<span class='" + DisableClassName + "'>Previous</span>&nbsp;&nbsp;&nbsp;";
            if ((PageNumber - 3) > 1)
                ReturnValue = ReturnValue + "<a href='" + PageUrl.Trim() + "?pn=" + Convert.ToString(PageNumber) + "&user=" + user + "&dt1=" + dt1 + "&dt2=" + dt2 + "' class='" + ClassName + "'>1</a>&nbsp;.....&nbsp;|&nbsp;";
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
                        ReturnValue = ReturnValue + "pn=" + i.ToString() + "&user=" + user + "&dt1=" + dt1 + "&dt2=" + dt2 + "' class='" + ClassName + "'>" + i.ToString() + "</a>&nbsp;|&nbsp;";
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
                        ReturnValue = ReturnValue + "pn=" + i.ToString() + "&user=" + user + "&dt1=" + dt1 + "&dt2=" + dt2 + "' class='" + ClassName + "'>" + i.ToString() + "</a>&nbsp;|&nbsp;";
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
                ReturnValue = ReturnValue + "pn=" + TotalPages.ToString() + "&user=" + user + "&dt1=" + dt1 + "&dt2=" + dt2 + "' class='" + ClassName + "'>" + TotalPages.ToString() + "</a>";
            }
            if (PageNumber < TotalPages)
            {
                ReturnValue = ReturnValue + "&nbsp;&nbsp;&nbsp;<a href='" + PageUrl.Trim();
                if (PageUrl.Contains("?"))
                    ReturnValue = ReturnValue + "&";
                else
                    ReturnValue = ReturnValue + "?";
                ReturnValue = ReturnValue + "pn=" + Convert.ToString(PageNumber + 1) + "&user=" + user + "&dt1=" + dt1 + "&dt2=" + dt2 + "' class='" + ClassName + "'>Next</a>";
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

                string user = Request["user"].ToString().Trim();

                int reccount = 0;


                int sno = 1;

                dt1 = !string.IsNullOrEmpty(TextDate.Text) ? Convert.ToDateTime(TextDate.Text) : Convert.ToDateTime(Request.QueryString["dt1"]);
                dt2 = !string.IsNullOrEmpty(TextDate2.Text) ? Convert.ToDateTime(TextDate2.Text).AddDays(1) : Convert.ToDateTime(Request.QueryString["dt2"]);

                string strQury = "select * from " + tablename + " where(recdate>='" + dt1.ToString("yyyy-MM-dd") + "' and recdate<'" + dt2.ToString("yyyy-MM-dd") + "') and (uname='" + user + "') order by recdate desc,rno desc ";

                com = new MySqlCommand(strQury, con);
                con.Open();
                MySqlDataAdapter ada = new MySqlDataAdapter(com);
                DataSet ds = new DataSet();
                ada.Fill(ds);

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    mystr = mystr + "<table width=90% cellspacing=1 border=1 style=border-collapse:collapse cellpadding=7>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablehead align=left valign=top>Submitted On : " + Convert.ToDateTime(dr1["recdate"].ToString().Trim()).ToString("dd-MMM-yyyy hh:mm:ss") + "</td>";
                    mystr = mystr + "<td class=tablehead align=right valign=top>Response Received On : " + Convert.ToDateTime(dr1["responsedate"].ToString().Trim()).ToString("dd-MMM-yyyy hh:mm:ss") + "</td>";
                    mystr = mystr + "</tr>";
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td align=center valign=top colspan=2>" + dr1["errdet"].ToString().Trim() + "</td>";
                    mystr = mystr + "</tr>";
                    mystr = mystr + "</table><br />";
                    reccount++;
                    sno++;
                }
                string attachment = "attachment; filename=Report.xls";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                StringBuilder sb = new StringBuilder();

                sb.Append(mystr);

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
            Response.Write("<script>alert('" + ex.ToString() + "');</script>");
        }
    }

    protected void getMaxRno(string myfield, string mytable)
    {
        try
        {

            com = new MySqlCommand("select count(*) from " + mytable, con1);
            con1.Open();
            dr = com.ExecuteReader();
            dr.Read();
            if (Convert.ToInt32(dr[0].ToString()) > 0)
            {
                com = new MySqlCommand("select max(" + myfield + ") from " + mytable, con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                zno = Convert.ToInt32(dr[0].ToString());
                zno = zno + 1;
                con.Close();
            }
            else
            {
                zno = 1;
            }
            con.Close();

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
}
