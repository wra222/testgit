using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CTNonBattery.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CTNonBattery.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object part_unit, object bom_item, string station)
        {
            if (part_unit != null)
            {
                //没有结合其它Product
                string partSn = ((PartUnit)part_unit).Sn.Trim();
                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct product = product_repository.GetProductByIdOrSn(partSn);
                if (product != null)
                {
                    if (product.ProId != ((PartUnit) part_unit).ProductId) //将会在PartUnit中增加ProId。
                    {
                        throw new FisException("CHK184", new string[] {});
                    }
                }
                if (!string.IsNullOrEmpty(partSn)
                    && partSn.Length == 14
                    && string.Compare(partSn, 0, "6", 0, 1) == 0)
                {
                    string hppn = partSn.Substring(0, 5);
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    var btInfo = partRepository.FindBattery(hppn);
                    if (btInfo == null || string.IsNullOrEmpty(btInfo.hssn))
                    {
                        throw new FisException("CHK873", new[] { hppn });
                    }
                }
            }
        }
    }
}
