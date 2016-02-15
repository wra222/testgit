/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:service for QC Repair Page
 * UI:CI-MES12-SPEC-FA-UI QC Repair.docx –2011/10/19 
 * UC:CI-MES12-SPEC-FA-UC QC Repair.docx –2011/10/19            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-14   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-0434, Jessica Liu, 2012-2-28
*/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
//2012-7-5，新需求
using System.IO;
using System.Reflection;
using System.Diagnostics;
//using Excel;
using System.Runtime.InteropServices;
//using System.Xml.Serialization;
//using com.inventec.iMESWEB;
//using NPOI;//add by qy
//using NPOI.HSSF.UserModel;
//using NPOI.HSSF.Util;//add by qy
//using NPOI.POIFS;//add by qy
//using NPOI.SS.UserModel;
//using NPOI.Util;//add by qy
//using NPOI.XSSF;
//using NPOI.XSSF.UserModel;//add by qy


public partial class FA_PAQCRepair : IMESBasePage 
{
    //2012-7-5，新需求
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //Jessica Liu, 2012-4-17
    private const int DEFAULT_ROWS = 6;    //12;
    private IPAQCRepair iPAQCRepair;
    private string commBllName = WebConstant.CommonObject;
    private string repairBllName = WebConstant.PAQCRepairObject;

    private IProduct iProduct;
    private IRepair iRepairLog;
    private Object commServiceObj;
    private Object repairServiceObj;
    private ITestStation iTestStation;
    public String UserId;
    public String Customer;
    //2012-7-5，新需求
    private const string templetFile = "EPIADefectTrackingReport.xls";
    private const string outputFile = ".xls";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //2012-4-16
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;

            if (!this.IsPostBack)
            {
                initLabel();
                bindTable(null, 12);
                setColumnWidth();            
                CmbPdLine.Station = Request["station"];
                //CmbPdLine.Station = "75";//Dean Test 20110312
                
                CmbPdLine.Customer = Master.userInfo.Customer;
                /*
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
                 */

                //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                refreshCmbReturnStation(null, "0");
            }

            repairServiceObj = ServiceAgent.getInstance().GetObjectByName<IPAQCRepair>(repairBllName);
            commServiceObj = ServiceAgent.getInstance().GetObjectByName<IRepair>(commBllName);

