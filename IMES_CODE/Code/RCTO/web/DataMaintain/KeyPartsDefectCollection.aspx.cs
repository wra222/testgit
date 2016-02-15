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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using System.Globalization;
using com.inventec.system.util;
using IMES.Maintain.Interface.MaintainIntf;


public partial class DataMaintain_KeyPartsDefectCollection : IMESBasePage
{
    //private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IKeyPartsDefectCollection iKeyPartDefectCollection;
    private const int DEFAULT_ROWS = 36;
    private const int COL_NUM = 14;
    public string today;
    public String userName;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iKeyPartDefectCollection = ServiceAgent.getInstance().GetMaintainObjectByName<IKeyPartsDefectCollection>(WebConstant.KeyPartsDefectCollection);
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            if (!this.IsPostBack)
            {
                today = DateTime.Now.ToString("yyyy-MM-dd");
                initLabel(sender,e);
                ShowRunInTimeControlList();
                
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



    #region 页面初始化方法
    private void initLabel(object sender, System.EventArgs e)
    {
        GetPdLineList( sender, e);

        GetFamilyList();

        GetPartNameList();

        GetVendorList();

        GetModuleNO();

        GetFailReason();

    }

    public void btnDateChange_Click(object sender, System.EventArgs e)
    {
        try
        {
            ShowRunInTimeControlList();
        }
        catch (FisException ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
        }
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "select2", "resetTableHeight();", true);
    }

