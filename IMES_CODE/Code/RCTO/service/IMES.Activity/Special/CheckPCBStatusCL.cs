// INVENTEC corporation (c)2012 all rights reserved. 
// Description:如果PCBStatus.Station=CL,则报错:该MB的生命周期已结束
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-13   Kerwin                       create
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.EcrVersion;

namespace IMES.Activity
{

    /// <summary>
    /// 如果PCBStatus.Station=CL,则报错:该MB的生命周期已结束
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-SA-UC PCA ICT Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     1.	如果PCBStatus.Station=CL,则报错:该MB的生命周期已结束
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
    ///         MB
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckPCBStatusCL : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPCBStatusCL()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 如果PCBStatus.Station=CL,则报错:该MB的生命周期已结束
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;

            if (currenMB != null && currenMB.MBStatus !=null && currenMB.MBStatus.Station == "CL")
            {
                throw new FisException("ICT011", new string[] { currenMB.Sn });
            }
            return base.DoExecute(executionContext);
        }

    }
}

