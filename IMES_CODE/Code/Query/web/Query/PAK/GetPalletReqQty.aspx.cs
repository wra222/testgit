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
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
public partial class Query_PAK_GetPalletReqQty : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
    string DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
      //  RegisterTxtBox(this.Master);
        DBConnection = CmbDBType.ddlGetConnection();
        RegisterMPGet(this.Master);
        try
        {
            if (!this.IsPostBack)
            {
                string dbName = "";
                if (Request["DBName"] != null)
                {
                    dbName = Request["DBName"].ToString().Trim();
                }
                CmbDBType.DefaultSelectDB = dbName;
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
           
        InitLabel();
 
    }
    private void InitLabel()
    {
        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {
        //gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(30);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(150);

    }
  
   private void BindGrv()
   {
         string Connection = CmbDBType.ddlGetConnection();
        
         DataTable dt =null;
         dt = PAK_Common.GetPalletReqQty(DBConnection);
          
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
        InitGridView();
        
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

        System.IO.MemoryStream ms = ExcelTool.GridViewToExcel(gvResult, "Data",3);
        this.Response.ContentType = "application/download";
        this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        this.Response.Clear();
        this.Response.BinaryWrite(ms.GetBuffer());
        ms.Close();
        ms.Dispose();
    }

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[i].Text = e.Row.Cells[i].Text.Trim();
            i = 1;
            if (e.Row.Cells[i].Text.IndexOf("藍色") >= 0)
            { e.Row.Cells[i].ForeColor = System.Drawing.Color.Blue; }
            if (e.Row.Cells[i].Text.IndexOf("綠色") >= 0)
            { e.Row.Cells[i].ForeColor = System.Drawing.Color.Green; }
        }
    }
}
