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

public partial class Query_PAK_PAQCUnTest : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    IPAK_PAQCUnTest PAQCUnTes = ServiceAgent.getInstance().GetObjectByName<IPAK_PAQCUnTest>(WebConstant.PAQCUnTest);
    string DBConnection;
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

        string customer = Master.userInfo.Customer;
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        //customer = "Champ";
      //  this.CmbPdLine.Customer = customer;        
        

        //this.gvResult.DataSource = getNullDataTable(ColumnList);
        //this.gvResult.DataBind();
      //  InitLabel();
        //InitGridView();
       // InitGrvColumnHeader();
      
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
    
  //       string Connection = CmbDBType.ddlGetConnection();
         DateTime shipDate;
         if (radShipDate.Checked)
         { shipDate=DateTime.ParseExact(txtShipDate.Text.Trim(), "yyyy-MM-dd", null); }
         else
         { shipDate = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null); }

         DataTable dt = PAQCUnTes.GetQueryResult(DBConnection, shipDate);
          if (dt == null || dt.Rows.Count == 0)
          {
              EnableBtnExcel(this, false, btnExcel.ClientID);
              BindNoData(dt,gvResult);
          }
          else
          {
              EnableBtnExcel(this, true, btnExcel.ClientID);
              gvResult.DataSource = dt;
              gvResult.DataBind();
              InitGrvColumnHeader();
          }
          endWaitingCoverDiv(this);
   
   
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
         //   BindNoData();
        }
         finally
        {
             endWaitingCoverDiv(this);
        }
  
      
    }
    private void InitGrvColumnHeader()
    {
        gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(60);
        
    }
    private void BindNoData()
    {
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();
        InitGridView();
        InitGrvColumnHeader();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {

        ExcelTool.SaveToExcel(this, gvResult, "data", "PAQC未完成測試報表", false, 2, 3, 8, 9, 10, 11);


        //MemoryStream ms = ExcelTool.GridViewToExcel(gvResult, "Data", 2, 3, 8, 9, 10, 11);
        //this.Response.ContentType = "application/download";
        //this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        //this.Response.Clear();
        //this.Response.BinaryWrite(ms.GetBuffer());
        //ms.Close();
        //ms.Dispose();
    }

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DateTime d81;
        DateTime d85;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          if(DateTime.TryParse(e.Row.Cells[6].Text,out d81))
          { e.Row.Cells[6].Text = d81.ToString("yyyy/MM/dd HH:mm"); }
          if (DateTime.TryParse(e.Row.Cells[7].Text, out d85))
          { e.Row.Cells[7].Text = d85.ToString("yyyy/MM/dd HH:mm"); }
        }
    }
}
