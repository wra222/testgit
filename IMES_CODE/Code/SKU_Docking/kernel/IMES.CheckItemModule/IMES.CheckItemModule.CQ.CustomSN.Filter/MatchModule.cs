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


namespace IMES.CheckItemModule.CQ.CustomSN.Filterr
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CustomSN.Filter.dll")]
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

            FlatBOMItem flatBomItem = (FlatBOMItem)bomItem;
            if (flatBomItem.ValueType.Trim()== subject.Trim())
            {
               
                ret = new PartUnit(flatBomItem.PartNoItem,  //Client 檢查FlatBomItem.PartNoItem 
                                            subject,
                                            flatBomItem.Descr,
                                            flatBomItem.Tp,  //Client 檢查flatBomItem.Tp
                                            "", 
                                            "", 
                                            flatBomItem.CheckItemType);
               
            }           
          
            return ret;
        }
    }
}
