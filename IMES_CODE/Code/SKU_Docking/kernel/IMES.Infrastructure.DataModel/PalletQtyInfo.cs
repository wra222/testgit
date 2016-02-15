using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(PalletStatndard))]
    public class PalletQtyInfo
    {
        [ORMapping(PalletStatndard.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(PalletStatndard.fn_editor)]
        public String editor = null;
        [ORMapping(PalletStatndard.fn_fullQty)]
        public String fullQty = null;
        [ORMapping(PalletStatndard.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(PalletStatndard.fn_litterQty)]
        public Int32 litterQty = int.MinValue;
        [ORMapping(PalletStatndard.fn_mediumQty)]
        public Int32 mediumQty = int.MinValue;
        [ORMapping(PalletStatndard.fn_tierQty)]
        public Int32 tierQty = int.MinValue;
        [ORMapping(PalletStatndard.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }
}
