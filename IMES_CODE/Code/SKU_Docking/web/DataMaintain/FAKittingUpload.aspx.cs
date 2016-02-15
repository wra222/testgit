using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using log4net;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;
using MaintainControl;
using System.Collections;
using System.Collections.Generic;


public partial class DataMaintain_FAKittingUpload : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private ILightNo iLightNo;
    private IFaKittingUpload iFaKittingUpload;
    private const int COL_NUM = 7;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    public const string FA_STAGE = "FA";
    public const string PAK_STAGE = "PAK";
    public const string FA_LABEL = "FA Label";
    public const string PAK_LABEL = "PAK Label";

    private Boolean isLineLoad=false;
    private Boolean isFamilyLoad=false;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.cmbMaintainLightNoPdLine.InnerDropDownList.Load += new EventHandler(cmbLine_Load);
        this.cmbMaintainFaKittingFamily.InnerDropDownList.Load += new EventHandler(cmbFamily_Load);
        try
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            //pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            //pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            //pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            //pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            //pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();

            iLightNo = (ILightNo)ServiceAgent.getInstance().GetMaintainObjectByName<ILightNo>(WebConstant.MaintainLightNoObject);
            iFaKittingUpload = (IFaKittingUpload)ServiceAgent.getInstance().GetMaintainObjectByName<IFaKittingUpload>(WebConstant.MaintainFaKittingUploadObject);
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
                this.cmbMaintainLightNoPdLine.Stage = FA_STAGE; //!!!
                this.cmbMaintainLightNoPdLine.initMaintainLightNoPdLine();

                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();
     
            }
            //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "InitControl", "initControls();", true);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }


    protected void btnQuery_ServerClick(Object sender, EventArgs e)
    {
        ShowListByLine();
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "QueryComplete", "DealHideWait();", true);       
    }

    private void cmbFamily_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainLightNoPdLine.IsPostBack)
        {
            isFamilyLoad =true;
            CheckAndShowList();
        }
    }

    private void CheckAndShowList()
    {
        if (isLineLoad == true && isFamilyLoad == true)
        {
            ShowListByLine();
        }
    }

    private void cmbLine_Load(object sender, System.EventArgs e)
    {
        if (!this.cmbMaintainLightNoPdLine.IsPostBack)
        {
            isLineLoad =true;
            CheckAndShowList();
        }
    }

    private Boolean ShowListByLine()
    {
        String line = this.cmbMaintainLightNoPdLine.InnerDropDownList.SelectedValue.Trim();
        String family = this.cmbMaintainFaKittingFamily.InnerDropDownList.SelectedValue.Trim();

        try
        {
            DataTable dataList;
            dataList = iFaKittingUpload.GetListForFaFromLine(line,family);
            if (dataList == null || dataList.Rows.Count == 0)
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
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        this.dQueryFamilyValue.Value = family;
        this.dQueryLineValue.Value = line;
        return true;

    }

    private void initLabel()
    {
        //this.lblPrompt.Text = this.GetLocalResourceObject(Pre + "_lblPrompt").ToString();
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblFamily.Text = this.GetLocalResourceObject(Pre + "_lblFamily").ToString();

        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.btnUpload.Value  = this.GetLocalResourceObject(Pre + "_btnUpload").ToString();
        this.btnToExcel.Value = this.GetLocalResourceObject(Pre + "_btnToExcel").ToString();

        this.btnQuery.Value = this.GetLocalResourceObject(Pre + "_btnQuery").ToString();


    }
    
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(18);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(18);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(16);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(12);
        //gd.HeaderRow.Cells[7].Width = Unit.Percentage(7);
        //gd.HeaderRow.Cells[8].Width = Unit.Percentage(7);
        //gd.HeaderRow.Cells[9].Width = Unit.Percentage(7);
        //gd.HeaderRow.Cells[10].Width = Unit.Percentage(10);
        //gd.HeaderRow.Cells[11].Width = Unit.Percentage(10);

    }

    private void bindTable(DataTable list, int defaultRow)
    {

        //select  Code, PartNo, Tp, Station, convert(int,LightNo), Qty, Sub, Safety_Stock, Max_Stock, Remark from WipBuffer(nolock) where Code=@pdline and KittingType=’PAK Kitting’

        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDescr").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemStation").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemLightNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemSub").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemSafeStock").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemMaxStock").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemRemark").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        //dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        //select PartNo, Tp, Ln, Qty, Sub, Safety_Stock, Max_Stock from #rm order by Ln

        dt.Columns.Add("ID");

        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < list.Rows[i].ItemArray.Length; j++)
                {

                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                       
                    }
                    else
                    {
                        dr[j] = Null2String(list.Rows[i][j]);
                    }

                }
                dt.Rows.Add(dr);
            }

            for (int i = list.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Rows.Count.ToString();
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
        if (dTableHeight.Value!="") gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();", true);

    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("\r\n", "<br>");
        sourceData = sourceData.Replace("\n", "<br>");
        sourceData = sourceData.Replace(@"\", @"\\");
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }


    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
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

}
