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

public partial class DataMaintain_AssignModelUploadFile : System.Web.UI.Page
{

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID); 

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IAssignModelMgr iAssignModelMgr;
    public String userName;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
	public string pmtMsgOk;
    private const int EXCEL_DATA_START_ROW = 1;
 
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			pmtMsgOk = this.GetLocalResourceObject(Pre + "_pmtMsgOk").ToString();
			
            iAssignModelMgr = (IAssignModelMgr)ServiceAgent.getInstance().GetMaintainObjectByName<IAssignModelMgr>(WebConstant.MaintainAssignModelMgrObject);
            if (!this.IsPostBack)
            {

                /*System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                HttpRuntimeSection section = (HttpRuntimeSection)config.GetSection("system.web/httpRuntime");
                double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);
                this.hidFileMaxSize.Value = maxFileSize.ToString();*/

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
        try
        {
            if (dFileUpload.FileName.Trim() == "")
            {
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                showErrorMessage(pmtMessage4);
                //showErrorMessage("需要选择上传的文件.");
                return;
            }        
            if (this.dFileUpload.HasFile)
            {
                IList<List<string>> lstAssignModel = new List<List<string>>();

                string strfile = this.dFileUpload.FileName;
                string fullName = Server.MapPath("../tmp/") + strfile;
                this.dFileUpload.PostedFile.SaveAs(fullName);
                DataTable dt = ExcelManager.getExcelSheetData(fullName, true);
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    i++;
                    if (i == 1)
                    {
                        continue;
                    }
                    List<string> item = new List<string>();
                    item.Add(dr[0].ToString());
                    item.Add(dr[1].ToString());
                    item.Add(dr[2].ToString());
                    item.Add(dr[3].ToString());
                    lstAssignModel.Add(item);
                }

                if (lstAssignModel.Count < 1)
                {
                    //showErrorMessage("没有需要上传的数据.");
                    pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                    showErrorMessage(pmtMessage3);
                    return;
                }

                this.hidIsSubmitOK.Value = "OK";
                iAssignModelMgr.Import(lstAssignModel, this.HiddenUserName.Value);

                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "OKComplete", "OKComplete();HideWait();", true);
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);

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

        this.btnOK.InnerText = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.InnerText = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();

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
