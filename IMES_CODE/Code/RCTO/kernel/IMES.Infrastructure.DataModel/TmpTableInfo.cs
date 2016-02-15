using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(TmpTable))]
    [Serializable]
    public class TmpTableInfo
    {
        [ORMapping(TmpTable.fn_begSN)]
        public String begSN = null;
        [ORMapping(TmpTable.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(TmpTable.fn_cust)]
        public String cust = null;
        [ORMapping(TmpTable.fn_custPN)]
        public String custPN = null;
        [ORMapping(TmpTable.fn_descr)]
        public String descr = null;
        [ORMapping(TmpTable.fn_editor)]
        public String editor = null;
        [ORMapping(TmpTable.fn_endSN)]
        public String endSN = null;
        [ORMapping(TmpTable.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(TmpTable.fn_iecpn)]
        public String iecpn = null;
        [ORMapping(TmpTable.fn_mspn)]
        public String mspn = null;
        [ORMapping(TmpTable.fn_pc)]
        public String pc = null;
        [ORMapping(TmpTable.fn_po)]
        public String po = null;
        [ORMapping(TmpTable.fn_qty)]
        public Int32 qty = int.MinValue;
        [ORMapping(TmpTable.fn_shipDate)]
        public DateTime shipDate = DateTime.MinValue;
        [ORMapping(TmpTable.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(TmpTable.fn_upload)]
        public String upload = null;
    }
}
