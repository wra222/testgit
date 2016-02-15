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

public partial class _ITCNDCheckQuery : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string stationHid = String.Empty;
    public string pcode = String.Empty;
    IPrintTemplate iPrintTemplate = ServiceAgent.getInstance().GetObjectByName<IPrintTemplate>(WebConstant.CommonObject);

    ITestStation iTestStation = ServiceAgent.getInstance().GetObjectByName<ITestStation>(WebConstant.CommonObject);
    public string portNumber = WebCommonMethod.getConfiguration("PortNumber");

    protected void Page_Load(object sender, EventArgs e)
    {
        initTableColumnHeader();
        if (!Page.IsPostBack)
        {
            stationHid = Request["Station"];
            pcode = Request["PCode"];

            this.stationHidden.Value = stationHid;
            this.pCodeHidden.Value = pcode;
            this.gridview.DataSource = getNullDataTable(10);
            this.gridview.DataBind();
      
        }
    }
    
    private DataTable getNullDataTable(int rowCount)
    {
        DataTable dt = initTable();
        DataRow newRow;
        for (int i = 0; i < rowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["Model"] = String.Empty;
            newRow["Pass Qty"] = String.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Pass Qty", Type.GetType("System.String"));
        return retTable;
    }


    private void initTableColumnHeader()
    {

        this.gridview.Columns[0].HeaderText = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.gridview.Columns[1].HeaderText = this.GetLocalResourceObject(Pre + "_lblPassQty").ToString();
        this.gridview.Columns[0].ItemStyle.Width = Unit.Pixel(100);
        this.gridview.Columns[1].ItemStyle.Width = Unit.Pixel(20);
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



    /// <summary>
    ///¸ßÁÁNewMO ListµÄµÚÒ»ÐÐ
    /// </summary>  
    private void highLightNewMOList()
    {
        String script = "<script language='javascript'>" + "\r\n" +
              "HighLightNewMOList();" + "\r\n" +
              "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "HighLightNewMOList", script, false);
    }
}
