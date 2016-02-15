using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(SupplierCode))]
    [Serializable]
    public class SupplierCodeInfo
    {
        [ORMapping(SupplierCode.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(SupplierCode.fn_code)]
        public String code = null;
        [ORMapping(SupplierCode.fn_editor)]
        public String editor = null;
        [ORMapping(SupplierCode.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(SupplierCode.fn_idex)]
        public String idex = null;
        [ORMapping(SupplierCode.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(SupplierCode.fn_vendor)]
        public String vendor = null;
    }
}
