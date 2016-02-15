//'
//' INVENTEC corporation (c)2008 all rights reserved. 
//' Description: Message Box page
//'             
//' Update: 
//' Date       Name                  Reason 
//' ========== ===================== =====================================
//' 2008-08-22  Zhao Meili(DD)        Create  
//' 2008-08-23  Zhao Meili(DD)        Modify 
//' 2008-08-24  Zhao Meili(DD)        Modify  
//' Known issues:
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;

public partial class ErrMessageDisplay : IMESBasePage
{
    public string pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    protected void Page_Load(object sender, EventArgs e)
    {
        string errMes = this.Request.QueryString["err"];

        if (!string.IsNullOrEmpty(errMes))
        {
            errMes = errMes.Replace("\n", "<br/>");
            errMes = errMes.Replace("\r", "<br/>");
            //errMes = (errMes.Replace( Chr(13).ToString(), "<br/>")).Replace(Chr(10).ToString(), "<br/>");
            string str = "document.getElementById('mess').innerHTML= '" + errMes + "'; " + " window.document.getElementById('form1').OK.blur() ;";
            string script = "<script type='text/javascript' language='javascript'>" + str + "</script>";
            ClientScript.RegisterStartupScript(GetType(), "", script);
        }
    }
}
