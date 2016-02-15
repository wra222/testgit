using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using System.ComponentModel;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::PilotMo))]
    [Serializable]
    public class PilotMoInfo
    {
        [ORMapping(mtns::PilotMo.fn_mo)]
        public string mo = null;

        [ORMapping(mtns::PilotMo.fn_stage)]
        public string stage = null;

        [ORMapping(mtns::PilotMo.fn_moType)]
        public string moType = null;

        [ORMapping(mtns::PilotMo.fn_model)]
        public string model = null;

        [ORMapping(mtns::PilotMo.fn_partNo)]
        public string partNo = null;

        [ORMapping(mtns::PilotMo.fn_vendor)]
        public string vendor = null;

        [ORMapping(mtns::PilotMo.fn_causeDescr)]
        public string causeDescr = null;

        [ORMapping(mtns::PilotMo.fn_qty)]
        public int qty = int.MinValue;

        [ORMapping(mtns::PilotMo.fn_planStartTime)]
        public DateTime planStartTime = DateTime.MinValue;

        [ORMapping(mtns::PilotMo.fn_state)]
        public string state = null;

        [ORMapping(mtns::PilotMo.fn_combinedQty)]
        public int combinedQty = int.MinValue;        
        
        [ORMapping(mtns::PilotMo.fn_combinedState)]
        public string combinedState = null;

        [ORMapping(mtns::PilotMo.fn_refID)]
        public string refID = null;

        [ORMapping(mtns::PilotMo.fn_remark)]
        public string remark = null;

        [ORMapping(mtns::PilotMo.fn_editor)]
        public String editor = null;

        [ORMapping(mtns::PilotMo.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;

        [ORMapping(mtns::PilotMo.fn_udt)]
        public DateTime udt = DateTime.MinValue;
       
    }

    public enum PilotMoStateEnum{
         Open=1,
         Release,
        Hold
    }

    public enum PilotMoCombinedStateEnum
    {
        Empty=1,
        Partial,
        Full
    }
}
