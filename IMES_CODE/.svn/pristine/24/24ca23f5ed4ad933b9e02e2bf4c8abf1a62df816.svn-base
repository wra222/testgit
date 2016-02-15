using System;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PCA.MBModel
{
    [ORMapping(typeof(Mbcode))]
    public class MBCode : FisObjectBase
    {
        [ORMapping(Mbcode.fn_mbcode)]
        private readonly string _mbCode = null;
        [ORMapping(Mbcode.fn_description)]
        private readonly string _description = null;
        [ORMapping(Mbcode.fn_multiQty)]
        private readonly short _multQty = short.MinValue;
        [ORMapping(Mbcode.fn_editor)]
        private readonly string _editor = null;
        [ORMapping(Mbcode.fn_cdt)]
        private readonly DateTime _cdt = DateTime.MinValue;
        [ORMapping(Mbcode.fn_udt)]
        private readonly DateTime _udt = DateTime.MinValue;
        [ORMapping(Mbcode.fn_type)]
        private readonly string _type = null;

        public MBCode()
        {
            
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="mbCode"></param>
        /// <param name="description"></param>
        /// <param name="multQty"></param>
        /// <param name="editor"></param>
        /// <param name="cdt"></param>
        /// <param name="udt"></param>
        public MBCode(string mbCode, string description, short multQty, string editor, DateTime cdt, DateTime udt, string type)
        {
            _mbCode = mbCode;
            _description = description;
            _multQty = multQty;
            _editor = editor;
            _cdt = cdt;
            _udt = udt;
            _type = type;
        }

        public string MbCode
        {
            get { return _mbCode; }
        }

        public string Description
        {
            get { return _description; }
        }

        public short MultQty
        {
            get { return _multQty; }
        }

        public string Editor
        {
            get { return _editor; }
        }

        public DateTime Cdt
        {
            get { return _cdt; }
        }

        public DateTime Udt
        {
            get { return _udt; }
        }

        public string Type
        {
            get { return _type; }
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _mbCode; }
        }

        #endregion
    }
}
