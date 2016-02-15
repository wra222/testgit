
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using log4net;
using System.Text;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.BSamIntf;
//using IMES.FisObject;
//using IMES.FisObject.PAK.Carton;
using System.IO;
public partial class BSam_CombineCartonInDN : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICombineCartonInDN iCombineCartonInDN=  ServiceAgent.getInstance().GetObjectByName<ICombineCartonInDN>(WebConstant.ICombineCartonInDN);
    public String userId;
    public String customer;
    public String station;
    public String code;
    public string floor;
    //public String Login;
    //public String AccountId;
    //public String UserName;
    public int initRowsCount = 6;
    public int initProductTableRowsCount = 6;
    public int initDnTableRowsCount = 3;
    public const int CheckPdfTime = 10;
    public string productTableHeader = "Product ID,Custromer SN,Model,Location";
    public string dnTableHeader = "Delivery No,Model,Ship Date,Qty,Remain Qty";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(customer))
        { customer = Master.userInfo.Customer; }
        if (string.IsNullOrEmpty(userId))
        { userId = Master.userInfo.UserId; }
        station = Request["Station"] ?? "";
        if (!this.IsPostBack)
        {
           
            floor = Request["Floor"] ?? "";
            GetInputMode();
            InitProductTable();
            InitDNTable();
            this.cmbPdLine.Station = station;
            this.cmbPdLine.Customer = customer;
            this.cmbPdLine.Stage = "PAK";
            InitLabel();
            this.lbFloor2.Text = Request["Floor"] ?? "";
              
        }
    }
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lbPdLine").ToString();
        this.lbTableTitle.Text = this.GetLocalResourceObject(Pre + "_lbTableTitle").ToString();

        this.lblCartonSN.Text = this.GetLocalResourceObject(Pre + "_lblCartonSN").ToString();
        this.lblPalletNo.Text = this.GetLocalResourceObject(Pre + "_lblPalletNo").ToString();

      //  this.lbProductInfo.Text = this.GetLocalResourceObject(Pre + "_lbProductInfo").ToString();
  
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnRePrint.Value = this.GetLocalResourceObject(Pre + "_btnRePrint").ToString();
        this.lbFloor.Text = this.GetLocalResourceObject(Pre + "_lbFloor").ToString();
        this.lblFullCartonQty.Text = this.GetLocalResourceObject(Pre + "_lblFullCartonQty").ToString();
        this.lblActualCartonQty.Text = this.GetLocalResourceObject(Pre + "_lblActualCartonQty").ToString();

    }
    private void grvDN_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < grvDN.Rows.Count; i++)
        {
            if (grvDN.Rows[i].Cells[1].Text.Trim().Equals("&nbsp;")) //No data
            {
                grvDN.Rows[i].Cells[0].Controls[1].Visible = false;
            }
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
  
   
   
    
    private void clearGrid()
    {
        //try
        //{
        //    this.grvDN.DataSource = getNullDataTable();
        //    this.grvDN.DataBind();
        //    this.UpdatePanelTable.Update();
        //    initTableColumnHeader();
        //    FirstRadioCheck();
        //}
        //catch (FisException ee)
        //{
        //    writeToAlertMessage(ee.mErrmsg);
        //}
        //catch (Exception ex)
        //{
        //    writeToAlertMessage(ex.Message);
        //}
    }
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void writeToAlertMessageAndEndWait(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("endWaitingCoverDiv();ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");getCommonInputObject().focus();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void writeToInfoMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToInfoMessage", scriptBuilder.ToString(), false);
    }

   
   
    private void InitProductTable()
    {

      string[] header = productTableHeader.Split(',');
      DataTable retTable = new DataTable();
      int k = 0;
      foreach (string s in header)
      {
          retTable.Columns.Add(s, Type.GetType("System.String"));
          grvProduct.Columns[k].HeaderText = s;
          k++;
      }
      DataRow newRow;
      for (int i = 0; i < initProductTableRowsCount; i++)
      {
          newRow = retTable.NewRow();
          foreach (string s in header)
          { newRow[s] = String.Empty; }
            retTable.Rows.Add(newRow);
      }
      grvProduct.DataSource = retTable;
      grvProduct.DataBind();
    //  IniGrvWidth();
      //      this.grvDN.Columns[0].HeaderText
   
    }
    private void IniGrvWidth()
    {
        this.grvProduct.HeaderRow.Cells[0].Width = Unit.Percentage(24);
        this.grvProduct.HeaderRow.Cells[1].Width = Unit.Percentage(24);
        this.grvProduct.HeaderRow.Cells[2].Width = Unit.Percentage(24);
        this.grvProduct.HeaderRow.Cells[3].Width = Unit.Percentage(24);
        this.grvProduct.HeaderRow.Cells[4].Width = Unit.Percentage(4);
    }
    private void InitDNTable()
    {

        string[] header = dnTableHeader.Split(',');
        DataTable retTable = new DataTable();
        int k = 0;
        foreach (string s in header)
        {
            retTable.Columns.Add(s, Type.GetType("System.String"));
            grvDN.Columns[k].HeaderText = s;
            k++;
        }
        DataRow newRow;
        for (int i = 0; i < initDnTableRowsCount; i++)
        {
            newRow = retTable.NewRow();
            foreach (string s in header)
            { newRow[s] = String.Empty; }
            retTable.Rows.Add(newRow);
        }
        grvDN.DataSource = retTable;
        grvDN.DataBind();
        //      this.grvDN.Columns[0].HeaderText
    }

    private void GetInputMode()
    {
        try
        {
            hidInputMode.Value = iCombineCartonInDN.GetInputMode("BSamInputMode");
        }
        catch (FisException ex)
        {
            writeToAlertMessageAndEndWait(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessageAndEndWait(ex.Message);
        }
        
    }



    protected void grvDN_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      
     

    }
    

  
    private void EndWaitingCoverDiv()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("endWaitingCoverDiv();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "endWaitingCoverDiv", scriptBuilder.ToString(), false);
    }
    

  

    private void ResetAll()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetAll();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetAll", scriptBuilder.ToString(), false);
    }
   

    private void CallClientFun(string funcName)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine(funcName + "();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "funcName", scriptBuilder.ToString(), false);
    }
    
    protected void btnInputFirstSN_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidInput.Value.Trim().Length == 11)
            { hidInput.Value = hidInput.Value.Trim().Substring(0, 10); }
            ArrayList retArr;
            retArr = iCombineCartonInDN.InputFirstCustSn(hidInput.Value, hidLine.Value, userId, station, customer,floor);
            S_BSam_Product sProduct = (S_BSam_Product)retArr[0];

            BindPrdGrv(sProduct);
            S_BSam_Carton sCarton = (S_BSam_Carton)retArr[1];
            List<S_BSam_DN> sLstDN;
            sLstDN = (List<S_BSam_DN>)retArr[2];
            BindDnGrv(sLstDN);
            hidActualQty.Value = sCarton.ActualQty;
            hidCartonSN.Value = sCarton.CartonSN;
            hidPalletNo.Value = sCarton.PalletNo;
            hidFullQty.Value = sCarton.FullQty;
           
            CallClientFun("SetInfo");
          
        
        }
        catch (FisException ex)
        {
           writeToAlertMessageAndEndWait(ex.mErrmsg);
        }
        catch (Exception ex)
        {
    
           writeToAlertMessageAndEndWait(ex.Message);
            
        }
  
    }
    private void BindDnGrv(List<S_BSam_DN> lstDN)
    { // "Delivery NO,Model,Ship Date,Qty,Remain Qty";
        string[] header = dnTableHeader.Split(',');
        DataTable dt = new DataTable();
        int k = 0;
        foreach (string s in header)
        {
            dt.Columns.Add(s, Type.GetType("System.String"));
            grvDN.Columns[k].HeaderText = s;
            k++;
        }
        DataRow newRow;
        foreach (S_BSam_DN sDN in lstDN)
        {
            newRow = dt.NewRow();
            newRow["Delivery No"] = sDN.DeliveryNo;
            newRow["Model"] = sDN.Model;
            newRow["Ship Date"] = sDN.ShipDate;
            newRow["Qty"] = sDN.Qty;
            newRow["Remain Qty"] = sDN.RemainQty;
            dt.Rows.Add(newRow);
        }

        if (lstDN.Count < initDnTableRowsCount)
        {

            for (int i = 0; i < initDnTableRowsCount - lstDN.Count; i++)
            {
                newRow = dt.NewRow();
                newRow["Delivery No"] = "";
                newRow["Model"] = "";
                newRow["Ship Date"] = "";
                newRow["Qty"] = "";
                newRow["Remain Qty"] = "";
                dt.Rows.Add(newRow);

            }
        
        }
        grvDN.DataSource = dt;
        grvDN.DataBind();
 
    }
    private void BindPrdGrv(S_BSam_Product sProduct)
    {
        string[] header = productTableHeader.Split(',');
        DataTable dt = new DataTable();
        int k = 0;
        foreach (string s in header)
        {
            dt.Columns.Add(s, Type.GetType("System.String"));
            grvProduct.Columns[k].HeaderText = s;
            k++;
        }
        DataRow newRow ;
        newRow = dt.NewRow();
        newRow["Product ID"] = sProduct.ProductID;
        newRow["Custromer SN"] = sProduct.CustomerSN;
        newRow["Model"] = sProduct.Model;
        newRow["Location"] = sProduct.Location;
 
        dt.Rows.Add(newRow);
        for (int i = 0; i < initProductTableRowsCount - 1;i++ )
        {
            newRow = dt.NewRow();
            newRow["Product ID"] = "";
            newRow["Custromer SN"] = "";
            newRow["Model"] = "";
            newRow["Location"] = "";
            dt.Rows.Add(newRow);

        }
        grvProduct.DataSource = dt;
        grvProduct.DataBind();
     //   IniGrvWidth();
    
    
    }
}
