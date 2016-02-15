// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据输入的COASN,获取COAStatus对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-11   Yuan XiaoWei                 create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.COA;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的COASN,获取COAStatus对象，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-COA Removal
    ///      CI-MES12-SPEC-PAK-COA Status Change
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据COASN，调用ICOAStatusRepository的Find方法，获取COAStatus对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                 PAK013
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.COASN
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.COAStatus
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              ICOAStatusRepository
    ///              COAStatus
    /// </para> 
    /// </remarks>
    public partial class GetCOAStatus : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public GetCOAStatus()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Get COAStatus Object and put it into Session.SessionKeys.COAStatus
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string coaSN = (string)CurrentSession.GetValue(Session.SessionKeys.COASN);
            var currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            COAStatus CurrentCOA = currentRepository.Find(coaSN);
            if (CurrentCOA == null)
            {
                FisException fe = new FisException("PAK013", new string[] { coaSN });
                throw fe;
            }
            CurrentSession.AddValue(Session.SessionKeys.COAStatus, CurrentCOA);
            return base.DoExecute(executionContext);
        }
	}
}
