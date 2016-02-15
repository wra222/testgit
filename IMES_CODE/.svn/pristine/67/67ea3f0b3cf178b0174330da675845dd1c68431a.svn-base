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


public partial class DataMaintain_Line : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private ILine iLine;
    Boolean isCustomerLoad;
    Boolean isStageLoad;

    private const int COL_NUM = 11;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage10;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            isCustomerLoad = false;
            isStageLoad = false;
            this.cmbCustomer.InnerDropDownList.Load += new EventHandler(cmbCustomer_Load);
            this.cmbMaintainStage.InnerDropDownList.Load += new EventHandler(cmbMaintainStage_Load);
            iLine = ServiceAgent.getInstance().GetMaintainObjectByName<ILine>(WebConstant.MaintainLineObject);

            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();

            if (!this.IsPostBack)
            {
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
        ShowListByCustomAndStage();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);
       
    }

    protected void btnCustomerChange_ServerClick(object sender, System.EventArgs e)
    {

        ShowListByCustomAndStage();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "queryUpdate", "setNewItemValue();DealHideWait();", true);
        
    }

    private void cmbCustomer_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbCustomer.IsPostBack)
        {
            isCustomerLoad = true;
            CheckAndStart();

        }
    }

    private void cmbMaintainStage_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainStage.IsPostBack)
        {
            isStageLoad = true;
            CheckAndStart();
        }
    }

    private void CheckAndStart()
    {
        if (isCustomerLoad == true && isStageLoad == true)
        {
            ShowListByCustomAndStage();
        }
    }



    private Boolean ShowListByCustomAndStage()
    {
        String customer = this.cmbCustomer.InnerDropDownList.SelectedValue;
        String stage = this.cmbMaintainStage.InnerDropDownList.SelectedValue;
        try
        {
            DataTable dataList;
            dataList = iLine.GetLineInfoList(customer,stage);
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
        this.lblCustomer.Text = this.GetLocalResourceObject(Pre + "_lblCustomer").ToString();
        this.lblStage.Text=this.GetLocalResourceObject(Pre + "_lblStage").ToString();


        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblDescription.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();

        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[11].Width = Unit.Percentage(13);
        gd.HeaderRow.Cells[12].Width = Unit.Percentage(13);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        //LineDef item = new LineDef();

        LineExinfo item = new LineExinfo();
        
        item.customerID = this.cmbCustomer.InnerDropDownList.SelectedValue.Trim();
        item.stage = this.cmbMaintainStage.InnerDropDownList.SelectedValue.Trim();
        item.line = this.dPdLine.Text.Trim().ToUpper();
        item.descr = this.dDescription.Text.Trim();
        item.AliasLine = this.dAliasLine.Text.Trim();
        //item.AvgManPower = Convert.ToInt32(this.dAvgManPower.Text.Trim());
        item.AvgManPower = this.dAvgManPower.Text.Trim() == "" ? 0 : Convert.ToInt32(this.dAvgManPower.Text.Trim());
        //item.AvgSpeed = Convert.ToInt32(this.dAvgSpeed.Text.Trim());
        item.AvgSpeed = this.dAvgSpeed.Text.Trim() == "" ? 0 : Convert.ToInt32(this.dAvgSpeed.Text.Trim());
        //item.AvgStationQty = Convert.ToInt32(this.dAvgStationQty.Text.Trim());
        item.AvgStationQty = this.dAvgStationQty.Text.Trim()== "" ? 0 : Convert.ToInt32(this.dAvgStationQty.Text.Trim());
        item.IEOwner = this.dIEOwner.Text.Trim();
        item.Owner = this.dOwner.Text.Trim();
        item.editor = this.HiddenUserName.Value; 

        string oldItemId = this.dOldId.Value.Trim();
        try
        {
            iLine.SaveLineEx(item);
            //if (oldItemId == item.line)
            //{
            //    iLine.UpdateLine(item, oldItemId);
            //}
            //else
            //{
            //    
            //}            
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
        ShowListByCustomAndStage();

        String itemId = replaceSpecialChart(item.line);
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);
    }

  

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        string oldId = this.dOldId.Value.Trim();
        try
        {
            iLine.DeleteLine(oldId);
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
        ShowListByCustomAndStage();
        this.updatePanel.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "DeleteComplete();DealHideWait();", true);


    }

    private void bindTable(DataTable  list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        //SELECT [Line]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //      ,[CustomerID]
        //      ,[Stage]
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemPdLine").ToString());
        dt.Columns.Add("Customer");
        dt.Columns.Add("Stage");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDescription").ToString());
        dt.Columns.Add("AliasLine");
        dt.Columns.Add("AvgManPower");
        dt.Columns.Add("AvgSpeed");
        dt.Columns.Add("AvgStationQty");
        dt.Columns.Add("IEOwner");
        dt.Columns.Add("Owner");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

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
        //    e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        //}
        e.Row.Cells[1].Attributes.Add("style", e.Row.Cells[1].Attributes["style"] + "display:none");
        e.Row.Cells[2].Attributes.Add("style", e.Row.Cells[2].Attributes["style"] + "display:none");
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
