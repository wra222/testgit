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
using IMES.CheckItemModule.Utility;

namespace IMES.CheckItemModule.CheckCT.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CheckCT.Filter.dll")]
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
            string ct = "";
            IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
            if (parts != null)
            {
                IList<KPVendorCode> kpVCList = (IList<KPVendorCode>)((IFlatBOMItem)bomItem).Tag;
                if (kpVCList == null)
                {
                    return ret;
                }

                foreach (IPart part in parts)
                {
                    if (kpVCList.Any(x => x.PartNo == part.PN &&
                                                      x.VendorCode==subject))
                    {
                        ret = new PartUnit(part.PN, subject, part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                        break;
                    }

                  //IList<string> lstCtKey = part.Attributes.Where(x => x.InfoType == "CT_KEY").Select(x => x.InfoValue).ToList();
                  //if (lstCtKey.Count > 0 && lstCtKey[0].Equals(subject))
                  //{
                  //     ret = new PartUnit(part.PN, subject, part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                   
                  // }
                  //  if (ret != null)
                  //  {
                  //      break;
                  //  }
                }
            }


            return ret;
        }
    }
}
