// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应PrintTemplate表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-02-01   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.PrintItem
{
    /// <summary>
    /// 打印模板
    /// </summary>
    public class PrintTemplate : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrintTemplate()
        {
            this._tracker.MarkAsAdded(this);
        }

        private int _piece;
        private string _templateName;
        private string _labelType;
        private string _spName;
        private string _description;
        private string _editor = null;
        private DateTime _cdt;
        private DateTime _udt;
        private int _layout;

        /// <summary>
        /// 打印张数
        /// </summary>
        public int Piece
        {
            get
            {
                return this._piece;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._piece = value;
            }
        }

        /// <summary>
        /// 模板名字
        /// </summary>
        public string TemplateName
        {
            get
            {
                return this._templateName;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._templateName = value;
            }
        }

        /// <summary>
        /// Label Type
        /// </summary>
        public string LblType
        {
            get
            {
                return this._labelType;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._labelType = value;
            }
        }

        /// <summary>
        /// 存储过程名字
        /// </summary>
        public string SpName
        {
            get
            {
                return this._spName;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._spName = value;
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._description = value;
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
        /// Cdt
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
        /// Udt
        /// </summary>
        public DateTime Udt
        {
            get
            {
                return _udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        public int Layout
        {
            get
            {
                return this._layout;
            }
            set
            {
                _tracker.MarkAsModified(this);
                this._layout = value;
            }
        }

        #region . Overrides of FisObjectBase .

        public override object Key
        {
            get { return this._templateName; }
        }

        #endregion
    }
}
