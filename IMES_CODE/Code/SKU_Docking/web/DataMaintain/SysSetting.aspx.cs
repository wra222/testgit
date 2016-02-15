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


public partial class DataMaintain_SysSetting : System.Web.UI.Page
{    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private ISysSetting iSysSetting;
    Boolean isSysSettingNameLoad;

    private const int COL_NUM = 3;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string type;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            isSysSettingNameLoad = false;
            this.cmbSysSettingName.Load += new EventHandler(cmbSysSettingName_Load);
            type = Request["Type"];
            iSysSetting = ServiceAgent.getInstance().GetMaintainObjectByName<ISysSetting>(WebConstant.MaintainSysSettingObject);
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
                //bindTable(null, DEFAULT_ROWS);
                initcmbSysSettingNameList();
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

    protected void btnSysSettingNameChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowListByType();
        //this.updatePanel1.Update();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "SysSettingNameChange", "setNewItemValue();DealHideWait();", true);
    }

    protected void btnTypeListUpdate_ServerClick(object sender, System.EventArgs e)
    {
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnTypeListUpdate", "setNewItemValue();DealHideWait();", true);
    }

    private void cmbSysSettingName_Load(object sender, System.EventArgs e)
    {
        if (!this.IsPostBack)
        {
            isSysSettingNameLoad = true;
            CheckAndStart();
        }
    }

    private void CheckAndStart()
    {
        if (isSysSettingNameLoad == true)
        {
           ShowListByType();
        }
    }

    private Boolean ShowListByType()
    {
            
        type = this.cmbSysSettingName.SelectedValue; 
        try
        {
            SysSettingInfo sysSettingCondition = new SysSettingInfo();
            if (type != "ALL")
            {
                sysSettingCondition.name = type;
            }
            IList<SysSettingInfo> dataList = iSysSetting.GetSysSettingListByCondition(sysSettingCondition);
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
        this.lblName.Text = this.GetLocalResourceObject(Pre + "_lblName").ToString();
        this.lblValue.Text = this.GetLocalResourceObject(Pre + "_lblValue").ToString();
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        //this.btnAddType.Value = this.GetLocalResourceObject(Pre + "_btnAddType").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void initcmbSysSettingNameList()
    {
         SysSettingInfo sysSettingCondition = new SysSettingInfo();
         if (type == "")
         {
             this.cmbSysSettingName.Items.Clear();
             this.cmbSysSettingName.Items.Add(string.Empty);
             this.cmbSysSettingName.Items.Add("ALL");
         }
         else
         {
             this.cmbSysSettingName.Items.Clear();
             this.cmbSysSettingName.Items.Add(string.Empty);
             sysSettingCondition.name = type;
         }
        IList<SysSettingInfo> sysSettingInfoList = iSysSetting.GetSysSettingListByCondition(sysSettingCondition);
        IList<ListItem> inneritem = new List<ListItem>();
        foreach(SysSettingInfo info in sysSettingInfoList)
        {
            ListItem item = new ListItem();
            item.Text = info.name;
            item.Value = info.name;
            inneritem.Add(item);
            this.cmbSysSettingName.Items.Add(item);
        }
        this.cmbSysSettingName.SelectedIndex = 1;
        ShowListByType();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(8);
        
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string id = this.HidID.Value.Trim();
        string nameValue = this.dName.Text.Trim();
        string valueValue = this.dValue.Text.Trim();
        string descValue = this.dDescr.Text.Trim();
        try
        {
            SysSettingInfo condition = new SysSettingInfo();
            if (id != "")
            {
                condition.id = Convert.ToInt32(id);
            }
            else
            {
                condition.name = nameValue;
                condition.value = valueValue;
            }
            IList<SysSettingInfo> sysSettingList = iSysSetting.GetSysSettingListByCondition(condition);

            SysSettingInfo addEditInfo = new SysSettingInfo();

            if (sysSettingList.Count == 0)
            {
                addEditInfo.name = nameValue;
                addEditInfo.value = valueValue;
                addEditInfo.description = descValue;
                iSysSetting.AddSysSettingInfo(addEditInfo);
            }
            else
            {
                addEditInfo.name = nameValue;
                addEditInfo.value = valueValue;
                addEditInfo.description = descValue;
                iSysSetting.UpdateSysSettingInfo(addEditInfo, condition);
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
        initcmbSysSettingNameList();
        //this.updatePanel1.Update();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        int id = Convert.ToInt32( this.HidID.Value);
        try
        {
            iSysSetting.DeleteSysSettingInfo(id);
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
        initcmbSysSettingNameList();
        //ShowListByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<SysSettingInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemName").ToString());  
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemValue").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDes").ToString()); 
        dt.Columns.Add("ID").ToString();

       if (list != null && list.Count != 0)
        {
            foreach (SysSettingInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.name;
                dr[1] = temp.value;
                dr[2] = temp.description;
                dr[3] = temp.id;
                
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
        //sourceData = Server.HtmlEncode(sourceData);
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

}
