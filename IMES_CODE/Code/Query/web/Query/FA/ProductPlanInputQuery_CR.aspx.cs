using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
//using IMES.DataModel;

public partial class Query_ProductPlanInputQuery_CR : IMESQueryBasePage 
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    IFA_ProductPlanInputQuery_CR ProductPlanInputQuery_CR = ServiceAgent.getInstance().GetObjectByName<IFA_ProductPlanInputQuery_CR>(WebConstant.IFA_ProductPlanInputQuery_CR);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    String DBConnection = "";

    public string[] GvName = { "InputDate", "PdLine", "Model", "Qty", "CR01", "CR01未投", "CR05", "CR05未投", "CR10", "CR10未測試", "CR32", "CR32未結合" };
    public int[] GvWidth = { 40, 30, 80, 50, 50, 50, 50, 50, 50, 50, 50, 50 };
    private static int IDX_Monthly = 1;

    private void GetConnection()
    {
        //string dbName = Request.QueryString["dbName"];
        //if (string.IsNullOrEmpty(dbName))
        //    dbName = "HPIMES_RPT";
        //DBInfo obj = iConfigDB.GetDBInfo();
        //DBConnection = string.Format(obj.HistoryConnectionString, dbName);
        string dbName = Request.QueryString["dbName"];
        if (string.IsNullOrEmpty(dbName))
            dbName = "HPIMES";
        DBInfo obj = iConfigDB.GetDBInfo();
        DBConnection = string.Format(obj.OnLineConnectionString, dbName);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        //GetConnection();
        //
        if (!IsPostBack) {

            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
            {
                txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
            }
            else if (DateTime.Now.Hour >= 20)
            {
                txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
                txtToDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 08:00");
            }
            else if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:30");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
            }
            GetLine();
        }
    }

    private void GetLine()
    {
        string Stage = "CleanRoom";
        string customer = Master.userInfo.Customer;

        DataTable dtPdLine = ProductPlanInputQuery_CR.GetPdLine(DBConnection, Stage);
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["PdLine"].ToString().Trim(), dr["PdLine"].ToString().Trim()));
            }
        }
    }

    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //string time = DateTime.Now.ToString("yyyy-MM-dd");
        //if (Convert.ToDateTime(this.txtFromDate.Text) >= Convert.ToDateTime(time))
        //{
        //    queryClick(sender, e);
        //}
        //else
        //{
        //    this.gvQuery.DataSource = null;
        //    this.gvQuery.DataBind();
        //    endWaitingCoverDivAndAlertTime();
        //}
        queryClick(sender, e);
        
    }
    
    public void queryClick(object sender, System.EventArgs e)
    {                
        try
        {
            
            string PdLine = lboxPdLine.SelectedValue.ToString();
            DataTable dt = getDataTable(PdLine, txtFromDate.Text, txtToDate.Text);
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < GvWidth.Length; i++)
                {
                    gvQuery.HeaderRow.Cells[i].Width = Unit.Pixel(GvWidth[i]);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (gvQuery.Rows[i].Cells[4].Text == "0")
                    {
                        gvQuery.Rows[i].Cells[4].BackColor = System.Drawing.Color.Yellow;
                    }
                }
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                endWaitingCoverDivAndAlert();
                EnableBtnExcel(this, false, btnExport.ClientID);
                return;
            }
            //hidPeriod.Value = qType;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.ToString());
        }
        finally
        {
            endWaitingCoverDiv();
            
        }
    }

    private DataTable getDataTable(string PdLine, string InputBegDate, string InputEndDate)
    {
        DataTable dt = ProductPlanInputQuery_CR.GetQueryResult(DBConnection, PdLine, InputBegDate, InputEndDate);
        DataTable dtForExcel = new DataTable();
        return dt;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        DataTable dt = (DataTable)gvQuery.DataSource;

        tu.ExportExcel(gvQuery, Page.Title, Page);
    }

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        //scriptBuilder.AppendLine("endWaitingCoverDiv();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDivAndAlert()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();alert('查無資料');" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void endWaitingCoverDivAndAlertTime()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();alert('所選時間必須大於今天');" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

}
