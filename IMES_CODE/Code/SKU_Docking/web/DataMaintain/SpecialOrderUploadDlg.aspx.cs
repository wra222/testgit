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
using System.Xml.Serialization;
using System.Xml;

public partial class DataMaintain_SpecialOrderUploadDlg : System.Web.UI.Page
{
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String userName;
    public String Category;
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
    private ISpecialOrder iSpecialOrder;
    private const int EXCEL_DATA_START_ROW = 2;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iSpecialOrder = ServiceAgent.getInstance().GetMaintainObjectByName<ISpecialOrder>(WebConstant.SpecialOrderObject);
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
                Category = Request.QueryString["Category"]; 
                userName = StringUtil.decode_URL(userName);
                Category = StringUtil.decode_URL(Category);
                this.HiddenUserName.Value = userName;
                this.HidCategory.Value = Category;
                initLabel();
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
            DataTable dt = new DataTable();
            if (dFileUpload.FileName.Trim() == "")
            {
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                showErrorMessage(pmtMessage4);
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
                    string extName = fileName.Substring(fileName.LastIndexOf("."));
                    //if (extName != ".xls")
                    //{
                    //    pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                    //    showErrorMessage(pmtMessage2);
                    //    return;
                    //}
                    ////////////////////
                    Guid guid = System.Guid.NewGuid();
                    fullFileName = path + "\\" + guid.ToString() + extName;
                    dFileUpload.PostedFile.SaveAs(fullFileName);
                    dt = ExcelManager.getExcelSheetData(fullFileName);
                }
                catch
                {
                    pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                    showErrorMessage(pmtMessage2);
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

                List<SpecialOrderInfo> importList = new List<SpecialOrderInfo>();
                int iRowsCountNPOI = dt.Rows.Count;
                Dictionary<string, int> modelInList = new Dictionary<string, int>();
                List<string> Assettag = new List<string>();

                for (int iRow = 1; iRow < iRowsCountNPOI; iRow++)
                {
                    int lineNum = iRow + EXCEL_DATA_START_ROW - 1;
                    string Value1NPOI = dt.Rows[iRow][0].ToString();
                    string Value2NPOI = dt.Rows[iRow][1].ToString();
                    string Value3NPOI = dt.Rows[iRow][2].ToString();
                    if (Value1NPOI == "" && Value2NPOI == "" && Value3NPOI == "")
                    {
                        continue;
                    }
                    if (Value1NPOI == "" || Value2NPOI == "" || Value3NPOI == "")
                    {
                        pmtMessage5= this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                        string tmpMessage = pmtMessage5 + " " + lineNum.ToString();
                        showErrorMessage(tmpMessage);
                        return;
                    }
                    int outValue;
                    if (Int32.TryParse(Value2NPOI, out outValue) != true)
                    {
                        pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                        string tmpMessage = pmtMessage5 + " " + lineNum.ToString();
                        showErrorMessage(tmpMessage);
                        return;
                    }
                    if (modelInList.ContainsKey(Value1NPOI) == false)
                    {
                        modelInList.Add(Value1NPOI, outValue);
                        Assettag.Add(Value3NPOI);
                    }
                    else
                    {
                        //int currentNum = modelInList[Value1NPOI] + outValue;
                        //modelInList[Value1NPOI] = currentNum;
                        showErrorMessage("上傳資料重複：第"+ lineNum+"行，『" + Value1NPOI + "』");
                        return;
                    }
                }
                int count = 0;
                foreach (KeyValuePair<string, int> kvp in modelInList)
                {
                    SpecialOrderInfo item = new SpecialOrderInfo();
                    SpecialOrderStatus Status = SpecialOrderStatus.Created;
                    item.FactoryPO = kvp.Key;
                    item.Qty = kvp.Value;
                    item.AssetTag = Assettag[count];
                    item.Editor = this.HiddenUserName.Value;
                    item.Category = this.HidCategory.Value;
                    item.Status = Status;
                    item.Remark = "";
                    importList.Add(item);
                    count++;
                }
                if (importList.Count < 1)
                {
                    pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                    showErrorMessage(pmtMessage3);
                    return;
                }
                IList<string> List = iSpecialOrder.InsertSpecialOrder(importList);
                string dataString="";
                ListToString(List, ref dataString);
                this.dUploadTableResultData.Value = dataString;
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
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        finally
        {
            TryDeleteTempFile(fullFileName);
        }
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "OKComplete", "OKComplete();DealHideWait();", true);
    }

    private void ListToString(IList<string> List, ref string dataString)
    {
        foreach (string item in List)
        {
            dataString += item + ",";
        }
        if (dataString != "")
        {
            dataString = dataString.Substring(0, dataString.Length - 1);
        }
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
        sourceData = sourceData.Replace("\r\n", "<br>");
        sourceData = sourceData.Replace("\n", "<br>");
        sourceData = sourceData.Replace(@"\", @"\\");
        sourceData = sourceData.Replace("'", "\\'");
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
