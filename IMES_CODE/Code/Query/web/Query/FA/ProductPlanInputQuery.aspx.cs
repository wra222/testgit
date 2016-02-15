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

public partial class Query_ProductPlanInputQuery : IMESQueryBasePage 
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    IFA_ProductPlanInputQuery ProductPlanInputQuery = ServiceAgent.getInstance().GetObjectByName<IFA_ProductPlanInputQuery>(WebConstant.IFA_ProductPlanInputQuery);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    String DBConnection = "";

    //public string[] GvName = { "ShipDate", "PlanLine", "Model","InputDate","InputLine","PlanQty", "39", "39未投", "3A", "3A未投", "40", "40未投" };
    public int[] GvWidth = {40,30, 80,50, 50, 50, 50, 50, 50, 50, 50, 50 };
    //public string[] GvNameDetail = { "ProductID", "CUSTSN", "Family", "Model", "CurrentStation", "Descr", "CurrentLine", "Cdt", "Udt" };
    public int[] GvWidthDetail = {20, 40, 50, 40, 50, 40, 80, 40, 80, 80 };
    //private static int IDX_Monthly = 1;
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
            
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
            {
                txtPeriodFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
                txtPeriodToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
            }
            else if (DateTime.Now.Hour >= 20)
            {
                txtPeriodFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
                txtPeriodToDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 08:00");
            }
            else if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8)
            {
                txtPeriodFromDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:30");
                txtPeriodToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
            }
            GetLine();
        }
    }

    private void GetLine()
    {
        string Starg = "FA";
        string customer = Master.userInfo.Customer;
        DataTable dtPdLine = ProductPlanInputQuery.GetPdLine(DBConnection, Starg);
        lboxPdLine.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                ListItem item = new ListItem();
                item.Text = dr["PdLine"].ToString().Trim();
                item.Value = dr["PdLine"].ToString().Trim();
                item.Selected = true;
                lboxPdLine.Items.Add(item);
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
            string redType = this.hidredValue.Value;
            string PdLine = this.hidPdLineList.Value;
            if (PdLine != "")
            {
                PdLine = PdLine.Substring(0, PdLine.Length - 1);
            }
            string model = this.hfModel.Value;
            DataTable dt=new DataTable();
            if (redType == "GET_DAY")
            {
                string txtday = this.txtFromDate.Text;
                dt = getDataTable(PdLine, txtFromDate.Text,model);
            }
            else
            {
                string txtfromday = this.txtPeriodFromDate.Text;
                string txttoday = this.txtPeriodToDate.Text;
                dt = getDataTablebyDayRange(PdLine, txtfromday, txttoday, model);
            }
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
            gvQuery_Detail.DataSource = null;
            gvQuery_Detail.DataBind();
            if (dt.Rows.Count > 0)
            {
                for (int i = 6; i <= dt.Columns.Count - 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        float TotalInputQty = 0;
                        for (int j = 0; j <= dt.Rows.Count - 1; j++)
                        {
                            float qty = float.Parse(gvQuery.Rows[j].Cells[i].Text);
                            TotalInputQty += qty;
                        }
                        if (gvQuery.FooterRow.Cells[i].Text != "0")
                        {
                            string type = "IN";//投入
                            string shipDate = "";
                            string pdLine = "";
                            string station = gvQuery.HeaderRow.Cells[i].Text.Trim();
                            shipDate = this.txtFromDate.Text;
                            //pdLine = e.Row.Cells[1].Text.Trim();

                            gvQuery.FooterStyle.Font.Bold = true;
                            gvQuery.FooterRow.Cells[i].Text = TotalInputQty.ToString();

                            gvQuery.FooterRow.Cells[i].CssClass = "querycell";
                            gvQuery.FooterRow.Cells[i].Attributes.Add("onclick", "SelectDetailTotal('" + shipDate + "','" + station + "','" + type + "')");
                            gvQuery.FooterRow.Cells[i].BackColor = System.Drawing.Color.Yellow;
                            
                        }
                    }
                }
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
                EnableBtnExcel2(this, false, btnExportDetail.ClientID);
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

    protected void QueryDetailClick(object sender, EventArgs e)
    {
        string redType = this.hidredValue.Value;
        string shipdate = this.hidshipDate.Value;
        string pdLine = this.hidpdLine.Value;
        string model = this.hidmodel.Value;
        string inputPdLine = this.hidinputPdLine.Value;
        string station = this.hidstation.Value;
        string type = this.hidtype.Value;
        try
        {
            DataTable dt = new DataTable();
            if (redType == "GET_DAY")
            {
                string txtday = this.hidshipDate.Value;
                dt = getDetailDataTable(pdLine, shipdate, model, station, type);
            }
            else
            {
                string txtfromday = this.txtPeriodFromDate.Text;
                string txttoday = this.txtPeriodToDate.Text;
                dt = getDetailDataTablebyDayRange(pdLine, txtfromday, txttoday, model, station, type, inputPdLine, shipdate);
            }
            gvQuery_Detail.DataSource = dt;
            gvQuery_Detail.DataBind();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < GvWidthDetail.Length; i++)
                {
                    gvQuery_Detail.HeaderRow.Cells[i].Width = Unit.Pixel(GvWidthDetail[i]);
                }
                EnableBtnExcel2(this, true, btnExportDetail.ClientID);
            }
            else
            {
                endWaitingCoverDivAndAlert();
                EnableBtnExcel2(this, false, btnExportDetail.ClientID);
                
                return;
            }
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

    protected void QueryDetailTotalClick(object sender, EventArgs e)
    {
        string redType = this.hidredValue.Value;
        string shipdate = this.txtFromDate.Text;
        string pdLine = this.hidpdLine.Value;
        string model = this.hidmodel.Value;
        string inputPdLine = this.hidinputPdLine.Value;
        string station = this.hidstation.Value;
        string type = this.hidtype.Value;
        try
        {
            DataTable dt = new DataTable();
            if (inputPdLine != "")
            {
                inputPdLine = inputPdLine.Substring(0, inputPdLine.Length - 1);
            }
            if (pdLine != "")
            {
                pdLine = pdLine.Substring(0, pdLine.Length - 1);
            }
            
            if (redType == "GET_DAY")
            {
                
                string txtday = this.hidshipDate.Value;
                dt = getDetailDataTable(pdLine, shipdate, model, station, type);
            }
            else
            {
                string txtfromday = this.txtPeriodFromDate.Text;
                string txttoday = this.txtPeriodToDate.Text;
                dt = getDetailDataTablebyDayRange("", txtfromday, txttoday, model, station, type, inputPdLine, "");
            }
            gvQuery_Detail.DataSource = dt;
            gvQuery_Detail.DataBind();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < GvWidthDetail.Length; i++)
                {
                    gvQuery_Detail.HeaderRow.Cells[i].Width = Unit.Pixel(GvWidthDetail[i]);
                }
                EnableBtnExcel2(this, true, btnExportDetail.ClientID);
            }
            else
            {
                endWaitingCoverDivAndAlert();
                EnableBtnExcel2(this, false, btnExportDetail.ClientID);

                return;
            }
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

    private DataTable getDataTable(string PdLine, string ShipDate,string model)
    {
        DataTable dt = ProductPlanInputQuery.GetQueryResult(DBConnection, PdLine, ShipDate, model);
        DataTable dtForExcel = new DataTable();
        return dt;
    }

    private DataTable getDataTablebyDayRange(string PdLine, string dayFrom, string dayTo, string model)
    {
        DataTable dt = ProductPlanInputQuery.GetQueryResultByDayRange(DBConnection, PdLine,dayFrom,dayTo, model);
        DataTable dtForExcel = new DataTable();
        return dt;
    }

    private DataTable getDetailDataTable(string PdLine, string ShipDate, string model, string station, string type)
    {
        DataTable dt = ProductPlanInputQuery.GetDetailQueryResult(DBConnection, PdLine, ShipDate, model,station,type);
        DataTable dtForExcel = new DataTable();
        return dt;
    }

    private DataTable getDetailDataTablebyDayRange(string PdLine, string dayFrom, string dayTo, string model, string station, string type, string inputPdLine, string ShipDate)
    {
        DataTable dt = ProductPlanInputQuery.GetDetailQueryResultByDayRange(DBConnection, PdLine, dayFrom, dayTo, model,station,type,inputPdLine, ShipDate);
        DataTable dtForExcel = new DataTable();
        return dt;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        DataTable dt = (DataTable)gvQuery.DataSource;
        tu.ExportExcel(gvQuery, Page.Title, Page);
    }

    protected void btnExportDetail_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        DataTable dt = (DataTable)gvQuery_Detail.DataSource;
        tu.ExportExcel(gvQuery_Detail, Page.Title, Page);
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
    protected void gvQuery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        string shipDate = "";
        string pdLine = "";
        string model = "";
        string inputPdLine = "";
        string station = "";
        string type = "";
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            shipDate = e.Row.Cells[0].Text.Trim();
            pdLine = e.Row.Cells[1].Text.Trim();
            model = e.Row.Cells[2].Text.Trim();
            inputPdLine = e.Row.Cells[3].Text.Trim();

            for (int i = 6; i < e.Row.Cells.Count; i++)
            {
                if (i % 2 == 0)
                {
                    type = "IN";//投入
                    station = gvQuery.HeaderRow.Cells[i].Text.Trim();
                }
                else
                { 
                    type = "NONE";//未投
                }
                if (e.Row.Cells[i].Text == "&nbsp;" || e.Row.Cells[i].Text == "0" || e.Row.Cells[i].Text == "OK")
                {
                }
                else
                {
                    e.Row.Cells[i].CssClass = "querycell";
                    e.Row.Cells[i].Attributes.Add("onclick", "SelectDetail('"+shipDate+"','"+pdLine+"','"+model+"','"+inputPdLine+"','"+station+"','"+type+"')");
                    e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                }
            }
        }
    }
}
