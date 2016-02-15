using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Battery))]
    [Serializable]
    public class BatteryInfo
    {
        [ORMapping(Battery.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Battery.fn_editor)]
        public String editor = null;
        [ORMapping(Battery.fn_hppn)]
        public String hppn = null;
        [ORMapping(Battery.fn_hssn)]
        public String hssn = null;
        [ORMapping(Battery.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Battery.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
