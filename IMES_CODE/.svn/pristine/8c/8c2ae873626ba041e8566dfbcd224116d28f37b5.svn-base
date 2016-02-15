using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using com.inventec.iMESWEB;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

public partial class FA_ChangeSamplePO : System.Web.UI.Page
{
    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Editor = "";
    protected string Customer;
	protected string Station;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitPage();
            Editor = Request.QueryString["UserId"];
            Customer = Request.QueryString["Customer"];
			Station = Request.QueryString["Station"];
			
			this.cmbPdLine.Stage = Request["Stage"];
			this.cmbPdLine.Station = Station;
            this.cmbPdLine.Customer = Customer;
        }
    }

    private void InitPage()
    {
		this.lblPdLine.Text = GetLocalResourceObject(Pre + "lblPdLine").ToString();
		
        this.LabelDataEntry.Text = GetLocalResourceObject(Pre + "LabelDataEntry").ToString();
        this.Panel1.GroupingText = "Source of Change PO";
		this.Panel2.GroupingText = "Destination of Change PO";

    }
}
