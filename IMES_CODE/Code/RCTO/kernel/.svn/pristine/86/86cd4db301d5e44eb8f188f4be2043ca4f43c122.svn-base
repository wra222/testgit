// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-12-19   210003                       ITC-1360-1193
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using System.Text.RegularExpressions;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.CheckItemModule.Utility;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
namespace IMES.CheckItemModule.MB.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.MB.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "MB";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展1阶，得到第一阶是MB的part [BomNodeType=MB]的MBCode[PartInfo.InfoValue(InfoType='MB')]，
            IFlatBOM ret = null;
            var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            String mb_info_value_string = "";
            var bom = (HierarchicalBOM)hierarchical_bom;
             // mantis 324
            Session session = GetSession(main_object);
            session.AddValue("IsCPUOnBoard","");
            bool b = Uti.CheckCPUOnBoard(bom);
            if (!b)
            {
                if (CheckBomHaveCPU(bom))
                {
                   CheckCPUinProduct(main_object); 
                }
            }
            else
            { session.AddValue("IsCPUOnBoard","Y");}
            // mantis 324

            if (bom.FirstLevelNodes != null)
            {
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    if (bom.FirstLevelNodes.ElementAt(i).Part != null)
                    {
                        if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("MB"))
                        {
                            if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                            {
                                IList<PartInfo> part_infos = bom.FirstLevelNodes.ElementAt(i).Part.Attributes;
                                if (part_infos != null)
                                {
                                    foreach (PartInfo part_info in part_infos)
                                    {
                                        if (part_info.InfoType.Equals("MB"))
                                        {
                                            if (mb_info_value_string.Length == 0)
                                            {
                                                mb_info_value_string = part_info.InfoValue;
                                            }
                                            else
                                            {
                                                if (!mb_info_value_string.Contains(part_info.InfoValue))
                                                {
                                                    mb_info_value_string += "," + part_info.InfoValue;
                                                }
                                            }
                                        }
                                    }
                                }
                                _qty = bom.FirstLevelNodes.ElementAt(i).Qty;
                                IPart part = ((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Part;
                                parts.Add(part);
                            }
                        }
                    }
                }
            }
            if (parts.Count > 0)
            {
                if (bom.FirstLevelNodes != null && bom.FirstLevelNodes.Count > 0)
                {
                    var flat_bom_item = new FlatBOMItem(_qty, part_check_type, parts);
                    flat_bom_item.Descr = bom.FirstLevelNodes.ElementAt(0).Part.Descr;
                    flat_bom_item.PartNoItem = mb_info_value_string;
                    IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                    flat_bom_items.Add(flat_bom_item);
                    ret = new FlatBOM(flat_bom_items);
                }
            }
            return ret;
        }
        public bool CheckCondition(object node)
        {
            //MBCode[PartInfo.InfoValue(InfoType='MB')]，并且第一阶的PartInfo不存在InfoTyp='VGA' InfoValue='SV'
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_mb = false;
            bool is_vga_sv = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            foreach (PartInfo part_info in part_infos)
            {
                if (part_info.InfoType.Trim().Equals("MB") && !is_mb)
                {
                    if(!is_mb) is_mb = true;
                    continue;

                }
                if (part_info.InfoType.Trim().Equals("VGA") && part_info.InfoValue.Trim().Equals("SV"))
                {
                    if (!is_vga_sv) is_vga_sv = true;
                    continue;
                }
            }
            if (is_mb && !is_vga_sv)
                return true;
            return false;
        }

        private void CheckCPUinProduct(object main_object)
        {
           IProduct prd = GetProduct(main_object);
           IList<IProductPart> prdPartLst = prd.ProductParts;
           int i=  prdPartLst.Where(x => x.CheckItemType == "CPU").ToList().Count;
           if (i == 0)
           {
               throw new FisException("CHK561", new string[] { });
           }
           //if (!prdPartLst.Any(x => x.CheckItemType == "CPU"))
           //         {
           //             throw new FisException("CHK561",new string[]{});
           //         }
         
        }
        private IProduct GetProduct(object main_object)
        {
            string objType = main_object.GetType().ToString();
            Session session = null;
            IProduct iprd = null;
            if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
                iprd = (IProduct)main_object;
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

        private bool CheckBomHaveCPU(IHierarchicalBOM curBOM)
        { 
           bool havecpu=false ;
            IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;
                if (null != bomNodeLst)
                {
                    foreach (IBOMNode bn in bomNodeLst)
                    {
                        IPart p = bn.Part;
                        if (p.Descr != null && p.Descr.IndexOf("CPU") >= 0 )
                        {
                            havecpu = true;
                             break;
                        }
                       
                    }
                }
                return havecpu;
        }
    }
}
