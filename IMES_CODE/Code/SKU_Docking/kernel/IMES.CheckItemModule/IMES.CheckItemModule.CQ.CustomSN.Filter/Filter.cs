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
using IMES.CheckItemModule.Utility;

namespace IMES.CheckItemModule.CQ.CustomSN.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CustomSN.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "CustomSN";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>(); 
            IFlatBOM ret = null;
            try
            {

                IProduct product = Uti.GetProduct(main_object, part_check_type);
                if (string.IsNullOrEmpty(product.CUSTSN))
                {
                     throw new FisException("Product.CustomSN is empty in " + part_check_type + " module");
                }
                string virtualPN="CustomSN" + "/" + product.CUSTSN;
                string descr = "Check CustomSN";
                IPart part = new Part()
                {
                    PN = virtualPN,      //PN要跟 flat_bom_item.PartNoItem值一樣                  
                    CustPn = "",
                    Remark = "",
                    Descr = descr,
                    Descr2 = "",
                    Type = part_check_type,
                    Udt = DateTime.Now,
                    Cdt = DateTime.Now
                };
                var flat_bom_item = new FlatBOMItem(qty, part_check_type, new List<IPart>() { part });
                flat_bom_item.PartNoItem = virtualPN;
                flat_bom_item.Tp = part_check_type;
                flat_bom_item.Descr = descr;
                flat_bom_item.ValueType = product.CUSTSN; //存放Part match時檢查的值
                flat_bom_items.Add(flat_bom_item);
                 
                if (flat_bom_items.Count > 0)
                {
                    ret = new FlatBOM(flat_bom_items);
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
