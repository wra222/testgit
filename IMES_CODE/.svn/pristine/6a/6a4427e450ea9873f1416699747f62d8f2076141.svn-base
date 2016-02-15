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


public partial class WipTrackingByDN_PAK : IMESQueryBasePage
{
   
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
    IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
    
  //  public string dbConnection = "";
    public string dbName;
    private static readonly ILog log = LogManager.GetLogger(typeof(WipTrackingByDN_PAK));

    protected void Page_Load(object sender, EventArgs e)
    {
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();

        AjaxPro.Utility.RegisterTypeForAjax(typeof(WipTrackingByDN_PAK)); 
        hidConnection.Value = CmbDBType.ddlGetConnection(); ;
        hidDBName.Value = Request["DBName"] ?? configDefaultDB;
        hidDockingDB.Value = iConfigDB.CheckDockingDB(hidDBName.Value) ? "Y" : "N";
        hidReqProcess.Value = Request["Process"] ?? "PAK";
        hidExcelPath.Value = MapPath(@"~\TmpExcel\");
   
        if (!this.IsPostBack)
        {
            Dictionary<string, string> dic = SetStationDic(hidConnection.Value);
            if (dbName == configDefaultDB)
            { Session.Add("NB-Station", dic); }
            else
            { Session.Add("Docking-Station", dic); }
           
            InitCondition();
         }
        
    }

    public object GetDefaultDB(object DefaultDB)
    {
        DefaultDB = iConfigDB.GetOnlineDefaultDBName();
        return DefaultDB;
    }
    
 
   
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        errorMsg=  errorMsg.Replace("'", "");
        errorMsg= errorMsg.Replace(@"""","");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        //scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
      //  ScriptManager.RegisterStartupScript(this, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
  
   

   
    private static Dictionary<string, string> SetStationDic(string connection)
    {
        IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
        DataTable dtStation = Station.GetStation(connection);
        Dictionary<string, string> DicStationDescr = new Dictionary<string, string>();
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
        txtShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
       
    }
    
    private void GetLine()
    {
        List<string> Process = new List<string>();
        Process.Add("FA");
        Process.Add("PAK");
        string customer = Master.userInfo.Customer;
        DataTable dtPdLine = QueryCommon.GetLine(Process, customer, true, hidConnection.Value);
        if (dtPdLine.Rows.Count > 0)
        {
           foreach (DataRow dr in dtPdLine.Rows)
            {
                lboxPdLine.Items.Add(new ListItem(dr["Line"].ToString().Trim(), dr["Line"].ToString().Trim()));
            }
        }
    }
       
    [System.Web.Services.WebMethod]

    public static string GetDetail_WebMethod(string Connection, string dn,string model,string line,string station)
    {
      //  e.Row.Cells[i].Attributes.Add("onclick", "SelectDetail('" + StationWC + "','" + Line + "','" + Model + "','" + DN + "')");
        //
        IPAK_WipTracking PAK_WipTracking = ServiceAgent.getInstance().GetObjectByName<IPAK_WipTracking>(WebConstant.PAK_WipTracking);
        DataTable dt2 = PAK_WipTracking.GetDetailForWipTracking(Connection, model, line.Replace("&nbsp;", ""), station, dn.Replace("&nbsp;", ""));
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(@"<DIV id='DIVdetaulTable' style='width: 98%; height: 200px;  overflow: auto;'> 
                                <table class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;width:98%; table-layout:fixed;'><thead><tr class='iMes_grid_HeaderRowGvExt'>");
   
        for (int i = 0; i < dt2.Columns.Count; i++)
        {
            if (i == 4 || i == 5) //style='width:150px'
            { sb.AppendFormat("<th  class='iMes_grid_HeaderRowGvExt' style='width:70px'>{0}</td>", dt2.Columns[i].ColumnName); }
            else if (i == 6)
            { sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width:250px' >{0}</th>", dt2.Columns[i].ColumnName); }
            else
            { sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='' >{0}</th>", dt2.Columns[i].ColumnName); }  
         
        }
        sb.AppendLine("</tr></thead><tbody>");
        for (int j = 0; j < dt2.Rows.Count; j++)
        {
            //style='background-color:#EBDDE2'
           // sb.AppendLine("<tr class='iMes_grid_RowGvExt'>");
            sb.AppendLine("<tr style='background-color:#4C7D7E;  color: White ; font-weight:bold'>");
            
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
               sb.AppendFormat("<td>{0}</td>", dt2.Rows[j][i].ToString()); 
        
            }
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table></div>");
        return sb.ToString();
        
    }
    [System.Web.Services.WebMethod]
    public static string DownExcel_WebMethod(string Connection, string shipDate, string model, string modelList, string line, string process, string type, string dbName,string path,string reqProcess,string prdType)
    {
       
        string inputModel = "";
        if (model != "")
        { inputModel = model.Trim(); }
        else if (modelList.Trim() != "")
        {
            inputModel = modelList.Trim();
        }

        IPAK_WipTracking PAK_WipTracking = ServiceAgent.getInstance().GetObjectByName<IPAK_WipTracking>(WebConstant.PAK_WipTracking);
        DataTable dt2;
        if (reqProcess == "PAK")
        {
            dt2 = PAK_WipTracking.WipTrackingByDN_PAK_Ex(Connection, shipDate, inputModel, line, process, type, prdType,dbName);
        }
        else
        {
            dt2 = PAK_WipTracking.WipTrackingByDN_FA_Ex(Connection, shipDate, modelList, line, process, type, dbName);
        }
        List<int> lst = new List<int>();
        for (int i = 3; i < dt2.Rows.Count - 6; i++)
        {
            lst.Add(i);
        }
        string fileID ;
        if (reqProcess == "PAK")
        {
            fileID = ExcelTool.DataTableToExcelSaveLocal(dt2, "Data", path, lst.ToArray());
        }
        else
        {
            string endNoDnStation = "POF";
            if (process == "FA") { endNoDnStation = "67"; }
            fileID = ExcelTool.DataTableToExcelSaveLocal_FA(dt2, "Data", 5, endNoDnStation, path, lst.ToArray());
        
        }
        return fileID;
    
    }

   
   
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetMain_WebMethod(string Connection, string shipDate, string model, string modelList, string line, string process, string type, string dbName,string reqProcess,string prdType)
    {
        model = model.Trim().ToUpper();
        modelList = modelList.Trim().ToUpper();
        if (reqProcess == "PAK" || prdType == "Tablet" || prdType == "RCTO" || prdType == "ThinClient")
        {
            return GetHtmlTableString_PAK(Connection, shipDate, model, modelList, line, process, type, dbName, prdType);
        }
        else
        {
            return GetHtmlTableString_FA(Connection, shipDate, model, modelList, line, process, type, dbName, prdType);
                 
        }
        
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static int[] GetDNQty_WebMethod(string Connection, string ShipDate,string model, string modelList, string DBName,string PrdType)
    {
  //      DateTime dtimeBegin = DateTime.Now;
        log.Debug("******** Begin GetDNQty_WebMethod(WipTrcaking_PAK) ********" + "(" + DBName+")");

        IPAK_WipTracking PAK_WipTracking = ServiceAgent.getInstance().GetObjectByName<IPAK_WipTracking>(WebConstant.PAK_WipTracking);
        IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
     //   ShipDate = ShipDate + " 00:00:00.000";
        DateTime shipD = DateTime.ParseExact(ShipDate.Trim(), "yyyy-MM-dd", null);
        string inputModel = "";
        if (model != "")
        { inputModel = model.Trim(); }
        else if (modelList.Trim() != "")
        {
            inputModel = modelList.Trim();
        }
        inputModel = inputModel.ToUpper();
        int[] dnQty ;
       // if(string.Equals(DBName, "HPDocking", StringComparison.CurrentCultureIgnoreCase))
        if (iConfigDB.CheckDockingDB(DBName))
      
        { dnQty = PAK_WipTracking.GetDNShipQty_Docking(Connection, shipD, inputModel); }
        else
        { dnQty = PAK_WipTracking.GetDNShipQty(Connection, shipD, inputModel, PrdType); }
        log.Debug("******** ShipDate :" + shipD.ToString());

        log.Debug("******** End GetDNQty_WebMethod(WipTrcaking_PAK) ********" + "dnQty1=" + dnQty[0].ToString());
     
        return dnQty;
    }
  
    private static void SetTableHeaderString(DataTable dt, int headerCount,StringBuilder sb,string connection,string dbName)
    {
    //    StringBuilder sb = new StringBuilder();
        Dictionary<string, string> DicStationDescr = GetSationDic(connection, dbName);
        sb.Append(@"<DIV id='DIVmainTable' style='width: 98%; height: 370px;  overflow: auto;'>  
                                         <table id='mainTable'  
                                        class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;table-layout:fixed;'>
                                       <tr class='iMes_grid_HeaderRowGvExt'>");

        string stationDescr;
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            if (i <= headerCount - 1)
            {
                if (i == 1 || i == 0)//DN & Model
                { sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt t5'>{0}</th>", dt.Columns[i].ColumnName); }
                else
                { sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt t6'>{0}</th>", dt.Columns[i].ColumnName); }

            }
            else
            {

                if (!DicStationDescr.TryGetValue(dt.Columns[i].ColumnName, out stationDescr))
                { stationDescr = ""; }
                sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt t6' onmouseover=\"SD('{0}')\" onmouseout='UT()'>{1}</th>", stationDescr, dt.Columns[i].ColumnName);
            }
        }
        sb.AppendLine("</tr><tbody>");
       
    }
    private static Dictionary<string, string> GetSationDic(string connection, string dbName)
    {
        Dictionary<string, string> DicStationDescr = new Dictionary<string, string>();
        IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
        string dicKey;
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        if (dbName == configDefaultDB)
        { dicKey = "NB-Station"; }
        else
        { dicKey = "Docking-Station"; }

        if (HttpContext.Current.Session[dicKey] != null)
        { DicStationDescr = (Dictionary<string, string>)HttpContext.Current.Session[dicKey]; }
        else
        { DicStationDescr = SetStationDic(connection); }
        return DicStationDescr;
    }
    public static string GetHtmlTableString_PAK(string Connection, string shipDate, string model, string modelList, string line, string process, string type, string dbName,string prdType)
    {
        DateTime dtimeBegin = DateTime.Now;
        log.Debug("******** Begin btnQuery_Click(WipTrcaking_PAK) ********");
        int headerCount = 6;
        bool IsPAK = false;
        if (process != "FA")
        { IsPAK = true; }
        else
        { headerCount = headerCount - 2; }

        int[] dnQty;
        string inputModel = "";
        if (model != "")
        { inputModel = model.Trim(); }
        else if (modelList.Trim() != "")
        {
            inputModel = modelList.Trim();
        }
        log.Debug("   ***** Begin WipTrackingByDN_PAK_Ex(WipTrcaking_PAK) *****");
   
        IPAK_WipTracking PAK_WipTracking = ServiceAgent.getInstance().GetObjectByName<IPAK_WipTracking>(WebConstant.PAK_WipTracking);

        DataTable dt2 = PAK_WipTracking.WipTrackingByDN_PAK_Ex(Connection, shipDate, inputModel, line, process, type,prdType, dbName);
        log.Debug("   ***** End WipTrackingByDN_PAK_Ex(WipTrcaking_PAK) *****");
        StringBuilder sb = new StringBuilder();
        SetTableHeaderString(dt2, headerCount, sb,Connection,dbName);
        string modelTemp = "";
        int count = 0;
        for (int j = 0; j < dt2.Rows.Count; j++)
        {
            string src = "";
            int cssIdx = 0;
            string data;
            string yellowClass = "";
            string rowCSS;
            if (dt2.Rows[j][1].ToString().Trim() != modelTemp)
            {
                count += 1;
                modelTemp = dt2.Rows[j][1].ToString().Trim();
            }
            cssIdx = count % 2;
            rowCSS = "row" + cssIdx.ToString();
            sb.Append("<tr class='" + rowCSS + "' onclick='CR(this)'>");
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                data = dt2.Rows[j][i].ToString().Trim().Replace("&nbsp;", "");
                if (i == 4 && IsPAK)
                {
                    if (data != "0")
                    { sb.AppendFormat("<td class='dn-pak' >{0}</td>", data); }
                    else
                    {
                        sb.AppendFormat("<td>{0}</td>", data);
                    }

                }
                else if (i >= headerCount)
                {
                    if (data == "")
                    { yellowClass = ""; src = ""; }
                    else
                    {
                        yellowClass = " class='querycell'  ";
                        src = @" onclick='GetDetail()' onmouseover='SDS()' onmouseout='UT()'";
                    }
                    sb.AppendFormat("<td{0}{1}>{2}</td>", yellowClass, src, data);
                }
                else
                {
                    sb.AppendFormat("<td>{0}</td>", data);
                }

            }
            sb.Append("</tr>");
        }
        sb.Append("</table></div> ");
        TimeSpan span = DateTime.Now.Subtract(dtimeBegin);
        log.Debug("End btnQuery_Click(WipTrcaking_PAK) ; Total : " + span.Seconds);
        return sb.ToString(); 
      
    }
    public static string GetHtmlTableString_FA(string Connection, string shipDate, string model, string modelList, string line, string process, string type, string dbName,string prdType)
    {

        Dictionary<string, string> DicStationDescr;
        IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
        string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
        string dicKey;
        if (dbName == configDefaultDB)
        { dicKey = "NB-Station"; }
        else
        { dicKey = "Docking-Station"; }

        if (HttpContext.Current.Session[dicKey] != null)
        { DicStationDescr = (Dictionary<string, string>)HttpContext.Current.Session[dicKey]; }
        else
        { DicStationDescr = SetStationDic(Connection); }

        int headerCount = 8;
        int[] dnQty;
        string inputModel = "";
        if (model != "")
        { inputModel = model.Trim(); }
        else if (modelList.Trim() != "")
        {
            inputModel = modelList.Trim();
        }

   
        IPAK_WipTracking PAK_WipTracking = ServiceAgent.getInstance().GetObjectByName<IPAK_WipTracking>(WebConstant.PAK_WipTracking);

        DataTable dt2;
        try
        {
            if (iConfigDB.CheckDockingDB(dbName))
            {
                dt2 = PAK_WipTracking.WipTrackingByDN_FA_Ex(Connection, shipDate, inputModel, line, process, type, dbName, out dnQty);

            }
            else
            {
                dt2 = PAK_WipTracking.WipTrackingByDN_FA_Ex(Connection, shipDate, inputModel, line, process, type, dbName, out dnQty);
            
            }
            

        }
        catch (Exception e)
        {
            throw new Exception("Please refresh");
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(@"<DIV id='DIVmainTable' style='width: 98%; height: 370px;  overflow: auto;'>  
                                         <table id='mainTable'  value1='{0}' value2='{1}' 
                                        class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;table-layout:fixed;'>
                                       <tr class='iMes_grid_HeaderRowGvExt'>", dnQty[0].ToString(), dnQty[1].ToString());

        string endNoDnStation = "POF";
        if (process == "FA") { endNoDnStation = "67"; }
        int idxNoDnStation = 0;
        int width;
        string stationDescr;
        for (int i = 0; i < dt2.Columns.Count; i++)
        {
            if (dt2.Columns[i].ColumnName == endNoDnStation) { idxNoDnStation = i; }

            if (i <= headerCount - 1)
            {
                if (i == 1 || i == 0)//DN & Model
                { sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt t5'>{0}</th>", dt2.Columns[i].ColumnName); }
                else
                { sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt t6'>{0}</th>", dt2.Columns[i].ColumnName); }

            }
            else
            {

                if (!DicStationDescr.TryGetValue(dt2.Columns[i].ColumnName, out stationDescr))
                { stationDescr = ""; }
                sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt t6' onmouseover=\"SD('{0}')\" onmouseout='UT()'>{1}</th>", stationDescr, dt2.Columns[i].ColumnName);
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
            sb.AppendLine("<tr class='" + rowCSS + "' onclick='CR(this)'>");
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
                        src = @" onclick='GetDetail()' onmouseover='SDS()' onmouseout='UnTip()'";
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
