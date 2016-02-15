/*
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


public partial class DataMaintain_LotSetting : System.Web.UI.Page
{    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private LotSetting iLotSetting;

    private const int COL_NUM = 8;//7

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
    public string pmtMessagePassQty;
    public string pmtMessageFailQty;
    public string pmtMessageCheckQty;
    public string pmtMessageDataBoth;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iLotSetting = ServiceAgent.getInstance().GetMaintainObjectByName<LotSetting>(WebConstant.LotSettingObject);

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
            pmtMessagePassQty = this.GetLocalResourceObject(Pre + "_pmtMessagePassQty").ToString();
            pmtMessageFailQty = this.GetLocalResourceObject(Pre + "_pmtMessageFailQty").ToString();
            pmtMessageCheckQty = this.GetLocalResourceObject(Pre + "_pmtMessageCheckQty").ToString();
            pmtMessageDataBoth = this.GetLocalResourceObject(Pre + "_pmtMessageDataBoth").ToString();

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
            IList<LotSettingInfo> dataList = iLotSetting.GetAllLotSettingItems();
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
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lblPassQty.Text = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.lblFailQty.Text = this.GetLocalResourceObject(Pre + "_lblFailQty").ToString();
        this.lblCheckQty.Text = this.GetLocalResourceObject(Pre + "_lblCheckQty").ToString();
        this.dCode.Text = this.GetLocalResourceObject(Pre + "_dCode").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
    }

    private void setColumnWidth()
    {
        //gd.HeaderRow.Cells[0].Width = Unit.Percentage(20);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[5].Width = Unit.Percentage(20);
        //gd.HeaderRow.Cells[6].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(17);
    }
    
    private bool checkIfRecordExist(LotSettingInfo item)
    {
        IList<LotSettingInfo> lotList = iLotSetting.GetAllLotSettingItems();
        foreach (LotSettingInfo tmp in lotList)
        {
            if ((tmp.line == item.line) &&(tmp.type ==item.type)) 
            {
                return true;
            }
        }
        return false;
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        LotSettingInfo item = new LotSettingInfo();
        LotSettingInfo cond = new LotSettingInfo();
        item.line = this.selLine.InnerDropDownList.SelectedValue.Trim();
        item.type = this.cmbType.SelectedItem.ToString();
        item.passQty = Convert.ToInt32(this.textPassQty.Text.Trim());
        item.failQty = Convert.ToInt32(this.textFailQty.Text.Trim());
        item.checkQty = Convert.ToInt32(this.textCheckQty.Text.Trim());
        item.editor = this.HiddenUserName.Value;

        cond.line = this.dOldCode.Value.Trim();
        cond.type = this.dOldType.Value.Trim();

        try
        {
            //if (item.line != cond.line  && item.type != cond.type  && checkIfRecordExist(item))
            if (checkIfRecordExist(item))
            {
                if (!((item.line == cond.line) && (item.type == cond.type)))
                {
                    jsAlert(pmtMessageDataBoth);
                    hideWait();
                    return;   
                }
            }
            iLotSetting.UpdateLotSetting(item, cond);  
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
               
        String itemId = replaceSpecialChart(item.line);
        String itemType = replaceSpecialChart(item.type);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "','" + itemType + "');DealHideWait();", true);
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        LotSettingInfo item = new LotSettingInfo();

        item.line =  this.selLine.InnerDropDownList.SelectedValue.Trim();
        item.type = this.cmbType.SelectedItem.ToString();
        /*
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
*/
        item.passQty =Convert.ToInt32( this.textPassQty.Text.Trim());
        item.failQty = Convert.ToInt32(this.textFailQty.Text.Trim());
        item.checkQty = Convert.ToInt32(this.textCheckQty.Text.Trim());
        item.editor = this.HiddenUserName.Value;
   
        try
        {
            if (checkIfRecordExist(item))
            {
                jsAlert(pmtMessageDataBoth);
                hideWait();
                return;
            }
            iLotSetting.AddLotSetting(item);
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
        String itemId = replaceSpecialChart(item.line);
        String itemType = replaceSpecialChart(item.type);
        this.updatePanel2.Update();
        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "','" + itemType + "');DealHideWait();", true);

    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string oldCode = this.dOldCode.Value.Trim();
        string oldType = this.dOldType.Value.Trim();
        
        try
        {
            LotSettingInfo item = new LotSettingInfo();
            item.line = oldCode;
            item.type  = oldType;
            iLotSetting.RemoveLotSetting(item);
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

    private void bindTable(IList<LotSettingInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCode").ToString());  //line
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemType").ToString());  //Type
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemIsHold").ToString());  //pass qty
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDes").ToString());    // fail qty
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCheckQty").ToString());    // check qty

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString()); //editor
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString()); //cdt
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString()); //udt
     

       if (list != null && list.Count != 0)
        {
            foreach (LotSettingInfo temp in list)
            {
                dr = dt.NewRow();

                //dr[0] = temp.line;
                //dr[1] = temp.passQty;
                //dr[2] = temp.failQty;
                //dr[3] = temp.checkQty;
                //dr[4] = temp.editor;
                //dr[5] = temp.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                //dr[6] = temp.udt.ToString("yyyy-MM-dd hh:mm:ss");
                dr[0] = temp.line;
                dr[1] = temp.type;
                dr[2] = temp.passQty;
                dr[3] = temp.failQty;
                dr[4] = temp.checkQty;
                dr[5] = temp.editor;
                dr[6] = temp.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                dr[7] = temp.udt.ToString("yyyy-MM-dd hh:mm:ss");
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

    private void jsAlert(string info)
    {
        String script = "<script language='javascript'> alert('" + info + "'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "jsAlert", script, false);
    }

    private void hideWait()
    {
        String script = "<script language='javascript'> HideWait(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "hideWait", script, false);
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
