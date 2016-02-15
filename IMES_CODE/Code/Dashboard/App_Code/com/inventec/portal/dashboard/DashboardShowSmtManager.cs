using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using com.inventec.portal.databaseutil;
using com.inventec.system.exception;
using com.inventec.system;
using System.Text;
using com.inventec.portal.dashboard.common;

/// <summary>
///DashboardShowManager 的摘要说明
/// </summary>
/// 

namespace com.inventec.portal.dashboard.Smt
{
    public class DashboardShowManager
    {
        public DashboardShowManager()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

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

        [AjaxPro.AjaxMethod]
        public static String GetShowInfoDashboardStringByWinId(String id)
        {
            DashboardShowingInfo result = GetShowInfoDashboardByWinId(id);
            String stringResult = JsonHelper<DashboardShowingInfo>.WriteObject(result);
            return stringResult;
        }


        [AjaxPro.AjaxMethod]  //部分ok
        public static DashboardShowingInfo GetShowInfoDashboardByWinId(String id)
        {
            String WindowId = GeneralUtil.Null2String(id);

            String StageDataTableName;
            String LineDataTableName;
            String StationDataTableName;

            GetDataDatabaseTableName(WindowId, out StageDataTableName, out LineDataTableName, out StationDataTableName);

            DashboardShowingInfo result = new DashboardShowingInfo();

            DataTable DashboardMainInfo = GetShowInfoDashboardMainByWinId(WindowId, StageDataTableName);

            //!!!没有找到时的处理
            if (DashboardMainInfo.Rows.Count == 0)
            {
                ExceptionManager.Throw("The specified item has been removed.");
            }

            //还需要进一步转换添加等，注意每一位都要根据新的内容赋值
            result.CurrentWindowSettingUdt = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["Udt"]);
            DateTime NowTime = DateTime.Now;
            result.NowTimeSecond = NowTime.ToString("fff"); //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            result.NowTimeShowString = NowTime.ToString("yyyy-MM-dd HH:mm");
            result.AlertMessage = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["AlertMessage"]);
            result.DisplayName = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DisplayName"]);
            result.WindowName = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["WindowName"]);
            //数字字符串
            result.RefreshTime = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["RefreshTime"]);
            if (result.RefreshTime == "")
            {
                result.RefreshTime = "0";
            }

            result.StageInfo.IsStageDisplay = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["IsStageDsp"]);
            if (result.StageInfo.IsStageDisplay == "")
            {
                result.StageInfo.IsStageDisplay = "True";
            }

            result.StageInfo.StageId = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StageId"]);

            String StageType = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StageType"]);

            if (StageType==Constants.SA_STAGE.ToString())
            {
                //是要以显示SA的风格显示Stage部分
                result.StageInfo.IsSaStage = "True";
                result.StageInfo.Input  = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["SAInput"]);
                result.StageInfo.Output = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["SAOutput"]);
            }
            else
            {
                result.StageInfo.IsSaStage = "False";
                result.StageInfo.Input  = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["SMTInput"]);
                result.StageInfo.Output = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["SMTOutput"]);

            }
            result.StageInfo.Goal = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["Goal"]);


            String StartWorkTime = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StartWorkTime"]);
            String StopWorkTime = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StopWorkTime"]);

            String DisplayFields = GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DisplayFields"]);
            StageDisplayFieldInfo displayFieldInfos = new StageDisplayFieldInfo();
            displayFieldInfos = DashboardManager.StageDisplayFieldsString2Struct(DisplayFields);

            result.StageInfo.IsInputDisplay = displayFieldInfos.IsInputDisplay;
            result.StageInfo.IsOutputDisplay = displayFieldInfos.IsOutputDisplay;
            result.StageInfo.IsGoalDisplay = displayFieldInfos.IsGoalDisplay;
            result.StageInfo.IsRateDisplay = displayFieldInfos.IsRateDisplay;

            int rate = 100;

            //未包含百分号，是如56等的数字字符串
            //!!!没有四舍五入
            //如果Goal=0, 当前的rate算是0还是1，询问后定为100%
            try
            {
                double goalNum = double.Parse(result.StageInfo.Goal);
                if (goalNum != 0)
                {
                    rate = (int)(double.Parse(result.StageInfo.Output) * 100 / goalNum);
                }
            }
            catch
            {
                //不需要处理， Rate为100
            }

            
            result.StageInfo.Rate = rate.ToString();
            int targetRate = GetTargetRate(StartWorkTime, StopWorkTime, NowTime);
            result.StageInfo.TargetRate = targetRate.ToString();

            if (rate < targetRate)
            {
                result.StageInfo.IsRateOk = "False";
            }
            else
            {
                result.StageInfo.IsRateOk = "True";
            }

            /////////
            List<LineShowingInfo> LineResult = GetShowInfoLineListByWinId(WindowId, LineDataTableName, StationDataTableName, NowTime, result.StageInfo.IsSaStage);
            result.StageInfo.LineShowingInfos = LineResult;

            //////////
            return result;

        }


        //ok
        private static void GetDataDatabaseTableName(String WindowId, out String StageDataTableName, out String LineDataTableName, out String StationDataTableName)
        {
            DataTable DataDatabaseInfo = GetDataDatabaseInfoByWinId(WindowId);

            if (DataDatabaseInfo.Rows.Count == 0)
            {
                ExceptionManager.Throw("The specified item has been removed.");
            }

            String DataSourceType = GeneralUtil.Null2String(DataDatabaseInfo.Rows[0]["DataSourceType"]);

            StageDataTableName = Constants.DASHBOARD_STAGE_DATA_UR;
            LineDataTableName = Constants.DASHBOARD_LINE_DATA_UR;
            StationDataTableName = Constants.DASHBOARD_FAMILY_DATA_UR;

            if (DataSourceType.Equals(Constants.DATASOURCE_TYPE_REAL))
            {
                StageDataTableName = Constants.DASHBOARD_STAGE_DATA;
                LineDataTableName = Constants.DASHBOARD_LINE_DATA;
                StationDataTableName = Constants.DASHBOARD_FAMILY_DATA;
            }

        }

        //取得target rate
        private static int GetTargetRate(String startWorkTime, String stopWorkTime, DateTime NowTime)
        {
            int rate = 100;

            int startworkMinute;
            int stopworkMinute;
            int nowMinute;

            NowTime = calcuWorkTimeMinute(startWorkTime, stopWorkTime, NowTime, out startworkMinute, out stopworkMinute, out nowMinute);

            int nowMinuteNext = nowMinute + 24 * 60;

            if (startworkMinute <= nowMinute && nowMinute < stopworkMinute)
            {
                rate = (nowMinute - startworkMinute) * 100 / (stopworkMinute - startworkMinute);
            }
            else if (startworkMinute <= nowMinuteNext && nowMinuteNext < stopworkMinute)
            {
                rate = (nowMinuteNext - startworkMinute) * 100 / (stopworkMinute - startworkMinute);
            }
            return rate;
        }

        private static DateTime calcuWorkTimeMinute(String startWorkTime, String stopWorkTime, DateTime NowTime, out int startworkMinute, out int stopworkMinute, out int nowMinute)
        {
            TimeInfo startWorkInfos = new TimeInfo();
            startWorkInfos = DashboardManager.TimeString2Struct(startWorkTime);
            startworkMinute = Int32.Parse(startWorkInfos.Hour) * 60 + Int32.Parse(startWorkInfos.Minute);

            TimeInfo stopWorkInfos = new TimeInfo();
            stopWorkInfos = DashboardManager.TimeString2Struct(stopWorkTime);
            stopworkMinute = Int32.Parse(stopWorkInfos.Hour) * 60 + Int32.Parse(stopWorkInfos.Minute);

            if (stopworkMinute <= startworkMinute)
            {
                stopworkMinute = stopworkMinute + 24 * 60;
            }

            nowMinute = NowTime.Hour * 60 + NowTime.Minute;
            return NowTime;
        }

        //判断当前是否处于上班时间
        private static Boolean IsOnWorktime(String startWorkTime, String stopWorkTime, DateTime NowTime)
        {
            Boolean result = false;

            int startworkMinute;
            int stopworkMinute;
            int nowMinute;

            NowTime = calcuWorkTimeMinute(startWorkTime, stopWorkTime, NowTime, out startworkMinute, out stopworkMinute, out nowMinute);

            int nowMinuteNext = nowMinute + 24 * 60;
            if (startworkMinute <= nowMinute && nowMinute < stopworkMinute || startworkMinute <= nowMinuteNext && nowMinuteNext < stopworkMinute)
            {
                result = true;
            }
            return result;
        }


        private static DataTable GetDataDatabaseInfoByWinId(String id)
        {

            DataTable result = new DataTable();
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT Dashboard_Window.ID AS WindowId, Dashboard_Window.DataSourceType AS DataSourceType FROM Dashboard_Window WHERE Dashboard_Window.ID=@param1";
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = id;
            paramList.Add(param1);

            result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            return result;

        }

        //参数Id: window id
        //内部取得当前时间Now  ok

        private static DataTable GetShowInfoDashboardMainByWinId(String id, String StageDataTableName)
        {

            DataTable result = new DataTable();
            String connectString = DatabaseUtil.GetConnectionString();

            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.Append("SELECT Dashboard_Window.ID AS WindowId,")
                .Append("Dashboard_Window.WindowName AS WindowName,")
                .Append("Dashboard_Window.DisplayName AS DisplayName,")
                .Append("Dashboard_Window.AlertMessage AS AlertMessage,")
                .Append("Dashboard_Window.RefreshTime AS RefreshTime,")
                .Append("Dashboard_Window.Udt AS Udt,")
                .Append("Dashboard_Stage_Target.Stage AS StageId,")
                .Append("Dashboard_Window.DataSourceType AS DataSourceType,")
                .Append("Dashboard_Window.IsStageDsp AS IsStageDsp,")
                .Append("Dashboard_Stage_Target.DisplayFields AS DisplayFields,")
                .Append("ISNULL(Dashboard_Stage_Data.SMTInput,0) AS SMTInput,")
                .Append("ISNULL(Dashboard_Stage_Data.SMTOutput,0) AS SMTOutput,")
                .Append("ISNULL(Dashboard_Stage_Data.SAInput,0) AS SAInput,")
                .Append("ISNULL(Dashboard_Stage_Data.SAOutput,0) AS SAOutput,")
                .Append("ISNULL(Dashboard_Family_Target.Goal,0) AS Goal,")
                .Append("Dashboard_Stage_Target.StartWorkTime AS StartWorkTime,")
                .Append("Dashboard_Stage_Target.StopWorkTime AS StopWorkTime,")
                .Append("[Dashboard_Stage].[Type] AS StageType ")                
                .Append("FROM Dashboard_Window ")
                .Append("INNER JOIN Dashboard_Stage_Target ON Dashboard_Window.ID = Dashboard_Stage_Target.WindowID ")
                .Append("INNER JOIN [Dashboard_Stage] ON Dashboard_Stage_Target.Stage=[Dashboard_Stage].Stage ")
                .Append("Left OUTER JOIN ")
                .Append(StageDataTableName)
                .Append(" AS Dashboard_Stage_Data ON Dashboard_Stage_Target.Stage = Dashboard_Stage_Data.Stage ")
                .Append("Left OUTER JOIN ")
                .Append("(SELECT SUM([Plan]) AS Goal, WindowsID FROM Dashboard_Family_Target WHERE WindowsID = @param1 GROUP BY WindowsID) AS Dashboard_Family_Target ")
                .Append("ON Dashboard_Window.ID=Dashboard_Family_Target.WindowsID ")
                .Append("WHERE Dashboard_Window.ID=@param1 ");
             

            String sqlString = sqlStringBuilder.ToString();
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = id;
            paramList.Add(param1);

            result = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);
            return result;

        }

        //参数Id: window id  ok
        private static List<LineShowingInfo> GetShowInfoLineListByWinId(String windowId, String LineDataTableName, String StationDataTableName, DateTime NowTime, String IsSaStage)
        {
            List<LineShowingInfo> result = new List<LineShowingInfo>();

            DataTable LineResult = new DataTable();
            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "";

            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.Append("SELECT [Dashboard_Line_Target].[Line] As LineId,[Line].Descr As Line ")
                            .Append(",[ID] ")
                            .Append(",[WindowsID] ")
                            .Append(",RTRIM(ISNULL(Shift,''))+' '+SUBSTRING(CONVERT(varchar,[StartWorkTime],120),12,5)+'-'+SUBSTRING(CONVERT(varchar,[StopWorkTime],120),12,5) AS WorkTime ")
                            .Append(",ISNULL(LineData.[Plan],0) AS [Plan] ")
                            .Append(",ISNULL(LineData.[Output],0) AS [Output] ")
                            .Append(",[StartWorkTime] ")
                            .Append(",[StopWorkTime] ")
                            .Append(",[FmlDspField] ")
                            .Append(",[Order] ")
                            .Append("FROM [Dashboard_Line_Target] INNER JOIN [Line] ON [Line].Line=[Dashboard_Line_Target].Line ")
                            .Append("LEFT OUTER JOIN ")
                            .Append("(SELECT [Dashboard_Family_Target].[Line], ")
                            .Append("SUM([Dashboard_Family_Target].[Plan]) AS [Plan], ")
                            .Append("SUM([Dashboard_Family_Data].[Output]) AS [Output] ")
                            .Append("FROM [Dashboard_Family_Target] ")
                            .Append("LEFT OUTER JOIN ")
                            .Append(StationDataTableName)
                            .Append(" AS [Dashboard_Family_Data] ")                             
                            .Append("ON [Dashboard_Family_Target].Line=[Dashboard_Family_Data].Line ")
                            .Append("AND [Dashboard_Family_Target].Family=[Dashboard_Family_Data].Family ")
                            .Append("AND [Dashboard_Family_Target].Series=[Dashboard_Family_Data].Series ")
                            .Append("WHERE [Dashboard_Family_Target].[WindowsID]=@param1 ")
                            .Append("GROUP BY [Dashboard_Family_Target].[Line]) AS LineData ")
                            .Append("ON [Dashboard_Line_Target].[Line]= LineData.Line ")
                            .Append("WHERE [WindowsID]=@param1 ORDER BY Dashboard_Line_Target.[Order]");

            sqlString = sqlStringBuilder.ToString();

            //LineDataTableName
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = windowId;
            paramList.Add(param1);
            LineResult = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);

            for (int i = 0; i < LineResult.Rows.Count; i++)
            {
                LineShowingInfo lineInfo = new LineShowingInfo();

                String Plan = GeneralUtil.Null2String(LineResult.Rows[i]["Plan"]);
                String Output = GeneralUtil.Null2String(LineResult.Rows[i]["Output"]);

                lineInfo.FmlDspField = GeneralUtil.Null2String(LineResult.Rows[i]["FmlDspField"]);
                lineInfo.LineName = GeneralUtil.Null2String(LineResult.Rows[i]["Line"]);
                lineInfo.WorkTime = GeneralUtil.Null2String(LineResult.Rows[i]["WorkTime"]);
                lineInfo.Plan =Plan;

                lineInfo.Output = Output;

                String LineId = GeneralUtil.Null2String(LineResult.Rows[i]["LineId"]);

                String StartWork = GeneralUtil.Null2String(LineResult.Rows[i]["StartWorkTime"]);
                String StopWork = GeneralUtil.Null2String(LineResult.Rows[i]["StopWorkTime"]);

                //不在工作时间内的内容在这里不拿掉，使得客户端计算当前显示行简便
                if (IsOnWorktime(StartWork, StopWork, NowTime) == true)
                {
                    lineInfo.IsInWorkTime = "True";
                }
                else
                {
                    lineInfo.IsInWorkTime = "False";
                }
                               
                //问题: 如果OutputTarget=0, 当前的rate算是0还是1，询问后定为100%
                int Rate = 100;
                try
                {
                    double PlanQty = double.Parse(Plan);

                    if (PlanQty != 0)
                    {
                        Rate = (int)(double.Parse(Output) * 100 / PlanQty);
                    }
                }
                catch
                {
                    //不需要处理， Rate为0
                }

                int TargetRate = GetTargetRate(StartWork, StopWork, NowTime);

                if (Rate >= TargetRate)
                {
                    lineInfo.IsRateOk = "True";
                }
                else
                {
                    lineInfo.IsRateOk = "False";
                }

                lineInfo.Rate = Rate.ToString();
                lineInfo.TargetRate = TargetRate.ToString();

                List<FamilyShowingInfo> StationResult = GetShowInfoStationListByLineAndWinId(windowId, LineId, StationDataTableName, lineInfo.FmlDspField, IsSaStage);

                lineInfo.FamilyShowingInfos = StationResult;
                result.Add(lineInfo);

            }

            return result;
        }


        //取得Line相关的Station数据显示列表，按order排序  ok
        //参数winId: window id
        //lineId: LineID,用户Line 表 的id
        private static List<FamilyShowingInfo> GetShowInfoStationListByLineAndWinId(String winId, String lineId, String stationDataTableName, String fmlDspField, String IsSaStage)
        {
            List<FamilyShowingInfo> result = new List<FamilyShowingInfo>();

            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "";

            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.Append("SELECT Dashboard_Family_Target.WindowsID, ")
                            .Append("RTRIM(LTRIM(Dashboard_Family_Target.Line)) AS Line, ")
                            .Append("RTRIM(LTRIM(Dashboard_Family_Target.Family)) AS Family, ")
                            .Append("RTRIM(LTRIM(Dashboard_Family_Target.Series)) AS Series, ")
                            .Append("ISNULL(Dashboard_Family_Target.[Plan],0) AS [Plan], ")
                            .Append("ISNULL(Dashboard_Family_Data.Output,0) AS Output, ")
                            .Append("ISNULL(Dashboard_Family_Data.DefectQty,0) AS DefectQty, ")
                            .Append("ISNULL(Dashboard_Family_Data.Input,0) AS Input, ")
                            .Append("ISNULL(Dashboard_Family_Data.AOI_Output,0) AS AOI_Output, ")
                            .Append("ISNULL(Dashboard_Family_Data.AOI_Defect,0) AS AOI_Defect, ")
                            .Append("ISNULL(Dashboard_Family_Data.ICT_Input,0) AS ICT_Input, ")
                            .Append("ISNULL(Dashboard_Family_Data.ICT_Defect,0) AS ICT_Defect ")
                            .Append("FROM Dashboard_Family_Target ")
                            .Append("LEFT OUTER JOIN ")
                            .Append(stationDataTableName)
                            .Append(" AS [Dashboard_Family_Data] ")
                            .Append("ON [Dashboard_Family_Target].Line=[Dashboard_Family_Data].Line ")
                            .Append("AND [Dashboard_Family_Target].Family=[Dashboard_Family_Data].Family ")
                            .Append("AND [Dashboard_Family_Target].Series=[Dashboard_Family_Data].Series ")
                            .Append("WHERE Dashboard_Family_Target.WindowsID=@param1 AND ")
                            .Append("RTRIM(LTRIM(Dashboard_Family_Target.Line))=@param2 ")
                            .Append("ORDER BY Dashboard_Family_Target.[Order]");

            sqlString = sqlStringBuilder.ToString();


            //stationDataTableNam
            List<ConditionValueSet> paramList = new List<ConditionValueSet>();
            ConditionValueSet param1 = new ConditionValueSet();
            param1.ParamName = "@param1";
            param1.DataType = "char(32)";
            param1.ParamValue = winId;
            paramList.Add(param1);

            //!!varchar(32)
            ConditionValueSet param2 = new ConditionValueSet();
            param2.ParamName = "@param2";
            param2.DataType = "char(10)";
            param2.ParamValue = lineId;
            paramList.Add(param2);

            DataTable StationResult = DatabaseUtil.GetResultBySQL(sqlString, connectString, paramList);


            int sumPlan = 0;
            int sumOutput = 0;
            int sumDefect = 0;
            int sumInput = 0;
            int sumICTInput = 0;
            int sumICTDefect = 0;
            int sumAOIOutput = 0;
            int sumAOIDefect = 0;

            SaStationDisplayFieldInfo displayFieldInfosSa = new SaStationDisplayFieldInfo();
            SmtStationDisplayFieldInfo displayFieldInfosSmt = new SmtStationDisplayFieldInfo();

            if (IsSaStage == "True")
            {
                if (fmlDspField != "")
                {
                    displayFieldInfosSa = DashboardManager.SaStationDisplayFieldsString2Struct(fmlDspField);
                }

            }
            else  //Smt
            {
                if (fmlDspField != "")
                {
                    displayFieldInfosSmt = DashboardManager.SmtStationDisplayFieldsString2Struct(fmlDspField);
                }
            }



            for (int i = 0; i < StationResult.Rows.Count; i++)
            {
                FamilyShowingInfo StationInfo = new FamilyShowingInfo();

                String Family = GeneralUtil.Null2String(StationResult.Rows[i]["Family"]);
                String Series = GeneralUtil.Null2String(StationResult.Rows[i]["Series"]);
                String Plan = GeneralUtil.Null2String(StationResult.Rows[i]["Plan"]);
                String Output = GeneralUtil.Null2String(StationResult.Rows[i]["Output"]);
                String DefectQty = GeneralUtil.Null2String(StationResult.Rows[i]["DefectQty"]);

                String Input = GeneralUtil.Null2String(StationResult.Rows[i]["Input"]);
                String AOI_Output = GeneralUtil.Null2String(StationResult.Rows[i]["AOI_Output"]);
                String AOI_Defect = GeneralUtil.Null2String(StationResult.Rows[i]["AOI_Defect"]);
                String ICT_Input = GeneralUtil.Null2String(StationResult.Rows[i]["ICT_Input"]);
                String ICT_Defect = GeneralUtil.Null2String(StationResult.Rows[i]["ICT_Defect"]);
                
                StationInfo.AOIDefect = AOI_Defect;
                StationInfo.AOIOutput = AOI_Output;
                StationInfo.Defect = DefectQty;
                StationInfo.Family = Family;
                StationInfo.ICTDefect = ICT_Defect;
                StationInfo.ICTInput = ICT_Input;
                //StationInfo.ICTYield=
                StationInfo.Input = Input;
                StationInfo.Output = Output;
                StationInfo.Plan = Plan;
                //StationInfo.Progress 
                StationInfo.Series = Series;
                //StationInfo.YieldRate

                sumPlan += Int32.Parse(Plan);
                sumOutput += Int32.Parse(Output);
                sumDefect += Int32.Parse(DefectQty);
                sumInput += Int32.Parse(Input);
                sumICTInput += Int32.Parse(ICT_Input);
                sumICTDefect += Int32.Parse(ICT_Defect);
                sumAOIOutput += Int32.Parse(AOI_Output);
                sumAOIDefect += Int32.Parse(AOI_Defect);


                if (IsSaStage == "True")
                {
                    StationInfo.IsDefectDsp = displayFieldInfosSa.IsDefectDsp;
                    StationInfo.IsICTDefectDsp = displayFieldInfosSa.IsICTDefectDsp;
                    StationInfo.IsICTInputDsp = displayFieldInfosSa.IsICTInputDsp;
                    StationInfo.IsICTYieldRateDsp = displayFieldInfosSa.IsICTYieldRateDsp;
                    StationInfo.IsInputDsp = displayFieldInfosSa.IsInputDsp;
                    StationInfo.IsYieldRateDsp = displayFieldInfosSa.IsYieldRateDsp;

                }
                else  //Smt
                {
                    StationInfo.IsDefectDsp = displayFieldInfosSmt.IsDefectDsp;
                    StationInfo.IsAOIDefectDsp = displayFieldInfosSmt.IsAOIDefectDsp;
                    StationInfo.IsAOIOutputDsp = displayFieldInfosSmt.IsAOIOutputDsp;
                    StationInfo.IsInputDsp = displayFieldInfosSmt.IsInputDsp;
                    StationInfo.IsYieldRateDsp = displayFieldInfosSmt.IsYieldRateDsp;
                }

                string ICTYield = GetYieldRate(ICT_Input, ICT_Defect);
                StationInfo.ICTYield = ICTYield;


                string YieldRate = GetYieldRate(Output, DefectQty);
                StationInfo.YieldRate = YieldRate;

                //算Progress
                string Progress = GetProgress(Plan, Output);
                StationInfo.Progress = Progress;

                result.Add(StationInfo);
            }

            FamilyShowingInfo StationSumInfo = new FamilyShowingInfo();

            if (IsSaStage == "True")
            {
                StationSumInfo.IsDefectDsp = displayFieldInfosSa.IsDefectDsp;
                StationSumInfo.IsICTDefectDsp = displayFieldInfosSa.IsICTDefectDsp;
                StationSumInfo.IsICTInputDsp = displayFieldInfosSa.IsICTInputDsp;
                StationSumInfo.IsICTYieldRateDsp = displayFieldInfosSa.IsICTYieldRateDsp;
                StationSumInfo.IsInputDsp = displayFieldInfosSa.IsInputDsp;
                StationSumInfo.IsYieldRateDsp = displayFieldInfosSa.IsYieldRateDsp;

            }
            else  //Smt
            {
                StationSumInfo.IsDefectDsp = displayFieldInfosSmt.IsDefectDsp;
                StationSumInfo.IsAOIDefectDsp = displayFieldInfosSmt.IsAOIDefectDsp;
                StationSumInfo.IsAOIOutputDsp = displayFieldInfosSmt.IsAOIOutputDsp;
                StationSumInfo.IsInputDsp = displayFieldInfosSmt.IsInputDsp;
                StationSumInfo.IsYieldRateDsp = displayFieldInfosSmt.IsYieldRateDsp;
            }


            if (StationResult.Rows.Count > 0)
            {

                StationSumInfo.Plan = sumPlan.ToString();
                StationSumInfo.Output = sumOutput.ToString();
                StationSumInfo.Defect = sumDefect.ToString();
                StationSumInfo.Input = sumInput.ToString();
                StationSumInfo.ICTInput = sumICTInput.ToString();
                StationSumInfo.ICTDefect = sumICTDefect.ToString();
                StationSumInfo.AOIOutput = sumAOIOutput.ToString();
                StationSumInfo.AOIDefect = sumAOIDefect.ToString();

                StationSumInfo.ICTYield = GetYieldRate(StationSumInfo.ICTInput, StationSumInfo.ICTDefect);
                StationSumInfo.YieldRate = GetYieldRate(StationSumInfo.Output, StationSumInfo.Defect);
                StationSumInfo.Progress = GetProgress(StationSumInfo.Plan, StationSumInfo.Output);

                StationSumInfo.Family = "Total";
                StationSumInfo.Series = "&nbsp";
            }
            else
            {
                StationSumInfo.Plan = "0";
                StationSumInfo.Output = "0";
                StationSumInfo.Defect = "0";
                StationSumInfo.Input = "0";
                StationSumInfo.ICTInput = "0";
                StationSumInfo.ICTDefect = "0";
                StationSumInfo.AOIOutput = "0";
                StationSumInfo.AOIDefect = "0";

                StationSumInfo.ICTYield = "";
                StationSumInfo.YieldRate = "";
                StationSumInfo.Progress = "";

                StationSumInfo.Family = "Total";
                StationSumInfo.Series = "&nbsp";

            }

            result.Add(StationSumInfo);
            return result;

        }

        private static string GetProgress(String Plan, String Output)
        {
            double Progress = 100;
            double tmpPlanQuantity = 0;
            try
            {
                tmpPlanQuantity = double.Parse(Plan);
                if (tmpPlanQuantity != 0)
                {
                    Progress = (double.Parse(Output) / tmpPlanQuantity) * 100;
                }
            }
            catch
            {
                //不需要处理， Progress = 100
            }

            string aa = Progress.ToString("0.00");
            return aa;
        }

        private static string GetYieldRate(String ICT_Input, String ICT_Defect)
        {
            string result = "";

            //算YieldRate
            double ICTYieldRate = 100;
            //用以判断产量为0
            double tmpICTInput = 0;
            try
            {
                tmpICTInput = double.Parse(ICT_Input);
                if (tmpICTInput != 0)
                {
                    ICTYieldRate = (1 - (double.Parse(ICT_Defect) / tmpICTInput)) * 100;
                }
            }
            catch
            {
                //不需要处理， Rate为100
            }

            result = ICTYieldRate.ToString("0.00");
            return result;
        }

    }
}
