using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Csnlog))]
    [Serializable]
    public class CSNLogInfo
    {
        [ORMapping(Csnlog.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Csnlog.fn_editor)]
        public String editor = null;
        [ORMapping(Csnlog.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Csnlog.fn_isPass)]
        public Int16 isPass = short.MinValue;
        [ORMapping(Csnlog.fn_pdLine)]
        public String pdLine = null;
        [ORMapping(Csnlog.fn_pno)]
        public String pno = null;
        [ORMapping(Csnlog.fn_snoId)]
        public String snoId = null;
        [ORMapping(Csnlog.fn_tp)]
        public String tp = null;
        [ORMapping(Csnlog.fn_wc)]
        public String wc = null;
    }
}
