using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using System.Collections.Generic;


public partial class DataMaintain_ModuleApprovalItem : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IModuleApprovalItem iModuleApprovalItem;
    private ISysSetting iSysSetting;
    Boolean isCheckItemTypeLoad;
    string[] checkModule = { "FAIBTO", "FAIBTF" };
    string[] checkActionName = { "ReleaseFA"};//ReleasePAK

    private const int COL_NUM = 12;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage10;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            isCheckItemTypeLoad = false;
            this.cmbModuleTop.Load += new EventHandler(cmbModuleTop_Load);
            iModuleApprovalItem = ServiceAgent.getInstance().GetMaintainObjectByName<IModuleApprovalItem>(WebConstant.ModuleApprovalItemObject);
            iSysSetting = ServiceAgent.getInstance().GetMaintainObjectByName<ISysSetting>(WebConstant.MaintainSysSettingObject);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
            //rr.Attributes.Add("OnClick", "changeRadio(this)");
            
            if (!this.IsPostBack)
            {
                
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                initcmbModuleTop("");
                initcmbModule();
                initFamilyList();
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

    protected void btnModuleChange_ServerClick(object sender, System.EventArgs e)
    {
        getModuleList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);
    }

    private void cmbModuleTop_Load(object sender, System.EventArgs e)
    {
        if (!this.IsPostBack)
        {
            isCheckItemTypeLoad = true;
            CheckAndStart();
        }
    }
    
    public void initFamilyList()
    {
        IList<string> familyList = iModuleApprovalItem.GetFamilyList();
        lboxFamily.Items.Clear();
        this.hidFamilyListCount.Value = familyList.Count.ToString();
        foreach (string item in familyList)
        {
            lboxFamily.Items.Add(new ListItem(item, item));
        }
    }

    public void btnModuleselectChange_ServerClick(object sender, System.EventArgs e)
    {
        string module = this.cmbModule.SelectedValue;
        initcmbActionName(module);
        initcmbDepartment(module);
        this.updatePanel3.Update();
        this.updatePanel4.Update();
        this.updatePanel5.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "changeModule();DealHideWait();", true);
    }

    private void initcmbModuleTop(string module)
    {
        this.cmbModuleTop.Items.Clear();
        IList<string> itemlist = iModuleApprovalItem.GetModuleListTop();
        this.cmbModuleTop.Items.Add(string.Empty);
        foreach (string item in itemlist)
        {
            this.cmbModuleTop.Items.Add(new ListItem { Value = item, Text = item });
        }
        if (module != "")
        {
            this.cmbModuleTop.SelectedValue = module;
        }
        this.updatePanel1.Update();
    }

    private void initcmbModule()
    {
        this.cmbModule.Items.Clear();
        IList<string> itemlist = iModuleApprovalItem.GetModuleList();
        this.cmbModule.Items.Add(string.Empty);
        foreach (string item in itemlist)
        {
            this.cmbModule.Items.Add(new ListItem { Value = item, Text = item });
        }
    }

    private void initcmbActionName(string module)
    {
        this.cmbActionName.Items.Clear();
        IList<string> itemlist = iModuleApprovalItem.GetActionNAmeList(module);
        this.cmbActionName.Items.Add(string.Empty);
        foreach (string item in itemlist)
        {
            this.cmbActionName.Items.Add(new ListItem { Value = item, Text = item });
        }
    }

    private void initcmbDepartment(string module)
    {
        this.cmbDepartment.Items.Clear();
        IList<string> itemlist = iModuleApprovalItem.GetDepartmentList(module);
        this.cmbDepartment.Items.Add(string.Empty);
        foreach (string item in itemlist)
        {
            this.cmbDepartment.Items.Add(new ListItem { Value = item, Text = item });
        }
    }

    private void CheckAndStart()
    {
        if (isCheckItemTypeLoad == true)
        {
            getModuleList();
        }
    }

    private Boolean getModuleList()
    {
        //ApprovalItemInfo condition = new ApprovalItemInfo();
        string Module = this.cmbModuleTop.SelectedValue;
        try
        {
            DataTable moduleList = iModuleApprovalItem.GetModuleList(Module);
            if (moduleList == null || moduleList.Rows.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(moduleList, DEFAULT_ROWS);
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
        return true;
    }

    private void initLabel()
    {
        this.lblList.Text = "CheckItemTypeEx List:";
        this.btnSave.Value = "Save";
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(6);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string[] familyList = { };
        if (this.hidCheckFamilyList.Value != "")
        {
            familyList = this.hidCheckFamilyList.Value.Substring(0, this.hidCheckFamilyList.Value.Length - 1).Split(',');
        }
        
        string isneedupload = this.hidIsNeedUpload.Value;
        string familycount = this.hidFamilyListCount.Value;

        

        ApprovalItemInfo item = new ApprovalItemInfo();
        item.Module = this.cmbModule.SelectedValue;
        item.ActionName = this.cmbActionName.SelectedValue;
        item.Department = this.cmbDepartment.SelectedValue;
        item.IsNeedApprove = this.cmbIsNeedApproval.SelectedValue;
        item.OwnerEmail = this.txtOwnerEmail.Value.Trim();
        item.CCEmail = this.txtCCEmail.Value.Trim();
        item.IsNeedUploadFile = this.hidIsNeedUpload.Value;
        item.NoticeMsg = this.txtNoticeMsg.Value.Trim();
        item.Editor = this.HiddenUserName.Value;
        
        string id = this.hidID.Value;
        try
        {
            ApprovalItemInfo condition = new ApprovalItemInfo();
            condition.Module = item.Module;
            condition.ActionName = item.ActionName;
            IList<ApprovalItemInfo> approvalList = iModuleApprovalItem.GetModuleList(condition);
            condition.Department = item.Department;
            IList<SysSettingInfo> sysList = iSysSetting.GetSysSettingListByCondition(new SysSettingInfo { name = "OnlyNeedOQCApprove" });
            if (sysList.Count == 0 || sysList[0].value == "Y")
            {
                if(item.Department != "OQC")
                {
                    throw new FisException("只能選擇OQC部門!");
                }
            }
            IList<ApprovalItemInfo> checkLists = approvalList.Where(x => x.Department != "OQC" && x.IsNeedApprove=="Y").ToList();
//            if(item.IsNeedApprove == "N" && checkModule.Contains(item.Module) && checkActionName.Contains(item.ActionName))
            if (item.IsNeedApprove == "N" && item.Module.StartsWith("FAI") && checkActionName.Contains(item.ActionName))
            {
                if (checkLists.Count == 0)
                {
                    throw new FisException("請先維護須審核的部門 !");
                }
                if (checkLists.Count == 1 && checkLists[0].Department == item.Department)
                {
                    throw new FisException("請先維護須審核的部門 !");
                }
                
            }
            //if (item.IsNeedApprove == "N" && checkModule.Contains(item.Module) && checkActionName.Contains(item.ActionName))
            //{
            //    IList<ApprovalItemInfo> checkList = approvalList.Where(x => x.IsNeedApprove == "Y").ToList();
            //    if (checkList.Count <= 1)
            //    {
            //        throw new FisException("請先維護須審核的部門 !");
            //    }
            //}
            approvalList = approvalList.Where(x => x.Department == item.Department).ToList();
            if (string.IsNullOrEmpty(id))
            {
                if (approvalList.Count > 0)
                {
                    //throw new FisException("CQCHK50021", new string[] { condition.Module, condition.ActionName, condition.Department });
                    item.ID = approvalList[0].ID;
                    item.Udt = DateTime.Now;
                    iModuleApprovalItem.UpdateApprovalItem(item);
                }
                item.Cdt = DateTime.Now;
                item.Udt = DateTime.Now;
                iModuleApprovalItem.InsertApprovalItem(item);
            }
            else
            {
                if (approvalList.Count > 0 && approvalList[0].ID != Convert.ToInt32(id))
                {
                    //throw new FisException("CQCHK50021", new string[] { condition.Module, condition.ActionName, condition.Department });
                    showErrorMessage(string.Format("資料重複：Module[{0}],ActionName[{1}],Department[{2}]",condition.Module, condition.ActionName, condition.Department));
                    return;
                }
                item.ID = Convert.ToInt32(id);
                item.Udt = DateTime.Now;
                iModuleApprovalItem.UpdateApprovalItem(item);
            }
            approvalList = iModuleApprovalItem.GetModuleList(condition);
            iModuleApprovalItem.DeleteApprovalItemAttr(approvalList[0].ID);
            if (isneedupload == "Y")
            {
                InsertApprovalItemAttr(familyList, familycount,approvalList[0].ID);
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
        string itemId = item.ID.ToString();
        initcmbModuleTop(item.Module);
        getModuleList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);
    }

    public void InsertApprovalItemAttr(string[] familyList, string familycount,long approvalID)
    {
        try
        {
            IList<ApprovalItemAttrInfo> list = new List<ApprovalItemAttrInfo>();
            if(familycount == familyList.Count().ToString())
            {
                ApprovalItemAttrInfo approvalitem = new ApprovalItemAttrInfo();
                approvalitem.ApprovalItemID = approvalID;
                approvalitem.AttrName = "FamilyNeedUploadFile";
                approvalitem.AttrValue = "ALL";
                approvalitem.Editor = this.HiddenUserName.Value; 
                approvalitem.Cdt = DateTime.Now;
                approvalitem.Udt = DateTime.Now;
                list.Add(approvalitem);
            }
            else
            {
                foreach(string item in familyList)
                {
                    ApprovalItemAttrInfo approvalitem = new ApprovalItemAttrInfo();
                    approvalitem.ApprovalItemID = approvalID;
                    approvalitem.AttrName = "FamilyNeedUploadFile";
                    approvalitem.AttrValue = item;
                    approvalitem.Editor = this.HiddenUserName.Value;
                    approvalitem.Cdt = DateTime.Now;
                    approvalitem.Udt = DateTime.Now;
                    list.Add(approvalitem);
                }
            }


            iModuleApprovalItem.InsertApprovalItemAttr(list);
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
    
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            iModuleApprovalItem.DeleteApprovalItem(Convert.ToInt32(this.hidID.Value));
            iModuleApprovalItem.DeleteApprovalItemAttr(Convert.ToInt64(this.hidID.Value));
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
        initcmbModuleTop("");
        getModuleList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(DataTable list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("ID");
        dt.Columns.Add("Module");
        dt.Columns.Add("ActionName");
        dt.Columns.Add("Department");
        dt.Columns.Add("IsNeedApprove");
        dt.Columns.Add("OwnerEmail");
        dt.Columns.Add("CCEmail");
        dt.Columns.Add("IsNeedUploadFile");
        dt.Columns.Add("NoticeMsg");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        dt.Columns.Add("Family");

        if (list != null && list.Rows.Count != 0)
        {
            foreach (DataRow item in list.Rows)
            {
                dr = dt.NewRow();
                dr[0] = item["ID"].ToString().Trim();
                dr[1] = item["Module"].ToString().Trim();
                dr[2] = item["ActionName"].ToString().Trim();
                dr[3] = item["Department"].ToString().Trim();
                dr[4] = item["IsNeedApprove"].ToString().Trim();
                dr[5] = item["OwnerEmail"].ToString().Trim();
                dr[6] = item["CCEmail"].ToString().Trim();
                dr[7] = item["IsNeedUploadFile"].ToString().Trim();
                dr[8] = item["NoticeMsg"].ToString().Trim();
                dr[9] = item["Editor"].ToString().Trim();
                var time = DateTime.Parse(item["Cdt"].ToString());
                dr[10] = time.ToString("yyyy-MM-dd HH:mm:ss");
                time = DateTime.Parse(item["Udt"].ToString());
                dr[11] = time.ToString("yyyy-MM-dd HH:mm:ss");
                string family = "";
                if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                {
                    family = item[0].ToString().Substring(0, item[0].ToString().Length - 1);
                }
                dr[12] = family;
                dt.Rows.Add(dr);
            }
            
            for (int i = list.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            this.hidRecordCount.Value = list.Rows.Count.ToString();
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
        e.Row.Cells[0].Attributes.Add("style", e.Row.Cells[0].Attributes["style"] + "display:none");
        e.Row.Cells[12].Attributes.Add("style", e.Row.Cells[12].Attributes["style"] + "display:none");
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
