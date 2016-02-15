using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using System.Collections.ObjectModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Repair
{
    /// <summary>
    /// 填充repair对象
    /// </summary>
    /// <param name="rep">repair</param>
    public delegate void FillRepair(Repair rep);
    /// <summary>
    /// MB/Product维修记录
    /// </summary>
    [ORMapping(typeof(mtns.Pcbrepair))]
    public class Repair : FisObjectBase
    {
        /// <summary>
        /// 维修状态
        /// </summary>
        public enum RepairStatus
        {
            NotFinished = 0,
            Finished
        }

        //private static IMBRepository _mbRepository = null;
        //private static IMBRepository MbRepository
        //{
        //    get
        //    {
        //        if (_mbRepository == null)
        //            _mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
        //        return _mbRepository;
        //    }
        //}

        //private static IProductRepository _prdctRepository = null;
        //private static IProductRepository PrdctRepository
        //{
        //    get
        //    {
        //        if (_prdctRepository == null)
        //            _prdctRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        //        return _prdctRepository;
        //    }
        //}
        public event FillRepair FillingRepairDefects;

        private IList<RepairDefect> _defects = null;
        private object _syncObj_defects = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public Repair()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Repair(int id, string sn, string model, string type, string lineid, string station, RepairStatus status, /*int returnID,*/ string editor, string testlogid, int logId, DateTime cdt, DateTime udt)
        {
            this._id = id;
            this._sn = sn;
            this._model = model;
            this._type = type;
            this._lineid = lineid;
            this._station = station;
            this._status = status;
            //this._returnID = returnID;
            this._editor = editor;
            this._testLogID = testlogid;
            this._logId = logId;
            this._cdt = cdt;
            this._udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        public Repair(int id, string sn, string model, string type, string lineid, string station, RepairStatus status, /*int returnID,*/ IList<RepairDefect> defects, string editor, string testlogid, int logId, DateTime cdt, DateTime udt)
        {
            this._id = id;
            this._sn = sn;
            this._model = model;
            this._type = type;
            this._lineid = lineid;
            this._station = station;
            this._status = status;
            //this._returnID = returnID;
            this._editor = editor;
            this._testLogID = testlogid;
            this._logId = logId;
            this._cdt = cdt;
            this._udt = udt;

            this._tracker.MarkAsAdded(this);

            _defects = new List<RepairDefect>();
            foreach (var defect in defects)
            {
                defect.Tracker = this._tracker.Merge(defect.Tracker);
                this._defects.Add(defect);
                this._tracker.MarkAsAdded(defect);
                this._tracker.MarkAsModified(this);
            }

        }

        #region . Essential Fields .

        [ORMapping(mtns.Pcbrepair.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns.Pcbrepair.fn_pcbno)]
        private string _sn = null;
        [ORMapping(mtns.Pcbrepair.fn_pcbmodelid)]
        private string _model = null;
        [ORMapping(mtns.Pcbrepair.fn_type)]
        private string _type = null;
        [ORMapping(mtns.Pcbrepair.fn_line)]
        private string _lineid = null;
        [ORMapping(mtns.Pcbrepair.fn_station)]
        private string _station = null;
        //[ORMapping(mtns.Pcbrepair.fn_status)]
        private RepairStatus _status;
        //private int _returnID;
        [ORMapping(mtns.Pcbrepair.fn_editor)]
        private string _editor = null;
        //[ORMapping(mtns.Pcbrepair.fn_testLogID)]
        private string _testLogID = null;
        [ORMapping(mtns.Pcbrepair.fn_logID)]
        private int _logId;
        [ORMapping(mtns.Pcbrepair.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns.Pcbrepair.fn_udt)]
        private DateTime _udt = DateTime.MinValue;


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
        /// Repair记录所属的主板/机器标识
        /// </summary>
        public string Sn
        {
            get
            {
                return this._sn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._sn = value;
            }
        }

        /// <summary>
        /// 机型
        /// </summary>
        public string Model
        {
            get
            {
                return this._model;
            }
            private set
            {
                this._tracker.MarkAsModified(this);
                this._model = value;
            }
        }

        /// <summary>
        /// ???
        /// </summary>
        public string Type
        {
            get
            {
                return this._type;
            }
            private set
            {
                this._tracker.MarkAsModified(this);
                this._type = value;
            }
        }

        /// <summary>
        /// 维修站Line
        /// </summary>
        public string LineID
        {
            get
            {
                return this._lineid;
            }
            private set
            {
                this._tracker.MarkAsModified(this);
                this._lineid = value;
            }
        }

        /// <summary>
        /// 维修站
        /// </summary>
        public string Station
        {
            get
            {
                return this._station;
            }
            private set
            {
                this._tracker.MarkAsModified(this);
                this._station = value;
            }
        }

        /// <summary>
        /// 维修状态 
        /// </summary>
        public RepairStatus Status
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

        ///// <summary>
        ///// FA维修站换主板操作对应的PCA RepairID
        ///// </summary>
        //public int ReturnID
        //{
        //    get
        //    {
        //        return this._returnID;
        //    }
        //    private set
        //    {
        //        this._tracker.MarkAsModified(this);
        //        this._returnID = value;
        //    }
        //}

        /// <summary>
        /// 维护人员
        /// </summary>
        public string Editor
        {
            get
            {
                return this._editor;
            }
            private set
            {
                this._tracker.MarkAsModified(this);
                this._editor = value;
            }
        }

        /// <summary>
        /// TestLogID
        /// </summary>
        public string TestLogID
        {
            get
            {
                return this._testLogID;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._testLogID = value;
            }
        }

        /// <summary>
        /// LogId(PCBLog or ProductLog)
        /// </summary>
        public int LogId
        {
            get { return _logId; }
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
            private set
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
            private set
            {
                this._tracker.MarkAsModified(this);
                this._udt = value;
            }
        }

        #endregion

        /// <summary>
        /// 设置维修状态
        /// </summary>
        /// <param name="status">维修状态</param>
        public void SetStatus(RepairStatus status)
        {
            this._tracker.MarkAsModified(this);
            this._status = status;
        }

        ///<summary>
        /// 设置Line
        ///</summary>
        ///<param name="line">Line</param>
        public void SetLine(string line)
        {
            this._tracker.MarkAsModified(this);
            this.LineID = line;
        }

        /// <summary>
        /// 设置Station
        /// </summary>
        /// <param name="station">Station</param>
        public void SetStation(string station)
        {
            this._tracker.MarkAsModified(this);
            this.Station = station;
        }

        /// <summary>
        /// 设置Editor
        /// </summary>
        /// <param name="editor">Editor</param>
        public void SetEditor(string editor)
        {
            this._tracker.MarkAsModified(this);
            this.Editor = editor;
        }

        /// <summary>
        /// 一次维修的Defect列表
        /// </summary>
        public IList<RepairDefect> Defects 
        { 
            get 
            {
                lock (_syncObj_defects)
                {
                    if (_defects == null && FillingRepairDefects != null)
                    {
                        FillingRepairDefects(this);
                    }
                    if (_defects != null)
                    {
                        return new ReadOnlyCollection<RepairDefect>(_defects);
                    }
                    else
                        return null;

                }
            } 
        }

        internal void AddRepairDefect(RepairDefect repdfct)
        {
            if (repdfct == null)
                return;

            lock (_syncObj_defects)
            {
                //if (this._defects == null)
                //    this._defects = new List<RepairDefect>();
                object naught = this.Defects;
                if (this._defects.Contains(repdfct))
                    return;

                repdfct.Tracker = this._tracker.Merge(repdfct.Tracker);
                this._defects.Add(repdfct);
                this._tracker.MarkAsAdded(repdfct);
                this._tracker.MarkAsModified(this);
            }
        }

        internal void RemoveRepairDefect(int repairDefectId)
        {
            lock (_syncObj_defects)
            {
                object naught = this.Defects;
                if (this._defects == null)
                    return;
                RepairDefect dfctFound = null;
                foreach (RepairDefect dfct in this._defects)
                {
                    if (dfct.Key.Equals(repairDefectId))
                    {
                        dfctFound = dfct;
                        break;
                    }
                }
                if (dfctFound != null)
                {
                    //this._defects.Remove(dfctFound);
                    dfctFound.Tracker = null;
                    this._tracker.MarkAsDeleted(dfctFound);
                    this._tracker.MarkAsModified(this);
                }
            }
        }

        internal void UpdateRepairDefect(RepairDefect repdfct)
        {
            if (repdfct == null)
                return;

            lock (_syncObj_defects)
            {
                object naught = this.Defects;
                if (this._defects == null)
                    return;
                int idx = 0;
                bool find = false;
                foreach (RepairDefect dfct in this._defects)
                {
                    if (dfct.Key.Equals(repdfct.Key))
                    {
                        find = true;
                        break;
                    }
                    idx++;
                }
                if (find)
                {
                    this._defects[idx] = repdfct;
                    this._tracker.MarkAsModified(this._defects[idx]);
                    this._tracker.MarkAsModified(this);
                }
            }
        }

        private int setter_LogId
        {
            set { _logId = value; }
        }

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
