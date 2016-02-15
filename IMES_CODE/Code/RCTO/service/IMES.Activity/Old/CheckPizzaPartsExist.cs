// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入Product的Pizza是否已经绑定有PizzaParts
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-06   Yuan XiaoWei                 create
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
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入Product的Pizza是否已经绑定有PizzaParts
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ReprintPizzaLabel
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
    public partial class CheckPizzaPartsExist : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPizzaPartsExist()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查输入Product的Pizza是否已经绑定有PizzaParts
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            int count = 0;
            if (!string.IsNullOrEmpty(currentProduct.PizzaID))
            {
                IPizzaRepository currentPizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                count = currentPizzaRepository.GetPizzaPartsCout(currentProduct.PizzaID);
            }

            if (count == 0)
            {
                var ex = new FisException("CHK100", new string[] { currentProduct.ProId });
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
    }
}
