using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

namespace IMES.CheckItemModule.CQ.CheckSlateBase.Filter
{

    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CheckSlateBase.Filter.dll")]
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
            if (subject.Trim().Length == 9 || subject.Trim().Length==10)
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
