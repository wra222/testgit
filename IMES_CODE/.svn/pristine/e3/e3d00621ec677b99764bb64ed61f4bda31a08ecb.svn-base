using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.FA.Product
{
    ///<summary>
    /// product抽样结果记录
    ///</summary>
    [ORMapping(typeof(Qcstatus))]
    public class ProductQCStatus : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductQCStatus()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductQCStatus(int id, string productID, string type, string line, string model, /*DateTime date, string custSN,*/ string status, string editor, DateTime cdt, DateTime udt)
        {
            _id = id;
            _productID = productID;
            _type = type;
            _line = line;
            _model = model;
            //_date = date;
            //_custSN = custSN;
            _status = status;
            _editor = editor;
            _cdt = cdt;
            _udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        [ORMapping(Qcstatus.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(Qcstatus.fn_productID)]
        private string _productID = null;
        [ORMapping(Qcstatus.fn_tp)]
        private string _type = null;
        [ORMapping(Qcstatus.fn_line)]
        private string _line = null;
        [ORMapping(Qcstatus.fn_model)]
        private string _model = null;
        //private DateTime _date;
        //private string _custSN;
        [ORMapping(Qcstatus.fn_status)]
        private string _status = null;
        [ORMapping(Qcstatus.fn_editor)]
        private string _editor = null;
        [ORMapping(Qcstatus.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(Qcstatus.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(Qcstatus.fn_remark)]
        private string _remark = null;

        /// <summary>
        /// ID
        /// </summary>
        public int ID 
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
        /// ProductID
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
        /// Type
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _type = value;
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
            private set
            {
                this._tracker.MarkAsModified(this);
                _line = value;
            }
        }

        ///<summary>
        ///</summary>
        public string Model
        {
            get
            {
                return _model;
            }
            private set
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /////<summary>
        ///// Date
        /////</summary>
        //public DateTime Date
        //{
        //    get
        //    {
        //        return _date;
        //    }
        //    private set
        //    {
        //        this._tracker.MarkAsModified(this);
        //        _date = value;
        //    }
        //}

        ///// <summary>
        ///// CustSN
        ///// </summary>
        //public string CustSN
        //{
        //    get
        //    {
        //        return _custSN;
        //    }
        //    private set
        //    {
        //        this._tracker.MarkAsModified(this);
        //        _custSN = value;
        //    }
        //}

        /// <summary>
        /// Status
        /// </summary>
        public string Status
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
        /// Editor
        /// </summary>
        public string Editor
        {
            get
            {
                return _editor;
            }
            private set
            {
                this._tracker.MarkAsModified(this);
                _editor = value;
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
            private set
            {
                this._tracker.MarkAsModified(this);
                _cdt = value;
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
            private set
            {
                this._tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _remark = value;
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
