using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(UPSPOAVPart))]
    [Serializable]
    public class UPSPOAVPartInfo
    {
        [ORMapping(UPSPOAVPart.fn_id)]
        public Int64 ID = long.MinValue;

        [ORMapping(UPSPOAVPart.fn_hppo)]
        public String HPPO = null;

        [ORMapping(UPSPOAVPart.fn_avpartno)]
        public String AVPartNo = null;

        [ORMapping(UPSPOAVPart.fn_iecpartno)]
        public String IECPartNo = null;

        [ORMapping(UPSPOAVPart.fn_iecparttype)]
        public String IECPartType = null;

        [ORMapping(UPSPOAVPart.fn_remark)]
        public String Remark = null;

        [ORMapping(UPSPOAVPart.fn_editor)]
        public String Editor = null;

        [ORMapping(UPSPOAVPart.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(UPSPOAVPart.fn_udt)]
        public DateTime Udt = DateTime.MinValue;      
    }
}
