using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonFunction_ShowKBEsop : System.Web.UI.Page
{
    public String PicLoc = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PicLoc = System.Configuration.ConfigurationManager.AppSettings["RDS_MVS_ESOP"];
            if (!PicLoc.EndsWith("/") && !PicLoc.EndsWith("\\"))
            {
                PicLoc += "/";
            }
        }

    }
}
