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

public partial class Query_PAK_PLTDimension : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "DeliveryNo,Model,Country,ShipWay,PLTNO,Weight,Len,Height,Width,Cdt";
    IPAK_PLTDimension PLTDimension = ServiceAgent.getInstance().GetObjectByName<IPAK_PLTDimension>(WebConstant.PLTDimension);
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
     

        //string customer = UserInfo.Customer;
        //customer = "Champ";
      //  this.CmbPdLine.Customer = customer;        
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); //DateTime.Today.AddDays(7).ToString("yyyy/MM/dd");       

      
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
        int j = 150;
        int k = 80;
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(j);
      gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
       gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(k);
        gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(k);
        gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(k);
        gvResult.HeaderRow.Cells[8].Width = Unit.Pixel(k);
        //gvResult.HeaderRow.Cells[9].Width = Unit.Pixel(j);
    //    gvResult.HeaderRow.Cells[10].Width = Unit.Pixel(j);
    }
  
   private void BindGrv()
   {
           string Connection = CmbDBType.ddlGetConnection();
          DateTime ShipDate=DateTime.Parse(txtShipDate.Text.Trim());
          string input = "";
          if (hidType.Value == "Pallet" || hidType.Value == "DN")
          { input = txtInput.Text.Trim(); }
       
          else
          { input = txtModel.Text.Trim(); }


          DataTable dt = PLTDimension.GetQueryResult(Connection, input, hidType.Value, ShipDate);
          if (dt == null || dt.Rows.Count == 0)
          {
              BindNoData(dt, gvResult);
              EnableBtnExcel(this, false, btnExcel.ClientID);
          }
          else
          {
              gvResult.DataSource = dt;
              EnableBtnExcel(this, true, btnExcel.ClientID);
              gvResult.DataBind();
              InitGridView();
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
        //ToolUtility t = new ToolUtility();
     //   t.ExportExcel(gvResult, "Excel", Page);
        ExcelTool.SaveToExcel(this, gvResult, "data", "PLTDimension", false);

    }

 
}
