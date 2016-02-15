using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using System.Text.RegularExpressions;
using IMES.Infrastructure.Util;
using IMES.Resolve.Common;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CQ.SmallBoard.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.SmallBoard.Filter.dll")]
    public class MatchModule : IMatchModule
    {
        static readonly IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
        private const string IsSmallBoard = "IsSmallBoard";
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
            FlatBOMItem  flatBomitem=(FlatBOMItem)bomItem;

            IList<CheckItemTypeRuleDef> lstChkItemRule =flatBomitem.CheckItemTypeRuleList;
            if (null == lstChkItemRule || lstChkItemRule.Count == 0)
            {
                throw new FisException("No ChkItemTypeRule!!");
            }            

            IList<IPart> partList = flatBomitem.AlterParts;
            IPart matchPart = null;
            CheckItemTypeRuleDef chkItemRule = lstChkItemRule[0];

            IList<string> mbCodeList = (IList<string>)flatBomitem.Tag;

            if (Regex.IsMatch(subject, chkItemRule.MatchRule))
            { 
             
                    IMB mb=mbRep.Find(subject);
                    if (mb == null  || 
                        !mb.MBInfos.Any(x=>x.InfoType == IsSmallBoard && x.InfoValue== GlobalConstName.Letter.Y))
                    {
                        throw new FisException("ICT029", new string[] { subject });
                    }

                    if (string.IsNullOrEmpty(mb.PCBModelID))
                    {
                        throw new FisException("ICT028", new string[] {string.Empty, string.Join(",", partList.Select(x => x.PartNo).ToArray()) });
                    }

                    matchPart = partList.Where(x => x.PartNo == mb.PCBModelID).FirstOrDefault();
                    if (matchPart == null)
                    {
                        throw new FisException("ICT028", new string[] { mb.PCBModelID, string.Join(",", partList.Select(x=>x.PartNo).ToArray())});
                    }                   
                   

                    ret = new PartUnit(matchPart.PN,
                                                 subject,
                                                 matchPart.BOMNodeType,
                                                 matchPart.Type,
                                                 matchPart.PN,
                                                 string.Empty,
                                                 flatBomitem.CheckItemType);
              
                   
                           

            }

            return ret;           
        } 
    }
}
