using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.Linq;
using IMES.FisObject.Common.Process;
namespace IMES.CheckItemModule.CQ.CRLCM.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CRLCM.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object part_unit, object bom_item, string station)
        {
            IProcessRepository CurrentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
            if (part_unit != null)
            {
                PartUnit pu = (PartUnit)part_unit;
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct partProduct = null;
                partProduct = productRepository.GetProductByCustomSn(pu.Sn);
               
                if (partProduct != null)
                {
                    FisException ex = new FisException("CQCHK0047", new string[] { pu.Sn });
                    ex.stopWF = false;
                    throw ex;
                }

            }
        }
    }
}
