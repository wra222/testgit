using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(AcAdapMaintain))]
    [Serializable]
    public class ACAdaptor
    {
        [ORMapping(AcAdapMaintain.fn_adppn)]
        public String adppn = null;
        [ORMapping(AcAdapMaintain.fn_agency)]
        public String agency = null;
        [ORMapping(AcAdapMaintain.fn_assemb)]
        public String assemb = null;
        [ORMapping(AcAdapMaintain.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(AcAdapMaintain.fn_cur)]
        public String cur = null;
        [ORMapping(AcAdapMaintain.fn_editor)]
        public String editor = null;
        [ORMapping(AcAdapMaintain.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(AcAdapMaintain.fn_supplier)]
        public String supplier = null;
        [ORMapping(AcAdapMaintain.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(AcAdapMaintain.fn_voltage)]
        public String voltage = null;
    }
}
