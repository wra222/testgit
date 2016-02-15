using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(ApprovalItem))]
    [Serializable]
    public class ApprovalItemInfo
    {
        [ORMapping(ApprovalItem.fn_id)]
        public Int64 ID = long.MinValue;

        [ORMapping(ApprovalItem.fn_module)]
        public String Module = null;

        [ORMapping(ApprovalItem.fn_actionName)]
        public String ActionName = null;

        [ORMapping(ApprovalItem.fn_department)]
        public String Department = null;

        [ORMapping(ApprovalItem.fn_isNeedApprove)]
        public String IsNeedApprove = null;

        [ORMapping(ApprovalItem.fn_ownerEmail)]
        public String OwnerEmail = null;

        [ORMapping(ApprovalItem.fn_ccemail)]
        public String CCEmail = null;

        [ORMapping(ApprovalItem.fn_isNeedUploadFile)]
        public String IsNeedUploadFile = null;

        [ORMapping(ApprovalItem.fn_noticeMsg)]
        public String NoticeMsg = null;


        [ORMapping(ApprovalItem.fn_editor)]
        public String Editor = null;

        [ORMapping(ApprovalItem.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(ApprovalItem.fn_udt)]
        public DateTime Udt = DateTime.MinValue;      
    }
}
