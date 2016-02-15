// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yang WeiHua                 create
// Known issues:
using System;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// ProductPart是Product/MB与Part的绑定关系类，该类实例不会脱离Product/MB对象单独存在；
    /// </summary>
    [ORMapping(typeof(Product_Part))]
    public class ProductPart : FisObjectBase, IProductPart
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductPart()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductPart(int id, string partid, string productid, string station, string partType, string customerPn, string iecpn, string partSn, string editor, DateTime udt, DateTime cdt)
        {
            _id = id;
            _partid = partid;
            _productid = productid;

            _station = station;

            _partType = partType;
            _customerPn = customerPn;
            _iecpn = iecpn;
            _partSn = partSn;

            _editor = editor;
            _udt = udt;
            _cdt = cdt;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(Product_Part.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(Product_Part.fn_partNo)]
        private string _partid = null;
        [ORMapping(Product_Part.fn_productID)]
        private string _productid = null;

        [ORMapping(Product_Part.fn_station)]
        private string _station = null;

        [ORMapping(Product_Part.fn_partType)]
        private string _partType = null;
        [ORMapping(Product_Part.fn_custmerPn)]
        private string _customerPn = null;
        [ORMapping(Product_Part.fn_iecpn)]
        private string _iecpn = null;
        [ORMapping(Product_Part.fn_partSn)]
        private string _partSn = null;

        [ORMapping(Product_Part.fn_editor)]
        private string _editor = null;
        [ORMapping(Product_Part.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(Product_Part.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;

        private string _value;

        private string _valueType;

        [ORMapping(Product_Part.fn_checkItemType)]
        private string _checkItemType = null;
        [ORMapping(Product_Part.fn_bomNodeType)]
        private string _bomNodeType = null;

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
        /// 绑定到Product的Part No
        /// </summary>
        public string PartID
        {
            get
            {
                return _partid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _partid = value;
            }
        }

        /// <summary>
        /// ProductID
        /// </summary>
        public string ProductID
        {
            get
            {
                return _productid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _productid = value;
            }
        }

        /// <summary>
        /// Part与Product的绑定站
        /// </summary>
        public string Station
        {
            get
            {
                return _station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _station = value;
            }
        }      

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
            get
            {
                return _customerPn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _customerPn = value;
            }
        }

        public string Iecpn
        {
            get
            {
                return _iecpn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _iecpn = value;
            }
        }

        public string PartSn
        {
            get
            {
                return _partSn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _partSn = value;
            }
        }

        /// <summary>
        /// 将Part与Product绑定的员工编号
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
        /// Product状态的更新日期时间
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

        /// <summary>
        /// Product状态记录的创建日期时间
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

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _value = value;
            }
        }

        public string ValueType
        {
            get
            {
                return _valueType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _valueType = value;
            }
        }

        public string CheckItemType
        {
            get
            {
                return _checkItemType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _checkItemType = value;
            }
        }

        public string BomNodeType
        {
            get
            {
                return _bomNodeType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _bomNodeType = value;
            }
        }

        public void SetValueSilently(string value)
        {
            _value = value;
        }

        public void SetPartTypeSilently(string value)
        {
            _partType = value;
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._partid; }
        }

        #endregion

        ///// <summary>
        ///// 有些类型的Part保存前需要对part数据进行特殊改变
        ///// </summary>
        ///// <param name="part"></param>
        ///// <returns></returns>
        //public static IProductPart PartSpecialDeal(IProductPart part)
        //{
        //    //B-CASS: PVS记录到Pizza_Parts, Code去掉前后A字符后，每4码中间使用-分割保存到DB中
        //    if (part.PartType.Equals("B-CASS") && part.ValueType.Equals("SN"))
        //    {
        //        String bcassSn = part.Value;
        //        if (bcassSn[0].Equals('A'))
        //        {
        //            bcassSn = bcassSn.Substring(1, bcassSn.Length - 1);
        //        }
        //        if (bcassSn[bcassSn.Length - 1].Equals('A'))
        //        {
        //            bcassSn = bcassSn.Substring(0, bcassSn.Length - 1);
        //        }

        //        String result = String.Empty;
        //        while (bcassSn.Length > 4)
        //        {
        //            string next4 = bcassSn.Substring(0, 4);
        //            bcassSn = bcassSn.Substring(4, bcassSn.Length - 4);
        //            result += next4;
        //            result += "-";
        //        }
        //        result += bcassSn;
        //        ((ProductPart)part).SetValueSilently(result);
        //    }
        //    return part;
        //}

        public static string PartSpecialDeal(string partType, string valueType, string partSn)
        {
            String result = String.Empty;
            if (partType.Equals("B-CASS") && valueType.Equals("SN"))
            {
                String bcassSn = partSn;
                if (bcassSn[0].Equals('A'))
                {
                    bcassSn = bcassSn.Substring(1, bcassSn.Length - 1);
                }
                if (bcassSn[bcassSn.Length - 1].Equals('A'))
                {
                    bcassSn = bcassSn.Substring(0, bcassSn.Length - 1);
                }

                while (bcassSn.Length > 4)
                {
                    string next4 = bcassSn.Substring(0, 4);
                    bcassSn = bcassSn.Substring(4, bcassSn.Length - 4);
                    result += next4;
                    result += "-";
                }
                result += bcassSn;
            }
            else
            {
                result = partSn;
            }
            return result;
        }

        /// <summary>
        /// 有些类型的Part保存前需要对part数据进行特殊改变, 取出后也要进行反向转换
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public static IProductPart PartReverseSpecialDeal(IProductPart part)
        {
            //B-CASS: PVS记录到Pizza_Parts, Code去掉前后A字符后，每4码中间使用-分割保存到DB中
            if (part.PartType.Equals("B-CASS") && part.ValueType.Equals("SN"))
            {
                String bcassSn = part.Value;
                bcassSn = "A" + bcassSn + "A";
                bcassSn = bcassSn.Replace("-", "");
                ((ProductPart)part).SetValueSilently(bcassSn);
            }
            return part;
        }

    }
}
