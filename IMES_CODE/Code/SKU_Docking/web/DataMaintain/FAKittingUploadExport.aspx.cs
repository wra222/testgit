using System;
using System.Web;
using IMES.Infrastructure;
using IMES.DataModel;
using System.IO;
using com.inventec.system.util;
using IMES.Maintain.Interface.MaintainIntf;
using System.Runtime.InteropServices;
using System.Text;
using com.inventec.iMESWEB;
using System.Web.UI;
using System.Data;


public partial class DataMaintain_FAKittingUploadExport: System.Web.UI.Page
{
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private IFaKittingUpload iFaKittingUpload = (IFaKittingUpload)ServiceAgent.getInstance().GetMaintainObjectByName<IFaKittingUpload>(WebConstant.MaintainFaKittingUploadObject);
    public int ExcelUploadStationColumnCount = 4;

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
        string line = this.dLine.Value.Trim();
        string family = this.dFamily.Value.Trim();

        //process = StringUtil.decode_URL(process);

        //System.Diagnostics.Process hProcess = null;
        DataTable result = new DataTable();
        //string fullFileName = "";

        string filePath = HttpContext.Current.Server.MapPath("~");
        //string path = filePath + "\\tmp";

        //try
        //{
        //    if (Directory.Exists(path) == false)
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //}
        //catch (FisException ex)
        //{
        //    //ignore
        //}

        //Guid guid = System.Guid.NewGuid();
        string extName = ".xls";
        //fullFileName = path + "\\" + guid.ToString() + extName;

        try
        {
            result = iFaKittingUpload.GetListForFaFromLine(line,family);
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
        DataTable dt = new DataTable();
        dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns.Add("");

        try
        {
            DataRow dr;
            dr = dt.NewRow();
            dr[0] = "Line:" + Null2String(line);
            dr[1] = "Family:" + Null2String(family);
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dt.Rows.Add("");
            dr = dt.NewRow();
            dr[0] = "PartNo";
            dr[1] = "Descr";
            dr[2] = "LightNo";
            dr[3] = "Qty";
            dt.Rows.Add(dr);

            foreach (DataRow item in result.Rows)
            {
                dr = dt.NewRow();
                //dr[0] = item.
                dr[0] = item[0].ToString();
                dr[1] = item[1].ToString();
                dr[2] = item[2].ToString();
                dr[3] = item[3].ToString();
                dt.Rows.Add(dr);
            }
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            string fileExport = "FA_" + line + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            MemoryStream ms = ExcelManager.ExportDataTableToExcel(dt);
            curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileExport + ".xls"));
            curContext.Response.BinaryWrite(ms.ToArray());


            //System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            //HSSFWorkbook workbook = new HSSFWorkbook();
            //MemoryStream ms = new MemoryStream();
            //HSSFSheet sheet = workbook.CreateSheet("sheetA");
            ////HSSFRow headerRow = sheet.CreateRow(0);

            ////// handling header.
            ////foreach (DataColumn column in dt.Columns)
            ////    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            //// handling value.
            //int rowIndex = 0;

            //foreach (DataRow row in dt.Rows)
            //{
            //    HSSFRow dataRow = sheet.CreateRow(rowIndex);

            //    foreach (DataColumn column in dt.Columns)
            //    {
            //        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
            //    }

            //    rowIndex++;
            //}
            //string fileExport = "FA_" + line + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            //workbook.Write(ms);
            //curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileExport + ".xls"));
            //curContext.Response.BinaryWrite(ms.ToArray());

            //workbook = null;
            //ms.Close();
            //ms.Dispose();
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
        //Microsoft.Office.Interop.Excel.Application mApp = null;
        //Microsoft.Office.Interop.Excel.Workbook m_book = null;
        //Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

        //try
        //{
        //    mApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
        //    mApp.Visible = false;
        //    mApp.DisplayAlerts = false;
        //    mApp.AlertBeforeOverwriting = false;
        //    m_book = mApp.Workbooks.Add(System.Reflection.Missing.Value);
        //    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)m_book.Worksheets[1];

