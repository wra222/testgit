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
using IMES.Maintain.Interface.MaintainIntf;
using System.Text;


public partial class RuleSetting : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 6;
    private const int DEFAULT_ROWS_Rules = 4;
    ILabelSettingManager iLabelSettingManager = ServiceAgent.getInstance().GetMaintainObjectByName<ILabelSettingManager>(WebConstant.ILabelSettingManager);
    private string editor;

    private string strLabelType, strTemplate;

    private const string constCustomer = "Customer";
    private const string constModel = "Model";
    private const string constModelInfo = "ModelInfo";
    private const string constDelivery = "Delivery";
    private const string constDeliveryInfo = "DeliveryInfo";
    private const string constPart = "Part";
    private const string constPartInfo = "PartInfo";
    protected string pmtMessage1 = "";
    protected string pmtMessage2 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
        pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
        try
        {
            if (Request.QueryString["UserName"] != null)
            {
                editor = Request.QueryString["UserName"];
            }
            // editor = UserInfo.UserId;//"itc98079";//
            if (!this.IsPostBack)
            {

                strLabelType = Request.QueryString["LabelType"];
                strTemplate = Request.QueryString["Template"];
                this.valLabelType.Text = strLabelType;
                this.valTemplate.Text = strTemplate;
                InitLabel();
                bindRules();
                bindRuleSettingTable();
                InitModeSelect();
                cmbAttribute.Mode = constCustomer;
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
    }

    #region 页面的初始化和数据绑定

    /// <summary>
    /// 下拉列表的赋值
    /// </summary>
    protected void InitModeSelect()
    {
        try
        {
            //string value1 = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbStationStatusItemValue1").ToString();
            //string value2 = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbStationStatusItemValue2").ToString();
            string value1 = constCustomer;
            string value2 = constModel;
            string value3 = constModelInfo;
            string value4 = constDelivery;
            string value5 = constDeliveryInfo;
            string value6 = constPart;
            string value7 = constPartInfo;

            selMode.Items.Add(new ListItem(value1, value1));
            selMode.Items.Add(new ListItem(value2, value2));
            selMode.Items.Add(new ListItem(value3, value3));
            selMode.Items.Add(new ListItem(value4, value4));
            selMode.Items.Add(new ListItem(value5, value5));
            selMode.Items.Add(new ListItem(value6, value6));
            selMode.Items.Add(new ListItem(value7, value7));
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

    }

    /// <summary>
    /// 绑定rule数据到gridview
    /// </summary>
    protected void bindRules()
    {
        try
        {
            IList<LabelRuleMaintainInfo> labelRuleList = iLabelSettingManager.getLabelRuleByTemplateName(this.valTemplate.Text);


            lstRules.Items.Clear();
            if (labelRuleList != null && labelRuleList.Count != 0)
            {
                foreach (LabelRuleMaintainInfo temp in labelRuleList)
                {
                    ListItem lstItem = new ListItem("Rule" + temp.RuleID, temp.RuleID.ToString());
                    lstRules.Items.Add(lstItem);
                }

            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_Rules; i++)
                {
                    ListItem lstItem = new ListItem("");

                    lstRules.Items.Add(lstItem);
                }
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
    }


    /// <summary>
    /// 设置各列的宽度百分比
    /// </summary>
    private void setColumnWidth()
    {
        gdRuleSettingList.HeaderRow.Cells[1].Width = Unit.Pixel(80);
        gdRuleSettingList.HeaderRow.Cells[2].Width = Unit.Pixel(230);
        gdRuleSettingList.HeaderRow.Cells[3].Width = Unit.Pixel(140);
        gdRuleSettingList.HeaderRow.Cells[4].Width = Unit.Pixel(80);
    }

    /// <summary>
    /// 获取RuleSetting数据并绑定到gridview
    /// </summary>
    protected void bindRuleSettingTable()
    {
        try
        {
            IList<LabelRuleSetMaintainInfo> labelRuleSetList = new List<LabelRuleSetMaintainInfo>();

            if (hidRuleId.Value.Length != 0)
            {
                labelRuleSetList = iLabelSettingManager.GetLabelRuleSetByRuleID(Int32.Parse(hidRuleId.Value));
            }

            DataTable dt = new DataTable();
            DataRow dr = null;


            dt.Columns.Add("id");
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colMode").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colAttribute").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colValue").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

            if (labelRuleSetList != null && labelRuleSetList.Count != 0)
            {
                foreach (LabelRuleSetMaintainInfo temp in labelRuleSetList)
                {
                    dr = dt.NewRow();

                    dr[0] = temp.Id;

                    switch (temp.Mode)
                    {
                        case "0":
                            dr[1] = constCustomer;
                            break;
                        case "1":
                            dr[1] = constModel;
                            break;
                        case "2":
                            dr[1] = constModelInfo;
                            break;
                        case "3":
                            dr[1] = constDelivery;
                            break;
                        case "4":
                            dr[1] = constDeliveryInfo;
                            break;
                        case "5":
                            dr[1] = constPart;
                            break;
                        case "6":
                            dr[1] = constPartInfo;
                            break;
                    }

                    dr[2] = temp.AttributeName;
                    dr[3] = temp.AttributeValue;
                    dr[4] = temp.Editor;

                    if (temp.Cdt == DateTime.MinValue)
                    {
                        dr[5] = "";
                    }
                    else
                    {
                        dr[5] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (temp.Udt == DateTime.MinValue)
                    {
                        dr[6] = "";
                    }
                    else
                    {
                        dr[6] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    dt.Rows.Add(dr);
                }

                for (int i = labelRuleSetList.Count; i < DEFAULT_ROWS; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }

            gdRuleSettingList.DataSource = dt;
            gdRuleSettingList.DataBind();
            setColumnWidth();
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Style.Add("display", "none");//id

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                    {
                        e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                    }
                }

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
    }


    /// <summary>
    /// label的初始化
    /// </summary>
    private void InitLabel()
    {
        this.lblLabelType.Text = this.GetLocalResourceObject(Pre + "_lblLabelType").ToString();

        this.lblRules.Text = this.GetLocalResourceObject(Pre + "_lblRules").ToString();
        this.lblTemplate.Text = this.GetLocalResourceObject(Pre + "_lblTemplate").ToString();
        this.lblRuleSettingItems.Text = this.GetLocalResourceObject(Pre + "_lblRuleSettingItems").ToString();
        this.lblMode.Text = this.GetLocalResourceObject(Pre + "_lblMode").ToString();
        this.lblAttribute.Text = this.GetLocalResourceObject(Pre + "_lblAttribute").ToString();
        this.lblValue.Text = this.GetLocalResourceObject(Pre + "_lblValue").ToString();
        this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
        this.btnExit.Value = this.GetLocalResourceObject(Pre + "_btnExit").ToString();
        this.btnSave2.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDelete2.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnDelete1.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnAdd1.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();

        //setFocus();


    }


    #endregion


    #region 一些异步事件

    protected void btnRefreshRuleSettingList_Click(object sender, System.EventArgs e)
    {
        bindRuleSettingTable();
    }

    protected void btnReloadCmbAttribute_Click(object sender, System.EventArgs e)
    {
        try
        {
            cmbAttribute.InnerDropDownList.Items.Clear();
            cmbAttribute.Mode = hidMode.Value;
            cmbAttribute.refresh();
            cmbAttribute.InnerDropDownList.Text = hidAttribute.Value;
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
    }


    #endregion


    #region  数据的增删改查触发的事件

    protected void btnAdd1_Click(object sender, System.EventArgs e)
    {
        string ruleid;
        try
        {
            LabelRuleMaintainInfo tmp = new LabelRuleMaintainInfo();
            tmp.TemplateName = this.valTemplate.Text;
            ruleid = iLabelSettingManager.AddLabelRule(tmp).ToString();
            bindRules();
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
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "Add1Complete", "Add1Complete(\"" + ruleid + "\");", true);
    }

    protected void btnDelete1_Click(object sender, System.EventArgs e)
    {
        try
        {
            iLabelSettingManager.DeleteLabelRule(Int32.Parse(lstRules.SelectedValue));
            bindRules();
        }
        catch (FisException ex)
        {
            showErrorMessage_Add1Complete(ex.mErrmsg, hidRuleId.Value);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage_Add1Complete(ex.Message, hidRuleId.Value);
            return;
        }
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "Delete1Complete", "Delete1Complete();", true);
    }

    protected void btnSave2_Click(object sender, System.EventArgs e)
    {
        string ruleSetId = null;
        try
        {
            LabelRuleSetMaintainInfo tmp = new LabelRuleSetMaintainInfo();
            //tmp.Mode = selMode.SelectedValue;
            switch (selMode.SelectedValue)
            {
                case constCustomer:
                    tmp.Mode = "0";
                    break;
                case constModel:
                    tmp.Mode = "1";
                    break;
                case constModelInfo:
                    tmp.Mode = "2";
                    break;
                case constDelivery:
                    tmp.Mode = "3";
                    break;
                case constDeliveryInfo:
                    tmp.Mode = "4";
                    break;
                case constPart:
                    tmp.Mode = "5";
                    break;
                case constPartInfo:
                    tmp.Mode = "6";
                    break;
            }

            tmp.AttributeName = cmbAttribute.InnerDropDownList.SelectedValue;
            tmp.AttributeValue = txtValue.Text;
            tmp.RuleID = Int32.Parse(hidRuleId.Value);
            tmp.Editor = editor;
            if (hidRuleSettingItemId.Value.Length > 0)
            {
                tmp.Id = Int32.Parse(hidRuleSettingItemId.Value);
            }
            else
            {
                tmp.Id = 0;
            }

            if ((cmbAttribute.InnerDropDownList.SelectedValue != hidAttribute.Value) && (hidRuleSettingItemId.Value.Length > 0))
            {
                tmp.Id = 0;
            }

            ruleSetId = iLabelSettingManager.SaveLabelRuleSet(tmp).ToString();
            bindRuleSettingTable();
        }
        catch (FisException ex)
        {
            showErrorMessage_Save2Complete(ex.mErrmsg, hidRuleSettingItemId.Value);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage_Save2Complete(ex.Message, hidRuleSettingItemId.Value);
            return;
        }
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "Save2Complete", "Save2Complete(\"" + ruleSetId + "\");", true);
    }

    protected void btnDelete2_Click(object sender, System.EventArgs e)
    {
        try
        {
            iLabelSettingManager.DeleteLabelRuleSet(Int32.Parse(hidRuleSettingItemId.Value));
            bindRuleSettingTable();
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

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "Delete2Complete", "Delete2Complete();", true);
    }

    #endregion


    #region 一些系统方法
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        //scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");"); 
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void showErrorMessage_Add1Complete(string errorMsg, string strRuleID)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        scriptBuilder.AppendLine("Add1Complete(\"" + strRuleID + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage1", scriptBuilder.ToString(), false);
    }

    private void showErrorMessage_Save2Complete(string errorMsg, string strRuleID)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        scriptBuilder.AppendLine("Save2Complete(\"" + strRuleID + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage1", scriptBuilder.ToString(), false);
    }

    #endregion


}
