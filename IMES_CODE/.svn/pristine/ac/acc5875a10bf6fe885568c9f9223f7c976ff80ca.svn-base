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
using IMES.DataModel;
using System.Text;

public partial class DefectComponentRejudge_Query : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    public string[] GvQueryColumnName = { "PartSn","PartType","ActionName","Customer","Model",
                                            "Family","SourcePrdId","DefectDescr","ReturnLine","Status",
                                            "Editor","Cdt"};
    public int[] GvQueryColumnNameWidth = { 95,50,90,50,70,
                                            70, 70,130,60,130,
                                            50,130};
    private const int COL_NUM = 10;
    public string stationHid = String.Empty;
    public string pcode = String.Empty;
    private const int DEFAULT_ROWS = 5;
    IDefectComponentRejudge iDefectComponentRejudge = ServiceAgent.getInstance().GetObjectByName<IDefectComponentRejudge>(WebConstant.DefectComponentRejudgeObject);
    protected void Page_Load(object sender, EventArgs e)
    {
        //initTableColumnHeader();
        bindTable(null);
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

    protected void btnQuery_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string state = this.txtPartSN.Value.Trim();
            DataTable ret = iDefectComponentRejudge.GetQuery(state,this.hidcustsn.Value);
            bindTable(ret);
            //this.updatePanel1.Update();
            ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "Query", "", true);
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
        finally
        {
            //endWaitingCoverDiv();
        }
    }
    
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PartSn", Type.GetType("System.String"));
        retTable.Columns.Add("PartType", Type.GetType("System.String"));
        retTable.Columns.Add("ActionName", Type.GetType("System.String"));
        retTable.Columns.Add("Customer", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Family", Type.GetType("System.String"));
        retTable.Columns.Add("SourcePrdId", Type.GetType("System.String"));
        retTable.Columns.Add("DefectDescr", Type.GetType("System.String"));
        retTable.Columns.Add("ReturnLine", Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    private void bindTable(DataTable list)
    {
        DataTable dt = initTable();
        DataRow dr = null;
        if (list != null && list.Rows.Count != 0)
        {
            foreach (DataRow temp in list.Rows)
            {
                dr = dt.NewRow();
                dr[0] = temp["PartSn"].ToString().Trim();
                dr[1] = temp["PartType"].ToString().Trim();
                dr[2] = temp["ActionName"].ToString().Trim();
                dr[3] = temp["Customer"].ToString().Trim();
                dr[4] = temp["Model"].ToString().Trim();
                dr[5] = temp["Family"].ToString().Trim();
                dr[6] = temp["SourcePrdId"].ToString().Trim();
                dr[7] = temp["DefectDescr"].ToString().Trim();
                dr[8] = temp["ReturnLine"].ToString().Trim();
                dr[9] = temp["Status"].ToString().Trim();
                dr[10] = temp["Editor"].ToString().Trim();
                DateTime time = Convert.ToDateTime(temp["Cdt"]);
                dr[11] = time.ToString("yyyy-MM-dd-HH:mm:ss");
                dt.Rows.Add(dr);
            }

            for (int i = dt.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getNullDataTable(DEFAULT_ROWS);
        }
        gridview.DataSource = dt;
        gridview.DataBind();
        InitGridView();
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; k < COL_NUM; k++)
            {
                newRow[k] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Length; i++)
        {
            gridview.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
    }
   
    private void showErrorMessage(string errorMsg)
    {
        bindTable(null);
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
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
