// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的DN,和PCS
//              判断当前DN是否还能再绑定数量为PCS的Product         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-02-20   Yuan XiaoWei                 create
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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 判断当前DN是否还能再绑定数量为PCS的Product 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于Combine PO in Carton
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.;
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK067
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PCS
    ///         Session.Delivery
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
    ///         Delivery
    /// </para> 
    /// </remarks>
    public partial class CheckDNCombineQty : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckDNCombineQty()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Delivery
        /// 判断该Delivery的数量是否大于该Delivery已经绑定的Product数量+PCS
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentPCS = (int)CurrentSession.GetValue(Session.SessionKeys.PCS);

            Delivery CurrentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            int combinedQty = currentProductRepository.GetCombinedQtyByDN(CurrentDelivery.DeliveryNo);


            if (CurrentDelivery.Qty < (currentPCS + combinedQty))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(CurrentDelivery.Qty.ToString());
                erpara.Add(combinedQty.ToString());
                erpara.Add(currentPCS.ToString());
                ex = new FisException("CHK067", erpara);

                throw ex;
            }
            else if (CurrentDelivery.Qty == (currentPCS + combinedQty))
            {
                CurrentSession.AddValue(Session.SessionKeys.isDNFull, true);
            }


            return base.DoExecute(executionContext);
        }


    }
}
