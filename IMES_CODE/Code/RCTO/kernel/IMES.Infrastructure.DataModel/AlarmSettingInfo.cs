using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(AlarmSetting))]
    public class AlarmSettingInfo
    {
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Stage
        {
            get { return _stage; }
            set { _stage = value; }
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

        public String Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Double YieldRate
        {
            get { return _yieldRate; }
            set { _yieldRate = value; }
        }

        public int MinQty
        {
            get { return _minQty; }
            set { _minQty = value; }
        }

        public String DefectType
        {
            get { return _defectType; }
            set { _defectType = value; }
        }

        public string Defects
        {
            get { return _defects; }
            set { _defects = value; }
        }

        public int DefectQty
        {
            get { return _defectQty; }
            set { _defectQty = value; }
        }

        public Double Period
        {
            get { return _period; }
            set { _period = value; }
        }

        public string Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }

        public DateTime Cdt
        {
            get { return _cdt; }
            set { _cdt = value; }
        }

        public DateTime Udt
        {
            get { return _udt; }
            set { _udt = value; }
        }

        public int lifeCycle
        {
            get { return _lifeCycle; }
            set { _lifeCycle = value; }
        }

        //private byte _lifeCycle;

        [ORMapping(AlarmSetting.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(AlarmSetting.fn_defectQty)]
        private Int32 _defectQty = int.MinValue;
        [ORMapping(AlarmSetting.fn_defectType)]
        private String _defectType = null;
        [ORMapping(AlarmSetting.fn_defects)]
        private String _defects = null;
        [ORMapping(AlarmSetting.fn_editor)]
        private String _editor = null;
        [ORMapping(AlarmSetting.fn_family)]
        private String _family = null;
        [ORMapping(AlarmSetting.fn_id)]
        private Int32 _id = int.MinValue;
        //[ORMapping(AlarmSetting.fn_lifeCycle)]
        private int _lifeCycle = int.MinValue;
        [ORMapping(AlarmSetting.fn_minQty)]
        private Int32 _minQty = int.MinValue;
        [ORMapping(AlarmSetting.fn_period)]
        private Double _period = double.MinValue;
        [ORMapping(AlarmSetting.fn_stage)]
        private String _stage = null;
        [ORMapping(AlarmSetting.fn_station)]
        private String _station = null;
        [ORMapping(AlarmSetting.fn_type)]
        private String _type = null;
        [ORMapping(AlarmSetting.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(AlarmSetting.fn_yieldRate)]
        private Double _yieldRate = double.MinValue;

        public DateTime CurrTime { get; set; }
    }
}
