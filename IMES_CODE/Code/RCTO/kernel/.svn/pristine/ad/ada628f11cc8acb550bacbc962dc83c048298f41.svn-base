using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.W8SPS.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.W8SPS.Filter.dll")]
    class MatchModule : IMatchModule
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

            if (subject.Trim().Length!=10)
            {           
                return null;
            }

           PartUnit ret = null; 

            IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
            string  SPSNo = ((FlatBOMItem)bomItem).PartNoItem;
            if (parts != null && parts.Count>0)
            {
                IPart part = parts[0];
                if (SPSNo.Contains (subject.Trim()))
                {
                    ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type,
                                                    string.Empty, part.CustPn,
                                                         ((FlatBOMItem)bomItem).CheckItemType);
                }
            }
       
            return ret;
        }
 
    }
}
