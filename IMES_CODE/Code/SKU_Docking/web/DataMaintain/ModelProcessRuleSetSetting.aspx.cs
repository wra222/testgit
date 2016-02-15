/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Model Process Rule Set Setting 
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-04-25   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
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
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Collections.Generic;
using com.inventec.system.util;

public partial class DataMaintain_ModelProcessRuleSetSetting : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //private const int DEFAULT_ROW_NUM = 15;
    private const int DEFAULT_SET_ROW_NUM = 16;
    private IModelProcess iModelProcess;
    private int cellsCount = 0;
    private int hideCount = 0;
    public String UserId;
    private int gCurrentID = -1;
    private int gCurrentRuleID = -1;
    private bool gHasHightRow = false;
    private bool gHasHighLightRowRule = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //UserId = UserInfo.UserId;
            iModelProcess = ServiceAgent.getInstance().GetMaintainObjectByName<IModelProcess>(WebConstant.IModelProcess);

            if (!this.IsPostBack)
            {
                UserId = Request.QueryString["userName"];
                UserId = StringUtil.decode_URL(UserId);
                this.HiddenUserName.Value = UserId;

                initLabel();
                initRuleSetList();
                initRuleSet();
            }
            else
            {
                UserId = this.HiddenUserName.Value;
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            bindRuleSetList(null, DEFAULT_SET_ROW_NUM);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindRuleSetList(null, DEFAULT_SET_ROW_NUM);
        }
    }

    private void initLabel()
    {
        this.lblRuleSetList.Text = this.GetLocalResourceObject(Pre + "_lblRuleSetList").ToString();
        this.lblRuleSet.Text = this.GetLocalResourceObject(Pre + "_lblRuleSet").ToString();
        this.btnUp.Value = this.GetLocalResourceObject(Pre + "_btnUp").ToString();
        this.btnDown.Value = this.GetLocalResourceObject(Pre + "_btnDown").ToString();
        this.btnDelete.Value  = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnClose.Value = this.GetLocalResourceObject(Pre + "_btnClose").ToString();
        this.Master.Page.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
    }

    private void initPage()
    {

    }

    private void initRuleSetList()
    {
        IList<RulesetInfoDataMaintain> lstRuleSet = iModelProcess.GetProcessRuleSetList();

        bindRuleSetList(lstRuleSet, DEFAULT_SET_ROW_NUM);
    }

    private void bindRuleSetList(IList<RulesetInfoDataMaintain> lstRuleSet, int defaultRowNum)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        RulesetInfoDataMaintain ruleset = null;

        dt.Columns.Add("ID");
        dt.Columns.Add("Priority");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRuleSet").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

        if (lstRuleSet == null || lstRuleSet.Count == 0)
        {
            for (int i = 0; i < defaultRowNum; i++)
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }

            this.hidRowCountRuleSetList.Value = "0";
        }
        else
        {
            int length = lstRuleSet.Count;

            for (int i = 0; i < lstRuleSet.Count; i++)
            {
                ruleset = lstRuleSet[i];
                dr = dt.NewRow();

                dr[0] = ruleset.Id.ToString();
                dr[1] = ruleset.Priority.ToString();

                string condition1 = (string.IsNullOrEmpty(ruleset.Condition1) ? string.Empty : (ruleset.Condition1 + "+"));
                string condition2 = (string.IsNullOrEmpty(ruleset.Condition2) ? string.Empty : (ruleset.Condition2 + "+"));
                string condition3 = (string.IsNullOrEmpty(ruleset.Condition3) ? string.Empty : (ruleset.Condition3 + "+"));
                string condition4 = (string.IsNullOrEmpty(ruleset.Condition4) ? string.Empty : (ruleset.Condition4 + "+"));
                string condition5 = (string.IsNullOrEmpty(ruleset.Condition5) ? string.Empty : (ruleset.Condition5 + "+"));
                string condition6 = (string.IsNullOrEmpty(ruleset.Condition6) ? string.Empty : (ruleset.Condition6 + "+"));
                string finalStr = string.Concat(condition1, condition2, condition3, condition4, condition5, condition6);

                dr[2] = finalStr.Substring(0, finalStr.Length - 1);
                dr[3] = ruleset.Editor;
                dr[4] = (ruleset.Cdt == null ? string.Empty : ruleset.Cdt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[5] = (ruleset.Udt == null ? string.Empty : ruleset.Udt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dt.Rows.Add(dr);
            }

            if (length < defaultRowNum)
            {
                for (int i = length; i < defaultRowNum; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }

            this.hidRowCountRuleSetList.Value = length.ToString();
        }

        this.gdRuleSet.DataSource = dt;
        gdRuleSet.DataBind();
        setColumnWidth();
    }

    private void setColumnWidth()
    {
        gdRuleSet.HeaderRow.Cells[0].Width = Unit.Percentage(1);
        gdRuleSet.HeaderRow.Cells[1].Width = Unit.Percentage(1);
        gdRuleSet.HeaderRow.Cells[2].Width = Unit.Percentage(52);
        gdRuleSet.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gdRuleSet.HeaderRow.Cells[4].Width = Unit.Percentage(18);
        gdRuleSet.HeaderRow.Cells[5].Width = Unit.Percentage(18);

    }

    private void initRuleSet()
    {
        IList<string> lstModelAttr = getModelAttr();
        IList<string> lstRuleSet = iModelProcess.GetModelInfoNameList();

        lstChkRuleSet.Items.Clear();

        foreach (string item in lstModelAttr)
        {
            lstChkRuleSet.Items.Add(item);
        }

        if (lstRuleSet != null && lstRuleSet.Count != 0)
        {
            foreach (string item in lstRuleSet)
            {
                lstChkRuleSet.Items.Add(item);
            }
        }
    }

    protected void hidBtn_ServerClick(object sender, EventArgs e)
    {
        ////select checkbox list for test
        string condition = hidFld1.Value;
        string conditionID = hidConditionID.Value;

        IList<ProcessRule> lstProcessRule = null;

        try
        {
            initRuleSet();
            if (!string.IsNullOrEmpty(condition))
            {
                checkedCheckBoxList(condition, lstChkRuleSet);
            }

            this.updatePanel1.Update();

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

    protected void gdRuleSet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");
        e.Row.Cells[1].Style.Add("display", "none");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

            if (string.Compare(e.Row.Cells[0].Text.Trim(), gCurrentID.ToString()) == 0)
            {
                e.Row.CssClass = "iMes_grid_SelectedRowGvExt";
                gHasHightRow = true;
                disableOrEnableDeleteButton(e.Row.RowIndex.ToString(), false);
            }
        }
    }

    protected void gdRuleSet_DataBound(object sender, EventArgs e)
    {
        if (!gHasHightRow)
        {
            disableOrEnableDeleteButton("-1", true);
        }
    }

    protected void btnUpDown_ServerClick(Object sender, EventArgs e)
    {
        string highPriorityID = this.hidConditionID.Value;
        string lowPriorityID = this.hidConditionID2.Value;
        string highPriorityCondition = this.hidFld1.Value;
        string lowPriorityCondition = this.hidFld2.Value;
        RulesetInfoDataMaintain highPriority = new RulesetInfoDataMaintain();
        RulesetInfoDataMaintain lowPriority = new RulesetInfoDataMaintain();

        highPriority.Id = int.Parse(highPriorityID);
        processCondition(highPriority, highPriorityCondition);
        lowPriority.Id = int.Parse(lowPriorityID);
        processCondition(lowPriority, lowPriorityCondition);

        highPriority.Editor = UserId;
        lowPriority.Editor = UserId;

        try
        {
            HtmlInputButton btn = (HtmlInputButton)sender;

            if (string.Compare(btn.ID, "btnUp") == 0)
            {
                gCurrentID = int.Parse(highPriorityID);
            }
            else
            {
                gCurrentID = int.Parse(lowPriorityID);
            }

            iModelProcess.ChangePriority(highPriority, lowPriority);
            initRuleSetList();
            initRuleSet();
            //checkedCheckList(highPriorityCondition, highPriorityID, lstRuleSet);
            if (string.Compare(btn.ID, "btnUp") == 0)
            {
                checkedCheckBoxList(highPriorityCondition, lstChkRuleSet);
            }
            else
            {
                checkedCheckBoxList(lowPriorityCondition, lstChkRuleSet);
            }
            this.updatePanel2.Update();
            this.updatePanel1.Update();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            hideWait();
        }
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string conditionID = hidConditionID.Value;

            iModelProcess.DeleteProcessRuleSet(int.Parse(conditionID));
            initRuleSetList();
            initRuleSet();
            this.updatePanel2.Update();
            this.updatePanel1.Update();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            hideWait();
        }
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string condition = hidFld1.Value;
            RulesetInfoDataMaintain rulesetInfo = new RulesetInfoDataMaintain();

            rulesetInfo.Editor = UserId;
            processCondition(rulesetInfo, condition);

            string ruleId = iModelProcess.AddProcessRuleSet(rulesetInfo);
            gCurrentID = int.Parse(ruleId);
            initRuleSetList();
            initRuleSet();
            checkedCheckBoxList(condition, lstChkRuleSet);
            this.updatePanel2.Update();
            this.updatePanel1.Update();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            hideWait();
        }
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string condition = hidFld1.Value;
            string conditionID = hidConditionID.Value;
            RulesetInfoDataMaintain rulesetInfo = new RulesetInfoDataMaintain();
            IList<ProcessRule> lstProcessRule = null;

            rulesetInfo.Editor = UserId;
            processCondition(rulesetInfo, condition);
            rulesetInfo.Id = int.Parse(conditionID);

            gCurrentID = rulesetInfo.Id;
            iModelProcess.EditAddProcessRuleSet(rulesetInfo);
            initRuleSetList();
            initRuleSet();
            checkedCheckBoxList(condition, lstChkRuleSet);

            if (gHasHightRow)
            {
                lstProcessRule = iModelProcess.GetRuleListByCondition(gCurrentID);
            }
            this.updatePanel2.Update();
            this.updatePanel1.Update();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            hideWait();
        }
    }

    private void processCondition(RulesetInfoDataMaintain priorityObj, string condition)
    {
        string[] columns = null;
        int columnLength = -1;

        if (!string.IsNullOrEmpty(condition))
        {
            columns = condition.Split('+');
        }

        columnLength = columns.Length;
        
        for (int i = 0; i < columnLength; i++)
        {
            priorityObj.GetType().GetProperty(("Condition" + (i + 1))).SetValue(priorityObj, columns[i], null);
        }
    }

    private void checkedCheckBoxList(string condition, CheckBoxList control)
    {
        ListItem li;

        string[] arrCondition = null;

        arrCondition = condition.Split('+');

        foreach (string item in arrCondition)
        {
            li = control.Items.FindByText(item);

            if (li != null)
            {
                li.Selected = true;
            }
        }
    }

    private void checkedCheckList(string condition, string id, HtmlSelect control)
    {
        //ListItem li;

        //if (string.IsNullOrEmpty(id))
        //{
        //    li = control.Items.FindByText(condition);

        //    if (li != null)
        //    {
        //        li.Selected = true;
        //    }
        //}
        //else
        //{
        //    control.Value = id;
        //}

    }

    private void disableOrEnableDeleteButton(string rowIndex, bool isDisable)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");

        if (rowIndex !="-1")
        {
            scriptBuilder.AppendLine("selectedRowIndex2=" + rowIndex + ";setSroll(" + rowIndex + ");");
        }
        else
        {
            scriptBuilder.AppendLine("selectedRowIndex2=" + rowIndex + ";");
        }

        if (isDisable)
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnUp.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDown.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnSave.ClientID + "').disabled=true;");
        }
        else
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnUp.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDown.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnSave.ClientID + "').disabled=false;");
        }

        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "disableOrEnableDeleteButton", scriptBuilder.ToString(), false);
    }

    private void disableOrEnableDeleteRuleButton(string rowIndex, bool isDisable)
    {
        //StringBuilder scriptBuilder = new StringBuilder();

        //scriptBuilder.AppendLine("<script language='javascript'>");

        //scriptBuilder.AppendLine("selectedRowIndex=" + rowIndex + ";");

        //if (isDisable)
        //{
        //    scriptBuilder.AppendLine("var tblObj = document.getElementById('" + gdRuleSet.ClientID + "');");
        //    scriptBuilder.AppendLine("selectLabelAndControl(tblObj.rows[selectedRowIndex2 + 1].cells[2].innerText);}");
        //}
        //else
        //{
        //    scriptBuilder.AppendLine("document.getElementById('" + this.btnDeleteRule.ClientID + "').disabled=false;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.btnSaveRule.ClientID + "').disabled=false;");

        //    scriptBuilder.AppendLine("if (selectedRowIndex2 == -1) {");
        //    scriptBuilder.AppendLine("getLabelArray(true); getTxtArray(true);");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.btnAddRule.ClientID + "').disabled=true;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt2.ClientID + "').disabled=true;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt3.ClientID + "').disabled=true;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt4.ClientID + "').disabled=true;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt5.ClientID + "').disabled=true;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt6.ClientID + "').disabled=true;}");
        //    scriptBuilder.AppendLine("else {");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.btnAddRule.ClientID + "').disabled=false;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt2.ClientID + "').disabled=false;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt3.ClientID + "').disabled=false;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt4.ClientID + "').disabled=false;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt5.ClientID + "').disabled=false;");
        //    scriptBuilder.AppendLine("document.getElementById('" + this.txt6.ClientID + "').disabled=false;");
        //    scriptBuilder.AppendLine("var tblObj = document.getElementById('" + gdRuleSet.ClientID + "');");
        //    scriptBuilder.AppendLine("var tblObj2 = document.getElementById('" + gd.ClientID + "');");
        //    scriptBuilder.AppendLine("selectLabelAndControl(tblObj.rows[selectedRowIndex2 + 1].cells[2].innerText);");
        //    scriptBuilder.AppendLine("setTextValue();}");

        //}

        //scriptBuilder.AppendLine("</script>");

        //ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "disableOrEnableDeleteRuleButton", scriptBuilder.ToString(), false);
    }

    //private void showErrorMessage(string errorMsg)
    //{
    //    StringBuilder scriptBuilder = new StringBuilder();

    //    scriptBuilder.AppendLine("<script language='javascript'>");
    //    scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
    //    scriptBuilder.AppendLine("</script>");

    //    ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    //}

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
   }


    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("endWaitingCoverDiv();");
        scriptBuilder.AppendLine("DealHideWait();");        
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    }

    private IList<string> getModelAttr()
    {
        IList<string> lstModelAttr = new List<string>();

        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemModel").ToString());
        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemLine").ToString());
        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemCustomer").ToString());
        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemFamily").ToString());
        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemCustPN").ToString());
        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemRegion").ToString());
        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemShipType").ToString());
        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemOSCode").ToString());
        lstModelAttr.Add(this.GetLocalResourceObject(Pre + "_listItemOSDesc").ToString());

        return lstModelAttr;
    }
}
