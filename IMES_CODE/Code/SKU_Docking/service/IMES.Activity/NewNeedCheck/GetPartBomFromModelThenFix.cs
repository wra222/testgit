// INVENTEC corporation (c)2011 all rights reserved. 
// Description: check Part Bom From Model and then fix
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-20   liuqingbiao                  create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
//using IMES.DataModel.KittingLocPLMappingInfo;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.PAK;
//using IMES.Infrastructure.Repository.PAK.COARepository;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data;

namespace IMES.Activity
{

    /// <summary>
    /// check Part Bom From Model and then fix
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC FA Kitting Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         根据Model得到此unit的part BOM和SELECT [Code],
    ///         [PartNo]  FROM [FA].[dbo].[WipBuffer] where [Code] = @family 的交集
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAKxxx
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class GetPartBomFromModelThenFix : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetPartBomFromModelThenFix()
        {
            InitializeComponent();
        }

        /// <summary>
        /// from _datatable -> _datatable_new
        /// </summary>
        /// <param name="_datatable"></param>
        /// <param name="_ret_list"></param>
        /// <returns></returns>
        public DataTable _datatable_to_get_new(DataTable _datatable, IList<string> _ret_list)
        {
            string partNo = "";
            string lightno = "";
            string sub = "";
            string sub_3 = "";
            DataTable _datatable_new = new DataTable();
            _datatable_new.Columns.Add("PartNo");
            _datatable_new.Columns.Add("LightNo");
            _datatable_new.Columns.Add("Sub");
            foreach (DataRow row in _datatable.Rows)
            {
                partNo = row["PartNo"].ToString().Trim();
                lightno = row["LightNo"].ToString().Trim();
                sub = row["Sub"].ToString().Trim(); 
                if (sub != "")
                    sub_3 = sub.Substring(0, 3);
                for (int i = 0; i < _ret_list.Count; i++)
                {
                    if (_ret_list[i] == partNo)
                    {
                        //_PartNo_list.Add(partNo);
                        //_LightNo_list.Add(lightno);
                        //_PartNo_LightNo_Qty_List.Add(_qty_ret_list[i]);

                        DataRow dr = _datatable_new.NewRow();
                        dr["PartNo"] = partNo;
                        dr["LightNo"] = lightno;
                        dr["Sub"] = sub;
                        _datatable_new.Rows.Add(dr);
                        break;
                    }
                }
            }
            // based on _datatable_new, delete correspoding rows.
            //_datatable.Rows[5].Delete();
            IList<string> _sub_3_list_ = new List<string>();
            IList<int> _sub_3_count_list_ = new List<int>();
            foreach (DataRow row in _datatable_new.Rows)
            {
                partNo = row["PartNo"].ToString().Trim();
                lightno = row["LightNo"].ToString().Trim();
                sub = row["Sub"].ToString().Trim();
                if (sub != "")
                    sub_3 = sub.Substring(0, 3);
                if (sub_3 != "")
                {
                    int exist_now = 0;
                    for (int i = 0; i < _sub_3_list_.Count; i++)
                    {
                        if (_sub_3_list_[i] == sub_3)
                        {
                            exist_now = 1; break;
                        }
                    }
                    if (exist_now == 0)
                    {
                        _sub_3_list_.Add(sub_3);
                    }
                }
            }
            // via upper lines we got a list _sub_3_list_:
            //   e.g. AAA BBB CCC ......

            // 得到_sub_3_list_相应的 sub_3 的数量列表，咨询过 UC, 
            // 说是 sub 项不应该有相同的，如果相同意味着维护数据可能有问题
            // 这就为我们下一步 删除 公用料 提供了依据 -- 就是说我们下一步 
            // 的删除是可行的。
            for (int i = 0; i < _sub_3_list_.Count; i++)
            {
                int count = 0;
                foreach (DataRow row in _datatable_new.Rows)
                {
                    sub = row["Sub"].ToString().Trim();
                    if (sub != "")
                        sub_3 = sub.Substring(0, 3);
                    if (sub_3 != "")
                    {
                        if (sub_3 == _sub_3_list_[i])
                        {
                            count++;
                        }
                    }
                }
                _sub_3_count_list_.Add(count);
            }

            // 现在得到了 _sub_3_list_, 和 _sub_3_count_list_
            // 我们要对 _sub_3_list_ 中，对应 _sub_3_count_list_ 的数量
            // 大于 1 的项 作 处理 -- 删除_datatable_new中的 Sub 最大的那一个
            for (int i = 0; i < _sub_3_list_.Count; i++)
            {
                if (_sub_3_count_list_[i] > 1)
                {
                    int index_ = 0;
                    int index_need_delete = -1;
                    string sub_tmp = "";
                    foreach (DataRow row in _datatable_new.Rows)
                    {
                        sub = row["Sub"].ToString().Trim();
                        if (sub != "")
                            sub_3 = sub.Substring(0, 3);
                        if (sub_3 != "")
                        {
                            if (sub.CompareTo(sub_tmp) >= 0) // = 的目的 是删除相等的最后一个
                            {
                                sub_tmp = sub; // find a larger one.
                                index_need_delete = index_;
                            }
                        }

                        index_++;
                    }

                    if (index_need_delete >= 0)
                    {
                        _datatable_new.Rows[index_need_delete].Delete();
                    }
                }
            }

            return _datatable_new;
        }

