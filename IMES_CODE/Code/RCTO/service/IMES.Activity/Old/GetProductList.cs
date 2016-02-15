// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 应用于070 Unpack站(UnpackCarton,UnpackDN,UnpackPallet)，用于获取符合条件的所有ProductID List
//                  
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-11   Lucy Liu                     create
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 应用于070 Unpack站，用于获取符合条件的所有ProductID List
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于070 Unpack站，用于获取符合条件的所有ProductID List
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取条件(PalletNo或者Carton SN)
    ///         2.根据1条件去Product表中找寻符合条件的所有ProductID
    ///         2.将其ProductID保存到NewScanedProductIDList
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Carton,Session.SessionKeys.PalletNo
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.NewScanedProductIDList
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class GetProductList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetProductList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行添加ProductID到NewScanedProductIDList列表的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            String cartonSN = (String)CurrentSession.GetValue(Session.SessionKeys.Carton);
            String palletNo = (String)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            List<string> productIDList = new List<string>();
            List<string> productCustSNList = new List<string>();
            if (!String.IsNullOrEmpty(cartonSN))
            {
                productIDList = productRepository.GetProductIDListByCarton(cartonSN);
                productCustSNList = productRepository.GetCustSnListByCarton(cartonSN);
            }
            else if (!String.IsNullOrEmpty(palletNo))
            {
                productIDList = productRepository.GetProductIDListByPalletNo(palletNo);
                productCustSNList = productRepository.GetCustSnListByPalletNo(palletNo);
            }


            CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIDList);
            CurrentSession.AddValue(Session.SessionKeys.NewScanedProductCustSNList, productCustSNList);
            return base.DoExecute(executionContext);
        }

      
    }
}
