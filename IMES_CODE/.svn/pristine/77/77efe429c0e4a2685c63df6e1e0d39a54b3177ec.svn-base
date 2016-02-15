/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-2-20   itc207024           create
 * 2010-6-2    itc210001           add: RenderDataTableToExcel
 * Known issues:Any restrictions about this file 
 *          
 */
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;//add by qy
using System.Web.UI;//add by qy
using System.Web.UI.HtmlControls;//add by qy
using System.Web.UI.WebControls;//add by qy
using System.Web.UI.WebControls.WebParts;//add by qy
using System.Xml.Linq;
using System.Data.OleDb;
using System.IO;
using System.Collections.Generic;
using NPOI;//add by qy
using NPOI.HPSF;//add by qy
using NPOI.HSSF;//add by qy
using NPOI.HSSF.UserModel;
using NPOI.POIFS;//add by qy
using NPOI.Util;//add by qy
using NPOI.HSSF.Util;//add by qy
using com.inventec.system.util;
using Excel;
using System.Text;
using System.Diagnostics;
using NPOI.SS.UserModel;


/// <summary>
///ExcelManager 的摘要说明
/// </summary>
public class ExcelManager
{
    public ExcelManager()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary> 
    /// 读取Excel文件中Sheet1的内容 
    /// </summary> 
    /// <param name="fileName">Excel文件名</param> 
    /// <returns>DataTable</returns> 
    public static System.Data.DataTable getExcelSheetData(string fileName)
    {
        FileStream file=null;
        try
        {
            //打开要读取的Excel
            file = new FileStream(fileName, FileMode.Open);

         if (fileName.Substring(fileName.Length - 5, 5).Equals(".xlsx"))
            {
                IExcelDataReader ExcelReader = ExcelReaderFactory.CreateOpenXmlReader(file);
                file.Close();
                ExcelReader.IsFirstRowAsColumnNames = false;
                DataSet result = ExcelReader.AsDataSet();

                DataTable dt = new DataTable();
                dt = result.Tables[0];
                ExcelReader.Close();

                return dt;
            }
     
            //读入Excel
            HSSFWorkbook workbook = new HSSFWorkbook(file);
     
            file.Close();
            ISheet sheet = workbook.GetSheetAt(0);
            //建立一个新的table
            DataTable dtNew = new DataTable(); ;
            IRow row = sheet.GetRow(0);
            int rowId = 0;
            int columnNum = 0;
            while (rowId <= sheet.LastRowNum)
            {
                if (sheet.GetRow(rowId) != null && sheet.GetRow(rowId).LastCellNum > columnNum)
                    columnNum = sheet.GetRow(rowId).LastCellNum;
                rowId++;
            }

            //column name
            for (int columnIndex = 0; columnIndex < columnNum; columnIndex++)
            {
                DataColumn dc = new DataColumn("column" + columnIndex);
                dtNew.Columns.Add(dc);
            }

            rowId = 0;
            while (rowId <= sheet.LastRowNum)
            {
                DataRow newRow = dtNew.NewRow();
                //读取所有column
                for (int colIndex = 0; colIndex < columnNum; colIndex++)
                {
                    if (sheet.GetRow(rowId) == null)
                        newRow[dtNew.Columns[colIndex]] = "";
                    else
                        newRow[dtNew.Columns[colIndex]] = com.inventec.system.util.StringUtil.Null2String(sheet.GetRow(rowId).GetCell(colIndex));
                }
                dtNew.Rows.Add(newRow);
                rowId++;
            }
            return dtNew;
        }
        catch (Exception e)
        {
            throw;
        }
        finally
        {
            if (file != null)
                file.Close();
        }
    }


    public static bool FileExists(string fileName, string type)
    {
        try
        {
            if ((type == null)
                || (type.Equals("")))
            {
                type = ".xls";
            }
            if ((fileName != null)
                && (!fileName.Equals("")))
            {
                if (!fileName.ToLower().EndsWith(type.ToLower()))
                {
                    return false;
                }
            }
            return File.Exists(fileName);
        }
        catch (Exception ef)
        {
            return false;
        }
    }

