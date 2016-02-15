using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.NumControl
{
    [ORMapping(typeof(Macrange))]
    public class MACRange : FisObjectBase
    {
        public class MACRangeStatus
        {
            public static readonly string Virgin = "R";
            public static readonly string Active = "A";
            public static readonly string Closed = "C";
        }

        public MACRange()
        {
            this._tracker.MarkAsAdded(this);
        }

        public MACRange(int id, string code, string begNo, string endNo, string status, string editor, DateTime cdt, DateTime udt)
        {
            _id = id;
            _code = code;
            _begNo = begNo;
            _endNo = endNo;
            _status = status;
            _editor = editor;
            _cdt = cdt;
            _udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        [ORMapping(Macrange.fn_id)]
        private int _id = int.MinValue;//default(int);
        [ORMapping(Macrange.fn_code)]
        private string _code = null;
        [ORMapping(Macrange.fn_begNo)]
        private string _begNo = null;
        [ORMapping(Macrange.fn_endNo)]
        private string _endNo = null;
        [ORMapping(Macrange.fn_status)]
        private string _status = null;
        [ORMapping(Macrange.fn_editor)]
        private string _editor = null;
        [ORMapping(Macrange.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;//default(DateTime);
        [ORMapping(Macrange.fn_udt)]
        private DateTime _udt = DateTime.MinValue;//default(DateTime);

        public int ID
        {
            get 
            { 
                return _id; 
            }
            set
            {
                _tracker.MarkAsModified(this);
                _id = value;
            }
        }
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _code = value;
            }
        }
        public string BegNo
        {
            get
            {
                return _begNo;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _begNo = value;
            }
        }
        public string EndNo
        {
            get
            {
                return _endNo;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _endNo = value;
            }
        }
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _status = value;
            }
        }
        public string Editor
        {
            get
            {
                return _editor;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _editor = value;
            }
        }
        public DateTime Cdt
        {
            get
            {
                return _cdt;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _cdt = value;
            }
        }
        public DateTime Udt
        {
            get
            {
                return _udt;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        #endregion

        public bool IsIn(string mac)
        {
            return (string.Compare(_begNo,mac) <= 0 && string.Compare(mac,_endNo) <= 0);
        }

        public bool IsEnd(string mac)
        {
            return (mac == _endNo);
        }

        public bool IsVirgin()
        {
            return (MACRangeStatus.Virgin == _status);
        }
        public bool IsActive()
        {
            return (MACRangeStatus.Active == _status);
        }
        public bool IsClosed()
        {
            return (MACRangeStatus.Closed == _status);
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
