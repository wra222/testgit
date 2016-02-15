/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description:Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx –2011/11/03
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx –2011/11/03          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-04-06              Create
 * Known issues:
 * TODO： 
 *
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


public partial class PAK_UnpackPalletNoForRCTO : IMESBasePage
{

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String station;
    public String pCode;
     
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            station= Request["Station"];
            pCode = Request["PCode"];
            hidIsFUR.Value = Request["IsFRU"] ?? "";
            if (!this.IsPostBack)
            {
                initLabel();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
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
        if (hidIsFUR.Value == "Y")
        { this.lblDummyPalletNo.Text = this.GetLocalResourceObject(Pre + "_CartonDN").ToString(); }
        else
        { this.lblDummyPalletNo.Text = this.GetLocalResourceObject(Pre + "_lblPalletNo").ToString(); }
   
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    

}
