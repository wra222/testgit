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


public partial class FA_ChangeToPIAEPIA : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //IBoardInput iBoardInput = ServiceAgent.getInstance().GetObjectByName<IBoardInput>(WebConstant.BoardInputObject);
    public String UserId;
    public String Customer;
    public int initRowsCount = 6;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
          
        {
       // cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
       // cmbPdLine.InnerDropDownList.AutoPostBack = true;
            if (!Page.IsPostBack)
            {     
                InitLabel();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                this.station.Value = Request["Station"];
                //this.pCode.Value = Request["PCode"];
                setFocus();
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
        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
    
       // this.btpPrintSet.Value = this.GetLocalResourceObject(Pre + "_btnPrtSet").ToString();
        this.labQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();

        ///////////////////////
        this.lblCustomSN.Text = this.GetLocalResourceObject(Pre + "_lblCustomSN").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblChangeToEPIA.Text = this.GetLocalResourceObject(Pre + "_radChangeToEPIA").ToString();
        this.lblEPIAChangeToPIA.Text = this.GetLocalResourceObject(Pre + "_radEPIAChangeToPIA").ToString();


    }

    /// <summary>
    /// 输出错误信息
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


    /// <summary>
    /// 快速控件等待再次输入
    /// </summary>
    private void callInputRun()
    {

        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callInputRun", script, false);
    }

  
    /// <summary>
    /// 扫入ProdId成功，向Label框中赋值
    /// </summary>
    private void setProdId(string value)
    {
        try
        {
            this.txtProdId.Text = value;
            this.UpdatePanel1.Update();
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

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void setScanHiddenQty()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "setScanHiddenQty();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setScanHiddenQty", script, false);
    }
    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }
}
