using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Pak_Pqclog))]
    [Serializable]
    public class PakPqclogInfo
    {
        [ORMapping(Pak_Pqclog.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Pak_Pqclog.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Pak_Pqclog.fn_pdLine)]
        public String pdLine = null;
        [ORMapping(Pak_Pqclog.fn_pno)]
        public String pno = null;
        [ORMapping(Pak_Pqclog.fn_snoId)]
        public String snoId = null;
        [ORMapping(Pak_Pqclog.fn_wc)]
        public String wc = null;
    }
}
