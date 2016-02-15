using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.Carton
{
    public class CartonLog : FisObjectBase, IAggregateRoot
    {
        public CartonLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        public CartonLog(string cartonSN, string station, int status, string line, string editor)
        {
            _cartonSN = cartonSN;
            _station = station;
            _status = status;
            _line =line;
            _editor = editor;
            _cdt = DateTime.Now;
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields
        private int _id = 0;
        private string _cartonSN = "";
        private string _station = "";
        private int _status=1;
        private string _line = "";
        private string _editor = "";
        private DateTime _cdt = DateTime.MinValue;
       
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

        public string Station
        {
            get
            {
                return this._station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
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
                this._line= value.Trim();
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
       
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _id; }
        }

        #endregion

    }
}
