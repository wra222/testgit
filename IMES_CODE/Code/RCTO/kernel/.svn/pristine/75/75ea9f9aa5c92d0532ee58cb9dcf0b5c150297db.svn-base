using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Material
{
    [ORMapping(typeof(mtns.MaterialLog))]
    public class MaterialLog : FisObjectBase
    {/// <summary>
        /// Constructor
        /// </summary>
        public MaterialLog()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns.MaterialLog.fn_id)]
        private int _id=int.MinValue;
        [ORMapping(mtns.MaterialLog.fn_materialCT)]
        private string _materialCT = null;
       
        [ORMapping(mtns.MaterialLog.fn_action)]
        private string _action = null;
        [ORMapping(mtns.MaterialLog.fn_stage)]
        private string _stage = null;
        [ORMapping(mtns.MaterialLog.fn_line)]
        private string _line = null;
        [ORMapping(mtns.MaterialLog.fn_preStatus)]
        private string _preStatus = null;
        [ORMapping(mtns.MaterialLog.fn_status)]
        private string _status = null;
        [ORMapping(mtns.MaterialLog.fn_comment)]
        private string _comment = null;

        [ORMapping(mtns.MaterialLog.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.MaterialLog.fn_cdt)]
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
        public string MaterialCT
        {
            get
            {
                return this._materialCT;
            }
            set
            {
               this._tracker.MarkAsModified(this);
               this._materialCT = value;
            }
        }
      
        public string Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._action = value;
            }
        }
        public string Stage
        {
            get
            {
                return this._stage;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._stage = value;
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
                this._line = value;
            }
        }

        public string PreStatus
        {
            get
            {
                return this._preStatus;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._preStatus = value;
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

        public string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._comment = value;
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
      
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _id; }
        }

        #endregion
    }
    
}
