/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:CombineCartonPalletFor146
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 
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
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;

public partial class PAK_CombineCartonPalletFor146 : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String Station;
	
	ICombineCartonPalletFor146 iCombineCartonPalletFor146 = ServiceAgent.getInstance().GetObjectByName<ICombineCartonPalletFor146>(WebConstant.CombineCartonPalletFor146);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
				initLabel();
                
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
				this.pCode.Value = Request["PCode"];
                this.CmbPdLine1.Station = Request["Station"];
                this.CmbPdLine1.Customer = Master.userInfo.Customer;
                this.CmbPdLine1.Stage = "PAK";
                setFocus();
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

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
//        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void initLabel()
    {
        this.lblPDLine.Text = this.GetLocalResourceObject(Pre + "_lblPDLine").ToString();
        //this.lblCollectionData.Text = this.GetLocalResourceObject(Pre + "_lblCollectionData").ToString();
        
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        //this.CmbPdLine1.InnerDropDownList.SelectedIndex = 1;
		this.btnSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
		this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }

}
