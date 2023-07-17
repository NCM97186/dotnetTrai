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

public partial class TSP_QuartBlack : System.Web.UI.Page
{
    MySqlCommand com, com1, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    int zno;
    //Table tb;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (Session["master"] == null)
            {
                //Response.Redirect("sessout.aspx");
            }

            con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

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
                DropCircle.Items.Add("");
                com = new MySqlCommand("select * from TRAI_circles order by circ", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    DropCircle.Items.Add(dr["circ"].ToString().Trim());
                }
                con.Close();

                int curryr = Convert.ToInt32(DateTime.Now.Year);
                DropYr.Items.Add("");
                for (int i = 2018; i <= curryr; i++)
                {
                    DropYr.Items.Add(i.ToString().Trim());
                }
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }




    protected void LoadData(object sender, EventArgs e)
    {
        try
        {
            string oper = "";
            string circ = DropCircle.SelectedItem.Text.Trim();
            string yr = DropYr.SelectedItem.Text.Trim();

            TextDate1.Text = "";
            TextDate2.Text = "";
            TextDate3.Text = "";
            TextDate4.Text = "";
            TextDate5.Text = "";
            TextOcc1.Text = "";
            TextOcc2.Text = "";
            TextOcc3.Text = "";
            TextOcc4.Text = "";
            TextOcc5.Text = "";
            TextWebLink.Text = "";

            if (circ != "" && yr != "")
            {
                com = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + Request["user"].ToString().Trim() + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                oper = dr["oper"].ToString().Trim();
                con.Close();

                int exists = 0;
                com = new MySqlCommand("select count(*) from TRAI_QuartBlackOut where (oper='" + oper + "') and (circ='" + circ + "') and (yr=" + Convert.ToInt32(yr) + ")", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                exists = Convert.ToInt32(dr[0].ToString().Trim());
                con.Close();

                if (exists > 0)
                {
                    com = new MySqlCommand("select * from TRAI_QuartBlackOut where (oper='" + oper + "') and (circ='" + circ + "') and (yr=" + Convert.ToInt32(yr) + ") order by rno", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        if (Convert.ToDateTime(dr["date1"].ToString().Trim()) > Convert.ToDateTime("1/1/2011"))
                        {
                            TextDate1.Text = Convert.ToDateTime(dr["date1"].ToString().Trim()).ToString("dd-MMM-yyyy");
                            TextOcc1.Text = dr["occ1"].ToString().Trim();
                        }
                        if (Convert.ToDateTime(dr["date2"].ToString().Trim()) > Convert.ToDateTime("1/1/2011"))
                        {
                            TextDate2.Text = Convert.ToDateTime(dr["date2"].ToString().Trim()).ToString("dd-MMM-yyyy");
                            TextOcc2.Text = dr["occ2"].ToString().Trim();
                        }
                        if (Convert.ToDateTime(dr["date3"].ToString().Trim()) > Convert.ToDateTime("1/1/2011"))
                        {
                            TextDate3.Text = Convert.ToDateTime(dr["date3"].ToString().Trim()).ToString("dd-MMM-yyyy");
                            TextOcc3.Text = dr["occ3"].ToString().Trim();
                        }
                        if (Convert.ToDateTime(dr["date4"].ToString().Trim()) > Convert.ToDateTime("1/1/2011"))
                        {
                            TextDate4.Text = Convert.ToDateTime(dr["date4"].ToString().Trim()).ToString("dd-MMM-yyyy");
                            TextOcc4.Text = dr["occ4"].ToString().Trim();
                        }
                        if (Convert.ToDateTime(dr["date5"].ToString().Trim()) > Convert.ToDateTime("1/1/2011"))
                        {
                            TextDate5.Text = Convert.ToDateTime(dr["date5"].ToString().Trim()).ToString("dd-MMM-yyyy");
                            TextOcc5.Text = dr["occ5"].ToString().Trim();
                        }

                        TextWebLink.Text = "";
                    }
                    con.Close();

                    TextDate1.Enabled = false;
                    TextDate2.Enabled = false;
                    TextDate3.Enabled = false;
                    TextDate4.Enabled = false;
                    TextDate5.Enabled = false;
                    TextOcc1.Enabled = false;
                    TextOcc2.Enabled = false;
                    TextOcc3.Enabled = false;
                    TextOcc4.Enabled = false;
                    TextOcc5.Enabled = false;
                    TextWebLink.Enabled = false;
                    Button2.Visible = false;
                }
                else
                {
                    TextDate1.Text = "";
                    TextDate2.Text = "";
                    TextDate3.Text = "";
                    TextDate4.Text = "";
                    TextDate5.Text = "";
                    TextOcc1.Text = "";
                    TextOcc2.Text = "";
                    TextOcc3.Text = "";
                    TextOcc4.Text = "";
                    TextOcc5.Text = "";
                    TextWebLink.Text = "";

                    TextDate1.Enabled = true;
                    TextDate2.Enabled = true;
                    TextDate3.Enabled = true;
                    TextDate4.Enabled = true;
                    TextDate5.Enabled = true;
                    TextOcc1.Enabled = true;
                    TextOcc2.Enabled = true;
                    TextOcc3.Enabled = true;
                    TextOcc4.Enabled = true;
                    TextOcc5.Enabled = true;
                    TextWebLink.Enabled = true;

                    Button2.Visible = true;
                }
                
                tableData.Visible = true;
                TdSubmit.Visible = true;
                trNote.Visible = true;

         
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
            int flag = 0;
            
            if (DropCircle.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a LSA');</script>");
            }
            if (DropYr.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a Calendar Year');</script>");
            }

            LoadData(null, null);

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
            int flag = 0;
            DateTime dt = Convert.ToDateTime(Convert.ToDateTime("1/1/2011").ToString("yyyy-MM-dd HH:mm:ss"));
            DateTime dt2 = dt;
            DateTime dt3 = dt;
            DateTime dt4 = dt;
            DateTime dt5 = dt;

            if (DropCircle.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a LSA');</script>");
            }
            if (DropYr.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a Calendar Year');</script>");
            }

            if (flag == 0)
            {
                string oper = "";
                string circ = DropCircle.SelectedItem.Text.Trim();
                string yr = DropYr.SelectedItem.Text.Trim();

                com = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + Request["user"].ToString().Trim() + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                oper = dr["oper"].ToString().Trim();
                con.Close();

                /*
                com = new MySqlCommand("select * from TRAI_QuartBlackOut where(oper='" + oper + "') and (circ='" + circ + "') and (yr=" + Convert.ToInt32(yr) + ") order by rno", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                try
                {
                    DateTime olddate = Convert.ToDateTime(dr["recdate"].ToString().Trim());
                    if (DateTime.Now > olddate.AddHours(48))
                    {
                        flag = 1;
                        Response.Write("<script>alert('Editing is only allowed upto 48 hours after entry.');</script>");
                    }
                }
                catch (Exception ex) { }
                con.Close();
                */

                if((TextDate1.Text.Trim()!="" && TextOcc1.Text.Trim()=="") || (TextDate2.Text.Trim()!="" && TextOcc2.Text.Trim()=="") || (TextDate3.Text.Trim()!="" && TextOcc3.Text.Trim()=="") || (TextDate4.Text.Trim()!="" && TextOcc4.Text.Trim()=="") || (TextDate5.Text.Trim()!="" && TextOcc5.Text.Trim()==""))
                {
                    flag = 1;
                    Response.Write("<script>alert('Please enter the occasion for each of the entered dates');</script>");
                }

                if(TextWebLink.Text.Trim()=="")
                {
                    flag = 1;
                    Response.Write("<script>alert('Please enter the weblink');</script>");
                }


                    if (flag == 0)
                    {
                        try
                        {
                            dt = Convert.ToDateTime(TextDate1.Text.Trim());
                        }
                        catch (Exception ex) { }

                        try
                        {
                            dt2 = Convert.ToDateTime(TextDate2.Text.Trim());
                        }
                        catch (Exception ex) { }

                        try
                        {
                            dt3 = Convert.ToDateTime(TextDate3.Text.Trim());
                        }
                        catch (Exception ex) { }

                        try
                        {
                            dt4 = Convert.ToDateTime(TextDate4.Text.Trim());
                        }
                        catch (Exception ex) { }

                        try
                        {
                            dt5 = Convert.ToDateTime(TextDate5.Text.Trim());
                        }
                        catch (Exception ex) { }

                        // insert current data, if any, into archive table //
                        getMaxRno("relno", "TRAI_QuartBlackOut_archive");
                        int relno = zno;
                        com = new MySqlCommand("select * from TRAI_QuartBlackOut where(oper='" + oper + "') and (circ='" + circ + "') and (yr=" + Convert.ToInt32(yr) + ") order by rno", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        while (dr.Read())
                        {
                            getMaxRno("rno", "TRAI_QuartBlackOut_archive");
                            com1 = new MySqlCommand("insert into TRAI_QuartBlackOut_archive values('" + zno + "','" + Convert.ToDateTime(dr["recdate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dr["uname"].ToString() + "','" + dr["oper"].ToString() + "','" + dr["circ"].ToString() + "','" + dr["yr"].ToString() + "','" + Convert.ToDateTime(dr["date1"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dr["occ1"].ToString().Trim() + "','" + Convert.ToDateTime(dr["date2"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dr["occ2"].ToString().Trim() + "','" + Convert.ToDateTime(dr["date3"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dr["occ3"].ToString().Trim() + "','" + Convert.ToDateTime(dr["date4"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dr["occ4"].ToString().Trim() + "','" + Convert.ToDateTime(dr["date5"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dr["occ5"].ToString().Trim() + "','" + TextWebLink.Text.Trim().Replace("'", "`") + "','" + relno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')", con1);
                            con1.Open();
                            com1.ExecuteNonQuery();
                            con1.Close();

                        }
                        con.Close();

                        // delete current data, if any
                        com = new MySqlCommand("delete from TRAI_QuartBlackOut where(oper='" + oper + "') and (circ='" + circ + "') and (yr=" + Convert.ToInt32(yr) + ")", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                        getMaxRno("rno", "TRAI_QuartBlackOut");

                        com = new MySqlCommand("insert into TRAI_QuartBlackOut values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + Request["user"].ToString().Trim() + "','" + oper + "','" + circ + "','" + yr + "','" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + TextOcc1.Text.Trim().Replace("'", "`") + "','" + dt2.ToString("yyyy-MM-dd HH:mm:ss") + "','" + TextOcc2.Text.Trim().Replace("'", "`") + "','" + dt3.ToString("yyyy-MM-dd HH:mm:ss") + "','" + TextOcc3.Text.Trim().Replace("'", "`") + "','" + dt4.ToString("yyyy-MM-dd HH:mm:ss") + "','" + TextOcc4.Text.Trim().Replace("'", "`") + "','" + dt5.ToString("yyyy-MM-dd HH:mm:ss") + "','" + TextOcc5.Text.Trim().Replace("'", "`") + "','" + TextWebLink.Text.Trim().Replace("'","`") + "')", con);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                        TextDate1.Text = "";
                        TextDate2.Text = "";
                        TextDate3.Text = "";
                        TextDate4.Text = "";
                        TextDate5.Text = "";
                        TextOcc1.Text = "";
                        TextOcc2.Text = "";
                        TextOcc3.Text = "";
                        TextOcc4.Text = "";
                        TextOcc5.Text = "";
                        TextWebLink.Text = "";

                        DropCircle.Text = "";
                        DropYr.Text = "";

                        tableData.Visible = false;
                        Response.Write("<script>alert('Data has been stored successfully.');</script>");
                    }
            }

            Button2.Attributes.Clear();
        }
        catch (Exception ex)
        {
            Button2.Attributes.Clear();
            Response.Write(ex.ToString());
        }
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





    protected void TextDate1_PreRender(object s, EventArgs e)
    {
        TextDate1.Attributes.Add("onfocus", "showCalender(calender,this)");
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

    protected void TextDate5_PreRender(object s, EventArgs e)
    {
        TextDate5.Attributes.Add("onfocus", "showCalender(calender,this)");
    }



}