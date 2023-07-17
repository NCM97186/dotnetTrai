using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class blank : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["msg"] == "smssent")
        {
            Response.Write("<script>alert('SMS sent successfully.');</script>");
            Session["msg"] = "";
        }

        if (Session["msg"] == "statusupdated")
        {
            Response.Write("<script>alert('Status updated successfully.');</script>");
            Session["msg"] = "";
        }

    }
}
