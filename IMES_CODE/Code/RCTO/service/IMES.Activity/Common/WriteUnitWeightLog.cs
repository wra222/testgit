// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 记录到UnitWeightLog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-07-12   itc202017                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;

namespace IMES.Activity
{
    /// <summary>
    /// 用于记录到UnitWeight Log
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      UnitWeight站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Product，调用Product的InsertUnitWeightLog方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    ///         更新ProductLog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    ///              UnitWeightLog
    /// </para> 
    /// </remarks>
    public partial class WriteUnitWeightLog : BaseActivity
    {
        ///<summary>
        /// 构造函数
        ///</summary>
        public WriteUnitWeightLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Write UnitWeight Log
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>(); 
            var logItem = new UnitWeightLog
            {
                productID = currentProduct.ProId,
                unitWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight),
                editor = Editor,
                cdt = DateTime.Now
            };

            productRepository.InsertUnitWeightLogDefered(CurrentSession.UnitOfWork, logItem);

            return base.DoExecute(executionContext);
        }
    }
}
