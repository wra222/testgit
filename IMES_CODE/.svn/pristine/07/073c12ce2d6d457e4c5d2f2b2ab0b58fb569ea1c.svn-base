using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.TRIS.PL.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TRIS.PL.Filter.dll")]
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
            //14码 or 15码 or 18码，前5位等于VC
			subject = subject.Trim();
            if ((subject.Length == 14) || (subject.Length == 15) || (subject.Length == 18))
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
                                    if (part_info.InfoValue.Trim().Equals(subject.Substring(0, 5)))
                                    {
                                        ret = new PartUnit(part.PN, subject, part.BOMNodeType, part.Type,
                                                           string.Empty, part.CustPn,
                                                           ((FlatBOMItem)bomItem).CheckItemType);
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
