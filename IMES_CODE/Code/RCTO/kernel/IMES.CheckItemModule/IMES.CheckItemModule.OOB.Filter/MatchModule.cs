using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.OOB.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.OOB.Filter.dll")]
    class MatchModule : IMatchModule
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
            if (subject.Length == 13)
            {
                     IList<IPart> flat_bom_items = ((IFlatBOMItem) bomItem).AlterParts;
                     foreach (IPart flat_bom_item in flat_bom_items)
                     {
                         if (flat_bom_item.BOMNodeType.Equals("P1"))
                         {
                             IList<PartInfo> part_infos = flat_bom_item.Attributes;
                             if (part_infos != null)
                             {
                                 foreach (PartInfo part_info in part_infos)
                                 {
                                     if (part_info.InfoType.Trim().Equals("DESC"))
                                     {
                                         if (part_info.InfoValue.Trim().Equals("OOB"))
                                         {
                                             foreach (PartInfo part_infoiec in part_infos)
                                                 if (part_infoiec.InfoType.Trim().Equals("IEC") && part_infoiec.InfoValue.Trim() == subject.Trim())
                                             {
                                                 ret = new PartUnit(flat_bom_item.PN, subject.Trim(), flat_bom_item.BOMNodeType, flat_bom_item.Type,
                                                                                   string.Empty, flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                                 break;
                                             }
                                         }
                                     }
                                 }//foreach (PartInfo part_info in part_infos)
                             }// if (part_infos != null)
                         }
                     }     
                }
             
            return ret;
        }
    }
}
