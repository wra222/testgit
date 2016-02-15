// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Delete Pizza
//             CI-MES12-SPEC-PAK-UC Packing Pizza       
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-14   zhu lei                      create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// Delete Pizza
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于Packing Pizza Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pizza
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
    ///         Pizza
    /// </para> 
    /// </remarks>
    public partial class DeletePizza : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeletePizza()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delete Pizza
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Pizza currentPizza = (Pizza)CurrentSession.GetValue(Session.SessionKeys.Pizza);
            IPizzaRepository currentPizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            currentPizzaRepository.Remove(currentPizza, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
    }
}
