// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// Known issues:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.CheckItemModule.CQ.AstSN.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.AstSN.Filter.dll")]
    public class MatchModule : IMatchModule
    {
        public Object Match(string subject, object bomItem, string station)
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException();
            }
            if (bomItem == null)
            {
                throw new ArgumentNullException();
            }
            PartUnit ret = null;

            FlatBOMItem flatBomItem = (FlatBOMItem)bomItem;
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPart part = flatBomItem.AlterParts[0];
            SupplierCodeInfo matchCodeInfo = null;

            matchCodeInfo = productRepository.GetSupplierCodeByVendor(part.PN);
            if (matchCodeInfo == null)
            {
                throw new FisException("CQCHK1094", false, new string[] { part.PN , part.Descr});
            }

            matchCodeInfo = productRepository.GetSupplierCodeByVendorsAndAstLike(new string[] {part.PN}, subject);
            if (matchCodeInfo != null)
           {

               ret = new PartUnit(part.PN,  //Client 檢查FlatBomItem.PartNoItem 
                                            subject,
                                            part.BOMNodeType,
                                            part.Descr,  //Client 檢查flatBomItem.Tp
                                            "", 
                                           part.CustPn, 
                                            flatBomItem.CheckItemType);
               
            }  
            return ret;
        }
    }
}
