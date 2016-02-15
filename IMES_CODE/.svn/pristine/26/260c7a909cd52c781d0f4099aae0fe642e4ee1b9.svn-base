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

public partial class RCTO_MBMaintain : System.Web.UI.Page
{

    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IRCTOMBMaintain mbmaintain;
    private const int COL_NUM = 7;

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

    protected void Page_Load(object sender, EventArgs e)
    {
        mbmaintain = (IRCTOMBMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<IRCTOMBMaintain>(WebConstant.MaintainMBObject);

        this.Family1.InnerDropDownList.SelectedIndexChanged += new EventHandler(Family1_Selected);
        this.Family1.InnerDropDownList.AutoPostBack = true;

        this.Family2.InnerDropDownList.SelectedIndexChanged += new EventHandler(Family2_Selected);
        this.Family2.InnerDropDownList.AutoPostBack = true;

        pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
        pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
        pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
        pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
        pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
        //pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
        //pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
        //pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
        //pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
        //need change..
        //pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
        userName = Master.userInfo.UserId;
        this.HiddenUserName.Value = userName;
        if (!this.IsPostBack)
        {
            initLabel();
            IList<IMES.DataModel.RctombmaintainInfo> datalst = null;
            try
            {
                this.Family1.initObject(out datalst);
                bindTable(datalst, DEFAULT_ROWS);

                string family;
                this.Family2.initObject(out family);
                this.CmbMBCode.refresh(family);
            }
            catch (FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
            }
            catch (Exception ee)
            {
                showErrorMessage(ee.Message);
            }
        }
    }

    private void Family1_Selected(object sender, System.EventArgs e)
    {
        try
        {
            if (this.Family1.InnerDropDownList.SelectedValue.Trim() == "")
            {
                bindTable(null, DEFAULT_ROWS);
                this.updatePanel2.Update();

                disableBtn();
                
                if (this.Family2.InnerDropDownList.SelectedValue.Trim() != "")
                {
                    this.CmbMBCode.clearContent();
                    //this.CmbMBCode.setSelected(-1);
                    //this.CmbMBCode.InnerDropDownList.SelectedValue = "";
                    this.CmbMBType.setSelected(-1);
                    this.txtRemark.Text = "";
                    updatePanel1.Update();
                }
                else
                {
                    this.CmbMBCode.clearContent();
                    //this.CmbMBCode.setSelected(-1);
                    //this.CmbMBCode.InnerDropDownList.SelectedValue = "";
                    this.CmbMBType.setSelected(-1);
                    this.txtRemark.Text = "";
                    updatePanel1.Update();
                }
                
            }
            else
            {
                IList<RctombmaintainInfo> list = new List<RctombmaintainInfo>();
                list = mbmaintain.getMBMaintaininfo(this.Family1.InnerDropDownList.SelectedValue);
                //if (list == null || list.Count == 0)
                {
                    disableBtn();
                }

                bindTable(list, DEFAULT_ROWS);
                this.updatePanel2.Update();

                this.CmbMBType.setSelected(-1);
                this.txtRemark.Text = "";
                updatePanel1.Update();

                this.Family2.InnerDropDownList.SelectedValue = this.Family1.InnerDropDownList.SelectedValue.Trim();
                this.CmbMBCode.refresh(this.Family1.InnerDropDownList.SelectedValue.Trim());
            }
            updateFamily2();
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

    private void Family2_Selected(object sender, System.EventArgs e)
    {
        try
        {
            if (this.Family2.InnerDropDownList.SelectedValue == "")
            {
                this.CmbMBCode.clearContent();
                this.CmbMBType.setSelected(-1);
                this.txtRemark.Text = "";
                updatePanel1.Update();
            }
            else
            {
                this.CmbMBCode.refresh(this.Family2.InnerDropDownList.SelectedValue.Trim());
            }
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

    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        try
        {
            RctombmaintainInfo cond = new RctombmaintainInfo();
            //调用删除方法.
            cond.family = this.hidFamily.Value;
            cond.code = this.hidCode.Value;
            cond.type = this.hidType.Value;

            if (cond.family == "" || cond.code == "" || cond.type == "")
            {
                showErrorMessage(pmtMessage5);
            }
            else
            {
                mbmaintain.deleteMBMaintain(cond);
            }
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
        string family = this.Family1.InnerDropDownList.SelectedValue.Trim();
        showList(family);
        this.updatePanel2.Update();
        //this.CmbCheckType.setSelected(0);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        ITCNDCheckSettingDef itcnd = new ITCNDCheckSettingDef();
        RctombmaintainInfo item = new RctombmaintainInfo();

        item.family = this.Family2.InnerDropDownList.SelectedValue.Trim();
        item.code = this.CmbMBCode.InnerDropDownList.SelectedValue.Trim();
        item.type = this.CmbMBType.InnerDropDownList.SelectedValue.Trim();
        item.remark = this.txtRemark.Text.Trim();
        item.editor = this.HiddenUserName.Value.Trim();
        item.cdt = DateTime.Now;
        item.udt = DateTime.Now;

        try
        {
            //调用添加的方法 相同的key时需要抛出异常...
            mbmaintain.addMBMaintain(item);
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
        showList(item.family);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete();HideWait();", true);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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
        this.lblFamily1.Text = this.GetLocalResourceObject(Pre + "_Family1").ToString();
        this.lblSetting.Text = this.GetLocalResourceObject(Pre + "_setting").ToString();
        this.lblFamily2.Text = this.GetLocalResourceObject(Pre + "_Family2").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_Code").ToString();
        this.lblType.Text = this.GetLocalResourceObject(Pre + "_Type").ToString();
        this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_Remark").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
    }

    private void bindTable(IList<RctombmaintainInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colFamily").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRemark").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());


        if (list != null && list.Count != 0)
        {
            foreach (RctombmaintainInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.code;
                dr[1] = temp.type;
                dr[2] = temp.family;
                dr[3] = temp.remark;
                dr[4] = temp.editor;
                dr[5] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[6] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");

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

            this.hidRecordCount.Value = "0";
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
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(10);
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

    private void disableBtn()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("disableBtn()");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "disableBtn", scriptBuilder.ToString(), false);
    }

    private void updateFamily2()
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("updateFamily2()");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "updateFamily2", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        return errorMsg;
    }

    private Boolean showList(string family)
    {
        IList<RctombmaintainInfo> dataLst = new List<RctombmaintainInfo>();
        try
        {
            dataLst = mbmaintain.getMBMaintaininfo(family);
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
