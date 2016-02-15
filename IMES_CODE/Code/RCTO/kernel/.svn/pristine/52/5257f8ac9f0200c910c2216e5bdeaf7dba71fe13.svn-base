using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using System.Text.RegularExpressions;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.Util;
using IMES.Resolve.Common;

namespace IMES.CheckItemModule.CommonRules.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CommonRules.Filter.dll")]
    public class MatchModule : IMatchModule
    {
        public object Match(string subject, object bomItem, string station)
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
            FlatBOMItem flatBomitem = (FlatBOMItem)bomItem;

            IList<CheckItemTypeRuleDef> lstChkItemRule = flatBomitem.CheckItemTypeRuleList;
            if (null == lstChkItemRule || lstChkItemRule.Count == 0)
            {
                throw new FisException("No ChkItemTypeRule!!");
            }

            TagData tagData = (TagData)flatBomitem.Tag;
            if (tagData == null)
            {
                throw new FisException("No tagData!!");
            }

            IList<IPart> partList = flatBomitem.AlterParts;
            IPart matchPart = null;
            CheckItemTypeRuleDef chkItemRule = lstChkItemRule[0];
            string customPN = null;
            string iecPN = null;
            string preMatchPartNo = null;

            if (flatBomitem.RelationBomItem != null)
            {
                int relationQty = flatBomitem.RelationBomItem.CheckedPart.Count;
                int checkedQty = flatBomitem.CheckedPart.Count;
                if (relationQty == 0 ||
                  checkedQty >= relationQty)
                {
                    preMatchPartNo = null;
                }
                else
                {
                    preMatchPartNo = flatBomitem.RelationBomItem.CheckedPart[checkedQty].Pn;
                }
            }

            UtilityCommonImpl utl = UtilityCommonImpl.GetInstance();
            var filterMatchRule = utl.FilterMatchRule;

            if (chkItemRule.MatchRuleGroupNames == null || chkItemRule.MatchRuleGroupNames.Count == 0)  //No Regular Expression Group Name
            {
                if (Regex.IsMatch(subject, chkItemRule.MatchRule))
                {
                    if (preMatchPartNo != null)
                    {
                        matchPart = partList.Where(x => x.PN == preMatchPartNo).FirstOrDefault();
                        if (matchPart == null)
                        {
                            return ret;
                        }
                        customPN = filterMatchRule.GetCustomPN(chkItemRule, tagData, (Part)matchPart, subject, out iecPN);
                    }
                    else
                    {
                        matchPart = partList.Where(x =>
                        {
                            customPN = filterMatchRule.GetCustomPN(chkItemRule, tagData, (Part)x, subject, out iecPN);
                            return !string.IsNullOrEmpty(customPN);
                        }).FirstOrDefault();
                    }
                    if (matchPart == null)
                    {
                        matchPart = partList[0];
                    }

                    //CheckRule
                    if (filterMatchRule.CheckSubjectByCheckRule(chkItemRule, tagData, matchPart, subject))
                    {
                        ret = new PartUnit(matchPart.PN,
                            //subject, 
                                                filterMatchRule.GetPartSNBySaveRule(chkItemRule, tagData, matchPart, subject),
                                                matchPart.BOMNodeType,
                                                matchPart.Type,
                                                iecPN ?? string.Empty,
                                                customPN ?? string.Empty,
                                                flatBomitem.CheckItemType);
                    }
                }
            }
            else  // has Group Name case
            {
                string customer = tagData.Product == null ?
                    (tagData.PCB == null ? string.Empty : tagData.PCB.Customer)
                    : tagData.Product.Customer ?? string.Empty;

                string partSN = null;
                foreach (IPart part in partList)
                {
                    if (preMatchPartNo != null && preMatchPartNo != part.PN)
                    {
                        continue;
                    }
                    if (filterMatchRule.CheckSubjectByMatchRule(chkItemRule, subject, tagData, part, customer, out partSN))
                    {
                        if (filterMatchRule.CheckSubjectByCheckRule(chkItemRule, tagData, part, subject))
                        {
                            customPN = filterMatchRule.GetCustomPN(chkItemRule, tagData, part, subject, out iecPN);
                            ret = new PartUnit(part.PN,
                                                        string.IsNullOrEmpty(partSN) ? filterMatchRule.GetPartSNBySaveRule(chkItemRule, tagData, matchPart, subject) : partSN,
                                                        part.BOMNodeType,
                                                        part.Type,
                                                        iecPN ?? string.Empty,
                                                        customPN ?? string.Empty,
                                                        flatBomitem.CheckItemType);
                            break;
                        }
                    }
                }
            }
            return ret;
        }
    }
}
