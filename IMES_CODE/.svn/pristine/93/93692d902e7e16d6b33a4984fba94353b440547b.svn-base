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

public partial class DataMaintain_SpecialRuninTimeControl : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private const int COL_NUM = 8;
    private IRunInTimeControl runInTimeControlManager;
    private IModelManager modelManager;
    private IFamily2 familyManager;
    public String userName;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            runInTimeControlManager = (IRunInTimeControl)ServiceAgent.getInstance().GetMaintainObjectByName<IRunInTimeControl>(WebConstant.IRunInTimeControl);
            modelManager = (IModelManager)ServiceAgent.getInstance().GetMaintainObjectByName<IModelManager>(WebConstant.IModelManager);
            familyManager = (IFamily2)ServiceAgent.getInstance().GetMaintainObjectByName<IFamily2>(WebConstant.MaintainCommonObject);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            userName = Master.userInfo.UserId;
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



    #region 页面初始化方法
    private void initLabel()
    {

        this.lblCode1.Text = this.GetLocalResourceObject(Pre + "_lblModelOrCPQSNOText").ToString();
        this.lblCode2.Text = this.GetLocalResourceObject(Pre + "_lblModelOrCPQSNOText").ToString();
        this.lblTime.Text = this.GetLocalResourceObject(Pre + "_lblTimeText").ToString();
        this.lblLst.Text = this.GetLocalResourceObject(Pre + "_lstRuninTime").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.lblControlType.Text = this.GetLocalResourceObject(Pre + "_lblControlTypeText").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStationText").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(20);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(15);
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
    private void bindTable(IList<RunInTimeControlInfoMaintain> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_CodeText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstControlType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_TimeText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_EditorText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_CdtText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_UdtText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_TypeText").ToString());
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
                dr[7] = StringUtil.Null2String(temp.Type);
                dr[8] = StringUtil.Null2String(temp.ID);
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
    /// 
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
        string code = this.oldCode.Value.ToString();
        string type = this.hidType.Value.ToString();
        string teststation = this.hidStation.Value;
        string id = this.hidId.Value.Trim();
        try
        {
            //runInTimeControlManager.DeleteRunInTimeControlByTypeCodeAndTeststation(type, code, teststation);
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

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string type = this.hidType.Value;
        string code = this.dCode2.Text;
        string hour = this.txtTime.Text;
        string oldCode = this.oldCode.Value;
        string oldStation = this.hidStation.Value;
        string id = this.hidId.Value.Trim();
        try
        {
            RunInTimeControlInfoMaintain runInTimeControlInfo = new RunInTimeControlInfoMaintain();
            runInTimeControlInfo.Code = code;
            runInTimeControlInfo.TestStation = this.CmbStation.InnerDropDownList.SelectedValue;
            if (!oldCode.Equals(code))
            {
                ModelMaintainInfo model = modelManager.GetModel(code);
                if (CheckIsInModelOrCPQSNO(model, code))
                {
                    if (model != null)
                    {
                        runInTimeControlInfo.Type = "Model";
                    }
                    else
                    {
                        runInTimeControlInfo.Type = "CPQSNO";
                    }
                }
                else
                {
                    showErrorMessage(pmtMessage4);
                    return;
                }
            }
            else
            {
                runInTimeControlInfo.Type = type;
            }

            if (this.drpControlType.SelectedIndex == 0)
            {
                runInTimeControlInfo.ControlType = false;
            }
            else
            {
                runInTimeControlInfo.ControlType = true;
            }
            
            runInTimeControlInfo.Hour = hour;
            runInTimeControlInfo.Editor = this.HiddenUserName.Value;
            runInTimeControlInfo.Udt = DateTime.Now;
            //runInTimeControlManager.UpdateRunInTimeControlByTypeCodeAndTestStation(runInTimeControlInfo, type, oldCode, oldStation);
            runInTimeControlInfo.ID = Int32.Parse(id);
            runInTimeControlManager.UpdateRunInTimeControlById(runInTimeControlInfo);
            //id = GetID(type, code, runInTimeControlInfo.TestStation);

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

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        string type = this.hidType.Value.ToString();
        string code = this.dCode2.Text;
        string hour = this.txtTime.Text;
        int id = 0;
        try
        {
            ModelMaintainInfo model = modelManager.GetModel(code);
            if (CheckIsInModelOrCPQSNO(model, code))
            {
                RunInTimeControlInfoMaintain runintime = new RunInTimeControlInfoMaintain();
                runintime.Code = code;
                if (model != null)
                {
                    runintime.Type = "Model";
                }
                else
                {
                    runintime.Type = "CPQSNO";
                }
                runintime.TestStation = this.CmbStation.InnerDropDownList.SelectedValue;
                int index = this.drpControlType.SelectedIndex;
                if (index == 0)
                {
                    runintime.ControlType = false;
                }
                else
                {
                    runintime.ControlType = true;
                }
                runintime.Hour = hour;
                runintime.Editor = this.HiddenUserName.Value;
                runintime.Cdt = DateTime.Now;
                runintime.Udt = DateTime.Now;
                id = runInTimeControlManager.InsertRunInTimeControl(runintime);
            }
            else
            {
                showErrorMessage(pmtMessage4);
                return;
            }
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

    private Boolean ShowRunInTimeControlList()
    {
        try
        {
            IList<RunInTimeControlInfoMaintain> resultList1 = new List<RunInTimeControlInfoMaintain>();
            IList<RunInTimeControlInfoMaintain> resultList2 = new List<RunInTimeControlInfoMaintain>();
            resultList1 = runInTimeControlManager.GetRunInTimeControlListByType("Model");
            //resultList2 = runInTimeControlManager.GetRunInTimeControlListByType("CPQSNO");
            resultList2 = runInTimeControlManager.GetRunInTimeControlListByCPQSNO("CPQSNO");
            if ((resultList1 == null || resultList1.Count == 0) && (resultList2 == null || resultList2.Count == 0))
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                if (resultList2 != null && resultList2.Count > 0)
                {
                    foreach (RunInTimeControlInfoMaintain rint in resultList2)
                    {
                        resultList1.Add(rint);
                    }
                }
                bindTable(resultList1, DEFAULT_ROWS);
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
    /// 判断输入的code是否在数据库的model或者CPQSNO中
    /// </summary>
    /// <param name="Code"></param>
    /// <returns></returns>
    private bool CheckIsInModelOrCPQSNO(ModelMaintainInfo model, string Code)
    {
        if (model == null && (!runInTimeControlManager.IfProductIsExists(Code)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    //根据type，code，teststation获取数据id
    private int GetID(string type, string code, string testStation)
    {
        RunInTimeControlInfoMaintain runintime = runInTimeControlManager.getRunintimeControlByCodeTypeAndTestStation(type, code, testStation);
        if (runintime != null)
        {
            return runintime.ID;
        }
        else
        {
            return 0;
        }
    }

    #endregion






}
