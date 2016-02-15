using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Packinglist_RePrint))]
    [Serializable]
    public class PackinglistRePrintInfo
    {
        [ORMapping(Packinglist_RePrint.fn_dn)]
        public String dn = null;
        [ORMapping(Packinglist_RePrint.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Packinglist_RePrint.fn_model)]
        public String model = null;
        [ORMapping(Packinglist_RePrint.fn_shipDate)]
        public String shipDate = null;
    }
}
