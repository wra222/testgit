 /*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:service for Combine AST Page
 * UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2011/12/5 
 * UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2011/12/5            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-2   Jessica Liu           (Reference Ebook SourceCode) Create
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

public partial class FA_CombineAST : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {
           //this.CmbStatus.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbStatus_Select);
           this.CmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdline_Select);

           //2012-4-16
           UserId = Master.userInfo.UserId;
           Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();

                this.hiddenStation.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];

                //this.CmbStatus.Station = Request["Station"];
                //this.CmbStatus.Customer = Master.userInfo.Customer;
                this.CmbPdLine.Station = Request["Station"];
                this.CmbPdLine.Customer = Master.userInfo.Customer;
                
                /*
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                 */

                setPdLineFocus();
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
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatus").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        //2012-7-14, Jessica Liu, for mantis
        //this.lblASTLength.Text = this.GetLocalResourceObject(Pre + "_lblASTLength").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblAST.Text = this.GetLocalResourceObject(Pre + "_lblAST").ToString();      
        //2012-4-28   
        //this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();                                 
    }

    /* 2011-12-26
    private void cmbStatus_Select(object sender, System.EventArgs e)
    {
        try
        {
            this.CmbStatus.Station = Request["Station"];
            this.CmbStatus.Customer = Master.userInfo.Customer;

            if (this.CmbStatus.InnerDropDownList.SelectedValue != "")
            {
                this.status.Value = this.CmbStatus.InnerDropDownList.SelectedValue;

                this.CmbPdLine.Focus();
            }
            else
            {
                setStatusFocus();
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
    */


    private void cmbPdline_Select(object sender, System.EventArgs e)
    {
        try
        {
            this.CmbPdLine.Station = Request["Station"];
            this.CmbPdLine.Customer = Master.userInfo.Customer;

            if (this.CmbPdLine.InnerDropDownList.SelectedValue != "")
            {
                this.line.Value = this.CmbPdLine.InnerDropDownList.SelectedValue;

                this.txtDataEntry.Focus();
            }
            else
            {
                setPdLineFocus();
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


    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            //this.CmbStatus.setSelected(1);
            this.CmbPdLine.setSelected(-1);

            //ÖØÖÃÎÄ±¾¿ò
            this.lblDataEntry.Text = "";
            this.lblStatus.Text = "";
            this.lblPdLine.Text = "";
            //2012-7-14, Jessica Liu, for mantis
            //this.lblASTLength.Text = "";
            this.lblProductID.Text = "";
            this.lblCustomerSN.Text = "";
            this.lblModel.Text = "";
            this.lblAST.Text = "";
            
            //2012-4-28
            //this.btnPrintSetting.Value = "";

            setPdLineFocus();
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

    private void setPdLineFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
               "window.setTimeout(setPdLineCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }

    private void setResetPage()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ResetPage(\"" + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "ResetPage", scriptBuilder.ToString(), false);
    }
}


