using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Process
{
    /// <summary>
    /// 站流程的设定条目, 每个条目定义指定站在成功或失败条件下的下一站
    /// </summary>
    [ORMapping(typeof(Process_Station))]
    public class ProcessStation : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessStation()
        {
            this._tracker.MarkAsAdded(this);
        }
        [ORMapping(Process_Station.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(Process_Station.fn_process)]
        private string _processid = null;
        [ORMapping(Process_Station.fn_preStation)]
        private string _prestation = null;
        [ORMapping(Process_Station.fn_station)]
        private string _stationid = null;
        [ORMapping(Process_Station.fn_status)]
        private int _status = int.MinValue;
        [ORMapping(Process_Station.fn_editor)]
        private string _editor = null;
        [ORMapping(Process_Station.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(Process_Station.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;

        /// <summary>
        /// Id
        /// </summary>
        public int ID 
        {
            get
            {
                return this._id;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._id = value;
            }
        }

        /// <summary>
        /// 所属的Process
        /// </summary>
        public string ProcessID
        {
            get
            {
                return this._processid;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._processid = value;
            }
        }

        ///// <summary>
        ///// 站流程设定适用的类型:MB,PrdId,Pallet
        ///// </summary>
        //public string Type { get; set; }

        /// <summary>
        /// 当前站
        /// </summary>
        public string PreStation
        {
            get
            {
                return this._prestation;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._prestation = value;
            }
        }

        /// <summary>
        /// 下一站
        /// </summary>
        public string StationID
        {
            get
            {
                return this._stationid;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._stationid = value;
            }
        }

        /// <summary>
        /// 上一站处理结果: 1, pass; 0, fail
        /// </summary>
        public int Status
        {
            get
            {
                return this._status;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._status = value;
            }
        }

        /// <summary>
        /// 维护人员工号
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
        /// 更新时间
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
        /// 创建时间
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

        public override object Key
        {
            get { return this._id; }
        }

        #endregion
    }
}
