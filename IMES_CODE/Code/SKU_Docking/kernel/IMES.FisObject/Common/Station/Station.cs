using System;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Station
{
    /// <summary>
    /// 产线站别定义
    /// </summary>
    [ORMapping(typeof(mtns.Station))]
    public class Station : FisObjectBase, IStation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Station()
        {
            this._tracker.MarkAsAdded(this);
        }

        [ORMapping(mtns.Station.fn_station)]
        private string _stationid = null;
        private StationType _stationType;
        [ORMapping(mtns.Station.fn_operationObject)]
        private int _operationObject = int.MinValue;
        [ORMapping(mtns.Station.fn_descr)]
        private string _descr = null;
        [ORMapping(mtns.Station.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.Station.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns.Station.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(mtns.Station.fn_name)]
        private string _name = null;

        /// <summary>
        /// Station代码
        /// </summary>
        public string StationId 
        {
            get
            {
                return this._stationid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._stationid = value;
            }
        }

        /// <summary>
        /// StationType
        /// </summary>
        public StationType StationType
        {
            get
            {
                return this._stationType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._stationType = value;
            }
        }

        /// <summary>
        /// 该站操作的对象类型0:未定义 1:MB; 2:Product;
        /// </summary>
        public int OperationObject
        {
            get
            {
                return this._operationObject;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._operationObject = value;
            }
        }

        /// <summary>
        /// Station描述
        /// </summary>
        public string Descr
        {
            get
            {
                return this._descr;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._descr = value;
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

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Udt
        {
            get
            {
                return this._udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._udt = value;
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._name = value;
            }
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _stationid; }
        }

        #endregion
    }
}