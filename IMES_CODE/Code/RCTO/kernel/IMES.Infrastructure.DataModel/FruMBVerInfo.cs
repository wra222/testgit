using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(FruMBVer))]
    public class FruMBVerInfo
    {
        [ORMapping(Smtct.fn_id)]
        public Int32 id = int.MinValue;

        [ORMapping(FruMBVer.fn_partNo)]
        public String partNo = null;

        [ORMapping(FruMBVer.fn_mbcode)]
        public String  mbCode= null;

        [ORMapping(FruMBVer.fn_ver)]
        public String ver = null;

        [ORMapping(FruMBVer.fn_remark)]
        public String remark = null;
                
        [ORMapping(FruMBVer.fn_editor)]
        public String editor = null;

        [ORMapping(FruMBVer.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;

        [ORMapping(FruMBVer.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
