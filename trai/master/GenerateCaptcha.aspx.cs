using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class master_GenerateCaptcha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        int height = 130;
        int width = 130;
        Bitmap bmp = new Bitmap(height, width);
        RectangleF rectf = new RectangleF(10, 5, 0, 0);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.White);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.DrawString(Session["captcha"].ToString(), new Font("Thaoma", 20, FontStyle.Italic), Brushes.Green, rectf);
        g.DrawRectangle(new Pen(Color.Red), 1, 1, width - 2, height - 2);
        g.Flush();
        Response.ContentType = "image/jpeg";
        bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
        g.Dispose();
        bmp.Dispose();

    }
}