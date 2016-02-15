using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PCA.MB
{
    /// <summary>
    /// 主板日志值对象。代表一条主板日志。
    /// </summary>
    [ORMapping(typeof(Pcblog))]
    public class MBLog : FisObjectBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MBLog()
        {
            this.Tracker.MarkAsAdded(this);
        }

        public MBLog(int id, string pcbid, string model, string stationID, int status, string lineID, string editor, DateTime cdt)
        {
            _id = id;
            _pcbid = pcbid;
            _model = model;
            _stationID = stationID;
            _status = status;
            _lineID = lineID;
            _editor = editor;
            _cdt = cdt;

            this.Tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(Pcblog.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(Pcblog.fn_pcbno)]
        private string _pcbid = null;
        [ORMapping(Pcblog.fn_pcbmodel)]
        private string _model = null;
        [ORMapping(Pcblog.fn_station)]
        private string _stationID = null;
        [ORMapping(Pcblog.fn_status)]
        private int _status = int.MinValue;
        [ORMapping(Pcblog.fn_line)]
        private string _lineID = null;
        [ORMapping(Pcblog.fn_editor)]
        private string _editor = null;
        [ORMapping(Pcblog.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;

        /// <summary>
        /// 记录标识
        /// </summary>
        public int ID 
        {
            get
            {
                return this._id;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._id = value;
            }
        }

        /// <summary>
        /// Log所属MB
        /// </summary>
        public string PCBID
        {
            get
            {
                return this._pcbid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._pcbid = value;
            }
        }

        /// <summary>
        /// MB机型
        /// </summary>
        public string Model
        {
            get
            {
                return this._model;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._model = value;
            }
        }

        /// <summary>
        /// Station
        /// </summary>
        public string StationID
        {
            get
            {
                return this._stationID;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._stationID = value;
            }
        }

        /// <summary>
        /// 过站结果1=PASS,0=FAIL
        /// </summary>
        public int Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._status = value;
            }
        }

        /// <summary>
        /// 所属Line
        /// </summary>
        public string LineID
        {
            get
            {
                return this._lineID;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._lineID = value;
            }
        }

        /// <summary>
        /// 维护人员
        /// </summary>
        public string Editor
        {
            get
            {
                return this._editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._editor = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cdt = value;
            }
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._id; }
        }

        #endregion
    }
}