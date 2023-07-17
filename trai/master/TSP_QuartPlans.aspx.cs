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

public partial class TSP_QuartPlans : System.Web.UI.Page
{
    MySqlCommand com, com1, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    int zno;
    //Table tb;
    TextBox[] arrPlan, arrSubs, arrLdate, arrWdate;
    TextBox t1, t2, t3, t4;
    int arrSize, cntr;
    

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


            arrSize = 5000;
            TdData.Controls.Clear();
            divHidden.Controls.Clear();

            arrPlan = new TextBox[arrSize];
            arrSubs = new TextBox[arrSize];
            arrLdate = new TextBox[arrSize];
            arrWdate = new TextBox[arrSize];

            for (int i = 0; i < arrSize; i++)
            {
                t1 = new TextBox();
                t1.ID = "P" + i.ToString().Trim();
                t1.Width = 250;
                t1.CssClass = "input";
                t1.Enabled = false;
                arrPlan[i] = t1;
                t2 = new TextBox();
                t2.ID = "S" + i.ToString().Trim();
                t2.Width = 100;
                t2.CssClass = "input";
                arrSubs[i] = t2;
                t3 = new TextBox();
                t3.ID = "L" + i.ToString().Trim();
                t3.Width = 120;
                t3.CssClass = "input";
                t3.Enabled = false;
                arrLdate[i] = t3;
                t4 = new TextBox();
                t4.ID = "W" + i.ToString().Trim();
                t4.Width = 120;
                t4.CssClass = "input";
                t4.Enabled = false;
                arrWdate[i] = t4;

                divHidden.Controls.Add(arrPlan[i]);
                divHidden.Controls.Add(arrSubs[i]);
                divHidden.Controls.Add(arrLdate[i]);
                divHidden.Controls.Add(arrWdate[i]);
            }

            int counthidden = 0;
            try
            {
                counthidden = Convert.ToInt32(TextHidden.Text.Trim());
            }
            catch (Exception ex) { }


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

                DropQtr.Items.Add("");
                DropQtr.Items.Add("March");
                DropQtr.Items.Add("June");
                DropQtr.Items.Add("September");
                DropQtr.Items.Add("December");

