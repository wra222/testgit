﻿using System;
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


public partial class DataMaintain_ConstValueType : System.Web.UI.Page
{    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IConstValueType iConstValueType;
    Boolean isConstValueTypeLoad;

    private const int COL_NUM = 6;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            isConstValueTypeLoad = false;
            this.cmbConstValueType.InnerDropDownList.Load += new EventHandler(cmbConstValueType_Load);
            this.cmbConstValueType.BATTCT = Request["Type"];
            iConstValueType = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueType>(WebConstant.ConstValueTypeObject);

            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
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

    protected void btnConstValueTypeChange_ServerClick(object sender, System.EventArgs e)
    {

        ShowListByType();
        ShowTypeDescriptionByType();
        //this.updatePanel1.Update();
        this.updatePanel2.Update();

        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "ConstValueTypeChange", "setNewItemValue();DealHideWait();", true);
        
    }

    protected void btnTypeListUpdate_ServerClick(object sender, System.EventArgs e)
    {
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnTypeListUpdate", "setNewItemValue();DealHideWait();", true);
    }

    private void cmbConstValueType_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbConstValueType.IsPostBack)
        {
            isConstValueTypeLoad = true;
            this.selecttype.Value = this.cmbConstValueType.InnerDropDownList.SelectedValue.ToString().Trim();
            CheckAndStart();
        }
    }

    private void CheckAndStart()
    {
        if (isConstValueTypeLoad == true)
        {
           ShowListByType();
           ShowTypeDescriptionByType();
        }
    }

    private void ShowTypeDescriptionByType()
    {
        string Type = this.selecttype.Value;
        string Description = "";
        IList<ConstValueTypeInfo> dataList = iConstValueType.GetConstValueTypeList("SYS",Type);
        if (dataList == null || dataList.Count == 0)
        {
            Description = "暫無資料...";
        }
        else
        {
            Description = (dataList[0].description.ToString().Trim() != "") ? dataList[0].description.ToString().Trim() : "暫無資料...";
        }
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "UpdatalblDescription", "UpdatalblDescription('" + Description + "');", true);
    }

    private Boolean ShowListByType()
    {
        String type = selecttype.Value;
        //String type = this.cmbConstValueType.InnerDropDownList.SelectedValue;

        try
        {
            IList<ConstValueTypeInfo> dataList = iConstValueType.GetConstValueTypeList(type);

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
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
        this.lblValue.Text = this.GetLocalResourceObject(Pre + "_lblValue").ToString();
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.btnAddType.Value = this.GetLocalResourceObject(Pre + "_btnAddType").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(1);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(1);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        ConstValueTypeInfo item = new ConstValueTypeInfo();
        item.type = selecttype.Value;
        IList<string> TypeList = new List<string>();
        if (this.dValue.Text.Trim() == "" || this.dValue.Text.Trim() == null)
        {
            item.value = this.HidValueList.Value.Trim();
            TypeList = this.HidValueList.Value.Split(',');   
        }
        else
        {
            item.value = this.dValue.Text.Trim();
        }
        item.description = this.dDescr.Text.Trim();
        item.editor = this.HiddenUserName.Value;
        IList<ConstValueTypeInfo> dataList = iConstValueType.GetConstValueTypeList(item.type);
        string[] strarray = item.value.Split(',');
        var Query =
            (from p in dataList
            where strarray.Contains(p.value) && p.description == item.description
            select new ConstValueTypeInfo { id = p.id, type = p.type, value = p.value ,description = p.description }).ToList<ConstValueTypeInfo>();
        
        if (Query.Count != 0)
        {
            string msg = "";
            foreach (ConstValueTypeInfo s in Query)
            {
                msg += s.value+",";
            }
            msg = msg.Substring(0, msg.Length - 1);
            showErrorMessage(msg + Environment.NewLine + pmtMessage6);
            return;
        }
        try
        {
            if (this.HidValueList.Value == "" || this.HidValueList.Value == null)
            {
                if (this.HidID.Value != "" && this.HidID.Value != null)
                {
                    item.id = Convert.ToInt32(this.HidID.Value);
                }

                iConstValueType.UpdateConstValueType(item);
            }
            else
            {
                iConstValueType.InsertMultiConstValueType(item.type, TypeList, item.description, item.editor);
                this.HidValueList.Value = "";
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
        ShowListByType();
        ShowTypeDescriptionByType();       
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();", true);
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        
        //string oldCode = this.dOldCode.Value.Trim();
        string type = selecttype.Value;
        string value = "";
        
        ConstValueTypeInfo item = new ConstValueTypeInfo();
        item.type = selecttype.Value;
        if (this.HidValueList.Value.Trim() == "" || this.HidValueList.Value.Trim() == null)
        {
            value = this.dValue.Text.Trim();
        }
        else
        {
            value = this.HidValueList.Value.Trim();
        }
        try
        {
            if (this.hidSelectId.Value != "")
            {
                string SelectID = this.hidSelectId.Value.Substring(0, this.hidSelectId.Value.Length - 1);
                IList<string> idList = SelectID.Split(',').Where(x => x != "").ToList();
                iConstValueType.RemoveConstValueType(idList);
            }
            else
            {
                if (this.HidValueList.Value == "" || this.HidValueList.Value == null)
                {
                    int id = Convert.ToInt32(this.HidID.Value);
                    iConstValueType.RemoveConstValueType(id);
                }
                else
                {
                    iConstValueType.RemoveMultiConstValueType(type, value);
                    this.HidValueList.Value = "";
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

        ShowListByType();
        ShowTypeDescriptionByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    protected void btnAddType_ServerClick(Object sender, EventArgs e)
    {
        this.cmbConstValueType.refresh();

        //string oldCode = this.dOldCode.Value.Trim();
        
        ShowListByType();
        ShowTypeDescriptionByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<ConstValueTypeInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemValue").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDes").ToString()); 
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString()); //4
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString()); //2
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString()); //3
        dt.Columns.Add("ID").ToString();

       if (list != null && list.Count != 0)
        {
            foreach (ConstValueTypeInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.value;
                dr[1] = temp.description;
                dr[2] = temp.editor;
                dr[3] = temp.cdt;
                dr[4] = temp.udt;
                dr[5] = temp.id;

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
            CheckBox chkSelect = e.Row.FindControl("chk") as CheckBox;
            DataRowView drv = e.Row.DataItem as DataRowView;
            string id = drv["ID"].ToString();
            chkSelect.Attributes.Add("onclick", string.Format("setSelectVal(this,'{0}');", id));
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
