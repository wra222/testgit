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

public partial class Query_KitFloatLocQuery : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    IPAK_KitFloatLocQuery intfKitFloatLoc = ServiceAgent.getInstance().GetObjectByName<IPAK_KitFloatLocQuery>(WebConstant.IPAK_KitFloatLocQuery);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
      
        DBConnection = CmbDBType.ddlGetConnection();
        if (!IsPostBack) {
            GetLine();                        
        }

        
    }
    private void GetLine()
    {
   
        IList<string> line = new List<string>() { "PAK" };
        DataTable dtPdLine = QueryCommon.GetLine(line, "HP", true, DBConnection);
        foreach (DataRow dr in dtPdLine.Rows)
        {
            droLine.Items.Add(new ListItem(dr["Line"].ToString().Trim(), dr["Line"].ToString().Trim()));
        
        }
   //     droLine.DataSource = dtPdLine;
     //   droLine.DataBind();

    
    }
    protected void InitPage() {                
      
    }
   
    protected void InitGridView() {
        gvQuery.DataSource = getNullQueryTable(1);
        gvQuery.DataBind();
    }

   
    
    
    private DataTable getDataTable()
    {
        DataTable dt = new DataTable();

     
        return dt;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "showErrorMessage", scriptBuilder.ToString(), false);
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

    protected DataTable getNullQueryTable(int rows) {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("Station",System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("Model",System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("PartNo",System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("Description", System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("Location", System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("Qty", System.Type.GetType("System.String")));
        for (int i = 0; i < rows; i++) { 
            DataRow dr = dt.NewRow();            
            dt.Rows.Add(dr);
        }            
        return dt;    
    }


    protected void btnQ_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txtModel.Text.Trim()))
        {
          writeToAlertMessage("Please input Model");
            return;
        }
        DataTable dt = intfKitFloatLoc.GetData(DBConnection,txtModel.Text.Trim(), droLine.SelectedValue);
        if (dt.Rows.Count == 0)
        { writeToAlertMessage("No data found!"); }
        else
        {
            gvQuery.DataSource = dt;
            gvQuery.DataBind();
        }
   
    }
}
