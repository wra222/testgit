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

public partial class Query_PCBTestDefect : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    ISA_TestDefect intfTestDefetct = ServiceAgent.getInstance().GetObjectByName<ISA_TestDefect>(WebConstant.ISA_TestDefect);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);


    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!IsPostBack)
        {
            InitPage();
            txtStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00");
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
        
        DropDownList ddlDB = ((DropDownList)CmbDBType.FindControl("ddlDB"));
        ddlDB.SelectedIndexChanged += new EventHandler(this.DB_SelectChange);
        ddlDB.AutoPostBack = true;        
        
    }

    protected void InitPage() {
        InitFamily();
        InitPdLine();    
        InitModel();
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

    protected void InitPdLine()
    {
        //CmbPdLine.Customer = Master.userInfo.Customer;        
        //CmbPdLine.Stage = "SA";
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

    protected void InitModel()
    {
        DataTable Model = intfTestDefetct.GetModel(DBConnection, ddlFamily.SelectedValue);
        lboxModel.Items.Clear();
        for (int i = 0; i < Model.Rows.Count; i++)
        {
            lboxModel.Items.Add(new ListItem(Model.Rows[i]["Model"].ToString(), Model.Rows[i]["Model"].ToString()));

        }
    }

    protected void InitStation()
    {
        DataTable dtStation = intfTestDefetct.GetTestStation(DBConnection, ddlFamily.SelectedValue);
        lboxStation.Items.Clear();
        

        if (dtStation.Rows.Count > 0)
        {
            for (int i = 0; i < dtStation.Rows.Count; i++)
            {
                lboxStation.Items.Add(new ListItem(dtStation.Rows[i]["Station"].ToString().Trim() + " - " + dtStation.Rows[i]["Descr"].ToString(),
                                                dtStation.Rows[i]["Station"].ToString().Trim()));
            }
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
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
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

        IList<string> lstFamily = new List<string>();
        IList<string> lstPdLine = new List<string>();
        IList<string> lstModel = new List<string>();
        IList<string> lstStation = new List<string>();        
        
        if (ddlFamily.SelectedIndex > 0){
            lstFamily.Add(ddlFamily.SelectedValue.Trim());
        }

        foreach (ListItem item in lboxPdLine.Items)
        {
            if( item.Selected){
                lstPdLine.Add(item.Value.Trim());
            }
        }
        
        foreach (ListItem item in lboxStation.Items)
        {
            if (item.Selected) {
                lstStation.Add(item.Value.Trim());
            }
        }
        foreach (ListItem item in lboxModel.Items)
        {
            if (item.Selected)
            {
                lstModel.Add(item.Value.Trim());
            }
        }
        dt = intfTestDefetct.GetDefectInfo(DBConnection, timeStart, timeEnd, lstFamily, lstPdLine, lstModel, lstStation);
            

        return dt;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        
        if (gvQuery.HeaderRow != null && gvQuery.HeaderRow.Cells.Count > 0)
            tu.ExportExcel(gvQuery, "PCBTestDefect", Page);
        else
            writeToAlertMessage("Please select one record!");
    }


    private void DB_SelectChange(object sender, EventArgs e)
    {
        InitPage();
    }

    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitModel();
        InitStation();
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

}
