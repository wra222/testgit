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

public partial class Query_PAK_WithdrawTestResult : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "Select,Consolidated,PalletNo,DeliveryNo,CartonQty,DeviceQty,Result";
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    protected void Page_Load(object sender, EventArgs e)
    {
       // RegisterMPGet(this.Master);
        try
        {
            
              
                InitCondition();
                DataTable dt = (DataTable)Session["WithdrawDT"];
                
                string idst = Request["DNList"];
                string shipDate = Request["ShipDate"];
                string model = Request["Model"];
                string[] idArr=  idst.Split(',');

              DataTable dtSelect = dt.Clone();
             
              string dn = "";
              string pallet = "";
                string select="";
              foreach (string s in idArr)
              {
                  dn = s.Split('-')[0].Trim();
                  pallet = s.Split('-')[1].Trim();
                  select="DeliveryNo='{0}' and PalletNo='{1}'";
                  select=string.Format(select,dn,pallet);
                 DataRow[] drArr= dt.Select(select);
                 dtSelect.ImportRow(drArr[0]);
              }
              dtSelect.Columns.Add("Checked",typeof(string));
              dtSelect.Columns.Add("Result", typeof(string));
              DateTime ShipDate = DateTime.Parse(shipDate);
              DataTable dtR = QueryCommon.GetWithdrawTest(ShipDate, model, "", dtSelect, Session["WConnection"].ToString());
                gvResult.DataSource = dtR;
                gvResult.DataBind();
                InitGridView();
         
        }
        catch (Exception ex)
        {
            ShowErr(ex.Message);
        }
     
    }

   
    public void ShowErr(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("alert(\"" + msg + "\");");

        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "ShowBtn", scriptBuilder.ToString(), false);
    }
    private void InitCondition()
    {
     
        InitDNTable();
        InitLabel();
        InitGridView();
      
    }
    private void InitLabel()
    {

        //lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        //lblShipDate.Text = this.GetLocalResourceObject(Pre + "_lblShipDate").ToString();
        //lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
  
  
   private void BindGrv()
   {
        //   string Connection = CmbDBType.ddlGetConnection();
        //  DateTime ShipDate=DateTime.Parse(txtShipDate.Text.Trim());
        // string status=dropStatus.SelectedValue;
        // if(status=="ALL"){status="";}
        // DataTable dt = QueryCommon.GetWithdrawTest(ShipDate, txtModel.Text.Trim(), status, null, Connection);
     
        //  if (dt == null || dt.Rows.Count == 0)
        //  {
        //      BindNoData(dt, gvResult);
        //    //  EnableBtnExcel(this, false, btnExcel.ClientID);
        //  }
        //  else
        //  {
        //      Session["WithdrawDT"] = dt;
        //      gvResult.DataSource = dt;
        ////      EnableBtnExcel(this, true, btnExcel.ClientID);
        //      gvResult.DataBind();
        //      InitGridView();
        //  }
    
   
   
   }
   private void InitGridView()
   {
       //  int j = 150;
       //  int k = 80;
       //  gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
       gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(60);
     

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
          //  showErrorMessage(ex.Message,this.UpdatePanel1);
          //  BindNoData();
        }
         finally
        {
             //endWaitingCoverDiv(this);
        }
  
      
    }
    private void InitDNTable()
    {

        string[] header = ColumnList.Split(',');
        DataTable retTable = new DataTable();
        int k = 0;
        foreach (string s in header)
        {
            retTable.Columns.Add(s, Type.GetType("System.String"));
            gvResult.Columns[k].HeaderText = s;
            k++;
        }
        DataRow newRow;
        for (int i = 0; i < 1; i++)
        {
            newRow = retTable.NewRow();
            foreach (string s in header)
            { newRow[s] = String.Empty; }
            retTable.Rows.Add(newRow);
        }
        gvResult.DataSource = retTable;
        gvResult.DataBind();
        //      this.grvDN.Columns[0].HeaderText
    }
    private void InitGrvColumnHeader()
    {
        //gvResult.HeaderRow.Cells[0].Text = "Select";
        //gvResult.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colBOL").ToString();

        
    }
    string lastPn = "";
    int count = 0; //#64E986   C3FDB8
    string[] bgclass = { "row1", "row2" };
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string dn = "";
        string palletNo = "";
        string status = "";
        string checke = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
            DataRowView drv = e.Row.DataItem as DataRowView;
            dn = drv["DeliveryNo"].ToString().Trim();
            palletNo = drv["PalletNo"].ToString().Trim();
            checke = drv["Select"].ToString().Trim();
            status = drv["Result"].ToString().Trim();
            chkSelect.Visible = true;
            if (string.IsNullOrEmpty(dn))
            {
                chkSelect.Visible = false;
            }
            else
            {
                chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", dn + "-" + palletNo));
                chkSelect.Attributes.Add("dn", dn);
                e.Row.Cells[0].Attributes.Add("dn", dn);
            }
            if (palletNo == "") { return; }
            if (palletNo == lastPn)
            {

            }
            else
            {
                lastPn = palletNo;
                count += 1;
                // e.Row.CssClass = "row2";
            }
            e.Row.CssClass = bgclass[count % 2];
            if (status == "Conflict by Sort")
            {
              
                e.Row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
            }
            else if (status == "Conflict by Pallet")
            {
                e.Row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#800000");
            }
            else
            {
                e.Row.Cells[6].ForeColor = System.Drawing.ColorTranslator.FromHtml("#0000FF");
            
            }
            if (checke == "1")
            {
                chkSelect.Checked = true;
              //  e.Row.Cells[0].BackColor = System.Drawing.Color.Yellow;
                chkSelect.BackColor = System.Drawing.Color.Yellow;
            }
            chkSelect.Enabled = false;
            e.Row.Cells[7].Visible = false;
        }
    }
    private void BindNoData()
    {
        //this.gvResult.DataSource = getNullDataTable(ColumnList);
        //this.gvResult.DataBind();
        //InitGridView();
        //InitGrvColumnHeader();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        //ToolUtility t = new ToolUtility();
     //   t.ExportExcel(gvResult, "Excel", Page);
     //   ExcelTool.SaveToExcel(this, gvResult, "data", "PLTDimension", false);

    }

 
}
