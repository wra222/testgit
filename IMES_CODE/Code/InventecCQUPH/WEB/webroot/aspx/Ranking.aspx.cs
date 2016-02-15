using System;
using System.Collections;
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
using UPH.Interface;
using System.Collections.Generic;

public partial class webroot_aspx_Ranking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
       
    {
        try
        {
            IUPH uph = ServiceAgent.getInstance().GetObjectByName<IUPH>(WebConstant.UPHIUPHALL);

            IList<string> linelist = uph.GetUPHLine();
        }
        catch (Exception ex)
        {
            //throw e.Message.ToString();
        }
    }
}
