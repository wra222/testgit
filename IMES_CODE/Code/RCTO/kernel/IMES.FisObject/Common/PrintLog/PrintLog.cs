using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.PrintLog
{
    /// <summary>
    /// PrintLog业务对象
    /// </summary>
    [ORMapping(typeof(mtns.PrintLog))]
    public class PrintLog : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrintLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns.PrintLog.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns.PrintLog.fn_name)]
        private string _name = null;
        [ORMapping(mtns.PrintLog.fn_begNo)]
        private string _begNo = null;
        [ORMapping(mtns.PrintLog.fn_endNo)]
        private string _endNo = null;
        [ORMapping(mtns.PrintLog.fn_descr)]
        private string _descr = null;
        [ORMapping(mtns.PrintLog.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.PrintLog.fn_cdt)]
        private DateTime _cdt =DateTime.MinValue;

        [ORMapping(mtns.PrintLog.fn_labelTemplate)]
        private string _labelTemplate = null;
        [ORMapping(mtns.PrintLog.fn_station)]
        private string _station = null;

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
        /// 序号种类
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._name = value;
            }
        }

        /// <summary>
        /// 开始号
        /// </summary>
        public string BeginNo
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
        /// 结束号
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
        /// Editor
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
        /// LabelTemplate
        /// </summary>
        public string LabelTemplate
        {
            get
            {
                return this._labelTemplate;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._labelTemplate = value;
            }
        }

        /// <summary>
        /// Station
        /// </summary>
        public string Station
        {
            get
            {
                return this._station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._station = value;
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
