// 2010-05-31 Liu Dong(eB1-4)         Modify ChangeLog的Cdt可以由上面传下来.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.FA.Product
{
    /// <summary>
    /// Product修改Log
    /// </summary>
    public class ProductChangeLog : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductChangeLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        private int _id;
        private string _productid;
        private string _mo;
        private string _station;
        private string _editor;
        private DateTime _cdt = DateTime.MinValue;// 2010-05-31 Liu Dong(eB1-4)         Modify ChangeLog的Cdt可以由上面传下来.

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _id; }
            set 
            {
                this._tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// Product Id
        /// </summary>
        public string ProductID
        {
            get { return _productid; }
            set
            {
                this._tracker.MarkAsModified(this);
                _productid = value;
            }
        }

        /// <summary>
        /// Mo of product
        /// </summary>
        public string Mo
        {
            get { return _mo; }
            set
            {
                this._tracker.MarkAsModified(this);
                _mo = value;
            }
        }

        /// <summary>
        /// Station
        /// </summary>
        public string Station
        {
            get { return _station; }
            set
            {
                this._tracker.MarkAsModified(this);
                _station = value;
            }
        }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get { return _editor; }
            set
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
            get { return _cdt; }
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
