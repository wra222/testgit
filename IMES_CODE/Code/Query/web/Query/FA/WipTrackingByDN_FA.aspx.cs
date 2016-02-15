using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
using log4net;

using System.Data;
using System.IO;


public partial class WipTrackingByDN_FA : IMESQueryBasePage
{
    private const int gvResult_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private static IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
   // IMES.Query.Interface.QueryIntf.IModel Model = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IModel>(WebConstant.Model);
    List<string> StationList = new List<string>();
    public int rowCount = 8;

    static IPAK_WipTracking PAK_WipTracking = ServiceAgent.getInstance().GetObjectByName<IPAK_WipTracking>(WebConstant.PAK_WipTracking);
    
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
    //static DataTable dtStation;
    String DBConnection = "";
 //   static Dictionary<string, string> DicStationDescr;
    public string dbName;
    private static readonly ILog log = LogManager.GetLogger(typeof(WipTrackingByDN_FA));

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(WipTrackingByDN_FA)); 
        DBConnection = CmbDBType.ddlGetConnection();
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        dbName = Request["DBName"] ?? configDefaultDB;
        hidDBName.Value = dbName;
        //
    
        if (!this.IsPostBack)
        {
            Dictionary<string, string> dic = SetStationDic(DBConnection);

            if (dbName == configDefaultDB)
            { Session.Add("NB-Station", dic); }
            else
            { Session.Add("Docking-Station", dic); }
            hidConnection.Value = DBConnection;
       
            InitCondition();
     
            
        }
        
    }
    
  
    
    private DataTable getNullDataTableDetail(int j)
    {
        DataTable dt = initTableDetail();
        DataRow newRow = null;
        for (int i = 0; i < j; i++) //ProductID,CUSTSN,Model,DeliveryNo,Line,Station,StationDescr
        {
            newRow = dt.NewRow();
            newRow["ProductID"] = "";
            newRow["CUSTSN"] = "";
            newRow["Model"] = "";
            newRow["DeliveryNo"] = "";
            newRow["Line"] = "";
            newRow["Station"] = "";
            newRow["StationDescr"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;
    }
    private DataTable initTableDetail()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ProductID", Type.GetType("System.String"));
        retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("DeliveryNo", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("StationDescr", Type.GetType("System.String"));

        return retTable;
    }
    private void BindNoData()
    {
   
        //this.gvResult.DataSource = getNullDataTable(1);
        //this.gvResult.DataBind();
        //InitGridView();
        //gvStationDetail.Visible = false;
    }
 
   
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        errorMsg=  errorMsg.Replace("'", "");
        errorMsg= errorMsg.Replace(@"""","");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);

    }
    
    protected void btnExport_Click(object sender, EventArgs e)
    {
     
        string modelList = "";
        hidModelList.Value = hidModelList.Value.Replace("'", "");
        txtModel.Text = txtModel.Text.Replace("'", "");
        string[] modelArr = hidModelList.Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        if (modelArr.Length == 0)
           {modelList = "";}
         else
          { modelList = hidModelList.Value; }
        if (!string.IsNullOrEmpty(txtModel.Text))
            {modelList = txtModel.Text.Trim();}
        string lineList = "";
        foreach (ListItem item in lboxPdLine.Items)
        {
            if (item.Selected)
            { lineList = lineList + item.Value.Trim() + ","; }
        }

        DataTable dt2;
        try
        {
            dt2 = PAK_WipTracking.WipTrackingByDN_FA_Ex(DBConnection, txtShipDate.Text, modelList, lineList, hidProcess.Value, hidType.Value, dbName);
            List<int> lst = new List<int>();
            for (int i = 3; i < dt2.Rows.Count - 6; i++)
            {
                lst.Add(i);
            }
            string endNoDnStation = "POF";
            if (hidProcess.Value == "FA") { endNoDnStation = "67"; }
            MemoryStream ms = ExcelTool.DataTableToExcel_FA_WIP(dt2, "Data", 5,endNoDnStation, lst.ToArray());
       
            this.Response.ContentType = "application/download";
           this.Response.AddHeader("Content-Disposition", "attachment; filename=" + "WipTracking.xls");
           this.Response.Clear();
           this.Response.BinaryWrite(ms.GetBuffer());
           ms.Close();
           ms.Dispose();
        }
        catch (Exception exp)
        {
            showErrorMessage(exp.Message);
        }

    }

    private static Dictionary<string, string> SetStationDic(string connection)
    {
        DataTable dtStation = Station.GetStation(connection);
        Dictionary<string, string>  DicStationDescr = new Dictionary<string, string>();
        foreach (DataRow dr in dtStation.Rows)
        {
            DicStationDescr.Add(dr["Station"].ToString().Trim(), dr["Descr"].ToString().Trim());
        }
        return DicStationDescr;
    }
    
    
    private static string GetTipString(string descr)
    {
        string s = @"Tip('{0}',SHADOW,true,SHADOWCOLOR, '#dd99aa',
                            FONTSIZE, '12pt',SHADOWWIDTH, 2,OFFSETY,-40,OFFSETX,-25,FADEIN,300)";
        s = string.Format(s, descr);
        return s;
    }
   
    private void InitCondition()
    {
        GetLine();
        string customer = Master.userInfo.Customer;
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
       
    }
  
    private void GetLine()
    {
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");
        string customer = Master.userInfo.Customer;
        DataTable dtPdLine = QueryCommon.GetLine(Process, customer, true, DBConnection);
        if (dtPdLine.Rows.Count > 0)
        {
           
            foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
    }
   
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "EndRequestHandler();endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
     //   ScriptManager.RegisterStartupScript(this.UpdatePanel2, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    // HideWait();
    private void HideWait()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "HideWait();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this, ClientScript.GetType(), "HideWait", script, false);
        //   ScriptManager.RegisterStartupScript(this.UpdatePanel2, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }
    
    private DataTable initTable()
    {
       
        DataTable retTable = new DataTable();
        retTable.Columns.Add("DN", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Qty", Type.GetType("System.String"));
        foreach (string STN in StationList)
        {
            retTable.Columns.Add(STN, Type.GetType("System.String"));
        }

        return retTable;
    }
    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["DN"] = "";
            newRow["Model"] = "";
            newRow["Line"] = "";
            newRow["Qty"] = "";

            foreach (string STN in StationList)
            {
                newRow[STN] = "";
            }

            dt.Rows.Add(newRow);
        }
        return dt;
    }
   
    [System.Web.Services.WebMethod]

    public static int[] GetDNQty_WebMethod(string Connection, DateTime ShipDate, string DBName, string Model)
    {
        int[] dnQty = PAK_WipTracking.GetDNShipQty(Connection, ShipDate, DBName, "");
        return dnQty;
    }
    [System.Web.Services.WebMethod]

    public static string GetDetail_WebMethod(string Connection, string dn,string model,string line,string station)
    {
      //  e.Row.Cells[i].Attributes.Add("onclick", "SelectDetail('" + StationWC + "','" + Line + "','" + Model + "','" + DN + "')");
        //

        DataTable dt2 = PAK_WipTracking.GetDetailForWipTracking(Connection, model, line.Replace("&nbsp;", ""), station, dn.Replace("&nbsp;", ""));
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<table class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;width:98%;table-layout:fixed;'><thead><tr class='iMes_grid_HeaderRowGvExt'>");
   
        for (int i = 0; i < dt2.Columns.Count; i++)
        {
          
                sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='' >{0}</th>", dt2.Columns[i].ColumnName);
         
        }
        sb.AppendLine("</tr></thead><tbody>");
        for (int j = 0; j < dt2.Rows.Count; j++)
        {
           // string classname = "";
           // classname = "lesson" + dt2.Rows[j][1].ToString();
            sb.AppendLine("<tr class='iMes_grid_RowGvExt'>");
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                sb.AppendFormat("<td>{0}</td>", dt2.Rows[j][i].ToString());
            }
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        return sb.ToString();
        
    }
    


   
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetMain_WebMethod(string Connection, string shipDate, string model, string modelList, string line, string process, string type,string dbName)
    {
        IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
        
        Dictionary<string, string> DicStationDescr;
        string dicKey ;
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        if(dbName==configDefaultDB)
        {dicKey="NB-Station";}
        else
        {dicKey="Docking-Station";}

        if (HttpContext.Current.Session[dicKey] != null)
        { DicStationDescr = (Dictionary<string, string>)HttpContext.Current.Session[dicKey]; }
        else
        { DicStationDescr = SetStationDic(Connection); }

        int headerCount = 8;
        int[] dnQty;
        string inputModel="";
        if (model != "")
        { inputModel = model.Trim(); }
        else if (modelList.Trim() != "")
        {
            inputModel = modelList.Trim();
        }
     

        DataTable dt2;
        dt2 = PAK_WipTracking.WipTrackingByDN_FA_Ex(Connection, shipDate, inputModel, line, process, type, dbName, out dnQty);
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(@"<DIV  style='width: 98%; height: 300px;  overflow: auto;'>  
                                         <table id='mainTable'  value1='{0}' value2='{1}' 
                                        class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;table-layout:fixed;'>
                                       <tr class='iMes_grid_HeaderRowGvExt'>", dnQty[0].ToString(),dnQty[1].ToString());

        string endNoDnStation = "POF";
        if (process == "FA") { endNoDnStation = "67"; }
        int idxNoDnStation=0;
        int width;
        string stationDescr;
        for (int i = 0; i < dt2.Columns.Count; i++)
        {
            if (dt2.Columns[i].ColumnName == endNoDnStation) { idxNoDnStation = i; }
            width = 60;
            if (i == 1 || i == 0)
            { width = 150; }
            if (i <= headerCount - 1)
            {
                sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt'  style='width:{1}px'>{0}</th>", dt2.Columns[i].ColumnName, width);
            }
            else
            {
                if(!DicStationDescr.TryGetValue(dt2.Columns[i].ColumnName,out stationDescr))
                {
                  stationDescr="";
                }
         
                sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width:{1}px' onmouseover=\"ShowDescr('{2}')\" onmouseout='UnTip()'>{0}</th>", dt2.Columns[i].ColumnName, width, stationDescr);

            }
         

        }
        sb.AppendLine("</tr><tbody>");
        string modelTemp = "";
        int count = 0;

        bool IsSameModel = false;
        for (int j = 0; j < dt2.Rows.Count; j++)
        {
            string src = "";
            int cssIdx = 0;
            string data;
            string yellowClass = "";
            string rowCSS;
            IsSameModel = false;
            if (dt2.Rows[j][1].ToString().Trim() != modelTemp)
            {
                count += 1;
                modelTemp = dt2.Rows[j][1].ToString().Trim();
            }
            else
            {
                IsSameModel = true;
            }

            cssIdx = count % 2;
            rowCSS = "row" + cssIdx.ToString();
           // string tipCmd ;
            sb.AppendLine("<tr class='" + rowCSS + "' onclick='ChangeRowColor()'>");
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                if (i >= 5 && i < idxNoDnStation && IsSameModel)
                {
                    data = "";
                }
                else
                {
                    data = dt2.Rows[j][i].ToString().Trim().Replace("&nbsp;", ""); 
                }
                
                if (i == 4)
                {
                    if (data != "0")
                    { sb.AppendFormat("<td class='dn-pak' >{0}</td>", data); }
                    else
                    {
                        sb.AppendFormat("<td style='width:150px'>{0}</td>", data);
                    }
                
                }
                else if (i >= headerCount)
                {
                
                    if (data == "")
                    { yellowClass = ""; src = ""; }
                    else
                    {
                        yellowClass = "class='querycell'  ";
                        src = @" onclick='GetDetail()' onmouseover='ShowDetailDescr()' onmouseout='UnTip()'";
                    }
                  //  tipCmd = string.Format("'{0}','{1}','{2}'", dt2.Columns[i].ColumnName, dt2.Rows[j][0].ToString().Trim(), dt2.Rows[j][1].ToString().Trim());
                    sb.AppendFormat("<td {0}{1} >{2}</td>", yellowClass, src, data);

                }
                else
                {
                    sb.AppendFormat("<td style='width:150px'>{0}</td>", data);
                }
               
            }
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table></div> ");
        return sb.ToString();

    }
   
}
