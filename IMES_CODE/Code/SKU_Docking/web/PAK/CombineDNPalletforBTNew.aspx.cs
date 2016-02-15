/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine DN & Pallet for BT
 * CI-MES12-SPEC-PAK-Combine DN & Pallet for BT.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-15  itc207003              Create
 * Known issues:
 * TODO:
*/
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
public partial class PAK_CombineDNPalletforBTNew : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICombineDNPalletforBT iCombineDNPalletforBT = ServiceAgent.getInstance().GetObjectByName<ICombineDNPalletforBT>(WebConstant.CombineDNPalletforBTObject);
    public String userId;
    public String customer;
    public String station;
    public int initRowsCount =4;
    protected void Page_Load(object sender, EventArgs e)
    {
         
        userId = Master.userInfo.UserId;
        if (null == station || "" == station)
        {
            station = Request["Station"];
        }
        customer = Master.userInfo.Customer;
        if (!this.IsPostBack)
        {
            if (null == station || "" == station)
            {
                station = Request["Station"];
            }
            this.cmbPdLine.Station = station;
            this.cmbPdLine.Stage = "PAK";
            this.cmbPdLine.Customer = Master.userInfo.Customer;        
            InitLabel();
            GetDNTableData();
      
        }
    }
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lbPdLine").ToString();
        this.lbFloor.Text = this.GetLocalResourceObject(Pre + "_lbFloor").ToString();
        this.lbTableDNTitle.Text = this.GetLocalResourceObject(Pre + "_lbTableDNTitle").ToString();

        this.lbPallet.Text = this.GetLocalResourceObject(Pre + "_lbPallet").ToString();
        this.lbScanQty.Text = this.GetLocalResourceObject(Pre + "_lbScanQty").ToString();
        this.lbPalletQty.Text = this.GetLocalResourceObject(Pre + "_lbPalletQty").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
    }
    private void GetSetting()
    {
        IList<string> info = new List<string>();

        //OAEditsURL  OAEditsTemplate   OAEditsXML   OAEditsPDF   OAEditsImage   FOPFullFileName  PDFPrintPath

        info.Add("PLEditsImage");//info.Add("EditsFISAddr");
        info.Add("PLEditsURL");
        info.Add("PLEditsXML");
        info.Add("PLEditsTemplate");
        info.Add("PLEditsPDF");
        info.Add("FOPFullFileName");

        ArrayList ret = new ArrayList();
        ret = iCombineDNPalletforBT.GetSysSettingList(info);
        ReGetAllSetting(ret);
    
    }
   
  
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
      
        }
    }
  
   
    
    private void GetDNTableData()
    {
        try
        {
          
            IList<S_RowData_DN> info = new List<S_RowData_DN>();
            DataTable dt = initTable();
            DataRow newRow;

            int cnt = 0;
            newRow = dt.NewRow();
            newRow["Delivery NO"] = "------------";
            newRow["Model"] = "----------";
            newRow["Customer P/N"] = "----------";
            newRow["PoNo"] = "----------";
            newRow["Date"] = "----------------";
            newRow["Qty"] = "--";
            newRow["Packed Qty"] = "--";
            dt.Rows.Add(newRow);
            foreach (S_RowData_DN ele in info)
            {
                newRow = dt.NewRow();
                newRow["Delivery NO"] += ele.DeliveryNO;
                newRow["Model"] += ele.Model;
               
                if (null != ele.CustomerPN)
                {
                    newRow["Customer P/N"] += ele.CustomerPN;
                }
                else
                {
                    newRow["Customer P/N"] += "";
                }
         
                newRow["PoNo"] += ele.PoNo;
                newRow["Date"] += ele.Date;
                newRow["Qty"] += ele.Qty;
                newRow["Packed Qty"] += ele.PackedQty;
                dt.Rows.Add(newRow);
                cnt++;
            }

            for (; cnt < initRowsCount; cnt++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }

            this.gridViewDN.DataSource = dt;
            this.gridViewDN.DataBind();
            UpdatePanelTableDN.Update();
            initTableColumnHeader();
           // FirstRadioCheck();
            
        }
        catch (FisException ee)
        {
            clearGrid();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
  
    private void clearGrid()
    {
        try
        {
            this.gridViewDN.DataSource = getNullDataTable();
            this.gridViewDN.DataBind();
            this.UpdatePanelTableDN.Update();
            initTableColumnHeader();
        //    FirstRadioCheck();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }
    private void writeToAlertMessageAndEndWait(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("endWaitingCoverDiv();ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
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

    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Delivery NO", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Customer P/N", Type.GetType("System.String"));
        retTable.Columns.Add("PoNo", Type.GetType("System.String"));
        retTable.Columns.Add("Date", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        retTable.Columns.Add("Packed Qty", Type.GetType("System.String"));
        return retTable;
    }
    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
   
    private void initTableColumnHeader()
    {
 
        this.gridViewDN.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titleDeliveryNO").ToString();
        this.gridViewDN.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleModel").ToString();
        this.gridViewDN.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleCustomerPN").ToString();
        this.gridViewDN.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titlePoNo").ToString();
        this.gridViewDN.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titleDate").ToString();
        this.gridViewDN.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.gridViewDN.HeaderRow.Cells[6].Text = this.GetLocalResourceObject(Pre + "_titlePackedQty").ToString();
      
        this.gridViewDN.HeaderRow.Cells[0].Width = Unit.Pixel(140);
        this.gridViewDN.HeaderRow.Cells[1].Width = Unit.Pixel(110);
        this.gridViewDN.HeaderRow.Cells[2].Width = Unit.Pixel(195);
        this.gridViewDN.HeaderRow.Cells[3].Width = Unit.Pixel(160);
        this.gridViewDN.HeaderRow.Cells[4].Width = Unit.Pixel(160);
        this.gridViewDN.HeaderRow.Cells[5].Width = Unit.Pixel(50);
        this.gridViewDN.HeaderRow.Cells[6].Width = Unit.Pixel(85);
    }
 
    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
   
    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }
   

    protected void btnAssign_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string dn = "";
            if (HDN.Value == "------------")
            {
                dn = "";
            }
            else
            {
                dn = HDN.Value;
            }
            string temp = iCombineDNPalletforBT.AssignAll(HSN.Value, HLine.Value, "", HFloor.Value, userId, station, customer, dn);
            HDNTemp.Value = temp;
            string templateName = iCombineDNPalletforBT.GetTemplateName(temp);
            //string templateName = "4109041452000010";
            if (templateName != "")
            {
                ReSuccess(templateName);
            }
            else
            {
                ReSuccess("");
            }
            //HSN.Value = "";
            //ReSuccess();
        }
        catch (FisException ee)
        {
            writeToAlertMessageAndEndWait(ee.mErrmsg);
            ReResetAll();
        }
        catch (Exception ex)
        {
            writeToAlertMessageAndEndWait(ex.Message);
            ReResetAll();
        }
    }
    private void BindGrv(S_RowData_DN sDN)
    {
        DataTable dt = initTable();
        DataRow newRow;
        newRow = dt.NewRow();
        newRow["Delivery NO"] = sDN.DeliveryNO;
        newRow["Model"] = sDN.Model;
        newRow["Customer P/N"] = sDN.CustomerPN;
        newRow["PoNo"] = sDN.PoNo;
        newRow["Date"] = sDN.Date;
        newRow["Qty"] = sDN.Qty;
        newRow["Packed Qty"] = sDN.PackedQty;
        dt.Rows.Add(newRow);
        for (int i = 0; i < 4; i++) 
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        this.gridViewDN.DataSource = dt;
        this.gridViewDN.DataBind();
        initTableColumnHeader();
        UpdatePanelTableDN.Update();
     
    }
    protected void btnCheckProduct_ServerClick(object sender, System.EventArgs e)
    {
      
        try
        {
            int ret = 0;
            ret = iCombineDNPalletforBT.CheckProduct(HLine.Value, userId, station, customer, HSN.Value);

            if (-3 == ret)
            {
                HSN.Value = "";
                string msgHold = this.GetLocalResourceObject(Pre + "_msgHold").ToString();
                ReResetAll();
                writeToAlertMessageAndEndWait(msgHold);
                return;
            }
            if (-4 == ret)
            {
                HSN.Value = "";
                string msgHold = this.GetLocalResourceObject(Pre + "_msgHoldWrong").ToString();
                ReResetAll();
                writeToAlertMessage(msgHold);
                return;
            }
            if (-10 == ret)
            {
                HSN.Value = "";
                ReResetAll();
                writeToAlertMessageAndEndWait("Wrong Customer S/N");
                return;
            }
            if (-1 == ret)
            { 
                HSN.Value = "";
                ReResetAll();
                writeToAlertMessageAndEndWait("非BT 流程机器!");
                return;
            }
            if (-2 == ret)
            {
                HSN.Value = "";
                ReResetAll();
                writeToAlertMessageAndEndWait("COA not matches!");
                return;
            }
        //    string temp = iCombineDNPalletforBT.AssignAll(HSN.Value, HLine.Value, "", HFloor.Value, userId, station, customer, "");
            ArrayList arrLst = iCombineDNPalletforBT.AssignAllNew(HSN.Value, HLine.Value, "", HFloor.Value, userId, station, customer, "");
            S_RowData_DN sDN = (S_RowData_DN)arrLst[3];
            HDNTemp.Value = sDN.DeliveryNO;
            BindGrv(sDN);
            HPalletNO.Value = arrLst[0].ToString();
            HQQty.Value = arrLst[1].ToString();
            HPQty.Value = arrLst[2].ToString();
            string templateName = iCombineDNPalletforBT.GetTemplateName(sDN.DeliveryNO);
           
            if (templateName != "")
            {
                ReSuccess(templateName);
            }
            else
            {
                ReSuccess("");
            }
       
        
        }
        catch (FisException ee)
        {
            clearGrid();
            ReResetAll();
            writeToAlertMessageAndEndWait(ee.mErrmsg);
           
        }
        catch (Exception ex)
        {
            clearGrid();
            ReResetAll();
            writeToAlertMessageAndEndWait(ex.Message);
           
        }
    }
    private void ReSuccess(string name)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getSuccess(\"" + name + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReSuccess", scriptBuilder.ToString(), false);
    }

    
    private void ReResetLast()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("resetLast();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReResetLast", scriptBuilder.ToString(), false);
    }
    private void ReResetAll()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ResetAll();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReResetAll", scriptBuilder.ToString(), false);
    }
    private void ReGetAllSetting(ArrayList ret)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getAllSetting(\"" + ret + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReGetAllSetting", scriptBuilder.ToString(), false);
    }

}
