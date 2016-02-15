// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应COA_Log表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.COA
{
    /// <summary>
    /// COALog业务对象
    /// </summary>
    [ORMapping(typeof(Coalog))]
    public class COALog : FisObjectBase
    {
        public COALog()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(Coalog.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(Coalog.fn_coasn)]
        private string _coasn = null;
        [ORMapping(Coalog.fn_station)]
        private string _stationid = null;
        [ORMapping(Coalog.fn_line)]
        private string _lineid = null;
        [ORMapping(Coalog.fn_editor)]
        private string _editor = null;
        [ORMapping(Coalog.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(Coalog.fn_tp)]
        private string _tp = null;
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
        /// 站别
        /// </summary>
        public string StationID
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
        /// Editor
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
        /// Tp
        /// </summary>
        public string Tp
        {
            get
            {
                return this._tp;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._tp = value;
            }
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _id; }
        }

        #endregion
    }
}
