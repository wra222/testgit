/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description: Reprint ReprintMBSplit
 * UI:CI-MES12-SPEC-PAK-UI MB Split.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC MB Split.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-10   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：Reprint ReprintMBSplit 列印所有Label；
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

public partial class PAK_ReprintMBSplit : System.Web.UI.Page
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
        this.lblMBSno.Text = this.GetLocalResourceObject(Pre + "_lblMBSno").ToString();
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