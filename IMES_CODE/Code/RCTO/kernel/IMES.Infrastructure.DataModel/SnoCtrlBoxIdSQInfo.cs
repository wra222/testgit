using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(SnoCtrl_BoxId_SQ))]
    [Serializable]
    public class SnoCtrlBoxIdSQInfo
    {
        [ORMapping(SnoCtrl_BoxId_SQ.fn_boxId)]
        public String boxId = null;
        [ORMapping(SnoCtrl_BoxId_SQ.fn_cust)]
        public String cust = null;
    }
}
