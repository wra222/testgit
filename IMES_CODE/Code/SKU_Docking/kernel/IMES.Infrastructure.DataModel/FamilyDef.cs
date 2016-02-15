using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{

    [Serializable]
    public struct FamilyDef
    {
        public String Family;
        public String Descr;
        public String CustomerID;
        public String Editor;
        public String cdt;
        public String udt;
    }


    [Serializable]
    [ORMapping(typeof(mtns.Family))]
    public class FamilyDefecr
    {
        [ORMapping(mtns.Family.fn_family)]
        public String Family = null;
        [ORMapping(mtns.Family.fn_descr)]
        public String Descr = null;
        [ORMapping(mtns.Family.fn_customerID)]
        public String CustomerID = null;
        [ORMapping(mtns.Family.fn_editor)]
        public String Editor = null;
        [ORMapping(mtns.Family.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(mtns.Family.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }

    [Serializable]
    [ORMapping(typeof(mtns.FamilyInfo))]
    public class FamilyInfoDef
    {
        [ORMapping(mtns.FamilyInfo.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(mtns.FamilyInfo.fn_descr)]
        public String descr = null;
        [ORMapping(mtns.FamilyInfo.fn_editor)]
        public String editor = null;
        [ORMapping(mtns.FamilyInfo.fn_family)]
        public String family = null;
        [ORMapping(mtns.FamilyInfo.fn_id)]
        public Int32 id = int.MinValue;
        [ORMapping(mtns.FamilyInfo.fn_name)]
        public String name = null;
        [ORMapping(mtns.FamilyInfo.fn_udt)]
        public DateTime udt = DateTime.MinValue;
        [ORMapping(mtns.FamilyInfo.fn_value)]
        public String value = null;
    }
}
