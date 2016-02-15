using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

//qa bug no:ITC-1136-0046,ITC-1136-0047,ITC-1136-0168
//ITC-1361-0066 ITC-1361-0068



public partial class Process : IMESBasePage
{
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int DEFAULT_ROWS = 15;
    public int DEFAULT_ROWS2 = 26;
    IProcessManager iProcessManager = ServiceAgent.getInstance().GetMaintainObjectByName<IProcessManager>(WebConstant.IProcessManager);
    public string pcbType="";
    public string productType = "";
    public string MaterialType = "";

    public int ExcelUploadStationColumnCount = 6;

    private const int COL_NUM1 = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            pcbType = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeValue1");
            productType = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeValue2");
            MaterialType = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeValue3");
            if (!this.IsPostBack)
            {
                InitLabel();
                ShowProcessList();
                bindProcessStationListTable(null);
                InitStationStatusSelect();
                string editor = Master.userInfo.UserId;
                this.HiddenUserName.Value = editor;
            }
         
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    private void setColumnWidth()
    {

        gdProcess.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gdProcess.HeaderRow.Cells[1].Width = Unit.Percentage(20);
        gdProcess.HeaderRow.Cells[2].Width = Unit.Percentage(24);
        gdProcess.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gdProcess.HeaderRow.Cells[4].Width = Unit.Percentage(18);
        gdProcess.HeaderRow.Cells[5].Width = Unit.Percentage(18);
       

    }

    protected void btnSaveAsRefresh_ServerClick(Object sender, EventArgs e)
    {
        RefreshListAndSetHighLight();
    }

