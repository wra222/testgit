
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
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Docking.Interface.DockingIntf;

public partial class FA_FARepairAddEdit : IMESBasePage
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //ITC-1122-0148 Tong.Zhi-Yong 2010-02-27
    //public string Domain = WebCommonMethod.getConfiguration("DocumentDomain");
    //ITC-1414-0138
    IFARepairForDocking iFARepair = (IFARepairForDocking)ServiceAgent.getInstance().GetObjectByName<IFARepairForDocking>(WebConstant.FARepairForDocking);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Vincent for new selected logic mark
            //this.cmbDefect.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbDefect_Selected);
            this.cmbDefect.InnerDropDownList.AutoPostBack = true;
            if (!this.IsPostBack)
            {
                //string customer = Master.userInfo.Customer;
                string customer = Request["Customer"];
                

                initLabel();

                this.cmbDefect.refreshDropContent("PRD");

                this.cmbObligation.Customer = customer;
                this.cmbObligation.refreshDropContent();                

                this.cmbMark.refreshDropContent();

                this.cmbMajorPart.Customer = customer;
                this.cmbMajorPart.MBpart = false;
                this.cmbMajorPart.refreshDropContent(string.Empty);

                this.cmbComponent.Customer = customer;
                this.cmbComponent.refreshDropContent();

                this.cmbCause.Customer = customer;                
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

    #region mark by vincent
    //private void cmbDefect_Selected(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        if (this.cmbDefect.InnerDropDownList.SelectedValue == "")
    //        {
    //            this.txtStation.Value = "";
    //        }
    //        else
    //        {
    //            string code = this.cmbDefect.InnerDropDownList.SelectedValue;
    //            string cause = this.cmbCause.InnerDropDownList.SelectedValue;
    //            IList<DefectCodeStationInfo> returnList = new List<DefectCodeStationInfo>();
    //            returnList = iFARepair.GetReturnStationForAdd(code, cause, this.hidProdId.Value);

    //            if (returnList != null && returnList.Count > 0)
    //            {
    //                this.txtStation.Value = returnList[0].nxt_stn;
    //            }
    //            else
    //            {
    //                returnList = iFARepair.GetReturnStationForAdd(code, "", this.hidProdId.Value);
    //                if (returnList != null && returnList.Count > 0)
    //                {
    //                    this.txtStation.Value = returnList[0].nxt_stn;
    //                }
    //                else
    //                {
    //                    this.txtStation.Value = "";
    //                    showErrorMessage("Please IE Maintain Return Station!");
    //                }
    //            }
    //            updatePanel2.Update();
    //        }
    //    }
    //    catch (FisException ee)
    //    {
    //        showErrorMessage(ee.mErrmsg);
    //    }
    //    catch (Exception ex)
    //    {
    //        showErrorMessage(ex.Message);
    //    }
    //}
    #endregion

    private void initLabel()
    {
        //this.lbl.Text = this.GetLocalResourceObject(Pre + "_lbl").ToString(); Dean 20110324 不算成退
        this.lblCause.Text = this.GetLocalResourceObject(Pre + "_lblCause").ToString();
        this.lblComponent.Text = this.GetLocalResourceObject(Pre + "_lblComponent").ToString();
        this.lblDefect.Text = this.GetLocalResourceObject(Pre + "_lblDefect").ToString();
        //this.lblFaultyPartSN.Text = this.GetLocalResourceObject(Pre + "_lblFaultyPartSN").ToString();
        this.lblFaultyPartSno.Text = this.GetLocalResourceObject(Pre + "_lblFaultyPartSno").ToString();
        this.lblMajorPart.Text = this.GetLocalResourceObject(Pre + "_lblMajorPart").ToString();
        this.lblMark.Text = this.GetLocalResourceObject(Pre + "_lblMark").ToString();
        //this.lblNewPartSN.Text = this.GetLocalResourceObject(Pre + "_lblNewPartSN").ToString();
        this.lblNewPartSno.Text = this.GetLocalResourceObject(Pre + "_lblNewPartSno").ToString();
        this.lblObligation.Text = this.GetLocalResourceObject(Pre + "_lblObligation").ToString();
        this.lblPartType.Text = this.GetLocalResourceObject(Pre + "_lblPartType").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.lblSite.Text = this.GetLocalResourceObject(Pre + "_lblSite").ToString();
        //this.lblTestStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();
        //this.lblVersion.Text = this.GetLocalResourceObject(Pre + "_lblVersion").ToString();
        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnOK.InnerText = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.InnerText = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblMac.Text = this.GetLocalResourceObject(Pre + "_lblMac").ToString();
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");                
        //scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("window.close();");
        scriptBuilder.AppendLine("</script>");
        //ScriptManager.RegisterStartupScript(this.Form, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