        /// <summary>
        /// check Part Box From Model and then fix
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            /*
             * Model下一阶Type	Data
             * ======================
             * 
             * BM	根据Model展2阶，得到第二阶为KP的PartNo和Qty，且第一阶是BM [ BM->KP]，即KP， 
             * 	    [还需要通过ModelBom反查其上阶为P1的PartNo（要求P1的PartNo的第七位为’-’） PartNo长度为10]
             * KP	根据Model展1阶，得到第一阶是KP的partNo和Qty，[KP],即KP  
             * 	    [还需要通过ModelBom反查其上阶为P1的PartNo（要求P1的PartNo的第七位为’-’） PartNo长度为10]
             * BM	根据Model展3阶，得到第二阶为P1的PartNo和第三阶为KP的Qty，
             * 	    且第一阶是BM 并且第三阶为KP[ BM->P1->KP]，即P1
             * P1	根据Model展2阶，得到第一阶是P1的partNo和第二阶为KP的Qty,且其下阶为KP，[ P1->KP]，即P1，
             * VK	根据Model展3阶，得到的第二阶为P1的PartNo，其中QTY取第一阶是VK的Qty和第二阶为P1的QTY 
             * 	    两者相比Qty大的那个,且第一阶是VK的，并且下下阶为KP，[ VK->P1->KP]，即P1
             * C4	根据Model展3阶，得到第二阶为P1的PartNo和Qty,且第一阶是C4，第三阶是KP，[C4->P1->KP],即P1
             * MB（VGA）	根据Model展1阶，得到第一阶是MB的part和Qty， [ MB]，并且第一阶的PartInfo存
             * 	    在InfoTyp='VGA' and InfoValue='SV'
             * PL	根据Model展2阶，得到第一阶是PL的part 和Qty， [ PL]，即PL，第一阶的Descr描述为
             * 	    Descr like 'TPM%'    or Descr like 'JGS%' or  Descr =( 'Touch screen'）
             * 	    or第一阶的PartInfo.InfoValue like '%Inverter%'
             * Common Parts	Part数据表中Part.PartNo(Part.BomNodeype='P1'  and  Part.Descr like 'CommonParts%')

             * 
             * Upper 3 kinds: *, ok, #
             *                *  -- call back: IList<IBOMNode> GetParentBomNodeByPnListAndBomNodeType(..., "P1")
             *                ok -- 
             *                #  -- call interface. IList<IPart> GetP1CommonParts(); 
             *            then combine the three kinds of ...
             * 
             * 注意Qty需要相乘,  要求P1的PartNo的第七位为’-’   ------- 此两句与我无关
             * 
             * IPartRepository  
             *      IList<IPart> GetP1CommonParts(); 
             * IBOMRepository           
             *     <summary>
             *     由给定的pnList得到BomNodeType=P1的父BOM节点
             *     </summary>
             *     <returns></returns>
             *     IList<IBOMNode> GetParentBomNodeByPnListAndBomNodeType(IList<string> pnList, string bomNodeType );
			 */

