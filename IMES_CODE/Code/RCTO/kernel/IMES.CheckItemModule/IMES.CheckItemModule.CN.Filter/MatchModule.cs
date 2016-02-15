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
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.CN.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CN.Filter.dll")]
    public class MatchModule : IMatchModule
    {
        public object Match(string subject, object bomItem, string station)
        {
            //Input=Pn
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

            if (12 == subject.Trim().Length)
            {
                if (subject.Substring(0,2).Equals("CN") || subject.Substring(0,2).Equals("7E"))
                {
                    IPart part = null;
                    if (flat_bom_items != null)
                    {
                        part = flat_bom_items.ElementAt(0);
                    }
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
