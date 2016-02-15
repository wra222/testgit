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
* ITC-1360-0053  增加PDLine过滤条件PAK 
* ITC-1360-0760  根据UC修改PDLine过滤条件 
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

public partial class PAK_PDPALabel02 : IMESBasePage
{
    protected string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

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
            
            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(DEFAULT_ROWS);

                Station = Request.QueryString["Station"];
                Pcode = Request.QueryString["PCode"];
                if (string.IsNullOrEmpty(Station))
                {
                    Station = "";
                }

                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                
                this.CmbPdLine1.Station = Station;
                this.CmbPdLine1.Customer = Master.userInfo.Customer;

                //this.CmbPdLine1.Stage = "PAK";
                this.CmbCode.CodeType = "PAK Label";
                //setColumnWidth();
                //setFocus();
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
        this.lblProductInfo.Text = this.GetLocalResourceObject(Pre + "_lblProductInfo").ToString();
        this.lblCustSN.Text = this.GetLocalResourceObject(Pre + "_lblCustSN").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblLabelList.Text = this.GetLocalResourceObject(Pre + "_lblLabelList").ToString();
        //this.lblNeedMenu.Text = this.GetLocalResourceObject(Pre + "_lblNeedMenu").ToString();
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet");
        this.QueryChk.Text = this.GetLocalResourceObject(Pre + "_QueryChk").ToString(); 
        //this.CmbPdLine1.InnerDropDownList.SelectedIndex = 1;
    }
    
    private void initTableColumnHeader()
    {
        this.gd.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_colPartNo").ToString();
        this.gd.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colTp").ToString();
        this.gd.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_colQty").ToString();
        
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("Type", Type.GetType("System.String"));
        retTable.Columns.Add("Scan", Type.GetType("System.String"));
        return retTable;


    }
    /*private DataTable initTable()
    {
        DataTable retTable = new DataTable();
       
        retTable.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartNo").ToString());
        retTable.Columns.Add(this.GetLocalResourceObject(Pre + "_colTp").ToString());
        retTable.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());

        return retTable;
    }
    */

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
   
    private void setColumnWidth()
    {
        //gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        //Set column width 
        //================================= Add Code ======================================


        //================================= Add Code End ==================================
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
