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

public partial class DataMaintain_FAIInfoMaintain : System.Web.UI.Page
{

    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IFAIInfoMaintain iFAIInfoMaintain;
    
    private const int COL_NUM = 20;

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
        iFAIInfoMaintain = (IFAIInfoMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<IFAIInfoMaintain>(WebConstant.IFAIINFOMAINTAIN);
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
        if (!this.IsPostBack)
        {
            this.TextBox1.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
            this.TextBox2.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
            this.TextBox4.Attributes.Add("onkeydown", "onTextBox1KeyDown()");
            initLabel();
            IList<FaiInfo> datalst = null;
            try
            {
                datalst = iFAIInfoMaintain.GetAllFAIInfoMaintainItems();
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

  

    protected void Add()
    {
        FaiInfo itcnd = new FaiInfo();
        try
        {
            itcnd.fin_time = DateTime.Parse(this.TextBox4.Text);
            itcnd.rec_time = DateTime.Parse(this.TextBox13.Text);
        }
        catch (System.Exception ex)
        {
            showErrorMessage("Finish Time or Receipt Time ,"+ ex.Message);
            showList();
            this.updatePanel2.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();SaveUpdateComplete('" + this.TextBox1.Text + "');HideWait();", true);
            return;
        }
        itcnd.iecpn = this.TextBox1.Text;
        itcnd.sno =  this.TextBox2.Text;
        itcnd.hpqpn = this.TextBox3.Text;
        itcnd.fin_time = DateTime.Parse(this.TextBox4.Text);
        itcnd.bios_typ = this.TextBox5.Text;
        itcnd.kbc_ver = this.TextBox6.Text;
        itcnd.vdo_bios = this.TextBox7.Text;
        itcnd.fdd_sup = this.TextBox8.Text;
        itcnd.hdd_sup = this.TextBox9.Text;
        itcnd.opt_sup = this.TextBox10.Text;
        itcnd.ng_record = this.TextBox11.Text;
        itcnd.imp_record = this.TextBox12.Text;
        itcnd.rec_time = DateTime.Parse(this.TextBox13.Text);
        itcnd.chk_stat = this.CmbCHKState.Value;
        itcnd.upc_code = this.TextBox15.Text;
        itcnd.ram_typ = this.TextBox16.Text;
        itcnd.bat_typ = this.TextBox17.Text;
        itcnd.editor = userName.Trim();
        itcnd.cdt = DateTime.Now;
        itcnd.udt = DateTime.Now;
        
        try
        {
            //调用添加的方法 相同的key时需要抛出异常...
            iFAIInfoMaintain.AddFAIInfoMaintain(itcnd);
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
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "addUpdate", "resetTableHeight();AddUpdateComplete(\"" + this.TextBox1.Text + "\");HideWait();", true);
    }
    protected void btnQuery_ServerClick(object sender, EventArgs e)
    {
        IList<FaiInfo> datalst = null;
        try
        {
            if (this.TextBox1.Text == "" && this.TextBox2.Text == "" && this.TextBox3.Text == "" && this.TextBox4.Text == "")
            {
            }
            else
            {
                DateTime.Parse(this.TextBox4.Text);
            }
        }
        catch (Exception ee)
        {
            showErrorMessage("Finish Time  ," + ee.Message);
            showList();
            this.updatePanel2.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();QueryUpdateComplete('" + this.TextBox1.Text + "');HideWait();", true);
            return;
        }
        try
        {
            if (this.TextBox1.Text == "" && this.TextBox2.Text == "" && this.TextBox3.Text == "" && this.TextBox4.Text == "")
            {
                datalst = iFAIInfoMaintain.GetAllFAIInfoMaintainItems();
                HQuery.Value = "";
            }
            else
            {
                datalst = iFAIInfoMaintain.GetFAIInfoMaintainItems(DateTime.Parse(this.TextBox4.Text), this.TextBox1.Text, this.TextBox3.Text, this.TextBox2.Text);
                if (null != datalst && datalst.Count > 0)
                {
                    HQuery.Value = "1";
                    Hidden1.Value = this.TextBox4.Text;
                    Hidden2.Value = this.TextBox1.Text;
                    Hidden3.Value = this.TextBox3.Text;
                    Hidden4.Value = this.TextBox2.Text;
                }
            }
        }
        catch (FisException fe)
        {
            showErrorMessage(fe.mErrmsg);
        }
        catch (Exception ee)
        {
            showErrorMessage(ee.Message);
        }

        if (datalst == null || datalst.Count == 0)
        {
            bindTable(null, DEFAULT_ROWS);
        }
        else
        {
            bindTable(datalst, DEFAULT_ROWS);
        }
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();QueryUpdateComplete('" + this.TextBox1.Text + "');HideWait();", true);
    }

    protected void btnSave_ServerClick(object sender, EventArgs e)
    {

        IList<FaiInfo> dataLst = new List<FaiInfo>();
        dataLst = iFAIInfoMaintain.GetAllFAIInfoMaintainItems();
        HOperation.Value = "add";
        if (dataLst != null && dataLst.Count != 0)
        {
            foreach (FaiInfo temp in dataLst)
            {
                if (temp.iecpn == this.TextBox1.Text)
                {
                    HOperation.Value = "save";
                    break;
                }
            }
        }
        if (HOperation.Value == "save")
        {
            FaiInfo itcnd = new FaiInfo();
            itcnd.sno = this.TextBox2.Text;
            itcnd.hpqpn = this.TextBox3.Text;
            try
            {
                itcnd.fin_time = DateTime.Parse(this.TextBox4.Text);
                itcnd.rec_time = DateTime.Parse(this.TextBox13.Text);
            }
            catch (System.Exception ex)
            {
                showErrorMessage("Finish Time or Receipt Time ," + ex.Message);
                showList();
                this.updatePanel2.Update();
                ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();SaveUpdateComplete('" + this.TextBox1.Text + "');HideWait();", true);
                return;
            }
            itcnd.bios_typ = this.TextBox5.Text;
            itcnd.kbc_ver = this.TextBox6.Text;
            itcnd.vdo_bios = this.TextBox7.Text;
            itcnd.fdd_sup = this.TextBox8.Text;
            itcnd.hdd_sup = this.TextBox9.Text;
            itcnd.opt_sup = this.TextBox10.Text;
            itcnd.ng_record = this.TextBox11.Text;
            itcnd.imp_record = this.TextBox12.Text;
            itcnd.rec_time = DateTime.Parse(this.TextBox13.Text);
            itcnd.chk_stat = this.CmbCHKState.Value;
            itcnd.upc_code = this.TextBox15.Text;
            itcnd.ram_typ = this.TextBox16.Text;
            itcnd.bat_typ = this.TextBox17.Text;
            itcnd.udt = DateTime.Now;
            FaiInfo cond = new FaiInfo();
            cond.iecpn = this.TextBox1.Text;
            try
            {
                //调用添加的方法 相同的key时需要抛出异常...
                iFAIInfoMaintain.UpdateFAIInfoMaintain(itcnd, cond);
            }
            catch (FisException fex)
            {
                showErrorMessage(fex.mErrmsg);

            }
            catch (System.Exception ex)
            {
                showErrorMessage(ex.Message);

            }
            showList();
            this.updatePanel2.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();SaveUpdateComplete('" + this.TextBox1.Text + "');HideWait();", true);
        }
        else
        {
            Add();
        }
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
        this.lblSetting.Text = this.GetLocalResourceObject(Pre + "_lblSettingList").ToString();
        this.Label1.Text = this.GetLocalResourceObject(Pre + "_Label1").ToString();
        this.Label2.Text = this.GetLocalResourceObject(Pre + "_Label2").ToString();
        this.Label3.Text = this.GetLocalResourceObject(Pre + "_Label3").ToString();
        this.Label4.Text = this.GetLocalResourceObject(Pre + "_Label4").ToString();
        this.Label5.Text = this.GetLocalResourceObject(Pre + "_Label5").ToString();
        this.Label6.Text = this.GetLocalResourceObject(Pre + "_Label6").ToString();
        this.Label7.Text = this.GetLocalResourceObject(Pre + "_Label7").ToString();
        this.Label8.Text = this.GetLocalResourceObject(Pre + "_Label8").ToString();
        this.Label9.Text = this.GetLocalResourceObject(Pre + "_Label9").ToString();
        this.Label10.Text = this.GetLocalResourceObject(Pre + "_Label10").ToString();
        this.Label11.Text = this.GetLocalResourceObject(Pre + "_Label11").ToString();
        this.Label12.Text = this.GetLocalResourceObject(Pre + "_Label12").ToString();
        this.Label13.Text = this.GetLocalResourceObject(Pre + "_Label13").ToString();
        this.Label14.Text = this.GetLocalResourceObject(Pre + "_Label14").ToString();
        this.Label15.Text = this.GetLocalResourceObject(Pre + "_Label15").ToString();
        this.Label16.Text = this.GetLocalResourceObject(Pre + "_Label16").ToString();
        this.Label17.Text = this.GetLocalResourceObject(Pre + "_Label17").ToString();
        this.Button1.Value = this.GetLocalResourceObject(Pre + "_Button1").ToString();
        this.Button2.Value = this.GetLocalResourceObject(Pre + "_Button2").ToString();
        this.Button3.Value = this.GetLocalResourceObject(Pre + "_Button3").ToString();
        this.Button4.Value = this.GetLocalResourceObject(Pre + "_Button4").ToString();
    }

    private void bindTable(IList<FaiInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col1").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col2").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col3").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col4").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col5").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col6").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col7").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col8").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col9").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col10").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col11").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col12").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col13").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col14").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col15").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col16").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col17").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col18").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col19").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_col20").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (FaiInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.iecpn;
                dr[1] = temp.sno;
                dr[2] = temp.hpqpn;
                dr[3] = temp.fin_time.ToString("yyyy-MM-dd"); 
                dr[4] = temp.bios_typ;
                dr[5] = temp.kbc_ver;
                dr[6] = temp.vdo_bios;
                dr[7] = temp.fdd_sup;
                dr[8] = temp.hdd_sup;
                dr[9] = temp.opt_sup;
                dr[10] = temp.ng_record;
                dr[11] = temp.imp_record;
                dr[12] = temp.rec_time.ToString("yyyy-MM-dd");
                dr[13] = temp.chk_stat;
                dr[14] = temp.upc_code;
                dr[15] = temp.ram_typ;
                dr[16] = temp.bat_typ;
                dr[17] = temp.editor;
                dr[18] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[19] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
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
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[3].Width = Unit.Pixel(180); 
        gd.HeaderRow.Cells[4].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[5].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[6].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[7].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[8].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[9].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[10].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[11].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[12].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[13].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[14].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[15].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[16].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[17].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[18].Width = Unit.Pixel(180);
        gd.HeaderRow.Cells[19].Width = Unit.Pixel(180);
        
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
        IList<FaiInfo> dataLst = new List<FaiInfo>();
        try
        {
            if (HQuery.Value == "")
            {
                dataLst = iFAIInfoMaintain.GetAllFAIInfoMaintainItems();
            }
            else
            {
                dataLst = iFAIInfoMaintain.GetFAIInfoMaintainItems(DateTime.Parse(Hidden1.Value), Hidden2.Value, Hidden3.Value, Hidden4.Value);
            }
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
