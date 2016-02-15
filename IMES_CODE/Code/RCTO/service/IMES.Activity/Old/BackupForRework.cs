/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: check if start mbsnno and end mbsnno has the same mo
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System;
using System.ComponentModel;
using System.ComponentModel.Design;
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// 备份Rework数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于121DismantleFA.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// Copy ProductStatus and Product and Product_Part and ProductInfo资料至Rework_ProductStatus, Rework_Product, Rework_Product_Part, Rework_ProductInfo
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
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
    ///         更新Rework_ProductStatus, Rework_Product, Rework_Product_Part, Rework_ProductInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class BackupForRework : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BackupForRework()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  备份数据
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            // Copy ProductStatus and Product and Product_Part and ProductInfo资料至Rework_ProductStatus, Rework_Product, Rework_Product_Part, Rework_ProductInfo
            //其中Rework_xxx 表中的ReworkCode = 'DISMANTLE'
            //@Code='DISMANTLE'
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            productRepository.CopyProductToReworkDefered(CurrentSession.UnitOfWork, currentProduct.ProId, "DIS00000");
            return base.DoExecute(executionContext);
        }
    }
}
