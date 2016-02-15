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


public partial class DataMaintain_EnergyLabel : System.Web.UI.Page
{    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IConstValueMaintain iConstValueMaintain;
    private IEnergyLabel iEnergyLabel;
    //Boolean isConstValueTypeLoad;

    private const int COL_NUM = 8;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;
    public String type2;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //isConstValueTypeLoad = false;
            iConstValueMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueMaintain>(WebConstant.ConstValueMaintainObject);
            iEnergyLabel = ServiceAgent.getInstance().GetMaintainObjectByName<IEnergyLabel>(WebConstant.EnergyLabelMaintainObject);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                initcmbEnergyLabelFamilyListTop();
                initcmbFamilyList();
                initcmbChinaLevelList();
                bindTable(null, DEFAULT_ROWS);
            }
            //this.cmbEnergyLabelFamilyListTop.Load += new EventHandler(cmbEnergyLabelFamilyListTop_Load);
            btnQuery_ServerClick(sender, e);
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

    private void initcmbEnergyLabelFamilyListTop()
    {
        try
        {
            this.cmbEnergyLabelFamilyListTop.Items.Clear();
            //this.cmbEnergyLabelFamilyListTop.Items.Add(string.Empty);
            this.cmbEnergyLabelFamilyListTop.Items.Add(new ListItem("ALL", "ALL"));
            IList<string> energyLabelList = iEnergyLabel.GetEnergyLabel_Family();
            foreach (string item in energyLabelList)
            {
                this.cmbEnergyLabelFamilyListTop.Items.Add(new ListItem(item, item));
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

    private void initcmbFamilyList()
    {
        try
        {
            this.cmbEnergyLabelFamilyList.Items.Clear();
            IList<string> energyLabelList = iEnergyLabel.GetFamily();
            foreach (string item in energyLabelList)
            {
                this.cmbEnergyLabelFamilyList.Items.Add(new ListItem(item, item));
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

    private void initcmbChinaLevelList()
    {
        try
        {
            this.cmbChinaLevel.Items.Clear();
            IList<string> energyLabelList = iEnergyLabel.GetChinaLevel();
            foreach (string item in energyLabelList)
            {
                this.cmbChinaLevel.Items.Add(new ListItem(item, item));
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

    //protected void btnConstValueTypeChange_ServerClick(object sender, System.EventArgs e)
    //{
      
    //    ShowListByType();
    //    //ShowTypeDescriptionByType();
    //    this.updatePanel2.Update();

    //    ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "ConstValueTypeChange", "setNewItemValue();DealHideWait();", true);
        
    //}

    protected void btnTypeListUpdate_ServerClick(object sender, System.EventArgs e)
    {
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnTypeListUpdate", "setNewItemValue();DealHideWait();", true);
    }

    public void cmbEnergyLabelFamilyListTop_Load(object sender, System.EventArgs e)
    {
        btnQuery_ServerClick(sender, e);
    }

    protected void btnQuery_ServerClick(object sender, System.EventArgs e)
    {
        ShowListByType();
        this.updatePanel1.Update();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnTypeListUpdate", "setNewItemValue();DealHideWait();", true);
    }

    private Boolean ShowListByType()
    {
        String type = selecttype.Value;
        type2 = this.cmbEnergyLabelFamilyListTop.SelectedItem.Value;
        try
        {
            EnergyLabelInfo condition = new EnergyLabelInfo();

            if (type2 != "ALL")
            {
                condition.family = type2;
            }


            IList<EnergyLabelInfo> energyLabelList = iEnergyLabel.GetEnergyLabelByCondition(condition);

            if (energyLabelList == null || energyLabelList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(energyLabelList, DEFAULT_ROWS);
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
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(7);
        
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        EnergyLabelInfo item = new EnergyLabelInfo();
        string userName = this.HiddenUserName.Value;
        item.family = this.cmbEnergyLabelFamilyList.SelectedValue;
        item.chinaLevel = this.cmbChinaLevel.SelectedValue;
        item.level = this.txtLevel.Text.Trim();
        item.seriesName = this.txtSeriesName.Text.Trim();
        item.saveEnergy = this.txtSaveEnergy.Text.Trim();
        item.editor = this.hidTbUser.Value;
        //item.cdt = DateTime.Now;
        //item.udt = DateTime.Now;
        try
        {
            iEnergyLabel.addeditEnergyLabel(item,userName);
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
        //ShowTypeDescriptionByType();              
        //String itemId = replaceSpecialChart("");
        string family = item.family;
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + family + "');DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        int ID = Convert.ToInt32(this.hidID.Value);
        try
        {
            EnergyLabelInfo item = new EnergyLabelInfo();
            item.id = ID;
            iEnergyLabel.DeleteEnergyLabel(ID);
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
        //ShowTypeDescriptionByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    protected void btnAddType_ServerClick(Object sender, EventArgs e)
    {
        //this.cmbEnergyLabelFamilyListTop.refresh();

        ShowListByType();
        //ShowTypeDescriptionByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<EnergyLabelInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        
        dt.Columns.Add("Family");//0
        dt.Columns.Add("Series Name");
        dt.Columns.Add("Level");
        dt.Columns.Add("China Level");
        dt.Columns.Add("Save Energy");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        dt.Columns.Add("ID");//8

       if (list != null && list.Count != 0)
        {
            foreach (EnergyLabelInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.family;
                dr[1] = temp.seriesName;
                dr[2] = temp.level;
                dr[3] = temp.chinaLevel;
                dr[4] = temp.saveEnergy;
                dr[5] = temp.editor;
                dr[6] = temp.cdt;
                dr[7] = temp.udt;
                dr[8] = temp.id;
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
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
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
