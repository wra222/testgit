using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(SnoDet_BTLoc))]
    [Serializable]
    public class SnoDetBtLocInfo
    {
        [ORMapping(SnoDet_BTLoc.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(SnoDet_BTLoc.fn_cpqsno)]
        public String cpqsno = null;
        [ORMapping(SnoDet_BTLoc.fn_editor)]
        public String editor = null;
        [ORMapping(SnoDet_BTLoc.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(SnoDet_BTLoc.fn_pdLine)]
        public String pdLine = null;
        [ORMapping(SnoDet_BTLoc.fn_sno)]
        public String sno = null;
        [ORMapping(SnoDet_BTLoc.fn_snoId)]
        public String snoId = null;
        [ORMapping(SnoDet_BTLoc.fn_status)]
        public String status = null;
        [ORMapping(SnoDet_BTLoc.fn_tp)]
        public String tp = null;
        [ORMapping(SnoDet_BTLoc.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
