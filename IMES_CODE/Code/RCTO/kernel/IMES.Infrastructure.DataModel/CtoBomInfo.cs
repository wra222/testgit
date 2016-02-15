using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Ctobom))]
    [Serializable]
    public class CtoBomInfo
    {
        [ORMapping(Ctobom.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Ctobom.fn_descr)]
        public String descr = null;
        [ORMapping(Ctobom.fn_editor)]
        public String editor = null;
        [ORMapping(Ctobom.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Ctobom.fn_mpno)]
        public String mpno = null;
        [ORMapping(Ctobom.fn_qty)]
        public Int32 qty = int.MinValue;
        [ORMapping(Ctobom.fn_remark)]
        public String remark = null;
        [ORMapping(Ctobom.fn_spno)]
        public String spno = null;
        [ORMapping(Ctobom.fn_tp)]
        public String tp = null;
        [ORMapping(Ctobom.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
