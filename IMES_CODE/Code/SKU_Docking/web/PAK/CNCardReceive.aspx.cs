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
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;


public partial class PAK_CNCardReceive : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String Station;

    protected void Page_Load(object sender, EventArgs e)
    {
        Pre = this.GetLocalResourceObject("language").ToString();
        if (!this.IsPostBack)
        {
            this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbldataentry").ToString();
            this.lbPartNo.Text = this.GetLocalResourceObject(Pre + "_lblpartno").ToString();
            this.lbBegNo.Text = this.GetLocalResourceObject(Pre + "_lblbegno").ToString();
            this.lbEndNo.Text = this.GetLocalResourceObject(Pre + "_lblendno").ToString();
            this.lbCount.Text = this.GetLocalResourceObject(Pre + "_lblcount").ToString();
            this.btsave.Value = this.GetLocalResourceObject(Pre + "_btnsave").ToString();

            Station = Request["Station"];
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
        }
    }
}
