using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
//using IMES.Station.Interface.CommonIntf;
using System.Data;
using System.IO;


using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

public partial class Query_FA_ProductEfficiency : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IPdLine PdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.PdLine);

    IFA_ProductEfficiency ProductEfficiency = ServiceAgent.getInstance().GetObjectByName<IFA_ProductEfficiency>(WebConstant.IProductEfficiency);

    public string[] GvQueryColumnName = { "LESSON", "TimeRange", "D_N" ,"Station", "[A]", "[B]", "[C]", "[D]", "[E]", "[F]", "[G]", "[H]", "[J]", "[K]", "[L]", "[M]", "[R]" };
    public int[] GvQueryColumnNameWidth = { 50, 200, 50 ,100, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };

    public string[] GvQueryDetailColumnName = { "Line", "LESSON", "Model"};
    public int[] GvQueryDetailColumnNameWidth = { 50, 50, 150 };


    


    string customer = "";
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Log.logMessage("Page_Load Start: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));

            DBConnection = CmbDBType.ddlGetConnection();            

            customer = Master.userInfo.Customer;
            if (!this.IsPostBack)
            {
                InitPage();
                InitCondition();
            }

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        Log.logMessage("Page_Load Finally: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
    }
    private void InitPage()
    {
        this.lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();        
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    }

    private void InitCondition()
    {

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

        GetPdline();
        GetModel();
        GetStation();

        DataTable dtPno = Family.GetFamily(DBConnection);

        if (dtPno.Rows.Count > 0)
        {
            ddlFamily.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dtPno.Rows)
            {
                ddlFamily.Items.Add(new ListItem(dr["Family"].ToString().Trim(), dr["Family"].ToString().Trim()));
            }
        }

        //this.gvResult.DataSource = getNullDataTable(1);
        //this.gvResult.DataBind();
        //InitGridView();      
    }
    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Length; i++)
		{
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
		}     
    }

    private void InitGridViewDetail()
    {
        for (int i = 0; i < GvQueryDetailColumnName.Length; i++)
        {
            gvDetail.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryDetailColumnNameWidth[i]);
        }
    }


    private void InitGridViewStyleByModel() {
        for (int i = 0; i < gvResult.Rows.Count; i++)
        {
            if (double.Parse(gvResult.Rows[i].Cells[6].Text.ToString()) > 0) {                
                gvResult.Rows[i].Cells[6].CssClass = "querycell";
            }

            if (double.Parse(gvResult.Rows[i].Cells[12].Text.ToString()) > 0) {                
                gvResult.Rows[i].Cells[12].CssClass = "querycell";
            }
            gvResult.Rows[i].Cells[6].Text = string.Format("{0:P}", double.Parse(gvResult.Rows[i].Cells[6].Text));
            gvResult.Rows[i].Cells[12].Text = string.Format("{0:P}", double.Parse(gvResult.Rows[i].Cells[12].Text));
        }


        if (double.Parse(gvResult.FooterRow.Cells[6].Text.ToString()) > 0)
        {
            gvResult.FooterRow.Cells[6].CssClass = "querycell";
        }

        if (double.Parse(gvResult.FooterRow.Cells[12].Text.ToString()) > 0)
        {
            gvResult.FooterRow.Cells[12].CssClass = "querycell";
        }
        gvResult.FooterRow.Cells[6].Text = string.Format("{0:P}", double.Parse(gvResult.FooterRow.Cells[6].Text));
        gvResult.FooterRow.Cells[12].Text = string.Format("{0:P}", double.Parse(gvResult.FooterRow.Cells[12].Text));
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            foreach (string columnname in GvQueryColumnName)
            {
                newRow[columnname] = "";
            }        
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvQueryColumnName)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }  
        
        return retTable;
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
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Log.logMessage("btnQuery_Click Start: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
        try
        {            
            IList<string> Model = new List<string>();
            hidModelList.Value = hidModelList.Value.Replace("'", "");
            txtModel.Text = txtModel.Text.Replace("'", "");

            IList<string> Station = new List<string>();
            //Station.Add("40"); Station.Add("55"); Station.Add("65");
            foreach (ListItem item in lboxStation.Items)
	        {
                if (item.Selected){
                    Station.Add(item.Value);
                }        		 
	        }
            
            if (txtModel.Text != "")
            {
                Model.Add(txtModel.Text.Trim());
            }
            else
            {
                Model = hidModelList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }

            Log.logMessage("GetQueryResult Start: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
            DataTable dt1 = new DataTable();
            dt1 = ProductEfficiency.GetQueryResult(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtFromDate.Text).AddDays(1),
                                         ddlFamily.SelectedValue, Model);
            Log.logMessage("GetQueryResult End: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
            if (dt1.Rows.Count > 0)
            {
                gvResult.DataSource = dt1;
                gvResult.DataBind();
                InitGridView();
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            Log.logMessage("gvResult.DataBind() End: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
            Response.Cookies["Efficent_Station"].Value = string.Join(",", Station.ToArray());
            if (Station.Count > 0) {

                Log.logMessage("GetQueryDetail Start: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
                DataTable dt2 = ProductEfficiency.GetQueryDetail(DBConnection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtFromDate.Text).AddDays(1),
                                         ddlFamily.SelectedValue, Model, Station);
                Log.logMessage("GetQueryDetail End: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
                if (dt2.Rows.Count > 0)
                {
                    gvDetail.DataSource = dt2;
                    gvDetail.DataBind();
                    EnableBtnExcel(this, true, btnExport.ClientID);
                    InitGridViewDetail();
                    Log.logMessage("gvDetail.DataBind() End: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
                }                
                else
                {
                    gvDetail.DataSource = null;
                    gvDetail.DataBind();            
                }
            }            

        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
        Log.logMessage("btnQuery_Click End: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));
    }

    private void BindNoData()
    {
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
        EnableBtnExcel(this, false, btnExport.ClientID);
    }

    

    protected void btnExport_Click(object sender, EventArgs e)
    {
        //ToolUtility tu = new ToolUtility();
        //tu.ExportExcel(gvResult, Page.Title, Page);
        Log.logMessage("btnExport_Click Start: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));

        GridView[] grs = new GridView[] { gvResult, gvDetail };
        MemoryStream ms = ExcelTool.GridViewToExcel(grs, new string[] { "效率", "FIS" });
        this.Response.ContentType = "application/download";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=ProductEddicency.xls");
        this.Response.Clear();        
        this.Response.BinaryWrite(ms.GetBuffer());
        Log.logMessage("btnExport_Click Start2: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));

        ms.Close();
        ms.Dispose();
        Log.logMessage("btnExport_Click End: " + DateTime.Now.ToString("hh:mm:dd:ss.fff"));

    }
   
    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetModel();
    }

    protected void GetModel()
    {
        hidModelList.Value = "";
    }

    protected void GetStation()
    {        
        List<string> station = new List<string>();
        station.AddRange(EfficiencyStation.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        DataTable dtStation = QueryCommon.GetStationName(station, DBConnection);

        string[] station_selected = {};
        if(!(Request.Cookies["Efficent_Station"] == null))
        {
            station_selected = Request.Cookies["Efficent_Station"].Value.Split(',');
        }

        if (dtStation.Rows.Count > 0)
        {
            if (station_selected.Length > 0)
            {
                foreach (DataRow dr in dtStation.Rows)
                {
                    ListItem item = new ListItem(dr["Station"].ToString().Trim() + " - " + dr["Name"].ToString().Trim(), dr["Station"].ToString().Trim());
                    if (Array.IndexOf(station_selected, item.Value) > -1)
                    {
                        item.Selected = true;
                    }
                    lboxStation.Items.Add(item);
                }
            }
            else {
                foreach (DataRow dr in dtStation.Rows)
                {
                    ListItem item = new ListItem(dr["Station"].ToString().Trim() + " - " + dr["Name"].ToString().Trim(), dr["Station"].ToString().Trim());
                    item.Selected = true;
                    lboxStation.Items.Add(item);
                }
            }
        }     
    }

    protected void GetPdline()
    {

        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");

        DataTable dtPdLine = QueryCommon.GetLine(Process, customer, false, DBConnection);
        lboxPdLine.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
    }
    
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[2].Text == "d") {
                e.Row.Cells[2].Text = "白班";
            }
            else if (e.Row.Cells[2].Text == "n") {
                e.Row.Cells[2].Text = "夜班";
            }

            switch (e.Row.Cells[0].Text)
	        {
                case "1":
                    e.Row.CssClass = "lesson1";
                    break;
                case "4":
                    e.Row.CssClass = "lesson4";
                    break;
                case "2":
                    e.Row.CssClass = "lesson2";
                    break;
                case "5":
                    e.Row.CssClass = "lesson5";
                    break;
                case "3":
                    e.Row.CssClass = "lesson3";
                    break;
                case "6":
                    e.Row.CssClass = "lesson6";
                    break;                    
                case "All":
                    e.Row.CssClass = "lesson_sum";
                    break;
                default:
                    break;
	        }
            
        }
    }

    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.CssClass = "lesson" + e.Row.Cells[1].Text.ToString();
            //switch (e.Row.Cells[1].Text)
            //{
            //   case "1":
            //        e.Row.CssClass = "lesson1";
            //        break;
            //    case "4":
            //        e.Row.CssClass = "lesson4";
            //        break;
            //    case "2":
            //        e.Row.CssClass = "lesson2";
            //        break;
            //    case "5":
            //        e.Row.CssClass = "lesson5";
            //        break;
            //    case "3":
            //        e.Row.CssClass = "lesson3";
            //        break;
            //    case "6":
            //        e.Row.CssClass = "lesson6";
            //        break;
            //    default:
            //        break;
            //}

        }
    }

}
