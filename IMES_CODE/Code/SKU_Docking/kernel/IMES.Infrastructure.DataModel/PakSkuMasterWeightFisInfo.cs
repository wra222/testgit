using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::Pak_Skumasterweight_Fis))]
    [Serializable]
    public class PakSkuMasterWeightFisInfo
    {
        [ORMapping(mtns::Pak_Skumasterweight_Fis.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(mtns::Pak_Skumasterweight_Fis.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(mtns::Pak_Skumasterweight_Fis.fn_model)]
        public String model = null;
        [ORMapping(mtns::Pak_Skumasterweight_Fis.fn_weight)]
        public Decimal weight = decimal.MinValue;
    }
}
