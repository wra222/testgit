// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-03-13   210003                       ITC-1360-1374
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.RoyaltyPaper.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.RoyaltyPaper.Filter.dll")]
    public class MatchModule : IMatchModule
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
            //若@Data 长度为10 位，且包含'XP-' | 'UL-' | 'W7-'，或者@Data 长度为9位，且包含'V-'，则用户刷入的是Royalty Paper S/N
            bool is_royalty_paper_sn = false;
            if (10 == subject.Length)
            {
                if (subject.Contains("XP-") || subject.Contains("UL-") || subject.Contains("W7-") || subject.Contains("W8-"))
                {
                    is_royalty_paper_sn = true;
                }
            }
            if (9 == subject.Length)
            {
                if (subject.Contains("V-") && subject.IndexOf("V-") == 0)
                {
                    is_royalty_paper_sn = true;
                }
            }
            if (is_royalty_paper_sn)
            {
                IPart part = null;
                if (flat_bom_items != null)
                {
                    part = flat_bom_items.ElementAt(0);
                }
                if (part != null)
                {
                    ret = new PartUnit(part.PN, subject, part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);                    
                }
            }
            return ret;
        }
    }
}
