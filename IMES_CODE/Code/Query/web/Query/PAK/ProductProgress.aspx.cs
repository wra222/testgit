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

public partial class Query_PAK_ProductProgress : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
    string DBConnection;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        DBConnection = CmbDBType.ddlGetConnection();
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

        txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        string customer = Master.userInfo.Customer;
        InitLabel();
      
      
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
         DateTime FromDate = DateTime.ParseExact(txtFromDate.Text.Trim(), "yyyy-MM-dd", null);
         DateTime ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "yyyy-MM-dd", null);
         DataTable dt = PAK_Common.GetProductProgress(DBConnection,
                                                                                      FromDate,
                                                                                      ToDate,dro.SelectedValue.Trim());
          if (dt == null || dt.Rows.Count == 0)
          {
              EnableBtnExcel(this, false, btnExcel.ClientID);
              base.BindNoData(dt,gvResult);
          }
          else
          {
              EnableBtnExcel(this, true, btnExcel.ClientID);
              gvResult.DataSource = dt;
              gvResult.DataBind();
              //Calc Total
              gvResult.FooterStyle.Font.Bold = true;
              gvResult.FooterRow.Cells[0].Text = "TOTAL";

              gvResult.FooterRow.Cells[0].Font.Size = FontUnit.Parse("16px");
              int totalIdx=7;
              if (dro.SelectedValue == "2PR")
              { totalIdx = 8; }
              int[] sum = new int[totalIdx];
              for (int i = 0; i <= dt.Rows.Count - 1; i++)
              {

                  for (int j = 0; j < totalIdx; j++)
                  {


                      sum[j] = sum[j] + int.Parse(gvResult.Rows[i].Cells[j + 2].Text);
                  }

              }
              for (int j = 0; j < totalIdx; j++)
              {
                  gvResult.FooterRow.Cells[j + 2].Text = sum[j].ToString();
                  gvResult.FooterRow.Cells[j + 2].Font.Bold = true;
                  gvResult.FooterRow.Cells[j + 2].Font.Size = FontUnit.Parse("14px");
              }
              gvResult.FooterRow.Visible = true;
              gvResult.FooterStyle.BorderColor = System.Drawing.Color.White;

 


              //Calc Total
          }
    
   
   }

  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
  
        try
       {
           BindGrv();
        }
         catch (Exception ex)
        {
            showErrorMessage(ex.Message,this.UpdatePanel1);
   
        }
         finally
        {
             endWaitingCoverDiv(this);
        }
  
      
    }
    private void InitGrvColumnHeader()
    {

        
    }
  
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ToolUtility t = new ToolUtility();
        t.ExportExcel(gvResult, "Excel", Page);
    }


}
