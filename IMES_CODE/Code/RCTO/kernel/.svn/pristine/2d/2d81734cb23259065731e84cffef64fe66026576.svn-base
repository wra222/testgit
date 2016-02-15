// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create

using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
namespace IMES.CheckItemModule.SecondPizzaID.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.SecondPizzaID.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            //Pizza ID 的状态(IMES_PAK..PizzaStatus.Status <> 'OK' 和'MP'，则报告错误：“Pizza 状态错误!”
            if (partUnit != null)
            {
                string pizza_id = ((PartUnit) partUnit).Sn.Trim();
                if (pizza_id.Length == 10)
                {
                    pizza_id = pizza_id.Substring(0, 9);
                }
                IPizzaRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                PizzaStatus pizza_status = pizza_repository.GetPizzaStatus(pizza_id);
                if (pizza_status != null)
                {
                    if (!(pizza_status.StationID.Trim().Equals("OK") || pizza_status.StationID.Trim().Equals("MP")))
                    {
                        throw new FisException("CHK182", new string[] { });
                    }
                }
//                string sn = ((PartUnit) part_unit).Sn;
                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IList<ProductInfo> products = product_repository.GetProductInfoListByKeyAndValue("KIT2", pizza_id);
                if (products != null && products.Count > 0)
                {
//                    foreach (var product_info in products)
//                    {
                    ProductInfo product_info = products[0];
                    List<string> erpara = new List<string>();
                    erpara.Add("Part");
                    erpara.Add(pizza_id);
                    erpara.Add(product_info.ProductID);
                    var ex = new FisException("CHK009", erpara);
                    throw ex;
//                    }
                }
            }
        }
    }
}
