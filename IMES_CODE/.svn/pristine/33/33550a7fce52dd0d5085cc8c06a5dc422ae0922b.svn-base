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

public partial class Query_PAK_ShipLogQuery : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    IPAK_ShipLogQuery ShipLogQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_ShipLogQuery>(WebConstant.ShipLogQuery);
    string DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        RegisterMPGet(this.Master);
        try
        {
            if (!this.IsPostBack)
            {
               InitCondition();
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg,this);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message,this);
        }

    }
    private void InitCondition()
    {

        txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        InitLabel();
        InitGridView();
        InitGrvColumnHeader();
      
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {
        
        //gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(20);
        //gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        //gvResult.HeaderRow.Cells[3].Width = Unit.Percentage(20);
        //gvResult.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[6].Width = Unit.Percentage(10);

    }
  
   private void BindGrv()

   {

       hidModelList.Value = hidModelList.Value.Replace("'", "");
       txtModel.Text = txtModel.Text.Replace("'", "");
       string[] modelArr = hidModelList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
       string modelList = "";
       if (modelArr.Length> 0)
       { 
         modelList = "'" + string.Join("','", modelArr) + "'";
       }

       if (!string.IsNullOrEmpty(txtModel.Text))
       {
         //  modelList = txtModel.Text.Trim();
           modelList = "'" + txtModel.Text.Trim() + "'";
       }
       
        // string Connection = CmbDBType.ddlGetConnection();
         DateTime FromDate = DateTime.ParseExact(txtFromDate.Text.Trim(), "yyyy-MM-dd", null); 
         DateTime ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "yyyy-MM-dd", null);
         DataTable dt = ShipLogQuery.GetQueryResult(DBConnection, modelList, FromDate, ToDate, ChxLstProductType1.GetCheckList());
          if (dt == null || dt.Rows.Count == 0)
          {
              EnableBtnExcel(this, false, btnExcel.ClientID);
              BindNoData(dt);
          }
          else
          {
              EnableBtnExcel(this, true, btnExcel.ClientID);
              gvResult.DataSource = dt;
              gvResult.DataBind();
              InitGrvColumnHeader();
          }
    
   
   
   }

  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
     //   beginWaitingCoverDiv(this.UpdatePanel1);     
        try
       {
         BindGrv();
        }
         catch (Exception ex)
        {
            showErrorMessage(ex.Message,this.UpdatePanel1);
            BindNoData();
        }
         finally
        {
             endWaitingCoverDiv(this);
        }
  
      
    }
    private void InitGrvColumnHeader()
    {

        
    }
    private void BindNoData()
    {
       // this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();
        InitGridView();
        InitGrvColumnHeader();
    }
    private void BindNoData(DataTable dt)
    {

        foreach (DataColumn dc in dt.Columns)
        {
            ColumnList = ColumnList + dc.ColumnName + ",";

        }
        ColumnList = ColumnList.Substring(0, ColumnList.Length - 1);
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ToolUtility t = new ToolUtility();
        t.ExportExcel(gvResult, "Excel", Page);
    }
    
}
