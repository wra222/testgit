using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.IO;
using System.Web.Configuration;
using com.inventec.system.util;
using System.Web;
using System.Reflection;
using System.Runtime.InteropServices;
using System;
using System.Data;

public partial class DataMaintain_FAKittingUploadDlg : System.Web.UI.Page
{

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String userName;
    public String lineValue;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;
    public string pmtMessage10;

    private const int EXCEL_DATA_START_ROW = 2;

    public string KittingTypeFAKitting;
    public string KittingTypePAKKitting;
    public string KittingTypeFALabel;
    public string KittingTypePAKLabel;

    public const string FA_STAGE = "FA";
    public const string PAK_STAGE = "PAK";
    public const string FA_LABEL = "FA Label";
    public const string PAK_LABEL = "PAK Label";

    public const string TMPKIT_FA = "FA";  //存TMPKIT表中的Type, char(4)
    public const string TMPKIT_PAK = "PAK"; //存TMPKIT表中的Type, char(4)

    private ILightNo iLightNo;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //!!!检查文件类型 xls

            iLightNo = (ILightNo)ServiceAgent.getInstance().GetMaintainObjectByName<ILightNo>(WebConstant.MaintainLightNoObject);

            //KittingTypeFAKitting = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightNoKittingTypeValue1");
            //KittingTypePAKKitting = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightNoKittingTypeValue2");
            //KittingTypeFALabel = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightNoKittingTypeValue3");
            //KittingTypePAKLabel = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainLightNoKittingTypeValue4");

            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            //pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();


            if (!this.IsPostBack)
            {              

                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                HttpRuntimeSection section = (HttpRuntimeSection)config.GetSection("system.web/httpRuntime");
                double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);
                this.hidFileMaxSize.Value = maxFileSize.ToString();

                this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
                
                userName = Request.QueryString["userName"];
                userName = StringUtil.decode_URL(userName);
                this.HiddenUserName.Value = userName;

                lineValue = Null2String(Request.QueryString["line"]);                
                lineValue = StringUtil.decode_URL(lineValue);
                initLabel();

                //InitComboxData(kittingTypeValue);


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

    //private void InitComboxData(string kittingType)
    //{
    //    if (kittingType == KittingTypeFAKitting)
    //    {
    //        this.cmbMaintainLightNoPdLine.Stage = FA_STAGE;
    //        this.cmbMaintainLightNoPdLine.initMaintainLightNoPdLine();
    //        this.cmbMaintainLightNoPartType.initMaintainLightNoPartType();            
    //    }
    //    else if (kittingType == KittingTypePAKKitting)
    //    {
    //        this.cmbMaintainLightNoPdLine.Stage = PAK_STAGE;
    //        this.cmbMaintainLightNoPdLine.initMaintainLightNoPdLine();
    //        this.cmbMaintainLightNoPartType.initMaintainLightNoPartType();
    //        this.cmbMaintainLightNoPartType.InnerDropDownList.SelectedIndex = 0;
    //    }
      
      
    //}

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
                //int endRow = worksheet.UsedRange.Rows.Count + startRow;
                if (endRow < startRow)
                {
                    pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                    showErrorMessage(pmtMessage3);
                    //showErrorMessage("没有需要上传的数据.");
                    return;
                }

                //string excelRange = string.Format("A{0}:{1}{2}",EXCEL_DATA_START_ROW.ToString(), "B", endRow);

                //object[,] getdata = (object[,])worksheet.get_Range(excelRange, Type.Missing).Value2;

                //String kittingType = this.hidKittingType.Value;
                //string pdLine =this.HidCurPdLine.Value;// this.cmbMaintainLightNoPdLine.InnerDropDownList.SelectedValue;
                string pdLine = this.dPdLine.Text;

                List<TmpKitInfoDef> importList = new List<TmpKitInfoDef>();
                int iRowsCountNPOI = dt.Rows.Count;
                //int iRowsCount = getdata.GetLength(0);
                //int iColsCount = getdata.GetLength(1);

                Dictionary<string, string> modelInList = new Dictionary<string, string>();

                //if (kittingType == KittingTypeFAKitting)
                //{

