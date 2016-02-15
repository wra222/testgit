using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.RBPC.Net.entity;
using com.inventec.RBPC.Net.intf;
using com.inventec.RBPC.Net.datamodel.intf;
using System.Text;
using System.Collections.Generic;
using IMES.Query.Interface.QueryIntf;
namespace com.inventec.iMESWEB
{

    /// <summary>
    /// Summary description for IMESBasePage
    /// </summary>
   
    public class IMESQueryBasePage : System.Web.UI.Page
    {
        IQueryCommon QueryCommon = ServiceAgent.getInstance().GetObjectByName<IQueryCommon>(WebConstant.QueryCommon);
        IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
        
        public static string PAKStation;
        public static string FAStation;
        public static string PAKOnlineStation;
        public static string FAOnlineStation;
        public static string YieldStation;

        public static string PCAInputStation;

        public static string Status87;

        public static string MVS_SUM ;
        public static string ITCND_SUM;
        public static string COA_SUM;
        public static string EfficiencyStation;


        
        private string connectionString = "";
        public IMESQueryBasePage()
        {
          
  
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (this.Master != null)
            {
                RegisterTxtBox(this.Master);
                RegisterMPGet(this.Master);
            }
          
                SetConnectionString();
                IniStationValue();
         
        
        }
        private void SetConnectionString()
        {
            DBInfo objDbInfo = iConfigDB.GetDBInfo();
            //  string[] dbList = objDbInfo.OnLineDBList;
            string[] dbList = objDbInfo.OnLineDBList;
            string configDefaultDB = iConfigDB.GetOnlineDefaultDBName();
            string defaultSelectDB =this.Page.Request["DBName"] != null ? Request["DBName"].ToString().Trim() :configDefaultDB;
            string defaultSelectDBType = this.Page.Request["DBType"] != null ? Request["DBType"].ToString().Trim() : "OnlineDB";
            if (defaultSelectDB !=configDefaultDB)
            {
                foreach (string s in dbList)
                {
                    if (s.ToUpper() == defaultSelectDB.ToUpper())
                    {
                        defaultSelectDB = s;
                        break;
                    }
                }
            }
            if (defaultSelectDBType == "HistoryDB") {
                connectionString = string.Format(objDbInfo.HistoryConnectionString, defaultSelectDB);
            } else {
                connectionString = string.Format(objDbInfo.OnLineConnectionString, defaultSelectDB);
            }
            


        }
       
        private void IniStationValue()
        {
            //string err = "";
            List<string> lst = new List<string> { "PAKStation", "FAStation", "PAKOnlineStation", "FAOnlineStation", "YieldStation", "Status87", "PCAInputStation", "MVS_SUM", "ITCND_SUM", "COA_SUM", "EffStation" };
            DataTable dt = GetSysSetting(lst, connectionString);
            PAKStation = FindValue(dt, "PAKStation");
            FAStation = FindValue(dt, "FAStation");
            PAKOnlineStation = FindValue(dt, "PAKOnlineStation");
            FAOnlineStation = FindValue(dt, "FAOnlineStation");

            YieldStation = FindValue(dt, "YieldStation");
            PCAInputStation = FindValue(dt, "PCAInputStation");
            
            Status87 = FindValue(dt, "Status87");

            MVS_SUM = FindValue(dt, "MVS_SUM"); ;
            ITCND_SUM = FindValue(dt, "ITCND_SUM");
            COA_SUM = FindValue(dt, "COA_SUM"); ;
            EfficiencyStation = FindValue(dt, "EffStation");

            
            //if (PAKStation == "") { err = "Please define " + "PAK station list!"; }
            //if (FAStation == "") { err = err + "\r\n" + "Please define " + "FA station list!"; }
            //if (PAKOnlineStation == "") { err = "Please define " + "PAKOnlineStation list!"; }
            //if (FAOnlineStation == "") { err = err + "\r\n" + "Please define " + "FAOnlineStation list!"; }
            //if (YieldStation == "") { err = err + "\r\n" + "Please define " + "YieldStation"; }

            //if (err != "")
            //{ showErrorMessage(err, this); }
        }
        private string FindValue(DataTable dt, string name)
        { 
          DataRow[] drList=  dt.Select(string.Format("name = '{0}'", name));
          if (drList.Length == 0)
          { return ""; }
          else
          { return drList[0]["Value"].ToString(); }
        }

