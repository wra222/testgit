using System;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// Assembly Code对象
    /// </summary>
    public class AssemblyCode : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public AssemblyCode(string assCode, string family, string pn)
        {
            _assCode = assCode;
            _family = family;
            _pn = pn;

            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AssemblyCode()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private int _id;
        private string _assCode;
        private string _family;
        private string _pn;
        private string _model;
        private string _region;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// Id
        /// </summary>
        public int ID
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
        /// AssemblyCode
        /// </summary>
        public string AssCode
        {
            get
            {
                return _assCode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _assCode = value;
            }
        }
        
        /// <summary>
        /// Family
        /// </summary>
        public string Family
        {
            get
            {
                return _family;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _family = value;
            }
        }

        /// <summary>
        /// Pn
        /// </summary>
        public string Pn
        {
            get
            {
                return _pn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _pn = value;
            }
        }

        /// <summary>
        /// Model
        /// </summary>
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /// <summary>
        /// Region
        /// </summary>
        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _region = value;
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

        #endregion

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