    public static List<string> getFirstColumnData(string fileName)
    {
        List<string> productIDList = new List<string>();
        DataTable result = getExcelSheetData(fileName);
        if (result.Rows != null && result.Rows.Count > 0)
        {
            int i = 0;
            while (i < result.Rows.Count)
            {
                if (result.Rows[i][0].ToString().Equals(""))
                {

                }
                else
                {
                    productIDList.Add(result.Rows[i][0].ToString());
                }
                i++;
            }
        }
        return productIDList;
    }




    public static Stream RenderDataTableToExcel(DataTable SourceTable)
    {
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        ISheet sheet = workbook.CreateSheet();
        IRow headerRow = sheet.CreateRow(0);
       
        // handling header.
        foreach (DataColumn column in SourceTable.Columns)
            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

        // handling value.
        int rowIndex = 1;

        foreach (DataRow row in SourceTable.Rows)
        {
            IRow dataRow = sheet.CreateRow(rowIndex);

            foreach (DataColumn column in SourceTable.Columns)
            {
                string rowValue = row[column.Ordinal].ToString();
                //人为调整没列的宽度为均等的20个字符宽度，可以根据需要自行调整
                sheet.SetColumnWidth(column.Ordinal, 256 * 20);
                //自动调整，效果不佳，弃用
                //sheet.AutoSizeColumn(column.Ordinal);
                if (rowValue == "&nbsp;")
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue("");
                    continue;
                }

                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());

            }



