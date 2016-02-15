﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.inventec.portal.databaseutil;
using com.inventec.system.exception;

/// <summary>
///DashboardManager 的摘要说明
/// </summary>
/// 
namespace com.inventec.portal.dashboard.Fa
{
    public class TimeInfo
    {
        private String hour;

        public String Hour
        {
            get { return hour; }
            set { hour = value; }
        }
        private String minute;

        public String Minute
        {
            get { return minute; }
            set { minute = value; }
        }
        private String second;

        public String Second
        {
            get { return second; }
            set { second = value; }
        }

        public TimeInfo()
        {
            this.hour = "00";
            this.minute = "00";
            this.second = "00";
        }
    }

    public class StageDisplayFieldInfo
    {
        private String isGoalDisplay;

        public String IsGoalDisplay
        {
            get { return isGoalDisplay; }
            set { isGoalDisplay = value; }
        }

        private String isSaInputDisplay;

        public String IsSaInputDisplay
        {
            get { return isSaInputDisplay; }
            set { isSaInputDisplay = value; }
        }

        private String isSaOutputDisplay;

        public String IsSaOutputDisplay
        {
            get { return isSaOutputDisplay; }
            set { isSaOutputDisplay = value; }
        }

        private String isRateDisplay; //两个都要用

        public String IsRateDisplay
        {
            get { return isRateDisplay; }
            set { isRateDisplay = value; }
        }

        private String isDnDisplay;

        public String IsDnDisplay
        {
            get { return isDnDisplay; }
            set { isDnDisplay = value; }
        }

        private String isFaInputDisplay;

        public String IsFaInputDisplay
        {
            get { return isFaInputDisplay; }
            set { isFaInputDisplay = value; }
        }

        private String isFaOutputDisplay;

        public String IsFaOutputDisplay
        {
            get { return isFaOutputDisplay; }
            set { isFaOutputDisplay = value; }
        }

        private String isPaInputDisplay;

        public String IsPaInputDisplay
        {
            get { return isPaInputDisplay; }
            set { isPaInputDisplay = value; }
        }

        private String isPaOutputDisplay;

        public String IsPaOutputDisplay
        {
            get { return isPaOutputDisplay; }
            set { isPaOutputDisplay = value; }
        }

        public StageDisplayFieldInfo()
        {
          
                this.isRateDisplay = "True";

                this.isGoalDisplay = "False";
                this.isSaInputDisplay  = "False";
                this.isSaOutputDisplay = "False";

                this.isDnDisplay = "False";
                this.isFaInputDisplay = "False";
                this.isFaOutputDisplay = "False";
                this.isPaInputDisplay = "False";
                this.isPaOutputDisplay = "False";

        }

        //public StageDisplayFieldInfo(Boolean isSA)
        //{
        //    if (isSA == true)
        //    {
        //        this.isGoalDisplay = "True";
        //        this.isSAInputDisplay = "True";
        //        this.isSAOutputDisplay = "True";

        //        this.isDNDisplay = "False";
        //        this.isFAInputDisplay = "False";
        //        this.isFAOutputDisplay = "False";
        //        this.isPAInputDisplay = "False";
        //        this.isPAOutputDisplay = "False";

        //    }
        //    else
        //    {

        //        this.isGoalDisplay = "False";
        //        this.isSAInputDisplay = "False";
        //        this.isSAOutputDisplay = "False";

        //        this.isDNDisplay = "True";
        //        this.isFAInputDisplay = "True";
        //        this.isFAOutputDisplay = "True";
        //        this.isPAInputDisplay = "True";
        //        this.isPAOutputDisplay = "True";
        //    }

        //    this.isRateDisplay = "True";
        //}

    }

    public class StationDisplayFieldInfo
    {
        private String quantityDsp;
 
        public String QuantityDsp
        {
            get { return quantityDsp; }
            set { quantityDsp = value; }
        }
        private String defectDsp;

        public String DefectDsp
        {
            get { return defectDsp; }
            set { defectDsp = value; }
        }
        private String wIPDsp;

        public String WIPDsp
        {
            get { return wIPDsp; }
            set { wIPDsp = value; }
        }
        private String yieldRateDsp;

        public String YieldRateDsp
        {
            get { return yieldRateDsp; }
            set { yieldRateDsp = value; }
        }

        public StationDisplayFieldInfo()
        {
            this.quantityDsp = "True";
            this.defectDsp = "True";
            this.wIPDsp = "True";
            this.yieldRateDsp = "True";
        }

    }

    public class DashboardManager
    {
        public DashboardManager()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        //tree view source
        //public static DataTable GetDashboardWindowNameList()
        //{
        //    //genutil.TLog<TabDef>.DebugInfo("GetPublishListInIntegrationFile", dashboardTab);
        //    String connectString = DatabaseUtil.GetConnectionString();
        //    String sqlString = "SELECT [ID] as id,[WindowName] as [name],'DASHBOARD_FILE' as [type],'0' as haschild,' ' as nodeuuid FROM [Dashboard_Window] order by [WindowName]";
        //    List<ConditionValueSet> paramList = new List<ConditionValueSet>();
        //    DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
        //    return result;
        //}

        //包含start work time和stop work time, List中有2项内容
        [AjaxPro.AjaxMethod]
        public static List<TimeInfo> GetStageWorkTimeInfo(String stageId)
        {
            List<TimeInfo> result = new List<TimeInfo>();
            String connectString = DatabaseUtil.GetConnectionString();

            String sqlString = "SELECT top 1 [StartWorkTime],[StopWorkTime] FROM [Dashboard_Stage_Target] WHERE [Stage] = @param1";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(10)";
            param1.ParamValue = stageId;
            paramList.Add(param1);

            DataTable sqlResult = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            if (sqlResult.Rows.Count > 0)
            {
                String StartWorkTime = GeneralUtil.Null2String(sqlResult.Rows[0]["StartWorkTime"]);
                String StopWorkTime = GeneralUtil.Null2String(sqlResult.Rows[0]["StopWorkTime"]);

                TimeInfo startWorkInfos = new TimeInfo();
                if (StartWorkTime != "")
                {
                    startWorkInfos = TimeString2Struct(StartWorkTime);
                }
                TimeInfo stopWorkInfos = new TimeInfo();
                if (StopWorkTime != "")
                {
                    stopWorkInfos = TimeString2Struct(StopWorkTime);
                }

                result.Add(startWorkInfos);
                result.Add(stopWorkInfos);

            }
            return result;

        }

        //包含start work time和stop work time, List中有2项内容
        [AjaxPro.AjaxMethod]
        public static WindowLineInfo GetLineBaseSettingInfo(String lineId)
        {
            WindowLineInfo result = new WindowLineInfo();
            String connectString = DatabaseUtil.GetConnectionString();

            String sqlString = "SELECT top 1  Dashboard_Line_Target.FPYTarget AS FPYTarget,Dashboard_Line_Target.FPYAlert AS FPYAlert, Dashboard_Line_Target.OutputTarget AS OutputTarget,[StartWorkTime],[StopWorkTime] FROM [Dashboard_Line_Target] WHERE [Line]=@param1";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(10)";
            param1.ParamValue = lineId;
            paramList.Add(param1);

            DataTable sqlResult = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            if (sqlResult.Rows.Count > 0)
            {

                result.OutputTarget = GeneralUtil.Null2String(sqlResult.Rows[0]["OutputTarget"]);
                result.FPYAlert = GeneralUtil.Null2String(sqlResult.Rows[0]["FPYAlert"]);
                result.FPYTarget = GeneralUtil.Null2String(sqlResult.Rows[0]["FPYTarget"]);

                String StartWorkTime = GeneralUtil.Null2String(sqlResult.Rows[0]["StartWorkTime"]);
                String StopWorkTime = GeneralUtil.Null2String(sqlResult.Rows[0]["StopWorkTime"]);

                TimeInfo startWorkInfos = new TimeInfo();
                if (StartWorkTime != "")
                {
                    startWorkInfos = TimeString2Struct(StartWorkTime);
                }
                TimeInfo stopWorkInfos = new TimeInfo();
                if (StopWorkTime != "")
                {
                    stopWorkInfos = TimeString2Struct(StopWorkTime);
                }

                result.StartWorkHour = startWorkInfos.Hour;
                result.StartWorkMinute = startWorkInfos.Minute;
                result.StopWorkHour = stopWorkInfos.Hour;
                result.StopWorkMinute = stopWorkInfos.Minute;

            }
            return result;

        }

       


