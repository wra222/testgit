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
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;


public partial class DataMaintain_ICASA : IMESBasePage
{

    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 33;
    private IICASA iICASA;
    private const int COL_NUM = 8;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iICASA = (IICASA)ServiceAgent.getInstance().GetMaintainObjectByName<IICASA>(WebConstant.ICASAObjet);

            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
                pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();

                userName = Master.userInfo.UserId;
                initLabel();
                ShowICASAInfo();
            }
            //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "", "", true);

        }
        catch (FisException ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            showErrorMessage(ex.Message);
        }
    }

    private Boolean ShowICASAInfo()
    {
        try
        {
            IList<ICASAInfo> dataList = iICASA.GetICASAList();
            if (dataList == null || dataList.Count == 0)
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
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);

            return false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            //show error
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }

        return true;

    }


    private void ShowMessage(string p)
    {
        throw new NotImplementedException(p);
    }

    //初始化所有Lable框
    private void initLabel()
    {
        this.lbICASAList.Text = this.GetLocalResourceObject(Pre + "_lbICASAList").ToString();
        this.lbVC.Text = this.GetLocalResourceObject(Pre + "_lbVC").ToString();
        this.lbAV.Text = this.GetLocalResourceObject(Pre + "_lbAV").ToString();
        this.lbAntel1.Text = this.GetLocalResourceObject(Pre + "_lbAntel1").ToString();
        this.lbAntel2.Text = this.GetLocalResourceObject(Pre + "_lbAntel2").ToString();
        this.lbIcasa.Text = this.GetLocalResourceObject(Pre + "_lbICASA").ToString();

        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }

    //设置表格的宽度
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(7);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(11);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(12);

    }

    //添加处理
    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        ICASAInfo item = new ICASAInfo();

        string id;

        item.vc = this.vc.Text.Trim().ToUpper(); ;
        item.av = this.av.Text.Trim();
        //issueCode 
        //ITC-1361-0014  itc210012  2012-02-15
        item.anatel1 = this.antel1.Text.Trim();
        item.anatel2 = this.antel2.Text.Trim();
        item.icasa = this.icasa.Text.Trim();
        item.edit = this.HiddenUserName.Value;

        try
        {
            id=iICASA.AddICASAInfo(item);
        }
        catch (FisException ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        //读取并显示列表信息
        ShowICASAInfo();
        id = replaceSpecialChart(id);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');DealHideWait();", true);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        string itemId = this.itemId.Value.Trim();
        ICASAInfo item = new ICASAInfo();
        item.vc = this.vc.Text.Trim().ToUpper();
        //ITC-1361-0103 ITC210012 2012-3-6
        item.av = this.av.Text.Trim();
        item.anatel1 = this.antel1.Text.Trim();
        item.anatel2 = this.antel2.Text.Trim();
        item.icasa = this.icasa.Text.Trim();
        item.edit = this.HiddenUserName.Value;

        try
        {
            iICASA.UpdateICASAInfo(item, itemId);
        }
        catch (FisException ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        //读取并显示列表信息
        ShowICASAInfo();
        itemId = replaceSpecialChart(itemId);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + itemId + "');DealHideWait();", true);
    }

    //删除处理
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string itemId = this.itemId.Value.Trim();
        try
        {
            iICASA.DeleteICASAInfo(itemId);
        }
        catch (FisException ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "DealHideWait", "DealHideWait();", true);
            //show error
            showErrorMessage(ex.Message);
            return;
        }
        ShowICASAInfo();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();DealHideWait();", true);

    }

    //给GridView控件，添加数据，没有数据则添加null
    private void bindTable(IList<ICASAInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_vc").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_av").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_antel1").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_antel2").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_icasa").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_editor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_cdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_udt").ToString());
        dt.Columns.Add("itemId");

        if (list != null && list.Count != 0)
        {
            foreach (ICASAInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.vc;
                dr[1] = temp.av;
                dr[2] = temp.anatel1;
                dr[3] = temp.anatel2;
                dr[4] = temp.icasa;
                dr[5] = temp.edit;
                dr[6] = temp.cdt;
                dr[7] = temp.udt;
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

        gd.DataSource = dt;
        gd.DataBind();
        gd.GvExtHeight = this.dTableHeight.Value;
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);
    }

    //显示错误信息
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }

    //设置GridView的列信息
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