            rowIndex++;
        }

        //逐列自动调整列宽，但效果不佳，且通用性差，以下代码弃用
        //sheet.AutoSizeColumn(0);
        //sheet.AutoSizeColumn(1);
        //sheet.AutoSizeColumn(2);
        //sheet.AutoSizeColumn(3);
        //sheet.AutoSizeColumn(4);
        //sheet.AutoSizeColumn(5);
        workbook.Write(ms);
        ms.Flush();
        ms.Position = 0;

        //for (int i = SourceTable.Columns.Count; i > 0; i--)
        //{
        //    sheet.AutoSizeColumn(i);
        //}

        sheet = null;
        headerRow = null;
        workbook = null;

        return ms;
    }

    public static void RenderDataTableToExcel(DataTable SourceTable, string FileName)
    {
        MemoryStream ms = RenderDataTableToExcel(SourceTable) as MemoryStream;
        FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
        byte[] data = ms.ToArray();

        fs.Write(data, 0, data.Length);
        fs.Flush();
        fs.Close();

        data = null;
        ms = null;
        fs = null;
    }

    public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
    {
        HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
        ISheet sheet = workbook.GetSheet(SheetName);

        DataTable table = new DataTable();

        IRow headerRow = sheet.GetRow(HeaderRowIndex);
        int cellCount = headerRow.LastCellNum;
      
        for (int i = headerRow.FirstCellNum; i < cellCount; i++)
        {
            DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
            table.Columns.Add(column);
        }

        int rowCount = sheet.LastRowNum;

        for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            DataRow dataRow = table.NewRow();

            for (int j = row.FirstCellNum; j < cellCount; j++)
                dataRow[j] = Null2String(row.GetCell(j).ToString());
        }

        ExcelFileStream.Close();
        workbook = null;
        sheet = null;
        return table;
    }

    public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
    {
        HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
        ISheet sheet = workbook.GetSheetAt(SheetIndex);

        DataTable table = new DataTable();

        IRow headerRow = sheet.GetRow(HeaderRowIndex);
        int cellCount = headerRow.LastCellNum;

        for (int i = headerRow.FirstCellNum; i < cellCount; i++)
        {
            DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
            table.Columns.Add(column);
        }

        int rowCount = sheet.LastRowNum;

        for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            DataRow dataRow = table.NewRow();

            for (int j = row.FirstCellNum; j < cellCount; j++)
            {
                if (row.GetCell(j) != null)
                    dataRow[j] = Null2String(row.GetCell(j).ToString());
                else
                {
                    dataRow[j] = null;
                }
            }

            table.Rows.Add(dataRow);
        }

        ExcelFileStream.Close();
        workbook = null;
        sheet = null;
        return table;
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }
    ///// </summary>
    //public static System.Collections.SortedList ListColumnsName;
    ///// <summary>
    ///// 导出Excel
    ///// </summary>
    ///// <param name="dgv"></param>
    ///// <param name="filePath"></param>
    //public static void ExportExcel(DataTable dtSource, string filePath)
    //{
    //    if (ListColumnsName == null || ListColumnsName.Count == 0)
    //        throw (new Exception("请对ListColumnsName设置要导出的列明！"));

    //    HSSFWorkbook excelWorkbook = CreateExcelFile();
    //    InsertRow(dtSource, excelWorkbook);
    //    SaveExcelFile(excelWorkbook, filePath);
    //}

    ///// <summary>
    ///// 导出Excel
    ///// </summary>
    ///// <param name="dgv"></param>
    ///// <param name="filePath"></param>
    //public static void ExportExcel(DataTable dtSource, Stream excelStream)
    //{
    //    if (ListColumnsName == null || ListColumnsName.Count == 0)
    //        throw (new Exception("请对ListColumnsName设置要导出的列明！"));

    //    HSSFWorkbook excelWorkbook = CreateExcelFile();
    //    InsertRow(dtSource, excelWorkbook);
    //    SaveExcelFile(excelWorkbook, excelStream);
    //}
    ///// <summary>
    ///// 保存Excel文件
    ///// </summary>
    ///// <param name="excelWorkBook"></param>
    ///// <param name="filePath"></param>
    //protected static void SaveExcelFile(HSSFWorkbook excelWorkBook, string filePath)
    //{
    //    FileStream file = null;
    //    try
    //    {
    //        file = new FileStream(filePath, FileMode.Create);
    //        excelWorkBook.Write(file);
    //    }
    //    finally
    //    {
    //        if (file != null)
    //        {
    //            file.Close();
    //        }
    //    }
    //}
    ///// <summary>
    ///// 保存Excel文件
    ///// </summary>
    ///// <param name="excelWorkBook"></param>
    ///// <param name="filePath"></param>
    //protected static void SaveExcelFile(HSSFWorkbook excelWorkBook, Stream excelStream)
    //{
    //    try
    //    {
    //        excelWorkBook.Write(excelStream);
    //    }
    //    finally
    //    {

    //    }
    //}
    ///// <summary>
    ///// 创建Excel文件
    ///// </summary>
    ///// <param name="filePath"></param>
    //protected static HSSFWorkbook CreateExcelFile()
    //{
    //    HSSFWorkbook hssfworkbook = new HSSFWorkbook();
    //    return hssfworkbook;
    //}

    ///// <summary>
    ///// 创建excel表头
    ///// </summary>
    ///// <param name="dgv"></param>
    ///// <param name="excelSheet"></param>
    //protected static void CreateHeader(HSSFSheet excelSheet)
    //{
    //    int cellIndex = 0;
    //    //循环导出列
    //    foreach (System.Collections.DictionaryEntry de in ListColumnsName)
    //    {
    //        HSSFRow newRow = excelSheet.CreateRow(0);
    //        HSSFCell newCell = newRow.CreateCell(cellIndex);
    //        newCell.SetCellValue(de.Value.ToString());
    //        cellIndex++;
    //    }
    //}
    ///// <summary>
    ///// 插入数据行
    ///// </summary>
    //protected static void InsertRow(DataTable dtSource, HSSFWorkbook excelWorkbook)
    //{
    //    int rowCount = 0;
    //    int sheetCount = 1;
    //    HSSFSheet newsheet = null;

    //    //循环数据源导出数据集
    //    newsheet = excelWorkbook.CreateSheet("Sheet" + sheetCount);
    //    CreateHeader(newsheet);
    //    foreach (DataRow dr in dtSource.Rows)
    //    {
    //        rowCount++;
    //        //超出10000条数据 创建新的工作簿
    //        if (rowCount == 10000)
    //        {
    //            rowCount = 1;
    //            sheetCount++;
    //            newsheet = excelWorkbook.CreateSheet("Sheet" + sheetCount);
    //            CreateHeader(newsheet);
    //        }

    //        HSSFRow newRow = newsheet.CreateRow(rowCount);
    //        InsertCell(dtSource, dr, newRow, newsheet, excelWorkbook);
    //    }
    //}

    ///// <summary>
    ///// 导出数据行
    ///// </summary>
    ///// <param name="dtSource"></param>
    ///// <param name="drSource"></param>
    ///// <param name="currentExcelRow"></param>
    ///// <param name="excelSheet"></param>
    ///// <param name="excelWorkBook"></param>
    ///// 
    //protected static void InsertCell(DataTable dtSource, DataRow drSource, HSSFRow currentExcelRow, HSSFSheet excelSheet, HSSFWorkbook excelWorkBook)
    //    {
    //        for (int cellIndex = 0; cellIndex < ListColumnsName.Count; cellIndex++)
    //        {
    //            //列名称
    //            string columnsName = ListColumnsName.GetKey(cellIndex).ToString();
    //            HSSFCell newCell = null;
    //            System.Type rowType = drSource[columnsName].GetType();
    //            string drValue = drSource[columnsName].ToString().Trim();
    //            switch (rowType.ToString())
    //            {
    //                case "System.String"://字符串类型
    //                    drValue = drValue.Replace("&", "&");
    //                    drValue = drValue.Replace(">", ">");
    //                    drValue = drValue.Replace("<", "<");
    //                    newCell = currentExcelRow.CreateCell(cellIndex);
    //                    newCell.SetCellValue(drValue);
    //                    break;
    //                case "System.DateTime"://日期类型
    //                    DateTime dateV;
    //                    DateTime.TryParse(drValue, out dateV);
    //                    newCell = currentExcelRow.CreateCell(cellIndex);
    //                    newCell.SetCellValue(dateV);

    //                    //格式化显示
    //                    HSSFCellStyle cellStyle = excelWorkBook.CreateCellStyle();
    //                    HSSFDataFormat format = excelWorkBook.CreateDataFormat();
    //                    cellStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");
    //                    newCell.CellStyle = cellStyle;

    //                    break;
    //                case "System.Boolean"://布尔型
    //                    bool boolV = false;
    //                    bool.TryParse(drValue, out boolV);
    //                    newCell = currentExcelRow.CreateCell(cellIndex);
    //                    newCell.SetCellValue(boolV);
    //                    break;
    //                case "System.Int16"://整型
    //                case "System.Int32":
    //                case "System.Int64":
    //                case "System.Byte":
    //                    int intV = 0;
    //                    int.TryParse(drValue, out intV);
    //                    newCell = currentExcelRow.CreateCell(cellIndex);
    //                    newCell.SetCellValue(intV.ToString());
    //                    break;
    //                case "System.Decimal"://浮点型
    //                case "System.Double":
    //                    double doubV = 0;
    //                    double.TryParse(drValue, out doubV);
    //                    newCell = currentExcelRow.CreateCell(cellIndex);
    //                    newCell.SetCellValue(doubV);
    //                    break;
    //                case "System.DBNull"://空值处理
    //                    newCell = currentExcelRow.CreateCell(cellIndex);
    //                    newCell.SetCellValue("");
    //                    break;
    //                default:
    //                    throw (new Exception(rowType.ToString() + "：类型数据无法处理!"));
    //            }
    //        }
    //    }
    //protected static void ExportExcel(DataTable dtSource)
    //{
       
    //    // ExcelManager.ListColumnsName = new SortedList(new StarTech.NPOI.NoSort());
    //    // ExcelManager.ListColumnsName.Add("MemberName", "姓名");
    //    // ExcelManager.ListColumnsName.Add("username", "账号");
    //    // ExcelManager.ListColumnsName.Add("starttime", "登陆时间");
    //    // ExcelManager.ListColumnsName.Add("lasttime", "在线到期时间");
    //    // ExcelManager.ListColumnsName.Add("state", "状态");
    //    //Response.Clear();
    //    //Response.BufferOutput = false;
    //    //Response.ContentEncoding = System.Text.Encoding.UTF8;
    //    //string filename = HttpUtility.UrlEncode(DateTime.Now.ToString("在线用户yyyyMMdd"));
    //    //Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
    //    //Response.ContentType = "application/ms-excel";
    //    //StarTech.NPOI.NPOIHelper.ExportExcel(dtSource, Response.OutputStream);
    //    //Response.Close();
    //}
    }

//排序实现接口 不进行排序 根据添加顺序导出
//public class NoSort : System.Collections.IComparer
//{
//    public int Compare(object x, object y)
//    {
//        return -1;
//    }
//}




    
  



