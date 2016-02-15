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

public partial class Query_FAReturnPCA : IMESQueryBasePage
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IConfigDB intfConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    ISA_FAReturnPCA intfFA = ServiceAgent.getInstance().GetObjectByName<ISA_FAReturnPCA>(WebConstant.ISA_FAReturnPCAQuery);
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
        InitModel();
    }

    protected void InitModel()
    {
        string[] Model = intfFA.GetModel(DBConnection);
        lboxModel.Items.Clear();
        for (int i = 0; i < Model.Length; i++)
        {
            lboxModel.Items.Add(new ListItem(Model[i], Model[i]));
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
            gvQuery.DataSource = getNullDataTable(dt.Rows.Count);            
            gvQuery.DataBind();
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                gvQuery.Rows[i].Cells[0].Text = dt.Rows[i]["PCBNo"].ToString();
                gvQuery.Rows[i].Cells[1].Text = dt.Rows[i]["MBCT"].ToString();
                gvQuery.Rows[i].Cells[2].Text = dt.Rows[i]["UnitCUSTSN"].ToString();
                gvQuery.Rows[i].Cells[3].Text = dt.Rows[i]["InfoValue"].ToString();
                gvQuery.Rows[i].Cells[4].Text = dt.Rows[i]["DefectCode"].ToString();
                gvQuery.Rows[i].Cells[5].Text = dt.Rows[i]["FailureDescription"].ToString();
                gvQuery.Rows[i].Cells[6].Text = dt.Rows[i]["Descr"].ToString();
                gvQuery.Rows[i].Cells[7].Text = dt.Rows[i]["Cause"].ToString();
                gvQuery.Rows[i].Cells[8].Text = dt.Rows[i]["Causedescription"].ToString();
                gvQuery.Rows[i].Cells[9].Text = dt.Rows[i]["Obligation"].ToString();
                gvQuery.Rows[i].Cells[10].Text = dt.Rows[i]["Site"].ToString();
                gvQuery.Rows[i].Cells[11].Text = dt.Rows[i]["MajorPart"].ToString();
                gvQuery.Rows[i].Cells[12].Text = dt.Rows[i]["Remark"].ToString();
                gvQuery.Rows[i].Cells[13].Text = dt.Rows[i]["Location"].ToString();
                gvQuery.Rows[i].Cells[14].Text = dt.Rows[i]["OldPartSno"].ToString();
                gvQuery.Rows[i].Cells[15].Text = dt.Rows[i]["Cdt"].ToString();
                gvQuery.Rows[i].Cells[16].Text = dt.Rows[i]["Udt"].ToString();

                gvQuery.Rows[i].Cells[17].Text = dt.Rows[i]["Family"].ToString();
                gvQuery.Rows[i].Cells[18].Text = dt.Rows[i]["FA_Line"].ToString();
                gvQuery.Rows[i].Cells[19].Text = dt.Rows[i]["Test_Station"].ToString();
                gvQuery.Rows[i].Cells[20].Text = dt.Rows[i]["白/夜"].ToString();
                gvQuery.Rows[i].Cells[21].Text = dt.Rows[i]["FA_DefectCode"].ToString();
                gvQuery.Rows[i].Cells[22].Text = dt.Rows[i]["FAfailurestation"].ToString();
                gvQuery.Rows[i].Cells[23].Text = dt.Rows[i]["FA_Descr"].ToString();
                gvQuery.Rows[i].Cells[24].Text = dt.Rows[i]["FA_Editor"].ToString();
                gvQuery.Rows[i].Cells[25].Text = dt.Rows[i]["SA_Editor"].ToString();

            }
            //gvQuery.DataBind();
            
            if (dt.Rows.Count > 0)
            {
                InitGridView();
                EnableBtnExcel(this, true, btnExport.ClientID);
            }
            
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.ToString());
            EnableBtnExcel(this, false, btnExport.ClientID);
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
        IList<string> lstModel = new List<string>();
        foreach (ListItem item in lboxModel.Items)
        {
            if (item.Selected) {
                lstModel.Add(item.Value);
            }
        }
        dt = intfFA.GetFAReturnPCAInfo(DBConnection, timeStart, timeEnd,lstModel);
            

        return dt;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        if (gvQuery.HeaderRow != null && gvQuery.HeaderRow.Cells.Count > 0)
            tu.ExportExcel(gvQuery, "FAReturnPCAQuery", Page);
        else
            writeToAlertMessage("Please select one record!"); 
        
    }
    //start~
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["PCBNo"] = "";
            newRow["MBCT"]="";
            newRow["UnitCUSTSN"] = "";
            newRow["InfoValue"] = "";
            newRow["DefectCode"] = "";
            newRow["FailureDescription"] = "";
            newRow["Descr"] = "";
            newRow["Cause"] = "";
            newRow["Causedescription"] = "";

            newRow["Obligation"] = "";
            newRow["Site"] = "";
            newRow["MajorPart"] = "";
            newRow["Remark"] = "";
            newRow["Location"] = "";
            newRow["OldPartSno"] = "";
            newRow["Cdt"] = "";
            newRow["Udt"] = "";


            newRow["Family"] = "";
            newRow["FA_Line"] = "";
            newRow["Test_Station"] = "";
            newRow["白/夜"] = "";
            newRow["FA_DefectCode"] = "";
            newRow["FAfailurestation"] = "";
            newRow["FA_Descr"] = "";
            newRow["FA_Editor"] = "";
            newRow["SA_Editor"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PCBNo", Type.GetType("System.String"));
        retTable.Columns.Add("MBCT", Type.GetType("System.String"));
        retTable.Columns.Add("UnitCUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("InfoValue", Type.GetType("System.String"));
        retTable.Columns.Add("DefectCode", Type.GetType("System.String"));
        retTable.Columns.Add("FailureDescription", Type.GetType("System.String"));
        retTable.Columns.Add("Descr", Type.GetType("System.String"));
        retTable.Columns.Add("Cause", Type.GetType("System.String"));
        retTable.Columns.Add("Causedescription", Type.GetType("System.String"));
        retTable.Columns.Add("Obligation", Type.GetType("System.String"));
        retTable.Columns.Add("Site", Type.GetType("System.String"));
        retTable.Columns.Add("MajorPart", Type.GetType("System.String"));
        retTable.Columns.Add("Remark", Type.GetType("System.String"));
        retTable.Columns.Add("Location", Type.GetType("System.String"));
        retTable.Columns.Add("OldPartSno", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        retTable.Columns.Add("Udt", Type.GetType("System.String"));

        retTable.Columns.Add("Family");
        retTable.Columns.Add("FA_Line");
        retTable.Columns.Add("Test_Station");
        retTable.Columns.Add("白/夜");
        retTable.Columns.Add("FA_DefectCode");
        retTable.Columns.Add("FAfailurestation");
        retTable.Columns.Add("FA_Descr");
        retTable.Columns.Add("FA_Editor");
        retTable.Columns.Add("SA_Editor");
        return retTable;
    }

    private void InitGridView()
    {
        int i = 100;
        int j = 70;
        int k = 180;
        int l = 250;
        int m = 400;

        gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(i);
        gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[2].Width = Unit.Pixel(i);
        gvQuery.HeaderRow.Cells[3].Width = Unit.Pixel(j);
        gvQuery.HeaderRow.Cells[4].Width = Unit.Pixel(i);
        gvQuery.HeaderRow.Cells[5].Width = Unit.Pixel(m);
        gvQuery.HeaderRow.Cells[6].Width = Unit.Pixel(m);
        gvQuery.HeaderRow.Cells[7].Width = Unit.Pixel(j);
        gvQuery.HeaderRow.Cells[8].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[9].Width = Unit.Pixel(i);
        gvQuery.HeaderRow.Cells[10].Width = Unit.Pixel(i);
        gvQuery.HeaderRow.Cells[11].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[12].Width = Unit.Pixel(k);

        gvQuery.HeaderRow.Cells[13].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[14].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[15].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[16].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[17].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[18].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[19].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[20].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[21].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[22].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[23].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[24].Width = Unit.Pixel(k);
        gvQuery.HeaderRow.Cells[25].Width = Unit.Pixel(k);

    }
    //end~
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
        scriptBuilder.AppendLine("endWaitingCoverDiv();");
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

    private void DB_SelectChange(object sender, EventArgs e)
    {
        //InitModel();        
    }

}