    protected void cmbPdLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
            ShowRunInTimeControlList();
        }
        catch (FisException ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
        }
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "select", "resetTableHeight();", true);
    }

    public void GetPdLineList(object sender, System.EventArgs e)
    {
        IList<ConstValueTypeInfo> list = new List<ConstValueTypeInfo>();
        ListItem item = null;
        string Type = "KPDefectPdLine";
        list = iKeyPartDefectCollection.GetConstValueTypeList(Type);
        this.cmbPdLine.Items.Clear();
        this.cmbPdLine.Items.Add(string.Empty);
        if (list != null)
        {
            foreach (ConstValueTypeInfo temp in list)
            {
                item = new ListItem(temp.value.Trim(), temp.value.Trim().ToString());
                this.cmbPdLine.Items.Add(item);
            }
        }
        this.UpdatePanel2.Update();
        if (this.cmbPdLine.Items.Count > 1)
        {
            this.cmbPdLine.SelectedIndex = 1;
            cmbPdLine_Selected(sender,e);
        }
    }

    public void GetFamilyList()
    {
        IList<ConstValueTypeInfo> list = new List<ConstValueTypeInfo>();
        ListItem item = null;
        string Type = "KPDefectFamily";
        list = iKeyPartDefectCollection.GetConstValueTypeList(Type);
        this.cmbFamily.Items.Clear();
        this.cmbFamily.Items.Add(string.Empty);
        if (list != null)
        {
            foreach (ConstValueTypeInfo temp in list)
            {
                item = new ListItem(temp.value.Trim(), temp.value.Trim().ToString());
                this.cmbFamily.Items.Add(item);
            }
        }
        this.UpdatePanel3.Update();
    }

    public void GetPartNameList()
    {
        IList<ConstValueTypeInfo> list = new List<ConstValueTypeInfo>();
        ListItem item = null;
        string Type = "KPDefectParts";
        list = iKeyPartDefectCollection.GetConstValueTypeList(Type);
        this.cmbPartName.Items.Clear();
        this.cmbPartName.Items.Add(string.Empty);
        if (list != null)
        {
            foreach (ConstValueTypeInfo temp in list)
            {
                item = new ListItem(temp.value.Trim(), temp.value.Trim().ToString());
                this.cmbPartName.Items.Add(item);
            }
        }
        this.UpdatePanel4.Update();
    }

    public void GetVendorList()
    {
        IList<ConstValueTypeInfo> list = new List<ConstValueTypeInfo>();
        ListItem item = null;
        string Type = "KPDefectVendor";
        list = iKeyPartDefectCollection.GetConstValueTypeList(Type);
        this.cmbVendor.Items.Clear();
        this.cmbVendor.Items.Add(string.Empty);
        if (list != null)
        {
            foreach (ConstValueTypeInfo temp in list)
            {
                item = new ListItem(temp.value.Trim(), temp.value.Trim().ToString());
                this.cmbVendor.Items.Add(item);
            }
        }
        this.UpdatePanel5.Update();
    }

    public void GetModuleNO() 
    {
        IList<ConstValueTypeInfo> list = new List<ConstValueTypeInfo>();
        ListItem item = null;
        string Type = "KPDefectModule";
        list = iKeyPartDefectCollection.GetConstValueTypeList(Type);
        this.cmbModuleNo.Items.Clear();
        this.cmbModuleNo.Items.Add(string.Empty);
        if (list != null)
        {
            foreach (ConstValueTypeInfo temp in list)
            {
                item = new ListItem(temp.value.Trim(), temp.value.Trim().ToString());
                this.cmbModuleNo.Items.Add(item);
            }
        }
        this.UpdatePanel6.Update();   
    }

    public void GetFailReason()
    {
        IList<ConstValueTypeInfo> list = new List<ConstValueTypeInfo>();
        ListItem item = null;
        string Type = "KPDefectReason";
        list = iKeyPartDefectCollection.GetConstValueTypeList(Type);
        this.cmbFailReason.Items.Clear();
        this.cmbFailReason.Items.Add(string.Empty);
        if (list != null)
        {
            foreach (ConstValueTypeInfo temp in list)
            {
                item = new ListItem(temp.value.Trim(), temp.value.Trim().ToString());
                this.cmbFailReason.Items.Add(item);
            }
        }
        this.UpdatePanel7.Update();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(3);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[12].Width = Unit.Percentage(11);
        gd.HeaderRow.Cells[13].Width = Unit.Percentage(11);
    }

    protected void gd_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

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
    #endregion


    #region 系统方法
    private void bindTable(IList<FailKPCollectionInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("ID").ToString();
        dt.Columns.Add("日期").ToString();
        dt.Columns.Add("線別").ToString();
        dt.Columns.Add("機型").ToString();
        dt.Columns.Add("材料").ToString();
        dt.Columns.Add("材料號").ToString();
        dt.Columns.Add("廠商").ToString();
        dt.Columns.Add("模號").ToString();
        dt.Columns.Add("現象").ToString();
        dt.Columns.Add("數量").ToString();
        dt.Columns.Add("備註").ToString();
        dt.Columns.Add("Editor").ToString();
        dt.Columns.Add("Cdt").ToString();
        dt.Columns.Add("Udt").ToString();

        if (list != null && list.Count != 0)
        {
            foreach (FailKPCollectionInfo temp in list)
            {

                dr = dt.NewRow();
                dr[0] = temp.ID;
                dr[1] = temp.Date.ToString("yyyy-MM-dd");
                dr[2] = temp.PdLine.ToString();
                dr[3] = temp.Family.ToString();
                dr[4] = temp.PartName.ToString();
                dr[5] = temp.PartNo.ToString();
                dr[6] = temp.Vendor.ToString();
                dr[7] = temp.Module.ToString();
                dr[8] = temp.FailReason.ToString();
                dr[9] = temp.Qty;
                dr[10] = temp.Remark;
                dr[11] = temp.Editor;
                dr[12] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[13] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
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

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        this.UpdatePanel1.Update();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorMsg"></param>
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceData"></param>
    /// <returns></returns>
    private static String replaceSpecialChart(String sourceData)
    {
        if (sourceData == null)
        {
            return "";
        }
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }
    #endregion


    #region 增删改查方法
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        int id = Convert.ToInt32( this.hidId.Value.Trim());
        try
        {
            iKeyPartDefectCollection.Delete(id);
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
        ShowRunInTimeControlList();
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "delete", "resetTableHeight();", true);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string id = this.hidId.Value.Trim();
        FailKPCollectionInfo items = new FailKPCollectionInfo();
        try
        {
            items.Family = this.cmbFamily.SelectedValue.ToString();
            items.PdLine = this.cmbPdLine.SelectedValue.ToString();
            items.PartName = this.cmbPartName.SelectedValue.ToString();
            items.PartNo = this.txtPartNO.Text.ToString();
            items.Vendor = this.cmbVendor.SelectedValue.ToString();
            items.Module = this.cmbModuleNo.SelectedValue.ToString();
            items.FailReason = this.cmbFailReason.SelectedValue.ToString();
            items.Qty = Convert.ToInt32( this.txtQty.Text.ToString());
            items.Remark = this.txtRemark.Text.ToString();
            items.Editor = userName.ToString();
            if (id == "" || id == null)
            {
                items.Date = Convert.ToDateTime( this.hidshipdate.Value.ToString());
                iKeyPartDefectCollection.AddLine(items);
            }
            else
            {
                items.ID =Convert.ToInt32(id);
                items.Date = Convert.ToDateTime(this.hidselecttimeforsave.Value.ToString());
                iKeyPartDefectCollection.UpdateLine(items);
            }
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
        ShowRunInTimeControlList();
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Update", "resetTableHeight();", true);

    }

    private Boolean ShowRunInTimeControlList()
    {
        try
        {
            DateTime ShipDate = new DateTime();
            IList<FailKPCollectionInfo> list = new List<FailKPCollectionInfo>();
            string line = this.cmbPdLine.SelectedValue.ToString();
            if (this.hidshipdate.Value != "")
            {
                ShipDate = Convert.ToDateTime(this.hidshipdate.Value);
            }
            if (this.cmbPdLine.SelectedIndex != 0 && line != "" && this.hidshipdate.Value != "")
            {
                list = iKeyPartDefectCollection.GetDefectPartList(ShipDate, line);
                if (list.Count > 0)
                {
                    bindTable(list, DEFAULT_ROWS);
                }
                else
                {
                    bindTable(null, DEFAULT_ROWS);
                }
            }
            else
            {
                bindTable(null, DEFAULT_ROWS);    
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
            //show error
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }
    #endregion






}
