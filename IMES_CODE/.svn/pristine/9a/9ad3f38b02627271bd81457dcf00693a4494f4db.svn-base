using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(ApprovalStatus))]
    [Serializable]
    public class ApprovalStatusInfo
    {
       [ORMapping(ApprovalItem.fn_id)]
        public Int64 ID = long.MinValue;

        [ORMapping(ApprovalStatus.fn_approvalItemID)]
        public Int64 ApprovalItemID = long.MinValue;

        [ORMapping(ApprovalStatus.fn_moduleKeyValue)]
        public String ModuleKeyValue = null;

        [ORMapping(ApprovalStatus.fn_status)]
        public String Status = null;

        [ORMapping(ApprovalStatus.fn_comment)]
        public String Comment = null;

        [ORMapping(ApprovalStatus.fn_editor)]
        public String Editor = null;

        [ORMapping(ApprovalStatus.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(ApprovalStatus.fn_udt)]
        public DateTime Udt = DateTime.MinValue;      
    }
}
