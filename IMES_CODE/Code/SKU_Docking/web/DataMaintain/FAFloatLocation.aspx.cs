using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class DataMaintain_FAFloatLocation : System.Web.UI.Page
{

    public String userName;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 30;
    private IFAFloatLocation iFAKitLoc;
    Boolean isFamilyTopLoad;
    Boolean isFamilyLoad;
    Boolean isPdLineLoad;

    private const int COL_NUM = 7;

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
    public string pmtMessage11;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            isFamilyTopLoad = false;
            isFamilyLoad = false;
            isPdLineLoad = false;
            this.cmbMaintainFamilyTop.InnerDropDownList.Load += new EventHandler(cmbFamilyTop_Load);
            this.cmbMaintainFamily.InnerDropDownList.Load += new EventHandler(cmbFamily_Load);
            this.cmbMaintainPdLine.InnerDropDownList.Load += new EventHandler(cmbPdLine_Load);
            iFAKitLoc = ServiceAgent.getInstance().GetMaintainObjectByName<IFAFloatLocation>(WebConstant.FAFloatLocationObjet);

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
            pmtMessage11 = this.GetLocalResourceObject(Pre + "_pmtMessage11").ToString();
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                ShowListByFamilyTop();
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

    //顶部下拉框onchange事件的后台处理
    protected void btnFamilyTopChange_ServerClick(object sender, System.EventArgs e)
    {

        ShowListByFamilyTop();

        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);

    }

    //加载
    private void cmbFamilyTop_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainFamilyTop.IsPostBack)
        {
            isFamilyTopLoad = true;
            CheckAndStart();

        }
    }

    private void cmbFamily_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainFamily.IsPostBack)
        {
            isFamilyLoad = true;
            CheckAndStart();

        }
    }

    private void cmbPdLine_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainPdLine.IsPostBack)
        {
            isPdLineLoad = true;
            CheckAndStart();

        }
    }

    private void CheckAndStart()
    {
        if (isFamilyTopLoad == true & isPdLineLoad == true & isFamilyLoad ==true)
        {
            ShowListByFamilyTop();
        }
    }

    //根据上部下拉框的family获取数据并显示
    private Boolean ShowListByFamilyTop()
    {
        String familyTop = this.cmbMaintainFamilyTop.InnerDropDownList.SelectedValue;
        try
        {
            IList<FAFloatLocationDef> dataList;
            if (familyTop == "All")
            {
                dataList = iFAKitLoc.GetKitLocList();
            }
            else {
                dataList = iFAKitLoc.GetKitLocListByFamily(familyTop);
            }
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

    //长期化lable框
    private void initLabel()
    {
        this.lbList.Text = this.GetLocalResourceObject(Pre + "_lbList").ToString();
        this.lbFamilyTop.Text = this.GetLocalResourceObject(Pre + "_lbFamilyTop").ToString();


        this.lbFamily.Text = this.GetLocalResourceObject(Pre + "_lbFamily").ToString();
        this.lbPType.Text = this.GetLocalResourceObject(Pre + "_lbPType").ToString();
        this.lbPdLine.Text = this.GetLocalResourceObject(Pre + "_lbPdLine").ToString();
        this.lbLocation.Text = this.GetLocalResourceObject(Pre + "_lbLocation").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    //设置GridView的列宽度
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(11);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(30);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(11);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(11);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(11);
    }

    //btnSave的后台处理
    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        FAFloatLocationDef item = new FAFloatLocationDef();

        item.family = this.cmbMaintainFamily.InnerDropDownList.SelectedValue;
        item.partType = this.pType.Text.Trim().ToUpper();
        item.location = this.location.Text.Trim();
        item.pdLine = this.cmbMaintainPdLine.InnerDropDownList.SelectedValue;
        item.editor = this.HiddenUserName.Value;

        string itemId = this.itemId.Value.Trim();
        try
        {
            iFAKitLoc.UpdateKitLoc(item, itemId);
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
        ShowListByFamilyTop();

        String family = replaceSpecialChart(item.family);
        String pType = replaceSpecialChart(item.partType);
        String pdLine = replaceSpecialChart(item.pdLine);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + family + "','" + pType + "','" + pdLine + "');", true);
    }

    //btnAdd的后台处理
    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        FAFloatLocationDef item = new FAFloatLocationDef();
        String family = null;
        String pType = null;
        String pdLine = null;
        item.family = this.cmbMaintainFamily.InnerDropDownList.SelectedValue;
        item.partType = this.pType.Text.Trim().ToUpper();
        item.location = this.location.Text.Trim();
        item.pdLine = this.cmbMaintainPdLine.InnerDropDownList.SelectedValue;
        item.editor = this.HiddenUserName.Value;

        try
        {
            iFAKitLoc.AddKitLoc(item);
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
        ShowListByFamilyTop();
        family = replaceSpecialChart(item.family);
        pType = replaceSpecialChart(item.partType);
        pdLine = replaceSpecialChart(item.pdLine);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + family + "','" + pType + "','" + pdLine + "');", true);

    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        try
        {
            FAFloatLocationDef item = new FAFloatLocationDef();
            item.id = int.Parse(this.itemId.Value.Trim());
            iFAKitLoc.DeleteKitLoc(item);
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
        ShowListByFamilyTop();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);


    }

    private void bindTable(IList<FAFloatLocationDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstPType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstPdLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstLocation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstUdt").ToString());
        dt.Columns.Add("itemId");
        if (list != null && list.Count != 0)
        {
            foreach (FAFloatLocationDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.family;
                dr[1] = temp.partType;
                dr[2] = temp.pdLine;
                dr[3] = temp.location;
                dr[4] = temp.editor;
                dr[5] = temp.cdt;
                dr[6] = temp.udt;
                dr[7] = temp.id.ToString();
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

        gd.DataSource = dt;
        gd.DataBind();
        gd.GvExtHeight = this.dTableHeight.Value;
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true); 
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
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
