﻿// INVENTEC corporation (c)2009 all rights reserved. 
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

namespace IMES.CheckItemModule.CQ.AnatelLabel.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.AnatelLabel.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "AnatelLabel";

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

                    var AnatelLabelPartList = from p in bomNodeList
                                              where p.Part.BOMNodeType == "PL" &&
                                                    p.Part.Descr == "Anatel label"
                                              select p.Part;
                    if (AnatelLabelPartList.Count() > 0)
                    {
                        IProduct product = GetProduct(main_object);
                        IList<IProductPart> productPartList = product.ProductParts;

                        if (productPartList.Any(x => x.PartSn.StartsWith("G")))
                        {
                            var printRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();


                            if (!printRep.CheckPrintLogListByRange(product.ProId, "Anatel label"))
                            {
                                throw new FisException("CHK519", new string[] { });
                            }
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