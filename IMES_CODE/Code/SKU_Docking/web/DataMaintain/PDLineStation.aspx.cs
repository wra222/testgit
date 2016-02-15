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

public partial class DataMaintain_PDLineStation : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IPdLineStation pdLineStation;

    public String userName;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public log4net.ILog log = log4net.LogManager.GetLogger("DataMaintain_PDLineStation"); 
    protected void Page_Load(object sender, EventArgs e)
    {
        log.Debug("MaintainPage PDLineStation is loading... ...");
        try
        {
            pdLineStation = (IPdLineStation)ServiceAgent.getInstance().GetMaintainObjectByName<IWarranty>(WebConstant.IPdLineStation);

            //注册Customer下拉框的选择事件
            this.cmbCustomer.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbCustomer_Selected);
            this.cmbStage.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbStage_Selected);
            this.CmbMaintainLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbLine_Selected);

            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                bindTable(null, DEFAULT_ROWS);
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

    protected void btnStageChange_ServerClick(object sender, System.EventArgs e)
    {
        ShowLineStationList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "DealHideWait();", true);

    }

    protected void btnCustomerChange_ServerClick(object sender, System.EventArgs e)
    {

        ShowLineStationList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "DealHideWait();", true);

    }

    protected void btnLineChange_ServerClick(object sender, System.EventArgs e)
    {

        ShowLineStationList();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "DealHideWait();", true);

    }
    /// <summary>
    /// 选择Customer下拉框，刷新Line
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void cmbCustomer_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //刷新PdLine下拉框内容
            this.CmbMaintainLine.Customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
            this.CmbMaintainLine.Stage = this.cmbStage.InnerDropDownList.SelectedValue;
            this.CmbMaintainLine.refresh();

            ShowLineStationList();
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

    /// <summary>
    /// 选择Stage下拉框，会刷新Line下拉框内容
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void cmbStage_Selected(object sender, System.EventArgs e)
    {
        try
        {
            //刷新PdLine下拉框内容
            this.CmbMaintainLine.Customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
            this.CmbMaintainLine.Stage = this.cmbStage.InnerDropDownList.SelectedValue;
            this.CmbMaintainLine.refresh();

            ShowLineStationList();

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

    /// <summary>
    /// 选择Stage下拉框
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void cmbLine_Selected(object sender, System.EventArgs e)
    {
        try
        {
            ShowLineStationList();
            this.updatePanel.Update();

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

    private void initLabel()
    {
        this.lblCustomer.Text = this.GetLocalResourceObject(Pre + "_lblCustomerText").ToString();
        this.lblStage.Text = this.GetLocalResourceObject(Pre + "_lblStageText").ToString();
        this.lblLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLineText").ToString();
        this.lblLineStationList.Text = this.GetLocalResourceObject(Pre + "_lblStationListText").ToString();

        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStationText").ToString();
        this.lblStatus.Text = this.GetLocalResourceObject(Pre + "_lblStatusText").ToString();

        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.DropDownList_MyStatus.Items.Add(new ListItem("Optional", "Optional"));
        this.DropDownList_MyStatus.Items.Add(new ListItem("Normal", "Normal"));
        this.DropDownList_MyStatus.SelectedIndex = 1;
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);  
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(30);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(16);
    }

    private void bindTable(DataTable list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;


        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_IDText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_StationText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_TypeText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_StatusText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_DescriptionText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_EditorText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_CdtText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_UdtText").ToString());

        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < list.Rows[i].ItemArray.Length; j++)
                {
                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        if (j == 3)
                        {
                            if (list.Rows[i][j].ToString().Trim().Equals("0"))
                            {
                                dr[3] = "Optional";
                            }
                            else if (list.Rows[i][j].ToString().Trim().Equals("1"))
                            {
                                dr[3] = "Normal";
                            }
                        }
                        else
                        {
                            dr[j] = StringUtil.Null2String(list.Rows[i][j]);
                        }
                    }
                }

                dt.Rows.Add(dr);
            }

            for (int i = list.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Rows.Count.ToString();
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

    }

    private void alertErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("alert('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "alertErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        if (sourceData == null)
        {
            return "";
        }
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string id = this.ID.Value.Trim();
        try
        {
            pdLineStation.DeleteLineStationByID(int.Parse(id));
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
        ShowLineStationList();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();DeleteComplete();", true);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
        pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();

        LineStation item = new LineStation();

        string id = this.ID.Value.Trim();
        string oldStation = this.oldStation.Value.Trim();

        string line = this.CmbMaintainLine.InnerDropDownList.SelectedValue.Trim();
        string station = this.cmbStation.InnerDropDownList.SelectedValue.Trim();
        if(line.Equals("") || station.Equals(""))
        {
            alertErrorMessage(pmtMessage3);
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();ShowRowEditInfo(null);", true);
            return;
        }

        string status = this.DropDownList_MyStatus.SelectedIndex.ToString();

        if (station != "" && station != oldStation && pdLineStation.IFLineStationIsExists(line, station))
        {
            alertErrorMessage(pmtMessage1);
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "ShowRowEditInfo(null);DealHideWait();", true);
            return;
        }
        try
        {
            item.Line = line;
            item.Station = station;
            item.Status = status;
            item.Editor = this.HiddenUserName.Value;
            item.Udt = DateTime.Now;
            if (station != "" && station != oldStation)
            {
                item.Cdt = DateTime.Now;
                id=pdLineStation.AddLineStation(item).ToString();
            }
            else if (station != "" && station == oldStation)
            {
                item.ID = int.Parse(id);
                pdLineStation.UpdateLineStation(item);
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

        ShowLineStationList();
        id = replaceSpecialChart(id);

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Update", "AddUpdateComplete('" + id + "');DealHideWait();", true);

    }


    private Boolean ShowLineStationList()
    {
        String line = this.CmbMaintainLine.InnerDropDownList.SelectedValue;
        try
        {
            DataTable dataList = pdLineStation.GetPdLineStationListByLine(line);
            if (dataList == null || dataList.Rows.Count == 0)
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

        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DealHideWait();ShowRowEditInfo(null);", true);
        return true;

    }

    protected void gd_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");

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
}
