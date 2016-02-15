/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PCA Repair Page
 *             
 * UI:CI-MES12-SPEC-SA-UI PCA Repair For Docking.docx
 * UC:CI-MES12-SPEC-SA-UC PCA Repair For Docking.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-12  itc202017             (Reference Ebook SourceCode) Create
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
using com.inventec.iMESWEB;
using IMES.Docking.Interface.DockingIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;

public partial class SA_RepairLogAddEdit : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //public string Domain = WebCommonMethod.getConfiguration("DocumentDomain");

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                string customer = Request["Customer"];

                initLabel();
                this.cmbDefect.refreshDropContent("PRD");

                this.cmbObligation.Customer = customer;
                this.cmbObligation.refreshDropContent();

                this.cmbMark.refreshDropContent();

                this.cmbMajorPart.Customer = customer;
                this.cmbMajorPart.MBpart = true;
                this.cmbMajorPart.refreshDropContent(string.Empty);

                this.cmbComponent.Customer = customer;
                this.cmbComponent.refreshDropContent();
                this.cmbCause.Customer = customer;
                this.cmbCause.Stage = "SA";

                //Master.DisplayInfoArea = false;
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

    private void initLabel()
    {
        //this.lblTestStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();
        this.lblCause.Text = this.GetLocalResourceObject(Pre + "_lblCause").ToString();
        this.lblComponent.Text = this.GetLocalResourceObject(Pre + "_lblComponent").ToString();
        this.lblDefect.Text = this.GetLocalResourceObject(Pre + "_lblDefect").ToString();
        this.lblFaultyPartSN.Text = this.GetLocalResourceObject(Pre + "_lblFaultyPartSN").ToString();
        this.lblFaultyPartSno.Text = this.GetLocalResourceObject(Pre + "_lblFaultyPartSno").ToString();
        this.lblMajorPart.Text = this.GetLocalResourceObject(Pre + "_lblMajorPart").ToString();
        this.lblManufacture.Text = this.GetLocalResourceObject(Pre + "_lblManufacture").ToString();
        //this.lblMark.Text = this.GetLocalResourceObject(Pre + "_lblMark").ToString();
        this.lblNewPartDateCode.Text = this.GetLocalResourceObject(Pre + "_lblNewPartDateCode").ToString();
        this.lblNewPartSN.Text = this.GetLocalResourceObject(Pre + "_lblNewPartSN").ToString();
        this.lblNewPartSno.Text = this.GetLocalResourceObject(Pre + "_lblNewPartSno").ToString();
        this.lblObligation.Text = this.GetLocalResourceObject(Pre + "_lblObligation").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.lblSite.Text = this.GetLocalResourceObject(Pre + "_lblSite").ToString();
        this.lblVersionA.Text = this.GetLocalResourceObject(Pre + "_lblVersionA").ToString();
        this.lblVersionB.Text = this.GetLocalResourceObject(Pre + "_lblVersionB").ToString();

        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnOK.InnerText = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.InnerText = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
        this.lblMark.Text = this.GetLocalResourceObject(Pre + "_lblMark").ToString();
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        //scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("window.close();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.Form, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
