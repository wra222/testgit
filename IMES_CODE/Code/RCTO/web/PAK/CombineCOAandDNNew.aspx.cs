/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine COA and DN
* CI-MES12-SPEC-PAK-UI Combine COA and DN.docx
* CI-MES12-SPEC-PAK-UC Combine COA and DN.docx         
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012/08/10   Du.Xuan               Create     
* Known issues:

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

public partial class PAK_CombineCoaAndDn : IMESBasePage
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
        this.lblProductInfo.Text = this.GetLocalResourceObject(Pre + "_lblProductInfo").ToString();
        this.lblCustSN.Text = this.GetLocalResourceObject(Pre + "_lblCustSN").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblLabelList.Text = this.GetLocalResourceObject(Pre + "_lblLabelList").ToString();
        this.lblCOA.Text = this.GetLocalResourceObject(Pre + "_lblCOA").ToString();
        this.lblCheckItem.Text = this.GetLocalResourceObject(Pre + "_lblCheckItem").ToString();
       
        
        this.lblDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblDataEntry");
        this.btnPrintSetting.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnPrtSet");
        this.btnRePrint.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnReprint");
        //this.CmbPdLine1.InnerDropDownList.SelectedIndex = 1;
    }
    
    private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDnNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colModel").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCustPN").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPono").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPackedQty").ToString());

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
    }
   
    private void setColumnWidth()
    {
        //gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        //Set column width 
        //================================= Add Code ======================================

        gd.HeaderRow.Cells[0].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);

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
