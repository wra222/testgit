using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;

namespace IMES.CheckItemModule.CQ.CheckSlateBase.Filter
{

    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CheckSlateBase.Filter.dll")]
    class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            if (partUnit != null)
            {
                PartUnit pn = ((PartUnit)partUnit);
                string sn = pn.Sn;
                string prdid = pn.ProductId;
                if (!string.IsNullOrEmpty(sn))
                {
                    var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    IProduct slateproduct = productRepository.GetProductByIdOrSn(sn);
                    if (slateproduct == null)
                    {
                        throw new FisException("SFC002", new string[] { "CheckSlateBase:"+sn });
                    }
                    Session session = (Session)pn.CurrentSession;
                    if (session == null)
                    {
                        throw new FisException("CheckSlateBase:Can not get Session instance from SessionManager!");
                    }
                    Product curProduct = (Product)session.GetValue(Session.SessionKeys.Product);
                    if (curProduct == null)
                    {
                        throw new FisException("CheckSlateBase:No product object in session");
                    }
                     IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    IHierarchicalBOM  curBOM = bomRepository.GetHierarchicalBOMByModel(curProduct.Model);
                    IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
                    bomNodeLst = curBOM.FirstLevelNodes;
                    bool is_match = false;
                    if (bomNodeLst != null && bomNodeLst.Count > 0)
                    {
                        foreach (IBOMNode ibomnode in bomNodeLst)
                        {
                            IPart currentPart = ibomnode.Part;
                            if (("ZM"+slateproduct.Model).Trim().Equals(currentPart.PN.Trim()))
                            {
                                is_match = true;
                                break;
                            }
                        }
                    }        
                    if (!is_match)
                    {
                        throw new FisException("CQCHK50117", new string[] { slateproduct.Model });
                    }
                }

            }
        }
    }
}
