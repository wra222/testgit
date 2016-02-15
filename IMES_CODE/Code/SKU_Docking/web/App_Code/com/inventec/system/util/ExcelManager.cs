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
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using Excel;
using System.Text;
using System.Web;
using System.Web.Security;//add by qy
using System.Web.UI;//add by qy
using System.Web.UI.HtmlControls;//add by qy
using System.Web.UI.WebControls;//add by qy
using System.Web.UI.WebControls.WebParts;//add by qy
using System.Xml.Linq;
using com.inventec.system.util;
using NPOI;//add by qy
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;//add by qy
using NPOI.POIFS;//add by qy
using NPOI.SS.UserModel;
using NPOI.Util;//add by qy
using NPOI.XSSF;
using NPOI.XSSF.UserModel;//add by qy

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
        return getExcelSheetData(fileName, false);
    }

    /// <summary> 
    /// 读取Excel文件中Sheet1的内容 
    /// </summary> 
    /// <param name="fileName">Excel文件名</param> 
    /// <param name="fileName">是否轉換成datetime</param>
    /// <returns>DataTable</returns> 
    public static System.Data.DataTable getExcelSheetData(string fileName, bool isConvertDate)
    {
        FileStream file = null;
        try
        {
            //打开要读取的Excel
            file = new FileStream(fileName, FileMode.Open);
            IWorkbook workbook;
            if (fileName.Contains(".xlsx"))
            {

                workbook = new XSSFWorkbook(file);
            }
            else
            {
                workbook = new HSSFWorkbook(file);
            }
            //读入Excel
            //HSSFWorkbook workbook = new HSSFWorkbook(file);
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
            DateTime dt2000 = new DateTime(2000, 1, 1);
            rowId = 0;
            while (rowId <= sheet.LastRowNum)
            {
                DataRow newRow = dtNew.NewRow();
                //读取所有column
                for (int colIndex = 0; colIndex < columnNum; colIndex++)
                {
                    if (sheet.GetRow(rowId) == null)
                        newRow[dtNew.Columns[colIndex]] = "";
                    else if (isConvertDate)
                    {
                        //newRow[dtNew.Columns[colIndex]] = com.inventec.system.util.StringUtil.Null2String(sheet.GetRow(rowId).GetCell(colIndex));
                        HSSFCell cell = sheet.GetRow(rowId).GetCell(colIndex) as HSSFCell;
                        bool isDate = false;
                        try
                        {
                            if ((cell != null) && (cell.DateCellValue != null) && (cell.DateCellValue > dt2000))
                                isDate = true;
                        }
                        catch
                        {
                        }

                        if (isDate)
                            newRow[dtNew.Columns[colIndex]] = cell.DateCellValue;
                        else
                            newRow[dtNew.Columns[colIndex]] = com.inventec.system.util.StringUtil.Null2String(sheet.GetRow(rowId).GetCell(colIndex));
                    }
                    else
                    {
                        newRow[dtNew.Columns[colIndex]] = com.inventec.system.util.StringUtil.Null2String(sheet.GetRow(rowId).GetCell(colIndex));
                    }
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

    public static MemoryStream ExportDataTableToExcel(DataTable SourceTable)
    {
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        ISheet sheet = workbook.CreateSheet();
        IRow headerRow = sheet.CreateRow(0);
        foreach (DataColumn column in SourceTable.Columns)
            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
        int rowIndex = 1;
        foreach (DataRow row in SourceTable.Rows)
        {
            IRow dataRow = sheet.CreateRow(rowIndex);
            foreach (DataColumn column in SourceTable.Columns)
            {
                string rowValue = row[column.Ordinal].ToString();
                sheet.SetColumnWidth(column.Ordinal, 256 * 20);
                if (rowValue == "&nbsp;")
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue("");
                    continue;
                }
                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
            }
            rowIndex++;
        }
        workbook.Write(ms);
        ms.Flush();
        ms.Position = 0;
        sheet = null;
        headerRow = null;
        workbook = null;
        return ms;
    }

    public static DataTable ExportDataTableFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex, string FileName)
    {

        IWorkbook workbook;
        if (FileName.Contains(".xlsx"))
        {

            workbook = new XSSFWorkbook(ExcelFileStream);
        }
        else
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
        }
        //HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
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

    public static DataTable ExportDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex, string FileName)
    {
        IWorkbook workbook;
        if (FileName.Contains(".xlsx"))
        {

            workbook = new XSSFWorkbook(ExcelFileStream);
        }
        else
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
        }

        //HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
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

    public static DataTable ExportDataTableFromExcel2(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex, string FileName)
    {
        IWorkbook workbook;
        if (FileName.Contains(".xlsx"))
        {

            workbook = new XSSFWorkbook(ExcelFileStream);
        }
        else
        {
            workbook = new HSSFWorkbook(ExcelFileStream);
        }

        //HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
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

        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            DataRow dataRow = table.NewRow();
            if (row == null)
            {
                break;
            }
            for (int j = row.FirstCellNum; j < cellCount; j++)
            {
                if (row.GetCell(0).ToString() == "")
                {
                    break;
                }
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

    public static MemoryStream ExportEPIADefectTrackingReport(IList<string> hidParam, string templetFullFileName)
    {
        FileStream file = null;
        file = new FileStream(templetFullFileName, FileMode.Open);
        IWorkbook workbook;
        if (templetFullFileName.Contains(".xlsx"))
        {

            workbook = new XSSFWorkbook(file);
        }
        else
        {
            workbook = new HSSFWorkbook(file);
        }
        file.Close();
        ISheet sheet = workbook.GetSheetAt(0);

        MemoryStream ms = new MemoryStream();
        ICell cell = sheet.CreateRow(4).CreateCell(0);
        cell.SetCellValue((string)hidParam[0]);
        cell = sheet.CreateRow(4).CreateCell(2);
        cell.SetCellValue((string)hidParam[1]);
        cell = sheet.CreateRow(4).CreateCell(3);
        cell.SetCellValue((string)hidParam[2]);
        cell = sheet.CreateRow(6).CreateCell(0);
        cell.SetCellValue((string)hidParam[3]);
        cell = sheet.CreateRow(6).CreateCell(2);
        cell.SetCellValue((string)hidParam[4]);
        cell = sheet.CreateRow(6).CreateCell(3);
        cell.SetCellValue((string)hidParam[5]);
        cell = sheet.CreateRow(8).CreateCell(0);
        cell.SetCellValue((string)hidParam[6]);

        workbook.Write(ms);
        ms.Flush();
        ms.Position = 0;
        sheet = null;
        workbook = null;
        return ms;
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

}



    
  



