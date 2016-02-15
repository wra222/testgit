/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description: Reprint Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-12   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：Reprint Config Label and POD Label
 * UC Revision：4078
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

public partial class PAK_ReprintUnitWeight : System.Web.UI.Page
{
    
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public int initRowsCount = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();


                this.hiddenStation.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                Master.DisplayInfoArea = false;
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
        this.lblCustsn.Text = this.GetLocalResourceObject(Pre + "_lblCustsn").ToString();
        this.lblReason.Text = this.GetLocalResourceObject(Pre + "_lblReason").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        
    }

    

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

  
}