/*
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

public partial class FA_CombineAndGenerateAST : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
	public String msgInputAST;

    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {
           //this.CmbStatus.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbStatus_Select);
           this.CmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdline_Select);

           UserId = Master.userInfo.UserId;
           Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();

                this.hiddenStation.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                this.CmbPdLine.Station = Request["Station"];
                this.CmbPdLine.Customer = Master.userInfo.Customer;
                
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
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSN").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblAST.Text = this.GetLocalResourceObject(Pre + "_lblAST").ToString();
		this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
		msgInputAST = this.GetLocalResourceObject(Pre + "_msgInputAST").ToString();
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
            //this.CmbStatus.setSelected(1);
            this.CmbPdLine.setSelected(-1);

            this.lblDataEntry.Text = "";
            this.lblStatus.Text = "";
            this.lblPdLine.Text = "";
            //2012-7-14, Jessica Liu, for mantis
            //this.lblASTLength.Text = "";
            this.lblProductID.Text = "";
            this.lblCustomerSN.Text = "";
            this.lblModel.Text = "";
            this.lblAST.Text = "";
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


