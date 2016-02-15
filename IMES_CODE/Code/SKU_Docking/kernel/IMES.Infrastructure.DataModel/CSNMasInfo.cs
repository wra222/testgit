using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Csnmas))]
    [Serializable]
    public class CSNMasInfo
    {
        [ORMapping(Csnmas.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Csnmas.fn_csn1)]
        public String csn1 = null;
        [ORMapping(Csnmas.fn_csn2)]
        public String csn2 = null;
        [ORMapping(Csnmas.fn_editor)]
        public String editor = null;
        [ORMapping(Csnmas.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Csnmas.fn_pdLine)]
        public String pdLine = null;
        [ORMapping(Csnmas.fn_pno)]
        public String pno = null;
        [ORMapping(Csnmas.fn_status)]
        public String status = null;
        [ORMapping(Csnmas.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