        public DataTable GetSysSetting(List<string> lstName, string DBConnection)
        {
          
            DataTable dt = QueryCommon.GetSysSetting(lstName, DBConnection);
            return dt;
        }
        public DataTable GetSysSetting2(List<string> lstName, string DBName)
        {
            IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
            DBInfo  objDbInfo=iConfigDB.GetDBInfo();
            string connString = string.Format(objDbInfo.OnLineConnectionString, DBName);
            DataTable dt = QueryCommon.GetSysSetting(lstName, connString);
            return dt;
        }
        public bool ValidatePrivilege(string pageSymbol)
        {
           
            return false;
        }
        public void showErrorMessage(string errorMsg,Control ctr)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine("<script language='javascript'>");
            scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
            scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
            scriptBuilder.AppendLine("</script>");
            ScriptManager.RegisterStartupScript(ctr, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        }
        public void beginWaitingCoverDiv(Control ctr)
        {
            String script = "<script language='javascript'>" + "\r\n" +
                "beginWaitingCoverDiv();" + "\r\n" +
                "</script>";
            ScriptManager.RegisterStartupScript(ctr, ClientScript.GetType(), "beginWaitingCoverDiv", script, false);
        }
        public void endWaitingCoverDiv(Control ctr)
        {
            String script = "<script language='javascript'>" + "\r\n" +
                "endWaitingCoverDiv();" + "\r\n" +
                "</script>";
            ScriptManager.RegisterStartupScript(ctr, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
        }
        public DataTable getNullDataTable(string colList)
        {
            DataTable dt = initTable(colList);
            string[] colArr = colList.Split(',');
            DataRow newRow = null;
            newRow = dt.NewRow();
            foreach (string col in colArr)
            {
                newRow[col] = "";
            
            }
            dt.Rows.Add(newRow);
          
            return dt;
        }
        public DataTable initTable(string colList)
        {
            DataTable retTable = new DataTable();
            string[] colArr = colList.Split(',');
            foreach (string col in colArr)
            {
                retTable.Columns.Add(col, Type.GetType("System.String"));
            }
            return retTable;
        }
        private  void searchContentPlaceHolder(Control ctrl, List<string> lst)
        {
            if (ctrl is ContentPlaceHolder)
                lst.Add(ctrl.ClientID);
            else if (ctrl.HasControls())
                foreach (Control c in ctrl.Controls)
                    searchContentPlaceHolder(c, lst);
        }
        public void RegisterTxtBox(MasterPage mp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                            <script type=""text/javascript"">

                          window.onload = function() {
                           $('input[type=text][isvalid!=false]').keydown(function(event) {
        // Allow: backspace, delete, tab, escape, and enter
                                if ( event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || 
                                     // Allow: Ctrl+A
                                    (event.keyCode == 65 && event.ctrlKey === true) || 
                                     // Allow: home, end, left, right
                                    (event.keyCode >= 35 && event.keyCode <= 39)) {
                                         // let it happen, don't do anything
                                         return;
                                }
                                else {
                                    // Ensure that it is a number and stop the keypress
                                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105 ) && (event.keyCode < 65|| event.keyCode > 90)) {
                                        event.preventDefault(); 
                                    }   
                                }
                            });}; 
                      </script>");
            Literal js = new Literal();
            js.Text = sb.ToString();

            mp.Page.Header.Controls.AddAt(0, js);
        
        }
        public  void RegisterMPGet(MasterPage mp)
        {
            List<string> lstCph = new List<string>();
            lstCph.Add(mp.ClientID);
            searchContentPlaceHolder(mp.Page.Form, lstCph);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                            <script type=""text/javascript"">
                            function ConvertID(objId) {
                                var inpObj = document.getElementById(objId);
                            ");
            foreach (string cphId in lstCph)
            {
                sb.AppendFormat(
                    "   if (!inpObj) inp = \"{0}_\" + objId;\n",
                    cphId);
            }
            sb.Append("return inp;\n}\n</script>");
            Literal js = new Literal();
            js.Text = sb.ToString();

            mp.Page.Header.Controls.AddAt(0, js);
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        public void BindNoData(DataTable dt,GridView gr)
        {
            string ColumnList = "";
            foreach (DataColumn dc in dt.Columns)
            {
                ColumnList = ColumnList + dc.ColumnName + ",";

            }
            ColumnList = ColumnList.Substring(0, ColumnList.Length - 1);

            gr.DataSource = getNullDataTable(ColumnList);
            gr.DataBind();

        }
        public void EnableBtnExcel2(Control control, bool enable, string ExcelControlID)
        {
            string tmp = "";
            if (enable)
            {
                tmp = "document.getElementById('" + ExcelControlID + "').style.display = 'inline';";
            }
            else
            {
                tmp = "document.getElementById('" + ExcelControlID + "').style.display = 'none';";

            }

            String script = "<script language='javascript'>" + "\r\n" +
              tmp + "\r\n" + "</script>";
            ScriptManager.RegisterStartupScript(control, typeof(System.Object), "EnableExcel2", script, false);

        }
        public void EnableBtnExcel(Control control, bool enable,string ExcelControlID)
        {
            string tmp = "";
            if (enable)
            {
                tmp = "document.getElementById('" + ExcelControlID+"').style.display = 'inline';";
            }
            else
            {
                tmp = "document.getElementById('" + ExcelControlID + "').style.display = 'none';";
            
            }

            String script = "<script language='javascript'>" + "\r\n" +
              tmp + "\r\n" + "</script>";
            ScriptManager.RegisterStartupScript(control, typeof(System.Object), ExcelControlID, script, false);

        }
    }
}