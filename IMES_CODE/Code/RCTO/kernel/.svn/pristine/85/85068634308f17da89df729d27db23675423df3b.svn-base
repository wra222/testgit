using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
//using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PCA.MB
{
    /// <summary>
    /// 主板扩展信属性。表示一条主板的扩展属性。
    /// </summary>
    [ORMapping(typeof(Pcbinfo))]
    public class MBInfo : FisObjectBase 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MBInfo()
        {
            this._tracker.MarkAsAdded(this);
        }

        public MBInfo(int id, string pcbid, string infotype, string infovalue, string editor, DateTime cdt, DateTime udt)
        {
            this._id = id;
            this._pcbid = pcbid;
            this._infotype = infotype;
            this._infovalue = infovalue;
            this._editor = editor;
            this._cdt = cdt;
            this._udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(Pcbinfo.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(Pcbinfo.fn_pcbno)]
        private string _pcbid = null;
        [ORMapping(Pcbinfo.fn_infoType)]
        private string _infotype = null;
        [ORMapping(Pcbinfo.fn_infoValue)]
        private string _infovalue = null;
        [ORMapping(Pcbinfo.fn_editor)]
        private string _editor = null;
        [ORMapping(Pcbinfo.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(Pcbinfo.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        /// <summary>
        /// 记录标识
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
        /// 扩展信息所属MB
        /// </summary>
        public string PCBID
        {
            get
            {
                return this._pcbid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._pcbid = value;
            }
        }

        /// <summary>
        /// 扩展属性名
        /// </summary>
        public string InfoType
        {
            get
            {
                return this._infotype;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._infotype = value;
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
        /// 维护人员
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
