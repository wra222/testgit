using System.Collections.Generic;
// INVENTEC corporation (c)2009 all rights reserved. 
// Description: CI-MES12-SPEC-FA-UC PCA Shipping Label Print.docx
//              Check MBSNo     
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-08   zhu lei                      create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 检查Product是否有重复的MBSNo
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于需要站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.MB 
    ///         
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
    ///    
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        Product
    /// </para> 
    /// </remarks>
    public partial class CheckMBSNo : BaseActivity
	{
        /// <summary>
        /// CheckMBSNo
        /// </summary>
        public CheckMBSNo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Product是否有重复的MBSNo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string mbSNo = this.Key;
            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //如果存在重复的MB SNo，则提示”此MB已打印Shipping Label”
            IProduct prd = currentProductRepository.GetProductByMBSn(mbSNo);
            if (prd != null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(mbSNo);
                throw new FisException("CHK254", errpara);
            }
            return base.DoExecute(executionContext);
        }
	
	}
}
