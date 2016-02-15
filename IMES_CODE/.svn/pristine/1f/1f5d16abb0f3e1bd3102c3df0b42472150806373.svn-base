using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using System.Collections.Generic;

public partial class DataMaintain_Station : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS =31;
    private IStation2 iStation;

    private const int COL_NUM = 8;

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

            iStation = ServiceAgent.getInstance().GetMaintainObjectByName<IStation2>(WebConstant.MaintainStationObject);

            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                //bindTable(null, DEFAULT_ROWS);
                ShowList();
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

    private Boolean ShowList()
    {
        try
        {
            DataTable dataList;
            dataList = iStation.GetStationInfoList();
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

    private void initLabel()
    {
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();

        this.lblStation.Text=this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.lblName.Text = this.GetLocalResourceObject(Pre + "_lblName").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();
        this.lblObject.Text = this.GetLocalResourceObject(Pre + "_lblObject").ToString();
        this.lblDescription.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        this.btnInfo.Value = this.GetLocalResourceObject(Pre + "_btnInfo").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(14);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        StationDef item = new StationDef();

        item.station = this.dStation.Text.Trim();
        item.stationType = this.cmbMaintainStationType.InnerDropDownList.SelectedValue.Trim();
        item.operationObject = this.cmbMaintainStationObject.InnerDropDownList.SelectedValue.Trim();
        item.descr = this.dDescription.Text.Trim();
        item.name = this.dName.Text.Trim();

        item.editor = this.HiddenUserName.Value;

        string oldItemId = this.dOldId.Value.Trim();
        try
        {
            if (oldItemId == item.station)
            {
                iStation.UpdateStation(item, oldItemId);
            }
            else
            {
                iStation.AddStation(item);
            }
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

        String itemId = replaceSpecialChart(item.station);
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);

    }

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string oldId = this.dOldId.Value.Trim();
        try
        {
            iStation.DeleteStation(oldId);
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
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);

    }

    private void bindTable(DataTable  list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        //SELECT [Station]
        //      ,[StationType]
        //      ,[OperationObject]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemName").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemOperationObject").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDescription").ToString());
        
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        dt.Columns.Add("OperationObjectValue");
        
        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                 for (int j = 0; j < list.Rows[i].ItemArray.Count(); j++)
                {
                    //dr[j] = Null2String(list.Rows[i][j]);

                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        dr[j] = Null2String(list.Rows[i][j]);
                    }

                }
                dr[8]=dr[3];
                dr[3] = GetSelectTextByValueOperationObject(dr[8].ToString());
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
        
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();", true); 
    }


    private string GetSelectTextByValueOperationObject(string value)
    {
        string returnValue = "";
        List<string[]> selectValues = new List<string[]>();
        string[] item1 = new String[2];
        item1[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationObjectText1");
        item1[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationObjectValue1");
        selectValues.Add(item1);

        string[] item2 = new String[2];
        item2[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationObjectText2");
        item2[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationObjectValue2");
        selectValues.Add(item2);

        string[] item3 = new String[2];
        item3[0] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationObjectText3");
        item3[1] = Resources.ComboxFixValues.ResourceManager.GetString(Pre + "_CmbMaintainStationObjectValue3");
        selectValues.Add(item3);

        for (int k = 0; k < selectValues.Count; k++)
        {
            if (value.Trim() == selectValues[k][1])
            {
                returnValue = selectValues[k][0];
                break;
            }
        }
        return returnValue;
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

}
