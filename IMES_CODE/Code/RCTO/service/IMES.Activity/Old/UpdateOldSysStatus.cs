/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: for set child mb sno for muti mb
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBModel;
using IMES.DataModel;
using IMES.FisObject.Common.Misc;
namespace IMES.Activity
{
    /// <summary>
    /// 更新MB状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB为主线对象的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///       调用存储过程
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
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
    ///        
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// IMiscRepository       
    /// </para> 
    /// </remarks>
    public partial class UpdateOldSysStatus : BaseActivity
    {     

        ///<summary>
        /// 构造函数
        ///</summary>
        public UpdateOldSysStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新Old System Status
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            IMiscRepository currentMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            currentMiscRepository.UpdateOldSysStatusDefered(CurrentSession.UnitOfWork, mb.Sn); 
            return base.DoExecute(executionContext);
        }
    }
}
