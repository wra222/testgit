// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-21   200038                       Create
// Known issues:
//

using System;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;

namespace IMES.CheckItemModule.MAC.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.MAC.Filter.dll")]
    public class MatchModule: IMatchModule
    {
        private const string Pattern = "0123456789ABCDEF";
        /// <summary>
        /// 12码，由’0123456789ABCDEF’组成
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

            //12码
            if ((subject.Length < 12))
            {
                return null;
            }
            else
            {
                subject = subject.Substring(0, 12);
            }
            //由’123456789ABCDEF’组成
            foreach (char c in subject)
            {
                if(!Pattern.Contains(c.ToString()))
                {
                    return null;
                }
            }

            ret = new PartUnit(((FlatBOMItem)bomItem).PartNoItem, subject.Trim(), "MAC", "MAC",
                               string.Empty, string.Empty,
                               ((FlatBOMItem)bomItem).CheckItemType);
            return ret;
        }
    }
}
