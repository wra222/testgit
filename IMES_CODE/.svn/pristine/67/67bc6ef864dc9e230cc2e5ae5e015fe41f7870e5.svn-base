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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

public partial class DataMaintain_AssetRange : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IAssetRange iassetRange;
    private IAssetRange iassetRange_SH;
    private ISysSetting iSysSetting;
    private const int COL_NUM = 10;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;

    protected void Page_Load(object sender, EventArgs e)
    {
        iassetRange = (IAssetRange)ServiceAgent.getInstance().GetMaintainObjectByName<IAssetRange>(WebConstant.ASSETRANGE);
        iassetRange_SH = (IAssetRange)ServiceAgent.getInstance().GetObjectByName<IAssetRange>(WebConstant.ASSETRANGE);
        iSysSetting = ServiceAgent.getInstance().GetMaintainObjectByName<ISysSetting>(WebConstant.MaintainSysSettingObject);
        if (!this.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
      //      pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            
            //need change..
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            //load data
            initLabel();
            initSelect();
            //find all AC Adaptor...
            //...
            IList<AssetRangeDef> datalst=null;
            try 
            {
                string code = this.cmbCodeSelect.SelectedValue.ToString().Trim();
                //datalst = iassetRange.GetAllAssetRanges();
                datalst = iassetRange.GetAssetRangeByCode(code);
            }
            catch(FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
                return;
            }
            catch(Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }
            bindTable(datalst, DEFAULT_ROWS);

        }

    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(this.dOldId.Value.Trim());
            iassetRange.DeleteAssetRangeItem(id);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        //按照ac adaptor list加载表格中的数据.
        initCodeTopSelect("");
        showListByAssetRangeList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }

    protected void btnCloseRange_ServerClick(object sender, EventArgs e)
    {
        AssetRangeDef def = new AssetRangeDef();
        string id = this.dOldId.Value.Trim();
        try
        {
            def.id = Convert.ToInt32(this.dOldId.Value.Trim());
            def.code = this.ttCode.Text.Trim();//this.ttCode.Text.Trim().ToUpper();
            def.begin = this.ttBegin.Text.Trim(); //this.ttBegin.Text.Trim().ToUpper();
            def.end = this.ttEnd.Text.Trim();//this.ttEnd.Text.Trim().ToUpper();
            def.remark = this.ttRemark.Text.Trim();
            def.status = this.hidStatus.Value;
            def.editor = this.HiddenUserName.Value.Trim();
            iassetRange.CloseActiveRange(def);
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        initCodeTopSelect(def.code);
        showListByAssetRangeList();
        updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "CloseRange", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        int CheckRange = -1;
        AssetRangeDef def = new AssetRangeDef();
        def.code = this.ttCode.Text.Trim();
        def.begin = this.ttBegin.Text.Trim();
        def.end = this.ttEnd.Text.Trim();
        def.remark = this.ttRemark.Text.Trim();
        def.editor = this.HiddenUserName.Value.Trim();
        //CheckRange = string.Compare(def.begin, def.end);
        
        System.DateTime cdt = DateTime.Now;
        string timeStr = cdt.ToString();
        def.cdt = timeStr;
        string id = "";
        try
        {
            iassetRange.CheckAssetRange(def.code, def.begin, def.end);
            iassetRange.CheckAddRangeItem(def, "");
            IList<SysSettingInfo> syslist = iSysSetting.GetSysSettingListByCondition(new SysSettingInfo { name = "CheckIPCAstRange" });
            if (syslist.Count != 0)
            {
                if (syslist[0].value == "Y")
                {
                    iassetRange_SH.CheckAddRangeItem(def, "_SH");
                }
            }
            id = iassetRange.AddAssetRangeItem(def);
            //if (CheckRange == 1)
            //{
            //    throw new Exception("輸入的 [Begin]:" + def.begin + " 不可大於 [End]:" + def.end + " ...");
            //}
            //int length = iassetRange.GetBeginLength(def.code);
            //if (length == -1)
            //{
            //    id = iassetRange.AddAssetRangeItem(def);
            //}
            //else
            //{
            //    if (length == def.begin.Length && length == def.end.Length)
            //    {
            //        id = iassetRange.AddAssetRangeItem(def);
            //    }
            //    else
            //    {
            //        throw new Exception("Input [Begin] and [End] Length need" + length + "!");
            //    }
            //}
            //调用添加的方法 相同的key时需要抛出异常...
            //id = iassetRange.AddAssetRangeItem(def);
            
        }
        catch (FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {

            showErrorMessage(ex.Message);
            return;
        }
        //按照ac adaptor list加载表格中的数据
        //...
        initCodeTopSelect(def.code);
        showListByAssetRangeList();
        this.updatePanel2.Update();
        //    string assemblyId = replaceSpecialChart(adaptor.assemb);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        
        AssetRangeDef def = new AssetRangeDef();
        def.id = Convert.ToInt32(this.dOldId.Value.Trim());
        def.code = this.ttCode.Text.Trim();//this.ttCode.Text.Trim().ToUpper();
        def.begin = this.ttBegin.Text.Trim(); //this.ttBegin.Text.Trim().ToUpper();
        def.end = this.ttEnd.Text.Trim();//this.ttEnd.Text.Trim().ToUpper();
        def.remark = this.ttRemark.Text.Trim();
        def.editor = this.HiddenUserName.Value.Trim();
        string id = this.dOldId.Value.Trim();
        try
        {
            //调用更新方法1... 相同key时需要抛出异常...
            iassetRange.CheckAssetRange(def.code, def.begin, def.end);
            iassetRange.CheckAddRangeItem(def, "");
            IList<SysSettingInfo> syslist = iSysSetting.GetSysSettingListByCondition(new SysSettingInfo { name = "CheckIPCAstRange" });
            if (syslist.Count != 0)
            {
                if (syslist[0].value == "Y")
                {
                    iassetRange_SH.CheckAddRangeItem(def, "_SH");
                }
            }
            iassetRange.UpdateAssetRangeItem(def);
            //int length = iassetRange.GetBeginLength(def.code);
            //if (length == -1)
            //{
            //    iassetRange.UpdateAssetRangeItem(def);
            //}
            //else
            //{
            //    if (length == def.begin.Length && length == def.end.Length)
            //    {
            //        iassetRange.UpdateAssetRangeItem(def);
            //    }
            //    else
            //    {
            //        throw new Exception("Input [Begin] and [End] Length need" + length + "!");
            //    }
            //}
            //iassetRange.UpdateAssetRangeItem(def);

        }
        catch (FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            
            showErrorMessage(ex.Message);
            return;
        }
        //根据ac acdaptor list的数据加载表格中的数据
        //...
        initCodeTopSelect(def.code);
        showListByAssetRangeList();
        updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[9].Attributes.Add("style", e.Row.Cells[8].Attributes["style"] + "display:none");
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
    private void initLabel()
    {
        //this.lblAssetRangeList.Text = this.GetLocalResourceObject(Pre + "_lblAssetRangeList").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblBegin.Text = this.GetLocalResourceObject(Pre + "_lblBegin").ToString();
        this.lblEnd.Text = this.GetLocalResourceObject(Pre + "_lblEnd").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
    }

    private void initSelect()
    {
        initCodeTopSelect("");
    }

    private void initCodeTopSelect(string SelectValue)
    {
        IList<string> lstCodeTop = iassetRange.GetCodeListInAssetRange();
        this.cmbCodeSelect.Items.Clear();
        if (lstCodeTop.Count != 0)
        {
            foreach (string item in lstCodeTop)
            {
                this.cmbCodeSelect.Items.Add(item.Trim());
            }
            if (SelectValue == "")
            {
                IList<AssetRangeDef> datalst = null;
                this.cmbCodeSelect.SelectedIndex = 0;
                string code = this.cmbCodeSelect.SelectedValue.ToString().Trim();
                datalst = iassetRange.GetAssetRangeByCode(code);
                bindTable(datalst, DEFAULT_ROWS);
            }
            else
            {
                this.cmbCodeSelect.SelectedValue = SelectValue;
            }
            
        }
        this.updatePanel1.Update();
    }

    protected void cmbCode_Selected(object sender, EventArgs e)
    {
        IList<AssetRangeDef> datalst = null;
        try
        {
            string Code = this.cmbCodeSelect.SelectedValue.ToString().Trim();
            datalst = iassetRange.GetAssetRangeByCode(Code);
            bindTable(datalst, DEFAULT_ROWS);
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
    }

    protected void Code_Input(object sender, EventArgs e)
    {
        IList<AssetRangeDef> datalst = null;
        try
        {
            string Code = this.hidCodeInput.Value.ToString().Trim();
            datalst = iassetRange.GetAssetRangeByCode(Code);
            if (datalst.Count != 0)
            {
                bindTable(datalst, DEFAULT_ROWS);
                initCodeTopSelect(Code);
            }
            else 
            {
                throw new Exception("Cant find that match Code...");    
            }
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
    }

    private void bindTable(IList<AssetRangeDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblBegin").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEnd").ToString());
        dt.Columns.Add("Status");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblRemark").ToString());
        dt.Columns.Add("Prefix");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblUdt").ToString());
        dt.Columns.Add("id");
        if (list != null && list.Count != 0)
        {
            foreach (AssetRangeDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.code;
                dr[1] = temp.begin;
                dr[2] = temp.end;
                dr[3] = temp.status;
                dr[4] = temp.remark;
                dr[5] = "";
                dr[6] = temp.editor;
                dr[7] = temp.cdt;
                dr[8] = temp.udt;
                dr[9] = temp.id;
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
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(6);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(25);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(12);
    }

   

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return errorMsg;
    }
    private Boolean showListByAssetRangeList()
    {
    //    string acadaptorlst = this.ttAcAdaptorList.Text.Trim();
        IList<AssetRangeDef> dataLst = null;
        try
        {
            //if (acadaptorlst == "")
            //{
            string Code = this.hidCode.Value.ToString();
            //dataLst = iassetRange.GetAllAssetRanges();
            dataLst = iassetRange.GetAssetRangeByCode(Code);
            //}
            //else 
            //{
            //    adaptorLst = iACAdaptor.GetAdaptorByAssembly(acadaptorlst);
            //}

            if (dataLst == null || dataLst.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataLst, DEFAULT_ROWS);
            }
        }
        catch (FisException fex)
        {
           
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
            return false;
        }
        catch (System.Exception ex)
        {

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }

   
}
