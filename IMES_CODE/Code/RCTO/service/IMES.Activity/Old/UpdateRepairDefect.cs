/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: 用于Repair时修改Defect信息。
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-11-12   Yuan XiaoWei                 create
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
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    /// <summary>
    /// 更新Defect信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于维修站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB或者Session.Product
    ///         Session.ReapirDefectInfo
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
    ///         PCARepair_DefectInfo 或者ProductRepair_DefectInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         PCARepair
    ///         PCARepairDefectInfo
    ///         IProduct
    ///         ProductRepair
    ///         ProductRepairDefectInfo
    /// </para> 
    /// </remarks>
    public partial class UpdateRepairDefect : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public UpdateRepairDefect()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 用于Repair时修改Defect信息。
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IRepairTarget repairTarget = GetRepairTarget();
            RepairDefect defect = null;

            defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);

            //Dean 20110530 因為PartType為Disable ，在新增時就要塞入PartType，來判斷是否回F0或40
            string RepairPartType = "";
            if (CurrentSession.GetValue(ExtendSession.SessionKeys.RepairPartType) != null)
            {
                defect.PartType = CurrentSession.GetValue(ExtendSession.SessionKeys.RepairPartType).ToString().Trim();
            }
            else
            {
                defect.PartType = RepairPartType.ToString().Trim();
            }
            //Dean 20110530 因為PartType為Disable ，在新增時就要塞入PartType，來判斷是否回F0或40

            repairTarget.UpdateRepairDefect(defect.RepairID, defect);
            UpdateRepairTarget(repairTarget);

            return base.DoExecute(executionContext);
        }
	}
}
