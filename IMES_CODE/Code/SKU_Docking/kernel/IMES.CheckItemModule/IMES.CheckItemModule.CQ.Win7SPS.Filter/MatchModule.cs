using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.CQ.Win7SPS.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.Win7SPS.Filter.dll")]
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

           FlatBOMItem flatBomItem = (FlatBOMItem)bomItem;
           string SPSNo = flatBomItem.ValueType;
           if (SPSNo.Contains(subject.Trim()))
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

