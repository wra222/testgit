using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::PartType))]
    [Serializable]
    public class PartTypeDef
    {
        [ORMapping(mtns::PartType.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(mtns::PartType.fn_code)]
        public String code = null;
        [ORMapping(mtns::PartType.fn_cust)]
        public String cust = null;
        [ORMapping(mtns::PartType.fn_description)]
        public String description = null;
        [ORMapping(mtns::PartType.fn_editor)]
        public String editor = null;
        [ORMapping(mtns::PartType.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(mtns::PartType.fn_indx)]
        public String indx = null;
        [ORMapping(mtns::PartType.fn_site)]
        public String site = null;
        [ORMapping(mtns::PartType.fn_tp)]
        public String tp = null;
        [ORMapping(mtns::PartType.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
