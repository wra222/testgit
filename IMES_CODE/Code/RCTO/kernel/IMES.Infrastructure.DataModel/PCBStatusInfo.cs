using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(Pcbstatus))]
    public class PCBStatusInfo
    {
        [ORMapping(Pcbstatus.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Pcbstatus.fn_editor)]
        public String editor = null;
        [ORMapping(Pcbstatus.fn_line)]
        public String line = null;
        [ORMapping(Pcbstatus.fn_pcbno)]
        public String pcbno = null;
        [ORMapping(Pcbstatus.fn_station)]
        public String station = null;
        [ORMapping(Pcbstatus.fn_status)]
        public Int32 status = int.MinValue;
        [ORMapping(Pcbstatus.fn_testFailCount)]
        public Int32 testFailCount = int.MinValue;
        [ORMapping(Pcbstatus.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
