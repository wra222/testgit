using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;

public partial class PAK_PalletCollection : System.Web.UI.Page
{
    protected string Pre = System.Configuration.ConfigurationManager.AppSettings[WebConstant.LANGUAGE] + "_";

    protected string Editor = "";
    protected string Customer;

    protected string AlertSelectLine = "";
    protected string AlertSelectFloor = "";
    protected string AlertWrongCode = "";
    protected string AlertSuccess = "";
    protected string AlertDeliveryFull = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitPage();
            Editor = Request.QueryString["UserId"];
            Customer = Request.QueryString["Customer"];
            this.CmbPdLine.Station = Request["station"];
            this.CmbPdLine.Customer = Customer;
            this.GridViewExt1.DataSource = GetTable();
            this.GridViewExt1.DataBind();
        }
    }



    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);

    }

    private void WriteAlertInfo(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "WriteAlertInfoAgent", scriptBuilder.ToString(), false);

    }

    private void InitPage()
    {
        AlertSelectLine = GetLocalResourceObject(Pre + "AlertSelectLine").ToString();
        AlertSelectFloor = GetLocalResourceObject(Pre + "AlertSelectFloor").ToString();
        AlertWrongCode = GetLocalResourceObject(Pre + "AlertWrongCode").ToString();
        AlertSuccess = GetLocalResourceObject(Pre + "AlertSuccess").ToString();
        AlertDeliveryFull = GetLocalResourceObject(Pre + "AlertDeliveryFull").ToString();
        this.LabelDataEntry.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "lblDataEntry");
        this.LabelLine.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "lblPdline");
        this.LabelFloor.Text = this.GetLocalResourceObject(Pre + "LabelFloor").ToString();
        this.btnPrintSet.Value = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "btnPrtSet");
        
    }

    private DataTable InitTable()
    {
        DataTable result = new DataTable();
        result.Columns.Add("CartonNo", Type.GetType("System.String"));
        return result;
    }

    private DataTable GetTable()
    {
        DataTable result = InitTable();
        for (int i = 0; i < DefaultRowsCount; i++)
        {
            DataRow newRow = result.NewRow();
            
            newRow[0] = string.Empty;
            result.Rows.Add(newRow);
        }
        return result;
    }

    private int DefaultRowsCount = 7;
}
