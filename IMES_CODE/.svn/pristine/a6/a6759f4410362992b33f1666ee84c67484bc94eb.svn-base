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

public partial class Query_PAK_UnShipSnList : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //public string PAKStation = WebCommonMethod.getConfiguration("PAKStation");
    string DBConnection;
    private string ColumnList = "";
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
 
    protected void Page_Load(object sender, EventArgs e)
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        hidConnection.Value = CmbDBType.ddlGetConnection(); ;
        hidDBName.Value = Request["DBName"] ?? configDefaultDB;
        hidExcelPath.Value = MapPath(@"~\TmpExcel\");
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
       IPAK_UnShipSnList PAKObj = ServiceAgent.getInstance().GetObjectByName<IPAK_UnShipSnList>(WebConstant.PAK_UnShipSnList);
       DateTime shipDate = DateTime.Parse(txtShipDate.Text);
       string inputModel = "";
       if (txtModel.Text.Trim() != "")
       { inputModel = txtModel.Text.Trim(); }
       else if (hidModelList.Value!= "")
       {
           inputModel = hidModelList.Value;
       }
      


       try
       {
           DataTable dt = PAKObj.GetDetail(hidConnection.Value, shipDate, inputModel);
           if (hidIsCheck.Value == "0")
           {
               gvResult.DataSource = dt;
               gvResult.DataBind();
           }
           else
           {
               dt.DefaultView.RowFilter = "UnQty>0";
               gvResult.DataSource = dt.DefaultView;
               gvResult.DataBind();
           }
      
       }
       catch (Exception ex)
       {
           showErrorMessage(ex.Message.Replace(@"""", ""), this.UpdatePanel1);
           //  BindNoData();
       }
       finally
       {
           endWaitingCoverDiv(this);
       }


       //if (dt == null || dt.Rows.Count == 0)
       //{
       //    EnableBtnExcel(this, false, btnExcel.ClientID);
       //    BindNoData(dt, gvResult);
       //}
        
   }

  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
     //   beginWaitingCoverDiv(this.UpdatePanel1);     
        try
       {
         // string[] dnArr = hidModelList.Value.Split('\n');
         
         BindGrv();
        }
         catch (Exception ex)
        {
            showErrorMessage(ex.Message,this.UpdatePanel1);
         //   BindNoData();
        }
         finally
        {
             endWaitingCoverDiv(this);
        }
  
      
    }
    private void InitGrvColumnHeader()
    {

        
    }
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].Text = "未结单Qty";
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[12].Text.Replace("&nbsp;","").Trim()== "Y")
            {
                e.Row.Cells[0].ForeColor = System.Drawing.Color.Red;
            }
            
        }
        e.Row.Cells[12].Visible = false;
    }
    private void BindNoData()
    {
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();
        InitGridView();
       // InitGrvColumnHeader();
    }
    [System.Web.Services.WebMethod]
    public static string DownExcel_WebMethod(string Connection, string shipDate, string model, string modelList,string path,bool isCheck)
    {

        string inputModel = "";
        if (model != "")
        { inputModel = model.Trim(); }
        else if (modelList.Trim() != "")
        {
            inputModel = modelList.Trim();
        }

        IPAK_UnShipSnList PAK_WipTracking = ServiceAgent.getInstance().GetObjectByName<IPAK_UnShipSnList>(WebConstant.PAK_UnShipSnList);
        DataTable[] dt2;

        dt2 = PAK_WipTracking.GetSnList(Connection, shipDate, inputModel);
      
        string fileID;
        fileID = GenUnShipSnList.GenExcelFile("UnShipSnList", path, dt2[0], dt2[1], isCheck);
       
        return fileID;

    }


}
