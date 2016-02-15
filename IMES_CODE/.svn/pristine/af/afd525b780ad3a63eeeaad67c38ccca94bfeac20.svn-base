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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;
using System.Web.Services;

public partial class SA_FIXTure_Input : System.Web.UI.Page
{
    protected string Editor = "";

    static IFIXTure fixture = ServiceAgent.getInstance().GetObjectByName<IFIXTure>(WebConstant.FIXTure);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmbConstValueType1.Type = "FIXTureLoc";
            Editor = Request.QueryString["UserId"];
        }
    }
}
