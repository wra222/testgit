/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: VGA Shipping label print
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-01-29   Tong.Zhi-Yong     Create 
 * 
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
using System.Collections.Generic;
using System.Text;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.DataModel;


public partial class SA_PrintContentWarranty : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
            }
            if (!this.IsPostBack)
            {
                initLabel();
                //UserId = Master.userInfo.UserId;
                //Customer = Master.userInfo.Customer;

                this.station.Value = Request["Station"]; //站号
                this.pCode.Value = Request["PCode"];
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
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblCustomerPN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerPN").ToString();
        this.lblWarranty.Text = this.GetLocalResourceObject(Pre + "_lblWarranty").ToString();
        this.lblConfiguration.Text = this.GetLocalResourceObject(Pre + "_lblConfiguration").ToString();
        this.lblAssetCheck.Text = this.GetLocalResourceObject(Pre + "_lblAssetCheck").ToString();
        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet"); 
        this.btnConfReprint.InnerText = this.GetLocalResourceObject(Pre + "_btnReprintConfiguration").ToString();
        this.btnWarrantyReprint.InnerText = this.GetLocalResourceObject(Pre + "_btnReprintWarranty").ToString();
        //setFocus();
   
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

    private void processDataEntry(string inputData)
    {
        this.txtProductID.Text = "test";
    }

    public void hiddenbtn_Click(object sender, EventArgs e)
    {
        setFocus();
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus();; </script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, ClientScript.GetType(), "setFocus", script, false);
    }

}
