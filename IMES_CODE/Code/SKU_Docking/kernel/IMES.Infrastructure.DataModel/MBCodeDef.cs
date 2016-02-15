using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(Mbcode))]
    public class MBCodeDef
    {
        [ORMapping(Mbcode.fn_mbcode)]
        public string MBCode = null;
        [ORMapping(Mbcode.fn_description)]
        public string Description = null;
        [ORMapping(Mbcode.fn_multiQty)]
        public int Qty = int.MinValue;
        [ORMapping(Mbcode.fn_type)]
        public string Type = null;
        [ORMapping(Mbcode.fn_editor)]
        public string editor = null;
        [ORMapping(Mbcode.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Mbcode.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
