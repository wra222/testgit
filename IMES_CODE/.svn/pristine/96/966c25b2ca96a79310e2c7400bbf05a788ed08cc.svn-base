/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: CompleteRepair。
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-11-13   Tong.Zhi-Yong                implement DoExecute method
 * 2012-01-11   Yang.Wei-Hua                 For IMES2012 SA Repair
 * Known issues:
 */
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 检查是否满足complete条件, 更新Repair状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///        
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB, Product为主线对象的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.检查MBRepair or ProductRepair是否满足complete条件, ;
    ///         2.更新MBReair or ProductRepair状态;
    ///         3.保存MBRepair or ProductRepair;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
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
    ///         update PCARepair
    ///         update ProductRepair
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IProduct
    ///         IMBRepository
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class CompleteRepair : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public CompleteRepair()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 检查是否满足complete条件, 更新Repair状态
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IRepairTarget repairTarget = GetRepairTarget();

            //check session
            if (CurrentSession == null)
            {
                List<String> erpara = new List<String>();

                throw new FisException("CHK021", erpara);
            }
            
            if (repairTarget == null)
            {
                throw new NullReferenceException();
            }

            if (typeof(MB).IsInstanceOfType(repairTarget))
            {
                //update [MTA_Mark] Mark = ‘1’
                IMBRepository repMB = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                repMB.UpdateMtaMarkByRepairIdDefered(CurrentSession.UnitOfWork, repairTarget.GetCurrentRepair().ID, "1");
            }

            //update Repair
            repairTarget.CompleteRepair(Line, Station, Editor);
            UpdateRepairTarget(repairTarget);                
            return base.DoExecute(executionContext);
        }
	}
}
