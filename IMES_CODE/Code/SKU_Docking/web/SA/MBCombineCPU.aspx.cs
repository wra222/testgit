/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for MB Combine CPU
 * UI:CI-MES12-SPEC-SA-UI MB Combine CPU.docx –2011/12/9 
 * UC:CI-MES12-SPEC-SA-UC MB Combine CPU.docx –2011/12/9            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-1-4   Jessica Liu           (Reference Ebook SourceCode) Create
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

public partial class SA_MBCombineCPU : IMESBasePage
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
                this.station.Value = Request["Station"]; 
                this.CmbPdLine1.Station = Request["Station"];  
                
                this.CmbPdLine1.Customer = Master.userInfo.Customer;
                /*
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                 */

                setFocus();
                this.Master.NeedPrint=false;
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
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblMB.Text = this.GetLocalResourceObject(Pre + "_lblMB").ToString();
        this.lblCPUVendorSN.Text = this.GetLocalResourceObject(Pre + "_lblCPUVendorSN").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();       
    }

    protected void hiddenbtn_Click(object sender, EventArgs e)
    {
        try
        {
            this.CmbPdLine1.setSelected(-1);

            this.txtDataEntry.Text = "";

            setFocus();
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

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }


    private void setFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout(setPdLineCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }
}
