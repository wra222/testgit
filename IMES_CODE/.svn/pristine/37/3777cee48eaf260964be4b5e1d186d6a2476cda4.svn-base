/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-FA-UC Generate Customer SN.docx –2011/11/11 
* UC:CI-MES12-SPEC-FA-UI Generate Customer SN.docx –2011/11/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-1-29   Du.Xuan               Create   
* Known issues:
* ITC-1360-510 添加Reason部分 
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class FA_GenerateCustomerSNReprint : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    private const int DEFAULT_ROWS = 12;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
            }
            if (!this.IsPostBack)
            {
                initLabel();
                  
                this.station.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                
                //UserId = Master.userInfo.UserId;
                //Customer = Master.userInfo.Customer;

                //this.btnRePrint.Visible = false;

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
    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void initLabel()
    {
        //this.lblProdID.Text = this.GetLocalResourceObject(Pre + "_lblProdID").ToString();
        //this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.lblReason.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblReason");
        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet"); 
        this.btnRePrint.InnerText = this.GetLocalResourceObject(Pre + "_btnRePrint").ToString();
    }
   
   /// <summary>
   /// 输出错误信息
   /// </summary>
   /// <param name="er"></param>
   //private void writeToAlertMessage(String ex)
   //{
   //    String script = "<script language='javascript'>" + "\r\n" +
   //        "ShowMessage('" + ex + "');" + "\r\n" +
   //        "</script>";
   //    ScriptManager.RegisterStartupScript(this.UpdatePanelAll, ClientScript.GetType(), "writeToAlertMessageAgent", script, false);
   //}

   private void writeToAlertMessage(string errorMsg)
   {

       StringBuilder scriptBuilder = new StringBuilder();

       scriptBuilder.AppendLine("<script language='javascript'>");
       scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
       scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
       scriptBuilder.AppendLine("</script>");

       ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

   }

   private void setResetPage()
   {
       StringBuilder scriptBuilder = new StringBuilder();

       scriptBuilder.AppendLine("<script language='javascript'>");
       scriptBuilder.AppendLine("ResetPage(\"" + "\");");
       scriptBuilder.AppendLine("</script>");

       ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "ResetPage", scriptBuilder.ToString(), false);

   }


}
