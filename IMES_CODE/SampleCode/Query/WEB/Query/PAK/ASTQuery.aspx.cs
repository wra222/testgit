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

public partial class Query_PAK_ASTQuery : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "Code,Range,MaxNo";
    IPAK_ASTQuery ASTQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_ASTQuery>(WebConstant.ASTQuery);
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
     

        string customer = Master.userInfo.Customer;
        //customer = "Champ";
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();
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
    }
  
   private void BindGrv()
   {
         string Connection = CmbDBType.ddlGetConnection();

         DataTable dt = ASTQuery.GetQueryResult(Connection);
          if (dt == null || dt.Rows.Count == 0)
          {
              EnableBtnExcel(this, false, btnExcel.ClientID);
              BindNoData();
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
     //  beginWaitingCoverDiv(this.UpdatePanel1);     
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
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ToolUtility t = new ToolUtility();
        t.ExportExcel(gvResult, "Excel", Page);
    }
    
}
