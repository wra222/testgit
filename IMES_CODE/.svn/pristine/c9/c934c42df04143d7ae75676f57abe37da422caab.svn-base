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

public partial class Query_PAK_StandModelWeight : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

   // IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);

    IPAK_StandModelWeight PAK_StandModelWeight = ServiceAgent.getInstance().GetObjectByName<IPAK_StandModelWeight>(WebConstant.IPAK_StandModelWeight);

    public string[] GvQueryColumnName = { "Model", "Family", "StandWeight" };
    public int[] GvQueryColumnNameWidth = { 150, 150, 150};
  
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
        this.lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.btnQuery.InnerText = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnExport.InnerText = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnEpExcel");
    }

    private void InitCondition()
    {
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView();      
    }
    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Length; i++)
		{
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
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
        string Connection = CmbDBType.ddlGetConnection();

        try
        {
            String Model;

            hidModelList.Value = hidModelList.Value.Replace("'", "");
            txtModel.Text = txtModel.Text.Replace("'", "");
            if (txtModel.Text != "")
            {
                Model = txtModel.Text;
            }
            else
            {
                Model = hidModelList.Value;
            }

            DataTable dt = new DataTable();
            //int sum = 0;
            dt = PAK_StandModelWeight.GetModelQueryResult(DBConnection, Model);

            if (dt.Rows.Count > 0)
            {
                gvResult.DataSource = dt;
                gvResult.DataBind();
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
        finally {
            endWaitingCoverDiv(this);
        }
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
        //MemoryStream ms = ExcelTool.GridViewToExcel(gvResult, "MO查詢");
        //this.Response.ContentType = "application/download";
        //this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        //this.Response.Clear();
        //this.Response.BinaryWrite(ms.GetBuffer());
        //ms.Close();
        //ms.Dispose();
    }
   
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}
