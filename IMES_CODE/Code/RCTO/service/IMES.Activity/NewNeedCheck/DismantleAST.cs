/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Dismantle AST
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 
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
using IMES.FisObject.Common .Process;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Part;
namespace IMES.Activity
{
    /// <summary>
    /// Dismantle AST处理
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// Dismantle AST处理
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    /// 根据类型更新对应表
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///      IProductRepository 
    /// </para> 
    /// </remarks>
    public partial class DismantleAST: BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DismantleAST()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Product，删除 ProductInfo表中的IMEI数据
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> erpara = new List<string>();
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
           
    
            
            currentProduct.RemovePartsByBomNodeType("AT");
            //DEBUG ITC-1360-0124 
            //For Fisobject ,Need Update method save data 
            //-----------------------------------------------------------------
            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
            //-----------------------------------------------------------------

            return base.DoExecute(executionContext);
        }
    }
}
