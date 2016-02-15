// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Model的标准重量对应ModelWeight表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-21   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PAK.StandardWeight
{
    /// <summary>
    /// Model的标准重量对应ModelWeight表
    /// </summary>
    public class ModelWeight : FisObjectBase, IAggregateRoot
    {
        public ModelWeight()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private string _model;
        private decimal _unitWeight;
        private decimal _cartonWeight;
        private string _sendStatus="";
        private string _remark="";
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// Model
        /// </summary>
        public string Model
        {
            get
            {
                return this._model;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._model = value;
            }
        }

        /// <summary>
        /// Unit重量
        /// </summary>
        public decimal UnitWeight
        {
            get
            {
                return this._unitWeight;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._unitWeight = value;
            }
        }


        /// <summary>
        /// Caton 重量
        /// </summary>
        public decimal CartonWeight
        {
            get
            {
                return this._cartonWeight;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cartonWeight = value;
            }
        }

        /// <summary>
        /// Send SAP Status
        /// </summary>
        public string SendStatus
        {
            get
            {
                return this._sendStatus;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._sendStatus = value;
            }
        }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._remark = value;
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

        #endregion

        #region IFisObject Members
        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._model; }
        }

        #endregion
    }
}
