using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.FAN.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.FAN.Filter.dll")]
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
            //1) 前8位等于VC
            subject = subject.Trim();
            string vc = subject;
            if (subject.Length > 8)
                vc = subject.Substring(0, 8);

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
                                if (part_info.InfoValue.Trim().Equals(vc))
                                {
                                    ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type,string.Empty, part.CustPn,((FlatBOMItem) bomItem).CheckItemType);
                                    break;
                                }
                            }
                        }
                    }
                    if (ret != null)
                    {
                        break;
                    }
                }
            }
            return ret;
        }
    }
}
