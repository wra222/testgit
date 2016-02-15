/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:service for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2011/11/21 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2011/11/21            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
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

public partial class FA_OnlineGenerateAST : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {
           this.CmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdline_Select);

           //2012-4-16
           UserId = Master.userInfo.UserId;
           Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();

                this.hiddenStation.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];                
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
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblCPQSNO.Text = this.GetLocalResourceObject(Pre + "_lblCPQSNO").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        //this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();                                            
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();                                            
    }

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
            this.CmbPdLine.setSelected(-1);

            //ÖØÖÃÎÄ±¾¿ò
            this.lblDataEntry.Text = "";
            this.lblPdLine.Text = "";
            this.lblProductID.Text = "";
            this.lblCPQSNO.Text = "";
            this.lblModel.Text = "";
            //this.btnReprint.Value = "";
            this.btnPrintSetting.Value = "";

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


