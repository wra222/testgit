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

public partial class Query_PAK_VirtualPalletInfo :IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    IPAK_VirtualPalletInfo VirtualPalletInfo = ServiceAgent.getInstance().GetObjectByName<IPAK_VirtualPalletInfo>(WebConstant.VirtualPalletInfo);
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterMPGet(this.Master);
        try
        {
            if (!this.IsPostBack)
            {
                txtFromDate.Attributes.Add("ReadOnly", "ReadOnly");
                txtToDate.Attributes.Add("ReadOnly", "ReadOnly");
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
       // customer = "Champ";
      //  this.CmbPdLine.Customer = customer;        
        txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); //DateTime.Today.AddDays(7).ToString("yyyy/MM/dd");       
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
       // this.gvResult.DataSource = getNullDataTable(ColumnList);
       // this.gvResult.DataBind();
        InitLabel();
      //  InitGridView();
        //InitGrvColumnHeader();
      
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {
        
        //gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(18);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(18);
        //gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(18);
        //gvResult.HeaderRow.Cells[3].Width = Unit.Percentage(23);
        //gvResult.HeaderRow.Cells[4].Width = Unit.Percentage(23);
    

    }
  
   private void BindGrv()
   {
         string Connection = CmbDBType.ddlGetConnection();
          DateTime FromDate=DateTime.Parse(txtFromDate.Text.Trim());  
          string toStrDate = txtToDate.Text.Trim() + " 23:59:59.999";
          DateTime  ToDate = DateTime.ParseExact(toStrDate, "yyyy-MM-dd HH:mm:ss.fff", null);
          DataTable dt = VirtualPalletInfo.GetQueryResult(Connection, FromDate, ToDate);
          if (dt == null || dt.Rows.Count == 0)
          {
              EnableBtnExcel(this, false, btnExcel.ClientID);
              BindNoData(dt, gvResult);
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

        gvResult.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_colPLT").ToString();
        gvResult.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colBOL").ToString();

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
        //ToolUtility t = new ToolUtility();
        //t.ExportExcel(gvResult, "Excel", Page);
        ExcelTool.SaveToExcel(this, gvResult, "data", "散裝虛擬棧板信息查詢", false);

    }
    
}
