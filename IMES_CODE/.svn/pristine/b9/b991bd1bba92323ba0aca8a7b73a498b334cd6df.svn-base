using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// Part的扩展属性类，该类实例不能脱离Part对象单独存在,且该类的对象为只读对象
    /// </summary>

    [ORMapping(typeof(mtns::PartInfo))]
    public class PartInfo : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pn"></param>
        /// <param name="infotype"></param>
        /// <param name="infovalue"></param>
        /// <param name="editor"></param>
        /// <param name="cdt"></param>
        /// <param name="udt"></param>
        public PartInfo(int id, string pn, string infotype, string infovalue, string editor, DateTime cdt, DateTime udt)
        {
            this._id = id;
            this._partNumber = pn;
            this._infoType = infotype;
            this._infovalue = infovalue;
            this._editor = editor;
            this._cdt = cdt;
            this._udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PartInfo()
        {
            this._tracker.MarkAsAdded(this);
        }

        [ORMapping(mtns::PartInfo.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns::PartInfo.fn_partNo)]
        private string _partNumber = null;
        [ORMapping(mtns::PartInfo.fn_infoType)]
        private string _infoType = null;
        [ORMapping(mtns::PartInfo.fn_infoValue)]
        private string _infovalue = null;
        [ORMapping(mtns::PartInfo.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns::PartInfo.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns::PartInfo.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        /// <summary>
        /// 扩展属性名
        /// </summary>
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

        /// <summary>
        /// Pn
        /// </summary>
        public string PN 
        {
            get
            {
                return this._partNumber;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._partNumber = value;
            }
        }

        /// <summary>
        /// 扩展属性名
        /// </summary>
        public string InfoType 
        {
            get
            {
                return this._infoType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._infoType = value;
            }
        }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string InfoValue 
        {
            get
            {
                return this._infovalue;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._infovalue = value;
            }
        }

        /// <summary>
        /// 数据维护人员工号
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
        
        /// <summary>
        /// 更新时间
        /// </summary>
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
