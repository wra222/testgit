using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(PoData_EDI))]
    public class PoDataEdiInfo
    {
        [ORMapping(PoData_EDI.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(PoData_EDI.fn_deliveryNo)]
        public String deliveryNo = null;
        [ORMapping(PoData_EDI.fn_descr)]
        public String descr = null;
        [ORMapping(PoData_EDI.fn_editor)]
        public String editor = null;
        [ORMapping(PoData_EDI.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(PoData_EDI.fn_model)]
        public String model = null;
        [ORMapping(PoData_EDI.fn_poNo)]
        public String poNo = null;
        [ORMapping(PoData_EDI.fn_qty)]
        public Int32 qty = int.MinValue;
        [ORMapping(PoData_EDI.fn_shipDate)]
        public String shipDate = null;
        [ORMapping(PoData_EDI.fn_status)]
        public String status = null;
        [ORMapping(PoData_EDI.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
