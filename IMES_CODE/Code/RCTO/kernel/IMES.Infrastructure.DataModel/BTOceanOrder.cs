using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.Bt_Seashipmentsku))]
    [Serializable]
    public class BTOceanOrder
    {
        [ORMapping(IMES.Infrastructure.Repository._Metas.Bt_Seashipmentsku.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Bt_Seashipmentsku.fn_editor)]
        public String editor = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Bt_Seashipmentsku.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Bt_Seashipmentsku.fn_model)]
        public String model = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Bt_Seashipmentsku.fn_pdLine)]
        public String pdLine = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Bt_Seashipmentsku.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
