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
using com.inventec.system;

/// <summary>
///DashBoardStruct 的摘要说明
/// </summary>
/// 
namespace com.inventec.portal.dashboard.Fa
{
    public class DashBoardStruct
    {
        public DashBoardStruct()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
    }
    [Serializable]
    public class WindowLineStationInfo
    {
        private String stationTargetId;

        public String StationTargetId
        {
            get { return stationTargetId; }
            set { stationTargetId = value; }
        }

        private String line;

        public String Line
        {
            get { return line; }
            set { line = value; }
        }

        private String stationId;

        public String StationId
        {
            get { return stationId; }
            set { stationId = value; }
        }

        private String station;

        public String Station
        {
            get { return station; }
            set { station = value; }
        }
        private String displayFields;

        public String DisplayFields
        {
            get { return displayFields; }
            set { displayFields = value; }
        }
        private String quantity;

        public String Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        private String defect;

        public String Defect
        {
            get { return defect; }
            set { defect = value; }
        }
        private String wIP;

        public String WIP
        {
            get { return wIP; }
            set { wIP = value; }
        }
        private String yieldRate;

        public String YieldRate
        {
            get { return yieldRate; }
            set { yieldRate = value; }
        }
        private String yieldTarget;

        public String YieldTarget
        {
            get { return yieldTarget; }
            set { yieldTarget = value; }
        }
        private String order;

        public String Order
        {
            get { return order; }
            set { order = value; }
        }

        String factorOfFPY;
        public String FactorOfFPY
        {
            get { return factorOfFPY; }
            set { factorOfFPY = value; }
        }

        public WindowLineStationInfo()
        {
            this.stationTargetId="";
            this.line="";
            this.stationId="";
            this.station="";
            this.quantity="True";
            this.defect="True";
            this.wIP="True";
            this.yieldRate="True";
            this.yieldTarget="";
            this.factorOfFPY = "True";
           
        }   

    }

    [Serializable]
    public class WindowLineInfo
    {
        private String lineID;

        public String LineID
        {
            get { return lineID; }
            set { lineID = value; }
        }
        private String stage;

        public String Stage
        {
            get { return stage; }
            set { stage = value; }
        }
        private String line;

        public String Line
        {
            get { return line; }
            set { line = value; }
        }

        private String fPYAlert;

        public String FPYAlert
        {
            get { return fPYAlert; }
            set { fPYAlert = value; }
        }

        private String fPYTarget;

        public String FPYTarget
        {
            get { return fPYTarget; }
            set { fPYTarget = value; }
        }
        private String outputTarget;

        public String OutputTarget
        {
            get { return outputTarget; }
            set { outputTarget = value; }
        }
        private String startWork;

        public String StartWork
        {
            get { return startWork; }
            set { startWork = value; }
        }
        private String startWorkHour;

        public String StartWorkHour
        {
            get { return startWorkHour; }
            set { startWorkHour = value; }
        }
        private String startWorkMinute;

        public String StartWorkMinute
        {
            get { return startWorkMinute; }
            set { startWorkMinute = value; }
        }
        private String stopWork;

        public String StopWork
        {
            get { return stopWork; }
            set { stopWork = value; }
        }
        private String stopWorkHour;

        public String StopWorkHour
        {
            get { return stopWorkHour; }
            set { stopWorkHour = value; }
        }
        private String stopWorkMinute;

        public String StopWorkMinute
        {
            get { return stopWorkMinute; }
            set { stopWorkMinute = value; }
        }
        private String stationDisplay;

        public String StationDisplay
        {
            get { return stationDisplay; }
            set { stationDisplay = value; }
        }

        private List<WindowLineStationInfo> windowLineStationInfos;

        public List<WindowLineStationInfo> WindowLineStationInfos
        {
            get { return windowLineStationInfos; }
            set { windowLineStationInfos = value; }
        }
        private String order;

        public String Order
        {
            get { return order; }
            set { order = value; }
        }

        private String lineTargetId;

        public String LineTargetId
        {
            get { return lineTargetId; }
            set { lineTargetId = value; }
        }

        public WindowLineInfo()
        {
            this.windowLineStationInfos = new List<WindowLineStationInfo>();
            //!!! 需要有空选项
            this.LineID="";
            this.line="";
            this.stage="";   
            this.fPYTarget="";
            this.fPYAlert = "";
            this.outputTarget="";
            this.startWorkHour="08";
            this.startWorkMinute="00";
            this.stopWorkHour="20";
            this.stopWorkMinute="00";
            this.stationDisplay="True";
            this.lineTargetId="";

        }
    }

    [Serializable]
    public class DashboardWindowInfo
    {
        private String windowId;