        //line列表
        [AjaxPro.AjaxMethod]
        public static DataTable GetLineListByStage(String stageId)
        {
            //genutil.TLog<TabDef>.DebugInfo("GetPublishListInIntegrationFile", dashboardTab);
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT RTRIM(LTRIM(Line)) AS selectId, RTRIM(LTRIM(Line)) As selectValue FROM Line WHERE RTRIM(LTRIM(Stage))=@param1 ORDER BY selectValue";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.DataType = "char(10)";
            param1.ParamName = "@param1";
            param1.ParamValue = GeneralUtil.Null2String(stageId);
            paramList.Add(param1);
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);

            return result;
        }

        //station列表
        [AjaxPro.AjaxMethod]
        public static DataTable GetStationListByLine(String lineId)
        {
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT DISTINCT RTRIM(LTRIM(Station.Station)) AS selectId, RTRIM(LTRIM(Station.Station))+' '+RTRIM(LTRIM(ISNULL(Station.Name,''))) AS selectValue FROM Line_Station INNER JOIN Station ON Line_Station.Station = Station.Station WHERE RTRIM(LTRIM(Line_Station.Line))=@param1 ORDER BY selectValue";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.DataType = "char(10)";
            param1.ParamName = "@param1";
            param1.ParamValue = GeneralUtil.Null2String(lineId);
            paramList.Add(param1);
            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            return result;
        }

        //line列表  是FA和PAK和起来的值
        [AjaxPro.AjaxMethod]
        public static DataTable GetLineListByStageEclipse( List<String> existLines)
        {
            //genutil.TLog<TabDef>.DebugInfo("GetPublishListInIntegrationFile", dashboardTab);
            String connectString = DatabaseUtil.GetConnectionString();
            StringBuilder sqlStringBuilder = new StringBuilder();

            List<ConditionValueSet> paramList = new List<ConditionValueSet>();

            //ConditionValueSet param1 = new ConditionValueSet();
            //param1.DataType = "char(10)";
            //param1.ParamName = "@param0";
            //param1.ParamValue = GeneralUtil.Null2String(stageId);
            //paramList.Add(param1);

            sqlStringBuilder.Append("SELECT DISTINCT RTRIM(LTRIM(Line)) AS selectId, RTRIM(LTRIM(Descr)) As selectValue ")
                    .Append("FROM Line ")
                    .Append("INNER JOIN Dashboard_Stage_Base ON Line.Stage=Dashboard_Stage_Base.Stage ")
                    .Append("WHERE (Dashboard_Stage_Base.Stage_Type=1 ")
                    .Append("OR Dashboard_Stage_Base.Stage_Type=2) ");  
                    
            int paramIndex = 1;            
            for (int i = 0; i < existLines.Count; i++)
            {

                ConditionValueSet paramItem = new ConditionValueSet();
                String paramName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramItem.ParamName = paramName;
                paramItem.DataType = "char(10)";
                paramItem.ParamValue = GeneralUtil.Null2String(existLines[i]);

                paramList.Add(paramItem);

                if (i == 0)
                {
                    sqlStringBuilder.Append("AND RTRIM(LTRIM(Line)) NOT IN (")
                        .Append(paramName);
                }
                else
                {
                    sqlStringBuilder.Append(",").Append(paramName);
                }

                if (i == existLines.Count - 1)
                {
                    sqlStringBuilder.Append(")");
                }
            }

            sqlStringBuilder.Append("ORDER BY selectValue");

            String sqlString = sqlStringBuilder.ToString();

            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);

