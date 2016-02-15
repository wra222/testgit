using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Rpt_PCARep))]
    [Serializable]
    public class RptPcaRepInfo
    {
        [ORMapping(Rpt_PCARep.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Rpt_PCARep.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Rpt_PCARep.fn_mark)]
        public String mark = null;
        [ORMapping(Rpt_PCARep.fn_remark)]
        public String remark = null;
        [ORMapping(Rpt_PCARep.fn_snoId)]
        public String snoId = null;
        [ORMapping(Rpt_PCARep.fn_status)]
        public String status = null;
        [ORMapping(Rpt_PCARep.fn_tp)]
        public String tp = null;
        [ORMapping(Rpt_PCARep.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(Rpt_PCARep.fn_username)]
        public String username = null;
    }
}