    private void RefreshListAndSetHighLight()
    {        
        string strProcess = this.importProcess.Value.Trim();
        strProcess = replaceSpecialChart(strProcess);
        ShowProcessList();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "AddProcessComplete", "DealHideWait();AddProcessComplete(\"" + strProcess + "\");", true);

    }
    
    protected void btnImport_Click(Object sender, EventArgs e)
    {
        RefreshListAndSetHighLight();
    }

    protected void btnExport_Click(Object sender, EventArgs e)
    {
        //string process = this.hidProcess.Value.Trim();

        //System.Diagnostics.Process hProcess = null;
        //ProcessInfoDef processInfo = new ProcessInfoDef();
        //string fullFileName = "";

        //string filePath = HttpContext.Current.Server.MapPath("~");
        //string path = filePath + "\\tmp";
        //Guid guid = System.Guid.NewGuid();
        //string extName = ".xls";
        //fullFileName = path + "\\" + guid.ToString() + extName;

        //try
        //{
        //    processInfo = iProcessManager.ExportProcess(process);
        //}
        //catch (FisException ex)
        //{
        //    showErrorMessage2(ex.mErrmsg,process);
        //    return;
        //}
        //catch (Exception ex)
        //{
        //    //show error
        //    showErrorMessage2(ex.Message, process);
        //    return;
        //}

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

        //    int totalRowCount = processInfo.ProcessStationList.Count;
        //    string excelRangeAll = string.Format("A1:{0}{1}", "F", totalRowCount + 9 - 1);
        //    Microsoft.Office.Interop.Excel.Range CurRange = worksheet.get_Range(excelRangeAll, Type.Missing);
        //    CurRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;// .xlVAlignCenter;//居中对齐
        //    CurRange.NumberFormatLocal = "@";

        //    worksheet.Cells[1, 1] = "Process Name";
        //    worksheet.Cells[2, 1] = "Process Type";
        //    worksheet.Cells[3, 1] = "Process Descr";
        //    worksheet.Cells[4, 1] = "Editor";
        //    worksheet.Cells[5, 1] = "Cdt";
        //    worksheet.Cells[6, 1] = "Udt";

        //    worksheet.Cells[1, 2] = Null2String(processInfo.ProcessInfo.Process);
        //    worksheet.Cells[2, 2] = Null2String(processInfo.ProcessInfo.Type);
        //    worksheet.Cells[3, 2] = Null2String(processInfo.ProcessInfo.Description);
        //    worksheet.Cells[4, 2] = Null2String(processInfo.ProcessInfo.Editor);
        //    worksheet.Cells[5, 2] = ((System.DateTime)processInfo.ProcessInfo.Cdt).ToString("yyyy-MM-dd");
        //    worksheet.Cells[6, 2] = ((System.DateTime)processInfo.ProcessInfo.Udt).ToString("yyyy-MM-dd");

        //    worksheet.Cells[7, 1] = "Station List";

        //    worksheet.Cells[8, 1] = "Prev Station";
        //    worksheet.Cells[8, 2] = "Status";
        //    worksheet.Cells[8, 3] = "Station";
        //    worksheet.Cells[8, 4] = "Editor";
        //    worksheet.Cells[8, 5] = "Cdt";
        //    worksheet.Cells[8, 6] = "Udt";

        //    if (totalRowCount > 0)
        //    {
        //        object[,] rawData = new object[totalRowCount, ExcelUploadStationColumnCount];

        //        // Copy the values to the object array
        //        for (int row = 0; row < totalRowCount; row++)
        //        {
        //            rawData[row, 0] = Null2String(processInfo.ProcessStationList[row].PreStation);

        //            string status = "";
        //            if (processInfo.ProcessStationList[row].Status == 0)
        //            {
        //                status = "FAIL";
        //            }
        //            else if (processInfo.ProcessStationList[row].Status == 1)
        //            {
        //                status = "PASS";
        //            }
        //            else if (processInfo.ProcessStationList[row].Status == 2)
        //            {
        //                status = "PROCESSING";
        //            }

        //            rawData[row, 1] = status;// Null2String(processInfo.ProcessStationList[row].Status);
        //            rawData[row, 2] = Null2String(processInfo.ProcessStationList[row].Station);
        //            rawData[row, 3] = Null2String(processInfo.ProcessStationList[row].Editor);
        //            rawData[row, 4] = ((System.DateTime)processInfo.ProcessStationList[row].Cdt).ToString("yyyy-MM-dd");
        //            rawData[row, 5] = ((System.DateTime)processInfo.ProcessStationList[row].Udt).ToString("yyyy-MM-dd");

        //        }
        //        // Fast data export to Excel
        //        string excelRange = string.Format("A9:{0}{1}",
        //            "F", totalRowCount + 9 - 1);

        //        worksheet.get_Range(excelRange, Type.Missing).Value2 = rawData;
        //    }

        //    CurRange.ColumnWidth = 20;
        //    //CurRange.EntireColumn.AutoFit();
        //    //CurRange.RowHeight = 19;//.EntireRow.AutoFit();//
        //    CurRange.EntireRow.AutoFit();


        //    m_book.SaveAs(fullFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

        //    //m_book.SaveAs(fullFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    //m_book.SaveAs(fullFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    //mApp.ActiveWorkbook.SaveCopyAs(fullFileName);
        //    IntPtr t = new IntPtr(mApp.Hwnd);
        //    int processId = 0;
        //    GetWindowThreadProcessId(t, out processId);
        //    hProcess = System.Diagnostics.Process.GetProcessById(processId);

        //}
        //catch (FisException ex)
        //{
        //    showErrorMessage2(ex.mErrmsg, process);
        //    return;
        //}
        //catch (Exception ex)
        //{
        //    //show error
        //    showErrorMessage2(ex.Message, process);
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

        //response.Clear();
        //response.ClearHeaders();
        //response.ContentType = "application/octet-stream";
        ////Response.AddHeader("content-type", "application/x-msdownload");
        ////Response.ContentType = "application/ms-excel";
        //response.AddHeader("Content-Disposition", "attachment; filename=" + process + ".xls");
        //response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");

        //response.OutputStream.Write(finalData, 0, finalData.Length);
        //response.Flush();
        //response.Close();
        //FileDownload(fullFileName);


    }


    private void showErrorMessage2(string errorMsg, string process)
    {
        string strProcess = replaceSpecialChart(process);

        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        //scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();ExportHighLight(\"" + strProcess + "\");");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void FileDownload(string FullFileName)
    {

        //byte[] finalData = GetFileByteData(FullFileName);
        //HttpResponse response = System.Web.HttpContext.Current.Response;
        //response.Clear();
        //response.ContentType = "application/octet-stream";
        //FileInfo DownloadFile = new FileInfo(FullFileName);
        //string name = DownloadFile.Name;
        //response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(name));
        //response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        //response.OutputStream.Write(finalData, 0, finalData.Length);
        //response.Flush();
        //response.Close();

        FileInfo DownloadFile = new FileInfo(FullFileName);
        HttpResponse response = System.Web.HttpContext.Current.Response;
        Response.Clear();
        //Response.ClearHeaders();
        //Response.Buffer = false;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AppendHeader("Content-Disposition ", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.Name));
        Response.AppendHeader("Content-Length ", DownloadFile.Length.ToString());
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        Response.WriteFile(DownloadFile.FullName);
        Response.Flush();
        Response.Close();

    }


    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
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

   //ok
    protected void btnDeleteProcess_Click(Object sender, EventArgs e)
    {
        string strProcessName = hidProcess.Value.Trim();

        try
        {
            iProcessManager.deleteProcess(strProcessName);

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }

        ShowProcessList();
        this.updatePanel1.Update();

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "AddProcessComplete", "DealHideWait();AddProcessComplete(\"" + "" + "\");", true);

    }
    //ok
    protected void btnAddProcess_Click(Object sender, EventArgs e)
    {
        string strProcess = txtProcess.Text;
        string strOldProcess = hidProcess.Value.Trim();
        try
        {
            ProcessMaintainInfo tmpProcessInfo = new ProcessMaintainInfo();

            tmpProcessInfo.Type = this.cmbMaintainProcessType.InnerDropDownList.SelectedValue.Trim();

            tmpProcessInfo.Process = strProcess;
            tmpProcessInfo.Description = txtDescription.Text;
            tmpProcessInfo.Editor = this.HiddenUserName.Value;

            if (strProcess == strOldProcess)
            {
                iProcessManager.saveProcess(strOldProcess, tmpProcessInfo);
            }
            else
            {

                iProcessManager.addProcess(tmpProcessInfo);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }

        ShowProcessList();
        this.updatePanel1.Update();
        strProcess = replaceSpecialChart(strProcess);

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "AddProcessComplete", "DealHideWait();AddProcessComplete(\"" + strProcess + "\");", true);

    }
    //ok
    private Boolean ShowProcessStationList()
    {
        string strProcess = hidProcess.Value.Trim();
        IList<ProcessStationMaintainInfo> processStationlist = iProcessManager.getProcessStationList(strProcess);

        try
        {
            bindProcessStationListTable(processStationlist);
        }
        catch (FisException ex)
        {
            bindProcessStationListTable(null);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindProcessStationListTable(null);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }

    //ok
    protected void btnRefreshProcessStationList_Click(Object sender, EventArgs e)
    {
        if (ShowProcessStationList() == false)
        {
            return;
        }
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "btnRefreshProcessStationList_Complete", "ShowRowEditInfoProcessStation(null);DealHideWait();", true);

    }

    //ok
    protected void btnSaveStation_Click(Object sender, EventArgs e)
    {
        int processStationID;
        string strProcessStationID;
        try
        {
            ProcessStationMaintainInfo tmpProcessStationInfo = new ProcessStationMaintainInfo();

            //如果有processStationId，并且station, preStation下拉框内容没变， 表示将来需要通过processStationId保存
            if ((hidProcessStationID.Value.Length != 0)
                && (hidStation.Value == selStation.InnerDropDownList.SelectedValue)
                && (hidPreStation.Value == selPreStation.InnerDropDownList.SelectedValue))
            {
                strProcessStationID = hidProcessStationID.Value;
            }
            //否则，做新增
            else 
            {
                strProcessStationID = "";
            }

            if (strProcessStationID.Length != 0)
            {
                tmpProcessStationInfo.Id = Int32.Parse(strProcessStationID);

            }

            
            tmpProcessStationInfo.Process = hidProcess.Value.Trim();
            tmpProcessStationInfo.Station = selStation.InnerDropDownList.SelectedValue;
            tmpProcessStationInfo.PreStation = selPreStation.InnerDropDownList.SelectedValue;
            

            if (selStationStatus.Text == "PASS")
            {
                tmpProcessStationInfo.Status = 1;
            }
            else if (selStationStatus.Text == "FAIL")//"FAIL"
            {
                tmpProcessStationInfo.Status = 0;
            }
            else//PROCESSING
            {
                tmpProcessStationInfo.Status = 2;
            }
            tmpProcessStationInfo.Editor = this.HiddenUserName.Value;

            processStationID = iProcessManager.saveProcessStation(tmpProcessStationInfo);
           
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        ShowProcessStationList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "addSaveStationComplete", "AddUpdateStationComplete(\"" + processStationID.ToString() + "\");DealHideWait();", true);

    }

    //ok
    protected void btnDeleteStation_Click(Object sender, EventArgs e)
    {
        try
        {
            string tmpId = hidProcessStationID.Value;
            iProcessManager.deleteProcessStation(Int32.Parse(tmpId));
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        ShowProcessStationList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "deleteStationComplete", "ShowRowEditInfoProcessStation(null);DealHideWait();", true);

    }

    private Boolean ShowProcessList()
    {
        IList<ProcessMaintainInfo> processlist = iProcessManager.getProcessList();
        try
        {
            bindProcessTable(processlist);
        }
        catch (FisException ex)
        {
            bindProcessTable(null);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindProcessTable(null);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;

    }

    protected void bindProcessTable(IList<ProcessMaintainInfo> processlist)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colProcess").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

        if (processlist != null && processlist.Count != 0)
        {
            foreach (ProcessMaintainInfo temp in processlist)
            {
                dr = dt.NewRow();
                dr[0] = temp.Type;
                dr[1] = temp.Process;
                dr[2] = temp.Description;
                dr[3] = temp.Editor;
                if (temp.Cdt == DateTime.MinValue)
                {
                    dr[4] = "";
                }
                else
                {
                    dr[4] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (temp.Udt == DateTime.MinValue)
                {
                    dr[5] = "";
                }
                else
                {
                    dr[5] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                dt.Rows.Add(dr);
            }

            for (int i = processlist.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }
        }

        gdProcess.DataSource = dt;
        gdProcess.DataBind();
        setColumnWidth();

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndexProcess", "selectedRowIndex_Process = null;", true);

    }


    protected void bindProcessStationListTable(IList<ProcessStationMaintainInfo> processStationlist)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;


        dt.Columns.Add("id");//hidden
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPreStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colStatus").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

        if (processStationlist != null && processStationlist.Count != 0)
        {
            foreach (ProcessStationMaintainInfo temp in processStationlist)
            {
                dr = dt.NewRow();

                dr[0] = temp.Id;
                dr[3] = temp.Station;
                dr[1] = temp.PreStation;
                if (temp.Status == 1)
                {
                    dr[2] = "PASS";
                }
                else if (temp.Status == 0)
                {
                    dr[2] = "FAIL";
                }
                else
                {//2
                    dr[2] = "PROCESSING";
                }
                dr[4] = temp.Editor;
                if (temp.Cdt == DateTime.MinValue)
                {
                    dr[5] = "";
                }
                else
                {
                    dr[5] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (temp.Udt == DateTime.MinValue)
                {
                    dr[6] = "";
                }
                else
                {
                    dr[6] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                dt.Rows.Add(dr);
            }

            for (int i = processStationlist.Count; i < DEFAULT_ROWS2; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < DEFAULT_ROWS2; i++)
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }
        }

        gdProcessStation.GvExtHeight = dTableHeight.Value;
        gdProcessStation.DataSource = dt;
        gdProcessStation.DataBind();

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();selectedRowIndex_ProcessStation = null;", true);
        //ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "ProcessSelectedComplete", "ProcessSelectedComplete();", true);
    }



    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblProcessList.Text = this.GetLocalResourceObject(Pre + "_lblProcessList").ToString();
        this.lblStationList.Text = this.GetLocalResourceObject(Pre + "_lblStationList").ToString();
        this.lblProcess.Text = this.GetLocalResourceObject(Pre + "_lblProcess").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblPreStation.Text = this.GetLocalResourceObject(Pre + "_lblPreStation").ToString();
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatus").ToString();
        this.lblDescription.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();

        this.lblProcessType.Text = this.GetLocalResourceObject(Pre + "_lblProcessType").ToString();  

        this.btnDelete1.Value = this.GetLocalResourceObject(Pre + "_btnDeleteProcess").ToString();
        this.btnAdd1.Value = this.GetLocalResourceObject(Pre + "_btnAddProcess").ToString();
        this.btnDelete2.Value = this.GetLocalResourceObject(Pre + "_btnDeleteStation").ToString();
        this.btnSave2.Value = this.GetLocalResourceObject(Pre + "_btnSaveStation").ToString();

        this.btnImport.Value = this.GetLocalResourceObject(Pre + "_btnImport").ToString();
        this.btnExport.Value = this.GetLocalResourceObject(Pre + "_btnExport").ToString();
        this.btnProcessRule.Value = this.GetLocalResourceObject(Pre + "_btnProcessRule").ToString();
        this.btnProcessRuleSet.Value = this.GetLocalResourceObject(Pre + "_btnProcessRuleSet").ToString();

        this.btnSaveAs.Value = this.GetLocalResourceObject(Pre + "_btnSaveAs").ToString();

    }

    protected void InitStationStatusSelect()
    {
        try
        {
            //string value1 = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbStationStatusItemValue1").ToString();
            //string value2 = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbStationStatusItemValue2").ToString();
            string value1 = this.GetLocalResourceObject(Pre + "_CmbStationStatusItemValue1").ToString();
            string value2 = this.GetLocalResourceObject(Pre + "_CmbStationStatusItemValue2").ToString();
            string value3 = this.GetLocalResourceObject(Pre + "_CmbStationStatusItemValue3").ToString();

            selStationStatus.Items.Add(new ListItem(value2, value2));//fail==0
            selStationStatus.Items.Add(new ListItem(value1, value1));//pass==1
            selStationStatus.Items.Add(new ListItem(value3, value3));//processing==2
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }

    }

    protected void gdProcess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = COL_NUM1; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM1; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }

    protected void gdProcessStation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Style.Add("display", "none");//id


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                    {
                        e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                    }
                }

            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }


}
