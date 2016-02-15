using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Pak_Whplt_Type))]
    [Serializable]
    public class PakWhPltTypeInfo
    {
        [ORMapping(Pak_Whplt_Type.fn_bol)]
        public String bol = null;
        [ORMapping(Pak_Whplt_Type.fn_carrier)]
        public String carrier = null;
        [ORMapping(Pak_Whplt_Type.fn_cdt)] 
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Pak_Whplt_Type.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Pak_Whplt_Type.fn_plt)] 
        public String plt = null;
        [ORMapping(Pak_Whplt_Type.fn_tp)]
        public String tp = null;
    }
}
