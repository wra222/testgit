// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应PalletWeight表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.Pallet
{
    /// <summary>
    /// Pallet称重对象
    /// </summary>
    public class PalletWeight : FisObjectBase, IAggregateRoot
    {
        private static IPalletWeightRepository _pltWeightRepository = null;
        private static IPalletWeightRepository PltWeightRepository
        {
            get
            {
                if (_pltWeightRepository == null)
                    _pltWeightRepository = RepositoryFactory.GetInstance().GetRepository<IPalletWeightRepository, PalletWeight>();
                return _pltWeightRepository;
            }
        }

        public PalletWeight()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private int _id;
        private string _family;
        private string _region;
        private short _qty;
        private decimal _weight;
        private string _tolerance;
        private string _editor;
        private DateTime _udt;
        private DateTime _cdt;

        /// <summary>
        /// 自增ID
        /// </summary>
        public int Id
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
        /// Family
        /// </summary>
        public string FamilyID
        {
            get
            {
                return this._family;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._family = value;
            }
        }

        /// <summary>
        /// Region
        /// </summary>
        public string Region
        {
            get
            {
                return this._region;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._region = value;
            }
        }

        /// <summary>
        /// 满栈板机器数量
        /// </summary>
        public short Qty
        {
            get
            {
                return this._qty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._qty = value;
            }
        }

        /// <summary>
        /// 标准重量
        /// </summary>
        public decimal Weight
        {
            get
            {
                return this._weight;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._weight = value;
            }
        }

        /// <summary>
        /// 允许误差
        /// </summary>
        public string Tolerance
        {
            get
            {
                return this._tolerance;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._tolerance = value;
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

        #endregion

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
