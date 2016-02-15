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

public partial class FA_TravelCardWithCollectKP : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ITravelCardWithCollectKP iTravelCardWithCollectKP = ServiceAgent.getInstance().GetObjectByName<ITravelCardWithCollectKP>(WebConstant.ITravelCardWithCollectKP);
    public string userId;
    public string customer;
    public string station;
    public string pCode;
   // public string station;
    public String AccountId;
    public String Login;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
          
        {
     //   cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
        this.cmbFamily.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbFamily_Selected);
        cmbPdLine.InnerDropDownList.AutoPostBack = true;
        userId = Master.userInfo.UserId;
        customer = Master.userInfo.Customer;  
            if (!Page.IsPostBack)
            {
              
                InitLabel();
            
                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = customer;
                station = Request["Station"];
                pCode = Request["PCode"];
                cmbFamily.Service = "016";
				AccountId = Master.userInfo.AccountId.ToString();
                Login = Master.userInfo.Login;
                FamilyInfoDef def = new FamilyInfoDef();
                def.name = "Category";
                def.value = Request["FamilyCategory"];
                cmbFamily.FamilyInfoDefObj = def;
       
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
    private void cmbFamily_Selected(object sender, System.EventArgs e)
    {
        try
        {
           this.cmbModel.Service = "002";
           if (this.cmbFamily.InnerDropDownList.SelectedValue == "")
            {
                this.cmbModel.clearContent();
         
            }
            else
            {
               this.cmbModel.refreshDropContent(this.cmbFamily.InnerDropDownList.SelectedValue);
                if (this.cmbModel.InnerDropDownList.Items.Count > 1)
                {
                    this.cmbModel.InnerDropDownList.SelectedIndex = 1;
              
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
   

    private void InitLabel()
    {
        
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.btpPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrtSet").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    //    setPdLineCombFocus();

    }

    
    private void writeToAlertMessage(string errorMsg)
    {
      

        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }
   
    

  

    private void callInputRun()
    {

        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "callInputRun", script, false);
    }

    /// <summary>
    /// 置快速控件焦点
    /// </summary>
    private void setFocus()
    {

        String script = "<script language='javascript'>  getCommonInputObject().focus();; </script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "setFocus", script, false);
    }

    /// <summary>
    /// 置快速控件焦点
    /// </summary>
    private void setPdLineCombFocus()
    {

        String script = "<script language='javascript'>  window.setTimeout (setPdLineCmbFocus,100); </script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }
  

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    
}
