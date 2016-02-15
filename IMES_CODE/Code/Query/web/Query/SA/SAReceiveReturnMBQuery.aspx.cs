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

public partial class Query_SAReceiveReturnMBQuery : IMESQueryBasePage
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    //IFamily intfFamily = ServiceAgent.getInstance().GetObjectByName<IFamily>(WebConstant.IFamilyBObject);
    //IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
    //ISA_PCBStationQuery intfPCBStationQuery = ServiceAgent.getInstance().GetObjectByName<ISA_PCBStationQuery>(WebConstant.ISA_PCBStationQuery);
    IConfigDB intfConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    //ISA_SAMBRepair intfSAMBRepair = ServiceAgent.getInstance().GetObjectByName<ISA_SAMBRepair>(WebConstant.ISA_SAMBRepair);
    ISA_SAReceiveReturnMBQuery iSA_SAReceiveReturnMBQuery = ServiceAgent.getInstance().GetObjectByName<ISA_SAReceiveReturnMBQuery>(WebConstant.ISA_SAReceiveReturnMBQuery);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    String DBConnection = "";
    public string StationList;

    public string[] GvDetailColumnName = { "Family", "收板日期", "退板日期",//3
                                            "白/夜", "测试站别", "线别", "不良代码", "主板序列号", "不良现象", "修护员", //7
                                            "ICT站别", "FUN站别", "负责人", "SA測試涵蓋", "外觀不良", //5
                                            "入修時間","出修時間","不良位置","修護結果","責任單位","修護員","備注"};//6
    public int[] GvDetailColumnNameWidth = { 100, 180, 180, 
                                                55, 55, 55, 70, 90, 220, 80, 
                                                55, 55, 80, 90, 55, 
                                                180,180,70,70,70,70,70};

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        StationList = PCAInputStation;
        if (!IsPostBack) {
            InitPage();
            InitCondition();
            txtStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        DropDownList ddlDB = ((DropDownList)CmbDBType.FindControl("ddlDB"));
        ddlDB.SelectedIndexChanged += new EventHandler(this.DB_SelectChange);
        ddlDB.AutoPostBack = true;     
    }

    protected void InitPage() {
        this.gvQuery.DataSource = getNullDataTable(1);
        this.gvQuery.DataBind();
        InitGridView();
    }

    private void InitCondition()
    {
        InitMBCode();
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();

            for (int n = 0; n < GvDetailColumnName.Length; n++)
            {
                newRow[GvDetailColumnName[n]] = "";
            }

            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();

        for (int n = 0; n < GvDetailColumnName.Length; n++)
        {
            retTable.Columns.Add(GvDetailColumnName[n], Type.GetType("System.String"));
        }
        return retTable;
    }

    private void InitGridView()
    {
        for (int n = 0; n < GvDetailColumnName.Length; n++)
        {
            gvQuery.HeaderRow.Cells[n].Width = Unit.Pixel(GvDetailColumnNameWidth[n]);
        }
    }

    protected void InitMBCode()
    {
        DataTable dtMBCode = iSA_SAReceiveReturnMBQuery.GetMBCode(DBConnection);
        this.ddlMBCode.Items.Clear();
        //this.ddlMBCode.Items.Add(new ListItem("-", ""));
        if (dtMBCode.Rows.Count > 0)
        {
            for (int i = 0; i < dtMBCode.Rows.Count; i++)
            {
                this.ddlMBCode.Items.Add(new ListItem(dtMBCode.Rows[i]["MBCode"].ToString(), dtMBCode.Rows[i]["MBCode"].ToString()));
            }
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
            DataTable dt = new DataTable();
            dt = getDataTable();            
            //gvQuery.DataSource = dt;
            this.gvQuery.DataSource = getNullDataTable(dt.Rows.Count);
            this.gvQuery.DataBind();
            
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int n = 0; n < GvDetailColumnName.Length; n++)
                {
                    string text = (null == dt.Rows[i][GvDetailColumnName[n]]) ? "" : dt.Rows[i][GvDetailColumnName[n]].ToString();
                    gvQuery.Rows[i].Cells[n].Text = text;
                }
                //gvQuery.Rows[i].Cells[6].Text = dt.Rows[i]["Status"].ToString() == "1" ? "OK" : "NG";
            }
            //gvQuery.DataBind();
            InitGridView();
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

        List<string> mbCodeList = new List<string>();
        for (int i = 0; i < this.ddlMBCode.Items.Count; i++)
        {
            if (this.ddlMBCode.Items[i].Selected)
            {
                mbCodeList.Add(this.ddlMBCode.Items[i].Value);
            }
        }
        dt = iSA_SAReceiveReturnMBQuery.GetMBRepairInfo(DBConnection, timeStart, timeEnd, mbCodeList);
        //if (mbCodeList.Count == 0)
        //{
        //    dt = iSA_SAReceiveReturnMBQuery.GetMBRepairInfo(DBConnection, timeStart, timeEnd);
        //}
        //else
        //{
        //    dt = iSA_SAReceiveReturnMBQuery.GetMBRepairInfo(DBConnection, timeStart, timeEnd, mbCodeList);
        //}   

        return dt;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        if (gvQuery.HeaderRow != null && gvQuery.HeaderRow.Cells.Count > 0)
            tu.ExportExcel(gvQuery, "SAMBRepair", Page);
        else
            writeToAlertMessage("Please select one record!"); 
    }

    protected void gvQuery_PreRender(object sender, EventArgs e)
    {
        gvQuery.UseAccessibleHeader = true;
        if (gvQuery.Rows.Count > 0) {
            gvQuery.HeaderRow.TableSection = TableRowSection.TableHeader;            
        }                
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
    
    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {

    }
    private void DB_SelectChange(object sender, EventArgs e)
    {

    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }

}
