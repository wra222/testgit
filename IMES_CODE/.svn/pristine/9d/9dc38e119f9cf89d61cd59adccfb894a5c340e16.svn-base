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
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Web.Configuration;

public partial class DataMaintain_BTOceanOrderUploadFile : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private static System.Configuration.Configuration config1 = WebConfigurationManager.OpenWebConfiguration("~");
    private static HttpRuntimeSection section = (HttpRuntimeSection)config1.GetSection("system.web/httpRuntime");
    public double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);
    public string msgExceedMaxSize;
    public string msgNoFile;
    public string msgInvalidFileType;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            msgExceedMaxSize = this.GetLocalResourceObject(Pre + "_msgExceedMaxSize").ToString();
            msgNoFile = this.GetLocalResourceObject(Pre + "_MsgNoFile").ToString();
            msgInvalidFileType = this.GetLocalResourceObject(Pre + "_MsgInvalidFileType").ToString();
            this.btnFileUpload.Value = this.GetLocalResourceObject(Pre + "_btnFileUpload").ToString();
            this.cancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
            hidFileMaxSize.Value = maxFileSize.ToString();
            if (!Request.QueryString["pdLine"].Equals(""))
            {
                string pdLine = Request.QueryString["pdLine"];
                this.lblPDline.Text = pdLine;
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

    protected void Upload_ServerClick(object sender, EventArgs e)
    {
        string strFileName = this.txtBrowse.PostedFile.FileName;
        string strFileExt = strFileName.Substring(strFileName.IndexOf("."));
        string strServerFileName = Guid.NewGuid().ToString() + strFileExt;
        string strServerFilePath = Server.MapPath(".") + "\\" + strServerFileName;
        try
        {
            txtBrowse.PostedFile.SaveAs(strServerFilePath);
            this.hidFileName.Value = strServerFilePath;
            ScriptManager.RegisterStartupScript(this, typeof(System.Object), "uploadFile", "uploadFile();", true);
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

    /// <summary>
    /// 判断是否为空
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorMsg"></param>
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");

    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;

    }
}
