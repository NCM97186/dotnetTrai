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
using System.IO;
using System.Text;

public partial class zzExcelToDb1 : System.Web.UI.Page
{
    string tmp = "";
    

    protected void Page_Load(object sender, EventArgs e)
    {
        
        /*
        if (Request.UrlReferrer == null)
        {
            Response.Redirect("logout.aspx");
        }
        if (!Request.UrlReferrer.ToString().ToUpper().Contains("MASTER"))
        {
            Response.Redirect("logout.aspx");
        }
        */

        Server.ScriptTimeout = 9999999;

        /*
        if (Session["master"] == null)
        {
            Response.Redirect("sessout.aspx");
        }
        */
        
    }




    protected void Button1_Click(object sender, EventArgs e)
    {
        int flag = 0;

        if (flag == 0)
        {
            if (null != uplTheFile.PostedFile)
            {
                if (uplTheFile.PostedFile != null)
                {
                    if (uplTheFile.PostedFile.ContentLength > 0)
                    {
                        try
                        {
                            tmp = uplTheFile.PostedFile.FileName;

                            string sr = DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "-" + System.IO.Path.GetFileName(tmp);
                            Session["xlfile"] = sr.Replace(" ", "_");
                            uplTheFile.PostedFile.SaveAs(Server.MapPath("Excel/" + sr));

                            txtOutput.InnerHtml = "Upload Successful!";
                            Session["msg"] = "Sales Data";
                            //Response.Redirect("zzExcelToDb2.aspx?t1=" + sr + "&user=" + Request["user"].ToString().Trim());
                            Response.Redirect("zzExcelToDb2.aspx?t1=" + sr);

                        }
                        catch (Exception ex)
                        {
                            txtOutput.InnerHtml = "Error saving file <b>C:\\" +
                                uplTheFile.Value + "</b><br>" + ex.ToString();

                        }
                    }
                    else
                    {
                        Response.Write("<script language=javascript>alert('Please Select The File To Upload');</script>");
                    }

                }
            }
        }
    }






}
