using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(Pcb_Part))]
    public class PCBPartInfo
    {
        [ORMapping(Pcb_Part.fn_id)]
        public Int32 ID= int.MinValue;

        [ORMapping(Pcb_Part.fn_bomNodeType)]
        public string BomNodeType = null;

        [ORMapping(Pcb_Part.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(Pcb_Part.fn_checkItemType)]
        public string CheckItemType = null;

        [ORMapping(Pcb_Part.fn_custmerPn)]
        public string CustmerPn = null;

        [ORMapping(Pcb_Part.fn_editor)]
        public string Editor = null;

        [ORMapping(Pcb_Part.fn_iecpn)]
        public string IECPn = null;

        [ORMapping(Pcb_Part.fn_partNo)]
        public string PartNo = null;

        [ORMapping(Pcb_Part.fn_partSn)]
        public string PartSn = null;

        [ORMapping(Pcb_Part.fn_partType)]
        public string PartType = null;

        [ORMapping(Pcb_Part.fn_pcbno)]
        public string PCBNo = null;

        [ORMapping(Pcb_Part.fn_station)]
        public string Station = null;

        [ORMapping(Pcb_Part.fn_udt)]
        public DateTime Udt = DateTime.MinValue;        
    }


    [Serializable]
    [ORMapping(typeof(UnpackPCB_Part))]
    public class UnpackPCBPartInfo
    {
        [ORMapping(UnpackPCB_Part.fn_id)]
        public Int32 ID = int.MinValue;

        [ORMapping(UnpackPCB_Part.fn_bomNodeType)]
        public string BomNodeType = null;

        [ORMapping(UnpackPCB_Part.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(UnpackPCB_Part.fn_checkItemType)]
        public string CheckItemType = null;

        [ORMapping(UnpackPCB_Part.fn_custmerPn)]
        public string CustmerPn = null;

        [ORMapping(UnpackPCB_Part.fn_editor)]
        public string Editor = null;

        [ORMapping(UnpackPCB_Part.fn_iecpn)]
        public string IECPn = null;

        [ORMapping(UnpackPCB_Part.fn_partNo)]
        public string PartNo = null;

        [ORMapping(UnpackPCB_Part.fn_partSn)]
        public string PartSn = null;

        [ORMapping(UnpackPCB_Part.fn_partType)]
        public string PartType = null;

        [ORMapping(UnpackPCB_Part.fn_pcbno)]
        public string PCBNo = null;

        [ORMapping(UnpackPCB_Part.fn_station)]
        public string Station = null;

        [ORMapping(UnpackPCB_Part.fn_udt)]
        public DateTime Udt = DateTime.MinValue;

        [ORMapping(UnpackPCB_Part.fn_ueditor)]
        public String UEditor = null;

        [ORMapping(UnpackPCB_Part.fn_updt)]
        public DateTime UPdt = DateTime.MinValue;

        [ORMapping(UnpackPCB_Part.fn_pcb_partid)]
        public Int32 PCB_PartID = int.MinValue;
    }
}
