using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.Battery.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.BsamBattery.Filter.dll")]
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
           // 1）长14
           //2）第一码是“6”

            if (subject.Trim().Length == 14 && subject.Trim().StartsWith("6"))
            {
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;

                if (parts != null && parts.Count > 0)
                {
                    IPart part = parts[0];
                   
                    ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type,
                                                        string.Empty, part.CustPn,
                                                             ((FlatBOMItem)bomItem).CheckItemType);
                  
                }
           
            }
            return ret;
        }
    }
}
