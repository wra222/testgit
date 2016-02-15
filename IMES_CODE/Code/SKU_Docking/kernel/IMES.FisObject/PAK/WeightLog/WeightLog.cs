// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应WeightLog表
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

namespace IMES.FisObject.PAK.WeightLog
{
    /// <summary>
    /// WeightLog业务对象
    /// </summary>
    public class WeightLog : FisObjectBase, IAggregateRoot
    {
        public WeightLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private int _id;
        private string _sn;
        private decimal _weight;
        private string _line;
        private string _station;
        private string _editor;
        private DateTime _cdt;

        /// <summary>
        /// 自增ID
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
        /// 序号
        /// </summary>
        public string SN
        {
            get
            {
                return this._sn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._sn = value;
            }
        }

        /// <summary>
        /// 重量
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
        /// 产线
        /// </summary>
        public string Line
        {
            get
            {
                return this._line;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._line = value;
            }
        }

        /// <summary>
        /// 站别
        /// </summary>
        public string Station
        {
            get
            {
                return this._station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._station = value;
            }
        }

        /// <summary>
        /// Editor
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
