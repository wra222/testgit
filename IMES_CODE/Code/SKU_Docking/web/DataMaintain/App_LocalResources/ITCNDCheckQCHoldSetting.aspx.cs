﻿/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Service for ITCND Check QC Hold Setting Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/5/10 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/5/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1361-0158, Jessica Liu, 2012-5-31
*/

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


public partial class DataMaintain_ITCNDCheckQCHoldSetting : System.Web.UI.Page
{    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IITCNDCheckQCHoldSetting iITCNDCheckQCHoldSetting;

    private const int COL_NUM = 6;

    /* 2012-5-11
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    */
    public string pmtMessageSelectIsHold;
    public string pmtMessageDelete;
    public string pmtMessageNeedCode;
    public string pmtMessageCodeOverLength;
    public string pmtMessageDescrOverLength;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iITCNDCheckQCHoldSetting = ServiceAgent.getInstance().GetMaintainObjectByName<IITCNDCheckQCHoldSetting>(WebConstant.ITCNDCheckQCHoldSettingObject);

            /* 2012-5-11
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            */
            pmtMessageSelectIsHold = this.GetLocalResourceObject(Pre + "_pmtMessageSelectIsHold").ToString();
            pmtMessageDelete = this.GetLocalResourceObject(Pre + "_pmtMessageDelete").ToString();
            pmtMessageNeedCode = this.GetLocalResourceObject(Pre + "_pmtMessageNeedCode").ToString();
            pmtMessageCodeOverLength = this.GetLocalResourceObject(Pre + "_pmtMessageCodeOverLength").ToString();
            pmtMessageDescrOverLength = this.GetLocalResourceObject(Pre + "_pmtMessageDescrOverLength").ToString();

            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                //bindTable(null, DEFAULT_ROWS);
                //2012-5-17
                this.dListNull.Value = "true";
                ShowList();
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

    private Boolean ShowList()
    {
        try
        {
            IList<ITCNDCheckQCHoldDef> dataList = iITCNDCheckQCHoldSetting.GetITCNDCheckQCHoldList();
            if (dataList == null || dataList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
                //2012-5-17
                this.dListNull.Value = "true";
            }
            else
            {
                bindTable(dataList, DEFAULT_ROWS);
                //2012-5-17
                this.dListNull.Value = "false";
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
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString();
        this.lblIsHold.Text = this.GetLocalResourceObject(Pre + "_lblIsHold").ToString();

        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        ITCNDCheckQCHoldDef item = new ITCNDCheckQCHoldDef();

        //ITC-1361-0158, Jessica Liu, 2012-5-31
        item.Code = this.dCode.Text.ToUpper().Trim();

        if (string.Compare(this.dIsHoldValue.Value, "Yes") == 0)
        {
            item.isHold = "1";
        }
        else if (string.Compare(this.dIsHoldValue.Value, "No") == 0)
        {
            item.isHold = "0";
        }
        else
        {
            showErrorMessage(pmtMessageSelectIsHold);
            return;
        }

        item.Descr = this.dDescr.Text.Trim();
        item.Editor = this.HiddenUserName.Value;

        string oldCode = this.dOldCode.Value.ToUpper().Trim();

        try
        {
            iITCNDCheckQCHoldSetting.UpdateITCNDCheckQCHold(item, oldCode);
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

        ShowList();
               
        String itemId = replaceSpecialChart(item.Code);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        ITCNDCheckQCHoldDef item = new ITCNDCheckQCHoldDef();

        item.Code = this.dCode.Text.ToUpper().Trim();

        if (string.Compare(this.dIsHoldValue.Value, "Yes") == 0)
        {
            item.isHold = "1";
        }
        else if (string.Compare(this.dIsHoldValue.Value, "No") == 0)
        {
            item.isHold = "0";
        }
        else
        {
            showErrorMessage(pmtMessageSelectIsHold);
            return;
        }

        item.Descr = this.dDescr.Text.Trim();
        item.Editor = this.HiddenUserName.Value;
   
        try
        {
            iITCNDCheckQCHoldSetting.AddITCNDCheckQCHold(item);
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
        ShowList();
        String itemId = replaceSpecialChart(item.Code);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);

    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        //ITC-1361-0168, Jessica Liu, 2012-10-24
        //string oldCode = this.dOldCode.Value.ToUpper().Trim();
        string oldCode = this.dOldCode.Value.Trim();
        
        try
        {
            ITCNDCheckQCHoldDef item = new ITCNDCheckQCHoldDef();
            item.Code = oldCode;
            iITCNDCheckQCHoldSetting.DeleteITCNDCheckQCHold(item);
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

        ShowList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<ITCNDCheckQCHoldDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCode").ToString());  //0
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemIsHold").ToString());  //1
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDes").ToString());   

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString()); //4
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString()); //2
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString()); //3
     

       if (list != null && list.Count != 0)
        {
            foreach (ITCNDCheckQCHoldDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Code;
                dr[1] = temp.isHold;
                dr[2] = temp.Descr;
                dr[3] = temp.Editor;
                dr[4] = temp.Cdt;
                dr[5] = temp.Udt;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Count.ToString();

            //2012-5-17
            this.btnDelete.Disabled = false;
            this.btnSave.Disabled = false;
        }else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.hidRecordCount.Value = "";

            //2012-5-16
            this.btnDelete.Disabled = true;
            this.btnSave.Disabled = true;
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
