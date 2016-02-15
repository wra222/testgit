using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.FA.Product
{
    [ORMapping(typeof(mtns.ProductInfo))]
    public class ProductInfo : FisObjectBase
    {
        /// <summary>
        /// Product的扩展属性
        /// </summary>
        public ProductInfo()
        {
            this._tracker.MarkAsAdded(this);
        }

        [ORMapping(mtns.ProductInfo.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns.ProductInfo.fn_productID)]
        private string _productid;
        [ORMapping(mtns.ProductInfo.fn_infoType)]
        private string _infotype;
        [ORMapping(mtns.ProductInfo.fn_infoValue)]
        private string _infovalue;
        [ORMapping(mtns.ProductInfo.fn_editor)]
        private string _editor;
        [ORMapping(mtns.ProductInfo.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns.ProductInfo.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

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
        /// Prodcut Id
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
        /// 属性名
        /// </summary>
        public string InfoType
        {
            get { return _infotype; }
            set
            {
                this._tracker.MarkAsModified(this);
                _infotype = value;
            }
        }

        /// <summary>
        /// 属性值
        /// </summary>
        public string InfoValue
        {
            get { return _infovalue; }
            set
            {
                this._tracker.MarkAsModified(this);
                _infovalue = value;
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

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt
        {
            get { return _udt; }
            set
            {
                this._tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        private bool _isInsertingOrUpdating = false;
        public bool IsInsertingOrUpdating
        {
            get 
            { 
                //bool ret = false;
                //if (this._tracker != null)
                //{
                //    var stt = this._tracker.GetState(this);
                //    logger.InfoFormat("ProductInfo Tracker: {0}, ProductID: {1}, Value: {2}", stt.ToString(), this.ProductID, this.InfoValue);
                //    ret = (stt == System.Data.DataRowState.Added || stt == System.Data.DataRowState.Modified);
                //    logger.InfoFormat("ProductInfo IsInsertingOrUpdating: {0}, ProductID: {1}, Value: {2}", ret.ToString(), this.ProductID, this.InfoValue);
                //}
                //return ret;
                return _isInsertingOrUpdating;
            }
            set
            {
                _isInsertingOrUpdating = value;
            }
        }

        public bool isWritenBoxId = false;

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
