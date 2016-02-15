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


public partial class DataMaintain_KittingCode : System.Web.UI.Page
{    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IKittingCodeMaintain iKittingCodeMaintain;
    Boolean isKittingCodeTypeLoad;

    private const int COL_NUM = 6;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            isKittingCodeTypeLoad = false;
            this.cmbKittingCodeType.InnerDropDownList.Load += new EventHandler(cmbKittingCodeType_Load);
            iKittingCodeMaintain = ServiceAgent.getInstance().GetMaintainObjectByName<IKittingCodeMaintain>(WebConstant.MaintainKittingCodeObject);

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

    protected void btnKittingCodeTypeChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowListByType();
        this.updatePanel1.Update();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "ComKittingCodeTypeChange", "setNewItemValue();DealHideWait();", true);
        
    }

    private void cmbKittingCodeType_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbKittingCodeType.IsPostBack)
        {
            isKittingCodeTypeLoad = true;
            CheckAndStart();

        }
    }

    private void CheckAndStart()
    {
        if (isKittingCodeTypeLoad == true && isKittingCodeTypeLoad == true)
        {
           ShowListByType();
        }
    }

    private Boolean ShowListByType()
    {
        String type = this.cmbKittingCodeType.InnerDropDownList.SelectedValue;

        try
        {
            IList<KittingCodeDef> dataList = iKittingCodeMaintain.GetKittingCodeList(type);
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

        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();

        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(1);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        KittingCodeDef item = new KittingCodeDef();
      
        item.Code = this.dCode.Text.Trim();
        item.Type = this.cmbKittingCodeType.InnerDropDownList.SelectedValue;

        item.Remark = this.dRemark.Text.Trim();
        item.Descr = this.dDescr.Text.Trim();
        item.Editor = this.HiddenUserName.Value; 
        string oldCode = this.dOldCode.Value.Trim();
        try
        {
            iKittingCodeMaintain.UpdateKittingCode(item, oldCode);
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
               
        String itemId = replaceSpecialChart(item.Code);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        KittingCodeDef item = new KittingCodeDef();

        item.Code = this.dCode.Text.Trim();
        item.Type = this.cmbKittingCodeType.InnerDropDownList.SelectedValue;

        item.Remark = this.dRemark.Text.Trim();
        item.Descr = this.dDescr.Text.Trim();
        item.Editor = this.HiddenUserName.Value;
   
        try
        {
             iKittingCodeMaintain.AddKittingCode(item);
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
        String itemId = replaceSpecialChart(item.Code);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);

    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        string oldCode = this.dOldCode.Value.Trim();
        string type = this.cmbKittingCodeType.InnerDropDownList.SelectedValue;
        try
        {
            KittingCodeDef item = new KittingCodeDef();
            item.Code = oldCode;
            item.Type = type;
            iKittingCodeMaintain.DeleteKittingCode(item);
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
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void bindTable(IList<KittingCodeDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCode").ToString());  //0
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDes").ToString());   
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemRemark").ToString());  //1

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString()); //4
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString()); //2
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString()); //3

        dt.Columns.Add("Type"); //6       

       if (list != null && list.Count != 0)
        {
            foreach (KittingCodeDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Code;
                dr[1] = temp.Descr;
                dr[2] = temp.Remark;
                dr[3] = temp.Editor;
                dr[4] = temp.Cdt;
                dr[5] = temp.Udt;
                dr[6] = temp.Type;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Count.ToString();
        }else
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
