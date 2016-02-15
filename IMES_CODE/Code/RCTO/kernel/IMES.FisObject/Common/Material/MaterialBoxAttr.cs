using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;


namespace IMES.FisObject.Common.Material
{
    [ORMapping(typeof(mtns.MaterialBoxAttr))]
    public class MaterialBoxAttr : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MaterialBoxAttr()
        {
            this._tracker.MarkAsAdded(this);
        }

        

        #region . Essential Fields .

        [ORMapping(mtns.MaterialBoxAttr.fn_boxId)]
        private string _boxId = null;

        [ORMapping(mtns.MaterialBoxAttr.fn_attrName)]
        private string _attrName = null;

        [ORMapping(mtns.MaterialBoxAttr.fn_attrValue)]
        private string _attrValue = null;

        [ORMapping(mtns.MaterialBoxAttr.fn_editor)]
        private string _Editor = null;
        [ORMapping(mtns.MaterialBoxAttr.fn_cdt)]
        private DateTime _Cdt = DateTime.MinValue;
        [ORMapping(mtns.MaterialBoxAttr.fn_udt)]
        private DateTime _Udt = DateTime.MinValue;
       

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

        public string AttrValue
        {
            get
            {
                return this._attrValue;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._attrValue = value;
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
            get { return new string[] { _boxId, _attrName }; }
        }

        #endregion

       
    }
}
