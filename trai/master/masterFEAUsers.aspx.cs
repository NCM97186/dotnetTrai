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
using System.Security.Cryptography;   // #######  For MD5 encryption


public partial class masterFEAUsers : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string str1, mystr, tablename, rights, oldpurity;
    int zno, conflag;
    double modrno;
    Button btnsave;
    Table tbadj;

    protected void Page_Load(object sender, EventArgs e)
    {

        con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con3 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con9 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
        con10 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

        try
        {
            tablename = "TRAI_FEA";

            if (Request.UrlReferrer == null)
            {
                Response.Redirect("logout.aspx");
            }
            if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
            {
                Response.Redirect("logout.aspx");
            }


            modrno = 0;
            try
            {
                modrno = Convert.ToDouble(Request["rno"].ToString().Trim());
            }
            catch (Exception ex) { }

            com = new MySqlCommand("select * from TRAI_admin where(uname='" + Request["user"].ToString().Trim() + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            rights = dr["rights"].ToString().Trim();
            if (dr["rights"].ToString().Contains("FEALogins_") || dr["rights"].ToString().Trim() == "All")
            {
                Button1.Visible = true;
            }
            con.Close();

            string Password = TextPassword.Text;
            TextPassword.Attributes.Add("value", Password);

            if (!IsPostBack)
            {
                com = new MySqlCommand("select * from TRAI_operators order by oper", con);
                con.Open();
                dr = com.ExecuteReader();
                while(dr.Read())
                {
                    CheckOper.Items.Add(dr["oper"].ToString().Trim());
                }
                con.Close();

                if (modrno > 0)
                {
                    com = new MySqlCommand("select * from " + tablename + " where(rno=" + modrno + ")", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        TextUserID.Text = dr["uname"].ToString().Trim();
                        TextPassword.Text = dr["pass"].ToString().Trim();
                        TextPassword.Attributes.Add("value", dr["pass"].ToString().Trim());
                        TextEmail.Text = dr["email"].ToString().Trim();
                        TextDesignation.Text = dr["designation"].ToString().Trim();
                        RadReview.Text = dr["review"].ToString().Trim();
                        RadReport.Text = dr["report"].ToString().Trim();
                        //RadParameters.Text = dr["parameters"].ToString().Trim();
                        RadFD.Text = dr["parameters"].ToString().Trim();
                        string op = dr["oper"].ToString().Trim();
                        for(int i=0;i<CheckOper.Items.Count;i++)
                        {
                            if(op.Contains("," + CheckOper.Items[i].Text.Trim() + ","))
                            {
                                CheckOper.Items[i].Selected = true;
                            }
                        }
                    }
                    con.Close();
                }

                ShowTransactions(null, null);
            }

            if (!IsPostBack)
            {
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

                    if (rights.Contains("FEALogins_Mod,") || rights == "All")
                    {
                        Button2.Visible = true;
                    }
                }
            }
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
            Response.Redirect("master_redirect.aspx?t1=masterFEAUsers.aspx&user=" + Request["user"].ToString().Trim());
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }






    protected void ShowTransactions(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;

            if (flag == 0)
            {
                string css = "tablecell3";
                int rowcnt = 0;
                mystr = "<center><b>F&EA User Details</b></center><br /><table width=90% cellspacing=1 cellpadding=8><tr><td class=tableheadcenter>User ID</td><td class=tableheadcenter>Email ID</td><td class=tableheadcenter>Designation</td><td class=tableheadcenter>Review Operators</td><td class=tableheadcenter>Review Access Level</td><td class=tableheadcenter>Reporting Tools Access</td><td class=tableheadcenter>Edit</td><td class=tableheadcenter>Delete</td></tr>";
                com = new MySqlCommand("select * from " + tablename + " order by uname", con);
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
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + dr["uname"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + dr["email"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + dr["designation"].ToString().Trim() + "</td>";
                    string revop = dr["oper"].ToString().Trim();
                    if(revop.Length>3)
                    {
                        revop = revop.Substring(1, revop.Length - 2);
                    }
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + revop + "</td>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + dr["review"].ToString().Trim() + "</td>";
                    mystr = mystr + "<td class=" + css + " align=center valign=top>" + dr["report"].ToString().Trim() + "</td>";
                    if (rights.ToString().Contains("FEALogins_Mod,") || rights == "All")
                    {
                        mystr = mystr + "<td class=" + css + " align=center><a href='masterFEAUsers.aspx?user=" + Request["user"].ToString().Trim() + "&rno=" + dr["rno"].ToString().Trim() + "' class=indexlinks><img src='images/iconedit.jpg' width=20 border=0></a></td>";
                    }
                    else
                    {
                        mystr = mystr + "<td class=" + css + " align=center></td>";
                    }
                    if (rights.ToString().Contains("FEALogins_Del,") || rights == "All")
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

            string oper = ",";
            string uname = TextUserID.Text.Trim().Replace("'", "`");
            string pass = TextPassword.Text.Trim().Replace("'", "`");
            string email = TextEmail.Text.Trim().Replace("'", "`");
            string desig = TextDesignation.Text.Trim().Replace("'", "`");
            string review = RadReview.SelectedItem.Text.Trim();
            string report = RadReport.SelectedItem.Text.Trim();
            //string parameters = RadParameters.SelectedItem.Text.Trim();
            string parameters = RadFD.SelectedItem.Text.Trim();

            
            if (uname == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a User ID');</script>");
            }
            if (pass == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a password');</script>");
            }
            if (email == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter an email ID');</script>");
            }
            if(!email.Contains("@") || !email.Contains("."))
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a valid email ID');</script>");
            }
            if (review == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select the review access level');</script>");
            }
            if (report == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select the reporting tools access type');</script>");
            }
            if (parameters == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select the F&EA parameters access type');</script>");
            }

            for (int i = 0; i < CheckOper.Items.Count;i++)
            {
                if (CheckOper.Items[i].Selected == true)
                {
                    oper = oper + CheckOper.Items[i].Text.Trim() + ",";
                }
            }

                if (flag == 0)
                {
                    int exists = 0;
                    com = new MySqlCommand("select count(*) from " + tablename + " where(uname='" + uname + "')", con);
                    con.Open();
                    dr = com.ExecuteReader();
                    dr.Read();
                    exists = Convert.ToInt32(dr[0].ToString().Trim());
                    con.Close();

                    if (exists > 0)
                    {
                        Response.Write("<script>alert('This User ID already exists.');</script>");
                        flag = 1;
                    }
                }


            if (flag == 0)
            {
                getMaxRno("rno", tablename);
                com = new MySqlCommand("insert into " + tablename + " values('" + zno + "','" + uname + "','" + pass + "','" + email + "','" + desig + "','" + review + "','" + report +"','" + Request["user"].ToString().Trim() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','','2001-01-01','" + oper + "','" + parameters + "')", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                Response.Redirect("masterFEAUsers.aspx?user=" + Request["user"].ToString().Trim());

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
            double recno = Convert.ToDouble(Request["rno"].ToString().Trim());

            int flag = 0;

            string oper = ",";
            string uname = TextUserID.Text.Trim().Replace("'", "`");
            string pass = TextPassword.Text.Trim().Replace("'", "`");
            string email = TextEmail.Text.Trim().Replace("'", "`");
            string desig = TextDesignation.Text.Trim().Replace("'", "`");
            string review = RadReview.SelectedItem.Text.Trim();
            string report = RadReport.SelectedItem.Text.Trim();
            //string parameters = RadParameters.SelectedItem.Text.Trim();
            string parameters = RadFD.SelectedItem.Text.Trim();

            if (uname == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a User ID');</script>");
            }
            if (pass == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a password');</script>");
            }
            if (email == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please enter an email ID');</script>");
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                flag = 1;
                Response.Write("<script>alert('Please enter a valid email ID');</script>");
            }
            if (review == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select the review access level');</script>");
            }
            if (report == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select the reporting tools access type');</script>");
            }
            if (parameters == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select the F&EA parameters access type');</script>");
            }

            for (int i = 0; i < CheckOper.Items.Count; i++)
            {
                if (CheckOper.Items[i].Selected == true)
                {
                    oper = oper + CheckOper.Items[i].Text.Trim() + ",";
                }
            }

            if (flag == 0)
            {
                int exists = 0;
                com = new MySqlCommand("select count(*) from " + tablename + " where(uname='" + uname + "') and (rno<>" + recno + ")", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                if (Convert.ToInt32(dr[0].ToString().Trim()) > 0)
                {
                    Response.Write("<script>alert('This User ID already exists');</script>");
                    flag = 1;
                }
                con.Close();
            }

            if (flag == 0)
            {
                com = new MySqlCommand("update " + tablename + " set uname='" + uname + "', pass='" + pass + "',email='" + email + "',designation='" + desig + "',review='" + review + "',report='" + report + "',modifiedby='" + Request["user"].ToString().Trim() + "',modifiedon='" + DateTime.Now.ToString("yyyy-MM-dd") + "',oper='" + oper + "',parameters='" + parameters + "' where(rno=" + recno + ")", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                Response.Redirect("masterFEAUsers.aspx?user=" + Request["user"].ToString().Trim());
            }


            if (rights.Contains("FEALogins_Mod,") || rights == "All")
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
            con1.Close();

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
            oldname = dr["uname"].ToString().Trim();
            con.Close();

            // Check if the selected value exists in any of the related tables, existing values are not to be deleted //
            /*
            com = new MySqlCommand("select count(*) from TRAI_partymaster where(groupname='" + oldname + "')", con);
            con.Open();
            dr = com.ExecuteReader();
            dr.Read();
            exists = Convert.ToInt32(dr[0].ToString().Trim());
            con.Close();
            */
            // Checking of existing value code ends here //

            if (exists == 0)
            {
                com = new MySqlCommand("delete from " + tablename + " where(rno=" + recno + ")", con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                Response.Redirect("masterFEAUsers.aspx?user=" + Request["user"].ToString().Trim());
            }
            else
            {
                Response.Write("<script>alert('Records exist with this ID, and hence it cannot be deleted');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString().Trim());
        }

    }






}
