using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

/// <summary>
/// Summary description for ToolUtility
/// </summary>
public class ToolUtility
{
	public ToolUtility()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string GetTipString(string descr)
    {
        string s = @"Tip('{0}',SHADOW, true, SHADOWCOLOR, '#dd99aa',
                            FONTSIZE, '12pt',SHADOWWIDTH, 2,OFFSETY,-40,OFFSETX,-25,FADEIN,300)";
        s = string.Format(s, descr);
        return s;
    }

    public void ExportExcel(GridView gv, string FileName, Page pp)
    {
        string serverRoot = HttpContext.Current.Server.MapPath("~").ToString();
        if (!Directory.Exists(serverRoot + "\\TmpExcel"))
        {
            Directory.CreateDirectory(serverRoot + "\\TmpExcel");
        }
        string tmp_filename = ExcelTool.GridViewToExcelSaveLocal(gv, FileName, serverRoot + "\\TmpExcel\\") + ".xls";
        string new_filename = FileName + "_" + pp.Session.SessionID + ".xls";
        if (System.IO.File.Exists(serverRoot + "\\TmpExcel\\" + new_filename))
        {
            System.IO.File.Delete(serverRoot + "\\TmpExcel\\" + new_filename);
        }

        System.IO.File.Copy(serverRoot + "\\TmpExcel\\" + tmp_filename, serverRoot + "\\TmpExcel\\" + new_filename);
        pp.Response.Redirect("~" + "\\TmpExcel\\" + new_filename);

        return;

        //GridView gv = (GridView)Page.Form.FindControl(gvc.Replace('_', '$')); //gvResult     

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gv.AllowPaging = false;
        gv.Visible = true;
        //gv.DataBind();
        //Change the Header Row back to white color     
        gv.HeaderRow.Style.Add("background-color", "green");
        //Apply style to Individual Cells     
        /*gv.HeaderRow.Cells[0].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[1].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[2].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[3].Style.Add("background-color", "green");
         */

        /*for (int i = 0; i <= gv.Rows.Count-1; i++)
        {
            GridViewRow row = gv.Rows[i];
            //Change Color back to white         
            row.BackColor = System.Drawing.Color.White;
            //Apply text style to each Row         
            row.Attributes.Add("class", "textmode");
            //Apply style to Individual Cells of Alternating Row         
            if (i % 2 != 0)
            {
                row.Cells[0].Style.Add("background-color", "#C2D69B");
                row.Cells[1].Style.Add("background-color", "#C2D69B");
                row.Cells[2].Style.Add("background-color", "#C2D69B");
                row.Cells[3].Style.Add("background-color", "#C2D69B");
            }
        }*/

        gv.RenderControl(hw);

        pp.Response.Clear();
        pp.Response.ClearHeaders();
        pp.Response.ClearContent();
        pp.Response.Buffer = true;
        pp.Response.Charset = "";
        //pp.Response.ContentType = "application/vnd.ms-excel";
        pp.Response.ContentType = "application/download";

        pp.Response.AddHeader("Cache-Control", "max-age=0");


        //style to format numbers to string     
        //string style = @"&lt;style&gt; .textmode { mso-number-format:\@; } &lt;/style&gt;";
        //Response.Write(style);
        pp.Response.Output.Write(sw.ToString());
        pp.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
        pp.Response.Flush();
        pp.Response.Close();

        pp.Response.ContentType = "application/download";
        pp.Response.Output.Write(sw.ToString());
        pp.Response.End();

    }

    public void ResponseWriteFile(Page pp, string downloadname, FileInfo File)
    {
        if (!File.Exists)
        {
            throw new Exception("File Not Exists!");
        }
        string mime_type = "";
        switch (File.Extension.ToUpper())
        {
            case ".XLSX":
                mime_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case ".XLS":
                mime_type = "application/x-excel";
                break;
            case ".ZIP":
                mime_type = "application/zip";
                break;

            case ".PDF":
            case ".DWG":
            case ".AI":
            default:
                mime_type = "application/force-download";
                break;
        }

        FileStream myFile = new FileStream(File.DirectoryName + "\\" + File.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        pp.Response.AddHeader("Content-Type ", mime_type);

        if (pp.Request.Browser.Browser.ToUpper() == "IE")
        {
            pp.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(downloadname, System.Text.Encoding.UTF8));
        }
        else
        {
            pp.Response.AddHeader("Content-Disposition", "attachment; filename=" + downloadname);
        }

        pp.Response.WriteFile(File.FullName);

    }

    public string GetStationDescr(DataTable dt, string station)
    {
        string descr = "";
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Station"].ToString().Trim() == station)
            {
                descr = dr["Descr"].ToString().Trim();
                return descr;
            }
        }
        return descr;
    }
    public bool DataTableToXmlString(DataTable dataInfo, ref string dataString)
    {
        XmlSerializer xmlSer = new XmlSerializer(typeof(DataTable));
        Encoding encode = new System.Text.UTF8Encoding();
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);

        xmlSer.Serialize(xtw, dataInfo);
        dataString = Encoding.UTF8.GetString(ms.ToArray()).Trim();

        xtw.Close();
        ms.Close();
        return true;
    }
    public bool XmlStringToDataTable(string dataString, ref DataTable dataInfo)
    {
        XmlSerializer ser = new XmlSerializer(typeof(DataTable));
        StringReader sr = new StringReader(dataString);
        dataInfo = (DataTable)ser.Deserialize(sr);
        sr.Close();
        return true;
    }

    private static byte[] GetFileByteData(String fileName)
    {

        FileStream fs = new FileStream(fileName, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        byte[] testArray = new byte[fs.Length];
        int count = br.Read(testArray, 0, testArray.Length);

        br.Close();

        return testArray;
    }

 
    private DataTable getNullDataTable(string colList)
    {
        DataTable dt = initTable(colList);
        string[] colArr = colList.Split(',');
        DataRow newRow = null;
        newRow = dt.NewRow();
        foreach (string col in colArr)
        {
            newRow[col] = "";

        }
        dt.Rows.Add(newRow);

        return dt;
    }
    private DataTable initTable(string colList)
    {
        DataTable retTable = new DataTable();
        string[] colArr = colList.Split(',');
        foreach (string col in colArr)
        {
            retTable.Columns.Add(col, Type.GetType("System.String"));
        }
        return retTable;
    }

    public  DataTable getExcelSheetData(string fileName, bool isConvertDate)
    {
        FileStream file = null;
        try
        {
            //打开要读取的Excel
            file = new FileStream(fileName, FileMode.Open);
            IWorkbook workbook;
            if (fileName.Contains(".xlsx"))
            {

                throw new Exception("Excel格式不正確"); 
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
                string Columnsname = sheet.GetRow(0).Cells[columnIndex].ToString();
                DataColumn dc = new DataColumn(Columnsname);
                dtNew.Columns.Add(dc);
            }
            DateTime dt2000 = new DateTime(2000, 1, 1);
            rowId = 1;
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
                            newRow[dtNew.Columns[colIndex]] = Null2String(sheet.GetRow(rowId).GetCell(colIndex));
                    }
                    else
                    {
                        newRow[dtNew.Columns[colIndex]] = Null2String(sheet.GetRow(rowId).GetCell(colIndex));
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
    public DataTable getExcelSheetData(Stream ExcelFileStream, bool isConvertDate)
    {
        FileStream file = null;
        try
        {
            //打开要读取的Excel
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
           // file = new FileStream(fileName, FileMode.Open);
           // IWorkbook workbook;
            //if (ExcelFileStream..Contains(".xlsx"))
            //{

            //    throw new Exception("Excel格式不正確");
            //}
            //else
            //{
            //    workbook = new HSSFWorkbook(file);
            //}
            //读入Excel
            //HSSFWorkbook workbook = new HSSFWorkbook(file);
           // file.Close();
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
                string Columnsname = sheet.GetRow(0).Cells[columnIndex].ToString();
                DataColumn dc = new DataColumn(Columnsname);
                dtNew.Columns.Add(dc);
            }
            DateTime dt2000 = new DateTime(2000, 1, 1);
            rowId = 1;
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
                            newRow[dtNew.Columns[colIndex]] = Null2String(sheet.GetRow(rowId).GetCell(colIndex));
                    }
                    else
                    {
                        newRow[dtNew.Columns[colIndex]] = Null2String(sheet.GetRow(rowId).GetCell(colIndex));
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

    private  string Null2String(object obj)
    {
        if (obj == null)
        {
            return "";
        }
        return obj.ToString();
    }
}
