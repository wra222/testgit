using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(Tsmodel))]
    [Serializable]
    public class TsModelInfo
    {
        [ORMapping(Tsmodel.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(Tsmodel.fn_editor)]
        public String editor = null;
        [ORMapping(Tsmodel.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(Tsmodel.fn_mark)]
        public String mark = null;
        [ORMapping(Tsmodel.fn_model)]
        public String model = null;
    }
}
