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

namespace IMES.FisObject.FA.Product
{
    /// <summary>
    /// Product的过站记录，该类实例不会脱离Product对象单独存在；
    /// </summary>
    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.ProductLog))]
    public class ProductLog : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductLog()
        {
            this._tracker.MarkAsAdded(this);
        }
        [ORMapping(IMES.Infrastructure.Repository._Metas.ProductLog.fn_id)]        
        private int _id = int.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.ProductLog.fn_model)]
        private string _model = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.ProductLog.fn_station)]
        private string _station = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.ProductLog.fn_status)] 
        private StationStatus _status = StationStatus.NULL;
        [ORMapping(IMES.Infrastructure.Repository._Metas.ProductLog.fn_line)]
        private string _line = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.ProductLog.fn_editor)]
        private string _editor = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.ProductLog.fn_cdt)] 
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.ProductLog.fn_productID)]
        private string _productID = null;

        /// <summary>
        /// Product ID
        /// </summary>
        public string ProductID
        {
            get
            {
                return _productID;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _productID = value;
            }
        }

        /// <summary>
        /// Log的ID
        /// </summary>
        public int Id 
        {
            get
            {
                return _id;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// Product机型
        /// </summary>
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /// <summary>
        /// Product经过的Station
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
        /// Product在该Station处理的结果
        /// </summary>
        public StationStatus Status
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
        /// Product所在的Line
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
        /// 在该站处理Product的员工编号
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
        /// Product完成某站的日期时间
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