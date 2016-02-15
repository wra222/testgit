using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.Carton
{
    public class CartonStatus : FisObjectBase, IAggregateRoot
    {
        public CartonStatus() { this._tracker.MarkAsAdded(this); }

        public CartonStatus(string cartonSN, string station, int status, string line, string editor)
        {
            this._tracker.MarkAsAdded(this);
            _cartonSN = cartonSN;
            _station = station;
            _status = status;
            _line = line;
            _editor = editor;
            _cdt = DateTime.Now;
            _udt = _cdt;
           
        }


        #region . Essential Fields
      
        private string _cartonSN = "";
        private string _station = "";
        private int _status = 1;
        private string _line = "";
        private string _editor = "";
        private DateTime _cdt = DateTime.MinValue;
        private DateTime _udt = DateTime.MinValue;

        

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

        public string Station
        {
            get
            {
                return this._station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._station = value.Trim();
            }
        }

        public int Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._status = value;
            }
        }

        public string Line
        {
            get
            {
                return this._line;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._line = value.Trim();
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
                this._editor = value.Trim();
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

        /// <summary>
        /// Parent object
        /// </summary>
        public Carton Parent
        {
            get;
            set;
        }

        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _cartonSN; }
        }

        #endregion

    }
}
