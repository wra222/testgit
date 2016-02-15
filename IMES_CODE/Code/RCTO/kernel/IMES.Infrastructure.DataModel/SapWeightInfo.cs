using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Sap_Weight))]
    [Serializable]
    public class SapWeightInfo
    {
        [ORMapping(Sap_Weight.fn__DNDIVShipment_)]
        public String dndivshipment = null;
        [ORMapping(Sap_Weight.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Sap_Weight.fn_kg)]
        public Decimal kg = decimal.MinValue;
        [ORMapping(Sap_Weight.fn_status)]
        public String status = null;
    }
}
