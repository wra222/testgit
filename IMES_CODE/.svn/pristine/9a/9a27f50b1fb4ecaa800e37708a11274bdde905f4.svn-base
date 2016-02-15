/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: RCTO Change Multi
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-07-17   Kerwin            Create 
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
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.Utility.Generates.impl;

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
    ///         CI-MES12-SPEC-SA-UC PCA ICT Input（RCTO）
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.MBNOList中每个MBNO，进行RCTO change
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK162
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MBNOList
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.MBNOList
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新Session.SessionKeys.MBNOList
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class RCTOChangeMulti : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public RCTOChangeMulti()
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
            List<string> childMBSnList = CurrentSession.GetValue(Session.SessionKeys.MBNOList) as List<string>;

            int MBSnLen = childMBSnList[0].Length;
            string PreMBSn = childMBSnList[0].Substring(0, MBSnLen-5);

            List<string> RCTOChildMBSnList = GetMaxMBSn(PreMBSn, childMBSnList.Count-1);
            RCTOChildMBSnList.Insert(0, PreMBSn + "R" + childMBSnList[0].Substring(MBSnLen - 4, 4));

            CurrentSession.AddValue(Session.SessionKeys.RCTOChildMBSnList, RCTOChildMBSnList);

            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, RCTOChildMBSnList[0]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, RCTOChildMBSnList[RCTOChildMBSnList.Count - 1]);
            return base.DoExecute(executionContext);
        }

        private List<string> GetMaxMBSn(string prefix,int qty)
        {
            List<string> result = new List<string>();
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    string currentMaxNum = numCtrlRepository.GetMaxNumber("MBSno", prefix + "{0}");
                    if (string.IsNullOrEmpty(currentMaxNum))
                    {
                        ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789ABCDEFGHJKLMNPRSTVWXYZ", 4, "ZZZZ", beginNO, '0');
                        for (int i = 0; i < qty;i++ ) {
                            result.Add(prefix + "R" + seqCvt.NumberRule.IncreaseToNumber(beginNO, i));
                        }
                        NumControl currentNumControl = new NumControl();
                        currentNumControl.NOName = "";
                        currentNumControl.NOType = "MBSno";
                        currentNumControl.Customer = Customer;

                        currentNumControl.Value = result[qty-1];
                        
                        numCtrlRepository.InsertNumControl(currentNumControl);
                        SqlTransactionManager.Commit();
                        return result;
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

                            for (int i = 1; i <= qty; i++)
                            {
                                result.Add(prefix + "R" + seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, i));
                                if (result[i-1].EndsWith("ZZZZ"))
                                {
                                    throw new FisException("CHK162", new string[] { });
                                }
                            }


                            IList<NumControl> numCtrlLst = numCtrlRepository.GetNumControlByNoTypeAndValue("MBSno", currentMaxNum);
                            NumControl currentNumControl = numCtrlLst[0];

                            currentNumControl.Value = result[qty - 1];
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentNumControl, uof);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return result;
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
