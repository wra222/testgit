using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PAK.FRU
{
    public class FRUPart : FisObjectBase, IFRUPart
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FRUPart(string fruID, string partID, string value, string editor, DateTime cdt)
        {
            _fruId = fruID;
            _partId = partID;
            _value = value;
            _editor = editor;
            _cdt = cdt;
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        
        private int _id;
        private string _fruId;
        private string _partId;
        private string _value;
        private string _editor;
        private DateTime _cdt;

        public int ID
        {
            get { return _id; }
            set
            {
                this._tracker.MarkAsModified(this);
                _id = value;
            }
        }

        public string FRUID
        {
            get { return _fruId; }
            set 
            {
                this._tracker.MarkAsModified(this);
                _fruId = value; 
            }
        }

        public string PartID
        {
            get { return _partId; }
        }

        public string Value
        {
            get { return _value; }
        }

        public string Editor
        {
            get { return _editor; }
        }

        public DateTime Cdt
        {
            get { return _cdt; }
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
