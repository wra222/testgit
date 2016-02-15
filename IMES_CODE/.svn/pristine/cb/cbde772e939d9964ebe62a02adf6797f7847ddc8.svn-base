using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using System.Xml.Serialization;
using System.Xml;

using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

/// <summary>
/// ExcelTool 的摘要描述
/// </summary>
namespace AlarmMail
{
    public class ExcelTool
{
	public ExcelTool()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}
    public static ICellStyle CreateHeaderStyle(HSSFWorkbook wb, short foreColor, short backgroundColor)
    {
        var style = wb.CreateCellStyle();
        style.FillForegroundColor = (short)17;
        var font = wb.CreateFont();
        font.Color = foreColor;
        font.FontHeightInPoints = 12;
        style.SetFont(font);

        style.FillBackgroundColor = 16;
        style.FillForegroundColor = backgroundColor;
        style.FillPattern = FillPatternType.SOLID_FOREGROUND;


        return style;
    }
    public static ICellStyle CreateHeaderStyle(HSSFWorkbook wb, short foreColor, short backgroundColor,short fontSize)
    {
        var style = wb.CreateCellStyle();
      //  style.FillForegroundColor = (short)17;
        var font = wb.CreateFont();
        font.Color = foreColor;
        font.FontHeightInPoints = fontSize;
        style.SetFont(font);

       // style.FillBackgroundColor = 16;
        if (backgroundColor > 0)
        { style.FillForegroundColor = backgroundColor; }
        
     //   style.FillPattern = FillPatternType.SOLID_FOREGROUND;


        return style;
    }
 

    private static void CreateHeadRow(DataTable dt, HSSFWorkbook workbook, ISheet sheet)
    {
        ICellStyle titleStyle = workbook.CreateCellStyle();
        var styleHeader = CreateHeaderStyle(workbook,
                HSSFColor.WHITE.index, HSSFColor.GREEN.index);

        IRow row = sheet.CreateRow(0);
        ICell c1 = row.CreateCell(0);
        ICell c2 = row.CreateCell(1);
        ICell c3 = row.CreateCell(2);
        int i = 0;
        foreach (DataColumn  cell in dt.Columns)
        {
            ICell iCell = row.CreateCell(i);
            iCell.SetCellValue(cell.ColumnName);
            iCell.CellStyle = styleHeader;
            sheet.AutoSizeColumn(i);
            i++;
        }
    }
    public static void DeleteOldFile(string path)
    {
        try
        {
            DirectoryInfo d = new DirectoryInfo(path);
          
            FileInfo[] fi;
            fi = d.GetFiles();
            foreach (FileInfo f in fi)
            {
                if (f.CreationTime < DateTime.Now.AddMinutes(-5))
                {
                    f.Delete();
                }


            }
        }
        catch
        { 
            return;
        }
       
    
    }
   

    private static bool CheckValueInArray(int value, int[] dataArr)
    {
        bool r = false;
        foreach(int i in dataArr)
        {
            if (i == value) { r = true; break; }
        }
        return r;
    }
   


    public static MemoryStream DataTableToExcel(DataTable dtSrc, string cellName, params int[] numericColIdx)
    {
        if (dtSrc.Rows.Count == 0)
        { throw new Exception("No Data"); }
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        ISheet sheet = workbook.CreateSheet(cellName);
        CreateHeadRow(dtSrc, workbook, sheet);

        int rowIdx = 1;

        bool needCheckNumeric = false;
        double douTemp;
        if (numericColIdx != null && numericColIdx.Length > 0)
        { needCheckNumeric = true; }
        foreach (DataRow dr in dtSrc.Rows)
        {
            IRow row = sheet.CreateRow(rowIdx);
            for (int i = 0; i < dr.ItemArray.Length ; i++)
            {
                string item = dr[i].ToString().Replace("&nbsp;", "");                
                if (needCheckNumeric)
                {                    
                    if (CheckValueInArray(i, numericColIdx))
                    {
                        if (double.TryParse(item, out douTemp))
                        { row.CreateCell(i).SetCellValue(douTemp); }
                        else
                        { row.CreateCell(i).SetCellValue(item); }
                    }
                    else
                    { row.CreateCell(i).SetCellValue(item); }
                }
                else
                {
                    row.CreateCell(i).SetCellValue(item);
                }
                if (rowIdx == dtSrc.Rows.Count)
                {
                    sheet.AutoSizeColumn(i);
                }
             //   sheet.AutoSizeColumn(i);
            }
            rowIdx++;
        }
       
        workbook.Write(ms);
        return ms;
    }
    public static MemoryStream DataTableToExcel(DataTable[] dtSrc, string[] sheetNames) { 
        int[] strindex = {};
        return DataTableToExcel(dtSrc, sheetNames, strindex);
    }


