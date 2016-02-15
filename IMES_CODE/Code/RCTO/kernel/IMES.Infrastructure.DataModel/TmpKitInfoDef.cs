using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]

    [ORMapping(typeof(TmpKit))]
    public class TmpKitInfoDef
    {
        [ORMapping(TmpKit.fn_id)]
        public int ID = int.MinValue;

        [ORMapping(TmpKit.fn_pdLine)]
        public String PdLine = null;

        [ORMapping(TmpKit.fn_model)]
        public String Model = null;

        [ORMapping(TmpKit.fn_qty)]
        public int Qty = int.MinValue;

        [ORMapping(TmpKit.fn_type)]
        public String Type = null;
    }
}
