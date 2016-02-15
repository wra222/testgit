using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(ApprovalItemAttr))]
    [Serializable]
    public class ApprovalItemAttrInfo
    {
        [ORMapping(ApprovalItemAttr.fn_id)]
        public Int64 ID = long.MinValue;

        [ORMapping(ApprovalItemAttr.fn_approvalItemID)]
        public Int64 ApprovalItemID = long.MinValue;

        [ORMapping(ApprovalItemAttr.fn_attrName)]
        public String AttrName = null;

        [ORMapping(ApprovalItemAttr.fn_attrValue)]
        public String AttrValue = null;
       
        [ORMapping(ApprovalStatus.fn_editor)]
        public String Editor = null;

        [ORMapping(ApprovalStatus.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(ApprovalStatus.fn_udt)]
        public DateTime Udt = DateTime.MinValue;      
    }
}
