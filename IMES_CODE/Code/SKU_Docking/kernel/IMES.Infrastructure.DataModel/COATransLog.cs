using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.Coatrans_Log))]
    [Serializable]
    public class COATransLog
    {
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coatrans_Log.fn_begNo)]
        public String begNo = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coatrans_Log.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coatrans_Log.fn_editor)]
        public String editor = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coatrans_Log.fn_endNo)]
        public String endNo = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coatrans_Log.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coatrans_Log.fn_pno)]
        public String pno = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coatrans_Log.fn_preStatus)]
        public String preStatus = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Coatrans_Log.fn_status)]
        public String status = null;
    }
}
