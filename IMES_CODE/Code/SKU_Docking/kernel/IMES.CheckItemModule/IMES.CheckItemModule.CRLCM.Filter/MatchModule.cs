// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using System.Linq;
using IMES.Infrastructure;
namespace IMES.CheckItemModule.CRLCM.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CRLCM.Filter.dll")]
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
        
            List<string> pnLst = new List<string>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct partProduct=null;
            PartUnit ret = null;
            partProduct = productRepository.Find(subject);
       
            if (partProduct == null)
            { return ret; }
            var bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM hirBom=  bomRepository.GetHierarchicalBOMByModel(partProduct.Model);
           if (hirBom.FirstLevelNodes != null)
          {
              for (int i = 0; i < hirBom.FirstLevelNodes.Count; i++)
              {

                  IPart part = ((BOMNode)hirBom.FirstLevelNodes.ElementAt(i)).Part;
                  if (part.BOMNodeType == "PL" && part.Descr.Equals("Clean Room"))
                  { pnLst.Add(part.PN); }
              }
          
          }
          else
        { return ret; }
         if (pnLst.Count > 0)
          {
              IList<IPart> flat_bom_items = ((IFlatBOMItem)bomItem).AlterParts;
              if (partProduct != null && flat_bom_items != null)
              {
                  foreach (IPart flat_bom_item in flat_bom_items)
                  {
                      if (pnLst.Contains(flat_bom_item.PN.Trim()))
                      {
                          ret = new PartUnit(flat_bom_item.PN.Trim(), subject.Trim(), flat_bom_item.BOMNodeType, flat_bom_item.Type,
                                             "", "", ((IFlatBOMItem)bomItem).CheckItemType);
                      }
                  }
              }
          
          }
          
            //if (ret == null && partProduct!=null)
            //{
            //    throw new FisException("CHK1016", new string[] { subject, partProduct.Model });
            //}
            return ret;
        }
    }
}
