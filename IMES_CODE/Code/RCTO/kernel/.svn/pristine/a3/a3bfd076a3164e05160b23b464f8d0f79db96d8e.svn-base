using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::MasterLabel))]
    [Serializable]
    public class MasterLabelInfo
    {
        [ORMapping(mtns::MasterLabel.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(mtns::MasterLabel.fn_code)]
        public String code = null;
        [ORMapping(mtns::MasterLabel.fn_editor)]
        public String editor = null;
        [ORMapping(mtns::MasterLabel.fn_family)]
        public String family = null;
        [ORMapping(mtns::MasterLabel.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(mtns::MasterLabel.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(mtns::MasterLabel.fn_vc)]
        public String vc = null;
    }
}
