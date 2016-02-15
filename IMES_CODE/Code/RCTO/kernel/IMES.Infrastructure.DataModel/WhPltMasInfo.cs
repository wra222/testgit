using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Wh_Pltmas))]
    [Serializable]
    public class WhPltMasInfo
    {
        [ORMapping(Wh_Pltmas.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Wh_Pltmas.fn_editor)]
        public String editor = null;
        [ORMapping(Wh_Pltmas.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Wh_Pltmas.fn_plt)]
        public String plt = null;
        [ORMapping(Wh_Pltmas.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(Wh_Pltmas.fn_wc)]
        public String wc = null;
    }
}
