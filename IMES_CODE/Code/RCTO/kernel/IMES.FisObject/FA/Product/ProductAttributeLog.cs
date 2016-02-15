using System;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.FA.Product
{
    /// <summary>
    /// Product制程控制状态变更Log
    /// </summary>
    public class ProductAttributeLog: FisObjectBase
    {
        #region . Essential Fields .

        private int _id;
        private string _productId;
        public string _model;
        public string _station;
        public string _descr;
        private string _attributeName;
        private string _newValue;
        private string _oldValue;
        private string _editor;
        private DateTime _cdt;

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                _tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// ProductId
        /// </summary>
        public string ProductId
        {
            get { return _productId; }
            set
            {
                _tracker.MarkAsModified(this);
                _productId = value;
            }
        }


        /// <summary>
        /// AttributeName
        /// </summary>
        public string AttributeName
        {
            get { return _attributeName; }
            set
            {
                _tracker.MarkAsModified(this);
                _attributeName = value;
            }
        }
        
        /// <summary>
        /// New attribute value
        /// </summary>
        public string NewValue
        {
            get { return _newValue; }
            set
            {
                _tracker.MarkAsModified(this);
                _newValue = value;
            }
        }

        /// <summary>
        /// Old attribute value
        /// </summary>
        public string OldValue
        {
            get { return _oldValue; }
            set
            {
                _tracker.MarkAsModified(this);
                _oldValue = value;
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                _tracker.MarkAsModified(this);
                _model = value;
            }
        }

        public string Station
        {
            get { return _station; }
            set
            {
                _tracker.MarkAsModified(this);
                _station = value;
            }
        }

        public string Descr
        {
            get { return _descr; }
            set
            {
                _tracker.MarkAsModified(this);
                _descr = value;
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
                _tracker.MarkAsModified(this);
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
                _tracker.MarkAsModified(this);
                _cdt = value;
            }
        }

        #endregion

        #region . Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductAttributeLog()
        {
            _tracker.MarkAsAdded(this);
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
