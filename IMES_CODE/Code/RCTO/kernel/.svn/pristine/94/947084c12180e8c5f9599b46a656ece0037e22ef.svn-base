using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.PartSn
{
    ///<summary>
    /// PartSn (CT sn)
    ///</summary>
    public class PartSn : FisObjectBase, IAggregateRoot
    {
        private string _iecSn;
        private string _iecPn;
        private string _type;
        private string _vendorSn;
        private string _vendorDCode;
        private string _vCode;
        private string _pn151;
        private string _editor;
        private string _dateCode;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PartSn(string iecSn, string iecPn, string type, string vendorSn, string vendorDCode, string vCode, string pn151, string editor, string dateCode)
        {
            _iecSn = iecSn;
            _iecPn = iecPn;
            _type = type;
            _vendorSn = vendorSn;
            _vendorDCode = vendorDCode;
            _vCode = vCode;
            _pn151 = pn151;
            _editor = editor;
            _dateCode = dateCode;

            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Iec Sn
        /// </summary>
        public string IecSn
        {
            get { return _iecSn; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._iecSn = value;
            }
        }

        /// <summary>
        /// Iec Pn
        /// </summary>
        public string IecPn
        {
            get { return _iecPn; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._iecPn = value;
            }
        }

        /// <summary>
        /// Part 类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._type = value;
            }
        }

        /// <summary>
        /// Vendor Sn
        /// </summary>
        public string VendorSn
        {
            get { return _vendorSn; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._vendorSn = value;
            }
        }

        /// <summary>
        /// Vendor Dcode
        /// </summary>
        public string VendorDCode
        {
            get { return _vendorDCode; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._vendorDCode = value;
            }
        }

        /// <summary>
        /// Vcode
        /// </summary>
        public string VCode
        {
            get { return _vCode; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._vCode = value;
            }
        }

        public string Pn151
        {
            get { return _pn151; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._pn151 = value;
            }
        }

        public string Editor
        {
            get { return _editor; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._editor = value;
            }
        }

        /// <summary>
        /// Date code
        /// </summary>
        public string DateCode
        {
            get { return _dateCode; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._dateCode = value;
            }
        }

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt
        {
            get { return _udt; }
            set
            {
                this._tracker.MarkAsModified(this);
                this._udt = value;
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
                this._cdt = value;
            }
        }

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _iecSn; }
        }
    }
}
