/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 将CPU Vendor Sn 绑定到Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于Product Combine CPU站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.检查CPU是否已与其他Product绑定;
    ///         2.更新Product对象
    ///         3.保存Product对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             该Cpu已绑定至其它MB
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    ///         Session.CPUVendorSn
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
    ///         PCB
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class CombineProductCPU : BaseActivity
	{
		public CombineProductCPU()
		{
			InitializeComponent();
		}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string cvsn = (string)CurrentSession.GetValue(Session.SessionKeys.CPUVendorSn);

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string anotherProductNo = currentProductRepository.IsUsedCvsn(cvsn);

            if (String.IsNullOrEmpty(anotherProductNo))
            {
                currentProduct.CVSN = cvsn;
                currentProductRepository.Update(currentProduct, CurrentSession.UnitOfWork);
            }
            else
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add("CPU");
                erpara.Add(cvsn);
                erpara.Add(anotherProductNo);
                ex = new FisException("CHK009", erpara);
                ex.stopWF = true;
                throw ex;
            }


            return base.DoExecute(executionContext);
        }
	}
}
