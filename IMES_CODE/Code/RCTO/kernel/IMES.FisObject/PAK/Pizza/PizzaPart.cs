// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应Pizza_Part表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-22   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.Pizza
{
    /// <summary>
    /// Pizza绑定的料
    /// </summary>
    [ORMapping(typeof(mtns::Pizza_Part))]    
    public class PizzaPart : FisObjectBase
    {
        public PizzaPart()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns::Pizza_Part.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns::Pizza_Part.fn_pizzaID)]
        private string _pizzaID = null;
        [ORMapping(mtns::Pizza_Part.fn_partNo)]
        private string _partID = null;
        [ORMapping(mtns::Pizza_Part.fn_station)]
        private string _station = null;
        [ORMapping(mtns::Pizza_Part.fn_partType)]
        private string _partType = null;
        [ORMapping(mtns::Pizza_Part.fn_custmerPn)]
        private string _customerPn = null;
        [ORMapping(mtns::Pizza_Part.fn_iecpn)]
        private string _iecpn = null;
        [ORMapping(mtns::Pizza_Part.fn_partSn)]
        private string _partSn = null;
        [ORMapping(mtns::Pizza_Part.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns::Pizza_Part.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns::Pizza_Part.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(mtns::Pizza_Part.fn_bomNodeType)]
        private string _bomNodeType = null;
        [ORMapping(mtns::Pizza_Part.fn_checkItemType)]
        private string _checkItemType = null;

        /// <summary>
        /// 记录标识
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
        /// PizzaID
        /// </summary>
        public string PizzaID
        {
            get { return _pizzaID; }
            set
            {
                this._tracker.MarkAsModified(this);
                _pizzaID = value;
            }
        }


        /// <summary>
        /// PartID
        /// </summary>
        public string PartNo
        {
            get { return _partID; }
            set
            {
                this._tracker.MarkAsModified(this);
                _partID = value;
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
        /// Part Type
        /// </summary>
        public string PartType
        {
            get
            {
                return _partType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _partType = value;
            }
        }

        public string CustomerPn
        {
            get { return _customerPn; }
            set
            {
                this._tracker.MarkAsModified(this);
                _customerPn = value;
            }
        }

        public string Iecpn
        {
            get { return _iecpn; }
            set
            {
                this._tracker.MarkAsModified(this);
                _iecpn = value;
            }
        }

        public string PartSn
        {
            get { return _partSn; }
            set
            {
                this._tracker.MarkAsModified(this);
                _partSn = value;
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
        /// BomNodeType
        /// </summary>
        public string BomNodeType
        {
            get { return _bomNodeType; }
            set
            {
                this._tracker.MarkAsModified(this);
                _bomNodeType = value;
            }
        }

        /// <summary>
        /// CheckItemType
        /// </summary>
        public string CheckItemType
        {
            get { return _checkItemType; }
            set
            {
                this._tracker.MarkAsModified(this);
                _checkItemType = value;
            }
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _pizzaID; }
        }

        #endregion
    }
}
