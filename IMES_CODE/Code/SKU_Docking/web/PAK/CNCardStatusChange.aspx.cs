/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:CN Card Status Change
 * CI-MES12-SPEC-PAK-CN Card Status Change.docx          
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
using com.inventec.system.exception;
public partial class PAK_CNCardStatusChange : System.Web.UI.Page
{
    ICNCardStatusChange iCNCardStatusChange = ServiceAgent.getInstance().GetObjectByName<ICNCardStatusChange>(WebConstant.CNCardStatusChangeObject);
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String userId;
    public String customer;
    public String station;
    public int initRowsCount = 6;
    public string msgStatusError = "";
    public string msgPnoError = "";
    public string msgCountError = "";
    public String cmdValue = "";
    public String placeValue = "";
    public string msgWrongCode = "";
   
    
    protected void Page_Load(object sender, EventArgs e)
    {
        btnGetCNList.ServerClick += new EventHandler(btnGetCNList_ServerClick);
        btnUpdateCNList.ServerClick += new EventHandler(btnUpdateCNList_ServerClick);
        msgStatusError = this.GetLocalResourceObject(Pre + "_msgStatusError").ToString();
        msgPnoError = this.GetLocalResourceObject(Pre + "_msgPnoError").ToString();
        msgCountError = this.GetLocalResourceObject(Pre + "_msgCountError").ToString();
        cmdValue = this.GetLocalResourceObject(Pre + "_cmdValue").ToString();
        msgWrongCode = this.GetLocalResourceObject(Pre + "_msgWrongCode").ToString();
        station = Request["Station"];
        userId = Master.userInfo.UserId;
        customer = Master.userInfo.Customer;
        placeValue =  "'A0','In W/H';'P1','In PdLine';'P0','In P/L Coa Center';'D1','In P/L';'A1','Consumed';'16','Return';'A2','Removal';'A3','Removal';'RE','Return to W/H';'01','Damaged';'02','Lost';'05','Obsolete';'11','Correction';'16','Rerurn'";
        if (!this.IsPostBack)
        {
            this.TextBox1.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
            this.TextBox2.Attributes.Add("onkeydown", "onTextBox2KeyDown()");
            InitLabel();
            initCNCardChange();
            initTableColumnHeader();
            //绑定空表格
            this.gridview.DataSource = getNullDataTable();
            this.gridview.DataBind();
            this.drpCNCardChange.Attributes.Add("onchange", "drpOnChange()");
        }
    }
    private void InitLabel()
    {
        this.lbCardNo.Text = this.GetLocalResourceObject(Pre + "_lbCardNo").ToString();
        this.lbRange.Text = this.GetLocalResourceObject(Pre + "_lbRange").ToString();
        this.lbPlace.Text = this.GetLocalResourceObject(Pre + "_lbPlace").ToString();
        this.lbCount.Text = this.GetLocalResourceObject(Pre + "_lbCount").ToString();
        this.lbPN.Text = this.GetLocalResourceObject(Pre + "_lbPN").ToString();
        this.lbTarget.Text = this.GetLocalResourceObject(Pre + "_lbTarget").ToString();
        this.btnQuery.Value =  this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }
    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.gridview.Columns[0].HeaderText = "HP P/N";
        this.gridview.Columns[1].HeaderText = "Begin No";
        this.gridview.Columns[2].HeaderText = "End No";
        this.gridview.Columns[3].HeaderText = "Qty";
    }
    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("HP P/N", Type.GetType("System.String"));
        retTable.Columns.Add("Begin No", Type.GetType("System.String"));
        retTable.Columns.Add("End No", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        return retTable;
    }

    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
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
            newRow["HP P/N"] = String.Empty;
            newRow["Begin No"] = String.Empty;
            newRow["End No"] = String.Empty;
            newRow["Qty"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private void initControl(IList<SelectInfoDef> lstCNCardChange)
    {
        ListItem item = null;

        this.drpCNCardChange.Items.Clear();
        this.drpCNCardChange.Items.Add(string.Empty);

        if (lstCNCardChange != null)
        {
            foreach (SelectInfoDef temp in lstCNCardChange)
            {
                item = new ListItem(temp.Text, temp.Value);
                this.drpCNCardChange.Items.Add(item);
            }

        }
    }
    public string GetCurrentPlace(string currentStatus)
    {
        int position = placeValue.IndexOf("'");
        while (position > -1)
        {
            placeValue = placeValue.Replace("'", "");
            position = placeValue.IndexOf("'");
        }
        string[] sArray = placeValue.Split(new char[2] { ',', ';' });
        bool find = false;
        foreach (string i in sArray)
        {
            if (i.ToString().IndexOf(currentStatus) != -1)
            {
                find = true;
                continue;
            }
            if (find)
            {
                return i.ToString();
            }
        }
        return "";
    }
    public void initCNCardChange()
    {

        IList<SelectInfoDef> lstCNCardChange = new List<SelectInfoDef>();

        int position = cmdValue.IndexOf("'");
        while (position > -1)
        {
            cmdValue = cmdValue.Replace("'", "");
            position = cmdValue.IndexOf("'");
        }
        string[] sArray = cmdValue.Split(new char[2] { ',', ';' });
        int count = 0;
        SelectInfoDef temp = null;
        foreach (string i in sArray)
        {
            if (count > 1)
            {
                count = 0;
                if (null != temp)
                {
                    lstCNCardChange.Add(temp);
                }
            }/*
            if (0 == count)
            {
                temp = new SelectInfoDef();
                temp.Text = i.ToString();
                count++;
                continue;
            }
            if (1 == count)
            {
                temp.Value = i.ToString();
                count++;
                continue;
            }*/
            if (0 == count)
            {
                temp = new SelectInfoDef();
                temp.Text = i.ToString();
                count++;
                continue;
            }
            if (1 == count)
            {
                string tmp = temp.Text;
                temp.Text = i.ToString();
                temp.Value = i.ToString() + "," + tmp;
                temp.Text = temp.Value;
                temp.Value = tmp;               
                count++;
                continue;
            }
        }
        if (lstCNCardChange != null && lstCNCardChange.Count != 0)
        {
            initControl(lstCNCardChange);
        }
        else
        {
            initControl(null);
        }
    }
    private void btnUpdateCNList_ServerClick(object sender, System.EventArgs e)
    {
        string begNo = begNO.Value;
        string endNo = endNO.Value;
        string statusTarget = status.Value;
        try
        {
            DateTime temp = new DateTime();
            iCNCardStatusChange.UpdateCSN(begNo, endNo, userId, temp.ToString(), statusTarget, "");
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
    private void btnGetCNList_ServerClick(object sender, System.EventArgs e)
    {
        try
        {
            string begNo = begNO.Value;
            string endNo = endNO.Value;
            int rangeCount = int.Parse(range.Value);
            string tempStatus = "";
            string tempPno = "";
            bool first = true;
            IList<S_RowData_CNCardStatus> infoBeg = iCNCardStatusChange.GetCSNList(begNo, begNo);
            if ((null == infoBeg) 
                ||(null != infoBeg && infoBeg.Count == 0))
            {
                ClearPage();
                writeToAlertMessage(msgWrongCode);
                return;
            }
            IList<S_RowData_CNCardStatus> infoEnd = iCNCardStatusChange.GetCSNList(endNo, endNo);
            if ((null == infoEnd)
                || (null != infoEnd && infoEnd.Count == 0))
            {
                ClearPage();
                writeToAlertMessage(msgWrongCode);
                return;
            }
            IList<S_RowData_CNCardStatus> info = iCNCardStatusChange.GetCSNList(begNo, endNo);
            int count = info.Count;
            if (rangeCount != count)
            {
                ClearPage();
                writeToAlertMessage(msgCountError);
                return;
            }
            foreach (S_RowData_CNCardStatus ele in info)
            {
                if (first)
                {
                    tempStatus = ele.Status;
                    tempPno = ele.Pno;
                    first = false;
                    continue;
                }
                if (tempStatus != ele.Status)
                {
                    ClearPage();
                    writeToAlertMessage(msgStatusError);
                    return;
                }
                if (tempPno != ele.Pno)
                {
                    ClearPage();
                    writeToAlertMessage(msgPnoError);
                    return;
                }
            }
            string place = GetCurrentPlace(tempStatus);
            place = tempStatus + "," + place;
            ReQueryPno(tempPno);
            ReQueryPlace(place);
            ReQueryStatus(tempStatus);
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void ClearPage()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ResetPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ClearPage", scriptBuilder.ToString(), false);
    }
    private void ReQueryPno(string pno)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQueryPno(\"" + pno + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReQueryPno", scriptBuilder.ToString(), false);
    }
    private void ReQueryPlace(string place)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQueryPlace(\"" + place + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReQueryPlace", scriptBuilder.ToString(), false);
    }
    private void ReQueryStatus(string status)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("getQueryStatus(\"" + status + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ReQueryStatus", scriptBuilder.ToString(), false);
    }
    
}
