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
using System.Text.RegularExpressions;
using IMES.DataModel;
using System.Text;
using com.inventec.iMESWEB;
using System.Web.Configuration;
using IMES.Infrastructure;
using System.Xml.Linq;
using IMES.Station.Interface.CommonIntf;
//using IMES.Maintain.Interface.MaintainIntf;

public partial class CommonControl_CmbUpLoadFile : System.Web.UI.UserControl
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private static System.Configuration.Configuration config1 = WebConfigurationManager.OpenWebConfiguration("~");
    private static HttpRuntimeSection section = (HttpRuntimeSection)config1.GetSection("system.web/httpRuntime");
    public double maxFileSize = Math.Round(section.MaxRequestLength / 1024.0, 1);
    public string msgExceedMaxSize;
    public string msgNoFile;
    public string msgInvalidFileType;
    public FileUpload fileUpload
    {
        get
        {
            return this.txtBrowse;
        }
    }

    public HtmlInputButton uploadFileButton
    {
        get
        {
            return this.btnFileUpload;
        }
    }

    public string stringPath
    {
        get
        {
            return this.txtBrowse.PostedFile.FileName.ToString();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        msgExceedMaxSize = this.GetLocalResourceObject(Pre + "_msgExceedMaxSize").ToString();
        msgNoFile = this.GetLocalResourceObject(Pre + "_MsgNoFile").ToString();
        msgInvalidFileType = this.GetLocalResourceObject(Pre + "_MsgInvalidFileType").ToString();
        try
        {
            this.btnFileUpload.Value = HttpContext.GetLocalResourceObject("~/CommonControl/CmbUpLoadFile.ascx", Pre + "_btnFileUpload").ToString();

            hidFileMaxSize.Value = maxFileSize.ToString();

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

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\0013", string.Empty).Replace("\0010", "\n") + "\");");
        scriptBuilder.AppendLine("</script>");
    }
}
