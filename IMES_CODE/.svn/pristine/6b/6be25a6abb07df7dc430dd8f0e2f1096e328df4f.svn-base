using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Material
{
    [ORMapping(typeof(mtns.MaterialBoxAttrLog))]
    public class MaterialBoxAttrLog : FisObjectBase
    {/// <summary>
        /// Constructor
        /// </summary>
        public MaterialBoxAttrLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns.MaterialBoxAttrLog.fn_id)]
        private int _id=int.MinValue;
        [ORMapping(mtns.MaterialBoxAttrLog.fn_boxId)]
        private string _boxId = null;

        [ORMapping(mtns.MaterialBoxAttrLog.fn_attrName)]
        private string _attrName = null;

        [ORMapping(mtns.MaterialBoxAttrLog.fn_status)]
        private string _status = null;

        [ORMapping(mtns.MaterialBoxAttrLog.fn_attrOldValue)]
        private string _attrOldValue = null;

        [ORMapping(mtns.MaterialBoxAttrLog.fn_attrNewValue)]
        private string _attrNewValue = null;

        [ORMapping(mtns.MaterialBoxAttrLog.fn_descr)]
        private string _descr = null;

        [ORMapping(mtns.MaterialBoxAttrLog.fn_editor)]
        private string _Editor = null;
        [ORMapping(mtns.MaterialBoxAttrLog.fn_cdt)]
        private DateTime _Cdt = DateTime.MinValue;
        

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

        public string BoxId
        {
            get
            {
                return this._boxId;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._boxId = value;
            }
        }

        public string AttrName
        {
            get
            {
                return this._attrName;
            }
            set
            {
               this._tracker.MarkAsModified(this);
               this._attrName = value;
            }
        }
       

        public string Status
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
        public string AttrOldValue
        {
            get
            {
                return this._attrOldValue;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._attrOldValue = value;
            }
        }

        public string AttrNewValue
        {
            get
            {
                return this._attrNewValue;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._attrNewValue = value;
            }
        }

        public string Descr
        {
            get
            {
                return this._descr;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._descr = value;
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
        
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _id; }
        }

        #endregion
    }
    
}
