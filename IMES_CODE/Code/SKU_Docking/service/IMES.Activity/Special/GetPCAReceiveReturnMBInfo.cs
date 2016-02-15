// INVENTEC corporation (c)2010all rights reserved. 
// Description:CI-MES12-SPEC-FA-UC IEC Label Print.docx
//             获取DCode            
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-12-01   zhu lei                      create
// Known issues:
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
using IMES.FisObject.PAK;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Warranty;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MB;

namespace IMES.Activity
{
    /// <summary>
    /// GetPCAReceiveReturnMB
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCAReceiveReturnMB
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         GetPCAReceiveReturnMB
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///        MBSN 
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         PCBTestLog or RepairInfo
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class GetPCAReceiveReturnMB : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public GetPCAReceiveReturnMB()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取DCode
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //DateTime dt = DateTime.Now;
            //string year = string.Empty;
            //string month = string.Empty;
            //string siteCode = string.Empty;
            MB mb = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IList<ArrayList> ret = new List<ArrayList>();
            IList<IMES.FisObject.Common.TestLog.TestLogDefect> mbTestLogDefcet = new List<IMES.FisObject.Common.TestLog.TestLogDefect>();
            IList<IMES.FisObject.Common.Repair.RepairDefect> mbRepairDefcet = new List<IMES.FisObject.Common.Repair.RepairDefect>();
            
            if ((mb.TestLogs == null || mb.TestLogs.Count == 0) || ( mb.TestLogs[0].Station != "33" && mb.TestLogs[0].Status != IMES.FisObject.Common.TestLog.TestLog.TestLogStatus.Fail))
            {
                if ((mb.Repairs == null || mb.Repairs.Count == 0) || ( mb.Repairs[0].Station != "33" && mb.Repairs[0].Status != IMES.FisObject.Common.Repair.Repair.RepairStatus.Finished))
                {
                    //error 沒有33站相關資料
                    throw new FisException("CHK1067", new string[] { mb.Sn });
                }
                else
                {
                    mbRepairDefcet = mb.Repairs[0].Defects;
                    foreach (IMES.FisObject.Common.Repair.RepairDefect items in mbRepairDefcet)
                    {
                        ArrayList array = new ArrayList();
                        array.Add(mb.Sn);
                        array.Add(mb.Repairs[0].LineID);
                        array.Add(items.DefectCodeID);
                        array.Add(items.Editor);
                        array.Add(items.Cdt);
                        ret.Add(array);
                    }
                }
            }
            else
            { 
                mbTestLogDefcet = mb.TestLogs[0].Defects;
                foreach (IMES.FisObject.Common.TestLog.TestLogDefect items in mbTestLogDefcet)
                {
                    ArrayList array = new ArrayList();
                    array.Add(mb.Sn);
                    array.Add(mb.TestLogs[0].Line);
                    array.Add(items.DefectCode);
                    array.Add(items.Editor);
                    array.Add(items.Cdt);
                    ret.Add(array);
                }
            }

            CurrentSession.AddValue("PCAReceiveReturnMBListInfo", ret);

            return base.DoExecute(executionContext);
        }
    }
}
