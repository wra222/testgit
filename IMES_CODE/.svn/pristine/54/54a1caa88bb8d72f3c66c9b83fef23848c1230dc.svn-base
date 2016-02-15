/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:
* UI:CI-MES12-SPEC-PAK-UI-Combine Carton in DN
* UC:CI-MES12-SPEC-PAK-UC-Combine Carton in DN          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/09/07    ITC000052             Create   
* TODO：
* 
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
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;

public partial class PAK_CombineCartonInDNForRCTO : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    //private IDefect iDefect;
    public String UserId;
    public String Customer;
    public String Station;
    public String Pcode;
    public int deliveryNum = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!this.IsPostBack)
            {
                initLabel();

                Station = Request.QueryString["Station"];
                Pcode = Request.QueryString["PCode"];
                if (string.IsNullOrEmpty(Station))
                {
                    Station = "81";
                }

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                this.cmbDelivery.Model = "";
            }
            this.cmbDelivery.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbDelivery_Selected);
            this.cmbDelivery.InnerDropDownList.AutoPostBack = true;
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.Message);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void initLabel()
    {
        this.lblDelivery.Text = this.GetLocalResourceObject(Pre + "_lblDelivery").ToString();
        this.TitleDelivery.Text = this.GetLocalResourceObject(Pre + "_TitleDelivery").ToString();
        this.lblTotalQty.Text = this.GetLocalResourceObject(Pre + "_lblTotalQty").ToString();
        this.lblPackedQty.Text = this.GetLocalResourceObject(Pre + "_lblPackedQty").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet");
    }


    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }
    public void btnGetDn_Click(object sender, System.EventArgs e)
    {
        try
        {
            string model = this.modelHidden.Value.Trim();
            this.cmbDelivery.refresh(model, "00");

            deliveryNum = this.cmbDelivery.deliveryNum;
            this.dnListHidden.Value = Convert.ToString(deliveryNum);

            String script = "<script language='javascript'> setDelivery(); </script>";
            ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "setDelivery", script, false);
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

    public void btnResetDnInfo_Click(object sender, System.EventArgs e)
    {
        try
        {
            cmbDelivery_Selected(sender, e);
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
    private void cmbDelivery_Selected(object sender, System.EventArgs e)
    {
        try
        {
            ICombineCartonInDNForRCTO CombineCartonInDN = ServiceAgent.getInstance().GetObjectByName<ICombineCartonInDNForRCTO>(WebConstant.CombineCartonInDNObjectForRCTO);
            ArrayList retList = new ArrayList();
            string dnno = this.cmbDelivery.InnerDropDownList.SelectedValue;
            retList = CombineCartonInDN.getDeliveryInfo(dnno);

            this.dnQtyHidden.Value = Convert.ToString(retList[0]);
            this.dnPcsHidden.Value = Convert.ToString(retList[1]);
            this.dnPackedHidden.Value = Convert.ToString(retList[2]);
            this.deliverymodelHidden.Value = Convert.ToString(retList[3]);
            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine("<script language='javascript'>");
            scriptBuilder.AppendLine("ChangeDn();");
            scriptBuilder.AppendLine("</script>");
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "ChangeDn", scriptBuilder.ToString(), false);
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

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
    }

    private void callInputRun()
    {
        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "callInputRun", script, false);
    }

}

