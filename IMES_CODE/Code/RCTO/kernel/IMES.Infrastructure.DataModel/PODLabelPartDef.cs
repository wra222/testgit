using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Podlabelpart))]
    [Serializable]
    public class PODLabelPartDef
    {
        [ORMapping(Podlabelpart.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Podlabelpart.fn_editor)]
        public String editor = null;
        [ORMapping(Podlabelpart.fn_family)]
        public String family = null;
        [ORMapping(Podlabelpart.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Podlabelpart.fn_partNo)]
        public String partNo = null;
        [ORMapping(Podlabelpart.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
