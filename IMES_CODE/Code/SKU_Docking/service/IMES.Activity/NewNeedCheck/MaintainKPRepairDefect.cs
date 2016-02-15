/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Maintain (Add/Edit) KP repair defect.
 * 
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ===========================
 * 2012-07-27   itc202017                    create
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 新增/更新维修KPDefect信息
    /// </summary>
    public partial class MaintainKPRepairDefect : BaseActivity
    {
        /// <summary>
        /// 用于KPRepair时新增/更新Defect信息。
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                bool bAdd = (bool)CurrentSession.GetValue("isAdd");

                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                RepairDefect defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);
                if (bAdd)
                {
                    KeyPartRepairInfo cond = new KeyPartRepairInfo();
                    cond.productID = this.Key;
                    cond.status = 0;
                    IList<KeyPartRepairInfo> repList = partRepository.GetKPRepairList(cond);
                    if (repList.Count > 0)
                    {
                        defect.RepairID = repList[0].id;
                    }
                    defect.Type = "KP";
                    defect.MTAID = "";
                    defect.ReturnStation = "";
                    partRepository.AddKPRepairDefect(defect);
                }
                else
                {
                    RepairDefect cond = new RepairDefect();
                    cond.ID = defect.ID;
                    defect.ID = int.MinValue;
                    partRepository.UpdateKPRepairDefect(defect, cond);
                }

                KeyPartRepairInfo cond1 = new KeyPartRepairInfo();
                cond1.productID = this.Key;
                cond1.status = 1;
                CurrentSession.AddValue(Session.SessionKeys.RepairTimes, partRepository.GetCountOfKpRepairCount(cond1));

                return base.DoExecute(executionContext);
            }
            catch (Exception e)
            {
                FisException ee = new FisException(e.Message);
                ee.stopWF = false;
                throw ee;
            }
        }

    }
}
