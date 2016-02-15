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
using IMES.FisObject.PCA.MB;

namespace IMES.CheckItemModule.CQ.CheckIMEI.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CheckIMEI.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "Check IMEI";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
            Hashtable share_part_no_set = new Hashtable();
            Hashtable qty_share_parts_set = new Hashtable();
            Hashtable descr_parts_set = new Hashtable();
            IFlatBOM ret = null;
            Boolean imeicheck = false;
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            try
            {
                IProduct product = GetProduct(main_object);
                if (bom.FirstLevelNodes != null)
                {
                    IList<IBOMNode> bomNodeList = bom.FirstLevelNodes;
                    foreach (IBOMNode bomNode in bomNodeList)
                    {
                        if (bomNode.Part.PN == "6060BWWAN001")
                        {
                            imeicheck = true;
                        }

                    }
                }
                if (imeicheck)
                {
                    var mb_repository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    IMB mb = mb_repository.Find(product.PCBID);
                    if (mb == null)
                    {
                        throw new FisException("CHK862", new string[] { });
                    }
                    string imei = (string)mb.GetExtendedProperty("IMEI");
                    if (string.IsNullOrEmpty(imei))
                    {
                         imei = (string)product.GetExtendedProperty("IMEI");
                    }

                    if (string.IsNullOrEmpty(imei))
                    {
                        throw new FisException("CQCHK50119", new string[] { });
                    }

                    string virtualPN = "IMEICheck" + "/" + imei;
                    string descr = "IMEICheck";
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
                    flat_bom_item.ValueType = imei; //存放Part match時檢查的值
                    flat_bom_items.Add(flat_bom_item);
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
