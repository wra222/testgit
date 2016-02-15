using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.TabletRoyaltyPaper.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TabletRoyaltyPaper.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            //a. Royalty Paper S/N 是否存在(IMES_PAK..COAStatus.COASN = @Data)c. 
            //状态必须是'P1' 或者'A2' 或者'A3' (IMES_PAK..COAStatus.Status？)，否则报告错误：“此号码不在可结合状态！”
            //d. 若用户刷入的Royalty Paper S/N 对应的Part No（IMES_PAK..COAStatus.IECPN） 
            //与前文Part List 中的Royalty Paper的Part No 不同，则报告错误' 此机型对应的Royalty Paper 为' + @RoyaltyPaperPartNo
            if (partUnit != null)
            {
                string sn = ((PartUnit)partUnit).Sn;
                if (!string.IsNullOrEmpty(sn))
                {
                    ICOAStatusRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                    COAStatus coa_status = pizza_repository.GetCoaStatus(sn);
                    if (coa_status == null)
                    {
                        throw new FisException("CQCHK0019", new string[] { "RoyaltyPaper", sn });
                    }
                    if (!(coa_status.Status.Equals("P1") || coa_status.Status.Equals("A2") || coa_status.Status.Equals("A3")))
                    {
                        throw new FisException("CHK175", new string[] { sn });
                    }
                    bool is_royalty = false;
                    string part_pn = "";
                    IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                    //只有一个Part。
                    foreach (IPart part in parts)
                    {
                        if (coa_status.IECPN.Equals(part.PN))
                        {
                            is_royalty = true;
                        }
                        part_pn = part.PN;
                    }
                    if (!is_royalty)
                    {
                        throw new FisException("CHK176", new string[] { part_pn });
                    }
                }
                else
                {
                    throw new FisException("CQCHK0019", new string[] { "RoyaltyPaper", "" });
                }
            }
            else
            {
                throw new FisException("CHK174", new string[] { });

            }
        }
    }
}
