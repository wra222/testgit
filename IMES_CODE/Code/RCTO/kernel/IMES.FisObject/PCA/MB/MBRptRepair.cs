using System;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PCA.MB
{
    /// <summary>
    /// 主板扩展信属性。表示一条主板的扩展属性。
    /// </summary>
    public class MBRptRepair : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MBRptRepair()
        {
            this._tracker.MarkAsAdded(this);
        }

        public MBRptRepair(string mbSn, string tp, string remark, string status, string mark, string userName, DateTime cdt, DateTime udt)
        {
            _mbSn = mbSn;
            _tp = tp;
            _remark = remark;
            _status = status;
            _mark = mark;
            _userName = userName;
            _cdt = cdt;
            _udt = udt;
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private string _mbSn;
        private string _tp;
        private string _remark;
        private string _status;
        private string _mark;
        private string _userName;
        private DateTime _cdt;
        private DateTime _udt;

        public string MBSn
        {
            get { return _mbSn; }
            set
            {
                this._tracker.MarkAsModified(this);
                _mbSn = value;
            }
        }

        public string Tp
        {
            get { return _tp; }
            set
            {
                this._tracker.MarkAsModified(this);
                _tp = value;
            }
        }

        public string Remark
        {
            get { return _remark; }
            set
            {
                this._tracker.MarkAsModified(this);
                _remark = value;
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                this._tracker.MarkAsModified(this);
                _status = value;
            }
        }

        public string Mark
        {
            get { return _mark; }
            set
            {
                this._tracker.MarkAsModified(this);
                _mark = value;
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                this._tracker.MarkAsModified(this);
                _userName = value;
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

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._mbSn; }
        }

        #endregion
    }
}
