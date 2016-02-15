using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// Part禁用信息
    /// </summary>
    public class PartForbidden : FisObjectBase
    {
        private int _id;
        private string _family;
        private string _model;
        private string _descr;
        private string _partNo;
        private string _assemblyCode;
        //private string _partType;
        //private string _dateCode;
        private int _status;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PartForbidden(int id, string family, string model, string descr, string partNo, string assemblyCode,/*string partType, string dateCode,*/ int status, string editor, DateTime cdt, DateTime udt)
        {
            _id = id;
            _family = family;
            _model = model;
            _descr = descr;
            _partNo = partNo;
            _assemblyCode = assemblyCode;
            //_partType = partType;
            //_dateCode = dateCode;
            _status = status;
            _editor = editor;
            _cdt = cdt;
            _udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        public PartForbidden()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Id
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                _tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// family
        /// </summary>
        public string Family
        {
            get { return _family; }
            set
            {
                _tracker.MarkAsModified(this);
                _family = value;
            }
        }

        /// <summary>
        /// Model
        /// </summary>
        public string Model
        {
            get { return _model; }
            set
            {
                _tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Descr
        {
            get { return _descr; }
            set
            {
                _tracker.MarkAsModified(this);
                _descr = value;
            }
        }

        /// <summary>
        /// Pn
        /// </summary>
        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _tracker.MarkAsModified(this);
                _partNo = value;
            }
        }

        /// <summary>
        /// AssemblyCode
        /// </summary>
        public string AssemblyCode
        {
            get { return _assemblyCode; }
            set
            {
                _tracker.MarkAsModified(this);
                _assemblyCode = value;
            }
        }

        //public string PartType
        //{
        //    get { return _partType; }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _partType = value;
        //    }
        //}

        //public string DateCode
        //{
        //    get { return _dateCode; }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _dateCode = value;
        //    }
        //}

        /// <summary>
        /// 禁用状态
        /// </summary>
        public int Status
        {
            get { return _status; }
            set
            {
                _tracker.MarkAsModified(this);
                _status = value;
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
                _tracker.MarkAsModified(this);
                _editor = value;
            }
        }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt
        {
            get { return _cdt; }
        }

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt
        {
            get { return _udt; }
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
