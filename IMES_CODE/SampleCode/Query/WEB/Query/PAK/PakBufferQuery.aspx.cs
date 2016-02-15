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

public partial class Query_PAK_PakBufferyQuery: IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "PalletNo,Qty,Actuall,Weight";
    IPAK_PakBufferQuery PakBufferQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_PakBufferQuery>(WebConstant.PakBufferQuery);
    static IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);

    static string DBConnection;
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

        txtFromDate.Text = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        string customer = Master.userInfo.Customer;
     
      //  this.CmbPdLine.Customer = customer;        
        

        //this.gvResult.DataSource = getNullDataTable(ColumnList);
        //this.gvResult.DataBind();
        //InitLabel();
        //InitGridView();
        //InitGrvColumnHeader();
      
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

       labHaveZeroPallet.Text = "";
         DateTime FromDate = DateTime.ParseExact(txtFromDate.Text.Trim(), "yyyy-MM-dd", null); 
         DateTime ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "yyyy-MM-dd", null);
         DataTable dt = PakBufferQuery.GetQueryResult(DBConnection, TextBox1.Text.Trim().Replace("'", ""), radList.SelectedValue, chkZero.Checked,
                                                                                FromDate,ToDate);
          if (dt == null || dt.Rows.Count == 0)
          {
              BindNoData();
              labW.Text= "0";
              labExclude.Text = "0";
              EnableBtnExcel(this, false, btnExcel.ClientID);
          }
          else
          {
              gvResult.DataSource = dt;
              gvResult.DataBind();
              labW.Text = total.ToString();
              labExclude.Text = totalExclude.ToString();
              if (HaveZeroPallet)
              {
                  labHaveZeroPallet.Text = "有棧板尚未稱重";
              }
              else
              {
                  labHaveZeroPallet.Text = "";
              
              }
              EnableBtnExcel(this, true, btnExcel.ClientID);
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
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ToolUtility t = new ToolUtility();
        t.ExportExcel(gvResult, "Excel", Page);
    }
    private void BindNoData()
    {
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();
        InitGridView();
        InitGrvColumnHeader();
    }

    float total;
    float totalExclude;
    bool HaveZeroPallet;
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        float tmp;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string plt = e.Row.Cells[0].Text.Trim();
            if (plt == "" || plt=="&nbsp;")
            { return; }
            if(float.TryParse(e.Row.Cells[3].Text.Trim(),out tmp))
           { total += tmp;
               if (tmp == 0)
               { 
                   HaveZeroPallet = true;
                   e.Row.BackColor = System.Drawing.Color.Red;
               }
            }
            tmp = 0;
            if (float.TryParse(e.Row.Cells[4].Text.Trim(), out tmp))
            { totalExclude += tmp; }
            tmp = 0;

            e.Row.Cells[0].Attributes.Add("onclick", "GetDN(this,'" + plt +"')");
            e.Row.Cells[0].CssClass = "pltClass";
       
        }
    }
    [System.Web.Services.WebMethod]

    public static string[] GetDNbyPalletNo(string palletNo)
    {
        DataTable dt = null;
        List<string> lst = new List<string>();

        string[] result;
        dt = PAK_Common.GetDnDataByPalletNo(DBConnection, palletNo);

        if (dt == null || dt.Rows.Count == 0)
        {
            result = new string[] { "" };
           // return result;
        }
        else
        {
            foreach(DataRow dr in dt.Rows)
            {
                lst.Add(dr["DeliveryNo"].ToString() + "," + dr["Model"].ToString() + "," + dr["Qty"].ToString() );
            }
            result = lst.ToArray();
        }
        return result;
    }

    //QueryCommon
}
