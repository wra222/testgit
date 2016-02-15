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
///DashboardShowStruct 的摘要说明  SA or SMT
/// </summary>
/// 
namespace com.inventec.portal.dashboard.Smt
{
    public class DashboardShowSmtStruct
    {
        public DashboardShowSmtStruct()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
    }
    [Serializable]
    public class FamilyShowingInfo //ok
    {
        private String family;

        public String Family
        {
            get { return family; }
            set { family = value; }
        }
        private String series;

        public String Series
        {
            get { return series; }
            set { series = value; }
        }
        private String plan;

        public String Plan
        {
            get { return plan; }
            set { plan = value; }
        }
        private String output;

        public String Output
        {
            get { return output; }
            set { output = value; }
        }

        private String defect;

        public String Defect
        {
            get { return defect; }
            set { defect = value; }
        }

        private String progress;

        public String Progress
        {
            get { return progress; }
            set { progress = value; }
        }


        private String input;

        public String Input
        {
            get { return input; }
            set { input = value; }
        }

        private String iCTInput;

        public String ICTInput
        {
            get { return iCTInput; }
            set { iCTInput = value; }
        }

        private String iCTDefect;

        public String ICTDefect
        {
            get { return iCTDefect; }
            set { iCTDefect = value; }
        }

        private String iCTYield;

        public String ICTYield
        {
            get { return iCTYield; }
            set { iCTYield = value; }
        }


        private String yieldRate;

        public String YieldRate
        {
            get { return yieldRate; }
            set { yieldRate = value; }
        }

        private String aOIOutput;

        public String AOIOutput
        {
            get { return aOIOutput; }
            set { aOIOutput = value; }
        }

        private String aOIDefect;

        public String AOIDefect
        {
            get { return aOIDefect; }
            set { aOIDefect = value; }
        }

        private String isInputDsp;

        public String IsInputDsp
        {
            get { return isInputDsp; }
            set { isInputDsp = value; }
        }

        private String isDefectDsp;

        public String IsDefectDsp
        {
            get { return isDefectDsp; }
            set { isDefectDsp = value; }
        }

        private String isYieldRateDsp;

        public String IsYieldRateDsp
        {
            get { return isYieldRateDsp; }
            set { isYieldRateDsp = value; }
        }

        private String isICTInputDsp;

        public String IsICTInputDsp
        {
            get { return isICTInputDsp; }
            set { isICTInputDsp = value; }
        }

        private String isICTDefectDsp;

        public String IsICTDefectDsp
        {
            get { return isICTDefectDsp; }
            set { isICTDefectDsp = value; }
        }

        private String isICTYieldRateDsp;

        public String IsICTYieldRateDsp
        {
            get { return isICTYieldRateDsp; }
            set { isICTYieldRateDsp = value; }
        }

        private String isAOIOutputDsp;

        public String IsAOIOutputDsp
        {
            get { return isAOIOutputDsp; }
            set { isAOIOutputDsp = value; }
        }

        private String isAOIDefectDsp;

        public String IsAOIDefectDsp
        {
            get { return isAOIDefectDsp; }
            set { isAOIDefectDsp = value; }
        }

        public FamilyShowingInfo()
        {
            this.isInputDsp = "False";
            this.isDefectDsp = "False";
            this.isYieldRateDsp = "False";
            this.isICTInputDsp = "False";
            this.isICTDefectDsp = "False";
            this.isICTYieldRateDsp = "False";
            this.isAOIOutputDsp = "False";
            this.isAOIDefectDsp = "False";
        }

    }
    [Serializable]
    public class LineShowingInfo  //ok
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

        //DAY 08:00 – 20:30 Dashboard_Line_Target.Shift，.StartWorkTime，.StopWorkTime
        private String workTime;

        public String WorkTime
        {
            get { return workTime; }
            set { workTime = value; }
        }


        private String plan;

        public String Plan
        {
            get { return plan; }
            set { plan = value; }
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
        //private String isInputDsp;

        //public String IsInputDsp
        //{
        //    get { return isInputDsp; }
        //    set { isInputDsp = value; }
        //}

        //private String isDefectDsp;

        //public String IsDefectDsp
        //{
        //    get { return isDefectDsp; }
        //    set { isDefectDsp = value; }
        //}

        //private String isYieldRateDsp;

        //public String IsYieldRateDsp
        //{
        //    get { return isYieldRateDsp; }
        //    set { isYieldRateDsp = value; }
        //}

        //private String isICTInputDsp;

        //public String IsICTInputDsp
        //{
        //    get { return isICTInputDsp; }
        //    set { isICTInputDsp = value; }
        //}

        //private String isICTDefectDsp;

        //public String IsICTDefectDsp
        //{
        //    get { return isICTDefectDsp; }
        //    set { isICTDefectDsp = value; }
        //}

        //private String isICTYieldRateDsp;

        //public String IsICTYieldRateDsp
        //{
        //    get { return isICTYieldRateDsp; }
        //    set { isICTYieldRateDsp = value; }
        //}

        //private String isAOIOutputDsp;

        //public String IsAOIOutputDsp
        //{
        //    get { return isAOIOutputDsp; }
        //    set { isAOIOutputDsp = value; }
        //}

        //private String isAOIDefectDsp;

        //public String IsAOIDefectDsp
        //{
        //    get { return isAOIDefectDsp; }
        //    set { isAOIDefectDsp = value; }
        //}

        private String fmlDspField;

        public String FmlDspField
        {
            get { return fmlDspField; }
            set { fmlDspField = value; }
        }


        private List<FamilyShowingInfo> familyShowingInfos;

        public List<FamilyShowingInfo> FamilyShowingInfos
        {
            get { return familyShowingInfos; }
            set { familyShowingInfos = value; }
        }

        public LineShowingInfo()
        {
            this.familyShowingInfos = new List<FamilyShowingInfo>();
            //this.isInputDsp = "False";
            //this.isDefectDsp = "False";
            //this.isYieldRateDsp = "False";
            //this.isICTInputDsp = "False";
            //this.isICTDefectDsp = "False";
            //this.isICTYieldRateDsp = "False";
            //this.isAOIOutputDsp = "False";
            //this.isAOIDefectDsp = "False";
        }

    }
    [Serializable]
    public class StageShowingInfo //ok
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

        private String isInputDisplay;

        public String IsInputDisplay
        {
            get { return isInputDisplay; }
            set { isInputDisplay = value; }
        }

     

        private String isOutputDisplay;

        public String IsOutputDisplay
        {
            get { return isOutputDisplay; }
            set { isOutputDisplay = value; }
        }

        private String isRateDisplay; //两个都要用

        public String IsRateDisplay
        {
            get { return isRateDisplay; }
            set { isRateDisplay = value; }
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

        private String input;

        public String Input
        {
            get { return input; }
            set { input = value; }
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