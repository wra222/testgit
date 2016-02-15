using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;

public partial class PCAOQCCosmeticQuery : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string stationHid = String.Empty;
    public string pcode = String.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        initTableColumnHeader();
        InitLabel();
        if (!Page.IsPostBack)
        {
            this.gridview.DataSource = getNullDataTable(10);
            this.gridview.DataBind();
        }
    }

    private void InitLabel()
    {
        this.btnCancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();
    }

    private DataTable getNullDataTable(int rowCount)
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["LotNo"] = String.Empty;
            newRow["PCBNo"] = String.Empty;
            newRow["Editor"] = String.Empty;
            newRow["Cdt"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("LotNo", Type.GetType("System.String"));
        retTable.Columns.Add("PCBNo", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }


    private void initTableColumnHeader()
    {

        this.gridview.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_colLotNo").ToString();
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_colPCBNo").ToString();
        this.gridview.Columns[2].HeaderText = this.GetLocalResourceObject(Pre + "_colEditor").ToString();
        this.gridview.Columns[3].HeaderText = this.GetLocalResourceObject(Pre + "_colCdt").ToString();

        this.gridview.Columns[0].ItemStyle.Width = Unit.Pixel(150);
        this.gridview.Columns[1].ItemStyle.Width = Unit.Pixel(150);
        this.gridview.Columns[2].ItemStyle.Width = Unit.Pixel(150);
        this.gridview.Columns[3].ItemStyle.Width = Unit.Pixel(250);
    }

    /// <summary>
    /// Êä³ö´íÎóÐÅÏ¢
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String er)
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "ShowMessage('" + er + "');" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "writeToAlertMessageAgent", script, false);
    }

    /// <summary>
    /// Îª±í¸ñÁÐ¼Ótooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {

                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
                if (i == 5)
                {
                    e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace(" ", "&nbsp;");
                }

            }
        }
    }

    protected void hidbtn_ServerClick(Object sender, EventArgs e)
    {
        IPCAOQCCosmetic iPCAOQCCosmetic = ServiceAgent.getInstance().GetObjectByName<IPCAOQCCosmetic>(WebConstant.PCAOQCCosmeticObject);

        ArrayList ret = new ArrayList();

        ret = iPCAOQCCosmetic.Query();

        bindTable((IList<string>)(ret[0]), (IList<string>)ret[1], (IList<string>)ret[2], (IList<string>)ret[3], 10);
    }

    private int bindTable(IList<string> list1, IList<string> list2, IList<string> list3, IList<string> list4, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        int ret = 0;


        initTableColumnHeader();

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colLotNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPCBNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());

        

        if (list1 != null && list1.Count != 0)
        {
            for(int i = 0; i < list1.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = list1[i];
                dr[1] = list2[i];
                dr[2] = list3[i];
                dr[3] = list4[i];

                dt.Rows.Add(dr);
            }
            for (int i = list1.Count; i < 10; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }

        gridview.DataSource = dt;
        gridview.DataBind();
        //setColumnWidth();
        return ret;
    }
}
