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

public partial class Query_ASTReport : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);        
    ISA_PCBStationQuery intfPCBStationQuery = ServiceAgent.getInstance().GetObjectByName<ISA_PCBStationQuery>(WebConstant.ISA_PCBStationQuery);
    IPAK_ASTReport intfASTReport = ServiceAgent.getInstance().GetObjectByName<IPAK_ASTReport>(WebConstant.IPAK_ASTReport);    

    String DBConnection = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!IsPostBack) {
            txtFromDate.Text = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            InitPage();                        
        }
    }

    protected void InitPage() {        
        InitFamily();        
    }

    protected void InitFamily() {
        DataTable dt = intfASTReport.GetFamily(DBConnection);
        if (dt.Rows.Count > 0) {
            ddlFamily.Items.Add(new ListItem("-","-"));
            for (int i = 0; i < dt.Rows.Count; i++) {
                ddlFamily.Items.Add(new ListItem(dt.Rows[i]["Family"].ToString(), dt.Rows[i]["Family"].ToString()));
            }
        }
    }

     protected void ddlFamily_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFamily.SelectedIndex > -1)
        {
            GetModel();
        }               
    }

    protected void GetModel()
    {
        string Connection = CmbDBType.ddlGetConnection();
        try
        {
            IList<string> lFamily = new List<string>();
            lFamily.Add(ddlFamily.SelectedValue);
            DataTable dt = intfASTReport.GetASTModel(DBConnection, lFamily);
            lstModel.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstModel.Items.Add(new ListItem(dt.Rows[i]["Model"].ToString(), dt.Rows[i]["Model"].ToString()));
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    public void btnQueryDetail_Click(object sender, System.EventArgs e)
    {
        string Connection = CmbDBType.ddlGetConnection();
        try
        {
            string Model = hfModel.Value;

            DataTable dt = intfASTReport.GetSelectDetail(Connection, DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text), Model);
            gvDetail.DataSource = dt;
            gvDetail.DataBind();
            gvDetail.Visible = true;
            if (dt.Rows.Count > 0)
            {
                gvQuery.GvExtHeight = "200px";
                gvDetail.Visible = true;
            }

        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
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
            dt.Columns.Add(new DataColumn("Rest", Type.GetType("System.String")));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (int.Parse(dt.Rows[i]["AST_QTY"].ToString()) > 0)
                {
                    dt.Rows[i]["Rest"] = getRest(dt.Rows[i]["Range"].ToString(), dt.Rows[i]["MaxNo"].ToString());
                }
            }
            


            gvQuery.DataBind();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (int.Parse(dt.Rows[i]["AST_QTY"].ToString())>0) {
                        gvQuery.Rows[i].Cells[2].CssClass = "ast_qty";
                        //gvQuery.Rows[i].Cells[2].Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor; this.style.backgroundColor='blue';this.className='rowclient';  ");
                        //gvQuery.Rows[i].Cells[2].Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
                        gvQuery.Rows[i].Cells[2].Attributes.Add("onclick", "SelectDetail('" + dt.Rows[i]["Model"].ToString() + "')");
                        int temp = 0;
                        if (int.TryParse(dt.Rows[i]["Rest"].ToString(), out temp))
                        {
                            if ((int.Parse(dt.Rows[i]["Qty_SUM"].ToString()) - int.Parse(dt.Rows[i]["AST_QTY"].ToString())) > int.Parse(dt.Rows[i]["Rest"].ToString()))                        
                            {
                                gvQuery.Rows[i].Cells[6].BackColor = System.Drawing.Color.Red;                             
                            }
                        }
                        else if (dt.Rows[i]["Rest"].ToString() == "Please check manaually")
                        {
                            gvQuery.Rows[i].Cells[6].BackColor = System.Drawing.Color.Red; 
                        }
                    }
                }
                gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(120);
                gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(100);
                gvQuery.HeaderRow.Cells[2].Width = Unit.Pixel(100);
                gvQuery.GvExtHeight = "370px";
                gvQuery.Visible = true;
                gvDetail.DataSource = null;
                gvDetail.Visible = false;
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
        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);    
        
        List<string> Family = new List<string>();
        Family.Add(ddlFamily.SelectedValue);
        List<string> Model = new List<string>();
        foreach (ListItem item in lstModel.Items)
        {
            if (item.Selected)
            {
                Model.Add(item.Value);
            }
        }

        dt = intfASTReport.GetASTReport(DBConnection, timeStart, timeEnd,Family, Model);

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
    private string getRest(string Range, string MaxNo) {
        string result = "";
        if (Range == "") {
            result = ""; return result;
        }
  
        try
        {   
            string[] aryRange = Range.Split('~');

            if (aryRange.Length == 2)
            {
                result = (int.Parse(aryRange[1].Substring(getNumberPosition(aryRange[1]))) - 
                        int.Parse(MaxNo.Substring(getNumberPosition(MaxNo)))).ToString();
            }
            else {
                result = "Please check manaually";
            }
            
        }
        catch (Exception ex)
        {
            result = "Please check manaually";         
        }
        return result;
    }
    private int getNumberPosition(string input) {       
        int ps = 0;
            if (input != "") {
                for (int i = input.Length -1; i >= 0 ; i--)
                {
                    if ("0123456789".IndexOf(char.Parse(input.Substring(i, 1))) == -1) {
                        ps = i + 1;
                        break;
                    }
                }
            }
        return ps;
    }

}
