using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI;
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
/// Summary description for GenUnShiftSnList
/// </summary>
public class GenUnShipSnList
{
	public GenUnShipSnList()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string GenExcelFile(string cellName, string path, DataTable dtModel,DataTable dtCustsn,bool isCheck)
    {

        if (dtModel.Rows.Count == 0)
        { throw new Exception("No Data"); }
        HSSFWorkbook workbook = new HSSFWorkbook();
        //  MemoryStream ms = new MemoryStream();
        ISheet sheet = workbook.CreateSheet(cellName);
 
        //Create Header Row
       ICellStyle titleStyle = workbook.CreateCellStyle();
       var styleHeader =ExcelTool.CreateHeaderStyle(workbook,
               HSSFColor.WHITE.index, HSSFColor.GREEN.index);

       IRow rowHeader = sheet.CreateRow(0);
       ICell c1 = rowHeader.CreateCell(0);
       ICell c2 = rowHeader.CreateCell(1);
       ICell c3 = rowHeader.CreateCell(2);
   
       string[] header = new string[] { "机型","排单数量","未出货数量" };
       for (int i = 0; i < header.Length;i++ )
       {
           ICell iCell = rowHeader.CreateCell(i);
           iCell.SetCellValue(header[i]);
           iCell.CellStyle = styleHeader;
           sheet.AutoSizeColumn(i);
       }
       var styleRed = ExcelTool.CreateHeaderStyle(workbook,
             HSSFColor.RED.index, -1,10);
        
        int rowIdx = 1;
        string Model = "";
        string Qty = "";
        string Pass85 = "";
        int unShift = 0;
        int countSN  ;
        int noMVS;
        int maxCol=0;
        DataRow[] drArr;
        if (isCheck)
        { drArr = dtModel.Select("Qty <> Pass85","ID"); }
        else
        { drArr = dtModel.Select(); }

      //  foreach (DataRow dr in dtModel.Rows)
        foreach (DataRow dr in drArr)
        
        {
            IRow row = sheet.CreateRow(rowIdx);
            Model = dr["Model"].ToString().Trim();
            Qty = dr["Qty"].ToString().Trim();
            Pass85 = dr["Pass85"].ToString().Trim();
            noMVS =int.Parse(dr["NoMVS"].ToString().Trim());
            
            unShift = int.Parse(Qty) - int.Parse(Pass85); 

            row.CreateCell(0).SetCellValue(Model);
            row.CreateCell(1).SetCellValue(int.Parse(Qty));
            row.CreateCell(2).SetCellValue(unShift);
            if(dr["HaveNextDN"].ToString().Trim()=="Y")
            { row.Cells[0].CellStyle = styleRed;}
           
            DataRow[] rowSNArr = dtCustsn.Select(" Model='" + Model + "'");
            if (rowSNArr.Length > maxCol) maxCol = rowSNArr.Length;
            countSN =3;
          
            foreach(DataRow drSN in rowSNArr)
            {
                row.CreateCell(countSN).SetCellValue(drSN["Custsn"].ToString().Trim());
                row.CreateCell(countSN+1).SetCellValue(drSN["Station"].ToString().Trim());
                row.CreateCell(countSN + 2).SetCellValue(drSN["Line"].ToString().Trim());
                
                sheet.SetColumnWidth(countSN, 14*256);
                sheet.SetColumnWidth(countSN+1, 6*256);
                sheet.SetColumnWidth(countSN + 2, 6 * 256);
                
                countSN = countSN+3;
                if (countSN > 253) { break; }
            }
              rowIdx++;
        }
   
        //sheet.AutoSizeColumn(0);
        //sheet.AutoSizeColumn(1);
        sheet.SetColumnWidth(0, 16 * 256);
        sheet.SetColumnWidth(1, 14 * 256);
        sheet.SetColumnWidth(2, 14 * 256);

        //for (int k = 3; k < maxCol*2;k++ )
        //{ sheet.AutoSizeColumn(k); }
        string guid = System.Guid.NewGuid().ToString();
        string fileNamePath = path + guid + ".XLS";
        FileStream file = new FileStream(fileNamePath, FileMode.Create);

        workbook.Write(file);
        file.Close();
        ExcelTool.DeleteOldFile(path);
        return guid;

        

        }
      


  }

