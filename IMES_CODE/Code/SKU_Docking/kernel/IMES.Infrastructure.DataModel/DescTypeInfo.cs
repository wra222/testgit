using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::DescType))]
    [Serializable]
    public class DescTypeInfo
    {
        [ORMapping(mtns::DescType.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(mtns::DescType.fn_code)]
        public String code = null;
        [ORMapping(mtns::DescType.fn_description)]
        public String description = null;
        [ORMapping(mtns::DescType.fn_editor)]
        public String editor = null;
        [ORMapping(mtns::DescType.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(mtns::DescType.fn_tp)]
        public String tp = null;
        [ORMapping(mtns::DescType.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
