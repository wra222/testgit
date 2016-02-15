using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Warranty
{
    ///<summary>
    /// Warranty对应实体
    ///</summary>
    [ORMapping(typeof(mtns.Warranty))]
    public class Warranty : FisObjectBase, IAggregateRoot
    {
        public Warranty()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Warranty(int id, string customer, string type, string dateCodeType, string warrantyFormat, string shipTypeCode, string warrantyCode, string descr, string editor, DateTime cdt, DateTime udt)
        {
            _id = id;
            _customer = customer;
            _type = type;
            _dateCodeType = dateCodeType;
            _warrantyFormat = warrantyFormat;
            _shipTypeCode = shipTypeCode;
            _warrantyCode = warrantyCode;
            _descr = descr;
            _editor = editor;
            _cdt = cdt;
            _udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        [ORMapping(mtns.Warranty.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns.Warranty.fn_customer)]
        private string _customer = null;
        [ORMapping(mtns.Warranty.fn_type)]
        private string _type = null;
        [ORMapping(mtns.Warranty.fn_dateCodeType)]
        private string _dateCodeType = null;
        [ORMapping(mtns.Warranty.fn_warrantyFormat)]
        private string _warrantyFormat = null;
        [ORMapping(mtns.Warranty.fn_shipTypeCode)]
        private string _shipTypeCode = null;
        [ORMapping(mtns.Warranty.fn_warrantyCode)]
        private string _warrantyCode = null;
        [ORMapping(mtns.Warranty.fn_descr)]
        private string _descr = null;
        [ORMapping(mtns.Warranty.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.Warranty.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns.Warranty.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        ///<summary>
        /// Id
        ///</summary>
        public int Id 
        {
            get { return _id; }
            set
            {
                this._tracker.MarkAsModified(this);
                _id = value;
            }
        }

        ///<summary>
        /// Customer
        ///</summary>
        public string Customer
        {
            get { return _customer; }
            set
            {
                this._tracker.MarkAsModified(this);
                _customer = value;
            }
        }

        ///<summary>
        /// Type
        ///</summary>
        public string Type
        {
            get { return _type; }
            set
            {
                this._tracker.MarkAsModified(this);
                _type = value;
            }
        }

        ///<summary>
        /// DateCodeType
        ///</summary>
        public string DateCodeType
        {
            get { return _dateCodeType; }
            set
            {
                this._tracker.MarkAsModified(this);
                _dateCodeType = value;
            }
        }

        ///<summary>
        /// WarrantyFormat
        ///</summary>
        public string WarrantyFormat
        {
            get { return _warrantyFormat; }
            set
            {
                this._tracker.MarkAsModified(this);
                _warrantyFormat = value;
            }
        }

        ///<summary>
        /// ShipTypeCode
        ///</summary>
        public string ShipTypeCode
        {
            get { return _shipTypeCode; }
            set
            {
                this._tracker.MarkAsModified(this);
                _shipTypeCode = value;
            }
        }

        ///<summary>
        /// WarrantyCode
        ///</summary>
        public string WarrantyCode
        {
            get { return _warrantyCode; }
            set
            {
                this._tracker.MarkAsModified(this);
                _warrantyCode = value;
            }
        }

        /// <summary>
        /// PdLine描述
        /// </summary>
        public string Descr
        {
            get { return _descr; }
            set
            {
                this._tracker.MarkAsModified(this);
                _descr = value;
            }
        }

        /// <summary>
        /// 维护人员
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
        /// 创建时间
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
        /// 更新时间
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


        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _id; }
        }

        /// <summary>
        /// Warranty类型
        /// </summary>
        public static class WarrantyType
        {
            public const string KPDateCode = "KPDateCode";
            public const string MBDateCode = "MBDateCode";
            public const string VBDateCode = "VBDateCode";
            public const string DKDateCode = "DKDateCode";
        }

        /// <summary>
        /// ShippingTypeCode
        /// </summary>
        public static class ShippingTypeCode
        {
            public const string J = "J";//J=正批货,
            public const string H = "H";//H=IEC FRU,
            public const string C = "C";//C=IPC FRU,
            public const string P = "P";//P=IPC RMA,
        }

    }
}
