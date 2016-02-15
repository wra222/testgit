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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using System.Globalization;
using com.inventec.system.util;

public partial class DataMaintain_MBAssemblyCodeMaitain : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private const int COL_NUM = 7;
    private IMBAssemblyCode IMBAssemblyCode;
    public String userName;
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
            IMBAssemblyCode = (IMBAssemblyCode)ServiceAgent.getInstance().GetMaintainObjectByName<IMBAssemblyCode>(WebConstant.IMBAssemblyCodeManager);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            userName = Master.userInfo.UserId;//UserInfo.UserName;
            this.HiddenUserName.Value = userName;

            if (!this.IsPostBack)
            {
                initLabel();
                ShowMBCFGList();
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

    #region 页面初始化方法
    private void initLabel()
    {
        this.lblMBAssemblyCode.Text = this.GetLocalResourceObject(Pre + "_lblMBAssemblyCodeListText").ToString();
        this.lblMBCode.Text = this.GetLocalResourceObject(Pre + "_lblMBCode").ToString();
        this.lblSeries.Text = this.GetLocalResourceObject(Pre + "_lblSeries").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
        this.lblAssemblyCode.Text = this.GetLocalResourceObject(Pre + "_lblAssemblyCode").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(20);
    }

    protected void gd_RowDataBound(Object sender, GridViewRowEventArgs e)
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
    #endregion


    #region 系统方法
    private void bindTable(IList<MBCFGDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;


        //dt.Columns.Add(" ");

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstMBcode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstSeries").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstAssemblyCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_EditorText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_CdtText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_UdtText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_IDText").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (MBCFGDef temp in list)
            {
                dr = dt.NewRow();


                dr[0] = StringUtil.Null2String(temp.MBCode);
                dr[1] = StringUtil.Null2String(temp.Series);
                string type = StringUtil.Null2String(temp.TP);
                if (type == "PC")
                {
                    dr[2] = "PC SKU";
                }
                else if (type == "RCTO")
                {
                    dr[2] = "RCTO & Base Unit";
                }
                else if (type == "BRZL")
                {
                    dr[2] = "BRZL";
                }
                dr[3] = StringUtil.Null2String(temp.CFG);
                dr[4] = StringUtil.Null2String(temp.Editor);
                dr[5] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[6] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[7] = StringUtil.Null2String(temp.ID);
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
    }

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
    /// 
    /// </summary>
    /// <param name="sourceData"></param>
    /// <returns></returns>
    private static String replaceSpecialChart(String sourceData)
    {
        if (sourceData == null)
        {
            return "";
        }
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }
    #endregion


    #region 增删改查方法
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string mbCode = this.hidMBCode.Value;
        string series = this.hidSeries.Value;
        string type = this.hidType.Value;
        try
        {
            IMBAssemblyCode.DeleteMBCFG(mbCode, series, type);
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
        ShowMBCFGList();
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "delete", "resetTableHeight();DeleteComplete();", true);
    }


    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string mbcode = this.drpMBCode.InnerDropDownList.SelectedValue;
        string series = this.dSeries.Text;
        string type = this.drpType.SelectedValue;
        string assemblyCode = this.dAssemblyCode.Text.ToUpper();
        string oldMbcode = this.hidMBCode.Value;
        string oldSeries = this.hidSeries.Value;
        string oldType = this.hidType.Value;
        try
        {
            MBCFGDef mbcfDef = new MBCFGDef();
            mbcfDef.MBCode = mbcode;
            mbcfDef.Series = series;
            mbcfDef.TP = type;
            mbcfDef.CFG = assemblyCode;
            mbcfDef.Editor = this.HiddenUserName.Value;
            mbcfDef.Udt = DateTime.Now;
            IMBAssemblyCode.UpdateMBCFG(mbcfDef, oldMbcode, oldSeries, oldType);


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
        ShowMBCFGList();
        this.UpdatePanel1.Update();
        int id = getLstID(mbcode, series, type);
        string currentID = replaceSpecialChart(id.ToString());
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + currentID + "');", true);

    }


    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        string mbcode = this.drpMBCode.InnerDropDownList.SelectedValue;
        string series = this.dSeries.Text;
        string type = this.drpType.SelectedValue;
        string assemblyCode = this.dAssemblyCode.Text.ToUpper();

        try
        {
            MBCFGDef mbcfDef = new MBCFGDef();
            mbcfDef.MBCode = mbcode;
            mbcfDef.Series = series;
            mbcfDef.TP = type;
            mbcfDef.CFG = assemblyCode;
            mbcfDef.Editor = this.HiddenUserName.Value;
            mbcfDef.Udt = DateTime.Now;
            mbcfDef.Cdt = DateTime.Now;
            IMBAssemblyCode.AddMBCFG(mbcfDef);
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
        ShowMBCFGList();
        this.UpdatePanel1.Update();
        int id = getLstID(mbcode, series, type);
        string currentID = replaceSpecialChart(id.ToString());
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + currentID + "');", true);
    }

    private Boolean ShowMBCFGList()
    {
        try
        {
            IList<MBCFGDef> mbcfgLst = new List<MBCFGDef>();
            mbcfgLst = IMBAssemblyCode.GetAllMBCFGLst();
            if (mbcfgLst == null || mbcfgLst.Count <= 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(mbcfgLst, DEFAULT_ROWS);
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

    protected int getLstID(string MBCode, string series, string type)
    {
        int id;
        IList<MBCFGDef> mbcfgLst = new List<MBCFGDef>();
        mbcfgLst = IMBAssemblyCode.GetMBCFGByCodeSeriesAndType(MBCode, series, type);
        if (mbcfgLst != null && mbcfgLst.Count > 0)
        {
            id = mbcfgLst.First().ID;
            return id;
        }
        else
        {
            return 0;
        }

    }

    #endregion
}
