using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(Acadapmaintain))]
    public class ACAdapMaintainInfo
    {
        [ORMapping(Acadapmaintain.fn_adppn)]
        public String adppn = null;
        [ORMapping(Acadapmaintain.fn_agency)]
        public String agency = null;
        [ORMapping(Acadapmaintain.fn_assemb)]
        public String assemb = null;
        [ORMapping(Acadapmaintain.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Acadapmaintain.fn_cur)]
        public String cur = null;
        [ORMapping(Acadapmaintain.fn_editor)]
        public String editor = null;
        [ORMapping(Acadapmaintain.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Acadapmaintain.fn_supplier)]
        public String supplier = null;
        [ORMapping(Acadapmaintain.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(Acadapmaintain.fn_voltage)]
        public String voltage = null;
    }
}
