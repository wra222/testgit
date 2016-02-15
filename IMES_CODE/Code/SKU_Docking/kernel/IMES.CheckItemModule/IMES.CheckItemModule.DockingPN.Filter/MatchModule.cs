using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.DockingPN.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingPN.Filter.dll")]
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
//            IList<IPart> flat_bom_items = ((IFlatBOMItem)bom_item).AlterParts;
//            IPart part = null;
//            if (flat_bom_items != null)
//            {
//                part = flat_bom_items.ElementAt(0);
//            }

            //若@Data 中存在字符'#'，则用户刷入的是Docking Part No

            if (subject.Contains("#"))
            {
                IList<IPart> flat_bom_items = ((IFlatBOMItem)bomItem).AlterParts;
                if (flat_bom_items != null)
                {
                    foreach (IPart flat_bom_item in flat_bom_items)
                    {
                        if (flat_bom_item.PN.Trim().Equals(subject.Trim()))
                        {
                            ret = new PartUnit(flat_bom_item.PN.Trim(), subject.Trim(), flat_bom_item.BOMNodeType, flat_bom_item.Type, string.Empty, flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                            break;
                        }
                    }
                }
            }
            return ret;
        }
    }
}
