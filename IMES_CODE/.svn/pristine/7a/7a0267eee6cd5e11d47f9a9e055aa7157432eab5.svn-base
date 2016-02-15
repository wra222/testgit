using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
using log4net;

using System.Data;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;

public partial class Query_SA_SMT_DashboardSet : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public ISA_SMTDashboard iSA_SMTDashboard = ServiceAgent.getInstance().GetObjectByName<ISA_SMTDashboard>(WebConstant.ISA_SMTDashboard);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    private const int DEFAULT_ROWS = 20;
    Boolean isSMTBoardLineLoad;
    private const int COL_NUM = 6;
    public String userName;
    public string connectionStr;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    protected void Page_Load(object sender, EventArgs e)
    {
        connectionStr=CmbDBType.ddlGetConnection();
        isSMTBoardLineLoad = false;
        this.cmbSMTMantainTableLine.Load += new EventHandler(cmbSMTMantainTableLine_Load);
        this.cmbSMTMantainTableLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbSMTLine_Selected);
        pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
        pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
        pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
        pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
        pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
        pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
        userName = Master.userInfo.UserId;
        if (!this.IsPostBack)
        {
            initLabel();
            //initcmbSMTMantainTableLineList();
        }
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "initControls();", true);
    }
    
    protected void btnSysSettingNameChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowListByLine();
        
        //this.updatePanel1.Update();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "SysSettingNameChange", "setNewItemValue();DealHideWait();", true);
    }
    //protected void btnRefreshTimeChange_ServerClick(object sender, System.EventArgs e)
    //{
    //    //ShowListByLine();
    //    //this.updatePanel1.Update();
    //    //this.updatePanel2.Update();
    //    ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "SysSettingNameChange", "setNewItemValue();DealHideWait();", true);
    //}

    protected void btnTypeListUpdate_ServerClick(object sender, System.EventArgs e)
    {
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "btnTypeListUpdate", "setNewItemValue();DealHideWait();", true);
    }

    private void ShowMessage(string p)
    {
        throw new NotImplementedException(p);
    }

    private void initLabel()
    {
        this.lbltopName.Text = this.GetLocalResourceObject(Pre + "_lblLine").ToString();
        this.lblDurTime.Text = this.GetLocalResourceObject(Pre + "_lblDurTime").ToString();
        this.lblStandardOutput.Text = this.GetLocalResourceObject(Pre + "_lblStandardOutput").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.lbltopRefreshTime.Text = this.GetLocalResourceObject(Pre + "_lblRefreshTime").ToString();
        this.lbltopStation.Text = this.GetLocalResourceObject(Pre + "_lbltopStation").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        ShowListByLine();
    }


    private Boolean ShowListByLine()
    {
        
        string line = this.cmbSMTMantainTableLine.InnerDropDownList.SelectedValue;
        this.cmdMaintainSMTDashboardRefreshTime.SmtLine = line;
        this.cmdMaintainSMTDashboardStation.SmtLine = line;
        try
        {
            SMT_DashBoard_MantainInfo condition = new SMT_DashBoard_MantainInfo();
            condition.line = line;
            IList<SMT_DashBoard_MantainInfo> dataList = iSA_SMTDashboard.GetSMTDashBoardLineListByCondition(condition);
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

    private void bindTable(IList<SMT_DashBoard_MantainInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDurTime").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemStandardOutput").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_editor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_cdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_udt").ToString());
        dt.Columns.Add("ID").ToString();

        if (list != null && list.Count != 0)
        {
            foreach (SMT_DashBoard_MantainInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.line;
                dr[1] = temp.durTime;
                dr[2] = temp.standardOutput;
                dr[3] = temp.editor;
                dr[4] = temp.StandardOutputCdt;
                dr[5] = temp.StandardOutputUdt;
                dr[6] = temp.id;

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

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(3);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(4);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(6);

    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string id = this.HidID.Value.Trim();
        string cdt = this.Hcdt.Value.Trim();
        string lineValue = this.cmbSMTMantainTableLine.InnerDropDownList.SelectedValue.Trim();
        string stationValue = this.cmdMaintainSMTDashboardStation.InnerDropDownList.SelectedValue.Trim();
        string durTimeValue = this.dDurTime.Text.Trim();
        string refreshTimeValue = this.cmdMaintainSMTDashboardRefreshTime.InnerDropDownList.SelectedValue;
        string StandardOutputValue = this.dStandardOutput.Text.Trim();
        try
        {
            SMT_DashBoard_MantainInfo condition = new SMT_DashBoard_MantainInfo();
           
            
            if (id != "")
            {
                condition.id = Convert.ToInt32(id);
                condition.line = lineValue;
                condition.refreshTime = Convert.ToInt32(refreshTimeValue);
                condition.station = stationValue;
                condition.editor = userName;
                condition.StandardOutputCdt = Convert.ToDateTime(cdt);
            }
            else
            {
                condition.line = lineValue;
                condition.durTime = durTimeValue;
                condition.refreshTime = Convert.ToInt32(refreshTimeValue);
                if (StandardOutputValue != "")
                {
                    condition.standardOutput = Convert.ToInt32(StandardOutputValue);
                }
                condition.station = stationValue;
                condition.editor = userName;
            }
            //先对SMT_Dashboard_Line_RefreshTime_Station表进行Insert或update
            iSA_SMTDashboard.AddOrUpDateSMTDashboardRefreshTimeAndStation(condition, connectionStr);
            if (refreshTimeValue == "" || StandardOutputValue == "")
            {    
                return;
            }
            IList<SMT_DashBoard_MantainInfo> InfoList = iSA_SMTDashboard.GetSMTDashBoardLineListByCondition(condition);

            SMT_DashBoard_MantainInfo addEditInfo = new SMT_DashBoard_MantainInfo();
            if (InfoList.Count >= 12&&id=="")
            {
                showErrorMessage(pmtMessage6);
                return;
            }
            if (InfoList.Count == 0)
            {
                addEditInfo.line = lineValue;
                addEditInfo.durTime = durTimeValue;
                addEditInfo.standardOutput = Convert.ToInt32(StandardOutputValue);
                addEditInfo.editor = userName;
                iSA_SMTDashboard.AddSMTDashboardInfo(addEditInfo);
            }
            
            else
            {
                addEditInfo.id = Convert.ToInt32(id);
                addEditInfo.line = lineValue;
                addEditInfo.durTime = durTimeValue;
                addEditInfo.editor = userName;
                addEditInfo.standardOutput = Convert.ToInt32(StandardOutputValue);
                addEditInfo.StandardOutputCdt = Convert.ToDateTime(cdt);
                iSA_SMTDashboard.UpdateSMTDashBoardInfo(addEditInfo);
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
        finally
        {
            initLabel();
            //initcmbSMTMantainTableLineList();
            //this.updatePanel1.Update();
            this.updatePanel2.Update();
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();", true);
        }
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        int id = Convert.ToInt32(this.HidID.Value);
        //try
        //{
        //    iSysSetting.DeleteSysSettingInfo(id);
        //}
        //catch (FisException ex)
        //{
        //    showErrorMessage(ex.mErrmsg);
        //    return;
        //}
        //catch (Exception ex)
        //{
        //    //show error
        //    showErrorMessage(ex.Message);
        //    return;
        //}
        //initcmbSysSettingNameList();
        //ShowListByType();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
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

    private void cmbSMTMantainTableLine_Load(object sender, System.EventArgs e)
    {
        if (!this.IsPostBack)
        {
            isSMTBoardLineLoad = true;
            CheckAndStart();
        }
    }

    private void CheckAndStart()
    {
        if (isSMTBoardLineLoad == true)
        {
            ShowListByLine();
        }
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }
    
    public void cmbSMTLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //刷新PdLine下拉框内容
            this.cmdMaintainSMTDashboardRefreshTime.SmtLine = this.cmbSMTMantainTableLine.InnerDropDownList.SelectedValue;
            this.cmdMaintainSMTDashboardRefreshTime.refresh();
            this.cmdMaintainSMTDashboardStation.SmtLine = this.cmbSMTMantainTableLine.InnerDropDownList.SelectedValue;
            this.cmdMaintainSMTDashboardStation.refresh();
            ShowListByLine();

        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

}
