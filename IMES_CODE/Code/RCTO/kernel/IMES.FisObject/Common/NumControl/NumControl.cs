using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.NumControl
{
    /// <summary>
    /// Number Control类
    /// </summary>
    [ORMapping(typeof(mtns.NumControl))]
    public class NumControl : FisObjectBase, IAggregateRoot
    {
        public NumControl()
        {
            this._tracker.MarkAsAdded(this);
        }

        public NumControl(int id, string notype, string noname, string value, string customer)
        {
            _id = id;
            _notype = notype;
            _noname = noname;
            _value = value;
            _customer = customer;

            this._tracker.MarkAsAdded(this);
        }

        [ORMapping(mtns.NumControl.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns.NumControl.fn_noType)]
        private string _notype = null;
        [ORMapping(mtns.NumControl.fn_noName)]
        private string _noname = null;
        [ORMapping(mtns.NumControl.fn_value)]
        private string _value = null;
        [ORMapping(mtns.NumControl.fn_customer)]
        private string _customer = null;

        /// <summary>
        /// ID
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
        /// 号码类型
        /// </summary>
        public string NOType
        {
            get
            {
                return _notype;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _notype = value;
            }
        }

        /// <summary>
        /// 号码名
        /// </summary>
        public string NOName
        {
            get
            {
                return _noname;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _noname = value;
            }
        }

        /// <summary>
        /// 号码值
        /// </summary>
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

        /// <summary>
        /// 客户别
        /// </summary>
        public string Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _customer = value;
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
