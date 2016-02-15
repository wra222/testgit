using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Pak_Shipmentweight_Fis))]
    [Serializable]
    public class PakShipmentWeightFisInfo
    {
        [ORMapping(Pak_Shipmentweight_Fis.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Pak_Shipmentweight_Fis.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Pak_Shipmentweight_Fis.fn_shipment)]
        public String shipment = null;
        [ORMapping(Pak_Shipmentweight_Fis.fn_type)]
        public String type = null;
        [ORMapping(Pak_Shipmentweight_Fis.fn_weight)]
        public Decimal weight = decimal.MinValue;
    }
}
