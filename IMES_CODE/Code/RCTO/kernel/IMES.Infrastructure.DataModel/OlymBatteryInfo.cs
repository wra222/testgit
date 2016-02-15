using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(OlymBattery))]
    public class OlymBatteryInfo
    {
        [ORMapping(OlymBattery.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(OlymBattery.fn_editor)]
        public String editor = null;
        [ORMapping(OlymBattery.fn_hppn)]
        public String hppn = null;
        [ORMapping(OlymBattery.fn_hssn)]
        public String hssn = null;
        [ORMapping(OlymBattery.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(OlymBattery.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
