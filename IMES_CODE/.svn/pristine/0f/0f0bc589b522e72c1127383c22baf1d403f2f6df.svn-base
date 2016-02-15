using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using System.Web.UI.WebControls;

//using NPOI;
//using NPOI.HPSF;
//using NPOI.HSSF;
//using NPOI.HSSF.UserModel;
//using NPOI.POIFS;
//using NPOI.Util;
//using NPOI.HSSF.Util;
using System.IO;
using System.Web;

public partial class CommonFunction_SessionInfo : System.Web.UI.Page
{
    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Customer;
    protected string AlertSelectLine = "";
    protected string AlertNoData = "NoData";
    protected string AlertSuccess = "";
    int[] columnWidth = new int[] { };
    protected string Editor = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Editor = Master.userInfo.UserId;
        this.GridViewExt1.DataSource = GetTable();
        this.GridViewExt1.DataBind();
    }

    //private void Excel(DataTable result, string SQLText)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        foreach (DataColumn dc in result.Columns)
    //        {
    //            dt.Columns.Add(dc.ToString());
    //        }
    //        DataRow dr;
    //        dr = dt.NewRow();
    //        foreach (DataColumn dc in result.Columns)
    //        {
    //            dr[dc.Ordinal] = dc.ToString();
    //        }
    //        dt.Rows.Add(dr);

    //        foreach (DataRow item in result.Rows)
    //        {
    //            dr = dt.NewRow();

    //            foreach (DataColumn column in result.Columns)
    //            {
    //                dr[column.Ordinal] = item[column].ToString();
    //            }
    //            dt.Rows.Add(dr);
    //        }

    //        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
    //        HSSFWorkbook workbook = new HSSFWorkbook();
    //        MemoryStream ms = new MemoryStream();
    //        HSSFSheet sheet = workbook.CreateSheet("sheetA");

    //        HSSFRow dataRow = new HSSFRow();

    //        //foreach (DataColumn column in result.Columns)
    //        //    dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName.ToString());

    //        int rowIndex = 0;

    //        foreach (DataRow row in dt.Rows)
    //        {
    //            dataRow = sheet.CreateRow(rowIndex);

    //            foreach (DataColumn column in dt.Columns)
    //            {
    //                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
    //            }

    //            rowIndex++;
    //        }
    //        string fileExport = SQLText + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
    //        workbook.Write(ms);
    //        curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileExport + ".xls"));
    //        curContext.Response.BinaryWrite(ms.ToArray());

    //        workbook = null;
    //        ms.Close();
    //        ms.Dispose();
    //    }
    //    catch (FisException ex)
    //    {
    //        writeToAlertMessage(ex.mErrmsg);
    //        return;
    //    }
    //    catch (Exception ex)
    //    {
    //        //show error
    //        writeToAlertMessage(ex.Message);
    //        return;
    //    }
    //}

    //public void ExportExcel(object sender, System.EventArgs e)
    //{
    //    try
    //    {
    //        string DB = this.HiddenDB.Value;
    //        string SQLText = this.HiddenSQLText.Value;
    //        DataTable dtData = InitTable();
    //        if (SQLText != "")
    //        {
    //            ICommonFunction CurrentService = ServiceAgent.getInstance().GetObjectByName<ICommonFunction>(com.inventec.iMESWEB.WebConstant.MaintainCommonFunctionObject);
    //            System.Data.DataSet SqlResult = CurrentService.GetSQLResult(Editor, DB, SQLText, null, null);
    //            if (SqlResult != null && SqlResult.Tables.Count > 0 && SqlResult.Tables[0].Rows.Count > 0)
    //            {
    //                dtData = SqlResult.Tables[0];
    //            }
    //        }
    //        if (dtData != null && dtData.Rows.Count > 0)
    //        {
    //            Excel(dtData, SQLText);
    //        }
    //        else
    //        {
    //            writeToAlertMessage(AlertNoData);
    //        }
    //    }
    //    catch (FisException fe)
    //    {
    //        //this.GridViewExt1.DataSource = GetTable();
    //        //this.GridViewExt1.DataBind();
    //        writeToAlertMessage(fe.mErrmsg);
    //    }
    //    catch (Exception ex)
    //    {
    //        //this.GridViewExt1.DataSource = GetTable();
    //        //this.GridViewExt1.DataBind();
    //        writeToAlertMessage(ex.Message);
    //    }
    //    //finally
    //    //{
    //    //    endWaitingCoverDiv();
    //    //}
    //}

    public void SetSession(object sender, System.EventArgs e)
    {
        HttpContext.Current.Session["MsgInfoTODialog"] = this.hidSQLMsg.Value;

        
        String script = "<script language='javascript'>" + "\r\n" +
            "ShowDialog();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanel3, ClientScript.GetType(), "ShowDialog", script, false);
    }

    public void DisplayResultGV(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dtData = GetDTList();
            this.Legend1.InnerText = this.HiddenSQLText.Value;
            if (dtData != null && dtData.Rows.Count > 0)
            {
                this.GridViewExt1.DataSource = dtData;
                this.GridViewExt1.DataBind();
            }
            else
            {
                writeToAlertMessage(AlertNoData);
            }
        }
        catch (FisException fe)
        {
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
            writeToAlertMessage(fe.mErrmsg);
        }
        catch (Exception ex)
        {
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    private DataTable GetDTList()
    {
        string DB = this.HiddenDB.Value;
        string SQLText = this.HiddenSQLText.Value;
        
        DataTable dt = InitTable();

        ICommonFunction CurrentService = ServiceAgent.getInstance().GetObjectByName<ICommonFunction>(com.inventec.iMESWEB.WebConstant.MaintainCommonFunctionObject);
        System.Data.DataSet SqlResult = CurrentService.GetSQLResult(Editor, DB, SQLText, null, null);
        if (SqlResult != null && SqlResult.Tables.Count > 0 && SqlResult.Tables[0].Rows.Count > 0)
        {
            dt = SqlResult.Tables[0];
        }

        DataRow newRow = null;

        if (dt != null && dt.Rows.Count != 0)
        {
            int DTRowsCount = dt.Rows.Count;
            this.LabelResultCount.Text = DTRowsCount.ToString();
            LabelDisplayCount.Text = DTRowsCount.ToString();
            if (DTRowsCount > 500)
            {
                LabelDisplayCount.Text = "500";
                for (int i = DTRowsCount-1; i > 499; i--)
                {
                    dt.Rows.RemoveAt(i);
                }
            }

            for (int i = dt.Rows.Count; i < DefaultRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }


            int gridviewWidth = 0;
            columnWidth = new int[] { };
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[0].ColumnName == null)
                {
                    gridviewWidth += 20;
                }
                else
                {
                    gridviewWidth += 8;// dt.Columns[i].ColumnName.Length * 3;
                }
            }
            if (gridviewWidth < 98)
            {
                gridviewWidth = 98;
            }
            this.GridViewExt1.Width = System.Web.UI.WebControls.Unit.Percentage(gridviewWidth * 1.0);


        }
        else
        {
            this.GridViewExt1.Width = System.Web.UI.WebControls.Unit.Percentage(98);
            for (int i = 0; i < DefaultRowsCount; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                
            }
            this.LabelResultCount.Text = "0";
            this.LabelDisplayCount.Text = "0";
        }



        return dt;

    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "document.getElementById('BtnQuery').disabled = false;endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("No Data", Type.GetType("System.String"));
        return result;
    }

    private DataTable GetTable()
    {
        DataTable result = InitTable();
        for (int i = 0; i < DefaultRowsCount; i++)
        {
            DataRow newRow = result.NewRow();
            result.Rows.Add(newRow);
        }
        return result;
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = DefaultRowsCount; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }
    }
    private int DefaultRowsCount = 20;
}
