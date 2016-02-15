/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: RCTO Change Single
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-07-16   Kerwin            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Utility.Generates.impl;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Activity
{
    /// <summary>
    /// 保存产生的MBNO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-SA-UC PCA ICT Input For RCTO 非链板
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.MB
    ///             1.修改MBSn
    ///             2.保存MB对象
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
    ///         Update PCB/PCBInfo/PCB_Part/PCBStatus/PCBLog/PCBTestLog Set PCBNo = @RCTOMBSn where PCBNo= @MBSn
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class RCTOChangeSingle : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public RCTOChangeSingle()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            MB currentMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            int MBSnLen = currentMB.Sn.Length;
            string PreMBSn = currentMB.Sn.Substring(0, MBSnLen-5);
            string endMBSn = currentMB.Sn.Substring(MBSnLen-4, 4);
            string RCTOMBSn = "";

            if (currentMB.Sn.StartsWith(PreMBSn + "C"))
            {
                RCTOMBSn = PreMBSn + "R" + endMBSn;
            }
            else
            {
                RCTOMBSn = GetMaxMBSn(PreMBSn);
            }

            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, RCTOMBSn);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, RCTOMBSn);

            IProductRepository CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            SqlParameter[] paramsArray = new SqlParameter[3];
            paramsArray[0] = new SqlParameter("MBSn", currentMB.Sn);
            paramsArray[1] = new SqlParameter("RCTOMBSn", RCTOMBSn);
            paramsArray[2] = new SqlParameter("Editor", Editor);
            CurrentProductRepository.ExecSpForNonQueryDefered(CurrentSession.UnitOfWork,SqlHelper.ConnectionString_FA, "IMES_RCTOChange", paramsArray);

            return base.DoExecute(executionContext);
        }

        private string GetMaxMBSn(string prefix)
        {
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    string currentMaxNum = numCtrlRepository.GetMaxNumber("MBSno", prefix + "{0}");
                    if (string.IsNullOrEmpty(currentMaxNum))
                    {

                        NumControl currentNumControl = new NumControl();
                        currentNumControl.NOName = "";
                        currentNumControl.NOType = "MBSno";
                        currentNumControl.Value = prefix + "R" + beginNO;
                        currentNumControl.Customer = Customer;
                        numCtrlRepository.InsertNumControl(currentNumControl);
                        SqlTransactionManager.Commit();
                        return currentNumControl.Value;
                    }
                    else
                    {
                        if (currentMaxNum.EndsWith("ZZZZ"))
                        {
                            throw new FisException("CHK162", new string[] { });
                        }
                        else
                        {
                            ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEFGHJKLMNPRSTVWXYZ", 4, "ZZZZ", beginNO, '0');
                            string sequenceNumber = currentMaxNum.Substring(currentMaxNum.Length - 4, 4);
                            sequenceNumber = seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, 1);

                            IList<NumControl> numCtrlLst = numCtrlRepository.GetNumControlByNoTypeAndValue("MBSno", currentMaxNum);
                            NumControl currentNumControl = numCtrlLst[0];

                            currentNumControl.Value = prefix + "R" + sequenceNumber;
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentNumControl, uof);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return currentNumControl.Value;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        private static object _syncRoot_GetSeq = new object();

        private readonly string beginNO = "D000";
    }
}
