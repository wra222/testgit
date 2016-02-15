/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Finish KP repair.
 * 
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ===========================
 * 2012-07-27   itc202017                    create
 * Known issues:
 */
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 更新KPRepair状态
    /// </summary>
    public partial class CompleteKPRepair : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompleteKPRepair()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新KPRepair状态
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

            KeyPartRepairInfo cond1 = new KeyPartRepairInfo();
            cond1.productID = this.Key;
            
            KeyPartRepairInfo item1 = new KeyPartRepairInfo();
            item1.productID = this.Key;
            item1.status = 1;
            item1.station = "KPR";
            item1.line = this.Line;
            item1.editor = this.Editor;
            item1.udt = DateTime.Now;

            partRepository.UpdateKPRepairDefered(CurrentSession.UnitOfWork, item1, cond1);

            IqcCause1Info cond2 = new IqcCause1Info();
            cond2.ctLabel = this.Key;
            //cond2.status = "0";

            IqcCause1Info item2 = new IqcCause1Info();
            item2.ctLabel = this.Key;
            item2.status = "1";
            item2.udt = DateTime.Now;

            defectRepository.UpdateUDTofIqcCause(item2, cond2);

            return base.DoExecute(executionContext);
        }
    }
}
