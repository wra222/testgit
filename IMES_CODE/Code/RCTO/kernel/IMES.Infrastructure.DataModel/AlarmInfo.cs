using System;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(Alarm))]
    public class AlarmInfo
    {
        [ORMapping(Alarm.fn_id)]
        private int _id = int.MinValue;

        /// <summary>
        /// SA/FA
        /// </summary>
        [ORMapping(Alarm.fn_stage)]
        private string _stage = null;

        /// <summary>
        /// 被搜索的TestLog的创建时间范围
        /// </summary>
        [ORMapping(Alarm.fn_startTime)]
        private DateTime _startTime = DateTime.MinValue;

        [ORMapping(Alarm.fn_endTime)]
        private DateTime _endTime = DateTime.MinValue;

        /// <summary>
        /// 因哪条报警标准而报警
        /// </summary>
        [ORMapping(Alarm.fn_alarmSettingID)]
        private int _alarmSettingId = int.MinValue;

        [ORMapping(Alarm.fn_line)]
        private string _line = null;

        [ORMapping(Alarm.fn_station)]
        private string _station = null;

        [ORMapping(Alarm.fn_family)]
        private string _family = null;

        /// <summary>
        /// 超过次数限制的Defect
        /// </summary>
        [ORMapping(Alarm.fn_defect)]
        private string _defect = null;

        /// <summary>
        /// ALM1:良率超标 ALM2:相同不良超标
        /// </summary>
        [ORMapping(Alarm.fn_reasonCode)]
        private string _reasonCode = null;

        [ORMapping(Alarm.fn_reason)]
        private string _reason = null;

        /// <summary>
        /// Created;Skip;Hold;Action;Releas
        /// </summary>
        [ORMapping(Alarm.fn_status)]
        private string _status = null;

        /// <summary>
        /// Alarm记录的创建时间
        /// </summary>
        [ORMapping(Alarm.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;

        [ORMapping(Alarm.fn_skipHoldPIC)]
        private string _skipHoldPic = null;

        [ORMapping(Alarm.fn_skipHoldTime)]
        private DateTime _skipHoldTime = DateTime.MinValue;

        /// <summary>
        /// 被Hold的多个Family或Model，每个Family或Model后都有','，"All"代表所有Family都要被Hold
        /// </summary>
        [ORMapping(Alarm.fn_holdModel)]
        private string _holdModel = null;

        /// <summary>
        /// 被Hold的多个Line，每个Line后都有','，"All"代表所有Line都要被Hold
        /// </summary>
        [ORMapping(Alarm.fn_holdLine)]
        private string _holdLine = null;

        /// <summary>
        /// 被Hold的多个Station，每个Station后都有','，"All"代表所有Station都要被Hold
        /// </summary>
        [ORMapping(Alarm.fn_holdStation)]
        private string _holdStation = null;

        [ORMapping(Alarm.fn_actionPIC)]
        private string _actionPic = null;

        [ORMapping(Alarm.fn_actionTime)]
        private DateTime _actionTime = DateTime.MinValue;

        [ORMapping(Alarm.fn_cause)]
        private string _cause = null;

        [ORMapping(Alarm.fn_action)]
        private string _action = null;

        [ORMapping(Alarm.fn_releasePIC)]
        private string _releasePic = null;

        [ORMapping(Alarm.fn_releaseTime)]
        private DateTime _releaseTime = DateTime.MinValue;

        /// <summary>
        /// 新输Remark会追加到已有数据之后
        /// </summary>
        [ORMapping(Alarm.fn_remark)]
        private string _remark = null;
        
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public DateTime ReleaseTime
        {
            get { return _releaseTime; }
            set { _releaseTime = value; }
        }

        public string ReleasePic
        {
            get { return _releasePic; }
            set { _releasePic = value; }
        }

        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public string Cause
        {
            get { return _cause; }
            set { _cause = value; }
        }

        public DateTime ActionTime
        {
            get { return _actionTime; }
            set { _actionTime = value; }
        }

        public string ActionPic
        {
            get { return _actionPic; }
            set { _actionPic = value; }
        }

        public string HoldStation
        {
            get { return _holdStation; }
            set { _holdStation = value; }
        }

        public string HoldLine
        {
            get { return _holdLine; }
            set { _holdLine = value; }
        }

        public string HoldModel
        {
            get { return _holdModel; }
            set { _holdModel = value; }
        }

        public DateTime SkipHoldTime
        {
            get { return _skipHoldTime; }
            set { _skipHoldTime = value; }
        }

        public string SkipHoldPic
        {
            get { return _skipHoldPic; }
            set { _skipHoldPic = value; }
        }

        public DateTime Cdt
        {
            get { return _cdt; }
            set { _cdt = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }

        public string ReasonCode
        {
            get { return _reasonCode; }
            set { _reasonCode = value; }
        }

        public string Defect
        {
            get { return _defect; }
            set { _defect = value; }
        }

        public string Family
        {
            get { return _family; }
            set { _family = value; }
        }

        public string Station
        {
            get { return _station; }
            set { _station = value; }
        }

        public string Line
        {
            get { return _line; }
            set { _line = value; }
        }

        public int AlarmSettingId
        {
            get { return _alarmSettingId; }
            set { _alarmSettingId = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public string Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }

}
