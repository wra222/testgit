// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Model允许的重量误差范围对应ModelTolerance表
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
    /// Model允许的重量误差范围对应ModelTolerance表
    /// </summary>
    public class ModelTolerance : FisObjectBase, IAggregateRoot
    {

        public ModelTolerance()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private string _model;
        private string _unitTolerance;
        private string _cartonTolerance;
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
        /// Unit 误差
        /// </summary>
        public string UnitTolerance
        {
            get
            {
                return this._unitTolerance;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._unitTolerance = value;
            }
        }

        /// <summary>
        /// Carton 误差
        /// </summary>
        public string CartonTolerance
        {
            get
            {
                return this._cartonTolerance;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cartonTolerance = value;
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
