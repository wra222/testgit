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


public partial class DataMaintain_AssetCheckRule : System.Web.UI.Page
{
    public String userName;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IAssetRule iAssetRule;
    private IDefectStation iDefectStation;
    private const int COL_NUM = 10;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;

    public const string ASTTYPE_ATSN1 = "ATSN1";
    public const string ASTTYPE_ATSN4 = "ATSN4";
    public const string ASTTYPE_ATSN7 = "ATSN7";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iAssetRule = ServiceAgent.getInstance().GetMaintainObjectByName<IAssetRule>(WebConstant.MaintainAssetRuleObject);
            iDefectStation = ServiceAgent.getInstance().GetMaintainObjectByName<IDefectStation>(WebConstant.MaintainDefectStationObject);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId; 
                this.HiddenUserName.Value = userName;
                initLabel();
                ShowList();
                InitTp();
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

    protected void btnAstTypeChange_ServerClick(Object sender, EventArgs e)
    {
        iAssetRule = ServiceAgent.getInstance().GetMaintainObjectByName<IAssetRule>(WebConstant.MaintainAssetRuleObject);
        InitCheckItems();
        InitAVPart();
        if (this.dCheckItemValue.Value != "")
        {
            ListItem selectedValue = this.cmbMaintainAssetRuleCheckItem.Items.FindByValue(this.dCheckItemValue.Value);
            if (selectedValue != null) 
            { 
                selectedValue.Selected = true;
            }
        }
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ComBoxAssetRuleAstTypeChange", "DealHideWait();", true);            
    }

