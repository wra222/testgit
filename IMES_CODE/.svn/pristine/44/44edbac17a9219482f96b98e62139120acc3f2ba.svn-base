using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PCA.MBChangeLog
{
    /// <summary>
    /// 主板Regenerate日志值对象
    /// </summary>
    [ORMapping(typeof(Change_PCB))]
    public class MBChangeLog : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MBChangeLog()
        {
            this.Tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MBChangeLog(string sn, string newSn, string reason, string editor, DateTime cdt)
        {
            _sn = sn;
            _newSn = newSn;
            _reason = reason;
            _editor = editor;
            _cdt = cdt;
            this.Tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        
        [ORMapping(Change_PCB.fn_oldPCBNo)]
        private string _sn = null;
        [ORMapping(Change_PCB.fn_newPCBNo)]
        private string _newSn = null;
        [ORMapping(Change_PCB.fn_reason)]
        private string _reason = null;
        [ORMapping(Change_PCB.fn_editor)]
        private string _editor = null;
        [ORMapping(Change_PCB.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;

        /// <summary>
        /// MB Sn
        /// </summary>
        public string Sn
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
        /// 所属Line
        /// </summary>
        public string NewSn
        {
            get
            {
                return this._newSn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._newSn = value;
            }
        }

        public string Reason
        {
            get
            {
                return this._reason;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._reason = value;
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

        #endregion

        #region Overrides of FisObjectBase
        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _sn; }
        }
        #endregion
    }
}
