using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PCA.PCBVersion
{
    public class PCBVersion : FisObjectBase, IAggregateRoot
    {
         /// <summary>
        /// Constructor
        /// </summary>
        public PCBVersion()
        {
            this._tracker.MarkAsAdded(this);
        }


        #region . Essential Fields .

        private string _Family = "";
        private string _MBCode = "";
        private string _PCBVer = "";

        private string _CTVer = "";
        private string _Supplier = "";
        private string _Remark = "";
      
        private string _Editor = "";
        private DateTime _Cdt = DateTime.MinValue;
        private DateTime _Udt = DateTime.MinValue;


        public string Family
        {
            get
            {
                return this._Family;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Family = value;
            }
        }

        public string MBCode
        {
            get
            {
                return this._MBCode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._MBCode = value;
            }
        }

        public string PCBVer
        {
            get
            {
                return this._PCBVer;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._PCBVer = value;
            }
        }

        public string CTVer
        {
            get
            {
                return this._CTVer;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._CTVer = value;
            }
        }

        public string Supplier
        {
            get
            {
                return this._Supplier;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Supplier = value;
            }
        }

        public string Remark
        {
            get
            {
                return this._Remark;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Remark = value;
            }
        }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get
            {
                return this._Editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Editor = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._Cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Cdt = value;
            }
        }

        public DateTime Udt
        {
            get
            {
                return this._Udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Udt = value;
            }
        }
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return new string[] { _Family, _MBCode, _PCBVer }; }
        }

        #endregion
    }
}
