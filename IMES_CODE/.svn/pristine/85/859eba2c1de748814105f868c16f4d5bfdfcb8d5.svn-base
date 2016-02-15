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

public partial class Query_SQEReport : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);    
    IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    IFA_SQEReport intfSQEReport = ServiceAgent.getInstance().GetObjectByName<IFA_SQEReport>(WebConstant.SQEReport);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);

    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    String DBConnection = "";

    public string[] GvName = { "Site", "M/Q", "Platforms", "Commodity Type", "HP_PN", "IEC_PN", "Vendor", "Consume_Qty" };
    public int[] GvWidth = { 50, 50, 100, 100, 160, 160, 160, 100, 160 };
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
			
			//ddlMaterialType.Items.Add(new ListItem("All","All"));
			ddlMaterialType.Items.Add(new ListItem("EE","EE"));
			ddlMaterialType.Items.Add(new ListItem("ME","ME"));
            ddlMaterialType.SelectedIndex = 0;

            ddlMaterialType_SelectedIndexChanged(null, null);
        }
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
            string kp = hidKp.Value;

            GetConnection();
            DataTable dt = intfSQEReport.GetQueryResult(DBConnection, qType, txtFromDate.Text, txtToDate.Text, ddlMaterialType.SelectedValue, kp);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    //dt.Rows[i]["FPFRate"] = String.Format("{0:P2}", double.Parse(dt.Rows[i]["FPFRate"].ToString()));
                }
            }

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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gvQuery.Rows.Count == 0)
            return;
        ToolUtility tu = new ToolUtility();
        myControls.GridViewExt gv = gvQuery;
        string fileName = "SQEReport";
        if ("M".Equals(hidPeriod.Value))
        {
            fileName = "Monthly" + fileName;
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
	
	protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.ddlKP.Items.Clear();
		    this.ddlKP.Items.Add(new ListItem("", ""));
		    GetConnection();
            DataTable dt = intfSQEReport.GetKpByMaterialType(DBConnection, ddlMaterialType.SelectedValue);        
            if (dt != null && dt.Rows.Count > 0)
		    {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string kp = dt.Rows[i][0].ToString();
                    this.ddlKP.Items.Add(new ListItem(kp, kp));
                }
		    }
		    if ("ME" == ddlMaterialType.SelectedValue)
		    {
			    this.ddlKP.Items.Add(new ListItem("Others","6070"));
		    }
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            
        }
    }

}
