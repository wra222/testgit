// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应DeliveryPallet表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-04   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.DN
{
    /// <summary>
    /// Delivery和Pallet绑定关系表
    /// </summary>
    public class DeliveryPallet : FisObjectBase
    {
        public DeliveryPallet()
        {
            this._tracker.MarkAsAdded(this);
        }

        private int _id = int.MinValue;

        private string _deliveryNo = null;

        private string _palletNo = null;

        private string _shipmentNo = null;

        private string _status = null;

        private short _deliveryQty = short.MinValue;

        private string _editor = null;

        private DateTime _cdt = DateTime.MinValue;

        private DateTime _udt = DateTime.MinValue;

        private int _deviceQty = 0;

        private string _palletType = null;

        /// <summary>
        /// DN在该Pallet上包含的箱數数量
        /// </summary>
        public short DeliveryQty
        {
            get
            {
                return _deliveryQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _deliveryQty = value;
            }
        }

        /// <summary>
        /// DN在该Pallet上包含的机器数量
        /// </summary>
        public int DeviceQty
        {
            get
            {
                return _deviceQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _deviceQty = value;
            }
        }

        /// <summary>
        /// DN在该Pallet 的 PalletType = ZP /ZC 
        ///  </summary>
        public string PalletType
        {
            get
            {
                return _palletType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _palletType = value;
            }
        }

        #region . Essential Fields .

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
        /// Delivery No.
        /// </summary>
        public string DeliveryID
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
        /// 栈板号码
        /// </summary>
        public string PalletID
        {
            get
            {
                return _palletNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _palletNo = value;
            }
        }



        /// <summary>
        /// shipment
        /// </summary>
        public string ShipmentID
        {
            get
            {
                return _shipmentNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _shipmentNo = value;
            }
        }

        /// <summary>
        /// 状态
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
