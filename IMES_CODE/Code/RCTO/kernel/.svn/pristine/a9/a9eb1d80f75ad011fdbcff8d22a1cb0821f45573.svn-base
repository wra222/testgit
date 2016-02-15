// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应COA_Status表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// 2009-10-26   Yuan XiaoWei                 ITC-1155-0112
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.ObjectModel;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.COA
{
    /// <summary>
    /// COA状态
    /// </summary>
    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.Coastatus))]
    public class COAStatus : FisObjectBase, IAggregateRoot
    {
        public static class COAStatusConst
        {
            public const string P1 = "P1";
            public const string P2 = "P2";
            public const string A1 = "A1";
            public const string A2 = "A2";
            public const string A3 = "A3";
        }

        private static ICOAStatusRepository _coaRepository = null;
        private static ICOAStatusRepository COARepository
        {
            get
            {
                if (_coaRepository == null)
                    _coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                return _coaRepository;
            }
        }

        public COAStatus()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coastatus.fn_coasn)]
        private string _coasn = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coastatus.fn_iecpn)]
        private string _iecpn = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coastatus.fn_status)]
        private string _status = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coastatus.fn_line)]
        private string _lineid = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coastatus.fn_editor)]
        private string _editor = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coastatus.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coastatus.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        /// <summary>
        /// COA序号
        /// </summary>
        public string COASN 
        {
            get
            {
                return this._coasn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._coasn = value;
            }
        }

        /// <summary>
        /// 料号
        /// </summary>
        public string IECPN
        {
            get
            {
                return this._iecpn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._iecpn = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
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
        /// 产线
        /// </summary>
        public string LineID
        {
            get
            {
                return this._lineid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._lineid = value;
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

        #endregion

        #region . Sub-Instances .

        private IList<COALog> _coaLogs = null;
        private object _syncObj_coaLogs = new object();

        public IList<COALog> CoaLogs
        {
            get 
            {
                lock (_syncObj_coaLogs)
                {
                    if (_coaLogs == null)
                    {
                        COARepository.FillCOALogs(this);
                    }
                    else
                    {
                        IList<COALog> szAllAdd = new List<COALog>();
                        foreach (COALog cl in _coaLogs)
                        {
                            if (cl.ID > 0)
                            {
                                szAllAdd.Clear();
                                break;
                            }
                            else
                            {
                                szAllAdd.Add(cl);
                            }
                        }
                        if (szAllAdd.Count > 0)
                        {
                            COARepository.FillCOALogs(this);
                            foreach (COALog cl in szAllAdd)
                            {
                                _coaLogs.Add(cl);
                            }
                        }
                    }
                    if (_coaLogs != null)
                    {
                        return new ReadOnlyCollection<COALog>(_coaLogs);
                    }
                    else
                        return null;
                }
            }
        }

        public void addCOALog(COALog newLog)
        {
            if (newLog == null)
                return;

            lock (_syncObj_coaLogs)
            {
                if (_coaLogs == null)
                {
                    _coaLogs = new List<COALog>();
                }

                //object naught = this.CoaLogs;
                if (this._coaLogs.Contains(newLog))
                    return;

                newLog.Tracker = this._tracker.Merge(newLog.Tracker);
                this._coaLogs.Add(newLog);
                this._tracker.MarkAsAdded(newLog);
                this._tracker.MarkAsModified(this);
            }
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _coasn; }
        }

        #endregion

    }
}
