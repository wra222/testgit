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
using log4net;

public partial class Query_PAK_PackingDailyReport : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    //public string PAKStation = WebCommonMethod.getConfiguration("PAKStation");
    //public string FAStation = WebCommonMethod.getConfiguration("FAStation");
    IPAK_PackingDailyReport PackingDailyReport = ServiceAgent.getInstance().GetObjectByName<IPAK_PackingDailyReport>(WebConstant.PackingDailyReport);
    private static readonly ILog log = LogManager.GetLogger(typeof(Query_PAK_PackingDailyReport));
    // log.Debug("******** Begin btnQuery_Click ********");   
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterMPGet(this.Master);
        try
        {
            if (!this.IsPostBack)
            {
                txtFromDate.Attributes.Add("ReadOnly", "ReadOnly");
             
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
     //   customer = "Champ";
      //  this.CmbPdLine.Customer = customer;        
        txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); //DateTime.Today.AddDays(7).ToString("yyyy/MM/dd");       

     //   this.gvResult.DataSource = getNullDataTable(ColumnList);
     //   this.gvResult.DataBind();
        InitLabel();
       // InitGridView();
       // InitGrvColumnHeader();
      
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {
        
        //gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(22);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(13);
        //gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(13);
        //gvResult.HeaderRow.Cells[3].Width = Unit.Percentage(13);
        //gvResult.HeaderRow.Cells[4].Width = Unit.Percentage(13);
        //gvResult.HeaderRow.Cells[5].Width = Unit.Percentage(13);
        //gvResult.HeaderRow.Cells[6].Width = Unit.Percentage(13);

    }
  
   private void BindGrv()
   {
         string Connection = CmbDBType.ddlGetConnection();
          DateTime FromDate=DateTime.Parse(txtFromDate.Text.Trim());
        //  log.Debug("== Begin PackingDailyReport.GetQueryResult ==");   
         
       //DataTable dt = PackingDailyReport.GetQueryResult(Connection, FromDate,PAKStation, FAStation);
         DataTable dt = PackingDailyReport.GetQueryResult(Connection, FromDate);
      
       
       //  log.Debug("== End PackingDailyReport.GetQueryResult ==");  
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


              gvResult.FooterStyle.Font.Bold = true;
              gvResult.FooterRow.Cells[0].Text = "TOTAL";
  
              gvResult.FooterRow.Cells[0].Font.Size = FontUnit.Parse("16px");
              int[] sum = new int[4];
              for (int i = 0; i <= dt.Rows.Count - 1; i++)
              {

                  for (int j = 0; j <4; j++)
                  {


                      sum[j] = sum[j] + int.Parse(gvResult.Rows[i].Cells[j+1].Text);
                  }

              }
              for (int j = 0; j <4; j++)
              {
                  gvResult.FooterRow.Cells[j+1].Text = sum[j].ToString();
                  gvResult.FooterRow.Cells[j+1].Font.Bold = true;
                  gvResult.FooterRow.Cells[j+1].Font.Size = FontUnit.Parse("14px");
              }
              gvResult.FooterRow.Visible = true;
              gvResult.FooterStyle.BorderColor = System.Drawing.Color.White;



              InitGrvColumnHeader();

           //   String script = "<script language='javascript'>" + "\r\n" +
           //"RegisterTd();" + "\r\n" +
           //"</script>";
           //   ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);

          }

       //   log.Debug("******** End btnQuery_Click for PackingDailyReport********");   
   
   }

  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
     //   beginWaitingCoverDiv(this.UpdatePanel1);     
        try
        {
            log.Debug("******** Begin btnQuery_Click for PackingDailyReport********");   
            BindGrv();
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message.Replace(@"""", ""),this.UpdatePanel1);
          //  BindNoData();
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
        ToolUtility t = new ToolUtility();
        t.ExportExcel(gvResult, "Excel", Page);
    }

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text.Trim() != "&nbsp;")
            {
            if(e.Row.Cells[5].Text.Trim() != "OK")
         { e.Row.BackColor = System.Drawing.Color.Yellow; }
            
            } 
        
        }
    }
}
