using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using com.inventec.iMESWEB;
using com.inventec.system.util;
using IMES.Infrastructure;
using System.Text;

public partial class DataMaintain_ProcessSaveAs : System.Web.UI.Page
{
    public String userName="";
    IProcessManager iProcessManager;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;

    protected void Page_Load(object sender, EventArgs e)
    {
        iProcessManager = ServiceAgent.getInstance().GetMaintainObjectByName<IProcessManager>(WebConstant.IProcessManager);
        if (!Page.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();

            string oldProcess = Request.QueryString["Process"];
            oldProcess = StringUtil.decode_URL(oldProcess);
            this.dOldProcess.Value = oldProcess;

            string oldType = Request.QueryString["Type"];
            oldType = StringUtil.decode_URL(oldType);
            this.dOldType.Value = oldType;

            initLabel();
            userName = Request.QueryString["userName"]; //UserInfo.UserId;
            userName = StringUtil.decode_URL(userName);
            this.HiddenUserName.Value = userName;
        }
    }

    private void initLabel()
    {
        this.lblProcess.Text = this.GetLocalResourceObject(Pre + "_lblProcess").ToString();
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
        this.dType.Text = this.dOldType.Value;
        this.btnOK.Value = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
    }

    protected void btnOK_ServerClick(object sender, System.EventArgs e)
    {

        string oldProcess = this.dOldProcess.Value.Trim();

        string process = this.dProcess.Text.Trim();
        string descr = this.dDescr.Text.Trim();
        string type = this.dOldType.Value.Trim();

        ProcessMaintainInfo processObj = new ProcessMaintainInfo();
        processObj.Description = descr;
        processObj.Process =process;
        processObj.Type  =type;
        processObj.Editor =this.HiddenUserName.Value;
        processObj.Cdt =DateTime.Now;
        processObj.Udt = DateTime.Now;
        try
        {
            iProcessManager.ProcessSaveAs(processObj, oldProcess);
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
        
        process = replaceSpecialChart(process);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + process + "');", true);

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
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
}
