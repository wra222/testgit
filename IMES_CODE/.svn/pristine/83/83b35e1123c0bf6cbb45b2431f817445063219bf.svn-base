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

public partial class Query_PAK_BulkPalletReport : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    IPAK_BulkPalletReport PalletReport = ServiceAgent.getInstance().GetObjectByName<IPAK_BulkPalletReport>(WebConstant.BulkPalletReport);
    //public string PAKStation = WebCommonMethod.getConfiguration("PAKStation");
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterMPGet(this.Master);
        try
        {
            if (!this.IsPostBack)
            {
                txtShipDate.Attributes.Add("ReadOnly", "ReadOnly");
             
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
     
           
    }
   
    private void InitGridView()
    {
        
        //gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(22);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(13);
    

    }
  
   private void BindGrv()
   {
         string Connection = CmbDBType.ddlGetConnection();
         DateTime shipDate = DateTime.Parse(txtShipDate.Text.Trim());
         DataTable dt = PalletReport.GetQueryResult(Connection, shipDate, droKind.SelectedValue, PAKStation,RadioButtonList1.SelectedValue);
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
              if (droKind.SelectedValue == "Detail")
              {
                  gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(50);
                  gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(70);
              }
              else
              {
                 
                  gvResult.FooterStyle.Font.Bold = true;
                  gvResult.FooterRow.Cells[0].Text = "TOTAL";
                       

                  for (int j = 1; j <6; j++)
                  {
                      gvResult.FooterRow.Cells[j].Text = sum[j].ToString();
                      gvResult.FooterRow.Cells[j].Font.Bold = true;
                      gvResult.FooterRow.Cells[j].Font.Size = FontUnit.Parse("16px");
                      gvResult.FooterRow.Cells[j].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0000FF");
                      
                  }
                  gvResult.FooterRow.Visible = true;
                  gvResult.FooterStyle.BorderColor = System.Drawing.Color.White;
             
              }
              
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
            //BindNoData();
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
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ExcelTool.SaveToExcel(this, gvResult, "data", "及時結板報表(散裝)", false);
  
        //MemoryStream ms = ExcelTool.GridViewToExcel(gvResult, "Data");
        //this.Response.ContentType = "application/download";
        //this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        //this.Response.Clear();
        //this.Response.BinaryWrite(ms.GetBuffer());
        //ms.Close();
        //ms.Dispose();
    }
    int[] sum = new int[6]; 
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (droKind.SelectedValue == "Detail")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;" && RadioButtonList1.SelectedValue == "ALL" && droKind.SelectedValue == "Detail")
                {
                    if (e.Row.Cells[6].Text != "1")
                    {
                        e.Row.BackColor = System.Drawing.Color.Yellow;
                    }

                }
            }
        }
        else // IF droKind.SelectedValue == "Summart" need add SUM
        {
            int tmp=0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 1; i < 6; i++)
                {
                    if(int.TryParse(e.Row.Cells[i].Text,out tmp))
                    {
                        sum[i] = sum[i] + tmp;
                    }
                  
                }
            }
           
        }
    }
}
