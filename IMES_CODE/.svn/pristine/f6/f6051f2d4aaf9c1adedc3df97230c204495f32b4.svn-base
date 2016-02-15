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
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;

//ITC-1136-0015 2010,2,4

public partial class DataMaintain_MACRange : System.Web.UI.Page
{
    public String userName;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IMACRange iMACRange;
    private const int COL_NUM = 7;

    public string STATUS_ACTIVE_VALUE;
    public string STATUS_CLOSED_VALUE;
    public string STATUS_CREATE_VALUE;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;
    public string pmtMessage10;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            //this.cmbCustomer.InnerDropDownList.Load += new EventHandler(cmbCustomer_Load);
            //this.cmbCustomer.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCustomer_SelectedChange);

            STATUS_CREATE_VALUE = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMacRangeStatusItemValue1");
            STATUS_ACTIVE_VALUE = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMacRangeStatusItemValue2");
            STATUS_CLOSED_VALUE = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMacRangeStatusItemValue3");
            iMACRange = (IMACRange)ServiceAgent.getInstance().GetMaintainObjectByName<IMACRange>(WebConstant.MaintainCommonObject); 
            
            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
                pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
                pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
                pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
                pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();

                userName = Master.userInfo.UserId;
                initLabel();
                ShowMACRangeList();
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

    //private void cmbCustomer_SelectedChange(object sender, System.EventArgs e)
    //{
    //    ShowFamilyListByCustom();
    //    this.updatePanel.Update();
    //}

    //private void cmbCustomer_Load(object sender, System.EventArgs e)
    //{
    //    ShowFamilyListByCustom();
    //}

    private Boolean ShowMACRangeList()
    {
        //String costomer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        try
        {
            IList<MACRangeDef> dataList;
            dataList = iMACRange.GetMACRangeList();
 
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
        setColumnWidth();
        return true;

    }


    private void ShowMessage(string p)
    {
        throw new NotImplementedException(p);
    }

    private void initLabel()
    {
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatus").ToString();
        this.lblBeginNo.Text = this.GetLocalResourceObject(Pre + "_lblBeginNo").ToString();
        this.lblEndNo.Text = this.GetLocalResourceObject(Pre + "_lblEndNo").ToString();

        this.btnAdd.Value  = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {

        gd.HeaderRow.Cells[0].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(17);

    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        MACRangeDef item = new MACRangeDef();
        item.Code = this.dCode.Text.Trim().ToUpper();
        item.BegNo = this.dBeginNo.Text.Trim().ToUpper();
        item.EndNo = this.dEndNo.Text.Trim().ToUpper();
        item.Editor = this.HiddenUserName.Value;
        //item.Status = this.cmbMacRangeStatus.InnerDropDownList.SelectedItem.Value;//SelectedValue;
        //item.StatusText = this.cmbMacRangeStatus.InnerDropDownList.SelectedItem.Text;

        string oldId = this.dOldId.Value.Trim();
        item.id = oldId;

        try
        {
            iMACRange.UpdateMACRange(item, oldId);
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
        ShowMACRangeList();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + oldId + "');DealHideWait();", true);

    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        MACRangeDef item = new MACRangeDef();
        item.Code = this.dCode.Text.Trim().ToUpper();
        item.BegNo = this.dBeginNo.Text.Trim().ToUpper();
        item.EndNo = this.dEndNo.Text.Trim().ToUpper();
        item.Editor = this.HiddenUserName.Value;
        //item.Status = this.cmbMacRangeStatus.InnerDropDownList.SelectedItem.Value;//SelectedValue;
        //item.StatusText = this.cmbMacRangeStatus.InnerDropDownList.SelectedItem.Text;

        item.Status = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMacRangeStatusItemValue1");
        item.StatusText = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMacRangeStatusItemText1");        

        string newId;
        try
        {
            newId = iMACRange.AddMACRange(item);
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
        ShowMACRangeList();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + newId + "');DealHideWait();", true);
    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string oldId = this.dOldId.Value.Trim();
        try
        {
            iMACRange.DeleteMACRange(oldId);
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
        ShowMACRangeList();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);


    }

    private void bindTable(IList<MACRangeDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_CodeText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_BeginNoText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_EndNoText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_StatusText").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        dt.Columns.Add("Id");
        dt.Columns.Add("StatusId");


        if (list != null && list.Count != 0)
        {
            foreach (MACRangeDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.Code;
                dr[1] = temp.BegNo;
                dr[2] = temp.EndNo;
                dr[3] = temp.StatusText;
                dr[4] = temp.Editor;
                dr[5] = temp.Cdt;
                dr[6] = temp.Udt;
                dr[7] = temp.id.ToString();
                dr[8] = temp.Status;

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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true); 


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

    //private void hideWait()
    //{
    //    StringBuilder scriptBuilder = new StringBuilder();

    //    scriptBuilder.AppendLine("<script language='javascript'>");
    //    scriptBuilder.AppendLine("setCommonFocus();");
    //    scriptBuilder.AppendLine("</script>");

    //    ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
    //}

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
