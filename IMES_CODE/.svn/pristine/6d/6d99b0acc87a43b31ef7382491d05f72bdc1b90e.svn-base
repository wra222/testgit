using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.ReprintLog
{
    /// <summary>
    /// 重印Log类
    /// </summary>
    public class ReprintLog : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReprintLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private int _id;
        private string _labelName;
        private string _begNo;
        private string _endNo;
        private string _descr;
        private string _reason;
        private string _editor;
        private DateTime _cdt;

        /// <summary>
        /// Id
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
        /// Label名称
        /// </summary>
        public string LabelName
        {
            get
            {
                return this._labelName;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._labelName = value;
            }
        }

        /// <summary>
        /// 开始号码
        /// </summary>
        public string BegNo
        {
            get
            {
                return this._begNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._begNo = value;
            }
        }

        /// <summary>
        /// 结束号码
        /// </summary>
        public string EndNo
        {
            get
            {
                return this._endNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._endNo = value;
            }
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Descr
        {
            get
            {
                return this._descr;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._descr = value;
            }
        }

        /// <summary>
        /// 重印原因
        /// </summary>
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
        /// 重印原因
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
        /// Cdt
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
            get { return this._id; }
        }

        #endregion
    }
}