            IList<string> _sendList = new List<string>();
            IList<int> _qty_sendlist = new List<int>();
            IList<IBOMNode> _backList = new List<IBOMNode>();
            // 解 bom 分 3 种 
            // 1 种 
            IList<string> _okList_1 = new List<string>();
            IList<int> _qty_okList_1 = new List<int>();
            // 2 种 
            IList<string> _okList_2 = new List<string>();
            IList<int> _qty_okList_2 = new List<int>();
            // 3 种 
            IList<IPart> _okList_ret_3 = new List<IPart>();
            IList<string> _okList_3 = new List<string>();
            IList<int> _qty_okList_3 = new List<int>();

            IList<string> _ret_list = new List<string>();
            IList<int> _qty_ret_list = new List<int>();
            string[] _para;
            IList<string> _PartNo_list = new List<string>();
            IList<string> _LightNo_list = new List<string>();
            IList<int> _PartNo_LightNo_Qty_List = new List<int>();

            //string[] FirstLevelBomNodeType = new string[]{"KP","P1","P2","BM","VK","MB"};
            string[] FirstLevelBomNodeType = new string[] { "KP", "P1", "PL", "BM", "VK", "MB", "C4" };

            ArrayList passToInterface = new ArrayList();
            string _got_pdline = (string)CurrentSession.GetValue("inter_pdline");
            string _got_boxId = (string)CurrentSession.GetValue("_boxId");
            Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
            string prodId = currentProduct.ProId;
            string model = currentProduct.Model;
            string family = currentProduct.Family;

            //Convert.ToSingle(_lightNo);

            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(model);
            IList<IBOMNode> bomList = bom.FirstLevelNodes;
            //bom.GetNodesByNodeType(
            int c4_begin_count = 0;

            // --------------------------
            // 解 bom, 一级 type = FirstLevelBomNodeType [upper has defined...]
            // -------------------------

