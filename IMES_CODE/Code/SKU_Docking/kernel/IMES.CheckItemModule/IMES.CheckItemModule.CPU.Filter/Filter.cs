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
using System.Linq;
using System.Text.RegularExpressions;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.DataModel;using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
namespace IMES.CheckItemModule.CPU.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CPU.Filter.dll")]
    public class Filter : IFilterModule, ITreeTraversal
    {
        private const string part_check_type = "CPU";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchical_bom"></param>
        /// <param name="station"></param>
        /// <param name="main_object"></param>
        /// <returns></returns>
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展3阶，得到第一阶是BM和P1的part其下阶和下下阶(注意Qty需要相乘) [ BM->KP->VC]或者[ P1->KP->VC]，即KP和VC，以及第一阶是KP的part的第一阶及其下阶[KP->VC],即KP和VC，
            IFlatBOM ret = null;
            List<IFlatBOMItem> items = new List<IFlatBOMItem>();
            var parts = new List<IPart>();
            int qty = 0;
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;

            // mantis 324
             if(Uti.CheckCPUOnBoard(bom))
             {  return ret;}
           var tree_traversal = new TreeTraversal();
            try
            {
                IList<QtyParts> bm_check_conditon_nodes = tree_traversal.BreadthFirstTraversal(bom.Root, "BM->P1->KP", "P1", this,"BM");
                String descr = "";
                if (bm_check_conditon_nodes != null && bm_check_conditon_nodes.Count > 0)
                {
                    foreach (QtyParts bm_check_conditon_node in bm_check_conditon_nodes)
                    {
                        if (bom.FirstLevelNodes != null)
                        {
                            IList<IBOMNode> bom_nodes = bom.FirstLevelNodes;
                            foreach (IBOMNode bom_node in bom_nodes)
                            {
                                if (bom_node.Part.BOMNodeType.Equals("BM"))
                                {
                                    if (bom_node.Children != null)
                                    {
                                        IList<IBOMNode> p1_child_nodes = bom_node.Children;
                                        foreach (var p1_child_node in p1_child_nodes)
                                        {
                                            if (p1_child_node.Part != null && p1_child_node.Part.BOMNodeType.Equals("P1"))
                                            {
                                                if (!descr.Contains(p1_child_node.Part.Descr))
                                                {
                                                    if (descr.Length == 0)
                                                    {
                                                        descr = p1_child_node.Part.Descr;
                                                    }
                                                    else
                                                    {
                                                        if (!descr.Contains(p1_child_node.Part.Descr))
                                                        {
                                                            descr += "," + p1_child_node.Part.Descr;
                                                        }
                                                        
                                                    }
                                                    
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //var item = new FlatBOMItem(bm_check_conditon_node.Qty, part_check_type, bm_check_conditon_node.Parts);
                        //items.Add(item);
                        if(bm_check_conditon_node.Parts != null)
                        {
                            foreach (IPart cpu_part in bm_check_conditon_node.Parts)
                            {
                                parts.Add(cpu_part);
                            }
                        }
                    }
                    var item = new FlatBOMItem(bm_check_conditon_nodes.ElementAt(0).Qty, part_check_type, parts);
                    item.PartNoItem = bm_check_conditon_nodes.ElementAt(0).Parts.ElementAt(0).PN;
                    item.Descr = descr;
                    items.Add(item);
                }
 
                if (items.Count > 0)
                {
                    ret = new FlatBOM(items);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return ret;
        }

        public bool CheckCondition(object node)
        {
            //第一阶的Descr描述为( Descr  like '%CPU%'）
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_contain_cpu = ((BOMNode)node).Part.Descr.Trim().Contains("CPU");

            if (is_contain_cpu)
                return true;
            return false;
        }
        
              

    }
}
