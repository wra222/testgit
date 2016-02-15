using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IIMES.CheckItemModule.AIO.CRLCM.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.AIO.CRLCM.Filter.dll")]
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

            string vendor_code = "";
            if (subject.Length == 14 || subject.Length == 18)
            {
                vendor_code = subject;
            }
            //如大于90码  从77码开始取14码，取得的14码的前5位等于VC，截取的Key为Match与Save的PartSn
            if (subject.Length > 90)
            {
                vendor_code = subject.Substring(76, 14);
            }
            if (vendor_code.Length == 14 || vendor_code.Length == 18)
            {
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                if (parts != null)
                {
                    foreach (IPart part in parts)
                    {
                        IList<PartInfo> part_infos = part.Attributes;
                        if (part_infos != null)
                        {
                            foreach (PartInfo part_info in part_infos)
                            {
                                if (part_info.InfoType.Equals("VendorCode"))
                                {
                                    if (part_info.InfoValue.Trim().Equals(vendor_code.Trim().Substring(0, 5)))
                                    {
                                        ret = new PartUnit(part.PN, vendor_code.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
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
            }
            //如大于90码  从77码开始取14码，取得的14码的前5位等于VC，截取的Key为Match与Save的PartSn
            //            if (subject.Length > 90)
            //            {
            //                vendor_code = subject.Substring(76, 14);
            //                IList<IPart> parts = ((FlatBOMItem)bom_item).AlterParts;
            //                if (parts != null)
            //                {
            //                    foreach (IPart part in parts)
            //                    {
            //                        ret = new PartUnit(part.PN, vendor_code.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bom_item).CheckItemType);
            //                        break;
            //                    }
            //                }
            //            }
            return ret;
        }
    }
}
