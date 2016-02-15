/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: 
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 
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


public partial class PAK_UnpackCombinePizza : IMESBasePage
{

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.station.Value = Request["Station"];
            this.pCode.Value = Request["PCode"];
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                initLabel();                
                
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
        this.lblDeliveryNo.Text = this.GetLocalResourceObject(Pre + "_lblDeliveryNo").ToString();
        this.lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();

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

        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

}
