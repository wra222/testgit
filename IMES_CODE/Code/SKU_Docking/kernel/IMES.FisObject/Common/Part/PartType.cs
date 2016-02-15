using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// Part类型
    /// </summary>
    [ORMapping(typeof(PartTypeEx))]
    public class PartType : FisObjectBase
    {
        [ORMapping(PartTypeEx.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(PartTypeEx.fn_partType)]
        private string _partType;
        [ORMapping(PartTypeEx.fn_partTypeGroup)]
        private string _partTypeGroup;
        [ORMapping(PartTypeEx.fn_editor)]
        private string _editor;
        [ORMapping(PartTypeEx.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(PartTypeEx.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        public PartType()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PartType(int id, string partType, string partTypeGroup, string editor, DateTime cdt, DateTime udt)
        {
            _id = id;
            _partType = partType;
            _partTypeGroup = partTypeGroup;
            _editor = editor;
            _cdt = cdt;
            _udt = udt;

            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// ID 类型名
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Part 类型名
        /// </summary>
        public string PartTypeName
        {
            get { return _partType; }
            set { _partType = value; }
        }

        /// <summary>
        /// Part大类别
        /// </summary>
        public string PartTypeGroup
        {
            get { return _partTypeGroup; }
            set { _partTypeGroup = value; }
        }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt
        {
            get { return _cdt; }
            set { _cdt = value; }
        }

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt
        {
            get { return _udt; }
            set { _udt = value; }
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
