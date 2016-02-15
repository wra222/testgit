using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Wh_Pltweight))]
    [Serializable]
    public class WhPltWeightInfo
    {
        [ORMapping(Wh_Pltweight.fn_actualCartonWeight)]
        public Decimal actualCartonWeight = decimal.MinValue;
        [ORMapping(Wh_Pltweight.fn_actualPltWeight)]
        public Decimal actualPltWeight = decimal.MinValue;
        [ORMapping(Wh_Pltweight.fn_dn)]
        public String dn = null;
        [ORMapping(Wh_Pltweight.fn_forecasetCartonWeight)]
        public Decimal forecasetCartonWeight = decimal.MinValue;
        [ORMapping(Wh_Pltweight.fn_forecasetPltWeight)]
        public Decimal forecasetPltWeight = decimal.MinValue;
        [ORMapping(Wh_Pltweight.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Wh_Pltweight.fn_plt)]
        public String plt = null;
        [ORMapping(Wh_Pltweight.fn_pltMaterialWeight)]
        public Decimal pltMaterialWeight = decimal.MinValue;
        [ORMapping(Wh_Pltweight.fn_pltQty)]
        public Int32 pltQty = int.MinValue;
        [ORMapping(Wh_Pltweight.fn_pltWeightInaccuracy)]
        public Decimal pltWeightInaccuracy = decimal.MinValue;
        [ORMapping(Wh_Pltweight.fn_remark)]
        public String remark = null;
    }
}
