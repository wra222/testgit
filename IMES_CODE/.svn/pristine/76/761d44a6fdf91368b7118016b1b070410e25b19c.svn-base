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
using System.Data;
using System.IO;
using System.Configuration;

[System.Runtime.InteropServices.GuidAttribute("5C65E95A-72B7-456C-BF68-18D106E5CD1B")]
public partial class Query_PAK_IdleTime : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string defaultSelectDB = "";
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IPAK_IdleTime PAK_IdleTime = ServiceAgent.getInstance().GetObjectByName<IPAK_IdleTime>(WebConstant.IPAK_IdleTime);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    private static DBInfo objDbInfo;
    //private string defaultSelectDB;
    private string defaultSelectDBType;
    private static string _onLinesConnString;
    private static string _hisLinesConnString;
    public string[] GvQueryColumnName = { "PdLine" };
    public int[] GvQueryColumnNameWidth = { 50};
    public string[] GvDetailColumnName = {"PdLine","Model","ProductID","CUSTSN","Station","Time","TimeLen<min>" };
    public int[] GvDetailColumnNameWidth = { 30, 80, 80, 80, 30, 100, 40 };

    string customer = "";
    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
    }
    
    private void InitPage()
    {
        InitSelectDay();
        InitSelectHours();
    }

    protected void InitSelectDay()
    {
        for (int i = 0; i < 3;i++ )
        {
            lboxSelectDay.Items.Add(DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"));
        }
    }
    protected void InitSelectHours()
    {
        for (int i = 1; i < 73; i++)
        {
            lboxHours.Items.Add(i.ToString());
        }
    }

    private void InitCondition()
    {
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        this.grvDetail.DataSource = getNullDetailDataTable(1);
        this.grvDetail.DataBind();
        InitGridView();
    }

    private void InitGridView()
    {
        //gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(GvQueryColumnNameWidth[0]);
        //for (int i = 1; i < gvResult.Columns.Count; i++)
        //{
        //    gvResult.HeaderRow.Cells[i].Width = 150;
        //}
        for (int i = 0; i < GvDetailColumnNameWidth.Length; i++)
        {
            grvDetail.HeaderRow.Cells[i].Width = Unit.Pixel(GvDetailColumnNameWidth[i]);
        }
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
    private DataTable getNullDetailDataTable(int j)
    {
        DataTable dt = initDetailTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            foreach (string columnname in GvDetailColumnName)
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
    private DataTable initDetailTable()
    {
        DataTable retTable = new DataTable();
        foreach (string columnname in GvDetailColumnName)
        {
            retTable.Columns.Add(columnname, Type.GetType("System.String"));
        }
        return retTable; 
    }

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //List<int> selList = new List<int>();
        //string cmd = "";
        //string Model = "";
        //string Line = "";
        //Model = e.Row.Cells[1].Text;
        //Line = e.Row.Cells[0].Text;
        //cmd = string.Format("ShowDetail('{0}','{1}')", Model,Line);
        //e.Row.Attributes.Add("onclick", cmd);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int j = 1; j < e.Row.Cells.Count; j++)
            {
                e.Row.Cells[j].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='yellow';this.className='rowclient' ");
                e.Row.Cells[j].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                string cmd = "";
                string Line = "";
                string Station = gvResult.HeaderRow.Cells[j].Text;
                Line = e.Row.Cells[0].Text;
                cmd = string.Format("ShowDetail('{0}','{1}')", Station, Line);
                e.Row.Cells[j].Attributes.Add("onclick", cmd);
            }
        }
    }
    
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        try
        {
            string Station = hidCol.Value;
            string StationNum = Station.Split('.')[0].Trim();
            string Hours = lboxHours.SelectedValue.ToString();
            string Day =  lboxSelectDay.SelectedValue.ToString();
            string Line = hidCol2.Value;
            int min = Convert.ToInt32(Hours);
            min = min * 60;
            DataTable dt = new DataTable();

            dt = PAK_IdleTime.GetIdleTimeDetailResult(DBConnection, Day, min, StationNum, Line);
            
            if (dt.Rows.Count > 0)
            {
                grvDetail.DataSource = dt;
                grvDetail.DataBind();
                InitGridView();
                EnableBtnExcel(this, true, btnDetailExport.ClientID);
            }
            else
            {
                BindNoDetailData();
                return;
            }
        }
        catch(Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoDetailData();        
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            string SelectDay = lboxSelectDay.SelectedValue.ToString();
            string SelectHours = lboxHours.SelectedValue.ToString();
            int min = Convert.ToInt32(SelectHours);
            min = min * 60;
            DataTable dt = new DataTable();
            
            dt = PAK_IdleTime.GetIdleTimeResult(DBConnection, SelectDay, min);
            
            if (dt.Rows.Count > 0)
            {
                gvResult.DataSource = dt;
                gvResult.DataBind();
                gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(GvQueryColumnNameWidth[0]);
                for (int i = 1; i < gvResult.HeaderRow.Cells.Count; i++)
                {
                    gvResult.HeaderRow.Cells[i].Width = 150;
                }
                BindNoDetailData();
                InitGridView();
                EnableBtnExcel(this, true, btnExport.ClientID); 
            }
            else
            {
                BindNoData();
                BindNoDetailData();
                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
            BindNoDetailData();
        }
        finally
        {
            endWaitingCoverDiv(this);
        }
    }
    
    private void BindNoDetailData()
    {
        this.grvDetail.DataSource = getNullDetailDataTable(1);
        this.grvDetail.DataBind();
        InitGridView();
        EnableBtnExcel(this, false, btnDetailExport.ClientID);
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
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvResult, Page.Title, Page);
    }
    protected void btnDetailExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(grvDetail, Page.Title, Page);
    }
}
