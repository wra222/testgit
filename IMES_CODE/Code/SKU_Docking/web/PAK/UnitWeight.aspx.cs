﻿/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:Unit Weight
 * UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx –2011/11/25
 * UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx –2011/11/25         
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-25   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class PAK_UnitWeight : System.Web.UI.Page
{
    
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String Station;
    protected string Pcode;
    protected int isTestWeight = WebCommonMethod.isTestWeight();
    public string PDFClinetPath = ConfigurationManager.AppSettings["UnitWeightPDFPath"].ToString();
    public string UnitWeightFilePath = ConfigurationManager.AppSettings["UnitWeightPath"].ToString(); 
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
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }


    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    /// <returns></returns>
     private void initLabel()
    {
        this.lblProductInfo.InnerText = this.GetLocalResourceObject(Pre + "_lblProductInfo").ToString();
        this.lblCheckItem.InnerText = this.GetLocalResourceObject(Pre + "_lblCheckItem").ToString();
        this.lblUnitWeight.Text = this.GetLocalResourceObject(Pre + "_lblUnitWeight").ToString();
        this.lblStdWeight.Text = this.GetLocalResourceObject(Pre + "_lblStdWeight").ToString();
        this.lblRMN.Text = this.GetLocalResourceObject(Pre + "_lblRMN").ToString();
        this.lblProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblCustSN.Text = this.GetLocalResourceObject(Pre + "_lblCustSN").ToString();
        this.lblConfigID.Text = this.GetLocalResourceObject(Pre + "_lblConfigID").ToString();
        this.lblBoxID.Text = this.GetLocalResourceObject(Pre + "_lblBoxID").ToString();
        this.lblAssetTagItem.Text = this.GetLocalResourceObject(Pre + "_lblAssetTagItem").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }
   

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

  
}