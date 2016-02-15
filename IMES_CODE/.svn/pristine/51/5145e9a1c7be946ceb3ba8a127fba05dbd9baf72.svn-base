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


public partial class DataMaintain_ChepPallet : IMESBasePage
{

    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 33;
    private IChepPallet iChepPallet;
    private const int COL_NUM = 4;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iChepPallet = (IChepPallet)ServiceAgent.getInstance().GetMaintainObjectByName<IChepPallet>(WebConstant.MaintainChepPalletObjet);

            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString() + "!";
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString() + "!";
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString() + "!";
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString() + "!";
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString() + "!";

                userName = Master.userInfo.UserId; 
                initLabel();
                ShowChepPalletInfo();
            }
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

    //显示列表的所有信息
    private Boolean ShowChepPalletInfo()
    {
        try
        {
            IList<ChepPalletDef> dataList = iChepPallet.GetChepPalletList();
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
        this.lbChepPalletList.Text = this.GetLocalResourceObject(Pre + "_lbChepPalletList").ToString() + ":";
        this.lbChepPalletNo.Text = this.GetLocalResourceObject(Pre + "_lbChepPalletNo").ToString() + ":";


        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();

    }

    //设置表格的宽度
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(40);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(26);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(17);

    }

    //添加处理
    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        ChepPalletDef item = new ChepPalletDef();

        string chepPallletNo = this.chepPalletNo.Text.Trim().ToUpper();

        item.palletNo = chepPallletNo;
        item.editor = this.HiddenUserName.Value;

        try
        {
            iChepPallet.AddChepPallet(item);
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
        ShowChepPalletInfo();
        chepPallletNo = replaceSpecialChart(chepPallletNo);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + chepPallletNo + "');DealHideWait();", true);
    }

    //删除处理
    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        string itemId = this.itemId.Value.Trim();
        try
        {
            iChepPallet.DeleteChepPallet(itemId);
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
        ShowChepPalletInfo();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();DealHideWait();", true);

    }

    //给GridView控件，添加数据，没有数据则添加null
    private void bindTable(IList<ChepPalletDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_chepPalletNo").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_editor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_cdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_udt").ToString());
        dt.Columns.Add("itemId");

        if (list != null && list.Count != 0)
        {
            foreach (ChepPalletDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.palletNo;
                dr[1] = temp.editor;
                dr[2] = temp.cdt;
                dr[3] = temp.udt;
                dr[4] = temp.id.ToString();
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
