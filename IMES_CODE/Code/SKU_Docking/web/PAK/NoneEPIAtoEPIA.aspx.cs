/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: NoneEPIAtoEPIA
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-09   zhu lei          Create 
 * 
 * Known issues:Any restrictions about this file 
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
using IMES.Infrastructure;
using System.Collections.Generic;
using System.Text;
using IMES.DataModel;

public partial class PAK_NoneEPIAtoEPIA : IMESBasePage
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private IForceEOQC iForceEOQC = ServiceAgent.getInstance().GetObjectByName<IForceEOQC>(WebConstant.ForceEOQC);
    private Object commServiceObj;
    private IProduct iProduct;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();
                setFocus();
            }

            commServiceObj = ServiceAgent.getInstance().GetObjectByName<IQCStatistic>(WebConstant.CommonObject);
            iProduct = ServiceAgent.getInstance().GetObjectByName<IProduct>(WebConstant.CommonObject);
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
        this.lblProdID.Text = this.GetLocalResourceObject(Pre + "_lblCustSN").ToString();       
    }

    protected void hidbtn_ServerClick(Object sender, EventArgs e)
    {
        string prodId = this.hidProdID.Value.Trim();
        string PdLine = string.Empty;
        string station = Request["station"];

        try
        {
            string editor = Master.userInfo.UserId;
            string customer = Master.userInfo.Customer;
            
            bool returnStationFlag = false;
            PdLine = iProduct.GetProductStatusInfo(prodId).pdLine;
            returnStationFlag = iForceEOQC.InputProdId(PdLine, prodId, editor, station, customer);
            if (returnStationFlag)
            {
                StringBuilder scriptBuilder = new StringBuilder();
                string smsg = this.GetLocalResourceObject(Pre + "_msgEOQC").ToString();
                scriptBuilder.AppendLine("<script language='javascript'>");
                scriptBuilder.AppendLine("ShowSuccessfulInfo(true, \"" + smsg + "\");");
                scriptBuilder.AppendLine("initPage();");
                scriptBuilder.AppendLine("</script>");

                ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showStationMessage", scriptBuilder.ToString(), false);
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
        finally
        {
            hideWait();
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }
}
