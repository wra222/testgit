using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Xml.Linq;
using com.inventec.imes.DBUtility;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using log4net;
using Microsoft.SqlServer.Dts.Runtime;



public partial class FA_ProductionPlan : IMESBasePage
{
    private IProductionPlan iProductionPlan = ServiceAgent.getInstance().GetObjectByName<IProductionPlan>(WebConstant.ProductionPlanObject);
    public string[] GvQueryColumnName = { "Ship Date", "PdLine", "Family", "Model", "PoNo", "PlanQty", "本次加印數", "未打印數", "已打印未投入數", "已打印已投入數", "Editor", "Cdt", "Udt", "Error" };
    public string[] GvQueryColumnName_Revise = { "Ship Date", "PdLine", "Family", "Model", "PoNo", "PlanQty", "本次加印數", "未打印數", "已打印未投入數", "已打印已投入數", "Editor", "Cdt", "Udt", "Error" };
    public int[] GvQueryColumnNameWidth = { 65, 30, 70, 65, 40, 40, 65, 60, 60, 60, 45, 80, 80, 60 };
    public String UserId;
    public String UserName;
    public String Customer;
    public String paraStation;
    public String CombinedPO;
    private IPdLine iPDLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.CommonObject);
    private IConstValue iConstValue = ServiceAgent.getInstance().GetObjectByName<IConstValue>(WebConstant.CommonObject);
    public int initRowsCount = 6;
    //private const int DEFAULT_ROWS = 36;

    public string today;
    public string selectPdline;
    public string JobName = ConfigurationManager.AppSettings["VirtualMO_JOBNAME"].ToString();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        
        UserId = Master.userInfo.UserId;
        UserName = Master.userInfo.UserName;
        Customer = Master.userInfo.Customer;
        paraStation = Request["Station"];
        CombinedPO = Request["CombinedPO"];
        if (!Page.IsPostBack)
        {
            IList<PdLineInfo> lstPDLine = null;
            
            lstPDLine = iPDLine.GetPdLineList(paraStation, Customer);
            var Query =
                (from item in lstPDLine
                 select new PdLineInfo { id = item.id.Substring(0, 1) }).ToList<PdLineInfo>().Distinct();
            initControl(Query.ToList<PdLineInfo>());
            
            InitCondition();
        }
        today = DateTime.Now.ToString("yyyy-MM-dd");
    }

    private void initControl(IList<PdLineInfo> lstPDLine)
    {
        ListItem item = null;
        if (lstPDLine != null)
        {
            foreach (PdLineInfo temp in lstPDLine)
            {
                item = new ListItem(temp.id.Trim(), temp.id.Trim());
                this.cmbPdLine.Items.Add(item);
            }
        }

    }
    protected void btnQuery_serverclick(object sender, EventArgs e)
    {
        try
        {
            ShowInfoClear();
            //string time = DateTime.Now.ToString("yyyy-MM-dd");
            if (this.cmbPdLine.SelectedValue.ToString() == "" || this.cmbPdLine.SelectedValue.ToString() == null)
            {
                InitCondition();
                showErrorMessage("Please Select PdLine...");
                writeToAlertMessage("Please Select PdLine...");
            }
            else
            {
                //if (Convert.ToDateTime(this.selectdate.Value) >= Convert.ToDateTime(time))
                //{
                string line = this.cmbPdLine.SelectedValue.ToString();
                DateTime ShipDate = Convert.ToDateTime(this.selectdate.Value);
                IList<ConstValueInfo> lstStation = iConstValue.GetConstValueListByType("NonInputStation", "");
                if (this.Collect.Checked)
                {
                    
                    string Station = lstStation[0].value.ToString();
                    IList<ProductPlanLog> dataList = iProductionPlan.GetProductPlanMOByLineAndShipDate(line, ShipDate, Station);
                    if (dataList == null || dataList.Count == 0)
                    {
                        bindTable(null);
                    }
                    else
                    {
                        bindTable(dataList);
                    }
                }
                else if (this.Scheduling.Checked)
                {
                    string action = "New";
                    IList<ProductPlanLog> dataList = iProductionPlan.GetProductPlanLogByLineAndShipDateAndAction(line, ShipDate, action);
                    if (dataList == null || dataList.Count == 0)
                    {
                        bindTable(null);
                    }
                    else
                    {
                        bindTable(dataList);
                    }
                }
                else if (this.subtractsingle.Checked)
                {
                    string action = "Revise";
                    IList<ProductPlanLog> dataList = iProductionPlan.GetProductPlanLogByLineAndShipDateAndAction(line, ShipDate, action);
                    if (dataList == null || dataList.Count == 0)
                    {
                        bindTable(null);
                    }
                    else
                    {
                        bindTable(dataList);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    private void bindTable(IList<ProductPlanLog> list)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        if (list != null && list.Count != 0)
        {
            if (list[0].Pass == null)
            { dt = initTable(13); }
            else {dt = initTable(14);}			
            foreach (ProductPlanLog temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ShipDate.ToString("yyyy-MM-dd");
                dr[1] = temp.PdLine;
                dr[2] = temp.Family;
                dr[3] = temp.Model;
                dr[4] = temp.PoNo;
                dr[5] = temp.PlanQty;
                dr[6] = temp.AddPrintQty;
                dr[7] = temp.RemainQty;
                dr[8] = temp.NonInputQty;
                dr[9] = temp.InputQty;
                dr[10] = temp.Editor;
                dr[11] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[12] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                if (temp.Pass != null)
                { dr[13] = temp.ErrDescr; }
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getNullDataTable(1);
        }
        
        gvResult.DataSource = dt;
        gvResult.DataBind();
        if (list == null || list[0].Pass == null)
        { InitGridView(13); }
        else { InitGridView(14); }
        //InitGridView();
        
        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);
    }
    private void bindTable(IList<ProductPlanLog> list, string type)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        if (list != null && list.Count != 0)
        {
            if (list[0].Pass == null)
            { dt = initTable(13); }
            else { 
				if (type == null)
				{ dt = initTable(14);}
				else
				{ dt = initTable_revise(14);}
			}
            foreach (ProductPlanLog temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ShipDate.ToString("yyyy-MM-dd");
                dr[1] = temp.PdLine;
                dr[2] = temp.Family;
                dr[3] = temp.Model;
                dr[4] = temp.PoNo;
                dr[5] = temp.PlanQty;
                dr[6] = temp.AddPrintQty;
                dr[7] = temp.RemainQty;
                dr[8] = temp.NonInputQty;
                dr[9] = temp.InputQty;
                dr[10] = temp.Editor;
                dr[11] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[11] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                if (temp.Pass != null)
                { dr[13] = temp.ErrDescr; }
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getNullDataTable(1);
        }
        
        gvResult.DataSource = dt;
        gvResult.DataBind();
        if (list == null || list[0].Pass == null)
        { InitGridView(13); }
        else { InitGridView(14); }
        //InitGridView();
        
        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);
    }
	

    
    protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Footer))
        {
            if (drv != null && e.Row.Cells.Count == 14)
            {
                if (e.Row.Cells[13].Text != "&nbsp;")  //條件式
                {
                    e.Row.Attributes.Add("style", "background-color:#ff0000"); //新增背景色屬性
                }
            }
        }
    }
    private void InitCondition()
    {
        this.gvResult.DataSource = getNullDataTable(1);
        this.gvResult.DataBind();
        InitGridView(13);
    }
    private void InitGridView(int j)
    {
        for (int i = 0; i < j; i++)
        {
            gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
        //for (int i = 0; i < GvQueryColumnNameWidth.Length; i++)
        //{
        //    gvResult.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        //}
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable(13);
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; i < 11; i++)
            {
                newRow[k] = "";
            }
            //foreach (string columnname in GvQueryColumnName)
            //{
            //    newRow[columnname] = "";
            //}
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable(int j)
    {
        DataTable retTable = new DataTable();
        for (int i = 0; i < j; i++)
        {
            retTable.Columns.Add(GvQueryColumnName[i], Type.GetType("System.String"));
        }
        //foreach (string columnname in GvQueryColumnName)
        //{
        //    retTable.Columns.Add(columnname, Type.GetType("System.String"));
        //}

        return retTable;
    }
    private DataTable initTable_revise(int j)
    {
        DataTable retTable = new DataTable();
        for (int i = 0; i < j; i++)
        {
            retTable.Columns.Add(GvQueryColumnName_Revise[i], Type.GetType("System.String"));
        }
        //foreach (string columnname in GvQueryColumnName)
        //{
        //    retTable.Columns.Add(columnname, Type.GetType("System.String"));
        //}

        return retTable;
    }	
    
    protected void btnUploadList_Revise_ServerClick(object sender, System.EventArgs e)
    {
        string newName = DateTime.Now.Ticks.ToString() + FileUp.PostedFile.FileName.Substring(FileUp.PostedFile.FileName.LastIndexOf("."));
        string fullName = Server.MapPath("~") + "/" + newName;
        string times = this.selectdate.Value;
        string line = this.selectline.Value;
 
        try
        {
            string fileName = FileUp.FileName;
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string extName = fileName.Substring(fileName.LastIndexOf("."));
            
            if (Convert.ToDateTime(this.selectdate.Value) >= Convert.ToDateTime(time))
            {
                ShowInfoClear();
                FileUp.SaveAs(fullName);
                DataTable dt = ExcelManager.getExcelSheetData(fullName);
                IList<TbProductPlan> dataList = new List<TbProductPlan>();
                //line = this.selectline.Value;//this.cmbPdLine.SelectedValue.ToString();
                DateTime ShipDate = Convert.ToDateTime(this.selectdate.Value);
                int coun = 0;
                string errormsg = "";
                foreach (DataRow dr in dt.Rows)
                {
                    coun += 1;
                    string dr0 = dr[0].ToString().Trim();
                    string dr1 = dr[1].ToString().Trim();
                    string dr2 = dr[2].ToString().Trim();
                    bool check = false;
                    if (CombinedPO == "Y")
                    {
                        if (!string.IsNullOrEmpty(dr0) && !string.IsNullOrEmpty(dr1) && !string.IsNullOrEmpty(dr2))
                        {
                            check = true;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dr0) && !string.IsNullOrEmpty(dr1))
                        {
                            check = true;
                        }
                    }


                    if (check)
                    {
                        TbProductPlan item = new TbProductPlan();
                        item.ShipDate = ShipDate;
                        item.PdLine = line;
                        item.Family = "";
                        item.PoNo = dr2;
                        item.Model = dr[0].ToString();
                        item.PlanQty = Convert.ToInt32(dr[1].ToString());
                        item.AddPrintQty = 0;
                        item.PrePrintQty = 0;
                        item.Editor = UserId;
                        item.Cdt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        item.Udt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        dataList.Add(item);
                        
                    }
                    else
                    {
                        if (dr.IsNull(0) || dr[0].ToString() == "")
                        {
                            errormsg = "Excel第" + coun + "行，第1列為空值";
                        }
                        else if (dr.IsNull(1) || dr[1].ToString() == "")
                        {
                            errormsg = "Excel第" + coun + "行，第2列為空值";
                        }
                    }

                }


                if (dataList.Count == 0)
                {
                    bindTable(null);
                    showErrorMessage("Excel can't be null...");
                    
                }
                else if (dt.Rows.Count != dataList.Count)
                {
                    bindTable(null);
                    showErrorMessage(errormsg);
                }
                else
                {
                    var listRet = (from p in dataList
                                   group p by p.Model into g
                                   where g.Count() > 1
                                   select g.Key).ToList();

                    if (listRet.Count > 0)
                    {
                        bindTable(null);
                        string Models = "";
                        foreach (var items in listRet)
                        {
                            Models += items.ToString() + ",";
                        }
                        Models = Models.Substring(0, Models.Length - 1);
                        showErrorMessage("12碼機型重複 : " + Models);
                    }
                    else
                    {
                        IList<ProductPlanLog> ShowList = new List<ProductPlanLog>();
                        ShowList = iProductionPlan.UploadProductPlan_Revise(dataList,CombinedPO);

                        bindTable(ShowList, "Revise");
                    }
                }
            }
            else
            {
                InitCondition();
                writeToAlertMessage("選擇時間需要大於等於今天...");
            }

        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            File.Delete(fullName);
            endWaitingCoverDiv();
            today = times;
            this.cmbPdLine.SelectedValue = line;
        }
    }
    protected void btnUploadList_ServerClick(object sender, System.EventArgs e)
    {
        string newName = DateTime.Now.Ticks.ToString() + FileUp.PostedFile.FileName.Substring(FileUp.PostedFile.FileName.LastIndexOf("."));
        string fullName = Server.MapPath("~") + "/" + newName;
        string times = this.selectdate.Value;
        string line = this.selectline.Value;
        try
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = FileUp.FileName;
            string extName = fileName.Substring(fileName.LastIndexOf("."));
            
            if (Convert.ToDateTime(this.selectdate.Value) >= Convert.ToDateTime(time))
            {
                ShowInfoClear();
                FileUp.SaveAs(fullName);
                DataTable dt = ExcelManager.getExcelSheetData(fullName);

                IList<TbProductPlan> dataList = new List<TbProductPlan>();
                //line = this.selectline.Value;//this.cmbPdLine.SelectedValue.ToString();
                DateTime ShipDate = Convert.ToDateTime(this.selectdate.Value);
                int coun = 0;
                string errormsg = "";
                foreach (DataRow dr in dt.Rows)
                {
                    coun += 1;
                    string dr0 = dr[0].ToString().Trim();
                    string dr1 = dr[1].ToString().Trim();
                    string dr2 = dr[2].ToString().Trim();
                    string dr3 = dr[3].ToString().Trim();
                    bool check = false;
                    if (CombinedPO == "Y")
                    {
                        if (!string.IsNullOrEmpty(dr0) && !string.IsNullOrEmpty(dr1) && !string.IsNullOrEmpty(dr2) && !string.IsNullOrEmpty(dr3))
                        {
                            check = true;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dr0) && !string.IsNullOrEmpty(dr1) && !string.IsNullOrEmpty(dr2))
                        {
                            check = true;
                        }
                    }
                    if (check)
                    {
                        TbProductPlan item = new TbProductPlan();
                        item.ShipDate = ShipDate;
                        item.PdLine = line;
                        item.Family = "";
                        item.Model = dr0;
                        item.PlanQty = int.Parse(dr1);
                        item.AddPrintQty = int.Parse(dr2);
                        item.PoNo = dr3;
                        item.PrePrintQty = 0;
                        item.Editor = UserId;
                        item.Cdt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        item.Udt = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        dataList.Add(item);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dr0))
                        {
                            errormsg = "Excel第" + coun + "行，第1列為空值";
                        }
                        else if (string.IsNullOrEmpty(dr1))
                        {
                            errormsg = "Excel第" + coun + "行，第2列為空值";
                        }
                        else if (string.IsNullOrEmpty(dr2))
                        {
                            errormsg = "Excel第" + coun + "行，第3列為空值";
                        }
                        else if (string.IsNullOrEmpty(dr3))
                        {
                            errormsg = "Excel第" + coun + "行，第4列為空值";
                        }
                    }

                }
                if (dataList.Count == 0)
                {
                    bindTable(null);
                    showErrorMessage("Excel can't be null...");
                    
                }
                else if (dt.Rows.Count != dataList.Count)
                {
                    bindTable(null);
                    showErrorMessage(errormsg);
                }
                else
                {
                    var listRet = (from p in dataList
                                   group p by p.Model into g
                                   where g.Count() > 1
                                   select g.Key).ToList();

                    if (listRet.Count > 0)
                    {
                        bindTable(null);
                        string Models = "";
                        foreach (var items in listRet)
                        {
                            Models += items.ToString() + ",";
                        }
                        Models = Models.Substring(0, Models.Length - 1);
                        showErrorMessage("12碼機型重複 : " + Models);
                    }
                    else
                    {
                        IList<ProductPlanLog> ShowList = new List<ProductPlanLog>();
                        ShowList = iProductionPlan.UploadProductPlan(dataList,CombinedPO);
                        bindTable(ShowList);
                    }
                }
            }
            else
            {
                InitCondition();
                writeToAlertMessage("選擇時間需要大於等於今天...");
            }
            
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            File.Delete(fullName);
            endWaitingCoverDiv();
            today = times;
            this.cmbPdLine.SelectedValue = line;
        }
    }

    private bool CheckExcelNullExists(DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
             
            foreach (object item in dr.ItemArray)
            {
                if ( item == null|| item == "")
                { return false; }
            }
        }
        return true;
    }

  





    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }


    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }



    /// <summary>
    /// 清空错误信息
    /// </summary>
    /// <param name="er"></param>
    private void ShowInfoClear()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" +  "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }


    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }


    /// <summary>
    /// AlertErrorMessage for Family
    /// </summary>  
    private void alertNoFamily()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (alertSelectFamily,100);" + "\r\n" +
            "</script>";
        //ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "alertSelectFamily", script, false);
    }

    

    /// <summary>
    /// Fresh Button
    /// </summary>  
    private void FreshButton()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (getMObyModel,100);" + "\r\n" +
            "</script>";
        //ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "getMObyModel", script, false);
    }

    
    /// <summary>
    /// Download 成功信息
    /// </summary>  
    //private void DownloadMOSucceed()
    //{
    //    String script = "<script language='javascript'>" + "\r\n" +
    //        "window.setTimeout (DownloadSucceed,100);" + "\r\n" +
    //        "</script>";
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "DownloadSucceed", script, false);
    //}
    private void clearTabel()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (clearTabel,100);" + "\r\n" +
            "</script>";
        //ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearTabel", script, false);
    }
}
