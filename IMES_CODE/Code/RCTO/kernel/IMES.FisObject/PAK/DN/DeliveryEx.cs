using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.DN
{
    public class DeliveryEx : FisObjectBase
    {
        public DeliveryEx()
        {
            this._tracker.MarkAsAdded(this);
        }

        private string _deliveryNo = "";
        private string _shipmentNo = "";
        private string _shipType = "";
        private string _palletType = "";
        private int _consolidateQty = 0;
        private int _qtyPerCarton = 0;
        private int _cartonQty = 0;
        private string _messageCode = "";
        private string _shipToParty = "";
        private string _priority = "";
        private string _groupId = "";
        private string _orderType = "";
        private string _bol = "";
        private string _hawb = "";
        private string _carrier = "";
        private string _shipWay = "";
        private string _packID ="";
        private string _stdPltFullQty = "";
        private string _stdPltStackType = "";


        private string _editor = "";


        private DateTime _udt = DateTime.MinValue;


        #region . Essential Fields .

        /// <summary>
        /// Delivery No.
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
        /// shipment
        /// </summary>
        public string ShipmentNo
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

        public string ShipType
        {
            get
            {
                return _shipType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _shipType = value;
            }
        }

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

        public int ConsolidateQty
        {
            get
            {
                return _consolidateQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _consolidateQty = value;
            }
        }

         public int QtyPerCarton
        {
            get
            {
                return _qtyPerCarton;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _qtyPerCarton = value;
            }
        }

        public int CartonQty
        {
            get
            {
                return _cartonQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _cartonQty = value;
            }
        }

        public string MessageCode
        {
            get
            {
                return _messageCode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _messageCode = value;
            }
        }

        public string ShipToParty
        {
            get
            {
                return _shipToParty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _shipToParty = value;
            }
        }

        public string Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _priority = value;
            }
        }

        public string GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _groupId = value;
            }
        }

        public string OrderType
        {
            get
            {
                return _orderType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _orderType = value;
            }
        }

        public string BOL
        {
            get
            {
                return _bol;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _bol = value;
            }
        }

        public string HAWB
        {
            get
            {
                return _hawb;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _hawb = value;
            }
        }

        public string Carrier
        {
            get
            {
                return _carrier;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _carrier = value;
            }
        }

        public string ShipWay
        {
            get
            {
                return _shipWay;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _shipWay = value;
            }
        }

        public string PackID
        {
            get
            {
                return _packID;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _packID = value;
            }
        }

        public string StdPltFullQty
        {
            get
            {
                return _stdPltFullQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _stdPltFullQty = value;
            }
        }

        public string StdPltStackType
        {
            get
            {
                return _stdPltStackType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _stdPltStackType = value;
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
            get { return this._deliveryNo; }
        }

        #endregion
    }
}
