using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
//using IMES.DataModel;

public partial class Query_MBRepair : IMESQueryBasePage
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
    ISA_PCBStationQuery intfPCBStationQuery = ServiceAgent.getInstance().GetObjectByName<ISA_PCBStationQuery>(WebConstant.ISA_PCBStationQuery);
    IConfigDB intfConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    ISA_SAMBRepair intfSAMBRepair = ServiceAgent.getInstance().GetObjectByName<ISA_SAMBRepair>(WebConstant.ISA_SAMBRepair);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    String DBConnection = "";
    public string StationList;

    public string[] GvDetailColumnName = { "PCBNo", "PCBModelID", "Line", "Station", "Family", "Model", "Status", "Location", "Descr", "Cause", "Obligation", "Remark", "Editor", "Cdt", "Udt" };
    public int[] GvDetailColumnNameWidth = { 100, 100, 70, 150, 150, 55, 55, 70, 220, 180, 180, 250, 70, 180, 180 };

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        StationList = PCAInputStation;
        if (!IsPostBack) {
            InitPage();
            InitCondition();
            txtStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        DropDownList ddlDB = ((DropDownList)CmbDBType.FindControl("ddlDB"));
        ddlDB.SelectedIndexChanged += new EventHandler(this.DB_SelectChange);
        ddlDB.AutoPostBack = true;     
    }

    protected void InitPage() {
            
    }

    private void InitCondition()
    {
        InitFamily();
        InitStation();
    }

    protected void InitFamily()
    {
        DataTable dtFamily = intfFamily.GetPCBFamily(DBConnection);
        ddlFamily.Items.Clear();
        ddlFamily.Items.Add(new ListItem("-", ""));
        if (dtFamily.Rows.Count > 0)
        {
            for (int i = 0; i < dtFamily.Rows.Count; i++)
            {
                ddlFamily.Items.Add(new ListItem(dtFamily.Rows[i]["Family"].ToString(), dtFamily.Rows[i]["Family"].ToString()));
            }
        }
    }

    protected void InitStation()
    {
        List<string> station = new List<string>();
        station.AddRange(StationList.Split(','));
        DataTable dtStation = QueryCommon.GetStationName(station, DBConnection);
        if (dtStation.Rows.Count > 0)
        {
            /*ddlStation.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtStation.Rows)
            {
                ddlStation.Items.Add(new ListItem(dr["Station"].ToString().Trim() + " - " + dr["Name"].ToString().Trim(), dr["Station"].ToString().Trim()));
            }*/

            lboxStation.Items.Clear();
            for (int i = 0; i < dtStation.Rows.Count; i++)
            {
                lboxStation.Items.Add(new ListItem(dtStation.Rows[i]["Station"].ToString() + " - " + dtStation.Rows[i]["Name"].ToString(),
                                                dtStation.Rows[i]["Station"].ToString()));

            }
        }
    }

    protected void InitModel()
    {

        List<string> lstFamily = new List<string>();
        lstFamily.Add(ddlFamily.SelectedValue);

        DataTable Model = intfPCBStationQuery.GetModel(DBConnection, lstFamily);

        lboxModel.Items.Clear();
        for (int i = 0; i < Model.Rows.Count; i++)
        {
            lboxModel.Items.Add(new ListItem(Model.Rows[i]["InfoValue"].ToString(), Model.Rows[i]["InfoValue"].ToString()));
        }
    }

    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        queryClick(sender, e);
    }
    
    public void queryClick(object sender, System.EventArgs e)
    {        
        try
        {
            DataTable dt = new DataTable();
            dt = getDataTable();            
            //gvQuery.DataSource = dt;
            this.gvQuery.DataSource = getNullDataTable(dt.Rows.Count);
            this.gvQuery.DataBind();
            
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int n = 0; n < GvDetailColumnName.Length; n++)
                {
                    string text = (null == dt.Rows[i][GvDetailColumnName[n]]) ? "" : dt.Rows[i][GvDetailColumnName[n]].ToString();
                    gvQuery.Rows[i].Cells[n].Text = text;
                }
                gvQuery.Rows[i].Cells[6].Text = dt.Rows[i]["Status"].ToString() == "1" ? "OK" : "NG";
            }
            //gvQuery.DataBind();
            InitGridView();
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.ToString());
        }
        finally
        {
            endWaitingCoverDiv();
        }
    }

    private DataTable getDataTable()
    {
        DataTable dt = new DataTable();

        DateTime timeStart = DateTime.Parse(txtStartTime.Text);
        DateTime timeEnd = DateTime.Parse(txtEndTime.Text);

        List<string> lstModel = new List<string>();
        for (int i = 0; i < lboxModel.Items.Count; i++)
        {
            if (lboxModel.Items[i].Selected)
            {
                lstModel.Add(lboxModel.Items[i].Value);
            }
        }

        List<string> lstStation = new List<string>();
        for (int i = 0; i < lboxStation.Items.Count; i++)
        {
            if (lboxStation.Items[i].Selected)
            {
                lstStation.Add(lboxStation.Items[i].Value);
            }
        }

        dt = intfSAMBRepair.GetMBRepairInfo(DBConnection, timeStart, timeEnd, ddlFamily.SelectedValue, lstModel, lstStation);
            

        return dt;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvQuery, "SAMBRepair" , Page);

    }
    private void DB_SelectChange(object sender, EventArgs e)
    {
           
    }


    protected void gvQuery_PreRender(object sender, EventArgs e)
    {
        gvQuery.UseAccessibleHeader = true;
        if (gvQuery.Rows.Count > 0) {
            gvQuery.HeaderRow.TableSection = TableRowSection.TableHeader;            
        }                
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();

            for (int n = 0; n < GvDetailColumnName.Length; n++)
            {
                newRow[GvDetailColumnName[n]] = "";
            }

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();

        for (int n = 0; n < GvDetailColumnName.Length; n++)
        {
            retTable.Columns.Add(GvDetailColumnName[n], Type.GetType("System.String"));
        }
        return retTable;
    }

    private void InitGridView()
    {
        for (int n = 0; n < GvDetailColumnName.Length; n++)
        {
            gvQuery.HeaderRow.Cells[n].Width = Unit.Pixel(GvDetailColumnNameWidth[n]);
        }
    }
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        //scriptBuilder.AppendLine("endWaitingCoverDiv();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitModel();
    }

}
