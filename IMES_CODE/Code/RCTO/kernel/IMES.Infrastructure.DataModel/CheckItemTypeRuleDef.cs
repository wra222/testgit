using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.CheckItemTypeRule))]
    [Serializable]
    public class CheckItemTypeRuleDef
    {
        [ORMapping(CheckItemTypeRule.fn_id)]
        public int ID = int.MinValue;

        [ORMapping(CheckItemTypeRule.fn_checkItemType)]
        public string CheckItemType = null;

        [ORMapping(CheckItemTypeRule.fn_line)]
        public string Line = null;

        [ORMapping(CheckItemTypeRule.fn_station)]
        public string Station = null;

        [ORMapping(CheckItemTypeRule.fn_family)]
        public string Family = null;

        [ORMapping(CheckItemTypeRule.fn_bomNodeType)]
        public string BomNodeType = null;

        [ORMapping(CheckItemTypeRule.fn_partDescr)]
        public string PartDescr = null;

        [ORMapping(CheckItemTypeRule.fn_partType)]
        public string PartType = null;

        [ORMapping(CheckItemTypeRule.fn_filterExpression)]
        public string FilterExpression = null;

        [ORMapping(CheckItemTypeRule.fn_matchRule)]
        public string MatchRule = null;

        [ORMapping(CheckItemTypeRule.fn_checkRule)]
        public string CheckRule = null;

        [ORMapping(CheckItemTypeRule.fn_saveRule)]
        public string SaveRule = null;

        [ORMapping(CheckItemTypeRule.fn_needUniqueCheck)]
        public string NeedUniqueCheck = null;

        [ORMapping(CheckItemTypeRule.fn_needCommonSave)]
        public string NeedCommonSave = null;

        [ORMapping(CheckItemTypeRule.fn_needSave)]
        public string NeedSave = null;

        [ORMapping(CheckItemTypeRule.fn_checkTestKPCount)]
        public string CheckTestKPCount = null;

        [ORMapping(CheckItemTypeRule.fn_descr)]
        public string Descr = null;

        [ORMapping(CheckItemTypeRule.fn_editor)]
        public String Editor = null;
        [ORMapping(CheckItemTypeRule.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;
        [ORMapping(CheckItemTypeRule.fn_udt)]
        public DateTime Udt = DateTime.MinValue;

        public int Priority;
        public IDictionary<string, bool> MatchRuleGroupNames { get; set; }
        public IDictionary<string, bool> CheckRuleGroupNames { get; set; }
        public IDictionary<string, bool> SaveRuleGroupNames { get; set; }
    }
}
