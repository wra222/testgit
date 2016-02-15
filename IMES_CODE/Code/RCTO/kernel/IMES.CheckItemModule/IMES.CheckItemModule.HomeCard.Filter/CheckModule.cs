using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.DataModel;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.HomeCard.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.HomeCard.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            //a.     CN Card No 是否存在(IMES_PAK..CSNMas.CSN2 = @Data)
            //b.    CN Card No 当前状态不为'P1'，则报告错误'Home Card 不可用！'
            //c.     若用户刷入的CN Card No 对应的Part No（IMES_PAK..CSNMas.Pno） 
            //与前文Part List 中的CN Card 的Part No 不同，则报告错误'Home Card 不匹配！'
            if (partUnit != null)
            {
                ICOAStatusRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                CSNMasInfo csn_mas_info = pizza_repository.GetCsnMas(((PartUnit)partUnit).Sn);
                if (csn_mas_info != null)
                {
                    if (!csn_mas_info.status.Equals("P1"))
                    {
                        throw new FisException("CHK178",new string[]{});
                    }
                    IList<IPart> parts = ((FlatBOMItem) bomItem).AlterParts;
                    foreach (IPart part in parts)
                    {
                        if (!part.PN.Equals(csn_mas_info.pno))
                        {
                            throw new FisException("CHK179",new string[]{});
                        }
                    }
                }
                else
                {
                    throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.HomeCard.Filter.CheckModule.Check" });
                }
            }
        }
    }
}
