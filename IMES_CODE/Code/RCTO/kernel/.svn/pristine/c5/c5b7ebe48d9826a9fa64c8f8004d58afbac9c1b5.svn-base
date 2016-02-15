using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PAK.FRU
{
    public class FRUWeight: FisObjectBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FRUWeight(string shipment, string type, decimal weight, string status, DateTime cdt)
        {
            _shipment = shipment;
            _type = type;
            _weight = weight;
            this.status = status;
            this.cdt = cdt;
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private string _shipment;
        private string _type;
        private decimal _weight;
        private string status;
        private DateTime cdt;

        public DateTime Cdt
        {
            get { return cdt; }
        }

        public string Status
        {
            get { return status; }
        }

        public decimal Weight
        {
            get { return _weight; }
        }

        public string Type
        {
            get { return _type; }
        }

        public string Shipment
        {
            get { return _shipment; }
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