    protected void btnTpChange_ServerClick(Object sender, EventArgs e)
    {
        InitAVPart();
        this.updatePanel5.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "ComBTpChange", "DealHideWait();", true);
    }

    private void InitCheckItems()
    {
        string astType = this.cmbMaintainAssetRuleAstType.InnerDropDownList.SelectedValue.Trim();
        string checktype = this.cmbCheckType.SelectedValue;
        this.cmbMaintainAssetRuleCheckItem.Items.Clear();
        this.cmbMaintainAssetRuleCheckItem.Items.Add(string.Empty);
        IList<ConstValueInfo> datalist = null;
        if (checktype == "Value")
        {
            datalist = iAssetRule.GetCheckItemValue("AstRuleCheckItem", astType);
        }
        else
        {
            datalist = iAssetRule.GetCheckItemValue("AstRuleCheckItem",checktype);
        }
        string types = "";
        if (datalist.Count != 0)
        {
            types = datalist[0].value;
        }
        IList<string> list = types.Split(',').ToList();
        if (list.Count > 0)
        {
            for (int i = 0; list.Count > i; i++)
            {
                cmbMaintainAssetRuleCheckItem.Items.Add(list[i].ToString().Trim());
            }
        }
        else
        {
            this.cmbMaintainAssetRuleCheckItem.Items.Add(string.Empty);
        }
    }

    private void InitAVPart()
    {
        string astType = this.cmbMaintainAssetRuleAstType.InnerDropDownList.SelectedValue.Trim();
        string tp = this.cmbTp.SelectedValue;
        this.cmbAVPart.Items.Clear();
        this.cmbAVPart.Items.Add(string.Empty);
        IList<string> list = new List<string>();
        if (astType != "" && tp != "")
        {
            list = iAssetRule.GetAVPartNoValue(astType, tp);
        }

        if (list.Count > 0)
        {
            for (int i = 0; list.Count > i; i++)
            {
                this.cmbAVPart.Items.Add(list[i].ToString().Trim());
            }
        }
        else
        {
            this.cmbAVPart.Items.Add(string.Empty);
        }
        this.updatePanel5.Update();
    }

    private void InitTp()
    {
        this.cmbTp.Items.Clear();
        this.cmbTp.Items.Add(string.Empty);
        IList<ConstValueInfo> datalist = null;
        datalist = iAssetRule.GetCheckItemValue("AstRuleCheckItem", "ASTCategory");
        string types = "";
        if (datalist.Count != 0)
        {
            types = datalist[0].value;
        }
        IList<string> list = types.Split(',').ToList();
        if (list.Count > 0)
        {
            for (int i = 0; list.Count > i; i++)
            {
                this.cmbTp.Items.Add(list[i].ToString().Trim());
            }
        }
        else
        {
            this.cmbTp.Items.Add(string.Empty);
        }
    }
   
    private Boolean ShowList()
    {
        try
        {
            IList<AstRuleInfo> dataList = new List<AstRuleInfo>();
            dataList = iAssetRule.GetAsSetRuleList();
            IList<StationMaintainInfo> stationlist = iDefectStation.GetStationList();
            IList<AstRuleInfo> dataListret = (from assetlist in dataList
                                               join station in stationlist on assetlist.station equals station.Station
                                               select new AstRuleInfo
                                               {
                                                   checkItem = assetlist.checkItem,
                                                   checkTp = assetlist.checkTp,
                                                   code = assetlist.code,
                                                   custName = assetlist.custName,
                                                   editor = assetlist.editor,
                                                   remark = assetlist.remark,
                                                   id = assetlist.id,
                                                   cdt = assetlist.cdt,
                                                   udt = assetlist.udt,
                                                   tp = assetlist.tp,
                                                   station = station.Station + " " + station.Descr
                                               }).ToList();
            if (dataListret == null || dataListret.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataListret, DEFAULT_ROWS);
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
        this.lblASTType.Text = this.GetLocalResourceObject(Pre + "_lblASTType").ToString();
        this.lblCheckItem.Text=this.GetLocalResourceObject(Pre + "_lblCheckItem").ToString();

        //this.lblCheckType.Text = this.GetLocalResourceObject(Pre + "_lblCheckType").ToString();
        this.lblCheckStation.Text = this.GetLocalResourceObject(Pre + "_lblCheckStation").ToString();
        this.lblCustName.Text = this.GetLocalResourceObject(Pre + "_lblCustName").ToString();

        this.lblList.Text=this.GetLocalResourceObject(Pre + "_lblAssetCheckRuleList").ToString();//

        //this.lblList.Text = "Asset&nbspCheck&nbspRule&nbspList:";//
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(9);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(3);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(7);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[9].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[10].Width = Unit.Percentage(2);
    }

   
    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        AstRuleInfo item = new AstRuleInfo();
        item.tp = this.cmbTp.SelectedValue;
        item.code = this.cmbMaintainAssetRuleAstType.InnerDropDownList.SelectedValue;
        item.checkItem = this.cmbMaintainAssetRuleCheckItem.SelectedValue;//this.cmbMaintainAssetRuleCheckItem.InnerDropDownList.SelectedValue;
        item.checkTp = this.cmbCheckType.SelectedValue;
        item.station = this.cmbMaintainAssetRuleCheckStation.InnerDropDownList.SelectedItem.Value;
        item.custName  = this.cmbAVPart.SelectedValue;
        item.editor = Master.userInfo.UserId; //this.HiddenUserName.Value;
        item.remark = this.txtRemark.Text.Trim();
        string itemId = "";
        try
        {
            itemId=iAssetRule.AddAssetRule(item);
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
        //itemId = replaceSpecialChart(itemId);
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemId + "');DealHideWait();", true);

    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        //AstRuleInfo item = new AstRuleInfo();
        //item.tp = "AT";
        //item.code = this.hCode.Value.Trim();
        //item.checkItem = this.cmbMaintainAssetRuleCheckItem.SelectedValue;//this.cmbMaintainAssetRuleCheckItem.InnerDropDownList.SelectedValue;
        //item.checkTp = this.hCheckTp.Value.Trim();
        //item.station = this.hStation.Value.Trim();
        //item.custName = this.dCustName.Text.Trim().ToUpper();
        //item.editor = this.HiddenUserName.Value;
        //item.id = Convert.ToInt32(this.dOldId.Value.Trim());

        try
        {
            iAssetRule.DeleteAssetRule(Convert.ToInt32(this.dOldId.Value.Trim()));
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

    private void bindTable(IList<AstRuleInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCheckStation").ToString());   //0
        dt.Columns.Add("Tp");                                                                   //1
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemASTType").ToString());        //2
        dt.Columns.Add("Check Type");                                                           //3
        dt.Columns.Add("AV PartNo");                                                            //4
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCheckItem").ToString());      //5
        dt.Columns.Add("Remark");                                                               //6
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());         //7
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());            //8
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());            //9
        dt.Columns.Add("id");                                                                   //10

        if (list != null && list.Count != 0)
        {
            foreach (AstRuleInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.station;
                dr[1] = temp.tp;
                dr[2] = temp.code;
                dr[3] = temp.checkTp;
                dr[4] = temp.custName;
                dr[5] = temp.checkItem;
                dr[6] = temp.remark;
                dr[7] = temp.editor;
                dr[8] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[9] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[10] = temp.id;
                dt.Rows.Add(dr);
            }
            this.hidRecordCount.Value = list.Count.ToString();
            for (int i = dt.Rows.Count -1; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
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
