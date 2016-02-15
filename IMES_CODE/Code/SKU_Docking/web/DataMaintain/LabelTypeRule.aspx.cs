﻿using System;
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
using com.inventec.RBPC.Net.intf;


public partial class DataMaintain_LabelTypeRule : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private ILabelTypeRuleMaintain iLabelTypeRuleMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<ILabelTypeRuleMaintain>(WebConstant.LabelTypeRuleObject);
    private IConstValueMaintain iConstValue = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueMaintain>(WebConstant.ConstValueMaintainObject);
    private IPermissionManager permissionManager = RBPCAgent.getRBPCManager<IPermissionManager>();
    private const int COL_NUM = 15;

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
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();

            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                initSubSyatem();
                initUI(null);
                this.cmbSubSystem.Attributes.Add("onchange", "SubSystemChange(this)");
                this.cmbUI.Attributes.Add("onchange", "UIChange(this)");
                bindTable(null, DEFAULT_ROWS);
            }
            //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "initControls();", true);
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
    
    protected void initSubSyatem()
    {
        IList<ConstValueInfo> List = iConstValue.GetConstValueListByType("LabelSettingFunction");
        var s = (from t in List
                 orderby t.description
                 select t).ToList();

        this.cmbSubSystem.Items.Clear();
        this.cmbSubSystem.Items.Add(string.Empty);

        foreach (ConstValueInfo temp in s)
        {
            ListItem item = new ListItem();
            item = new ListItem(temp.name, temp.value);
            this.cmbSubSystem.Items.Add(item);
        }
    }

    protected void initUI(string UIValue)
    {
        if (UIValue != null)
        {
            DataTable dtTreeNodes = permissionManager.GetPermissionByOP(UIValue);
            dtTreeNodes.DefaultView.Sort = "name ASC";
            dtTreeNodes = dtTreeNodes.DefaultView.ToTable();
            this.cmbUI.Items.Clear();
            this.cmbUI.Items.Add(string.Empty);
            foreach (DataRow dr in dtTreeNodes.Select("","name"))
            {
                string name = dr["name"].ToString();
                string descr = dr["descr"].ToString();
                ListItem item = new ListItem();
                item = new ListItem(name, descr);
                this.cmbUI.Items.Add(item);
            }
        }
        else
        {
            this.cmbUI.Items.Clear();
            this.cmbUI.Items.Add(string.Empty);
        }
    }

    protected void btnSubSystemChange_ServerClick(object sender, System.EventArgs e)
    {
        string UIValue = this.hidUI.Value;
        initUI(UIValue);
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);
    }

    protected void btnUIChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);
    }

    protected void btnRefash_ServerClick(object sender, System.EventArgs e)
    {
        ShowList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);
    }


    private Boolean ShowList()
    {
        string pcode = this.hidpcode.Value;
        try
        {
            IList<LabelTypeRuleDef> lstLabelTypeRule = iLabelTypeRuleMaintain.GetLabeTypeRuleByPCode(pcode);
            if (lstLabelTypeRule == null || lstLabelTypeRule.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(lstLabelTypeRule, DEFAULT_ROWS);
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
        this.lblLabelType.Text = "LabelType:";
        this.lblSubSystem.Text = "SubSystem:";
        this.lblUI.Text = "UI:";
        this.lblList.Text = "LabelTypeRule List";
        this.btnModelInfo.Value = "ModelInfo";
        this.btnDeliveryInfo.Value = "DeliveryInfo";
        this.btnPartInfo.Value = "PartInfo";
        this.lblStation.Text = "Station:";
        this.lblFamily.Text = "Family:";
        this.lblModel.Text = "Model:";
        this.lblBomLevel.Text = "BomLevel:";
        this.lblBomNodeType.Text = "BomNodeType:";
        this.lblPartNo.Text = "PartNo:";
        this.lblPartDescr.Text = "PartDescr:";
        this.lblPartType.Text = "PartType";
        this.lblRemark.Text = "Remark";
        this.btnSave.Value = "Save";
        this.btntestRE.Value = "TestRE";
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        //gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        //gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(6);
        //gd.HeaderRow.Cells[11].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[12].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[13].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[14].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[15].Width = Unit.Percentage(6);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        LabelTypeRuleDef item = new LabelTypeRuleDef();
        item.Station = this.txtStation.Value.Trim();
        item.Family = this.txtFamily.Value.Trim();
        item.Model = this.txtModel.Value.Trim();
        item.BomLevel = Convert.ToInt32(this.txtBomLevel.Value.Trim());
        item.BomNodeType = this.txtBomNodeType.Value.Trim();
        item.PartNo = this.txtPartNo.Value.Trim();
        item.PartDescr = this.txtPartDescr.Value.Trim();
        item.PartType = this.txtPartType.Value.Trim();
        item.Remark = this.txtRemark.Value.Trim();
        item.Editor = this.HiddenUserName.Value;
        item.LabelType = this.hidLabelType.Value;
        
        item.ModelConstValue = this.hidModelConstValue.Value;
        item.DeliveryConstValue = this.hidDeliveryConstValue.Value;
        item.PartConstValue = this.hidPartConstValue.Value;
            

        try
        {
            iLabelTypeRuleMaintain.UpdateAndInsertLabeTypeRule(item);
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
        string LabelType = Convert.ToString(item.LabelType);
        ShowList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + LabelType + "');DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string LabelType = this.hidLabelType.Value;
            DeleteConstValue(LabelType);
            iLabelTypeRuleMaintain.DeleteLabelTypeRule(LabelType);
            
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
        ShowList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void DeleteConstValue(string LabelType)
    {
        string[] LabelTypeRuleConstValueName = { "Model_", "Delivery_", "PartInfo_" };
        foreach (string LabelTypeItem in LabelTypeRuleConstValueName)
        {
            ConstValueInfo item = new ConstValueInfo();
            item.type = LabelTypeItem + LabelType;
            iConstValue.DeleteConstValueByCondition(item);
        }
    }

    private void bindTable(IList<LabelTypeRuleDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("LabelType");
        dt.Columns.Add("Station");
        dt.Columns.Add("Family");
        dt.Columns.Add("Model");
        dt.Columns.Add("ModelConstValue");
        dt.Columns.Add("DeliveryConstValue");
        dt.Columns.Add("BomLevel");
        dt.Columns.Add("PartNo");
        dt.Columns.Add("BomNodeType");
        dt.Columns.Add("PartDescr");
        dt.Columns.Add("PartType");
        dt.Columns.Add("PartConstValue");
        dt.Columns.Add("Remark");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());
        if (list != null && list.Count != 0)
        {
            foreach (LabelTypeRuleDef item in list)
            {
                dr = dt.NewRow();
                dr[0] = item.LabelType.Trim();
                dr[1] = item.Station.Trim();
                dr[2] = item.Family.Trim();
                dr[3] = item.Model.Trim();
                dr[4] = item.ModelConstValue.Trim();
                dr[5] = item.DeliveryConstValue.Trim();
                dr[6] = item.BomLevel;
                dr[7] = item.PartNo.Trim();
                dr[8] = item.BomNodeType.Trim();
                dr[9] = item.PartDescr.Trim();
                dr[10] = item.PartType.Trim();
                dr[11] = item.PartConstValue.Trim();
                dr[12] = item.Remark == null ? "" : item.Remark.Trim();
                dr[13] = item.Editor.Trim();
                dr[14] = item.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[15] = item.Udt.ToString("yyyy-MM-dd HH:mm:ss");
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
        e.Row.Cells[4].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        e.Row.Cells[5].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        e.Row.Cells[11].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
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
