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

public partial class Query_PAK_BTLocQuery : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "";
    IPAK_BTLocQuery BTLocQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_BTLocQuery>(WebConstant.BTLocQuery);
    protected void Page_Load(object sender, EventArgs e)
    {
      
        RegisterMPGet(this.Master);
        try
        {
           // CmbExportExcel.GetGV = gvResult.ClientID;
            if (!this.IsPostBack)
            {
                CmbPdLine.Customer = "HP";
                 CmbPdLine.Stage = "PAK";
                 CmbPdLine.Width = "250";
                 InitCondition();
               //  CmbExportExcel.Visible = false;
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
        customer = "Champ";
      //  InitLabel();

      //  this.CmbPdLine.Customer = customer;        
        

     //   this.gvResult.DataSource = getNullDataTable(ColumnList);
      //  this.gvResult.DataBind();
      
       // InitGridView();
        //InitGrvColumnHeader();
      
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {
        gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(20);
        gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gvResult.HeaderRow.Cells[3].Width = Unit.Percentage(20);
        gvResult.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gvResult.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gvResult.HeaderRow.Cells[6].Width = Unit.Percentage(10);

    }
  
   private void BindGrv()
   {
         string Connection = CmbDBType.ddlGetConnection();
         DataTable dt = BTLocQuery.GetQueryResult(Connection, CmbPdLine.InnerDropDownList.SelectedValue, txtInput.Text.Trim(), radInputType.SelectedValue);
                                                                                   

          if (dt == null || dt.Rows.Count == 0)
          {
             // CmbExportExcel.GetGV = grvDetail.ClientID;
              BindNoData(dt);
              EnableBtnExcel(this, false, btnExcel.ClientID);
             // CmbExportExcel.Visible = false;
          }
          else
          {
              gvResult.DataSource = dt;
              gvResult.DataBind();
              InitGrvColumnHeader();
              EnableBtnExcel(this, true, btnExcel.ClientID);
           }
    
   
   
   }

  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BindDetailNoData();
       try
       {
           hidLocID.Value = "";
         
           if (radInputType.SelectedValue == "Model" && CmbPdLine.InnerDropDownList.SelectedValue == "")
           {
               gvResult.GvExtHeight = "200px";
             BindDetailNoData();
            grvDetail.Visible = true;
           }
           else
           {
               gvResult.GvExtHeight = "400px";
             grvDetail.Visible = false;

           }
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
    private void BindDetailNoData()
    {
        string colList = "LocID,CPQSNO,Model,PdLine,Cdt";
      //  this.grvDetail.DataSource = null;
        this.grvDetail.DataSource = getNullDataTable(colList);
        this.grvDetail.DataBind();
       
    }

    private void BindNoData(DataTable dt)
    {

        foreach (DataColumn dc in dt.Columns)
        {
            ColumnList = ColumnList + dc.ColumnName+",";
        
        }
        ColumnList = ColumnList.Substring(0, ColumnList.Length - 1);
        
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();
  
    }


    protected void btnShowDetail_Click(object sender, EventArgs e)
    {
        if (hidLocID.Value == "")
        {
            BindDetailNoData();
            return;
        }
        BindDetailGrv();
        endWaitingCoverDiv(this);
    }
    private void BindDetailGrv()
    {
    
        string Connection = CmbDBType.ddlGetConnection();
        DataTable dt = BTLocQuery.GetDetailResult(Connection, hidLocID.Value.Trim());
        if (dt == null || dt.Rows.Count == 0)
        {
         //   BindNoData(dt);
        }
        else
        {

            grvDetail.DataSource = dt;
            grvDetail.DataBind();
          // InitGrvColumnHeader();
        }
                                                                                   


        //grvDetail
    }


    protected void btnExcel_Click(object sender, EventArgs e)
    {
         ToolUtility t = new ToolUtility();
         t.ExportExcel(gvResult, "Excel",Page);
    }

    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string t = "alert('" + e.Row.Cells[1].Text + "')";
        e.Row.Cells[1].Attributes.Add("onclick", t);
    }
}
