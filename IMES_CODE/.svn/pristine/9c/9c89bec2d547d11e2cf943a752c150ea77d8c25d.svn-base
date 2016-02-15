using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    //[Serializable]
    //public class ConstValueTypeInfo
    //{
    //    public int id { get; set; }
    //    public String type { get; set; }
    //    public String value { get; set; }
    //    public String description { get; set; }
    //    public String editor { get; set; }
    //    public DateTime cdt { get; set; }
    //    public DateTime udt { get; set; }
    //}

    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.ConstValueType))]
    [Serializable]
    public class ConstValueTypeInfo
    {
        [ORMapping(ConstValueType.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(ConstValueType.fn_type)]
        public String type = null;
        [ORMapping(ConstValueType.fn_value)]
        public String value = null;
        [ORMapping(ConstValueType.fn_description)]
        public String description = null;
        [ORMapping(ConstValueType.fn_editor)]
        public String editor = null;
        [ORMapping(ConstValueType.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(ConstValueType.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
