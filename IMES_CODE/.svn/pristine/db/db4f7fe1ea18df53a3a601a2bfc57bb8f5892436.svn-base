using System;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.DN
{
    [ORMapping(typeof(mtns.DeliveryLog))]
    public class DeliveryLog : FisObjectBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public DeliveryLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        public DeliveryLog(int id, string deliveryNo, string status, string station, string line, string editor, DateTime cdt)
        {
            _id = id;
            _deliveryNo = deliveryNo;
            _status = status;
            _station = station;
            _line = line;
            _editor = editor;
            _cdt = cdt;
            this._tracker.MarkAsAdded(this);
        }

        #endregion

        #region . Essential Fields .
        [ORMapping(mtns.DeliveryLog.fn_id)]
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [ORMapping(mtns.DeliveryLog.fn_deliveryNo)]
        private string _deliveryNo = null;
        [ORMapping(mtns.DeliveryLog.fn_status)]
        private string _status = null;
        [ORMapping(mtns.DeliveryLog.fn_station)]
        private string _station = null;
        [ORMapping(mtns.DeliveryLog.fn_line)]
        private string _line = null;
        [ORMapping(mtns.DeliveryLog.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.DeliveryLog.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;

        /// <summary>
        /// HDVD
        /// </summary>
        public string DeliveryNo
        {
            get { return _deliveryNo; }
            set
            {
                _tracker.MarkAsModified(this);
                _deliveryNo = value;
            }
        }
        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._id; }
        }

        #endregion
    }
}
