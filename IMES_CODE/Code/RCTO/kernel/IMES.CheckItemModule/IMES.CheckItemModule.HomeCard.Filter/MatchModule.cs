// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.HomeCard.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.HomeCard.Filter.dll")]
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
            IList<IPart> flat_bom_items = ((IFlatBOMItem)bomItem).AlterParts;
            //3.	若@Data 长度为14位，以'HPNB' 开头，则用户刷入的数据为CN Card No – 即前文的Home Card

            if (14 == subject.Trim().Length)
            {
                if (subject.Contains("HPNB") && subject.IndexOf("HPNB") == 0)
                {
                    IPart part = null;
                    if (flat_bom_items != null)
                    {
                        part = flat_bom_items.ElementAt(0);
                    }
                    if (part != null)
                    {
                        ret = new PartUnit(part.PN.Trim(), subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);    
                    }
                }
            }

            return ret;
        }
    }
}
