/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for REV Label Print For Docking Page
 * UI:CI-MES12-SPEC-FA-UI REV Label Print For Docking.docx –2012/5/28 
 * UC:CI-MES12-SPEC-FA-UC REV Label Print For Docking.docx –2012/5/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-30   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1414-0139, Jessica Liu, 2012-6-13
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

public partial class Docking_REVLabelPrintForDocking : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {    
           UserId = Master.userInfo.UserId;
           Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                this.hiddenStation.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                if (!Page.IsPostBack)
                {
                    initLabel();
                    this.cmbFamily.Customer = Customer;
                    this.cmbDataCode.Customer = Customer;
                    //Order by Family.Family
                    //ITC-1414-0139, Jessica Liu, 2012-6-13
                    this.cmbFamily.Service = "015";
                }
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
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblDCode.Text = this.GetLocalResourceObject(Pre + "_lblDCode").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();                                  
    }


    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            this.cmbFamily.setSelected(0);
            this.cmbDataCode.setSelected(0);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
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


