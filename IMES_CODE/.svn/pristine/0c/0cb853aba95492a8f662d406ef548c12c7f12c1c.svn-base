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
using com.inventec.portal.databaseutil;
using com.inventec.system.exception;
using System.Collections.Generic;
using com.inventec.system;
//modify ITC-1370-0001

/// <summary>
///DashboardCommon 的摘要说明
/// </summary>
/// 

namespace com.inventec.portal.dashboard.common
{
    public class CurrentTimeInfo
    {
        private String timeShowString;

        public String TimeShowString
        {
            get { return timeShowString; }
            set { timeShowString = value; }
        }
        private String second;

        public String Second
        {
            get { return second; }
            set { second = value; }
        }

    }

    public class DashboardCommon
    {
        //!!! need check tmp use
        //public const String FamilySeries = "(Select distinct Part.Descr as Family, PartInfo.InfoValue as Series from Part Inner join PartInfo on Part.PartNo = PartInfo.PartNo Where PartInfo.InfoType = 'MB' OR PartInfo.InfoType = 'VB' OR PartInfo.InfoType = 'SB') AS FamilySeries ";
        //!!! need 
        public const String FamilySeries = "(Select distinct Part.Descr as Family, PartInfo.InfoValue as Series from Part Inner join PartInfo on Part.PartNo = PartInfo.PartNo where Part.BomNodeType = 'MB' and PartInfo.InfoType = 'MB') AS FamilySeries ";                                      

        public DashboardCommon()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public static String GetStageNameByType(String type)
        {
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT [Stage] FROM [Dashboard_Stage] WHERE [Type]=@param1";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.DataType = "int";
            param1.ParamName = "@param1";
            param1.ParamValue = GeneralUtil.Null2String(type);
            paramList.Add(param1);
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            if (result.Rows.Count <= 0)
            {
                return "";
            }
            String stage = GeneralUtil.Null2String(result.Rows[0][0]);
            return stage;
        }

        public static DataTable GetDashboardWindowNameList()
        {
            //genutil.TLog<TabDef>.DebugInfo("GetPublishListInIntegrationFile", dashboardTab);
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT [ID] as id,[WindowName] as [name],'DASHBOARD_FILE' as [type],'0' as haschild,' ' as nodeuuid FROM [Dashboard_Window] order by [WindowName]";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            return result;
        }

        [AjaxPro.AjaxMethod]
        public static String GetCurrentTimeInfoString()
        {
            CurrentTimeInfo result = GetCurrentTimeInfo();
            String stringResult = JsonHelper<CurrentTimeInfo>.WriteObject(result);
            return stringResult;
        }

        [AjaxPro.AjaxMethod]
        public static CurrentTimeInfo GetCurrentTimeInfo()
        {
            CurrentTimeInfo current = new CurrentTimeInfo();
            DateTime NowTime = DateTime.Now;
            current.Second = NowTime.ToString("fff"); //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            current.TimeShowString = NowTime.ToString("yyyy-MM-dd HH:mm");
            return current;

        }

        //显示列表
        [AjaxPro.AjaxMethod]
        public static DataTable GetDashboardWindowList()
        {
            //genutil.TLog<TabDef>.DebugInfo("GetPublishListInIntegrationFile", dashboardTab);
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT [ID],WindowName AS [Window_Name],DisplayName AS [Display_Name],CONVERT(CHAR(10),[Cdt],21) AS [Create_Time],CONVERT(CHAR(10),[Udt] ,21) AS [Update_Time],[Editor] FROM [Dashboard_Window] ORDER BY [WindowName]";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);

            return result;
        }

        [AjaxPro.AjaxMethod]
        public static DataTable GetDashboardWindowListEmpty()
        {
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT top 0 [ID],WindowName AS [Window_Name],DisplayName AS [Display_Name],CONVERT(CHAR(10),[Cdt],21) AS [Create_Time],CONVERT(CHAR(10),[Udt] ,21) AS [Update_Time],[Editor] FROM [Dashboard_Window] ORDER BY [WindowName]";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);

            return result;

        }

        /// <summary>
        /// 返回TYPE 
        /// </summary>
        /// <param name="windowId"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod]
        public static int GetStageType(String windowId) 
        {
            //genutil.TLog<TabDef>.DebugInfo("GetPublishListInIntegrationFile", dashboardTab);
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT [Type] FROM [Dashboard_Stage] INNER JOIN [Dashboard_Stage_Target] ON [Dashboard_Stage].Stage=Dashboard_Stage_Target.Stage WHERE [Dashboard_Stage_Target].[WindowID]=@param1";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.DataType = "char(32)";
            param1.ParamName = "@param1";
            param1.ParamValue = GeneralUtil.Null2String(windowId);
            paramList.Add(param1);
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            if (result.Rows.Count <= 0)
            {
                ExceptionManager.Throw("The specified item has been removed.");
            }
            String stageType = GeneralUtil.Null2String(result.Rows[0][0]);

            int resultType=0;
            try
            {
                resultType = Int32.Parse(stageType);
            }
            catch
            {
                ExceptionManager.Throw("The stage type is not right.");
            }
            return resultType;
        }
        /// <summary>
        /// select*from Dashboard_Defect
        /// </summary>
        /// <returns></returns>
        [AjaxPro.AjaxMethod]
        public static DataTable GetTop10Defect()
        {
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "select*from Dashboard_Defect";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);

            return result;

        }

        public static DataTable GetAllStage()
        {
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT distinct [Stage] FROM [Dashboard_Stage] ";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            return result;
        }

    }
}
