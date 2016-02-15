// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应FISToSAPWeight表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-27   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
namespace IMES.FisObject.PAK.Pallet
{
    /// <summary>
    /// FISToSAPWeight类对应FISToSAPWeight表
    /// </summary>
    public class FISToSAPWeight : FisObjectBase
    {


        public FISToSAPWeight()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private string _shipment;
        private string _type;
        private decimal _weight;
        private string _status;
        private DateTime _cdt;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get { return _cdt; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cdt = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._status = value;
            }
        }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight
        {
            get { return _weight; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._weight = value;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._type = value;
            }
        }

        /// <summary>
        /// Shipment号码
        /// </summary>
        public string Shipment
        {
            get { return _shipment; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._shipment = value;
            }
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._shipment; }
        }

        #endregion
    }
}
