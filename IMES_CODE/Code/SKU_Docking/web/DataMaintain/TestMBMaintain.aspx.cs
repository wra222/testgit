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

public partial class DataMaintain_TestMBMaintain : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    ITestMB iTestMb = (ITestMB)ServiceAgent.getInstance().GetMaintainObjectByName<ITestMB>(WebConstant.ITestMBManager);
    private const int COL_NUM = 8;
    private string curPCode = "";
    private string userName;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    protected bool isFamilyLoad = false;
    protected bool isFamilyListLoad = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CmbFamilyList.InnerDropDownList.Load += new EventHandler(cmbFamilyList_load);
        this.CmbFamily.InnerDropDownList.Load += new EventHandler(cmbFamily_load);
        curPCode = Request["PCode"];
        this.drpCode.SystemType = curPCode;
        try
        {
            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                userName = Master.userInfo.UserId;
                this.HiddenUsername.Value = userName;
                initLabel();
                ShowMBTestCodeList();
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


    #region 页面的label与gridview控件的初始化，added by ShhWang on 2011-10-11
    /// <summary>
    /// 初始化页面Label文本显示，added by ShhWang on 2011-10-13
    /// </summary>
    private void initLabel()
    {
        this.lblFamilyLst.Text = this.GetLocalResourceObject(Pre + "_lblFamilyText").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblTestMBLstText").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamilyText").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCodeText").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemarkText").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblTypeText").ToString();
        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
    }

    /// <summary>
    /// 设置gridview列的宽度百分比 added by ShhWang on 2011-10-13
    /// </summary>
    private void setColumnWidth()
    {

        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(25);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(15);
    }
    #endregion


    #region 对于数据的增删改查触发的事件
    /// <summary>
    /// 添加数据方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_serverClick(Object sender, EventArgs e)
    {

        MB_TestDef mbTest = new MB_TestDef();
        string code = this.drpCode.InnerDropDownList.SelectedValue.ToString();
        string family = this.CmbFamily.InnerDropDownList.SelectedValue.ToString();
        mbTest.Code = code;
        mbTest.editor = this.HiddenUsername.Value;
        mbTest.Family = family;
        int selectIndex = this.drpType.SelectedIndex;
        if (selectIndex == 0)
        {
            mbTest.Type = false;
        }
        else
        {
            mbTest.Type = true;
        }
        mbTest.Remark = this.dRemark.Text;
        mbTest.cdt = DateTime.Now;
        mbTest.udt = DateTime.Now;
        try
        {
            iTestMb.addMB_Test(mbTest);
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
        this.CmbFamilyList.InnerDropDownList.SelectedValue = this.CmbFamily.InnerDropDownList.SelectedValue;
        this.UpdatePanel1.Update();
        this.UpdatePanel2.Update();
        ShowMBTestCodeList();
        int id = getLstID(code, family, mbTest.Type);
        string currentID = replaceSpecialChart(id.ToString());
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + currentID + "');", true);

    }

    /// <summary>
    /// 删除一条数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        MBCodeDef mbcDef = new MBCodeDef();
        string code = this.hidCode.Value;
        string dType = this.hidType.Value;
        bool type = false;
        if (dType == "Image")
        {
            type = true;
        }
        string family = this.hidFamily.Value;
        try
        {
            iTestMb.deleteMB_Test(code, family, type);

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
        ShowMBTestCodeList();
        this.UpdatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "deleteComplete", "DeleteComplete();", true);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Boolean ShowMBTestCodeList()
    {
        try
        {
            IList<MB_TestDef> resultList = new List<MB_TestDef>();
            string family = this.CmbFamilyList.InnerDropDownList.SelectedValue;
            if (family != null && !family.Equals(""))
            {
                resultList = iTestMb.getMBTestbyFamily(family);
            }
            if (resultList == null || resultList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(resultList, DEFAULT_ROWS);
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
    /// <summary>
    /// 根据code，family，type获取数据id号
    /// </summary>
    /// <param name="code"></param>
    /// <param name="family"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    protected int getLstID(string code, string family, bool type)
    {
        int id;
        IList<MB_TestDef> mbtestLst = new List<MB_TestDef>();
        mbtestLst = iTestMb.GetMBTestByCodeFamilyAndType(code, family, type);
        if (mbtestLst != null && mbtestLst.Count > 0)
        {
            id = mbtestLst.First().id;
            return id;
        }
        else
        {
            return 0;
        }

    }
    #endregion


    #region 各下拉列表装载或者变化时触发的事件
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbFamilyList_load(object sender, EventArgs e)
    {
        if (!this.CmbFamilyList.IsPostBack)
        {
            ShowMBTestCodeList();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbFamily_load(object sender, EventArgs e)
    {
        if (!this.CmbFamily.IsPostBack)
        {
            showCodeByFamily();
        }
    }

    /// <summary>
    ///  family列表变化时改变code列表绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FamilyChange_ServerClick(object sender, EventArgs e)
    {
        string selectedValue = this.CmbFamily.InnerDropDownList.SelectedValue;
        if (!selectedValue.Equals(""))
        {
            this.drpCode.Family = selectedValue;
            this.drpCode.initCode();
            if (this.hidCode.Value != "")
            {
                ListItem itemSelected = this.drpCode.InnerDropDownList.Items.FindByValue(this.hidCode.Value);
                if (itemSelected != null)
                {
                    itemSelected.Selected = true;
                }
            }
            ((UpdatePanel)this.drpCode.FindControl("up")).Update();
            //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "FamilyChange", "DealHideWait();", true);
        }
    }

    /// <summary>
    ///family变化时改变数据列表显示
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FamilyListChange_ServerClick(object sender, EventArgs e)
    {
        ShowMBTestCodeList();
        this.UpdatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "FamilyChange", "setNewItemValue();DealHideWait();", true);

    }

    protected void showCodeByFamily()
    {
        string family = this.CmbFamily.InnerDropDownList.SelectedValue;
        this.drpCode.Family = family;
        this.drpCode.initCode();
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
        e.Row.Cells[7].Style.Add("display", "none");
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
    private void bindTable(IList<MB_TestDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstRemark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstIdItem").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (MB_TestDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = Null2String(temp.Code);
                dr[1] = Null2String(temp.Type == true ? "Image" : "Normal");
                dr[2] = Null2String(temp.Family);
                dr[3] = Null2String(temp.Remark);
                dr[4] = Null2String(temp.editor);
                dr[5] = ((System.DateTime)temp.cdt).ToString("yyyy-MM-dd HH:mm:ss");
                dr[6] = ((System.DateTime)temp.udt).ToString("yyyy-MM-dd HH:mm:ss");
                dr[7] = Null2String(temp.id);
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();", true);
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
