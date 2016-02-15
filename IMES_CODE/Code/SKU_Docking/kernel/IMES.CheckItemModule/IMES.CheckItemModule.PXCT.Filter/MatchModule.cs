﻿// INVENTEC corporation (c)2009 all rights reserved. 
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

namespace IMES.CheckItemModule.PXCT.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.PXCT.Filter.dll")]
    public class MatchModule: IMatchModule
    {
        private const string VendorCodePropertyName = "VendorCode";

        /// <summary>
        /// 前5位等于VC
        /// </summary>
        /// <param name="subject">value to be matched</param>
        /// <param name="bomItem">bomItem</param>
        /// <param name="station">station</param>
        /// <returns></returns>
        public object Match(string subject, object bomItem, string station)
        {
            PartUnit ret = null;
            if (subject == null)
            {
                throw new ArgumentNullException();
            }
            if (bomItem == null)
            {
                throw new ArgumentNullException();
            }

            if (subject.Length < 5)
            {
                return null;
            }
            string prefix = subject.Substring(0, 5);

            IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
            if (parts == null)
            {
                return null;
            }

            foreach (IPart part in parts)
            {
                string vc = part.GetAttribute(VendorCodePropertyName);
                if (string.Compare(vc, prefix) == 0)
                {
                    ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type,
                                       string.Empty, part.CustPn,
                                       ((FlatBOMItem)bomItem).CheckItemType);
                    break;
                }
            }
            return ret;
        }
    }
}
