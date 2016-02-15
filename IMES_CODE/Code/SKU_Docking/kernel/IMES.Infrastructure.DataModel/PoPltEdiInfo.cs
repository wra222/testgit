using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(PoPlt_EDI))]
    public class PoPltEdiInfo
    {
        [ORMapping(PoPlt_EDI.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(PoPlt_EDI.fn_combineQty)]
        public Int32 combineQty = int.MinValue;
        [ORMapping(PoPlt_EDI.fn_conQTY)]
        public Int32 conQTY = int.MinValue;
        [ORMapping(PoPlt_EDI.fn_consolidate)]
        public String consolidate = null;
        [ORMapping(PoPlt_EDI.fn_deliveryNo)]
        public String deliveryNo = null;
        [ORMapping(PoPlt_EDI.fn_editor)]
        public String editor = null;
        [ORMapping(PoPlt_EDI.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(PoPlt_EDI.fn_plt)]
        public String plt = null;
        [ORMapping(PoPlt_EDI.fn_qty)]
        public Int32 qty = int.MinValue;
        [ORMapping(PoPlt_EDI.fn_ucc)]
        public String ucc = null;
        [ORMapping(PoPlt_EDI.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
