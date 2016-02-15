using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(PoData))]
    public class PoDataInfo
    {
        [ORMapping(PoData.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(PoData.fn_deliveryNo)]
        public String deliveryNo = null;
        [ORMapping(PoData.fn_descr)]
        public String descr = null;
        [ORMapping(PoData.fn_editor)]
        public String editor = null;
        [ORMapping(PoData.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(PoData.fn_model)]
        public String model = null;
        [ORMapping(PoData.fn_poNo)]
        public String poNo = null;
        [ORMapping(PoData.fn_qty)]
        public Int32 qty = int.MinValue;
        [ORMapping(PoData.fn_shipDate)]
        public String shipDate = null;
        [ORMapping(PoData.fn_shipment)]
        public String shipment = null;
        [ORMapping(PoData.fn_status)]
        public String status = null;
        [ORMapping(PoData.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
