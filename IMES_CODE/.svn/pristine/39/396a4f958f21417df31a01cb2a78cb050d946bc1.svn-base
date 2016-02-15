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

public partial class DataMaintain_PartTypeAttribute : System.Web.UI.Page
{

    public String userName;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 30;
    private const string PUB = "<Pub>";
    private IPartTypeAttribute iPartTypeAttribute;
    Boolean isPartTypeLoad;
    Boolean isSiteLoad;
    Boolean isCustLoad;

    private const int COL_NUM = 8;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            isPartTypeLoad = false;
            isSiteLoad = false;
            isCustLoad = false;
            this.cmbMaintainPartType.InnerDropDownList.Load += new EventHandler(cmbPartType_Load);
            this.CmbMaintainPartTypeSite.InnerDropDownList.Load += new EventHandler(cmbPartTypeSite_Load);
            this.CmbMaintainPartTypeCust.InnerDropDownList.Load += new EventHandler(cmbPartTypeCust_Load);
            iPartTypeAttribute = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeAttribute>(WebConstant.PartTypeAttributeObjet);

            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();

            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                ShowListByPartType();
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
    //partType变更时触发
    protected void btnPartTypeChange_ServerClick(object sender, System.EventArgs e)
    {

        ShowListByPartType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);

    }

    private void cmbPartType_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainPartType.IsPostBack)
        {
            isPartTypeLoad = true;
            CheckAndStart();

        }
    }

    private void cmbPartTypeSite_Load(object sender, System.EventArgs e)
    {
        if (!this.CmbMaintainPartTypeSite.IsPostBack)
        {
            isSiteLoad = true;
            CheckAndStart();

        }
    }

    private void cmbPartTypeCust_Load(object sender, System.EventArgs e)
    {
        if (!this.CmbMaintainPartTypeCust.IsPostBack)
        {
            isCustLoad = true;
            CheckAndStart();

        }
    }

    private void CheckAndStart()
    {
        if (isPartTypeLoad == true & isSiteLoad == true & isCustLoad == true)
        {
            ShowListByPartType();
        }
    }

    private Boolean ShowListByPartType()
    {
        String partType = this.cmbMaintainPartType.InnerDropDownList.SelectedValue;
        try
        {
            IList<PartTypeAttributeDef> dataList;
            dataList = iPartTypeAttribute.GetPartTypeListByTp(partType);
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


    private void initLabel()
    {
        this.lbPartType.Text = this.GetLocalResourceObject(Pre + "_lbPartType").ToString();
        this.lbPartTypeList.Text = this.GetLocalResourceObject(Pre + "_lbPartTypeList").ToString();
        this.lbCode.Text = this.GetLocalResourceObject(Pre + "_lbCode").ToString();
        this.lbIndex.Text = this.GetLocalResourceObject(Pre + "_lbIndex").ToString();
        this.lbDescr.Text = this.GetLocalResourceObject(Pre + "_lbDescr").ToString();
        this.lbSite.Text = this.GetLocalResourceObject(Pre + "_lbSite").ToString();
        this.lbCust.Text = this.GetLocalResourceObject(Pre + "_lbCust").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(30);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(10);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        PartTypeAttributeDef item = new PartTypeAttributeDef();
        item.tp = this.cmbMaintainPartType.InnerDropDownList.SelectedValue.Trim();
        item.code = this.code.Text.Trim().ToUpper();
        item.index = this.index.Text.Trim().ToUpper();
        item.description = this.descr.Text.Trim().ToUpper();

        if (this.site.Checked == true)
        {
            item.site = PUB;
        }
        else
        {
            var input = "";
            var SiteChekList = this.CmbMaintainPartTypeSite.InnerDropDownList;
            for (int i = 0; i < SiteChekList.Items.Count; i++)
            {
                if (SiteChekList.Items[i].Selected == true)
                {
                    input = input + "<" + SiteChekList.Items[i].Text + ">";
                }
            }
            item.site = input;
        }
        if (this.cust.Checked == true)
        {
            item.cust = PUB;
        }
        else
        {
            var input = "";
            var CustChekList = this.CmbMaintainPartTypeCust.InnerDropDownList;
            for (int i = 0; i < CustChekList.Items.Count; i++)
            {
                if (CustChekList.Items[i].Selected == true)
                {
                    input = input + "<" + CustChekList.Items[i].Text + ">";
                }
            }
            item.cust = input;
        }
        item.editor = this.HiddenUserName.Value;

        string itemId = this.itemId.Value.Trim();
        try
        {
            iPartTypeAttribute.UpdatePartType(item, itemId);
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
        ShowListByPartType();

        itemId = replaceSpecialChart(itemId);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + itemId + "');", true);
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        PartTypeAttributeDef item = new PartTypeAttributeDef();
        String id = null;
        item.tp = this.cmbMaintainPartType.InnerDropDownList.SelectedValue.Trim();
        item.code = this.code.Text.Trim().ToUpper();
        item.index = this.index.Text.Trim().ToUpper();
        item.description = this.descr.Text.Trim().ToUpper();

        if (this.site.Checked == true)
        {
            item.site = PUB;
        }
        else {
            var input = "";
            var SiteChekList = this.CmbMaintainPartTypeSite.InnerDropDownList;
            for (int i = 0; i < SiteChekList.Items.Count; i++ )
            {
                if (SiteChekList.Items[i].Selected == true)
                {
                    input = input + "<"+SiteChekList.Items[i].Text+">";
                }
            }
            item.site = input;
        }
        if (this.cust.Checked == true)
        {
            item.cust = PUB;
        }
        else
        {
            var input = "";
            var CustChekList = this.CmbMaintainPartTypeCust.InnerDropDownList;
            for (int i = 0; i < CustChekList.Items.Count; i++)
            {
                if (CustChekList.Items[i].Selected == true)
                {
                    input = input + "<" + CustChekList.Items[i].Text + ">";
                }
            }
            item.cust = input;
        }
        item.editor = this.HiddenUserName.Value;

        try
        {
            id = iPartTypeAttribute.AddPartType(item);
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
        ShowListByPartType();
        id = replaceSpecialChart(id);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + id + "');", true);

    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        try
        {
            //PartTypeAttributeDef item = new PartTypeAttributeDef();
            string id = this.itemId.Value.Trim();
            iPartTypeAttribute.DeletePartType(id);
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
        ShowListByPartType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);


    }

    private void bindTable(IList<PartTypeAttributeDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstIndex").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstDescr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstSite").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstCust").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstUdt").ToString());
        dt.Columns.Add("itemId");
        if (list != null && list.Count != 0)
        {
            foreach (PartTypeAttributeDef item in list)
            {
                dr = dt.NewRow();

                dr[0] = item.code;
                dr[1] = item.index;
                dr[2] = item.description;
                dr[3] = item.site;
                dr[4] = item.cust;
                dr[5] = item.editor;
                dr[6] = item.cdt;
                dr[7] = item.udt;
                dr[8] = item.id.ToString();
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
