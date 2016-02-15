using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.Special_Det))]
    [Serializable]
    public class SpecialDetInfo
    {
        [ORMapping(IMES.Infrastructure.Repository._Metas.Special_Det.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Special_Det.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Special_Det.fn_sno1)]
        public String sno1 = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Special_Det.fn_snoId)]
        public String snoId = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Special_Det.fn_tp)]
        public String tp = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Special_Det.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
