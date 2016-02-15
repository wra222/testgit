// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
//          Date                          Reason                            Name                        
// ==========   =======================      ============
// 2014-05-20                         Create                         Vincent 
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.PAK.DN;

namespace IMES.CheckItemModule.CQ.ICASALabel.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.ICASALabel.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "ICASALabel";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();          

            IFlatBOM ret = null;
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            HierarchicalBOM bom = (HierarchicalBOM)hierarchical_bom;
            try
            {
                if (bom.FirstLevelNodes != null)
                {

                    IList<IBOMNode> bomNodeList = bom.FirstLevelNodes;

                    var ICASALabelList = from p in bomNodeList
                                            where p.Part.PN=="6060BICASA01"
                                            select new { Qty = p.Qty, PartNo = p.Part.PN };

                    if (ICASALabelList.Count() > 0)
                    {
                        IProduct product = GetProduct(main_object);
                       
                         var printRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();


                         if (!printRep.CheckPrintLogListByRange(product.ProId, "ICASA label"))
                         {
                             throw new FisException("CHK518", new string[] { });
                         }
                       
                    }
                  
                }
              
            }
            catch (Exception e)
            {
                throw;
            }

            return ret;
        }

        private IProduct GetProduct(object main_object)
        {
            string objType = main_object.GetType().ToString();
            IMES.Infrastructure.Session session = null;
            IProduct iprd = null;
            if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
                iprd = (IProduct)main_object;
            }
            else if (main_object.GetType().Equals(typeof(IMES.Infrastructure.Session)))
            {
                session = (IMES.Infrastructure.Session)main_object;
                iprd = (IProduct)session.GetValue(Session.SessionKeys.Product);
            }

            if (iprd == null)
            {
                throw new FisException("Can not get Product object in " + part_check_type + " module");
            }
            return iprd;
        }

    }
}
