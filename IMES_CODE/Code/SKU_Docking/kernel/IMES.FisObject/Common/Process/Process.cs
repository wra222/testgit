using System;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectBase;
using System.Collections.ObjectModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.Common.Process
{
    /// <summary>
    /// Process定义MB/Product在各站的处理流程
    /// </summary>
    public class Process  : FisObjectBase, IAggregateRoot
    {
        private static IProcessRepository _prcsRepository = null;
        private static IProcessRepository PrcsRepository
        {
            get
            {
                if (_prcsRepository == null)
                    _prcsRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
                return _prcsRepository;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Process()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Process(string process, /*string name, string lastStationId, string stationId,*/ string editor, DateTime cdt, DateTime udt, string type)
        {
            this._process = process;
            //this._name = name;
            //this._lastStationId = lastStationId;
            //this._stationId = stationId;
            this._editor = editor;
            this._cdt = cdt;
            this._udt = udt;
            this._type = type;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private string _process = null;
        //private string _name = null;
        //private string _lastStationId = null;
        //private string _stationId = null;
        private string _descr = null;
        private string _editor = null;
        private DateTime _cdt = default(DateTime);
        private DateTime _udt = default(DateTime);
        private string _type = null;

        /// <summary>
        /// 流程名字
        /// </summary>
        public string process
        {
            get 
            { 
                return this._process; 
            }
            set 
            {
                _tracker.MarkAsModified(this);
                this._process = value; 
            } 
        }

        ///// <summary>
        ///// Process名称
        ///// </summary>
        //public string Name 
        //{
        //    get 
        //    { 
        //        return this._name;
        //    }
        //    set 
        //    {
        //        _tracker.MarkAsModified(this);
        //        this._name = value;
        //    } 
        //}

        ///// <summary>
        ///// LastStationID
        ///// </summary>
        //public string LastStationID
        //{
        //    get { return this._lastStationId; }
        //    set { this._lastStationId = value; }
        //}

        ///// <summary>
        ///// StationID
        ///// </summary>
        //public string StationID 
        //{
        //    get { return this._stationId;}
        //    set { this._stationId = value; } 
        //}

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Descr
        {
            get
            {
                return this._descr;
            }
            set
            {
                _tracker.MarkAsModified(this);
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
                _tracker.MarkAsModified(this);
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
                _tracker.MarkAsModified(this);
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
                _tracker.MarkAsModified(this);
                this._udt = value; 
            } 
        }

        /// <summary>
        /// Type
        /// </summary>
        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._type = value;
            }
        }

        #endregion

        #region . Sub-Instances .

        private IList<ProcessStation> _processStations = null;
        private object _syncObj_processStations = new object();

        /// <summary>
        /// Process包含的站流程设定条目
        /// </summary>
        public IList<ProcessStation> ProcessStations 
        {
            get 
            {
                lock (_syncObj_processStations)
                {
                    if (_processStations == null)
                    {
                        PrcsRepository.FillProcessStations(this);
                    }
                    if (_processStations != null)
                    {
                        return new ReadOnlyCollection<ProcessStation>(this._processStations);
                    }
                    else
                        return null;
                }
            }
            //set 
            //{ 
            //    this._processStations = value; 
            //}
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _process; }
        }

        #endregion
    }
}