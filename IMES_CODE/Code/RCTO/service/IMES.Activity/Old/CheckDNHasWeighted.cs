// INVENTEC corporation (c)2009 all rights reserved. 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-03   207006                 create
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
using IMES.Infrastructure;
using IMES.FisObject.PAK.WeightLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// 检查被选DN是否已称重
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
    ///         按照model在FRUWeightLog表中找不到记录
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Delivery 
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
    ///        Delivery
    /// </para> 
    /// </remarks>
    public partial class CheckDNHasWeighted : BaseActivity
    {

        /// <summary>
        /// 
        /// </summary>
        public CheckDNHasWeighted()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var delivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            IWeightLogRepository weightRep = RepositoryFactory.GetInstance().GetRepository<IWeightLogRepository, WeightLog>();
            int result = weightRep.GetCountOfFRUWeightLog(delivery.ModelName);
            
            if (result == 0)
            {
                throw new FisException("CHK102", new string[] { delivery.ModelName });
            }
          
            return base.DoExecute(executionContext);
        }
    }
}
