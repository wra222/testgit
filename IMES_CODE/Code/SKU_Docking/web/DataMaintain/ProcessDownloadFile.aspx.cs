using System;
using System.Data;//add by qy
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.UI;
using com.inventec.iMESWEB;
using com.inventec.system.util;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Maintain.Interface.MaintainIntf;
//using NPOI;//add by qy
//using NPOI.HSSF.UserModel;
//using NPOI.HSSF.Util;//add by qy
//using NPOI.POIFS;//add by qy
//using NPOI.SS.UserModel;
//using NPOI.Util;//add by qy
//using NPOI.XSSF;
//using NPOI.XSSF.UserModel;//add by qy

public partial class DataMaintain_ProcessDownloadFile : System.Web.UI.Page
{
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    IProcessManager iProcessManager = ServiceAgent.getInstance().GetMaintainObjectByName<IProcessManager>(WebConstant.IProcessManager);
    public int ExcelUploadStationColumnCount = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    private void SendComplete(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);

        scriptBuilder.AppendLine("<script language='javascript'>");
        if (errorMsg != "")
        {
            scriptBuilder.AppendLine("onComplete('" + errorMsg+ "');");
        }
        else
        {
            scriptBuilder.AppendLine("onComplete();");
        }
        scriptBuilder.AppendLine("</script>");

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "onComplete", scriptBuilder.ToString(), false);

   }

    public static byte[] GetFileByteData(String fileName)
    {

        FileStream fs = new FileStream(fileName, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        byte[] testArray = new byte[fs.Length];
        int count = br.Read(testArray, 0, testArray.Length);

        br.Close();

        return testArray;
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("\r\n", "<br>");
        sourceData = sourceData.Replace("\n", "<br>");
        sourceData = sourceData.Replace(@"\", @"\\");
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string process = this.dProcess.Value.Trim();
        ProcessInfoDef processInfo = new ProcessInfoDef();
        try
        {
            processInfo = iProcessManager.ExportProcess(process);
        }
        catch (FisException ex)
        {
            SendComplete(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            SendComplete(ex.Message);
            return;
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns.Add("");
        try
        {

            DataRow dr;
            dr = dt.NewRow();
            dr[0] = "Process Name";
            dr[1] = processInfo.ProcessInfo.Process;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Process Type";
            dr[1] = processInfo.ProcessInfo.Type;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Process Descr";
            dr[1] = processInfo.ProcessInfo.Description;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Editor";
            dr[1] = processInfo.ProcessInfo.Editor;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Cdt";
            dr[1] = processInfo.ProcessInfo.Cdt.ToString("yyyy-MM-dd");
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Udt";
            dr[1] = processInfo.ProcessInfo.Udt.ToString("yyyy-MM-dd");
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Station List";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Prev Station";
            dr[1] = "Status";
            dr[2] = "Station";
            dr[3] = "Editor";
            dr[4] = "Cdt";
            dr[5] = "Udt";
            dt.Rows.Add(dr);

            foreach (ProcessStationMaintainInfo info in processInfo.ProcessStationList)
            {
                dr = dt.NewRow();
                dr[0] = info.PreStation;
                if (info.Status == 0)
                {
                    dr[1] = "FAIL";
                }
                else if (info.Status == 1)
                {
                    dr[1] = "PASS";
                }
                else if (info.Status == 2)
                {
                    dr[1] = "PROCESSING";
                }
                dr[2] = info.Station;
                dr[3] = info.Editor;
                dr[4] = info.Cdt.ToString("yyyy-MM-dd");
                dr[5] = info.Udt.ToString("yyyy-MM-dd");
                dt.Rows.Add(dr);
            }


            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            MemoryStream ms = ExcelManager.ExportDataTableToExcel(dt);
            //HSSFWorkbook workbook = new HSSFWorkbook();
            //MemoryStream ms = new MemoryStream();
            //ISheet sheet = workbook.CreateSheet("sheetA");
            //HSSFRow headerRow = sheet.CreateRow(0);

            //// handling header.
            //foreach (DataColumn column in dt.Columns)
            //    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            //int rowIndex = 0;

            //foreach (DataRow row in dt.Rows)
            //{
            //    IRow dataRow = sheet.CreateRow(rowIndex);

            //    foreach (DataColumn column in dt.Columns)
            //    {
            //        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
            //    }

            //    rowIndex++;
            //}

            //workbook.Write(ms);
            curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + process + ".xls"));
            curContext.Response.BinaryWrite(ms.ToArray());

            //workbook = null;
            ms.Close();
            ms.Dispose();

        }
        catch (FisException ex)
        {
            SendComplete(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            SendComplete(ex.Message);
            return;
        }
    }

    //public static void insertRows(ISheet sheet)
    //{
    //    int InsertRowIndex = 1;
    //    int InsertRowCount = 2;
    //    IRow MyRows = sheet.GetRow(InsertRowIndex -1);

    //    #region 批量移动行
    //    sheet.ShiftRows(InsertRowIndex,sheet.LastRowNum,InsertRowCount,true,false);
    //    #endregion


    //    #region 对批量移动后空出的空行插，创建相应的行，并以插入行的上一行为格式源(即：插入行-1的那一行)
    //    for (int i = InsertRowIndex; i < InsertRowIndex + InsertRowCount - 1; i++)
    //    {
    //        IRow targetRow = null;
    //        ICell sourceCell = null;
    //        ICell targetCell = null;

    //        targetRow = sheet.CreateRow(i + 1);

    //        for (int m = MyRows.FirstCellNum; m < MyRows.LastCellNum; m++)
    //        {
    //            sourceCell = MyRows.GetCell(m);
    //            if (sourceCell == null)
    //                continue;
    //            targetCell = targetRow.CreateCell(m);

    //            targetCell = sourceCell;
    //            targetCell.CellStyle = sourceCell.CellStyle;
    //            targetCell.SetCellType(sourceCell.CellType);

    //        }
    //        //CopyRow(sourceRow, targetRow);

    //        //Util.CopyRow(sheet, sourceRow, targetRow);
    //    }

    //    IRow firstTargetRow = sheet.GetRow(InsertRowIndex);
    //    ICell firstSourceCell = null;
    //    ICell firstTargetCell = null;

    //    for (int m = MyRows.FirstCellNum; m < MyRows.LastCellNum; m++)
    //    {
    //        firstSourceCell = MyRows.GetCell(m);
    //        if (firstSourceCell == null)
    //            continue;
    //        firstTargetCell = firstTargetRow.CreateCell(m);

    //        firstTargetCell = firstSourceCell;
    //        firstTargetCell.CellStyle = firstSourceCell.CellStyle;
    //        firstTargetCell.SetCellType(firstSourceCell.CellType);
    //    }
    //    #endregion
    
    //}
}
