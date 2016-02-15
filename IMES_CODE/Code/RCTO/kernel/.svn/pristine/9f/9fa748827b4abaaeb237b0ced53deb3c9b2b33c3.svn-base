using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::CELDATA))]
    [Serializable]
    public class CeldataInfo
    {
        [ORMapping(mtns::CELDATA.fn_platform)]
        public String platform = null;
        [ORMapping(mtns::CELDATA.fn_productSeriesName)]
        public String productSeriesName = null;
        [ORMapping(mtns::CELDATA.fn_category)]
        public String category = null;
        [ORMapping(mtns::CELDATA.fn_grade)]
        public int grade = int.MinValue;
        [ORMapping(mtns::CELDATA.fn_tec)]
        public String tec = null;
        [ORMapping(mtns::CELDATA.fn_zmod)]
        public String zmod = null;
        [ORMapping(mtns::CELDATA.fn_editor)]
        public String editor = null;
        [ORMapping(mtns::CELDATA.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
    }
}
