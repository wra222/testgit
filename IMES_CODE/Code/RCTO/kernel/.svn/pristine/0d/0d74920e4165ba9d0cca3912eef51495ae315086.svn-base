// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yang WeiHua                 create
// Known issues:
using System;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.FA.Product
{
    /// <summary>
    /// product的过站状态，改类完全从属于Product类，其实例不应脱离Product类而存在；
    /// </summary>
    [ORMapping(typeof(mtns.ProductStatus))]
    public class ProductStatus : FisObjectBase
    {
        ///<summary>
        /// Product状态
        ///</summary>
        public ProductStatus()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ProductStatus(string proId, string stationId, string line, string reworkCode, string editor, DateTime udt, DateTime cdt, StationStatus status)
        {
            _proId = proId;
            _stationId = stationId;
            _line = line;
            _reworkCode = reworkCode;
            _editor = editor;
            _testFailCount = 0;
            _udt = udt;
            _cdt = cdt;
            _status = status;
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ProductStatus(string proId, string stationId, string line, string reworkCode, int testFailCount, string editor, DateTime udt, DateTime cdt, StationStatus status)
        {
            _proId = proId;
            _stationId = stationId;
            _line = line;
            _reworkCode = reworkCode;
            _editor = editor;
            _testFailCount = testFailCount;
            _udt = udt;
            _cdt = cdt;
            _status = status;
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns.ProductStatus.fn_productID)]
        private string _proId;
        [ORMapping(mtns.ProductStatus.fn_station)]
        private string _stationId;
        [ORMapping(mtns.ProductStatus.fn_line)]
        private string _line;
        [ORMapping(mtns.ProductStatus.fn_reworkCode)]
        private string _reworkCode;
        [ORMapping(mtns.ProductStatus.fn_testFailCount)]
        private int _testFailCount = 0;
        [ORMapping(mtns.ProductStatus.fn_editor)]
        private string _editor;
        [ORMapping(mtns.ProductStatus.fn_udt)]
        private DateTime _udt;
        [ORMapping(mtns.ProductStatus.fn_cdt)]
        private DateTime _cdt;
        [ORMapping(mtns.ProductStatus.fn_status)]
        private StationStatus _status;

        /// <summary>
        /// Product Id
        /// </summary>
        public string ProId
        {
            get { return _proId; }
            set
            {
                _tracker.MarkAsModified(this);
                _proId = value;
            }
        }

        /// <summary>
        /// Product经过的前站站号
        /// </summary>
        public string StationId 
        {
            get
            {
                return this._stationId;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._stationId = value;
            }
        }

        /// <summary>
        /// Product所在线别
        /// </summary>
        public string Line
        {
            get
            {
                return this._line;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._line = value;
            }
        }

        /// <summary>
        /// Rework code
        /// </summary>
        public string ReworkCode
        {
            get
            {
                return this._reworkCode;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._reworkCode = value;
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
        /// 操作Product的员工编号
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
        /// Product状态的更新日期时间
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
        /// Product状态记录的创建日期时间
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
        /// Product在前站的处理结果
        /// </summary>
        public StationStatus Status 
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

        #endregion

        #region . Overrides of FisObjectBase .

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._proId; }
        }

        #endregion
    }
}