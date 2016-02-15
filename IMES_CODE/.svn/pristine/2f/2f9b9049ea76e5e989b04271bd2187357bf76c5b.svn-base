/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for RepairInfoQuery Page
 *             
 * UI:CI-MES12-SPEC-FA-UI RepairInfo Query.docx
 * UC:CI-MES12-SPEC-FA-UC RepairInfo Query.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-08-30  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Collections;
using System.Configuration;
using System.Web.Services;
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
using IMES.DataModel;

public partial class FA_RepairInfoEdit : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    [WebMethod]
    public static RepairInfo getRepairInfo(int id)
    {
        try
        {
            return ServiceAgent.getInstance().GetObjectByName<IRepairInfo>(WebConstant.RepairInfoObject).GetDefectInfo(id);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static void setRepairInfo(string proId, RepairInfo rep)
    {
        try
        {
            ServiceAgent.getInstance().GetObjectByName<IRepairInfo>(WebConstant.RepairInfoObject).EditRepairInfo(proId, rep);
            return;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                string customer = Request["Customer"];

                initLabel();
                this.cmbDefect.refreshDropContent("PRD");
                this.cmbSubDefect.Type = "QCSUB";

                this.cmbCause.Customer = customer;
                this.cmbCause.Stage = "FA";
                this.cmbMajorPart.Customer = customer;
                this.cmbMajorPart.MBpart = false;
                this.cmbComponent.Customer = customer;
                this.cmbTrackingStatus.Customer = customer;
                this.cmbObligation.Customer = customer;
                this.cmbDistribution.Customer = customer;
                this.cmbResponsibility.Customer = customer;
                this.cmb4M.Customer = customer;
                this.cmbCover.Customer = customer;
                this.cmbUncover.Customer = customer;
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
        this.lblDefect.Text = this.GetLocalResourceObject(Pre + "_lblDefect").ToString();
        this.lblTestStn.Text = this.GetLocalResourceObject(Pre + "_lblTestStn").ToString();
        this.lblSubDefect.Text = this.GetLocalResourceObject(Pre + "_lblSubDefect").ToString();
        this.lblCause.Text = this.GetLocalResourceObject(Pre + "_lblCause").ToString();
        this.lblMajorPart.Text = this.GetLocalResourceObject(Pre + "_lblMajorPart").ToString();
        this.lblComponent.Text = this.GetLocalResourceObject(Pre + "_lblComponent").ToString();
        this.lblSite.Text = this.GetLocalResourceObject(Pre + "_lblSite").ToString();
        this.lblTrackStat.Text = this.GetLocalResourceObject(Pre + "_lblTrackStat").ToString();
        this.lblObligation.Text = this.GetLocalResourceObject(Pre + "_lblObligation").ToString();
        this.lblMark.Text = this.GetLocalResourceObject(Pre + "_lblMark").ToString();
        this.lblAction.Text = this.GetLocalResourceObject(Pre + "_lblAction").ToString();
        this.lblDistribution.Text = this.GetLocalResourceObject(Pre + "_lblDistribution").ToString();
        this.lblResponsibility.Text = this.GetLocalResourceObject(Pre + "_lblResponsibility").ToString();
        this.lbl4M.Text = this.GetLocalResourceObject(Pre + "_lbl4M").ToString();
        this.lblCover.Text = this.GetLocalResourceObject(Pre + "_lblCover").ToString();
        this.lblUncover.Text = this.GetLocalResourceObject(Pre + "_lblUncover").ToString();
        this.lblOldPart.Text = this.GetLocalResourceObject(Pre + "_lblOldPart").ToString();
        this.lblNewPart.Text = this.GetLocalResourceObject(Pre + "_lblNewPart").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();

        this.btnOK.InnerText = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.InnerText = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
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
