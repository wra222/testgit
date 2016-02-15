/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: MAC Print
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-03-15  Chen Xu(EB1-4)       Create 
 * Known issues:
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

public partial class FA_MVunpack : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public String AccountId;
    public String UserName;
    public String Login;
    public String station;
    private Object commServiceObj;
    private ITestStation iTestStation;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                initLabel();

                this.CmbReturnStation.Type = "FATest";
                
                this.CmbReturnStation.Customer = Master.userInfo.Customer;
                station = Request["Station"]; //站号
                AccountId = Request["AccountId"];
                UserName = Request["UserName"];
                Login = Request["Login"];
                //this.pCode.Value = Request["PCode"];
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                commServiceObj = ServiceAgent.getInstance().GetObjectByName<ITestStation>(WebConstant.CommonObject);
                //setFocus();
                iTestStation = (ITestStation)commServiceObj;
                refreshCmbReturnStation("");
                refreshCmbReturnStation("1");
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
    private void refreshCmbReturnStation( string status)
    {
        if (!string.IsNullOrEmpty(status))
        {
            IList<StationInfo> returnList = null;
            returnList = iTestStation.GetStationListByType("FATest");
            int index = 0;
            int selectIndex = -1;
            if (returnList != null && returnList.Count > 0)
            {

                this.CmbReturnStation.clearContent();
                foreach (StationInfo temp in returnList)
                {
                    ListItem item = null;
                    string value = temp.StationId + " " + temp.Descr;
                    item = new ListItem(value, temp.StationId);
                    this.CmbReturnStation.InnerDropDownList.Items.Add(item);
                    if (temp.StationId == "55")
                        selectIndex = index;
                    index++;
                }
                
            }
            else
            {
                this.CmbReturnStation.clearContent();
            }
            if (selectIndex != -1)
            {
                this.CmbReturnStation.setSelected(selectIndex+1);
            }
        }
        else
        {
            this.CmbReturnStation.clearContent();
        }
    }
    private void initLabel()
    {
        this.lbRetStation.Text = this.GetLocalResourceObject(Pre + "_lbRetStation").ToString();
        this.lbCustSN.Text = this.GetLocalResourceObject(Pre + "_lbCustSN").ToString();
       // refreshCmbReturnStation("1");
        
    }

    public void hiddenbtn_Click(object sender, EventArgs e)
    {
        setFocus();
    }
   
    private void setFocus()
    {

        String script = "<script language='javascript'>  getCommonInputObject().focus();; </script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, ClientScript.GetType(), "setFocus", script, false);
    }

    private void writeToAlertMessage(string errorMsg)
    {


        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }
   
}
