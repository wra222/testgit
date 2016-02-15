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
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;





public partial class Query_FA_FALocQuery :  IMESQueryBasePage
{
    IFA_LocQuery FALocQuery = ServiceAgent.getInstance().GetObjectByName<IFA_LocQuery>(WebConstant.IFA_LocQuery);
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
    //IFA_MPInput MPInput = ServiceAgent.getInstance().GetObjectByName<IFA_MPInput>(WebConstant.MPInput);
   // IMES.Query.Interface.QueryIntf.IModel Model = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IModel>(WebConstant.Model);
   // IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IPdLine PdLine = ServiceAgent.getInstance().GetObjectByName<IPdLine>(WebConstant.PdLine);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    string customer = "";
    String DBConnection = "";
    public static string _txtFromdate = "";
    private static string _txtTodate = "";
    private static string _station = "";
    private static string _type = "";
    private static string _pdline = "";
    private static string _list = "";
    string xx = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        customer = "HP";
        DBConnection = CmbDBType.ddlGetConnection();
        if (!this.IsPostBack)
        {
            InitPage();
            
        }
    }
    private void InitPage()
    {


        if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20)
        {
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
        }
        else if (DateTime.Now.Hour >= 20)
        {
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd 20:30");
            txtToDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 08:00");
        }
        else if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 8)
        {
            txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 20:30");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd 08:00");
        }
        GetPdline();
        BindNoData();
    }

    protected void GetPdline()
    {

        List<string> Process = new List<string>();
        Process.Add("FA");
        DataTable dtPdLine = QueryCommon.GetLine(Process, customer, true, DBConnection);
        lboxPdLine.Items.Clear();
        if (dtPdLine.Rows.Count > 0)
        {
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim() + " " + dr["Descr"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
    }
  
   
    protected void btnQuery_Click(object sender, EventArgs e)
    { 
   
    }
    
     protected void btnChangeLine_Click(object sender, EventArgs e)
     {
        
         GetPdline();
     }
     protected void btnExport_Click(object sender, EventArgs e)
    {
        ToolUtility tu = new ToolUtility();
        tu.ExportExcel(gvResult, Page.Title, Page);
    }
      protected void btnDetailExport_Click(object sender, EventArgs e)
    { 
    
    }
      [System.Web.Services.WebMethod(EnableSession = true)]
      public static void GetMain_WebMethod(string txtFromdate, string txtTodate, string station, string type, string pdline,string list)
      {
          _txtFromdate = txtFromdate;
          _txtTodate = txtTodate;
          _station = station;
          _type = type;
          _pdline = pdline;
          _list = list;
          
      }

      protected void btqry_Click(object sender, EventArgs e)
      {

          try
          {
              IList<string> Model = new List<string>();

              hidModelList.Value = hidModelList.Value.Replace("'", "");
              hfModel.Value = hfModel.Value.Replace("'", "");
              if (hfModel.Value != "")
              {
                  Model.Add(hfModel.Value.Trim());
              }
              else
              {
                  Model = hidModelList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
              }
              DataTable dt = FALocQuery.GetLocRinDown(DBConnection, _station, _type, _txtFromdate, _txtTodate, _pdline, _list,Model);
              if (dt.Rows.Count > 0)
              {


                  this.gvResult.DataSource = dt;
                  this.gvResult.DataBind();

              }
              else
              {

                  showErrorMessage("Not Found Any Information!!");
                  BindNoData();
              }
          }
          catch (Exception ex)
          {
              showErrorMessage(ex.Message);
              BindNoData();
          }
          finally
          {
              endWaitingCoverDiv(this);
          }
      }
      private DataTable getNullDataTable(int j)
      {
          DataTable dt = initTable();
          DataRow newRow = null;
          for (int i = 0; i < j; i++)
          {
              newRow = dt.NewRow();
              dt.Rows.Add(newRow);
          }
          return dt;
      }
      private void BindNoData()
      {
          this.gvResult.DataSource = getNullDataTable(1);
          this.gvResult.DataBind();
          InitGridView();
          EnableBtnExcel(this, false, btnExport.ClientID);
      }
    
      private void InitGridView()
      {
          int i = 100;
          int j = 50;
          int k = 30;
          gvResult.HeaderRow.Cells[0].Width = Unit.Pixel(k);
          gvResult.HeaderRow.Cells[1].Width = Unit.Pixel(k);
          gvResult.HeaderRow.Cells[2].Width = Unit.Pixel(i);
          gvResult.HeaderRow.Cells[3].Width = Unit.Pixel(i);
          gvResult.HeaderRow.Cells[4].Width = Unit.Pixel(i);
          gvResult.HeaderRow.Cells[5].Width = Unit.Pixel(k);
          gvResult.HeaderRow.Cells[6].Width = Unit.Pixel(k);
          gvResult.HeaderRow.Cells[7].Width = Unit.Pixel(i);
          gvResult.HeaderRow.Cells[8].Width = Unit.Pixel(i);
          gvResult.HeaderRow.Cells[9].Width = Unit.Pixel(i);
          gvResult.HeaderRow.Cells[10].Width = Unit.Pixel(i);
          gvResult.HeaderRow.Cells[11].Width = Unit.Pixel(i);
       
      }
      private DataTable initTable()
      {
          DataTable retTable = new DataTable();
          retTable.Columns.Add("ProductID", Type.GetType("System.String"));
          retTable.Columns.Add("LOC", Type.GetType("System.String"));
          retTable.Columns.Add("InLOCTime", Type.GetType("System.String"));
           retTable.Columns.Add("Model", Type.GetType("System.String"));
          retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
          retTable.Columns.Add("DownLoadLOC", Type.GetType("System.String"));
          retTable.Columns.Add("RunInLOC", Type.GetType("System.String"));
          retTable.Columns.Add("DownLoadPassTime", Type.GetType("System.String"));
          retTable.Columns.Add("MvsPass", Type.GetType("System.String"));
          retTable.Columns.Add("RunInPass", Type.GetType("System.String"));
          retTable.Columns.Add("PostPass", Type.GetType("System.String"));
           retTable.Columns.Add("ItcndetPass", Type.GetType("System.String"));
          return retTable;
        
          
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
}