                for (int iRow = 1; iRow < iRowsCountNPOI; iRow++)
                {
                    int lineNum = iRow + EXCEL_DATA_START_ROW - 1;

                    TmpKitInfoDef item = new TmpKitInfoDef();
                    string Value1NPOI = dt.Rows[iRow][0].ToString();
                    //object value1 = getdata[iRow, 1];
                    //string strValue1 = Null2String(value1);
                    //object value2 = getdata[iRow, 2];
                    //string strValue2 = Null2String(value2);

                    if (Value1NPOI == "")
                    {
                        continue;
                    }

                    item.Model = Value1NPOI;
                    item.Type = TMPKIT_FA;
                    item.PdLine = pdLine;
                    item.Qty = 0;
                    if (modelInList.ContainsKey(item.Model) == false)
                    {
                        importList.Add(item);
                        modelInList.Add(item.Model, item.Model);
                    }
                    else
                    {
                        //上传的excel中有重复的model item.Model
                        pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString() + " " + item.Model;
                        showErrorMessage(pmtMessage10);
                        return;
                    }

                }

                if (importList.Count < 1)
                {
                    //showErrorMessage("没有需要上传的数据.");
                    pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                    showErrorMessage(pmtMessage3);
                    return;
                }

                iLightNo.ImportTmpKit(importList, pdLine, TMPKIT_FA);

                string partType = this.HidCurPartType.Value; //this.cmbMaintainLightNoPartType.InnerDropDownList.SelectedValue;
                iLightNo.UploadModelForWipBufferFA(pdLine, partType);
                //this.hidIsSubmitOK.Value = "OK";
                //iForwarder.ImportForwarder(forwarderList);

                //}
                //else //if (kittingType == KittingTypePAKKitting)
                //{

                //for (int iRow = 1; iRow <= iRowsCount; iRow++)
                //{
                //    int lineNum = iRow + EXCEL_DATA_START_ROW - 1;

                //    TmpKitInfoDef item = new TmpKitInfoDef();
                //    object value1 = getdata[iRow, 1];
                //    string strValue1 = Null2String(value1);
                //    object value2 = getdata[iRow, 2];
                //    string strValue2 = Null2String(value2);


                //    if (strValue1 == "" && strValue2 == "")
                //    {
                //        continue;
                //    }

                //    if (strValue1 == "" || strValue2 == "")
                //    {
                //        pmtMessage5= this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                //        string tmpMessage = pmtMessage5 + " " + lineNum.ToString();
                //        showErrorMessage(tmpMessage);
                //        return;
                //    }

                //    int outValue;
                //    if (Int32.TryParse(strValue2, out outValue) != true)
                //    {
                //        pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                //        string tmpMessage = pmtMessage5 + " " + lineNum.ToString();
                //        showErrorMessage(tmpMessage);
                //        return;
                //    }

                //    //数量不能小于等于0
                //    if (outValue <= 0)
                //    {
                //        pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                //        string tmpMessage = pmtMessage5 + " " + lineNum.ToString();
                //        showErrorMessage(tmpMessage);
                //        return;
                //    }

                //    item.Model = strValue1;
                //    item.Type = TMPKIT_PAK;
                //    item.PdLine = pdLine;
                //    item.Qty = outValue;

                //    if (modelInList.ContainsKey(item.Model) == false)
                //    {
                //        importList.Add(item);
                //        modelInList.Add(item.Model, item.Model);
                //    }
                //    else
                //    {
                //        //上传的excel中有重复的model item.Model
                //        pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString()+" "+item.Model;
                //        showErrorMessage(pmtMessage10);
                //        return;
                //    }

                //}
                //if (importList.Count < 1)
                //{
                //    //showErrorMessage("没有需要上传的数据.");
                //    pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                //    showErrorMessage(pmtMessage3);
                //    return;
                //}

                //iLightNo.ImportTmpPAKKit(importList, pdLine, TMPKIT_PAK);
                //iLightNo.UploadModelForWipBufferPAK(pdLine);
                //}                 

            }
            else
            {
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "OKComplete", "OKComplete();DealHideWait();", true);

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
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblPartType.Text = this.GetLocalResourceObject(Pre + "_lblPartType").ToString();
        this.dPdLine.Text = lineValue;

    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("\r\n", "<br>");
        sourceData = sourceData.Replace("\n", "<br>");
        sourceData = sourceData.Replace(@"\", @"\\");
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }


    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg+ "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }    
    
}
