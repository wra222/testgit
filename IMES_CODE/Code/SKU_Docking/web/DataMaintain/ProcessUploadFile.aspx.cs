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

public partial class DataMaintain_ProcessUploadFile : System.Web.UI.Page
{

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IProcessManager iProcessManager;
    public String userName;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    private const int EXCEL_DATA_START_ROW = 1;
    private const string EXCEL_LIST_TITLE_STRING = "Station List";
       
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iProcessManager = ServiceAgent.getInstance().GetMaintainObjectByName<IProcessManager>(WebConstant.IProcessManager);
                        
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
        string fullFileName = "";
        try
        {
            string fileName = dFileUpload.FileName;
            string filePath = HttpContext.Current.Server.MapPath("~");
            string path = filePath + "\\tmp";
            MakeDirIfNotExist(path);

            DataTable dt = new DataTable();
            if (dFileUpload.FileName.Trim() == "")
            {
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                showErrorMessage(pmtMessage4);
                return;
            }

            if (dFileUpload.HasFile)
            {
                try
                {
                    string extName = fileName.Substring(fileName.LastIndexOf("."));
                    //if (extName != ".xls" && extName != ".xlsx" && extName != ".xlsm")
                    //if (extName != ".xls")
                    //{
                    //    pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                    //    showErrorMessage(pmtMessage2);
                    //    return;
                    //}
                    Guid guid = System.Guid.NewGuid();
                    fullFileName = path + "\\" + guid.ToString() + extName;
                    dFileUpload.PostedFile.SaveAs(fullFileName);

                    dt = ExcelManager.getExcelSheetData(fullFileName);

                }
                catch (Exception ex)
                {

                    pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                    showErrorMessage(ex.Message);
                    return;
                }

                int startRow = EXCEL_DATA_START_ROW;
                int endRow = dt.Rows.Count + startRow;
                if (endRow < startRow)
                {
                    pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                    showErrorMessage(pmtMessage3);
                    return;
                }
                ProcessMaintainInfo processInfo = new ProcessMaintainInfo();
                List<ProcessStationMaintainInfo> processStationInfo = new List<ProcessStationMaintainInfo>();
                //下面中B1,B2,B3固定存上传的process信息，Editor,Cdt,Udt有可能没有，有了也不要从这取
                string tmpValueNPOI;
                tmpValueNPOI = dt.Rows[1][1].ToString();
                string processName = tmpValueNPOI;
                if (processName == "" || processName.Length > 10)
                {
                    pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString() + " " + 1.ToString();
                    showErrorMessage(pmtMessage5);
                    return;
                }
                string pcbType = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeValue1");
                string productType = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainProcessTypeValue2");
                //!!!目前只有2种类型的可以上传
                tmpValueNPOI = dt.Rows[2][1].ToString();
                string processType = tmpValueNPOI;
                if (processType == "" || (processType != pcbType && processType != productType))
                {
                    pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString() + " " + 2.ToString();
                    showErrorMessage(pmtMessage5);
                    return;
                }
                tmpValueNPOI = dt.Rows[3][1].ToString();
                string processDescr = tmpValueNPOI;
                if (processDescr == "" || processDescr.Length > 80)
                {
                    pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString() + " " + 3.ToString();
                    showErrorMessage(pmtMessage5);
                    return;
                }
                processInfo.Process = processName;
                processInfo.Type = processType;
                processInfo.Description = processDescr;
                processInfo.Editor = this.HiddenUserName.Value;
                int iRowsCountNPOI = dt.Rows.Count;
                int iColsCountNPOI = dt.Columns.Count;
                //从A1到A7搜寻Station List，这单元必有，找到后这行的下面一行就是数据标题，再下面一行是数据
                int listStartRow = 0;
                for (int iRow = 1; iRow <= 8; iRow++)
                {
                    string ValueNPOI = dt.Rows[iRow][0].ToString();
                    if (ValueNPOI == EXCEL_LIST_TITLE_STRING)
                    {
                        listStartRow = iRow;
                        break;
                    }
                }

                if (listStartRow == 0)
                {
                    string pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
                    showErrorMessage(pmtMessage6);
                    return;
                }

                int stationStartRow = listStartRow + 2;

                for (int iRow = stationStartRow; iRow < iRowsCountNPOI; iRow++)
                {
                    int lineNum = iRow + EXCEL_DATA_START_ROW - 1;

                    ProcessStationMaintainInfo item = new ProcessStationMaintainInfo();
                    string Value1NPOI = dt.Rows[iRow][2].ToString();
                    string Value2NPOI = dt.Rows[iRow][0].ToString();
                    string Value3NPOI = dt.Rows[iRow][1].ToString().ToUpper();
                    if (Value1NPOI == "" && Value2NPOI == "" && Value3NPOI == "")
                    {
                        continue;
                    }
                    //按数据库的长度
                    if (Value1NPOI == "" || (Value3NPOI != "FAIL" && Value3NPOI != "PASS" && Value3NPOI != "PROCESSING")
                        || Value1NPOI.Length > 10 || Value2NPOI.Length > 10)//FAIL（0）、PASS（1）和PROCESSING 
                    {
                        pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString() + " " + lineNum.ToString();
                        showErrorMessage(pmtMessage5);
                        return;
                    }
                    item.Process = processName;
                    item.Station = Value1NPOI;
                    item.PreStation = Value2NPOI;
                    if (Value3NPOI == "FAIL")
                    {
                        item.Status = 0;
                    }
                    else if (Value3NPOI == "PASS")
                    {
                        item.Status = 1;
                    }
                    else
                    {
                        item.Status = 2;
                    }

                    item.Editor = this.HiddenUserName.Value;
                    item.Cdt = DateTime.Now;
                    item.Udt = DateTime.Now;
                    processStationInfo.Add(item);
                }
                string uploadProcess = iProcessManager.UploadProcess(processInfo, processStationInfo);
                this.hidIsSubmitOK.Value = "OK";
                this.hidProcess.Value = uploadProcess;

            }
            else
            {
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                showErrorMessage(pmtMessage2);
                return;
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
        finally
        {
            TryDeleteTempFile(fullFileName);
        }
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
