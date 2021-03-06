/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for DOA MB Upload Page
 *             
 * UI:CI-MES12-SPEC-FA-UI DOA MB List Upload.docx
 * UC:CI-MES12-SPEC-FA-UC DOA MB List Upload.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-11-20  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.OleDb;
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

public partial class FA_DOAMBUpload : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;

    IDOAMBUpload iDOAMBUpload = ServiceAgent.getInstance().GetObjectByName<IDOAMBUpload>(WebConstant.DOAMBUploadObject);

    public int initRowsCount = 10;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;

            if (!Page.IsPostBack)
            {        
                InitLabel();
                clearGrid();
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
    
    protected void gd_DataBound1(object sender, GridViewRowEventArgs e)
    {
    }

    protected void gd_DataBound2(object sender, GridViewRowEventArgs e)
    {
    }

    protected void btnToExcel_ServerClick(object sender, System.EventArgs e)
    {
        DataTable2Excel(GetExcelContents());
    }

    private DataTable GetExcelContents()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(GetLocalResourceObject(Pre + "_titleMBSN").ToString());
        dt.Columns.Add(GetLocalResourceObject(Pre + "_titleCause").ToString());
        
        for (int i = 0; i < gve2.Rows.Count; i++)
        {
            if (!String.IsNullOrEmpty(gve2.Rows[i].Cells[0].Text.Trim())
                && !(gve2.Rows[i].Cells[0].Text.Trim().ToLower() == "&nbsp;"))
            {
                dr = dt.NewRow();
                dr[0] = gve2.Rows[i].Cells[0].Text.Trim();
                dr[1] = gve2.Rows[i].Cells[1].Text.Trim();
                dt.Rows.Add(dr);
            }
        }

        return dt;
    }

    private static void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.UI.WebControls.DataGrid dgExport = null;
        // 当前对话
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dtData != null)
        {
            //            <bug>
            //            BUG NO:ITC-1103-0261 
            //            REASON:中文编码
            //            </bug>
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            curContext.Response.Charset = "UTF-8";
            curContext.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=gb2312 >");
            // 导出excel文件
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);
            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dtData.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            for (int i = 0; i < dgExport.Items.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }

            // 返回客户端
            dgExport.RenderControl(htmlWriter);

            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();
        }
    }

    protected void btnUploadList_ServerClick(object sender, System.EventArgs e)
    {
        string newName = DateTime.Now.Ticks.ToString() + FileUp.PostedFile.FileName.Substring(FileUp.PostedFile.FileName.LastIndexOf("."));
        string fullName = Server.MapPath("~") + "/" + newName;
        try
        {
            clearGrid();

            FileUp.SaveAs(fullName);

            IList<string> col = ExcelManager.getFirstColumnData(fullName);
            IList<string> snList = new List<string>();
            foreach (string s in col)
            {
                string mbsn = s.Trim().ToUpper();
                if (mbsn.Length == 11 && (mbsn[4] == 'M' || mbsn[4] == 'B'))
                {
                    mbsn = mbsn.Substring(0, 10);
                }
                if (mbsn != "" && !snList.Contains(mbsn))
                {
                    if (mbsn.Length != 10 && mbsn.Length != 11)
                    {
                        jsAlert(GetLocalResourceObject(Pre + "_msgBadSNInFile").ToString());
                        return;
                    }
                    snList.Add(mbsn);
                }
            }

            if (snList.Count == 0)
            {
                jsAlert(GetLocalResourceObject(Pre + "_msgNoSN").ToString());
                return;
            }

            IList<string> passList = new List<string>();
            IList<S_RowData_FailMBSN> failList = new List<S_RowData_FailMBSN>();
            iDOAMBUpload.SaveDOAMBList(snList, UserId, out passList, out failList);

            showTableData(passList, failList);

            return;
        }
        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            File.Delete(fullName);
            endWaitingCoverDiv();
        }
    }

    protected void btnUploadSingle_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid();

            IList<string> snList = new List<string>();
            string mbsn = hidMBSN.Value.Trim();
            if (mbsn.Length == 11 && (mbsn[4] == 'M' || mbsn[4] == 'B'))
            {
                mbsn = mbsn.Substring(0, 10);
            }
            snList.Add(mbsn);

            IList<string> passList = new List<string>();
            IList<S_RowData_FailMBSN> failList = new List<S_RowData_FailMBSN>();
            iDOAMBUpload.SaveDOAMBList(snList, UserId, out passList, out failList);

            showTableData(passList, failList);

            hidMBSN.Value = "";

            return;
        }
        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }
    protected void btnReset_ServerClick(object sender, System.EventArgs e)
    {
        clearGrid();
        hidMBSN.Value = "";
        return;
    }

    private void showTableData(IList<string> data1, IList<S_RowData_FailMBSN> data2)
    {
        showTableData1(data1);
        showTableData2(data2);
    }

    private void showTableData1(IList<string> data)
    {
        DataTable dt = initTable1();
        DataRow newRow;
        int cnt = 0;
        if (data.Count > 0)
        {
            foreach (string ele in data)
            {
                newRow = dt.NewRow();                             
                newRow["MBSN"] += ele;                
                dt.Rows.Add(newRow);
                cnt++;
            }
        }

        for (; cnt < initRowsCount; cnt++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        this.gve1.DataSource = dt;
        this.gve1.DataBind();
        initTableColumnHeader1();
        up1.Update();
        return;
    }

    private void showTableData2(IList<S_RowData_FailMBSN> data)
    {
        DataTable dt = initTable2();
        DataRow newRow;
        int cnt = 0;
        if (data.Count > 0)
        {
            IList<string> causeList = iDOAMBUpload.GetFailCauseList();
            foreach (S_RowData_FailMBSN ele in data)
            {
                newRow = dt.NewRow();
                newRow["MBSN"] += ele.m_mbsn;
                newRow["Cause"] += causeList[ele.m_cause];
                dt.Rows.Add(newRow);
                cnt++;
            }
        }

        for (; cnt < initRowsCount; cnt++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        this.gve2.DataSource = dt;
        this.gve2.DataBind();
        initTableColumnHeader2();
        up2.Update();
        return;
    }

    private void InitLabel()
    {
        lblDataEntry.Text = GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        lblFile.Text = GetLocalResourceObject(Pre + "_lblFile").ToString();
        lblPassList.Text = GetLocalResourceObject(Pre + "_lblPassList").ToString();
        lblFailList.Text = GetLocalResourceObject(Pre + "_lblFailList").ToString();
        btnExport.InnerText = GetLocalResourceObject(Pre + "_btnExport").ToString();
    }

    private DataTable getNullDataTable1()
    {
        DataTable dt = initTable1();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable2()
    {
        DataTable dt = initTable2();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable1()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MBSN", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable2()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MBSN", Type.GetType("System.String"));
        retTable.Columns.Add("Cause", Type.GetType("System.String"));
        return retTable;
    }

    private void initTableColumnHeader1()
    {
        gve1.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titleMBSN").ToString();
        gve1.HeaderRow.Cells[0].Width = Unit.Percentage(100);
    }

    private void initTableColumnHeader2()
    {
        gve2.HeaderRow.Cells[0].Text = GetLocalResourceObject(Pre + "_titleMBSN").ToString();
        gve2.HeaderRow.Cells[1].Text = GetLocalResourceObject(Pre + "_titleCause").ToString();
        gve2.HeaderRow.Cells[0].Width = Unit.Percentage(40);
        gve2.HeaderRow.Cells[1].Width = Unit.Percentage(60);
    }

    protected void clearGrid(object sender, System.EventArgs e)
    {
        try
        {
            clearGrid();
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

    private void clearGrid()
    {
        try
        {
            gve1.DataSource = getNullDataTable1();
            gve1.DataBind();
            up1.Update();
            initTableColumnHeader1();
            gve2.DataSource = getNullDataTable2();
            gve2.DataBind();
            up2.Update();
            initTableColumnHeader2();
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
    
    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
}
