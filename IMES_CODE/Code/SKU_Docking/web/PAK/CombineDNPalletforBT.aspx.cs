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
public partial class PAK_CombineDNPalletforBT : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICombineDNPalletforBT iCombineDNPalletforBT = ServiceAgent.getInstance().GetObjectByName<ICombineDNPalletforBT>(WebConstant.CombineDNPalletforBTObject);
    public String userId;
    public String customer;
    public String station;
    public int initRowsCount = 6;
    protected void Page_Load(object sender, EventArgs e)
    {
        gridViewDN.DataBound += new EventHandler(gridViewDN_DataBound);
        btnGridFresh.ServerClick += new EventHandler(btnGridFresh_ServerClick);
        btnGridFreshSuccess.ServerClick += new EventHandler(btnGridFreshSuccess_ServerClick);
        btnGetPallet.ServerClick += new EventHandler(btnGetPallet_ServerClick);
        btnGetProduct.ServerClick += new EventHandler(btnGetProduct_ServerClick);
        btnCheckProduct.ServerClick += new EventHandler(btnCheckProduct_ServerClick);
        btnClear.ServerClick += new EventHandler(btnClear_ServerClick);
        btnAssign.ServerClick += new EventHandler(btnAssign_ServerClick);
        btnDisplay.ServerClick += new EventHandler(btnDisplay_ServerClick);
        btnGetSetting.ServerClick += new EventHandler(btnGetSetting_ServerClick);
        btnFocus.ServerClick += new EventHandler(btnFocus_ServerClick);
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
            clearGridProduct();
            this.drpPalletChange.Attributes.Add("onchange", "drpOnChange()");
            //GetSetting(); 
        }
    }
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lbPdLine").ToString();
        this.lbFloor.Text = this.GetLocalResourceObject(Pre + "_lbFloor").ToString();
        this.lbTableDNTitle.Text = this.GetLocalResourceObject(Pre + "_lbTableDNTitle").ToString();
        this.lbPalletList.Text = this.GetLocalResourceObject(Pre + "_lbPalletList").ToString();
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
        /*ReImage((string)ret[0]);
        ReEditURL((string)ret[1]);
        ReXML((string )ret[2]);
        ReTemplate((string)ret[3]);
        ReEditsPDF((string)ret[4]);
        ReFOPFullFileName((string)ret[5]);*/
        /* string templateName = iCombineDNPalletforBT.GetTemplateName("4108932703000010");
         if (templateName != "")
         {
             ReDoPDF(templateName);
         }*/
    }
    private void btnFocus_ServerClick(object sender, System.EventArgs e)
    {
        TxtFocus2();
    }
    private void gridViewDN_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < gridViewDN.Rows.Count; i++)
        {
            if (gridViewDN.Rows[i].Cells[1].Text.Trim().Equals("&nbsp;")) //No data
            {
                gridViewDN.Rows[i].Cells[0].Controls[1].Visible = false;
            }
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadioButton chkSelect = e.Row.FindControl("radio") as RadioButton;
            DataRowView drv = e.Row.DataItem as DataRowView;
            string id = drv["Delivery NO"].ToString();
            chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id));
        }
    }
    private void btnGetSetting_ServerClick(object sender, System.EventArgs e)
    {
        string templateName = iCombineDNPalletforBT.GetTemplateName("4108932703000010");
        if (templateName != "")
        {
            ReDoPDF(templateName);
        }
    }
    private void btnGridFresh_ServerClick(object sender, System.EventArgs e)
    {
        GetDNTableData2();
        FirstRadioCheckAndEnd();
        //TxtFocus();
        //ReWaitEnd();
    }
    private void btnGridFreshSuccess_ServerClick(object sender, System.EventArgs e)
    {
        GetDNTableData2();
        FirstRadioCheckAndEndSuccess();
        //TxtFocus();
        //ReWaitEnd();
    }
    
    private void ReDisplay(string display)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getDisplay(\"" + display + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReDisplay", scriptBuilder.ToString(), false);
    }
    private void btnDisplay_ServerClick(object sender, System.EventArgs e)
    {
        ReDisplay(HDisplay.Value);
    }
    private void GetDNTableData()
    {
        try
        {
            IList<S_RowData_DN> info = new List<S_RowData_DN>();
            info = iCombineDNPalletforBT.GetDNList();
            if (info == null ||
                info.Count == 0)
            {
                clearGrid();
                return;
            }
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
            FirstRadioCheck();
            
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
    private void GetDNTableData2()
    {
        try
        {
            IList<S_RowData_DN> info = new List<S_RowData_DN>();
            info = iCombineDNPalletforBT.GetDNList();
            if (info == null ||
                info.Count == 0)
            {
                clearGrid();
                return;
            }
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
            FirstRadioCheck();
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
    private void clearGridProduct()
    {
        try
        {
            this.gridViewProduct.DataSource = getNullDataTableProduct();
            this.gridViewProduct.DataBind();
            this.UpdatePanelTableProduct.Update();
            initTableProductColumnHeader();
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
        retTable.Columns.Add("radio");
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
    private DataTable initTableProduct()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Product Id", Type.GetType("System.String"));
        retTable.Columns.Add("Customer S/N", Type.GetType("System.String"));
        return retTable;
    }
    private void initTableColumnHeader()
    {
        this.gridViewDN.HeaderRow.Cells[0].Text = "";
        this.gridViewDN.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleDeliveryNO").ToString();
        this.gridViewDN.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleModel").ToString();
        this.gridViewDN.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titleCustomerPN").ToString();
        this.gridViewDN.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titlePoNo").ToString();
        this.gridViewDN.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleDate").ToString();
        this.gridViewDN.HeaderRow.Cells[6].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.gridViewDN.HeaderRow.Cells[7].Text = this.GetLocalResourceObject(Pre + "_titlePackedQty").ToString();
        this.gridViewDN.HeaderRow.Cells[0].Width = Unit.Pixel(20);
        this.gridViewDN.HeaderRow.Cells[1].Width = Unit.Pixel(140);
        this.gridViewDN.HeaderRow.Cells[2].Width = Unit.Pixel(110);
        this.gridViewDN.HeaderRow.Cells[3].Width = Unit.Pixel(195);
        this.gridViewDN.HeaderRow.Cells[4].Width = Unit.Pixel(160);
        this.gridViewDN.HeaderRow.Cells[5].Width = Unit.Pixel(160);
        this.gridViewDN.HeaderRow.Cells[6].Width = Unit.Pixel(50);
        this.gridViewDN.HeaderRow.Cells[7].Width = Unit.Pixel(85);
    }
    private void initTableProductColumnHeader()
    {
        this.gridViewProduct.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titleProductId").ToString(); ;
        this.gridViewProduct.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleCustomerSN").ToString();

        this.gridViewProduct.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        this.gridViewProduct.HeaderRow.Cells[1].Width = Unit.Pixel(100);
    }
    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewDN_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }
        }
    }
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
    private DataTable getNullDataTableProduct()
    {
        DataTable dt = initTableProduct();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private void FirstRadioCheck()
    {
        String script = "<script language='javascript'>  FirstRadioToCheck(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "FirstRadioCheck", script, false);
    }
    private void FirstRadioCheckAndEnd()
    {
        String script = "<script language='javascript'>  FirstRadioToCheckAndEnd(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "FirstRadioCheckAndEnd", script, false);
    }
    private void FirstRadioCheckAndEndSuccess()
    {
        String script = "<script language='javascript'>  FirstRadioCheckAndEndSuccess(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "FirstRadioCheckAndEndSuccess", script, false);
    }
    
    private void btnGetPallet_ServerClick(object sender, System.EventArgs e)
    {
        //TxtFocus();
        clearGridProduct();
        if (HDN.Value == "------------" 
            || HDN.Value== "")
        {
            initControl(null);
            return;
        }
        IList<SelectInfoDef> lstDNChange = new List<SelectInfoDef>();
        try
        {
            lstDNChange = iCombineDNPalletforBT.GetPalletList(HDN.Value);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        if (lstDNChange != null && lstDNChange.Count != 0)
        {
            initControl(lstDNChange);
        }
        else
        {
            initControl(null);
        }
        //TxtFocus();
    }
    private void initControl(IList<SelectInfoDef> lstDNChange)
    {
        ListItem item = null;

        this.drpPalletChange.Items.Clear();
        this.drpPalletChange.Items.Add(string.Empty);

        if (lstDNChange != null)
        {
            foreach (SelectInfoDef temp in lstDNChange)
            {
                item = new ListItem(temp.Text, temp.Value);
                this.drpPalletChange.Items.Add(item);
            }
            if (drpPalletChange.Items.Count > 1)
            {
                int index = 0;
                int i = 1;
                int tempScanQty = -1;
                while (i < drpPalletChange.Items.Count)
                {
                    IList<ProductModel> lstproduct = new List<ProductModel>();
                    lstproduct = iCombineDNPalletforBT.GetProductList(drpPalletChange.Items[i].Text, HDN.Value);
                    if (lstproduct != null && lstproduct.Count > tempScanQty)
                    {
                        tempScanQty = lstproduct.Count;
                        index = i;
                    }
                    i++;
                }
                if (index > 0)
                {
                    IList<ProductModel> lstproduct = new List<ProductModel>();
                    drpPalletChange.Items[index].Selected = true;
                    lstproduct = iCombineDNPalletforBT.GetProductList(drpPalletChange.Items[index].Text, HDN.Value);
                    ProductFrech(lstproduct);
                    int count = 0;
                    count = lstproduct.Count;
                    ReScanQty(count.ToString());
                    RePQty(drpPalletChange.Items[index].Value);
                }
            }
        }
        this.UpdatePanel1.Update(); 
    }
    private void ProductFrech(IList<ProductModel> lstproduct)
    {
        DataTable dt = initTableProduct();
        DataRow newRow;
        int cnt = 0;


        foreach (ProductModel ele in lstproduct)
        {
            newRow = dt.NewRow();
            newRow["Product Id"] += ele.ProductID;
            newRow["Customer S/N"] += ele.CustSN;

            dt.Rows.Add(newRow);
            cnt++;
        }
        for (; cnt < initRowsCount; cnt++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }

        this.gridViewProduct.DataSource = dt;
        this.gridViewProduct.DataBind();
        UpdatePanelTableProduct.Update();
        initTableProductColumnHeader();
    }
    private void btnClear_ServerClick(object sender, System.EventArgs e)
    {
        clearGridProduct();
    }
    private void btnGetProduct_ServerClick(object sender, System.EventArgs e)
    {
        clearGridProduct();
        if (null == HPalletNO.Value 
            ||HPalletNO.Value == "" 
            || null == HDN.Value
            || "" == HDN.Value)
        {
            return;
        }
        IList<ProductModel> lstproduct = new List<ProductModel>();
        try
        {
            lstproduct = iCombineDNPalletforBT.GetProductList(HPalletNO.Value, HDN.Value);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        ProductFrech(lstproduct);
        int count = 0;
        count = lstproduct.Count;
        ReScanQty(count.ToString());

    }

    private void btnAssign_ServerClick(object sender, System.EventArgs e)
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
            writeToAlertMessage(ee.mErrmsg);
            ReResetAll();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            ReResetAll();
        }
    }

    private void btnCheckProduct_ServerClick(object sender, System.EventArgs e)
    {
        if (null ==  HSN.Value||
            HSN.Value == "")
        {
            return;
        }
        try
        {
            int ret = 0;
            ret = iCombineDNPalletforBT.CheckProduct(HLine.Value, userId, station, customer, HSN.Value);

            if (-3 == ret)
            {
                HSN.Value = "";
                string msgHold = this.GetLocalResourceObject(Pre + "_msgHold").ToString();
                writeToAlertMessage(msgHold);
                ReResetLast();
                return;
            }
            if (-4 == ret)
            {
                HSN.Value = "";
                string msgHold = this.GetLocalResourceObject(Pre + "_msgHoldWrong").ToString();
                writeToAlertMessage(msgHold);
                ReResetLast();
                return;
            }
            if (-10 == ret)
            {
                HSN.Value = "";
                writeToAlertMessage("Wrong Customer S/N");
                ReResetLast();
                return;
            }
            if (-1 == ret)
            { 
                HSN.Value = "";
                writeToAlertMessage("非BT 流程机器!");
                ReResetLast();
                return;
            }
            if (-2 == ret)
            {
                HSN.Value = "";
                writeToAlertMessage("COA not matches!");
                ReResetLast();
                return;
            }
            string retStr = iCombineDNPalletforBT.CheckProductAndDN(HSN.Value, HDN.Value);
            if ("" != retStr)
            {
                if (retStr.IndexOf("#@#") != -1)
                {
                    ReModelCDSI(retStr);
                }
                else
                {
                    ReModelFalse(retStr);
                }
            }
            else
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
                //string templateName ="4109041452000010";
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
        
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            ReResetAll();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            ReResetAll();
        }
    }
    private void ReScanQty(string scanQty)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getScanQty(\"" + scanQty + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReScanQty", scriptBuilder.ToString(), false);
    }
    private void RePQty(string pQty)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getPQty(\"" + pQty + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "RePQty", scriptBuilder.ToString(), false);
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
    private void ReModelFalse(string modelBySN)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getModelFalse(\"" + modelBySN + "\");"); 
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReModelFalse", scriptBuilder.ToString(), false);
    }

    private void ReModelCDSI(string modelBySN)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getModelCDSI(\"" + modelBySN + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReModelCDSI", scriptBuilder.ToString(), false);
    }
    private void ReWaitEnd()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getWaitEnd();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReWaitEnd", scriptBuilder.ToString(), false);
    }

    private void ReDoPDF(string tempName)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getPDF(\"" + tempName + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReDoPDF", scriptBuilder.ToString(), false);
    }





    private void ReXML(string xml)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getXML(\"" + xml + "\");"); 
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReXML", scriptBuilder.ToString(), false);
    }
    private void ReEditURL(string edit)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getEditURL(\"" + edit + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReEditURL", scriptBuilder.ToString(), false);
    }



    private void ReImage(string image)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getImage(\"" + image + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReImage", scriptBuilder.ToString(), false);
    }
    private void ReTemplate(string template)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getTemplate(\"" + template + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReTemplate", scriptBuilder.ToString(), false);
    }
    private void ReEditsPDF(string editsPDF)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getEditsPDF(\"" + editsPDF + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReEditsPDF", scriptBuilder.ToString(), false);
     }
    private void ReFOPFullFileName(string fop)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getFOPFullFileName(\"" + fop + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReFOPFullFileName", scriptBuilder.ToString(), false);
    }
    private void ReGetAllSetting(ArrayList ret)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getAllSetting(\"" + ret + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReGetAllSetting", scriptBuilder.ToString(), false);
    }
    private void TxtFocus()
    {
        String script = "<script language='javascript'>  callNextInput(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "TxtFocus", script, false);
    }
    private void TxtFocus2()
    {
        String script = "<script language='javascript'>  callNextInput2(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "TxtFocus2", script, false);
    }
}
