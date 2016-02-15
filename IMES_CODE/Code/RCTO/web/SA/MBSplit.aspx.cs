/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:MB Split
 * UI:CI-MES12-SPEC-SA-UI MB Split.docx 
 * UC:CI-MES12-SPEC-SA-UC MB Split.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13   Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	连板切割入口，实现连板的切割
 *                2.	打印子板标签
 * UC Revision: 3924
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;
using com.inventec.imes.DBUtility;

public partial class SA_MBSplit : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IVirtualMo iVirtualMo = ServiceAgent.getInstance().GetObjectByName<IVirtualMo>(WebConstant.VirtualMoObject);

    public String UserId;
    public String Customer;
    public int initRowsCount = 6;

    public string curFamily = string.Empty;
    public string curModel = string.Empty;
    public string curStation = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////注册下拉框的选择事件
            this.CmbPdLine1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            this.CmbPdLine1.InnerDropDownList.AutoPostBack = true;

            if (!IsPostBack)
            {

                InitLbl();
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                
                curStation = Request["station"];

                this.CmbPdLine1.Station = curStation;
                this.CmbPdLine1.Customer = Customer;
             //   this.CmbPdLine1.Stage = "SA";
                setPdLineFocus();
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }


    private void InitLbl()
    {
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblMBSno.Text = this.GetLocalResourceObject(Pre + "_lblMBSno").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnReprint.Value = this.GetLocalResourceObject(Pre + "_btnReprint").ToString();
    }


    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
            this.CmbPdLine1.Station = Request["station"];
            this.CmbPdLine1.Customer = Master.userInfo.Customer;
        //    this.CmbPdLine1.Stage = "SA";

            if (this.CmbPdLine1.InnerDropDownList.SelectedValue == "")
            {
                alertNoPdLine();
                setPdLineFocus();
            }
            else
            {
                setDataEntryFocus();
            }

        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// setPdLineCmbFocus
    /// </summary>
    /// <param name="er"></param>
    private void setPdLineFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
               "window.setTimeout(setPdLineCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }

    /// <summary>
    /// AlertErrorMessage for Family
    /// </summary>  
    private void alertNoPdLine()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (alertNoPdLine,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertSelectFamily", script, false);
    }


    /// <summary>
    /// setDataEntryFocus
    /// </summary>
    /// <param name="er"></param>
    private void setDataEntryFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
               "window.setTimeout(setInputFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }
}
