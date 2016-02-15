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

/// <summary>
///DashboardShowStruct 的摘要说明
/// </summary>
/// 
namespace com.inventec.portal.dashboard.Fa
{
    public class DashboardShowStruct
    {
        public DashboardShowStruct()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
    }
    [Serializable]
    public class StationShowingInfo
    {

        private String isQuantityDsp;

        public String IsQuantityDsp
        {
            get { return isQuantityDsp; }
            set { isQuantityDsp = value; }
        }
        private String isDefectDsp;

        public String IsDefectDsp
        {
            get { return isDefectDsp; }
            set { isDefectDsp = value; }
        }
        private String isWIPDsp;

        public String IsWIPDsp
        {
            get { return isWIPDsp; }
            set { isWIPDsp = value; }
        }
        private String isYieldRateDsp;

        public String IsYieldRateDsp
        {
            get { return isYieldRateDsp; }
            set { isYieldRateDsp = value; }
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
        private String isYieldRateOk;

        public String IsYieldRateOk
        {
            get { return isYieldRateOk; }
            set { isYieldRateOk = value; }
        }

        private String stationName;

        public String StationName
        {
            get { return stationName; }
            set { stationName = value; }
        }

        public StationShowingInfo()
        {

        }

    }
    [Serializable]
    public class LineShowingInfo
    {
        private String isInWorkTime;

        public String IsInWorkTime
        {
            get { return isInWorkTime; }
            set { isInWorkTime = value; }
        }
        private String lineName;

        public String LineName
        {
            get { return lineName; }
            set { lineName = value; }
        }
        private String fPY;

        public String FPY
        {
            get { return fPY; }
            set { fPY = value; }
        }
        private String isFPYOk;

        public String IsFPYOk
        {
            get { return isFPYOk; }
            set { isFPYOk = value; }
        }
        private String target;

        public String Target
        {
            get { return target; }
            set { target = value; }
        }
        private String output;

        public String Output
        {
            get { return output; }
            set { output = value; }
        }
        private String rate;

        public String Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        private String targetRate;

        public String TargetRate
        {
            get { return targetRate; }
            set { targetRate = value; }
        }
        private String isRateOk;

        public String IsRateOk
        {
            get { return isRateOk; }
            set { isRateOk = value; }
        }
        private String isStationDsp;

        public String IsStationDsp
        {
            get { return isStationDsp; }
            set { isStationDsp = value; }
        }

        //private String fPYTarget;

        //public String FPYTarget
        //{
        //    get { return fPYTarget; }
        //    set { fPYTarget = value; }
        //}

        //private String outputTarget;

        //public String OutputTarget
        //{
        //    get { return outputTarget; }
        //    set { outputTarget = value; }
        //}

        private List<StationShowingInfo> stationShowingInfos;

        public List<StationShowingInfo> StationShowingInfos
        {
            get { return stationShowingInfos; }
            set { stationShowingInfos = value; }
        }
        public LineShowingInfo()
        {
            this.stationShowingInfos = new List<StationShowingInfo>();
        }

    }
    [Serializable]
    public class StageShowingInfo
    {
        private String isStageDisplay;

        public String IsStageDisplay
        {
            get { return isStageDisplay; }
            set { isStageDisplay = value; }
        }
     
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

        private String stageId;

        public String StageId
        {
            get { return stageId; }
            set { stageId = value; }
        }

        private String isSaStage;

        public String IsSaStage
        {
            get { return isSaStage; }
            set { isSaStage = value; }
        }

        private String goal;

        public String Goal
        {
            get { return goal; }
            set { goal = value; }
        }

        private String saInput;

        public String SaInput
        {
            get { return saInput; }
            set { saInput = value; }
        }

        private String saOutput;

        public String SaOutput
        {
            get { return saOutput; }
            set { saOutput = value; }
        }

        private String dn;

        public String Dn
        {
            get { return dn; }
            set { dn = value; }
        }

        private String faInput;

        public String FaInput
        {
            get { return faInput; }
            set { faInput = value; }
        }

        private String faOutput;

        public String FaOutput
        {
            get { return faOutput; }
            set { faOutput = value; }
        }

        private String paInput;

        public String PaInput
        {
            get { return paInput; }
            set { paInput = value; }
        }

        private String paOutput;

        public String PaOutput
        {
            get { return paOutput; }
            set { paOutput = value; }
        }

        private String rate;

        public String Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        private String targetRate;

        public String TargetRate
        {
            get { return targetRate; }
            set { targetRate = value; }
        }
        private String isRateOk;

        public String IsRateOk
        {
            get { return isRateOk; }
            set { isRateOk = value; }
        }
        private List<LineShowingInfo> lineShowingInfos;

        public List<LineShowingInfo> LineShowingInfos
        {
            get { return lineShowingInfos; }
            set { lineShowingInfos = value; }
        }
        public StageShowingInfo()
        {
            this.lineShowingInfos = new List<LineShowingInfo>();
            this.goal = "0";
            this.dn = "0";
        }

    }
    [Serializable]
    public class DashboardShowingInfo
    {
        //dashiboard window设定保存的时间Udt，每次取此时间，当有变化时，
        //取全部内容，显示从新开始
        private String currentWindowSettingUdt;

        public String CurrentWindowSettingUdt
        {
            get { return currentWindowSettingUdt; }
            set { currentWindowSettingUdt = value; }
        }
        //当前服务器时间
        private String nowTimeSecond;

        public String NowTimeSecond
        {
            get { return nowTimeSecond; }
            set { nowTimeSecond = value; }
        }

        //当前用来显示在面板上的时间
        private String nowTimeShowString;

        public String NowTimeShowString
        {
            get { return nowTimeShowString; }
            set { nowTimeShowString = value; }
        }


        //刷新时间间隔时间
        private String refreshTime;

        public String RefreshTime
        {
            get { return refreshTime; }
            set { refreshTime = value; }
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
        private StageShowingInfo stageInfo;

        public StageShowingInfo StageInfo
        {
            get { return stageInfo; }
            set { stageInfo = value; }
        }

        public DashboardShowingInfo()
        {
            this.stageInfo = new StageShowingInfo();
        }
    }

}