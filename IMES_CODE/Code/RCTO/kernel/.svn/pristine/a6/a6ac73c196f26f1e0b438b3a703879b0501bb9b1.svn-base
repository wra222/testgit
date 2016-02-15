// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-04-11   210003                      ITC-1360-1650
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;

namespace IMES.CheckItemModule.Asstage3.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.Asstage3.Filter.dll")]
    public class Filter : IFilterModule, ITreeTraversal
    {
        private const string part_check_type = "Asstage3";
    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchical_bom"></param>
        /// <param name="station"></param>
        /// <param name="main_object"></param>
        /// <returns></returns>
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IFlatBOM ret = null;
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            try
            {
                //Filter:
                // 展BOM Product_Part.BomNode= 'PP' and PartType='Asstage-3'
                // 檢查Product_Part.BomNode= 'PP' and PartType='Asstage-3'
                // 無Product_Part資料則報錯
                HierarchicalBOM bom = (HierarchicalBOM)hierarchical_bom;
                IList<IBOMNode> bomNodes = bom.GetNodesByNodeType("PP");
                if (bomNodes.Any(x => x.Part.Type == "Asstage-3"))
                {
                    Session session = GetSession(main_object);
                    IProduct prd = GetProduct(main_object);
                    IList<IProductPart> prdPartLst = prd.ProductParts;
                    if (!prdPartLst.Any(x => x.BomNodeType == "PP" && x.PartType == "Asstage-3"))
                    {
                        throw new FisException("CQCHK1057",new string[]{});
                    }
                 }
               return ret;
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
            Session session = null;
            IProduct iprd = null;
            if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
               iprd= (IProduct)main_object;
             }
            else if (main_object.GetType().Equals(typeof(IMES.Infrastructure.Session)))
            {
                session = (Session)main_object;
                iprd = (IProduct)session.GetValue(Session.SessionKeys.Product);
            }
         
            if (iprd == null)
            {
                throw new FisException("Can not get Product object in " + part_check_type + " module");
            }
            return iprd;
        }

        private Session GetSession(object main_object)
        {
            string objType = main_object.GetType().ToString();
            Session.SessionType sessionType = Session.SessionType.Product;
            Session session = null;
            if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
                IProduct prd = (IProduct)main_object;
                session = SessionManager.GetInstance.GetSession
                                               (prd.ProId, sessionType);
                if (session == null)
                {
                    session = SessionManager.GetInstance.GetSession
                                                   (prd.CUSTSN, sessionType);
                }
            }
            else if (main_object.GetType().Equals(typeof(IMES.Infrastructure.Session)))
            {
                session = (Session)main_object;
            }

            if (session == null)
            {
                throw new FisException("Can not get session in " + part_check_type + " module");
            }
            return session;
        }

        public bool CheckCondition(object node)
        { return false; }
    }
         
}
