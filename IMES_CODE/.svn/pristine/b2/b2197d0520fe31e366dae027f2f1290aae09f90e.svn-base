using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PCA.MB
{
    ///<summary>
    /// MODismantleLog
    ///</summary>
    public class MODismantleLog : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MODismantleLog(string pcbNo, string tp, string smtMo, string reason, string editor, DateTime cdt, int id)
        {
            _pcbNo = pcbNo;
            _tp = tp;
            _smtMo = smtMo;
            _reason = reason;
            _editor = editor;
            _cdt = cdt;
            _id = id;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private int _id;
        private string _pcbNo;
        private string _tp;
        private string _smtMo;
        private string _reason;
        private string _editor;
        private DateTime _cdt;

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set 
            {
                this._tracker.MarkAsModified(this);
                _id = value; 
            }
        }

        /// <summary>
        /// PcbNo
        /// </summary>
        public string PcbNo
        {
            get { return _pcbNo; }
        }

        /// <summary>
        /// Tp
        /// </summary>
        public string Tp
        {
            get { return _tp; }
        }

        /// <summary>
        /// SmtMo
        /// </summary>
        public string SmtMo
        {
            get { return _smtMo; }
        }

        /// <summary>
        /// Reason
        /// </summary>
        public string Reason
        {
            get { return _reason; }
        }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get { return _editor; }
        }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt
        {
            get { return _cdt; }
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
