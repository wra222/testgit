/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for Const Value Maintain Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-8-6     Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Collections;


public partial class DataMaintain_Region : System.Web.UI.Page
{    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    //private IConstValueMaintain iConstValueMaintain;
    private IRegion iRegion;
    Boolean isConstValueTypeLoad;

    private const int COL_NUM = 7;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public String type2;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            isConstValueTypeLoad = false;
            this.cmbCustomerTop.InnerDropDownList.Load += new EventHandler(cmbCustomerTop_Load);
            
            iRegion = ServiceAgent.getInstance().GetMaintainObjectByName<IRegion>(WebConstant.IRegion);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                initcustomer();
                bindTable(null, DEFAULT_ROWS);
            }

            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "initControls();", true);
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

    public void initcustomer()
    {
        this.cmbCustomer.Items.Clear();
        this.cmbCustomer.Items.Add(string.Empty);
        ICustomer iCustomer = ServiceAgent.getInstance().GetMaintainObjectByName<ICustomer>(WebConstant.MaintainCommonObject);
        IList<CustomerInfo> lst = iCustomer.GetCustomerList();
        foreach (CustomerInfo item in lst)
        {
            ListItem value = new ListItem();
            value.Text = item.customer;
            value.Value = item.customer;
            this.cmbCustomer.Items.Add(value);
        }
    }

    protected void btnRegionChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowListByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "RegionChange", "setNewItemValue();DealHideWait();", true);
    }

    protected void btnRegionTopListUpdate_ServerClick(object sender, System.EventArgs e)
    {
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnRegionTopListUpdate", "setNewItemValue();DealHideWait();", true);
    }

    private void cmbCustomerTop_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbCustomerTop.IsPostBack)
        {
            
        }
        isConstValueTypeLoad = true;
        CheckAndStart();
    }

    private void CheckAndStart()
    {
        if (isConstValueTypeLoad == true)
        {
            ShowListByType();
        }
    }

    private Boolean ShowListByType()
    {
        String type =selecttype.Value;
        type2 = this.cmbCustomerTop.InnerDropDownList.SelectedValue;
        try
        {
            RegionInfo condition = new RegionInfo();
            condition.Customer = type2;
            IList<RegionInfo> dataList = iRegion.GetRegionList(condition);
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
            //show error
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }

        return true;

    }

    private void ShowMessage(string p)
    {
        throw new NotImplementedException(p);
    }

    private void initLabel()
    {
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(7);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string customer = this.cmbCustomer.SelectedValue;
        string region = this.txtRegion.Text.Trim().ToUpper();
        RegionInfo checkRegion = new RegionInfo();
        checkRegion.Customer = customer;
        checkRegion.Region = region;
        RegionInfo item = new RegionInfo();
        item.Customer = customer;
        item.Region = region;
        item.RegionCode = this.txtRegionCode.Text.Trim().ToUpper();
        item.Description = this.txtDescr.Text.Trim();
        item.Editor = this.HiddenUserName.Value; 
        try
        {
            IList<RegionInfo> checkList = iRegion.GetRegionList(checkRegion);
            if (checkList.Count == 0)
            {
                item.Cdt = DateTime.Now;
                item.Udt = DateTime.Now;
                iRegion.InsertRegion(item);
            }
            else
            {
                item.Cdt = checkList[0].Cdt;
                item.Udt = DateTime.Now;
                iRegion.UpdateRegion(checkRegion, item);
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

        ShowListByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string type = selecttype.Value;
        try
        {
            RegionInfo item = new RegionInfo();
            item.Customer = this.cmbCustomer.SelectedValue;
            item.Region = this.txtRegion.Text.ToUpper().Trim();
            iRegion.DeleteRegion(item);
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

        ShowListByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<RegionInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Customer");  //0
        dt.Columns.Add("Region");  //1
        dt.Columns.Add("RegionCode");  //2
        dt.Columns.Add("Description");  //3
        dt.Columns.Add("Editor");  //4
        dt.Columns.Add("Cdt");  //5
        dt.Columns.Add("Udt");  //6

       if (list != null && list.Count != 0)
        {
            foreach (RegionInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Customer;
                dr[1] = temp.Region;
                dr[2] = temp.RegionCode;
                dr[3] = temp.Description;
                dr[4] = temp.Editor;
                dr[5] = temp.Cdt;
                dr[6] = temp.Udt;
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
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true); 
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
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
