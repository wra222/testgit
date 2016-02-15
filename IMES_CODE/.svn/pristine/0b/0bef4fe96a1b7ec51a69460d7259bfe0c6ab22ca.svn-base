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

public partial class Query_PCBInputQuery : IMESQueryBasePage
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);    
    ISA_PCBStationQuery intfPCBStationQuery = ServiceAgent.getInstance().GetObjectByName<ISA_PCBStationQuery>(WebConstant.ISA_PCBStationQuery);
    ISA_PCBInputQuery intfPCBInputQuery = ServiceAgent.getInstance().GetObjectByName<ISA_PCBInputQuery>(WebConstant.ISA_PCBInputQuery);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    public string StationList;

    public int FixedColCount = 3;    
    String DBConnection = "";

    public string[] GvDetailColumnName = { "No", "PCBNo", "Station", "Status", "Line", "Editor", "Udt" };
    public int[] GvDetailColumnNameWidth = { 50, 80, 80, 80, 80, 120,  150 };

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        StationList = PCAInputStation;
        if (!IsPostBack) {
            InitPage();
            InitCondition();
            txtStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00");
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
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
        InitPdLine();
        InitStation();

        this.gvQuery.DataSource = getNullDataTable(1);
        this.gvQuery.DataBind();
        InitGridView();   
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

    protected void InitPdLine()
    {
        //CmbPdLine.Stage = "SA"; //PCA = SA
        //CmbPdLine.Customer = Master.userInfo.Customer;
        List<string> Process = new List<string>();
        Process.Add("SA");

        DataTable dtPdLine = QueryCommon.GetLine(Process, Master.userInfo.Customer, false, DBConnection);
        lboxPdLine.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
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
            ddlStation.Items.Add(new ListItem("", ""));

            foreach (DataRow dr in dtStation.Rows)
            {
                ddlStation.Items.Add(new ListItem(dr["Station"].ToString().Trim() + " - " + dr["Name"].ToString().Trim(), dr["Station"].ToString().Trim()));
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

    private void InitGridView()
    {
        gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(50);
        gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(150);
        gvQuery.HeaderRow.Cells[2].Width = Unit.Pixel(50);

        for (int r = FixedColCount; r <= StationList.Split(',').Length + (FixedColCount - 1); r++)
        {
            gvQuery.HeaderRow.Cells[r].Width = Unit.Pixel(60);
        }
        InitGridViewHeader();
    }

    private void InitGridView_Detail()
    {
        for (int r = 0; r <= GvDetailColumnName.Length + (FixedColCount - 1); r++)
        {
            gvStationDetail.HeaderRow.Cells[r].Width = Unit.Pixel(GvDetailColumnNameWidth[r]);
        }        
    }

    private void InitGridViewHeader()
    {
        DataTable dtStation = Station.GetStation(DBConnection);
        for (int r = FixedColCount; r <= StationList.Split(',').Length + (FixedColCount - 1); r++)
        {
            ToolUtility tool = new ToolUtility();
            string descr = tool.GetStationDescr(dtStation, gvQuery.HeaderRow.Cells[r].Text.Trim());
            string tip = tool.GetTipString(descr);
            gvQuery.HeaderRow.Cells[r].Attributes.Add("onmouseover", tip);
            gvQuery.HeaderRow.Cells[r].Attributes.Add("onmouseout", "UnTip()");

        }
    }



    private void DB_SelectChange(object sender, EventArgs e)
    {
        InitFamily();
    }
    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitModel();

    }

    
  
    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        queryClick(sender, e);
        EnableBtnExcel(this, true, btnExport.ClientID);                
    }

    public void queryClick(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();

            DateTime timeStart = DateTime.Parse(txtStartTime.Text);
            DateTime timeEnd = DateTime.Parse(txtEndTime.Text);
            List<string> lstPdLine = new List<string>();
            for (int i = 0; i < lboxPdLine.Items.Count; i++)
            {
                if (lboxPdLine.Items[i].Selected)
                {
                    lstPdLine.Add(lboxPdLine.Items[i].Value);
                }
            }


            List<string> lstModel = new List<string>();
            for (int i = 0; i < lboxModel.Items.Count; i++)
            {
                if (lboxModel.Items[i].Selected)
                {
                    lstModel.Add(lboxModel.Items[i].Value);
                }
            }
            dt = intfPCBInputQuery.GetPCBInputQuery(DBConnection, timeStart, timeEnd, lstPdLine, ddlFamily.SelectedValue, lstModel, ddlStation.SelectedValue, StationList);



            if (dt.Rows.Count == 0)
            {
                gvQuery.DataSource = getNullDataTable(1);
                showErrorMessage("Not Found PCB Information!!");
                gvQuery.Height = Unit.Pixel(50);
            }
            else
            {
                this.gvQuery.DataSource = getNullDataTable(dt.Rows.Count);
                this.gvQuery.DataBind();
                this.gvQuery.Height = Unit.Pixel(13 * dt.Rows.Count > 250 ? 250 : 13 * dt.Rows.Count);

                int[] sum = new int[dt.Columns.Count];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvQuery.Rows[i].Cells[0].Text = dt.Rows[i]["Line"].ToString();
                    gvQuery.Rows[i].Cells[1].Text = dt.Rows[i]["Family"].ToString();
                    gvQuery.Rows[i].Cells[2].Text = dt.Rows[i]["Model"].ToString();

                    string r_Family = "";
                    string r_Line = "";
                    string r_Model = "";

                    r_Line = gvQuery.Rows[i].Cells[0].Text.Trim();
                    r_Family = gvQuery.Rows[i].Cells[1].Text.Trim();
                    r_Model = gvQuery.Rows[i].Cells[2].Text.Trim();
                    for (int j = FixedColCount; j <= dt.Columns.Count - 1; j++)
                    {

                        string r_Station = "";
                        r_Station = dt.Columns[j].ColumnName.ToString();


                        gvQuery.Rows[i].Cells[j].Text = string.IsNullOrEmpty(dt.Rows[i][j].ToString()) ? "0" : dt.Rows[i][j].ToString();
                        if (gvQuery.Rows[i].Cells[j].Text != "0")
                        {
                            gvQuery.Rows[i].Cells[j].CssClass = "querycell";
                            //gvResult.Rows[i].Cells[j].BackColor = System.Drawing.Color.Yellow;
                            //gvResult.Rows[i].Cells[j].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='blue';this.className='rowclient';  ");
                            //gvResult.Rows[i].Cells[j].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                            gvQuery.Rows[i].Cells[j].Attributes.Add("onclick", "SelectDetail('" + r_Station + "','" + r_Line + "','" + r_Model + "','" + r_Family + "')");
                        }
                        if (lstPdLine.Count > 0 || lstModel.Count > 0 || ddlFamily.SelectedIndex > 0)
                        {
                            sum[j] = sum[j] + int.Parse(gvQuery.Rows[i].Cells[j].Text);
                        }
                    }
                }

                for (int j = FixedColCount; j <= dt.Columns.Count - 1; j++)
                {
                    //gvResult.HeaderRow.Cells[j].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='blue';this.className='rowclient';  ");
                    //gvResult.HeaderRow.Cells[j].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                    gvQuery.HeaderRow.Cells[j].Attributes.Add("onclick", "SelectDetail('" + gvQuery.HeaderRow.Cells[j].Text + "','All','All','All','true')");
                }


                if (lstPdLine.Count > 0 || lstModel.Count > 0 || ddlFamily.SelectedIndex > 0)
                {
                    gvQuery.FooterStyle.Font.Bold = true;
                    gvQuery.FooterRow.Cells[0].Text = "";
                    gvQuery.FooterRow.Cells[1].Text = "";
                    gvQuery.FooterRow.Cells[2].Text = "TOTAL";
                    for (int j = FixedColCount; j <= dt.Columns.Count - 1; j++)
                    {
                        gvQuery.FooterRow.Cells[j].Text = sum[j].ToString();
                        gvQuery.FooterRow.Cells[j].Font.Bold = true;
                        gvQuery.FooterRow.Cells[j].Font.Size = FontUnit.Parse("16px");
                        if (sum[j] > 0)
                        {
                            gvQuery.HeaderRow.Cells[j].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='blue';this.className='rowclient';  ");
                            gvQuery.HeaderRow.Cells[j].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                            gvQuery.HeaderRow.Cells[j].Attributes.Add("onclick", "SelectDetail('" + gvQuery.HeaderRow.Cells[j].Text + "','All','All','All','true')");
                        }
                    }
                    gvQuery.FooterRow.Visible = true;
                    gvQuery.FooterStyle.BorderColor = System.Drawing.Color.White;
                    if (gvQuery.HeaderRow != null)
                    {
                        gvQuery.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }

                }


            }
            InitGridView();
            gvStationDetail.DataSource = null;
            gvStationDetail.DataBind();
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

    public void btnQueryDetail_Click(object sender, System.EventArgs e)
    {

        try
        {
            string Station = hfStation.Value;
            string Line = hfLine.Value;
            string Model = hfModel.Value;
            string Family = hfFamily.Value;
            DataTable dt = new DataTable();

            dt = intfPCBInputQuery.GetSelectDetail(DBConnection, DateTime.Parse(txtStartTime.Text), DateTime.Parse(txtEndTime.Text), Family, Model, Line, Station, ddlStation.SelectedValue);
            gvStationDetail.DataSource = dt;
            gvStationDetail.DataBind();
            gvStationDetail.Visible = true;
            if (dt.Rows.Count > 0)
            {

                EnableBtnExcel(this, true, btnExport.ClientID);
                EnableBtnExcel2(this, true, btnDetailExport.ClientID);
                gvQuery.GvExtHeight = "200px";
                gvStationDetail.Visible = true;
                InitGridView_Detail();
            }
            else
            {
            }

        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            //BindNoData();
        }
        finally { 
            endWaitingCoverDiv();
        }
    }
    


    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvQuery, "PCBInputQuery", Page);
    }
    protected void btnDetailExport_Click(object sender, EventArgs e) {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvStationDetail, "PCBInputDetailQuery", Page);
    }

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);

    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("setCommonFocus();");
        //scriptBuilder.AppendLine("endWaitingCoverDiv();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Family", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));

        foreach (string STN in StationList.Split(','))
        {
            retTable.Columns.Add(STN, Type.GetType("System.String"));            
        }
        
        return retTable;
    }

    private DataTable initTable_Detail() {
        DataTable retTable = new DataTable();
        
        foreach (string STN in GvDetailColumnName)
        {
            retTable.Columns.Add(STN, Type.GetType("System.String"));
        }

        return retTable;
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Line"] = "";
            newRow["Family"] = "";
            newRow["Model"] = "";

            foreach (string STN in StationList.Split(','))
            {
                newRow[STN] = "";
            }

            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable_Detail(int j)
    { 
     DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
          foreach (string STN in GvDetailColumnName)
            {
                newRow[STN] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }

}
