/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PCAShippingLabelPrint
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei           Create
 * 2012-02-27   Li.Ming-Jun       ITC-1360-0507
 * 
 * Known issues:Any restrictions about this file 
 */
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
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections.Generic;

public partial class FA_PCAShippingLabelPrint : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //this.rblPCA.SelectedIndexChanged += new EventHandler(rblPCA_Changed);
            if (!this.IsPostBack)
            {
                initLabel();
                initRegion();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.CmbPdLine.Station = Request["station"];
                this.CmbPdLine.Customer = Customer;
                this.CmbDCode.Customer = Customer;
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    private void initLabel()
    {
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.lblDCode.Text = this.GetLocalResourceObject(Pre + "_lblDCode").ToString();
        this.lblProductModel.Text = this.GetLocalResourceObject(Pre + "_lblProductModel").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblECR.Text = this.GetLocalResourceObject(Pre + "_lblECR").ToString();
        this.lblPdline.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblMAC.Text = this.GetLocalResourceObject(Pre + "_lblMAC").ToString();
        this.lblMBSNO.Text = this.GetLocalResourceObject(Pre + "_lblMBSNO").ToString();
        this.lblRegion.Text = this.GetLocalResourceObject(Pre + "_lblRegion").ToString();
        this.lblVersion.Text = this.GetLocalResourceObject(Pre + "_lblVersion").ToString();
        this.btnPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.rblPCA.SelectedValue = "ForRCTO";
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void initRegion()
    {
        IConstValue commonService = ServiceAgent.getInstance().GetObjectByName<IConstValue>(WebConstant.CommonObject);

        drpRegion.Items.Add(new ListItem("", ""));
        IList<ConstValueInfo> ConstList = commonService.GetConstValueListByType("Region","");
        if (!(ConstList == null) && (ConstList.Count > 0))
        {
            foreach (ConstValueInfo obliItem in ConstList)
            {
                drpRegion.Items.Add(new ListItem(obliItem.name, obliItem.id.ToString()));
            }
        }
    }
}
