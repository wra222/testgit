// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-21   200038                       Create
// Known issues:
//

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.PXNCT.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.PXNCT.Filter.dll")]
    public class MatchModule: IMatchModule
    {

        /// <summary>
        /// 前5位等于VC
        /// </summary>
        /// <param name="subject">value to be matched</param>
        /// <param name="bomItem">bomItem</param>
        /// <param name="station">station</param>
        /// <returns></returns>
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
            IList<IPart> parts = ((IFlatBOMItem)bomItem).AlterParts;
            if (parts == null)
            {
                return ret;
            }

            foreach (IPart part in parts)
            {
                if (part.PN.Equals(subject.Trim()))
                {
                    ret = new PartUnit(subject.Trim(), string.Empty, part.BOMNodeType, 
                                        part.Type, "", part.CustPn, 
                                        ((IFlatBOMItem)bomItem).CheckItemType);
                    break;
                }
            }
            return ret;
        }
    }
}
