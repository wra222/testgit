using System;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [Serializable]
    [ORMapping(typeof(DefectHoldRule))]
    public class DefectHoldRuleInfo
    {
        [ORMapping(DefectHoldRule.fn_id)]
        public int ID = int.MinValue;

        [ORMapping(DefectHoldRule.fn_checkInStation)]
        public string CheckInStation = null;

        [ORMapping(DefectHoldRule.fn_line)]
        public string Line = null;


        [ORMapping(DefectHoldRule.fn_family)]
        public string Family = null;

        [ORMapping(DefectHoldRule.fn_defectCode)]
        public string DefectCode = null;

        [ORMapping(DefectHoldRule.fn_equalSameDefectCount)]
        public Nullable<int> EqualSameDefectCount = null;

        [ORMapping(DefectHoldRule.fn_overDefectCount)]
        public Nullable<int> OverDefectCount = null;

        [ORMapping(DefectHoldRule.fn_exceptCause)]
        public string ExceptCause = null;

        [ORMapping(DefectHoldRule.fn_holdStation)]
        public string HoldStation = null;

        
        [ORMapping(DefectHoldRule.fn_holdCode)]
        public string HoldCode = null;
        [ORMapping(DefectHoldRule.fn_holdDescr)]
        public string HoldDescr = null;

        [ORMapping(DefectHoldRule.fn_editor)]
        public string Editor = null;

        [ORMapping(DefectHoldRule.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue ;

        [ORMapping(DefectHoldRule.fn_udt)]
        public DateTime Udt = DateTime.MinValue;
    } 

}
