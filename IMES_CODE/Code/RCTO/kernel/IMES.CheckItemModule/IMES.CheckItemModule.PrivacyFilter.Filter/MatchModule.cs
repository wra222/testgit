using System;
using System.Collections.Generic;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.PrivacyFilter.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.PrivacyFilter.Filter.dll")]
    class MatchModule : IMatchModule
    {
        public object Match(string subject, object bomItem, string station)
        {
            //Input=Pn
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
                    if (flat_bom_item.PN.Equals(subject.Trim()))
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
                }
//                foreach (IPart flat_bom_item in flat_bom_items)
//                {
//
//                    string part_pn = flat_bom_item.PN.Trim();
//                    if (part_pn.Length > 3)
//                    {
//                        part_pn = part_pn.Substring(3, part_pn.Length - 3);
//                        if (part_pn.Length > 10)
//                        {
//                            part_pn = part_pn.Substring(0, 10);
//                        }
//                        if (part_pn.Equals(subject.Trim()) || part_pn.Equals(subject.Trim()))
//                        {
//                            ret = new PartUnit(subject.Trim(), string.Empty,flat_bom_item.BOMNodeType,flat_bom_item.Type, "", flat_bom_item.CustPn,((IFlatBOMItem) bom_item).CheckItemType);
//                            break;
//                        }
//                    }
//                }
            }
            return ret;
        }
    }
}
