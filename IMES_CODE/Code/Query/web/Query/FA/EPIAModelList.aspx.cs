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

public partial class Query_FA_EPIAModelList : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);    
    IFA_EPIAModelList EPIAModelList = ServiceAgent.getInstance().GetObjectByName<IFA_EPIAModelList>(WebConstant.EPIAModelList);
    IMES.Query.Interface.QueryIntf.IModel Model = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IModel>(WebConstant.Model);
    List<string> StationList = new List<string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
        this.lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblFrom.Text = this.GetLocalResourceObject(Pre + "_lblFrom").ToString();
        this.lblTo.Text = this.GetLocalResourceObject(Pre + "_lblTo").ToString();        
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    }
    private void InitCondition()
    {
        string customer = Master.userInfo.Customer;        
        txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

/*        DataTable dtPno = Model.GetModel();

        if (dtPno.Rows.Count > 0)
        {
            ddlModel.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtPno.Rows)
            {
                ddlModel.Items.Add(new ListItem(dr["Model"].ToString().Trim(), dr["Model"].ToString().Trim()));
            }
        }*/

        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();
    }
    private void InitGridView()
    {
        int i = 100;
        int j = 50;
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);

        for (int r = 2; r <= StationList.Count - 1; r++)
        {
            gvResult.HeaderRow.Cells[r].Width = Unit.Pixel(i);
        }
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();            
            newRow["Model"] = "";

            foreach (string STN in StationList)
            {
                newRow[STN] = "";
            }

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();        
        retTable.Columns.Add("Model", Type.GetType("System.String"));

        foreach (string STN in StationList)
        {
            retTable.Columns.Add(STN, Type.GetType("System.String"));
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
        //beginWaitingCoverDiv();     
        string Connection = CmbDBType.ddlGetConnection();

        try
        {
            DateTime ToDate = DateTime.Now;
            string toStrDate = txtToDate.Text.Trim() + " 23:59:59.999";
            ToDate = DateTime.ParseExact(toStrDate, "yyyy-MM-dd HH:mm:ss.fff", null);

            DataTable dtStation = Station.GetStation("2");

            if (dtStation.Rows.Count > 0)
            {
                foreach (DataRow dr in dtStation.Rows)
                {
                    StationList.Add(dr["Descr"].ToString().Trim());
                }
            }

            DataTable dt = EPIAModelList.GetQueryResult(Connection, DateTime.Parse(txtFromDate.Text), ToDate,
                            txtModel.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                this.gvResult.DataSource = null;
                this.gvResult.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvResult.DataBind();

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvResult.Rows[i].Cells[0].Text = dt.Rows[i]["Model"].ToString();                    

                    for (int j = 2; j <= StationList.Count - 1; j++)
                    {
                        gvResult.Rows[i].Cells[j].Text = dt.Rows[i][j].ToString();
                    }
                }
                InitGridView();
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            else
            {
                BindNoData();

                showErrorMessage("Not Found Any Information!!");
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            BindNoData();
        }

        endWaitingCoverDiv();
    }
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
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
        tu.ExportExcel(gvResult, Page.Form.Name, Page);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}
