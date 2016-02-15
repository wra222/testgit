using System;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PCA.MB
{
    /// <summary>
    /// 主板状态值类。表示一个主板的当前状态。
    /// </summary>
    [ORMapping(typeof(mtns.Pcbstatus))]
    public class MBStatus : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MBStatus()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pcbid"></param>
        /// <param name="station"></param>
        /// <param name="status"></param>
        /// <param name="testFailCount"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="udt"></param>
        /// <param name="cdt"></param>
        public MBStatus(string pcbid, string station, MBStatusEnum status, string editor, string line, DateTime udt, DateTime cdt)
        {
            _pcbid = pcbid;
            _station = station;
            _status = status;
            _testFailCount = 0;
            _editor = editor;
            _line = line;
            _udt = udt;
            _cdt = cdt;

            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MBStatus(string pcbid, string station, MBStatusEnum status, int testFailCount, string editor, string line, DateTime udt, DateTime cdt)
        {
            _pcbid = pcbid;
            _station = station;
            _status = status;
            _testFailCount = testFailCount;
            _editor = editor;
            _line = line;
            _udt = udt;
            _cdt = cdt;

            this._tracker.MarkAsAdded(this);
        }

        [ORMapping(mtns.Pcbstatus.fn_pcbno)]
        private string _pcbid = null;
        [ORMapping(mtns.Pcbstatus.fn_station)]
        private string _station = null;
        [ORMapping(mtns.Pcbstatus.fn_status)]
        private MBStatusEnum _status;
        [ORMapping(mtns.Pcbstatus.fn_testFailCount)]
        private int _testFailCount = 0;
        [ORMapping(mtns.Pcbstatus.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.Pcbstatus.fn_line)]
        private string _line = null;
        [ORMapping(mtns.Pcbstatus.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(mtns.Pcbstatus.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;

        /// <summary>
        /// Pcbid
        /// </summary>
        public string Pcbid
        {
            get
            {
                return _pcbid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _pcbid = value;
            }
        }

        /// <summary>
        /// Station
        /// </summary>
        public string Station 
        {
            get
            {
                return _station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _station = value;
            }
        }

        /// <summary>
        /// Status
        /// </summary>
        public MBStatusEnum Status 
        {
            get
            {
                return _status;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _status = value;
            }
        }

        /// <summary>
        /// TestFailCount
        /// </summary>
        public int TestFailCount
        {
            get
            {
                return this._testFailCount;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._testFailCount = value;
            }
        }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor 
        {
            get
            {
                return _editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _editor = value;
            }
        }

        /// <summary>
        /// Line
        /// </summary>
        public string Line 
        {
            get
            {
                return _line;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _line = value;
            }
        }

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt 
        {
            get
            {
                return _udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt 
        {
            get
            {
                return _cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _cdt = value;
            }
        }

        #region . Overrides of FisObjectBase .

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._pcbid; }
        }

        #endregion
    }

    /// <summary>
    /// 主板状态枚举值。
    /// </summary>
    public enum MBStatusEnum
    {
        NULL = -1,
        Fail = 0,
        Pass,
        CL
    }
}