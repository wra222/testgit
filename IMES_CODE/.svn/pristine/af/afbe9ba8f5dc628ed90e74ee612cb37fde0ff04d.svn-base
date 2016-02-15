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

public partial class Query_PAK_WithdrawTest : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private string ColumnList = "Select,Consolidated,PalletNo,DeliveryNo,CartonQty,DeviceQty,CombinedStatus";
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
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
    public void ShowBtn(bool b)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowBtn(\"" + b.ToString() + "\");");

        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "ShowBtn", scriptBuilder.ToString(), false);
    }
    private void InitCondition()
    {
     

        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); //DateTime.Today.AddDays(7).ToString("yyyy/MM/dd");       

        InitDNTable();
        InitLabel();
        InitGridView();
      
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        lblShipDate.Text = this.GetLocalResourceObject(Pre + "_lblShipDate").ToString();
   
    }
    private void InitGridView()
    {
      //  int j = 150;
      //  int k = 80;
      //  gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(j);
       gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(60);
      //gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(j);
      // gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(j);
      //  //gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(j);
      //  gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(k);
      //  gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(k);
      //  gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(k);
      //  gvResult.HeaderRow.Cells[8].Width = Unit.Pixel(k);
    
    }
  
   private void BindGrv()
    {
          hidSelectID.Value = "";
           string Connection = CmbDBType.ddlGetConnection();
          DateTime ShipDate=DateTime.Parse(txtShipDate.Text.Trim());
          Session["WConnection"] = Connection;
         string status=dropStatus.SelectedValue;
         if(status=="ALL"){status="";}
         DataTable dt = QueryCommon.GetWithdrawTest(ShipDate, txtModel.Text.Trim(), status, null, Connection);
     
          if (dt == null || dt.Rows.Count == 0)
          {
              //BindNoData(dt, gvResult);
              //BindNoData(dt, gvResult);
              InitDNTable();
              ShowBtn(false);
            //  EnableBtnExcel(this, false, btnExcel.ClientID);
          }
          else
          {
              Session["WithdrawDT"] = dt;
              ShowBtn(true);
              gvResult.DataSource = dt;
        //      EnableBtnExcel(this, true, btnExcel.ClientID);
              gvResult.DataBind();
              InitGridView();
          
          }
    
   
   
   }
   protected void btnTest_Click(object sender, EventArgs e)
   {
      DataTable dt=(DataTable)Session["WithdrawDT"];
       string idArr=hidSelectID.Value;
      DataTable dtSelect = dt.Copy();
      dtSelect.Rows.Clear();
      int iz= dtSelect.Rows.Count;
  //    dtSelect.Rows.Add(d
      int i = dt.Rows.Count;
      dt.Rows[2].Delete();
      int ir = dt.Rows.Count;
      string s;
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
        for (int i = 0; i < 10; i++)
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
        gvResult.HeaderRow.Cells[0].Text = "Select";
        gvResult.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colBOL").ToString();

        
    }
    string lastPn = "";
    int count = 0; //#64E986   C3FDB8
    string[] bgclass = { "row1", "row2" };
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string dn = "";
        string palletNo = "";
        string status="";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
            DataRowView drv = e.Row.DataItem as DataRowView;
            dn = drv["DeliveryNo"].ToString().Trim();
            palletNo = drv["PalletNo"].ToString().Trim();
            status = drv["CombinedStatus"].ToString().Trim(); 
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
             if(status=="Empty")
             {
               e.Row.Cells[6].Text="Empty(未結合)";
               e.Row.Cells[6].BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
             }
             else if (status=="Partial")
             {
               e.Row.Cells[6].Text="Partial(部分結合)";
               e.Row.Cells[6].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF00");

             }
             else if (status=="Full")
             {
              e.Row.Cells[6].Text="Full(結合完成)";
              e.Row.Cells[6].BackColor = System.Drawing.ColorTranslator.FromHtml("#EDDA74");
             }
             else //error
             {
                 e.Row.Cells[6].BackColor = System.Drawing.ColorTranslator.FromHtml("#FF00FF");

             }
           
        }
    }
    private void BindNoData()
    {
        InitDNTable();
     
    }


 
}
