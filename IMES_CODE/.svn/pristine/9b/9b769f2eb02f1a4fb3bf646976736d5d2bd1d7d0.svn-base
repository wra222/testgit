using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;



public partial class SA_GenerateSMTMO : IMESBasePage
{
    public string pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private int fullRowCount = 6;
    IGenSMTMO iGenSMTMO = ServiceAgent.getInstance().GetObjectByName<IGenSMTMO>(WebConstant.GenSMTMOObject);
    public string userId;
    public string customer;
       
    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            this.CmbMBCode.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbMBCode_Selected);
            if (!this.IsPostBack)
            {
                Master.NeedPrint = false;
                InitPage();                
                this.GridViewExt1.DataSource = getBomDataTable();
                this.GridViewExt1.DataBind();
                InitGridView();
                this.station.Value = Request["Station"];
                //station.Value=""
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;                                                                
            }
        }
        catch (FisException ee)
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            InitGridView();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            InitGridView();
            writeToAlertMessage(ex.Message);
        }
    }

    /// <summary>
    /// 选择MBCode下拉框，会刷新Model下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbMBCode_Selected(object sender, System.EventArgs e)
    {
        try {            
            if (this.CmbMBCode.InnerDropDownList.SelectedValue == "")
            {                
                this.CmbModel.clearContent();
             
            }
            else
            {               
                //this.CmbModel.refreshDropContent(this.CmbMBCode.InnerDropDownList.SelectedValue);
                IList<string> infos = new List<string>();
                this.CmbModel.clearContent();
                infos = iGenSMTMO.GetModelByMBCode(this.CmbMBCode.InnerDropDownList.SelectedValue);
                if(infos != null && infos.Count > 0)
                {
                    foreach (string temp in infos)
                    {
                        this.CmbModel.InnerDropDownList.Items.Add(new ListItem(temp));
                    }
                    this.CmbModel.setSelected(1);                    
                    if (this.CmbModel.InnerDropDownList.Items.Count > 1)
                    {
                        this.CmbModel.InnerDropDownList.SelectedIndex = 1;
                    }
                }
            }
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


    /// <summary>
    /// 用实际数据填充gridview
    /// </summary>
    /// <returns></returns>
    private DataTable getBomDataTable()
    {        
        DataTable dt = initBomTable();
        DataRow newRow = null;      
        //try 
        //{
            IList<SmtmoInfo> sMTMOInfoList = iGenSMTMO.Query(Master.userInfo.UserId, this.station.Value, Master.userInfo.Customer);
            if (sMTMOInfoList != null && sMTMOInfoList.Count != 0)
            {
                foreach (SmtmoInfo temp in sMTMOInfoList)
                {
                    newRow = dt.NewRow();

                    MbCodeAndMdlInfo info = new MbCodeAndMdlInfo();
                    info = iGenSMTMO.GetMBMDLByPno(temp.iecpartno);
                    //if (info != null)
                    {
                        newRow["MO"] = temp.smtmo;
                        if (info != null)
                        {
                            newRow["MBCODE"] = info.mbCode;
                            newRow["Descr"] = info.mdl;
                        }
                        else
                        {
                            newRow["MBCODE"] = string.Empty;
                            newRow["Descr"] = string.Empty;                        
                        }
                        newRow["Model"] = temp.iecpartno;
                        newRow["Qty"] = temp.qty.ToString();
                        newRow["PQty"] = temp.printQty.ToString();
                        newRow["Remark"] = temp.remark;
                        newRow["Cdt"] = temp.cdt.ToString("yy-MM-dd  HH:mm:ss");
                        dt.Rows.Add(newRow);
                    }
                }
                if (sMTMOInfoList.Count < fullRowCount)
                {
                    for (int i = sMTMOInfoList.Count; i < fullRowCount; i++)
                    {
                        newRow = dt.NewRow();
                        dt.Rows.Add(newRow);
                    }
                }
            }
            else
            {
                for (int i = 0; i < fullRowCount; i++)
                {
                    newRow = dt.NewRow();
                    newRow["MO"] = String.Empty;
                    newRow["MBCODE"] = String.Empty;
                    newRow["Descr"] = String.Empty;
                    newRow["Model"] = String.Empty;
                    newRow["Qty"] = String.Empty;
                    newRow["PQty"] = String.Empty;
                    newRow["Remark"] = String.Empty;
                    newRow["Cdt"] = String.Empty;
                    dt.Rows.Add(newRow);
                }
            }            
        //}
        //catch (FisException ee)
        //{
        //    writeToAlertMessage(ee.mErrmsg);
        //}
        //catch (Exception ex)
        //{
        //    writeToAlertMessage(ex.Message);
        //}      
        return dt;
    }

    /// <summary>
    /// 用实际数据填充gridview
    /// </summary>
    /// <returns></returns>
    private DataTable getBomDataTable1()
    {
        DataTable dt = initBomTable();            
        DataRow newRow = null;
        string remark;                
        try
        {
            IList<SmtmoInfo> sMTMOInfoList = iGenSMTMO.Report(Master.userInfo.UserId, this.station.Value, Master.userInfo.Customer);
            if (sMTMOInfoList != null && sMTMOInfoList.Count != 0)
            {
                foreach (SmtmoInfo temp in sMTMOInfoList)
                {
                    MbCodeAndMdlInfo info = new MbCodeAndMdlInfo();
                    info = iGenSMTMO.GetMBMDLByPno(temp.iecpartno);

                    newRow = dt.NewRow();
                    newRow["MO"] = temp.smtmo;
                    if (info != null)
                    {
                        newRow["MBCODE"] = info.mbCode;
                        newRow["Descr"] = info.mdl;
                    }
                    else
                    {
                        newRow["MBCODE"] = string.Empty;
                        newRow["Descr"] = string.Empty;
                    }
                    newRow["Model"] = temp.iecpartno;
                    newRow["Qty"] = temp.qty.ToString();
                    newRow["PQty"] = temp.printQty.ToString();
                    remark = temp.remark;
                    remark = remark.Replace("<","&lt;");
                    remark = remark.Replace(">","&gt;");
                    remark = remark.Replace(Environment.NewLine, "<br>");
                    newRow["Remark"] = remark;
                    newRow["Cdt"] = temp.cdt.ToString("yy-MM-dd  HH:mm:ss");
                    dt.Rows.Add(newRow);                  
                }                
            }
            else
            {
                for (int i = 0; i < fullRowCount; i++)
                {
                    newRow = dt.NewRow();
                    newRow["MO"] = String.Empty;
                    newRow["MBCODE"] = String.Empty;
                    newRow["Descr"] = String.Empty;
                    newRow["Model"] = String.Empty;
                    newRow["Qty"] = String.Empty;
                    newRow["PQty"] = String.Empty;
                    newRow["Remark"] = String.Empty;
                    newRow["Cdt"] = String.Empty;
                    dt.Rows.Add(newRow);
                }                
            }
            dt.Columns[0].ColumnName = this.GetLocalResourceObject(pre + "_" + "colMO").ToString();
            dt.Columns[1].ColumnName = this.GetLocalResourceObject(pre + "_" + "colMBCODE").ToString();
            dt.Columns[2].ColumnName = this.GetLocalResourceObject(pre + "_" + "colDes").ToString();
            dt.Columns[3].ColumnName = this.GetLocalResourceObject(pre + "_" + "colModel").ToString();
            dt.Columns[4].ColumnName = this.GetLocalResourceObject(pre + "_" + "colQty").ToString();
            dt.Columns[5].ColumnName = this.GetLocalResourceObject(pre + "_" + "colPQty").ToString();
            dt.Columns[6].ColumnName = this.GetLocalResourceObject(pre + "_" + "colRemark").ToString();
            dt.Columns[7].ColumnName = this.GetLocalResourceObject(pre + "_" + "colCDT").ToString();
        }
     
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }

        return dt;
    }
 
    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
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
    /// 为各列数据增加toolTip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (String.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) || (e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {
                CheckBox check = (CheckBox)e.Row.FindControl("RowChk");
                check.Style.Add("display", "none");
            }
            for (int i = 1; i <e.Row .Cells .Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;                   
                    e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace(Environment.NewLine, "<br>");                                     
                }                                                              
            }            
        }
    }
    /// <summary>
    /// 查询数据，刷新gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void queryClick(object sender, System.EventArgs e)
    {
        try
        {
            beginWaitingCoverDiv();
            this.GridViewExt1.DataSource = getBomDataTable();
            this.GridViewExt1.DataBind();
            InitGridView();
            endWaitingCoverDiv();
        }
        catch (FisException ee)
        {
            endWaitingCoverDiv();
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            InitGridView();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            endWaitingCoverDiv();
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            InitGridView();
            writeToAlertMessage(ex.Message);
        }       
    }

    /// <summary>
    /// 将查询的数据导出到excel中
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void excelClick(object sender, System.EventArgs e)
    {           
        DataTable2Excel(getBomDataTable1());
    }

    public static void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.UI.WebControls.DataGrid  dgExport = null;
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
                //excel.Columns[3].ItemStyle.CssClass = "xlsText";
                //excel.Items[i].Cells[3].Style.Add("mso-number-format", "\"@\"");
                for (int j = 0; j < 8; j++)
                {
                    //if (j == 7)
                    //{
                    //    dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:yyy-mm-dd ##:##:##");
                    //}
                    //else
                    //{
                    dgExport.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                    //}
                }
            }

            // 返回客户端
            dgExport.RenderControl(htmlWriter);
            
            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();
        }
    }

    /// <summary>
    ///置焦点
    /// </summary>  
    private void setFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (setMBCodeCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setMBCodeCmbFocus", script, false);
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }

    /// <summary>
    /// reset页面信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnHidden_Click(object sender, System.EventArgs e)
    {
        try
        {           
            //重置mbCode下拉框,并触发它的选择事件
            this.CmbMBCode.setSelected(0);
            cmbMBCode_Selected(sender, e);

            this.GridViewExt1.DataSource = getBomDataTable();
            this.GridViewExt1.DataBind();
            InitGridView();

            setFocus();
        }
        catch (FisException ee)
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            InitGridView();
            writeToAlertMessage(ee.mErrmsg);
        
        }
        catch (Exception ex)
        {
            this.GridViewExt1.DataSource = getNullDataTable();
            this.GridViewExt1.DataBind();
            InitGridView();
            writeToAlertMessage(ex.Message);           
        }
    }


    /// <summary>
    /// 初始化静态Label
    /// </summary>
    private void InitPage()
    {
        this.lblModel.Text = this.GetLocalResourceObject(pre + "_lblModel").ToString();
        this.lblMBCOCE.Text = this.GetLocalResourceObject(pre + "_lblMB").ToString();
        this.lblProType.Text = this.GetLocalResourceObject(pre + "_lblprotype").ToString();
        this.lblQuan.Text = this.GetLocalResourceObject(pre + "_lblQuan").ToString();
        this.lblTest.Text = this.GetLocalResourceObject(pre + "_lblTest").ToString();
        this.lblTrial.Text = this.GetLocalResourceObject(pre + "_lblTrial").ToString();
        this.lblPilot.Text = this.GetLocalResourceObject(pre + "_lblPilot").ToString();
        this.lblTrace.Text = this.GetLocalResourceObject(pre + "_lblTrace").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(pre + "_lblQty").ToString();
        this.lblrange.Text = this.GetLocalResourceObject(pre + "_lblRange").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(pre + "_lblRemark").ToString();
        this.btnGenerate.Value = this.GetLocalResourceObject(pre + "_btnGenerate").ToString();
        this.lblUnprintTitle.Text = this.GetLocalResourceObject(pre + "_lblUnprintSMT").ToString();
        this.btnQuery.Value = this.GetLocalResourceObject(pre + "_btnQuery").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(pre + "_btnDelete").ToString();
        this.btnToExl.Value = this.GetLocalResourceObject(pre + "_btnToExcel").ToString();
        /*                                
        this.lblSpecial.Text = this.GetLocalResourceObject(pre + "_lblSpecial").ToString();                
        this.lblRemarkLimited.Text = this.GetLocalResourceObject(pre + "_lblRemarkLimited").ToString();
         */
        setFocus();
    }

    /// <summary>
    /// 初始化列名及列宽
    /// </summary>
    private void InitGridView()
    {
        GridViewExt1.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(pre + "_" + "colMO").ToString();
        GridViewExt1.HeaderRow.Cells[2].Text = this.GetLocalResourceObject(pre + "_" + "colMBCODE").ToString();
        GridViewExt1.HeaderRow.Cells[3].Text = this.GetLocalResourceObject(pre + "_" + "colDes").ToString();
        GridViewExt1.HeaderRow.Cells[4].Text = this.GetLocalResourceObject(pre + "_" + "colModel").ToString();
        GridViewExt1.HeaderRow.Cells[5].Text = this.GetLocalResourceObject(pre + "_" + "colQty").ToString();
        GridViewExt1.HeaderRow.Cells[6].Text = this.GetLocalResourceObject(pre + "_" + "colPQty").ToString();
        GridViewExt1.HeaderRow.Cells[7].Text = this.GetLocalResourceObject(pre + "_" + "colRemark").ToString();
        GridViewExt1.HeaderRow.Cells[8].Text = this.GetLocalResourceObject(pre + "_" + "colCDT").ToString();
        GridViewExt1.HeaderRow.Cells[0].Width = Unit.Pixel(60);
        GridViewExt1.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        GridViewExt1.HeaderRow.Cells[2].Width = Unit.Pixel(100);
        GridViewExt1.HeaderRow.Cells[3].Width = Unit.Pixel(120);
        GridViewExt1.HeaderRow.Cells[4].Width = Unit.Pixel(120);
        GridViewExt1.HeaderRow.Cells[5].Width = Unit.Pixel(50);
        GridViewExt1.HeaderRow.Cells[6].Width = Unit.Pixel(70);
        GridViewExt1.HeaderRow.Cells[7].Width = Unit.Pixel(100);
    }
    /// <summary>
    /// 用空数据填充gridview
    /// </summary>
    /// <returns></returns>
    private DataTable getNullDataTable()
    {

        DataTable dt = initBomTable();
        DataRow newRow = null;

        for (int i = 0; i < fullRowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["MO"] = String.Empty;
            newRow["MBCODE"] = String.Empty;
            newRow["Descr"] = String.Empty;
            newRow["Model"] = String.Empty;
            newRow["Qty"] = String.Empty;
            newRow["PQty"] = String.Empty;
            newRow["Remark"] = String.Empty;
            newRow["Cdt"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    /// <summary>
    ///定义各列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initBomTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MO", Type.GetType("System.String"));
        retTable.Columns.Add("MBCODE", Type.GetType("System.String"));
        retTable.Columns.Add("Descr", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        retTable.Columns.Add("PQty", Type.GetType("System.String"));
        retTable.Columns.Add("Remark", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        //retTable.Columns["MO"].ColumnName = "新的列名";        
        return retTable;
    }

}

