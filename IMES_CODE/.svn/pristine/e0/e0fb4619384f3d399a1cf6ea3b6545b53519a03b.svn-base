// INVENTEC corporation (c)2011 all rights reserved. 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   210003                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.FisObject.PAK.Paking;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 获取Region信息。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Packing List
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK007
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
    public partial class GetRegionList : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public GetRegionList()
		{
			InitializeComponent();
		}
        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.Regions
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //从Session里取得Product对象
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.InfoFormat("GetProductActivity: Key: {0}", this.Key);
            IPakingRepository PAK_PAKComnRepository = RepositoryFactory.GetInstance().GetRepository<IPakingRepository, PAK_PAKComn>();
            IList<string> regions = PAK_PAKComnRepository.GetRegions();
            if (null == regions || regions.Count == 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(CurrentProduct.ProId);
                ex = new FisException("PAK007", erpara);
                throw ex;
            }
            return base.DoExecute(executionContext);
        }
	}
}
