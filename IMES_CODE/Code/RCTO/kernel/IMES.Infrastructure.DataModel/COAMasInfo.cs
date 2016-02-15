using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Coamas))]
    [Serializable]
    public class COAMasInfo
    {
        [ORMapping(Coamas.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Coamas.fn_editor)]
        public String editor = null;
        [ORMapping(Coamas.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Coamas.fn_pdLine)]
        public String pdLine = null;
        [ORMapping(Coamas.fn_pno)]
        public String pno = null;
        [ORMapping(Coamas.fn_snoId)]
        public String snoId = null;
        [ORMapping(Coamas.fn_tp)]
        public String tp = null;
        [ORMapping(Coamas.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(Coamas.fn_wc)]
        public String wc = null;
    }
}
