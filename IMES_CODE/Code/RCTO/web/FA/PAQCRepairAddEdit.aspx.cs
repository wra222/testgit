/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:service for QC Repair Add/Edit Page
 * UI:CI-MES12-SPEC-FA-UI QC Repair.docx –2011/10/19 
 * UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2011/10/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0463, Jessica Liu, 2012-2-16
* ITC-1360-0441, Jessica Liu, 2012-2-17
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

public partial class FA_PAQCRepairAddEdit : IMESBasePage 
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //public string Domain = WebCommonMethod.getConfiguration("DocumentDomain");

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                string customer = Request["Customer"]; //Master.userInfo.Customer;
                
                initLabel();
                this.CmbDefect.refreshDropContent("PRD");
                this.CmbTestStation.Customer = customer;
                //ITC-1360-0441, Jessica Liu, 2012-2-17
                //this.CmbSubDefect.Type = "PRD";//Dean 20110513 QCSUB-->PRD
                this.CmbSubDefect.Type = "QCSUB";

                //this.CmbSubDefect.refreshDropContent("PRD");//Dean 20110513
                this.CmbCause.Customer = customer;
                this.CmbPMajorPart.Customer = customer;
                this.CmbPMajorPart.MBpart = false;
                this.CmbPMajorPart.refreshDropContent(string.Empty);
                
                this.CmbComponent.Customer = customer;
                this.CmbComponent.refreshDropContent();

                this.CmbTrackingStatus.Customer = customer;
                this.CmbObligation.Customer = customer;
                this.CmbObligation.refreshDropContent();
                this.CmbDistribution.Customer = customer;
                this.CmbResponsibility.Customer = customer;
                this.Cmb4M.Customer = customer;
                this.CmbCover.Customer = customer;
                this.CmbUncover.Customer = customer;

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
        lbl4M.Text = this.GetLocalResourceObject(Pre + "_lbl4M").ToString();
        lblAction.Text = this.GetLocalResourceObject(Pre + "_lblAction").ToString();
        lblCause.Text = this.GetLocalResourceObject(Pre + "_lblCause").ToString();
        lblComponent.Text = this.GetLocalResourceObject(Pre + "_lblComponent").ToString();
        lblCover.Text = this.GetLocalResourceObject(Pre + "_lblCover").ToString();
        lblDefect.Text = this.GetLocalResourceObject(Pre + "_lblDefect").ToString();
        lblDistribution.Text = this.GetLocalResourceObject(Pre + "_lblDistribution").ToString();
        /* 2012-1-11，根据UC删除
        lblFaultyPartSN.Text = this.GetLocalResourceObject(Pre + "_lblFaultyPartSN").ToString();
        lblFaultyPartSno.Text = this.GetLocalResourceObject(Pre + "_lblFaultyPartSno").ToString();
        */
        lblMajorPart.Text = this.GetLocalResourceObject(Pre + "_lblMajorPart").ToString();
        lblMark.Text = this.GetLocalResourceObject(Pre + "_lblMark").ToString();
        /* 2012-1-11，根据UC删除
        lblNewPartSN.Text = this.GetLocalResourceObject(Pre + "_lblNewPartSN").ToString();
        lblNewPartSno.Text = this.GetLocalResourceObject(Pre + "_lblNewPartSno").ToString();
        */
        lblObligation.Text = this.GetLocalResourceObject(Pre + "_lblObligation").ToString();
        /* 2012-1-11，根据UC删除
        lblPartType.Text = this.GetLocalResourceObject(Pre + "_lblPartType").ToString();
        */
        lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        lblResponsibility.Text = this.GetLocalResourceObject(Pre + "_lblReponsibility").ToString();
        lblSite.Text = this.GetLocalResourceObject(Pre + "_lblSite").ToString();
        lblSubDefect.Text = this.GetLocalResourceObject(Pre + "_lblSubDefect").ToString();
        lblTestStation.Text = this.GetLocalResourceObject(Pre + "_lblPiaTestStation").ToString();
        lblTrackingStatus.Text = this.GetLocalResourceObject(Pre + "_lblTrackingStatus").ToString();
        lblUncover.Text = this.GetLocalResourceObject(Pre + "_lblUncover").ToString();
        //lblVersion.Text = this.GetLocalResourceObject(Pre + "_lblVersion").ToString();
        //ITC-1360-0463, Jessica Liu, 2012-2-16
        //lblMAC.Text = this.GetLocalResourceObject(Pre + "_lblMAC").ToString();
        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnOK.InnerText = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.InnerText = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
    }

   private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        //scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("window.close();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.Form, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
