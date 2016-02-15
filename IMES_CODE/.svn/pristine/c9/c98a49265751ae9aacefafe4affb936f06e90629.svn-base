// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 删除MO
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-24   Yuan XiaoWei                 create
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
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{

    /// <summary>
    /// 删除MO
    /// 如果欲删除的Mo，已经开始列印MB Label，则需要报告错误：
    /// “The Mo has combined MBNo or SBNo or VBNo, Can't delete!
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         删除MO
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.delete from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 and PrintQty = 0
    ///         2.select distinct SMTMO from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK014
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
    ///         update PCBStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class DeleteSMTMO : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteSMTMO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行逻辑操作，删除可以删除的MO
        /// 如果有不能删除的
        /// 把MO号码记录下来抛出业务异常
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IMBMORepository currentMBMORepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            IList<string> moList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.MBMONOList);
            IList<string> printedMoList = currentMBMORepository.DeleteSMTMO(moList);
            if (printedMoList != null && printedMoList.Count > 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                string printedMOs = "";
                foreach (string tempMo in printedMoList)
                {
                    printedMOs += tempMo + ",";
                }
                printedMOs.TrimEnd((','));
                erpara.Add(printedMOs);
                ex = new FisException("CHK014", erpara);
                CurrentSession.Exception = ex;
            }
            return base.DoExecute(executionContext);
        }
    }
}
