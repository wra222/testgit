using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.IO;
using com.inventec.fisreport.system.util;
using System.Net.Mail;
using System.Web.Configuration;
using com.inventec.system.util;
using System.Web;
using System.Reflection;
using System.Globalization;
using System.Runtime.InteropServices;
//ITC-1281-0006  2011,5,16 Adopted Suggestions

public partial class DataMaintain_ForwarderUploadFile : System.Web.UI.Page
{

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IForwarder iForwarder;
    public String userName;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    private const int EXCEL_DATA_START_ROW = 2;
 
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            iForwarder = (IForwarder)ServiceAgent.getInstance().GetMaintainObjectByName<IForwarder>(WebConstant.MaintainForwarderObject);
            if (!this.IsPostBack)
            {

                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                HttpRuntimeSection section = (HttpRuntimeSection)config.GetSection("system.web/httpRuntime");
                double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);
                this.hidFileMaxSize.Value = maxFileSize.ToString();

                this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
                initLabel();
                userName = Request.QueryString["userName"];
                userName = StringUtil.decode_URL(userName);
                this.HiddenUserName.Value = userName;
            }
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            this.hidMsg1.Value = pmtMessage1;
 
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    protected void btnOK_ServerClick(Object sender, EventArgs e)
    {
        //Microsoft.Office.Interop.Excel.Application mApp = null;
        //Microsoft.Office.Interop.Excel.Workbook m_book = null;
        //Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
        string fullFileName = "";
        //System.Diagnostics.Process process = null;
        try
        {
            DataTable dt = new DataTable();
            if (dFileUpload.FileName.Trim() == "")
            {
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                showErrorMessage(pmtMessage4);
                //showErrorMessage("需要选择上传的文件.");
                return;
            }
            if (dFileUpload.HasFile)
            {
                string fileName = dFileUpload.FileName;
                string filePath = HttpContext.Current.Server.MapPath("~");
                string path = filePath + "\\tmp";
                //根下tmp目录存上传临时文件
                MakeDirIfNotExist(path);
                try
                {
                    ////////////////////
                    string extName = fileName.Substring(fileName.LastIndexOf("."));
                    //if (extName != ".xls" && extName != ".xlsx" && extName != ".xl" && extName != ".xla" && extName != ".xlt" && extName != ".xlm" && extName != ".xlc" && extName != ".xlw"
                    //   && extName != ".xlsm" && extName != ".xltx" && extName != ".xltm" && extName != ".xlsb" && extName != ".xlam")
                    //{
                    //    pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                    //    showErrorMessage(pmtMessage2);
                    //    //showErrorMessage("上传的文件格式不正确.");
                    //    return;
                    //}
                    if (extName != ".xls")
                    //if (extName != ".xls" && extName != ".xlsx" && extName != ".xlsm")
                    {
                        pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                        showErrorMessage(pmtMessage2);
                        //showErrorMessage("上传的文件格式不正确.");
                        return;
                    }
                    Guid guid = System.Guid.NewGuid();
                    fullFileName = path + "\\" + guid.ToString() + extName;
                    dFileUpload.PostedFile.SaveAs(fullFileName);
                    FileStream file = null;
                    file = new FileStream(fullFileName, FileMode.Open);

                    //dt = ExcelManager.getExcelSheetData(dFileUpload.PostedFile.FileName);
                    dt = ExcelManager.ExportDataTableFromExcel2(file, 1, 0, fullFileName);
                    //dt = ExcelManager.RenderDataTableFromExcel(file, 1, 0);

                    //Guid guid = System.Guid.NewGuid();
                    //fullFileName = path + "\\" + guid.ToString() + extName;
                    //dFileUpload.PostedFile.SaveAs(fullFileName);

                    //mApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                    //mApp.Visible = false;
                    //m_book = mApp.Workbooks.Open(fullFileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    //worksheet = ((Microsoft.Office.Interop.Excel.Worksheet)mApp.Sheets[1]);
                    ////////////////////

                    //IntPtr t = new IntPtr(mApp.Hwnd);
                    //int processId = 0;
                    //GetWindowThreadProcessId(t, out processId);
                    //process = System.Diagnostics.Process.GetProcessById(processId);
                }
                catch
                {
                    pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                    showErrorMessage(pmtMessage2);
                    //showErrorMessage("上传的文件格式不正确.");
                    return;
                }

                int startRow = EXCEL_DATA_START_ROW;
                int endRow = dt.Rows.Count + startRow;
                if (endRow < startRow)
                {

                    pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                    showErrorMessage(pmtMessage3);
                    //showErrorMessage("没有需要上传的数据.");
                    return;
                }

                //string excelRange = string.Format("A{0}:{1}{2}",EXCEL_DATA_START_ROW.ToString(), "E", endRow);

                //object[,] getdata = (object[,])worksheet.get_Range(excelRange, Type.Missing).Value2;

                List<ForwarderInfo> forwarderList = new List<ForwarderInfo>();

                int iRowsCountNPOI = dt.Rows.Count;
                for (int iRow = 1; iRow < iRowsCountNPOI; iRow++)
                {
                    int lineNum = iRow + EXCEL_DATA_START_ROW - 1;

                    ForwarderInfo item = new ForwarderInfo();

                    string Value1NPOI = dt.Rows[iRow][0].ToString();
                    string Value2NPOI = dt.Rows[iRow][1].ToString();
                    string Value3NPOI = dt.Rows[iRow][2].ToString();
                    string Value4NPOI = dt.Rows[iRow][3].ToString();
                    string Value5NPOI = dt.Rows[iRow][4].ToString();
                    //string Value6NPOI = dt.Rows[iRow][5].ToString();

                    //object value1 = getdata[iRow, 1];
                    //string strValue1 = Null2String(value1);
                    //object value2 = getdata[iRow, 2];
                    //string strValue2 = Null2String(value2);
                    //object value3 = getdata[iRow, 3];
                    //string strValue3 = Null2String(value3);
                    //object value4 = getdata[iRow, 4];
                    //string strValue4 = Null2String(value4);
                    //object value5 = getdata[iRow, 5];
                    //string strValue5 = Null2String(value5);

                    if (Value1NPOI == "" && Value2NPOI == "" && Value3NPOI == "" && Value4NPOI == "" && Value5NPOI == "")
                    {
                        continue;
                    }

                    if (Value1NPOI == "" || Value2NPOI == "" || Value3NPOI == "" || Value4NPOI == "" || Value5NPOI == "" )
                    {
                        pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString() + " " + lineNum.ToString();
                        showErrorMessage(pmtMessage5);
                        return;
                    }


                    double days;
                    bool isSuccess = Double.TryParse(Value1NPOI, out days);


                    if (isSuccess)
                    {
                        //微软的bug, excel中直接拉，认为有1900-2-29，实际上没有，在那里差了一天
                        //Excel中1900-2-28取到的数是59， //1900-3-1取到的数是61，不连续
                        if (days <= 60)
                        {
                            days = days - 1;
                        }
                        else
                        {
                            days = days - 2;
                        }
                        DateTime curDate = DateTime.Parse("1900-01-01").AddDays(days);
                        Value1NPOI = curDate.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    }
                    else
                    {
                        try
                        {
                            DateTime curDate = DateTime.Parse(Value1NPOI);
                            Value1NPOI = curDate.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                        }
                        catch
                        {
                            //什么都不做，保持原来的数据形式
                            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString() + " " + lineNum.ToString();
                            showErrorMessage(pmtMessage5);
                            return;
                        }
                    }
                    
                    item.Date = Value1NPOI;
                    item.Forwarder = Value2NPOI.ToUpper();
                    item.MAWB = Value3NPOI;
                    item.Driver = Value4NPOI;
                    item.TruckID = Value5NPOI.ToUpper();
                    item.ContainerId = "";
                    item.Editor = this.HiddenUserName.Value;
                    item.Cdt = DateTime.Now;
                    item.Udt = DateTime.Now;
                    forwarderList.Add(item);

                }

                if (forwarderList.Count < 1)
                {
                    //showErrorMessage("没有需要上传的数据.");
                    pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                    showErrorMessage(pmtMessage3);
                    return;
                }

                this.hidIsSubmitOK.Value = "OK";
                iForwarder.ImportForwarder(forwarderList);

            }
            else
            {
                //pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                //showErrorMessage(pmtMessage4);
                ////showErrorMessage("需要选择上传的文件.");
                //return;
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                showErrorMessage(pmtMessage2);
                //showErrorMessage("上传的文件格式不正确.");
                return;
            }

        }
        //catch (SmtpException ex)
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
        finally
        {
            TryDeleteTempFile(fullFileName);
        }
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
        //    if (process != null)
        //    {
        //        try
        //        {
        //            process.Kill();
        //        }
        //        catch
        //        {
        //            // ignore
        //        }
        //    }
        //    // Collect the unreferenced objects
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //    TryDeleteTempFile(fullFileName);
        //}
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "OKComplete", "OKComplete();HideWait();", true);

    }

    private static void TryDeleteTempFile(string fullFileName)
    {
        try
        {
            File.Delete(fullFileName);
        }
        catch
        {
            //忽略
        }
    }

  
    private static void MakeDirIfNotExist(string path)
    {
        if (!Directory.Exists(path))
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch
            {
                //忽略
            }
        }
    }

    private void initLabel()
    {

        this.btnOK.Value = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();

    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }


    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }    
    
}