            //1st level nodes.
            foreach (IBOMNode node in bomList)
            {
                string _type = node.Part.BOMNodeType;
                if (FirstLevelBomNodeType.Contains(_type))
                {
                    switch (_type)
                    {
                        //KP	根据Model展1阶，得到第一阶是KP的partNo，[KP],即KP  
                        //   *  [还需要通过ModelBom反查其上阶为P1的PartNo（要求P1的PartNo的第七位为’-’） 
                        //      PartNo长度为10]
                        case "KP":
                            _qty_sendlist.Add(node.Qty);
                            _sendList.Add(node.Part.PN);
                            break;
                        // BM	根据Model展2阶，得到第二阶为KP的PartNo和Qty，且第一阶是BM [ BM->KP]，即KP， 
                        // 	 *  [还需要通过ModelBom反查其上阶为P1的PartNo（要求P1的PartNo的第七位为’-’） PartNo长度为10]
                        // BM	根据Model展3阶，得到第二阶为P1的PartNo和第三阶为KP的Qty，
                        // 	ok  且第一阶是BM 并且第三阶为KP[ BM->P1->KP]，即P1
                        // VK	根据Model展3阶，得到的第二阶为P1的PartNo，其中QTY取第一阶是VK的Qty和第二阶为P1的QTY 
                        // 	ok  两者相比Qty大的那个,且第一阶是VK的，并且下下阶为KP，[ VK->P1->KP]，即P1
                        // C4	根据Model展3阶，得到第二阶为P1的PartNo和Qty,且第一阶是C4，第三阶是KP，[C4->P1->KP],即P1
                        //  ok
                        case "C4":
                        case "VK":
                        case "BM":
                            {
                                if (_type == "C4")
                                {
                                    c4_begin_count++;
                                }
                                IList<IBOMNode> _2ndBomList = node.Children;
                                foreach (IBOMNode _2nd_node in _2ndBomList)
                                {
                                    bool _2nd_node_is_p1_and_3rd_has_KP_node = false;
                                    int _2nd_node_is_p1_and_3rd_has_KP_node_3_qty = 0;
                                    if (_2nd_node.Part.BOMNodeType == "KP")
                                    {
                                        if (node.Part.BOMNodeType == "BM")
                                        {
                                            _qty_sendlist.Add(_2nd_node.Qty); //
                                            _sendList.Add(_2nd_node.Part.PN);
                                            continue;
                                        }
                                    }
                                    else if (_2nd_node.Part.BOMNodeType == "P1")
                                    {
                                        IList<IBOMNode> _3rdBomList = _2nd_node.Children;
                                        foreach (IBOMNode _3rd_node in _3rdBomList)
                                        {
                                            if (_3rd_node.Part.BOMNodeType == "KP")
                                            {
                                                _2nd_node_is_p1_and_3rd_has_KP_node = true;
                                                _2nd_node_is_p1_and_3rd_has_KP_node_3_qty = _3rd_node.Qty;
                                                break;
                                            }
                                        }
                                    }

                                    if (_2nd_node_is_p1_and_3rd_has_KP_node)
                                    {
                                        // to three case, set Qty list.
                                        if (node.Part.BOMNodeType == "BM")
                                        {
                                            _qty_okList_2.Add(_2nd_node_is_p1_and_3rd_has_KP_node_3_qty);
                                        }
                                        else if (node.Part.BOMNodeType == "VK")
                                        {
                                            // get lagger of level1's qty and level2's qty
                                            if (node.Qty >= _2nd_node.Qty)
                                            {
                                                _qty_okList_2.Add(node.Qty);
                                            }
                                            else
                                            {
                                                _qty_okList_2.Add(_2nd_node.Qty);
                                            }
                                        }
                                        else // C4
                                        {
                                            _qty_okList_2.Add(_2nd_node.Qty);
                                        }
                                        // set PN to be level2's PN
                                        _okList_2.Add(_2nd_node.Part.PN);
                                    }
                                }
                            }
                            break;
                        // P1 根据Model展2阶，得到第一阶是P1的partNo和第二阶为KP的Qty,且其下阶为KP，[ P1->KP]，即P1，
                        //  ok
                        case "P1":
                            {
                                bool _2nd_node_is_KP = false;
                                int _2nd_node_is_KP__qty = 0;
                                IList<IBOMNode> _2ndBomList = node.Children;
                                foreach (IBOMNode _2nd_node in _2ndBomList)
                                {
                                    if (_2nd_node.Part.BOMNodeType == "KP")
                                    {
                                        _2nd_node_is_KP = true;
                                        _2nd_node_is_KP__qty = _2nd_node.Qty;
                                        break;
                                    }
                                }

                                if (_2nd_node_is_KP)
                                {
                                    _qty_okList_2.Add(_2nd_node_is_KP__qty);
                                    _okList_2.Add(node.Part.PN);
                                }
                            }
                            break;
                        //  MB（VGA）	根据Model展1阶，得到第一阶是MB的part和Qty， [ MB]，并且第一阶的PartInfo存
                        //  	ok  在InfoTyp='VGA' and InfoValue='SV'
                        case "MB":
                            {
                                foreach (PartInfo info in node.Part.Attributes)
                                {
                                    if ((info.InfoType == "VGA") && (info.InfoValue == "SV"))
                                    {
                                        _qty_okList_2.Add(node.Qty);
                                        _okList_2.Add(node.Part.PN);
                                        break;
                                    }
                                }
                            }
                            break;
                        // PL	根据Model展2阶，得到第一阶是PL的part 和Qty， [ PL]，即PL，第一阶的Descr描述为
                        // 	ok  Descr like 'TPM%'    or Descr like 'JGS%' or  Descr =( 'Touch screen'）
                        // 	    or第一阶的PartInfo.InfoValue like '%Inverter%'
                        case "PL":
                            {
                                bool _is_right_node = false;
                                if ((node.Part.Descr.StartsWith("TPM")) ||
                                    (node.Part.Descr.StartsWith("JGS")) ||
                                    (node.Part.Descr == "Touch screen"))
                                {
                                    _is_right_node = true;
                                }
                                else
                                {
                                    //check if PartInfo.InfoValue like '%Inverter%'
                                    foreach (PartInfo info in node.Part.Attributes)
                                    {
                                        //if (info.InfoType.Contains("Inverter"))
                                        if (info.InfoValue.Contains("Inverter"))
                                        {
                                            _is_right_node = true;
                                            break;
                                        }
                                    }
                                }

                                if (_is_right_node)
                                {
                                    _qty_okList_2.Add(node.Qty);
                                    _okList_2.Add(node.Part.PN);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            // translate list1 into string.
            if (_sendList.Count > 0)
            {
                /*
                 *  PROBLEM!! PERHAPS MAY LEAD A BUG!
                 *  2 in node -> 1 node case.
                 *  应避免多对一的情况
                 */
                // 反 查 
                _backList = bomRep.GetParentBomNodeByPnListAndBomNodeType(_sendList, "P1");

                foreach (IBOMNode node in _backList)
                {
                    int i = 0;
                    int _index = -1;

                    // 改 bug.
                    if ((node.Part.PN.Length != 10) || (node.Part.PN.Substring(6, 1) != "-"))
                    {
                        continue;
                    }

                    IBOMNode child = node.Children[0];
                    
                    for (i = 0; i < _sendList.Count; i++)
                    {
                        if (child.Part.PN == _sendList[i])
                        {
                            _index = i;
                            break;
                        }
                    }

                    if (_index >= 0)
                    {
                        _qty_okList_1.Add(_qty_sendlist[i]);//node.Qty); // here this sentence must check!!!
                    }
                    else
                    {
                        // here Create an Exception!
                        // have not find the parent to whose partno = %s corresponding.
                        FisException ex = new FisException("CHK239", new string[] { child.Part.PN }); //"have not find the parent to whose partno = %s corresponding!"
                        throw ex;
                    }

                    _okList_1.Add(node.Part.PN);
                }
            }

            // call interface to get list3.
            // --- UC -----------
            //   Common Parts	Part数据表中Part.PartNo(Part.BomNodeype='P1'  and  Part.Descr like 'CommonParts%')
            // ---- UC end ------
            _okList_ret_3 = partRep.GetPartsByBomNodeTypeAndLikeDescr("P1", "CommonParts"); //GetP1CommonParts();
            if (_okList_ret_3.Count > 0)
            {
                foreach (IPart part in _okList_ret_3)
                {
                    /*
                     * 
                     *  QTY PROBLEM!! BUG!
                     *  There is no corresponding Qty we need! 
                     * 
                     */
                    
                        _qty_okList_3.Add(1); // ;)
                        _okList_3.Add(part.PN);
                    
                }
            }

            // 解完 bom， 3种加起来
            // combine 3 list.
            //foreach (string str in _okList_1)
            for (int i = 0; i < _okList_1.Count; i++)
            {
                _ret_list.Add(_okList_1[i]);
                _qty_ret_list.Add(_qty_okList_1[i]);
            }

            for (int i = 0; i < _okList_2.Count; i++)
            {
                _ret_list.Add(_okList_2[i]);
                _qty_ret_list.Add(_qty_okList_2[i]);
            }

            for (int i = 0; i < _okList_3.Count; i++)
            {
                _ret_list.Add(_okList_3[i]);
                _qty_ret_list.Add(_qty_okList_3[i]);

            }

            //IList<WipBuffer> wipbufferList = productRep.GetWipBufferListByPnoListAndCode(string[] pnoList, string code);
            _para = _ret_list.ToArray();
            // UC modified, and liudong write another interface:
            //      DataTable GetPartNoAndLightNoFromWipBuffer(string family);
            //IList<WipBuffer> wipbufferList = productRep.GetWipBufferListByPnoListAndCode(_para, family);

            // chuli shu ju kuai......
            // --- UC ---
            // 7.	根据Model得到此unit的part BOM和下列两种方法取得的库位信息的合集的交集
            // --- UC ---
            {
                string partNo = "";
                string lightno = "";
                //string sub = "";
                //string sub_3 = "";
                WipBuffer __condition__wipbuffer__ = new WipBuffer();
                __condition__wipbuffer__.Code = family;
                __condition__wipbuffer__.Line = _got_pdline.Substring(0,1);
                //DataTable _datatable = new DataTable();
                if (productRep.CheckExistLightNoFromKitLocAndWipBuffer(family, _got_pdline.Substring(0, 1)) == true)
                {
                    // ----------------------------------
                    if (productRep.CheckExistWipBuffer(__condition__wipbuffer__) == true)
                    {
                        DataTable _datatable = productRep.GetPartNoAndLightNoFromWipBufferWithLineForDoubleLine(family, _got_pdline.Substring(0, 1));
                        //DataTable _datatable_new = _datatable_to_get_new(_datatable, _ret_list);
                        DataTable _datatable_new = _datatable;

                        // transfer _datatable_new -> partNo, lightNo --> 3 lists...
                        foreach (DataRow row in _datatable_new.Rows)
                        {
                            partNo = row["PartNo"].ToString().Trim();
                            lightno = row["LightNo"].ToString().Trim();
                            for (int i = 0; i < _ret_list.Count; i++)
                            {
                                if (_ret_list[i] == partNo)
                                {
                                    _PartNo_list.Add(partNo);
                                    _LightNo_list.Add(lightno);
                                    _PartNo_LightNo_Qty_List.Add(_qty_ret_list[i]);
                                    break;
                                }
                            }
                        }
                    }
                    else // code = family and line = PdLine.SbString(0,1) -- can not find, so only use code = family.
                    {
                        //DataTable _datatable = productRep.GetPartNoAndLightNoFromWipBuffer(family);
                        DataTable _datatable = productRep.GetPartNoAndLightNoFromWipBufferForDoubleLine(family, _got_pdline.Substring(0, 1));
                        //DataTable _datatable_new = _datatable_to_get_new(_datatable, _ret_list);
                        DataTable _datatable_new = _datatable;

                        foreach (DataRow row in _datatable_new.Rows)
                        {
                            partNo = row["PartNo"].ToString().Trim();
                            lightno = row["LightNo"].ToString().Trim();
                            for (int i = 0; i < _ret_list.Count; i++)
                            {
                                if (_ret_list[i] == partNo)
                                {
                                    _PartNo_list.Add(partNo);
                                    _LightNo_list.Add(lightno);
                                    _PartNo_LightNo_Qty_List.Add(_qty_ret_list[i]);
                                    break;
                                }
                            }
                        }
                    }
                    // ------------------------------------
                }
                else // origionally.
                {
                    // origionally codes for :
                    // if condition1
                    //      1.1
                    // else
                    //      1.2
                    //
                    // union
                    //
                    // 1.2
                    if (productRep.CheckExistWipBuffer(__condition__wipbuffer__) == true)
                    {
                        DataTable _datatable = productRep.GetPartNoAndLightNoFromWipBufferWithLine(family, _got_pdline.Substring(0, 1));

                        //DataTable _datatable_new = _datatable_to_get_new(_datatable, _ret_list);
                        DataTable _datatable_new = _datatable;

                        foreach (DataRow row in _datatable_new.Rows)
                        {
                            partNo = row["PartNo"].ToString().Trim();
                            lightno = row["LightNo"].ToString().Trim();
                            for (int i = 0; i < _ret_list.Count; i++)
                            {
                                if (_ret_list[i] == partNo)
                                {
                                    _PartNo_list.Add(partNo);
                                    _LightNo_list.Add(lightno);
                                    _PartNo_LightNo_Qty_List.Add(_qty_ret_list[i]);
                                    break;
                                }
                            }
                        }
                    }
                    else // code = family and line = PdLine.SbString(0,1) -- can not find, so only use code = family.
                    {
                        //DataTable _datatable = productRep.GetPartNoAndLightNoFromWipBuffer(family);
                        DataTable _datatable = productRep.GetPartNoAndLightNoFromWipBuffer(family, _got_pdline.Substring(0, 1));

                        //DataTable _datatable_new = _datatable_to_get_new(_datatable, _ret_list);
                        DataTable _datatable_new = _datatable;

                        foreach (DataRow row in _datatable_new.Rows)
                        {
                            partNo = row["PartNo"].ToString().Trim();
                            lightno = row["LightNo"].ToString().Trim();
                            for (int i = 0; i < _ret_list.Count; i++)
                            {
                                if (_ret_list[i] == partNo)
                                {
                                    _PartNo_list.Add(partNo);
                                    _LightNo_list.Add(lightno);
                                    _PartNo_LightNo_Qty_List.Add(_qty_ret_list[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
            } // chu li shu ju kuai jie shu.
            //HAS: //.Tp//.ID//.KittingType//.Max_Stock//.Remark//.Safety_Stock;//.Qty;//PartNo;//LightNo;
            // Important: PartNo, LightNo, Qty -- Qty is not right perhaps, so we do not use it!

            //CurrentSession.AddValue("_wipbufferList", wipbufferList);
            CurrentSession.AddValue("_ret_list", _ret_list);
            CurrentSession.AddValue("_qty_ret_list", _qty_ret_list);
            CurrentSession.AddValue("_PartNo_list", _PartNo_list);
            CurrentSession.AddValue("_LightNo_list", _LightNo_list);
            CurrentSession.AddValue("_PartNo_LightNo_Qty_List", _PartNo_LightNo_Qty_List);

            /*
             * The following is to write database, so must move into another activity. 
             */

            return base.DoExecute(executionContext);
        }

    }
}