        //    //worksheet = (Microsoft.Office.Interop.Excel.Worksheet)m_book.Worksheets.Add(Missing.Value, Missing.Value, 1, Missing.Value);
        //    //m_book = mApp.Workbooks.Open(fullFileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //    //worksheet = ((Microsoft.Office.Interop.Excel.Worksheet)mApp.Sheets[1]);

        //    int totalRowCount = result.Rows.Count;
        //    string excelRangeAll = string.Format("A1:{0}{1}", "D", totalRowCount + 4 - 1);
        //    Microsoft.Office.Interop.Excel.Range CurRange = worksheet.get_Range(excelRangeAll, Type.Missing);
        //    CurRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;// .xlVAlignCenter;//居中对齐
        //    CurRange.NumberFormatLocal = "@";

        //    worksheet.Cells[1, 1] = "Line:" + Null2String(line);
        //    worksheet.Cells[1, 2] = "Family:" + Null2String(family); 
    
        //    worksheet.Cells[3, 1] = "PartNo";
        //    worksheet.Cells[3, 2] = "Descr";
        //    worksheet.Cells[3, 3] = "LightNo";
        //    worksheet.Cells[3, 4] = "Qty";


        //    if (totalRowCount > 0)
        //    {
        //        object[,] rawData = new object[totalRowCount, ExcelUploadStationColumnCount];

        //        // Copy the values to the object array
        //        for (int row = 0; row < totalRowCount; row++)
        //        {
        //            //select PartNo, Tp, Ln, Qty, Sub, Safety_Stock, Max_Stock from #rm order by Ln
        //            rawData[row, 0] = Null2String(result.Rows[row][0]);
        //            rawData[row, 1] = Null2String(result.Rows[row][1]);
        //            rawData[row, 2] = Null2String(result.Rows[row][2]);
        //            rawData[row, 3] = Null2String(result.Rows[row][3]);
          
        //        }
        //        // Fast data export to Excel
        //        string excelRange = string.Format("A4:{0}{1}","D", totalRowCount + 4 - 1);

        //        worksheet.get_Range(excelRange, Type.Missing).Value2 = rawData;
        //    }

        //    CurRange.ColumnWidth = 28;
        //    //CurRange.EntireColumn.AutoFit();
        //    //CurRange.RowHeight = 19;//.EntireRow.AutoFit();//
        //    CurRange.EntireRow.AutoFit();


        //    m_book.SaveAs(fullFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    IntPtr t = new IntPtr(mApp.Hwnd);
        //    int processId = 0;
        //    GetWindowThreadProcessId(t, out processId);
        //    hProcess = System.Diagnostics.Process.GetProcessById(processId);

        //}
        //catch (FisException ex)
        //{
        //    SendComplete(ex.mErrmsg);
        //    return;
        //}
        //catch (Exception ex)
        //{
        //    //show error
        //    SendComplete(ex.Message);
        //    return;
        //}
        //finally
        //{
        //    if (mApp != null)
        //    {
        //        try
        //        {
        //            if (m_book != null) m_book.Close(false, null, null);
        //            mApp.Quit();
        //            if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
        //            if (m_book != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(m_book);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(mApp);
        //        }
        //        catch
        //        {
        //            //ignore
        //        }
        //        worksheet = null;
        //        m_book = null;
        //        mApp = null;
        //    }
        //    if (hProcess != null)
        //    {
        //        try
        //        {
        //            hProcess.Kill();
        //        }
        //        catch
        //        {
        //            // ignore
        //        }
        //    }
        //    // Collect the unreferenced objects
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //}

        //byte[] finalData = GetFileByteData(fullFileName);

        //try
        //{
        //    File.Delete(fullFileName);
        //}
        //catch (Exception ex)
        //{
        //    //ignore
        //}

        //HttpResponse response = System.Web.HttpContext.Current.Response;

        //fileExport = "FA_" + line + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");

        //response.Clear();
        //response.ClearHeaders();
        //response.ContentType = "application/octet-stream";
        ////Response.AddHeader("content-type", "application/x-msdownload");
        ////Response.ContentType = "application/ms-excel";
        //response.AddHeader("Content-Disposition", "attachment; filename=" + fileExport + ".xls");
        //response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");

        //response.OutputStream.Write(finalData, 0, finalData.Length);
        //response.Flush();
        //response.Close();
 
    }
}
