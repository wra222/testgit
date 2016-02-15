/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description:PCA ICT INPUT-04 MB ReInput
 * UI:CI-MES12-SPEC-SA-UI PCA ICT Input.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA ICT Input.docx         
 * Update: 
 * Date        Name                 Reason 
 * ==========  ==================== =====================================
 * 2012-01-18   Chen Xu             Create
 * 2012-02-06   Chen Xu             Modify:ITC-1360-0256
 * Known issues:
 * TODO：
 * UC 具体业务：  重流板子
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

public partial class PAK_MBReInput : System.Web.UI.Page
{
    
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
   

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            this.CmbPdLine1.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);
            this.CmbPdLine1.InnerDropDownList.AutoPostBack = true;

            if (!this.IsPostBack)
            {
                initLabel();

                ////注册下拉框的选择事件
                
                this.hiddenStation.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                
                this.CmbPdLine1.Station = Request["station"];
                this.CmbPdLine1.Customer = Master.userInfo.Customer;

                //ITC-1360-0256:对于没有站号的Station，按照Stage 取得Line 
                this.CmbPdLine1.Stage = "SA";
               
                
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

    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
            this.CmbPdLine1.Station = Request["station"];
            this.CmbPdLine1.Customer = Master.userInfo.Customer;
            this.CmbPdLine1.Stage = "SA";   
            
            if (this.CmbPdLine1.InnerDropDownList.SelectedValue == "")
            {
                alertNoPdLine();
                setPdLineFocus();
                ClearPageData();
            }
            else
            {
                ClearPageData();
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
    /// 初始化页面的静态label
    /// </summary>
    /// <returns></returns>
     private void initLabel()
    {
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblMBSno.Text = this.GetLocalResourceObject(Pre + "_lblMBSno").ToString();
        this.lblDCode.Text = this.GetLocalResourceObject(Pre + "_lblDCode").ToString();
        this.lblECR.Text = this.GetLocalResourceObject(Pre + "_lblECR").ToString();
        this.lblMAC.Text = this.GetLocalResourceObject(Pre + "_lblMAC").ToString();
        this.lblMBCT.Text = this.GetLocalResourceObject(Pre + "_lblMBCT").ToString();
        this.btnReInput.Value = this.GetLocalResourceObject(Pre + "_btnReInput").ToString();

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

    /// <summary>
    /// AlertErrorMessage for Family
    /// </summary>  
    private void alertNoPdLine()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (alertNoPdLine,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "alertSelectFamily", script, false);
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
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
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
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "setInputFocus", script, false);
    }

    /// <summary>
    /// Clear Page Data
    /// </summary>
    /// <param name="er"></param>
    private void ClearPageData()
    {
        String script = "<script language='javascript'>" + "\r\n" +
               "window.setTimeout(ClearData,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "ClearData", script, false);
    }
}