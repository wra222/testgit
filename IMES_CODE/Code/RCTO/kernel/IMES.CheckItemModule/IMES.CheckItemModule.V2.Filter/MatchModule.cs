using System;
using System.Linq; 
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.CheckItemModule.Utility;

namespace IMES.CheckItemModule.V2.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.V2.Filter.dll")]
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
            //14码，前5位等于VC
            if (subject.Length == 14)
            {
                IList<IPart> parts = ((FlatBOMItem) bomItem).AlterParts;
                if (parts == null)
                {
                    return ret;
                }
                IList<KPVendorCode> kpVCList = (IList<KPVendorCode>)((IFlatBOMItem)bomItem).Tag;
                if (kpVCList == null)
                {
                    return ret;
                }

                foreach (IPart part in parts)
                {
                    if (kpVCList.Any(x => x.PartNo == part.PN &&
                                                        x.VendorCode.IndexOf(subject.Substring(0, 5)) == 0))
                    {
                        ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                        break;
                    }

                    if (part.Attributes.Any(x => x.InfoType == "VendorCode" &&
                                                                              x.InfoValue.IndexOf(subject.Substring(0, 5)) == 0))
                    {
                        ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                        break;
                    } 


                    //IList<PartInfo> part_infos = part.Attributes;
                    //if (part_infos != null)
                    //{
                    //    foreach (PartInfo part_info in part_infos)
                    //    {
                    //        if (part_info.InfoType.Equals("VendorCode"))
                    //        {
                    //            if (part_info.InfoValue.Trim().Equals(subject.Trim().Substring(0, 5)))
                    //            {
                    //                ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    //if (ret != null)
                    //{
                    //    break;
                    //}
                }
            }
            return ret;
        }
    }
}
