using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Model
{
    public class OSWIN : FisObjectBase, IAggregateRoot
    {
        public OSWIN()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private int _ID;
        private string _Family;
        private string _Zmode;
        private string _OS;
        private string _AV;
        private string _Image;
        private string _BomZmode="";

        private string _Editor;
        private DateTime _Cdt= DateTime.MinValue;
        private DateTime _Udt=DateTime.MinValue;

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._ID = value;
            }
        }

        /// <summary>
        /// _Family
        /// </summary>
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


        /// <summary>
        /// Zmod
        /// </summary>
        public string Zmode
        {
            get
            {
                return this._Zmode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Zmode = value;
            }
        }


        /// <summary>
        ///_OS
        /// </summary>
        public string OS
        {
            get
            {
                return this._OS;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._OS = value;
            }
        }

        /// <summary>
        ///_AV
        /// </summary>
        public string AV
        {
            get
            {
                return this._AV;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._AV = value;
            }
        }

        /// <summary>
        ///_Image
        /// </summary>
        public string Image
        {
            get
            {
                return this._Image;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Image = value;
            }
        }

        /// <summary>
        ///_BomZmode
        /// </summary>
        public string BomZmode
        {
            get
            {
                return this._BomZmode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._BomZmode = value;
            }
        }

        /// <summary>
        ///Editor
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

        /// <summary>
        /// 更新时间
        /// </summary>
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
        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._ID; }
        }

        #endregion
    }
}
