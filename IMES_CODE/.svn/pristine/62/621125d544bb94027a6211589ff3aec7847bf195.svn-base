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
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;

//ITC-1136-0015 2010,2,4

public partial class DataMaintain_SpecialOrder : System.Web.UI.Page
{
    public String userName;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private ISpecialOrder iSpecialOrder;
    private const int COL_NUM = 9;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;
    public string pmtMessage10;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            iSpecialOrder = ServiceAgent.getInstance().GetMaintainObjectByName<ISpecialOrder>(WebConstant.SpecialOrderObject);
            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
                pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
                pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
                pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
                pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
                userName = Master.userInfo.UserId;
                initLabel();
                initcmb();
                bindTable(null, DEFAULT_ROWS);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    private Boolean ShowSpecialOrderList(string FactoryPO, string Category, SpecialOrderStatus SpecialOrderStatus, DateTime StartTime, DateTime EndTime)
    {
        try
        {
            IList<SpecialOrderInfo> dataList = new List<SpecialOrderInfo>();
            if (FactoryPO != "")
            {
                dataList = iSpecialOrder.GetSpecialOrderByPO(FactoryPO);
            }
            else
            {
                dataList = iSpecialOrder.GetSpecialOrder(Category, SpecialOrderStatus, StartTime, EndTime);
            }
            if (dataList == null || dataList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataList, DEFAULT_ROWS);
            }
        }
        catch (FisException ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        setColumnWidth();
        return true;
    }

    private void ShowMessage(string p)
    {
        throw new NotImplementedException(p);
    }

    private void initLabel()
    {
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatus").ToString();
        this.btnAdd.Value  = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }

    private void initcmb()
    {
        IList<ConstValueTypeInfo> lstConstValueType = iSpecialOrder.GetConstValueTypeList("SpecialOrderCategory");
        ListItem item = null;
        this.cmbCategoryTOP.Items.Clear();
        this.cmbCategory.Items.Clear();
        this.cmbCategoryTOP.Items.Add(string.Empty);
        this.cmbCategory.Items.Add(string.Empty);
        if (lstConstValueType != null)
        {
            foreach (ConstValueTypeInfo temp in lstConstValueType)
            {
                item = new ListItem();
                item.Text = temp.value;
                item.Value = temp.value;
                this.cmbCategoryTOP.Items.Add(item);
                this.cmbCategory.Items.Add(item);
            }
            this.cmbCategoryTOP.SelectedIndex = 0;
            this.cmbCategory.SelectedIndex = 0;
        }
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(10);
    }

    protected void btnQuery_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string FactoryPO = this.txtFactoryPOTOP.Value.Trim();
            string Category = this.dCategoryTOP.Value.Trim();
            string Status = this.dStatusTOP.Value.Trim();
            DateTime StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            DateTime EndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            SpecialOrderStatus SpecialOrderStatus = new SpecialOrderStatus();
            if (FactoryPO == "")
            {
                switch(Status){
                    case "Created":
                        SpecialOrderStatus = SpecialOrderStatus.Created;
                        break;
                    case "Active":
                        SpecialOrderStatus = SpecialOrderStatus.Active;
                        break;
                    case "Closed":
                        SpecialOrderStatus = SpecialOrderStatus.Closed;
                        break;
                }
                if (this.dStartTime.Value.Trim() != "" && this.dEndTime.Value.Trim() != "")
                {
                    StartTime = Convert.ToDateTime(this.dStartTime.Value.Trim());
                    EndTime = Convert.ToDateTime(this.dEndTime.Value.Trim() + " 23:59:59");
                }
            }
            ShowSpecialOrderList(FactoryPO,Category, SpecialOrderStatus, StartTime, EndTime);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Query", "DealHideWait();", true);
    }
    
    protected void btnUpLoadQuery_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string FactoryPO = "";
            string Category = this.dCategoryTOP.Value.Trim();
            SpecialOrderStatus SpecialOrderStatus = SpecialOrderStatus.Created;
            DateTime StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            DateTime EndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            ShowSpecialOrderList(FactoryPO,Category, SpecialOrderStatus, StartTime, EndTime);
            string errordate = this.dUploadResultData.Value.Trim();
            if (errordate != "")
            {
                showErrorMessage(errordate);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Query", "DealHideWait();", true);//
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        SpecialOrderInfo item = new SpecialOrderInfo();
        SpecialOrderStatus OrderStatus = SpecialOrderStatus.Created;
        string OldFactoryPO = this.dFactoryPO.Value.Trim();
        item.Editor = this.HiddenUserName.Value;
        item.FactoryPO = this.txtFactoryPO.Value.Trim();
        item.Category = this.dCategory.Value.Trim();
        item.AssetTag = this.txtAssetTag.Value.Trim();
        item.Qty = Convert.ToInt32(this.txtQty.Value);
        item.Status = OrderStatus;
        item.Remark = this.txtRemark.Value.Trim();
        try
        {
            if (item.FactoryPO == OldFactoryPO)
            {
                iSpecialOrder.UpdateSpecialOrder(item);
            }
            else
            {
                iSpecialOrder.InsertSpecialOrder(item);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        btnQuery_ServerClick(sender, e);
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + item.FactoryPO + "');DealHideWait();", true);//
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        SpecialOrderInfo item = new SpecialOrderInfo();
        SpecialOrderStatus OrderStatus = SpecialOrderStatus.Created;
        item.Editor = this.HiddenUserName.Value;
        item.FactoryPO = this.txtFactoryPO.Value.Trim();
        item.Category = this.dCategory.Value.Trim();
        item.AssetTag = this.txtAssetTag.Value.Trim();
        item.Qty = Convert.ToInt32(this.txtQty.Value);
        item.Status = OrderStatus;
        item.Remark = this.txtRemark.Value.Trim();
        try
        {
            if (iSpecialOrder.ExistSpecialOrder(item.FactoryPO))
            {
                showErrorMessage("[" + item.FactoryPO + "]:此FactoryPO已經存在...");
                return;
            }
            else
            {
                iSpecialOrder.InsertSpecialOrder(item);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        btnQuery_ServerClick(sender, e);
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + item.FactoryPO + "');DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string FactoryPO = this.dFactoryPO.Value.Trim();
        try
        {
            iSpecialOrder.DeleteSpecialOrder(FactoryPO);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        btnQuery_ServerClick(sender, e);
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<SpecialOrderInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("FactoryPO");
        dt.Columns.Add("Category");
        dt.Columns.Add("AssetTag");
        dt.Columns.Add("Qty");
        dt.Columns.Add("Status");
        dt.Columns.Add("Remark");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        if (list != null && list.Count != 0)
        {
            foreach (SpecialOrderInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.FactoryPO;
                dr[1] = temp.Category;
                dr[2] = temp.AssetTag;
                dr[3] = temp.Qty;
                dr[4] = temp.Status;
                dr[5] = temp.Remark;
                dr[6] = temp.Editor;
                dr[7] = temp.Cdt;
                dr[8] = temp.Udt;
                dt.Rows.Add(dr);
            }
            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            this.hidRecordCount.Value = list.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            this.hidRecordCount.Value = "";
        }
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;        
        gd.DataBind();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true); 
    }
    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }
}
