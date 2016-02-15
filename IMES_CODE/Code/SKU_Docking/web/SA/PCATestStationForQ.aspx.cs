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

public partial class SA_PCATestStation : IMESBasePage 
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
                
                ////将pdLine combo box的初始化参数传入
                //this.cmbPdLine.Station = Request["Station"];
                //this.cmbPdLine.Station = "20";
                this.cmbTestStation.Type = "SATEST";
                this.cmbTestStation.Station = "SAForQ";
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
    public void GetLotTable()
    {
            IPCATestStation iPCATestStation = ServiceAgent.getInstance().GetObjectByName<IPCATestStation>(WebConstant.PCATestStationObject);
            IList<LotInfo> listlotinfo = iPCATestStation.GetLotInfoLst(this.cmbPdLine.InnerDropDownList.SelectedValue);
            int iTotalPCS = 0;
            if (listlotinfo != null && listlotinfo.Count > 0)
            {
                DataTable dt = initTable();
                DataRow newRow;
                //for (int i = 0; i<listlotinfo.Count;i++)
                for (int i = listlotinfo.Count-1; i >= 0 ; i--)
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


                this.gridview.DataSource = dt;
                this.gridview.DataBind();
                initTableColumnHeader();
                this.txtTotalPCS.Text = Convert.ToString(iTotalPCS);
                
                updatePanel4.Update();
                updatePanelPCS.Update();
            }
            else
            {
                DataTable dt = initTable();
                DataRow newRow;
                for (int i = 0; i < initRowsCountForLot; i++)
                {
                    newRow = dt.NewRow();
                    dt.Rows.Add(newRow);
                }
                this.gridview.DataSource = dt;
                this.gridview.DataBind();
                initTableColumnHeader();
                this.txtTotalPCS.Text = "";

                updatePanel4.Update();
                updatePanelPCS.Update();
            }
    }

    public void updateLotTable(IList<LotInfo> listlotinfo)
    {
        int iTotalPCS = 0;
        if (listlotinfo != null && listlotinfo.Count > 0)
        {
            DataTable dt = initTable();
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


            this.gridview.DataSource = dt;
            this.gridview.DataBind();
            initTableColumnHeader();
            this.txtTotalPCS.Text = Convert.ToString(iTotalPCS);

            updatePanel4.Update();
            updatePanelPCS.Update();
        }
        else
        {
            DataTable dt = initTable();
            DataRow newRow;
            for (int i = 0; i < initRowsCountForLot; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
            this.gridview.DataSource = dt;
            this.gridview.DataBind();
            initTableColumnHeader();
            this.txtTotalPCS.Text = "";

            updatePanel4.Update();
            updatePanelPCS.Update();
        }
    }

    private void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        writeToNullMessage("");
        if (this.cmbPdLine.InnerDropDownList.SelectedValue != "")
        {
            GetLotTable();   
        }
        else
        {
            DataTable dt = initTable();
            DataRow newRow;
            for (int i = 0; i < initRowsCountForLot; i++)
            {
                newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
            this.gridview.DataSource = dt;
            this.gridview.DataBind();
            initTableColumnHeader();
            this.txtTotalPCS.Text = "";

            updatePanel4.Update();
            updatePanelPCS.Update();
        }
    }
    /// <summary>
    /// 选择pdLIne或者testStation下拉框，会reset界面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmbTestStation_Selected(object sender, System.EventArgs e)
    {
        //DEBUG ITC-1414-0003 2012/05/31 clear message
        writeToNullMessage("");
        try
        {
            if (this.cmbTestStation.InnerDropDownList.SelectedValue == "")
            { this.cmbPdLine.clearContent(); }
            else
            {
                var selecttstation = cmbTestStation.InnerDropDownList.SelectedValue;
                
                var selectLine = cmbPdLine.InnerDropDownList.SelectedItem.Text;
                this.cmbPdLine.refresh(this.cmbTestStation.InnerDropDownList.SelectedValue, this.customerHidden.Value);
                int i = 0;
                foreach (ListItem lst in this.cmbPdLine.InnerDropDownList.Items)
                {
                    if (selectLine == lst.Text)
                    {
                        cmbPdLine.InnerDropDownList.SelectedIndex = i;
                        GetLotTable(); 
                        return;
                    }
                    i++;
                }
    
            }
            GetLotTable(); 

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
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lbpdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lbtestStation.Text = this.GetLocalResourceObject(Pre + "_lblTestStation").ToString();
        this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.lblotlist.Text = this.GetLocalResourceObject(Pre + "_lblotlist").ToString();
        this.lbTotalPCS.Text = this.GetLocalResourceObject(Pre + "_lbTotalPCS").ToString();
        this.txtTotalPCS.Text = "";
        setFocus();
      
    }


    private DataTable getNullDataTable()
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < initRowsCountForLot; i++)
        {
            newRow = dt.NewRow();
            newRow["ChkAll"] = String.Empty;
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
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ChkAll", Type.GetType("System.String"));
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
        this.gridview.Columns[0].HeaderText = "";
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_tblLotNo").ToString();
        this.gridview.Columns[2].HeaderText = this.GetLocalResourceObject(Pre + "_tblType").ToString();
        this.gridview.Columns[3].HeaderText = this.GetLocalResourceObject(Pre + "_tblMBCode").ToString();
        this.gridview.Columns[4].HeaderText = this.GetLocalResourceObject(Pre + "_tblQty").ToString();
        this.gridview.Columns[0].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Pixel(44);
        this.gridview.Columns[0].HeaderStyle.Wrap = false;
        this.gridview.Columns[1].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(24);
        this.gridview.Columns[2].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(24);
        this.gridview.Columns[3].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(24);
        this.gridview.Columns[4].HeaderStyle.Width = System.Web.UI.WebControls.Unit.Percentage(23);  


        //this.gridview.Columns[0].ItemStyle.Width = Unit.Percentage(10);
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

    private void writeTouccessfulInfo(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowSuccessfulInfo(true);");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeTouccessfulInfo", scriptBuilder.ToString(), false);

    }
    private void writeToNullMessage(string errorMsg)
    {


        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToNullMessage", scriptBuilder.ToString(), false);

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
            if (String.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) || (e.Row.Cells[1].Text.Trim().ToLower() == "&nbsp;"))
            {
                CheckBox check = (CheckBox)e.Row.FindControl("RowChk");
                check.Style.Add("display", "none");
            }
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

    /// <summary>
    /// reset页面信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnReset_Click(object sender, System.EventArgs e)
    {
        try
        {
            string[] lotstringlist = this.txtLotnolst.Value.Split('+');
            IList<string> resultlot = new List<string>();
            for (int i = 0; i < lotstringlist.Length; i++)
            {
                if (lotstringlist[i].Trim() !="")
                   resultlot.Add(lotstringlist[i]);
            }
            IPCATestStation iPCATestStation = ServiceAgent.getInstance().GetObjectByName<IPCATestStation>(WebConstant.PCATestStationObject);
            IList<LotInfo> listlotinfo = iPCATestStation.UpdateSelectLotStatus(resultlot,this.cmbPdLine.InnerDropDownList.SelectedValue);
            updateLotTable(listlotinfo);
            //DEBUG ITC-1414-0006  ADD success info 2012/05/31
            writeTouccessfulInfo("");

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


 
}
