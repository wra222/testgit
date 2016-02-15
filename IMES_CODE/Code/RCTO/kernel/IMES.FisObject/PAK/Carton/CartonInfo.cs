using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.Carton
{
    public class CartonInfo : FisObjectBase, IAggregateRoot
    {
        public CartonInfo()
        {
            this._tracker.MarkAsAdded(this);
        }

        public CartonInfo(string cartonSN, string name, string value, string editor)
        {
            _cartonSN = cartonSN;
            _infoType = name;
            _infoValue = value;
            _editor = editor;
            _cdt = DateTime.Now;
            _udt = _cdt;
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields
        private int _id = 0;
        private string _cartonSN = "";
        private string _infoType = "";
        private string _infoValue = "";
        private string _editor = "";
        private DateTime _cdt = DateTime.MinValue;
        private DateTime _udt = DateTime.MinValue;

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

        public string CartonNo
        {
            get
            {
                return this._cartonSN;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cartonSN = value.Trim();
            }
        }

        public string InfoType
        {
            get
            {
                return this._infoType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._infoType = value;
            }
        }

        public string InfoValue
        {
            get
            {
                return this._infoValue;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._infoValue= value;
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
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
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

        public DateTime Udt
        {
            get
            {
                return this._udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._udt = value;
            }
        }

        public Carton Parent
        {
            get;
            set;
        }
        #endregion
        #region IFisObject Members
        public override object Key
        {
            get { return _id; }
        }

        #endregion

    }
}
