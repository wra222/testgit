using System;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Defect
{
    /// <summary>
    /// 系统中使用的DefectCode, DefectCode分为2类：PRD, QC
    /// </summary>
    public class Defect : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Defect(string defectCode, string type, string descr, string editor, DateTime cdt, DateTime udt)
        {
            _defectCode = defectCode;
            _type = type;
            _descr = descr;
            _editor = editor;
            _cdt = cdt;
            _udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        public Defect()
        {
            this._tracker.MarkAsAdded(this);
        }

        private string _defectCode;
        private string _type;
        private string _descr;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;
        private string _engDescr;

        /// <summary>
        /// Defect code
        /// </summary>
        public string DefectCode
        {
            get
            {
                return _defectCode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _defectCode = value;
            }
        }

        /// <summary>
        /// DefectCode类型，取值范围：'PRD', 'QC'
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _type = value;
            }
        }

        /// <summary>
        /// Defect描述
        /// </summary>
        public string Descr
        {
            get
            {
                return _descr;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _descr = value;
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
        /// 更新时间
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

        /// <summary>
        /// English Defect描述
        /// </summary>
        public string EngDescr
        {
            get
            {
                return _engDescr;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _engDescr = value;
            }
        }
         
        /// <summary>
        /// Defect类型
        /// </summary>
        public static class DefectType
        {
            public const string PRD = "PRD"; 
            public const string QC = "QC";
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._defectCode; }
        }

        #endregion
    }
}