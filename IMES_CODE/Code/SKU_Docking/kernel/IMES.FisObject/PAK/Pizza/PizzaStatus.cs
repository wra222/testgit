// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应Pizza_Status表
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
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.Pizza
{
    /// <summary>
    /// Pizza状态
    /// </summary>
    [ORMapping(typeof(mtns::PizzaStatus))]
    public class PizzaStatus : FisObjectBase
    {
        public PizzaStatus()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns::PizzaStatus.fn_pizzaID)]
        private string _pizzaID = null;
        [ORMapping(mtns::PizzaStatus.fn_line)]
        private string _line = null;
        [ORMapping(mtns::PizzaStatus.fn_station)]
        private string _station = null;
        [ORMapping(mtns::PizzaStatus.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns::PizzaStatus.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns::PizzaStatus.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

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
        /// 产线
        /// </summary>
        public string LineID
        {
            get { return _line; }
            set
            {
                this._tracker.MarkAsModified(this);
                _line = value;
            }
        }

        /// <summary>
        /// 站别
        /// </summary>
        public string StationID
        {
            get { return _station; }
            set
            {
                this._tracker.MarkAsModified(this);
                _station = value;
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
