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

public partial class DataMaintain_OfflineLabelSetting : System.Web.UI.Page
{
    public String userName;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IOfflineLabelSetting offlineLabelSetting = (IOfflineLabelSetting)ServiceAgent.getInstance().GetMaintainObjectByName<IOfflineLabelSetting>(WebConstant.IOfflineLabelSettingManager);
    private const int COL_NUM = 14;
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
                ShowOfflineLabelSettingList();
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

        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblOfflineLabelSettingText").ToString();
        this.lblFileName.Text = this.GetLocalResourceObject(Pre + "_lblFileNameText").ToString();
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescrText").ToString();
        this.lblLabelSpec.Text = this.GetLocalResourceObject(Pre + "_lblLabelSpecText").ToString();
        this.lblParam1.Text = this.GetLocalResourceObject(Pre + "_lblParam1Text").ToString();
        this.lblParam2.Text = this.GetLocalResourceObject(Pre + "_lblParam2Text").ToString();
        this.lblParam3.Text = this.GetLocalResourceObject(Pre + "_lblParam3Text").ToString();
        this.lblParam4.Text = this.GetLocalResourceObject(Pre + "_lblParam4Text").ToString();
        this.lblParam5.Text = this.GetLocalResourceObject(Pre + "_lblParam5Text").ToString();
        this.lblParam6.Text = this.GetLocalResourceObject(Pre + "_lblParam6Text").ToString();
        this.lblParam7.Text = this.GetLocalResourceObject(Pre + "_lblParam7Text").ToString();
        this.lblParam8.Text = this.GetLocalResourceObject(Pre + "_lblParam8Text").ToString();
        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.InnerText = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    /// <summary>
    /// 设置gridview列的宽度百分比 added by ShhWang on 2011-10-13
    /// </summary>
    private void setColumnWidth()
    {

        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);

        gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(5);

        gd.HeaderRow.Cells[5].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[12].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[13].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[14].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[15].Width = Unit.Percentage(10);



    }
    #endregion


    #region 对于数据的增删改查触发的事件
    /// <summary>
    /// 修改数据方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_serverClick(Object sender, EventArgs e)
    {
        OfflineLableSettingDef item = new OfflineLableSettingDef();
        string oldFileName = this.hidFileName.Value.ToString().Trim();
        item = offlineLabelSetting.getOfflineLabelSetting(oldFileName).First();
        string FileName = this.dFileName.Text;
        item.fileName = FileName;
        item.description = this.dDescr.Text;
        item.labelSpec = this.dLabelSpec.Text;
        item.PrintMode = int.Parse(this.cmbPrintModel.SelectedValue);
        item.SPName = this.txtSPName.Text.ToString().Trim();
        item.param1 = this.dParam1.Text;
        item.param2 = this.dParam2.Text;
        item.param3 = this.dParam3.Text;
        item.param4 = this.dParam4.Text;
        item.param5 = this.dParam5.Text;
        item.param6 = this.dParam6.Text;
        item.param7 = this.dParam7.Text;
        item.param8 = this.dParam8.Text;
        item.editor = this.HiddenUsername.Value;
        item.udt = DateTime.Now;
        try
        {
            offlineLabelSetting.updateOfflineLabelSetting(item, oldFileName); ;

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
        ShowOfflineLabelSettingList();
        this.UpdatePanel2.Update();
        FileName = replaceSpecialChart(FileName);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + FileName + "');", true);

    }

    ///// <summary>
    ///// 添加数据方法
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    protected void btnAdd_serverClick(Object sender, EventArgs e)
    {
        OfflineLableSettingDef item = new OfflineLableSettingDef();
        string FileName = this.dFileName.Text;
        item.fileName = this.dFileName.Text;
        item.description = this.dDescr.Text;
        item.labelSpec = this.dLabelSpec.Text;
        item.PrintMode = int.Parse(this.cmbPrintModel.SelectedValue);
        item.SPName = this.txtSPName.Text.ToString().Trim();
        item.param1 = this.dParam1.Text;
        item.param2 = this.dParam2.Text;
        item.param3 = this.dParam3.Text;
        item.param4 = this.dParam4.Text;
        item.param5 = this.dParam5.Text;
        item.param6 = this.dParam6.Text;
        item.param7 = this.dParam7.Text;
        item.param8 = this.dParam8.Text;
        item.editor = this.HiddenUsername.Value;
        item.cdt = DateTime.Now;
        item.udt = DateTime.Now;
        try
        {
            offlineLabelSetting.addOfflineLabelSetting(item);
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
        ShowOfflineLabelSettingList();
        this.UpdatePanel2.Update();
        FileName = replaceSpecialChart(FileName);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + FileName + "');", true);

    }

    /// <summary>
    /// 删除一条数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        OfflineLableSettingDef item = new OfflineLableSettingDef();
        string FileName = this.dFileName.Text;
        string oldFileName = this.hidFileName.Value.Trim();

        try
        {
            IList<OfflineLableSettingDef> listOffline = new List<OfflineLableSettingDef>();
            listOffline = offlineLabelSetting.getOfflineLabelSetting(oldFileName);
            if (listOffline.Count > 0)
            {
                item = listOffline.First();
            }
            offlineLabelSetting.deleteOfflineLabelSetting(item);
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
        ShowOfflineLabelSettingList();
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
    private void bindTable(IList<OfflineLableSettingDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstFileName").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstDescr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstLabelSpec").ToString());

        dt.Columns.Add("PrintModel");
        dt.Columns.Add("SPName");

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemParam1").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemParam2").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemParam3").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemParam4").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemParam5").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemParam6").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemParam7").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemParam8").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());
        string[] type = { "Bat方式", "模板方式", "PDF方式", "Bartender方式", "Bartender Server方式" };
        if (list != null && list.Count != 0)
        {
            foreach (OfflineLableSettingDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = Null2String(temp.fileName);
                dr[1] = Null2String(temp.description);
                dr[2] = Null2String(temp.labelSpec);
                string printModel = type[temp.PrintMode];
                dr[3] = printModel;
                dr[4] = Null2String(temp.SPName);
                dr[5] = Null2String(temp.param1);
                dr[6] = Null2String(temp.param2);
                dr[7] = Null2String(temp.param3);
                dr[8] = Null2String(temp.param4);
                dr[9] = Null2String(temp.param5);
                dr[10] = Null2String(temp.param6);
                dr[11] = Null2String(temp.param7);
                dr[12] = Null2String(temp.param8);
                dr[13] = Null2String(temp.editor);
                dr[14] = ((System.DateTime)temp.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                dr[15] = ((System.DateTime)temp.udt).ToString("yyyy-MM-dd HH:mm:ss");
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true); 
    }


    /// <summary>
    /// 控制数据列表显示
    /// </summary>
    /// <returns></returns>
    protected Boolean ShowOfflineLabelSettingList()
    {

        try
        {
            IList<OfflineLableSettingDef> dataList;
            dataList = offlineLabelSetting.getAllOfflineLabelSetting();
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
    /// 控制转义字符
    /// </summary>
    /// <param name="sourceData"></param>
    /// <returns></returns>
    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
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

