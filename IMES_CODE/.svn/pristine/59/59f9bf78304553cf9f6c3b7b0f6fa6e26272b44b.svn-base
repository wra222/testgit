// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的UpdateProductList中的ProductID列表，
//              更新列表中所有Product的DeliveryNo栏位          
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-03   Yuan XiaoWei                 create
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
    /// 用于更新一组Product的DeliveryID
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PAK Pallet Data Collection
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取NewScanedProductIDList，调用ProductRepository.BindDN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.NewScanedProductIDList
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
    ///         更新Product表的DeliveryNo 
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              IProduct
    /// </para> 
    /// </remarks>
    public partial class BindDN : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public BindDN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 执行将Delivery绑定到Product列表的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Delivery CurrentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);

            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> ProductIDList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            ProductRepository.BindDNDefered(CurrentSession.UnitOfWork, CurrentDelivery.DeliveryNo, ProductIDList);

            return base.DoExecute(executionContext);
        }
	}
}
