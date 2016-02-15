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

public partial class Query_PAK_StatusByDeliveryNo : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    IPAK_StatusByDeliveryNo StatusByDeliveryNo = ServiceAgent.getInstance().GetObjectByName<IPAK_StatusByDeliveryNo>(WebConstant.StatusByDeliveryNo);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    protected void Page_Load(object sender, EventArgs e)
    {
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

     
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        string customer = Master.userInfo.Customer;
     
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
        //gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        //gvResult.HeaderRow.Cells[5].Width = Unit.Percentage(20);


  
    }
  
   private void BindGrv()
   {
    
         string Connection = CmbDBType.ddlGetConnection();
         DateTime ShipDate = DateTime.Parse(txtShipDate.Text.Trim());
         List<String> lst = new List<string>();
         if (rad1.Checked)
         {

             if (string.IsNullOrEmpty(txtDeliveryNo.Text.Trim()))
             {
              
                 string[] arr = hidDNList.Value.Split(',');
                 foreach (string s in arr)
                 {
                     if (!string.IsNullOrEmpty(s))
                     { lst.Add(s.Trim()); }
                 
                 }
             }
             else
             {

                 lst.Add( txtDeliveryNo.Text.Trim());
             }
         
         
         }

         if (rad1.Checked && lst.Count == 0)
         {
            
             showErrorMessage("Please input DN", this);
             return;
         }




         DataTable dt = StatusByDeliveryNo.GetQueryResult(Connection, lst,ShipDate);
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
              //gvResult.HeaderRow.Cells[0].Width = Unit.Parse("110px");
              //gvResult.HeaderRow.Cells[1].Width = Unit.Parse("110px");
              //gvResult.HeaderRow.Cells[2].Width = Unit.Parse("110px");
              //gvResult.HeaderRow.Cells[3].Width = Unit.Parse("110px");
              gvResult.HeaderRow.Cells[5].Width = Unit.Parse("140px");
               gvResult.HeaderRow.Cells[6].Width = Unit.Parse("100px");
               gvResult.HeaderRow.Cells[7].Width = Unit.Parse("80px");
        //    gvResult.HeaderRow.Cells[8].Width = Unit.Parse("40px");
            //gvResult.HeaderRow.Cells[9].Width = Unit.Parse("40px");
            //gvResult.HeaderRow.Cells[9].Width = Unit.Parse("40px");
            //gvResult.HeaderRow.Cells[10].Width = Unit.Parse("80px");
              
           //   gvResult.HeaderRow.Cells[9].Width = Unit.Parse("80px");

              InitGrvColumnHeader();
          }

     
   
   }
   public void endWaitingCoverDiv(Control ctr)
   {
       String script = "<script language='javascript'>" + "\r\n" +
           "endWaitingCoverDiv();" + "\r\n" +
           "</script>";
       ScriptManager.RegisterStartupScript(ctr, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
   }
  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
//     beginWaitingCoverDiv(this.UpdatePanel1);     
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
        this.gvResult.DataSource = getNullDataTable(ColumnList);
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

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DateTime d;
        //    if(DateTime.TryParse(e.Row.Cells[11].Text,out d))
        //    {
        //               e.Row.Cells[11].Text=d.ToString("yyyy/MM/dd HH:mm");
        //    }

        //}
    }
  
}
