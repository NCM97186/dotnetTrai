using System;
using System.Globalization;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;

public partial class FEA_logout : System.Web.UI.Page
{
    SqlCommand com, com1;
    SqlConnection con, con1;
    SqlDataReader dr, dr1;
    int rno;
    string myrecords, sortby, cond;
    double t1;

    protected void Page_Load(object sender, EventArgs e)
    {

        Session.Abandon();
    }
}
