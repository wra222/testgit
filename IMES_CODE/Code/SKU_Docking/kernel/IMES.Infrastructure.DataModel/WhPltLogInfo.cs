using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Wh_Pltlog))]
    [Serializable]
    public class WhPltLogInfo
    {
        [ORMapping(Wh_Pltlog.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Wh_Pltlog.fn_editor)]
        public String editor = null;
        [ORMapping(Wh_Pltlog.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Wh_Pltlog.fn_plt)]
        public String plt = null;
        [ORMapping(Wh_Pltlog.fn_wc)]
        public String wc = null;
    }
}
