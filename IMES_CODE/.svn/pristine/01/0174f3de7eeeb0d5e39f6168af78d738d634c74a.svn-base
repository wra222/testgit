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
using IMES.Infrastructure;
using System.Collections.Generic;
using System.Text;

public partial class DataMaintain_LightStation : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private ILightSta iter;
    private const int COL_NUM = 6;

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
        iter = (ILightSta)ServiceAgent.getInstance().GetMaintainObjectByName<ILightSta>(WebConstant.LIGHTSTATIONMAINTAIN);
        if (!this.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            //pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            //pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            //pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            //need change..
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            //load data
            initLabel();
            //find all AC Adaptor...
            //...
            IList<LightStDef> datalst = null;
            try
            {
                datalst = iter.GetAllLightStation();
            }
            catch (FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
                //return;
            }
            catch (Exception ee)
            {
                showErrorMessage(ee.Message);
                //return;
            }
            bindTable(datalst, DEFAULT_ROWS);
        }

    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(this.dOldId.Value.Trim());
            //调用删除方法.
            iter.DelelteLightStationItem(id);
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
        showListByACAdaptorList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "delete", "resetTableHeight();DeleteComplete();HideWait();", true);
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        LightStDef def = new LightStDef();
        def.family = this.ddlFamily.InnerDropDownList.SelectedValue.Trim();
        def.iecpn = this.ttIECPN.Text.Trim();
        def.sation = this.ddlLightStation.InnerDropDownList.SelectedValue.Trim();
        def.editor = this.HiddenUserName.Value;

        System.DateTime cdt = DateTime.Now; ;
        string timeStr = cdt.ToString("yyyy-MM-dd hh:mm:ss");
        def.cdt = timeStr;
        string id = "";
        try
        {

            //调用添加的方法 相同的key时需要抛出异常...
            id = iter.AddLightStationItem(def).ToString();
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
        showListByACAdaptorList();
        this.updatePanel2.Update();
        //    string assemblyId = replaceSpecialChart(adaptor.assemb);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        LightStDef def = new LightStDef();
        def.family = this.ddlFamily.InnerDropDownList.SelectedValue.Trim();
        def.sation = this.ddlLightStation.InnerDropDownList.SelectedValue.Trim();
        def.iecpn = this.ttIECPN.Text.Trim();

        def.editor = this.HiddenUserName.Value.Trim();
        def.id = Convert.ToInt32(this.dOldId.Value.Trim());
        string id = this.dOldId.Value.Trim();
        try
        {
            //调用更新方法1... 相同key时需要抛出异常...
            iter.UpdateLightStationItem(def, Convert.ToInt32(id));
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
        showListByACAdaptorList();
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
        this.lblLightStationLst.Text = this.GetLocalResourceObject(Pre + "_lblLightStationList").ToString();
        this.lblIECPN.Text = this.GetLocalResourceObject(Pre + "_lblIECPN").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblLightStation.Text = this.GetLocalResourceObject(Pre+"_lblLightStation").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();

    }
    private void bindTable(IList<LightStDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colIECPN").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
       
        dt.Columns.Add("id");
        if (list != null && list.Count != 0)
        {
            foreach (LightStDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.iecpn;
                dr[1] = temp.family;
                dr[2] = temp.sation;
                dr[3] = temp.editor;
                dr[4] = temp.cdt;
                dr[5] = temp.udt;
                dr[6] = temp.id.ToString();
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
    private Boolean showListByACAdaptorList()
    {
       
        IList<LightStDef> dataList = null;
        try
        {

            dataList = iter.GetAllLightStation();


            if (dataList == null || dataList.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataList, DEFAULT_ROWS);
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
