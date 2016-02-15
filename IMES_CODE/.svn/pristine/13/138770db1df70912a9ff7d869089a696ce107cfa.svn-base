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

public partial class DataMaintain_PODLabel : IMESBasePage
{

    public String userName;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IPODLabelPartMaintain ipodlabel = (IPODLabelPartMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<IPODLabelPartMaintain>(WebConstant.IPODLabelPartManager);
    private const int COL_NUM = 5;
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
            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
                userName = Master.userInfo.UserId;
                this.HiddenUsername.Value = userName;
                initLabel();
                ShowPODLabelPartList();
                setColumnWidth();
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


    #region 页面的label与gridview控件的初始化，added by ShhWang on 2011-10-11
    /// <summary>
    /// 初始化页面Label文本显示，added by ShhWang on 2011-10-13
    /// </summary>
    private void initLabel()
    {

        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblPODLabelListText").ToString();
        this.lblFamilyLabel.Text = this.GetLocalResourceObject(Pre + "_lblFamilyText").ToString();
        this.lblPartNo.Text = this.GetLocalResourceObject(Pre + "_lblPartNoText").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }

    /// <summary>
    /// 设置gridview列的宽度百分比 added by ShhWang on 2011-10-13
    /// </summary>
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(27);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(22);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(17);

    }
    #endregion


    #region 对于数据的增删改查触发的事件
    ///// <summary>
    ///// 修改及添加数据方法
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void btnSave_serverClick(Object sender, EventArgs e)
    {
        PODLabelPartDef item = new PODLabelPartDef();
        string familyId = this.CmbFamily.InnerDropDownList.SelectedValue.ToString().Trim();
        string partNo = this.dPartNo.Text;
        item.family = familyId;
        item.partNo = partNo.Trim(); //partNo.Trim().ToUpper();
        item.editor = this.HiddenUsername.Value;
        item.udt = DateTime.Now;
        string oldPartNo = this.hidPartNo.Value.Trim();
        string oldFamily = this.HidFamily.Value.Trim();
        try
        {
            ipodlabel.UpdatePODLabelPart(item, oldPartNo, oldFamily);
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
        ShowPODLabelPartList();
        this.UpdatePanel2.Update();
        partNo = replaceSpecialChart(partNo);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + partNo + "');", true);

    }

    /// <summary>
    /// 删除一条数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string oldPartNo = this.hidPartNo.Value.Trim();
        try
        {
            ipodlabel.DeletePODLabelPart(oldPartNo);
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
        ShowPODLabelPartList();
        this.UpdatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();", true);

    }
    #endregion


    #region gridview事件
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

    /// <summary>
    /// gridview的数据绑定 added by ShhWang on 2011-10-13
    /// </summary>
    /// <param name="list"></param>
    /// <param name="defaultRow"></param>
    private void bindTable(IList<PODLabelPartDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_partnoText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_familyText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (PODLabelPartDef temp in list)
            {
                dr = dt.NewRow();
                dr[0] = Null2String(temp.partNo);
                dr[1] = Null2String(temp.family);
                dr[2] = Null2String(temp.editor);
                dr[3] = ((System.DateTime)temp.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                dr[4] = ((System.DateTime)temp.udt).ToString("yyyy-MM-dd HH:mm:ss");
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
        setColumnWidth();
        gd.GvExtHeight = dTableHeight.Value;
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "setTableHeight", "resetTableHeight();iSelectedRowIndex = null;", true);

    }

    /// <summary>
    /// 显示数据列表
    /// </summary>
    /// <returns></returns>
    protected Boolean ShowPODLabelPartList()
    {

        try
        {
            IList<PODLabelPartDef> dataList;
            dataList = ipodlabel.GetPODLabelPartList();
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

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }

        return true;

    }
    #endregion


    #region 一些系统方法
    /// <summary>
    /// 错误信息处理
    /// </summary>
    /// <param name="errorMsg"></param>
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 转义字符
    /// </summary>
    /// <param name="sourceData"></param>
    /// <returns></returns>
    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }

    /// <summary>
    /// 空值处理
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
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
