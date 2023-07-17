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


public partial class masterttypes : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights;
    int zno;
    double modrno;
    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        tablename = "TRAI_ttypes";

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

            Button1.Visible = false;
            Button2.Visible = false;

            com = new MySqlCommand("select * from TRAI_admin where(uname='" + Request["user"].ToString().Trim() + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            rights = dr["rights"].ToString().Trim();
            if (dr["rights"].ToString().Contains("TTypes_Add,") || dr["rights"].ToString().Trim() == "All")
            {
                Button1.Visible = true;
            }

            con.Close();


            if (!IsPostBack)
            {
                string css = "tablecell3";
                int rowcnt = 0;
                mystr = "<table width=100% cellspacing=1 cellpadding=5><tr><td class=tablehead>Tariff Product Type</td><td class=tablehead>Tariff Product Code</td><td class=tablehead>Edit</td><td class=tablehead>Delete</td></tr>";
                com = new MySqlCommand("select * from " + tablename + " order by rno", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (rowcnt % 2 == 0)
                    {
                        css = "tablecell3";
                    }
                    else
                    {
                        css = "tablecell3b";
                    }
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=" + css + " align=left>" + dr["ttype"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=" + css + " align=left>" + dr["tcode"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=" + css + " align=center><a href='masterttypes.aspx?user=" + Request["user"].ToString().Trim() + "&rno=" + dr["rno"].ToString().Trim() + "' class=indexlinks><img src='images/iconedit.jpg' width=20 border=0></a></td>";
                    if (rights.ToString().Contains("TTypes_Del,") || rights == "All")
                    {
                        mystr = mystr + "<td class=" + css + " align=center><a href='javascript:funDelRecord(" + dr["rno"].ToString().Trim() + ");' class=indexlinks><img src='images/icondelete.jpg' width=20 border=0></a></td>";
                    }
                    else
                    {
                        mystr = mystr + "<td class=" + css + " align=center></td>";
                    }
                    mystr = mystr + "</tr>";
                    rowcnt++;
                }
                con.Close();
                mystr = mystr + "</table>";

                divresults.InnerHtml = mystr;


                try
                {
                    modrno = Convert.ToInt32(Request["rno"].ToString().Trim());
                }
                catch (Exception ex)
                {
                    modrno = -1;
                }

                if (modrno > 0)
                {
                    com = new MySqlCommand("select * from " + tablename + " where(rno=" + modrno + ")", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    TextBox1.Text = dr["ttype"].ToString().Trim();
                    TextBox2.Text = dr["tcode"].ToString().Trim();
                    con.Close();


                }
            }


            try
            {
                modrno = Convert.ToInt32(Request["rno"].ToString().Trim());
            }
            catch (Exception ex)
            {
                modrno = -1;
            }
            if (modrno > 0)
            {
                Button1.Visible = false;

                if (rights.Contains("TTypes_Mod,") || rights == "All")
                {
                    Button2.Visible = true;
                }
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
            int flag = 0;
            string t1 = TextBox1.Text.Trim().Replace("'", "`");
            string t2 = TextBox2.Text.Trim().Replace("'", "`");
            if (t1 == "")
            {
                Response.Write("<script>alert('Please enter the Tariff Product Name');</script>");
                flag = 1;
            }
            if (t2 == "")
            {
                Response.Write("<script>alert('Please enter the Tariff Product Code');</script>");
                flag = 1;
            }

            if (flag == 0)
            {
                com = new MySqlCommand("select count(*) from " + tablename + " where(ttype='" + t1 + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
                {
                    flag = 1;
                    Response.Write("<script>alert('This Tariff Product Name already exists.');</script>");
                }
                con.Close();
                com = new MySqlCommand("select count(*) from " + tablename + " where(tcode='" + t2 + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
                {
                    flag = 1;
                    Response.Write("<script>alert('This Tariff Product Code already exists.');</script>");
                }
                con.Close();
            }

            if (flag == 0)
            {

                getMaxRno("rno", tablename);

                com = new MySqlCommand("insert into " + tablename + " values('" + zno + "','" + t1 + "','" + t2 + "')", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                Response.Redirect("masterttypes.aspx?user=" + Request["user"].ToString().Trim());

            }


        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }







    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;
            double recno = Convert.ToDouble(Request["rno"].ToString().Trim());
            string t1 = TextBox1.Text.Trim().Replace("'", "`");
            string t2 = TextBox2.Text.Trim().Replace("'", "`");
            string oldname = "";

            if (t1 == "")
            {
                Response.Write("<script>alert('Please enter the Tariff Product Name');</script>");
                flag = 1;
            }
            if (t2 == "")
            {
                Response.Write("<script>alert('Please enter the Tariff Product Code');</script>");
                flag = 1;
            }

            com = new MySqlCommand("select count(*) from " + tablename + " where(ttype='" + t1 + "') and (rno<>" + recno + ")", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
            {
                Response.Write("<script>alert('This Tariff Product Name already exists');</script>");
                flag = 1;
            }
            con.Close();
            com = new MySqlCommand("select count(*) from " + tablename + " where(tcode='" + t2 + "') and (rno<>" + recno + ")", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
            {
                Response.Write("<script>alert('This Tariff Product Code already exists');</script>");
                flag = 1;
            }
            con.Close();

            if (flag == 0)
            {

                com = new MySqlCommand("select * from " + tablename + " where(rno=" + recno + ")", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                oldname = dr["circ"].ToString().Trim();
                con.Close();

                com = new MySqlCommand("update " + tablename + " set ttype='" + t1 + "',tcode='" + t2 + "' where(rno=" + recno + ")", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                Response.Redirect("masterttypes.aspx?user=" + Request["user"].ToString().Trim());
            }


            if (rights.Contains("TTypes_Mod,") || rights == "All")
            {
                Button1.Visible = false;
                Button2.Visible = true;
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }






    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            double recno = Convert.ToDouble(TextDelNo.Text.Trim());
            int exists = 0;
            string oldname = "";

            com = new MySqlCommand("select * from " + tablename + " where(rno=" + recno + ")", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            oldname = dr["ttype"].ToString().Trim();
            con.Close();

            /*
            // Check if the selected value exists in any of the related tables, existing values are not to be deleted //

            com = new MySqlCommand("select count(*) from TRAI_operators where(oper='" + oldname + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            exists = Convert.ToInt32(dr[0].ToString().Trim());
            con.Close();

            // Checking of existing value code ends here //
            */

            if (exists == 0)
            {
                com = new MySqlCommand("delete from " + tablename + " where(rno=" + recno + ")", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                Response.Redirect("masterttypes.aspx?user=" + Request["user"].ToString().Trim());
            }
            else
            {
                Response.Write("<script>alert('Tariff records related to this entry are present in the database. Hence it cannot be deleted.');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
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
            Response.Write(ex.ToString().Trim());
        }

    }






    protected void ButtonClear_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("master_redirect.aspx?t1=masterttypes.aspx&user=" + Request["user"].ToString().Trim());
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }



}
