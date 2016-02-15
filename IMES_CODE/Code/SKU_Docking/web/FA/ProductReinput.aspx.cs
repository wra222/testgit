/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PoData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
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
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;
using System.IO;

public partial class FA_ProductReinput : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    IPoData iPoData = ServiceAgent.getInstance().GetObjectByName<IPoData>(WebConstant.PoDataObject);

    public int initRowsCount = 6;
 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
           
            if (!Page.IsPostBack)
            {        
                InitLabel();
                InitTable();
                hidGUID.Value = Guid.NewGuid().ToString().Replace("-", "");
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
    	
    private void InitLabel()
    {
        this.lblDataEntry.Text = GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblFile.Text = GetLocalResourceObject(Pre + "_lblFile").ToString();
        this.lblStation.Text = GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblReinputList.Text = GetLocalResourceObject(Pre + "_lblReinputList").ToString();
        this.lblFailList.Text = GetLocalResourceObject(Pre + "_lblFailList").ToString();
        this.PrintChk.Text = this.GetLocalResourceObject(Pre + "_PrintChk").ToString();
    }


    private DataTable initTable2()
    {
        DataTable dt = new DataTable();
        DataRow newRow = null;

        dt.Columns.Add("ProdId");
        dt.Columns.Add("CustSN");
        dt.Columns.Add("CurStation");

        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        gve2.DataSource = dt;
        gve2.DataBind();

        //this.gve2.HeaderRow.Cells[0].Width = Unit.Pixel(70);
        //gve2.HeaderRow.Cells[1].Width = Unit.Pixel(70);
        //gve2.HeaderRow.Cells[2].Width = Unit.Pixel(60);

        return dt;
    }



    private DataTable initTable3()
    {
        DataTable dt = new DataTable();
        DataRow newRow = null;

        dt.Columns.Add("ProdIDCustSN");
        dt.Columns.Add("Cause");

        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        gve3.DataSource = dt;
        gve3.DataBind();

        //gve3.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        //gve3.HeaderRow.Cells[1].Width = Unit.Pixel(100);

        return dt;
    }

    private void InitTable()
    {
        try
        {
            initTable2();
            initTable3();
            
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


  

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }

    private void clickDateFrom()
    {
        String script = "<script language='javascript'>document.getElementById('btnFrom').click();</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "clickDateFrom", script, false);
    }
    
    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void blockPage()
    {
        String script = "<script language='javascript'> document.getElementById('btnUpload').disabled = true;document.getElementById('btnQuery').disabled = true; </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "blockPage", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
}
