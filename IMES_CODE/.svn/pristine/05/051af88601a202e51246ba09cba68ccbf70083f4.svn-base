// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的NewScanedProductIDList中的ProductID列表，
//              更新列表中所有Product的PalletNo栏位          
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
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 用于更新一组Product的PalletNo
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
    ///         1.从Session中获取NewScanedProductIDList，调用ProductRepository.BindPallet
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
    ///         更新Product表的PalletNo 
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class BindPallet : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BindPallet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pallet绑定到Product列表
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string PalletID = ((Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet)).PalletNo;

            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> ProductIDList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            ProductRepository.BindPalletDefered(CurrentSession.UnitOfWork, PalletID, ProductIDList);

            return base.DoExecute(executionContext);
        }
    }
}
