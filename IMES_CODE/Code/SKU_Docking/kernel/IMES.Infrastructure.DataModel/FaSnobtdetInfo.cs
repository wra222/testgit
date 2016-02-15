using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.Fa_Snobtdet))]
    [Serializable]
    public class FaSnobtdetInfo
    {
        [ORMapping(IMES.Infrastructure.Repository._Metas.Fa_Snobtdet.fn_bt)]
        public String bt = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Fa_Snobtdet.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Fa_Snobtdet.fn_editor)]
        public String editor = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Fa_Snobtdet.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Fa_Snobtdet.fn_remark)]
        public String remark = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Fa_Snobtdet.fn_snoId)]
        public String snoId = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Fa_Snobtdet.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