    public static MemoryStream DataTableToExcel(DataTable[] dtSrc, string[] sheetNames, int[] AbsoluteStringIndex)
    {        
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        for (int m = 0; m < dtSrc.Length; m++)
        {
            if (dtSrc[m].Rows.Count > 0)
            {
                ISheet sheet = workbook.CreateSheet(sheetNames[m]);
                CreateHeadRow(dtSrc[m], workbook, sheet);

                int rowIdx = 1;
                foreach (DataRow dr in dtSrc[m].Rows)
                {
                    IRow row = sheet.CreateRow(rowIdx);
                    for (int i = 0; i < dr.ItemArray.Length; i++)
                    {
                        string item = dr[i].ToString().Replace("&nbsp;", "");
                        row.CreateCell(i);
                        
                        int tt ;
                        System.Diagnostics.Debug.WriteLine(item);

                        if (Array.IndexOf(AbsoluteStringIndex, i) > -1 || 
                            !(int.TryParse(item,out tt))
                            )
                        {
                            row.Cells[i].SetCellType(CellType.STRING);
                            row.Cells[i].SetCellValue(item);
                        }
                        else
                        {                            
                            row.Cells[i].SetCellType(CellType.NUMERIC);
                            row.Cells[i].SetCellValue(tt);
                        }                        
                        
                        if (rowIdx == dtSrc[m].Rows.Count)
                        {
                            sheet.AutoSizeColumn(i);
                        }
                    }
                    rowIdx++;
                }
            }
        }
        
        workbook.Write(ms);
        return ms;

    }
    public static string DataTableToExcelSaveLocal(DataTable dtSrc, string cellName, string path, params int[] numericColIdx)
    {
        if (dtSrc.Rows.Count == 0)
        { throw new Exception("No Data"); }
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        ISheet sheet = workbook.CreateSheet(cellName);
        CreateHeadRow(dtSrc, workbook, sheet);

        int rowIdx = 1;

        bool needCheckNumeric = false;
        double douTemp;
        if (numericColIdx != null && numericColIdx.Length > 0)
        { needCheckNumeric = true; }
        foreach (DataRow dr in dtSrc.Rows)
        {
            IRow row = sheet.CreateRow(rowIdx);
            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                string item = dr[i].ToString().Replace("&nbsp;", "");
                if (needCheckNumeric)
                {
                    if (CheckValueInArray(i, numericColIdx))
                    {
                        if (double.TryParse(item, out douTemp))
                        { row.CreateCell(i).SetCellValue(douTemp); }
                        else
                        { row.CreateCell(i).SetCellValue(item); }
                    }
                    else
                    { row.CreateCell(i).SetCellValue(item); }
                }
                else
                {
                    row.CreateCell(i).SetCellValue(item);
                }
                if (rowIdx == dtSrc.Rows.Count)
                {
                    sheet.AutoSizeColumn(i);
                }
                //   sheet.AutoSizeColumn(i);
            }
            rowIdx++;
        }
        string guid=System.Guid.NewGuid().ToString();
        string fileNamePath =path +guid+".XLS";
        FileStream file = new FileStream(fileNamePath, FileMode.Create);

        workbook.Write(file);
        file.Close();
        DeleteOldFile(path);
        return guid;

    }                                                                                        //(DataTable dtSrc, string cellName, string path, params int[] numericColIdx)
    public static string DataTableToExcelSaveLocal_FA(DataTable dtSrc, string cellName, int startIdx, string endStation, string path, params int[] numericColIdx)
    {

        int endIdx = 0;
        int j = 0;
        foreach (DataColumn cell in dtSrc.Columns)
        {
            if (cell.ColumnName.Trim() == endStation)
            { endIdx = j; break; }

            j++;
        }

        if (dtSrc.Rows.Count == 0)
        { throw new Exception("No Data"); }
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        ISheet sheet = workbook.CreateSheet(cellName);
        CreateHeadRow(dtSrc, workbook, sheet);

        int rowIdx = 1;
        string tmpModel = "";
        bool needCheckNumeric = false;
        double douTemp;
        if (numericColIdx != null && numericColIdx.Length > 0)
        { needCheckNumeric = true; }
        string Model = "";
        bool IsSameModel = false;
        foreach (DataRow dr in dtSrc.Rows)
        {
            IsSameModel = false;
            IRow row = sheet.CreateRow(rowIdx);
            Model = dr[1].ToString().Replace("&nbsp;", "");
            if (tmpModel != Model)
            { tmpModel = Model; }
            else
            { IsSameModel = true; }

            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                 string item = dr[i].ToString().Replace("&nbsp;", "");
                if (needCheckNumeric)
                {
                    if (CheckValueInArray(i, numericColIdx))
                    {
                        if (double.TryParse(item, out douTemp))
                        { row.CreateCell(i).SetCellValue(douTemp); }
                        else
                        { row.CreateCell(i).SetCellValue(item); }
                    }
                    else
                    { row.CreateCell(i).SetCellValue(item); }
                }
                else
                {
                    row.CreateCell(i).SetCellValue(item);
                }
                if (i >= startIdx && i <= endIdx)
                {
                   if (IsSameModel)
                    {
                        row.GetCell(i).SetCellValue("");
                    }
                }
                 if (rowIdx == dtSrc.Rows.Count)
                {
                    sheet.AutoSizeColumn(i);
                }

            }
            rowIdx++;
        }

        string guid = System.Guid.NewGuid().ToString();
        string fileNamePath = path + guid + ".XLS";
        FileStream file = new FileStream(fileNamePath, FileMode.Create);

        workbook.Write(file);
        file.Close();
        DeleteOldFile(path);
        return guid;
     
    }
}
}
    