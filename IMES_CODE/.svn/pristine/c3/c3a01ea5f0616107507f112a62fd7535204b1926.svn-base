/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:PDPA Label 01
* UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx –2011/10/28 
* UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx –2011/10/28           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-10-20   Du.Xuan               Create   
* Known issues:
* [ITC-1360-0003][Code下拉框不能显示]:UC变更，代码未随之修改。已经变更逻辑为新的UC描述
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
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.DataModel;

public partial class PAK_PDPALabel01_CQ : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPDPALabel01 pdpalabel = ServiceAgent.getInstance().GetObjectByName<IPDPALabel01>(WebConstant.IPDPALabel01);

    private const int DEFAULT_ROWS = 12;
    //private IDefect iDefect;
    public String UserId;
    public String Customer;
    public String Station;
    public String Pcode;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //this.CmbCode.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCode_Selected);
            Station = Request.QueryString["Station"];
            Pcode = Request.QueryString["PCode"];

            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                initLabel();
                //this.gd.DataSource = getNullDataTable();
                //this.gd.DataBind();
                //initTableColumnHeader();
                bindTable(DEFAULT_ROWS);

                
                this.CmbPdLine1.Station = Station;
                this.CmbPdLine1.Customer = Master.userInfo.Customer;
                //this.CmbPdLine1.Stage = "PAK";

                //ITC-1360-0003
                this.CmbCode.CodeType = "PAK Label";
                //setColumnWidth();
                //setFocus();
				
				string sMax = "javascript:maximizeImage(this);return true;";
				string sMin = "javascript:minimizeImage(this); return true;";
				ShowImage0.Attributes["onMouseOver"] = sMax;
				ShowImage0.Attributes["onMouseOut"] = sMin;
				ShowImage1.Attributes["onMouseOver"] = sMax;
				ShowImage1.Attributes["onMouseOut"] = sMin;
				ShowImage2.Attributes["onMouseOver"] = sMax;
				ShowImage2.Attributes["onMouseOut"] = sMin;
				ShowImage3.Attributes["onMouseOver"] = sMax;
				ShowImage3.Attributes["onMouseOut"] = sMin;
				ShowImage4.Attributes["onMouseOver"] = sMax;
				ShowImage4.Attributes["onMouseOut"] = sMin;
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.Message);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    private void initLabel()
    {
        this.lblPDLine.Text = this.GetLocalResourceObject(Pre + "_lblPDLine").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblFloor.Text = this.GetLocalResourceObject(Pre + "_lblFloor").ToString();
        this.lblCustSN.Text = this.GetLocalResourceObject(Pre + "_lblCustSN").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblProductInfo.Text = this.GetLocalResourceObject(Pre + "_lblProductInfo").ToString();
        this.lblLabelList.Text = this.GetLocalResourceObject(Pre + "_lblLabelList").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        
        this.QueryChk.Text = this.GetLocalResourceObject(Pre + "_QueryChk").ToString();

        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet");
        this.btnReprint.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnReprint");
        //this.CmbPdLine1.InnerDropDownList.SelectedIndex = 1;
    }

    private void initTableColumnHeader()
    {
        this.gd.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_colPartNo").ToString();
        this.gd.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colTp").ToString();
        this.gd.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_colQty").ToString();
        
    }

    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colTp").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }

    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
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


    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
    }

    private void callInputRun()
    {
        String script = "<script language='javascript'> getAvailableData('processDataEntry'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanel, ClientScript.GetType(), "callInputRun", script, false);
    }

}
