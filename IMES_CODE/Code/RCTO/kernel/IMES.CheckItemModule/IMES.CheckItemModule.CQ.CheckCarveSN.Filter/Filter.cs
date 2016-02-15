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
using IMES.FisObject.Common.Model;

namespace IMES.CheckItemModule.CQ.CheckCarveSN.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CheckCarveSN.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const int qty = 1;
        private const string part_check_type = "CheckCarveSN";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IFlatBOM ret = null;
            bool NeedCheckContent = false;
            string mn = "";
            string pn = "";
            string warranty = "";
            try
            {
                IProduct product = GetProduct(main_object);
                IList<ConstValueTypeInfo> lstConstValueType = partRep.GetConstValueTypeList("CheckCarveSN");
                if (null != lstConstValueType && lstConstValueType.Count > 0)
                {
                    foreach (ConstValueTypeInfo cvt in lstConstValueType)
                    {
                        if (cvt.value == product.Family)
                        {
                            NeedCheckContent = true;
                            break;
                        }
                    }
                }

                if (NeedCheckContent)
                {
                    IList<IMES.FisObject.Common.Model.ModelInfo> rmnList = modelRep.GetModelInfoByModelAndName(product.Model, "MN1");
                    if (rmnList != null && rmnList.Count > 0)
                    {
                         mn = rmnList[0].Value.Trim();
                    }
                    IList<IMES.FisObject.Common.Model.ModelInfo> pnList = modelRep.GetModelInfoByModelAndName(product.Model, "PN");
                    if (pnList != null && pnList.Count > 0)
                    {
                        pn = pnList[0].Value.Trim();
                    }
                    IList<IMES.FisObject.Common.Model.ModelInfo> warrantyList = modelRep.GetModelInfoByModelAndName(product.Model, "WARRANTY");
                    if (warrantyList != null && warrantyList.Count > 0)
                    {
                        warranty = warrantyList[0].Value.Trim()+",";
                    }
                    string carveSn = product.CUSTSN+"," + mn+"," + pn+"," + warranty.Replace(",", "y");

                    string virtualPN = product.CUSTSN;
                    //  string virtualPN = product.CUSTSN  + mn  + pn  + warranty.Replace(",", "y");
                    string descr = "Check CarveSN";
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
                    flat_bom_item.ValueType = carveSn.ToUpper(); //存放Part match時檢查的值
                    //flat_bom_item.ValueType = "5CG505000BHPDESKTOPMINIDVD-WRITERODDMODULE801433-0021Y1Y0Y"; //存放Part match時檢查的值
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