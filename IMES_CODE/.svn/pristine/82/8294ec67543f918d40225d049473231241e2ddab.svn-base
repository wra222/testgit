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
using System.Linq;


public partial class DataMaintain_CheckItemType : System.Web.UI.Page
{    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private ICheckItemType iCheckItemType;
    Boolean isCheckItemTypeLoad;

    private const int COL_NUM = 12;
    public string pmtMessage4;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            isCheckItemTypeLoad = false;
            this.btnCheckItemTypeChange.Load += new EventHandler(cmbCheckItemType_Load);//btnCheckItemTypeChange
            iCheckItemType = ServiceAgent.getInstance().GetMaintainObjectByName<ICheckItemType>(WebConstant.MaintainCheckItemTypeObject);
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                initcmbCheckItemTypeList();
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

    protected void btnCheckItemTypeChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowListByType();
        //this.updatePanel1.Update();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnCheckItemTypeChange", "setNewItemValue();DealHideWait();", true);
    }

    protected void btnTypeListUpdate_ServerClick(object sender, System.EventArgs e)
    {
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnTypeListUpdate", "setNewItemValue();DealHideWait();", true);
    }

    private void cmbCheckItemType_Load(object sender, System.EventArgs e)
    {
        if (!this.IsPostBack)
        {
            isCheckItemTypeLoad = true;
            CheckAndStart();
        }
    }

    private void CheckAndStart()
    {
        if (isCheckItemTypeLoad == true)
        {
           ShowListByType();
        }
    }

    private Boolean ShowListByType()
    {
        string type = this.cmbCheckItemType.SelectedValue;
        try
        {
            CheckItemTypeInfo condition = new CheckItemTypeInfo();
            if (type != "ALL")
            {
                condition.name = type;
            }
            IList<CheckItemTypeInfo> dataList = iCheckItemType.GetCheckItemTypeByCondition(condition);
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
        this.lbltopName.Text = this.GetLocalResourceObject(Pre + "_lbltopName").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.lblCheckItemType.Text = this.GetLocalResourceObject(Pre + "_lblCheckItemType").ToString();
        this.lblDisPlayName.Text = this.GetLocalResourceObject(Pre + "_lblDisPlayName").ToString();
        this.lblCheckModule.Text = this.GetLocalResourceObject(Pre + "_lblCheckModule").ToString();
        this.lblFilterModule.Text = this.GetLocalResourceObject(Pre + "_lblFilterModule").ToString();
        this.lblMatchModule.Text = this.GetLocalResourceObject(Pre + "_lblMatchModule").ToString();
        this.lblSaveModule.Text = this.GetLocalResourceObject(Pre + "_lblSaveModule").ToString();
        this.lblNeedUniqueCheck.Text = this.GetLocalResourceObject(Pre + "_lblNeedUniqueCheck").ToString();
        this.lblNeedCommonSave.Text = this.GetLocalResourceObject(Pre + "_lblNeedCommonSave").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

        this.cmbNeedCommonSave.Items.Add(new ListItem("True", "True"));
        this.cmbNeedCommonSave.Items.Add(new ListItem("False", "False"));
        this.cmbNeedCommonSave.SelectedIndex = 0;
        this.cmbNeedUniqueCheck.Items.Add(new ListItem("True", "True"));
        this.cmbNeedUniqueCheck.Items.Add(new ListItem("False", "False"));
        this.cmbNeedUniqueCheck.SelectedIndex = 0;
        this.cmbNeedPartForbidCheck.Items.Add(new ListItem("True", "True"));
        this.cmbNeedPartForbidCheck.Items.Add(new ListItem("False", "False"));
        this.cmbNeedPartForbidCheck.SelectedIndex = 0;
    }

    private void initcmbCheckItemTypeList()
    {
        this.cmbCheckItemType.Items.Clear();
        //this.cmbCheckItemType.Items.Add(string.Empty);
        this.cmbCheckItemType.Items.Add("ALL");
        CheckItemTypeInfo condition = new CheckItemTypeInfo();
        IList<CheckItemTypeInfo> chcekItemTypeInfoList = iCheckItemType.GetCheckItemTypeByCondition(condition);
        IList<ListItem> inneritem = new List<ListItem>();
        foreach (CheckItemTypeInfo info in chcekItemTypeInfoList)
        {
            ListItem item = new ListItem();
            item.Text = info.name;
            item.Value = info.name;
            inneritem.Add(item);
            this.cmbCheckItemType.Items.Add(item);
        }
        this.cmbCheckItemType.SelectedIndex = 0;
        ShowListByType();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(8);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        CheckItemTypeInfo addEditInfo = new CheckItemTypeInfo();
        string hidChcekItemType = this.hidChcekItemType.Value.Trim();
        string chcekItemType = this.txtCheckItemType.Text.Trim();
        string displayName = this.txtDisPlayName.Text.Trim();
        string filterModule = this.txtFilterModule.Text.Trim();
        string matchModule= this.txtMatchModule.Text.Trim();
        string checkModule = this.txtCheckModule.Text.Trim();
        string saveModule = this.txtSaveModule.Text.Trim();
        bool needUniqueCheck = this.cmbNeedUniqueCheck.SelectedValue == "True" ? true : false;
        bool needCommonSave = this.cmbNeedCommonSave.SelectedValue == "True" ? true : false;
        bool needPartForbidCheck = this.cmbNeedPartForbidCheck.SelectedValue == "True" ? true : false;
        try
        {
            CheckItemTypeInfo condition = new CheckItemTypeInfo();
            if (hidChcekItemType == "")
            {
                condition.name = chcekItemType;
            }
            else
            {
                condition.name = hidChcekItemType;
            }
            IList<CheckItemTypeInfo> checkItemTypeList = iCheckItemType.GetCheckItemTypeByCondition(condition);
            
            addEditInfo.name = chcekItemType;
            addEditInfo.displayName = displayName;
            addEditInfo.filterModule = filterModule;
            addEditInfo.matchModule = matchModule;
            addEditInfo.checkModule = checkModule;
            addEditInfo.saveModule = saveModule;
            addEditInfo.needUniqueCheck = needUniqueCheck;
            addEditInfo.needCommonSave = needCommonSave;
            addEditInfo.needPartForbidCheck = needPartForbidCheck;
            addEditInfo.editor = this.HiddenUserName.Value;
            if (checkItemTypeList.Count == 0)
            {
                addEditInfo.cdt = DateTime.Now;
                addEditInfo.udt = DateTime.Now;
                iCheckItemType.AddCheckItemTypeInfo(addEditInfo);
            }
            else
            {
                iCheckItemType.UpdateCheckItemTypeInfo(addEditInfo);
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
        initcmbCheckItemTypeList();
        this.updatePanel2.Update();
        this.cmbCheckItemType.SelectedValue = addEditInfo.name;
        ShowListByType();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string checkItemTypeName = this.hidChcekItemType.Value;
        try
        {
            iCheckItemType.DeleteCheckItemTypeInfo(checkItemTypeName);
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
        initcmbCheckItemTypeList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<CheckItemTypeInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("CheckItemType");
        dt.Columns.Add("DisplayName");
        dt.Columns.Add("FilterModule");
        dt.Columns.Add("MatchModule");
        dt.Columns.Add("ChcekModule");
        dt.Columns.Add("SaveModule");
        dt.Columns.Add("NeedUniqueCheck");
        dt.Columns.Add("NeedCommonSave");
        dt.Columns.Add("NeedPartForbidCheck");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");  
        if (list != null && list.Count != 0)
        {
            foreach (CheckItemTypeInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.name;
                dr[1] = temp.displayName;
                dr[2] = temp.filterModule;
                dr[3] = temp.matchModule;
                dr[4] = temp.checkModule;
                dr[5] = temp.saveModule;
                dr[6] = temp.needUniqueCheck;
                dr[7] = temp.needCommonSave;
                dr[8] = temp.needPartForbidCheck;
                dr[9] = temp.editor;
                dr[10] = temp.cdt;
                dr[11] = temp.udt;
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
