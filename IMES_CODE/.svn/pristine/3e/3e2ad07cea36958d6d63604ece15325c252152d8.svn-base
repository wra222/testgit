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

public partial class Query_SA_PCBOQCQuery : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
   // IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    ISA_OQCQuery ISAOQC = ServiceAgent.getInstance().GetObjectByName<ISA_OQCQuery>(WebConstant.ISA_OQCQuery);
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

    protected void InitPage()
    {
        GetCause();
    }

    private void GetCause()
    {
        List<string> infotype = new List<string>();
        infotype.Add("SACause");
        DataTable dt = QueryCommon.GetDefectInfo(infotype, "HP", DBConnection);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                lboxcause.Items.Add(new ListItem(dr["Code"].ToString().Trim() + " - " + dr["Description"].ToString().Trim(), dr["Code"].ToString().Trim()));
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
            if (dt.Rows.Count > 0)
            {
              
                gvQuery.DataSource = dt;
                gvQuery.DataBind();
            }
            else
            {
                writeToAlertMessage("No Data!!");
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
        DataTable dt = new DataTable();
        DateTime timeStart = DateTime.Parse("2014-06-01");
        DateTime timeEnd = DateTime.Parse(txtEndTime.Text);

        IList<string> cause = new List<string>();
        string process=IboxProcess.SelectedValue.Trim();
        string status=lboxStatus.SelectedValue.Trim();
        foreach (ListItem item in lboxcause.Items)
        {
            if (item.Selected)
            {
                cause.Add(item.Value.Trim());
            }
        }
        dt = ISAOQC.GetPCBOQCQuery(DBConnection, timeStart, timeEnd, status, cause, process);
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

  
    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);

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
    //public override void VerifyRenderingInServerForm(Control control)
    //{

    //}
}
