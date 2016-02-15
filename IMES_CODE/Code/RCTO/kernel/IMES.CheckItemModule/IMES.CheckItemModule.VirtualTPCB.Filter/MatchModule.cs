// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-19   200038                       Create
// Known issues:
//
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.VirtualTPCB.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.VirtualTPCB.Filter.dll")]
    class MatchModule : IMatchModule
    {
        /// <summary>
        /// 5码 = VendorCode
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="bomItem"></param>
        /// <param name="station"></param>
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

            if (subject.Length != 5)
            {
                return null;
            }
            IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
            if (parts == null)
            {
                return null;
            }
            foreach (IPart part in parts)
            {
                var vendorCode = part.GetAttribute("VendorCode");
                if (string.Compare(vendorCode, subject) == 0)
                {
                    ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                    break;
                }
            }
            return ret;
        }
    }
}
