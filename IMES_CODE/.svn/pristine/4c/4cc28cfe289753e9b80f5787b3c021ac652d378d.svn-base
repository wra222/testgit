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

public partial class DataMaintain_Part : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IPartManager iPart = (IPartManager)ServiceAgent.getInstance().GetMaintainObjectByName<IPartManager>(WebConstant.IPartManager);
    Boolean isBomNodeTypeLoad;
    Boolean isDescTypeLoad;
    public string username;
    private const int COL_NUM = 10;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                this.drpMaintainPartNodeType.InnerDropDownList.Load += new EventHandler(drpMaintainPartNodeType_OnLoad);
                this.drpTypeDesc.InnerDropDownList.Load += new EventHandler(drpTypeDesc_OnLoad);
                this.drpMaintainPartType.InnerDropDownList.Load += new EventHandler(drpMaintainPartType_OnLoad);
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
                pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
                username = Master.userInfo.UserId;
                this.HiddenUserName.Value = username;
                initLabel();
                bindTable(null, DEFAULT_ROWS);
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "initControls();", true);

            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }


    }


    #region 数据的增删改查

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        PartDef item = new PartDef();
        IList<PartDef> partLst = new List<PartDef>();
        string PartNo = this.dPartNo.Text;
        string nodeType = this.drpMaintainPartType.InnerDropDownList.SelectedValue.ToString();
        string desc = this.drpTypeDesc.InnerDropDownList.SelectedValue.ToString();
        bool autoDL = this.checkAutoDL.Checked;
        item.partNo = PartNo;
        item.partType = this.dPartType.Text;
        item.bomNodeType = nodeType;
        item.descr = desc;
        item.remark = this.dRemark.Text;
        item.custPartNo = this.dCustPn.Text;
        item.flag = 1;
        if (autoDL)
        {
            item.autoDL = "Y";
        }
        else
        {
            item.autoDL = "N";
        }
        item.editor = this.HiddenUserName.Value;
        item.cdt = DateTime.Now;
        item.udt = DateTime.Now;
        try
        {
            iPart.addPart(item);
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
        showListPartByPartType(nodeType);
        this.updatePanel2.Update();
        this.updatePanel3.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + this.dPartNo.Text + "');", true);
    }


    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        PartMaintainInfo item = new PartMaintainInfo();
        string oldPartType = this.drpMaintainPartType.InnerDropDownList.SelectedValue;
        string oldPartNo = this.hidPartNo.Value.ToString();
        try
        {
            iPart.deletePart(oldPartNo);
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
        showListPartByPartType(oldPartType);
        this.updatePanel2.Update();
        this.updatePanel3.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();", true);

    }


    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        PartDef item = new PartDef();
        string oldPartNo = this.hidPartNo.Value.ToString().Trim();
        string partType = this.drpMaintainPartType.InnerDropDownList.SelectedValue.ToString();
        string desc = this.drpTypeDesc.InnerDropDownList.SelectedValue.ToString();
        string partNo = this.dPartNo.Text;
        bool autoDL = this.checkAutoDL.Checked;
        item.partNo = partNo;
        item.partType = this.dPartType.Text;
        item.descr = desc;
        item.remark = this.dRemark.Text;
        item.custPartNo = this.dCustPn.Text;
        item.bomNodeType = this.drpMaintainPartType.InnerDropDownList.SelectedValue.ToString();
        if (autoDL)
        {
            item.autoDL = "Y";
        }
        else
        {
            item.autoDL = "N";
        }
        item.editor = this.HiddenUserName.Value;
        item.udt = DateTime.Now;
        try
        {
            iPart.updatePart(item, oldPartNo);
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
        showListPartByPartType(partType);
        this.updatePanel2.Update();
        this.updatePanel3.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + this.dPartNo.Text + "');", true);
    }



    private Boolean showListPartByPartType(string nodeType)
    {
        try
        {
            IList<PartDef> lstPartByPartType = new List<PartDef>();
            lstPartByPartType = iPart.getLstByBomNode(nodeType);
            bindTable(lstPartByPartType, DEFAULT_ROWS);
            this.updatePanel2.Update();
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);

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


    protected void btnSetAttribute_ServerClick(object sender, EventArgs e)
    {
        string nodeType = this.hidPartType.Value;
        showListPartByPartType(nodeType);
        this.updatePanel2.Update();
        this.updatePanel3.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + this.hidPartNo.Value.Trim() + "');", true);
    }
    #endregion



    #region 下拉列表控件的触发事件

    protected void drpMaintainPartNodeType_OnLoad(object sender, EventArgs e)
    {
        string nodeType = this.drpMaintainPartNodeType.InnerDropDownList.SelectedValue.Trim();
        showListPartByPartType(nodeType);
    }

    protected void drpMaintainPartType_OnLoad(object sender, EventArgs e)
    {
        try
        {
            string partType = this.drpMaintainPartType.InnerDropDownList.SelectedValue;
            if (partType != "")
            {
                this.drpTypeDesc.NodeType = partType;
                this.drpTypeDesc.initMaintainDescType();
                this.updatePanel3.Update();
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "DealHideWait();resetTableHeight();iSelectedRowIndex = null;", true);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);

        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
        }

    }
    protected void drpTypeDesc_OnLoad(object sender, EventArgs e)
    {
        //try
        //{
        //    string partType = this.drpMaintainPartType.InnerDropDownList.SelectedValue;
        //    if (partType != "")
        //    {
        //        this.drpTypeDesc.NodeType = partType;
        //        this.drpTypeDesc.initMaintainDescType();
        //        this.updatePanel3.Update();
        //        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();", true);
        //    }
        //}
        //catch (FisException ex)
        //{
        //    showErrorMessage(ex.mErrmsg);

        //}
        //catch (Exception ex)
        //{
        //    //show error
        //    showErrorMessage(ex.Message);
        //}

    }
    protected void btnBomNodeTypeChange_ServerClick(object sender, EventArgs e)
    {
        string nodeType = this.drpMaintainPartNodeType.InnerDropDownList.SelectedValue.Trim();
        showListPartByPartType(nodeType);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "DealHideWait();resetTableHeight();iSelectedRowIndex = null;", true);
    }


    protected void btnPartTypeChange_ServerClick(object sender, EventArgs e)
    {
        string nodeType = this.drpMaintainPartType.InnerDropDownList.SelectedValue.Trim();
        this.drpTypeDesc.NodeType = nodeType;
        this.drpTypeDesc.initMaintainDescType();
        if (nodeType != "")
        {
            ListItem selectedValue = this.drpTypeDesc.InnerDropDownList.Items.FindByValue(this.hidDesc.Value);
            if (selectedValue != null)
            {
                selectedValue.Selected = true;
            }
        }
        this.updatePanel3.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();DealHideWait()", true);
    }
    #endregion



    #region gridview相关
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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


    private void bindTable(IList<PartDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstNodeType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstDescription").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstPartType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstCustPN").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstAutoDL").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstRemark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());
        if (list != null && list.Count != 0)
        {
            foreach (PartDef pmin in list)
            {
                dr = dt.NewRow();
                dr[0] = Null2String(pmin.partNo);
                dr[1] = Null2String(pmin.bomNodeType);
                dr[2] = Null2String(pmin.descr);
                dr[3] = Null2String(pmin.partType);
                dr[4] = Null2String(pmin.custPartNo);
                dr[5] = Null2String(pmin.autoDL);
                dr[7] = Null2String(pmin.editor);
                dr[6] = Null2String(pmin.remark);
                dr[8] = ((System.DateTime)pmin.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                dr[9] = ((System.DateTime)pmin.udt).ToString("yyyy-MM-dd HH:mm:ss");
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
    private void initLabel()
    {
        this.lblPartNo.Text = this.GetLocalResourceObject(Pre + "_lblPartNoText").ToString();
        this.lblPartList.Text = this.GetLocalResourceObject(Pre + "_lbPartListText").ToString();
        this.lblPartType.Text = this.GetLocalResourceObject(Pre + "_lblPartTypeText").ToString();
        this.lblPartNodeType.Text = this.GetLocalResourceObject(Pre + "_lblPartNodeTypeText").ToString();
        this.lblNodeType.Text = this.GetLocalResourceObject(Pre + "_lblNodeTypeText").ToString();
        this.lblDesc.Text = this.GetLocalResourceObject(Pre + "_lblDescText").ToString();
        this.lblAutoDL.Text = this.GetLocalResourceObject(Pre + "_lblAutoDownloadText").ToString();
        this.lblCustPN.Text = this.GetLocalResourceObject(Pre + "_lblCustPNText").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemarkText").ToString();
        this.btnSetAttribute.Value = this.GetLocalResourceObject(Pre + "_btnSetAttribute").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(10);

    }
    #endregion



    #region 系统相关
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    #endregion

}
