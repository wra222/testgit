


/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PACosmetic
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei            Create 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Station.Interface.StationIntf;

public partial class PAK_OnlineProductIDHold : System.Web.UI.Page
{
    public string[] GvQueryColumnName = { "CUSTSN","ProductID","Model","PreStation","PreStatus",
                                            "PdLine","HoldStation","HoldUser","HoldTime","HoldCode",
                                            "HoldDescr"};
    public int[] GvQueryColumnNameWidth = { 45,45,50,40,40,
                                            35,45,45,65,40,
                                            50};
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IReleaseProductIDHold iReleaseProductIDHold = ServiceAgent.getInstance().GetObjectByName<IReleaseProductIDHold>(WebConstant.ReleaseProductIDHoldObject);
    private const int DEFAULT_ROWS = 6;
    public String UserId;
    public String Customer;
    public String GuidCode;
    public string HoldStationValue;
    public string[] HoldStationList;

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
                HoldStationValue = Request["HoldStation"] ?? "";
                initLabel();
             
              
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
   
    private void initLabel()
    {
        this.Panel3.GroupingText = this.GetLocalResourceObject(Pre + "_pnlInputDefectList").ToString();
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

        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updHidden, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
  
}

