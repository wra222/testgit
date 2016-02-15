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

public partial class Query_PCBStationQuery : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    ISA_PCBStationQuery intfPCBStationQuery = ServiceAgent.getInstance().GetObjectByName<ISA_PCBStationQuery>(WebConstant.ISA_PCBStationQuery);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    private static int IdxDefectQty = 5;
    public int[] GvDetailColumnNameWidth = { 110, 110, 80, 60, 90, 70, 80, 80, 80, 140, 150, 150 };

    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!IsPostBack) {
            InitPage();
            txtStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        DropDownList ddlDB = ((DropDownList)CmbDBType.FindControl("ddlDB"));
        ddlDB.SelectedIndexChanged += new EventHandler(this.DB_SelectChange);
        ddlDB.AutoPostBack = true;
    }

    protected void InitPage() {
        InitFamily();
        InitPdLine();
        InitStation();
    }

    protected void InitFamily()
    {
        DataTable dtFamily = intfFamily.GetPCBFamily(DBConnection);
        ddlFamily.Items.Clear();
        ddlFamily.Items.Add(new ListItem("All", ""));
        if (dtFamily.Rows.Count > 0)
        {
            for (int i = 0; i < dtFamily.Rows.Count; i++)
            {
                ddlFamily.Items.Add(new ListItem(dtFamily.Rows[i]["Family"].ToString(), dtFamily.Rows[i]["Family"].ToString()));
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

    private void DB_SelectChange(object sender, EventArgs e)
    {
        InitFamily();
    }
    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitModel();

    }

    protected void InitStation()
    {
        
        List<string> lstFamily = new List<string>();
        lstFamily.Add(ddlFamily.SelectedValue);

        DataTable Station = intfPCBStationQuery.GetStation(DBConnection, lstFamily);

        lboxStation.Items.Clear();
        for (int i = 0; i < Station.Rows.Count; i++)
        {
            lboxStation.Items.Add(new ListItem(Station.Rows[i]["Station"].ToString() + " - " + Station.Rows[i]["Descr"].ToString(),
                                            Station.Rows[i]["Station"].ToString()));

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
            DataTable dt = getDataTable();
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
            if (dt.Rows.Count > 0)
            {
                gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(120);
                gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(100);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (gvQuery.Rows[i].Cells[IdxDefectQty].Text != "0")
                    {
                        gvQuery.Rows[i].Cells[IdxDefectQty].CssClass = "querycell";
                        gvQuery.Rows[i].Cells[IdxDefectQty].Attributes.Add("onclick", "SelectDetail('" + gvQuery.Rows[i].Cells[0].Text + "')");
                    }
                }
            }
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
        DataTable dt = null;

        DateTime timeStart = DateTime.Parse(txtStartTime.Text);
        DateTime timeEnd = DateTime.Parse(txtEndTime.Text);
        List<string> lstPdLine = new List<string>();
        for (int i = 0; i < lboxPdLine.Items.Count; i++)
        {
            if (lboxPdLine.Items[i].Selected) {
                lstPdLine.Add(lboxPdLine.Items[i].Value);
            }
        }

        List<string> lstFamily = new List<string>();
        if ((ddlFamily.SelectedIndex > 0) && (! "".Equals(ddlFamily.SelectedValue))) {
            lstFamily.Add(ddlFamily.SelectedValue);
        }        
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
            
        dt = intfPCBStationQuery.GetPCBStationQuery(DBConnection, timeStart, timeEnd, lstPdLine, lstFamily, lstModel, lstStation);        
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                dt.Rows[i]["FPFRate"] = String.Format("{0:P2}", double.Parse(dt.Rows[i]["FPFRate"].ToString()));
                dt.Rows[i]["FPYRate"] = String.Format("{0:P2}", double.Parse(dt.Rows[i]["FPYRate"].ToString()));
            }     
        }
        return dt;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gvQuery.HeaderRow == null)
            return;
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvQuery, "PCBStationQuery", Page);
    }
    protected void btnDetailExport_Click(object sender, EventArgs e)
    {
        if (gvStationDetail.HeaderRow == null)
            return;
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvStationDetail, "PCBStationDetailQuery", Page);
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
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    private void InitGridView_Detail()
    {
        for (int r = 0; r <= GvDetailColumnNameWidth.Length; r++)
        {
            gvStationDetail.HeaderRow.Cells[r].Width = Unit.Pixel(GvDetailColumnNameWidth[r]);
        }
    }

    public void btnQueryDetail_Click(object sender, System.EventArgs e)
    {
        try
        {
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

            string Family = hfFamily.Value;
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

            DataTable dt = intfPCBStationQuery.GetSelectDetail(DBConnection, timeStart, timeEnd, lstPdLine, Family, lstModel, lstStation);
            gvStationDetail.DataSource = dt;
            gvStationDetail.DataBind();
            gvStationDetail.Visible = true;
            if (dt.Rows.Count > 0)
            {
                InitGridView_Detail();
            }
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

}