                int curryr = Convert.ToInt32(DateTime.Now.Year);
                DropYr.Items.Add("");
                for (int i = 2019; i <= curryr; i++)
                {
                    DropYr.Items.Add(i.ToString().Trim());
                }
            }

            LoadData(null, null);

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
            TdData.Controls.Clear();
            divHidden.Controls.Clear();

            int flag = 0;

            if (RadPrePost.SelectedIndex == -1)
            {
                flag = 1;
                Response.Write("<script>alert('Please select Prepaid or Postpaid');</script>");
            }
            if (DropCircle.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a LSA');</script>");
            }
            if (DropQtr.SelectedItem.Text.Trim() == "" || DropYr.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a Quarter and Year');</script>");
            }


            if(flag==0)
            {
                string oper = "";
                string prepost = "";
                try
                {
                    prepost = RadPrePost.SelectedItem.Text.Trim();
                }
                catch (Exception ex) { }
                string circ = DropCircle.SelectedItem.Text.Trim();
                string qtr = DropQtr.SelectedItem.Text.Trim();
                string yr = DropYr.SelectedItem.Text.Trim();

                if (prepost != "" && circ != "" && qtr != "" && yr != "")
                {
                    com = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + Request["user"].ToString().Trim() + "')", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    oper = dr["oper"].ToString().Trim();
                    con.Close();

                    Table1.Visible = true;

                    LoadData(null,null);
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
            int flag = 0;

            int counthidden = 0;
            try
            {
                counthidden = Convert.ToInt32(TextHidden.Text.Trim());
            }
            catch (Exception ex) { }


            for (int i = 0; i < counthidden; i++)
            {
                if (arrSubs[i].Text.Trim() != "")
                {

                }
                else
                {
                    flag = 1;
                    //Response.Write("<script>alert('Please enter all fields for row no. " + (i + 1).ToString().Trim() + "');<script>");
                }
                try
                {
                    double tempval = Convert.ToDouble(arrSubs[i].Text.Trim());
                }
                catch (Exception ex)
                {
                    flag = 1;
                }
                
            }

            if (flag == 1)
            {
                Response.Write("<script>alert('Please fill in subscriber number for all plans. Partial filling is not allowed, and subscribers should be numeric only.');</script>");
                //Button1_Click(null, null);
            }

            if (RadPrePost.SelectedIndex == -1)
            {
                flag = 1;
                Response.Write("<script>alert('Please select Prepaid or Postpaid');</script>");
            }
            if (DropCircle.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a LSA');</script>");
            }
            if (DropQtr.SelectedItem.Text.Trim() == "" || DropYr.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a Quarter and Year');</script>");
            }
            if ((DropQtr.SelectedItem.Text.Trim() == "March" && DropYr.SelectedItem.Text.Trim() == "2018") || (DropQtr.SelectedItem.Text.Trim() == "June" && DropYr.SelectedItem.Text.Trim() == "2018") || (DropQtr.SelectedItem.Text.Trim() == "September" && DropYr.SelectedItem.Text.Trim() == "2018"))
            {
                flag = 1;
                Response.Write("<script>alert('Data Prior to quarter ending Dec. 2018 cannot be submitted online.');</script>");
            }
            
            if (flag == 0)
            {
                string oper = "";
                string prepost = RadPrePost.SelectedItem.Text.Trim();
                string circ = DropCircle.SelectedItem.Text.Trim();
                string qtr = DropQtr.SelectedItem.Text.Trim();
                string yr = DropYr.SelectedItem.Text.Trim();

                com = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + Request["user"].ToString().Trim() + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                oper = dr["oper"].ToString().Trim();
                con.Close();

                com = new MySqlCommand("select * from TRAI_QuartNotOnOffer where(oper='" + oper + "') and (prepost='" + prepost + "') and (circ='" + circ + "') and (qtr='" + qtr + "') and (yr=" + Convert.ToInt32(yr) + ") order by rno", con);
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

                if (flag == 0)
                {
                    // insert current data, if any, into archive table //
                    getMaxRno("relno", "TRAI_QuartNotOnOffer_archive");
                    int relno = zno;
                    com = new MySqlCommand("select * from TRAI_QuartNotOnOffer where(oper='" + oper + "') and (prepost='" + prepost + "') and (circ='" + circ + "') and (qtr='" + qtr + "') and (yr=" + Convert.ToInt32(yr) + ") order by rno", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        getMaxRno("rno", "TRAI_QuartNotOnOffer_archive");
                        com1 = new MySqlCommand("insert into TRAI_QuartNotOnOffer_archive values('" + zno + "','" + Convert.ToDateTime(dr["recdate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dr["uname"].ToString() + "','" + dr["oper"].ToString() + "','" + dr["prepost"].ToString() + "','" + dr["circ"].ToString() + "','" + dr["qtr"].ToString() + "','" + dr["yr"].ToString() + "','" + dr["planname"].ToString() + "','" + dr["subscribers"].ToString() + "','" + dr["ldate"].ToString() + "','" + dr["wdate"].ToString() + "','" + relno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')", con1);
                        con1.Open();
                        com1.ExecuteNonQuery();
                        con1.Close();

                    }
                    con.Close();

                    // delete current data, if any
                    com = new MySqlCommand("delete from TRAI_QuartNotOnOffer where(oper='" + oper + "') and (prepost='" + prepost + "') and (circ='" + circ + "') and (qtr='" + qtr + "') and (yr=" + Convert.ToInt32(yr) + ")", con);
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();

                    for (int i = 0; i < counthidden; i++)
                    {
                        if (arrSubs[i].Text.Trim() != "")
                        {
                            getMaxRno("rno", "TRAI_QuartNotOnOffer");

                            com = new MySqlCommand("insert into TRAI_QuartNotOnOffer values('" + zno + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + Request["user"].ToString().Trim() + "','" + oper + "','" + prepost + "','" + circ + "','" + qtr + "','" + yr + "','" + arrPlan[i].Text.Trim().Replace("'", "`") + "','" + arrSubs[i].Text.Trim().Replace("'", "`") + "','" + Convert.ToDateTime(arrLdate[i].Text.Trim().Replace("'", "`")).ToString("yyyy-MM-dd HH:mm:ss") + "','" + Convert.ToDateTime(arrWdate[i].Text.Trim().Replace("'", "`")).ToString("yyyy-MM-dd HH:mm:ss") + "')", con);
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    for (int i = 0; i < counthidden; i++)
                    {
                        arrPlan[i].Text = "";
                        arrSubs[i].Text = "";
                        arrLdate[i].Text = "";
                        arrWdate[i].Text = "";
                    }
                    RadPrePost.SelectedIndex = -1;
                    DropCircle.Text = "";
                    DropQtr.Text = "";
                    DropYr.Text = "";
                    Response.Write("<script>alert('Data has been stored successfully.');</script>");
                    TdData.Visible = false;
                    TdSubmit.Visible = false;
                    trNote.Visible = false;
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






    protected void LoadData(object sender, EventArgs e)
    {
        try
        {
            string oper = "";
            string prepost = "";
            try
            {
                prepost = RadPrePost.SelectedItem.Text.Trim();
            }
            catch (Exception ex) { }
            string circ = DropCircle.SelectedItem.Text.Trim();
            string qtr = DropQtr.SelectedItem.Text.Trim();
            string yr = DropYr.SelectedItem.Text.Trim();

            if (prepost != "" && circ != "" && qtr != "" && yr != "")
            {

                for (int i = 0; i < arrSize; i++)
                {
                    arrPlan[i].Text = "";
                    arrSubs[i].Text = "";
                    arrLdate[i].Text = "";
                    arrWdate[i].Text = "";
                }

                com = new MySqlCommand("select * from TRAI_TSPUsers where(uname='" + Request["user"].ToString().Trim() + "')", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                oper = dr["oper"].ToString().Trim();
                con.Close();

                int exists = 0;
                com = new MySqlCommand("select count(*) from TRAI_QuartNotOnOffer where (oper='" + oper + "') and (prepost='" + prepost + "') and (circ='" + circ + "') and (qtr='" + qtr + "') and (yr=" + Convert.ToInt32(yr) + ")", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                exists = Convert.ToInt32(dr[0].ToString().Trim());
                con.Close();

                Table tb = new Table();
                tb.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                tb.CellPadding = 5;
                tb.CellSpacing = 1;
                tb.BorderWidth = 0;

                TableRow trr1 = new TableRow();
                TableCell tcc1 = new TableCell();
                TableCell tcc2 = new TableCell();
                TableCell tcc2b = new TableCell();
                TableCell tcc3 = new TableCell();
                TableCell tcc4 = new TableCell();
                tcc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tcc1.Width = System.Web.UI.WebControls.Unit.Percentage(5);
                tcc2.Width = System.Web.UI.WebControls.Unit.Percentage(30);
                tcc2b.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                tcc3.Width = System.Web.UI.WebControls.Unit.Percentage(15);
                tcc4.Width = System.Web.UI.WebControls.Unit.Percentage(40);
                tcc1.CssClass = "tablehead";
                tcc2.CssClass = "tablehead";
                tcc2b.CssClass = "tablehead";
                tcc3.CssClass = "tablehead";
                tcc4.CssClass = "tablehead";
                tcc1.Text = "S.No.";
                tcc2.Text = "Plan Name";
                tcc2b.Text = "Number of subscriber (as on last day of quarter)";
                tcc3.Text = "Launch date of plan";
                tcc4.Text = "Withdrawal date of plan";
                trr1.Controls.Add(tcc1);
                trr1.Controls.Add(tcc2);
                trr1.Controls.Add(tcc2b);
                trr1.Controls.Add(tcc3);
                trr1.Controls.Add(tcc4);
                tb.Controls.Add(trr1);

                int cntr = 0;
                int rows1 = 0;

                if (exists > 0)
                {
                    com = new MySqlCommand("select * from TRAI_QuartNotOnOffer where (oper='" + oper + "') and (prepost='" + prepost + "') and (circ='" + circ + "') and (qtr='" + qtr + "') and (yr=" + Convert.ToInt32(yr) + ") order by rno", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();
                        TableCell tc2b = new TableCell();
                        TableCell tc3 = new TableCell();
                        TableCell tc4 = new TableCell();
                        tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                        tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tc2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        string css = "tablecell3";
                        if (rows1 % 2 == 0)
                        {
                            css = "tablecell3b";
                        }
                        tc1.CssClass = css;
                        tc2.CssClass = css;
                        tc2b.CssClass = css;
                        tc3.CssClass = css;
                        tc4.CssClass = css;

                        arrPlan[cntr].Text = dr["plan"].ToString().Trim();
                        arrSubs[cntr].Text = dr["subscribers"].ToString().Trim();
                        arrLdate[cntr].Text = Convert.ToDateTime(dr["ldate"].ToString().Trim()).ToString("dd-MMM-yyyy");
                        arrWdate[cntr].Text = Convert.ToDateTime(dr["wdate"].ToString().Trim()).ToString("dd-MMM-yyyy");

                        arrSubs[cntr].Enabled = false;
                        
                        Button2.Visible = false;
                        trNote.Visible = false;

                        tc1.Text = (cntr + 1).ToString();
                        tc2.Controls.Add(arrPlan[cntr]);
                        tc2b.Controls.Add(arrSubs[cntr]);
                        tc3.Controls.Add(arrLdate[cntr]);
                        tc4.Controls.Add(arrWdate[cntr]);
                        tr.Controls.Add(tc1);
                        tr.Controls.Add(tc2);
                        tr.Controls.Add(tc2b);
                        tr.Controls.Add(tc3);
                        tr.Controls.Add(tc4);
                        tb.Controls.Add(tr);

                        cntr++;
                        rows1++;
                    }
                    con.Close();
                }
                else
                {
                    string ttypes = "";
                    string archiveqry = "";

                    /*
                    if (prepost == "Prepaid")
                    {
                        ttypes = " and (upper(ttype)='PREPAID PLAN VOUCHER')";
                    }
                    if (prepost == "Postpaid")
                    {
                        ttypes = " and (upper(ttype)='POSTPAID PLAN')";
                    }
                    */

                    DateTime dt1 = Convert.ToDateTime("2011/1/1");
                    DateTime dt2 = Convert.ToDateTime("2011/1/1");

                    if (qtr == "March")
                    {
                        dt1 = Convert.ToDateTime(yr + "/1/1");
                        dt2 = Convert.ToDateTime(yr + "/4/1");
                    }
                    if (qtr == "June")
                    {
                        dt1 = Convert.ToDateTime(yr + "/4/1");
                        dt2 = Convert.ToDateTime(yr + "/7/1");
                    }
                    if (qtr == "September")
                    {
                        dt1 = Convert.ToDateTime(yr + "/7/1");
                        dt2 = Convert.ToDateTime(yr + "/10/1");
                    }
                    if (qtr == "December")
                    {
                        dt1 = Convert.ToDateTime(yr + "/10/1");
                        dt2 = Convert.ToDateTime((Convert.ToInt32(yr) + 1).ToString().Trim() + "/1/1");
                    }
                    archiveqry = " and (archivedate>='" + dt1.ToString("yyyy-MM-dd") + "' and archivedate<'" + dt2.ToString("yyyy-MM-dd") + "')";



                    //com = new MySqlCommand("select * from TRAI_archive where (oper='" + oper + "') and (circ='" + circ + "') and (categ='" + prepost + "') and (actiontotake='WITHDRAWAL') " + ttypes + archiveqry + " order by rno", con);
                    com = new MySqlCommand("select * from TRAI_archive where (oper='" + oper + "') and (circ='" + circ + "') and (categ='" + prepost + "') and (actiontotake='WITHDRAWAL') " + archiveqry + " order by rno", con);
                        
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();
                        TableCell tc2b = new TableCell();
                        TableCell tc3 = new TableCell();
                        TableCell tc4 = new TableCell();
                        tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                        tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tc2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tc3.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tc4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        string css = "tablecell3";
                        if (rows1 % 2 == 0)
                        {
                            css = "tablecell3b";
                        }
                        tc1.CssClass = css;
                        tc2.CssClass = css;
                        tc2b.CssClass = css;
                        tc3.CssClass = css;
                        tc4.CssClass = css;

                        string ldate = "";
                        com1 = new MySqlCommand("select * from TRAI_archive where(uniqueid='" + dr["uniqueid"].ToString().Trim() + "') and (oper='" + oper + "') and (circ='" + circ + "') and (categ='" + prepost + "') and (actiontotake='LAUNCH' or actiontotake='REVISION') order by rno limit 1", con1);
                        con1.Open();
                        dr1 = com1.ExecuteReader();
                        dr1.Read();
                        try
                        {
                            ldate = Convert.ToDateTime(dr1["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy");
                        }
                        catch (Exception ex) { }
                        con1.Close();

                        arrPlan[cntr].Text = dr["planname"].ToString().Trim();
                        arrSubs[cntr].Text = "";
                        arrLdate[cntr].Text = ldate;
                        arrWdate[cntr].Text = Convert.ToDateTime(dr["actiondate"].ToString().Trim()).ToString("dd-MMM-yyyy");

                        arrSubs[cntr].Enabled = true;
                        
                        Button2.Visible = true;
                        trNote.Visible = true;


                        tc1.Text = (cntr + 1).ToString();
                        tc2.Controls.Add(arrPlan[cntr]);
                        tc2b.Controls.Add(arrSubs[cntr]);
                        tc3.Controls.Add(arrLdate[cntr]);
                        tc4.Controls.Add(arrWdate[cntr]);
                        tr.Controls.Add(tc1);
                        tr.Controls.Add(tc2);
                        tr.Controls.Add(tc2b);
                        tr.Controls.Add(tc3);
                        tr.Controls.Add(tc4);
                        tb.Controls.Add(tr);

                        cntr++;
                        rows1++;
                    }
                    con.Close();
                    

                }


                TdData.Controls.Add(tb);
                TdData.Visible = true;
                //tdCert.Visible = true;
                TdSubmit.Visible = true;
                trNote.Visible = true;

                TextHidden.Text = cntr.ToString().Trim();

            }
            
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




}