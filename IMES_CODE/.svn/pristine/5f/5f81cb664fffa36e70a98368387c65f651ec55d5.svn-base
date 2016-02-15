// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 记录到ProductTestLog,ProductTestLog_DefectInfo表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-28   Yuan XiaoWei                 create
// 2012-03-01   Yuan XiaoWei                 ITC-1360-0983
// Known issues:

using System;
using System.ComponentModel;
using System.Collections;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.TestLog;
using System.Collections.Generic;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    /// <summary>
    /// 为Product添加测试Log
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于FA测试站CI-MES12-SPEC-FA-UC FA Test Station
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
    ///         Session.Product
    ///         Session.DefectList
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
    ///         insert ProductTestLog
    ///         insert ProductTestLog_DefectInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         TestLog
    ///         
    /// </para> 
    /// </remarks>
    public partial class WriteProductTestLog : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public WriteProductTestLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 记录到ProductTestLog,ProductTestLog_DefectInfo表
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            var prod = (IProduct)session.GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> defectList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);

            //Dean 20110625
            string AllowPass = "";
            string DefectStation = "";
            string InsertStation = "";
          //  string logDescr=(string)CurrentSession.GetValue("ProductTestLogDescr")??"";
          //  string logAction = (string)CurrentSession.GetValue("ProductTestLogAction") ?? "";
            //Benson

            string fixid = (string)session.GetValue(Session.SessionKeys.FixtureID);
            if (string.IsNullOrEmpty(fixid))
            {
                fixid = string.Empty;
            }

            string actionName = (string)session.GetValue(ExtendSession.SessionKeys.TestLogActionName);
            if (string.IsNullOrEmpty(actionName))
            {
                actionName = string.Empty;
            }

            string errorCode = (string)session.GetValue(ExtendSession.SessionKeys.TestLogErrorCode);
            if (string.IsNullOrEmpty(errorCode))
            {
                errorCode = string.Empty;
            }

            string descr = (string)session.GetValue(ExtendSession.SessionKeys.TestLogDescr);
            if (string.IsNullOrEmpty(descr))
            {
                descr = string.Empty;
            }

            string remark = (string)session.GetValue(ExtendSession.SessionKeys.TestLogRemark);
            if (string.IsNullOrEmpty(remark))
            {
                remark = string.Empty;
            }

            string testLogJoinId = (string)session.GetValue(ExtendSession.SessionKeys.TestLogJoinId) ?? string.Empty;
            //Benson


            if (session.GetValue(ExtendSession.SessionKeys.AllowPass) != null)
            {
                AllowPass = (string)session.GetValue(ExtendSession.SessionKeys.AllowPass);
            }

            if (session.GetValue(ExtendSession.SessionKeys.DefectStation) != null)
            {
                DefectStation = (string)session.GetValue(ExtendSession.SessionKeys.DefectStation);
            }
            //Dean 20110625

            string line = this.Line;
            if (string.IsNullOrEmpty(line))
            {
                line = prod.Status.Line;
            }

            TestLog.TestLogStatus status = TestLog.TestLogStatus.Fail;
            if (IsPass)
            {
                status = TestLog.TestLogStatus.Pass;
            }

            if (AllowPass == "N")//Check AllowPass =N  Dean 20110625
            {
                InsertStation = DefectStation;
            }
            else //Normal Flow
            {
                InsertStation = this.Station;
            }
            TestLog tItem;
            /*
            if (string.IsNullOrEmpty(logDescr) && string.IsNullOrEmpty(logAction))
            {
                 tItem = new TestLog(0, prod.ProId, line, "", InsertStation, status, "", this.Editor, "PRD", DateTime.Now);//Dean 20110625 this.Station==>InsertStation

            }
            else
            {
                 tItem = new TestLog(0, prod.ProId, line, "", InsertStation, status, "",logAction,"",logDescr, this.Editor, "PRD", DateTime.Now);//Dean 20110625 this.Station==>InsertStation
            
            }*/
            string type = "PRD";
            if (defectList == null || defectList.Count == 0)
            {
                tItem = new TestLog(0, prod.ProId, line, fixid, InsertStation, new List<TestLogDefect>(), status, testLogJoinId,
                                               actionName, errorCode, descr, this.Editor, type, DateTime.Now);
            }
            else
            {
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

                if (AllowPass == "N")//Check AllowPass =N  Dean 20110625
                {
                    tItem = new TestLog(0, prod.ProId, line, fixid, InsertStation, defects, status, testLogJoinId,
                        actionName, errorCode, descr, this.Editor, type, DateTime.Now);
                }
                else //Normal Flow
                {
                    tItem = new TestLog(0, prod.ProId, line, fixid, InsertStation, defects, status, testLogJoinId,
                                                   actionName, errorCode, descr, this.Editor, type, DateTime.Now);
                }
            }

            tItem.Remark = remark;         


            prod.AddTestLog(tItem);

            productRepository.Update(prod, session.UnitOfWork);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// IsReprint
        /// </summary>
        public static DependencyProperty IsPassProperty = DependencyProperty.Register("IsPass", typeof(bool), typeof(WriteProductTestLog));


        /// <summary>
        /// IsPass:True Or False
        /// </summary>
        [DescriptionAttribute("IsPass")]
        [CategoryAttribute("IsPass Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool IsPass
        {
            get
            {
                return ((bool)(base.GetValue(WriteProductTestLog.IsPassProperty)));
            }
            set
            {
                base.SetValue(WriteProductTestLog.IsPassProperty, value);
            }
        }

    }
}
