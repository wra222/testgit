/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Sevice for Online Generate AST Reprint Page
 * UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/2/1 
 * UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/2/1            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-2-1   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
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

public partial class FA_OnlineGenerateASTReprint : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {    
           //2012-4-16
           UserId = Master.userInfo.UserId;
           Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();
                
                if (Request["Station"] != null)
                {
                    this.hiddenStation.Value = Request["Station"]; //站号
                }
                else
                {
                    this.hiddenStation.Value = "";
                }
                
                this.pCode.Value = Request["PCode"];
                /*
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                 */
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
        //this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        //this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblReason.Text = this.GetLocalResourceObject(Pre + "_lblReason").ToString();
        //this.btnPrintSetting.Text = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        //this.btnReprint.Text = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();          
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


