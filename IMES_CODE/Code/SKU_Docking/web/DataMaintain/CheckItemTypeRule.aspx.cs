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


public partial class DataMaintain_CheckItemTypeRule : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private ICheckItemTypeListMaintain iCheckItemTypeListMaintain;
    Boolean isCheckItemTypeLoad;
    private const int COL_NUM = 14;

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
            this.cmbCheckItemTypeList.InnerDropDownList.Load += new EventHandler(cmbCheckItemTypeList_Load);
            iCheckItemTypeListMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<ICheckItemTypeListMaintain>(WebConstant.CheckItemTypeListObject);
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
   
    protected void btnCheckTypeChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowListByCustomAndStage();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);
    }
    
    private void cmbCheckItemTypeList_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbCheckItemTypeList.IsPostBack)
        {
            isCheckItemTypeLoad = true;
            CheckAndStart();
        }
    }

    private void CheckAndStart()
    {
        if (isCheckItemTypeLoad == true)
        {
            ShowListByCustomAndStage();
        }
    }

    private Boolean ShowListByCustomAndStage()
    {
        String CheckType = this.cmbCheckItemTypeList.InnerDropDownList.SelectedValue;
        try
        {
            IList<CheckItemTypeRuleDef>  lstCheckItemType = iCheckItemTypeListMaintain.GetCheckItemTypeRuleByItemType(CheckType);
            if (lstCheckItemType == null || lstCheckItemType.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(lstCheckItemType, DEFAULT_ROWS);
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
        this.lblCustomer.Text = "CheckItemType:";
        this.lblList.Text = "CheckItemTypeRule List:";
        this.lblLine.Text = "1.Line:";
        this.lblStation.Text = "2.Station:";
        this.lblFamily.Text = "3.Family:";
        this.lblBomNodeType.Text = "4.BomNodeType:";
        this.lblPartDescr.Text = "5.PartDescr:";
        this.lblPartType.Text = "6.PartType:";
        this.lblFilterExpression.Text = "7.FilterExpression:";
        this.lblMatchRule.Text = "8.MatchRule:";
        this.lblCheckRule.Text = "9.CheckRule:";
        this.lblSaveRule.Text = "10.SaveRule:";
        this.lblNeedUniqueCheck.Text = "11.NeedUniqueCheck";
        this.lblNeedCommonSave.Text = "12.NeedCommonSave";
        this.lblNeedSave.Text = "13.NeedSave";
        this.lblCheckTestKPCount.Text = "14.CheckTestKPCount";
        this.lblDescr.Text = "15.Descr:";
        this.btnSave.Value = "Save";
        this.btntestRE.Value = "TestRE";
        this.btntestExpression.Value = "TestExpression";
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(3);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(3);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[12].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[13].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[14].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[15].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[16].Width = Unit.Percentage(13);
        gd.HeaderRow.Cells[17].Width = Unit.Percentage(13);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        CheckItemTypeRuleDef item = new CheckItemTypeRuleDef();
        CheckItemTypeRuleDef checkitem = new CheckItemTypeRuleDef();
        item.CheckItemType = this.cmbCheckItemTypeList.InnerDropDownList.SelectedValue.Trim();
        item.Line = this.cmbLine.InnerDropDownList.SelectedValue.Trim();
        item.Station = this.cmbStation.InnerDropDownList.SelectedValue.Trim();
        item.Family = this.cmbFamily.InnerDropDownList.SelectedValue.Trim();
        item.BomNodeType = this.txtBomNodeType.Value.Trim();
        item.PartDescr = this.txtPartDescr.Value.Trim();
        item.PartType = this.txtPartType.Value.Trim();
        item.FilterExpression = this.txtFilterExpression.Value.Trim();
        item.MatchRule = this.txtMatchRule.Value.Trim();
        item.CheckRule = this.txtCheckRule.Value.Trim();
        item.SaveRule = this.txtSaveRule.Value.Trim();
        item.NeedUniqueCheck = this.cmbNeedUniqueCheck.SelectedItem.Value;
        item.NeedCommonSave = this.cmbNeedCommonSave.SelectedItem.Value;
        item.NeedSave = this.cmbNeedSave.SelectedItem.Value;
        item.CheckTestKPCount = this.cmbCheckTestKPCount.SelectedItem.Value;
        item.Descr = this.txtDescr.Value.Trim();
        item.Editor = this.HiddenUserName.Value;
        item.ID = Convert.ToInt32(this.hidID.Value != "" ? this.hidID.Value : "-1");

        checkitem.Line = this.cmbLine.InnerDropDownList.SelectedValue.Trim();
        checkitem.Station = this.cmbStation.InnerDropDownList.SelectedValue.Trim();
        checkitem.Family = this.cmbFamily.InnerDropDownList.SelectedValue.Trim();
        checkitem.BomNodeType = this.txtBomNodeType.Value.Trim();
        checkitem.PartDescr = this.txtPartDescr.Value.Trim();
        checkitem.PartType = this.txtPartType.Value.Trim();
        checkitem.FilterExpression = this.txtFilterExpression.Value.Trim();
        checkitem.MatchRule = this.txtMatchRule.Value.Trim();
        checkitem.CheckRule = this.txtCheckRule.Value.Trim();
        checkitem.SaveRule = this.txtSaveRule.Value.Trim();
        checkitem.NeedUniqueCheck = this.cmbNeedUniqueCheck.SelectedItem.Value;
        checkitem.NeedCommonSave = this.cmbNeedCommonSave.SelectedItem.Value;
        checkitem.NeedSave = this.cmbNeedSave.SelectedItem.Value;
        checkitem.CheckTestKPCount = this.cmbCheckTestKPCount.SelectedItem.Value;
        checkitem.Descr = this.txtDescr.Value.Trim();
        try
        {
            if (iCheckItemTypeListMaintain.CheckExistCheckItemTypeRule(checkitem))
            {
                showErrorMessage("Is exist");
                return;
            }
            if (item.ID != -1)
            {
                iCheckItemTypeListMaintain.UpdateCheckItemTypeRule(item);
            }
            else
            {
                iCheckItemTypeListMaintain.AddCheckItemTypeRule(item);
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
        string itemId = Convert.ToString(item.ID);
        ShowListByCustomAndStage();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            iCheckItemTypeListMaintain.DeleteCheckItemTypeRule(Convert.ToInt32(hidID.Value));
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
        ShowListByCustomAndStage();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<CheckItemTypeRuleDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Line");
        dt.Columns.Add("Station");
        dt.Columns.Add("Family");
        dt.Columns.Add("BomNodeType");
        dt.Columns.Add("PartDescr");
        dt.Columns.Add("PartType");
        dt.Columns.Add("FilterExpression");
        dt.Columns.Add("MatchRule");
        dt.Columns.Add("CheckRule");
        dt.Columns.Add("SaveRule");
        dt.Columns.Add("NeedUniqueCheck");
        dt.Columns.Add("NeedCommonSave");
        dt.Columns.Add("NeedSave");
        dt.Columns.Add("CheckTestKPCount");
        dt.Columns.Add("Descr");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());
        dt.Columns.Add("ID");
        if (list != null && list.Count != 0)
        {
            foreach (CheckItemTypeRuleDef item in list)
            {
                dr = dt.NewRow();
                dr[0] = item.Line.Trim();
                dr[1] = item.Station.Trim();
                dr[2] = item.Family.Trim();
                dr[3] = item.BomNodeType.Trim();
                dr[4] = item.PartDescr.Trim();
                dr[5] = item.PartType.Trim();
                dr[6] = item.FilterExpression.Trim();
                dr[7] = item.MatchRule.Trim();
                dr[8] = item.CheckRule.Trim();
                dr[9] = item.SaveRule.Trim();
                dr[10] = item.NeedUniqueCheck.Trim();
                dr[11] = item.NeedCommonSave.Trim();
                dr[12] = item.NeedSave.Trim();
                dr[13] = item.CheckTestKPCount.Trim();
                dr[14] = item.Descr.Trim();
                dr[15] = item.Editor.Trim();
                dr[16] = item.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[17] = item.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[18] = item.ID;
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
        e.Row.Cells[18].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
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