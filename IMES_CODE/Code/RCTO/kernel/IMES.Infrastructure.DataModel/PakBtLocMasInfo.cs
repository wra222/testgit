using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Pak_Btlocmas))]
    [Serializable]
    public class PakBtLocMasInfo
    {
        [ORMapping(Pak_Btlocmas.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Pak_Btlocmas.fn_cmbQty)]
        public Int32 cmbQty = int.MinValue;
        [ORMapping(Pak_Btlocmas.fn_editor)]
        public String editor = null;
        [ORMapping(Pak_Btlocmas.fn_fl)]
        public String fl = null;
        [ORMapping(Pak_Btlocmas.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Pak_Btlocmas.fn_locQty)]
        public Int32 locQty = int.MinValue;
        [ORMapping(Pak_Btlocmas.fn_model)]
        public String model = null;
        [ORMapping(Pak_Btlocmas.fn_pdLine)]
        public String pdLine = null;
        [ORMapping(Pak_Btlocmas.fn_snoId)]
        public String snoId = null;
        [ORMapping(Pak_Btlocmas.fn_status)]
        public String status = null;
        [ORMapping(Pak_Btlocmas.fn_tp)]
        public String tp = null;
        [ORMapping(Pak_Btlocmas.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
