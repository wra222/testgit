/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:WHPalletControl
 * CI-MES12-SPEC-PAK-W/H Pallet Control.docx          
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

using IMES.Station.Interface.CommonIntf;


public partial class PAK_WHPalletControl : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String userId;
    public String customer;
    public String station;
    public int initRowsCount = 6;
    public string SevendaysStatus;
    public string msgOT;
    public string msgRW;
    public string DateStatus1;
    public string DateStatus2;
    public string DateStatus3;
    public string DateStatusNotIN;
    public string msgNotIN;
    IWHPalletControl currentWHPallet = ServiceAgent.getInstance().GetObjectByName<IWHPalletControl>(WebConstant.WHPalletControlObject);
    protected void Page_Load(object sender, EventArgs e)
    {
        
        btnOnQueryIN.ServerClick += new EventHandler(btnOnQueryIN_ServerClick);
        btn7Days.ServerClick += new EventHandler(btn7Days_ServerClick);
        btnOnQueryNotIN.ServerClick += new EventHandler(btnOnQueryNotIN_ServerClick);
        btnTotalCount.ServerClick += new EventHandler(btnTotal_ServerClick);
        btnOnInput.ServerClick += new EventHandler(btnOnInput_ServerClick);
        btnOnRemove.ServerClick += new EventHandler(btnRemove_ServerClick);
        SevendaysStatus = this.GetLocalResourceObject(Pre + "_SevendaysStatus").ToString();
        DateStatus1 = this.GetLocalResourceObject(Pre + "_DateStatus1").ToString();
        DateStatus2 = this.GetLocalResourceObject(Pre + "_DateStatus2").ToString();
        DateStatus3 = this.GetLocalResourceObject(Pre + "_DateStatus3").ToString();
        DateStatusNotIN = this.GetLocalResourceObject(Pre + "_DateStatusNotIN").ToString();
        msgNotIN = this.GetLocalResourceObject(Pre + "_msgNotIN").ToString();
        msgOT = this.GetLocalResourceObject(Pre + "_msgOT").ToString();
        msgRW = this.GetLocalResourceObject(Pre + "_msgRW").ToString();
        userId = Master.userInfo.UserId;

        if (!this.IsPostBack)
        {
            station = Request["Station"];
            customer = Master.userInfo.Customer;
            this.TextBox1.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
            this.TextBox2.Attributes.Add("onkeydown", "onTextBox2KeyDown()");
            InitLabel();
            clearGrid();
        }
    }
    private void InitLabel()
    {
        this.lbTableTitle.Text = this.GetLocalResourceObject(Pre + "_lbTableTitle").ToString();
        this.lbDataEntryTitle.Text = this.GetLocalResourceObject(Pre + "_lbDataEntryTitle").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
        this.lbPalletNOTitle.Text = this.GetLocalResourceObject(Pre + "_lbPalletNOTitle").ToString();
        this.lbPalletNO.Text = this.GetLocalResourceObject(Pre + "_lbPalletNO").ToString();
        this.btnRemove.Value = this.GetLocalResourceObject(Pre + "_btnRemove").ToString();
        this.btnQueryIN.Value = this.GetLocalResourceObject(Pre + "_btnQueryIN").ToString();
        this.btnQueryNotIN.Value = this.GetLocalResourceObject(Pre + "_btnQueryNotIN").ToString();
        this.btnQuery7Days.Value = this.GetLocalResourceObject(Pre + "_btnQuery7Days").ToString();
        this.btnExcel.Value = this.GetLocalResourceObject(Pre + "_btnExcel").ToString();
        this.lbFrom.Text = this.GetLocalResourceObject(Pre + "_lbFrom").ToString();
        this.lbTo.Text = this.GetLocalResourceObject(Pre + "_lbTo").ToString();
        this.btnTotal.Value = this.GetLocalResourceObject(Pre + "_btnTotal").ToString();
    }
    

    private void clearGrid()
    {
        try
        {
            this.gridViewExt1.DataSource = getNullDataTable();
            this.gridViewExt1.DataBind();
            this.UpdatePanelTable.Update();
            initTableColumnHeader();
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
    private void writeToInfoSuccess(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfoSuccessWH(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToInfoSuccess", scriptBuilder.ToString(), false);

    }
    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("DeliveryNO", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("PalletNO", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        retTable.Columns.Add("Forwarder", Type.GetType("System.String"));
        retTable.Columns.Add("HAWB", Type.GetType("System.String"));
        retTable.Columns.Add("Satus", Type.GetType("System.String"));
        retTable.Columns.Add("LOC", Type.GetType("System.String"));
        return retTable;
    }
    private void initTableColumnHeader()
    {
        
        this.gridViewExt1.HeaderRow.Cells[0].Text = this.GetLocalResourceObject(Pre + "_titleDeliveryNO").ToString();
        this.gridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_titleModel").ToString();
        this.gridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(Pre + "_titlePalletNO").ToString();
        this.gridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        this.gridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(Pre + "_titleForwarder").ToString();
        this.gridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(Pre + "_titleHAWB").ToString();
        this.gridViewExt1.HeaderRow.Cells[6].Text = this.GetLocalResourceObject(Pre + "_titleSatus").ToString();
        this.gridViewExt1.HeaderRow.Cells[7].Text = this.GetLocalResourceObject(Pre + "_titleLOC").ToString();
        this.gridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(85);
        this.gridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(75);
        this.gridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(100);
        this.gridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(20);
        this.gridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(65);
        this.gridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(65);
        this.gridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(55);
        this.gridViewExt1.HeaderRow.Cells[7].Width = Unit.Pixel(65);
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
    protected int btnGetCount_ServerClick(object sender, EventArgs e)
    {
        int ret = currentWHPallet.GetPalletCount();
        return ret;
    }
    protected void btnOnQueryIN_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string dateFrom = HDateFrom.Value;
            string dateTo = HDateTo.Value;
            DateTime dateF = DateTime.Parse(dateFrom);
            DateTime dateT = DateTime.Parse(dateTo);
            //dateT = new DateTime(dateT.Year, dateT.Month, dateT.Day, 23, 59, 59);
            //dateT = dateT.AddDays(1);
            dateT = new DateTime(dateT.Year, dateT.Month, dateT.Day, 0, 0, 0);
            IList<S_Table_Data> ret = currentWHPallet.GetDateFromTo(dateF, dateT);
            int cnt = 0;
            DataTable dt = initTable();
            DataRow newRow;
            foreach (S_Table_Data ele in ret)
            {
                newRow = dt.NewRow();
                newRow["DeliveryNO"] += ele.DeliveryNO;
                newRow["Model"] += ele.Model;
                newRow["PalletNO"] += ele.PalletNO;
                newRow["Qty"] += ele.Qty;
                newRow["Forwarder"] += ele.Forwarder;
                newRow["HAWB"] += ele.HAWB;
                if (ele.Satus == "IN")
                {
                    newRow["Satus"] += SevendaysStatus;
                }
                else if (ele.Satus == "OT")
                {
                    newRow["Satus"] += msgOT;
                }
                else if (ele.Satus == "RW")
                {
                    newRow["Satus"] += msgRW;
                }
                newRow["LOC"] += ele.LOC;
                dt.Rows.Add(newRow);
                cnt++;
            }
            if (cnt == 0)
            {
                writeToAlertMessage(msgNotIN);
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
            writeToAlertMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            return;
        }
    }
    protected void btnOnQueryNotIN_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string dateFrom = HDateFrom.Value;
            string dateTo = HDateTo.Value;
            DateTime dateF = DateTime.Parse(dateFrom);
            DateTime dateT = DateTime.Parse(dateTo);
            //dateT = dateT.AddDays(1);
            dateT = new DateTime(dateT.Year, dateT.Month, dateT.Day, 0, 0, 0);
            IList<S_Table_Data> ret = currentWHPallet.GetDateFromToNotIN(dateF, dateT);
            int cnt = 0;
            DataTable dt = initTable();
            DataRow newRow;
            foreach (S_Table_Data ele in ret)
            {
                newRow = dt.NewRow();
                newRow["DeliveryNO"] += ele.DeliveryNO;
                newRow["Model"] += ele.Model;
                newRow["PalletNO"] += ele.PalletNO;
                newRow["Qty"] += ele.Qty;
                newRow["Forwarder"] += ele.Forwarder;
                newRow["HAWB"] += ele.HAWB;
                newRow["Satus"] += DateStatusNotIN;
                newRow["LOC"] += ele.LOC;
                dt.Rows.Add(newRow);
                cnt++;
            }
            if (cnt == 0)
            {
                writeToAlertMessage("No data can be found !");
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
            writeToAlertMessage(ee.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            return;
        }
    }
    
    protected void btnExcel_ServerClick(object sender, EventArgs e)
    {
        /*IList<S_Table_Data> ret = currentWHPallet.Get7Days();
        int cnt = 0;
        DataTable dt = initTable();
        DataRow newRow;
        foreach (S_Table_Data ele in ret)
        {
            newRow = dt.NewRow();
            newRow["Delivery NO"] += ele.DeliveryNO;
            newRow["Model"] += ele.Model;
            newRow["PalletNO"] += ele.PalletNO;
            newRow["Qty"] += ele.Qty;
            newRow["Forwarder"] += ele.Forwarder;
            newRow["HAWB"] += ele.HAWB;
            if (ele.Satus == "IN")
            {
                newRow["Satus"] += SevendaysStatus;
            }
            newRow["LOC"] += ele.LOC;
            dt.Rows.Add(newRow);
            cnt++;
        }
        if (cnt == 0)
        {
            writeToAlertMessage("No data can be found !");
        }
        for (; cnt < initRowsCount; cnt++)
        {
            newRow = dt.NewRow();
            dt.Rows.Add(newRow);
        }
        dt.Columns[0].ColumnName = this.GetLocalResourceObject(Pre + "_titleDeliveryNO").ToString();
        dt.Columns[1].ColumnName = this.GetLocalResourceObject(Pre + "_titleModel").ToString();
        dt.Columns[2].ColumnName = this.GetLocalResourceObject(Pre + "_titlePalletNO").ToString();
        dt.Columns[3].ColumnName = this.GetLocalResourceObject(Pre + "_titleQty").ToString();
        dt.Columns[4].ColumnName = this.GetLocalResourceObject(Pre + "_titleForwarder").ToString();
        dt.Columns[5].ColumnName = this.GetLocalResourceObject(Pre + "_titleHAWB").ToString();
        dt.Columns[6].ColumnName = this.GetLocalResourceObject(Pre + "_titleSatus").ToString();
        dt.Columns[7].ColumnName = this.GetLocalResourceObject(Pre + "_titleLOC").ToString();*/
        
        //DataTable2Excel(this.gridViewExt1.);
       
    }
    protected void btn7Days_ServerClick(object sender, EventArgs e)
    {
        try
        {
            IList<S_Table_Data> ret = currentWHPallet.Get7Days();
            int cnt = 0;
            DataTable dt = initTable();
            DataRow newRow;
            foreach (S_Table_Data ele in ret)
            {
                newRow = dt.NewRow();
                newRow["DeliveryNO"] += ele.DeliveryNO;
                newRow["Model"] += ele.Model;
                newRow["PalletNO"] += ele.PalletNO;
                newRow["Qty"] += ele.Qty;
                newRow["Forwarder"] += ele.Forwarder;
                newRow["HAWB"] += ele.HAWB;
                if (ele.Satus == "IN")
                {
                    newRow["Satus"] += SevendaysStatus;
                }
                newRow["LOC"] += ele.LOC;
                dt.Rows.Add(newRow);
                cnt++;
            }
            if (cnt == 0)
            {
                writeToAlertMessage("No data can be found !");
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
             writeToAlertMessage(ee.mErrmsg);
             return;
         }
         catch (Exception ex)
         {
             writeToAlertMessage(ex.Message);
             return;
         }
    }
    protected void btnTotal_ServerClick(object sender, EventArgs e)
    {
        int count = 0;
        try
        {
            count = currentWHPallet.GetPalletCount();
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
        ReTotal(count.ToString());

    }
    private void ReTotal(string count)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getTotal(\"" + count + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReSuccess", scriptBuilder.ToString(), false);
    }
    protected void btnOnInput_ServerClick(object sender, EventArgs e)
    {
        try
        {
            S_CheckInput_Re reCheckInput = currentWHPallet.CheckInput(HPLT.Value);
            int count = 0;
            string plt = HPLT.Value;
            if (reCheckInput.found == false)
            {
                //此板不存在！
                count = 2;
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            else
            {
                plt = reCheckInput.plt;
            }
            HPLT.Value = plt;
            //此板已刷入，不需再刷！
            bool reCheckIn = currentWHPallet.CheckIn(plt);
            if (reCheckIn == true)
            {
                count = 3;
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            //此板已出貨，不能再刷入！
            bool reCheckOut = currentWHPallet.CheckOut(plt);
            if (reCheckOut == true)
            {
                count = 4;
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            //WC not in ('DT', 'RW')栈板状态错误!
            bool reCheckDT = currentWHPallet.CheckDT(plt);
            if (reCheckDT == false)
            {
                count = 5;
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            int reNonAssign = currentWHPallet.NonAssignDelivery(plt, userId);
            if (reNonAssign == 1)
            {
                //小棧板，請自行管控庫位
                count = 11;
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            else if (reNonAssign == 2)
            {
                //FDE散裝虛擬板，請自行管控庫位
                count = 12;
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            else if (reNonAssign == 3)
            {
                //非整機棧板，請自行管控庫位
                count = 13;
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            else if (reNonAssign == -1)
            {
                count = 14;
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }

            S_common_ret reOrderPlt = currentWHPallet.OrderPltType(plt);

            S_common_ret reAssignBol =  currentWHPallet.AssignBol(plt, userId);
            if (reAssignBol.state == 201)
            {
                count = 31;
                //此貨代庫位已滿,請自行處理擺放
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            else if (reAssignBol.state == 3)
            {
                count = 31;
                //此貨代庫位已滿,請自行處理擺放
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            else if (reAssignBol.state == 103)
            {
                string temp = "";
                if (Pre == "cn")
                {
                    temp = "請將此棧板放入" + reAssignBol.describe + " 庫位";
                    temp =  temp.Replace("#@#", " 區");

                }
                else if (Pre.IndexOf("en") != -1)
                {
                    temp = "Please, put the pallet in " + reAssignBol.describe + " Lib";
                    temp = temp.Replace("#@#", " Zone, ");
                }
                writeToInfoSuccess(temp);
                ReResetSN();
                return;
            }
            else if (reAssignBol.state == -4)
            {
                count = 15;
                //NO WhPltType data
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            //currentWHPallet.UpdatePalletWH(plt, userId, "", station, customer);
            S_common_ret reAssignPallet = currentWHPallet.AssignPallet(plt, userId);
            if (reAssignPallet.state == 2 || reAssignPallet.state == 3)
            {
               // count = 41;
                count = 31;
                //此貨代庫位已滿,請自行處理擺放
                ReDisplay(count.ToString());
                ReResetSN();
                return;
            }
            else if (reAssignPallet.state == 101)
            {
                
                //請將此棧板放入'+@col+'區'+@loc+'庫位
                string temp = "";
                if (Pre == "cn")
                {
                    temp = "請將此棧板放入" + reAssignPallet.describe + " 庫位";
                    temp = temp.Replace("#@#", " 區");

                }
                else if (Pre.IndexOf("en") != -1)
                {
                    temp = "Please, put the pallet in " + reAssignPallet.describe + " Lib";
                    temp = temp.Replace("#@#", " Zone, ");
                }
               
                writeToInfoSuccess(temp);
                ReResetSN();
                return;
            }
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
            ReResetSN();
            return;
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
            ReResetSN();
            return;
        }
    }
    private void ReDisplay(string count)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getDisplay(\"" + count + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReDisplay", scriptBuilder.ToString(), false);
    }

    private static void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.UI.WebControls.DataGrid dgExport = null;
        // 当前对话
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dtData != null)
        {
            //            <bug>
            //            BUG NO:ITC-1103-0261 
            //            REASON:中文编码
            //            </bug>
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            curContext.Response.Charset = "UTF-8";
            curContext.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=gb2312 >");
            // 导出excel文件
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);
            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dtData.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            for (int i = 0; i < dgExport.Items.Count; i++)
            {
                dgExport.Items[i].Cells[0].ColumnSpan = 2;
                for (int j = 0; j < 8; j++)
                {
                    dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }

            // 返回客户端
            dgExport.RenderControl(htmlWriter);
            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();
        }
    }
    protected void btnRemove_ServerClick(object sender, EventArgs e)
    {
       
        try
        {
            S_CheckInput_Re reCheckInput = currentWHPallet.CheckInput(HRMPLT.Value);
            int count = 0;
            string plt = reCheckInput.plt;
            if (reCheckInput.found == false)
            {
                //此板不存在！
                count = 2;
                ReRemoveDisplay(count.ToString());
                return;
            }
            else
            {
                plt = reCheckInput.plt;
            }
            HRMPLT.Value = plt;
            //此板已經退回產線，不能再退
            bool reCheckRW = currentWHPallet.CheckRW(plt);
            if (reCheckRW == true)
            {
                count = 3;
                ReRemoveDisplay(count.ToString());
                return;
            }
            //此板已出貨，不能再退回產線！
            bool reCheckOut = currentWHPallet.CheckOut(plt);
            if (reCheckOut == true)
            {
                count = 4;
                ReRemoveDisplay(count.ToString());
                return;
            }
            bool reCheckExist = currentWHPallet.CheckExist(plt);
            if (reCheckExist == false)
            {
                count = 5;
                ReRemoveDisplay(count.ToString());
                return;
            }
            currentWHPallet.RemovePallet(plt, userId);
            //currentWHPallet.UpdatePalletWH(plt, userId, "", station, customer);
            string temp = "";
            if (Pre == "cn")
            {
                temp = "栈板:" + plt + "刷出成功！";

            }
            else if (Pre.IndexOf("en") != -1)
            {
                temp = "Pallet:" + plt + " is removed, Success！";
            }
            writeToInfoSuccess(temp);
            ReRemoveDisplay("6");
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
    private void ReRemoveDisplay(string count)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getRemoveDisplay(\"" + count + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReDisplay", scriptBuilder.ToString(), false);
    }
    private void ReResetSN()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getResetSN();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReResetSN", scriptBuilder.ToString(), false);
    }
}
