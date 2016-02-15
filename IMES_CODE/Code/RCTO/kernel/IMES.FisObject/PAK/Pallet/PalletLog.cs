// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应PalletLog表
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

namespace IMES.FisObject.PAK.Pallet
{
    /// <summary>
    /// PalletLog业务对象
    /// </summary>
    public class PalletLog : FisObjectBase
    {
        public PalletLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        private int _id;
        private string _palletNo;
        private string _station;
        private string _line;
        private string _editor;
        private DateTime _cdt;

        /// <summary>
        /// Log的ID
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
        /// Pallet序号
        /// </summary>
        public string PalletNo
        {
            get
            {
                return _palletNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _palletNo = value;
            }
        }

        /// <summary>
        /// 经过的Station
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

        /// <summary>
        /// 所在的Line
        /// </summary>
        public string Line
        {
            get
            {
                return _line;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _line = value;
            }
        }

        /// <summary>
        /// Editor
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

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._id; }
        }
    }
}
