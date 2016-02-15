/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Model Process Rule Setting 
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-04-20   Tong.Zhi-Yong     Create 
 * ITC-1361-0076
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

public partial class DataMaintain_ModelProcessRuleSetting : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROW_NUM = 15;
    private const int DEFAULT_SET_ROW_NUM = 6; //no use
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
            //UserId = "";//Master.userInfo.UserId; //UserInfo.UserId;
            iModelProcess = ServiceAgent.getInstance().GetMaintainObjectByName<IModelProcess>(WebConstant.IModelProcess);

            if (!this.IsPostBack)
            {

                UserId = Request.QueryString["userName"];
                UserId = StringUtil.decode_URL(UserId);
                this.HiddenUserName.Value = UserId;
                string processName= Request.QueryString["ProcessName"];
                processName = StringUtil.decode_URL(processName);
                this.hidProcessInit.Value = processName;
                initLabel();
                initRuleSetList();
                bindTable(null, DEFAULT_ROW_NUM, null);
                this.hidRecordCount.Value = "0";
            }
            else
            {
                initLabel();
                UserId = this.HiddenUserName.Value;
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            //bindRuleSetList(null, DEFAULT_SET_ROW_NUM);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            //bindRuleSetList(null, DEFAULT_SET_ROW_NUM);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
    }

    private void initLabel()
    {
        this.lblRuleSetList.Text = this.GetLocalResourceObject(Pre + "_lblRuleSetList").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.lblRuleList.Text = this.GetLocalResourceObject(Pre + "_lblRuleList").ToString();
        this.btnAddRule.Value = this.GetLocalResourceObject(Pre + "_btnAddRule").ToString();
        this.btnSaveRule.Value = this.GetLocalResourceObject(Pre + "_btnSaveRule").ToString();
        this.lblProcessName.Text = this.GetLocalResourceObject(Pre + "_lblProcessName").ToString();
        this.btnSetting.Value = this.GetLocalResourceObject(Pre + "_btnSetting").ToString();
        this.btnClose.Value = this.GetLocalResourceObject(Pre + "_btnClose").ToString();
        this.Master.Page.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
    }

    private void initPage()
    {

    }

    private void initRuleSetList()
    {
        IList<RulesetInfoDataMaintain> lstRuleSet = iModelProcess.GetProcessRuleSetList();
        this.selRuleSetList.Items.Clear();
        if (lstRuleSet != null && lstRuleSet.Count != 0)
        {
            string id = string.Empty;
            string priority = string.Empty;
            this.selRuleSetList.Items.Add(string.Empty);
            foreach (RulesetInfoDataMaintain ruleset in lstRuleSet)
            {
                id = ruleset.Id.ToString();
                priority = ruleset.Priority.ToString();

                string condition1 = (string.IsNullOrEmpty(ruleset.Condition1) ? string.Empty : (ruleset.Condition1 + "+"));
                string condition2 = (string.IsNullOrEmpty(ruleset.Condition2) ? string.Empty : (ruleset.Condition2 + "+"));
                string condition3 = (string.IsNullOrEmpty(ruleset.Condition3) ? string.Empty : (ruleset.Condition3 + "+"));
                string condition4 = (string.IsNullOrEmpty(ruleset.Condition4) ? string.Empty : (ruleset.Condition4 + "+"));
                string condition5 = (string.IsNullOrEmpty(ruleset.Condition5) ? string.Empty : (ruleset.Condition5 + "+"));
                string condition6 = (string.IsNullOrEmpty(ruleset.Condition6) ? string.Empty : (ruleset.Condition6 + "+"));
                string finalStr = string.Concat(condition1, condition2, condition3, condition4, condition5, condition6);
                finalStr = finalStr.Substring(0, finalStr.Length - 1);
                this.selRuleSetList.Items.Add(new ListItem(finalStr, id + "," + priority));
            }
        }
        else
        {
            this.selRuleSetList.Items.Add(string.Empty);
        }
    }

    private void bindRuleSetList(IList<RulesetInfoDataMaintain> lstRuleSet, int defaultRowNum)
    {
        this.selRuleSetList.Items.Clear();
        this.selRuleSetList.Items.Add(string.Empty);
    }

    private void bindTable(IList<ProcessRule> lstProcessRule, int defaultRowNum, string condition)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        string[] columns = null;

        if (!string.IsNullOrEmpty(condition))
        {
            columns = condition.Split('+');
        }

        dt.Columns.Add("ID");
        if (columns == null || columns.Length == 0)
        {
            dt.Columns.Add("1");
        }

        if (columns != null && columns.Length != 0)
        {
            foreach (string item in columns)
            {
                dt.Columns.Add(item);
            }

            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
        }

        if (lstProcessRule == null || lstProcessRule.Count == 0)
        {
            for (int i = 0; i < defaultRowNum; i++)
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }
            
            this.hidRecordCount.Value = "0";
        }
        else
        {
            int length = lstProcessRule.Count;
            int columnLength = columns.Length;

            foreach (ProcessRule r in lstProcessRule)
            {
                int i = 0;

                dr = dt.NewRow();

                dr[0] = r.Id;

                for (i = 0; i < columnLength; i++)
                {
                    dr[1 + i] = r.GetType().GetProperty("Value" + (i + 1)).GetValue(r, null);
                }

                dr[i + 1] = r.Editor;
                dr[i + 2] = (r.Cdt == null ? string.Empty : r.Cdt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                dr[i + 3] = (r.Udt == null ? string.Empty : r.Udt.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));

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

            this.hidRecordCount.Value = length.ToString();
        }

        gd.DataSource = dt;
        gd.DataBind();
        if (lstProcessRule == null)
        {
            gd.HeaderRow.Cells[1].Text = string.Empty;
        }
    }

    protected void hidBtn_ServerClick(object sender, EventArgs e)
    {
        ////select checkbox list for test
        string condition = hidFld1.Value;
        string conditionID = hidConditionID.Value;
        string process = hidProcess.Value;
        //string defaultItem = this.GetLocalResourceObject(Pre + "_listItemDefault").ToString();
        IList<ProcessRule> lstProcessRule = null;

        try
        {
            if (string.IsNullOrEmpty(condition))
            {
                this.hidRecordCount.Value = "0";
                bindTable(null, DEFAULT_ROW_NUM, null);
            }
            else
            {
                lstProcessRule = iModelProcess.GetRuleListByConditionAndProcess(int.Parse(conditionID), process);

                if (lstProcessRule == null || lstProcessRule.Count == 0)
                {
                    this.hidRecordCount.Value = "0";
                }
                else
                {
                    this.hidRecordCount.Value = lstProcessRule.Count.ToString();
                }

                bindTable(lstProcessRule, DEFAULT_ROW_NUM, condition);
            }
            this.updatePanel3.Update();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
    }

    protected void hidBtn2_ServerClick(object sender, EventArgs e)
    {
        ////select checkbox list for test
        string condition = hidFld1.Value;
        string conditionID = hidConditionID.Value;
        string process = hidProcess.Value;
                
        try
        {
            string currentSelect = selRuleSetList.Items[this.selRuleSetList.SelectedIndex].Text ;
            initRuleSetList();

            ListItem selectedValue = this.selRuleSetList.Items.FindByText(currentSelect);
            if (selectedValue != null)
            {
                selectedValue.Selected = true;
            }
            else
            {
                bindTable(null, DEFAULT_ROW_NUM, null);
                this.hidRecordCount.Value = "0";
                this.updatePanel3.Update();
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
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

            if (string.Compare(e.Row.Cells[0].Text.Trim(), gCurrentRuleID.ToString()) == 0)
            {
                e.Row.CssClass = "iMes_grid_SelectedRowGvExt";
                gHasHighLightRowRule = true;
                disableOrEnableDeleteRuleButton(e.Row.RowIndex.ToString(), false);
            }
        }
    }

    protected void gd_DataBound(object sender, EventArgs e)
    {
        if (!gHasHighLightRowRule)
        {
            disableOrEnableDeleteRuleButton("-1", true);
        }
    }

    protected void gd_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");
    }

    protected void btnDeleteRule_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            string ruleID = this.hidRuleID.Value;
            string rulesetID = this.hidConditionID.Value;
            IList<ProcessRule> lstProcessRule = null;
            string condition = this.hidFld1.Value;

            iModelProcess.DeleteRule(int.Parse(ruleID));
            lstProcessRule = iModelProcess.GetRuleListByConditionAndProcess(int.Parse(rulesetID), hidProcess.Value);

            bindTable(lstProcessRule, DEFAULT_ROW_NUM, condition);
            this.updatePanel3.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "dealWait", "DeleteComplete();DealHideWait();", true);

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }

    }

    protected void btnAddRule_ServerClick(Object sender, EventArgs e)
    {
        string process = this.hidProcess.Value;
        string txt1 = this.hidtxt1.Value;
        string txt2 = this.hidtxt2.Value;
        string txt3 = this.hidtxt3.Value;
        string txt4 = this.hidtxt4.Value;
        string txt5 = this.hidtxt5.Value;
        string txt6 = this.hidtxt6.Value;

        ProcessRule rule = new ProcessRule();
        string ruleSetID = this.hidConditionID.Value;
        string condition = this.hidFld1.Value;
        IList<ProcessRule> lstProcessRule = null;

        string ruleId = "";

        try
        {
            rule.Process = process;
            rule.Editor = UserId;
            rule.Value1 = txt1;
            rule.Value2 = txt2;
            rule.Value3 = txt3;
            rule.Value4 = txt4;
            rule.Value5 = txt5;
            rule.Value6 = txt6;
            rule.Rule_set_id = int.Parse(ruleSetID);

            ruleId=iModelProcess.AddRule(rule);
            gCurrentRuleID = int.Parse(ruleId);
            iModelProcess.CheckAdd_SaveModelProcess(int.Parse(ruleId));
            lstProcessRule = iModelProcess.GetRuleListByConditionAndProcess(int.Parse(ruleSetID), process);

            bindTable(lstProcessRule, DEFAULT_ROW_NUM, condition);
            this.updatePanel3.Update();

            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "dealWait", "DealHideWait();", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }

    }

    protected void btnSaveRule_ServerClick(Object sender, EventArgs e)
    {
        string process = this.hidProcess.Value;
        string txt1 = this.hidtxt1.Value;
        string txt2 = this.hidtxt2.Value;
        string txt3 = this.hidtxt3.Value;
        string txt4 = this.hidtxt4.Value;
        string txt5 = this.hidtxt5.Value;
        string txt6 = this.hidtxt6.Value;
        ProcessRule rule = new ProcessRule();
        string ruleSetID = this.hidConditionID.Value;
        string condition = this.hidFld1.Value;
        IList<ProcessRule> lstProcessRule = null;
        string ruleID = this.hidRuleID.Value;

        try
        {
            rule.Process = process;
            rule.Editor = UserId;
            rule.Value1 = txt1;
            rule.Value2 = txt2;
            rule.Value3 = txt3;
            rule.Value4 = txt4;
            rule.Value5 = txt5;
            rule.Value6 = txt6;
            rule.Rule_set_id = int.Parse(ruleSetID);
            rule.Id = int.Parse(ruleID);

            iModelProcess.EditRule(rule);
            gCurrentRuleID = rule.Id;
            iModelProcess.CheckAdd_SaveModelProcess(int.Parse(ruleID));
            lstProcessRule = iModelProcess.GetRuleListByConditionAndProcess(int.Parse(ruleSetID), process);

            bindTable(lstProcessRule, DEFAULT_ROW_NUM, condition);
            this.updatePanel3.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "dealWait", "DealHideWait();", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            //bindTable(null, DEFAULT_ROW_NUM, null);
            //this.hidRecordCount.Value = "0";
        }
     
    }

    private void disableOrEnableDeleteRuleButton(string rowIndex, bool isDisable)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");

        scriptBuilder.AppendLine("selectedRowIndex=" + rowIndex + ";");
        scriptBuilder.AppendLine("var sel= document.getElementById('" + selRuleSetList.ClientID + "')");
        if (isDisable)
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnSaveRule.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("if (emptyPattern.test(sel.value)) {");
            scriptBuilder.AppendLine("getLabelArray(true); getTxtArray(true);");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnAddRule.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt1.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt2.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt3.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt4.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt5.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt6.ClientID + "').disabled=true;}");
            scriptBuilder.AppendLine("else {");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnAddRule.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt1.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt2.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt3.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt4.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt5.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt6.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("var selRuleSetList = document.getElementById('" + this.selRuleSetList.ClientID + "');");
            scriptBuilder.AppendLine("selectLabelAndControl(selRuleSetList.options[selRuleSetList.selectedIndex].text);}");
        }
        else
        {
            scriptBuilder.AppendLine("document.getElementById('" + this.btnDelete.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnSaveRule.ClientID + "').disabled=false;");

            scriptBuilder.AppendLine("if (emptyPattern.test(sel.value)) {");
            scriptBuilder.AppendLine("getLabelArray(true); getTxtArray(true);");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnAddRule.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt2.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt3.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt4.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt5.ClientID + "').disabled=true;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt6.ClientID + "').disabled=true;}");
            scriptBuilder.AppendLine("else {");
            scriptBuilder.AppendLine("document.getElementById('" + this.btnAddRule.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt2.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt3.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt4.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt5.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("document.getElementById('" + this.txt6.ClientID + "').disabled=false;");
            scriptBuilder.AppendLine("var selRuleSetList = document.getElementById('" + this.selRuleSetList.ClientID + "');");
            scriptBuilder.AppendLine("var tblObj2 = document.getElementById('" + gd.ClientID + "');");
            scriptBuilder.AppendLine("selectLabelAndControl(selRuleSetList.options[selRuleSetList.selectedIndex].text);");
            scriptBuilder.AppendLine("setTextValue();}");
        }

        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "disableOrEnableDeleteRuleButton", scriptBuilder.ToString(), false);
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
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

   
}
