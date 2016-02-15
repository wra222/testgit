
// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 将Product对象的DeliveryID和CartonSN清空
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-11   Lucy Liu                 create
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
    /// 将Product对象的DeliveryID和CartonSN清空  
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FG Shipping Label(TRO) Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.;将Product的DeliveryID和CartonSN清空
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     
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
    public partial class UpdateProductDeliveryCartonSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateProductDeliveryCartonSN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Session.Product获取Product对象
        /// 将Product的DeliveryID和CartonSN清空      
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得Product对象
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            
            //将Product的DeliveryID和CartonSN清空  
            currentProduct.DeliveryNo = "";
            currentProduct.CartonSN = "";            
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ProductRepository.Update(currentProduct, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }

    }
}


