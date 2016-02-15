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

public partial class DataMaintain_RunInTimeControl : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IRunInTimeControl runInTimeControlManager;
    private IModelManager modelManager;
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
            runInTimeControlManager = (IRunInTimeControl)ServiceAgent.getInstance().GetMaintainObjectByName<IRunInTimeControl>(WebConstant.IRunInTimeControl);
            modelManager = (IModelManager)ServiceAgent.getInstance().GetMaintainObjectByName<IModelManager>(WebConstant.IModelManager);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject("eng_pmtMessage6").ToString() + "\r\n" + this.GetLocalResourceObject("cn_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            userName = Master.userInfo.UserId;//UserInfo.UserName;
            this.HiddenUserName.Value = userName;
            if (!this.IsPostBack)
            {
                initLabel();
                ShowRunInTimeControlList();
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


    #region 初始化页面显示
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gd_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[7].Style.Add("display", "none");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }

    private void initLabel()
    {

        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamilyText").ToString();
        this.lblTime.Text = this.GetLocalResourceObject(Pre + "_lblTimeText").ToString();
        this.lblLst.Text = this.GetLocalResourceObject(Pre + "_lstRuninTime").ToString();
        this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.InnerText = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStationText").ToString();
        this.lblControlType.Text = this.GetLocalResourceObject(Pre + "_lblControlTypeText").ToString();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(18);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(16);
    }

    private void bindTable(IList<RunInTimeControlInfoMaintain> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_FamilyText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstControlType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_TimeText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_EditorText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_CdtText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_UdtText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstIDText").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (RunInTimeControlInfoMaintain temp in list)
            {
                dr = dt.NewRow();
                dr[0] = StringUtil.Null2String(temp.Code);
                dr[1] = StringUtil.Null2String(temp.TestStation);
                if (temp.ControlType == false)
                {
                    dr[2] = "Longest";
                }
                else
                {
                    dr[2] = "Shortest";
                }

                dr[3] = StringUtil.Null2String(temp.Hour);
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

    #endregion


    #region  增删改查方法
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string code = this.oldCode.Value;
        string teststation = this.hidStation.Value;

        string id = this.hidId.Value.Trim();
        try
        {
            runInTimeControlManager.DeleteRunInTimeControlById(id);
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
        ShowRunInTimeControlList();
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "delete", "resetTableHeight();DeleteComplete();", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string type = "Family";
        string code = this.CmbFamily.InnerDropDownList.SelectedValue;
        string hour = this.txtTime.Text;
        string oldCode = this.oldCode.Value.ToString();
        string oldStation = this.hidStation.Value;
        string id = this.hidId.Value.Trim();
        try
        {
            RunInTimeControlInfoMaintain runInTimeControlInfo = new RunInTimeControlInfoMaintain();
            runInTimeControlInfo.Code = code;
            runInTimeControlInfo.Type = type;
            runInTimeControlInfo.TestStation = this.CmbStation.InnerDropDownList.SelectedValue;
            if (this.drpControlType.SelectedValue == "Longest")
            {
                runInTimeControlInfo.ControlType = false;
            }
            else
            {
                runInTimeControlInfo.ControlType = true;
            }
            runInTimeControlInfo.Hour = this.txtTime.Text;
            runInTimeControlInfo.Editor = this.HiddenUserName.Value;
            runInTimeControlInfo.Udt = DateTime.Now;

            runInTimeControlInfo.ID = Int32.Parse(id);
            runInTimeControlManager.UpdateRunInTimeControlById(runInTimeControlInfo);
            //id = getId(type, code, runInTimeControlInfo.TestStation);
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
        ShowRunInTimeControlList();
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Update", "resetTableHeight();AddUpdateComplete('" + id + "');", true);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        string code = this.CmbFamily.InnerDropDownList.SelectedValue.Trim();
        string time = this.txtTime.Text;
        int id = 0;
        try
        {
            RunInTimeControlInfoMaintain rin = new RunInTimeControlInfoMaintain();
            rin.Code = code;
            rin.Hour = time;
            rin.Type = "Family";
            if (this.drpControlType.SelectedValue == "Shortest")
            {
                rin.ControlType = true;
            }
            else
            {
                rin.ControlType = false;
            }
            rin.TestStation = this.CmbStation.InnerDropDownList.SelectedValue;
            rin.Editor = this.HiddenUserName.Value;
            rin.Cdt = DateTime.Now;
            rin.Udt = DateTime.Now;
            runInTimeControlManager.InsertRunInTimeControl(rin);
            id = getId(rin.Type, rin.Code, rin.TestStation);
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
        ShowRunInTimeControlList();
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Boolean ShowRunInTimeControlList()
    {
        try
        {
            IList<RunInTimeControlInfoMaintain> resultList = new List<RunInTimeControlInfoMaintain>();
            resultList = runInTimeControlManager.GetRunInTimeControlListByType("Family");
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


    public int getId(string type, string code, string testCode)
    {
        RunInTimeControlInfoMaintain runintime = new RunInTimeControlInfoMaintain();
        runintime = runInTimeControlManager.getRunintimeControlByCodeTypeAndTestStation(type, code, testCode);
        if (runintime != null)
        {
            return runintime.ID;
        }
        else
            return 0;
    }
    #endregion


    #region 系统方法
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
}
