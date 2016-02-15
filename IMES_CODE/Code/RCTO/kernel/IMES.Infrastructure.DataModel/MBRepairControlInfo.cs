using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;


namespace IMES.DataModel
{
   
    [Serializable]
    [ORMapping(typeof(MBRepairControl))]
    public class MBRepairControlInfo
    {
        [ORMapping(MBRepairControl.fn_id)]
        public int ID = int.MinValue;
        [ORMapping(MBRepairControl.fn_partNo)]
        public string PartNo = null;
        [ORMapping(MBRepairControl.fn_materialType)]
        public string MaterialType = null;

        [ORMapping(MBRepairControl.fn_stage)]
        public string Stage = null;

        [ORMapping(MBRepairControl.fn_family)]
        public string Family = null;
        [ORMapping(MBRepairControl.fn_pcbmodelid)]
        public string PCBModelID = null;
        [ORMapping(MBRepairControl.fn_line)]
        public string Line = null;
        [ORMapping(MBRepairControl.fn_location)]
        public string Location = null;
        [ORMapping(MBRepairControl.fn_qty)]
        public int Qty = int.MinValue;
        [ORMapping(MBRepairControl.fn_status)]
        public string Status = null;
        [ORMapping(MBRepairControl.fn_loc)]
        public string Loc = null;
        [ORMapping(MBRepairControl.fn_assignUser)]
        public string AssignUser = null;
        [ORMapping(MBRepairControl.fn_remark)]
        public string Remark = null;
        [ORMapping(MBRepairControl.fn_editor)]
        public string Editor = null;
        [ORMapping(MBRepairControl.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;
        [ORMapping(MBRepairControl.fn_udt)]
        public DateTime Udt = DateTime.MinValue;
    }
}
