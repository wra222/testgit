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
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;


namespace IMES.CheckItemModule.FirstPizzaID.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.FirstPizzaID.Filter.dll")]
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
            //若@Data 长度为10位，并且第1位是今年或者去年的西元年最后一位，则用户刷入的是1st Pizza ID Barcode，@Data 前9位为真正的1st Pizza ID
            //Vincent 2014-3-4 for CQ Change Pizza Id check rule
            if (subject.Length == 10)
            {
                string pattern=@"^\d[A-Z][1-9,A-C]\d{6}\w$";
                string now_year = DateTime.Now.ToString("yyyy");
                string last_year = DateTime.Now.AddYears(-1).ToString("yyyy");
                if ((now_year.Substring(now_year.Length - 1) == subject.Substring(0,1) ||
                     last_year.Substring(last_year.Length - 1) == subject.Substring(0, 1)) && Regex.IsMatch(subject,pattern) )
                {
                    //是1st Pizza ID Barcode
                    IList<IPart> parts = ((IFlatBOMItem)bomItem).AlterParts;
                    Part part = null;
                    if (parts != null)
                    {
                        part = (Part)parts.ElementAt(0);
                    }
                    
                    string first_pizza_id = subject.Substring(0, 9);
                    if (part != null)
                    {
                        ret = new PartUnit(part.PN, first_pizza_id, part.BOMNodeType, part.Type, "", part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);                        
                    }
                }
            }
            return ret;
        }
    }
}
