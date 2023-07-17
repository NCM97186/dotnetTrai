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



public partial class masterusers : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string rights, uname, pass, t1, email, myrecords;
    double rno;
    int zno;

    protected void Page_Load(object sender, EventArgs e)
    {
        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        if (Request.UrlReferrer == null)
        {
            Response.Redirect("logout.aspx");
        }
        if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
        {
            Response.Redirect("logout.aspx");
        }

        Button1.Visible = true;
        ImageButton1.Visible = false;

        if (Session["msg"] == "added")
        {
            Response.Write("<script>alert('Record has been added successfully');</script>");
        }
        if (Session["msg"] == "updated")
        {
            Response.Write("<script>alert('Record has been updated successfully');</script>");
        }
        if (Session["msg"] == "deleted")
        {
            Response.Write("<script>alert('Record has been deleted successfully');</script>");
        }
        Session["msg"] = "";

        try
        {
            t1 = Request["t1"].ToString().Trim();
        }
        catch (Exception ex)
        {
            t1 = "";
        }

        com = new MySqlCommand("select * from TRAI_admin where(uname='" + Request["user"].ToString().Trim() + "')", con);
        con.Open();
        dr = com.ExecuteReader();
        dr.Read();
        string userrights = dr["rights"].ToString().Trim();
        con.Close();

        if (t1 == "")
        {

        }
        else
        {

            if (!IsPostBack)
            {
                blankall(null, null);

                Button1.Visible = false;
                ImageButton1.Visible = true;


                com = new MySqlCommand("select * from TRAI_admin where(uname='" + t1 + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                TextBox1.Text = dr["uname"].ToString().Trim();
                TextBox3.Enabled = false;
                TextHidden.Text = dr["uname"].ToString().Trim();
                TextBox2.Text = dr["pass"].ToString().Trim();
                TextBox3.Text = dr["uname"].ToString().Trim();
                TextBox4.Text = dr["email"].ToString().Trim();



                if (dr["rights"].ToString().Trim().Contains("masterusers_Add,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masterusers_Add.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("masterusers_Mod,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masterusers_Mod.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("masterusers_Del,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masterusers_Del.Checked = true;
                }

                if (dr["rights"].ToString().Trim().Contains("masteroperators_Add,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masteroperators_Add.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("masteroperators_Mod,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masteroperators_Mod.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("masteroperators_Del,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masteroperators_Del.Checked = true;
                }

                if (dr["rights"].ToString().Trim().Contains("mastercircles_Add,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    mastercircles_Add.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("mastercircles_Mod,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    mastercircles_Mod.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("mastercircles_Del,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    mastercircles_Del.Checked = true;
                }

                if (dr["rights"].ToString().Trim().Contains("masterTSPLogins_Add,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masterTSPLogins_Add.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("masterTSPLogins_Mod,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masterTSPLogins_Mod.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("masterTSPLogins_Del,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    masterTSPLogins_Del.Checked = true;
                }

                if (dr["rights"].ToString().Trim().Contains("FEALogins_Add,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    FEALogins_Add.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("FEALogins_Mod,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    FEALogins_Mod.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("FEALogins_Del,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    FEALogins_Del.Checked = true;
                }

                if (dr["rights"].ToString().Trim().Contains("TTypes_Add,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    TTypes_Add.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("TTypes_Mod,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    TTypes_Mod.Checked = true;
                }
                if (dr["rights"].ToString().Trim().Contains("TTypes_Del,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    TTypes_Del.Checked = true;
                }


                if (dr["rights"].ToString().Trim().Contains("FDAmount,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    FDAmount.Checked = true;
                }




                if (dr["rights"].ToString().Trim().Contains("ReportFeedbacks,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    ReportFeedbacks.Checked = true;
                }

                if (dr["rights"].ToString().Trim().Contains("ReportDownloads,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    ReportDownloads.Checked = true;
                }

                if (dr["rights"].ToString().Trim().Contains("ReportHitCounter,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    ReportHitCounter.Checked = true;
                }

                if (dr["rights"].ToString().Trim().Contains("ReportTariffCount,") || dr["rights"].ToString().Trim().ToUpper() == "ALL")
                {
                    ReportTariffCount.Checked = true;
                }
                
                

                con.Close();
            }
        }



        myrecords = "<table width=90% cellspacing=1 cellpadding=5><tr>";
        myrecords = myrecords + "<td width=30% class=tablehead align=center>Username</td>";
        myrecords = myrecords + "<td width=20% class=tablehead align=center>Edit</td>";
        myrecords = myrecords + "<td width=20% class=tablehead align=center>Delete</td>";
        myrecords = myrecords + "</tr>";

        string css = "tablecell3";
        int rowcnt = 0;
        com = new MySqlCommand("select * from TRAI_admin order by uname", con);
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
            myrecords = myrecords + "<tr>";
            myrecords = myrecords + "<td class=" + css + " align=left>" + dr["uname"].ToString().Trim() + "</td>";
            myrecords = myrecords + "<td class=" + css + " align=center><a href='masterusers.aspx?user=" + Request["user"].ToString().Trim() + "&t1=" + dr["uname"].ToString().Trim() + "' class=indexlinks><img src=images/iconedit.jpg width=25 border=0></a></td>";
            myrecords = myrecords + "<td class=" + css + " align=center><a href=javascript:funDelRecord('" + dr["uname"].ToString().Trim() + "'); class=indexlinks><img src='images/icondelete.jpg' width=20 border=0></a></td>";
            myrecords = myrecords + "</tr>";
            rowcnt++;
        }
        con.Close();
        myrecords = myrecords + "</table>";

        divresults.InnerHtml = myrecords;


    }





    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            uname = TextBox3.Text.Trim().Replace("'", "`");
            pass = TextBox2.Text.Trim().Replace("'", "`");
            email = TextBox4.Text.Trim().Replace("'", "`");
            if (uname == "" || pass == "" || !email.Contains("@"))
            {
                Response.Write("<script>alert('Please enter valid values in all mandatory fields');</script>");
            }
            else
            {
                com = new MySqlCommand("select count(*) from TRAI_admin where(uname='" + uname + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                int exists = Convert.ToInt32(dr[0].ToString().Trim());
                con.Close();

                if (exists > 0)
                {
                    Response.Write("<script>alert('This username already exists');</script>");
                }
                else
                {

                    rights = "";
                    funrights(null, null);

                    com = new MySqlCommand("select count(*) from TRAI_admin", con1);
                    con1.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    if (Convert.ToInt32(dr[0].ToString()) > 0)
                    {
                        com = new MySqlCommand("select max(rno) from TRAI_admin", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        dr.Read();
                        rno = Convert.ToInt32(dr[0].ToString());
                        rno = rno + 1;
                        con.Close();
                    }
                    else
                    {
                        rno = 1;
                    }
                    con1.Close();

                    com = new MySqlCommand("insert into TRAI_admin values('" + rno + "','" + uname + "','" + pass + "','" + rights + "','" + email + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();


                    Session["msg"] = "added";
                    Response.Redirect("masterusers.aspx?user=" + Request["user"].ToString().Trim());

                }
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
            uname = TextBox3.Text.Trim().Replace("'", "`");
            pass = TextBox2.Text.Trim().Replace("'", "`");
            email = TextBox4.Text.Trim().Replace("'", "`");
            if (uname == "" || pass == "" || !email.Contains("@"))
            {
                Response.Write("<script>alert('Please enter valid values in all mandatory fields');</script>");
            }
            else
            {
                if (TextHidden.Text.Trim().ToLower() == "admin")
                {
                    com = new MySqlCommand("update TRAI_admin set pass='" + pass + "',email='" + email + "' where(uname='" + TextHidden.Text.Trim() + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
                else
                {

                    rights = "";
                    funrights(null, null);

                    com = new MySqlCommand("update TRAI_admin set uname='" + uname + "',pass='" + pass + "',rights='" + rights + "',email='" + email + "' where(uname='" + TextHidden.Text.Trim() + "')", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }


                Session["msg"] = "updated";
                Response.Redirect("masterusers.aspx?user=" + Request["user"].ToString().Trim());
            }


            try
            {
                t1 = Request["t1"].ToString().Trim();
            }
            catch (Exception ex)
            {
                t1 = "";
            }
            if(t1!="")
            {
                Button1.Visible = false;
                ImageButton1.Visible = true;
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }



    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            if (TextHidden.Text.Trim().ToLower() != "admin")
            {
                com = new MySqlCommand("delete from TRAI_admin where(uname='" + TextHidden.Text.Trim() + "')", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                Session["msg"] = "deleted";
                Response.Redirect("masterusers.aspx?user=" + Request["user"].ToString().Trim());
            }
            else
            {
                Response.Write("<script>alert('The master admin cannot be deleted');</script>");
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }






    protected void ButtonClear_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("master_redirect.aspx?t1=masterusers.aspx&user=" + Request["user"].ToString().Trim());
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }




    protected void funrights(object sender, EventArgs e)
    {
        try
        {

            if (masterusers_Add.Checked == true)
            {
                rights = rights + "masterusers_Add" + ",";
            }
            if (masterusers_Mod.Checked == true)
            {
                rights = rights + "masterusers_Mod" + ",";
            }
            if (masterusers_Del.Checked == true)
            {
                rights = rights + "masterusers_Del" + ",";
            }

            if (masteroperators_Add.Checked == true)
            {
                rights = rights + "masteroperators_Add" + ",";
            }
            if (masteroperators_Mod.Checked == true)
            {
                rights = rights + "masteroperators_Mod" + ",";
            }
            if (masteroperators_Del.Checked == true)
            {
                rights = rights + "masteroperators_Del" + ",";
            }

            if (mastercircles_Add.Checked == true)
            {
                rights = rights + "mastercircles_Add" + ",";
            }
            if (mastercircles_Mod.Checked == true)
            {
                rights = rights + "mastercircles_Mod" + ",";
            }
            if (mastercircles_Del.Checked == true)
            {
                rights = rights + "mastercircles_Del" + ",";
            }

            if (masterTSPLogins_Add.Checked == true)
            {
                rights = rights + "masterTSPLogins_Add" + ",";
            }
            if (masterTSPLogins_Mod.Checked == true)
            {
                rights = rights + "masterTSPLogins_Mod" + ",";
            }
            if (masterTSPLogins_Del.Checked == true)
            {
                rights = rights + "masterTSPLogins_Del" + ",";
            }

            if (FEALogins_Add.Checked == true)
            {
                rights = rights + "FEALogins_Add" + ",";
            }
            if (FEALogins_Mod.Checked == true)
            {
                rights = rights + "FEALogins_Mod" + ",";
            }
            if (FEALogins_Del.Checked == true)
            {
                rights = rights + "FEALogins_Del" + ",";
            }

            if (TTypes_Add.Checked == true)
            {
                rights = rights + "TTypes_Add" + ",";
            }
            if (TTypes_Mod.Checked == true)
            {
                rights = rights + "TTypes_Mod" + ",";
            }
            if (TTypes_Del.Checked == true)
            {
                rights = rights + "TTypes_Del" + ",";
            }

            if (FDAmount.Checked == true)
            {
                rights = rights + "FDAmount" + ",";
            }

            

            if (ReportFeedbacks.Checked == true)
            {
                rights = rights + "ReportFeedbacks" + ",";
            }
            if (ReportDownloads.Checked == true)
            {
                rights = rights + "ReportDownloads" + ",";
            }
            if (ReportHitCounter.Checked == true)
            {
                rights = rights + "ReportHitCounter" + ",";
            }
            if (ReportTariffCount.Checked == true)
            {
                rights = rights + "ReportTariffCount" + ",";
            }
            
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }



    protected void blankall(object sender, EventArgs e)
    {
        masterusers_Add.Checked = false;
        masterusers_Mod.Checked = false;
        masterusers_Del.Checked = false;

        masteroperators_Add.Checked = false;
        masteroperators_Mod.Checked = false;
        masteroperators_Del.Checked = false;

        mastercircles_Add.Checked = false;
        mastercircles_Mod.Checked = false;
        mastercircles_Del.Checked = false;

        masterTSPLogins_Add.Checked = false;
        masterTSPLogins_Mod.Checked = false;
        masterTSPLogins_Del.Checked = false;

        FEALogins_Add.Checked = false;
        FEALogins_Mod.Checked = false;
        FEALogins_Del.Checked = false;

        TTypes_Add.Checked = false;
        TTypes_Mod.Checked = false;
        TTypes_Del.Checked = false;

        FDAmount.Checked = false;



        ReportFeedbacks.Checked = false;
        ReportDownloads.Checked = false;
        ReportHitCounter.Checked = false;
        ReportTariffCount.Checked = false;
    }







    protected void getMaxRno(string myfield, string mytable)
    {
        try
        {

            com9 = new MySqlCommand("select count(*) from " + mytable, con9);
            con9.Open();
            dr9 = com9.ExecuteReader();
            dr9.Read();
            if (Convert.ToInt32(dr9[0].ToString()) > 0)
            {
                com10 = new MySqlCommand("select max(" + myfield + ") from " + mytable, con10);
                con10.Open();
                dr10 = com10.ExecuteReader();
                dr10.Read();
                zno = Convert.ToInt32(dr10[0].ToString());
                zno = zno + 1;
                con10.Close();
            }
            else
            {
                zno = 1;
            }
            con9.Close();

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }





}
