/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: 用于Repair时新增Defect信息。
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-11-12   Yuan XiaoWei                 create
 * 2009-11-13   Tong.Zhi-Yong                implement DoExecute method
 * 2010-01-08   Tong.Zhi-Yong                Modify ITC-1103-0071
 * 2010-03-06   Tong.Zhi-Yong                Modify ITC-1122-0205
 * 2012-01-11   Yang.Wei-Hua                 For IMES2012 SA Repair
 * Known issues:
 */
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure.Extend;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{

    /// <summary>
    /// 新增维修Defect信息
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
    ///         从Session中获取Defect, 调用目标对象的AddRepairDefect方法来保存。
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
    public partial class AddRepairDefect : BaseActivity
	{

        /// <summary>
        /// 用于Repair时新增Defect信息。
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IRepairTarget repairTarget = GetRepairTarget();

            //IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            //if (product != null)
            //{
            //    string value = (string)CurrentSession.GetValue(Session.SessionKeys.MAC);
            //    product.MAC = value;
            //}

            RepairDefect defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);
            defect.Editor = this.Editor;

            //ITC-1103-0071 Tong.Zhi-Yong 2010-01-08
            if (repairTarget.GetType() == typeof(MB))
            {
                MBModel modelObj = ((IMB)repairTarget).ModelObj;

                if (modelObj == null)
                {
                    defect.Type = string.Empty;
                }
                else
                {
                    defect.Type = modelObj.Type;
                }
            }
            else
            {
                //defect.Type = ((IProduct)repairTarget).ProId;
                /* ITC-1360-0442, Jessica Liu, 2012-2-16
                //ITC-1122-0205 Tong.Zhi-Yong 2010-03-06
                defect.Type = "PRD";
                */
                defect.Type = "PRD";

            }

            //Dean 20110530 因為PartType為Disable ，在新增時就要塞入PartType，來判斷是否回F0或40
            string RepairPartType = "";
            if (CurrentSession.GetValue(ExtendSession.SessionKeys.RepairPartType) != null)
            {
                defect.PartType = CurrentSession.GetValue(ExtendSession.SessionKeys.RepairPartType).ToString().Trim();
            }
            else
            {
                defect.PartType = RepairPartType.Trim();
            }
            //Dean 20110530 因為PartType為Disable ，在新增時就要塞入PartType，來判斷是否回F0或40
                            
            repairTarget.AddRepairDefect(repairTarget.GetCurrentRepair().ID, defect);
            UpdateRepairTarget(repairTarget);
            return base.DoExecute(executionContext);
        }

	}
}
