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
using System.Xml.Linq;
using com.inventec.iMESWEB;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

public partial class PAK_AssignWHLocation : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string userId;
    public string customer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try {
             if (!Page.IsPostBack)
            {
                InitLabel();
                this.station.Value = Request["Station"]; 
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
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
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbFloor.Text = this.GetLocalResourceObject(Pre + "_lblFloor").ToString();
        //-- this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbCustomerSn.Text = this.GetLocalResourceObject(Pre + "_lblCustomerSn").ToString();
        this.lbProdId.Text = this.GetLocalResourceObject(Pre + "_lblProdId").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lbLocation.Text = this.GetLocalResourceObject(Pre + "_lblLocation").ToString();
        this.lbQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();

        this.lblProductInfo.Text = this.GetLocalResourceObject(Pre + "_lblProductInfo").ToString();
        this.lblWHLocation_Info.Text = this.GetLocalResourceObject(Pre + "_lblWHLocation_Info").ToString();
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
    /// 置快速控件焦点
    /// </summary>
    //private void setPdLineCombFocus()
    //{
    //
    //    String script = "<script language='javascript'>  window.setTimeout (setPdLineCmbFocus,100); </script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    //}

    /// <summary>
    /// 置快速控件焦点
    /// </summary>
    private void setFloorCmbFocus()
    {

        String script = "<script language='javascript'>  window.setTimeout (setFloorCmbFocus,100); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setFloorCmbFocus", script, false);
    }
}