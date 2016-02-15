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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Repair;

using IMES.Infrastructure.FisObjectBase;

using IMES.FisObject.Common.Model;
using System.Collections.Generic;

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
    ///         1.根据Property设定更新MB对象状态;
    ///         2.保存MB对象;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MBNO
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
    ///         update MB
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class DeleteMBRepair : BaseActivity
    {

        ///<summary>
        /// 构造函数
        ///</summary>
        public DeleteMBRepair()
        {
            InitializeComponent();
        }

        /// <summary>
        ///DeleteMBRepair
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            IMB item = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IList<Repair>  getRepair = new List<Repair>();
            getRepair = item.Repairs;
            getRepair = item.GetRepair();
            foreach (Repair tmp in getRepair)
            {
                IList<RepairDefect> getRepairDefect = new List<RepairDefect>();
                getRepairDefect = tmp.Defects;
                foreach (RepairDefect aRepairDefect in getRepairDefect)
                {
                    item.RemoveRepairDefect(tmp.ID, aRepairDefect.ID);
                }
                //item.RemoveRepair(tmp.ID);
            }
            item.RemoveAllRepair();
            mbRepository.Update(item, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
    }
}