            iPAQCRepair = (IPAQCRepair)repairServiceObj;
            iRepairLog = (IRepair)commServiceObj;
            iProduct = (IProduct)commServiceObj;
            iTestStation = (ITestStation)commServiceObj;
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
            this.hidRecordCount.Value = "0";
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            refreshCmbReturnStation(null, "0");
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
            this.hidRecordCount.Value = "0";
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            refreshCmbReturnStation(null, "0");
        }
    }
   
    private void initLabel()
    {
        this.lblPdLine.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPdline");
        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnEdit.InnerText = this.GetLocalResourceObject(Pre + "_btnEdit").ToString();
        this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        //this.btnSave.InnerText = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

        this.lblProdIDCustSN.Text = this.GetLocalResourceObject(Pre + "_lblProdidCustSN").ToString();
        this.lblRepairLog.Text = this.GetLocalResourceObject(Pre + "_lblRepairLog").ToString();
        this.lblTestStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();

        //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblReturnStation.Text = this.GetLocalResourceObject(Pre + "_lblReturnStation").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(20);
    }

    //2012-7-5，新需求
    public static byte[] GetFileByteData(String fileName)
    {

        FileStream fs = new FileStream(fileName, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        byte[] testArray = new byte[fs.Length];
        int count = br.Read(testArray, 0, testArray.Length);

        br.Close();

        return testArray;
    }

    //2012-7-5，新需求
    protected void hidbtnforexcel_ServerClick(Object sender, EventArgs e)
    {
        //DateTime beforeTime;
        //DateTime afterTime;

        //获得存放路径
        string filePath = HttpContext.Current.Server.MapPath("~");
        string path = filePath + "\\tmp";
        try
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }
        catch (FisException ex)
        {
            //ignore
        }
        string templetFullFileName = path + "\\" + templetFile;

        //FileStream file = null;
        //file = new FileStream(templetFullFileName, FileMode.Open);
        //Guid guid = System.Guid.NewGuid();
        //string templetFullFileName = path + "\\" + templetFile;
        //string outputFullFileName = path + "\\" + guid.ToString() + outputFile;

        //System.Diagnostics.Process hProcess = null;

        //Microsoft.Office.Interop.Excel.Application app = null;
        //Microsoft.Office.Interop.Excel.Workbook workBook = null;
        //Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        try
        {

            //读入Excel

            //IWorkbook workbook;

            //if (templetFullFileName.Contains(".xlsx"))
            //{

            //    workbook = new XSSFWorkbook(file);
            //}
            //else
            //{
            //    workbook = new HSSFWorkbook(file);
            //}
            ////HSSFWorkbook workbook = new HSSFWorkbook(file);
            //file.Close();
            //ISheet sheet = workbook.GetSheetAt(0);
            IList<string> hidParam = new List<string>();
            hidParam.Add((string)this.hidParam1.Value);
            hidParam.Add((string)this.hidParam2.Value);
            hidParam.Add((string)this.hidParam3.Value);
            hidParam.Add((string)this.hidParam4.Value);
            hidParam.Add((string)this.hidParam5.Value);
            hidParam.Add((string)this.hidParam6.Value);
            hidParam.Add((string)this.hidParam7.Value);

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            //HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = ExcelManager.ExportEPIADefectTrackingReport(hidParam,templetFullFileName);
            //ICell cell = sheet.CreateRow(4).CreateCell(0);
            //cell.SetCellValue((string)this.hidParam1.Value);
            //cell = sheet.CreateRow(4).CreateCell(2);
            //cell.SetCellValue((string)this.hidParam2.Value);
            //cell = sheet.CreateRow(4).CreateCell(3);
            //cell.SetCellValue((string)this.hidParam3.Value);
            //cell = sheet.CreateRow(6).CreateCell(0);
            //cell.SetCellValue((string)this.hidParam4.Value);
            //cell = sheet.CreateRow(6).CreateCell(2);
            //cell.SetCellValue((string)this.hidParam5.Value);
            //cell = sheet.CreateRow(6).CreateCell(3);
            //cell.SetCellValue((string)this.hidParam6.Value);
            //cell = sheet.CreateRow(8).CreateCell(0);
            //cell.SetCellValue((string)this.hidParam7.Value);



            string fileExport = "FA_" + this.hidPdLine.Value.Trim() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            //workbook.Write(ms);
            curContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileExport + ".xls"));
            curContext.Response.BinaryWrite(ms.ToArray());

            //workbook = null;
            ms.Close();
            ms.Dispose();
            ////创建一个Application对象并使其可见
            ////beforeTime = DateTime.Now;
            //app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            //app.Visible = false;
            //app.DisplayAlerts = false;
            //app.AlertBeforeOverwriting = false;
            ////afterTime = DateTime.Now;

            ////打开模板文件，得到WorkBook对象
            //workBook = app.Workbooks.Open(templetFullFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            //                                                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            ////得到WorkSheet对象
            //workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets.get_Item(1);

            ////复制1个WorkSheet对象
            ////((Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.get_Item(1)).Copy(Type.Missing, workBook.Worksheets[1]);

            ////获取要写入数据的WorkSheet对象，并重命名
            //Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.get_Item(1);
            ///* 2012-7-30，Jessica Liu， 需求变更：excel模板修改
            //sheet.Name = (string)this.hidParam0.Value.Replace("-", "").Replace(":", "").Replace(" ", "");

            ////将数据写入WorkSheet
            //sheet.Cells[2, 1] = (string)this.hidParam1.Value;
            //sheet.Cells[2, 2] = (string)this.hidParam2.Value;
            //sheet.Cells[2, 3] = (string)this.hidParam3.Value;
            //sheet.Cells[4, 1] = (string)this.hidParam4.Value;
            //sheet.Cells[4, 2] = (string)this.hidParam5.Value;
            //sheet.Cells[4, 3] = (string)this.hidParam6.Value;
            //sheet.Cells[6, 1] = (string)this.hidParam7.Value;
            //*/
            ////将数据写入WorkSheet
            //sheet.Cells[5, 1] = (string)this.hidParam1.Value;
            //sheet.Cells[5, 3] = (string)this.hidParam2.Value;
            //sheet.Cells[5, 4] = (string)this.hidParam3.Value;
            //sheet.Cells[7, 1] = (string)this.hidParam4.Value;
            //sheet.Cells[7, 3] = (string)this.hidParam5.Value;
            //sheet.Cells[7, 4] = (string)this.hidParam6.Value;
            //sheet.Cells[9, 1] = (string)this.hidParam7.Value;

            ///*
            ////写文本框数据
            //Microsoft.Office.Interop.Excel.TextBox txtAuthor = (Microsoft.Office.Interop.Excel.TextBox)sheet.TextBoxes("txtAuthor");
            //Microsoft.Office.Interop.Excel.TextBox txtDate = (Microsoft.Office.Interop.Excel.TextBox)sheet.TextBoxes("txtDate");
            //Microsoft.Office.Interop.Excel.TextBox txtVersion = (Microsoft.Office.Interop.Excel.TextBox)sheet.TextBoxes("txtVersion");

            //txtAuthor.Text = "ITC";
            //txtDate.Text = DateTime.Now.ToShortDateString();
            //txtVersion.Text = "1.0.0.0";
            //*/

            ////输出Excel文件并退出
            //workBook.SaveAs(outputFullFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);


            //IntPtr t = new IntPtr(app.Hwnd);
            //int processId = 0;
            //GetWindowThreadProcessId(t, out processId);
            //hProcess = System.Diagnostics.Process.GetProcessById(processId);

            //workBook.Close(null, null, null);
            //app.Workbooks.Close();
            //app.Application.Quit();
            //app.Quit();

            //System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

            //workSheet = null;
            //workBook = null;
            //app = null;

            //GC.Collect();

        }
        catch (FisException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //finally
        //{
        //    /*
        //    Process[] myProcesses;
        //    DateTime startTime;
        //    myProcesses = Process.GetProcessesByName("Excel");

        //    //得不到Excel进程ID，暂时只能判断进程启动时间
        //    foreach (Process myProcess in myProcesses)
        //    {
        //        startTime = myProcess.StartTime;

        //        if (startTime > beforeTime && startTime < afterTime)
        //        {
        //            myProcess.Kill();
        //        }
        //    }
        //    */

        //    if (app != null)
        //    {
        //        try
        //        {
        //            if (workBook != null)
        //            {
        //                workBook.Close(false, null, null);
        //            }

        //            app.Application.Quit();
        //            app.Quit();

        //            if (workSheet != null)
        //            {
        //                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
        //            }

        //            if (workBook != null)
        //            {
        //                System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
        //            }

        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        //        }
        //        catch
        //        {
        //            //ignore
        //        }

        //        workSheet = null;
        //        workBook = null;
        //        app = null;
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

        //byte[] finalData = GetFileByteData(outputFullFileName);
        //try
        //{
        //    File.Delete(outputFullFileName);
        //}
        //catch (Exception ex)
        //{
        //    //ignore
        //}

        //HttpResponse response = System.Web.HttpContext.Current.Response;
        //string fileExport = "FA_" + this.hidPdLine.Value.Trim() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
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

    protected void hidbtn_ServerClick(Object sender, EventArgs e)
    {
        string ProdId = this.hidProdId.Value.Trim();
        string PdLine = this.hidPdLine.Value.Trim();
        ProductStatusInfo statusInfo;
        IList<RepairInfo> logList;
        ProductInfo info;
        string para = null;

        try
        {
            string editor = Master.userInfo.UserId;
            string station = Request["Station"];
            string customer = Master.userInfo.Customer;            
            bool isCustSN = false;
            info = iProduct.GetProductInfoByCustomSn(ProdId);

            if (info.id != null && !info.id.Trim().Equals(string.Empty))//customersn
            {
                statusInfo = iProduct.GetProductStatusInfo(info.id);
                CmbPdLine.InnerDropDownList.SelectedValue = statusInfo.pdLine;// Pdline;//Uupdate By Dean 20110314                
                CmbPdLine.setSelected(CmbPdLine.InnerDropDownList.SelectedIndex);  //Uupdate By Dean 20110314                
                iPAQCRepair.InputProdId(statusInfo.pdLine, info.id, editor, station, customer);
                
                //this.lblPdLineName.Text = statusInfo.pdLine;//Uupdate By Dean 2011034   

                //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                this.lblModelContent.Text = info.modelId;
                
                isCustSN = true;

                this.lblTestStationContent.Text = statusInfo.station + " " + iTestStation.GeStationDescr(statusInfo.station);
                //ITC-1360-0434, Jessica Liu, 2012-2-28
                logList = iPAQCRepair.GetOQCProdRepairList(info.id, 0, "PRD");//iRepairLog.GetProdRepairList(info.id);
                para = info.id;
            }
            else
            {  
                ProdId = ProdId.Length > 9 ? ProdId.Substring(0, 9) : ProdId; //productID
                statusInfo = iProduct.GetProductStatusInfo(ProdId);
                CmbPdLine.InnerDropDownList.SelectedValue = statusInfo.pdLine;//Uupdate By Dean 20110314
                CmbPdLine.setSelected(CmbPdLine.InnerDropDownList.SelectedIndex);  //Uupdate By Dean 20110314

                iPAQCRepair.InputProdId(statusInfo.pdLine, ProdId, editor, station, customer);
                
                //this.lblPdLineName.Text = statusInfo.pdLine;//Uupdate By Dean 20110314       

                //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                this.lblModelContent.Text = info.modelId;
                
                this.lblTestStationContent.Text = statusInfo.station + " " + iTestStation.GeStationDescr(statusInfo.station);
                //ITC-1360-0434, Jessica Liu, 2012-2-28
                logList = iPAQCRepair.GetOQCProdRepairList(ProdId, 0, "PRD");//iRepairLog.GetProdRepairList(ProdId);
                para = ProdId;
            }

            if (logList == null || logList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);

                //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                refreshCmbReturnStation(null, "0");
            }
            else
            {
                int ret = bindTable(logList, DEFAULT_ROWS);

                //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                if (ret == 1)
                {
                    refreshCmbReturnStation(ProdId, "0");
                }
            }

            hideWait(para);
            disableControl(isCustSN ? info.customSN : ProdId);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
            clearLabel();
            this.hidRecordCount.Value = "0";
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            refreshCmbReturnStation(null, "0");
            hideWait(null);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
            clearLabel();
            this.hidRecordCount.Value = "0";
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            refreshCmbReturnStation(null, "0");
            hideWait(null);
        }
    }

    protected void hidRefresh_ServerClick(Object sender, EventArgs e)
    {
        IList<RepairInfo> logList;
        string ProdId = this.hidProdId.Value.Trim();
        ProductInfo info;

        try
        {
            info = iProduct.GetProductInfoByCustomSn(ProdId);

            if (info.id != null && !info.id.Trim().Equals(string.Empty))
            {
                //ITC-1360-0434, Jessica Liu, 2012-2-28
                logList = iPAQCRepair.GetOQCProdRepairList(info.id, 0, "PRD");//iRepairLog.GetProdRepairList(info.id);
            }
            else
            {
                //ITC-1360-0434, Jessica Liu, 2012-2-28
                logList = iPAQCRepair.GetOQCProdRepairList(ProdId, 0, "PRD");//iRepairLog.GetProdRepairList(ProdId);
            }

            if (logList == null || logList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
                //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                refreshCmbReturnStation(null, "0");
            }
            else
            {
                int ret = bindTable(logList, DEFAULT_ROWS);
                //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
                if (ret == 1)
                {
                    refreshCmbReturnStation(ProdId, "0");
                }
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
            this.hidRecordCount.Value = "0";
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            refreshCmbReturnStation(null, "0");
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
            this.hidRecordCount.Value = "0";
            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            refreshCmbReturnStation(null, "0");
        }
    }

    private int bindTable(IList<RepairInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        int ret = 0;

        dt.Columns.Add(" ");
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPdLine").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colTestStn").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDefect").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCause").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCreateDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUpdateDate").ToString());
        dt.Columns.Add("hideCol");

        if (list != null && list.Count != 0)
        {
            foreach (RepairInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.isManual;
                //dr[1] = temp.pdLine;
                //dr[2] = temp.testStation;
                dr[1] = temp.defectCodeID + " " + temp.defectCodeDesc;
                dr[2] = temp.cause + " " + temp.causeDesc;
                dr[3] = temp.cdt;
                dr[4] = temp.udt;
                dr[5] = getHideColumn(temp);

                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Count.ToString();

            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            ret = 1;
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }

            this.hidRecordCount.Value = "";

            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            this.CmbReturnStation.clearContent();

            //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
            ret = 0;
        }

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

        //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
        return ret;
    }

    private string getHideColumn(RepairInfo temp)
    {
        StringBuilder builder = new StringBuilder();
        string seperator = "\u0003";

        builder.Append(temp.id);
        builder.Append(seperator);
        builder.Append(temp.repairID);
        builder.Append(seperator);
        builder.Append(temp.type);
        builder.Append(seperator);
        builder.Append(temp.obligation);
        builder.Append(seperator);
        builder.Append(temp.component);
        builder.Append(seperator);
        builder.Append(temp.site);
        builder.Append(seperator);
        builder.Append(temp.majorPart);
        builder.Append(seperator);
        builder.Append(temp.remark);
        builder.Append(seperator);
        builder.Append(temp.vendorCT);
        builder.Append(seperator);
        builder.Append(temp.partType);
        builder.Append(seperator);
        builder.Append(temp.oldPart);
        builder.Append(seperator);
        builder.Append(temp.oldPartSno);
        builder.Append(seperator);
        builder.Append(temp.newPart);
        builder.Append(seperator);
        builder.Append(temp.newPartSno);
        builder.Append(seperator);
        builder.Append(temp.manufacture);
        builder.Append(seperator);
        builder.Append(temp.versionA);
        builder.Append(seperator);
        builder.Append(temp.versionB);
        builder.Append(seperator);
        builder.Append(temp.returnSign);
        builder.Append(seperator);
        builder.Append(temp.mark);
        builder.Append(seperator);
        builder.Append(temp.subDefect);
        builder.Append(seperator);
        builder.Append(temp.piaStation);
        builder.Append(seperator);
        builder.Append(temp.distribution);
        builder.Append(seperator);
        builder.Append(temp._4M);
        builder.Append(seperator);
        builder.Append(temp.responsibility);
        builder.Append(seperator);
        builder.Append(temp.action);
        builder.Append(seperator);
        builder.Append(temp.cover);
        builder.Append(seperator);
        builder.Append(temp.uncover);
        builder.Append(seperator);
        builder.Append(temp.trackingStatus);
        builder.Append(seperator);
        builder.Append(temp.isManual);
        builder.Append(seperator);
        builder.Append(temp.editor);
        builder.Append(seperator);
        builder.Append(temp.newPartDateCode);
        builder.Append(seperator);
        builder.Append(temp.defectCodeID);
        builder.Append(seperator);
        builder.Append(temp.cause);

        //2012-5-4
        builder.Append(seperator);
        builder.Append(temp.location);
        builder.Append(seperator);
        builder.Append(temp.mtaID);

        return builder.ToString();
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[5].Style.Add("display", "none");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Font.Size = FontUnit.Point(16);
            e.Row.Cells[5].Text = e.Row.Cells[5].Text.Replace(Environment.NewLine, "<br>");
            if (!e.Row.Cells[0].Text.Equals("&nbsp;"))
            {
                if (e.Row.Cells[0].Text.Equals("0"))
                {
                    e.Row.Cells[0].Text = "×";
                }
                else
                {
                    e.Row.Cells[0].Text = "√";
                }
            }

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("getAvailableData(\"processFun\"); inputFlag = false;");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void hideWait(string productId)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");

        if (!string.IsNullOrEmpty(productId))
        {
            scriptBuilder.AppendLine("globalProID='" + productId + "';");
        }

        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private void disableControl(string para)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("disableControls('" + para + "');");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "disableControls", scriptBuilder.ToString(), false);
    }

    private void clearLabel()
    {
        this.lblTestStationContent.Text = string.Empty;
    }

    //2012-8-1, Jessica Liu, 需求变更：增加Return Station 计算逻辑, 修改解DN条件
    private void refreshCmbReturnStation(string prodid, string status)
    {
        if (!string.IsNullOrEmpty(prodid))
        {
            IList<StationInfo> returnList = null;
            returnList = iPAQCRepair.GetReturnStationList(prodid, Convert.ToInt32(status));
            if (returnList != null && returnList.Count > 0)
            {
                this.CmbReturnStation.clearContent();
                foreach (StationInfo temp in returnList)
                {
                    ListItem item = null;
                    item = new ListItem(temp.Descr, temp.StationId);
                    this.CmbReturnStation.InnerDropDownList.Items.Add(item);
                }

                //存在且只存在一条非空记录，则自动选择该记录
                if (returnList.Count == 1)
                {
                    this.CmbReturnStation.setSelected(1);
                }
            }
            else
            {
                this.CmbReturnStation.clearContent();
            }
        }
        else
        {
            this.CmbReturnStation.clearContent();
        }
    }
}
