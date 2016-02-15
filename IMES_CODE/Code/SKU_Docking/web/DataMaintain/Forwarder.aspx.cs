using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using log4net;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;
using MaintainControl;
using System.Collections;
using System.Collections.Generic;


public partial class DataMaintain_Forwarder : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IForwarder iForwarder;
    private const int COL_NUM = 9;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    public string today;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            //pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            //pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            //pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();

            iForwarder = (IForwarder)ServiceAgent.getInstance().GetMaintainObjectByName<IForwarder>(WebConstant.MaintainForwarderObject);
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                today = iForwarder.GetCurDate().ToString("yyyy-MM-dd");
                this.dUploadDate.Text = today;
                bindTable(null, DEFAULT_ROWS);
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

    private void initLabel()
    {

        this.lblQueryCondition.Text = this.GetLocalResourceObject(Pre + "_lblQueryCondition").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();

        this.lblUploadDate.Text = this.GetLocalResourceObject(Pre + "_lblUploadDate").ToString();
        this.lblStartDate.Text = this.GetLocalResourceObject(Pre + "_lblStartDate").ToString();
        this.lblEndDate.Text = this.GetLocalResourceObject(Pre + "_lblEndDate").ToString();
        this.lblDate.Text = this.GetLocalResourceObject(Pre + "_lblDate").ToString();
        this.lblForwarder.Text = this.GetLocalResourceObject(Pre + "_lblForwarder").ToString();
        this.lblMAWB.Text = this.GetLocalResourceObject(Pre + "_lblMAWB").ToString();
        this.lblDriver.Text = this.GetLocalResourceObject(Pre + "_lblDriver").ToString();
        this.lblTruckID.Text = this.GetLocalResourceObject(Pre + "_lblTruckID").ToString();

        this.btnQuery.Value  = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();
        this.btnUpdate.Value = this.GetLocalResourceObject(Pre + "_btnUpdate").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnUpload.Value  = this.GetLocalResourceObject(Pre + "_btnUpload").ToString();

    }

    protected void btnQuery_ServerClick(Object sender, EventArgs e)
    {
        ShowList();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "QueryComplete", "ShowRowEditInfo(null);DealHideWait();", true);
    }

    //protected void btnRefrehList1_ServerClick(Object sender, EventArgs e)
    //{
    //    string reworkId = this.dOldId.Value.Trim();
    //    if (ShowList1() == false)
    //    {
    //        return;
    //    }
    //    this.updatePanel1.Update();
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "UpdateComplete", "UpdateComplete('" + reworkId + "');DealHideWait();", true);
    //}


    //鍥犱负閲嶅埛鏂颁篃鍙兘瑕?2涓棩鏈熷弬鏁帮紝鏃ユ湡鍙傛暟鍙湪query鏃舵斁鍏?
    private Boolean ShowList()
    {
        string dateFrom = this.dCalValue1.Value.Trim();
        string dateTo = this.dCalValue2.Value.Trim();
        try
        {
            DataTable dataList;
            dataList = iForwarder.GetForwarderList(dateFrom, dateTo);
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
        return true;

    }


    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);

        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(8);
    }

    private void bindTable(DataTable list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemForwarder").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemMAWB").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDriver").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemTruck").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUploadDate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());
        dt.Columns.Add("Id");
        dt.Columns.Add("ContainerID");
        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < list.Rows[i].ItemArray.Length; j++)
                {

                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        //dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    }
                    else
                    {
                        dr[j] = Null2String(list.Rows[i][j]);
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
        if (dTableHeight.Value!="") gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);

    }


    //protected void btnReworkChange_ServerClick(Object sender, EventArgs e)
    //{
    //    ShowList2();
    //    this.updatePanel2.Update();
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "HideWait", "DealHideWait();", true);

    //}

    protected void btnUpdate_ServerClick(Object sender, EventArgs e)
    {
        ForwarderInfo item = new ForwarderInfo();

        item.Driver = this.dDriver.Text.Trim();
        item.TruckID = this.dTruckID.Text.Trim().ToUpper();
        item.Editor = this.HiddenUserName.Value;

        string oldItemId = this.dOldId.Value.Trim();
        item.Id = Int32.Parse(oldItemId);

        item.Date = this.dDateValue.Value.Trim();
        item.Forwarder = this.dForwarderValue.Value.Trim();
        item.MAWB = this.dMAWBValue.Value.Trim();
        item.ContainerId = this.dContainerID.Text.Trim();
        try
        {
            iForwarder.UpdateForwarder(item);
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
        ShowList();

        String itemId = item.Id.ToString();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);
    }

    //protected void btnAdd_ServerClick(Object sender, EventArgs e)
    //{
    //    QCRatioDef item = new QCRatioDef();
    //    String itemId;

    //    String model = this.cmbMaintainModelByFamily.InnerDropDownList.SelectedValue;
    //    if (model != "")
    //    {
    //        item.Family = model;
    //    }
    //    else
    //    {
    //        item.Family = this.cmbMaintainFamily.InnerDropDownList.SelectedValue;
    //    }

    //    item.QCRatio = this.dQCRatio.Text.Trim();
    //    item.EOQCRatio = this.dEQQCRatio.Text.Trim();
    //    item.PAQCRatio = this.dPAQCRatio.Text.Trim();
    //    item.Customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
    //    item.Editor = this.HiddenUserName.Value;

    //    try
    //    {
    //        itemId = iQCRatio.AddQCRatio(item);
    //    }
    //    catch (FisException ex)
    //    {
    //        showErrorMessage(ex.mErrmsg);
    //        return;
    //    }
    //    catch (Exception ex)
    //    {
    //        //show error
    //        showErrorMessage(ex.Message);
    //        return;
    //    }
    //    ShowListByCustom();
    //    itemId = replaceSpecialChart(itemId);
    //    this.updatePanel2.Update();
    //    ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);

    //}


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        //string familyId = this.dFamily.Text.Trim();
        string oldId = this.dOldId.Value.Trim();
        try
        {
            ForwarderInfo item = new ForwarderInfo();
            item.Id = Int32.Parse(oldId);
            iForwarder.DeleteForwarder(item);
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
        ShowList();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);


    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
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
        //for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        //{
            
        //}
        e.Row.Cells[8].Attributes.Add("style", e.Row.Cells[8].Attributes["style"] + "display:none");
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