        public String WindowId
        {
            get { return windowId; }
            set { windowId = value; }
        }
        private String windowName;

        public String WindowName
        {
            get { return windowName; }
            set { windowName = value; }
        }
        private String displayName;

        public String DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        private String alertMessage;

        public String AlertMessage
        {
            get { return alertMessage; }
            set { alertMessage = value; }
        }
        private String refreshTime;

        public String RefreshTime
        {
            get { return refreshTime; }
            set { refreshTime = value; }
        }
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
        private String dataSourceType;

        public String DataSourceType
        {
            get { return dataSourceType; }
            set { dataSourceType = value; }
        }
        private String stageTargetId;

        public String StageTargetId
        {
            get { return stageTargetId; }
            set { stageTargetId = value; }
        }
        private String stageId;

        public String StageId
        {
            get { return stageId; }
            set { stageId = value; }
        }
        private String stageName;

        public String StageName
        {
            get { return stageName; }
            set { stageName = value; }
        }
        private String isStageDisplay;

        public String IsStageDisplay
        {
            get { return isStageDisplay; }
            set { isStageDisplay = value; }
        }
        private String startWorkTime;

        public String StartWorkTime
        {
            get { return startWorkTime; }
            set { startWorkTime = value; }
        }
        private String startWorkTimeHour;

        public String StartWorkTimeHour
        {
            get { return startWorkTimeHour; }
            set { startWorkTimeHour = value; }
        }
        private String startWorkTimeMinute;

        public String StartWorkTimeMinute
        {
            get { return startWorkTimeMinute; }
            set { startWorkTimeMinute = value; }
        }
        private String stopWorkTime;

        public String StopWorkTime
        {
            get { return stopWorkTime; }
            set { stopWorkTime = value; }
        }
        private String stopWorkTimeHour;

        public String StopWorkTimeHour
        {
            get { return stopWorkTimeHour; }
            set { stopWorkTimeHour = value; }
        }
        private String stopWorkTimeMinute;

        public String StopWorkTimeMinute
        {
            get { return stopWorkTimeMinute; }
            set { stopWorkTimeMinute = value; }
        }
        private String displayFields;

        public String DisplayFields
        {
            get { return displayFields; }
            set { displayFields = value; }
        }
        //private String isDnDisplay;

        //public String IsDnDisplay
        //{
        //    get { return isDnDisplay; }
        //    set { isDnDisplay = value; }
        //}
        //private String isUnPaDisplay;

        //public String IsUnPaDisplay
        //{
        //    get { return isUnPaDisplay; }
        //    set { isUnPaDisplay = value; }
        //}
        //private String isOutputDisplay;

        //public String IsOutputDisplay
        //{
        //    get { return isOutputDisplay; }
        //    set { isOutputDisplay = value; }
        //}
        //private String isRateDisplay;

        //public String IsRateDisplay
        //{
        //    get { return isRateDisplay; }
        //    set { isRateDisplay = value; }
        //}

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

        private String cdt;

        public String Cdt
        {
            get { return cdt; }
            set { cdt = value; }
        }
        private String udt;

        public String Udt
        {
            get { return udt; }
            set { udt = value; }
        }
        private String editor;

        public String Editor
        {
            get { return editor; }
            set { editor = value; }
        }
        private List<WindowLineInfo> windowLineInfos;

        public List<WindowLineInfo> WindowLineInfos
        {
            get { return windowLineInfos; }
            set { windowLineInfos = value; }
        }

        public DashboardWindowInfo()
        {
            this.windowId = "";
            this.windowLineInfos = new List<WindowLineInfo>();
            this.windowName = "New";
            this.displayName = "New";
            this.alertMessage = "";
            this.refreshTime="";
            this.hour ="00";
            this.minute = "01";
            this.second = "00";
            this.dataSourceType =Constants.DATASOURCE_TYPE_REAL;
            this.stageTargetId="";
            //this.stageId=""; 默认第一条记录
            //this.stageName="";
            this.isStageDisplay="True";
            this.startWorkTimeHour = "08";
            this.startWorkTimeMinute = "00";
            this.stopWorkTimeHour = "08";
            this.stopWorkTimeMinute = "00";
            this.isDnDisplay="False";
            this.isFaInputDisplay ="False";
            this.isFaOutputDisplay ="False";
            this.isPaInputDisplay  = "False";
            this.isPaOutputDisplay  = "False";
            this.isGoalDisplay  = "False";
            this.isSaInputDisplay = "False";
            this.isSaOutputDisplay  = "False";
            this.isRateDisplay="False";
            this.cdt="";

        }
    }
}