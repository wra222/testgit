/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WEB/Docking/ProdIdPrint Page
 * UI:CI-MES12-SPEC-FA-UI ProdId Print For Docking.docx –2012/5/22 
 * UC:CI-MES12-SPEC-FA-UC ProdId Print For Docking.docx –2012/5/22            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-22   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using IMES.Docking.Interface.DockingIntf;



public partial class ProdIdSinglePrint : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IMO iMO = ServiceAgent.getInstance().GetObjectByName<IMO>(WebConstant.CommonObject);

    IProdIdPrintForDocking iProdIdPrint = ServiceAgent.getInstance().GetObjectByName<IProdIdPrintForDocking>(WebConstant.ProdIdPrintForDocking);
    public string userId;
    public string customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Pre = this.GetLocalResourceObject("language").ToString();
            this.cmbFamily.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);
            this.cmbModel.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbModel_Selected);
            this.cmbMO.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbMO_Selected);

            InitLabel();

            if (!Page.IsPostBack)
            {
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
                this.pCode.Value = Request["PCode"];
                this.station1.Value = Request["Station"];
                this.editor1.Value = userId;
                this.customer1.Value = customer;

                this.cmbPdLine.Customer = customer;
                //this.cmbPdLine.Station = "F0";
                this.cmbPdLine.Station = Request["Station"];

                this.cmbFamily.Service = "ProdIdPrintForDocking";
                this.cmbFamily.Customer = customer;

                this.cmbModel.Service = "ProdIdPrintForDocking";
                this.cmbMO.Service = "ProdIdPrintForDocking";
            }
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

    /// <summary>
    /// </summary>
    private void cmbFamily_Selected(object sender, System.EventArgs e)
    {
        try
        {
            this.cmbMO.Service = "ProdIdPrintForDocking";
            this.cmbModel.Service = "ProdIdPrintForDocking";


            if (this.cmbFamily.InnerDropDownList.SelectedValue == "")
            {
                this.cmbModel.clearContent();
                this.cmbMO.clearContent();
                this.lbShowMoQty.Text = "";
                this.lbShowReQty.Text = "";
                this.UpdatePanel1.Update();
                this.UpdatePanel2.Update();
            }
            else
            {
                this.cmbModel.refreshDropContent(this.cmbFamily.InnerDropDownList.SelectedValue);
                if (this.cmbModel.InnerDropDownList.Items.Count > 1)
                {
                    this.cmbModel.InnerDropDownList.SelectedIndex = 1;
                    cmbModel_Selected(sender, e);
                }
                else
                {
                    this.cmbMO.clearContent();
                    this.lbShowMoQty.Text = "";
                    this.lbShowReQty.Text = "";
                    this.UpdatePanel1.Update();
                    this.UpdatePanel2.Update();
                }
            }
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

    /// <summary>
    /// </summary>
    private void cmbModel_Selected(object sender, System.EventArgs e)
    {
        try
        {
            this.cmbMO.Service = "ProdIdPrintForDocking";
            this.cmbModel.Service = "ProdIdPrintForDocking";
            if (this.cmbModel.InnerDropDownList.SelectedValue == "")
            {
                this.cmbMO.clearContent();
                this.lbShowMoQty.Text = "";
                this.lbShowReQty.Text = "";
                this.UpdatePanel1.Update();
                this.UpdatePanel2.Update();
            }
            else
            {
                this.cmbMO.refreshDropContent(this.cmbModel.InnerDropDownList.SelectedValue);
                if (this.cmbMO.InnerDropDownList.Items.Count > 1)
                {
                    this.cmbMO.InnerDropDownList.SelectedIndex = 1;
                    cmbMO_Selected(sender, e);
                }
                else
                {
                    this.lbShowMoQty.Text = "";
                    this.lbShowReQty.Text = "";
                    this.UpdatePanel1.Update();
                    this.UpdatePanel2.Update();
                }
            }
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

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbMO_Selected(object sender, System.EventArgs e)
    {
        int remainQty;
        if (this.cmbMO.InnerDropDownList.SelectedValue == "")
        {
            this.lbShowMoQty.Text = "";
            this.lbShowReQty.Text = "";
        }
        else
        {
            try
            {
                MOInfo moInfo = iProdIdPrint.GetMOInfo(this.cmbMO.InnerDropDownList.SelectedValue);
                this.lbShowMoQty.Text = moInfo.qty.ToString();
                remainQty = moInfo.qty - moInfo.pqty;
                this.lbShowReQty.Text = remainQty.ToString();
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
        this.UpdatePanel1.Update();
        this.UpdatePanel2.Update();
    }

    /// <summary>
    /// </summary>
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbMO.Text = this.GetLocalResourceObject(Pre + "_lblMO").ToString();
        this.lbMoQty.Text = this.GetLocalResourceObject(Pre + "_lblMOQty").ToString();
        this.lbReQty.Text = this.GetLocalResourceObject(Pre + "_lblRemainQty").ToString();
        this.lbPeriod.Text = this.GetLocalResourceObject(Pre + "_lblPeriod").ToString();
        this.lbThisMonth.Text = this.GetLocalResourceObject(Pre + "_lblTMonth").ToString();
        this.lbNextMonth.Text = this.GetLocalResourceObject(Pre + "_lblNMonth").ToString();
        this.lbQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lbECR.Text = this.GetLocalResourceObject(Pre + "_lblECR").ToString();
        
        
        this.btnPrint.Value = this.GetLocalResourceObject(Pre + "_btnPrint").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
        this.btpPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
    }

    /// <summary>
    /// </summary>
    private void cmbModel_Selected1(object sender, System.EventArgs e)
    {
        try
        {
            this.cmbMO.Service = "013";
            this.cmbModel.Service = "013";

            if (this.cmbModel.InnerDropDownList.SelectedValue == "")
            {
                this.cmbMO.clearContent();
            }
            else
            {
                this.cmbMO.refreshDropContent(this.cmbModel.InnerDropDownList.SelectedValue);
            }

            this.lbShowMoQty.Text = "";
            this.lbShowReQty.Text = "";
            this.UpdatePanel1.Update();
            this.UpdatePanel2.Update();
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
    /// <summary>
    /// µ÷ÓÃweb service´òÓ¡½Ó¿Ú³É¹¦ºóÐèÒªresetÒ³ÃæÐÅÏ¢
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {
            showSuccessInfo1();
            if (this.txtQty.Value == this.lbShowReQty.Text)
            {
                //MB Label ÒÑ¾­È«²¿ÁÐÓ¡Íê±ÏÊ±£¬ÐèÒª½«¸ÃMO ´ÓMO ÏÂÀ­ÁÐ±íÖÐÉ¾³ý£¬µ±Ç°Ñ¡ÔñMO Îª¿Õ£¬Çå¿ÕMO ÊýÁ¿£¬Ê£ÓàÊýÁ¿£»ÆäËûÄÚÈÝ±£³Ö²»±ä
                cmbModel_Selected1(sender, e);
            }
            else
            {
                cmbMO_Selected(sender, e);
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            endWaitingCoverDiv();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            endWaitingCoverDiv();
        }
    }

    /// <summary>
    /// resetÒ³ÃæÐÅÏ¢
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            showSuccessInfo2();
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

    /// <summary>
    /// Êä³ö´íÎóÐÅÏ¢
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void showSuccessInfo1()
    {
        String script = "<script language='javascript'>" + "\r\n" +
             "test();" + "\r\n" +
             "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "showSuccessInfo1", script, false);
    }

    private void showSuccessInfo2()
    {
        String script = "<script language='javascript'>" + "\r\n" +
             "test1();" + "\r\n" +
             "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "showSuccessInfo2", script, false);
    }

}
