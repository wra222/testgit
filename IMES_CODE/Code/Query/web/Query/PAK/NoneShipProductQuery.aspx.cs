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

public partial class Query_NoneShipProductQuery : IMESQueryBasePage 
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_NoneShipProductQuery iNoneShipProductQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_NoneShipProductQuery>(WebConstant.IPAK_NoneShipProductQuery);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    String DBConnection = "";
    public int[] GvWidth = {50, 50, 50, 50, 50, 50, 50 };
    private void GetConnection()
    {
        string dbName = Request.QueryString["dbName"];
        if (string.IsNullOrEmpty(dbName))
            dbName = "HPIMES";
        DBInfo obj = iConfigDB.GetDBInfo();
        DBConnection = string.Format(obj.OnLineConnectionString, dbName);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!IsPostBack) {
            GetLine();
            GetStation();
        }
    }

    private void GetLine()
    {
        string customer = Master.userInfo.Customer;
        DataTable dtPdLine = iNoneShipProductQuery.GetPdLine(DBConnection);
        lboxPdLine.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                ListItem item = new ListItem();
                item.Text = dr["AliasLine"].ToString().Trim();
                item.Value = dr["AliasLine"].ToString().Trim();
                lboxPdLine.Items.Add(item);
            }
        }
    }

    private void GetStation()
    {
        string customer = Master.userInfo.Customer;
        DataTable dtStation = iNoneShipProductQuery.GetStation(DBConnection);

        lboxStation.Items.Clear();
        if (dtStation.Rows.Count > 0)
        {
            foreach (DataRow dr in dtStation.Rows)
            {
                string value = dr["Value"].ToString().Trim();
                string[] stationList = value.Split(',');
                foreach (string items in stationList)
                {
                    ListItem item = new ListItem();
                    item.Text = items.Trim();
                    item.Value = items.Trim();
                    lboxStation.Items.Add(item);
                }
            }
        }
    }

    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        queryClick(sender, e);
    }
    
    public void queryClick(object sender, System.EventArgs e)
    {                
        try
        { 
            string model = this.hfModel.Value;
            string PdLine = this.hidPdLineList.Value;
            string station = this.hidstationList.Value;
           
            DataTable dt = getDataTable(model, PdLine, station);
            
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
            
            if (dt.Rows.Count > 0)
            {
                
                for (int i = 0; i < GvWidth.Length; i++)
                {
                    gvQuery.HeaderRow.Cells[i].Width = Unit.Pixel(GvWidth[i]);
                }
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                endWaitingCoverDivAndAlert();
                
                EnableBtnExcel(this, false, btnExport.ClientID);
                return;
            }

            String script = "<script language='javascript'>" + "\r\n" +
            "cleanhfModel();" + "\r\n" +
            "</script>";
            ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "afterquery", script, false);
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

    private DataTable getDataTable(string model,string PdLine, string station)
    {

        DataTable dt = iNoneShipProductQuery.GetQueryResult(DBConnection, model, PdLine, station);
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
        scriptBuilder.AppendLine("</script>");
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDivAndAlert()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();alert('查無資料');" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void gvQuery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DateTime d;
            if (DateTime.TryParse(e.Row.Cells[5].Text, out d))
            {
                e.Row.Cells[5].Text = d.ToString("yyyy/MM/dd");
            }
            if (DateTime.TryParse(e.Row.Cells[6].Text, out d))
            {
                e.Row.Cells[6].Text = d.ToString("yyyy/MM/dd HH:mm");
            }
           
        }
    }
}
