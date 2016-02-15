using System;
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

namespace IMES.CheckItemModule.CommnZJVC.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CommnZJVC.Filter.dll")]
    class MatchModule : IMatchModule
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

            IList<CheckItemTypeRuleDef> lstChkItemRule = ((FlatBOMItem)bomItem).CheckItemTypeRuleList;
            if (null == lstChkItemRule || lstChkItemRule.Count == 0)
            {
                throw new FisException("Err : no chkItemRule");
            }
            CheckItemTypeRuleDef chkItemRule = lstChkItemRule[0];

            Hashtable hash = new Hashtable();
            if (!UtilityCommonImpl.GetInstance().CheckByRegexGroup(chkItemRule.MatchRule, subject, hash))
                return null;

            if (!UtilityCommonImpl.GetInstance().CheckGroupNameByDescr(hash, chkItemRule.Descr, chkItemRule.CheckItemType))
                return null;

            IList<IPart> retParts = null;
            string vcode = hash["VCODE"] as string;
            if (!string.IsNullOrEmpty(vcode))
            {
                retParts = UtilityCommonImpl.GetInstance().VerifyGroupData_PartInfoCommon((FlatBOMItem)bomItem, null, vcode, "VCODE", "VCODE");
                if (retParts.Count == 0)
                {
                    // 刷入資料[]有誤!
                    //throw new FisException("CHK1127", new string[] { "VCODE" });
                    return null;
                }
            }
            else
            {
                vcode = "";
            }

            string vc = hash["VC"] as string;
            if (!string.IsNullOrEmpty(vc))
            {
                retParts = UtilityCommonImpl.GetInstance().VerifyGroupData_PartInfoCommon((FlatBOMItem)bomItem, null, vc, "VendorCode", "VC");
                if (retParts.Count == 0)
                {
                    // 刷入資料[]有誤!
                    //throw new FisException("CHK1127", new string[] { "VC" });
                    return null;
                }
            }

            if (retParts == null || retParts.Count == 0)
            {
                ret = (PartUnit)MatchPartNo(subject, bomItem, station);
            }
            else
            {
                IPart part = retParts[0];
                ret = new PartUnit(part.PN, subject, part.BOMNodeType, part.Type, string.Empty, vcode, ((FlatBOMItem)bomItem).CheckItemType);
            }
            return ret;
        }

        public Object MatchPartNo(string subject, object bomItem, string station)
        {
            string vendorCode = "";
            if (subject == null)
            {
                throw new ArgumentNullException();
            }
            if (bomItem == null)
            {
                throw new ArgumentNullException();
            }
            PartUnit ret = null;
            IList<IPart> flat_bom_items = ((IFlatBOMItem)bomItem).AlterParts;
            if (flat_bom_items != null)
            {
                foreach (IPart flat_bom_item in flat_bom_items)
                {
                    vendorCode = GetInfoType(flat_bom_item.Attributes, "VendorCode");
                    if (string.IsNullOrEmpty(vendorCode) && flat_bom_item.PN.Equals(subject.Trim()))
                    {
                        ret = new PartUnit(subject.Trim(), string.Empty, flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                        break;
                    }
                }
                if (ret == null)
                {
                    foreach (IPart flat_bom_item in flat_bom_items)
                    {
                        if (flat_bom_item.PN.Substring(0, 3).Equals("DIB"))
                        {

                            string part_pn = flat_bom_item.PN.Substring(3, flat_bom_item.PN.Length - 3);
                            if (part_pn.Equals(subject.Trim()))
                            {
                                ret = new PartUnit(flat_bom_item.PN.Trim(), string.Empty, flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                break;
                            }
                        }
                    }
                    if (ret == null)
                    {
                        foreach (IPart flat_bom_item in flat_bom_items)
                        {
                            string part_pn = flat_bom_item.PN.Trim();
                            if (part_pn.Length > 3)
                            {
                                part_pn = part_pn.Substring(3, part_pn.Length - 3);
                                if (part_pn.Length > 10)
                                {
                                    part_pn = part_pn.Substring(0, 10);
                                }
                                if (part_pn.Equals(subject.Trim()) || part_pn.Equals(subject.Trim()))
                                {
                                    ret = new PartUnit(flat_bom_item.PN.Trim(), string.Empty, flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                    break;
                                }
                            }
                        }
                        if (ret == null)
                        {
                            foreach (IPart flat_bom_item in flat_bom_items)
                            {
                                string part_pn = flat_bom_item.PN.Trim();
                                string TabletKBCT = subject.Trim();
                                if (subject.Trim().Length > 10)
                                {
                                    subject = subject.Substring(0, 10);
                                }
                                if (part_pn.Equals(subject.Trim()) || part_pn.Equals(subject.Trim()))
                                {
                                    ret = new PartUnit(flat_bom_item.PN.Trim(), TabletKBCT, flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                    break;
                                }

                            }
                        }
                    }
                }
            }
            return ret;
        }

        private string GetInfoType(IList<PartInfo> part_infos, string infoName)
        {
            string ret = "";
            foreach (PartInfo part_info in part_infos)
            {
                if (part_info.InfoType.Equals(infoName))
                {
                    ret = part_info.InfoValue.Trim();
                    break;
                }
            }
            return ret;
        }
    }
}
