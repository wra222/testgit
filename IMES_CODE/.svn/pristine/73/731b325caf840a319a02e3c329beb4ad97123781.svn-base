// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入SN的是否和原来输入的ProductID一致
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-21   Yuan XiaoWei                 create
// Known issues:
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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.BOM;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入SN的是否和原来输入的ProductID一致
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于036,046，047等
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK028
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.CustSN
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class CheckSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckSN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查输入SN的是否和原来输入的ProductID一致
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            var currentCustSn = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            IProduct currentSNProduct = productRepository.GetProductByCustomSn(currentCustSn);
            if (currentSNProduct == null && currentCustSn.Length > 1)
            {
                currentSNProduct = productRepository.GetProductByCustomSn(currentCustSn.Substring(0, currentCustSn.Length - 1));
            }
            if (currentSNProduct != null)
            {
                currentCustSn = currentSNProduct.CUSTSN;
            }

            if (currentSNProduct == null || string.Compare(currentProduct.CUSTSN, currentSNProduct.CUSTSN, false) != 0)
            {
                var ex = new FisException("CHK111", new string[] { currentCustSn, currentProduct.ProId });
                ex.stopWF = false;
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
    }
}
