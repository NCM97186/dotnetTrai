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

public partial class FEA_canceltaken : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["master"] == null)
        {
            //Response.Redirect("sessout.aspx");
        }

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        if (Request.UrlReferrer == null)
        {
            Response.Redirect("FEA_logout.aspx");
        }
        if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
        {
            Response.Redirect("FEA_logout.aspx");
        }

        if(!IsPostBack)
        {
            TextDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
        try
        {
            DateTime dt = Convert.ToDateTime(Request["dt"].ToString().Trim());
            TextDate.Text = dt.ToString("dd-MMM-yyyy");
            Button1_Click(null,null);
        }
        catch (Exception ex) { }


    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt1 = Convert.ToDateTime(TextDate.Text.Trim());
            DateTime dt2 = dt1.AddDays(1);

            string mystr = "<table width=98% cellspacing=1 border=1 style=border-collapse:collapse; cellpadding=5>";
            mystr = mystr + "<tr><td class=tablehead align=center width=10%>TSP</td><td class=tablehead align=center width=10%>LSA</td><td class=tablehead align=center width=10%>Tariff Type</td><td class=tablehead align=center width=10%>Price</td><td class=tablehead align=center width=10%>Unique ID</td><td class=tablehead align=center width=10%>Date - Taken on Record</td><td class=tablehead align=center>Tariff Summary</td><td class=tablehead align=center width=10%>Cancel</td></tr>";

            int cntr = 0;
            com = new MySqlCommand("select * from TRAI_tariffs where (checked='Yes') and (checkedon>='" + dt1.ToString("yyyy-MM-dd") + "' and checkedon<'" + dt2.ToString("yyyy-MM-dd") + "') order by checkedon desc", con);
            con.Open();
            dr = com.ExecuteReader();
            while(dr.Read())
            {
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell align=left valign=top>" + dr["oper"].ToString().Trim() + "</td>";
                mystr = mystr + "<td class=tablecell align=left valign=top>" + dr["circ"].ToString().Trim() + "</td>";
                mystr = mystr + "<td class=tablecell align=left valign=top>" + dr["ttype"].ToString().Trim() + "</td>";
                if (dr["mrp"].ToString().Trim() != "-1")
                {
                    mystr = mystr + "<td class=tablecell align=left valign=top>" + dr["mrp"].ToString().Trim().Replace("-1", "") + "</td>";
                }
                else
                {
                    mystr = mystr + "<td class=tablecell align=left valign=top>" + dr["ISP_rental"].ToString().Trim().Replace("-1", "") + "</td>";
                }
                mystr = mystr + "<td class=tablecell align=left valign=top>" + dr["uniqueid"].ToString().Trim() + "</td>";
                mystr = mystr + "<td class=tablecell align=left valign=top>" + Convert.ToDateTime(dr["checkedon"].ToString().Trim()).ToString("dd-MMM-yyyy") + "</td>";
                mystr = mystr + "<td class=tablecell align=left valign=top>" + dr["tariffdet"].ToString().Trim() + "</td>";
                //mystr = mystr + "<td class=tablecell align=center valign=top><a href='FEA_canceltaken.aspx?user=" + Request["user"].ToString().Trim() + "&uid=" + dr["uniqueid"].ToString().Trim() + "' class=indexlinks1><b><u>CANCEL</u></b></a></td>";
                mystr = mystr + "<td class=tablecell align=center valign=top><a href=javascript:funDel('" + dr["uniqueid"].ToString().Trim() + "') class=indexlinks1><b><u>CANCEL</u></b></a></td>";
                mystr = mystr + "</tr>";
                cntr++;
            }
            con.Close();

            mystr = mystr + "</table>";

            if(cntr>0)
            {
                divDetails.Visible = true;
                divresult.InnerHtml = mystr;
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }



    

    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            string UID = TextHidden.Text.Trim();

            string L5User = "";
            com = new MySqlCommand("select * from TRAI_FEA where(review='Level 5') order by rno limit 1", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            try
            {
                L5User = dr["uname"].ToString().Trim();
            }
            catch (Exception ex) { }
            con.Close();

            //com = new MySqlCommand("update TRAI_tariffs set checked='No',checkedby='',checkedon='2001-01-01',currstaff='',currstafflevel=0 where (uniqueid='" + UID + "')", con);
            com = new MySqlCommand("update TRAI_tariffs set checked='No',checkedby='',checkedon='2001-01-01',currstaff='" + L5User + "',currstafflevel=5 where (uniqueid='" + UID + "')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            com = new MySqlCommand("delete from TRAI_tariffreviewlog where (uniqueid='" + UID + "') and (actiontaken='Taken on Record')", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            Response.Redirect("FEA_canceltaken.aspx?user=" + Request["user"].ToString().Trim() + "&dt=" + TextDate.Text.Trim());

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




}