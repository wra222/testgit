/*
* INVENTEC corporation ©2012 all rights reserved. 
* Description:FA MB Return
* CI-MES12-SPEC-PAK-FA MB Return.docx –2012/1/10           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-1-10   207003                Create   
* Known issues:
* TODO：
* 
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

public partial class SA_FAMBReturn : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String Station;
    IFAMBReturn iFAMBReturn = ServiceAgent.getInstance().GetObjectByName<IFAMBReturn>(WebConstant.FAMBReturnObject);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            this.TextBox1.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
            btnSave.ServerClick += new EventHandler(btnSave_ServerClick);
            btnMakeSure.ServerClick += new EventHandler(btnMakeSure_ServerClick);
            btnCancel.ServerClick += new EventHandler(btnCancel_ServerClick);
            if (null == Station || "" == Station)
            {
                Station = Request["Station"];
            }
            if (!this.IsPostBack)
            {
                if (null == Station || "" == Station)
                {
                    Station = Request["Station"];
                }
                initLabel();
                setFocus();
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

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void initLabel()
    {
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
        this.btnFAMBReturn.Value = this.GetLocalResourceObject(Pre + "_btnFAMBReturn").ToString();
        this.btnMBClear.Value = this.GetLocalResourceObject(Pre + "_btnMBClear").ToString();
    }
    private void btnSave_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            iFAMBReturn.Cancel(HMBSno.Value);
            if (null == Station)
            {
                iFAMBReturn.OnMBSNSave("", UserId, "", Customer, HMBSno.Value);
            }
            else
            {
                iFAMBReturn.OnMBSNSave("", UserId, Station, Customer, HMBSno.Value);
            }
            ReConfirm();
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            ReReset();
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            ReReset();
            return;
        }
        
    }
    private void btnMakeSure_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
             iFAMBReturn.MakeSureSave(HMBSno.Value, true);
             ReSuccess();
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            ReReset();
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            ReReset();
            return;
        }
    }
    private void btnCancel_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            iFAMBReturn.MakeSureSave(HMBSno.Value, false);
            ReReset();
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            ReReset();
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            ReReset();
            return;
        }
    }

    private void ReSuccess()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getSuccess();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReSuccess", scriptBuilder.ToString(), false);
    }
    private void ReConfirm()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getConfirm();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReConfirm", scriptBuilder.ToString(), false);
    }
    private void ReReset()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ResetPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReReset", scriptBuilder.ToString(), false);
    }
    private void setFocus()
    {
        //String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        //ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
        //DEBUG ITC-1360-0352
        String script = "<script language='javascript'>" + "\r\n" +
                        "window.setTimeout (callNextInput,100);" + "\r\n" +
                        "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "callNextInput", script, false);
    }
}
