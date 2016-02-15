// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Delivery的扩展属性类
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-31   Yuan XiaoWei                 create
// Known issues:

using System;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.DN
{
    /// <summary>
    /// Delivery的扩展属性类
    /// </summary>
    [ORMapping(typeof(mtns.DeliveryInfo))]
    public class DeliveryInfo : FisObjectBase, IAggregateRoot
    {
        public DeliveryInfo()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns.DeliveryInfo.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns.DeliveryInfo.fn_deliveryNo)]
        private string _deliveryNo = null;
        [ORMapping(mtns.DeliveryInfo.fn_infoType)]
        private string _infoType = null;
        [ORMapping(mtns.DeliveryInfo.fn_infoValue)]
        private string _infoValue = null;
        [ORMapping(mtns.DeliveryInfo.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.DeliveryInfo.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns.DeliveryInfo.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        /// <summary>
        /// 自增ID
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
        /// DN号码
        /// </summary>
        public string DeliveryNo
        {
            get
            {
                return _deliveryNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _deliveryNo = value;
            }
        }

        /// <summary>
        /// 扩展属性的名称
        /// </summary>
        public string InfoType
        {
            get
            {
                return _infoType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _infoType = value;
            }
        }

        /// <summary>
        /// 扩展属性的值
        /// </summary>
        public string InfoValue
        {
            get
            {
                return _infoValue;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _infoValue = value;
            }
        }

        /// <summary>
        /// 维护人员工号
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
        /// 创建时间
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

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Udt
        {
            get
            {
                return _udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _udt = value;
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