using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;

namespace IMES.CheckItemModule.KBCheck.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.KBCheck.Filter.dll")]
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
            PartUnit ret = null;
            //14码，前5位等于VC
            if (subject.Length == 14 || subject.Length == 18)
            {
                FlatBOMItem item = (FlatBOMItem)bomItem;
                if (item.PartNoItem.Equals(subject.Trim().Substring(0, 5)))
                {
                    if (!item.ValueType.Equals(subject.Trim()))
                    {
                        //need change error Code
                        throw new FisException("FIL005", new string[] { subject });
                    }
                    IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                    if (parts != null && parts.Count>0)
                    {
                        ret = new PartUnit(parts[0].PN, subject.Trim(), parts[0].BOMNodeType, parts[0].Type,
                                                          string.Empty, parts[0].CustPn,
                                                          ((FlatBOMItem)bomItem).CheckItemType);
                    }

                } 
            }
            return ret;
        }
    }
}
