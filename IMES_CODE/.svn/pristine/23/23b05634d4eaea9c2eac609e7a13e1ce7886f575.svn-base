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

public partial class Query_PAK_NonBulkPalletSummaryRpt : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "DN,FWD,Model,NG,Line";
   // public string PAKStation = WebCommonMethod.getConfiguration("PAKStation");
    IPAK_NonBulkPalletSummaryRpt PackingCombinePallet = ServiceAgent.getInstance().GetObjectByName<IPAK_NonBulkPalletSummaryRpt>(WebConstant.NonBulkPalletSummaryRpt);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);

    String DBConnection = "";
    private string _dbName;
    protected void Page_Load(object sender, EventArgs e)
    {  
        string Connection = CmbDBType.ddlGetConnection();
        DBConnection = CmbDBType.ddlGetConnection();
        RegisterMPGet(this.Master);
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        _dbName= Request["DBName"] ?? configDefaultDB;
     //   ChxLstProductType1.IsHide = string.Equals(_dbName, "HPDocking", StringComparison.CurrentCultureIgnoreCase);
        ChxLstProductType1.IsHide = (iConfigDB.CheckDockingDB(_dbName));
       
        try
        {
            if (!this.IsPostBack)
            {
                txtShipDate.Attributes.Add("ReadOnly", "ReadOnly");
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
     

        //string customer = UserInfo.Customer;
        //customer = "Champ";
      //  this.CmbPdLine.Customer = customer;        
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); //DateTime.Today.AddDays(7).ToString("yyyy/MM/dd");       
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); //DateTime.Today.AddDays(7).ToString("yyyy/MM/dd");       
        
      
        InitLabel();
      
      
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblShipDate.Text = this.GetLocalResourceObject(Pre + "_lblShipDate").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
   {
//        int j = 150;
       
//      gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
//       gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(j);
//      //gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
//      // gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
//gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(j);
//      //  gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(k);
//   gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(j);
//       gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(j);
//      //  gvResult.HeaderRow.Cells[8].Width = Unit.Pixel(k);
//        //gvResult.HeaderRow.Cells[9].Width = Unit.Pixel(j);
//    //    gvResult.HeaderRow.Cells[10].Width = Unit.Pixel(j);
    }
  
   private void BindGrv()
   {
      //    string Connection = CmbDBType.ddlGetConnection();
       string prodType = ChxLstProductType1.GetCheckList();
          DateTime ShipDate=DateTime.Parse(txtShipDate.Text.Trim());
          DateTime toShipDate = DateTime.Parse(txtToDate.Text.Trim());
         
          DataTable dt = PackingCombinePallet.GetQueryResultForSummary(DBConnection, ShipDate,toShipDate, prodType,_dbName);
          if (dt == null || dt.Rows.Count == 0)
          {
              BindNoData(dt, gvResult);
              BindDetailNoData();
              EnableBtnExcel(this, false, btnExcel.ClientID);
          }
          else
          {
              gvResult.DataSource = dt;
              EnableBtnExcel(this, true, btnExcel.ClientID);
             // ToolUtility t = new ToolUtility();
            //  string s = "";
            //  t.DataTableToXmlString(dt, ref s);
            //  hidGrv1.Value = s;
              gvResult.DataBind();
              InitGridView();
         //     grvDetail.Visible = false;
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
        ExcelTool.SaveToExcel(this, gvResult, "data", "及時結板報表(非散裝)summary", false, 3, 4, 5, 6);
        //MemoryStream ms = ExcelTool.GridViewToExcel(gvResult, "Data", 3, 4, 5, 6);
        //this.Response.ContentType = "application/download";
        //this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        //this.Response.Clear();
        //this.Response.BinaryWrite(ms.GetBuffer());
        //ms.Close();
        //ms.Dispose();
      
    }
    private void SendComplete(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);

        scriptBuilder.AppendLine("<script language='javascript'>");
        if (errorMsg != "")
        {
            scriptBuilder.AppendLine("onComplete('" + errorMsg + "');");
        }
        else
        {
            scriptBuilder.AppendLine("onComplete();");
        }
        scriptBuilder.AppendLine("</script>");

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "onComplete", scriptBuilder.ToString(), false);

    }
    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("\r\n", "<br>");
        sourceData = sourceData.Replace("\n", "<br>");
        sourceData = sourceData.Replace(@"\", @"\\");
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }
    protected void btnDetailExcel_Click(object sender, EventArgs e)
    {
        
        ToolUtility t = new ToolUtility();
      t.ExportExcel(grvDetail, "Excel", Page);
     // t.ToExcel(hidGrv1.Value);
        
    }
    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        if (hidCol.Value == "")
        {
            BindDetailNoData();
            return;
        }
        BindDetailGrv();
        endWaitingCoverDiv(this);
    }
    private void BindDetailNoData()
    {
        //string colList = "ShipDate,DN,Model,PLT,BOL,CUSTSN,Verify,ShipToDate,虛擬站板號";
        //  this.grvDetail.DataSource = null;
       // BindNoData(dt, gvResult);
        this.grvDetail.DataSource = getNullDataTable(ColumnList);
        this.grvDetail.DataBind();

    }
    private void BindDetailGrv()
    {
        string type = "";
        //string Connection = CmbDBType.ddlGetConnection();
        DateTime ShipDate = DateTime.Parse(txtShipDate.Text.Trim());
        if (hidCol.Value.Trim().ToUpper() == "DELIVERYNO")
        {
            type = "DN";
           // ColumnList = "Dn,Shipdate,Model,Pallet,Pallet Qty,Pallet OK數量,NG";
        }
        else if (hidCol.Value.Trim().ToUpper().StartsWith("CONSOLIDATE"))
        {
            type = "Consolidated";
          //  ColumnList = "Dn,Shipdate,Model,Pallet,Pallet Qty,DN OK數量,NG";
        }
        else
        {
            type = "Pallet";
           // ColumnList = "DN,ShipDate,Model,PLT,BOL,CUSTSN,Verify,ShipToDate,虛擬站板號";

        }
        DataTable dt = PackingCombinePallet.GetDetailForSummary(DBConnection,ShipDate,PAKStation,hidSelNo.Value.Trim());
       
        if (dt == null || dt.Rows.Count == 0)
        {
          

            BindDetailNoData();
        }
        else
        {
            

            grvDetail.DataSource = dt;
            grvDetail.DataBind();
            grvDetail.HeaderRow.Cells[0].Width = Unit.Pixel(180);
            //grvDetail.Visible = true;
            // InitGrvColumnHeader();
        }



        //grvDetail
    }
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       // List<int> selList = new List<int>();
       // selList.Add(0);
       // selList.Add(6);
       // selList.Add(7);
        string col = "";
        string selNo="";
        string cmd = "";
      if (e.Row.RowType == DataControlRowType.DataRow)
        {
         
                e.Row.Cells[1].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='yellow';this.className='rowclient' ");
                e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                col = gvResult.HeaderRow.Cells[1].Text;
                selNo = e.Row.Cells[1].Text;
                cmd = "ShowDetail('{0}','{1}')";
                cmd = string.Format(cmd, col, selNo);
                e.Row.Cells[1].Attributes.Add("onclick", cmd);

      }
       //}

    }
}
