using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(Mbcfg))]
    public class MBCFGDef
    {
        [ORMapping(Mbcfg.fn_mbcode)]
        public string MBCode = null;
        [ORMapping(Mbcfg.fn_series)]
        public string Series = null;
        [ORMapping(Mbcfg.fn_tp)]
        public string TP = null;
        [ORMapping(Mbcfg.fn_cfg)]
        public string CFG = null;
        [ORMapping(Mbcfg.fn_editor)]
        public string Editor = null;
        [ORMapping(Mbcfg.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;
        [ORMapping(Mbcfg.fn_udt)]
        public DateTime Udt = DateTime.MinValue;
        [ORMapping(Mbcfg.fn_id)]
        public int ID = int.MinValue;
    }
}
