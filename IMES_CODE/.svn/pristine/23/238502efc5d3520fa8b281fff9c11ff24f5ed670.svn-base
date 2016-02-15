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

public partial class Query_KitPartQuery : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    IPAK_KitPartQuery intfKitPartQuery = ServiceAgent.getInstance().GetObjectByName<IPAK_KitPartQuery>(WebConstant.IPAK_KitPartQuery);    

    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!IsPostBack) {            
            InitPage();                        
        }
    }
        protected void InitPage() {        
        InitFamily();
        InitGridView();
    }

    protected void InitFamily() {
        DataTable dt = intfKitPartQuery.GetFamily(DBConnection);
        if (dt.Rows.Count > 0) {
            ddlFamily.Items.Add(new ListItem("-","-"));
            for (int i = 0; i < dt.Rows.Count; i++) {
                ddlFamily.Items.Add(new ListItem(dt.Rows[i]["Family"].ToString(), dt.Rows[i]["Family"].ToString()));
            }
        }
    }

    protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        IList<string> lFamily = new List<string>();
        lFamily.Add(ddlFamily.SelectedValue);
        DataTable dt = intfKitPartQuery.GetModel(DBConnection, lFamily);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ddlModel.Items.Add(new ListItem(dt.Rows[i]["Model"].ToString(), dt.Rows[i]["Model"].ToString()));
        }
    }
    protected void InitGridView() {
        gvModel.DataSource = getNullModelTable(1);
        gvModel.DataBind();
    }

    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnAdd_Click(object sender, EventArgs e)
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
            if (dt.Rows.Count > 0)
            {
                gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(120);
                gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(100);
            }
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.ToString());
        }
        finally
        {
            hideWait();
        }
    }

    private DataTable getDataTable()
    {
        DataTable dt = new DataTable();

        List<string> lstModel = new List<string>();
        List<string> lstCnt = new List<string>();

        string modelcount = hidmodelcnt.Value;
        string[] items = modelcount.Split(';');
        foreach (var item in items)
        {
            if (item != ""){
                string model = item.Split(':')[0];
                string cnt = item.Split(':')[1];
                if (lstModel.IndexOf(model) == -1)
                {
                    lstModel.Add(model);
                    lstCnt.Add(cnt);
                }
                else{
                    lstCnt[lstModel.IndexOf(model)] = (int.Parse(lstCnt[lstModel.IndexOf(model)].ToString()) + int.Parse(cnt)).ToString();                                            
                }
            }            
        }

        dt = intfKitPartQuery.GetData(DBConnection, lstModel, lstCnt);
        hidmodelcnt.Value = "";
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

    protected DataTable getNullModelTable(int rows) {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("Family",System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("Model",System.Type.GetType("System.String")));
        dt.Columns.Add(new DataColumn("Qty",System.Type.GetType("System.String")));
        for (int i = 0; i < rows; i++) { 
            DataRow dr = dt.NewRow();            
            dt.Rows.Add(dr);
        }            
        return dt;
    
    }



}
