/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: DeleteRepair。
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-11-16   Tong.Zhi-Yong                implement DoExecute method
 * Known issues:
 */
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
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 删除RepairDefect信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCB维修站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.检查Defect是否未维修完成;
    ///         2.通过PCAReair对象删除DefectInfo;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             只有未维修完成的Defect才允许删除
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.PCAReapirDefectInfo
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
    ///         PCARepair_DefectInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         PCARepair
    ///         PCARepairDefectInfo
    /// </para> 
    /// </remarks>
    public partial class DeleteRepairDefect : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public DeleteRepairDefect()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 删除Repair
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                int repairDefectID;

                //check session
                if (CurrentSession == null)
                {
                    throw new FisException();
                }

                repairDefectID = (int)CurrentSession.GetValue(Session.SessionKeys.RepairDefectID);

                IRepairTarget repairTarget = GetRepairTarget();

                if (repairTarget == null)
                {
                    throw new NullReferenceException();
                }

                repairTarget.RemoveRepairDefect(repairTarget.GetCurrentRepair().ID, repairDefectID);
                UpdateRepairTarget(repairTarget);

                return base.DoExecute(executionContext);
            }
            catch (FisException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }

        }
	}
}
