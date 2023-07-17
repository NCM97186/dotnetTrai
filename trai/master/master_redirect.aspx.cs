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

public partial class master_redirect : System.Web.UI.Page
{
    MySqlCommand com, com1;        // ###  'MySqlCommand' instead of 'SqlCommand'
    MySqlConnection con, con1;     // ###  'MySqlConnection' instead of 'SqlConnection'
    MySqlDataReader dr, dr1;           // ###  'MySqlDataReader' instead of 'SqlReader'
    string t1, user, masterdate, mode,dbtouse;
    int flag;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            con = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);
            con1 = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString);

            Server.ScriptTimeout = 9999999;

            flag = 0;

            t1 = Request["t1"].ToString().Trim();
            user = Request["user"].ToString().Trim();
            
            if (Session["master"] != null)
            {
                if (Session["master"] == "")
                {
                    //Response.Write("<center><br /><br /><br /><br /><br /><br /><br /><font face=arial size=2><b>Your Session Has Expired. Please <a href=master_login.aspx class=indexlinks target=_parent>Click Here</a> to login again.");
                    //flag = 1;
                }
            }
            else
            {
                //Response.Write("<center><br /><br /><br /><br /><br /><br /><br /><font face=arial size=2><b>Your Session Has Expired. Please <a href=master_login.aspx class=indexlinks target=_parent>Click Here</a> to login again.");
                //flag = 1;
            }



            if (flag == 0)
            {
                //Session["admin"] = "admin";

                if (Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
                {
                    Response.Redirect(t1 + "?user=" + user);
                }
                else
                {
                    Response.Redirect("blank.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }



    }
}
