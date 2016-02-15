using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(UPSModel))]
    [Serializable]
    public class UPSModelInfo
    {
        [ORMapping(UPSModel.fn_model)]
        public String Model = null;

        [ORMapping(UPSModel.fn_firstReceiveDate)]
        public DateTime FirstReceiveDate = DateTime.MinValue;

        [ORMapping(UPSModel.fn_lastReceiveDate)]
        public DateTime LastReceiveDate = DateTime.MinValue;

        [ORMapping(UPSModel.fn_status)]
        public String Status = null;

        [ORMapping(UPSModel.fn_remark)]
        public String Remark = null;

        [ORMapping(UPSModel.fn_editor)]
        public String Editor = null;

        [ORMapping(UPSModel.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(UPSModel.fn_udt)]
        public DateTime Udt = DateTime.MinValue;      
    }
}
