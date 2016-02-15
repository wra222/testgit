// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.SecondPizzaID.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.SecondPizzaID.Filter.dll")]
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
            IList<IPart> flat_bom_items = ((IFlatBOMItem)bomItem).AlterParts;
            IPart part = null;
            if (flat_bom_items != null)
            {
                part = flat_bom_items.ElementAt(0);
            }
            
            //长度为10，且以'P' 开头，（前9位为真正的2nd Pizza ID）
            if (subject.Length == 10)
            {
                if (subject[0] == 'P')
                {
                    if (part != null)
                    {
                        ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                    }
                }
            }
            return ret;
        }
    }
}
