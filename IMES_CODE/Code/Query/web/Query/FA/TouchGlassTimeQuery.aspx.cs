using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Xml.Linq;
using com.inventec.imes.DBUtility;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
//using Microsoft.SqlServer.Dts.Runtime;
using System.Data.SqlClient;

public partial class Query_FATouchGlassTime_Query : System.Web.UI.Page
{
    IFA_TouchGlassTimeQuery CT_Query = ServiceAgent.getInstance().GetObjectByName<IFA_TouchGlassTimeQuery>(WebConstant.IFA_TouchGlassTimeQuery);
    String DBConnection = "";
    public string pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!Page.IsPostBack)
        {
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
    public void Query_Click(object sender, EventArgs e)
    {
        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        string ct = txtct.Text;
        if (ct != "")
        {
            DataTable dt = CT_Query.GetCTNo(DBConnection, ct);
            bindTable(dt);
        }
        else
        {
            if (timeEnd.Day - timeStart.Day > 7)
            {
                showErrorMessage("選擇的日期太長，只能查詢7天的數據！");
            }
            else
            {
                DataQuery("1");
            }
         }
    }
    protected void btnUploadList_ServerClick(object sender, System.EventArgs e)
    {
        string newName = DateTime.Now.Ticks.ToString() + FileUp.PostedFile.FileName.Substring(FileUp.PostedFile.FileName.LastIndexOf("."));
        string fullName = Server.MapPath("~") + "/" + newName;
        try
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = FileUp.FileName;
            string extName = fileName.Substring(fileName.LastIndexOf("."));
            ShowInfoClear();
            FileUp.SaveAs(fullName);
            DataTable dt = ExcelManager.getExcelSheetData(fullName);
            int coun = 0;
            string errormsg = "";
            if (dt.Rows.Count > 0)
            {
                CT_Query.InsertShipNo(DBConnection, dt);
            }
            else
            {
                showErrorMessage("您選擇的Excel無數據或格式不正確！");
            }
            DataQuery("2");
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            File.Delete(fullName);
            endWaitingCoverDiv();

        }
    }
    public void DataQuery(string tp)
    {
        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        DataTable dt = CT_Query.GetCTMessage(DBConnection, timeStart, timeEnd, tp);
        bindTable(dt);
    }
    private void bindTable(DataTable dt)
    {
        gvResult.DataSource = dt;
        gvResult.DataBind();
        if (dt.Rows.Count > 0)
        {
            InitGridView();
        }
        else
        {
            showErrorMessage("當前查詢無任何數據！");
        }
    }
    private void InitGridView()
    {
        gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(12);
        gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        txtct.Text = "";
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();

        if (gvResult.HeaderRow != null && gvResult.HeaderRow.Cells.Count > 0)
            tu.ExportExcel(gvResult, "Touch Glass Time", Page);
        else
        {
            string str = "   alert( 'Please select one record! ' );";
            string script = "<script type='text/javascript' language='javascript'>" + str + "</script>";
            ClientScript.RegisterStartupScript(GetType(), "", script);
        }
    }
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    private void ShowInfoClear()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

}
