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
    protected string Customer;
    protected string station;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Editor = Request.QueryString["UserId"];
            Customer = Request.QueryString["Customer"];
            station = Request["Station"] ?? "";
            this.cmbPdLine.Station = station;
            this.cmbPdLine.Customer = Customer;
            this.cmbPdLine.Stage = "SA";
        }
    }
}
