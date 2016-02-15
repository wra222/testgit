using System;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.FA.Product
{
    /// <summary>
    /// Product制程控制状态
    /// </summary>
    public class ProductAttribute : FisObjectBase
    {
        #region . Essential Fields .

        private string _productId;
        private string _attributeName;
        private string _attributeValue;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

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
        /// AttributeValue
        /// </summary>
        public string AttributeValue
        {
            get { return _attributeValue; }
            set
            {
                _tracker.MarkAsModified(this);
                _attributeValue = value;
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

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt
        {
            get { return _udt; }
            set
            {
                _tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        #endregion

        #region . Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductAttribute()
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
            get { return _productId + _attributeName; }
        }

        #endregion
    }
}
