/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:POD Label Check
 * UI:CI-MES12-SPEC-PAK-UI POD Label Check.docx –2011/10/28 
 * UC:CI-MES12-SPEC-PAK-UC POD Label Check.docx –2011/10/28           
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-28   Chen Xu               Create
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

public partial class PAK_PODLabelCheck : System.Web.UI.Page
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
                this.hiddenStation.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.txtDataEntry.Focus();
                
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
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
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
