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
using IMES.FisObject.Common.Model;

namespace IMES.CheckItemModule.CQ.MasterLabel.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.MasterLabel.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "MasterLabel";

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

                    var masterLabelPart =( from p in bomNodeList
                                            where p.Part.PN == "60NOMSTLBL01"
                                            select new { Qty = p.Qty, PartNo = p.Part.PN }).FirstOrDefault();
                    if (masterLabelPart != null)
                    {
                        IProduct product=GetProduct(main_object);
                        IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                        IList<IMES.FisObject.Common.Model.ModelInfo> pcid = modelRep.GetModelInfoByModelAndName(product.Model, "PCID");
                        IPart part = new Part()
                        {
                            PN = masterLabelPart.PartNo+"/"+pcid[0].Value,      //PN要跟 flat_bom_item.PartNoItem值一樣                  
                            CustPn = "",
                            Remark = "",
                            Descr = "Master Label",
                            Descr2 = "",
                            Type = "MasterLabel",
                            Udt = DateTime.Now,
                            Cdt = DateTime.Now
                        };

                        var flat_bom_item = new FlatBOMItem(masterLabelPart.Qty, part_check_type, new List<IPart>() { part });
                        flat_bom_item.PartNoItem = masterLabelPart.PartNo + "/" + pcid[0].Value;
                        flat_bom_item.Tp = "MasterLabel";
                        flat_bom_item.Descr = "Check Master Label";
                        flat_bom_item.ValueType = pcid[0].Value; //存放Part match時檢查的值
                        flat_bom_items.Add(flat_bom_item);
                    } 
                }

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
