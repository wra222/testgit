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

public partial class Query_MaterialUsed : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    IFA_MaterialUsed intfMaterialUsed = ServiceAgent.getInstance().GetObjectByName<IFA_MaterialUsed>(WebConstant.MaterialUsed);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    String DBConnection = "";

    public string[] GvName = { "Site", "M/Q", "Platforms", "Commodity Type", "HP_PN", "IEC_PN", "Vendor", "Consume_Qty" };
    public int[] GvWidth = { 50, 80, 150, 290, 120, 120, 150, 100 };
    private static int IDX_Monthly = 1;

    private void GetConnection()
    {
        string dbName = Request.QueryString["dbName"];
        if (string.IsNullOrEmpty(dbName))
            dbName = "HPIMES_RPT";
        DBInfo obj = iConfigDB.GetDBInfo();
        DBConnection = string.Format(obj.HistoryConnectionString, dbName);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack) {
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

        /*
            for (int i=0; i<20; i++){
                string y = DateTime.Now.AddYears(i * -1).Year.ToString();
                ddlQryMonth_Year.Items.Add(new ListItem(y,y));
                ddlQryQuarter_Year.Items.Add(new ListItem(y, y));
            }
            for (int i = 1; i <= 12; i++)
            {
                string m = String.Format("{0:00}", i);
                ddlQryMonth_Month.Items.Add(new ListItem(m, m));
            }
            for (int i = 1; i <= 4; i++){
                string q = "Q" + i.ToString();
                ddlQryQuarter_Quarter.Items.Add(new ListItem(q, q));
            }
        */
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
            string qType = rbPeriod.SelectedValue; //, parm1 = "", parm2 = "";
            /*
            if ("M".Equals(qType))
            {
                parm1 = ddlQryMonth_Year.SelectedValue;
                parm2 = ddlQryMonth_Month.SelectedValue;
            }
            else if ("Q".Equals(qType))
            {
                parm1 = ddlQryQuarter_Year.SelectedValue;
                parm2 = ddlQryQuarter_Quarter.SelectedValue;
            }
            */

            //DataTable dt = getDataTable(qType, parm1, parm2);
            DataTable dt = getDataTable(qType, txtFromDate.Text, txtToDate.Text);
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < GvWidth.Length; i++)
                {
                    gvQuery.HeaderRow.Cells[i].Width = Unit.Pixel(GvWidth[i]);
                }
            }
            else
            {
                endWaitingCoverDivAndAlert();
                return;
            }
            hidPeriod.Value = qType;
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

    private DataTable getDataTable(string qType, string parm1, string parm2)
    {
        GetConnection();
        DataTable dt = intfMaterialUsed.GetQueryResult(DBConnection, qType, parm1, parm2);        
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                //dt.Rows[i]["FPFRate"] = String.Format("{0:P2}", double.Parse(dt.Rows[i]["FPFRate"].ToString()));
            }     
        }
        return dt;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gvQuery.Rows.Count == 0)
            return;
        ToolUtility tu = new ToolUtility();
        myControls.GridViewExt gv = gvQuery;
        string fileName = "MaterialUsed";
        if ("M".Equals(hidPeriod.Value))
        {
            fileName = "Monthly" + fileName;

            DataTable dt = (DataTable) gvQuery.DataSource;
            DataTable dt2 = new DataTable();
            for (int p = 0; p < gvQuery.HeaderRow.Cells.Count; p++)
                dt2.Columns.Add(gvQuery.HeaderRow.Cells[p].Text);
            for (int i = 0; i < gvQuery.Rows.Count; i++)
            {
                DataRow r = dt2.NewRow();
                for (int j = 0; j < gvQuery.Rows[i].Cells.Count; j++) {
                    if (j == IDX_Monthly)
                        r[j] = "=\"" + gvQuery.Rows[i].Cells[j].Text + "\"";
                    else
                        r[j] = gvQuery.Rows[i].Cells[j].Text;
                }
                dt2.Rows.Add(r);
            }
            gv = new myControls.GridViewExt();
            gv.DataSource = dt2;
            gv.DataBind();
        }
        else if ("Q".Equals(hidPeriod.Value))
        {
            fileName = "Quarterly" + fileName;
        }
        tu.ExportExcel(gv, fileName, Page);
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
    private void endWaitingCoverDivAndAlert()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();alert('查無資料');" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

}
