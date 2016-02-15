// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-03-24   210003                       UC changed

using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;

namespace IMES.CheckItemModule.DockingPN.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingPN.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            //用户刷入的数据需要与上文得到的Docking Part No 相同
            if (partUnit != null)
            {
                string pn = ((PartUnit) partUnit).Pn.Trim();
                bool exist_part_no = false;
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                foreach (IPart part in parts)
                {
                    if (part.PN.Equals(pn))
                    {
                        exist_part_no = true;
                    }
                }
                if (!exist_part_no)
                {
                    throw new FisException("CHK174", new string[] { pn });
                }
            }
        }
    }
}
