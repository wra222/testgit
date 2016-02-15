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


public partial class DataMaintain_PalletQuantity : IMESBasePage
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 35;
    private IPalletQty iPalletQty;
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
    public string pmtMessage11;
    public string pmtMessage12;
    public string pmtMessage13;
    public string pmtMessage14;
    public string pmtMessage15;
    public string pmtMessage16;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iPalletQty = (IPalletQty)ServiceAgent.getInstance().GetMaintainObjectByName<IPalletQty>(WebConstant.MaintainPalletQtyOject);

            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString() + "!";
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString() + "!";
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString() + "!";
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString() + "!";
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString() + "!";
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString() + "!";
                pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString() + "!";
                pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString() + "!";
                pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString() + "!";
                pmtMessage11 = this.GetLocalResourceObject(Pre + "_pmtMessage11").ToString() + "!";
                pmtMessage12 = this.GetLocalResourceObject(Pre + "_pmtMessage12").ToString() + "!";
                pmtMessage13 = this.GetLocalResourceObject(Pre + "_pmtMessage13").ToString() + "!";
                pmtMessage14 = this.GetLocalResourceObject(Pre + "_pmtMessage14").ToString() + "!";
                pmtMessage15 = this.GetLocalResourceObject(Pre + "_pmtMessage15").ToString() + "!";
                pmtMessage16 = this.GetLocalResourceObject(Pre + "_pmtMessage16").ToString();
                userName = Master.userInfo.UserId; //UserInfo.UserName;
                this.hiddenUserName.Value = userName;
                initLabel();
                ShowPalletQtyInfo();
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

    //查找并显示PalletQty信息
    private Boolean ShowPalletQtyInfo()
    {
        try
        {
            IList<PalletQtyDef> dataList = iPalletQty.GetQtyInfoList();
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

    //初始化lable项
    private void initLabel()
    {
        this.palletQtyList.Text = this.GetLocalResourceObject(Pre + "_lbPalletQtyList").ToString() + " :";
        this.lbFullQty.Text = this.GetLocalResourceObject(Pre + "_lbFullQty").ToString() + " :";
        this.lbLitterQty.Text = this.GetLocalResourceObject(Pre + "_lbLitterQty").ToString() + " :";
        this.lbMediumQty.Text = this.GetLocalResourceObject(Pre + "_lbMediumQty").ToString() + " :";
        this.lbTireQty.Text = this.GetLocalResourceObject(Pre + "_lbTireQty").ToString() + " :";


        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.InnerText = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    //设置Gridview的列宽度
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(12);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(18);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(17);

    }

    //添加操作
    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        string fullQty;
        try
        {
            PalletQtyDef qtyInfo = new PalletQtyDef();
            qtyInfo.fullQty = this.fullQty.Text.Trim();
            qtyInfo.tireQty = this.tireQty.Text.Trim();
            qtyInfo.mediumQty = this.mediumQty.Text.Trim();
            qtyInfo.litterQty = this.litterQty.Text.Trim();
            qtyInfo.editor = this.hiddenUserName.Value;
            fullQty = iPalletQty.AddQtyInfo(qtyInfo);
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
        ShowPalletQtyInfo();
        fullQty = replaceSpecialChart(fullQty);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + fullQty + "');DealHideWait();", true);
    }

    //更新操作
    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {

        string itemId = this.itemId.Value.Trim();
        string fullQty = this.fullQty.Text.Trim();
        try
        {
            PalletQtyDef qtyInfo = new PalletQtyDef();
            qtyInfo.fullQty = this.fullQty.Text.Trim();
            qtyInfo.tireQty = this.tireQty.Text.Trim();
            qtyInfo.mediumQty = this.mediumQty.Text.Trim();
            qtyInfo.litterQty = this.litterQty.Text.Trim();
            qtyInfo.editor = this.hiddenUserName.Value;
            iPalletQty.UpdateQtyInfo(qtyInfo, itemId);
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
        ShowPalletQtyInfo();
        fullQty = replaceSpecialChart(fullQty);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + fullQty + "');DealHideWait();", true);
    }

    //删除操组
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        string itemId = this.itemId.Value.Trim();
        try
        {
            //执行删除
            iPalletQty.DeleteQtyInfo(itemId);
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
        ShowPalletQtyInfo();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();DealHideWait();", true);

    }

    //给GridView控件添加数据，没有数据则添加null
    private void bindTable(IList<PalletQtyDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_fullQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tireQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_mediumQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_litterQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_editor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_cdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_udt").ToString());
        dt.Columns.Add("id");

        if (list != null && list.Count != 0)
        {
            foreach (PalletQtyDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.fullQty;
                dr[1] = temp.tireQty;
                dr[2] = temp.mediumQty;
                dr[3] = temp.litterQty;
                dr[4] = temp.editor;
                dr[5] = temp.cdt;
                dr[6] = temp.udt;
                dr[7] = temp.id.ToString();
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
        gd.GvExtHeight = dTableHeight.Value;
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
