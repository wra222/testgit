// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 将bom中需要检料的part通过kitting middleware传至Kitting Light Server
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
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.PAK;
//using IMES.Infrastructure.Repository.PAK.COARepository;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data;

namespace IMES.Activity
{

    /// <summary>
    /// Send Bom Part To Kitting Server
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
    ///         数据已经组织好，只要从session中取出，利用接口
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK007
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
    public partial class SendBomPartToKittingServer : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SendBomPartToKittingServer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Send Bom Part To Kitting Server
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            ///////////////////////////////////////////////////////////////////////////////////////////////
            //modify ITC-1360-1810 bug
            //moidfy by itc-200052, 2012-5-14
            // 由于FA Kitting Input产线测试结果不太良好，现将其部分业务功能提取到SP，直接调用SP去实现：
            // 名称：op_FAKitting_MiddleWare.sql
            // 参数：
            //     @pid char(9) ,--ProductID
            //     @boxid char(4), --4码BOXID
            //     @editor char(30),
            //     @kitpdline char(10)—PdLine
            //     
            // 结果：(返回的结果集还要看当时最新的存储过程部署代码)
            // OK,Descr--执行成功
            // NG,'1',Descr--执行失败
            //SP包含内容：
            //7.	根据Model得到此unit的part BOM和下列两种方法取得的库位信息的合集的交集
            //8.	将bom中需要检料的part通过kitting middleware传至Kitting Line Server
         
            
            
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string _got_pdline = (string)CurrentSession.GetValue("inter_pdline");
            string _got_boxId = (string)CurrentSession.GetValue("_boxId");
            string _product_id = (string)CurrentSession.GetValue("_prodId");
           // string _editor = (string)CurrentSession.GetValue("Editor");


