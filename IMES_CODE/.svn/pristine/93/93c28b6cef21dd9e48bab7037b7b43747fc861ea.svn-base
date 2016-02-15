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
using System.Text;
using IMES.Infrastructure;

public partial class DataMaintain_WeightSetting : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IWeightSetting iws;
    private const int COL_NUM = 9;
    public const int ZERO = 0;
    public const int ONE = 1;

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
        iws = (IWeightSetting)ServiceAgent.getInstance().GetMaintainObjectByName<IWeightSetting>(WebConstant.WEIGHTSETTINGMAINTAIN);
        if (!this.IsPostBack)
        {
            //pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            //need change..
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            //load data
            initLabel();
            this.ttCommport.Text = "123";
            //find all AC Adaptor...
            //...
            IList<COMSettingDef> datalst = null;
            try
            {
                datalst = iws.GetAllWeightSettingItems();
            }
            catch (FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
            }
            catch (Exception ee)
            {
                showErrorMessage(ee.Message);
            }
            bindTable(datalst, DEFAULT_ROWS);
          
        }

    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
       
        try
        {
            COMSettingDef def = new COMSettingDef();
            def.id = Convert.ToInt32(this.dOldId.Value.Trim());
            //调用删除方法.
            iws.RemoveWeightSettingItem(def);
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
        //...
        showList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "delete", "resetTableHeight();DeleteComplete();HideWait();", true);
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        COMSettingDef def = new COMSettingDef();
        string name = this.ttName.Text.Trim();
        def.name = name;
        def.commport = this.ttCommport.Text.Trim();
        def.rthreshold = this.ttRThreshold.Text.Trim();
        def.baudRate = this.ttBaudRate.Text.Trim();
        def.handshaking = this.ttHandshaking.Text.Trim();
        def.sthreshold = this.ttSThreshold.Text.Trim();
        def.editor = this.HiddenUserName.Value.Trim();
        //issuecode
        //ITC-1361-0062  itc210012  2012-2-15
        if (!String.IsNullOrEmpty(this.dOldId.Value.Trim()))
        {
            def.id = Convert.ToInt32(this.dOldId.Value.Trim());
        }
       
        int id =0;
        try
        {
            id=iws.AddOrUpdateWeightSettingItem(def);

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

        showList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
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
    private void initLabel()
    {
        
        this.lblWeightSettingTitle.Text = this.GetLocalResourceObject(Pre + "_lblWeightSettingTitle").ToString();
        this.lblName.Text = this.GetLocalResourceObject(Pre + "_lblName").ToString();
        this.lblCommport.Text = this.GetLocalResourceObject(Pre + "_lblCommPort").ToString();
        this.lblRThreshold.Text = this.GetLocalResourceObject(Pre + "_lblRThreshold").ToString();
        this.lblBaudRate.Text = this.GetLocalResourceObject(Pre + "_lblBaudRate").ToString();
        this.lblHandshaking.Text = this.GetLocalResourceObject(Pre + "_lblHandshaking").ToString();
        this.lblSThreshold.Text = this.GetLocalResourceObject(Pre + "_lblSThreshold").ToString();

        //init commport,rt,hand,st value.
        //this.ttCommport.Text = ONE.ToString();
        //this.ttRThreshold.Text = ZERO.ToString();
        //this.ttHandshaking.Text = ZERO.ToString();
        //this.ttSThreshold.Text = ZERO.ToString();
        //this.updatePanel1.Update();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();

    }
    private void bindTable(IList<COMSettingDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colName").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCommPort").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colBaudRate").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRThreshold").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colSThreshold").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colHandskaking").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
        dt.Columns.Add("id");
        if (list != null && list.Count != 0)
        {
            foreach (COMSettingDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.name;
                dr[1] = temp.commport;
                dr[2] = temp.baudRate;
                dr[3] = temp.rthreshold;
                dr[4] = temp.sthreshold;
                dr[5] = temp.handshaking;
                dr[6] = temp.editor;
                dr[7] = temp.cdt;
                dr[8] = temp.udt;
                dr[9] = temp.id.ToString();
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

        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true);
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(14);
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
    private Boolean showList()
    {
       
        IList<COMSettingDef> dataLst = null;
        try
        {
            //if (acadaptorlst == "")
            {
                dataLst = iws.GetAllWeightSettingItems();
            }
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
