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
//using IMES.Station.Interface.CommonIntf;
//using IMES.Station.Interface.StationIntf;
using IMES.Docking.Interface.DockingIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;

public partial class DOCKING_PCATestStation : IMESBasePage 
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int initRowsCount = 4;
    public int initRowsCountForLot = 8;
    public string userId;
    public string customer;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            //initTableColumnHeader();
            //注册pdLine下拉框的选择事件
            this.cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_Selected);

            //注册test station下拉框的选择事件
            this.cmbTestStation.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbTestStation_Selected);

            if (!Page.IsPostBack)
            {
                Master.NeedPrint = false;
                InitLabel();
                initTableColumnHeader();
                //绑定空表格
                this.gridview.DataSource = getNullDataTable();
                this.gridview.DataBind();
                //ViewState["ds"] = this.gridview.DataSource;
                initTableColumnHeaderforMBCode();
                this.gridviewMB.DataSource = getNullDataTableForMBCode();
                this.gridviewMB.DataBind();

                initTableColumnHeaderforLot();
                this.GridViewLot.DataSource = getNullDataTableForLot();
                this.GridViewLot.DataBind();
                
                ////将pdLine combo box的初始化参数传入
                //this.cmbPdLine.Station = Request["Station"];
                //this.cmbPdLine.Station = "20";
                this.cmbTestStation.Type = "SATEST";
                //this.station.Value = Request["Station"];
                //this.station.Value = "20";
                userId = Master.userInfo.UserId;
                customer = Master.userInfo.Customer;
                this.useridHidden.Value = userId;
                this.customerHidden.Value = customer;
                //按照UC人员的要求，初始时不设置Line 2/25
                //this.cmbPdLine.Customer = customer;
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
    /// 选择pdLIne或者testStation下拉框，会reset界面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbTestStation_Selected(object sender, System.EventArgs e)
    {
        //try
        //{
          
        //    //刷新MO下拉框内容
        //    if (this.cmbTestStation.InnerDropDownList.SelectedValue == "")
        //    {
        //        //清空MO下拉框内容
        //        this.cmbPdLine.clearContent();
             
        //    }
        //    else
        //    {
        //        this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, UserInfo.Customer);
        //    }
        //}
        //catch (FisException ee)
        //{
        //    writeToAlertMessage(ee.mErrmsg);
        //}
        //catch (Exception ex)
        //{
        //    writeToAlertMessage(ex.Message);
        //}
        try
        {
            String script = "";
            if (this.cmbTestStation.InnerDropDownList.SelectedValue == "")
            { 
                this.cmbPdLine.clearContent(); 
                script = "<script language='javascript'>" + "\r\n" +
                         "window.setTimeout (stationchange,100);" + "\r\n" +
                         "</script>";
                ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "stationchange", script, false);
            }
            else
            {
                var selecttstation = cmbTestStation.InnerDropDownList.SelectedValue;
                if (selecttstation.ToUpper() == "1A")//‘1A’：Fru Runin Test
                {
                    script = "<script language='javascript'>" + "\r\n" +
                         "window.setTimeout (FruRuninTest,100);" + "\r\n" +
                         "</script>";
                    ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "FruRuninTest", script, false);
                }
                else
                {
                    var selectLine = cmbPdLine.InnerDropDownList.SelectedItem.Text;
                    this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, this.customerHidden.Value);
                    int i = 0;
                    foreach (ListItem lst in this.cmbPdLine.InnerDropDownList.Items)
                    {
                        if (selectLine == lst.Text)
                        {
                            cmbPdLine.InnerDropDownList.SelectedIndex = i;
                            changeLottable();
                            script = "<script language='javascript'>" + "\r\n" +
                                            "window.setTimeout (stationchange,100);" + "\r\n" +
                                            "</script>";
                            ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "stationchange", script, false);

                            return;
                        }
                        i++;
                    }
                    
                }
            }
            changeLottable();
            script = "<script language='javascript'>" + "\r\n" +
                         "window.setTimeout (stationchange,100);" + "\r\n" +
                         "</script>";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "stationchange", script, false);

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

    private void changeLottable()
    {
        if ((this.cmbTestStation.InnerDropDownList.SelectedValue.Trim() == "15") && (this.cmbPdLine.InnerDropDownList.SelectedValue.Trim() != ""))
        {
            IPCATestStation iPCATestStation = ServiceAgent.getInstance().GetObjectByName<IPCATestStation>(WebConstant.PCATestStationDocking);
            IList<LotInfo> listlotinfo = iPCATestStation.GetLotInfoLst(this.cmbPdLine.InnerDropDownList.SelectedValue);

            updateLotTable(listlotinfo);
        }
        else
        {
            
            DataTable dt = initTableForLot();
            DataRow newRow;
            for (int i = 0; i < initRowsCountForLot; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
            this.GridViewLot.DataSource = dt;
            this.GridViewLot.DataBind();
            initTableColumnHeaderforLot();
            this.txtTotalPCS.Text = "";

            updatePanel3.Update();
            updatePanelPCS.Update();
        }
    }
    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        var script = "";
        if ((this.cmbPdLine.InnerDropDownList.SelectedValue != "") && (this.cmbTestStation.InnerDropDownList.SelectedValue.Trim() == "15"))
        {
           
            IPCATestStation iPCATestStation = ServiceAgent.getInstance().GetObjectByName<IPCATestStation>(WebConstant.PCATestStationDocking);
            IList<LotInfo> listlotinfo = iPCATestStation.GetLotInfoLst(this.cmbPdLine.InnerDropDownList.SelectedValue);

            updateLotTable(listlotinfo);
        }
        else
        {
            DataTable dt = initTableForLot();
            DataRow newRow;
            for (int i = 0; i < initRowsCountForLot; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
            this.GridViewLot.DataSource = dt;
            this.GridViewLot.DataBind();
            initTableColumnHeaderforLot();
            this.txtTotalPCS.Text = "";

            updatePanel3.Update();
            updatePanelPCS.Update();
        }
        //ITC-1414-0050
        script = "<script language='javascript'>" + "\r\n" +
                 "window.setTimeout (stationchange,100);" + "\r\n" +
                 "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "stationchange", script, false);

    }

    public void updateLotTable(IList<LotInfo> listlotinfo)
    {
        int iTotalPCS = 0;
        if (listlotinfo != null && listlotinfo.Count > 0)
        {
            DataTable dt = initTableForLot();
            DataRow newRow;
            //for (int i = 0; i<listlotinfo.Count;i++)
            for (int i = listlotinfo.Count - 1; i >= 0; i--)
            {
                newRow = dt.NewRow();
                newRow["LotNo"] = listlotinfo[i].lotNo;
                newRow["Type"] = listlotinfo[i].type;
                newRow["MBCode"] = listlotinfo[i].mbcode;
                newRow["Qty"] = listlotinfo[i].qty;
                dt.Rows.Add(newRow);
                iTotalPCS = iTotalPCS + listlotinfo[i].qty;
            }
            if (listlotinfo.Count < initRowsCountForLot)
            {
                for (int i = listlotinfo.Count; i < initRowsCountForLot; i++)
                {
                    newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                }
            }


            this.GridViewLot.DataSource = dt;
            this.GridViewLot.DataBind();
            initTableColumnHeaderforLot();
            this.txtTotalPCS.Text = Convert.ToString(iTotalPCS);

            updatePanel3.Update();
            updatePanelPCS.Update();
        }
        else
        {
            DataTable dt = initTableForLot();
            DataRow newRow;
            for (int i = 0; i < initRowsCountForLot; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
            this.GridViewLot.DataSource = dt;
            this.GridViewLot.DataBind();
            initTableColumnHeaderforLot();
            this.txtTotalPCS.Text = "";
            updatePanel3.Update();
            updatePanelPCS.Update();
        }
        var script = "<script language='javascript'>" + "\r\n" +
                 "window.setTimeout (ChangeFocus,100);" + "\r\n" +
                 "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "ChangeFocus", script, false);

    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbpdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbtestStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();
        this.lbPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lbFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lbMBSn.Text = this.GetLocalResourceObject(Pre + "_lblMBSn").ToString();
        this.lb111.Text = this.GetLocalResourceObject(Pre + "_lbl111").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lbDefectList.Text = this.GetLocalResourceObject(Pre + "_lblDefectList").ToString();
        this.nineSelect.Text = this.GetLocalResourceObject(Pre + "_nineSelect").ToString();//"Dont Scan 9999";
        this.lblotlist.Text = this.GetLocalResourceObject(Pre + "_lblotlist").ToString();
        this.lbTotalPCS.Text = this.GetLocalResourceObject(Pre + "_lbTotalPCS").ToString();
        this.FruCheck.Text = this.GetLocalResourceObject(Pre + "_FruCheck").ToString();//"Dont Scan 9999";
        this.txtTotalPCS.Text = "";
        setFocus();
      
    }

    private DataTable getNullDataTable()
    {
       DataTable dt = initTable();
       DataRow newRow;
       for (int i = 0; i < initRowsCount; i++)
       {
            newRow = dt.NewRow();
            newRow["DefectId"] = String.Empty;
            newRow["DefectDescr"] = String.Empty;
            dt.Rows.Add(newRow);
       }
       
       return dt;
    }
    private DataTable getNullDataTableForMBCode()
    {
        DataTable dt = initTableForMBCode();
        DataRow newRow;
        for (int i = 0; i < initRowsCount; i++)
        {
            newRow = dt.NewRow();
            newRow["MBCode"] = String.Empty;
            newRow["PassQty"] = String.Empty;
            dt.Rows.Add(newRow);
        }

        return dt;
    }
    private DataTable getNullDataTableForLot()
    {
        DataTable dt = initTableForLot();
        DataRow newRow;
        for (int i = 0; i < initRowsCountForLot; i++)
        {
            newRow = dt.NewRow();
            newRow["LotNo"] = String.Empty;
            newRow["Type"] = String.Empty;
            newRow["MBCode"] = String.Empty;
            newRow["Qty"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    /// <summary>
    /// 初始化列类型
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
       DataTable retTable  = new DataTable();
       retTable.Columns.Add("DefectId", Type.GetType("System.String"));
       retTable.Columns.Add("DefectDescr", Type.GetType("System.String"));
      
        //this.retTable.HeaderRow.Cells[0].Width = Unit.Percentage(30); 
       return retTable;
    }

    private DataTable initTableForMBCode()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("MBCode", Type.GetType("System.String"));
        retTable.Columns.Add("PassQty", Type.GetType("System.String"));

        //this.retTable.HeaderRow.Cells[0].Width = Unit.Percentage(30); 
        return retTable;
    }
    private DataTable initTableForLot()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("LotNo", Type.GetType("System.String"));
        retTable.Columns.Add("Type", Type.GetType("System.String"));
        retTable.Columns.Add("MBCode", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));

        //this.retTable.HeaderRow.Cells[0].Width = Unit.Percentage(30); 
        return retTable;
    }
    /// <summary>
    /// 设置表格列名称及宽度
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {
        this.gridview.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_lblDefectCode").ToString();
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        this.gridview.Columns[0].ItemStyle.Width = Unit.Percentage(30);

        this.gridview.Columns[1].ItemStyle.Width = Unit.Percentage(70);

        //this.gridview.HeaderRow.Cells[0].Width = Unit.Percentage(30);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
    }
    private void initTableColumnHeaderforMBCode()
    {
        this.gridviewMB.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_tblMBCode").ToString();
        this.gridviewMB.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_tblpassqty").ToString();
        this.gridviewMB.Columns[0].ItemStyle.Width = Unit.Percentage(50);

        this.gridviewMB.Columns[1].ItemStyle.Width = Unit.Percentage(50);

        //this.gridview.HeaderRow.Cells[0].Width = Unit.Percentage(30);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
    }
    private void initTableColumnHeaderforLot()
    {
        this.GridViewLot.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_tblLotNo").ToString();
        this.GridViewLot.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_tblType").ToString();
        this.GridViewLot.Columns[2].HeaderText = this.GetLocalResourceObject(Pre + "_tblMBCode").ToString();
        this.GridViewLot.Columns[3].HeaderText = this.GetLocalResourceObject(Pre + "_tblQty").ToString();

        //this.gridviewMB.Columns[0].ItemStyle.Width = Unit.Percentage(50);
        //this.gridviewMB.Columns[1].ItemStyle.Width = Unit.Percentage(50);

        //this.gridview.HeaderRow.Cells[0].Width = Unit.Percentage(30);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
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
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewLot_RowDataBound(object sender, GridViewRowEventArgs e)
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
    /// <summary>
    /// 为表格列加tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExtMB_RowDataBound(object sender, GridViewRowEventArgs e)
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
      /// <summary>
    ///置焦点
    /// </summary>  
    private void setFocus()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (setTestStationCmbFocus,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setTestStationCmbFocus", script, false);
    }


    public void btnRefresh_Click(object sender, System.EventArgs e)
    {
        changeLottable();
    }
    /// <summary>
    /// reset页面信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            //this.cmbMBCode.InnerDropDownList.SelectedIndex = 0;

            //重置mbCode下拉框,并触发它的选择事件
            this.cmbTestStation.setSelected(0);
            cmbTestStation_Selected(sender, e);
            setFocus();
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
    /// btnFru_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnFru_Click(object sender, System.EventArgs e)
    {
        //DEBUG ITC-1360-0358 ADD Server method refresh Pdline
        try
        {
            var selectLine = cmbPdLine.InnerDropDownList.SelectedItem.Text;
            this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, this.customerHidden.Value);
            int i = 0;
            foreach (ListItem lst in this.cmbPdLine.InnerDropDownList.Items)
            {
                if (selectLine == lst.Text)
                {
                    cmbPdLine.InnerDropDownList.SelectedIndex = i;
                    return;
                }
                i++;
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
    /// nineSelect_CheckedChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void nineSelect_CheckedChanged(object sender, EventArgs e)
    {
        if (nineSelect.Checked == true)
        {

        }
        else
        {

        }

    }
}
