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

public partial class FEA_repQuartBlack : System.Web.UI.Page
{
    MySqlCommand com, com1, com2, com3, com9, com10;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1, con2, con3, con9, con10;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1, dr2, dr3, dr9, dr10;           // ###  'MySqlDataReader' instead of 'SqlReader'
    Table tbresults;
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
            
            int curryr = Convert.ToInt32(DateTime.Now.Year);
            DropYr.Items.Add("");
            for (int i = 2018; i <= curryr; i++)
            {
                DropYr.Items.Add(i.ToString().Trim());
            }
        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;
            TdData.InnerHtml = "";
            TdData.Visible = false;
            TdPrint.Visible = false;

            string tablename = "TRAI_QuartBlackOut";
            string yr = DropYr.SelectedItem.Text.Trim();

            string operconditions = "";
            if (RadOType.SelectedItem.Text == "TSP")
            {
                operconditions += " (upper(oper)='AIRCEL' or upper(oper)='AIRTEL' or upper(oper)='BSNL' or upper(oper)='IDEA' or upper(oper)='JIO' or upper(oper)='MTNL' or upper(oper)='QUADRANT (CONNECT)' or upper(oper)='TATA TELE' or upper(oper)='TELENOR' or upper(oper)='VODAFONE' or upper(oper)='VODAFONE IDEA' or upper(oper)='SURFTELECOM' or upper(oper)='AEROVOYCE')";
            }
            if (RadOType.SelectedItem.Text == "ISP")
            {
                operconditions += " (upper(oper)!='AIRCEL' and upper(oper)!='AIRTEL' and upper(oper)!='BSNL' and upper(oper)!='IDEA' and upper(oper)!='JIO' and upper(oper)!='MTNL' and upper(oper)!='QUADRANT (CONNECT)' and upper(oper)!='TATA TELE' and upper(oper)!='TELENOR' and upper(oper)!='VODAFONE' and upper(oper)!='VODAFONE IDEA' and upper(oper)!='SURFTELECOM' and upper(oper)!='AEROVOYCE')";
            }
            if (RadOType.SelectedItem.Text == "Both")
            {
                // no condition here, as all TSP/ISP are to be included
                operconditions += " (rno>0)";
            }

            if (DropYr.SelectedItem.Text.Trim() == "")
            {
                flag = 1;
                Response.Write("<script>alert('Please select a calendar year');</script>");
            }

            if (flag == 0)
            {
                int circcount = 0;
                int opercount = 0;
                
                com = new MySqlCommand("select count(*) from TRAI_circles", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                circcount = Convert.ToInt32(dr[0].ToString().Trim());
                con.Close();

                com = new MySqlCommand("select count(*) from TRAI_operators", con);
                con.Open();
                dr = com.ExecuteReader();
                dr.Read();
                opercount = Convert.ToInt32(dr[0].ToString().Trim());
                con.Close();

                string[] arrcirc=new string[circcount];
                string[] arroper=new string[opercount];

                int cntr1 = 0;
                //com = new MySqlCommand("select * from TRAI_operators order by oper", con);
                com = new MySqlCommand("select * from TRAI_operators where " + operconditions + " order by oper", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    arroper[cntr1] = dr["oper"].ToString().Trim();
                    cntr1++;
                }
                con.Close();


                int cntr2 = 0;
                com = new MySqlCommand("select * from TRAI_circles order by circ", con);
                con.Open();
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    arrcirc[cntr2] = dr["circ"].ToString().Trim();
                    cntr2++;
                }
                con.Close();

                string mystr = "<center><b><u>Black Out Days Report - " + yr.ToString() + "</u></b></center><br />";
                mystr = mystr + "<table width=100% style=border-collapse:collapse;min-width:450px; cellspacing=1 border=1 cellpadding=5>";
                mystr = mystr + "<tr>";
                mystr = mystr + "<td class=tablecell align=left><b>Name of Circle</b></td>";
                for (int i = 0; i < cntr1;i++)
                {
                    mystr = mystr + "<td class=tablecell style=min-width:70px; align=left valign=top>" + arroper[i] + "</td>";
                }
                con.Close();
                mystr = mystr + "</tr>";

                for (int i = 0; i < cntr2; i++)
                {
                    mystr = mystr + "<tr>";
                    mystr = mystr + "<td class=tablecell align=left valign=top>" + arrcirc[i] + "</td>";
                        
                    for(int j = 0; j < cntr1; j++)
                    {
                        string mydates = "";
                        com = new MySqlCommand("select * from " + tablename + " where(oper='" + arroper[j].ToString().Trim() + "') and (circ='" + arrcirc[i].ToString().Trim() + "') and (yr='" + yr + "') order by rno", con);
                        con.Open();
                        dr = com.ExecuteReader();
                        while(dr.Read())
                        {
                            for (int x = 1; x <= 5; x++)
                            {
                                if (Convert.ToDateTime(dr["date" + x.ToString().Trim()]) > Convert.ToDateTime("1/2/2011"))
                                {
                                    mydates += Convert.ToDateTime(dr["date" + x.ToString().Trim()]).ToString("dd-MMM-yy") + "<br />";
                                }
                            }

                            if(mydates!="")
                            {
                                mydates = "<div style=min-height:60px;>" + mydates + "</div>";
                            }

                            // Check for Delay in Reporting //
                            
                            DateTime MaxFileDate = Convert.ToDateTime((Convert.ToInt32(yr) + 1) + "/" + "1" + "/" + "7");    // 7th Jan of Next Year
                            TimeSpan ts = Convert.ToDateTime(Convert.ToDateTime(dr["recdate"].ToString().Trim()).ToShortDateString()) - MaxFileDate;
                            int delay = ts.Days;
                            
                            if (delay > 0)
                            {
                                mydates = mydates + "<br /><br /><font color=red>Delay:<br />" + delay.ToString() + " Day(s)" + "</font>";
                            }
                                                        
                            // Check for Delay in Reporting - Code Ends Here //

                        }
                        con.Close();
                        
                        mystr = mystr + "<td class=tablecell align=left valign=top>" + mydates.ToString() + "</td>";
                    }
                    mystr = mystr + "</tr>";
                }

                mystr = mystr + "</table><br /><br />";

                TdData.InnerHtml = mystr;
                TdData.Visible = true;
                TdPrint.Visible = true;

                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#Bookmark1';", true);
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }







    protected void ButtonExcel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Data.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Output.Write(TdData.InnerHtml);
            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {
            //Response.Write(ex.ToString());
        }
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
            Response.Write(ex.ToString().Trim());
        }

    }




}