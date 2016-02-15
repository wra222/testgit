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

public partial class DataMaintain_CombineKPSetting : System.Web.UI.Page
{

    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private ICombineKPSetting icombineKPSetting;
    private const int COL_NUM = 8;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;
    public string pmtMessage10;
    public string pmtMessage11;
    public string pmtMessage12;
    protected void Page_Load(object sender, EventArgs e)
    {
        icombineKPSetting = (ICombineKPSetting)ServiceAgent.getInstance().GetMaintainObjectByName<ICombineKPSetting>(WebConstant.ICOMBINEKPSETTING);
        if (!this.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            //pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            //pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage12 = this.GetLocalResourceObject(Pre + "_pmtMessage12").ToString();
            pmtMessage11 = this.GetLocalResourceObject(Pre + "_pmtMessage11").ToString();
            //need change..
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            initLabel();
            IList<StationCheckInfo> datalst = null;
            try
            {
                datalst = icombineKPSetting.GetAllCombineKPSettingItems();
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
            this.CmbCheckType.StateOfDrp = "checktype";
            this.CmbStation.StateOfDrp = "station";
            this.CmbLine.StateOfDrp = "line";
        }
    }

    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        try
        {
            StationCheckInfo def = new StationCheckInfo();
            //调用删除方法.
            def.id = Convert.ToInt32(this.hidhidcol.Value.Trim());
            icombineKPSetting.RemoveCombineKPSetting(def);
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
        this.CmbCheckType.setSelected(0);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {

        //this.CmbCheckType.setSelected(1);
        //this.CmbStation.clearContent();


        StationCheckInfo itcnd = new StationCheckInfo();
        itcnd.line = this.CmbLine.InnerDropDownList.SelectedValue.Trim();
        itcnd.station = this.CmbStation.InnerDropDownList.SelectedValue.Trim();
        itcnd.checkItemType = this.CmbCheckType.InnerDropDownList.SelectedValue.Trim();
        itcnd.family = this.TxtFamily.Text.Trim();
        itcnd.model = this.txtModel.Text.Trim();
        itcnd.customer = "HP";
        itcnd.editor = this.HiddenUserName.Value.Trim();
        itcnd.cdt = DateTime.Now;
        itcnd.udt = DateTime.Now;
        string con =  itcnd.station + itcnd.line + itcnd.checkItemType + itcnd.editor + itcnd.cdt.ToString("yyyy-MM-dd HH:mm:ss") + itcnd.udt.ToString("yyyy-MM-dd HH:mm:ss");
        con = con.Trim();
        try
        {
            //调用添加的方法 相同的key时需要抛出异常...
            icombineKPSetting.AddCombineKPSetting(itcnd);
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "addUpdate", "resetTableHeight();AddUpdateComplete(\"" + con + "\");HideWait();", true);
    }


    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        if ( HStation.Value == this.CmbStation.InnerDropDownList.SelectedValue.Trim()
            && HLine.Value ==  this.CmbLine.InnerDropDownList.SelectedValue.Trim()
            && HCheckType.Value == this.CmbCheckType.InnerDropDownList.SelectedValue.Trim()
            && HFamily.Value == this.TxtFamily.Text.Trim()
            && HModel.Value == this.txtModel.Text.Trim())
        {
            return;
        }
        StationCheckInfo itcnd = new StationCheckInfo();
        StationCheckInfo cond = new StationCheckInfo();
        cond.id = Convert.ToInt32(this.hidhidcol.Value.Trim());
        itcnd.line = this.CmbLine.InnerDropDownList.SelectedValue.Trim();
        itcnd.station = this.CmbStation.InnerDropDownList.SelectedValue.Trim();
        itcnd.family = this.TxtFamily.Text.Trim();
        itcnd.model = this.txtModel.Text.Trim();
        itcnd.checkItemType = this.CmbCheckType.InnerDropDownList.SelectedValue.Trim();
        itcnd.customer = "HP";
        itcnd.editor = this.HiddenUserName.Value.Trim();
        itcnd.udt = DateTime.Now;

        try
        {
            //调用添加的方法 相同的key时需要抛出异常...
            icombineKPSetting.UpdateCombineKPSetting(itcnd, cond);
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();SaveUpdateComplete('" + this.hidhidcol.Value + "');HideWait();", true);
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
        this.lblSetting.Text = this.GetLocalResourceObject(Pre + "_lblSettingList").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
        this.lblLine.Text = this.GetLocalResourceObject(Pre + "_lblLine").ToString();
        this.lblStation.Text = this.GetLocalResourceObject(Pre + "_lblStation").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.lblCheckType.Text = this.GetLocalResourceObject(Pre + "_lblCheckType").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
    }

    private void bindTable(IList<StationCheckInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colLine").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colModel").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCheckType").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add("hideCol");


        if (list != null && list.Count != 0)
        {
            foreach (StationCheckInfo temp in list)
            {
                dr = dt.NewRow();

                //dr[0] = temp.station;
                //dr[1] = temp.line;
                //dr[2] = temp.checkItemType;
                //dr[3] = temp.editor;
                //dr[4] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                //dr[5] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                //dr[6] = temp.id.ToString();
                dr[0] = temp.station;
                dr[1] = temp.line;
                dr[2] = temp.family;
                dr[3] = temp.model;
                dr[4] = temp.checkItemType;
                dr[5] = temp.editor;
                dr[6] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[7] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[8] = temp.id.ToString();
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
        //gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[1].Width = Unit.Percentage(5);
        //gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(5);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(24);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(13);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(12);
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        return errorMsg;
    }

    private Boolean showList()
    {
        IList<StationCheckInfo> dataLst = new List<StationCheckInfo>();
        try
        {
            dataLst = icombineKPSetting.GetAllCombineKPSettingItems();
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