            return result;
        }

        //station列表
        [AjaxPro.AjaxMethod]
        public static DataTable GetStationListByLineEclipse(String lineId, List<String> existStations)
        {
            String connectString = DatabaseUtil.GetConnectionString();
            StringBuilder sqlStringBuilder = new StringBuilder();

            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.DataType = "char(10)";
            param1.ParamName = "@param0";
            param1.ParamValue = GeneralUtil.Null2String(lineId);
            paramList.Add(param1);

            sqlStringBuilder.Append("SELECT DISTINCT RTRIM(LTRIM(Station.Station)) AS selectId, RTRIM(LTRIM(Station.Station))+' '+RTRIM(LTRIM(ISNULL(Station.Name,''))) AS selectValue FROM Line_Station INNER JOIN Station ON Line_Station.Station = Station.Station WHERE RTRIM(LTRIM(Line_Station.Line))=@param0 ");
            int paramIndex = 1;
            for (int i = 0; i < existStations.Count; i++)
            {

                ConditionValueSet paramItem = new ConditionValueSet();
                String paramName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramItem.ParamName = paramName;
                paramItem.DataType = "char(10)";
                paramItem.ParamValue = GeneralUtil.Null2String(existStations[i]);

                paramList.Add(paramItem);
                if (i == 0)
                {
                    sqlStringBuilder.Append("AND RTRIM(LTRIM(Station.Station)) NOT IN (")
                        .Append(paramName);
                }
                else
                {
                    sqlStringBuilder.Append(",").Append(paramName);
                }

                if (i == existStations.Count - 1)
                {
                    sqlStringBuilder.Append(")");
                }
            }

            sqlStringBuilder.Append("ORDER BY selectValue");
            String sqlString = sqlStringBuilder.ToString();

            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            return result;
        }

        [AjaxPro.AjaxMethod]
        public static WindowLineInfo GetNewLine()
        {
            return new WindowLineInfo();
        }

        [AjaxPro.AjaxMethod]
        public static List<WindowLineInfo> GetNewLineList()
        {
            return new List<WindowLineInfo>();
        }

        [AjaxPro.AjaxMethod]
        public static WindowLineStationInfo GetNewStation()
        {
            return new WindowLineStationInfo();
        }

        [AjaxPro.AjaxMethod]
        public static List<WindowLineStationInfo> GetNewStationList()
        {
            List<WindowLineStationInfo> result = new List<WindowLineStationInfo>();
            return result;
        }

        //取得编辑或新增信息
        //新增时， id为空字串
        [AjaxPro.AjaxMethod]
        public static DashboardWindowInfo GetDashboardWindowSetting(String WindowId)
        {
            WindowId=GeneralUtil.Null2String(WindowId);

            DashboardWindowInfo result = new DashboardWindowInfo();

            if (WindowId == "")
            {
                return result;
            }
            else
            {
                DataTable DashboardMainInfo = GetEditDashboardMainByWinId(WindowId);
                if (DashboardMainInfo.Rows.Count == 0)
                {
                    ExceptionManager.Throw("The specified item has been removed.");
                }

                //还需要进一步转换添加等，注意每一位都要根据新的内容赋值
                result.WindowId=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["ID"]);
                result.WindowName=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["WindowName"]);

                result.DisplayName=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DisplayName"]);
                result.AlertMessage=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["AlertMessage"]);
                result.RefreshTime=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["RefreshTime"]);

                TimeInfo timeInfos=new TimeInfo();
                if(result.RefreshTime!="")
                {
                    int freshTime=Int32.Parse(result.RefreshTime);
                    timeInfos= TimeInt2Struct(freshTime);
                   
                }
                result.Hour=timeInfos.Hour;
                result.Second=timeInfos.Second;
                result.Minute=timeInfos.Minute;

                result.DataSourceType=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DataSourceType"]);
                result.StageTargetId=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StageTargetId"]);
                result.StageId=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StageId"]);
                result.StageName=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StageName"]);
                result.IsStageDisplay=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["IsStageDsp"]);
                result.StartWorkTime=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StartWorkTime"]);

                TimeInfo startWorkInfos=new TimeInfo();
                if(result.StartWorkTime!="")
                {
                    startWorkInfos= TimeString2Struct(result.StartWorkTime);
                   
                }

                result.StartWorkTimeHour=startWorkInfos.Hour;
                result.StartWorkTimeMinute=startWorkInfos.Minute;
                                
                result.StopWorkTime=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StopWorkTime"]);

                TimeInfo stopWorkInfos=new TimeInfo();
                if(result.StopWorkTime!="")
                {
                    stopWorkInfos= TimeString2Struct(result.StopWorkTime);
                   
                }

                result.StopWorkTimeHour=stopWorkInfos.Hour;
                result.StopWorkTimeMinute=stopWorkInfos.Minute;
                
                result.DisplayFields=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DisplayFields"]);

                StageDisplayFieldInfo displayFieldInfos = new StageDisplayFieldInfo();

                if(result.DisplayFields!="")
                {
                    displayFieldInfos = StageDisplayFieldsString2Struct(result.DisplayFields);
                   
                }

                result.IsDnDisplay=displayFieldInfos.IsDnDisplay;
                result.IsFaInputDisplay =displayFieldInfos.IsFaInputDisplay ;
                result.IsFaOutputDisplay = displayFieldInfos.IsFaOutputDisplay;
                result.IsPaInputDisplay = displayFieldInfos.IsPaInputDisplay;
                result.IsPaOutputDisplay  = displayFieldInfos.IsPaOutputDisplay;
                result.IsGoalDisplay = displayFieldInfos.IsGoalDisplay;
                result.IsSaInputDisplay = displayFieldInfos.IsSaInputDisplay;
                result.IsSaOutputDisplay = displayFieldInfos.IsSaOutputDisplay;
                result.IsRateDisplay=displayFieldInfos.IsRateDisplay;


                result.Cdt=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["Cdt"]);
                result.Udt=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["Udt"]);
                result.Editor=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["Editor"]);

                DataTable LineResult = GetEditLineListByWinId(WindowId);

                for (int i = 0; i < LineResult.Rows.Count; i++)
                {
                    WindowLineInfo lineInfo = new WindowLineInfo();
                    lineInfo.LineTargetId= GeneralUtil.Null2String(LineResult.Rows[i]["LineTargetId"]);
                    lineInfo.LineID= GeneralUtil.Null2String(LineResult.Rows[i]["LineId"]);
                    lineInfo.Stage= GeneralUtil.Null2String(LineResult.Rows[i]["SatgeName"]);
                    lineInfo.Line= GeneralUtil.Null2String(LineResult.Rows[i]["LineName"]);
                    lineInfo.FPYTarget= GeneralUtil.Null2String(LineResult.Rows[i]["FPYTarget"]);
                    lineInfo.FPYAlert = GeneralUtil.Null2String(LineResult.Rows[i]["FPYAlert"]);
                    lineInfo.OutputTarget = GeneralUtil.Null2String(LineResult.Rows[i]["OutputTarget"]);
                    lineInfo.StartWork= GeneralUtil.Null2String(LineResult.Rows[i]["StartWorkTime"]);

                    
                    TimeInfo lineStartWorkInfos=new TimeInfo();
                    if(lineInfo.StartWork!="")
                    {
                        lineStartWorkInfos= TimeString2Struct(lineInfo.StartWork);
                       
                    }
                    lineInfo.StartWorkHour=lineStartWorkInfos.Hour;
                    lineInfo.StartWorkMinute=lineStartWorkInfos.Minute;

                    lineInfo.StopWork= GeneralUtil.Null2String(LineResult.Rows[i]["StopWorkTime"]);

                    TimeInfo lineStopWorkInfos = new TimeInfo();
                    if (lineInfo.StopWork != "")
                    {
                        lineStopWorkInfos = TimeString2Struct(lineInfo.StopWork);

                    }
                    lineInfo.StopWorkHour = lineStopWorkInfos.Hour;
                    lineInfo.StopWorkMinute = lineStopWorkInfos.Minute;

                    lineInfo.StationDisplay= GeneralUtil.Null2String(LineResult.Rows[i]["IsStationDsp"]);
                    lineInfo.Order= GeneralUtil.Null2String(LineResult.Rows[i]["SortOrder"]);
                    List<WindowLineStationInfo> StationResult = GetEditStationListByLineAndWinId(WindowId, lineInfo.LineID);
                    lineInfo.WindowLineStationInfos = StationResult;
                    result.WindowLineInfos.Add(lineInfo);
                }
            }
            return result;
        }

        //刷新时间
        private static TimeInfo TimeInt2Struct(int time)
        {
            int countTime = time;
            int hour = (int)(countTime / 3600);
            countTime = countTime - hour * 3600;
            int minute = (int)(countTime / 60);
            countTime = countTime - minute * 60;
            int second = countTime;

            String formatHour=hour.ToString("00");
            String formatMinute=minute.ToString("00");
            String formatSecond=second.ToString("00");

            TimeInfo timeResult = new TimeInfo();
            timeResult.Hour = formatHour;
            timeResult.Minute = formatMinute;
            timeResult.Second = formatSecond;
            return timeResult;
        }


        private static int Struct2TimeInt(TimeInfo time)
        {
            int result=0;            
            result+=Int32.Parse(time.Hour)*3600;
            result+=Int32.Parse(time.Minute)*60;
            result+=Int32.Parse(time.Second);
            return result;
        }

        //start stop work
        public static TimeInfo TimeString2Struct(String timeString)
        {
            TimeInfo timeResult = new TimeInfo();
            try
            {
                DateTime time = DateTime.Parse(timeString);
                int hour = time.Hour;
                int minute = time.Minute;
                int second = 0;

                String formatHour = hour.ToString("00");
                String formatMinute = minute.ToString("00");
                String formatSecond = second.ToString("00");


                timeResult.Hour = formatHour;
                timeResult.Minute = formatMinute;
                timeResult.Second = formatSecond;
            }
            catch
            {
                //不需要处理
            }
            return timeResult;
        }

        //start stop work
        //存盘时时间的数据
        private static String Struct2TimeString(TimeInfo timeInfo)
        {         
            String timeString="2003-01-01 "+timeInfo.Hour+":"+timeInfo.Minute+":"+"00";
            DateTime time = DateTime.Parse(timeString);
            String timeResult = time.ToString();
            return timeResult;

        }


        //这部分字段顺序固定
        //DnDisplay
        //UnPaDisplay
        //OutputDisplay
        //RateDisplay
        public  static StageDisplayFieldInfo StageDisplayFieldsString2Struct(String DisplayFieldsString)
        {
            StageDisplayFieldInfo result = new StageDisplayFieldInfo();
            try
            {
                //如 "Quantilt=true". 逗号分隔
                String[] separator = { "," };
                String[] values = DisplayFieldsString.Split(separator, StringSplitOptions.None);
                String[] separatorNext = { "=" };

                String[] RateDisplayInfos = values[0].Split(separatorNext, StringSplitOptions.None);
                if (RateDisplayInfos[1].ToLower() == "true")
                {
                    result.IsRateDisplay = "True";
                }
                else
                {
                    result.IsRateDisplay = "False";
                }
                /////////////

                String[] SAInputDisplayInfos = values[1].Split(separatorNext, StringSplitOptions.None);
                if (SAInputDisplayInfos[1].ToLower() == "true")
                {
                    result.IsSaInputDisplay = "True";
                }
                else
                {
                    result.IsSaInputDisplay = "False";
                }

                String[] SAOutputDisplayInfos = values[2].Split(separatorNext, StringSplitOptions.None);
                if (SAOutputDisplayInfos[1].ToLower() == "true")
                {
                    result.IsSaOutputDisplay  = "True";
                }
                else
                {
                    result.IsSaOutputDisplay = "False";
                }

                String[] GoalDisplayInfos = values[3].Split(separatorNext, StringSplitOptions.None);
                if (GoalDisplayInfos[1].ToLower() == "true")
                {
                    result.IsGoalDisplay = "True";
                }
                else
                {
                    result.IsGoalDisplay = "False";
                }
                ///////////

                String[] DnDisplayInfos = values[4].Split(separatorNext, StringSplitOptions.None);
                if (DnDisplayInfos[1].ToLower() == "true")
                {
                    result.IsDnDisplay = "True";
                }
                else
                {
                    result.IsDnDisplay  = "False";
                }

                String[] FAInputDisplayInfos = values[5].Split(separatorNext, StringSplitOptions.None);
                if (FAInputDisplayInfos[1].ToLower() == "true")
                {
                    result.IsFaInputDisplay = "True";
                }
                else
                {
                    result.IsFaInputDisplay = "False";
                }

                String[] FAOutputDisplayInfos = values[6].Split(separatorNext, StringSplitOptions.None);
                if (FAOutputDisplayInfos[1].ToLower() == "true")
                {
                    result.IsFaOutputDisplay = "True";
                }
                else
                {
                    result.IsFaOutputDisplay = "False";
                }

                String[] PAInputDisplayInfos = values[7].Split(separatorNext, StringSplitOptions.None);
                if (PAInputDisplayInfos[1].ToLower() == "true")
                {
                    result.IsPaInputDisplay  = "True";
                }
                else
                {
                    result.IsPaInputDisplay = "False";
                }

                String[] PAOutputDisplayInfos = values[8].Split(separatorNext, StringSplitOptions.None);
                if (PAOutputDisplayInfos[1].ToLower() == "true")
                {
                    result.IsPaOutputDisplay = "True";
                }
                else
                {
                    result.IsPaOutputDisplay = "False";
                }

            }
            catch
            {
                //不需要处理
            }
            return result;

        }

        //Display
        private static String Struct2StageDisplayFieldsString(StageDisplayFieldInfo DisplayFieldsInfo)
        {
            String result = "";

            if (DisplayFieldsInfo.IsRateDisplay.ToLower() == "true")
            {
                result += "Rate=True,";
            }
            else
            {
                result += "Rate=False,";
            }

            if (DisplayFieldsInfo.IsSaInputDisplay.ToLower() == "true")
            {
                result += "SaIn=True,";
            }
            else
            {
                result += "SaIn=False,";
            }

            if (DisplayFieldsInfo.IsSaOutputDisplay.ToLower() == "true")
            {
                result += "SaOut=True,";
            }
            else
            {
                result += "SaOut=False,";
            }

            if (DisplayFieldsInfo.IsGoalDisplay.ToLower() == "true")
            {
                result += "Goal=True,";
            }
            else
            {
                result += "Goal=False,";
            }

            if (DisplayFieldsInfo.IsDnDisplay.ToLower() == "true")
            {
                result += "Dn=True,";
            }
            else
            {
                result += "Dn=False,";
            }

            if (DisplayFieldsInfo.IsFaInputDisplay.ToLower() == "true")
            {
                result += "FaIn=True,";
            }
            else
            {
                result += "FaIn=False,";
            }

            if (DisplayFieldsInfo.IsFaOutputDisplay.ToLower() == "true")
            {
                result += "FaOut=True,";
            }
            else
            {
                result += "FaOut=False,";
            }

            if (DisplayFieldsInfo.IsPaInputDisplay.ToLower() == "true")
            {
                result += "PaIn=True,";
            }
            else
            {
                result += "PaIn=False,";
            }

            if (DisplayFieldsInfo.IsPaOutputDisplay.ToLower() == "true")
            {
                result += "PaOut=True";
            }
            else
            {
                result += "PaOut=False";
            }
            return result;

        }

        //这部分字段顺序固定
        //QuantityDsp
        //DefectDsp
        //WIPDsp
        //YieldRateDsp
        public static StationDisplayFieldInfo StationDisplayFieldsString2Struct(String DisplayFieldsString)
        {
            StationDisplayFieldInfo result = new StationDisplayFieldInfo();

            //如 "Quantilt=true". 逗号分隔
            String[] separator = { "," };
            String[] values = DisplayFieldsString.Split(separator, StringSplitOptions.None);
            String[] separatorNext = { "=" };

            String[] QuantityDspInfos = values[0].Split(separatorNext, StringSplitOptions.None);
            if (QuantityDspInfos[1].ToLower() == "true")
            {
                result.QuantityDsp = "True";
            }
            else
            {
                result.QuantityDsp = "False";
            }

            String[] DefectDspInfos = values[1].Split(separatorNext, StringSplitOptions.None);
            if (DefectDspInfos[1].ToLower() == "true")
            {
                result.DefectDsp = "True";
            }
            else
            {
                result.DefectDsp = "False";
            }

            String[] WIPDspDisplayInfos = values[2].Split(separatorNext, StringSplitOptions.None);
            if (WIPDspDisplayInfos[1].ToLower() == "true")
            {
                result.WIPDsp = "True";
            }
            else
            {
                result.WIPDsp = "False";
            }

            String[] YieldRateDspDisplayInfos = values[3].Split(separatorNext, StringSplitOptions.None);
            if (YieldRateDspDisplayInfos[1].ToLower() == "true")
            {
                result.YieldRateDsp = "True";
            }
            else
            {
                result.YieldRateDsp = "False";
            }
            return result;

        }

        //Display
        private static String Struct2StationDisplayFieldsString(StationDisplayFieldInfo DisplayFieldsInfo)
        {
            String result = "";

            if (DisplayFieldsInfo.QuantityDsp.ToLower() == "true")
            {
                result += "QuantityDsp=True,";
            }
            else
            {
                result += "QuantityDsp=False,";
            }

            if (DisplayFieldsInfo.DefectDsp.ToLower() == "true")
            {
                result += "DefectDsp=True,";
            }
            else
            {
                result += "DefectDsp=False,";
            }

            if (DisplayFieldsInfo.WIPDsp.ToLower() == "true")
            {
                result += "WIPDsp=True,";
            }
            else
            {
                result += "WIPDsp=False,";
            }

            if (DisplayFieldsInfo.YieldRateDsp.ToLower() == "true")
            {
                result += "YieldRateDsp=True";
            }
            else
            {
                result += "YieldRateDsp=False";
            }
            return result;

        }

        private static DataTable GetEditDashboardMainByWinId(String windowId)
        {
            DataTable result = new DataTable();
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = new StringBuilder().Append("SELECT Dashboard_Window.ID AS ID, Dashboard_Window.WindowName AS WindowName, Dashboard_Window.DisplayName AS DisplayName,")
                .Append("Dashboard_Window.AlertMessage AS AlertMessage, Dashboard_Window.RefreshTime AS RefreshTime,")
                .Append("Dashboard_Window.DataSourceType AS DataSourceType, Dashboard_Stage_Target.ID AS StageTargetId, RTRIM(LTRIM([Dashboard_Stage].Stage)) AS StageId,")
                .Append("RTRIM(LTRIM([Dashboard_Stage].Stage)) AS StageName, Dashboard_Window.IsStageDsp AS IsStageDsp,")
                .Append("Dashboard_Stage_Target.StartWorkTime AS StartWorkTime, Dashboard_Stage_Target.StopWorkTime AS StopWorkTime,")
                .Append("Dashboard_Stage_Target.DisplayFields AS DisplayFields, Dashboard_Window.Cdt AS Cdt,")
                .Append("Dashboard_Window.Udt AS Udt, Dashboard_Window.Editor AS Editor FROM Dashboard_Window ")
                .Append("INNER JOIN Dashboard_Stage_Target ON Dashboard_Window.ID = Dashboard_Stage_Target.WindowID ")
                .Append("LEFT OUTER JOIN [Dashboard_Stage] ON Dashboard_Stage_Target.Stage = [Dashboard_Stage].Stage where Dashboard_Window.ID=@param1")
                .ToString();

            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = windowId;
            paramList.Add(param1);

            result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            return result;

        }

        //取Stage下line信息 
        //Id: window id
        private static DataTable GetEditLineListByWinId(String WindowId)
        {
            DataTable result = new DataTable();
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT Dashboard_Line_Target.ID AS LineTargetId, RTRIM(LTRIM(Dashboard_Line_Target.Line)) AS LineId, RTRIM(LTRIM([Dashboard_Stage].Stage)) AS SatgeName, RTRIM(LTRIM(Line.Descr)) AS LineName, Dashboard_Line_Target.FPYTarget AS FPYTarget,Dashboard_Line_Target.FPYAlert AS FPYAlert, Dashboard_Line_Target.OutputTarget AS OutputTarget, Dashboard_Line_Target.StartWorkTime AS StartWorkTime, Dashboard_Line_Target.StopWorkTime AS StopWorkTime, Dashboard_Line_Target.IsStationDsp AS IsStationDsp, Dashboard_Line_Target.[Order] AS SortOrder FROM [Dashboard_Stage] INNER JOIN Dashboard_Stage_Target ON [Dashboard_Stage].Stage = Dashboard_Stage_Target.Stage INNER JOIN Dashboard_Window ON Dashboard_Stage_Target.WindowID = Dashboard_Window.ID INNER JOIN Dashboard_Line_Target ON Dashboard_Window.ID = Dashboard_Line_Target.WindowsID LEFT OUTER JOIN Line ON Dashboard_Line_Target.Line = Line.Line where Dashboard_Window.ID=@param1 order by SortOrder";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = WindowId;
            paramList.Add(param1);

            result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            return result;
            
        }

        //winId: window id
        //lineId: LineID,用户Line 表 的id
        private static List<WindowLineStationInfo> GetEditStationListByLineAndWinId(String WindowId, String lineId)
        {

            List<WindowLineStationInfo> result = new List<WindowLineStationInfo>();
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT Dashboard_Station_Target.ID AS StationTargetId, RTRIM(LTRIM(Line.Descr)) AS LineName, RTRIM(LTRIM(Station.Station)) AS StationId, RTRIM(LTRIM(Station.Station))+' '+RTRIM(LTRIM(ISNULL(Station.Name,''))) AS StationName, Dashboard_Station_Target.DisplayFields AS DisplayFields, Dashboard_Station_Target.YieldTarget AS YieldTarget, Dashboard_Station_Target.FactorOfFPY AS FactorOfFPY, Dashboard_Station_Target.[Order] AS SortOrder FROM Dashboard_Window INNER JOIN Dashboard_Line_Target ON Dashboard_Window.ID = Dashboard_Line_Target.WindowsID INNER JOIN Line ON Dashboard_Line_Target.Line = Line.Line INNER JOIN Dashboard_Station_Target ON Dashboard_Window.ID = Dashboard_Station_Target.WindowsID AND Dashboard_Line_Target.Line = Dashboard_Station_Target.Line INNER JOIN Station ON Dashboard_Station_Target.Station = Station.Station where Dashboard_Window.ID=@param1 AND RTRIM(LTRIM(Dashboard_Line_Target.Line))=@param2 ORDER BY SortOrder ";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = WindowId;
            paramList.Add(param1);

            //!!varchar(32)
            ConditionValueSet param2 = new ConditionValueSet();
            param2.ParamName = "@param2";
            param2.DataType = "char(10)";
            param2.ParamValue = lineId;
            paramList.Add(param2);

            DataTable StationResult = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);

            for (int i = 0; i < StationResult.Rows.Count; i++)
            {
                WindowLineStationInfo StationInfo = new WindowLineStationInfo();
                StationInfo.StationTargetId = StationResult.Rows[i]["StationTargetId"].ToString();
                StationInfo.Line = StationResult.Rows[i]["LineName"].ToString();
                StationInfo.StationId = StationResult.Rows[i]["StationId"].ToString();
                StationInfo.Station = StationResult.Rows[i]["StationName"].ToString();
                StationInfo.DisplayFields = StationResult.Rows[i]["DisplayFields"].ToString();


                StationDisplayFieldInfo displayFieldInfos = new StationDisplayFieldInfo();

                if (StationInfo.DisplayFields != "")
                {
                    displayFieldInfos = StationDisplayFieldsString2Struct(StationInfo.DisplayFields);

                }

                StationInfo.Quantity = displayFieldInfos.QuantityDsp;
                StationInfo.Defect = displayFieldInfos.DefectDsp;
                StationInfo.WIP = displayFieldInfos.WIPDsp;
                StationInfo.YieldRate = displayFieldInfos.YieldRateDsp;

                StationInfo.YieldTarget = StationResult.Rows[i]["YieldTarget"].ToString();
                StationInfo.FactorOfFPY = StationResult.Rows[i]["FactorOfFPY"].ToString();
                StationInfo.Order = StationResult.Rows[i]["SortOrder"].ToString();

                result.Add(StationInfo);
            }
            return result;            
        }

        //保存数据，各list中填入order
        //返回Windows的id
        [AjaxPro.AjaxMethod]
        public static String SaveDashboardWindowSetting(DashboardWindowInfo windowInfo)
        {
            String connectString = DatabaseUtil.GetConnectionString();

            String windowId = GeneralUtil.Null2String( windowInfo.WindowId);
            String windowName=GeneralUtil.Null2String( windowInfo.WindowName);

            String sqlString = "";
            StringBuilder sqlStringBuilder = new StringBuilder();

            //同时更新全部相同的stage的start work和stop work的所有SQL
            StringBuilder sqlStageUpdateAllStringBuilder = new StringBuilder();
            //同时更新全部相同的line的start work和stop work的所有SQL
            StringBuilder sqlLineUpdateAllStringBuilder = new StringBuilder();

            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            int paramIndex=0;

            //如果原先有数据，先删除
            if (windowId != "")
            {
                CheckWindowSettingExist(windowId);

                ConditionValueSet paramItem = new ConditionValueSet();
                String paramName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramItem.ParamName = paramName;
                paramItem.DataType = "char(32)";
                paramItem.ParamValue = windowId;

                paramList.Add(paramItem);

                sqlStringBuilder.Append("DELETE FROM [Dashboard_Station_Target] WHERE WindowsID=").Append(paramName).Append(" ");
                sqlStringBuilder.Append("DELETE FROM [Dashboard_Line_Target] WHERE WindowsID=").Append(paramName).Append(" ");
                sqlStringBuilder.Append("DELETE FROM [Dashboard_Stage_Target] WHERE WindowID=").Append(paramName).Append(" ");
                sqlStringBuilder.Append("DELETE FROM [Dashboard_Window] WHERE ID=").Append(paramName).Append(" ");
            }
            else
            {
                //检查名称是否重复
                windowId = DatabaseUtil.GetUUID();
            }

            CheckWindowNameSame(windowName,windowId);

            ConditionValueSet paramItemWindowId = new ConditionValueSet(); 
            paramItemWindowId.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramItemWindowId.DataType = "char(32)";
            paramItemWindowId.ParamValue = windowId;
            paramList.Add(paramItemWindowId);

           ConditionValueSet paramWindowName = new ConditionValueSet(); 
            paramWindowName.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramWindowName.DataType = "nvarchar(128)";
            paramWindowName.ParamValue = windowName;
            paramList.Add(paramWindowName);

            ConditionValueSet paramDisplayName = new ConditionValueSet(); 
            paramDisplayName.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramDisplayName.DataType = "nvarchar(128)";
            paramDisplayName.ParamValue = GeneralUtil.Null2String( windowInfo.DisplayName);
            paramList.Add(paramDisplayName);


            ConditionValueSet paramAlertMessage = new ConditionValueSet(); 
            paramAlertMessage.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramAlertMessage.DataType = "nvarchar(512)";
            paramAlertMessage.ParamValue = GeneralUtil.Null2String( windowInfo.AlertMessage);
            paramList.Add(paramAlertMessage);

            TimeInfo timeInfos=new TimeInfo();
            timeInfos.Hour=windowInfo.Hour;
            timeInfos.Minute=windowInfo.Minute;
            timeInfos.Second=windowInfo.Second;
            int refreshTime=Struct2TimeInt(timeInfos);

            ConditionValueSet paramRefreshTime = new ConditionValueSet(); 
            paramRefreshTime.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramRefreshTime.DataType = "bigint";
            paramRefreshTime.ParamValue = refreshTime.ToString();
            paramList.Add(paramRefreshTime);

            ConditionValueSet paramDataSourceType = new ConditionValueSet(); 
            paramDataSourceType.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramDataSourceType.DataType = "int";
            paramDataSourceType.ParamValue = GeneralUtil.Null2String( windowInfo.DataSourceType);
            paramList.Add(paramDataSourceType);

            ConditionValueSet paramIsStageDsp = new ConditionValueSet(); 
            paramIsStageDsp.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramIsStageDsp.DataType = "bit";
            paramIsStageDsp.ParamValue = GeneralUtil.Null2String( windowInfo.IsStageDisplay);
            paramList.Add(paramIsStageDsp);

            ConditionValueSet paramCdt = new ConditionValueSet();
            //新建时
            if (windowInfo.Cdt != "")
            {                
                paramCdt.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramCdt.DataType = "datetime";
                paramCdt.ParamValue = GeneralUtil.Null2String(windowInfo.Cdt);
                paramList.Add(paramCdt);
            }

            ConditionValueSet paramEditor = new ConditionValueSet(); 
            paramEditor.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramEditor.DataType = "nvarchar(100)";
            paramEditor.ParamValue = GeneralUtil.Null2String( windowInfo.Editor);
            paramList.Add(paramEditor);


            //create time由于没有地方修改，而保留了原来的create time
            sqlStringBuilder.Append("INSERT INTO [Dashboard_Window] ([ID],[WindowName],[DisplayName],[AlertMessage],[RefreshTime]")
                .Append(",[DataSourceType],[IsStageDsp],[Cdt],[Udt],[Editor]) VALUES(")
                .Append(paramItemWindowId.ParamName).Append(",")
                .Append(paramWindowName.ParamName).Append(",")
                .Append(paramDisplayName.ParamName).Append(",")
                .Append(paramAlertMessage.ParamName).Append(",")
                .Append(paramRefreshTime.ParamName).Append(",")
                .Append(paramDataSourceType.ParamName).Append(",")
                .Append(paramIsStageDsp.ParamName).Append(",");
            if (windowInfo.Cdt != "")
            {
                sqlStringBuilder.Append(paramCdt.ParamName).Append(",");
            }
            else
            {
                sqlStringBuilder.Append("getdate()").Append(",");
            }
            sqlStringBuilder.Append("getdate()").Append(",")
                .Append(paramEditor.ParamName)
                .Append(") ");



            ConditionValueSet paramStageTargetId = new ConditionValueSet(); 
            paramStageTargetId.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramStageTargetId.DataType = "char(32)";
            paramStageTargetId.ParamValue =  DatabaseUtil.GetUUID();
            paramList.Add(paramStageTargetId);

            ConditionValueSet paramStageId = new ConditionValueSet(); 
            paramStageId.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramStageId.DataType = "char(10)";
            paramStageId.ParamValue = GeneralUtil.Null2String( windowInfo.StageId);
            paramList.Add(paramStageId);

            //window id见上面

            TimeInfo stageStartWorkTimeInfo=new TimeInfo ();
            stageStartWorkTimeInfo.Hour= windowInfo.StartWorkTimeHour;
            stageStartWorkTimeInfo.Minute=windowInfo.StartWorkTimeMinute;
            String stageStartWorkTime=Struct2TimeString(stageStartWorkTimeInfo);

            ConditionValueSet paramStartWorkTime = new ConditionValueSet(); 
            paramStartWorkTime.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramStartWorkTime.DataType = "datetime";
            paramStartWorkTime.ParamValue =stageStartWorkTime;
            paramList.Add(paramStartWorkTime);

            TimeInfo stageStopWorkTimeInfo=new TimeInfo ();
            stageStopWorkTimeInfo.Hour= windowInfo.StopWorkTimeHour;
            stageStopWorkTimeInfo.Minute=windowInfo.StopWorkTimeMinute;
            String stageStopWorkTime=Struct2TimeString(stageStopWorkTimeInfo);

            ConditionValueSet paramStopWorkTime = new ConditionValueSet(); 
            paramStopWorkTime.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramStopWorkTime.DataType = "datetime";
            paramStopWorkTime.ParamValue =stageStopWorkTime;
            paramList.Add(paramStopWorkTime);

            StageDisplayFieldInfo stageDisplayFieldInfo=new StageDisplayFieldInfo ();
            stageDisplayFieldInfo.IsDnDisplay = windowInfo.IsDnDisplay;
            stageDisplayFieldInfo.IsFaInputDisplay = windowInfo.IsFaInputDisplay;
            stageDisplayFieldInfo.IsFaOutputDisplay = windowInfo.IsFaOutputDisplay;
            stageDisplayFieldInfo.IsPaInputDisplay = windowInfo.IsPaInputDisplay;
            stageDisplayFieldInfo.IsPaOutputDisplay = windowInfo.IsPaOutputDisplay;
            stageDisplayFieldInfo.IsGoalDisplay = windowInfo.IsGoalDisplay;
            stageDisplayFieldInfo.IsSaInputDisplay = windowInfo.IsSaInputDisplay;
            stageDisplayFieldInfo.IsSaOutputDisplay = windowInfo.IsSaOutputDisplay;
            stageDisplayFieldInfo.IsRateDisplay = windowInfo.IsRateDisplay;


            String stageDisplayFieldValue =Struct2StageDisplayFieldsString(stageDisplayFieldInfo);

            ConditionValueSet paramDisplayFields = new ConditionValueSet(); 
            paramDisplayFields.ParamName = "@param" + paramIndex;
            paramIndex = paramIndex + 1;
            paramDisplayFields.DataType = "varchar(100)";
            paramDisplayFields.ParamValue =stageDisplayFieldValue;
            paramList.Add(paramDisplayFields);

            StringBuilder sqlStageStringBuilder = new StringBuilder();
            sqlStageStringBuilder.Append("INSERT INTO [Dashboard_Stage_Target] ([ID],[Stage],[WindowID],[StartWorkTime],[StopWorkTime],[DisplayFields])")
                .Append("VALUES(")
                .Append(paramStageTargetId.ParamName).Append(",")
                .Append(paramStageId.ParamName).Append(",")
                .Append(paramItemWindowId.ParamName).Append(",")
                .Append(paramStartWorkTime.ParamName).Append(",")
                .Append(paramStopWorkTime.ParamName).Append(",")
                .Append(paramDisplayFields.ParamName)
                .Append(") ");

            sqlStageUpdateAllStringBuilder.Append("UPDATE [Dashboard_Stage_Target] SET [StartWorkTime] = ")
                .Append(paramStartWorkTime.ParamName).Append(",")
                .Append("[StopWorkTime] =")
                .Append(paramStopWorkTime.ParamName)
                .Append(" WHERE [Stage]=")
                .Append(paramStageId.ParamName).Append(" ");

            StringBuilder sqlLineStringBuilder = new StringBuilder();
            StringBuilder sqlStationStringBuilder = new StringBuilder();

            //判断windowId,如果不为空，先删除对应的station,line, stage window，再加入
            for (int i = 0; i < windowInfo.WindowLineInfos.Count; i++)
            {
                WindowLineInfo lineItem= windowInfo.WindowLineInfos[i];  
           
                ConditionValueSet paramLineTargetId = new ConditionValueSet(); 
                paramLineTargetId.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramLineTargetId.DataType = "char(32)";
                paramLineTargetId.ParamValue =  DatabaseUtil.GetUUID();
                paramList.Add(paramLineTargetId);

                ConditionValueSet paramLineId = new ConditionValueSet(); 
                paramLineId.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramLineId.DataType = "char(10)";
                paramLineId.ParamValue =  GeneralUtil.Null2String(lineItem.LineID);
                paramList.Add(paramLineId);

                ConditionValueSet paramOutputTarget = new ConditionValueSet(); 
                paramOutputTarget.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramOutputTarget.DataType = "int";
                paramOutputTarget.ParamValue = GeneralUtil.Null2String(lineItem.OutputTarget);
                paramList.Add(paramOutputTarget);

                ConditionValueSet paramFPYTarget = new ConditionValueSet(); 
                paramFPYTarget.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramFPYTarget.DataType = "float";
                paramFPYTarget.ParamValue = GeneralUtil.Null2String(lineItem.FPYTarget);
                paramList.Add(paramFPYTarget);

                ConditionValueSet paramFPYAlert = new ConditionValueSet();
                paramFPYAlert.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramFPYAlert.DataType = "float";
                paramFPYAlert.ParamValue = GeneralUtil.Null2String(lineItem.FPYAlert);
                paramList.Add(paramFPYAlert);

                TimeInfo lineStartWorkTimeInfo=new TimeInfo ();
                lineStartWorkTimeInfo.Hour= lineItem.StartWorkHour;
                lineStartWorkTimeInfo.Minute=lineItem.StartWorkMinute;
                String lineStartWorkTime=Struct2TimeString(lineStartWorkTimeInfo);

                ConditionValueSet paramLineStartWorkTime = new ConditionValueSet(); 
                paramLineStartWorkTime.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramLineStartWorkTime.DataType = "datetime";
                paramLineStartWorkTime.ParamValue = lineStartWorkTime;
                paramList.Add(paramLineStartWorkTime);

                TimeInfo lineStopWorkTimeInfo=new TimeInfo ();
                lineStopWorkTimeInfo.Hour= lineItem.StopWorkHour;
                lineStopWorkTimeInfo.Minute=lineItem.StopWorkMinute;
                String lineStopWorkTime=Struct2TimeString(lineStopWorkTimeInfo);

                ConditionValueSet paramLineStopWorkTime = new ConditionValueSet(); 
                paramLineStopWorkTime.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramLineStopWorkTime.DataType = "datetime";
                paramLineStopWorkTime.ParamValue = lineStopWorkTime;
                paramList.Add(paramLineStopWorkTime);

                ConditionValueSet paramIsStationDsp = new ConditionValueSet(); 
                paramIsStationDsp.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramIsStationDsp.DataType = "bit";
                paramIsStationDsp.ParamValue = lineItem.StationDisplay;
                paramList.Add(paramIsStationDsp);

                ConditionValueSet paramLineOrder = new ConditionValueSet(); 
                paramLineOrder.ParamName = "@param" + paramIndex;
                paramIndex = paramIndex + 1;
                paramLineOrder.DataType = "int";
                paramLineOrder.ParamValue = i.ToString();
                paramList.Add(paramLineOrder);

                sqlLineStringBuilder.Append("INSERT INTO [Dashboard_Line_Target] ([ID],[WindowsID],[Line],[OutputTarget],[FPYTarget],[FPYAlert],[StartWorkTime],[StopWorkTime],[IsStationDsp],[Order])")
                 .Append("VALUES (")
                 .Append(paramLineTargetId.ParamName).Append(",")   
                 .Append(paramItemWindowId.ParamName).Append(",")
                 .Append(paramLineId.ParamName).Append(",")
                 .Append(paramOutputTarget.ParamName).Append(",")
                 .Append(paramFPYTarget.ParamName).Append(",")
                 .Append(paramFPYAlert.ParamName).Append(",")
                 .Append(paramLineStartWorkTime.ParamName).Append(",")
                 .Append(paramLineStopWorkTime.ParamName).Append(",")
                 .Append(paramIsStationDsp.ParamName).Append(",")
                 .Append(paramLineOrder.ParamName )
                 .Append(") ");

                //存盘时，相同的line的基本数据同时变
                sqlLineUpdateAllStringBuilder.Append("UPDATE [Dashboard_Line_Target] SET [StartWorkTime] = ")
                .Append(paramLineStartWorkTime.ParamName)
                .Append(",[StopWorkTime] =")
                .Append(paramLineStopWorkTime.ParamName)
                .Append(",[OutputTarget] =")
                .Append(paramOutputTarget.ParamName)
                .Append(",[FPYTarget] =")
                .Append(paramFPYTarget.ParamName)
                .Append(",[FPYAlert] =")
                .Append(paramFPYAlert.ParamName)
                .Append(" WHERE [Line]=")
                .Append(paramLineId.ParamName).Append(" ");

                //存SA的目标量
               // String SA = ConfigurationManager.AppSettings["SA"].ToString();
               // ConditionValueSet paramSaStage = new ConditionValueSet(); 
               // paramSaStage.ParamName = "@param" + paramIndex;
               // paramIndex = paramIndex + 1;
               // paramSaStage.DataType = "char";
               // paramSaStage.ParamValue = SA;
               // paramList.Add(paramSaStage);

               // sqlLineUpdateAllStringBuilder.Append("DELETE FROM [Dashboard_StageGoal] WHERE Stage=")
               //.Append(paramSaStage.ParamName).Append(" ")
               //.Append("INSERT INTO [Dashboard_StageGoal] ([Stage],[Goal]) ")
               //.Append("SELECT a.Stage, SUM(a.OutputTarget) as GoalNum ")
               //.Append("FROM (SELECT DISTINCT [Dashboard_Line_Target].[Line] ")
               //.Append(",[Dashboard_Line_Target].[OutputTarget],[Line].Stage ")
               //.Append("FROM [Dashboard_Line_Target] INNER JOIN [Line] ")
               //.Append("ON [Dashboard_Line_Target].[Line]=[Line].[Line] ")
               //.Append("WHERE [Line].Stage=")
               //.Append(paramSaStage.ParamName).Append(" ) AS a GROUP BY a.Stage ");


                for (int j = 0; j < lineItem.WindowLineStationInfos.Count; j++)
                {

                    WindowLineStationInfo stationItem= lineItem.WindowLineStationInfos[j];  

                    ConditionValueSet paramStationTargetId = new ConditionValueSet(); 
                    paramStationTargetId.ParamName = "@param" + paramIndex;
                    paramIndex = paramIndex + 1;
                    paramStationTargetId.DataType = "char(32)";
                    paramStationTargetId.ParamValue =  DatabaseUtil.GetUUID();
                    paramList.Add(paramStationTargetId);

                    ConditionValueSet paramStationId = new ConditionValueSet(); 
                    paramStationId.ParamName = "@param" + paramIndex;
                    paramIndex = paramIndex + 1;
                    paramStationId.DataType = "char(10)";
                    paramStationId.ParamValue = GeneralUtil.Null2String(stationItem.StationId);
                    paramList.Add(paramStationId);
                    
                    ConditionValueSet paramYieldTarget = new ConditionValueSet(); 
                    paramYieldTarget.ParamName = "@param" + paramIndex;
                    paramIndex = paramIndex + 1;
                    paramYieldTarget.DataType = "float";
                    paramYieldTarget.ParamValue = GeneralUtil.Null2String(stationItem.YieldTarget);
                    paramList.Add(paramYieldTarget);

                    //true 1 True都可
                    ConditionValueSet paramFactorOfFPY = new ConditionValueSet();
                    paramFactorOfFPY.ParamName = "@param" + paramIndex;
                    paramIndex = paramIndex + 1;
                    paramFactorOfFPY.DataType = "bit";
                    paramFactorOfFPY.ParamValue = GeneralUtil.Null2String(stationItem.FactorOfFPY);
                    paramList.Add(paramFactorOfFPY);

                    StationDisplayFieldInfo stationDisplayFieldsInfo=new StationDisplayFieldInfo ();
                    stationDisplayFieldsInfo.DefectDsp=stationItem.Defect;
                    stationDisplayFieldsInfo.QuantityDsp=stationItem.Quantity;
                    stationDisplayFieldsInfo.WIPDsp=stationItem.WIP;
                    stationDisplayFieldsInfo.YieldRateDsp=stationItem.YieldRate;

                    String stationDisplayFieldsInfoValue=Struct2StationDisplayFieldsString(stationDisplayFieldsInfo);

                    ConditionValueSet paramStationDisplayFields = new ConditionValueSet(); 
                    paramStationDisplayFields.ParamName = "@param" + paramIndex;
                    paramIndex = paramIndex + 1;
                    paramStationDisplayFields.DataType = "varchar(100)";
                    paramStationDisplayFields.ParamValue = stationDisplayFieldsInfoValue;
                    paramList.Add(paramStationDisplayFields);

                    ConditionValueSet paramStationOrder = new ConditionValueSet(); 
                    paramStationOrder.ParamName = "@param" + paramIndex;
                    paramIndex = paramIndex + 1;
                    paramStationOrder.DataType = "int";
                    paramStationOrder.ParamValue = j.ToString();
                    paramList.Add(paramStationOrder);

                    sqlStationStringBuilder.Append("INSERT INTO [Dashboard_Station_Target] ([ID],[WindowsID],[Line],[Station],[YieldTarget],[FactorOfFPY],[DisplayFields],[Order])")
                        .Append("VALUES (")
                        .Append(paramStationTargetId.ParamName).Append(",")   
                        .Append(paramItemWindowId.ParamName).Append(",")
                        .Append(paramLineId.ParamName).Append(",")
                        .Append(paramStationId.ParamName).Append(",")
                        .Append(paramYieldTarget.ParamName).Append(",")
                        .Append(paramFactorOfFPY.ParamName).Append(",")
                        .Append(paramStationDisplayFields.ParamName).Append(",")
                        .Append(paramStationOrder.ParamName)
                        .Append(") ");

                }
            }

            sqlString = sqlStringBuilder.ToString() + sqlStageStringBuilder.ToString() + sqlLineStringBuilder.ToString() + sqlStationStringBuilder.ToString() + sqlStageUpdateAllStringBuilder.ToString() + sqlLineUpdateAllStringBuilder.ToString();
            DatabaseUtil.ExecSqlNonQueryWithParam(sqlString, connectString, paramList);

            return windowId;
        }

        private static Boolean CheckWindowSettingExist(String windowId)
        {

            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT Dashboard_Window.ID AS ID, Dashboard_Window.WindowName AS WindowName FROM Dashboard_Window where Dashboard_Window.ID=@param1";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = windowId;
            paramList.Add(param1);

            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            if(result.Rows.Count<=0)
            {
                ExceptionManager.Throw("This edit item has been removed.");
            }

            return true;

        }

        private static Boolean CheckWindowNameSame(String windowName, String windowId)
        {

            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT Dashboard_Window.ID AS ID, Dashboard_Window.WindowName AS WindowName FROM Dashboard_Window where Dashboard_Window.ID <>@param1 and Dashboard_Window.WindowName=@param2";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = windowId;
            paramList.Add(param1);

            ConditionValueSet param2 = new ConditionValueSet();
            param2.ParamName = "@param2";
            param2.DataType = "nvarchar(128)";
            param2.ParamValue = windowName;
            paramList.Add(param2);

            DataTable result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            if(result.Rows.Count>0)
            {
                ExceptionManager.Throw("The window name already exists.");
            }
            return true;

        }

        [AjaxPro.AjaxMethod]
        public static void DeleteDashboardWindowSetting(String windowId)
        {
            String connectString = DatabaseUtil.GetConnectionString();
            StringBuilder sqlStringBuilder = new StringBuilder();
            windowId = GeneralUtil.Null2String(windowId);

            if (windowId != "")
            {
                List<ConditionValueSet> paramList = new List<ConditionValueSet>();
                ConditionValueSet paramItem = new ConditionValueSet();
                paramItem.ParamName = "@param";
                paramItem.DataType = "char(32)";
                paramItem.ParamValue = windowId;
                paramList.Add(paramItem);

                sqlStringBuilder.Append("DELETE FROM [Dashboard_Station_Target] WHERE WindowsID=").Append("@param").Append(" ");
                sqlStringBuilder.Append("DELETE FROM [Dashboard_Line_Target] WHERE WindowsID=").Append("@param").Append(" ");
                sqlStringBuilder.Append("DELETE FROM [Dashboard_Stage_Target] WHERE WindowID=").Append("@param").Append(" ");
                sqlStringBuilder.Append("DELETE FROM [Dashboard_Window] WHERE ID=").Append("@param").Append(" ");

                String sqlString = sqlStringBuilder.ToString();
                DatabaseUtil.ExecSqlNonQueryWithParam(sqlString, connectString, paramList);

            }
        }
       
    }
}