using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::AssetRange))]
    [Serializable]
    public class AssetRangeInfo
    {
        [ORMapping(mtns::AssetRange.fn__Begin_)]
        public String begin = null;
        [ORMapping(mtns::AssetRange.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(mtns::AssetRange.fn_code)]
        public String code = null;
        [ORMapping(mtns::AssetRange.fn_editor)]
        public String editor = null;
        [ORMapping(mtns::AssetRange.fn__End_)]
        public String end = null;
        [ORMapping(mtns::AssetRange.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(mtns::AssetRange.fn_remark)]
        public String remark = null;
        [ORMapping(mtns::AssetRange.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(mtns::AssetRange.fn_status)]
        public string status =null;
    }

    [Serializable]
    public class AssetRangeCodeInfo
    {
        public int ID {get; set;}
        public string Begin{get; set;}
        public string End{get; set;}
        public string Status { get; set; }
    }
}
