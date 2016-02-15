/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Combine COA and DN
 * CI-MES12-SPEC-PAK-Combine COA and DN.docx          
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
public partial class PAK_CombineCOAandDN : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICombineCOAandDN iCombineCOAandDN = ServiceAgent.getInstance().GetObjectByName<ICombineCOAandDN>(WebConstant.CombineCOAandDNObject);
    ICOAStatusChange iCOAStatusChange = ServiceAgent.getInstance().GetObjectByName<ICOAStatusChange>(WebConstant.COAStatusChangeObject);
    public String userId;
    public String customer;
    public String station;
    public String code;

    public String Login;
    public String AccountId;
    public String UserName;
    public int initRowsCount = 6; 
    protected void Page_Load(object sender, EventArgs e)
    {
        gridViewExt1.DataBound += new EventHandler(gridViewExt1_DataBound);
        btnGridFresh.ServerClick += new EventHandler(btnGridFresh_ServerClick);
        btnGridFreshFull.ServerClick += new EventHandler(btnGridFreshFull_ServerClick);
        //btnGetProduct.ServerClick += new EventHandler(btnGetProduct_ServerClick);
        btnCheckInput.ServerClick += new EventHandler(btnCheckInput_ServerClick);
        btnCheckCOA.ServerClick += new EventHandler(btnCheckCOA_ServerClick);
        btnGetModel.ServerClick += new EventHandler(btnGetModel_ServerClick);
        btnDisplay.ServerClick += new EventHandler(btnDisplay_ServerClick);
        btnGridFreshReset.ServerClick += new EventHandler(btnGridFreshReset_ServerClick);
        btnGridFreshSuccess.ServerClick += new EventHandler(btnGridFreshSuccess_ServerClick);
        btnFocus.ServerClick += new EventHandler(btnFocus_ServerClick);
        btnCancel.ServerClick += new EventHandler(btnCancel_ServerClick);
        userId = Master.userInfo.UserId;
        customer = Master.userInfo.Customer;
        if (null == station || "" == station)
        {
            station = Request["Station"];
        }
        if (null == AccountId || "" == AccountId)
        {
            AccountId = Request["AccountId"];
        }
        if (null == Login || "" == Login)
        {
            Login = Request["Login"];
        }
        if (null == code || "" == code)
        {
            code = Request["PCode"];
        }
        if (null == UserName || "" == UserName)
        {
            UserName = Request["UserName"];
        }
        if (!this.IsPostBack)
        {
            this.cmbPdLine.Station = station;
            this.cmbPdLine.Customer = Master.userInfo.Customer;
            this.cmbPdLine.Stage = "PAK";
            customer = Master.userInfo.Customer;
            writeToInfoMessage("Please check radio, and then input customer S/N !");
            InitLabel();
            GetTableData();
        }
    }
    private void InitLabel()
    {
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lbPdLine").ToString();
        this.lbTableTitle.Text = this.GetLocalResourceObject(Pre + "_lbTableTitle").ToString();
        this.lbCustomerSN.Text = this.GetLocalResourceObject(Pre + "_lbCustomerSN").ToString();
        this.lbProductID.Text = this.GetLocalResourceObject(Pre + "_lbProductID").ToString();
        this.lbModel.Text = this.GetLocalResourceObject(Pre + "_lbModel").ToString();
        this.chkIsBT.Text = this.GetLocalResourceObject(Pre + "_chkIsBT").ToString();
        this.lbProductInfo.Text = this.GetLocalResourceObject(Pre + "_lbProductInfo").ToString();
        this.lbCheckItem.Text = this.GetLocalResourceObject(Pre + "_lbCheckItem").ToString();
        this.lbCOA.Text = this.GetLocalResourceObject(Pre + "_lbCOA").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
        this.btnPrintSetting.Value = this.GetLocalResourceObject(Pre + "_btnPrintSetting").ToString();
        this.btnRePrint.Value = this.GetLocalResourceObject(Pre + "_btnRePrint").ToString();
    }
    private void gridViewExt1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < gridViewExt1.Rows.Count; i++)
        {
            if (gridViewExt1.Rows[i].Cells[1].Text.Trim().Equals("&nbsp;")) //No data
            {
                gridViewExt1.Rows[i].Cells[0].Controls[1].Visible = false;
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
            string Modle = drv["Model"].ToString().Trim();
            string PoNo = drv["PoNo"].ToString().Trim();
            chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id + "@#@#@" + Modle + "&!&!&" + PoNo));
        }
    }
    private void GetTableDataRetry()
    {
        try
        {
            IList<S_RowData_COAandDN> info = new List<S_RowData_COAandDN>();
            info = iCombineCOAandDN.GetDNList();
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
            foreach (S_RowData_COAandDN ele in info)
            {
                newRow = dt.NewRow();
                newRow["Delivery NO"] += ele.DeliveryNO;
                newRow["Model"] += ele.Model;
                if (null != ele.CustomerPN && ele.CustomerPN.IndexOf("/") != -1)
                {
                    string temp = ele.CustomerPN;
                    while (null != temp && temp.IndexOf("/") != -1)
                    {
                        if (temp.IndexOf("/") + 1 == temp.Length)
                        {
                            temp = "";
                        }
                        else
                        {
                            temp = temp.Substring(temp.IndexOf("/") + 1);
                        }
                    }
                    newRow["Customer P/N"] += temp;
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
            this.gridViewExt1.DataSource = dt;
            this.gridViewExt1.DataBind();
            this.UpdatePanelTable.Update();
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
    private void GetTableData()
    {
        try
        {
            IList<S_RowData_COAandDN> info = new List<S_RowData_COAandDN>();
            info = iCombineCOAandDN.GetDNList();
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
            foreach (S_RowData_COAandDN ele in info)
            {
                newRow = dt.NewRow();
                newRow["Delivery NO"] += ele.DeliveryNO;
                newRow["Model"] += ele.Model;
                if (null != ele.CustomerPN && ele.CustomerPN.IndexOf("/") != -1)
                {
                    string temp = ele.CustomerPN;
                    while (null != temp && temp.IndexOf("/") != -1)
                    {
                        if (temp.IndexOf("/") + 1 == temp.Length)
                        {
                            temp = "";
                        }
                        else
                        {
                            temp = temp.Substring(temp.IndexOf("/") + 1);
                        }
                    }
                    newRow["Customer P/N"] += temp;
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
            this.gridViewExt1.DataSource = dt;
            this.gridViewExt1.DataBind();
            this.UpdatePanelTable.Update();
            initTableColumnHeader();
            //TxtFocus();
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
    private void GetTableDataFull()
    {
        try
        {
            IList<S_RowData_COAandDN> info = new List<S_RowData_COAandDN>();
            info = iCombineCOAandDN.GetDNList();
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
            foreach (S_RowData_COAandDN ele in info)
            {
                newRow = dt.NewRow();
                newRow["Delivery NO"] += ele.DeliveryNO;
                newRow["Model"] += ele.Model;
                if (null != ele.CustomerPN && ele.CustomerPN.IndexOf("/") != -1)
                {
                    string temp = ele.CustomerPN;
                    while (null != temp && temp.IndexOf("/") != -1)
                    {
                        if (temp.IndexOf("/") + 1 == temp.Length)
                        {
                            temp = "";
                        }
                        else
                        {
                            temp = temp.Substring(temp.IndexOf("/") + 1);
                        }
                    }
                    newRow["Customer P/N"] += temp;
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
            this.gridViewExt1.DataSource = dt;
            this.gridViewExt1.DataBind();
            this.UpdatePanelTable.Update();
            initTableColumnHeader();
            //TxtFocus();
            FirstRadioCheckFull();
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

    private void btnGridFresh_ServerClick(object sender, System.EventArgs e)
    {
        GetTableData();
        string temp = HInfo.Value;
        if (temp != "")
        {
            writeToInfoMessage(temp);
            HInfo.Value = "";
            //this.updatePanelAll.Update();
        }
        else
        {
            //ReReset("");
        }
    }
    private void btnGridFreshFull_ServerClick(object sender, System.EventArgs e)
    {
        GetTableDataFull();
    }
    
    private void btnGridFreshReset_ServerClick(object sender, System.EventArgs e)
    {
        GetTableData();
        ReReset("Reset!");  
    }
    
    private void btnGridFreshSuccess_ServerClick(object sender, System.EventArgs e)
    {
        GetTableData();
        ReSuccessFresh();
    }

    private void btnFocus_ServerClick(object sender, System.EventArgs e)
    {
        TxtFocus();
    }
    
    private void clearGrid()
    {
        try
        {
            this.gridViewExt1.DataSource = getNullDataTable();
            this.gridViewExt1.DataBind();
            this.UpdatePanelTable.Update();
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
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
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
    private void initTableColumnHeader()
    {
        this.gridViewExt1.HeaderRow.Cells[0].Text = "";
        this.gridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleDeliveryNO").ToString();
        this.gridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titleModel").ToString();
        this.gridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titleCustomerPN").ToString();
        this.gridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titlePoNo").ToString();
        this.gridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleDate").ToString();
        this.gridViewExt1.HeaderRow.Cells[6].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.gridViewExt1.HeaderRow.Cells[7].Text = this.GetLocalResourceObject(Pre + "_titlePackedQty").ToString();
        this.gridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(16);
        this.gridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(104);
        this.gridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(82);
        this.gridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(136);
        this.gridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(127);
        this.gridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(86);
        this.gridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(25);
        this.gridViewExt1.HeaderRow.Cells[7].Width = Unit.Pixel(78);
    }
    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
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

    private void FirstRadioCheck()
    {
        String script = "<script language='javascript'>  FirstRadioToCheck(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "FirstRadioCheck", script, false);
    }
    private void FirstRadioCheckFull()
    {
        String script = "<script language='javascript'>  GetDNByModelFull(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "FirstRadioCheckFull", script, false);
    }
    private void TxtFocus()
    {
        String script = "<script language='javascript'>  callNextInput(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callNextInput", script, false);
    }
    public void btnGetProduct_ServerClick(object sender, System.EventArgs e)
    {
        HProduct.Value = "false";
        this.updatePanelAll.Update();
        S_RowData_Product temp = new S_RowData_Product();
        try
        {
            if (null == station)
            {
                station = "";
            }
            temp = iCombineCOAandDN.GetProduct(HLine.Value, userId, station, customer, HSN.Value);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            HFull.Value = "1";
            ResetAll();
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            HFull.Value = "1";
            ResetAll();
            return;
        }
        bool checkRet = false;
        try
        {
            checkRet = CheckCOAForUI();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            ResetAll();
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            ResetAll();
            return;
        }
        if (null == temp.ProductID || "" == temp.ProductID 
            || "statusnotfind" == temp.ProductID || "statuswrong" == temp.ProductID 
            || temp.Model == null || temp.Model == "")
        {

            string productWrong = "Wrong  customer S/N !";
            if ("statuswrong" == temp.ProductID)
            {
                productWrong = this.GetLocalResourceObject(Pre + "_msgHold").ToString();
            }
            if ("statusnotfind" == temp.ProductID)
            {
                productWrong = this.GetLocalResourceObject(Pre + "_msgHoldWrong").ToString();
            }
            ReGetProductFail(productWrong);
            return;
        }
        if (null != temp.Model)
        {
            HTxtModel.Value = temp.Model;
        }
        if (null != temp.ProductID)
        {
            HTxtProductID.Value = temp.ProductID;
        }
        HDNTemp.Value = temp.DN;
        HFACTORYPO.Value = "";
        HISWIN8.Value = "";
        if (temp.isWin8 == "true")
        {
            HISWIN8.Value = "true";
        }
        if (temp.isCDSI == "true")
        {
            HFACTORYPO.Value = temp.isFactoryPo;
            ReIsCDSI(temp.isBT);
            return;
        }
        if (null != temp.isBT)
        {
            ReIsBT(temp.isBT);
        }
    }
    private void ReModel(string model)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getModel(\"" + model + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReModel", scriptBuilder.ToString(), false);
    }
    private void ReProductID(string productID)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getProductID(\"" + productID + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReProductID", scriptBuilder.ToString(), false);
    }
    private void ReIsBT(string isBT)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getIsBT(\"" + isBT + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReIsBT", scriptBuilder.ToString(), false);
    }
    private void ReIsCDSI(string isBT)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getIsCDSI(\"" + isBT + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReIsCDSI", scriptBuilder.ToString(), false);
    }
    private bool CheckCOA()
    {
        bool re = false;
        string reStr = "";
        if (HSN.Value == "" || HDN.Value == "")
        {
            return false;
        }
        string reCheckPart = iCombineCOAandDN.CheckPart(HSN.Value);
        if (reCheckPart != "false")
        {
            //CheckPart有，还需要检查，检查通过后SaveNoCoa
            ReCOAToInput();
            ReCheckCOAShow();
            return true;
        }

        re = iCombineCOAandDN.CheckInternalCOA(HSN.Value, HDN.Value);
        if (re == true)
        {
            //CheckInternalCOA有的，不收集SaveNoCoa
            ReNoCOAToInput();
            return false;
        }
        //CheckPart没有的
        reStr = iCombineCOAandDN.CheckBOM(HSN.Value);
        if (reStr == "NotFind")
        {
            //bom中没有，不收集SaveNoCoa
            ReNoCOAToInput();
            return false;
        }
        //bom中有，并且CheckInternalCOA没有的，收集SaveCoa
        ReCOAToInput();
        ReIfCheckCOAShow(reStr);
        return true;
    }

    private bool CheckCOAForUI()
    {
        bool re = false;
        string reStr = "";
        if (HSN.Value == "")
        {
            return false;
        }
        string reCheckPart = iCombineCOAandDN.CheckPart(HSN.Value);
        if (reCheckPart != "false")
        {
            //CheckPart有，还需要检查，检查通过后SaveNoCoa
            ReNeedCOAToInput(reCheckPart);
            return true;
        }

        re = iCombineCOAandDN.CheckInternalCOA(HSN.Value, HDN.Value);
        if (re == true)
        {
            //CheckInternalCOA有的，不收集SaveNoCoa
            ReNoCOAToInput();
            return false;
        }
        //CheckPart没有的
        reStr = iCombineCOAandDN.CheckBOM(HSN.Value);
        if (reStr == "NotFind")
        {
            //bom中没有，不收集SaveNoCoa
            ReNoCOAToInput();
            return false;
        }
        ReNeedCOAToInput(reStr);
        //bom中有，并且CheckInternalCOA没有的，收集SaveCoa
        return true;
    }

    private void btnGetModel_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string mode = iCombineCOAandDN.GetModel(HDN.Value);
            ResetMode(mode);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            return;
        }
    }
    private void btnCheckCOA_ServerClick(object sender, System.EventArgs e)
    {
        bool ret = false;
        try
        {
            ret = CheckCOA();
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            return;
        }
        if (ret == false)
        {
            SaveNoCoa();
        }
    }
    private void SaveNoCoa()
    {
        try
        {
            string QCIS = "";
            string changeDN = "";
            if (HDN.Value != "" && HSN.Value != "")
            {
                if (null == station)
                {
                    station = "";
                }
                QCIS = iCombineCOAandDN.UpdateDeliveryStatus(HLine.Value, userId, station, customer, HDN.Value, HSN.Value, "");
            }
            if (QCIS.IndexOf("true") != -1)
            {
                //HQCView.Value = "true";
                //this.updatePanelAll.Update();
            }
            if (QCIS.IndexOf("#@$#") != -1)
            {
                changeDN = QCIS.Substring(QCIS.IndexOf("#@$#") + "#@$#".Length);
                HDN.Value = changeDN;
                //this.updatePanelAll.Update();
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "begin", "beginWaitingCoverDiv();ShowSuccessfulInfo(true, \"DN changes,refresh table!\");", true);
                GetTableDataRetry();
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "end", "endWaitingCoverDiv();", true);
                //HDN.Value = QCIS.Substring(QCIS.IndexOf("#@$#") + "#@$#".Length);
            }
            PrintPizzaCOO(changeDN);
        }
        catch (FisException ee)
        {
            iCombineCOAandDN.Cancel(HSN.Value);
            HInfo.Value = ee.mErrmsg;
            //this.updatePanelAll.Update();
            //writeToInfoMessage(ee.mErrmsg);
            ResetAllFailed();
            return;
        }
        catch (Exception ex)
        {
            iCombineCOAandDN.Cancel(HSN.Value);
            HInfo.Value = ex.Message;
            //this.updatePanelAll.Update();
            //writeToInfoMessage(ex.Message);
            ResetAllFailed();
            return;
        }
    }
  
    private void SaveCoa()
    {
        try
        {
            string QCIS = "";
            string changeDN = "";
            if (HDN.Value != ""&& HCoaSN.Value != ""&& HSN.Value != "")
            {
                if (null == station)
                {
                    station = "";
                }
                QCIS = iCombineCOAandDN.UpdateDeliveryStatus(HLine.Value, userId, station, customer, HDN.Value, HSN.Value, HCoaSN.Value);
            }
            if (QCIS.IndexOf("true") != -1)
            {
                //HQCView.Value = "true";
                //this.updatePanelAll.Update();
            }
            if (QCIS.IndexOf("#@$#") != -1)
            {
                changeDN = QCIS.Substring(QCIS.IndexOf("#@$#") + "#@$#".Length);
                HDN.Value = changeDN;
                //this.updatePanelAll.Update();
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "begin", "beginWaitingCoverDiv();ShowSuccessfulInfo(true, \"DN changes,refresh table!\");", true);
                GetTableDataRetry();
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "end", "endWaitingCoverDiv();", true);
                //HDN.Value = QCIS.Substring(QCIS.IndexOf("#@$#") + "#@$#".Length);
            }
            PrintPizzaCOO(changeDN);
        }
        catch (FisException ee)
        {
            //writeToAlertMessage(ee.mErrmsg);
            iCombineCOAandDN.Cancel(HSN.Value);
            HInfo.Value = ee.mErrmsg;
            //this.updatePanelAll.Update();
            //writeToInfoMessage(ee.mErrmsg);
            //ResetAll();
            ResetAllFailed();
            return;
        }
        catch (Exception ex)
        {
            //writeToAlertMessage(ex.Message);
            iCombineCOAandDN.Cancel(HSN.Value);
            HInfo.Value = ex.Message;
            //this.updatePanelAll.Update();
            //writeToInfoMessage(ex.Message);
            //ResetAll();
            ResetAllFailed();
            return;
        }
        //ReSuccess();
    }
    private void ReCOAToInput()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getIfCheckCOAFocus();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReCOAToInput", scriptBuilder.ToString(), false);
    }
    private void ReNoCOAToInput()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getNoCheckCOAFocus();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReNoCOAToInput", scriptBuilder.ToString(), false);
    }
    private void ReNeedCOAToInput(string coa)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getNeedCheckCOAFocus(\"" + coa + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReNeedCOAToInput", scriptBuilder.ToString(), false);
    }
    private void ReIfCheckCOAShow(string partNO)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getIfCheckCOAShow(\"" + partNO + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReIfCheckCOAShow", scriptBuilder.ToString(), false);
    }

    private void ReCheckCOAShow()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getCheckCOAShow();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReCheckCOAShow", scriptBuilder.ToString(), false);
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
    private void btnCheckInput_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            IList<S_RowData_COAStatus> info = iCOAStatusChange.GetCOAList(HCoaSN.Value, HCoaSN.Value);
            //使用COAStatus.COASN = @COA查询COAStatus 表没有存在记录，则表明用户刷入的COA 不存在
            if (null == info || info.Count == 0)
            {
                writeToAlertMessage("COA is not exist!Please reinput COA.");
                ReCOAToInput();
                return;
            }
            

            if (HPartNO.Value == "Check@Coa")    
            {
                int ret = iCombineCOAandDN.CheckPartCoa(HSN.Value, HCoaSN.Value);
                if (-1 == ret)
                {
                    string temp = this.GetLocalResourceObject(Pre + "_msgCoaBingWrong").ToString();
                    writeToAlertMessage(temp);
                    ResetAll();
                    return;
                }
                else if (-2 == ret)
                {
                    string temp = this.GetLocalResourceObject(Pre + "_msgCoaBindPWrong").ToString();
                    writeToAlertMessage(temp);
                    ResetAll();
                    return;
                }
                else
                {
                    foreach (S_RowData_COAStatus ele in info)
                    {
                        ReCoaECPN(ele.Pno);
                        break;
                    }
                    SaveNoCoa();
                    return;
                }
            }



            foreach (S_RowData_COAStatus ele in info)
            {
                //用户刷入的COA 如果Part No(COAStatus.IECPN) 与上文不符，则报告错误：'COA Pn is wrong! Please reinput COA.'
                if (ele.Pno != HPartNO.Value)
                {
                    writeToAlertMessage("COA Pn is wrong! Please reinput COA.");
                    ReCOAToInput();
                    return;
                }
                //用户刷入的COA 如果不是可以结合状态，需要报告错误：'Invalid COA! Please reinput COA.'
                if (ele.Status != "P1")
                {
                    writeToAlertMessage("Invalid COA! Please reinput COA.");
                    ReCOAToInput();
                    return;
                }
            }
            int re = iCombineCOAandDN.CheckPartCoa(HSN.Value, HCoaSN.Value);
            if (-2 == re)
            {
                string temp = this.GetLocalResourceObject(Pre + "_msgCoaBindPWrong").ToString();
                writeToAlertMessage(temp);
                ResetAll();
                return;
            }
            
           
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            return;
        }

        SaveCoa();
    }
    private void btnCancel_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            iCombineCOAandDN.Cancel(HSN.Value);
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            return;
        }
    }
    private void ReSuccess()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getSuccess();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReSuccess", scriptBuilder.ToString(), false);
    }
    private void ReSuccessFresh()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getSuccessFresh();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReSuccessFresh", scriptBuilder.ToString(), false);
    }
    private void ReReset(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getReset(\"" + msg + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReReset", scriptBuilder.ToString(), false);
    }
    private void ReCoaECPN(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getCoaECPN(\"" + msg + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReCoaECPN", scriptBuilder.ToString(), false);
    }
    private void ReGetProductFail(string productWrong)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getProductFail(\"" + productWrong + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReGetProductFail", scriptBuilder.ToString(), false);
    }
    private void ReGetDNFail()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getDNFail();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReGetDNFail", scriptBuilder.ToString(), false);
    }

    private void PrintPizzaCOO(string changeDN)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("PrintPizzaAndCOO(\"" + changeDN + "\");"); 
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "PrintPizzaCOO", scriptBuilder.ToString(), false);
    }

    private void ResetAll()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetAll();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetAll", scriptBuilder.ToString(), false);
    }
    private void ResetAllFailed()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getFailed();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetAllFailed", scriptBuilder.ToString(), false);
    }
    private void ResetMode(string mode)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetMode(\"" + mode + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ResetMode", scriptBuilder.ToString(), false);
    }
}
