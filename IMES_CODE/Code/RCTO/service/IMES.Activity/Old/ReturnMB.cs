/* INVENTEC corporation (c)2010 all rights reserved. 
 * Description: MB成退
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2010-02-25   Tong.Zhi-Yong                implement DoExecute method
 * 2010-03-05   Tong.Zhi-Yong                Modify ITC-1122-0188
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using System.Collections.Generic;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Misc;

namespace IMES.Activity
{
    /// <summary>
    /// MB成退
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FA Repair
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         将记录保存到PCBRepair(Line是FA Line,Station是PCA Station)和PCBRepair_DefectInfo表(其中DefectCode是当前修护记录对应的DefectCode，IsManual=0)
    ///         将FA和PCA的Repair ID保存到ReturnRepair表
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.CurrentRepairdefect
    ///         Session.SessionKeys.MB
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
    ///         ReturnRepair, PCBRepair, PCBRepair_DefectInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    ///         IProduct
    ///         IMiscRepository
    /// </para> 
    /// </remarks>
	public partial class ReturnMB: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnMB()
		{
			InitializeComponent();
		}

        /// <summary>
        /// MB成退
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                IRepairTarget repairTarget = GetRepairTarget();
                IProduct product = (IProduct)repairTarget;
                Repair currentRepair = repairTarget.GetCurrentRepair();
                IMBRepository imr = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                IList<RepairDefect> repairDefects = new List<RepairDefect>();
                RepairDefect defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);
                IMiscRepository miscRpst = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();

                //Insert a Repair to PCB
                IMB oldMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
                Repair repair = null;
                RepairDefect repairDefect = new RepairDefect(0, 0, oldMB.ModelObj == null ? string.Empty : oldMB.ModelObj.Type, defect.DefectCodeID, null, null, null, null, null,
                                null, null, null, null, null, null, null, null, null, false, null,
                                null, null, string.Empty, null, "0", null, null, null, null, null, null, null,
                                null, null, null, this.Editor, DateTime.Now, DateTime.Now);
                repairDefects.Add(repairDefect);
                //Update by Dean 20110615 由23-->18(SARepair Station),TSB為23,E-Book為18
                repair = new Repair(0, oldMB.Sn, oldMB.Model, oldMB.ModelObj == null ? string.Empty : oldMB.ModelObj.Type, this.Line, "18", IMES.FisObject.Common.Repair.Repair.RepairStatus.NotFinished, repairDefects, this.Editor, null, DateTime.Now, DateTime.Now);
                oldMB.AddRepair(repair);
                imr.Update(oldMB, CurrentSession.UnitOfWork);

                //Insert Record To ReturnRepair Table
                ReturnRepair returnRepair = new ReturnRepair();
                //ITC-1122-0188 Tong.Zhi-Yong 2010-03-05
                CurrentSession.UnitOfWork.RegisterSetterBetween(new IMES.Infrastructure.UnitOfWork.SetterBetween(defect, "RepairID", returnRepair, "setter_ProductRepairID"));

                if (defect.ID == 0) //Add
                {
                    CurrentSession.UnitOfWork.RegisterSetterBetween(new IMES.Infrastructure.UnitOfWork.SetterBetween(defect, "ID", returnRepair, "setter_ProductRepairDefectID"));
                }
                else
                {
                    returnRepair.ProductRepairDefectID = defect.ID;
                }

                CurrentSession.UnitOfWork.RegisterSetterBetween(new IMES.Infrastructure.UnitOfWork.SetterBetween(repair, "ID", returnRepair, "setter_PCBRepairID"));
                miscRpst.AddReturnRepairDefered(CurrentSession.UnitOfWork, returnRepair);

                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
	}
}
