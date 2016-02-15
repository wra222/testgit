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

public partial class DataMaintain_FamilyMBCode : System.Web.UI.Page
{

    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IFamilyMBCode iFamilyMB;
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
    public string pmtMessage10;
    public string pmtMessage11;
    public string pmtMessage12;
    protected void Page_Load(object sender, EventArgs e)
    {
        iFamilyMB = (IFamilyMBCode)ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyMBCode>(WebConstant.IFAMILYMBCODE);
        this.TextBox1.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
        if (!this.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage12 = this.GetLocalResourceObject(Pre + "_pmtMessage12").ToString();
            pmtMessage11 = this.GetLocalResourceObject(Pre + "_pmtMessage11").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            initLabel();
            IList<FamilyMbInfo> datalst = null;
            try
            {
                datalst = iFamilyMB.GetAllFamilyMbItems();
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
            FamilyMbInfo def = new FamilyMbInfo();
            //调用删除方法.
            def.family = HFamily.Value.Trim();
            def.mb = HMBCode.Value.Trim();
            def.remark = HDescr.Value.Trim();
            iFamilyMB.RemoveFamilyMb(def);
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        FamilyMbInfo itcnd = new FamilyMbInfo();
        itcnd.family = this.TxtFamily.Text.Trim();
        itcnd.mb = this.txtMBCode.Text.Trim();
        itcnd.remark = this.txtDescr.Text.Trim();
        itcnd.editor = this.HiddenUserName.Value.Trim();
        itcnd.cdt = DateTime.Now;
        itcnd.udt = DateTime.Now;
        string con = itcnd.family + itcnd.mb;
        con = con.Trim();
        try
        {
            //调用添加的方法 相同的key时需要抛出异常...
            iFamilyMB.AddFamilyMb(itcnd);
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
       
        FamilyMbInfo itcnd = new FamilyMbInfo();
        FamilyMbInfo cond = new FamilyMbInfo();
        cond.family = HFamily.Value.Trim();
        cond.mb = HMBCode.Value.Trim();
        itcnd.family = this.TxtFamily.Text.Trim();
        itcnd.mb = this.txtMBCode.Text.Trim();
        itcnd.remark = this.txtDescr.Text.Trim();
        itcnd.editor = this.HiddenUserName.Value.Trim();
        itcnd.udt = DateTime.Now;
        this.hidhidcol.Value =  this.TxtFamily.Text.Trim() + this.txtMBCode.Text.Trim() ;
        try
        {
            //调用添加的方法 相同的key时需要抛出异常...
            iFamilyMB.UpdateFamilyMb(itcnd, cond);
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
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.lblMBCode.Text = this.GetLocalResourceObject(Pre + "_lblMBCode").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();
        this.lblDescr.Text = this.GetLocalResourceObject(Pre + "_lblDescr").ToString(); 
    }

    private void bindTable(IList<FamilyMbInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_Family").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_MBCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescr").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
        dt.Columns.Add("hideCol");


        if (list != null && list.Count != 0)
        {
            foreach (FamilyMbInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.family;
                dr[1] = temp.mb;
                dr[2] = temp.remark;
                dr[3] = temp.editor;
                dr[4] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[5] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[6] = temp.family.Trim() + temp.mb.Trim();
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
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(5);
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
        IList<FamilyMbInfo> dataLst = new List<FamilyMbInfo>();
        try
        {
            dataLst = iFamilyMB.GetAllFamilyMbItems();
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