            DataTable dt = productRep.Callop_FAKitting_MiddleWare(_product_id, _got_boxId, this.Editor, _got_pdline);
            if (dt == null || dt.Rows.Count != 1)
            {
                //" 出错了---，系统错误"; 

                FisException ex = new FisException("PAK006", new string[] { });
                throw ex;


            }
            else
            {
                if (dt.Rows[0][0].ToString().Trim() != "OK")
                {
                    //"执行失败，失败原因：" + dt.Rows[0][2].ToString().Trim();
                    List<string> erpara = new List<string>();
                    erpara.Add(dt.Rows[0][2].ToString().Trim());
                    FisException ex;
                    ex = new FisException("PAK123", erpara);
                    throw ex;

                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////

            /*
			 * Now interface shortage, so here is empty. later will added!
			 */

            /*
             *   a.根据part对应的LightNo更新Kitting_Location_FA_X表中灯位对应的记录，其中不同线别对应不同的表
             *   ConfigedLEDStatus=1,RunningLEDStatus=0,LEDValues=Qty
             *   线别是根据operator选择的PdLine
             *   Kitting_Loc_PLMapping里定义了WipBuffer.LightNo和Kitting_Location_FA_X表里TagId对应关系
             *   b.将BoxId 写到Kitting_Location_FA_X表
             *   update Priority=1,3,5,7,9,11的纪录
             *   ConfigedLEDStatus=1,RunningLEDStatus=0,LEDValues=convert(int,convert(float(8),'+boxid#+'))
             *   注：根据kitting架的个数，priority的数字会有所变动，如5个架子的话，则priority=1，3，5，7，9的会有变化
             */

  /*          
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string _got_pdline = (string)CurrentSession.GetValue("inter_pdline");
            string _got_boxId = (string)CurrentSession.GetValue("_boxId");
            string _product_id = (string)CurrentSession.GetValue("_prodId");
            //IList<WipBuffer> wipbufferList = (IList<WipBuffer>)CurrentSession.GetValue("_wipbufferList");
            //IList<string> _ret_list = (IList<string>)CurrentSession.GetValue("_ret_list");
            //IList<int> _qty_ret_list = (IList<int>)CurrentSession.GetValue("_qty_ret_list");

            // added later. 2012.02.29
            IList<string> _PartNo_list = (IList<string>)CurrentSession.GetValue("_PartNo_list");
            IList<string> _LightNo_list = (IList<string>)CurrentSession.GetValue("_LightNo_list");
            IList<int> _PartNo_LightNo_Qty_List = (IList<int>)CurrentSession.GetValue("_PartNo_LightNo_Qty_List");
            int i = 0;
            // step a.
            for (i=0; i<_PartNo_list.Count; i++)
            {
                int searchTagIDSuccess = 0;
                KittingLocationFaXInfo _Value = new KittingLocationFaXInfo();
                _Value.tableNameEpilogue = _got_pdline[0].ToString();
                _Value.configedLEDStatus = true;
                _Value.editor = this.Editor;
                _Value.ledvalues = Convert.ToString(_PartNo_LightNo_Qty_List[i]);
                _Value.runningLEDStatus = false;

                KittingLocationFaXInfo _condition = new KittingLocationFaXInfo();
                _condition.tableNameEpilogue = _got_pdline[0].ToString();

                KittingLocPLMappingInfo info = new KittingLocPLMappingInfo();
                info.lightNo = (short)Convert.ToInt16(_LightNo_list[i]);
                info.pdLine = "FA-" + _got_pdline[0]; //pdline='FA-'+left(PDLINE#,1)
                IList<KittingLocPLMappingInfo> __list = palletRep.GetKittingLocPLMappingInfoList(info);
                foreach (KittingLocPLMappingInfo _node in __list)
                {
                    _condition.tagID = _node.tagID; searchTagIDSuccess = 1; break;
                }
                if (searchTagIDSuccess == 0)
                {
                    string strLightNo = Convert.ToString(info.lightNo);
                    FisException ex = new FisException("CHK240", new string[] { info.pdLine, strLightNo });
                    throw ex;
                }

                palletRep.UpdateKittingLocationFaXInfoDefered(CurrentSession.UnitOfWork, _Value, 
                    1, //int configedLEDStatus, 
                    0, //int runningLEDStatus, 
                    -1, 
                    _condition, 
                    new int[]{}
                    );

                //QB.L
                //KittingLogInfo log__info = new KittingLogInfo();
                //log__info.boxId = _got_boxId;
                //log__info.cdt = DateTime.Now;
                //log__info.configedLEDStatus = _Value.configedLEDStatus;
                //log__info.editor = this.Editor;
                //log__info.ledvalues = _Value.ledvalues;
                //log__info.line = "FA-" + _got_pdline[0];
                //log__info.priority = 0;
                //log__info.productID = _product_id;
                //log__info.runningLEDStatus =  _Value.runningLEDStatus;
                //log__info.table = _Value.tableNameEpilogue;
                //log__info.tagID = _condition.tagID;
                //log__info.time = 1;
                //productRep.InsertKittingLogInfoDefered(CurrentSession.UnitOfWork, log__info);
                //QB.L

                //D.L
                KittingLogInfo item = new KittingLogInfo();
                item.boxId = _got_boxId;
                item.line = "FA-" + _got_pdline[0];
                item.productID = _product_id;
                item.time = 1;
                productRep.InsertKittingLogInfosFromKittingLocationFaXDefered(CurrentSession.UnitOfWork, item, _condition, new int[] { });
                //D.L
            }

            // step b.
            // --Cut off loop--: for (i=0; i<_PartNo_list.Count; i++)
            if (_PartNo_list.Count > 0)
            {
                // --Cut off loop--: int searchTagIDSuccess = 0;
                // --Cut off loop--: string _got_tagId = "";

                KittingLocationFaXInfo setValue = new KittingLocationFaXInfo();
                KittingLocationFaXInfo condition = new KittingLocationFaXInfo();
                condition.tableNameEpilogue = _got_pdline[0].ToString();
                setValue.tableNameEpilogue = _got_pdline[0].ToString();
                setValue.configedLEDStatus = true;
                setValue.runningLEDStatus = false;
                setValue.ledvalues = _got_boxId; //"9999"; //convert(int,convert(float(8),'+boxid#+'))
                setValue.editor = this.Editor;

                // --Cut off loop--: KittingLocPLMappingInfo info = new KittingLocPLMappingInfo();
                // --Cut off loop--: info.lightNo = (short)Convert.ToInt16(_LightNo_list[i]);
                // --Cut off loop--: info.pdLine = "FA-" + _got_pdline[0]; //pdline='FA-'+left(PDLINE#,1)

                // --Cut off loop--: IList<KittingLocPLMappingInfo> __list = palletRep.GetKittingLocPLMappingInfoList(info);
                // --Cut off loop--: foreach (KittingLocPLMappingInfo _node in __list)
                // --Cut off loop--: {
                // --Cut off loop--:     // currently, although it is a list, I only return the tagId of the first node.
                // --Cut off loop--:     _got_tagId = _node.tagID; searchTagIDSuccess = 1; break;
                // --Cut off loop--: }

                // --Cut off loop--: if (searchTagIDSuccess == 0)
                // --Cut off loop--: {
                // --Cut off loop--:     string strLightNo = Convert.ToString(info.lightNo);
                // --Cut off loop--:     FisException ex = new FisException("CHK240", new string[] { info.pdLine, strLightNo });
                // --Cut off loop--:     throw ex;
                // --Cut off loop--: }

                int[] priority = new int[] { 1, 3, 5, 7, 9, 11 }; // modify {1,3,5,7,9} into {1,3,5,7,9,11}
                
                    //ConfigedLEDStatus=1,RunningLEDStatus=0,LEDValues=convert(int,convert(float(8),'+boxid#+'))
                    //condition.priority = priority[i];
                //--cut_cond_tag__set     condition.tagID = _got_tagId;
                    //palletRep.UpdateKittingLocationFaXInfo(setValue, condition);
                    palletRep.UpdateKittingLocationFaXInfoDefered(CurrentSession.UnitOfWork, setValue, 
                    1, //int configedLEDStatus, 
                    0, //int runningLEDStatus, 
                    -1, 
                    condition, 
                    priority);

                //QB.L
                //KittingLogInfo log__info = new KittingLogInfo();
                //log__info.boxId = _got_boxId;
                //log__info.cdt = DateTime.Now;
                //log__info.configedLEDStatus = setValue.configedLEDStatus;
                //log__info.editor = this.Editor;
                //log__info.ledvalues = setValue.ledvalues;
                //log__info.line = "FA-" + _got_pdline[0];
                //log__info.priority = 0;
                //log__info.productID = _product_id;
                //log__info.runningLEDStatus = setValue.runningLEDStatus;
                //log__info.table = setValue.tableNameEpilogue;
                //log__info.tagID = _got_tagId;
                //log__info.time = 2;
                //productRep.InsertKittingLogInfoDefered(CurrentSession.UnitOfWork, log__info);
                //QB.L
                
                //D.L
                KittingLogInfo item = new KittingLogInfo();
                item.boxId = _got_boxId;
                item.line = "FA-" + _got_pdline[0];
                item.productID = _product_id;
                item.time = 2;
                productRep.InsertKittingLogInfosFromKittingLocationFaXDefered(CurrentSession.UnitOfWork, item, condition, priority);
                //D.L
            }
            */
            return base.DoExecute(executionContext);
        } // end of protected internal override ActivityExecutionStatus DoExecute

    } // end of public partial class SendBomPartToKittingServer : BaseActivity
} // end of namespace IMES.Activity

