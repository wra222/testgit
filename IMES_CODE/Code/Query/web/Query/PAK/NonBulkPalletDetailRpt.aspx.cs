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
using log4net;
public partial class Query_PAK_NonBulkPalletDetailRpt : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "DeliveryNo,Model,TotalQty,NG Qty,Plan GI Date,FWD,Consolidate,PLTNO,PLTQty,OK Qty,Weight,WH,Loc";
    IPAK_NonBulkPalletSummaryRpt PackingCombinePallet = ServiceAgent.getInstance().GetObjectByName<IPAK_NonBulkPalletSummaryRpt>(WebConstant.NonBulkPalletSummaryRpt);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
  //  public string PAKStation = WebCommonMethod.getConfiguration("PAKStation");
    public string defaultSelectDB;
    private static readonly ILog log = LogManager.GetLogger(typeof(Query_PAK_NonBulkPalletDetailRpt));
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterMPGet(this.Master);
        hidExcelPath.Value = MapPath(@"~\TmpExcel\");
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        defaultSelectDB = this.Page.Request["DBName"] != null ? Request["DBName"].ToString().Trim() : configDefaultDB;
        //ChxLstProductType1.IsHide = (defaultSelectDB.ToUpper().IndexOf("HPDOCKING") >= 0);
        ChxLstProductType1.IsHide = iConfigDB.CheckDockingDB(configDefaultDB);
    
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
        int j =150;
        int i = 100;
        //gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(i);
        //gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(j);

        //gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(j);
        //gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(j);
        for (int m = 0; m < 8; m++)
        {
            gvResult.HeaderRow.Cells[m].Width = Unit.Pixel(i);

        }


        for (int k = 8; k < 17; k++)
        {
            gvResult.HeaderRow.Cells[k].Width = Unit.Pixel(i);
        
        }
        gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(j);
        gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(j);
    }
    private void InitDetailGridView()
    {
        int j = 150;
        int i = 80;
        grvDetail.HeaderRow.Cells[0].Width = Unit.Pixel(j);
        grvDetail.HeaderRow.Cells[1].Width = Unit.Pixel(i);
        grvDetail.HeaderRow.Cells[1].Width = Unit.Pixel(j);
        grvDetail.HeaderRow.Cells[4].Width = Unit.Pixel(j);

        grvDetail.HeaderRow.Cells[6].Width = Unit.Pixel(j);
        grvDetail.HeaderRow.Cells[7].Width = Unit.Pixel(j);

    }
    private void BindGrv()
    {
        string prodType = ChxLstProductType1.GetCheckList();
        string Connection = CmbDBType.ddlGetConnection();
        DateTime ShipDate=DateTime.Parse(txtShipDate.Text.Trim());
        DateTime toDate = DateTime.Parse(txtToDate.Text.Trim());
        string prdType = iConfigDB.CheckDockingDB(defaultSelectDB) ? "" : ChxLstProductType1.GetCheckList();

        DataTable dt ;

        if (iConfigDB.CheckDockingDB(defaultSelectDB))
        {
            dt = PackingCombinePallet.GetQueryResultForDetatilMain(Connection, ShipDate,toDate,PAKStation);

        }
        else
        {
            dt = PackingCombinePallet.GetQueryResultForDetatilMain(Connection, ShipDate,toDate, PAKStation, prodType);

        }
        //log.Debug("   ***** End GetTable(NonBulkDetail) *****");
          

        if (dt == null || dt.Rows.Count == 0)
        {
            BindNoData(dt, gvResult);
            BindDetailNoData();
            EnableBtnExcel(this, false, "btnMainExcel");
        }
        else
        {

            if (Session["Main_NonBulkDetail"] == null)
            { 
                Session.Add("Main_NonBulkDetail", dt); 
            }
            else
            { 
                Session["Main_NonBulkDetail"] = dt; 
            }
            // log.Debug("   ***** Begin DataBind(NonBulkDetail) *****");

            gvResult.DataSource = dt;
            //   log.Debug("   ***** End DataBind(NonBulkDetail) *****");

            EnableBtnExcel(this, true,"btnMainExcel");
            gvResult.DataBind();
            BindDetailNoData();

            InitGridView();
            //  log.Debug("   ***** End (NonBulkDetail) *****");

        }
        grvDetail.Visible = false;
        EnableBtnExcel(this, false,"btnDetailExcel");
        UpdatePanel2.Update();


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
        //t.ExportExcel(gvResult, "Excel", Page);
        //MemoryStream ms = ExcelTool.GridViewToExcel(gvResult, "Data",2,3,8,9,10,11);
        //this.Response.ContentType = "application/download";
        //this.Response.AddHeader("Content-Disposition", "attachment; filename=file.xls");
        //this.Response.Clear();
        //this.Response.BinaryWrite(ms.GetBuffer());
        //ms.Close();
        //ms.Dispose();
        ExcelTool.SaveToExcel(this, gvResult, "data", "及時結板報表(非散裝)Detail", false, 2, 3, 8, 9, 10, 11);

    }
    protected void btnDetailExcel_Click(object sender, EventArgs e)
    {
        
      //  ToolUtility t = new ToolUtility();
     //   t.ExportExcel(grvDetail, "Excel", Page);

        ExcelTool.SaveToExcel(this, grvDetail, "data", "及時結板報表(非散裝)Detail", false, 2, 3, 8, 9, 10, 11);
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
        this.grvDetail.DataSource = getNullDataTable(ColumnList);
        this.grvDetail.DataBind();

    }
    private void BindDetailGrv()
    {
        string type = "";
        string Connection = CmbDBType.ddlGetConnection();
        DateTime ShipDate = DateTime.Parse(txtShipDate.Text.Trim());
        if (hidCol.Value.Trim().ToUpper() == "DELIVERYNO")
        {
            type = "DN";
           // ColumnList = "Dn,Shipdate,Model,Pallet,Pallet Qty,Pallet OK數量,NG";
        }
        else if (hidCol.Value.Trim().ToUpper().StartsWith("CONSOLIDATE"))
        {
            type = "Consolidated";
            if (hidSelNo.Value.Trim().Length == 10 && hidSelNo.Value.Trim().IndexOf("/")==-1)
            {
                type = "Shipment";
            }
          //  ColumnList = "Dn,Shipdate,Model,Pallet,Pallet Qty,DN OK數量,NG";
        }
        else
        {
            type = "Pallet";
           // ColumnList = "DN,ShipDate,Model,PLT,BOL,CUSTSN,Verify,ShipToDate,虛擬站板號";

        }

        DataTable dt = PackingCombinePallet.GetQueryResultForDetatilSub(Connection, ShipDate,type, hidSelNo.Value.Trim(),PAKStation,hidModel.Value);
       
        if (dt == null || dt.Rows.Count == 0)
        {
            EnableBtnExcel(this, false,"btnDetailExcel");

            BindDetailNoData();
        }
        else
        {
            EnableBtnExcel(this, true,"btnDetailExcel");
            grvDetail.Visible = true;
            grvDetail.DataSource = dt;
            grvDetail.DataBind();
            InitGridView();
            InitDetailGridView();
            if (Session["Sub_NonBulkDetail"] == null)
            { Session.Add("Sub_NonBulkDetail", dt); }
            else
            { Session["Sub_NonBulkDetail"] = dt; }
            
        }



        //grvDetail
    }
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        List<int> selList = new List<int>();
        selList.Add(0);
        selList.Add(6);
        selList.Add(7);
        string col = "";
        string selNo="";
        string cmd = "";
        string model = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            model = e.Row.Cells[1].Text.Trim();
            foreach (int i in selList)
            {
                if (e.Row.Cells[i].Text != "" && e.Row.Cells[i].Text != "&nbsp;")
                {
                    e.Row.Cells[i].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='yellow';this.className='rowclient' ");
                    e.Row.Cells[i].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                    col = gvResult.HeaderRow.Cells[i].Text;
                    selNo = e.Row.Cells[i].Text;
                    cmd = "ShowDetail('{0}','{1}','{2}')";
                    cmd = string.Format(cmd, col, selNo, model);
                    e.Row.Cells[i].Attributes.Add("onclick", cmd);
                }
            }
            if ( (e.Row.Cells[6].Text == "" || e.Row.Cells[6].Text == "&nbsp;" ) && (e.Row.Cells[0].Text != "" && e.Row.Cells[0].Text != "&nbsp;" ))
            {

                e.Row.Cells[6].Text = e.Row.Cells[0].Text.Substring(0, 10);
                e.Row.Cells[6].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='yellow';this.className='rowclient' ");
                e.Row.Cells[6].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                col = gvResult.HeaderRow.Cells[6].Text;
                selNo = e.Row.Cells[6].Text;
                cmd = "ShowDetail('{0}','{1}','{2}')";
                cmd = string.Format(cmd, col, selNo, model);
                e.Row.Cells[6].Attributes.Add("onclick", cmd);
            }
          
          
        }

    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string DownExcel_WebMethod(string item,string path)
    { 
       string key=item+"_NonBulkDetail";

        if (HttpContext.Current.Session[key] == null)
        {
            throw new Exception("請重新查詢");
        }
        DataTable dt = (DataTable)HttpContext.Current.Session[key];
       int y= HttpContext.Current.Session.Timeout;
        string fileID = ExcelTool.DataTableToExcelSaveLocal(dt, "Data", path, 2, 3, 8, 9, 10, 11);
        return fileID;

    
    }
}
