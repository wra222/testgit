// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   Kerwin                       create
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
using IMES.FisObject.FA.Product;

using IMES.FisObject.PAK.COA;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.Repository.PAK;
//using IMES.Infrastructure.Repository.PAK.COARepository;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 判断当前Product的DeliveryNo的InfoType= CustPo / IECSo对应的InfoValue属性是否全
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断当前Product的Product.DeliveryNo 在DeliveryInfo分别查找InfoType= CustPo / IECSo对应的InfoValue，
    ///           其中某一个得不到时，都需要报错:DN資料不全CustPo/IECSo,請檢查一下船務
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
    public partial class CheckPizzaLabel : BaseActivity
    {
        /// <summary>
        /// check pizza label
        /// </summary>
        public CheckPizzaLabel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// activity execution status "DoExecute"
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            // if success, set the value of following.
            //CurrentSession.AddValue("CheckPizzaLabelPass", true); //.GetValue(Session.SessionKeys.Product)

            // chech pizza label process.
            //
            // 
            //
            // 1. special check1. --> content label. 1）查询Product取得Delivery 2）查询DeliveryInfo 3）查询ProductLog
            //   1）如果SKU 绑定的Delivery (IMES_FA..Product.DeliveryNo) 的
            //   2）RegId = 'SCN' and ShipTp = 'CTO' 
            //     (RegId: IMES_PAK..DeliveryInfo.InfoType = 'RegId'；
            //        ShipTp: IMES_PAK..DeliveryInfo.InfoType = 'ShipTp')时，
            //   3）如果在IMES_FA..ProductLog 中不存在WC = '8D' 的记录时，需要报告错误：“未打印Content Label!”
            // 2. special chech2. --> check desiccant [gan zao ji]. 1）查询Product取得Delivery 2）查询DeliveryInfo
            //   1）如果机器是海运方式出货（SKU 绑定的Delivery (IMES_FA..Product.DeliveryNo) 的
            //   2）ShipWay = 'T002' (ShipWay: IMES_PAK..DeliveryInfo.InfoType = 'ShipWay')时，表示海运方式出货)，
            //      需要弹出Message Box 提示用户：“此机器为海运方式出货,请检查是否有装乾燥剂”
           
            //string product_sn = status.IECPN;
#if false            
            var product_id = CurrentSession.Key; //GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IProduct currentProduct = productRepository.Find(product_id); //GetProductByCustomSn(product_sn);
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            string currentDN = currentProduct.DeliveryNo;
            Delivery thisDelivery = deliveryRepository.Find(currentDN);
            if (thisDelivery == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentProduct.DeliveryNo);
                throw new FisException("CHK107", errpara);
            }
            CurrentSession.AddValue(Session.SessionKeys.Delivery, thisDelivery);
            IList<DeliveryInfo> this_list = thisDelivery.DeliveryInfoes;
            bool regidCondition = false;
            bool shiptpCondition = false;
            //bool Label_must_print_but_have_not_print = false;
            foreach (DeliveryInfo info in thisDelivery.DeliveryInfoes)
            {
                if ((!regidCondition) && info.InfoType == "RegId" && info.InfoValue == "SCN")
                {
                    regidCondition = true; if (shiptpCondition) break;
                }
                if ((!shiptpCondition) && info.InfoType == "ShipTp" && info.InfoValue == "CTO")
                {
                    shiptpCondition = true; if (regidCondition) break;
                }
            }

            if (shiptpCondition && regidCondition)
            {
                // neccessary print.
                // Here we check if the log reprent it has bee printed, if no, 
                //      give Error Message "未打印Content Label!"
                foreach (ProductLog _log in currentProduct.ProductLogs)
                {
                    if (_log.Station == "8D")
                    {
                        //Label_must_print_but_have_not_print = true;
                        FisException ex = new FisException("SFC098", new string[] { "未打印Content Label!" });
                        throw ex;
                    }
                }
            }


            foreach (DeliveryInfo info in thisDelivery.DeliveryInfoes)
            {
                if (info.InfoType == "ShipWay" && info.InfoValue == "T002")
                {
                    // it must be shiped. so message must be given to tell user to check if 
                    FisException ex = new FisException("SFC099", new string[] { "此机器为海运方式出货,请检查是否有装乾燥剂" });
                    throw ex;
                }
            }
#endif

            return base.DoExecute(executionContext);
        }

    }
}

