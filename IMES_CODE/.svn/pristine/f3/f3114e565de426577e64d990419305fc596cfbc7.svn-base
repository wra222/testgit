using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.OfflinePizzaKitting.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.OfflinePizzaKitting.Filter.dll")]
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
            if (subject.Length == 14)
            {
                IList<IPart> flat_bom_items = ((IFlatBOMItem)bomItem).AlterParts;
                if (flat_bom_items != null)
                {
                    foreach (IPart flat_bom_item in flat_bom_items)
                    {
                        IList<PartInfo> part_infos = flat_bom_item.Attributes;
                        foreach (PartInfo part_info in part_infos)
                        {
                            if (part_info.InfoType.Equals("VendorCode"))
                            {
                                //对于Vendor CT，则遍历Part List 找到存在Vendor Code 属性的记录.
                                if (part_info.InfoValue.IndexOf(subject.Substring(0, 5)) == 0)
                                {
                                    ret = new PartUnit(flat_bom_item.PN, subject.Trim(), flat_bom_item.BOMNodeType, flat_bom_item.Type, string.Empty, flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                    break;
                                }
                            }
                        }
                        if (ret != null)
                        {
                            break;
                        }
                    }

                }
            }
            if (ret == null)
            {
                ret = (PartUnit)MatchPartNo(subject, bomItem, station);
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
                    }
                }
            }
            return ret;
        }
    }
}
