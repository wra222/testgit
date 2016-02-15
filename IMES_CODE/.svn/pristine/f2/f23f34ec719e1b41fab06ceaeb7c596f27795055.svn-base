// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应COAReceive表
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

namespace IMES.FisObject.PAK.COA
{
    /// <summary>
    /// 对应COAReceive表,产线接收到的COA记录
    /// </summary>
    [ORMapping(typeof(Coareceive))]
    public class COAReceive : FisObjectBase
    {
        public COAReceive()
        {
            this._tracker.MarkAsAdded(this);
        }

        [ORMapping(Coareceive.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(Coareceive.fn_begSN)]
        private string _BegSN = null;
        [ORMapping(Coareceive.fn_endSN)]
        private string _EndSN = null;
        [ORMapping(Coareceive.fn_po)]
        private string _PO = null;
        [ORMapping(Coareceive.fn_custPN)]
        private string _CustPN = null;
        [ORMapping(Coareceive.fn_iecpn)]
        private string _IECPN = null;
        [ORMapping(Coareceive.fn_mspn)]
        private string _MSPN = null;
        [ORMapping(Coareceive.fn_descr)]
        private string _Descr = null;
        [ORMapping(Coareceive.fn_shipDate)]
        private DateTime _ShipDate = DateTime.MinValue;
        [ORMapping(Coareceive.fn_qty)]
        private int _Qty = int.MinValue;
        [ORMapping(Coareceive.fn_cust)]
        private string _Cust = null;
        [ORMapping(Coareceive.fn_upload)]
        private string _Upload = null;
        [ORMapping(Coareceive.fn_editor)]
        private string _editor = null;
        [ORMapping(Coareceive.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(Coareceive.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._id = value;
            }
        }

        /// <summary>
        /// 开始号码
        /// </summary>
        public string BegSN
        {
            get
            {
                return this._BegSN;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._BegSN = value;
            }
        }

        /// <summary>
        /// 结束号码
        /// </summary>
        public string EndSN
        {
            get
            {
                return this._EndSN;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._EndSN = value;
            }
        }


        /// <summary>
        /// PO
        /// </summary>
        public string PO
        {
            get
            {
                return this._PO;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._PO = value;
            }
        }

        /// <summary>
        /// 客户PN
        /// </summary>
        public string CustPN
        {
            get
            {
                return this._CustPN;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._CustPN = value;
            }
        }

        /// <summary>
        /// IECPN
        /// </summary>
        public string IECPN
        {
            get
            {
                return this._IECPN;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._IECPN = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MSPN
        {
            get
            {
                return this._MSPN;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._MSPN = value;
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descr
        {
            get
            {
                return this._Descr;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Descr = value;
            }
        }


        /// <summary>
        /// 出货日期
        /// </summary>
        public DateTime ShipDate
        {
            get
            {
                return this._ShipDate;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._ShipDate = value;
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get
            {
                return this._Qty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Qty = value;
            }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public string Cust
        {
            get
            {
                return this._Cust;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Cust = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Upload
        {
            get
            {
                return this._Upload;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Upload = value;
            }
        }

        /// <summary>
        /// 维护人员
        /// </summary>
        public string Editor
        {
            get
            {
                return this._editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._editor = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cdt = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Udt
        {
            get
            {
                return this._udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._udt = value;
            }
        }

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
