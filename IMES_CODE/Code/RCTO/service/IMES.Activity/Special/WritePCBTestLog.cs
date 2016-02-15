// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 记录到PCBTestLog,PCBTestLog_DefectInfo表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-29   Kerwin                 create
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.FisObjectRepositoryFramework;


namespace IMES.Activity
{
    /// <summary>
    /// 为主板添加测试Log
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-SA-UC PCA Test Station
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Session.DefectList创建TestLog对象
    ///         2.为IMB对象添加TestLog;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.TestLog
    ///         Session.FixtureID
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
    ///         insert PCBTestLog
    ///         insert PCBTestLog_DefectInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         TestLog
    ///         
    /// </para> 
    /// </remarks>
    public partial class WritePCBTestLog : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public WritePCBTestLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// insert into pcbtestlog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IList<string> defectList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);

            IList<TestLogDefect> defects = new List<TestLogDefect>();

            if (defectList != null)
            {
                foreach (string item in defectList)
                {
                    //add defect
                    TestLogDefect defectItem = new TestLogDefect(0, 0, item, this.Editor, DateTime.Now);
                    defects.Add(defectItem);
                }
            }

            //get 111 type
            string type = "MB";// mb.ModelObj.Type;

            string line = this.Line;
            if (string.IsNullOrEmpty(line))
            {
                line = mb.MBStatus.Line;
            }

            string fixid = (string)CurrentSession.GetValue(Session.SessionKeys.FixtureID);
            if (string.IsNullOrEmpty(fixid))
            {
                fixid = string.Empty;
            }

            string actionName = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.TestLogActionName);
            if (string.IsNullOrEmpty(actionName))
            {
                actionName = string.Empty;
            }

            string errorCode = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.TestLogErrorCode);
            if (string.IsNullOrEmpty(errorCode))
            {
                errorCode = string.Empty;
            }

            string descr = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.TestLogDescr);
            if (string.IsNullOrEmpty(descr))
            {
                descr = string.Empty;
            }

            string remark = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.TestLogRemark);
            if (string.IsNullOrEmpty(remark))
            {
                remark = string.Empty;
            }


            TestLog tItem;

            string AllowPass = "";
            string DefectStation = "";
            if (CurrentSession.GetValue(ExtendSession.SessionKeys.AllowPass) != null)
            {
                AllowPass = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.AllowPass);
            }

            if (CurrentSession.GetValue(ExtendSession.SessionKeys.DefectStation) != null)
            {
                DefectStation = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.DefectStation);
            }

            if (defectList == null || defectList.Count == 0)
            {
                tItem = new TestLog(0, mb.Sn, line, fixid, this.Station, new List<TestLogDefect>(), TestLog.TestLogStatus.Pass, "",
                                               actionName, errorCode, descr, this.Editor, type, DateTime.Now);
            }
            else
            {
                if (AllowPass == "N")//Check AllowPass =N  Dean 20110625
                {
                    tItem = new TestLog(0, mb.Sn, line, fixid, DefectStation, defects, TestLog.TestLogStatus.Fail, "",
                        actionName, errorCode, descr, this.Editor, type, DateTime.Now);
                }
                else //Normal Flow
                {
                    tItem = new TestLog(0, mb.Sn, line, fixid, this.Station, defects, TestLog.TestLogStatus.Fail, "",
                                                   actionName, errorCode, descr, this.Editor, type, DateTime.Now);
                }
            }

            tItem.Remark = remark;

            mb.AddTestLog(tItem);
            mbRepository.Update(mb, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
    }
}
