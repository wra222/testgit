using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(DefectComponent))]
    [Serializable]
    public class DefectComponentInfo
    {

        [ORMapping(DefectComponent.fn_id)]
        public Int64 ID = long.MinValue;

        [ORMapping(DefectComponent.fn_repairID)]
        public int RepairID = int.MinValue;

        [ORMapping(DefectComponent.fn_batchID)]
        public string BatchID = null;

        [ORMapping(DefectComponent.fn_customer)]
        public string Customer = null;

        [ORMapping(DefectComponent.fn_model)]
        public string Model = null;

        [ORMapping(DefectComponent.fn_family)]
        public string Family = null;

        [ORMapping(DefectComponent.fn_defectCode)]
        public string DefectCode = null;

        [ORMapping(DefectComponent.fn_defectDescr)]
        public string DefectDescr = null;

        [ORMapping(DefectComponent.fn_returnLine)]
        public string ReturnLine = null;

        [ORMapping(DefectComponent.fn_partSn)]
        public string PartSn = null;

        [ORMapping(DefectComponent.fn_partNo)]
        public string PartNo = null;

        [ORMapping(DefectComponent.fn_partType)]
        public string PartType = null;

        [ORMapping(DefectComponent.fn_bomNodeType)]
        public string BomNodeType = null;

        [ORMapping(DefectComponent.fn_iecpn)]
        public string IECPn = null;

        [ORMapping(DefectComponent.fn_customerPn)]
        public string CustomerPn = null;

        [ORMapping(DefectComponent.fn_vendor)]
        public string Vendor = null;

        [ORMapping(DefectComponent.fn_checkItemType)]
        public string CheckItemType = null;

        [ORMapping(DefectComponent.fn_comment)]
        public string Comment = null;

        [ORMapping(DefectComponent.fn_status)]
        public string Status = null;
        
        [ORMapping(DefectComponent.fn_editor)]
        public string Editor = null;

        [ORMapping(DefectComponent.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(DefectComponent.fn_udt)]
        public DateTime Udt = DateTime.MinValue;


    }



    [ORMapping(typeof(DefectComponentLog))]
    [Serializable]
    public class DefectComponentLogInfo
    {

        [ORMapping(DefectComponentLog.fn_id)]
        public Int64 ID = long.MinValue;

        [ORMapping(DefectComponentLog.fn_actionName)]
        public string ActionName = null;

        [ORMapping(DefectComponentLog.fn_componentID)]
        public long ComponentID = long.MinValue;

        [ORMapping(DefectComponentLog.fn_repairID)]
        public int RepairID = int.MinValue;

        [ORMapping(DefectComponentLog.fn_partSn)]
        public string PartSn = null;

        [ORMapping(DefectComponentLog.fn_customer)]
        public string Customer = null;

        [ORMapping(DefectComponentLog.fn_model)]
        public string Model = null;

        [ORMapping(DefectComponentLog.fn_family)]
        public string Family = null;

        [ORMapping(DefectComponentLog.fn_defectCode)]
        public string DefectCode = null;

        [ORMapping(DefectComponentLog.fn_defectDescr)]
        public string DefectDescr = null;

        [ORMapping(DefectComponentLog.fn_returnLine)]
        public string ReturnLine = null;

        [ORMapping(DefectComponentLog.fn_remark)]
        public string Remark = null;

        [ORMapping(DefectComponentLog.fn_batchID)]
        public string BatchID = null;

        [ORMapping(DefectComponentLog.fn_comment)]
        public string Comment = null;

        [ORMapping(DefectComponentLog.fn_status)]
        public string Status = null;

        [ORMapping(DefectComponentLog.fn_editor)]
        public string Editor = null;

        [ORMapping(DefectComponentLog.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;       

    }

    [ORMapping(typeof(DefectComponentBatchStatus))]
    [Serializable]
    public class DefectComponentBatchStatusInfo
    {
        [ORMapping(DefectComponentBatchStatus.fn_batchID)]
        public string BatchID = null;

        [ORMapping(DefectComponentBatchStatus.fn_status)]
        public string Status = null;

        [ORMapping(DefectComponentBatchStatus.fn_printDate)]
        public DateTime PrintDate = DateTime.MinValue;

        [ORMapping(DefectComponentBatchStatus.fn_vendor)]
        public string Vendor = null;

        [ORMapping(DefectComponentBatchStatus.fn_returnLine)]
        public string ReturnLine = null;

        [ORMapping(DefectComponentBatchStatus.fn_totalQty)]
        public int TotalQty = int.MinValue;

        [ORMapping(DefectComponentBatchStatus.fn_editor)]
        public string Editor = null;

        [ORMapping(DefectComponentBatchStatus.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(DefectComponentBatchStatus.fn_udt)]
        public DateTime Udt = DateTime.MinValue;
    }

    [ORMapping(typeof(DefectComponentBatchStatusLog))]
    [Serializable]
    public class DefectComponentBatchStatusLogInfo
    {
        [ORMapping(DefectComponentBatchStatusLog.fn_id)]
        public Int64 id = long.MinValue;

        [ORMapping(DefectComponentBatchStatusLog.fn_batchID)]
        public string BatchID = null;

        [ORMapping(DefectComponentBatchStatusLog.fn_status)]
        public string Status = null;

        [ORMapping(DefectComponentBatchStatusLog.fn_editor)]
        public string Editor = null;

        [ORMapping(DefectComponentBatchStatusLog.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;
    }

}
