using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Xml.Linq;
using com.inventec.imes.DBUtility;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
//using Microsoft.SqlServer.Dts.Runtime;
using System.Data.SqlClient;

public partial class Query_PAK_CT_TAT_Query : System.Web.UI.Page
{
    ICT_TAT_Query CT_Query = ServiceAgent.getInstance().GetObjectByName<ICT_TAT_Query>(WebConstant.ICT_TAT_Query);
    String DBConnection = "";
    public string pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();
        if (!Page.IsPostBack)
        {
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
    public void Query_Click(object sender, EventArgs e)
    {
        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
        if (timeEnd.Day - timeStart.Day > 7)
        {
            showErrorMessage("選擇的日期太長，只能查詢7天的數據！");
        }
        else
        {
            DataQuery("1");
        }
    }
    public void  DataQuery(string tp)
    {
        DateTime timeStart = DateTime.Parse(txtFromDate.Text);
        DateTime timeEnd = DateTime.Parse(txtToDate.Text);
         DataTable dt=CT_Query.GetCTMessage(DBConnection, timeStart, timeEnd, tp);
         bindTable(dt);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();

        if (gvResult.HeaderRow != null && gvResult.HeaderRow.Cells.Count > 0)
            tu.ExportExcel(gvResult, "CT_TAT", Page);
        else
        {
            string str = "   alert( 'Please select one record! ' );";
            string script = "<script type='text/javascript' language='javascript'>" + str + "</script>";
            ClientScript.RegisterStartupScript(GetType(), "", script);

        }
    }
    protected void btnUploadList_ServerClick(object sender, System.EventArgs e)
    {
        string newName = DateTime.Now.Ticks.ToString() + FileUp.PostedFile.FileName.Substring(FileUp.PostedFile.FileName.LastIndexOf("."));
        string fullName = Server.MapPath("~") + "/" + newName;  
        string times = this.selectdate.Value;
        string line = "";
        try
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = FileUp.FileName;
            string extName = fileName.Substring(fileName.LastIndexOf("."));
 
       

                ShowInfoClear();
                FileUp.SaveAs(fullName);
                DataTable dt = ExcelManager.getExcelSheetData(fullName);
                //FileStream file = null;
                //file = new FileStream(fullName, FileMode.Open);
                //DataTable dt=ExcelManager.RenderDataTableFromExcel(file, 1, 0);

                   
                int coun = 0;
                string errormsg = "";
                if (dt.Rows.Count > 0)
                {
                    CT_Query.InsertShipNo(DBConnection, dt);
                }
                else
                {
                    showErrorMessage("您選擇的Excel無數據或格式不正確！");
                }
                DataQuery("2");
            #region
            /*
                foreach (DataRow dr in dt.Rows)
                {
                    coun += 1;
                    if ((!dr.IsNull(0) ) && (dr[0].ToString() != "" ))
                    {
                      
                    }
                    else
                    {
                        if (dr.IsNull(0) || dr[0].ToString() == "")
                        {
                            errormsg = "Excel第" + coun + "行，第1列為空值";
                        }
                        //else if (dr.IsNull(1) || dr[1].ToString() == "")
                        //{
                        //    errormsg = "Excel第" + coun + "行，第2列為空值";
                        //}
                        //else if (dr.IsNull(2) || dr[2].ToString() == "")
                        //{
                        //    errormsg = "Excel第" + coun + "行，第3列為空值";
                        //}
                    }

                }
                if (dataList.Count == 0)
                {
                    bindTable(null);
                    showErrorMessage("Excel can't be null...");

                }
                else if (dt.Rows.Count != dataList.Count)
                {
                    bindTable(null);
                    showErrorMessage(errormsg);
                }
                else
                {
                    var listRet = (from p in dataList
                                   group p by p.Model into g
                                   where g.Count() > 1
                                   select g.Key).ToList();

                    if (listRet.Count > 0)
                    {
                        bindTable(null);
                        string Models = "";
                        foreach (var items in listRet)
                        {
                            Models += items.ToString() + ",";
                        }
                        Models = Models.Substring(0, Models.Length - 1);
                        showErrorMessage("12碼機型重複 : " + Models);
                    }
                    else
                    {
                        IList<ProductPlanLog> ShowList = new List<ProductPlanLog>();
                        ShowList = iProductionPlan.UploadProductPlan(dataList);
                        bindTable(ShowList);
                    }
                }
            */
#endregion
           
        }

   
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            File.Delete(fullName);
           
            endWaitingCoverDiv();
           
        }
    }
    private void bindTable(DataTable dt)
    {
        gvResult.DataSource = dt;
        gvResult.DataBind();
        if (dt.Rows.Count > 0)
        {
            InitGridView();
        }
        else
        {
            showErrorMessage("當前查詢無任何數據！");
        }

        //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true);
    }
    private void beginWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "beginWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    private void ShowInfoClear()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowInfo(\"" + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
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
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void InitGridView()
    {

        gvResult.HeaderRow.Cells[0].Width = Unit.Percentage(12);
        gvResult.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gvResult.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gvResult.HeaderRow.Cells[3].Width = Unit.Percentage(15);
        gvResult.HeaderRow.Cells[4].Width = Unit.Percentage(5);
        gvResult.HeaderRow.Cells[5].Width = Unit.Percentage(10);
        gvResult.HeaderRow.Cells[6].Width = Unit.Percentage(10);
        gvResult.HeaderRow.Cells[7].Width = Unit.Percentage(5);
        gvResult.HeaderRow.Cells[8].Width = Unit.Percentage(5);

    }
}
