// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入的PizzaID是否存在
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
    /// 检查输入的PizzaID是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于Packing Pizza Label Reprint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK100
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PizzaID
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
    public partial class CheckPizzaIDExist : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPizzaIDExist()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查输入的PizzaID是否存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var currentPizzaID = (string)CurrentSession.GetValue(Session.SessionKeys.PizzaID);

            Pizza resultPizza = null;
            if (!string.IsNullOrEmpty(currentPizzaID))
            {
                IPizzaRepository currentPizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                 resultPizza = currentPizzaRepository.Find(currentPizzaID);
            }

            if (resultPizza == null)
            {
                var ex = new FisException("CHK100", new string[] { currentPizzaID });
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
    }
}
