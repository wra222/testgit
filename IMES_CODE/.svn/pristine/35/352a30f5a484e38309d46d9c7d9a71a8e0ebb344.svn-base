using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CN.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CN.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {

            //没有与其他Kit绑定，不存在于Pizza_Part.Value
            //在Product.CUSTSN中存在
            //且
            //Product.Model等于CN阶下阶的pn
            //((IFlatBOMItem)bom_item).
            if (partUnit != null)
            {
                string pizza_id = ((PartUnit) partUnit).Sn.Trim();
                IPizzaRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                int count = pizza_repository.GetPizzaPartsCout(pizza_id);
                if (count > 0)
                {
                    throw new FisException("CHK183", new string[] { pizza_id });
                }

                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                var product = (Product) product_repository.GetProductByIdOrSn(pizza_id);
                if (product != null)
                {
                    var hierarchical_repository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    String model = ((IFlatBOMItem) bomItem).Model;
                    var hierarchical_bom = hierarchical_repository.GetHierarchicalBOMByModel(model);
                    //TreeTraversal tree_traversal = new TreeTraversal();
                    //IList<QtyParts> check_conditon_nodes = tree_traversal.BreadthFirstTraversal(hierarchical_bom.Root, "CN->PN", "PN", this);
                    Boolean is_pn_equal = false;
                    if (hierarchical_bom.FirstLevelNodes != null)
                    {
                        foreach (IBOMNode cn_node in hierarchical_bom.FirstLevelNodes)
                        {
                            if (cn_node.Part.BOMNodeType.Equals("CN"))
                            {
                                IList<IBOMNode> cn_next_nodes = cn_node.Children;
                                if (cn_next_nodes != null)
                                {
                                    foreach (IBOMNode cn_next_node in cn_next_nodes)
                                    {
                                        //if (cn_next_node.Part.PN.Trim().Equals(((PartUnit)part_unit).Pn.Trim()))
                                        if (cn_next_node.Part.PN.Trim().Equals(product.Model.Trim()))
                                        {
                                            is_pn_equal = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (is_pn_equal)
                    {
                        return;
                    }
                }
                throw new FisException("CHK861", new string[] { ((PartUnit)partUnit).Pn });
            }
            else
            {
                throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.CN.Filter.CheckModule.Check" });
            }
        }
    }
}
