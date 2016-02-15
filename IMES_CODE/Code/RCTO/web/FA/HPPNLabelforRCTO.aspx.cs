/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: HPPN Label Print
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2012-08-13  itc000052               Create 
 * Known issues:
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

public partial class FA_HPPNLabelforRCTO : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IHPPNLabelforRCTO iHPPNLabelforRCTO = ServiceAgent.getInstance().GetObjectByName<IHPPNLabelforRCTO>(WebConstant.HPPNLabelforRCTOObject);

    public String UserId;
    public String Customer;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();

                this.station.Value = Request["Station"]; //站号
                //this.pCode.Value = Request["PCode"];
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                setFocus();

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

    private void initLabel()
    {
        this.lblHPPN.Text = this.GetLocalResourceObject(Pre + "_lblHPPN").ToString();

        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.btnPrintSetting.Text = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
        
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
