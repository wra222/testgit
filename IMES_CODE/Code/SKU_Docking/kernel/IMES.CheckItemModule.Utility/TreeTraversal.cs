using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.Infrastructure;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
namespace IMES.CheckItemModule.Utility
{
    public class TreeTraversal
    {
        public IList<QtyParts> BreadthFirstTraversal(String part_no_filter_str,IBOMNode node, string search_path, string search_node_type, ITreeTraversal check_condition)
        {
            return BreadthFirstTraversal(part_no_filter_str,node,search_path,search_node_type,check_condition,null);
        }
        //FA代码已经稳定，所以加此函数供PAK使用。等PAK稳定后，再考虑代码合并。
        public IList<QtyParts> BreadthFirstTraversal(String part_no_filter_str,IBOMNode node, string search_path, string search_node_type, ITreeTraversal check_condition,string check_type)
        {
            List<IBOMNode> collect_gather_node = new List<IBOMNode>();
            List<QtyParts> ret = new List<QtyParts>();
            Queue<IBOMNode> bom_node_queue = new Queue<IBOMNode>(30);
            try
            {
                if (string.IsNullOrEmpty(search_path))
                {
                    throw new FisException("CHK169", new string[] { "IMES.CheckItemModule.Utility.TreeTraversal.BreadthFirstTraversal" });
                }
                else
                {
                    if (!string.IsNullOrEmpty(search_node_type.Trim()) && search_path.Contains(search_node_type.Trim()))
                    {
                        IBOMNode bom_node = node;
                        string[] trave_path = parse_paths(search_path);
                        if (bom_node != null)
                        {
                            bom_node_queue.Enqueue(node);
                            IBOMNode queue_node;
                            int search_depth = trave_path.Length;

                            while (bom_node_queue.Count > 0)
                            {
                                queue_node = bom_node_queue.Dequeue();
                                if (queue_node.Children != null && queue_node.Children.Count > 0)
                                {
                                    int offset = 1;
                                    for (int i = 0; i < queue_node.Children.Count; i++)
                                    {
                                        bom_node = queue_node.Children.ElementAt(i);
                                        if (bom_node.Part.BOMNodeType.Trim().Equals(trave_path[bom_node.Level - offset].Trim()) && bom_node.Level <= search_depth)
                                        {
                                            if (string.IsNullOrEmpty(check_type))
                                            {
                                                bom_node_queue.Enqueue(bom_node);
                                                if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                {
                                                    if (!string.IsNullOrEmpty(part_no_filter_str))
                                                    {
                                                        if (!bom_node.Part.PN.Trim().Substring(0, 3).Equals(part_no_filter_str.Trim()))
                                                        {
                                                            if (check_condition.CheckCondition(bom_node))
                                                            {
                                                                collect_gather_node.Add(bom_node);
                                                            }
                                                        }
                                                    }
                                                    else     //使用不需要PartNo过滤的情况
                                                    {
                                                        if (check_condition.CheckCondition(bom_node))
                                                        {
                                                            collect_gather_node.Add(bom_node);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                int check_level = toDepth(trave_path, check_type);
                                                if (bom_node.Level == check_level && bom_node.Part.BOMNodeType.Trim().Equals(check_type))
                                                {
                                                    if (!string.IsNullOrEmpty(part_no_filter_str))
                                                    {
                                                        if (!bom_node.Part.PN.Trim().Substring(0, 3).Equals(part_no_filter_str.Trim()))
                                                        {
                                                            if (check_condition.CheckCondition(bom_node))
                                                            {
                                                                bom_node_queue.Enqueue(bom_node);
                                                            }
                                                        }
                                                    }
                                                    else   //使用不需要PartNo过滤的情况
                                                    {
                                                        if (check_condition.CheckCondition(bom_node))
                                                        {
                                                            bom_node_queue.Enqueue(bom_node);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    bom_node_queue.Enqueue(bom_node);

                                                }
                                                if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                {
                                                    collect_gather_node.Add(bom_node);
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (queue_node.Level == search_depth)
                                    {
                                        String check_type_str = "";
                                        if (string.IsNullOrEmpty(check_type))
                                        {
                                            check_type_str =search_node_type.Trim();
                                        }
                                        else
                                        {
                                            check_type_str = check_type;
                                        }
                                        int check_level = toDepth(trave_path, check_type_str);
                                        if (queue_node.Part != null && queue_node.Level == check_level && queue_node.Part.BOMNodeType.Trim().Equals(check_type_str))
                                        {
                                            if (check_condition.CheckCondition(queue_node))
                                            {
                                                if (queue_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                {
                                                    if (!string.IsNullOrEmpty(part_no_filter_str))
                                                    {
                                                        if (!queue_node.Part.PN.Trim().Substring(0, 3).Equals(part_no_filter_str.Trim()))
                                                        {
                                                            if (!collect_gather_node.Contains(queue_node))
                                                            {
                                                                collect_gather_node.Add(queue_node);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (queue_node.Part != null && queue_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                            {
                                                if (!string.IsNullOrEmpty(part_no_filter_str))
                                                {
                                                    if (!queue_node.Part.PN.Trim().Substring(0, 3).Equals(part_no_filter_str.Trim()))
                                                    {
                                                        if (!collect_gather_node.Contains(queue_node))
                                                        {
                                                            collect_gather_node.Add(queue_node);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (queue_node.Level < search_depth)
                                    {
                                        IBOMNode parent_node = queue_node;
                                        while (parent_node != null)
                                        {
                                            if (contains(collect_gather_node, parent_node))
                                            {
                                                remove(collect_gather_node, parent_node);
                                            }
                                            parent_node = parent_node.Parent;
                                        }
                                    }
                                }
                            }

                            for (int j = 0; j < collect_gather_node.Count(); j++)
                            {
                                bom_node = collect_gather_node.ElementAt(j);

                                bom_node_queue.Enqueue(bom_node);
                                BOMNode tmp = (BOMNode)bom_node;

                                int qty = bom_node.Qty;
                                do
                                {
                                    if (tmp.Parent != null)
                                    {
                                        tmp = (BOMNode)tmp.Parent;
                                        qty *= tmp.Qty;
                                    }

                                } while (tmp.Level != node.Level);
                                //Packing Pizza 特例

                                if (bom_node.Children != null && bom_node.Children.Count > 0)
                                {
                                    qty *= ((BOMNode)bom_node.Children.ElementAt(0)).Qty;
                                }

//                                int qty = 0;
//                                if (bom_node.Part.BOMNodeType.Trim().Equals(search_node_type))
//                                {
//                                    qty = bom_node.Qty;
//                                }
//                                tmp = (BOMNode)bom_node;
//                                do
//                                {
//                                    if (tmp.Parent != null)
//                                    {
//                                        int search_node_level = toDepth(trave_path, search_node_type);
//
//                                        tmp = (BOMNode)tmp.Parent;
//                                        if (tmp.Part.BOMNodeType.Trim().Equals(search_node_type))
//                                        {
//                                            qty = tmp.Qty;
//                                        }
//                                        else
//                                        {
//                                            if (tmp.Level < search_node_level)
//                                            {
//                                                qty *= tmp.Qty;
//                                            }
//                                        }
//
//                                    }
//                                } while (tmp.Level != 1);

                                List<IPart> parts = new List<IPart>();
                                parts.Add(bom_node.Part);
                                QtyParts bom_item = new QtyParts(qty, parts);
                                ret.Add(bom_item);
                            }
                        }
                    }
                    else
                    {
                        string[] param = {
                                    "IMES.CheckItemModule.Utility.BreadthFirstTraversal",
                                    "search_node"
                                 };
                        throw new FisException("CHK156", param);
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }
            if (collect_gather_node.Count > 0)
                return ret;
            else
                return null;

        }
               /// <summary>
        /// BOMNode树的遍历，得到所需的BOMNode列表。
        /// </summary>
        /// <param name="node">遍历的开始节点</param>
        /// <param name="search_path">遍历的指定路径。以BOMNodeType为路径节点，用“->”将它们彼此隔开。</param>
        /// <param name="search_node_type">以给定的BOMNodeType收集BOMNode。</param>
        /// <param name="check_condition"></param>
        /// <param name="check_type"></param>
        /// <returns></returns>
        public IList<QtyParts> BreadthFirstTraversal(IBOMNode node, string search_path, string search_node_type, ITreeTraversal check_condition)
        {
           return BreadthFirstTraversal(node, search_path, search_node_type, check_condition, string.Empty);
        }
        /// <summary>
        /// BOMNode树的遍历，得到所需的BOMNode列表。
        /// </summary>
        /// <param name="node">遍历的开始节点</param>
        /// <param name="search_path">遍历的指定路径。以BOMNodeType为路径节点，用“->”将它们彼此隔开。</param>
        /// <param name="search_node_type">以给定的BOMNodeType收集BOMNode。</param>
        /// <param name="check_condition"></param>
        /// <param name="check_type"></param>
        /// <returns></returns>
        public IList<QtyParts> BreadthFirstTraversal(IBOMNode node, string search_path, string search_node_type, ITreeTraversal check_condition,string check_type)
        {
            List<IBOMNode> collect_gather_node = new List<IBOMNode>();
            List<QtyParts> ret = new List<QtyParts>();
            Queue<IBOMNode> bom_node_queue = new Queue<IBOMNode>(30);
            try
            {
                if (string.IsNullOrEmpty(search_path))
                {
                    throw new FisException("CHK169", new string[] { "IMES.CheckItemModule.Utility.TreeTraversal.BreadthFirstTraversal" });
                }
                else
                {
                    if (!string.IsNullOrEmpty(search_node_type.Trim()) && search_path.Contains(search_node_type.Trim()))
                    {
                        IBOMNode bom_node = node;
                        string[] trave_path = parse_paths(search_path);
                        //if (bom_node.Part.BOMNodeType.Trim().Equals(trave_path[bom_node.Level].Trim()))//由于Root.part为空，所以暂时去掉该句。
                        if (bom_node != null)
                        {
                            bom_node_queue.Enqueue(node);
                            IBOMNode queue_node;
                            //int obtain_depth = toDepth(trave_path, search_node_type);
                            int search_depth = trave_path.Length;

                            while (bom_node_queue.Count > 0)
                            {
                                queue_node = bom_node_queue.Dequeue();
                                //if (queue_node.IsRoot && queue_node.Level == 0)
                                //{
                                //    if (queue_node.Part == null)    //Root 为Model
                                //    {
                                //        if (queue_node.Children != null)
                                //        {
                                //            for (int i = 0; i < queue_node.Children.Count; i++)
                                //            {
                                //                bom_node = queue_node.Children.ElementAt(i);
                                //                if (bom_node.Part.BOMNodeType.Trim().Equals(trave_path[bom_node.Level - 1].Trim()) && bom_node.Level <= search_depth)
                                //                {
                                //                    if (string.IsNullOrEmpty(check_type))
                                //                    {
                                //                        bom_node_queue.Enqueue(bom_node);
                                //                        if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                //                        {
                                //                            if (check_condition.CheckCondition(bom_node))
                                //                            {
                                //                                collect_gather_node.Add(bom_node);
                                //                            }
                                //                        }
                                //                    }
                                //                    else
                                //                    {
                                //                        if (b)
                                //                        {
                                                            
                                //                        }
                                //                        bom_node_queue.Enqueue(bom_node);
                                //                        if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                //                        {
                                //                            if (check_condition.CheckCondition(bom_node))
                                //                            {
                                //                                collect_gather_node.Add(bom_node);
                                //                            }
                                //                        }
                                //                    }

                                //                }
                                //            }
                                //        }
                                //    }

                                //}
                                //else
                                //{
                                    if (queue_node.Children != null && queue_node.Children.Count > 0)
                                    {
                                        int offset = 1;
                                        //if (node.IsRoot && node.Part != null)
                                        //{
                                        //    offset = 0;
                                        //    search_depth -= 1;
                                        //}
                                        for (int i = 0; i < queue_node.Children.Count; i++)
                                        {
                                            bom_node = queue_node.Children.ElementAt(i);
                                            if (bom_node.Part.BOMNodeType.Trim().Equals(trave_path[bom_node.Level - offset].Trim()) && bom_node.Level <= search_depth)
                                            {
                                                if (string.IsNullOrEmpty(check_type))
                                                {
                                                    bom_node_queue.Enqueue(bom_node);
                                                    if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                    {
                                                        if (check_condition.CheckCondition(bom_node))
                                                        {
                                                            collect_gather_node.Add(bom_node);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    int check_level = toDepth(trave_path, check_type);
                                                    if (bom_node.Level == check_level && bom_node.Part.BOMNodeType.Trim().Equals(check_type))
                                                    {
                                                        if (check_condition.CheckCondition(bom_node))
                                                        {
                                                            bom_node_queue.Enqueue(bom_node);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bom_node_queue.Enqueue(bom_node);

                                                    }
                                                    if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                    {
                                                        if (bom_node.Level == 1)
                                                        {
                                                            if(bom_node.Part.BOMNodeType.Trim().Equals(check_type))
                                                            {
                                                                if (check_condition.CheckCondition(bom_node))
                                                                {
                                                                    collect_gather_node.Add(bom_node);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            collect_gather_node.Add(bom_node);
                                                        }
                                                    }
                                                }
                                            }
                                            //else
                                            //{
                                            //    if (contains(collect_gather_node,queue_node))   //不满足路径要求的，当阶删除。
                                            //    {
                                            //        remove(collect_gather_node,queue_node);
                                            //    }
                                            //    else
                                            //    {
                                            //        if (queue_node.Level > 0 && queue_node.Parent != null)  //不满足路径要求的，父阶删除。
                                            //        {
                                            //            IBOMNode parent_node = queue_node.Parent;
                                            //            while(parent_node != null)
                                            //            {
                                            //                if (contains(collect_gather_node,parent_node))
                                            //                {
                                            //                    remove( collect_gather_node,parent_node);
                                            //                }
                                            //                parent_node = parent_node.Parent;
                                            //            }
                                            //        }
                                                    
                                            //    }
                                            //}
                                        }
                                    }
                                    else
                                    {
                                        if (queue_node.Level == search_depth)
                                        {
                                            int check_level = toDepth(trave_path, check_type);
                                            if (queue_node.Part != null && queue_node.Level == check_level && queue_node.Part.BOMNodeType.Trim().Equals(check_type))
                                            {
                                                if (check_condition.CheckCondition(queue_node))
                                                {
                                                    if (queue_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                    {
                                                        if (!collect_gather_node.Contains(queue_node))
                                                        {
                                                            collect_gather_node.Add(queue_node);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (queue_node.Part != null && queue_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                {
                                                    if (!collect_gather_node.Contains(queue_node))
                                                    {
                                                        collect_gather_node.Add(queue_node);
                                                    }
                                                }
                                            }
                                        }
                                        if (queue_node.Level < search_depth)
                                        {
                                            IBOMNode parent_node = queue_node;
                                            while(parent_node != null)
                                            {
                                                if (contains(collect_gather_node, parent_node))
                                                {
                                                    remove(collect_gather_node, parent_node);
                                                }
                                                parent_node = parent_node.Parent;
                                            }
                                        }
                                    }
                                //}
 
                                //remove the first node from queue
                                //queue_node = 
                                //bom_node_queue.Dequeue();

                                //if (queue_node.Part != null && queue_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                //{
                                //    if (check_condition.CheckCondition(queue_node))
                                //    {
                                //        collect_gather_node.Add(queue_node);
                                //    }
                                //}
                            }

                            //if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                            //{
                            //List<IBOMNode> collect_gather_node_temp = new List<IBOMNode>();
                            //foreach (IBOMNode collect_node in collect_gather_node)
                            //{
                            //    if (!collect_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()) || !check_condition.CheckCondition(collect_node))
                            //    {
                            //        //collect_gather_node.Add(bom_node);
                            //        collect_gather_node_temp.Add(collect_node);
                            //    }
                            //}
                            //foreach (IBOMNode bom_node_temp in collect_gather_node_temp)
                            //{
                            //    collect_gather_node.Remove(bom_node_temp);
                            //}
                            //}


                            for (int j = 0; j < collect_gather_node.Count(); j++)
                            {
                                bom_node = collect_gather_node.ElementAt(j);
                                //Part part = new Part(bom_node.Part.PN,bom_node.Part.BOMNodeType,bom_node.Part.Type,bom_node.Part.CustPn,bom_node.Part.Descr,
                                //    bom_node.Part.Remark,bom_node.Part.AutoDL,bom_node.Part.Editor,bom_node.Part.Cdt,bom_node.Part.Udt,bom_node.Part.Descr2);
                                //foreach (PartInfo info in bom_node.Part.Attributes)
                                //{
                                //    part.AddAttribute(info);
                                //}

                                bom_node_queue.Enqueue(bom_node);
                                BOMNode tmp = (BOMNode)bom_node;
                                int qty = 0;
                                if (bom_node.Part.BOMNodeType.Trim().Equals(search_node_type))
                                {
                                    qty = bom_node.Qty;
                                }
                                do
                                {
                                    if (tmp.Parent != null)
                                    {
                                        int search_node_level = toDepth(trave_path, search_node_type);

                                        tmp = (BOMNode)tmp.Parent;
                                        if (tmp.Part.BOMNodeType.Trim().Equals(search_node_type))
                                        {
                                            qty = tmp.Qty;
                                        }
                                        else
                                        {
                                            if (tmp.Level < search_node_level )
                                            {
                                                qty *= tmp.Qty;
                                            }
                                        }
                                        
                                    }
                                } while (tmp.Level != 1);

                                //if (bom_node.Children.Count > 0)
                                //{
                                //    qty *= ((BOMNode)bom_node.Children.ElementAt(0)).Qty;
                                //}

                                //while (bom_node_queue.Count > 0)
                                //{
                                //    queue_node = bom_node_queue.Peek();
                                //    for (int i = 0; i < queue_node.Children.Count; i++)
                                //    {
                                //        bom_node = queue_node.Children.ElementAt(i);
                                //        if (((BOMNode)node).Part == null)
                                //        {
                                //            if (bom_node.Part.BOMNodeType.Trim().Equals(trave_path[bom_node.Level - 1].Trim()))
                                //            {
                                //                bom_node_queue.Enqueue(bom_node);
                                //            }
                                //        }
                                //        else
                                //        {
                                //            if (bom_node.Part.BOMNodeType.Trim().Equals(trave_path[bom_node.Level].Trim()))
                                //            {
                                //                bom_node_queue.Enqueue(bom_node);
                                //            }
                                //        }
                                //    }
                                //    queue_node = bom_node_queue.Dequeue();
                                //    if (queue_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                //    {
                                //        if (check_condition.CheckCondition(queue_node))
                                //        {
                                //            foreach (PartInfo attr in queue_node.Part.Attributes)
                                //                part.AddAttribute(attr);
                                //        }
                                //    }
                                ////}
                                List<IPart> parts = new List<IPart>();
                                parts.Add(bom_node.Part);
                                QtyParts bom_item = new QtyParts(qty, parts);
                                ret.Add(bom_item);
                            }
                        }
                    }
                    else
                    {
                        string[] param = {
                                    "IMES.CheckItemModule.Utility.BreadthFirstTraversal",
                                    "search_node"
                                 };
                        throw new FisException("CHK156", param);
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }
            if (collect_gather_node.Count > 0)
                return ret;
            else
                return null;
        }

        /// <summary>
        /// BOMNode树的遍历，得到所需的BOMNode列表。
        /// </summary>
        /// <param name="node">遍历的开始节点</param>
        /// <param name="search_path">遍历的指定路径。以BOMNodeType为路径节点，用“->”将它们彼此隔开。</param>
        /// <param name="search_node_type">以给定的BOMNodeType收集BOMNode。</param>
        /// <param name="check_condition"></param>
        /// <param name="check_type"></param>
        /// <returns></returns>
        public IList<QtyParts> BreadthFirstTraversalForHD(IBOMNode node, string search_path, string search_node_type, ITreeTraversal check_condition, string check_type)
        {
            List<IBOMNode> collect_gather_node = new List<IBOMNode>();
            List<QtyParts> ret = new List<QtyParts>();
            Queue<IBOMNode> bom_node_queue = new Queue<IBOMNode>(30);
            try
            {
                if (string.IsNullOrEmpty(search_path))
                {
                    throw new FisException("CHK169", new string[] { "IMES.CheckItemModule.Utility.TreeTraversal.BreadthFirstTraversal" });
                }
                else
                {
                    if (!string.IsNullOrEmpty(search_node_type.Trim()) && search_path.Contains(search_node_type.Trim()))
                    {
                        IBOMNode bom_node = node;
                        string[] trave_path = parse_paths(search_path);
                        //if (bom_node.Part.BOMNodeType.Trim().Equals(trave_path[bom_node.Level].Trim()))//由于Root.part为空，所以暂时去掉该句。
                        if (bom_node != null)
                        {
                            bom_node_queue.Enqueue(node);
                            IBOMNode queue_node;
                            //int obtain_depth = toDepth(trave_path, search_node_type);
                            int search_depth = trave_path.Length;

                            while (bom_node_queue.Count > 0)
                            {
                                queue_node = bom_node_queue.Dequeue();
 
                                if (queue_node.Children != null && queue_node.Children.Count > 0)
                                {
                                    int offset = 1;

                                    for (int i = 0; i < queue_node.Children.Count; i++)
                                    {
                                        bom_node = queue_node.Children.ElementAt(i);
                                        if (bom_node.Part.BOMNodeType.Trim().Equals(trave_path[bom_node.Level - offset].Trim()) && bom_node.Level <= search_depth)
                                        {
                                            if (string.IsNullOrEmpty(check_type))
                                            {
                                                bom_node_queue.Enqueue(bom_node);
                                                if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                {
                                                    if (check_condition.CheckCondition(bom_node))
                                                    {
                                                        collect_gather_node.Add(bom_node);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                int check_level = toDepth(trave_path, check_type);
                                                if (bom_node.Level == check_level && bom_node.Part.BOMNodeType.Trim().Equals(check_type))
                                                {
                                                    if (check_condition.CheckCondition(bom_node))
                                                    {
                                                        bom_node_queue.Enqueue(bom_node);
                                                    }
                                                }
                                                else
                                                {
                                                    bom_node_queue.Enqueue(bom_node);

                                                }
                                                if (bom_node.Part != null && bom_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                {
                                                    if (bom_node.Level == 1)
                                                    {
                                                        if (bom_node.Part.BOMNodeType.Trim().Equals(check_type))
                                                        {
                                                            if (check_condition.CheckCondition(bom_node))
                                                            {
                                                                collect_gather_node.Add(bom_node);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        collect_gather_node.Add(bom_node);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (queue_node.Level == search_depth)
                                    {
                                        int check_level = toDepth(trave_path, check_type);
                                        if (queue_node.Part != null && queue_node.Level == check_level && queue_node.Part.BOMNodeType.Trim().Equals(check_type))
                                        {
                                            if (check_condition.CheckCondition(queue_node))
                                            {
                                                if (queue_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                                {
                                                    if (!collect_gather_node.Contains(queue_node))
                                                    {
                                                        collect_gather_node.Add(queue_node);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (queue_node.Part != null && queue_node.Part.BOMNodeType.Trim().Equals(search_node_type.Trim()))
                                            {
                                                if (!collect_gather_node.Contains(queue_node))
                                                {
                                                    collect_gather_node.Add(queue_node);
                                                }
                                            }
                                        }
                                    }
                                    if (queue_node.Level < search_depth)
                                    {
                                        IBOMNode parent_node = queue_node;
                                        while (parent_node != null)
                                        {
                                            if (contains(collect_gather_node, parent_node))
                                            {
                                                remove(collect_gather_node, parent_node);
                                            }
                                            parent_node = parent_node.Parent;
                                        }
                                    }
                                }
                            }
                            Hashtable identical_vendor_code = new Hashtable();
                            for (int j = 0; j < collect_gather_node.Count(); j++)
                            {
                                bom_node = collect_gather_node.ElementAt(j);
                                bom_node_queue.Enqueue(bom_node);
                                BOMNode tmp = (BOMNode)bom_node;
                                int qty = 0;
                                string parent_descr = "";
                                string current_descr = "";
                                string vendor_code = "";
                                if (bom_node.Part.BOMNodeType.Trim().Equals(search_node_type))
                                {
//                                    qty = bom_node.Qty;
                                    qty = 1;
                                    current_descr = bom_node.Part.Descr;
                                    IList<PartInfo> part_infos = bom_node.Part.Attributes;
                                    if (part_infos != null)
                                    {
                                        foreach (PartInfo part_info in part_infos)
                                        {
                                            if (part_info.InfoType.Equals("VendorCode"))
                                            {
                                                vendor_code = part_info.InfoValue;
                                                break;
                                            }
                                        }
                                    }
                                }
                                do
                                {
                                    if (tmp.Parent != null)
                                    {
                                        int search_node_level = toDepth(trave_path, search_node_type);

                                        tmp = (BOMNode)tmp.Parent;
                                        
                                        if (tmp.Part != null && tmp.Part.BOMNodeType.Trim().Equals(search_node_type))
                                        {
                                            qty = tmp.Qty;
                                            parent_descr = tmp.Part.Descr;
                                        }
                                        else
                                        {
                                            if (tmp.Level < search_node_level)
                                            {
                                                qty *= tmp.Qty;
                                                parent_descr = tmp.Part.Descr;
                                            }
                                        }

                                    }
                                } while (tmp.Level != 1);
//                                string identical_vendor_code_key = parent_descr + current_descr + vendor_code;
                                string identical_vendor_code_key = current_descr + vendor_code;
                                if (identical_vendor_code.ContainsKey(identical_vendor_code_key))
                                {
                                    ((QtyParts) identical_vendor_code[identical_vendor_code_key]).Qty += qty;
                                }
                                else
                                {
                                    List<IPart> parts = new List<IPart>();
                                    parts.Add(bom_node.Part);
                                    QtyParts qty_parts = new QtyParts(qty, parts);
                                    identical_vendor_code.Add(identical_vendor_code_key, qty_parts);
                                }
//                                List<IPart> parts = new List<IPart>();
//                                parts.Add(bom_node.Part);
//                                QtyParts bom_item = new QtyParts(qty, parts);
//                                ret.Add(bom_item);
                            }
                            if (identical_vendor_code.Count > 0)
                            {
                                foreach (DictionaryEntry de in identical_vendor_code)
                                {
                                    ret.Add((QtyParts)de.Value);
                                }
                            }
                        }
                    }
                    else
                    {
                        string[] param = {
                                    "IMES.CheckItemModule.Utility.BreadthFirstTraversal",
                                    "search_node"
                                 };
                        throw new FisException("CHK156", param);
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }
            if (collect_gather_node.Count > 0)
                return ret;
            else
                return null;
        }


        /// <summary>
        /// 遍历深度
        /// </summary>
        /// <param name="paths">遍历路径</param>
        /// <param name="node_type">节点类型</param>
        /// <returns>返回深度</returns>
        private int toDepth(string[] paths,string node_type)
        {
            int collect_gather_node = -1;
            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i].Trim().Equals(node_type.Trim()))
                {
                    collect_gather_node = i;
                    break;
                }
            }
            return collect_gather_node + 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        private string[] parse_paths(string paths)
        {
            string[] ret = null;
            string[] trave_path = paths.Split(new Char[2] { '-', '>' });
            IList<string> path_list = new List<string>();
            for (int i = 0; i < trave_path.Length; i++)
            {
                if (trave_path[i].Trim().Length > 0)
                {
                    path_list.Add(trave_path[i].Trim());
                }
            }
            ret = path_list.ToArray();
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="e_node"></param>
        /// <returns></returns>
        private Boolean contains(IList<IBOMNode> nodes,IBOMNode e_node)
        {
            if (nodes != null && e_node.Part != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    if (node.Part != null && node.Part.PN.Trim().Equals(e_node.Part.PN))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="e_node"></param>
        private void remove(IList<IBOMNode> nodes,IBOMNode e_node)
        {
            if (nodes != null && e_node.Part != null && nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    if (node.Part != null && node.Part.PN.Trim().Equals(e_node.Part.PN))
                    {
                        nodes.Remove(node);
                        break;
                    }
                }
            }
        }
    }
}
