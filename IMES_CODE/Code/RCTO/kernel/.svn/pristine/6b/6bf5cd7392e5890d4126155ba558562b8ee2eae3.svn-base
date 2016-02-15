using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.VGA.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.VGA.Filter.dll")]
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
            //MBCode match   MBSNO的前两码
            //if (subject != null)
            //{
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                if (parts != null)
                {
                    foreach (IPart part in parts)
                    {
                        if (part.BOMNodeType.Trim().Equals("MB"))
                        {
                            IList<PartInfo> part_infos = part.Attributes;
                            if (part_infos != null)
                            {
                                foreach (PartInfo part_info in part_infos)
                                {
                                    if (part_info.InfoType.Trim().Equals("MB"))
                                    {
                                        if (part_info.InfoValue.Trim().Equals(subject.Trim().Substring(0,2)))
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
                //}
            }
            return ret;
        }
    }
}