/*
            //foreach (string _lightNo in _lightno_list)
            foreach (WipBuffer _wipbuffer_node in wipbufferList)
            {
                int searchTagIDSuccess = 0;
                KittingLocationFaXInfo _Value = new KittingLocationFaXInfo();
                //
                // Liu, Qing-Biao (劉慶彪 ITC) [13:28]:
                //   第一个字母 -- 需要和表对应上吧，而刘东要求我制定哪个表：
                //   _KittingLocPLMappingInfo
                //   {
                //      string tableNameEpilogue = "A";
                //      ....
                // 
                //   我需要对这个 string 进行设置，如何设置呢？
                // 
                // 
                // Liu, Cai-Bin (劉彩賓 ITC) [13:29]:
                //   tableNameEpilogue ＝ＰＤＬｉｎｅ第一个字母
                //
                _Value.tableNameEpilogue = _got_pdline[0].ToString();
                _Value.configedLEDStatus = true;
                _Value.editor = this.Editor;
                //_Value.gateWayIP; //_Value.group; //_Value.id; 

                for (int i = 0; i < _ret_list.Count; i++)
                {
                    if (_ret_list[i] == _wipbuffer_node.PartNo)
                    {
                        _Value.ledvalues = Convert.ToString(_qty_ret_list[i]); //"100";//Qty;
                        break;
                    }
                }
                //_Value.mgroup; //_Value.priority; //_Value.rackID; //_Value.runningDate; //_Value.runningLEDB_Lock;
                _Value.runningLEDStatus = false;
                //_Value.tagDescr; //_Value.tagTp; //_Value.udt; //_Value.tagID = _;

                KittingLocationFaXInfo _condition = new KittingLocationFaXInfo();
                KittingLocPLMappingInfo info = new KittingLocPLMappingInfo();
                info.lightNo = (short)Convert.ToSingle(_wipbuffer_node.LightNo);
                info.pdLine = "FA-" + _got_pdline[0]; //pdline='FA-'+left(PDLINE#,1)
                IList<KittingLocPLMappingInfo> __list = palletRep.GetKittingLocPLMappingInfoList(info);
                foreach (KittingLocPLMappingInfo _node in __list)
                {
                    // currently, although it is a list, I only return the tagId of the first node.
                    _condition.tagID = _node.tagID; searchTagIDSuccess = 1; break;
                }
                //_condition.tableNameEpilogue = "A"; // I asked liu.dong if it is not neccessary to set this. he asked yes.

                if (searchTagIDSuccess == 0)
                {
                    //"To pdLine:%1, LightNo:%2, tagID not found through GetKittingLocPLMappingInfoList!"
                    string strLightNo = Convert.ToString(info.lightNo);
                    FisException ex = new FisException("CHK240", new string[] { info.pdLine, strLightNo });
                    throw ex;
                }

                //palletRep.UpdateKittingLocationFaXInfo(_Value, _condition);
                //DEBUG 0509
                palletRep.UpdateKittingLocationFaXInfoDefered(CurrentSession.UnitOfWork, _Value, _condition);
            } // end of foreach (WipBuffer _wipbuffer_node in wipbufferList)
            // now step a. has finished now.

            // step b. begin.
            //foreach (string _lightNo in _lightno_list)
            foreach (WipBuffer _wipbuffer_node in wipbufferList)
            {
                int searchTagIDSuccess = 0;
                string _got_tagId = "";
                KittingLocationFaXInfo setValue = new KittingLocationFaXInfo();
                KittingLocationFaXInfo condition = new KittingLocationFaXInfo();
                setValue.configedLEDStatus = true;
                setValue.runningLEDStatus = false;
                setValue.ledvalues = _got_boxId; //"9999"; //convert(int,convert(float(8),'+boxid#+'))

                KittingLocPLMappingInfo info = new KittingLocPLMappingInfo();
                info.lightNo = (short)Convert.ToSingle(_wipbuffer_node.LightNo);
                info.pdLine = "FA-" + _got_pdline[0]; //pdline='FA-'+left(PDLINE#,1)
                IList<KittingLocPLMappingInfo> __list = palletRep.GetKittingLocPLMappingInfoList(info);
                foreach (KittingLocPLMappingInfo _node in __list)
                {
                    // currently, although it is a list, I only return the tagId of the first node.
                    _got_tagId = _node.tagID; searchTagIDSuccess = 1; break;
                }

                if (searchTagIDSuccess == 0)
                {
                    string strLightNo = Convert.ToString(info.lightNo);
                    FisException ex = new FisException("CHK240", new string[] { info.pdLine, strLightNo });
                    throw ex;
                }

                int[] priority = new int[] { 1, 3, 5, 7, 9, 11 }; // modify {1,3,5,7,9} into {1,3,5,7,9,11}
                for (int i=0; i<priority.Length; i++)
                {
                    //ConfigedLEDStatus=1,RunningLEDStatus=0,LEDValues=convert(int,convert(float(8),'+boxid#+'))
                    condition.priority = priority[i];
                    condition.tagID = _got_tagId;
                    //palletRep.UpdateKittingLocationFaXInfo(setValue, condition);
                    palletRep.UpdateKittingLocationFaXInfoDefered(CurrentSession.UnitOfWork, setValue, condition);
                }
            } // end of foreach (WipBuffer _wipbuffer_node in wipbufferList)
*/
