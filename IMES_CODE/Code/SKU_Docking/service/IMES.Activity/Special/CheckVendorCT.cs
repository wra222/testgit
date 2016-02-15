// INVENTEC corporation (c)2010 all rights reserved. 
// Description:检查输入的VendorCT长度是否正确
//             CI-MES12-SPEC-FA-UC IEC Label Print       
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-14   zhu lei                      create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.Infrastructure;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入的VendorCT长度是否正确
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于IEC Label Print
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
    public partial class CheckVendorCT : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckVendorCT()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查输入的VendorCT长度是否正确
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var currentVendorCT = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);

            if (currentVendorCT.Length != 14)
            {
                var ex = new FisException("CHK163", new string[] {  });
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
    }
}
