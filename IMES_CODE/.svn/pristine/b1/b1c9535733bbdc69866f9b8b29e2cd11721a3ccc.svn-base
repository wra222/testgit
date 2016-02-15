using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using com.inventec.imes.DBUtility;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

public partial class Query_PAK_PAKKitPartQuery : IMESQueryBasePage
{

    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_ChepPallet ChepPallet = ServiceAgent.getInstance().GetObjectByName<IPAK_ChepPallet>(WebConstant.ChepPallet);
    IPAK_KitPartQuery PAK_KitPartQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_KitPartQuery>(WebConstant.IPAK_KitPartQuery);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
      
        try
        {  
            if (!this.IsPostBack)
            {
                InitPage();
                InitCondition();
            }
        }

        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    private void InitPage()
    {


    }
    private void InitCondition()
    {

        gvResult.DataSource = getNullDataTable("PartNo,Descr,Qty,Line");
        gvResult.DataBind();
      //  InitGridView();
    }

    private void InitGridView()
    {
        int i = 100;
        int j = 70;
        int k = 150;
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);
        gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(j);
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        //retTable.Columns.Add("Pallet No", Type.GetType("System.String"));
        //retTable.Columns.Add("RFID Pallet No", Type.GetType("System.String"));
        //retTable.Columns.Add("Carrier", Type.GetType("System.String"));
        //retTable.Columns.Add("Editor", Type.GetType("System.String"));
        //retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        //retTable.Columns.Add("Udt", Type.GetType("System.String"));
        return retTable;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void ShowTotal(int Count)
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "ShowTotal();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "ShowTotal", script, false);
    }
    private void BindNoData()
    {

        this.gvResult.DataBind();
        InitGridView();

    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{

    //}
    public DataTable BuildPartTable()
    {
        DataTable table = new DataTable();


        //{
        DataColumn column = new DataColumn("PartNo");
        table.Columns.Add(column);
        DataColumn column2 = new DataColumn("Descr");
        table.Columns.Add(column2);
        DataColumn column3 = new DataColumn("Qty");
        table.Columns.Add(column3);
        return table;

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.FileName == "")
        {
            showErrorMessage("Please select excel file!!");
            return;
        }
        beginWaitingCoverDiv();
        try
        {
          //  DataTable table = RenderDataTableFromExcel(this.FileUpload1.FileContent, 0);
            DataTable table = ConvertToDataTable(this.FileUpload1.FileContent);
            DataTable dt = PAK_KitPartQuery.GetData(DBConnection,table, chkGrpByLine.Checked);
            gvResult.DataSource = dt;
            gvResult.DataBind();
            if (dt.Rows.Count > 0)
            {
                EnableBtnExcel(this, true, btnExcel.ClientID);
            }
            else
            {
                //       DataTable dt=
                
                gvResult.DataSource = getNullDataTable("PartNo,Descr,Qty,Line");
                gvResult.DataBind();
                EnableBtnExcel(this, false, btnExcel.ClientID);
            }

        }
        catch (Exception exp)
        {
            showErrorMessage(exp.Message);

        }
        finally
        { endWaitingCoverDiv(); }



    }
    //private string SaveFile()
    //{

    //    string filePath = HttpContext.Current.Server.MapPath("~");
    //    string path = filePath + "\\tmp";
    //    string fullFileName;
    //    try
    //    {
    //        if (Directory.Exists(path) == false)
    //        {
    //            Directory.CreateDirectory(path);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //ignore
    //    }

    //    Guid guid = System.Guid.NewGuid();

    //    string extName = ".xls";
    //    fullFileName = path + "\\" + guid.ToString() + extName;
    //    //    fullFileName = path + "\\" + "xxxxxxxxxx.xls";
    //    //    FileUpload1.SaveAs(fullFileName);

    //    DataTable table = RenderDataTableFromExcel(this.FileUpload1.FileContent, 0);
    //    int i = table.Rows.Count;
    //    gvResult.DataSource = table;
    //    gvResult.DataBind();

    //    return fullFileName;

    //}
    public DataTable ConvertToDataTable(Stream ExcelFileStream)
    {
        HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
        ISheet sheet = workbook.GetSheetAt(0);
        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
        DataTable table = getNullDataTable("Model,Qty,Line");
        int tmp = 0;
        int lastCellNum = 3;
        string model = "";
        string qty = "";
        string line = "";
        if (!chkGrpByLine.Checked) { lastCellNum = 2; }
        while (rows.MoveNext())
        {
            IRow row = (HSSFRow)rows.Current;
            DataRow dr = table.NewRow();

            for (int i = 0; i < lastCellNum; i++)
            {
               ICell cell = row.GetCell(i);
                if (cell == null)
                {
                    dr[i] = "";
                }
                else
                {
                    dr[i] = cell.ToString();
                }
            }
            model = dr[0].ToString();
            qty = dr[1].ToString();
            line = dr[2].ToString();
            if (chkGrpByLine.Checked)
            {
                if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(qty) || string.IsNullOrEmpty(line))
                { continue; }
            }
            else
            {
                if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(qty))
                { continue; }
            }
            if (!int.TryParse(qty, out tmp))
            { throw new Exception("Excel格式不正確"); }
         
            table.Rows.Add(dr);
        }
        return table;
    
    }

    public  DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex)
    {
        HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
        ISheet sheet = workbook.GetSheetAt(SheetIndex);

        DataTable table = new DataTable();

        IRow headerRow = sheet.GetRow(0);
        int cellCount = headerRow.LastCellNum;
      // 
      
        DataColumn column = new DataColumn("Model");
        table.Columns.Add(column);
        DataColumn column2 = new DataColumn("Qty");
        table.Columns.Add(column2);
        DataColumn column3 = new DataColumn("Line");
        table.Columns.Add(column3);
        int rowCount = sheet.LastRowNum;
        int tmp;
        string model ="";
        string qty = "";
        string line = "";
        for (int i = 0; i < sheet.LastRowNum + 1; i++)
        {
            
            IRow row = sheet.GetRow(i);
            DataRow dataRow = table.NewRow();
            if (row == null) { continue; }
            for (int j = row.FirstCellNum; j < cellCount; j++)
            {
                if (row.GetCell(0)!=null) { model = row.GetCell(0).ToString(); }
                if (row.GetCell(1)!=null) { qty = row.GetCell(1).ToString(); }
                if (row.GetCell(2)!=null) { line = row.GetCell(2).ToString(); }
               
         
                if (chkGrpByLine.Checked)
                {
                    if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(qty) || string.IsNullOrEmpty(line))
                    { continue; }
                }
                else
                {
                    if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(qty))
                    { continue; }
                }
               
                if(!int.TryParse(row.GetCell(1).ToString(),out tmp))
                { throw new Exception("Excel格式不正確"); }
                if (row.GetCell(j) != null)
                    dataRow[j] = row.GetCell(j).ToString();
            }

            table.Rows.Add(dataRow);
        }

        ExcelFileStream.Close();
        workbook = null;
        sheet = null;
        return table;
    }
    private  void CreateHeadRow(GridView gr, HSSFWorkbook workbook, ISheet sheet)
    {
        ICellStyle titleStyle = workbook.CreateCellStyle();
        var styleHeader = CreateHeaderStyle(workbook,
                HSSFColor.WHITE.index, HSSFColor.GREEN.index);

        IRow row = sheet.CreateRow(0);
        ICell c1 = row.CreateCell(0);
        ICell c2 = row.CreateCell(1);
        ICell c3 = row.CreateCell(2);
        int i = 0;
        foreach (TableCell cell in gr.HeaderRow.Cells)
        {
            ICell iCell = row.CreateCell(i);
            iCell.SetCellValue(cell.Text);
            iCell.CellStyle = styleHeader;
            sheet.AutoSizeColumn(i);
            i++;
        }

    }
    private  bool CheckValueInArray(int value, int[] dataArr)
    {
        bool r = false;
        foreach (int i in dataArr)
        {
            if (i == value) { r = true; break; }
        }
        return r;
    }
    public  MemoryStream GridViewToExcel(GridView grSrc, string cellName, params int[] numericColIdx)
    {
        if (grSrc.Rows.Count == 0)
        { throw new Exception("No Data"); }
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        ISheet sheet = workbook.CreateSheet(cellName);
        CreateHeadRow(grSrc, workbook, sheet);
        string model = "";
        int rowIdx = 1;

        bool needCheckNumeric = false;
        double douTemp;
        if (numericColIdx != null && numericColIdx.Length > 0)
        { needCheckNumeric = true; }
        foreach (GridViewRow gr in grSrc.Rows)
        {
            model = gr.Cells[0].Text;
            IRow row = sheet.CreateRow(rowIdx);
            for (int i = 0; i < gr.Cells.Count; i++)
            {
                gr.Cells[i].Text = gr.Cells[i].Text.Replace("&nbsp;", "");
                if (needCheckNumeric)
                {
                    if (CheckValueInArray(i, numericColIdx))
                    {
                        if (double.TryParse(gr.Cells[i].Text, out douTemp))
                        { row.CreateCell(i).SetCellValue(douTemp); }
                        else
                        { row.CreateCell(i).SetCellValue(gr.Cells[i].Text); }


                    }
                    else
                    { row.CreateCell(i).SetCellValue(gr.Cells[i].Text); }

                }
                else
                {
                    row.CreateCell(i).SetCellValue(gr.Cells[i].Text);
                }
              ICell cell=row.GetCell(i);
              if (model.Length == 12 && gr.Cells[1].Text == "" && gr.Cells[2].Text == "0")
              {
                  if (i == 0)
                  {
                      var redStyle = CreateRedStyle(workbook, HSSFColor.RED.index);
                      cell.CellStyle = redStyle;
                      sheet.AutoSizeColumn(i);

                  }
                  if (i == 2)
                  {
                      ICell cell2 = row.GetCell(2);
                      cell2.SetCellValue("");
                  }
               
                //  c1.SetCellValue(gr.Cells[0].Text);
              }
                sheet.AutoSizeColumn(i);
            }
            rowIdx++;

        }
        workbook.Write(ms);
        return ms;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {

        if (gvResult.HeaderRow.Cells.Count==4) // Group By Line
        {
            ExportExcelByNPOI();
        }
        else
        {
            MemoryStream ms = GridViewToExcel(gvResult, "Data", 2);
            this.Response.ContentType = "application/download";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
            this.Response.Clear();
            this.Response.BinaryWrite(ms.GetBuffer());
            ms.Close();
            ms.Dispose();
        }
    }
    public ICellStyle CreateHeaderStyle(
          HSSFWorkbook wb, short foreColor, short backgroundColor)
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
    public ICellStyle CreateRedStyle(
          HSSFWorkbook wb, short foreColor)
    {
        var style = wb.CreateCellStyle();
      //  style.FillForegroundColor = (short)17;
        var font = wb.CreateFont();
        font.Color = foreColor;
        font.FontHeightInPoints = 12;
        style.SetFont(font);

     //   style.FillBackgroundColor = 16;
   //     style.FillForegroundColor = backgroundColor;
        style.FillPattern = FillPatternType.SOLID_FOREGROUND;


        return style;
    }
   
    public void ExportExcelByNPOI()
    {
        string line = "";
        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream ms = new MemoryStream();
        ISheet sheet = null;
        string model = "";
      
        int rowIdx = 1;
        foreach (GridViewRow gr in gvResult.Rows)
        {
            model = gr.Cells[0].Text.Trim();
            if (gr.Cells[3].Text != line)
            {
                IRow row;
              //  ICellStyle titleStyle = workbook.CreateCellStyle();

                var styleHeader = CreateHeaderStyle(workbook,
                        HSSFColor.WHITE.index, HSSFColor.GREEN.index);

                if (workbook.GetSheet(gr.Cells[3].Text) == null)
                {
                    sheet = workbook.CreateSheet(gr.Cells[3].Text);
                }

                row = sheet.CreateRow(0);
                ICell c1 = row.CreateCell(0);
                ICell c2 = row.CreateCell(1);
                ICell c3 = row.CreateCell(2);
                c1.SetCellValue("Part No");
                c2.SetCellValue("Description");
                c3.SetCellValue("Qty");

                c1.CellStyle = styleHeader;
                c2.CellStyle = styleHeader;
                c3.CellStyle = styleHeader;

                sheet.SetColumnWidth(0, 20 * 256);
                sheet.SetColumnWidth(1, 25 * 256);
                sheet.SetColumnWidth(2, 8 * 256);

                rowIdx = 1;
            }
            IRow row2 = sheet.CreateRow(rowIdx);
            if (model.Length == 12 && string.IsNullOrEmpty(gr.Cells[1].Text) && gr.Cells[2].Text=="0")
            {
                var redStyle = CreateRedStyle(workbook,HSSFColor.RED.index);
                ICell c1 = row2.CreateCell(0);
                c1.CellStyle = redStyle;
                c1.SetCellValue(gr.Cells[0].Text);
           
             //   row2.CreateCell(0).CellStyle.FillForegroundColor = HSSFColor.RED.index;
                //row2.CreateCell(1).SetCellValue(gr.Cells[1].Text.Replace("&nbsp;", ""));
                // row2.CreateCell(2).SetCellValue(gr.Cells[2].Text);
                row2.CreateCell(2).SetCellValue("");
            }
            else
            {
                row2.CreateCell(0).SetCellValue(gr.Cells[0].Text);
                row2.CreateCell(1).SetCellValue(gr.Cells[1].Text);
                // row2.CreateCell(2).SetCellValue(gr.Cells[2].Text);
                row2.CreateCell(2).SetCellValue(double.Parse(gr.Cells[2].Text));
            }


            rowIdx++;
            line = gr.Cells[3].Text;
        }


        workbook.Write(ms);
        //  logger.Info("end npoi");

        this.Response.ContentType = "application/download";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        this.Response.Clear();
        this.Response.BinaryWrite(ms.GetBuffer());

        ms.Close();
        ms.Dispose();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtModel.Text))
        {
            showErrorMessage("Please Input Model!!");
            return;
        }
        try
        {
            //  DataTable table = RenderDataTableFromExcel(this.FileUpload1.FileContent, 0);
           
            DataTable table = getNullDataTable("Model,Qty,Line");
            DataRow dataRow = table.NewRow();
            dataRow[0] = txtModel.Text.Trim();
            dataRow[1] = "1";
            dataRow[2] = "";
            table.Rows.Add(dataRow);
            DataTable dt = PAK_KitPartQuery.GetData(DBConnection,table, false);
            gvResult.DataSource = dt;
            gvResult.DataBind();
            if (dt.Rows.Count > 0)
            {
                EnableBtnExcel(this, true, btnExcel.ClientID);
            }
            else
            {
                //       DataTable dt=

                gvResult.DataSource = getNullDataTable("PartNo,Descr,Qty,Line");
                gvResult.DataBind();
                EnableBtnExcel(this, false, btnExcel.ClientID);
            }

        }
        catch (Exception exp)
        {
            showErrorMessage(exp.Message);

        }
   

    }
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        { 
            e.Row.Cells[1].Text=  e.Row.Cells[1].Text.Replace("&nbsp;","");
            if( e.Row.Cells[0].Text.StartsWith("_") &&  e.Row.Cells[1].Text=="")
            {
                e.Row.Cells[0].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[0].Text = e.Row.Cells[0].Text.Replace("_", "");
                if (chkGrpByLine.Checked)
                {
                    if (e.Row.Cells.Count == 4) //Group by Line
                    { e.Row.Cells[3].Text = e.Row.Cells[3].Text.Replace("_", ""); }
           
                }
            }
         
        }
    }
}
