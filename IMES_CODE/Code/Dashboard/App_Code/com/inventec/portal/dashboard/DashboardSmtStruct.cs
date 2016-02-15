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
namespace com.inventec.portal.dashboard.Smt
{
    public class DashBoardSmtStruct
    {
	    public DashBoardSmtStruct()
	    {
		    //
		    //TODO: 在此处添加构造函数逻辑
		    //
	    }
    }
    [Serializable]
    public class WindowLineFamilyInfo
    {
        private String familyTargetId;

        public String FamilyTargetId
        {
            get { return familyTargetId; }
            set { familyTargetId = value; }
        }

        private String line;

        public String Line
        {
            get { return line; }
            set { line = value; }
        }

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
        //private String displayFields;

        //public String DisplayFields
        //{
        //    get { return displayFields; }
        //    set { displayFields = value; }
        //}
        private String planQty;

        public String PlanQty
        {
            get { return planQty; }
            set { planQty = value; }
        }
       
        private String order;

        public String Order
        {
            get { return order; }
            set { order = value; }
        }


        public WindowLineFamilyInfo()
        {
            this.family ="";
            this.line="";
            this.series = "";
            this.planQty ="";           
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

        private String shift;

        public String Shift
        {
            get { return shift; }
            set { shift = value; }
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


        private List<WindowLineFamilyInfo> windowLineFamilyInfos;

        public List<WindowLineFamilyInfo> WindowLineFamilyInfos
        {
            get { return windowLineFamilyInfos; }
            set { windowLineFamilyInfos = value; }
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
            this.windowLineFamilyInfos = new List<WindowLineFamilyInfo>();
            //!!! 需要有空选项
            this.LineID="";
            this.line="";
            this.shift ="DAY";   
            this.startWorkHour="08";
            this.startWorkMinute="00";
            this.stopWorkHour="20";
            this.stopWorkMinute="30";
            //this.stationDisplay="True";
            this.isInputDsp = "True";
            this.isDefectDsp = "True";
            this.isYieldRateDsp = "True";
            this.isICTInputDsp = "True";
            this.isICTDefectDsp = "True";
            this.isICTYieldRateDsp = "True";
            this.isAOIOutputDsp = "True";
            this.isAOIDefectDsp = "True";


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

        //private String isSaStage;

        //public String IsSaStage
        //{
        //    get { return isSaStage; }
        //    set { isSaStage = value; }
        //}

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

            this.isGoalDisplay = "False";
            this.isInputDisplay = "False";
            this.isOutputDisplay = "False";
            this.isRateDisplay = "False";            
            this.cdt="";
            //this.isSaStage = "True";

        }
    }
}