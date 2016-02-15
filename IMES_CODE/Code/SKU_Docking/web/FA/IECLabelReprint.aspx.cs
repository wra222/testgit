/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: IECLabelReprint
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-09   zhu lei          Create 
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure;
using System.Collections.Generic;
using System.Text;
using IMES.DataModel;

public partial class FA_IECLabelReprint : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                Master.DisplayInfoArea = false;          //关闭开关
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
        this.lblVendorCT.Text = this.GetLocalResourceObject(Pre + "_lblVendorCT").ToString();
        this.lblReason.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblReason").ToString();
        this.btnPrintSetting.Value = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet").ToString();
        this.btnPrint.Value = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnReprint").ToString();
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
