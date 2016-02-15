using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.FA.LCM
{
    /// <summary>
    /// LCM与ME绑定关系实体
    /// </summary>
    public class LCMME : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LCMME()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LCMME(int id, string lcmSn, string meSn, string meType, string editor, DateTime cdt)
        {
            _id = id;
            _lcmSn = lcmSn;
            _meSn = meSn;
            _meType = meType;
            _editor = editor;
            _cdt = cdt;
            this._tracker.MarkAsAdded(this);
        }

        private int _id;
        private string _lcmSn;
        private string _meSn;
        private string _meType;
        private string _editor;
        private DateTime _cdt;

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _id; }
            set 
            {
                this._tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// LCM sn
        /// </summary>
        public string LCMSn
        {
            get { return _lcmSn; }
            set
            {
                this._tracker.MarkAsModified(this);
                _lcmSn = value;
            }
        }

        /// <summary>
        /// Me sn
        /// </summary>
        public string MESn
        {
            get { return _meSn; }
            set
            {
                this._tracker.MarkAsModified(this);
                _meSn = value;
            }
        }

        /// <summary>
        /// Me type
        /// </summary>
        public string METype
        {
            get { return _meType; }
            set
            {
                this._tracker.MarkAsModified(this);
                _meType = value;
            }
        }

        /// <summary>
        /// Editor
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
        /// Cdt
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
