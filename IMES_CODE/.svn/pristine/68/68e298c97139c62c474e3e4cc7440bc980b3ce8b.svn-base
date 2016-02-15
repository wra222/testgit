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

/// <summary>
///DashboardShowManager 的摘要说明
/// </summary>
/// 

namespace com.inventec.portal.dashboard.Fa
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
        public static String  GetShowInfoDashboardStringByWinId(String id)
        {
            DashboardShowingInfo result= GetShowInfoDashboardByWinId(id);
            String stringResult=JsonHelper<DashboardShowingInfo>.WriteObject(result);
            return stringResult;
        }


        [AjaxPro.AjaxMethod]
        public static DashboardShowingInfo GetShowInfoDashboardByWinId(String id)
        {
            String WindowId=GeneralUtil.Null2String(id);

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
            result.CurrentWindowSettingUdt=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["Udt"]);
            DateTime NowTime = DateTime.Now;
            result.NowTimeSecond = NowTime.ToString("fff"); //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            result.NowTimeShowString = NowTime.ToString("yyyy-MM-dd HH:mm");
            result.AlertMessage=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["AlertMessage"]);
            result.DisplayName=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DisplayName"]);
            result.WindowName=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["WindowName"]);
            //数字字符串
            result.RefreshTime=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["RefreshTime"]);
            if (result.RefreshTime == "")
            {
                result.RefreshTime = "0";
            }

            result.StageInfo.IsStageDisplay=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["IsStageDsp"]);
            if (result.StageInfo.IsStageDisplay == "")
            {
                result.StageInfo.IsStageDisplay = "True";
            }

            result.StageInfo.StageId =GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StageId"]);

            //存SA的目标量
            //String SA = ConfigurationManager.AppSettings["SA"];
            //if (SA != null && SA.ToString() == result.StageInfo.StageId)
            //{
            //    //是要以显示SA的风格显示Stage部分
            //    result.StageInfo.IsSaStage = "True";
            //}
            //else
            //{
                result.StageInfo.IsSaStage = "False";
            //}

            //SA的部分拿掉了，不必再显示
            result.StageInfo.Goal = "0";// GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["GOAL"]);
            result.StageInfo.SaInput =GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["SAInput"]);
            result.StageInfo.SaOutput =GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["SAOutput"]);
            result.StageInfo.Dn =GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DN"]);
            result.StageInfo.FaInput =GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["FAInput"]);
            result.StageInfo.FaOutput =GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["FAOutput"]);
            result.StageInfo.PaInput =GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["PAInput"]);
            result.StageInfo.PaOutput =GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["PAOutput"]);

            //String DataSourceType=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DataSourceType"]);

            //String StageDataTableName=Constants.DASHBOARD_STAGE_DATA_UR;
            //String LineDataTableName=Constants.DASHBOARD_LINE_DATA_UR;
            //String StationDataTableName=Constants.DASHBOARD_STATION_DATA_UR;

            //if (DataSourceType.Equals( Constants.DATASOURCE_TYPE_REAL))
            //{
            //    String StageDataTableName=Constants.DASHBOARD_STAGE_DATA;
            //    String LineDataTableName=Constants.DASHBOARD_LINE_DATA;
            //    String StationDataTableName=Constants.DASHBOARD_STATION_DATA;
            //}
            
            String StartWorkTime=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StartWorkTime"]);
            String StopWorkTime=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["StopWorkTime"]);

            String DisplayFields=GeneralUtil.Null2String(DashboardMainInfo.Rows[0]["DisplayFields"]);
            StageDisplayFieldInfo displayFieldInfos = new StageDisplayFieldInfo();
            displayFieldInfos = DashboardManager.StageDisplayFieldsString2Struct(DisplayFields);

            result.StageInfo.IsDnDisplay = displayFieldInfos.IsDnDisplay;
            result.StageInfo.IsFaInputDisplay = displayFieldInfos.IsFaInputDisplay;
            result.StageInfo.IsFaOutputDisplay = displayFieldInfos.IsFaOutputDisplay;
            result.StageInfo.IsPaInputDisplay = displayFieldInfos.IsPaInputDisplay;
            result.StageInfo.IsPaOutputDisplay = displayFieldInfos.IsPaOutputDisplay;
            result.StageInfo.IsGoalDisplay = displayFieldInfos.IsGoalDisplay;
            result.StageInfo.IsSaInputDisplay = displayFieldInfos.IsSaInputDisplay;
            result.StageInfo.IsSaOutputDisplay = displayFieldInfos.IsSaOutputDisplay;
            result.StageInfo.IsRateDisplay = displayFieldInfos.IsRateDisplay;

            int rate = 100;
            if (result.StageInfo.IsSaStage == "True")
            {

                //未包含百分号，是如56等的数字字符串
                //!!!没有四舍五入
                //如果DN=0, 当前的rate算是0还是1，询问后定为100%
                try
                {
                    double goalNum = double.Parse(result.StageInfo.Goal);
                    if (goalNum != 0)
                    {
                        rate = (int)(double.Parse(result.StageInfo.SaOutput) * 100 / goalNum);
                    }
                }
                catch
                {
                    //不需要处理， Rate为100
                }               

            }
            else
            {
                try
                {
                    double dnNum = double.Parse(result.StageInfo.Dn);
                    if (dnNum != 0)
                    {
                        rate = (int)(double.Parse(result.StageInfo.PaOutput ) * 100 / dnNum);
                    }
                }
                catch
                {
                    //不需要处理， Rate为100
                }    

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
            List<LineShowingInfo> LineResult = GetShowInfoLineListByWinId(WindowId, LineDataTableName, StationDataTableName, NowTime);
            result.StageInfo.LineShowingInfos = LineResult;           

            //////////
            return result;

        }
      

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
            StationDataTableName = Constants.DASHBOARD_STATION_DATA_UR;

            if (DataSourceType.Equals(Constants.DATASOURCE_TYPE_REAL))
            {
                StageDataTableName = Constants.DASHBOARD_STAGE_DATA;
                LineDataTableName = Constants.DASHBOARD_LINE_DATA;
                StationDataTableName = Constants.DASHBOARD_STATION_DATA;
            }

            //StageDataTableName = ConfigurationManager.AppSettings["DASHBOARD_STAGE_DATA_UR"].ToString();
            //LineDataTableName = ConfigurationManager.AppSettings["DASHBOARD_LINE_DATA_UR"].ToString();
            //StationDataTableName =ConfigurationManager.AppSettings["DASHBOARD_STATION_DATA_UR"].ToString();

            //if (DataSourceType.Equals(Constants.DATASOURCE_TYPE_REAL))
            //{
            //    StageDataTableName = ConfigurationManager.AppSettings["DASHBOARD_STAGE_DATA"].ToString();
            //    LineDataTableName = ConfigurationManager.AppSettings["DASHBOARD_LINE_DATA"].ToString();
            //    StationDataTableName = ConfigurationManager.AppSettings["DASHBOARD_STATION_DATA"].ToString();
            //}
            
        }

        //取得target rate
        private static int GetTargetRate(String startWorkTime, String stopWorkTime, DateTime NowTime)
        {
            int rate = 100;

            int startworkMinute;
            int stopworkMinute;
            int nowMinute;

            NowTime = calcuWorkTimeMinute(startWorkTime, stopWorkTime, NowTime, out startworkMinute, out stopworkMinute, out nowMinute);

            int nowMinuteNext=nowMinute + 24 * 60;

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
            Boolean result=false;

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
        //内部取得当前时间Now

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
                            //.Append("ISNULL(Dashboard_StageGoal.Goal,0) AS GOAL,")
                            .Append("ISNULL(Dashboard_Stage_Data.DN,0) AS DN,")
                            .Append("ISNULL(Dashboard_Stage_Data.FAInput,0) AS FAInput,")
                            .Append("ISNULL(Dashboard_Stage_Data.FAOutput,0) AS FAOutput,")
                            .Append("ISNULL(Dashboard_Stage_Data.PAInput,0) AS PAInput,")
                            .Append("ISNULL(Dashboard_Stage_Data.PAOutput,0) AS PAOutput, ")
                            .Append("ISNULL(Dashboard_Stage_Data.SAInput,0) AS SAInput,")
                            .Append("ISNULL(Dashboard_Stage_Data.SAOutput,0) AS SAOutput,")
                            .Append("Dashboard_Stage_Target.StartWorkTime AS StartWorkTime, ")
                            .Append("Dashboard_Stage_Target.StopWorkTime AS StopWorkTime ")
                            .Append("FROM Dashboard_Window ")
                            .Append("INNER JOIN Dashboard_Stage_Target ON Dashboard_Window.ID = Dashboard_Stage_Target.WindowID ")
                            //.Append("Left OUTER JOIN Dashboard_StageGoal ON Dashboard_Stage_Target.Stage = Dashboard_StageGoal.Stage ")
                            .Append("Left OUTER JOIN ")
                            .Append(StageDataTableName)
                            .Append(" AS Dashboard_Stage_Data ON Dashboard_Stage_Target.Stage = Dashboard_Stage_Data.Stage ")
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
            /*         
         
            
            */
        }

        //参数Id: window id
        private static List<LineShowingInfo> GetShowInfoLineListByWinId(String windowId,String LineDataTableName,String StationDataTableName,DateTime NowTime)
        {
            List<LineShowingInfo> result = new List<LineShowingInfo>();

            DataTable LineResult = new DataTable();
            String connectString = DatabaseUtil.GetConnectionString();
            //String sqlString = "SELECT Dashboard_Window.ID, ISNULL(Dashboard_Line_Target.FPYTarget,0) AS FPYTarget, ISNULL(Dashboard_Line_Target.OutputTarget,0) AS OutputTarget, Dashboard_Line_Target.StartWorkTime, Dashboard_Line_Target.IsStationDsp, Dashboard_Line_Target.StopWorkTime, Dashboard_Line_Target.[Order] AS [Order], RTRIM(LTRIM(Dashboard_Line_Target.Line)) AS Line, ISNULL(LineData.Input,0) AS Input, ISNULL(LineData.Output,0) AS Output, ISNULL(LineData.DefQty,0) AS DefQty FROM Dashboard_Window INNER JOIN Dashboard_Line_Target ON Dashboard_Window.ID = Dashboard_Line_Target.WindowsID LEFT OUTER JOIN (SELECT RTRIM(LTRIM(Line.Line)) AS Line, Dashboard_Line_Data.Input, Dashboard_Line_Data.Output, Dashboard_Line_Data.DefQty FROM Line INNER JOIN "
            String sqlString = "SELECT Dashboard_Window.ID, ISNULL(Dashboard_Line_Target.FPYTarget,0) AS FPYTarget,ISNULL(Dashboard_Line_Target.FPYAlert,0) AS FPYAlert, ISNULL(Dashboard_Line_Target.OutputTarget,0) AS OutputTarget, Dashboard_Line_Target.StartWorkTime, Dashboard_Line_Target.IsStationDsp, Dashboard_Line_Target.StopWorkTime, Dashboard_Line_Target.[Order] AS [Order], RTRIM(LTRIM(Dashboard_Line_Target.Line)) AS LineId, RTRIM(LTRIM(Line.Descr)) AS Line, ISNULL(LineData.Input,0) AS Input, ISNULL(LineData.Output,0) AS Output FROM Dashboard_Window INNER JOIN Dashboard_Line_Target ON Dashboard_Window.ID = Dashboard_Line_Target.WindowsID INNER JOIN [Line] ON [Line].Line=[Dashboard_Line_Target].Line LEFT OUTER JOIN (SELECT RTRIM(LTRIM(Line.Line)) AS Line, Dashboard_Line_Data.Input, Dashboard_Line_Data.Output FROM Line INNER JOIN "
                + LineDataTableName
                + " AS Dashboard_Line_Data ON Dashboard_Line_Data.Line = Line.Line) AS LineData ON Dashboard_Line_Target.Line=LineData.Line WHERE Dashboard_Window.ID=@param1 ORDER BY Dashboard_Line_Target.[Order]";
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
                
                String FPYTarget= GeneralUtil.Null2String(LineResult.Rows[i]["FPYTarget"]);
                String FPYAlert = GeneralUtil.Null2String(LineResult.Rows[i]["FPYAlert"]);
                String OutputTarget= GeneralUtil.Null2String(LineResult.Rows[i]["OutputTarget"]);
                String StartWork = GeneralUtil.Null2String(LineResult.Rows[i]["StartWorkTime"]);
                String StopWork = GeneralUtil.Null2String(LineResult.Rows[i]["StopWorkTime"]);
                
                String Line=GeneralUtil.Null2String(LineResult.Rows[i]["Line"]);
                String Input=GeneralUtil.Null2String(LineResult.Rows[i]["Input"]);
                String Output=GeneralUtil.Null2String(LineResult.Rows[i]["Output"]);
                //String DefQty=GeneralUtil.Null2String(LineResult.Rows[i]["DefQty"]);


                String LineId = GeneralUtil.Null2String(LineResult.Rows[i]["LineId"]);

                lineInfo.LineName=Line;
                lineInfo.IsStationDsp=GeneralUtil.Null2String(LineResult.Rows[i]["IsStationDsp"]);

                //不在工作时间内的内容在这里不拿掉，使得客户端计算当前显示行简便
                if (IsOnWorktime(StartWork, StopWork, NowTime) == true)
                {
                    lineInfo.IsInWorkTime = "True";
                }
                else
                {
                    lineInfo.IsInWorkTime = "False";
                }

                double FPY = 1;
                //!!! need modify
                //try
                //{
                //    float tmpValueOutput = float.Parse(Output);
                //    if (tmpValueOutput != 0)
                //    {
                //        FPY = (1 - (float.Parse(DefQty) / tmpValueOutput)) * 100;
                //    }
                //}
                //catch
                //{
                //    //不需要处理，FPY=0
                //}

                List<StationShowingInfo> StationResult = GetShowInfoStationListByLineAndWinId(windowId, LineId, StationDataTableName, ref FPY);

                FPY = FPY * 100;
                lineInfo.FPY = FPY.ToString("0.00");

                String FPYTargetformat = double.Parse(FPYTarget).ToString("0.00");
                String FPYAlertformat = double.Parse(FPYAlert).ToString("0.00");


                if (double.Parse(lineInfo.FPY) >= double.Parse(FPYTargetformat))
                {
                    lineInfo.IsFPYOk="True";
                }
                else if (double.Parse(lineInfo.FPY) < double.Parse(FPYTargetformat) && double.Parse(lineInfo.FPY) >= double.Parse(FPYAlertformat))
                {
                    //合格
                    lineInfo.IsFPYOk="JustPass";
                }
                else
                {
                    lineInfo.IsFPYOk="False";
                }

                //问题: 如果OutputTarget=0, 当前的rate算是0还是1，询问后定为100%
                int Rate = 100;
                try
                {
                    double tmpValueOutputTarget = double.Parse(OutputTarget);

                    if (tmpValueOutputTarget != 0)
                    {
                        Rate = (int)(double.Parse(Output) * 100 / tmpValueOutputTarget);
                    }
                }
                catch
                {
                    //不需要处理， Rate为0
                }

                int TargetRate=GetTargetRate(StartWork, StopWork, NowTime);

                if(Rate>=TargetRate)
                {
                    lineInfo.IsRateOk="True";
                }
                else
                {
                    lineInfo.IsRateOk="False";
                }

                lineInfo.Rate=Rate.ToString();
                lineInfo.TargetRate=TargetRate.ToString();

                lineInfo.Output=Output;
                lineInfo.Target=OutputTarget;                 

                lineInfo .StationShowingInfos=StationResult;
                result.Add(lineInfo);

            }
            return result;
        }


        //取得Line相关的Station数据显示列表，按order排序
        //参数winId: window id
        //lineId: LineID,用户Line 表 的id
        private static List<StationShowingInfo> GetShowInfoStationListByLineAndWinId(String winId, String lineId, String stationDataTableName, ref double lineFpy)
        {
            List<StationShowingInfo> result = new List<StationShowingInfo>();

            String connectString = DatabaseUtil.GetConnectionString();
            String sqlString = "SELECT Dashboard_Window.ID, RTRIM(LTRIM(Dashboard_Line_Target.Line)) AS Line, RTRIM(LTRIM(Dashboard_Station_Target.Station))+' '+RTRIM(LTRIM(ISNULL(Station.Name,''))) AS Station, Dashboard_Station_Target.YieldTarget, Dashboard_Station_Target.FactorOfFPY, Dashboard_Station_Target.DisplayFields, Dashboard_Station_Target.[Order], ISNULL(Dashboard_Station_Data.Quantity,0) AS Quantity, ISNULL(Dashboard_Station_Data.Defect,0) AS Defect, ISNULL(Dashboard_Station_Data.WIP,0) AS WIP FROM Dashboard_Window INNER JOIN Dashboard_Line_Target ON Dashboard_Window.ID = Dashboard_Line_Target.WindowsID INNER JOIN Dashboard_Station_Target ON Dashboard_Window.ID = Dashboard_Station_Target.WindowsID AND Dashboard_Line_Target.Line = Dashboard_Station_Target.Line INNER JOIN Station ON Station.Station=Dashboard_Station_Target.Station LEFT OUTER JOIN "
                + stationDataTableName
                + " AS Dashboard_Station_Data ON Dashboard_Station_Target.Station = Dashboard_Station_Data.Station AND Dashboard_Station_Target.Line = Dashboard_Station_Data.Line WHERE Dashboard_Window.ID=@param1 AND RTRIM(LTRIM(Dashboard_Line_Target.Line))=@param2 ORDER BY Dashboard_Station_Target.[Order] ";
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

            for (int i = 0; i < StationResult.Rows.Count; i++)
            {
                StationShowingInfo StationInfo = new StationShowingInfo();

                String DisplayFields = StationResult.Rows[i]["DisplayFields"].ToString();
                String YieldTarget = StationResult.Rows[i]["YieldTarget"].ToString();
                String Quantity = StationResult.Rows[i]["Quantity"].ToString();
                String Defect = StationResult.Rows[i]["Defect"].ToString();
                String WIP = StationResult.Rows[i]["WIP"].ToString();

                String FactorOfFPY = StationResult.Rows[i]["FactorOfFPY"].ToString();                
                                
                StationInfo.StationName = StationResult.Rows[i]["Station"].ToString();

                StationDisplayFieldInfo displayFieldInfos = new StationDisplayFieldInfo();

                if (DisplayFields != "")
                {
                    displayFieldInfos = DashboardManager.StationDisplayFieldsString2Struct(DisplayFields);
                }

                StationInfo.IsQuantityDsp = displayFieldInfos.QuantityDsp;
                StationInfo.IsDefectDsp = displayFieldInfos.DefectDsp;
                StationInfo.IsWIPDsp = displayFieldInfos.WIPDsp;
                StationInfo.IsYieldRateDsp = displayFieldInfos.YieldRateDsp;

                StationInfo.Defect = Defect;
                StationInfo.Quantity = Quantity;
                StationInfo.WIP = WIP;

                double YieldRate = 100;
                //用以判断产量为0
                double tmpValueQuantity = 0;
                try
                {
                    tmpValueQuantity = double.Parse(Quantity);
                    if (tmpValueQuantity != 0)
                    {
                        YieldRate = (1 - (double.Parse(Defect) / tmpValueQuantity)) * 100;
                    }
                }
                catch
                {
                    //不需要处理， Rate为0
                }

                //!!! need check
                if (FactorOfFPY == "True" && tmpValueQuantity != 0)
                {
                    lineFpy = YieldRate * lineFpy / 100;
                }

                    
                StationInfo.YieldRate=YieldRate.ToString("0.00");

                String YieldTargetformat = "0.00";

                try
                {
                    YieldTargetformat = double.Parse(YieldTarget).ToString("0.00");
                }
                catch
                {
                    //不需要处理 YieldTargetforma="0.00";
                }

                //产量为0时，stationShowingInfo.yieldRate字串为空字串，不显示文字
                if (double.Parse(StationInfo.YieldRate) >= double.Parse(YieldTargetformat))
                {
                    ////产量为0时，stationShowingInfo.yieldRate字串为空字串，不显示文字
                    //if (tmpValueQuantity == 0)
                    //{
                    //    StationInfo.YieldRate = "";
                    //}
                    StationInfo.IsYieldRateOk = "True";
                }
                else
                {
                    StationInfo.IsYieldRateOk = "False";
                }

                result.Add(StationInfo);
            }
            return result; 

        }

    }
}
