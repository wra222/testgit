/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UpdateShipDate
 * CI-MES12-SPEC-PAK-UpdateShipDate.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
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


public partial class UpdateShipDate : IMESBasePage
{
    IUpdateShipDate currentBll = ServiceAgent.getInstance().GetObjectByName<IUpdateShipDate>(WebConstant.UpdateShipDateObject);
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    
    public String UserId;
    public String Customer;
    public String today;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            btnButton1.ServerClick += new EventHandler(btnButton1_ServerClick);
            btnButton2.ServerClick += new EventHandler(btnButton2_ServerClick);
            today = DateTime.Now.ToString("yyyy-MM-dd");
            if (!this.IsPostBack)
            {
                initLabel();
                //this.TextBox1.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
                this.station.Value = Request["Station"];
                this.pCode.Value = Request["PCode"];
                hidDN.Value = "";
                Customer = Master.userInfo.Customer;
                ReSuccess("");
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
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable InitDNTable()
    {
        DataTable dnTable = new DataTable();
        dnTable.Columns.Add("Status", Type.GetType("System.String"));
        dnTable.Columns.Add("ShipDate", Type.GetType("System.String"));
        dnTable.Columns.Add("Qty", Type.GetType("System.Int32"));
        return dnTable;
    }

    private void initLabel()
    {
        this.LabelDN.Text = this.GetLocalResourceObject(Pre + "_lblLabelDN").ToString();
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatus").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lblShipdate.Text = this.GetLocalResourceObject(Pre + "_lblShipdate").ToString();
        this.lblUpdateTo.Text = this.GetLocalResourceObject(Pre + "_lblUpdateTo").ToString();
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
    private void writeToInfo(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "writeToInfo", scriptBuilder.ToString(), false);

    }


    protected void btnButton1_ServerClick(object sender, EventArgs e)
    {

        string strDNNO = hidDN.Value;
        try
        {
            DNQueryCondition condition = new DNQueryCondition();
            condition.DeliveryNo = strDNNO;
            S_RowData_ShipDate dn = currentBll.GetDN(strDNNO);
            if (dn.dn == null || dn.dn == "")
            {
                
                ReReset();
                return;
            }
            ReStatus(dn.Status);
            ReShipDate(dn.ShipDate);
            ReQty(dn.Qty);
            /*if (dn.Status.Equals("98"))
            {
                writeToAlertMessage(this.GetLocalResourceObject(Pre + "_msgInvalidStatus").ToString());
                HIfUpdate.Value = "doNotSave";
            }*/
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
    private void ReStatus(string Status)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getStatus(\"" + Status + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "ReStatus", scriptBuilder.ToString(), false);
    }
    private void ReReset()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("SetPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "ReReset", scriptBuilder.ToString(), false);
    }
    private void ReShipDate(string ShipDate)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getShipDate(\"" + ShipDate + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "ReShipDate", scriptBuilder.ToString(), false);
    }
    private void ReSuccess(string display)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getSuccess(\"" + display + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "ReSuccess", scriptBuilder.ToString(), false);
    }
    private void ReSuccessFull()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getSuccessFull();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "ReSuccessFull", scriptBuilder.ToString(), false);
    }
    private void ReQty(string Qty)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQty(\"" + Qty + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanelAll, typeof(System.Object), "ReQty", scriptBuilder.ToString(), false);
    }

    protected void btnButton2_ServerClick(object sender, EventArgs e)
    {
        try
        {
            if (HIfUpdate.Value == "doNotSave" || hidDN.Value == "")
            {
                ReSuccess("");
                return;
            }
            string strDNNO = hidDN.Value;        
            DNUpdateCondition condition = new DNUpdateCondition();
            condition.DeliveryNo = strDNNO;
            condition.ShipDate = DateTime.Parse(hidDate.Value);
            currentBll.UpDN(strDNNO, hidDate.Value,UserId);
            ReSuccessFull();
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
            ReSuccess("");
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            ReSuccess("");
        }
        //ReSuccess("");
    }

}
