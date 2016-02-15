using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CQ.LeapMotion.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.LeapMotion.Filter.dll")]
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

            if (subject.Length >= 20)
                subject = subject.Substring(subject.Length - 14, 14);
            
            //前5位等于VC
            if (subject.Length >= 5)
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
                                    if (part_info.InfoValue.Trim().Equals(subject.Trim().Substring(0, 5)))
                                    {
                                        ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type,
                                                           string.Empty, part.CustPn,
                                                           ((FlatBOMItem) bomItem).CheckItemType);
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
            }
            return ret;
        }
    }
}
