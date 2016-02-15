// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using IMES.DataModel;
using IMES.CheckItemModule.Utility;


namespace IMES.CheckItemModule.CheckValue.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CheckValue.Filter.dll")]
    public class MatchModule : IMatchModule
    {
        public Object Match(string subject, object bomItem, string station)
        {
            if (subject == null)
            {
                throw new ArgumentNullException();
            }
            if (bomItem == null)
            {
                throw new ArgumentNullException();
            }
            PartUnit ret = null;
            FlatBOMItem flatBomItem = (FlatBOMItem)bomItem;
            if (flatBomItem.CheckItemTypeRuleList != null &&
                flatBomItem.CheckItemTypeRuleList.Count > 0)
            {
                CheckItemTypeRuleDef ruleInfo = flatBomItem.CheckItemTypeRuleList[0];

                UtilityCommonImpl utl = UtilityCommonImpl.GetInstance();
                var filterMatchRule = utl.FilterMatchRule;
                if (string.IsNullOrEmpty(ruleInfo.MatchRule))
                {
                    if (subject == flatBomItem.ValueType)
                    {
                        ret = new PartUnit(flatBomItem.PartNoItem,  //Client 檢查FlatBomItem.PartNoItem 
                                                              subject,
                                                              flatBomItem.Descr,
                                                              flatBomItem.Tp,  //Client 檢查flatBomItem.Tp
                                                              string.Empty,
                                                              string.Empty,
                                                              flatBomItem.CheckItemType);
                    }
                }              
                else
                {
                    if (filterMatchRule.CheckSubjectByRegexGroup(ruleInfo, subject, flatBomItem.ValueType,
                                                                   ruleInfo.MatchRuleGroupNames==null || ruleInfo.MatchRuleGroupNames.Count == 0 ? null : ruleInfo.MatchRuleGroupNames.First().Key))
                    {
                        ret = new PartUnit(flatBomItem.PartNoItem,  //Client 檢查FlatBomItem.PartNoItem 
                                                               subject,
                                                               flatBomItem.Descr,
                                                               flatBomItem.Tp,  //Client 檢查flatBomItem.Tp
                                                               string.Empty,
                                                               string.Empty,
                                                               flatBomItem.CheckItemType);
                    }
                }
            }
            return ret;
        }
    }
}
