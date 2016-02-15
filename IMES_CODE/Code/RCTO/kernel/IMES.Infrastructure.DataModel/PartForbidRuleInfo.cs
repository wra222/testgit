using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(PartForbidRule))]
    [Serializable]
    public class PartForbidRuleInfo
    {

        [ORMapping(PartForbidRule.fn_id)]
        public long ID = long.MinValue;

        [ORMapping(PartForbidRule.fn_customer)]
        public string Customer = null;

        [ORMapping(PartForbidRule.fn_line)]
        public string Line = null;


        [ORMapping(PartForbidRule.fn_family)]
        public string Family = null;

        [ORMapping(PartForbidRule.fn_category)]
        public string Category = null;

        [ORMapping(PartForbidRule.fn_status)]
        public string Status = null;

        [ORMapping(PartForbidRule.fn_exceptModel)]
        public string ExceptModel = null;

        [ORMapping(PartForbidRule.fn_bomNodeType)]
        public string BomNodeType = null;

        [ORMapping(PartForbidRule.fn_vendorCode)]
        public string VendorCode = null;


        [ORMapping(PartForbidRule.fn_partNo)]
        public string PartNo = null;

        [ORMapping(PartForbidRule.fn_noticeMsg)]
        public string NoticeMsg = null;

        [ORMapping(PartForbidRule.fn_remark)]
        public string Remark = null;

        [ORMapping(PartForbidRule.fn_refID)]
        public string RefID = null;

        [ORMapping(PartForbidRule.fn_editor)]
        public string Editor = null;

        [ORMapping(PartForbidRule.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(PartForbidRule.fn_udt)]
        public DateTime Udt = DateTime.MinValue;


    }



}
